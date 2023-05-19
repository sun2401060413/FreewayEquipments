using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GuardRailRepairing))]
public class GuardRailRepairingEditror : Editor
{
    GuardRailRepairing myScript;
    private void OnEnable()
    {
        myScript = (GuardRailRepairing)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.BeginVertical();
        if (GUILayout.Button("修复材质"))
            myScript.GuardRailMaterialRecovery();

        GUILayout.EndVertical();
    }
}
