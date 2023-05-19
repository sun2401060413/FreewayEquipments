using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sz.device
{
    /// <summary>
    /// 设备类型编码，添加新设备时，需维护这个表（1/4）,[NEXT添加:DeviceTypeDictionary]
    /// </summary>
    enum DEVICE_TYPE
    {
        unknown = 0,

        TrafficSignalLamp = 1,          // 交通信号灯
        WarnLight = 2,                  // 声光报警
        BarrierMachine = 3,             // 栏杆机
        VMS = 4,                        // 情报版
        WindMachine = 5,                // 风机
        WayControlSign = 6,             // 车道控制器
        Light = 7,                      // 照明灯具(单个)
        SpinAlarm = 8,                  // 旋转报警器
        ElectricDoor = 9,               // 电动门
        AlterableSpeedRestrict = 10,    // 可变限速标志
        VehicleDedect = 11,             // 车辆检测器
        
        /// TODO
        COVI = 12,                      // COVI
        BrightnessDetect = 13,          // 亮度检测
        WeatherDetect = 14,             // 天气检测
        WindSpeedDirectionDetect = 15,  // 风速风向
        FireDetect = 16,                // 火灾报警
        EventDetector = 17,             // 事件检测
        NoxDetector = 18,               // 氮氧化物检测
    }

    /// <summary>
    /// 设备类型名称字典，添加新设备时，需要维护这个表（2/4）[NEXT添加:GetDeviceTypeCode()]
    /// 不同设备类型可能对应相同的名称
    /// </summary>
    public class DeviceTypeDictionary
    {
        static public Dictionary<int, string> deviceStr = new Dictionary<int, string> {
            { (int)DEVICE_TYPE.unknown,"Unknonwn" },
            { (int)DEVICE_TYPE.TrafficSignalLamp, "TrafficSignalLamp"},
            { (int)DEVICE_TYPE.WarnLight, "WarnLight"},
            { (int)DEVICE_TYPE.BarrierMachine, "BarrierMachine"},
            { (int)DEVICE_TYPE.VMS, "VMS"},
            { (int)DEVICE_TYPE.WindMachine, "WindMachine"},
            { (int)DEVICE_TYPE.WayControlSign, "WayControlSign"},
            { (int)DEVICE_TYPE.Light, "Light"},
            { (int)DEVICE_TYPE.SpinAlarm, "SpinAlarm"},
            { (int)DEVICE_TYPE.ElectricDoor, "ElectricDoor"},
            { (int)DEVICE_TYPE.AlterableSpeedRestrict, "AlterableSpeedRestrict"},

            { (int)DEVICE_TYPE.VehicleDedect, "VehicleDedect"},
            { (int)DEVICE_TYPE.COVI, "COVI"},
            { (int)DEVICE_TYPE.BrightnessDetect, "BrightnessDetect"},
            { (int)DEVICE_TYPE.WeatherDetect, "WeatherDetect"},
            { (int)DEVICE_TYPE.WindSpeedDirectionDetect, "WindSpeedDirectionDetect"},
            { (int)DEVICE_TYPE.FireDetect, "FireDetect"},
            { (int)DEVICE_TYPE.EventDetector, "EventDetector"},
            { (int)DEVICE_TYPE.NoxDetector, "NoxDetector"},
        };

        static public string GetDeviceTypeStr(int DeviceTypeCode)
        {
            return deviceStr[DeviceTypeCode];
        }
    }


    public class DeviceTypeIdentity : MonoBehaviour
    {
        [SerializeField]//起作用
        private int _type;
        [SerializeField]//起作用
        private int _id;

        [HideInInspector]
        public int TYPE
        {
            get { return _type; }
            set { _type = value; }
        }
        [HideInInspector]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private void Awake()
        {
            GetDeviceTypeCode();
        }


        /// <summary>
        /// 获取所在对象的设备类型
        /// </summary>
        /// <returns>输出控制脚本对象</returns>
        public int GetDeviceTypeCode()
        {

            // 交通信号灯:1
            if (gameObject.TryGetComponent<TrafficLightControl>(out TrafficLightControl o1))
            {
                _type = (int)DEVICE_TYPE.TrafficSignalLamp;
                return _type;
            }

            // 声光报警器:2
            if (gameObject.TryGetComponent<LightAlarmControl>(out LightAlarmControl o2))
            {
                _type = (int)DEVICE_TYPE.WarnLight;
                return _type;
            }

            // 自动栏杆机:3
            if (gameObject.TryGetComponent<AutoBarrierControl>(out AutoBarrierControl o3))
            {
                _type = (int)DEVICE_TYPE.BarrierMachine;
                return _type;
            }

            // 可变情报板:4
            if (gameObject.TryGetComponent<VMSRemoteControl>(out VMSRemoteControl o4))
            {
                _type = (int)DEVICE_TYPE.VMS;
                return _type;
            }

            // 风机:5
            if (gameObject.TryGetComponent<FanControl>(out FanControl o5))
            {
                _type = (int)DEVICE_TYPE.WindMachine;
                return _type;
            }

            // 车道指示器:6
            //if (gameObject.TryGetComponent<LaneIndicatorControl>(out LaneIndicatorControl o6))
            if (gameObject.TryGetComponent<TwoLaneIndicatorControl>(out TwoLaneIndicatorControl o6))
            {
                _type = (int)DEVICE_TYPE.WayControlSign;
                return _type;
            }

            // 照明灯具:7
            if (gameObject.TryGetComponent<LampControl>(out LampControl o7))
            {
                _type = (int)DEVICE_TYPE.Light;
                return _type;
            }

            // 旋转报警装置:8
            if (gameObject.TryGetComponent<SpinAlarmControl>(out SpinAlarmControl o8))
            {
                _type = (int)DEVICE_TYPE.SpinAlarm;
                return _type;
            }

            // 卷帘门:9
            if (gameObject.TryGetComponent<BasicDoor>(out BasicDoor o9))
            {
                _type = (int)DEVICE_TYPE.ElectricDoor;
                return _type;
            }

            // 限速标志:10
            if (gameObject.TryGetComponent<VSLSControl>(out VSLSControl o10))
            {
                _type = (int)DEVICE_TYPE.AlterableSpeedRestrict;
                return _type;
            }

            // 车检器:11
            if (gameObject.TryGetComponent<VehicleDetectorMLControl>(out VehicleDetectorMLControl o11))
            {
                _type = (int)DEVICE_TYPE.VehicleDedect;
                return _type;
            }

            /// ****** 新增设备时，需要在此处添加类型判断逻辑（3/4）[NEXT添加:sz.network.EventTrigger:EventProcess()] ******
            // COVI:12
            if (gameObject.TryGetComponent<COVIDetectorControl>(out COVIDetectorControl o12))
            {
                _type = (int)DEVICE_TYPE.COVI;
                return _type;
            }

            // 亮检:13
            if (gameObject.TryGetComponent<BrightnessDetectorControl>(out BrightnessDetectorControl o13))
            {
                _type = (int)DEVICE_TYPE.BrightnessDetect;
                return _type;
            }
            // 天气:14
            if (gameObject.TryGetComponent<WeatherDetectControl>(out WeatherDetectControl o14))
            {
                _type = (int)DEVICE_TYPE.WeatherDetect;
                return _type;
            }
            // 风速风向:15 WindSpeedDirectionDetect
            if (gameObject.TryGetComponent<WindSpeedDirectionDetetorControl>(out WindSpeedDirectionDetetorControl o15))
            {
                _type = (int)DEVICE_TYPE.WindSpeedDirectionDetect;
                return _type;
            }


            _type = (int)DEVICE_TYPE.unknown;
            return _type;   // 未找到指定设备类型
        }

        /// <summary>
        /// 获取不同脚本类所对应设备类型名称（不同设备脚本类可能对应相同的设备类型名称）
        /// </summary>
        /// <param name="type_code">设备脚本类编码</param>
        /// <returns>脚本类对应设备类型名</returns>
        static public string GetDeviceTypeStr(int type_code)
        {
            if (DeviceTypeDictionary.deviceStr.ContainsKey(type_code))
                return DeviceTypeDictionary.GetDeviceTypeStr(type_code);
            else
                return null;
        }
    }

}

