using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using sz.network;

public class MultiDeviceDeploy : MonoBehaviour
{
    [Serializable]
    public class OFFSET
    {
        public string class_name;
        public Vector3 offset;

        public OFFSET(string input_name, Vector3 input_offset)
        {
            class_name = input_name;
            offset = input_offset;
        }
    }


    [Header("是否允许编辑:false-不允许,true-允许")]
    public bool isEditable = true;
    public GameObject Area_1, Area_2, Area_3;
    public GameObject Event_Trigger;
    public GameObject ObjParent = null;

    //public List<GameObject> PrefabList;
    [Header("各类设备预制体")]
    public GameObject WayControlSign;
    public GameObject TrafficSignalLamp;
    public GameObject ElectricDoor;
    public GameObject Light;
    public GameObject VMS_H;
    public GameObject VMS_G;
    public GameObject VMS_F;
    public GameObject WindMachine;
    public GameObject WarnLight;
    public GameObject AlterableSpeedRestrict;
    public GameObject BarrierMachine;

    [Header("各类设备安装偏移")]
    public Vector3 Offset_WayControlSign = Vector3.zero;
    public Vector3 Offset_TrafficSignalLamp = Vector3.zero;
    public Vector3 Offset_ElectricDoor = Vector3.zero;
    public Vector3 Offset_Light = Vector3.zero;
    public Vector3 Offset_VMS_H = Vector3.zero;
    public Vector3 Offset_VMS_G = Vector3.zero;
    public Vector3 Offset_VMS_F = Vector3.zero;
    public Vector3 Offset_WindMachine = Vector3.zero;
    public Vector3 Offset_WarnLight = Vector3.zero;
    public Vector3 Offset_AlterableSpeedRestrict = Vector3.zero;
    public Vector3 Offset_BarrierMachine = Vector3.zero;


    private GameObject instance_obj;

    private DeviceInfoLoaderXLS loader;
    private EventTrigger trigger;


    public void DevicesDeploy()
    {
        loader = transform.GetComponent<DeviceInfoLoaderXLS>();
        trigger = Event_Trigger.GetComponent<EventTrigger>();

        for (int i = 0; i < loader.DeviceInfoList.Count; i++)
        //for (int i = 0; i < 5; i++)
        {

            try
            {
                string class_name = loader.DeviceInfoList[i].Class;
                float pos = loader.DeviceInfoList[i].Position;
                string area = loader.DeviceInfoList[i].Area;
                Debug.Log(class_name + ";" + pos.ToString() + ";" + area+";"+ loader.DeviceInfoList[i].Name);
                switch (area)
                {
                    case "1":
                        Area_1.GetComponent<SingleDeviceDeploy>().Offset = GetOffset(class_name);
                        instance_obj = Area_1.GetComponent<SingleDeviceDeploy>().GetPosition(GetPrefab(class_name), pos);
                        break;
                    case "2":
                        Area_2.GetComponent<SingleDeviceDeploy>().Offset = GetOffset(class_name);
                        instance_obj = Area_2.GetComponent<SingleDeviceDeploy>().GetPosition(GetPrefab(class_name), pos);
                        break;
                    case "3":
                        Area_3.GetComponent<SingleDeviceDeploy>().Offset = GetOffset(class_name);
                        instance_obj = Area_3.GetComponent<SingleDeviceDeploy>().GetPosition(GetPrefab(class_name), pos);
                        break;
                    default:
                        break;
                }


                instance_obj.name = loader.DeviceInfoList[i].Name;
                instance_obj.transform.parent = ObjParent.transform;

                EventTrigger.CTRL_EVENT ctrl_event = new EventTrigger.CTRL_EVENT();
                ctrl_event.title = loader.DeviceInfoList[i].Name;
                ctrl_event.devicetype = loader.DeviceInfoList[i].Class;
                ctrl_event.id = loader.DeviceInfoList[i].ID;
                ctrl_event.controller = instance_obj;

                if (isEditable)
                {
                    trigger.GameEvent.Add(ctrl_event);
                }
                
            }
            catch (Exception)
            {

                throw;
            }

        }

    }

    public GameObject GetPrefab(string class_name)
    {

        switch (class_name)
        {
            case "WayControlSign":
                return WayControlSign;
            case "TrafficSignalLamp":
                return TrafficSignalLamp;
            case "ElectricDoor":
                return ElectricDoor;
            case "Light":
                return Light;
            case "VMS_H":
                return VMS_H;
            case "VMS_G":
                return VMS_G;
            case "VMS_F":
                return VMS_F;
            case "WindMachine":
                return WindMachine;
            case "WarnLight":
                return WarnLight;
            case "AlterableSpeedRestrict":
                return AlterableSpeedRestrict;
            case "BarrierMachine":
                return BarrierMachine;
            default:
                return null;
        }

    }


    /// <summary>
    /// 根据设备类型返回设备偏移向量;
    /// </summary>
    /// <param name="class_name">设备类型</param>
    /// <returns></returns>
    public Vector3 GetOffset(string class_name)
    {
        switch (class_name)
        {
            case "WayControlSign":
                return Offset_WayControlSign;
            case "TrafficSignalLamp":
                return Offset_TrafficSignalLamp;
            case "ElectricDoor":
                return Offset_ElectricDoor;
            case "Light":
                return Offset_Light;
            case "VMS_H":
                return Offset_VMS_H;
            case "VMS_G":
                return Offset_VMS_G;
            case "VMS_F":
                return Offset_VMS_F;
            case "WindMachine":
                return Offset_WindMachine;
            case "WarnLight":
                return Offset_WarnLight;
            case "AlterableSpeedRestrict":
                return Offset_AlterableSpeedRestrict;
            case "BarrierMachine":
                return Offset_BarrierMachine;
            default:
                return Vector3.zero;
        }
    }



}
