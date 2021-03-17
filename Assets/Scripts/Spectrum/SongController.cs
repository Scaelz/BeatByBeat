using System;
using System.Collections;
using System.Threading;
using UnityEngine;

using System.Numerics;
using DSPLib;
using EventBusSystem;
using System.Threading.Tasks;

public class SongController : IController, IFrameUpdate, ITrackSelectedHandler
{
    int _spawnedIndex = -10;
    int _numChannels;
    int _numTotalSamples;
    int _sampleRate;
    float _clipLength;
    float[] _multiChannelSamples;
    SpectralFluxAnalyzer _preProcessedSpectralFluxAnalyzer;
    AudioSource _mockSource;
    private readonly AudioSource _hearableSource;
    private readonly INoteProvider _noteProvider;
    private readonly float _delay;
    private bool _loadComplete;
    private bool _songIsEnded = false;
    private float _delayTimer = 0;


    public SongController(AudioSource audioSource, AudioSource hearableSource,
    INoteProvider noteProvider, float delay)
    {
        this._mockSource = audioSource;
        this._hearableSource = hearableSource;
        this._noteProvider = noteProvider;
        this._delay = delay;
        EventBus.Subscribe(this);
    }

    ~SongController()
    {
        EventBus.Unsubscribe(this);
    }

    public void RepeatTrack()
    {
        _loadComplete = true;
        _songIsEnded = false;
    }

    public void FrameUpdate(float deltaTime)
    {
        if (!_mockSource.isPlaying && _hearableSource.isPlaying)
        {
            _songIsEnded = true;
        }

        if (_loadComplete)
        {
            if (!_mockSource.isPlaying && !_songIsEnded)
            {
                _mockSource.Play();
            }
            if (_mockSource.time >= _delay && !_hearableSource.isPlaying)
            {
                _hearableSource.Play();
            }
            int indexToPlot = getIndexFromTime(_mockSource.time) / 1024;
            try
            {
                if (_preProcessedSpectralFluxAnalyzer.spectralFluxSamples[indexToPlot].isPeak)
                {
                    if (_spawnedIndex != indexToPlot)
                    {
                        _spawnedIndex = indexToPlot;
                        Note note = _noteProvider.GetNote();
                        EventBus.RaiseEvent<IBeatReadyHandler>(x => x.OnBeatReady(note));
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                _loadComplete = false;
                MusicIsComplete();
            }

        }
    }

    async void MusicIsComplete()
    {
        var time = (int)TimeSpan.FromSeconds(_delay).TotalMilliseconds;
        await Task.Run(() => Timer(time));
        EventBus.RaiseEvent<ITrackIsOverHandler>(x => x.CloseSession());
    }

    void Timer(int msTimeout)
    {
        Thread.Sleep(msTimeout);
    }

    public int getIndexFromTime(float curTime)
    {
        float lengthPerSample = this._clipLength / (float)this._numTotalSamples;

        return Mathf.FloorToInt(curTime / lengthPerSample);
    }

    public float getTimeFromIndex(int index)
    {
        return ((1f / (float)this._sampleRate) * index);
    }

    public void getFullSpectrumThreaded(Action<float> callback)
    {
        try
        {
            // We only need to retain the samples for combined channels over the time domain
            float[] preProcessedSamples = new float[this._numTotalSamples];

            int numProcessed = 0;
            float combinedChannelAverage = 0f;
            for (int i = 0; i < _multiChannelSamples.Length; i++)
            {
                combinedChannelAverage += _multiChannelSamples[i];

                // Each time we have processed all channels samples for a point in time, we will store the average of the channels combined
                if ((i + 1) % this._numChannels == 0)
                {
                    preProcessedSamples[numProcessed] = combinedChannelAverage / this._numChannels;
                    numProcessed++;
                    combinedChannelAverage = 0f;
                }
            }

            //Debug.Log("Combine Channels done");
            //Debug.Log(preProcessedSamples.Length);

            // Once we have our audio sample data prepared, we can execute an FFT to return the spectrum data over the time domain
            int spectrumSampleSize = 1024;
            int iterations = preProcessedSamples.Length / spectrumSampleSize;

            FFT fft = new FFT();
            fft.Initialize((UInt32)spectrumSampleSize);

            //Debug.Log(string.Format("Processing {0} time domain samples for FFT", iterations));
            double[] sampleChunk = new double[spectrumSampleSize];
            for (int i = 0; i < iterations; i++)
            {
                // Grab the current 1024 chunk of audio sample data
                Array.Copy(preProcessedSamples, i * spectrumSampleSize, sampleChunk, 0, spectrumSampleSize);

                // Apply our chosen FFT Window
                double[] windowCoefs = DSP.Window.Coefficients(DSP.Window.Type.Hanning, (uint)spectrumSampleSize);
                double[] scaledSpectrumChunk = DSP.Math.Multiply(sampleChunk, windowCoefs);
                double scaleFactor = DSP.Window.ScaleFactor.Signal(windowCoefs);

                // Perform the FFT and convert output (complex numbers) to Magnitude
                Complex[] fftSpectrum = fft.Execute(scaledSpectrumChunk);
                double[] scaledFFTSpectrum = DSPLib.DSP.ConvertComplex.ToMagnitude(fftSpectrum);
                scaledFFTSpectrum = DSP.Math.Multiply(scaledFFTSpectrum, scaleFactor);

                // These 1024 magnitude values correspond (roughly) to a single point in the audio timeline
                float curSongTime = getTimeFromIndex(i) * spectrumSampleSize;

                // Send our magnitude data off to our Spectral Flux Analyzer to be analyzed for peaks
                _preProcessedSpectralFluxAnalyzer.analyzeSpectrum(Array.ConvertAll(scaledFFTSpectrum, x => (float)x), curSongTime);
                callback((i * 100) / iterations);
            }

            callback(100);
            //Debug.Log("Spectrum Analysis done");
            //Debug.Log("Background Thread Completed");
            _loadComplete = true;
        }
        catch (Exception e)
        {
            // Catch exceptions here since the background thread won't always surface the exception to the main thread
            Debug.Log(e.ToString());
        }
    }

    public void PrepareTrack(AudioClip clip, Action<float> callback)
    {
        _mockSource.clip = clip;
        _hearableSource.clip = clip;

        _preProcessedSpectralFluxAnalyzer = new SpectralFluxAnalyzer();
        // Need all audio samples.  If in stereo, samples will return with left and right channels interweaved
        // [L,R,L,R,L,R]
        _multiChannelSamples = new float[_mockSource.clip.samples * _mockSource.clip.channels];
        _numChannels = _mockSource.clip.channels;
        _numTotalSamples = _mockSource.clip.samples;
        _clipLength = _mockSource.clip.length;

        // We are not evaluating the audio as it is being played by Unity, so we need the clip's sampling rate
        this._sampleRate = _mockSource.clip.frequency;

        _mockSource.clip.GetData(_multiChannelSamples, 0);
        //Debug.Log("GetData done");

        Thread bgThread = new Thread(() => this.getFullSpectrumThreaded(callback));

        //Debug.Log("Starting Background Thread");
        bgThread.Start();

    }
}