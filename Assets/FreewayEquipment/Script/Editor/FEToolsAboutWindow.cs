using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FEToolsAboutWindow : EditorWindow
{
    private Texture2D top_logo;

    FEToolsAboutWindow()
    {
        this.titleContent = new GUIContent("About");
    }

    private void OnEnable()
    {
        top_logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/FreewayEquipment/Other/Image/CA_Logo_b.png");
    }

    private void OnGUI()
    {
        // 绘制logo
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(top_logo, GUILayout.Width(90), GUILayout.Height(90));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        // 绘制标题
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 18;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Freeway Equipments tools");
        GUI.skin.label.fontSize = 14;
        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        GUILayout.Label("Version 0.0.5");
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        GUILayout.Label("Created by Sun Zhu on July 13, 2020");
        GUILayout.Label("Updated by Sun Zhu on July 20, 2020");
        GUI.skin.label.fontSize = 14;
        GUILayout.Label("长安大学交通系统工程研究所");

        GUILayout.EndVertical();

        if (GUILayout.Button("在线手册", GUILayout.Height(20)))
            Application.OpenURL("https://sun_zhu.gitee.io/notes/2019_summer/Unity/FreewayEquipment/");

        ////添加名为"Save Bug"按钮，用于调用SaveBug()函数
        //if (GUILayout.Button("退出"))
        //{
        //    SaveBug();
        //}

    }

    private void OnCancel()
    {

    }
}
