using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sz.device;

public class _binderControl : MonoBehaviour
{
    public GameObject master;
    public GameObject[] slavers;

    private int CtrlCode;
    private int SavedCtrlCode = -1;

    // private DeviceTypeIdentity devTypeId;

    // Start is called before the first frame update
    void Start()
    {
        CtrlCode = GetCtrlCode(master);           // 根据类型获取mater的控制码

        foreach (GameObject slave in slavers)
        {
            SetCtrlCode(slave, CtrlCode);         // 设置每一个Slave的CtrlCode
        }

        SavedCtrlCode = CtrlCode;
    }

    // Update is called once per frame
    void Update()
    {
        CtrlCode = GetCtrlCode(master);
        if (CtrlCode != SavedCtrlCode)
        {
            foreach (GameObject slave in slavers)
            {
                SetCtrlCode(slave, CtrlCode);
            }
            SavedCtrlCode = CtrlCode;
        }
    }

    // 泛型的写法，需要一个基类，比如此例子中的'_baseBoolControls'.
    public void SetOneCtrlCode<T>(GameObject obj, int value) where T : _baseCtrlCodeControl
    {
        var script = obj.GetComponent<T>();
        //script.Status = value;
        script.CtrlCode = value;
    }

    public int GetOneCtrlCode<T>(GameObject obj) where T : _baseCtrlCodeControl
    {
        var script = obj.GetComponent<T>();
        return script.CtrlCode;
    }

    public int GetOneStatus<T>(GameObject obj) where T : _baseCtrlCodeControl
    {
        var script = obj.GetComponent<T>();
        return script.Status;
    }

    public void SetCtrlCode(GameObject obj, int value)
    {
        int _type = obj.GetComponent<DeviceTypeIdentity>().TYPE;
        switch (_type)
        {
            case 2:
                SetOneCtrlCode<LightAlarmControl>(obj, value);
                break;
            case 3:
                SetOneCtrlCode<AutoBarrierControl>(obj, value);
                break;
            case 7:
                SetOneCtrlCode<LampControl>(obj, value);
                break;
            case 8:
                SetOneCtrlCode<SpinAlarmControl>(obj, value);
                break;
            case 9:
                SetOneCtrlCode<BasicDoor>(obj, value);
                break;
            default:
                break;
        }
    }

    public int GetCtrlCode(GameObject obj)
    {
        int _type = obj.GetComponent<DeviceTypeIdentity>().TYPE;
        switch (_type)
        {
            case 2:
                return GetOneCtrlCode<LightAlarmControl>(obj);
            case 3:
                return GetOneCtrlCode<AutoBarrierControl>(obj);
            case 7:
                return GetOneCtrlCode<LampControl>(obj);
            case 8:
                return GetOneCtrlCode<SpinAlarmControl>(obj);
            case 9:
                return GetOneCtrlCode<BasicDoor>(obj);
            default:
                return -1;
        }
    }
}
