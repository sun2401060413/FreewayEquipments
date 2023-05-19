using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class WeatherDetectControl : _baseDetectorControl
{
    [Serializable]
    public class WeatherInfo
    {
        public float windspeed;         // 0-风速
        public int winddirection;       // 1-风向
        public float temperature_atmospheric;       // 2-大气温度
        public float humidity_atmospheric;          // 3-大气湿度
        public float precipitation;     // 4-降水量
        public float visiblity;         // 5-能见度
        public float temperature_roadsurface;  // 6-路面温度
        public bool isWet;              // 7-路面潮湿
        public bool isDry;              // 8-路面干燥
        public bool isIce;              // 9-路面积雪
        public float temperature_frozen;// 10-冻结温度
        public float water_thickness;   // 11-水膜厚度
        public float saltness;          // 12-盐水浓度
        public float ice_percentage;    // 13-冰百分比
        public float snow_thickness;    // 14-积雪厚度
    }

    public WeatherInfo weather_info;

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
            ret = string.Format("[data]{0:f1},{1:D2},{2:f1},{3:f1},{4:f1},{5:G},{6:f1},{7:G},{8:G},{9:G},{10:f1},{11:G},{12:G},{13:G},{14:f1}[/data]", 
                weather_info.windspeed,                     // 0: 123.4
                weather_info.winddirection,                 // 1: 01
                weather_info.temperature_atmospheric,       // 2: 12.3
                weather_info.humidity_atmospheric,          // 3: 23.4
                weather_info.precipitation,                 // 4: 1.2
                weather_info.visiblity,                     // 5: 100
                weather_info.temperature_roadsurface,       // 6: 12.3
                MsgLangTrans(weather_info.isWet),           // 7: 1
                MsgLangTrans(weather_info.isDry),           // 8: 1
                MsgLangTrans(weather_info.isIce),           // 9: 1
                weather_info.temperature_frozen,            // 10:-12.3
                weather_info.water_thickness,               // 11:100
                weather_info.saltness,                      // 12:12
                weather_info.ice_percentage,                // 13:23
                weather_info.snow_thickness                 // 14:12
                );
            isSendAvailable = false;
            last_sent_sec = Time.timeSinceLevelLoad;
        }
        return ret;
    }

    static public string MsgLangTrans(bool v)
    {
        // 转换True/False为"是"/"否"
        // return v ? "是" : "否";
        return v ? 1.ToString() : 0.ToString();
    }

}
