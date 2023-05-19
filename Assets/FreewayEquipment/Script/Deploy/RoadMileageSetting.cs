using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoadMileageSetting : MonoBehaviour
{
    [Header("路径对象")]
    public GameObject TrafficObj;
    [Header("定位参考点")]
    public Transform ref_pt = null;             // 参考点(初始为null，默认以p0为参考点)
    [Header("参考点桩号")]
    public float ref_pt_value = 0;              // 参考点桩号

    private float base_pt_value = 0;                  // p0默认桩号，初始为0，选择以p0为参考点时，可设置具体值；不以p0为参考点时，可通过参考点桩号计算。
    [Header("桩号方向:true-增;false-减")]
    public bool isIncrease = true;

    [HideInInspector]
    public List<Transform> pts;

    [HideInInspector]
    public List<float> pts_dist;

    //[HideInInspector]
    public List<float> pts_miles;

    private float multiplier = 1;

    //
    void Start()
    {
        if (!ref_pt)                            // 如果参考点为空，选择以p0为参考点;
            ref_pt = TrafficObj.transform.GetChild(0).GetChild(0);

        if (isIncrease)
            multiplier = 1;
        else
            multiplier = -1;
    }

    // 
    void Update()
    {
        //GetInfo();
    }


    public void GetInfo()
    {

        if (ref_pt == null)                     // 如果参考点为空，选择以p0为参考点
        {
            ref_pt = TrafficObj.transform.GetChild(0).GetChild(0);
        }


        //Debug.Log("HEHEHE");
        pts.Clear();
        foreach (Transform item in TrafficObj.transform.GetChild(0).transform)
        {
            pts.Add(item);
            //Debug.Log(item.name);
        }

        pts_miles.Clear();
        pts_dist.Clear();

        float tmp = 0;                          // 点距累计初始

        multiplier = SetMultiplier(isIncrease);         //递增或递减
        //Debug.Log("multiplier:" + multiplier);

        // 依据起点桩号和上下行计算后续轨迹点桩号
        for (int i = 0; i < pts.Count - 1; i++)
        {
            tmp = tmp + Mathf.Abs(Vector3.Distance(pts[i + 1].transform.position, pts[i].transform.position)); // [1,i]:[0,i-1]
            pts_dist.Add(tmp);
            //Debug.Log(tmp);
        }

        // pts_miles:base_pt_value基础上的距离累积值；ref_pt_value,参考点的值；pts_miles[i],参考点距离累积值，
        // 则base_pt_value参考修正后的值为:ref_pt_value - pts_miles[i]
        //if (Transform.Equals(pts[i], ref_pt))       // [0:i-1]
        //    base_pt_value = ref_pt_value - pts_dist[i] * multiplier;

        int index = pts.IndexOf(ref_pt);        // 查找参考点索引位置
        //Debug.Log(index);
        if (index > 0)
        {
            //base_pt_value = ref_pt_value - pts_dist[index - 1] * multiplier;
            base_pt_value = ref_pt_value - pts_dist[index - 1] * multiplier;
        }
        else
        {
            base_pt_value = ref_pt_value;
        }

        //Debug.Log("pts_dist[index - 1]:"+ pts_dist[index - 1]+",base_pt_value:"+base_pt_value);

        pts_miles.Clear();
        pts_miles.Add(base_pt_value);
        for (int i = 0; i < pts.Count-1; i++)
        {
            pts_miles.Add(base_pt_value + pts_dist[i]*multiplier);  
        }
        //Debug.Log(pts_miles[0]);
    }

    // 设置数据增减方向
    float SetMultiplier(bool code = true)
    {
        if (code)
            return 1;
        else
            return -1;
    }

}
