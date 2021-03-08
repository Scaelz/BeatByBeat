using UnityEngine;

public interface IDestinationMove
{
    Vector3 Destination { get; set; }
    float Speed { get; set; }
    void Move();
}
