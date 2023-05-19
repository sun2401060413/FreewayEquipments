using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class VehicleDetectorControl : _baseDetectorControl
{
    // 按车型统计数量(可以private)
    public int count = 0;               // 当前计数
    private int count_samll = 0, count_medium = 0, count_large = 0, count_giant = 0; //小型,中型，大型，特大型

    // 用于计算时间占有率，无需分车型
    public float exist_sec = 0;

    // 按车型统计数量
    public int saved_count = 0;         // 上一次计数周期后的计数值
    public int saved_count_small = 0, saved_count_medium = 0, saved_count_large = 0, saved_count_giant = 0; //小型,中型，大型，特大型

    // 按车型统计速度
    public float saved_speed = 0;       // 上一次计数周期后的平均车速;
    public float saved_speed_samll = 0, saved_speed_medium = 0, saved_speed_large = 0, saved_speed_giant = 0; //小型,中型，大型，特大型

    // 时间占有率，不分车型
    public float saved_timeOccupancy = 0;// 上一次计数周期后的时间占有率;

    private bool last_check;            // fixeddelay中车辆存在状态;

    private Rigidbody rgbd;             // 求速度

    // 速度保存列表
    private List<float> speedlist = new List<float>();          // 速度集(不分车型)
    private List<float> speedlist_small = new List<float>();    // 小型车;
    private List<float> speedlist_medium = new List<float>();   // 中型车;
    private List<float> speedlist_large = new List<float>();    // 大型车;
    private List<float> speedlist_giant = new List<float>();    // 特大型车;

    private AutoBarrierCollider cldr;   // 检测器


    protected override int Process()
    {
        if (Status == 0) // 新启动
        {
            start_sec = SetStartSec();
            last_check = false;

            saved_count = 0;
            saved_count_small = 0; saved_count_medium = 0; saved_count_large = 0; saved_count_giant = 0;
            saved_speed = 0;
            saved_speed_samll = 0; saved_speed_medium = 0; saved_speed_large = 0; saved_speed_giant = 0;
            saved_timeOccupancy = 0;
        }
        Status = CtrlCode;
        return Status;
    }
    // Start is called before the first frame update
    protected override void ExtendStart()
    {
        // 临时保存
        count = 0;
        exist_sec = 0;

        // 结果保存
        saved_count = 0;
        saved_count_small = 0; saved_count_medium = 0; saved_count_large = 0; saved_count_giant = 0;

        saved_speed = 0;
        saved_speed_samll = 0;saved_speed_medium = 0; saved_speed_large = 0; saved_speed_giant = 0;

        speedlist.Clear();
        speedlist_small.Clear(); speedlist_medium.Clear(); speedlist_large.Clear(); speedlist_giant.Clear();

        saved_timeOccupancy = 0;

        cldr = transform.GetChild(0).GetComponent<AutoBarrierCollider>();
        //update_time_str = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        //update_time = System.DateTime.Now;
        UpdateDateTime();
    }

    protected override void ExtendFixedUpdate()
    {
        if (CtrlCode > 0)   // 开始计数
        {
            current_sec = (float)Time.timeSinceLevelLoad;
            if (current_sec >= start_sec + interval_sec)    // 计数周期到
            {
                start_sec = SetStartSec();                  // 重新设置起点
                speedlist.Clear();                          // 速度集清空

            }
            else                                            // 计数周期未到
            {
                if (isCarEntered(cldr.hasVechicleInFront))
                {
                    // 不是所有目标都有这个脚本的; 对不含车辆类型id的目标不予统计
                    if(cldr.cldObj.TryGetComponent<VehicleTypeIdentity>(out var Veh_id_script))
                    {
                        int type_code = Veh_id_script.SizeType;
                        count = count + 1;
                        countUpdateByType(type_code);

                        float curSpeed = Vector3.Magnitude(cldr.cldObj.GetComponent<Rigidbody>().velocity);
                        speedlist.Add(curSpeed);
                        speedlistAddByType(type_code, curSpeed);
                    }

                }
            }

        }
        else                // 结束计数
        {
            Status = 0;
        }
    }

    /// <summary>
    /// 重新设置计数周期起点
    /// </summary>
    /// <returns>计数周期起始秒</returns>
    protected override float SetStartSec()
    {
        // 初始化本周期计时起点;统计上一周期统计量并保存
        saved_count = count;                        // 计数值存储
        saved_count_small = count_samll; saved_count_medium = count_medium; saved_count_large = count_large; saved_count_giant = count_giant;

        saved_speed = AvgSpeed(speedlist);
        saved_speed_samll = AvgSpeed(speedlist_small); saved_speed_medium = AvgSpeed(speedlist_medium); saved_speed_large = AvgSpeed(speedlist_large); saved_speed_giant = AvgSpeed(speedlist_giant);

        saved_timeOccupancy = exist_sec / interval_sec;

        count = 0;                                  // 当前计数复位
        count_samll = 0; count_medium = 0; count_large = 0; count_giant = 0;
        exist_sec = 0;                              // 存在时间清零
        Status = 1;                                 // 开启计数状态
        //last_check = false;                     // 初始化车辆存在状态（不存在→存在，计数一次）
        //update_time_str = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        //update_time = System.DateTime.Now;
        UpdateDateTime();
        time_id = GetTimeId(update_time);

        isReadable = true;                          // 标明变量存取完毕，已可读;

        return (float)Time.timeSinceLevelLoad;
    }

    /// <summary>
    /// 车辆进入计数一次
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    bool isCarEntered(bool value)
    {
        bool res = last_check == false && value == true ? true : false;
        last_check = value;
        if (value)
        {
            exist_sec = exist_sec + Time.deltaTime;
        }
        return res;
    }

    // 计算均值
    float AvgSpeed(List<float> SpeedList)
    {
        float ret = 0;
        if (SpeedList.Count > 0)
            ret = SpeedList.Average();
        return ret;
    }

    // 根据车辆类型记录车速
    void speedlistAddByType(int type_code, float addSpeed)
    {
        switch (type_code)
        {
            case 0:
                speedlist_small.Add(addSpeed);
                break;
            case 1:
                speedlist_medium.Add(addSpeed);
                break;
            case 2:
                speedlist_large.Add(addSpeed);
                break;
            case 3:
                speedlist_giant.Add(addSpeed);
                break;
            default:
                break;
        }
    }

    // 根据车辆类型统计数量
    void countUpdateByType(int type_code)
    {
        switch (type_code)
        {
            case 0:
                count_samll = count_samll + 1;
                break;
            case 1:
                count_medium = count_medium + 1;
                break;
            case 2:
                count_large = count_large + 1;
                break;
            case 3:
                count_giant = count_giant + 1;
                break;
            default:
                break;

        }
    }
}
