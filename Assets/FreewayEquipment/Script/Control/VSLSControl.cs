using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using sz.tools;

public class VSLSControl : _baseCtrlCodeControl
{
    //[Header("圆环显示状态:0-不显示,1-红色圆环")]
    //[Range(0, 1)]
    // private int RingDisplay;            // 圆环显示，1-红色圆环 
    // private int RingStatus;             // 圆环显示状态;
    //private int tempStatus;             // 临时状态

    //public int CtrlCode;
    //public int Status;

    private Transform obj_image;
    private Transform obj_text;
    private TextMeshPro testMesh;

    protected override void ExtendStart()
    {
        obj_text = CodeEditTools.GetChildTransformByName(transform, "DisplayText");
        obj_image = CodeEditTools.GetChildTransformByName(transform, "DisplayImage");
        testMesh = obj_text.GetChild(0).GetComponent<TextMeshPro>();
        //testMesh = obj_text.GetchGetComponent<TextMeshPro>();
    }

    protected override int Process()
    {
        if (CtrlCode > 0)
        {
            ShowOn();
            return CtrlCode;
        }

        if (CtrlCode == 0)
        {
            ShowOff();
            return 0;
        }

        return -1;
    }

    void ShowOn()
    {
        obj_image.gameObject.SetActive(true);
        obj_text.gameObject.SetActive(true);
        Debug.Log(CtrlCode.ToString());
        testMesh.text = CtrlCode.ToString();
        testMesh.color = Color.red;
        Status = CtrlCode;
    }

    void ShowOff()
    {
        obj_image.gameObject.SetActive(false);
        obj_text.gameObject.SetActive(false);
        Status = 0;
    }

}
