using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusSystem;
using System.Threading;

public class NoteCollectController : IController, INoteCollectedHandler
{
    BlockData _blockData;
    Pool<Block> _blockPool;
    public NoteCollectController(BlockData blockData, Pool<Block> blockPool)
    {
        EventBus.Subscribe(this);
        _blockData = blockData;
        _blockPool = blockPool;
    }

    ~NoteCollectController()
    {
        EventBus.Unsubscribe(this);
    }

    public void CollectNote(NoteCollectArgs args)
    {
        if (args.Accuracy < 0)
        {
            RestoreNote((Block)args.Collectable);
            return;
        }
        args.CollectionTime = _blockData.CollectTime;
        args.Collectable.Collect(args);
        var block = (Block)args.Collectable;
        RestoreNote(block);

    }

    async void RestoreNote(Block obj)
    {
        await Task.Run(() => Timer());
        _blockPool.ResetMember(obj);
    }
    void Timer()
    {
        int delay = (int)TimeSpan.FromSeconds(_blockData.CollectTime).TotalMilliseconds * 3;
        Thread.Sleep(delay);
    }
}
