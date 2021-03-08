using System.Net.NetworkInformation;
using UnityEngine;
using EventBusSystem;

public interface IBlockSpawnRequestHandler : IGlobalSubscriber
{
    void Spawn(SpawnArgs args);
}
