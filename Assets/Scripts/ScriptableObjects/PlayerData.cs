using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player")]
public class PlayerData : ScriptableObject
{
    public GameObject Prefab;
    public Vector3 startPosition;
    public float speed;

    public int TriggerTimeMilliseconds;
}
