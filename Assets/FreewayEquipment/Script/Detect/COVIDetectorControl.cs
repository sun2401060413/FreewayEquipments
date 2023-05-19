using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class COVIDetectorControl : _baseDetectorControl
{
    //ET,2020-06-08 08:08:08,VehicleDedect,test2,[co]12[/co] [vi]1000[/vi],#,ACK
    public float value_CO = -1;
    public float value_VI = -1;

    protected override void ExtendStart()
    {
        // 一开始就发送
        isReadable = true;
        isSendAvailable = true;
    }

    protected override void ExtendFixedUpdate()
    {
        if (CtrlCode > 0)
        {
            current_sec = (float)Time.timeSinceLevelLoad;
            if (current_sec >= (float)(update_sec + interval_sec))
            {
                update_sec = SetStartSec();
            }
        }
        SetSendSleep();
    }

    public override string DetectorSendTask()
    {
        string ret = null;
        if (isReadable && isSendAvailable)
        {
            ret = string.Format("[co]{0:G}[/co][vi]{1:G}[/vi]", value_CO, value_VI);
            isSendAvailable = false;
            last_sent_sec = Time.timeSinceLevelLoad;
        }
        return ret;
    }

}
