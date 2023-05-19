using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using sz.device;
using System.Text.RegularExpressions;

namespace sz.network
{

    public class MsgProcess
    {
        ///// <summary>
        ///// 接收的指令消息
        ///// </summary>
        //public struct MSG_INFO_RECV
        //{
        //    string HEADER;      // 报文头
        //    string TIME;        // 时间戳
        //    string DEVICE;      // 设备类型
        //    string ID;          // 设备ID
        //    string DATA;        // 控制指令
        //    string END;         // 报文尾
        //}

        ///// <summary>
        ///// 发送的反馈消息
        ///// </summary>
        //public struct MSG_INFO_SEND
        //{
        //    string HEADER;      // 报文头
        //    string TIME;        // 时间戳
        //    string DEVICE;      // 设备类型
        //    string ID;          // 设备ID
        //    string STATUS;      // 设备状态
        //    string END;         // 报文尾
        //    string TAG;         // 反馈标识
        //    string ERR;         // 错误标识

        //}

        ///// <summary>
        ///// 接收的心跳包消息
        ///// </summary>
        //public struct MSG_HEART_RECV
        //{
        //    string HEADER;      // 起始符
        //    string TIME;        // 时间戳
        //    string END;         // 报文尾
        //}

        ///// <summary>
        ///// 发送的心跳包消息
        ///// </summary>
        //public struct MSG_HEART_SEND
        //{
        //    string HEADER;      // 起始符
        //    string TIME;        // 时间戳
        //    string END;         // 报文尾
        //    string TAG;         // 反馈标识
        //}


        // 情报板显示文字报文解析
        // 示例:[info]谨慎驾驶\n减速慢行[/info][stayTime]2[/stayTime][color]red[/color][fontFamilty]宋体[/fontFamilty]
        [Serializable]
        public class MSG_VMS
        {
            private string info;
            private int stayTime;
            private string color;
            private string fontFamilty;

            public MSG_VMS()        // XmlSerializer序列化要求一定要有无参数构造函数 
            {
                info = "";
                stayTime = 0;
                color = "";
                fontFamilty = "";
            }

            public MSG_VMS(string _info, int _stayTime, string _color, string _fontFamilty)
            {
                info = _info;
                stayTime = _stayTime;
                color = _color;
                fontFamilty = _fontFamilty;
            }

            public string INFO
            {
                get { return info; }
                set { info = value; }
            }

            public int HOLD
            {
                get { return stayTime; }
                set { stayTime = value; }
            }

            public string COLOR
            {
                get { return color; }
                set { color = value; }
            }

            public string FONT
            {
                get { return fontFamilty; }
                set { fontFamilty = value; }
            }

        }


        // 报文解析

        /// <summary>
        /// 报文内容解析
        /// </summary>
        /// <param name="Msg">待分解报文</param>
        /// <returns>解析词表</returns>
        static public List<string[]> GetSplitMsg(string Msg)
        {
            // 按照目前的协议，发送的数据包括以'#'和以'#,ACK'结尾两种形式;
            Msg = Msg.Replace("#,ACK", "#");            // 当前不纠结ACK，因此先直接替换;  
            string[] msglist = Regex.Split(Msg, ",#", RegexOptions.IgnoreCase);//存在问题，数据中包含颜色时，会提前结束
            // string[] msglist = Msg.Split(new char[] { ',#' });/
            List<string[]> splitmsglist = new List<string[]>();
            for (int i = 0; i < msglist.Length; i++)
            {
                if (msglist[i].Length == 0)
                {
                    break;
                }
                else
                {
                    string tmp = msglist[i] + ",#"; 
                    string[] src_split = tmp.Split(new char[] { ',' });
                    //ET,2020-06-08 08:08:08,VehicleDedect,test2,[data]时间,时间序号[/data],#
                    // 0,          1        ,       2     ,  3  ,    4     ,      5        ,6
                    string split_data = "";
                    for (int j = 4; j < src_split.Length - 1; j++)
                        split_data = split_data + src_split[j] + ",";       //中间数据组合
                    split_data = split_data.Substring(0, split_data.Length-1); //删除末尾逗号;

                    string[] new_split = new string[] { src_split[0], src_split[1], src_split[2], src_split[3], split_data, src_split[src_split.Length - 1] };
                    splitmsglist.Add(new_split);
                }
            }
            
            return splitmsglist;
        }

        /// <summary>
        /// 词表合并为报文
        /// </summary>
        /// <param name="wordlist">待合并词表</param>
        /// <returns>合并报文</returns>
        static public string GetMergeMsg(string[] wordlist)
        {
            string ret = "";
            foreach (string s in wordlist)
                ret = ret + s + ',';
            return ret;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        static public string GetTimestamp()
        {
            // 时间格式：2020-06-08 08:08:08,即yyyy-MM-dd HH:mm:ss
            string ret = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
            return ret;
        }

        /// <summary>
        /// 判断消息类型
        /// </summary>
        /// <param name="wordlist">解析词表</param>
        /// <returns>消息类型编码：1-指令;2-心跳;0-未定义</returns>
        static public int GetMsgType(string[] wordlist)
        {
            switch (wordlist[0])
            {
                case "ET":              // 指令消息
                    return 1;
                case "HT":              // 心跳消息
                    return 2;
                default:                // 其他
                    return 0;
            }
            // 对于没有while的场景，return和break一样;
        }

        /// <summary>
        /// 生成指令反馈报文
        /// </summary>
        /// <param name="ret_wordllist">反馈消息词表</param>
        /// <param name="status">反馈状态数据</param>
        /// <param name="error">错误标识,0-无错误(默认)，1-设备编码错误，2-设备类型错误</param>
        /// <returns></returns>
        static public string GetETResponse(string[] ret_wordllist, string status = "0", int error = 0)
        {
            if (isDetector(ret_wordllist))
            {
                return null;
            }
            else
            {
                ret_wordllist[1] = GetTimestamp();          // 填写时间戳
                ret_wordllist[4] = status;       // 填写反馈状态

                //if (ret_wordllist[4].Length == 0)
                //    ret_wordllist[4] = "0";
                string ret = GetMergeMsg(ret_wordllist);     // 生成基础报文

                Debug.Log(ret);

                // 添加错误提示
                if (error == 1)
                    ret += "找不到设备,";                    // 未识别的设备编码
                if (error == 2)
                    ret += "类型码错误,";                    // 未识别的设备类型

                // 报文添加反馈标识
                ret += "ACK";
                return ret;
            }

        }

        //ET,2020-06-08 08:08:08,VehicleDedect,test2,[data]时间,时间序号[/data],#,ACK
        static public string GetETResponse(int typeCode, string device_id, string res_data)
        {
            if (res_data != null)
            {
                string typeStr = DeviceTypeIdentity.GetDeviceTypeStr(typeCode);     // 获取设备类型名称
                string res = "";
                res = "ET," +
                    GetTimestamp() + "," +
                    typeStr + "," +
                    device_id + "," +
                    res_data + "," +
                    "#,ACK";
                return res;
            }
            else
                return null;    // 数据空时不用生成报文，没用

        }


        /// <summary>
        /// 生成心跳报文
        /// </summary>
        /// <returns>心跳报文</returns>
        static public string GetHTResponse()
        {
            string ret = "HT," + GetTimestamp() + ",#,ACK";
            return ret;
        }


        //// 情报板显示文字报文解析
        //// 示例:[info]谨慎驾驶\n减速慢行[/info][stayTime]2[/stayTime][color]red[/color][fontFamilty]宋体[/fontFamilty]
        //[info]谨慎驾驶减速慢行[/info][stayTime]2[/stayTime][color]yellow[/color][fontFamilty]宋体[/fontFamilty]
        //[info]你是哈哈哈[/info][stayTime]2[/stayTime][color]#00ff00[/color][fontFamilty]宋体[/fontFamilty]

        /// <summary>
        /// 整合情报板内容信息，不应因为信息内容有逗号就分开
        /// </summary>
        /// <param name="wordlist">利益逗号分隔的词集</param>
        /// <returns>组合的信息内容</returns>
        /// 
        static public string GetMergedVMSMessage(string[] wordlist)
        {
            string ret = "";
            for (int i = 4; i < wordlist.Length - 1; i++)
            {
                ret = ret + wordlist[i] + ',';
            }
            ret = ret.Remove(ret.Length - 1);
            return ret;
        }

        /// <summary>
        /// 分割多屏信息
        /// </summary>
        /// <param name="msg">消息整体</param>
        /// <returns>分割的消息</returns>
        static public string[] GetSplitVMSMessage(string msg)
        {
            string[] ret = msg.Split('@');
            return ret;
        }


        static public List<MSG_VMS> GetVMSMessage(string[] msgs)
        {
            List<MSG_VMS> msg_vms_list = new List<MSG_VMS>();
            foreach (string msg in msgs)
            {
                MSG_VMS msg_vms = new MSG_VMS();

                msg_vms.INFO = MidStrEx(msg, "[info]", "[/info]");
                msg_vms.HOLD = int.Parse(MidStrEx(msg, "[stayTime]", "[/stayTime]"));   // 不需要Try异常
                msg_vms.COLOR = MidStrEx(msg, "[color]", "[/color]");
                msg_vms.FONT = MidStrEx(msg, "[fontFamilty]", "[/fontFamilty]");

                msg_vms_list.Add(msg_vms);
            }

            return msg_vms_list;
        }

        static public string[] GetSplitLIMessage(string msg)
        {
            string[] ret = msg.Split(';');
            return ret;
        }

        /// <summary>
        /// 获取VMS控制信息
        /// </summary>
        /// <param name="wordlist"></param>
        /// <returns></returns>
        static public List<MSG_VMS> GetVMSControlInfo(string[] wordlist)
        {
            string mergedmsgs = GetMergedVMSMessage(wordlist);
            string[] msgs = GetSplitVMSMessage(mergedmsgs);
            return GetVMSMessage(msgs);
        }

        /// <summary>
        /// 提取指定字符串之间的字符串
        /// </summary>
        /// <param name="sourse">源字符串</param>
        /// <param name="startstr">前匹配字符</param>
        /// <param name="endstr">后匹配字符</param>
        /// <returns>前后字符之间的字符</returns>
        public static string MidStrEx(string sourse, string startstr, string endstr)
        {
            string result = string.Empty;
            int startindex, endindex;
            try
            {
                startindex = sourse.IndexOf(startstr);
                if (startindex == -1)
                    return result;
                string tmpstr = sourse.Substring(startindex + startstr.Length);
                endindex = tmpstr.IndexOf(endstr);
                if (endindex == -1)
                    return result;
                result = tmpstr.Remove(endindex);
            }
            catch (Exception ex)
            {
                Debug.Log("MidStrEx Err:" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 控制指令为数值的
        /// </summary>
        public class NormalDevice
        {
            private string _DeviceType;
            private string _DeviceId;
            private int _ControlCode;
            private int _ControlStatus;

            public NormalDevice()
            {
                _DeviceType = "";
                _DeviceId = "";
                _ControlCode = -1;
                _ControlStatus = -1;
            }

            public NormalDevice(string devicetype, string deviceid, int controlcode, int controlstatus)
            {
                _DeviceType = devicetype;
                _DeviceId = deviceid;
                _ControlCode = controlcode;
                _ControlStatus = controlstatus;
            }

            public string TYPE
            {
                get { return _DeviceType; }
                set { _DeviceType = value; }
            }

            public string ID
            {
                get { return _DeviceId; }
                set { _DeviceId = value; }
            }

            public int CTRL_CODE
            {
                get { return _ControlCode; }
                set { _ControlCode = value; }
            }

            public int STATUS
            {
                get { return _ControlStatus; }
                set { _ControlStatus = value; }
            }

        }


        public class VMSDevice
        {
            private string _DeviceType;
            private string _DeviceId;
            private MSG_VMS _ControlCode;
            private MSG_VMS _Status;

            public VMSDevice()
            {
                _DeviceType = "";
                _DeviceId = "";
                _ControlCode = new MSG_VMS();
                _Status = new MSG_VMS();
            }

            public VMSDevice(string devicetype, string deviceid, MSG_VMS controlcode)
            {
                _DeviceType = devicetype;
                _DeviceId = deviceid;
                _ControlCode = controlcode;
                _Status = new MSG_VMS();
            }

            public string TYPE
            {
                get { return _DeviceType; }
                set { _DeviceType = value; }
            }

            public string ID
            {
                get { return _DeviceId; }
                set { _DeviceId = value; }
            }

            public MSG_VMS CTRL_CODE
            {
                get { return _ControlCode; }
                set { _ControlCode = value; }
            }

            public MSG_VMS STATUS
            {
                get { return _Status; }
                set { _Status = value; }
            }

        }


        // 判断设备是不是VMS
        static public bool isVMSDevice(string[] cmd)
        {
            //if (cmd[4].Contains("[")) // 添加检测器类型后，好多报文都带方括号，因此直接从类型名称判断吧
            if(cmd[2].Contains("VMS"))
                return true;
            else
                return false;
        }


        //当前检测器包括（2020-07-12 Ver 0.0.0）:
        //BrightnessDetect;             亮度检测器；
        //COVI;                         COVI
        //EventDetector;                时间检测器
        //FireDetect;                   火灾检测器
        //NoxDetector;                  氮氧化物检测器
        //rdDevice;                     交调设备
        //TemperaturDetector;           温度检测
        //VehicleDedect;                车辆检测器
        //WeatherDetect;                天气检测器
        //WindSpeedDirectionDetect      风速风向检测器;

        // ！！！注意: 变量内容拼写存在错误，不要改正！！！拼写错误在协议中。
        static private Dictionary<string, int> DetectorClassList = new Dictionary<string, int>
        {
            { "BrightnessDetect", -1},
            { "COVI", -1},
            { "EventDetector", -1},
            { "FireDetect", -1},
            { "NoxDetector", -1},
            { "rdDevice", -1},
            { "TemperaturDetector", -1},
            { "VehicleDedect", -1},
            { "WeatherDetect", -1},
            { "WindSpeedDirectionDetect", -1}
        };

        static public bool isDetector(string[] cmd)
        {
            // 判断报文涉及设备是否为检测器类设备
            string DeviceType = cmd[2];
            if (DetectorClassList.ContainsKey(DeviceType))
                return true;
            return false;
        }

        static public bool isDetector(string DeviceType)
        {
            if (DetectorClassList.ContainsKey(DeviceType))
                return true;
            return false;
        }
    }
}