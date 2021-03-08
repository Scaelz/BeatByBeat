using System.Collections.ObjectModel;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class RateWords
{
    public ScoreRate Rate;
    public List<string> Words;
    public Color Color;
}

[CreateAssetMenu(fileName = "ScoreData", menuName = "Data/Score")]
public class ScoreData : ScriptableObject
{
    public int noteScore = 20;
    public float maxMultiplier = 10;
    public GameObject InfoTextPrefab;
    public float TextDuration;
    public RateWords[] RateWords;
}
