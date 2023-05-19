using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverMode : MonoBehaviour
{
    public GameObject path;

    private GameObject SelectedCar;
    private Vector3 pos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (SelectedCar)
        {
            pos = SelectedCar.transform.position;

            int type = SelectedCar.GetComponent<VehicleTypeIdentity>().SizeType;
            if (type == 0)
            {
                pos.x = pos.x + 0.3f;
                pos.y = pos.y + 1.4f;
                pos.z = pos.z + 0.38f;
            }
            else if (type == 1)
            {
                pos.x = pos.x + 1f;
                pos.y = pos.y + 2f;
                pos.z = pos.z + 1.5f;
            }
            else if (type == 3)
            {
                pos.x = pos.x + 0.3f;
                pos.y = pos.y + 2f;
                pos.z = pos.z + 1.7f;
            }
            else
            {
                pos.x = pos.x + 0.3f;
                pos.y = pos.y + 2f;
                pos.z = pos.z + 1.5f;
            }

            transform.rotation = SelectedCar.transform.rotation;

            transform.position = pos;
        }
        else
        {
            SelectedCar = GetACar();
        }

    }
    
    

    public GameObject GetACar()
    {
        GameObject obj = new GameObject();
        int index = path.transform.GetChild(1).transform.childCount;
        try
        {
            obj = path.transform.GetChild(1).GetChild(index - 1).gameObject;
        }
        catch (System.Exception)
        {
            obj = null;
            throw;
        }

        return obj;
    }

    public void DeleteSelectedCar()
    {
        //SelectedCar = null;
        SelectedCar = GetACar();
    }
}
