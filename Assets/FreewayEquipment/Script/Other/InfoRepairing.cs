using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sz.network;
using sz.device;

public class InfoRepairing : MonoBehaviour
{
    public GameObject Devices;
    [SerializeField]
    private int DevicesCount = 0;
    public Texture2D i_nolight_set, i_redcross_set, i_greenarrow_set,i_greenarrowleft_set;
    public Material m_nolight_set, m_redcross_set, m_greenarrow_set, m_greenarrowleft_set;

    public GameObject DataLoader;
    public GameObject Controller;

    public void OnRepairNullInfoInDevices()
    {
        //Assets\FreewayEquipment\Model\Textures

        for (int i = 0; i < Devices.transform.childCount; i++)
        {
            Transform tmp = Devices.transform.GetChild(i);

            if (tmp.GetComponent<DeviceTypeIdentity>().TYPE == 6)   //车检器
            {
                LaneIndicatorControl L1_script = tmp.GetChild(0).GetComponent<LaneIndicatorControl>();
                LaneIndicatorControl L2_script = tmp.GetChild(1).GetComponent<LaneIndicatorControl>();
                L1_script.meshrd = L1_script.transform.GetChild(0).GetComponent<MeshRenderer>();
                L2_script.meshrd = L2_script.transform.GetChild(0).GetComponent<MeshRenderer>();
                L1_script.i_nolight = i_nolight_set; L1_script.i_redcross = i_redcross_set; L1_script.i_greenarrow = i_greenarrow_set; L1_script.i_greenarrowleft = i_greenarrowleft_set;
                L2_script.i_nolight = i_nolight_set; L2_script.i_redcross = i_redcross_set; L2_script.i_greenarrow = i_greenarrow_set; L2_script.i_greenarrowleft = i_greenarrowleft_set;
                L1_script.m_nolight = m_nolight_set; L1_script.m_redcross = m_redcross_set; L1_script.m_greenarrow = m_greenarrow_set; L1_script.m_greenarrowleft = m_greenarrowleft_set;
                L2_script.m_nolight = m_nolight_set; L2_script.m_redcross = m_redcross_set; L2_script.m_greenarrow = m_greenarrow_set; L2_script.m_greenarrowleft = m_greenarrowleft_set;

            }
        }
    }

    public void GetDevicesCount()
    {
        DevicesCount = Devices.transform.childCount;
    }

    public void OnRepairNullInfoInController()
    {
        DeviceInfoLoaderXLS dataloader = DataLoader.GetComponent<DeviceInfoLoaderXLS>();
        EventTrigger event_trigger = Controller.GetComponent<EventTrigger>();
        for (int i = 0; i < Devices.transform.childCount; i++)
        {
            EventTrigger.CTRL_EVENT tmpEvent = new EventTrigger.CTRL_EVENT();
            tmpEvent.title = dataloader.DeviceInfoList[i].Name;
            tmpEvent.id = dataloader.DeviceInfoList[i].ID;
            tmpEvent.devicetype = dataloader.DeviceInfoList[i].Class;
            tmpEvent.controller = Devices.transform.GetChild(i).gameObject;

            event_trigger.GameEvent.Add(tmpEvent);
        }
    }

}
