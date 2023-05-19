using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LampsDeploy : MonoBehaviour
{
    /// <summary>
    /// 简单版回路设定
    /// </summary>
    [Serializable]
    public class SimpleGroup
    {
        public string name;        // 回路名称;
        public int class_code;     // 回路属性设置:1-入口段、入口过渡段;2-出口段、出口过渡段;3-基本段;
        public float length;       // 回路长度;
        public float space;        // 灯具间隔;
    }

    public GameObject Road;     // 路线对象
    public GameObject Lamp;     // 灯具预制体
    public Vector3 Offset;      // 灯具偏移（只设置一侧即可）

    public float Start;         // 隧道起点（桩号）
    public float End;           // 隧道终点（桩号）

    public GameObject Parent;   // 存放设备对象的父对象（方便管理）

    [Header("按照回路从入口到出口的顺序填入回路布设信息")]
    public List<SimpleGroup> Group_list = new List<SimpleGroup>();

    private RoadMileageSetting Settings;
    private SingleDeviceDeploy Deployer;

    // 基本段起止桩号
    private float BaseSegStart;     // 基本段起始桩号;
    private float BaseSegEnd;       // 基本段终止桩号;

    private float CurrentMileage;   // 当前设备桩号;

    private float multpiler;        // 桩号步进因子(递增为1，递减为-1);

    private List<SimpleGroup> EnterList;        // 入口方向回路设置
    private List<SimpleGroup> ExitList;          // 出口方向
    private SimpleGroup BaseSegProp;

    private float EnterRangeLimit;
    private float ExitRangeLimit;

    public void DeployGroupOfLamps()
    {
        Deployer = Road.GetComponent<SingleDeviceDeploy>();
        Settings = Road.GetComponent<RoadMileageSetting>();

        multpiler = GetMultplier(Settings.isIncrease);

        GetSegmentProp(Group_list);

        BaseSegStart = DeployEnterSeg();
        BaseSegEnd = DeployExitSeg();
        DeployBaseSeg();
    }

    private void GetSegmentProp(List<SimpleGroup> list)
    {
        try
        {
            int index = list.FindIndex(elem => elem.class_code == 3);
            if (index == 0)
            {
                Debug.Log("未找到入口段入口过渡段设置");
                return;
            }
            if (index == list.Count)
            {
                Debug.Log("未找到出口段出口过渡段设置");
                return;
            }

            EnterList = list.GetRange(0, index);
            //foreach (var code in StartList)
            //    Debug.Log("StratPt:" + code.class_code+",name:"+code.name);

            ExitList = list.GetRange(index + 1, list.Count - index - 1);
            //foreach (var code in EndList)
            //    Debug.Log("EndPt:" + code.class_code+",name:"+code.name);

            BaseSegProp = list[index];
            //Debug.Log("BaseSegProp:"+BaseSegProp.class_code+",name:"+ BaseSegProp.name);   

        }
        catch (Exception)
        {
            Debug.Log("未找到基本段设置");
            throw;
        }
        
    }

    private float GetMultplier(bool isIncrease)
    {
        if (isIncrease)
            return 1;
        else
            return -1;
    }


    private float DeployEnterSeg()
    {
        // 从起点开始按照布设规则逐个布设;
        CurrentMileage = Start;
        EnterRangeLimit = Start;
        for (int i = 0; i < EnterList.Count; i++)   // 回路组;
        {
            // 1. 计算回路范围桩号;
            EnterRangeLimit = EnterRangeLimit + EnterList[i].length*multpiler;

            GameObject line_A = new GameObject();       // 线路子节点
            GameObject line_B = new GameObject();       // 线路子节点

            Vector3 Offset_right = Offset;              // A路在右侧（自设）
            Vector3 Offset_left = new Vector3(Offset.x, Offset.y, -Offset.z);  // B路在左侧（自设）

            // 修改线路对象名称
            line_A.name = EnterList[i].name + "_A";
            line_B.name = EnterList[i].name + "_B";

            // 设定
            line_A.transform.parent = Parent.transform;
            line_B.transform.parent = Parent.transform;

            int count = 1;
            while (Math.Abs(CurrentMileage - EnterRangeLimit)>EnterList[i].space)
            {
                // 1. 计算设备桩号
                CurrentMileage = CurrentMileage + EnterList[i].space*multpiler;
                Deployer.obj = Lamp;
                Deployer.mileage = CurrentMileage;

                // 2. 计算设备偏置
                Deployer.Offset = Offset_right;
                // 3. 设备布设
                GameObject _instance_A = Deployer.GetPosition(Deployer.obj, CurrentMileage);
                // 4. 修改设备名称
                _instance_A.name = line_A.name + "_" + count.ToString();
                // 5. 修改设备父节点（便于管理）
                _instance_A.transform.parent = line_A.transform;

                // 2. 计算设备偏置
                Deployer.Offset = Offset_left;
                // 3. 设备布设
                GameObject _instance_B = Deployer.GetPosition(Deployer.obj, CurrentMileage);
                // 调转方向
                
                _instance_B.transform.Rotate(0,180,0);

                // 4. 修改设备名称
                _instance_B.name = line_B.name + "_" + count.ToString();
                // 5. 修改设备父节点（便于管理）
                _instance_B.transform.parent = line_B.transform;

                count = count + 1;
            }

        }
        return CurrentMileage;
    }

    private float DeployExitSeg()
    {
        CurrentMileage = End;
        ExitRangeLimit = End;
        for (int i = ExitList.Count - 1; i >= 0; i--)
        {
            // 1. 计算回路桩号边界
            ExitRangeLimit = ExitRangeLimit - ExitList[i].length * multpiler;

            // 创建回路节点对象
            GameObject line_A = new GameObject();
            GameObject line_B = new GameObject();

            Vector3 Offset_right = Offset;              // A路在右侧（自设）
            Vector3 Offset_left = new Vector3(Offset.x, Offset.y, -Offset.z);  // B路在左侧（自设）

            line_A.name = ExitList[i].name + "_A";
            line_B.name = ExitList[i].name + "_B";

            line_A.transform.parent = Parent.transform;
            line_B.transform.parent = Parent.transform;

            int count = 1;
            while (Math.Abs(CurrentMileage - ExitRangeLimit) > ExitList[i].space)
            {
                // 1. 计算设备布设位置桩号
                CurrentMileage = CurrentMileage - ExitList[i].space * multpiler;
                Deployer.obj = Lamp;
                Deployer.mileage = CurrentMileage;

                // 2. 计算设备偏置
                Deployer.Offset = Offset;
                // 3. 设备布设
                GameObject _instance_A = Deployer.GetPosition(Deployer.obj, CurrentMileage);
                // 4. 修改设备名称
                _instance_A.name = line_A.name + "_" + count.ToString();
                // 5. 修改设备父节点（便于管理）
                _instance_A.transform.parent = line_A.transform;

                // 2. 计算设备偏置
                Deployer.Offset = Offset_left;
                // 3. 设备布设
                GameObject _instance_B = Deployer.GetPosition(Deployer.obj, CurrentMileage);
                // 调转方向

                _instance_B.transform.Rotate(0, 180, 0);

                // 4. 修改设备名称
                _instance_B.name = line_B.name + "_" + count.ToString();
                // 5. 修改设备父节点（便于管理）
                _instance_B.transform.parent = line_B.transform;


                count = count + 1;
            }


        }

        return CurrentMileage;
    }

    private void DeployBaseSeg()
    {
        BaseSegStart = BaseSegStart + BaseSegProp.space*multpiler;  // 错过一个灯位
        BaseSegEnd = BaseSegEnd - BaseSegProp.space* multpiler;

        CurrentMileage = BaseSegStart;

        // 创建回路节点对象
        GameObject line_A = new GameObject();
        GameObject line_B = new GameObject();

        Vector3 Offset_right = Offset;              // A路在右侧（自设）
        Vector3 Offset_left = new Vector3(Offset.x, Offset.y, -Offset.z);  // B路在左侧（自设）

        line_A.name = BaseSegProp.name + "_A";
        line_B.name = BaseSegProp.name + "_B";

        line_A.transform.parent = Parent.transform;
        line_B.transform.parent = Parent.transform;

        int count = 1;
        while (Math.Abs(CurrentMileage - BaseSegEnd) > BaseSegProp.space)
        {
            // 1. 计算设备布设位置桩号
            CurrentMileage = CurrentMileage + BaseSegProp.space * multpiler;
            Deployer.obj = Lamp;
            Deployer.mileage = CurrentMileage;

            // 2. 计算设备偏置
            Deployer.Offset = Offset;
            // 3. 设备布设
            GameObject _instance_A = Deployer.GetPosition(Deployer.obj, CurrentMileage);
            // 4. 修改设备名称
            _instance_A.name = line_A.name + "_" + count.ToString();
            // 5. 修改设备父节点（便于管理）
            _instance_A.transform.parent = line_A.transform;

            // 2. 计算设备偏置
            Deployer.Offset = Offset_left;
            // 3. 设备布设
            GameObject _instance_B = Deployer.GetPosition(Deployer.obj, CurrentMileage);
            // 调转方向

            _instance_B.transform.Rotate(0, 180, 0);

            // 4. 修改设备名称
            _instance_B.name = line_B.name + "_" + count.ToString();
            // 5. 修改设备父节点（便于管理）
            _instance_B.transform.parent = line_B.transform;

            count = count + 1;
        }
    }
}
