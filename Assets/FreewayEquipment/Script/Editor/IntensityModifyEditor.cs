using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IntensityModify))]
public class IntensityModifyEditor : Editor
{
    public IntensityModify myScript;

    public override void OnInspectorGUI()
    {
        myScript = (IntensityModify)target;

        DrawDefaultInspector();

        if (GUILayout.Button("修改设置"))
            myScript.OnModifyTheLampsValue();

    }
}
