using UnityEngine;
public interface IBlock : IActivatable, IRevertable, IDestinationMove
{
    void SetMaterial(Material material);
}
