using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class VehicleDetectorMLControl : _baseDetectorControl
{
    [Serializable]
    public class DETECTOR
    {
        public int id;
        public GameObject detector;
    }

    public bool isGroupReadable = false; // 车检器组可读(由于车检器周期性统计变量,标明数据是否准备好);
    
    public List<DETECTOR> detector_list = new List<DETECTOR>();

    protected override int Process()
    {
        if (CtrlCode == 0 || CtrlCode == 1)
        {
            foreach (var elem in detector_list)
                elem.detector.GetComponent<VehicleDetectorControl>().CtrlCode = CtrlCode;
            Status = CtrlCode;
        }
        return Status;
    }

    protected override void ExtendStart()
    {
        foreach (DETECTOR elem in detector_list)
        {
            VehicleDetectorControl tmpScript = elem.detector.GetComponent<VehicleDetectorControl>();
            tmpScript.interval_sec = interval_sec;
            tmpScript.resent_interval = resent_interval;
        }
    }

    protected override void ExtendUpdate()
    {
        //Debug.Log(Time.timeSinceLevelLoad.ToString() + ":" + GetReadableStatus());
        if (GetReadableStatus())        // 可读;
        {
            //Debug.Log(Time.timeSinceLevelLoad.ToString + ":" + GetReadableStatus());
            isGroupReadable = true;

            SetReadableStatus();         // 读取状态后复位，以便知道下一次统计;
        }

        SetSendSleep();                 // 未接受到反馈时，判断是否可以再次发送
    }

    //ET,2020-06-08 08:08:08,VehicleDedect,test2,
    //[data]时间(yyyy-mm-dd hh:mm),时间序号(int),车道号(int),
    //小车流量(int),中型车流量(int),大型车流量(int),特大型车流量(int),
    //小车车速(float),中型车车速(float),大型车速(float),特大型车速(float),
    //时间占有率(float)...[/data],#,ACK
    //protected override string GetDetectorDataStr()
    protected string GetDetectorDataStr()
    {
        // 根据id对detector_list排序
        detector_list.Sort((a, b) => a.id.CompareTo(b.id));
        string res = null;
        if (detector_list.Count > 0)
        {
            VehicleDetectorControl tmpobjScript = detector_list[0].detector.GetComponent<VehicleDetectorControl>();
            res = tmpobjScript.update_time_str + "," + tmpobjScript.time_id.ToString() + ",";
            for (int i = 0; i < detector_list.Count; i++)
            {
                VehicleDetectorControl objScript = detector_list[i].detector.GetComponent<VehicleDetectorControl>();
                if (objScript.Status > 0)
                {
                    string tmp = detector_list[i].id.ToString() + "," +
                        objScript.saved_count_small.ToString() + "," +
                        objScript.saved_count_medium.ToString() + "," +
                        objScript.saved_count_large.ToString() + "," +
                        objScript.saved_count_giant.ToString() + "," +
                        objScript.saved_speed_samll.ToString() + "," +
                        objScript.saved_speed_medium.ToString() + "," +
                        objScript.saved_speed_large.ToString() + "," +
                        objScript.saved_speed_giant.ToString() + "," +
                        objScript.saved_timeOccupancy.ToString() + ",";
                    res = res + tmp;
                }
            }
            if (res.Length > 0)
                res = res.Substring(0, res.Length - 1);
        }

        return res;
    }

    public bool GetReadableStatus()
    {
        // 车检器组中全部车检器均处于可读状态时，设置为组可读状态;
        bool res = true;
        for (int i = 0; i < detector_list.Count; i++)
        {
            VehicleDetectorControl objScript = detector_list[i].detector.GetComponent<VehicleDetectorControl>();
            if (objScript.Status > 0)
            {
                res = res & objScript.isReadable;
            }
            //else
            //{
            //    res = false;
            //}
        }
        return res;
    }

    public void SetReadableStatus(bool value = false)
    {
        // 设置单个车道车检器的可读状态
        for (int i = 0; i < detector_list.Count; i++)
        {
            VehicleDetectorControl objScript = detector_list[i].detector.GetComponent<VehicleDetectorControl>();
            objScript.isReadable = value;
        }
    }

    public override string DetectorSendTask()
    {
        string ret = null;
        if (isGroupReadable && isSendAvailable)   // 可发、未成功、允许发;
        {
            ret = "[data]" + GetDetectorDataStr() + "[/data]";
            isSendAvailable = false;
            last_sent_sec = Time.timeSinceLevelLoad;
        }
        return ret;
    }

}
