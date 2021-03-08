using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    public Vector3 Destination { get; set; }
    public float Speed { get; set; }

    public void Move()
    {
        var targetPos = new Vector3(Destination.x, transform.position.y, Destination.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, Speed);
    }
}
