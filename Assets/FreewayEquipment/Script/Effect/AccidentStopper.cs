using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccidentStopper : _baseCtrlCodeControl
{
    private AutoBarrierCollider ckcldr;
    private BoxCollider bxcldr;

    protected override void ExtendStart()
    {
        bxcldr = transform.GetComponent<BoxCollider>();
        ckcldr = transform.GetChild(0).GetComponent<AutoBarrierCollider>();

        bxcldr.enabled = false;
    }

    protected override int Process()
    {
        switch (CtrlCode)
        {
            case 0:
                return Disable();
            case 1:
                return Enable();
            default:
                return -1;
        }
    }

    private int Enable()
    {
        if (!ckcldr.hasVechicleInFront)
        {
            bxcldr.enabled = true;
            return 1;
        }
        return -1;
    }

    private int Disable()
    {
        bxcldr.enabled = false;
        return 0;
    }

}
