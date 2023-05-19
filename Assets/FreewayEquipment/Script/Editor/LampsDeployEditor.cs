using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LampsDeploy))]
public class LampsDeployEditor : Editor
{
    Texture2D preview;

    private void OnEnable()
    {
        preview = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/FreewayEquipment/Other/Image/LampDeploy.png");
    }

    public override void OnInspectorGUI()
    {
        LampsDeploy myScript = (LampsDeploy)target;

        DrawDefaultInspector();

        EditorGUILayout.HelpBox("灯具回路编码决定了灯具位置的计算基准(入口以Start为起点，出口以end为终点计算，其余为基本段)" + 
                                "灯具回路编码规则:入口/入口过渡段编码为1，出口/出口过渡段编码为2" +
                                "剩余的为基本段，编码为3", MessageType.Info);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //GUILayout.Label(preview);
        GUILayout.Label(preview, GUILayout.Width(300), GUILayout.Height(300));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        if (GUILayout.Button("生成照明回路"))
        {
            myScript.DeployGroupOfLamps();
        }
    }

}
