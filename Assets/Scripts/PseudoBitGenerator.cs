
using EventBusSystem;

public class PseudoBitGenerator
{
    public void OnTimedEvent()
    {
        EventBus.RaiseEvent<IBeatReadyHandler>(x => x.OnBeatReady((Note)UnityEngine.Random.Range(0, 4)));
    }
}
