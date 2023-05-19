using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InfoRepairing))]
public class InfoRepairingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InfoRepairing myScript = (InfoRepairing)target;


        if (GUILayout.Button("修复设备空缺引用"))
        {
            myScript.OnRepairNullInfoInDevices();
        }

        if (GUILayout.Button("统计Devices设备数量"))
        {
            myScript.GetDevicesCount();
        }

        if (GUILayout.Button("修复控制管理器"))
        {
            myScript.OnRepairNullInfoInController();
        }
    }


}
