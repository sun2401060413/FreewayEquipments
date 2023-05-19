using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class RadioControl : _baseCtrlCodeControl
{

    //public int CtrlCode;    // 0-停止，1，2，3循环播放第i个音频
    //public int Status;

    public AudioClip[] clips;

    private AudioSource audio_source;


    // Start is called before the first frame update
    protected override void ExtendStart()
    {
        audio_source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CtrlCode > 0 && CtrlCode != Status)
            PlayClip(CtrlCode);
        if (CtrlCode == 0)
            StopClip();
    }

    protected override int Process()
    {
        if (CtrlCode > 0)
            return PlayClip(CtrlCode);
        if (CtrlCode == 0)
        {
            StopClip();
            return 0;
        }

        return -1;
    }


    int PlayClip(int i)
    {
        if (i > clips.Length)
        {
            CtrlCode = 0;
            Status = 0;
            return Status;
        }
        audio_source.clip = clips[i - 1];
        audio_source.Play();
        // audio_source.Play(clips[i - 1]);
        Status = i;
        return Status;
    }

    void StopClip()
    {
        audio_source.Stop();
        Status = 0;
    }

}
