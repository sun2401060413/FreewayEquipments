using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class WindSpeedDirectionDetetorControl : _baseDetectorControl
{
    //[speed]12[/speed][direction]1[/direction]
    public float value_speed;
    public float value_direction;

    protected override void ExtendStart()
    {
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
            ret = string.Format("[speed]{0:G}[/speed][direction]{1:G}[/direction]", value_speed, value_direction);
            isSendAvailable = false;
            last_sent_sec = Time.timeSinceLevelLoad;
        }
        return ret;
    }

}
