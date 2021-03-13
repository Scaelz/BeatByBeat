using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "AnnounceTextData", menuName = "Data/AnnounceText")]
public class AnnounceTextData : ScriptableObject
{
    public GameObject Prefab;
    public float DefaultX = 2000;

    [Header("In properties")]
    public float InPositionX = 0;
    public float InTime = 0.75f;
    public Ease InEase = Ease.OutElastic;

    [Header("Out properties")]
    public float OutPositionX = -2000;
    public float OutTime = 0.4f;
    public Ease OutEase = Ease.InFlash;
}
