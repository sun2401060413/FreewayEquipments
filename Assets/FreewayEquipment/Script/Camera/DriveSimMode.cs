using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSimMode : MonoBehaviour
{
    public GameObject Cam;
    public GameObject Car;
    public Vector3 Offset = Vector3.zero;

    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Car)
        {
            pos = Car.transform.position;

            Cam.transform.position = pos + Offset.x*Car.transform.forward + Offset.y*Car.transform.up + Offset.z*Car.transform.right;

            Cam.transform.rotation = Car.transform.rotation;

        }
    }
}
