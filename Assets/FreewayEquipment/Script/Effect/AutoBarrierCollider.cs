using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AutoBarrierCollider : MonoBehaviour
{
    public bool hasVechicleInFront = false;
    public string tagName;
    public GameObject cldObj;

    /// <summary>
    /// 设定碰撞器内如果有车辆时，hasVehicleInFront 布尔量为true;
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider other)
    {
        hasVechicleInFront = true;
        cldObj = other.transform.parent.gameObject;
        tagName = cldObj.tag;
    }

    private void OnTriggerExit(Collider other)
    {
        hasVechicleInFront = false;
        cldObj = null;
        tagName = "Untagged";
    }


}
