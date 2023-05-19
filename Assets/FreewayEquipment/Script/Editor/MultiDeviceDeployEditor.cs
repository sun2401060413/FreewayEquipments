using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MultiDeviceDeploy))]
public class MultiDeviceDeployEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MultiDeviceDeploy myScript = (MultiDeviceDeploy)target;


        if (GUILayout.Button("设备部署"))
        {
            myScript.DevicesDeploy();
        }
    }

}
