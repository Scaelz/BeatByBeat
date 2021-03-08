using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusSystem;
using System.Linq;

public class SpawnArgs : EventArgs
{
    public Vector3 position;
    public Note note;
}

public class BlockSpawnPostionCalculator : IBeatReadyHandler
{

    BlockData _blockData;
    MovePoint[] _movePoints;

    public BlockSpawnPostionCalculator(BlockData blockData, MovePoint[] movePoints)
    {
        _blockData = blockData;
        _movePoints = movePoints;
        EventBus.Subscribe(this);
    }

    ~BlockSpawnPostionCalculator()
    {
        EventBus.Unsubscribe(this);
    }

    public void OnBeatReady(Note note)
    {
        var movePoint = _movePoints.Where(x => x.note == note).FirstOrDefault();
        var offsetDistance = new Vector3(0, 0.0f, _blockData.timeToReachPlayer * _blockData.speed);
        var spawnPoint = movePoint.transform.position + offsetDistance;
        var args = new SpawnArgs() { position = spawnPoint, note = note };
        EventBus.RaiseEvent<IBlockSpawnRequestHandler>(x => x.Spawn(args));
    }
}
