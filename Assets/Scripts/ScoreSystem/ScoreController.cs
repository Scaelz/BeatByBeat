using EventBusSystem;

public class ScoreController : IController, INoteCollectedHandler, IApplicationRequestHandler
{
    ScoreData _scoreData;
    int _currentMultiplier = 1;
    int _totalScore;

    public ScoreController(ScoreData scoreData)
    {
        _scoreData = scoreData;
        EventBus.Subscribe(this);
    }

    ~ScoreController()
    {
        EventBus.Unsubscribe(this);
    }

    public void ApplycationRequestHandle(ApplicationRequest request)
    {
        if (request == ApplicationRequest.Reload)
        {
            _totalScore = 0;
            _currentMultiplier = 1;
        }
    }

    public void CollectNote(NoteCollectArgs args)
    {
        var accuracy = args.Accuracy;
        var scoreArgs = new ScoreEarnedArgs();

        if (accuracy >= 0.7f)
        {
            scoreArgs.Rate = ScoreRate.Poor;
            _currentMultiplier = 1;
        }
        else if (accuracy >= 0.45f && accuracy < 0.7f)
        {
            scoreArgs.Rate = ScoreRate.Good;
            _currentMultiplier = 1;
        }
        else if (accuracy > 0 && accuracy < 0.45f)
        {
            scoreArgs.Rate = ScoreRate.Perfect;
            if (_currentMultiplier != _scoreData.maxMultiplier)
                _currentMultiplier++;
        }
        else
        {
            return;
        }
        scoreArgs.CurrentIncome = _scoreData.noteScore * _currentMultiplier;
        _totalScore += scoreArgs.CurrentIncome;
        scoreArgs.Total = _totalScore;
        scoreArgs.Multiplier = _currentMultiplier;
        scoreArgs.ScorePosition = args.Collectable.transform.position;

        EventBus.RaiseEvent<IScoreEarnedHandler>(x => x.ProcessScoreData(scoreArgs));
    }
}
