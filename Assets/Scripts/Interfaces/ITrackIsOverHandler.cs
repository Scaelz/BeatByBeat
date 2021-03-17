using EventBusSystem;
using UnityEngine;

public interface ITrackIsOverHandler : IGlobalSubscriber
{
    void CloseSession();
}
