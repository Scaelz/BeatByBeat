using UnityEngine;
using EventBusSystem;
using DG.Tweening;

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
            Jump();
        }
    }
    public float Speed { get; set; }
    bool _arrived;

    public void Move()
    {
        var targetPos = new Vector3(Destination.x, transform.position.y, Destination.z);

        // transform.position = Vector3.Lerp(transform.position, targetPos, Speed);
        if (!_arrived && (targetPos - transform.position).sqrMagnitude < 0.1f)
        {
            EventBus.RaiseEvent<IPlayerReachDestinationHandler>(x => x.AtArrivalsAction());
            _arrived = true;
        }
    }

    private void Jump()
    {
        transform.DOMoveX(Destination.x, Speed).SetEase(Ease.OutQuad);
        transform.DOMoveZ(Destination.z, Speed).SetEase(Ease.OutQuad);
        transform.DOBlendableMoveBy(new Vector3(0, 0.75f, 0), Speed * 0.5f).SetLoops(2, LoopType.Yoyo);
    }

    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
    }
}
