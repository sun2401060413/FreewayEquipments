using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateModelMesh))]
public class CteateModelMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CreateModelMesh myScript = (CreateModelMesh)target;

        DrawDefaultInspector();

        if (GUILayout.Button("生成CUBE"))
            myScript.DrawCubeObject();

        if (GUILayout.Button("生成PLANE"))
            myScript.DrawPlaneObject();

        if (GUILayout.Button("生成道路"))
            myScript.DrawPlaneBetweenTwoPoints();

        if (GUILayout.Button("生成区域盒"))
            myScript.DrawClubBetweenTwoPoints();
    }
}
