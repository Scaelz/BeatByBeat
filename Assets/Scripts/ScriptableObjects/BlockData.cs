using UnityEngine;
using System;

[Serializable]
public class BlockMaterial
{
    public Note Note;
    public Material Material;
}

[CreateAssetMenu(fileName = "BlockData", menuName = "Data/Block")]
public class BlockData : ScriptableObject
{
    public GameObject Prefab;
    public GameObject ShutteredPrefab;
    public float timeToReachPlayer;
    public Vector3 spawnOffset;
    public Vector3 moveDirection = Vector3.back;
    public float speed;
    public BlockMaterial[] BlockMaterials;
    public int PrewarmCount = 10;
    public float CollectTime = 1;
}
