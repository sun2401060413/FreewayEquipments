using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateChemicalsAccident : MonoBehaviour
{
    public int CtrlCode = 0;
    public int Status = 0;

    public GameObject Cam;
    public Vector3 spawn_offset = new Vector3(0, 0, 0);
    public Vector3 target_offset = new Vector3(0, 0, 0);

    public GameObject prefab_obj;

    public GameObject instance_obj;

    private AutoBarrierCollider cldr;

    private Vector3 spawn_point;
    private Vector3 target_point;

    public GameObject PathObj;

    private MoveAlongRoad roadmode_srcript = null;

    public int points_idx;

    private void Start()
    {
        cldr = gameObject.GetComponent<AutoBarrierCollider>();
        CtrlCode = 0;
        Status = 0;

    }

    private void Update()
    {
        if (cldr.hasVechicleInFront && CtrlCode > 0 && Status == 0)
        {
            GameObject.Destroy(cldr.cldObj);
        }

        if (CtrlCode != Status && CtrlCode > 0)
        {
            //spawn_point = cam_position.position + spawn_offset.x * cam_position.forward + spawn_offset.y * cam_position.up + spawn_offset.z * cam_position.right;
            //target_point = cam_position.position + target_offset.x * cam_position.forward + target_offset.y * cam_position.up + target_offset.z * cam_position.right;
            spawn_point = Cam.transform.position + spawn_offset.x * Cam.transform.forward + spawn_offset.y * Cam.transform.up + spawn_offset.z * Cam.transform.right;
            target_point = Cam.transform.position + target_offset.x * Cam.transform.forward + target_offset.y * Cam.transform.up + target_offset.z * Cam.transform.right;
            //target_point = Cam.transform.position + target_offset.x * Cam.transform.forward + target_offset.y * Cam.transform.up + target_offset.z * Cam.transform.right;
            instance_obj = Instantiate(prefab_obj, spawn_point, Cam.transform.rotation);
            instance_obj.AddComponent<MovePath>();
            instance_obj.AddComponent<CarAIController>();
            instance_obj.GetComponent<MovePath>().finishPos = target_point;
            instance_obj.GetComponent<MovePath>().nextFinishPos = target_point;
            CarAIController car_ai_script = instance_obj.GetComponent<CarAIController>();
            CarMove car_move_script = instance_obj.GetComponent<CarMove>();
            car_ai_script.MOVE_SPEED = 15;
            car_ai_script.INCREASE = 2;
            car_ai_script.DECREASE = 2;
            car_ai_script.TO_CAR = 10;
            car_ai_script.TO_SEMAPHORE = 10;
            car_ai_script.MaxAngle = 8;

            car_move_script.SetTranctionControl(0.2f);
            Status = CtrlCode;
        }
    }


    public void GetObjectPosition()
    {
        SingleDeviceDeploy deploy_script = PathObj.GetComponent<SingleDeviceDeploy>();
        if(transform.parent.TryGetComponent<MoveAlongRoad>(out roadmode_srcript))
        {
            deploy_script.GetPosition(PathObj, roadmode_srcript.pos_record, out points_idx);
        }
    }




}
