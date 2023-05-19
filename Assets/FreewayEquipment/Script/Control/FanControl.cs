using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class FanControl : _baseCtrlCodeControl
{
    [Header("控制码:0-停止,1-正转,2-反转")]
    [Range(0,2)]
    //public int CtrlCode;
    //public int Status;

    //private Animation ani;
    private AudioSource aud;

    // Start is called before the first frame update
    protected override void ExtendStart()
    {
        ani = gameObject.GetComponent<Animation>();
        aud = gameObject.GetComponent<AudioSource>();

        Status = 0;
    }


    protected override int Process()
    {
        switch (CtrlCode)
        {
            case 0:
                StopRotating();
                return 0;
            case 1:
                ForwardRotating();
                return 1;
            case 2:
                ReverseRotating();
                return 2;
            default:
                return -1;
        }
    }


    void ForwardRotating()
    {
        ani.Play("ForwardRotating");
        aud.Play();
        Status = 1;
    }

    void ReverseRotating()
    {
        ani.Play("ReverseRotating");
        aud.Play();
        Status = 2;
    }

    void StopRotating()
    {
        if (Status == 1)
        {
            ani.Stop("ForwardRotating");
        }
        if (Status == 2)
        {
            ani.Stop("ReverseRotating");
        }
        aud.Stop();
        Status = 0;

    }


}
