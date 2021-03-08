using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "Data/Global")]
public class GlobalData : ScriptableObject
{
    public string PlayerDataPath;
    public string BlockDataPath;
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
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(Path.ChangeExtension(path, null));
    }
}
