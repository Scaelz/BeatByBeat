using System.Reflection;
using UnityEngine;


[CreateAssetMenu(fileName = "MusicData", menuName = "Data/Music")]
public class MusicData : ScriptableObject

{
    public AudioSource MockSource;
    public AudioSource RealSource;
    public AudioClip[] TrackList;
}
