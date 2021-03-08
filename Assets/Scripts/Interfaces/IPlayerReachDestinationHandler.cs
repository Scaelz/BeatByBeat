using EventBusSystem;

public interface IPlayerReachDestinationHandler : IGlobalSubscriber
{
    void AtArrivalsAction();
}
