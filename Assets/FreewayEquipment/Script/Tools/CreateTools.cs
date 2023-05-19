using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using sz.network;

public class CreateTools : MonoBehaviour
{
    [HideInInspector]
    public GameObject CamPlayer;
    [Header("交通流对象(任选一路用于驾驶模式展示)")]
    public GameObject roadtraffic;
    [Header("路径对象")]
    public GameObject roadUpward;
    public GameObject roadDownward;
    //[Header("xls数据文件地址")]
    [HideInInspector]
    public string filepath;

    private GameObject road_up, road_down;

    [HideInInspector]
    public RoadMileageSetting roadUpSetter, roadDownSetter;
    [HideInInspector]
    public GameObject socketserv,devices,controller;
    [HideInInspector]
    public GameObject dataloader, DemoCanvas;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void CreateRoadMileageSetter()
    {
        road_up = new GameObject("1-RoadUpwardMileage");
        road_down = new GameObject("1-RoadDownwardMileage");

        roadUpSetter = road_up.AddComponent<RoadMileageSetting>();
        road_up.AddComponent<SingleDeviceDeploy>();
        roadDownSetter = road_down.AddComponent<RoadMileageSetting>();
        road_down.AddComponent<SingleDeviceDeploy>();

        if (roadUpward != null)
        {
            roadUpSetter.TrafficObj = roadUpward;
            roadUpSetter.isIncrease = true;
        }

        if (roadDownward != null)
        {
            roadDownSetter.TrafficObj = roadDownward;
            roadDownSetter.isIncrease = false;
        }


    }

    public void CreateSocketServer()
    {
        socketserv = new GameObject("2-SocketServer");
        socketserv.AddComponent<ServSocket>();
            
    }

    public void CreateDevicesAndController()
    {
        devices = new GameObject("3-Devices");
        controller = new GameObject("3-Controller");
        controller.AddComponent<sz.network.EventTrigger>();
        if (socketserv != null)
            controller.GetComponent<sz.network.EventTrigger>().socket_serv = socketserv;
    }

    public void CreateDataLoader()
    {
        //dataloader = new GameObject("4-Dataloader");
        //dataloader.AddComponent<DeviceInfoLoaderXLS>();
        //dataloader.AddComponent<MultiDeviceDeploy>();
        GameObject pts = new GameObject();
# if UNITY_EDITOR
        //var pts = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/FreewayEquipment/Prefabs/EDIT/DataLoader1.prefabs");
        pts = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/FreewayEquipment/Prefabs/EDIT/DataLoader 1.prefab");
        //GameObject pts = Resources.Load("Assets/FreewayEquipment/Prefabs/EDIT/binder.prefab") as GameObject;
# endif
        dataloader = Instantiate(pts);
        dataloader.name = "4-Dataloader";
        if (filepath != null)
            dataloader.GetComponent<DeviceInfoLoaderXLS>().filepath = filepath;
        if (devices != null)
            dataloader.GetComponent<MultiDeviceDeploy>().ObjParent = devices;
        if (controller != null)
            dataloader.GetComponent<MultiDeviceDeploy>().Event_Trigger = controller;
        if (road_up != null)
        {
            dataloader.GetComponent<MultiDeviceDeploy>().Area_1 = road_up;
            dataloader.GetComponent<MultiDeviceDeploy>().Area_3 = road_up;
        }
        if (road_down != null)
            dataloader.GetComponent<MultiDeviceDeploy>().Area_2 = road_down;
        
    }

    public void CreateCamPlayer()
    {
        GameObject pts = new GameObject();
#if UNITY_EDITOR
        pts = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/FreewayEquipment/Prefabs/PLAY/CamPlayer.prefab");
# endif
        CamPlayer = Instantiate(pts);
        CamPlayer.name = "5-CamPlayer";

        if(roadtraffic != null)
            CamPlayer.GetComponent<DriverMode>().path = roadtraffic;

        CamPlayer.GetComponent<MoveAlongRoad>().Roads.Clear();

        if (road_up != null)
            CamPlayer.GetComponent<MoveAlongRoad>().Roads.Add(road_up);

        if(road_down != null)
            CamPlayer.GetComponent<MoveAlongRoad>().Roads.Add(road_down);

    }


    public void CreateDemoUI()
    {
        //var pts = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/FreewayEquipment/Prefabs/UI/DemoCanvas.prefabs");
        GameObject pts = new GameObject();
# if UNITY_EDITOR
        pts = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/FreewayEquipment/Prefabs/UI/DemoCanvas.prefab");
# endif
        DemoCanvas = Instantiate(pts);
        DemoCanvas.name = "6-DemoCanvas";
        if(CamPlayer)
            DemoCanvas.GetComponent<sz.ui.CamCtrlUI>().MainCam = CamPlayer;
    }




}
