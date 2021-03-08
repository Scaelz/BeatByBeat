using EventBusSystem;
public interface IMoverActivatedHandler : IGlobalSubscriber
{
    void AddNewMover(IDestinationMove mover);
}
