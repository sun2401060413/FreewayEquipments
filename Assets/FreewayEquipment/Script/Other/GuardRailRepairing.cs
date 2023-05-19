using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardRailRepairing : MonoBehaviour
{
    public Material mat;
    public GameObject obj;

    public void GuardRailMaterialRecovery()
    {
        foreach (Transform child in obj.transform)
        {
            ////Debug.Log(child.name);
            //MeshRenderer mr = child.GetChild(0).GetComponent<MeshRenderer>();
            //try
            //{
            //    mr.materials[0] = mat;
            //}
            //catch
            //{
            //    mr.sharedMaterials[0] = mat;
            //}
            child.gameObject.AddComponent<BoxCollider>();
        }
    }

}
