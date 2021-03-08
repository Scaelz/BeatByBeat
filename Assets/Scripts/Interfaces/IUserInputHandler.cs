using EventBusSystem;


public interface IUserInputHandler : IGlobalSubscriber
{
    void UpdatePlayerDestination(Note note);
}
