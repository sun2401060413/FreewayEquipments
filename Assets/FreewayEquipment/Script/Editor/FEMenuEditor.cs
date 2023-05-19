using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using sz.network;

public class FEMenuEditor : MonoBehaviour
{
    [MenuItem("FETools/ToolManager/Demo")]
    private static void CreateToolManager()
    {
        GameObject go = new GameObject("FEToolManager");
        go.AddComponent<CreateTools>();
        return;
    }

    [MenuItem("FETools/ToolManager/RoadMileageSetter")]
    private static void CreateRoadMileageSetter()
    {
        GameObject go = new GameObject("RoadMileageSetter");
        go.AddComponent<RoadMileageSetting>();
        go.AddComponent<SingleDeviceDeploy>();
        return;
    }

    [MenuItem("FETools/ToolManager/Controller")]
    private static void CreateController()
    {
        GameObject ctrller = new GameObject("Controller");
        ctrller.AddComponent<EventTrigger>();
        GameObject devs = new GameObject("Devices");
        return;
    }

    [MenuItem("FETools/ToolManager/DataLoader")]
    private static void CreateDataLoader()
    {
        GameObject go = new GameObject("DataLoader");
        go.AddComponent<DeviceInfoLoaderXLS>();
        go.AddComponent<MultiDeviceDeploy>();
        return;
    }

    [MenuItem("FETools/ToolManager/Binder")]
    private static void CreateBinder()
    {
        GameObject go = new GameObject("Binder");
        go.AddComponent<_binderControl>();
        return;
    }

    [MenuItem("FETools/ToolManager/LampDeployer")]
    private static void Create()
    {
        GameObject go = new GameObject("LampDeployer");
        go.AddComponent<LampsDeploy>();
        return;
    }


    [MenuItem("FETools/CamController/CamPlayer")]
    private static void CreateCamPlayer()
    {
        GameObject pfb = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/FreewayEquipment/Prefabs/PLAY/CamPlayer.prefab");
        Instantiate(pfb);
        return;
    }

    [MenuItem("FETools/Network/SocketServer")]
    private static void CreateSocketServer()
    {
        GameObject go = new GameObject("SocketServ");
        go.AddComponent<ServSocket>();
        return;
    }

    [MenuItem("FETools/UI/DemoUI")]
    private static void CreateDemoUI()
    {
        var pfb = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/FreewayEquipment/Prefabs/UI/DemoCanvas.prefab");
        GameObject go = Instantiate(pfb);
        return;
    }

    [MenuItem("FETools/About")]
    private static void AboutInfo()
    {
        EditorWindow.GetWindow(typeof(FEToolsAboutWindow));
        return;
    }


}