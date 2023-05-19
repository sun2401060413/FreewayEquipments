using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sz.tools;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class AutoBarrierControl : _baseCtrlCodeControl
{
    public bool withCollider = false;

    private AutoBarrierCollider _collider;
    // Start is called before the first frame update
    protected override void ExtendStart()
    {
        ani = transform.GetChild(0).GetComponent<Animation>();
        CtrlCode = 1;
        Status = 1;

        _collider = CodeEditTools.GetChildTransformByName(transform, "CheckBox").GetComponent<AutoBarrierCollider>();

    }

    protected override int Process()
    {
        if (withCollider)       // 带碰撞检测
        {
            if (!_collider.hasVechicleInFront && 0 == CtrlCode)
            {
                Disable();
                return 0;
            }

            if (1 == CtrlCode)
            {
                Enable();
                return 1;
            }

            return -1;

        }
        else                    // 不带碰撞检测
        {
            switch (CtrlCode)
            {
                case 0:
                    Disable();
                    return 0;
                case 1:
                    Enable();
                    return 1;

                default:
                    return -1;
            }
        }
    }

    protected void Enable()
    {
        ani.Play("Barrier_rise");
        Status = 1;
    }

    protected void Disable()
    {
        
        ani.Play("Barrier_drop");
        Status = 0;
    }
  
}
