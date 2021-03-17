using System.Collections;
using System;
using System.Linq;
using UnityEngine;
using TMPro;
using EventBusSystem;
public class ScoreUI : MonoBehaviour, IScoreEarnedHandler, IApplicationRequestHandler
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
            _globalScore.text = String.Concat(Enumerable.Repeat("0", 5 - scoreText.Length)) + scoreText + " pts";

            yield return null;
        }
    }

    public void ApplycationRequestHandle(ApplicationRequest request)
    {
        if (request == ApplicationRequest.Reload)
        {
            _globalScore.text = "00000 pts";
            _multiplier.text = "1x";
        }
    }
}
