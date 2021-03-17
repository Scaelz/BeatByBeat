using UnityEngine;
public interface IPlayer : IDestinationMove, IComponent
{
    void SetStartPosition(Vector3 position);
}
