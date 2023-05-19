using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using sz.tools;

namespace sz.ui
{
    public enum ACCIDENT_AREA   // 事故区域类型
    {
        both = 0,               // 两车道 
        slowlane = 1,           // 慢车道/主车道
        fastlane = 2            // 快车道/超车道
    }

    public enum ACCIDENT_TYPE   // 事故类型
    {
        rear_ended = 0,         // 追尾
        fire = 1,               // 火灾
        chemicals = 2,          // 危化品
        abandoned = 3           // 抛洒物
    }

    public class CamCtrlUI : MonoBehaviour
    {
        public GameObject MainCam;      // 摄像机【定制】
        public Button GodMode;          // 上帝模式选择键
        public Button DriverMode;       // 驾驶模式选择键
        public Button RoadMode;         // 巡线模式选择键
        public Button ReSelect;         // 驾驶模式-重新选择车辆
        public Button CreateCongest;    // 创建事故键
        public Dropdown Direction;      // 巡线方向
        public Slider Mileage;          // 桩号选择划钮
        public InputField MileageDisplay;   // 桩号显示 
        public Text Note;               // 桩号显示标题问题
        public Dropdown AccidentArea;   // 事故发生车道选择下拉框
        public Dropdown AccidentType;   // 事故类型选择下拉框
        public Button DriveSimMode;     // 模拟驾驶模式选择键
        public Button DriveSimReset;    // 模式驾驶模式重置键
        public Button WeatherSet;       // 天气设置按钮
        public Button CamFollowView;    // 车辆跟随视角按钮
        //public bool WeatherSetEnable = false;

        //[Header("用于巡线模式")]
        //public GameObject CamController;
        //[Header("用于产生")]
        //public GameObject Path;         // 用于司机视角
        [Header("用于产生事故")]
        public GameObject StopperPrefab;
        public List<GameObject> FirePrefab;
        public GameObject chemical_acc_creator;
        [Header("天气控制面板")]
        public GameObject WeatherSetUI;

        [Header("模拟驾驶对象")]
        public GameObject DriveCar;
        [Header("用于模拟驾驶重生")]
        public GameObject SpawnPoint;

        private int Status = 1; // 1-GodMode,2-DriverMode,3-RoadMode;
        private Vector3 GodModeSavedPos = Vector3.zero;
        private Quaternion GodModeSavedOritation = Quaternion.identity;
        private float RoadModeSavedPos = 0;
        private float initRoadModePos = 0;
        private bool DriveSimStatus = false;


        private MouseLookAdvanced ScriptMouseLookAdvanced;  // 漫游模式摄像机设置
        private DriverMode ScriptDriverMode;                // 司机模式摄像机设置
        private DriveSimMode ScriptDriveSimMode;            // 模拟驾驶模式摄像机设置
        private MoveAlongRoad ScriptMoveAlongRoad;          // 巡线模式摄像机设置

        //private int AccidentAreaCode = 0;   // 0-两车道，1-主车道，2-超车道

        [HideInInspector]
        public bool AccStatus = false;  // 是否属于事故状态

        private List<GameObject> stopper = new List<GameObject>();

        private bool set_fire = false;  // 设置火灾状态
        private bool onfire = false;    // 起火状态;
        private AutoBarrierCollider stopper_cldr = null;   // 停车器的碰撞器，可用于检测驶入目标并触发事故。
        private GameObject onfire_obj = null;
        private List<GameObject> fire_instances = new List<GameObject>();

        private bool set_chemical = false; // 设置危化品事故状态
        private CreateChemicalsAccident chemicalsAccidentCreator = null;

        // 用于模拟驾驶车辆跟随视角
        private bool CamFollowViewStatus = false;
        private Vector3 saved_camPos = Vector3.zero;

        // 后视镜
        public GameObject L_mirror;
        public GameObject R_mirror;

        // Start is called before the first frame update
        void Start()
        {
            Status = 0;

            ScriptMouseLookAdvanced = MainCam.GetComponent<MouseLookAdvanced>();
            ScriptDriverMode = MainCam.GetComponent<DriverMode>();
            ScriptDriveSimMode = MainCam.GetComponent<DriveSimMode>();
            ScriptMoveAlongRoad = MainCam.GetComponent<MoveAlongRoad>();

            initRoadModePos = MainCam.GetComponent<MoveAlongRoad>().Roads[0].GetComponent<RoadMileageSetting>().pts_miles[0];
            //if (WeatherSetEnable)
            //    WeatherSet.gameObject.SetActive(true);
            //else
            //    WeatherSet.gameObject.SetActive(false);
            WeatherSetUI.gameObject.SetActive(false);

            // 后视镜
            L_mirror = CodeEditTools.GetChildTransformByName(DriveCar.transform, "Camera_L").gameObject;
            R_mirror = CodeEditTools.GetChildTransformByName(DriveCar.transform, "Camera_R").gameObject;

            stopper_cldr = null;
            set_chemical = false;
            chemical_acc_creator.SetActive(false);

            // 模拟驾驶模式下,摄像机跟随视角触发状态；
            CamFollowViewStatus = false;
            saved_camPos = MainCam.GetComponent<DriveSimMode>().Offset;

            OnGodMode();

        }

        // Update is called once per frame
        void Update()
        {
            if (set_fire)
                SetFireUpdate();
            if (onfire)
                FirePosUpdate();
            if (set_chemical)
                SetChemicalUpdate();

        }

        public void OnGodMode()
        {
            if (3 == Status)
            {
                RoadModeSavedPos = Mileage.value;
            }

            GodMode.image.color = Color.green;
            DriverMode.image.color = Color.white;
            RoadMode.image.color = Color.white;

            ScriptMouseLookAdvanced.enabled = true;
            ScriptDriverMode.enabled = false;
            ScriptDriveSimMode.enabled = false;
            ScriptMoveAlongRoad.enabled = false;

            if (Status > 0)
            {
                MainCam.transform.position = GodModeSavedPos;
                MainCam.transform.rotation = GodModeSavedOritation;
            }

            Direction.gameObject.SetActive(false);
            Mileage.gameObject.SetActive(false);
            MileageDisplay.gameObject.SetActive(false);
            Note.gameObject.SetActive(false);
            ReSelect.gameObject.SetActive(false);
            CreateCongest.gameObject.SetActive(false);
            AccidentArea.gameObject.SetActive(false);
            AccidentType.gameObject.SetActive(false);
            DriveSimMode.gameObject.SetActive(false);
            DriveSimReset.gameObject.SetActive(false);
            CamFollowView.gameObject.SetActive(false);
            // 后视镜模式
            L_mirror.SetActive(false);
            R_mirror.SetActive(false);
            //if(WeatherSetEnable)
            //    WeatherSetUI.SetActive(false);

            Status = 1;
        }

        public void OnDriverMode()
        {
            if (1 == Status)    // 保留漫游模式的摄像机位置
            {
                GodModeSavedPos = MainCam.transform.position;
                GodModeSavedOritation = MainCam.transform.rotation;
            }
            if (3 == Status)    // 保留巡线模式的摄像机位置
            {
                RoadModeSavedPos = Mileage.value;
            }

            // 主要的三种模式
            GodMode.image.color = Color.white;
            DriverMode.image.color = Color.green;
            RoadMode.image.color = Color.white;

            ScriptMouseLookAdvanced.enabled = false;
            ScriptDriverMode.enabled = true;
            ScriptDriveSimMode.enabled = false;
            ScriptMoveAlongRoad.enabled = false;

            Direction.gameObject.SetActive(false);
            Mileage.gameObject.SetActive(false);
            MileageDisplay.gameObject.SetActive(false);
            Note.gameObject.SetActive(false);
            ReSelect.gameObject.SetActive(true);
            CreateCongest.gameObject.SetActive(false);
            AccidentArea.gameObject.SetActive(false);
            AccidentType.gameObject.SetActive(false);

            // DriveSimMode
            DriveSimMode.gameObject.SetActive(true);
            if (!DriveSimStatus)
            {
                DriveSimReset.gameObject.SetActive(false);
                CamFollowView.gameObject.SetActive(false);
            }
            
            // 后视镜
            L_mirror.SetActive(false);
            R_mirror.SetActive(false);

            Status = 2;
        }

        public void OnRoadMode()
        {
            if (1 == Status)
            {
                GodModeSavedPos = MainCam.transform.position;
                GodModeSavedOritation = MainCam.transform.rotation;
            }

            GodMode.image.color = Color.white;
            DriverMode.image.color = Color.white;
            RoadMode.image.color = Color.green;

            ScriptMouseLookAdvanced.enabled = false;
            ScriptDriverMode.enabled = false;
            ScriptDriveSimMode.enabled = false;
            ScriptMoveAlongRoad.enabled = true;

            OnDropDownChanged();

            // 
            if (0 == RoadModeSavedPos)
                Mileage.value = initRoadModePos;
            else
                Mileage.value = RoadModeSavedPos;


            MoveAlongRoad ctrl = MainCam.GetComponent<MoveAlongRoad>();
            ctrl.Moveable = true;
            ctrl.GetPosition(Direction.value, Mileage.value);

            Direction.gameObject.SetActive(true);
            Mileage.gameObject.SetActive(true);
            MileageDisplay.gameObject.SetActive(true);
            Note.gameObject.SetActive(true);
            ReSelect.gameObject.SetActive(false);
            CreateCongest.gameObject.SetActive(true);
            AccidentArea.gameObject.SetActive(true);
            AccidentType.gameObject.SetActive(true);
            DriveSimMode.gameObject.SetActive(false);
            DriveSimReset.gameObject.SetActive(false);
            CamFollowView.gameObject.SetActive(false);

            // 后视镜
            L_mirror.SetActive(false);
            R_mirror.SetActive(false);

            Status = 3;
        }

        public void OnReSelect()
        {
            ScriptDriverMode.DeleteSelectedCar();
        }


        public void OnDropDownChanged()
        {
            switch (Direction.value)
            {
                case 0:
                    //Mileage.minValue = 1533159;
                    //Mileage.maxValue = 1538390;
                    int length_0 = MainCam.GetComponent<MoveAlongRoad>().Roads[0].GetComponent<RoadMileageSetting>().pts_miles.Count;
                    var v01 = MainCam.GetComponent<MoveAlongRoad>().Roads[0].GetComponent<RoadMileageSetting>().pts_miles[0];
                    var v02 = MainCam.GetComponent<MoveAlongRoad>().Roads[0].GetComponent<RoadMileageSetting>().pts_miles[length_0 - 1];
                    Mileage.minValue = v01 < v02 ? v01 : v02;
                    Mileage.maxValue = v01 < v02 ? v02 : v01;
                    break;
                case 1:
                    //Mileage.minValue = 1533183;
                    //Mileage.maxValue = 1538268;
                    int length_1 = MainCam.GetComponent<MoveAlongRoad>().Roads[1].GetComponent<RoadMileageSetting>().pts_miles.Count;
                    var v11 = MainCam.GetComponent<MoveAlongRoad>().Roads[1].GetComponent<RoadMileageSetting>().pts_miles[0];
                    var v12 = MainCam.GetComponent<MoveAlongRoad>().Roads[1].GetComponent<RoadMileageSetting>().pts_miles[length_1 - 1];
                    Mileage.minValue = v11 < v12 ? v11 : v12;
                    Mileage.maxValue = v11 < v12 ? v12 : v11;
                    break;
            }
            MoveAlongRoad ctrl = MainCam.GetComponent<MoveAlongRoad>();
            ctrl.GetPosition(Direction.value, Mileage.value);
        }

        public void OnSilderChanged()
        {
            MileageDisplay.text = Mileage.value.ToString();
            MoveAlongRoad ctrl = MainCam.GetComponent<MoveAlongRoad>();
            ctrl.GetPosition(Direction.value, Mileage.value);
        }

        //void MousePick()
        //{
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit hit;

        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            Debug.Log(hit.transform.name);
        //            //Debug.Log(hit.transform.tag);
        //        }
        //    }
        //}


        public void OnCreateCongest()
        {
            if (!AccStatus)
            {
                stopper.Clear();

                // float multiplier = Path.GetComponent<RoadMileageSetting>().isIncrease ? 1 : -1;
                float multiplier = MainCam.GetComponent<MoveAlongRoad>().Roads[Direction.value].GetComponent<RoadMileageSetting>().isIncrease ? 1 : -1;
                float stopper_milage = Mileage.value + multiplier * 20;

                bool area_flag = false;     // 事故区域标识; true: 行车道, false: 超车道；【有些事故只发生在一侧】

                if (AccidentArea.value == (int)ACCIDENT_AREA.both || AccidentArea.value == (int)ACCIDENT_AREA.slowlane)
                {

                    //Path.GetComponent<SingleDeviceDeploy>().Offset = new Vector3(0, 0, 2);
                    MainCam.GetComponent<MoveAlongRoad>().Roads[Direction.value].GetComponent<SingleDeviceDeploy>().Offset = new Vector3(0, 0, 2);
                    //GameObject tmpObj = Path.GetComponent<SingleDeviceDeploy>().GetPosition(StopperPrefab, stopper_milage);
                    GameObject tmpObj = MainCam.GetComponent<MoveAlongRoad>().Roads[Direction.value].GetComponent<SingleDeviceDeploy>().GetPosition(StopperPrefab, stopper_milage);
                    tmpObj.GetComponent<AccidentStopper>().CtrlCode = 1;
                    stopper.Add(tmpObj);

                    area_flag = true;
                }

                if (AccidentArea.value == (int)ACCIDENT_AREA.both || AccidentArea.value == (int)ACCIDENT_AREA.fastlane)
                {
                    //Path.GetComponent<SingleDeviceDeploy>().Offset = new Vector3(0, 0, -2);
                    MainCam.GetComponent<MoveAlongRoad>().Roads[Direction.value].GetComponent<SingleDeviceDeploy>().Offset = new Vector3(0, 0, -2);
                    //GameObject tmpObj = Path.GetComponent<SingleDeviceDeploy>().GetPosition(StopperPrefab, stopper_milage);
                    GameObject tmpObj = MainCam.GetComponent<MoveAlongRoad>().Roads[Direction.value].GetComponent<SingleDeviceDeploy>().GetPosition(StopperPrefab, stopper_milage);
                    tmpObj.GetComponent<AccidentStopper>().CtrlCode = 1;
                    stopper.Add(tmpObj);
                }


                float side_multiplier = area_flag ? 1 : -1;
      
                //Vector3 side_offset = new Vector3(0, 0, 2 * side_multiplier);

                if (AccidentType.value == (int)ACCIDENT_TYPE.fire)
                {
                    fire_instances.Clear();
                    stopper_cldr = CodeEditTools.GetChildTransformByName(stopper[0].transform, "CheckBox").GetComponent<AutoBarrierCollider>();
                    set_fire = true;
                }

                if (AccidentType.value == (int)ACCIDENT_TYPE.chemicals)
                {
                    chemical_acc_creator.SetActive(true);
                    chemicalsAccidentCreator = chemical_acc_creator.GetComponent<CreateChemicalsAccident>();
                    chemicalsAccidentCreator.spawn_offset.z = 2 * side_multiplier;
                    chemicalsAccidentCreator.target_offset.z = 2 * side_multiplier;
                    chemicalsAccidentCreator.PathObj = MainCam.GetComponent<MoveAlongRoad>().Roads[Direction.value];
                    chemicalsAccidentCreator.Status = 0;
                    chemicalsAccidentCreator.CtrlCode = 1;
                    chemicalsAccidentCreator.GetObjectPosition();
                    set_chemical = true;
                }

                if (AccidentType.value == (int)ACCIDENT_TYPE.abandoned)
                {
                    // TODO
                }


                //stopper.tag = "Untagged";
                CreateCongest.transform.GetChild(0).GetComponent<Text>().text = "删除事故";
                AccStatus = true;
            }
            else
            {
                // 取消停车标记
                foreach (GameObject elem in stopper)
                {
                    GameObject.Destroy(elem);
                }
                stopper.Clear();
                stopper_cldr = null;
                onfire = false;

                // 取消火灾效果
                if (fire_instances.Count > 0)
                {
                    foreach (GameObject elem in fire_instances)
                    {
                        GameObject.Destroy(elem);
                    }
                }

                // 取消危化品事故创建器

                if (set_chemical)
                {
                    chemicalsAccidentCreator.CtrlCode = 0;
                    GameObject.Destroy(chemicalsAccidentCreator.instance_obj);
                    chemical_acc_creator.SetActive(false);
                    set_chemical = false;
                }


                CreateCongest.transform.GetChild(0).GetComponent<Text>().text = "创建事故";
                AccStatus = false;
            }
        }

        public void SetFireUpdate()
        {
            if (stopper_cldr != null)
            {
                if (stopper_cldr.hasVechicleInFront && !onfire)
                {
                    onfire_obj = stopper_cldr.cldObj;
                    foreach (GameObject elem in FirePrefab)
                    {
                        GameObject tmp = new GameObject();
                        tmp.transform.parent = onfire_obj.transform;
                        tmp = Instantiate(elem, onfire_obj.transform.localPosition, Quaternion.identity);
                        fire_instances.Add(tmp);
                    }

                    // 完成起火
                    set_fire = false;
                    onfire = true;
                }
            }
        }

        public void FirePosUpdate()
        {
            if (fire_instances.Count > 0)
            {
                foreach (GameObject elem in fire_instances)
                {
                    if (elem != null)
                    {
                        elem.transform.position = onfire_obj.transform.position;
                    }
                }
            }
        }

        public void SetChemicalUpdate()
        {
            if (chemicalsAccidentCreator != null)
            {
                chemicalsAccidentCreator.CtrlCode = 1;
            }
        }

        ///// <summary>
        ///// 复制主相机   
        ///// </summary>
        ///// <returns></returns>
        //public GameObject CopyMainCamera(Transform parent)
        //{
        //    GameObject obj = new GameObject();
        //    obj.AddComponent<Camera>();
        //    obj.AddComponent<AudioListener>();

        //    obj.GetComponent<Camera>().CopyFrom(MainCam.GetComponent<Camera>());


        //    return Instantiate(obj, parent);
        //}

        public void OnWeatherSetClicked()
        {
            if (!WeatherSetUI.activeInHierarchy)
            {
                WeatherSetUI.SetActive(true);
            }
            else
            {
                WeatherSetUI.SetActive(false);
            }

        }

        public void OnDriveSimMode()
        {
            if (DriveSimStatus == false)
            {
                DriveSimStatus = true;

                DriveSimMode.image.color = Color.green;

                ScriptDriverMode.enabled = false;
                ScriptDriveSimMode.enabled = true;

                DriveCar.SetActive(true);
                ReSelect.gameObject.SetActive(false);
                DriveSimReset.gameObject.SetActive(true);
                CamFollowView.gameObject.SetActive(true);

                // 后视镜
                L_mirror.SetActive(true);
                R_mirror.SetActive(true);
            }
            else
            {
                DriveSimStatus = false;

                DriveSimMode.image.color = Color.white;

                ScriptDriveSimMode.enabled = false;

                DriveCar.SetActive(false);

                // 后视镜
                L_mirror.SetActive(false);
                R_mirror.SetActive(false);

                OnDriverMode();
            }
        }

        public void OnDriveSimReset()
        {
            DriveCar.transform.localPosition = SpawnPoint.transform.localPosition;
            DriveCar.transform.rotation = SpawnPoint.transform.rotation;
        }


        public void OnCamFollowView()
        {
            DriveSimMode script = MainCam.GetComponent<DriveSimMode>();
            if (CamFollowViewStatus)
            {
                CamFollowViewStatus = false;
                CamFollowView.image.color = Color.white;
                CamFollowView.transform.GetChild(0).GetComponent<Text>().text = "跟随视角";
                script.Offset = saved_camPos;
            }
            else
            {
                script.Offset = new Vector3(-7, 3, 0);
                CamFollowViewStatus = true;
                CamFollowView.image.color = Color.green;
                CamFollowView.transform.GetChild(0).GetComponent<Text>().text = "驾驶视角";
                
            }
        }
        


    }

}

