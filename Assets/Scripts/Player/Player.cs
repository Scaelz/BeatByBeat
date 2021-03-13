using UnityEngine;
using EventBusSystem;

public class Player : MonoBehaviour, IPlayer
{
    private Vector3 _destination;
    public Vector3 Destination
    {
        get => _destination;
        set
        {
            _arrived = false;
            _destination = value;
        }
    }
    public float Speed { get; set; }
    bool _arrived;

    public void Move()
    {
        var targetPos = new Vector3(Destination.x, transform.position.y, Destination.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, Speed);
        if (!_arrived && (targetPos - transform.position).sqrMagnitude < 0.1f)
        {
            EventBus.RaiseEvent<IPlayerReachDestinationHandler>(x => x.AtArrivalsAction());
            _arrived = true;
        }
    }
}
