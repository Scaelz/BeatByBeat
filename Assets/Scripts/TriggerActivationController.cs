using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusSystem;

public class TriggerActivationController : IUserInputHandler
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

    public void UpdatePlayerDestination(Note note)
    {
        _collider.enabled = true;
        WaitForSeconds();
    }

    async void WaitForSeconds()
    {
        await Task.Run(() => Timer());
        _collider.enabled = false;
    }

    void Timer()
    {
        Thread.Sleep(_activationTime);
    }
}
