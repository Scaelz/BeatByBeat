using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EventBusSystem;

public class PlayerDestinationSelector : IUserInputHandler
{
    private readonly IPlayer _player;
    private readonly MovePoint[] _movePoints;

    public PlayerDestinationSelector(IPlayer player, MovePoint[] movePoints)
    {
        _player = player;
        _movePoints = movePoints;
        EventBus.Subscribe(this);
    }

    ~PlayerDestinationSelector()
    {
        EventBus.Unsubscribe(this);
    }

    public void UpdatePlayerDestination(Note note)
    {
        var pos = _movePoints.Where(x => x.note == note).FirstOrDefault();
        _player.Destination = pos.transform.position;
    }
}
