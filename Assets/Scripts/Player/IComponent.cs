using UnityEngine;

public interface IComponent
{
    Transform transform { get; }
    GameObject gameObject { get; }
}
