using UnityEngine;
using EventBusSystem;

public class ScoreController : IController, INoteCollectedHandler
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

    public void CollectNote(NoteCollectArgs args)
    {
        var accuracy = args.Accuracy;
        var scoreArgs = new ScoreEarnedArgs();

        if (accuracy >= 0.5f)
        {
            scoreArgs.Rate = ScoreRate.Poor;
        }
        else if (accuracy >= 0.2f && accuracy < 0.5f)
        {
            scoreArgs.Rate = ScoreRate.Good;
        }
        else if (accuracy > 0 && accuracy < 0.2f)
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
