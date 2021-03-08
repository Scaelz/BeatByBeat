using EventBusSystem;
public interface IScoreEarnedHandler : IGlobalSubscriber
{
    void ProcessScoreData(ScoreEarnedArgs args);
}
