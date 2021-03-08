using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusSystem;

public class CompositeMoveController : IController, IFrameUpdate, IMoverActivatedHandler
{
    List<IDestinationMove> movers;

    public CompositeMoveController()
    {
        this.movers = new List<IDestinationMove>();
        EventBus.Subscribe(this);
    }

    public CompositeMoveController(IDestinationMove[] movers) : base()
    {
        foreach (var mover in movers)
        {
            this.movers.Add(mover);
        }
    }

    ~CompositeMoveController()
    {
        EventBus.Unsubscribe(this);
    }

    public void Add(IDestinationMove newMover)
    {
        if (!movers.Contains(newMover))
        {
            movers.Add(newMover);
        }
    }

    public void Remove(IDestinationMove moverToDelete)
    {
        if (movers.Contains(moverToDelete))
            movers.Remove(moverToDelete);
    }

    public void FrameUpdate(float deltaTime)
    {
        foreach (var mover in movers)
        {
            mover.Move();
        }
    }

    public void AddNewMover(IDestinationMove mover)
    {
        Add(mover);
    }
}
