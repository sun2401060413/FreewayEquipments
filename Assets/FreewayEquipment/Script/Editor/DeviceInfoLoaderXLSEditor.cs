using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DeviceInfoLoaderXLS))]
public class DeviceInfoLoaderXLSEditor : Editor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DeviceInfoLoaderXLS myScript = (DeviceInfoLoaderXLS)target;


        if (GUILayout.Button("读入数据"))
        {
            myScript.LoadDeviceDataFromXLS();
            //Debug.Log(myScript.filepath);
        }
    }



}
