using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class BrightnessDetectorControl : _baseDetectorControl
{
    //ET,2020-06-08 08:08:08,BrightnessDetect,test2,[out]洞外光强度[/out][in] 洞内光照[/in],#,ACK
    public float outside_value = -1;
    public float inside_value = -1;

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
            ret = string.Format("[out]{0:G}[/out][in]{1:G}[/in]", outside_value, inside_value);
            isSendAvailable = false;
            last_sent_sec = Time.timeSinceLevelLoad;
        }
        return ret;
    }


}
