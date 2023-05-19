using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateTools))]
public class CreateToolsEditor : Editor
{
    private GUIStyle boxStyle;
    private CreateTools myScript;

    private void OnEnable()
    {
        myScript = (CreateTools)target;
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
        GUILayout.Label("欢迎使用CreateTools！");
        GUI.skin.label.fontStyle = FontStyle.Normal;
        GUILayout.BeginHorizontal();
        GUILayout.Label("Version: 0.0.5");
        GUILayout.Label("Created by Sun Zhu on July 13, 2020");
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        DrawDefaultInspector();

        GUILayout.BeginHorizontal();
        GUILayout.Label("数据文件地址");
        GUILayout.TextField(myScript.filepath);
        if (GUILayout.Button("选择文件", GUILayout.Width(80)))
        {
            // TODO: 待补充更多文件格式.
            string[] fliter = { "xlsx和xls文件", "xlsx;*.xls;" };
            myScript.filepath = EditorUtility.OpenFilePanelWithFilters("打开数据文件", "C:\\", fliter);
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginVertical("", boxStyle);



        EditorGUILayout.LabelField("STEP:1");
        if (GUILayout.Button("1.创建桩号生成管理器"))
            myScript.CreateRoadMileageSetter();

        if (myScript.roadUpward == null && myScript.roadDownward != null)
            EditorGUILayout.HelpBox("请先设置上行路径！", MessageType.Error);
        else if (myScript.roadUpward != null && myScript.roadDownward == null)
            EditorGUILayout.HelpBox("请先设置下行路径！", MessageType.Error);
        else if (myScript.roadUpward == null && myScript.roadDownward == null)
            EditorGUILayout.HelpBox("请先设置上/下行路径！", MessageType.Error);


        EditorGUILayout.LabelField("STEP:2");
        if (GUILayout.Button("2.创建SocketServer"))
            myScript.CreateSocketServer();

        EditorGUILayout.LabelField("STEP:3");
        if (GUILayout.Button("3.创建设备事件管理器"))
            myScript.CreateDevicesAndController();
        if (myScript.socketserv == null)
            EditorGUILayout.HelpBox("请先创建SocketServer！", MessageType.Error);

        EditorGUILayout.LabelField("STEP:4");
        if (GUILayout.Button("4.创建数据读取器"))
            myScript.CreateDataLoader();
        if (myScript.filepath.Length == 0)
            EditorGUILayout.HelpBox("请先设置文件路径！", MessageType.Error);

        EditorGUILayout.LabelField("STEP:5");
        if (GUILayout.Button("5.创建自定义摄像机"))
            myScript.CreateCamPlayer();
        if (myScript.roadtraffic == null)
            EditorGUILayout.HelpBox("请设置交通流对象！", MessageType.Error);

        EditorGUILayout.LabelField("STEP:6");
        if (GUILayout.Button("6.创建DemoUI"))
            myScript.CreateDemoUI();

        GUILayout.EndVertical();
    }

}
