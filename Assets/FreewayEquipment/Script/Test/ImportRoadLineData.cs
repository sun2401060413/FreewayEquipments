using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using EasyRoads3Dv3;
using Excel;
using RoadArchitect;
using RoadArchitect.Splination;
using UnityEngine;
using sz.tools;
//using sz.infrastructure;

public class ImportRoadLineData : MonoBehaviour
{
    [HideInInspector]
    public string filepath;

    public Transform roadArchitect;
    private RoadSystem roadSystem;
    private Road road;
    private Transform _road;
    public Transform _spline;
    private List<Transform> _nodeList = new List<Transform>();

    private WaitForSeconds wait = new WaitForSeconds(0.1f);

    [Serializable]
    public class DEVICE_INFO
    {
        public float upper_x;
        public float upper_y;
        public float upper_z;

        public float lower_x;
        public float lower_y;
        public float lower_z;
    }

    [Serializable]
    public class TUNNEL_INFO
    {
        public Transform start_point;
        public Transform end_point;
        public Transform parent_point;
    }

    public class NODE_INFO
    {
        public int _node_index;
        public string _node_name;
        public Transform _node_transform;
    }

    public List<DEVICE_INFO> DeviceInfoList = new List<DEVICE_INFO>();

    string basePath = RoadEditorUtility.GetBasePath();

    private static string[] RoadMaterialDropdownEnumDesc = new string[]{
         "Asphalt",
         "Dirt",
         "Brick",
         "Cobblestone"
     };

    private static string[] tempEnumDescriptions = new string[]{
         "Two",
         "Four",
         "Six"
     };

    public List<TUNNEL_INFO> TunnelInfoList = new List<TUNNEL_INFO>();
    public List<NODE_INFO> NodeInfoList = new List<NODE_INFO>();
    public GameObject tunnelcellsPrefab;

    public int LoadDeviceDataFromXLS()
    {
        DeviceInfoList.Clear();
        DataSet ds = LoadXLS();

        for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
        {
            DEVICE_INFO _info = new DEVICE_INFO();

            float _tmp_upper_x, _tmp_upper_y, _tmp_upper_z, _tmp_lower_x, _tmp_lower_y, _tmp_lower_z;
            float.TryParse(ds.Tables[0].Rows[i][0].ToString(), out _tmp_upper_x);   // [0]: upper_x
            float.TryParse(ds.Tables[0].Rows[i][1].ToString(), out _tmp_upper_y);   // [1]: upper_y
            float.TryParse(ds.Tables[0].Rows[i][2].ToString(), out _tmp_upper_z);   // [2]: upper_z
            float.TryParse(ds.Tables[0].Rows[i][3].ToString(), out _tmp_lower_x);   // [3]: lower_x
            float.TryParse(ds.Tables[0].Rows[i][4].ToString(), out _tmp_lower_y);   // [4]: lower_y
            float.TryParse(ds.Tables[0].Rows[i][5].ToString(), out _tmp_lower_z);   // [5]: lower_z

            _info.upper_x = _tmp_upper_x;
            _info.upper_y = _tmp_upper_y;
            _info.upper_z = _tmp_upper_z;
            _info.lower_x = _tmp_lower_x;
            _info.lower_y = _tmp_lower_y;
            _info.lower_z = _tmp_lower_z;
            DeviceInfoList.Add(_info);
            Debug.Log(_info.upper_x);

        }

        return 0;
    }

    DataSet LoadXLS()
    {
        FileStream fs = File.Open(filepath, FileMode.Open, FileAccess.Read);
        IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(fs);

        return reader.AsDataSet();
    }

    public void CreateRoadlineViaPts(string _path)  // 初步判断问题不在这里
    {
        roadSystem = roadArchitect.GetComponent<RoadSystem>();
        roadSystem.isMultithreaded = false;
        GameObject roadObj = roadSystem.AddRoad();
        road = roadObj.GetComponent<Road>();
        road.isUsingMultithreading = false;

        int count = 0;
        int interval = 3;

        int dev_info_length = DeviceInfoList.Count;

        // chosen some points in the original set
        List<DEVICE_INFO> _tmp_list = new List<DEVICE_INFO>();

        foreach (var _info in DeviceInfoList)
        {
            if (count % interval == 0)
            {
                _tmp_list.Add(_info);
            }
            count++;
        }

        int loop_count = 0;
        // int end_count = _list.Count;
        int end_count = 319;     // TODO: 修改：注：end_count:15;loop_count:17，即[15:17]可以运行,[15;32]OK,
        foreach (var _info in _tmp_list)
        {
            if (loop_count == 320)        // 验证测试:10、15不乱序；17、20乱序；
                break;
            road.editorMousePos = new Vector3(_info.upper_x, _info.upper_y, _info.upper_z);
            road.isEditorMouseHittingTerrain = true;
            if (road.spline && road.spline.previewSpline)
            {
                if (road.spline.previewSpline.nodes == null || road.spline.previewSpline.nodes.Count < 1)
                {
                    road.spline.Setup();
                }
                road.spline.previewSpline.mousePos = new Vector3(_info.upper_x, _info.upper_y, _info.upper_z);
                road.spline.previewSpline.isDrawingGizmos = true;
            }

            //bool isUsed = false;
            if (road.isEditorMouseHittingTerrain)
            {
                Event.current.Use();
                if (loop_count == end_count)
                {
                    Construction.CreateAmountNodes(_road: road, _isSpecialEndNode: false, _vectorSpecialLoc: road.editorMousePos, _isInterNode: false, _isUpdateEnable: true);
                }
                else
                {
                    Construction.CreateAmountNodes(_road: road, _isSpecialEndNode: false, _vectorSpecialLoc: road.editorMousePos, _isInterNode: false);
                }
                
            }
            else
            {
                Debug.Log("Invalid surface for new node. Must be terrain.");
            }

            loop_count = loop_count + 1;

        }


        //// 打印node

        //Transform spline_transform = road.GetComponent<Transform>().GetChild(0);
        //Debug.Log(spline_transform.name);
        //Debug.Log("=============================");
        //for (int i = 0; i < spline_transform.childCount; i++)
        //{
        //    Debug.Log(spline_transform.GetChild(i).name);
        //}
        return;
    }




    // // Function to pause for a while
    // IEnumerator WaitForSeconds()
    // {
    //     yield return new WaitForSeconds(10f);
    // }


    //public void CreateTunnelCellsViaPts(List<TUNNEL_INFO> _tunnel_info)

    #if UNITY_EDITOR
    [ContextMenu("Play")]
    public void CreateTunnelCellsViaPts()
    {
        foreach (var _info in TunnelInfoList)
        {
            _nodeList = sz.tools.CodeEditTools.GetAllChildTransforms(_info.parent_point);
            bool _creating_flag = false;
            foreach (var _node in _nodeList)
            {
                if (_node.name == _info.start_point.name)
                {
                    _creating_flag = true;
                }

                if (_node.name == _info.end_point.name)
                {
                    _creating_flag = false;
                }

                if (_creating_flag)
                {
                    SplineN _node_splineN = _node.GetComponent<SplineN>();

                    if (_node_splineN.SplinatedObjects.Count == 0)
                    {
                        SplinatedMeshMaker _splinatedMeshMaker = _node_splineN.AddSplinatedObject();

                        // Debug.Log("AddSplinatedObject!");
                        // Debug.Log(_node_splineN.SplinatedObjects.Count);
                        _splinatedMeshMaker.objectName = "tunnelcells";
                        _splinatedMeshMaker.currentSplination = tunnelcellsPrefab;
                        _splinatedMeshMaker.isTrimEnd = true;
                    }

                }

            }

        }


        // Debug.Log("CreateTunnelCellsViaPts!");
        // SplineN _splineN = TunnelInfoList[0].start_point.GetComponent<SplineN>();

        // if (_splineN.SplinatedObjects.Count == 0)
        // {
        //     SplinatedMeshMaker _splinatedMeshMaker = _splineN.AddSplinatedObject();
        //
        //     Debug.Log("AddSplinatedObject!");
        //     Debug.Log(_splineN.SplinatedObjects.Count);
        //     _splinatedMeshMaker.objectName = "tunnelcells";
        //     _splinatedMeshMaker.currentSplination = tunnelcellsPrefab;
        //     _splinatedMeshMaker.isTrimEnd = true;
        // }
    }



    #endif



    // Get the father node of the tunnel


}
