using System.Collections;
using UnityEngine;
using TMPro;
using EventBusSystem;
public class ScoreUI : MonoBehaviour, IScoreEarnedHandler
{
    [SerializeField]
    TextMeshProUGUI _globalScore;
    [SerializeField]
    TextMeshProUGUI _multiplier;
    int currentScoreValue;

    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public void ProcessScoreData(ScoreEarnedArgs args)
    {
        StopAllCoroutines();
        StartCoroutine(ConsistenlyUpdateScore(args.Total));
        UpdateMultiplier(args.Multiplier);
    }

    void UpdateMultiplier(int multiplier)
    {
        _multiplier.text = "x" + multiplier.ToString();
    }

    IEnumerator ConsistenlyUpdateScore(int score)
    {
        while (currentScoreValue != score)
        {
            currentScoreValue++;
            var scoreText = currentScoreValue.ToString();
            for (int i = 0; i <= 5 - scoreText.Length; i++)
            {
                scoreText = scoreText.Insert(0, "0");
            }
            _globalScore.text = scoreText + " pts";

            yield return null;
        }
    }
}
