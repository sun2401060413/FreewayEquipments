using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Excel;
using System.IO;
using System.Data;

public class DeviceInfoLoaderXLS : MonoBehaviour
{
    public string filepath;

    [Serializable]
    public class DEVICE_INFO
    {
        public string  Name;               // [9]: 备注信息
        public string  ID;                 // [0]: 设备ID
        public string  Class;              // [2]: 设备类型
        public float   Position;           // [4]: 设备位置
        public string  Area;               // [5]: 设备区域【上行，下行】
    }

    public List<DEVICE_INFO> DeviceInfoList = new List<DEVICE_INFO>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int LoadDeviceDataFromXLS()
    {
        DeviceInfoList.Clear();
        DataSet ds = LoadXLS();
        //Debug.Log("列数：" + ds.Tables[0].Columns.Count);
        //Debug.Log("行数：" + ds.Tables[0].Rows.Count);
        //Debug.Log("info:" + ds.Tables[0].Rows[0][0]);
        for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
        {
            DEVICE_INFO _info = new DEVICE_INFO();
            _info.ID = ds.Tables[0].Rows[i][0].ToString();                  // [0]: 设备ID
            _info.Class = ds.Tables[0].Rows[i][2].ToString();               // [2]: 设备类型
            
            float _tmp; 
            float.TryParse(ds.Tables[0].Rows[i][4].ToString(), out _tmp);   // [4]: 设备位置
            _info.Position = _tmp * 1000;

            _info.Area = ds.Tables[0].Rows[i][5].ToString();                // [5]: 设备区域
            _info.Name = ds.Tables[0].Rows[i][9].ToString();                // [9]: 备注信息

            DeviceInfoList.Add(_info);

        }

        return 0;
    }

    DataSet LoadXLS()
    {
        FileStream fs = File.Open(filepath, FileMode.Open, FileAccess.Read);
        IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(fs);

        return reader.AsDataSet();
    }


    

}
