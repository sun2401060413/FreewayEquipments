using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAlongRoad : MonoBehaviour
{
    //public GameObject Camera;
    public List<GameObject> Roads;
    public bool Moveable = false;
    public Vector3 Offset = Vector3.zero;

    private int pt_upper_index, pt_lower_index;
    private Vector3 worldPosition = Vector3.zero;

    [HideInInspector]
    public int road_id_record = 0;
    [HideInInspector]
    public float pos_record = 0;

    public void GetPosition(int road_id, float _pos)
    {
        road_id_record = road_id;
        pos_record = _pos;


        GameObject road = Roads[road_id];
        RoadMileageSetting settings = road.GetComponent<RoadMileageSetting>();
        if (settings.isIncrease)    // 桩号自小变大,找第一个大于参考值的数值
        {
            //pt_lower_index = settings.pts_miles.Find(elem => elem > mileage); // Find是查找数值;FindIndex是查找索引

            try  // findIndex 找不到会报错
            {
                pt_upper_index = settings.pts_miles.FindIndex(elem => elem >= _pos);     //从p0开始
                pt_lower_index = pt_upper_index == 0 ? 0 : pt_upper_index - 1;      // 确保不超范围;
            }
            catch (System.Exception) //找不到，已有桩号中，没有比他大的;
            {
                pt_upper_index = settings.pts_miles.Count - 1;
                pt_lower_index = pt_upper_index;
                throw;
            }

        }
        else                        // 桩号自大变小，找第一个小于参考值的数值
        {
            try
            {
                pt_lower_index = settings.pts_miles.FindIndex(elem => elem <= _pos);     // 从p0开始
                pt_upper_index = pt_lower_index == 0 ? 0 : pt_lower_index - 1;  // 确保不超范围
            }
            catch (System.Exception) // 找不到，已有桩号中，没有比他小的;
            {
                pt_lower_index = settings.pts_miles.Count - 1;
                pt_upper_index = pt_lower_index;
                throw;
            }

        }

        try
        {
            float pt_miles_lower = settings.pts_miles[pt_lower_index];
            float pt_miles_upper = settings.pts_miles[pt_upper_index];

            if (pt_miles_lower != pt_miles_upper)
            {
                float ratio = (_pos - pt_miles_lower) / (pt_miles_upper - pt_miles_lower);
                //Debug.Log("ratio:" + ratio);
                worldPosition = (settings.pts[pt_upper_index].position - settings.pts[pt_lower_index].position) * ratio + settings.pts[pt_lower_index].position;

            }
            else
            {
                worldPosition = settings.pts[pt_lower_index].position;
            }

            //instance_obj = Instantiate
            if (Moveable)
            {
                transform.position = worldPosition;
                if (settings.isIncrease)
                {
                    transform.LookAt(settings.pts[pt_upper_index].position);
                }
                else
                {
                    transform.LookAt(settings.pts[pt_lower_index].position);
                }


                worldPosition = worldPosition + Offset.x * transform.forward + Offset.y * transform.up + Offset.z * transform.right;
                transform.position = worldPosition;
            }
        }
        catch (System.Exception)
        {
            Debug.Log("错误~~~~");
            throw;
        }

    }

}
