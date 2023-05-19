using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class _baseDetectorControl : MonoBehaviour
{
    public int CtrlCode;        // 控制码;
    public int Status;          // 控制状态;

    [HideInInspector]
    public bool isSendable = false; // 默认信息可发送功能[考虑到某些继承子类不需要自带的发送功能];
    [HideInInspector]
    public bool isReadable = false; // 信息可读标志位;此标识置位时,表明信息已经可读,读取完成不需要再读时,将此位复位[可用于发送失败后的重发];
    public bool isSendAvailable = true; // 所有数据读取完成，允许发送(用于反馈失败的重复发送);

    public float interval_sec = 60;     // 默认计数周期
    public float resent_interval = 5;   // 默认重发时常

    // 统计设置
    // 信息更新时刻
    public string update_time_str;  // 更新事件[yyyy-mm-dd hh:mm]
    public DateTime update_time;
    public float update_sec;        // 信息更新秒;
    public float last_sent_sec;     // 上一次发送时间;

    public int time_id;             // 时间序号

    [HideInInspector]
    public float start_sec;        // 周期起始时间(用于统计)
    [HideInInspector]
    public float current_sec;      // 周期当前时间

    // 扩展功能虚函数
    protected virtual void ExtendStart() { }
    protected virtual void ExtendUpdate() { }
    protected virtual void ExtendFixedUpdate() { }

    protected virtual int Process() { return CtrlCode; }       // 启动方法，指令码下达后仅执行一次

    //protected virtual string GetDetectorDataStr() { return null; }

    /// <summary>
    /// 检测器报文发送任务
    /// 每执行一次，需要将isSendAvailable复位一次;
    /// </summary>
    /// <returns></returns>
    public virtual string DetectorSendTask() { return null; }
    //public virtual string DetectorSendTask()
    //{
    //    if (isReadable && isSendAvailable)
    //    {
    //        isSendAvailable = false;
    //        last_sent_sec = Time.timeSinceLevelLoad;
    //    }
    //    return GetDetectorDataStr();
    //}
    // Start is called before the first frame update
    void Start()
    {
        Status = -1;
        isReadable = false;
        ExtendStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (CtrlCode != Status)
            Status = Process();

        ExtendUpdate();
    }

    void FixedUpdate()
    {
        //if(isSendable)

        ExtendFixedUpdate();
    }

    public void UpdateDateTime()
    {
        update_time_str = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        update_time = System.DateTime.Now;
    }

    /// <summary>
    /// 获取时间id，即采集周期是一天中的第几个周期
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int GetTimeId(DateTime value)
    {
        int sec_id = value.Hour * 60 * 60 + value.Minute * 60 + value.Second;
        Debug.Log(sec_id + ":" + sec_id / (int)interval_sec);
        return sec_id / (int)interval_sec;
    }

    //protected void baseReset
    public void SetSendSleep()
    {
        if (Time.timeSinceLevelLoad - last_sent_sec >= resent_interval)
            isSendAvailable = true;
    }

    /// <summary>
    /// 重新设置下一消息报送周期延时起点
    /// </summary>
    /// <returns></returns>
    protected virtual float SetStartSec()
    {
        UpdateDateTime();
        time_id = GetTimeId(update_time);

        isReadable = true;

        return (float)Time.timeSinceLevelLoad;
    }
}
