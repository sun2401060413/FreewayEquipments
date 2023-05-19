using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class TwoLaneIndicatorControl : MonoBehaviour
{
    public int frontCtrlCode;
    public int backCtrlCode;

    public int frontStatus;
    public int backStatus;

    private LaneIndicatorControl indicator_front;
    private LaneIndicatorControl indicator_back;

    // Start is called before the first frame update
    void Start()
    {
        indicator_front = transform.GetChild(0).GetComponent<LaneIndicatorControl>();
        indicator_back = transform.GetChild(1).GetComponent<LaneIndicatorControl>();
    }

    // Update is called once per frame
    void Update()
    {
        indicator_front.CtrlCode = frontCtrlCode;
        indicator_back.CtrlCode = backCtrlCode;

        frontStatus = indicator_front.Status;
        backStatus = indicator_back.Status;
    }
}
