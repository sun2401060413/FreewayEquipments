using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDeviceDeploy : MonoBehaviour
{
    [Header("道路桩号设置器")]
    public GameObject obj;
    [HideInInspector]
    public GameObject instance_obj;
    public float mileage;
    [HideInInspector]
    public Vector3 worldPosition = Vector3.zero;
    public Vector3 Offset = Vector3.zero;

    private RoadMileageSetting settings;
    [HideInInspector]
    public int pt_lower_index = 0, pt_upper_index = 0;


    public void GetPosition()
    {
        GetPosition(obj, mileage);
    }

    public GameObject GetPosition(GameObject _obj, float _pos)
    {
        settings = transform.GetComponent<RoadMileageSetting>();
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

            //Debug.Log("pt_miles_lower:" + pt_miles_lower);
            //Debug.Log("pt_miles_upper:" + pt_miles_upper);

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

            instance_obj = Instantiate(_obj, Vector3.zero, Quaternion.identity);
            //instance_obj = Instantiate


            instance_obj.transform.position = worldPosition;
            if (settings.isIncrease)
            {
                instance_obj.transform.LookAt(settings.pts[pt_upper_index].position);
            }
            else
            {
                instance_obj.transform.LookAt(settings.pts[pt_lower_index].position);
            }


            worldPosition = worldPosition + Offset.x * instance_obj.transform.forward + Offset.y * instance_obj.transform.up + Offset.z * instance_obj.transform.right;
            instance_obj.transform.position = worldPosition;

            return instance_obj;
        }
        catch (System.Exception)
        {
            Debug.Log("错误~~~~");
            throw;
        }

    }


    public void GetPosition(GameObject _obj, float _pos, out int next_idx)
    {
        next_idx = 0;
        settings = transform.GetComponent<RoadMileageSetting>();
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

        next_idx = pt_upper_index;

    }
}
