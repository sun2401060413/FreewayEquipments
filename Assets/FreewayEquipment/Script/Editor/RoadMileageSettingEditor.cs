using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadMileageSetting))]
public class RoadMileageSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.BeginVertical();
        EditorGUILayout.HelpBox("路径对象TrafficObj可选用UTS-PRO生成的交通流对象" +
                                "或其他保持Obj-points-pt从属关系的对象。定位可选择" +
                                "对象点集中的某个点。注意:为保持设备安装偏置较小" +
                                "最好将点集置于道路中线", MessageType.Info);
        RoadMileageSetting myScript = (RoadMileageSetting)target;
        if (GUILayout.Button("计算桩号"))
        {
            myScript.GetInfo();
        }
        GUILayout.EndVertical();
    }

}
