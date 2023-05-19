using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ImportRoadLineData))]
public class ImportRoadLineDataEditor : Editor
{
    private GUIStyle boxStyle;
    private ImportRoadLineData myImportData;

    private void OnEnable()
    {
        myImportData = (ImportRoadLineData)target;
    }
    
    public override void OnInspectorGUI()
    {
        if (boxStyle == null)
        {
            boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.textColor = GUI.skin.label.normal.textColor;
            boxStyle.fontStyle = FontStyle.Bold;
            boxStyle.alignment = TextAnchor.UpperLeft;
        }

        GUILayout.BeginVertical("", boxStyle);
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUILayout.Label("欢迎使用RoadLineDataImporter！");
        GUI.skin.label.fontStyle = FontStyle.Normal;
        GUILayout.BeginHorizontal();
        GUILayout.Label("Version: 0.0.1");
        GUILayout.Label("Created by Sun Zhu on Sep 27, 2022");
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        DrawDefaultInspector();

        GUILayout.BeginHorizontal();
        GUILayout.Label("数据文件地址");
        GUILayout.TextField(myImportData.filepath);

        if (GUILayout.Button("选择文件", GUILayout.Width(80)))
        {
            // TODO: 待补充更多文件格式.
            string[] fliter = { "xlsx和xls文件", "xlsx;*.xls;" };
            myImportData.filepath = EditorUtility.OpenFilePanelWithFilters("打开数据文件", "C:\\", fliter);
            myImportData.LoadDeviceDataFromXLS();
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginVertical("", boxStyle);
        EditorGUILayout.LabelField("STEP:1");
        if (GUILayout.Button("创建道路"))
        {
            myImportData.CreateRoadlineViaPts("RoadLine");
            SceneView.RepaintAll();
        }

        EditorGUILayout.LabelField("STEP:2");
        if (GUILayout.Button("创建隧道"))
        {
            // myImportData.CreateTunnelCellsViaPts(myImportData.TunnelInfoList);
            myImportData.CreateTunnelCellsViaPts();
            // myImportData.CreateTunnelViaPts();
        }

        GUILayout.EndVertical();
    }
    
}
