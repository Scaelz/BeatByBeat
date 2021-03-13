using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusSystem;

public class TriggerActivationController : IPlayerReachDestinationHandler
{
    Collider _collider;
    int _activationTime;

    public TriggerActivationController(IPlayer player, int activeTimeMilliseconds)
    {
        EventBus.Subscribe(this);
        _collider = player.gameObject.GetComponent<Collider>();
        _activationTime = activeTimeMilliseconds;
    }

    ~TriggerActivationController()
    {
        EventBus.Unsubscribe(this);
    }

    async void DelayedDeactivation()
    {
        await Task.Run(() => Timer());
        _collider.enabled = false;
    }

    void Timer()
    {
        Thread.Sleep(_activationTime);
    }

    public void AtArrivalsAction()
    {
        _collider.enabled = true;
        DelayedDeactivation();
    }
}
