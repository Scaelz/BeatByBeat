using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory<T> : IFactory<T> where T : MonoBehaviour, IPlayer
{
    PlayerData _playerData;

    public PlayerFactory(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public T Create()
    {
        var playerObject = GameObject.Instantiate(_playerData.Prefab);
        var playerComponent = playerObject.AddComponent<T>();
        playerComponent.Destination = _playerData.startPosition;
        playerComponent.Speed = _playerData.speed;
        return playerComponent;
    }
}
