using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VehicleDetectorMLControl))]
public class VehicleDetectorMLControlEditor : Editor
{
    Texture2D preview;
    private void OnEnable()
    {
        preview = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/FreewayEquipment/Other/Image/VehicleDetectorNumber.png");
    }

    public override void OnInspectorGUI()
    {
        VehicleDetectorMLControl myScript = (VehicleDetectorMLControl)target;

        DrawDefaultInspector();

        EditorGUILayout.HelpBox("车检器编号规则:以道路某点上行方向(桩号增大方向)道路右侧为起点，" +
                                "以道路下行方向(桩号减小方向)道路右侧为终点，从起点到终点按车道依"+
                                "次编号。其中奇数编号用于编码上行方向检测器，偶数编号用于编码下行"+
                                "方向检测器", MessageType.Info);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //GUILayout.Label(preview);
        GUILayout.Label(preview, GUILayout.Width(200),GUILayout.Height(200));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        //if (GUILayout.Button("报文生成测试"))
        //    Debug.Log(myScript.GetDetectorDataStr());

    }
}
