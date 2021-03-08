using UnityEngine;

public class BlockFactory<T> : IFactory<T> where T : MonoBehaviour, IBlock
{
    BlockData _blockData;

    public BlockFactory(BlockData blockData)
    {
        _blockData = blockData;
    }

    public T Create()
    {
        var blockInstance = GameObject.Instantiate(_blockData.Prefab);
        var blockComponent = blockInstance.AddComponent<T>();
        blockComponent.Destination = _blockData.moveDirection;
        blockComponent.Speed = _blockData.speed;
        return blockComponent;
    }
}
