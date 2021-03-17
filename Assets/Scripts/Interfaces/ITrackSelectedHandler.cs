using System;
using EventBusSystem;
using UnityEngine;

public interface ITrackSelectedHandler : IGlobalSubscriber
{
    void PrepareTrack(AudioClip clip, Action<float> callback);
}
