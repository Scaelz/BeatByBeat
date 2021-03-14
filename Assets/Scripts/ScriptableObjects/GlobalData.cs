using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "Data/Global")]
public class GlobalData : ScriptableObject
{
    public string PlayerDataPath;
    public string BlockDataPath;
    public string ScoreDataPath;
    public string MusicDataPath;
    private PlayerData _playerData;
    public PlayerData PlayerData
    {
        get
        {
            if (_playerData == null)
            {
                _playerData = Load<PlayerData>("Data/" + PlayerDataPath);
            }
            return _playerData;
        }
    }
    private BlockData _blockData;
    public BlockData BlockData
    {
        get
        {
            if (_blockData == null)
            {
                _blockData = Load<BlockData>("Data/" + BlockDataPath);
            }
            return _blockData;
        }
    }
    private ScoreData _scoreData;
    public ScoreData ScoreData
    {
        get
        {
            if (_scoreData == null)
            {
                _scoreData = Load<ScoreData>("Data/" + ScoreDataPath);
            }
            return _scoreData;
        }
    }
    private MusicData _musicData;
    public MusicData MusicData
    {
        get
        {
            if (_musicData == null)
            {
                _musicData = Load<MusicData>("Data/" + MusicDataPath);
            }
            return _musicData;
        }
    }
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(Path.ChangeExtension(path, null));
    }
}
