using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sz.network; 

public class VMSRemoteControl : MonoBehaviour
{
    /// <summary>
    /// 目的是想通过直接传输分隔后的词表，然后根据设定自动切换多屏
    /// </summary>
    public string[] msgs = null;                            // 切分后的报文词表
    public bool input_status = false;                       // 是否新输入？新输入后需要重新设置显示配置

    private VMSControl vms_ctrl;                             // VMS控制脚本
    private List<MsgProcess.MSG_VMS> msg_list;              // 显示信息

    private float current_t;                                // 当前运行时间
    private float record_t;                                 // 上一次记录的时间

    private int clip_num = 1;                               // 屏幕片段数
    private int clip_current = 1;                           // 当前屏幕编号

    // Start is called before the first frame update
    void Start()
    {
        vms_ctrl = gameObject.GetComponent<VMSControl>();
        current_t = (float)Time.timeSinceLevelLoad;
        record_t = current_t;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        current_t = (float)Time.timeSinceLevelLoad;
        if (msgs != null)
        {
            if (input_status)
            {
                msg_list = MsgProcess.GetVMSControlInfo(msgs);
                DisplaySetting();
                input_status = false;
            }
            Display();
        }

    }


    /// <summary>
    /// 主要用于多屏切换
    /// </summary>
    private void Display()
    {
        // 存在多屏时，才有必要执行此切换脚本
        if (clip_num > 1)
        {
            // 延时指定时间后切换屏幕
            if (current_t - record_t > msg_list[clip_current-1].HOLD)
            {
                // 设置片段
                if (clip_current < clip_num)
                {
                    clip_current += 1;          // 未到最后一屏，自增
                }
                else
                {
                    clip_current = 1;           // 到最后一屏，切换至第一屏
                    
                }

                // 计时起点重置
                record_t = current_t;       // 计时七点重置

                // 切换后屏幕文本输出
                vms_ctrl.display_text = msg_list[clip_current - 1].INFO;
                vms_ctrl.color_code = msg_list[clip_current - 1].COLOR;
                
            }

        }

    }

    private void DisplaySetting()
    {
        clip_num = msg_list.Count;
        clip_current = 1;
        record_t = current_t;
        vms_ctrl.display_text = msg_list[0].INFO;
        vms_ctrl.color_code = msg_list[0].COLOR;
    }

    ///// <summary>
    ///// 将颜色与标识码对应起来
    ///// </summary>
    ///// <param name="ColorString">颜色信息</param>
    ///// <returns>颜色标识</returns>
    //private int GetColorCode(string ColorString)
    //{
    //    switch (ColorString)
    //    {
    //        case "red":
    //            return 0;
    //        case "yellow":
    //            return 1;
    //        case "green":
    //            return 2;
    //        case "white":
    //            return 3;
    //        case "#FFBF00":
    //            return 4;
    //        default:
    //            return 0;
    //        //default:
    //        //    return 2;   // 默认为白色

    //    }
    //}

}
