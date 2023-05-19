using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SingleDeviceDeploy))]
public class SingleDeviceDeployEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SingleDeviceDeploy myScript = (SingleDeviceDeploy)target;

        if (GUILayout.Button("放置设备"))
        {
            //myScript.GetInfo();
            myScript.GetPosition();
        }
    }
}
