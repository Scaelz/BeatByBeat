using EventBusSystem;

public interface IBeatReadyHandler : IGlobalSubscriber
{
    void OnBeatReady(Note note);
}
