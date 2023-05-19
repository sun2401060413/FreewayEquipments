using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using sz.device;

//// json库
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;


namespace sz.network
{
    /// <summary>
    /// 消息类型，ET:控制消息;HT:心跳消息
    /// </summary>
    enum MSG_TYPE
    {
        ET = 1,     // 控制消息
        HT = 2,     // 心跳消息
    }


    /// <summary>
    /// 事件错误码
    /// </summary>
    enum ERR_CODE_EVENT
    {
        NO_ERROR = 0,
        NO_DEVICE_ID = 1,
        NO_DEVICE_TYPE = 2,
    }

    public class EventTrigger : MonoBehaviour
    {


        [Serializable]
        public class CTRL_EVENT     // 关联指令和控制器
        {
            public string title;
            public string devicetype;
            public string id;
            public GameObject controller;
        }


        public GameObject socket_serv;
        //public CTRL_EVENT[] GameEvent;
        public List<CTRL_EVENT> GameEvent = new List<CTRL_EVENT>();

        private ServSocket component_socket_serv;

        // 设备ID与控制事件字典
        private Dictionary<string, int> _devicelist = new Dictionary<string, int>();
        // 检测器设备字典
        private Dictionary<string, int> _detectorlist = new Dictionary<string, int>();  //string:设备编号，int:设备索引

        private string out_status;


        private void Awake()
        {
            GetDeviceDict();
        }

        // Start is called before the first frame update
        void Start()
        {
            component_socket_serv = socket_serv.GetComponent<ServSocket>();
        }

        // Update is called once per frame
        void Update()
        {
            // 只有一个队列
            string recvstr = component_socket_serv.GetRecvStr();
            // KeyValuePair<string, string> recvs = component_socket_serv.GetRecvStr();
            // 
            //if (recvs.Value != null)
            if (recvstr != null)
            {
                //string[] cmd = MsgProcess.GetSplitMsg(recvs.Value);
                //string[] cmd = MsgProcess.GetSplitMsg(recvstr);
                List<string[]> msglist = MsgProcess.GetSplitMsg(recvstr);
                for (int i = 0; i < msglist.Count; i++)
                {
                    string[] cmd = msglist[i];
                    int msgtype = MsgProcess.GetMsgType(cmd);
                    if ((int)MSG_TYPE.ET == msgtype)       // ET: 指令消息
                    {
                        //Debug.Log(recvstr);
                        string status = cmd[4];
                        string ret_status;
                        //Debug.Log("recvs.key:" + recvs.Key + ",recvs.value:" + recvs.Value);
                        int err = EventProcess(cmd, out ret_status);
                        //component_socket_serv.SocketSend(MsgProcess.GetETResponse(cmd, status, err), recvs.Key);
                        string retstr = MsgProcess.GetETResponse(cmd, status, err);
                        if(retstr != null)
                            component_socket_serv.SocketSend(retstr);
                        // Debug.Log("status:" + status + ",err:" + err);
                    }

                    if ((int)MSG_TYPE.HT == msgtype)       // HT：心跳消息
                    {
                        // component_socket_serv.SocketSend(MsgProcess.GetHTResponse(), recvs.Key);
                        component_socket_serv.SocketSend(MsgProcess.GetHTResponse());
                    }
                }
            }

            foreach (var elem in _detectorlist)
            {
                GameObject tarObj = GameEvent[_detectorlist[elem.Key]].controller;
                string device_id = GameEvent[_detectorlist[elem.Key]].id;
                string resStr = SendTaskProcess(device_id, tarObj);
                if (resStr != null && component_socket_serv.clientSocket != null)
                    component_socket_serv.SocketSend(resStr);
                //if (tarObj)
            }
        }


        /// <summary>
        /// 设备注册
        /// 建立设备编号与事件对象的关联
        /// </summary>
        void GetDeviceDict()
        {
            _devicelist.Clear();
            _detectorlist.Clear();

            for (int i = 0; i < GameEvent.Count; i++)
            {
                _devicelist.Add(GameEvent[i].id, i);
                // 如果是个检测器
                // )
                if (MsgProcess.isDetector(DeviceTypeIdentity.GetDeviceTypeStr(GameEvent[i].controller.GetComponent<DeviceTypeIdentity>().TYPE)))
                {
                    _detectorlist.Add(GameEvent[i].id, i);
                }
            }
        }


        public string SendTaskProcess(string device_id, GameObject tarObj)
        {
            DeviceTypeIdentity dtype = tarObj.GetComponent<DeviceTypeIdentity>();
            
            switch (dtype.TYPE)
            {
                case (int)DEVICE_TYPE.VehicleDedect:
                    VehicleDetectorMLControl send_11 = tarObj.GetComponent<VehicleDetectorMLControl>();
                    return MsgProcess.GetETResponse(dtype.TYPE, device_id, send_11.DetectorSendTask());
                case (int)DEVICE_TYPE.COVI:
                    COVIDetectorControl send_12 = tarObj.GetComponent<COVIDetectorControl>();
                    return MsgProcess.GetETResponse(dtype.TYPE, device_id, send_12.DetectorSendTask());
                case (int)DEVICE_TYPE.BrightnessDetect:
                    BrightnessDetectorControl send_13 = tarObj.GetComponent<BrightnessDetectorControl>();
                    return MsgProcess.GetETResponse(dtype.TYPE, device_id, send_13.DetectorSendTask());
                case (int)DEVICE_TYPE.WeatherDetect:
                    WeatherDetectControl send_14 = tarObj.GetComponent<WeatherDetectControl>();
                    return MsgProcess.GetETResponse(dtype.TYPE, device_id, send_14.DetectorSendTask());
                case (int)DEVICE_TYPE.WindSpeedDirectionDetect:
                    WindSpeedDirectionDetetorControl send_15 = tarObj.GetComponent<WindSpeedDirectionDetetorControl>();
                    return MsgProcess.GetETResponse(dtype.TYPE, device_id, send_15.DetectorSendTask());
                default:
                    break;
            }
            return null ;
        }

        /// <summary>
        /// 单个设备控制
        /// </summary>
        /// <param name="cmd">控制指令</param>
        /// <param name="status">反馈状态</param>
        /// <returns></returns>
        public int EventProcess(string[] cmd, out string status)
        {
            status = null;
            int idx = -1;

            if (!_devicelist.ContainsKey(cmd[3]))
                return (int)ERR_CODE_EVENT.NO_DEVICE_ID;
            else
                idx = _devicelist[cmd[3]];

            //Debug.Log("正在处理:" + idx + "号设备！");


            // ***** 新增设备类型，需要在下面添加处理逻辑 （4/4）******
            if (MsgProcess.isVMSDevice(cmd))
            {
                status = MsgProcess.GetMergedVMSMessage(cmd);
                VMSRemoteControl ctrl = GameEvent[idx].controller.GetComponent<VMSRemoteControl>();
                ctrl.msgs = cmd;
                ctrl.input_status = true;
                return (int)ERR_CODE_EVENT.NO_ERROR;
            }

            else if (MsgProcess.isDetector(cmd))    // 判断是否为检测器类
            {
                //Debug.Log("isDetecotr:" + cmd[2]);
                DeviceTypeIdentity dtype = GameEvent[idx].controller.GetComponent<DeviceTypeIdentity>();
                int type_code = dtype.TYPE;
                switch (type_code)
                {
                    case (int)DEVICE_TYPE.VehicleDedect:
                        VehicleDetectorMLControl ctrl_11 = GameEvent[idx].controller.GetComponent<VehicleDetectorMLControl>();
                        // 判断和设置发送状态（如果接收不到就一直发送）
                        ctrl_11.isGroupReadable = false;        // 收到反馈，标识发送成功;
                        return (int)ERR_CODE_EVENT.NO_ERROR;
                    case (int)DEVICE_TYPE.COVI:
                        COVIDetectorControl ctrl_12 = GameEvent[idx].controller.GetComponent<COVIDetectorControl>();
                        ctrl_12.isReadable = false;
                        return (int)ERR_CODE_EVENT.NO_ERROR;
                    case (int)DEVICE_TYPE.BrightnessDetect:
                        BrightnessDetectorControl ctrl_13 = GameEvent[idx].controller.GetComponent<BrightnessDetectorControl>();
                        ctrl_13.isReadable = false;
                        return (int)ERR_CODE_EVENT.NO_ERROR;
                    case (int)DEVICE_TYPE.WeatherDetect:
                        WeatherDetectControl ctrl_14 = GameEvent[idx].controller.GetComponent<WeatherDetectControl>();
                        ctrl_14.isReadable = false;
                        return (int)ERR_CODE_EVENT.NO_ERROR;
                    case (int)DEVICE_TYPE.WindSpeedDirectionDetect:
                        WindSpeedDirectionDetetorControl ctrl_15 = GameEvent[idx].controller.GetComponent<WindSpeedDirectionDetetorControl>();
                        ctrl_15.isReadable = false;
                        return (int)ERR_CODE_EVENT.NO_ERROR;
                    default:
                        break;
                }
                return (int)ERR_CODE_EVENT.NO_DEVICE_TYPE;   // 找不到设备类型
            }

            else
            {
                DeviceTypeIdentity dtype = GameEvent[idx].controller.GetComponent<DeviceTypeIdentity>();
                int type_code = dtype.TYPE;
                switch (type_code)
                {
                    case (int)DEVICE_TYPE.TrafficSignalLamp:
                        return SetCtrlCode<TrafficLightControl>(idx, cmd, out status);

                    case (int)DEVICE_TYPE.WarnLight:
                        return SetCtrlCode<LightAlarmControl>(idx, cmd, out status);

                    case (int)DEVICE_TYPE.BarrierMachine:
                        return SetCtrlCode<AutoBarrierControl>(idx, cmd, out status);

                    case (int)DEVICE_TYPE.WindMachine:
                        return SetCtrlCode<FanControl>(idx, cmd, out status);

                    case (int)DEVICE_TYPE.WayControlSign:
                        //LaneIndicatorControl ctrl_6 = GameEvent[idx].controller.GetComponent<LaneIndicatorControl>();
                        TwoLaneIndicatorControl ctrl_6 = GameEvent[idx].controller.GetComponent<TwoLaneIndicatorControl>();
                        string[] msgs = MsgProcess.GetSplitLIMessage(cmd[4]);
                        int.TryParse(msgs[0], out ctrl_6.frontCtrlCode);
                        int.TryParse(msgs[1], out ctrl_6.backCtrlCode);
                        status = cmd[4];
                        return (int)ERR_CODE_EVENT.NO_ERROR;

                    case (int)DEVICE_TYPE.Light:
                        return SetCtrlCode<LampControl>(idx, cmd, out status);

                    case (int)DEVICE_TYPE.SpinAlarm:
                        return SetCtrlCode<SpinAlarmControl>(idx, cmd, out status);

                    case (int)DEVICE_TYPE.ElectricDoor:
                        return SetCtrlCode<BasicDoor>(idx, cmd, out status);

                    case (int)DEVICE_TYPE.AlterableSpeedRestrict:
                        return SetCtrlCode<VSLSControl>(idx, cmd, out status);

                    default:
                        break;
                }

                return (int)ERR_CODE_EVENT.NO_DEVICE_TYPE;   // 找不到设备类型
            }
        }


        public int SetCtrlCode<T>(int idx, string[] cmd, out string status) where T : _baseCtrlCodeControl
        {
            DeviceTypeIdentity dtype = GameEvent[idx].controller.GetComponent<DeviceTypeIdentity>();
            T contrllor = GameEvent[idx].controller.GetComponent<T>();
            int.TryParse(cmd[4], out contrllor.CtrlCode);
            status = cmd[4];    // TODO: 程序执行结果返回.
            return (int)ERR_CODE_EVENT.NO_ERROR;
        }
    }

}

