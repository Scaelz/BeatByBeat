using UnityEngine;
using EventBusSystem;
using System.Linq;

public class BlockSpawnerController<T> : IController, IBlockSpawnRequestHandler
where T : MonoBehaviour, IBlock
{
    Pool<T> _blockPool;
    BlockData _blockData;
    public BlockSpawnerController(Pool<T> blockPool, BlockData blockData)
    {
        EventBus.Subscribe(this);
        _blockPool = blockPool;
        _blockData = blockData;
        for (int i = 0; i < blockData.PrewarmCount; i++)
        {
            var block = _blockPool.Create();
            _blockPool.ResetMember(block);
        }
    }

    ~BlockSpawnerController()
    {
        EventBus.Unsubscribe(this);
    }

    public void Spawn(SpawnArgs args)
    {
        var block = _blockPool.Allocate();
        block.transform.position = args.position;
        block.SetMaterial(_blockData.BlockMaterials.Where(x => x.Note == args.note).FirstOrDefault().Material);
        EventBus.RaiseEvent<IMoverActivatedHandler>(x => x.AddNewMover(block));
    }
}
