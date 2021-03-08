using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTest : MonoBehaviour
{
    public float Speed;
    public Vector3 Destination;
    Vector3 last;


    // Start is called before the first frame update
    void Start()
    {
        last = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var time = Time.deltaTime;
        Debug.Log($"Time {time} with speed {Speed}. Distance {Speed * time}");
        transform.position += new Vector3(0, 0, Destination.z * Speed * time);
        var distance = transform.position - last;
        last = transform.position;
        Debug.Log($"Distance {distance} passed in {time}. Speed {distance / time}");
    }
}
