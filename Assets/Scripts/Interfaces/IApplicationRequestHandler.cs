using EventBusSystem;

public interface IApplicationRequestHandler : IGlobalSubscriber
{
    void ApplycationRequestHandle(ApplicationRequest request);
}
