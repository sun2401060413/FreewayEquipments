using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
public class CarControlUsingLogitech : MonoBehaviour
{
    public GameObject CtrlCar;
    private CarUserControl CtrlScript;
    private int logitect_x;
    private int logitect_y;
    private int logitect_z;

    public float vol;
    private int gears = 1;

    private float tmp = 0;

    private Rigidbody rb;

    private bool stop_status = false;
    //public float res;
    // Start is called before the first frame update
    void Start()
    {
        CtrlScript = CtrlCar.GetComponent<CarUserControl>();
        rb = gameObject.GetComponent<Rigidbody>();
        LogitechGSDK.LogiSteeringInitialize(false);
        logitect_x = 0;
        logitect_y = 0;
        logitect_z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Input();
        Output();
        
    }
    void OnApplicationQuit()
    {
        LogitechGSDK.LogiSteeringShutdown();
    }
    void Input()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);
           //TODO 这里就可以获取 想要的方向盘数据了
           logitect_x = rec.lX;
           logitect_y = rec.lY;
           logitect_z = rec.lRz;

            if(rec.rgbButtons[15] == 128)
            {
                gears = -1;
            }
            else
            {
                gears = 1;
            }
        } 
    }

    void Output()
    {
        CtrlScript.h = SteeringWheelTrans(logitect_x);

        //tmp = rb.velocity.sqrMagnitude>0.5?HandbrakeTrans(logitect_z):0;
        //CtrlScript.v = (ThrottleTrans(logitect_y)-tmp)*gears;
        //vol = rb.velocity.sqrMagnitude;
        stop_status = rb.velocity.sqrMagnitude < 0.1; 
        float tmp_v = (ThrottleTrans(logitect_y) - HandbrakeTrans(logitect_z)) * gears;
        if (gears >0 )
            CtrlScript.v = stop_status & tmp_v < 0 ? 0 : tmp_v;
        else
            CtrlScript.v = stop_status & tmp_v > 0 ? 0 : tmp_v;
        //CtrlScript.v = HandbrakeTrans(logitect_z)*gears*-1;

    }

    float SteeringWheelTrans(int value, int tresh = 32000)
    {
        int tmp = Mathf.Abs(value)>tresh?sign(value)*tresh:value;
        return (float)tmp/(float)tresh;
    }

    float ThrottleTrans(int value, int tresh = 32767)
    {
        return (float)(-1*value+tresh)/(float)(2*tresh);
    }

    float HandbrakeTrans(int value, int tresh = 32767)
    {
        return (float)(-1*value+tresh)/(float)(2*tresh);
    }

    int sign(int value)
    {
        if(value>0)
            return 1;
        else
            return -1;
    }
}
