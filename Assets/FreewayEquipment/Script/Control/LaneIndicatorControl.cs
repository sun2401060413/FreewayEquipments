using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class LaneIndicatorControl : _baseCtrlCodeControl
{
    //private GameObject RedCross;            // 红叉
    //private GameObject GreenArrow;          // 绿箭头
    //private GameObject GreenArrowLeft;      // 左向绿箭头


    public Texture2D i_nolight, i_redcross, i_greenarrow, i_greenarrowleft;
    public MeshRenderer meshrd;

    public Material m_nolight, m_redcross, m_greenarrow, m_greenarrowleft;
    //private Material mat;


    // Start is called before the first frame update
    protected override void ExtendStart()
    {
        // 初始化指示灯[如果预制体结构发生变化，请根据目标构成重新设置GetChild(?)中的?,数字标识子物体在hierarchy的构成顺序];
        //RedCross = transform.GetChild(1).gameObject;
        //GreenArrow = transform.GetChild(2).gameObject;
        //GreenArrowLeft = transform.GetChild(3).gameObject;

        //meshrd.materials[4] = input_mat;    // 无法直接赋值
        meshrd.materials[4].CopyPropertiesFromMaterial(m_nolight);
        // Debug.Log("material:" + meshrd.material);
        // Debug.Log("enable" + meshrd.enabled);

        // Debug.Log("MAT NAME:"+mat.name);
        meshrd.materials[4].EnableKeyword("_EMISSION");
        meshrd.materials[4].SetTexture("_EmissionMap", i_nolight);

        // 初始化指示灯状态[关闭]
        //EnableGreenArrow();
    }

    // Update is called once per frame
    void Update()
    {
        // 关闭
        if (CtrlCode == -1 && Status != -1)
            DisableLight();

        // 开启红叉
        if (CtrlCode == 0 && Status != 0)
            EnableRedCross();

        // 开启绿箭头
        if (CtrlCode == 1 && Status != 1)
            EnableGreenArrow();

        // 开启左向绿箭头
        if (CtrlCode == 2 && Status != 2)
            EnableGreenArrowLeft();
        //meshrd.materials[4].EnableKeyword("_EMISSION");
        //meshrd.materials[4].SetTexture("_EmissionMap", i_greenarrow);
        //EnableRedCross();
    }

    protected override int Process()
    {
        switch (CtrlCode)
        {
            case 0:
                EnableRedCross();
                return 0;
            case 1:
                EnableGreenArrow();
                return 1;
            case 2:
                EnableGreenArrowLeft();
                return 2;
            default:
                DisableLight();
                return -1;
        }
    }



    // 关闭
    void DisableLight()
    {
        //RedCross.SetActive(false);
        //GreenArrow.SetActive(false);
        //GreenArrowLeft.SetActive(false);
        meshrd.materials[4].CopyPropertiesFromMaterial(m_nolight);
        meshrd.materials[4].EnableKeyword("_EMISSION");
        meshrd.materials[4].SetTexture("_EmissionMap", i_nolight);

        Status = -1;
    }

    void EnableRedCross()
    {
        //RedCross.SetActive(true);               // 红叉
        //GreenArrow.SetActive(false);
        //GreenArrowLeft.SetActive(false);
        //Debug.Log("~~~~~~~~~~~~~~" + mat.name);
        //mat.EnableKeyword("_EMISSION");
        //mat.SetTexture("_EmissionMap", i_redcross);
        meshrd.materials[4].CopyPropertiesFromMaterial(m_redcross);
        meshrd.materials[4].EnableKeyword("_EMISSION");
        meshrd.materials[4].SetTexture("_EmissionMap", i_redcross);

        Status = 0;
    }

    void EnableGreenArrow()
    {
        //RedCross.SetActive(false);
        //GreenArrow.SetActive(true);             // 绿箭头
        //GreenArrowLeft.SetActive(false);
        //mat.EnableKeyword("_EMISSION");
        //mat.SetTexture("_EmissionMap", i_greenarrow);
        meshrd.materials[4].CopyPropertiesFromMaterial(m_greenarrow);
        meshrd.materials[4].EnableKeyword("_EMISSION");
        meshrd.materials[4].SetTexture("_EmissionMap", i_greenarrow);

        Status = 1;
    }

    void EnableGreenArrowLeft()
    {
        //RedCross.SetActive(false);
        //GreenArrow.SetActive(false);
        //GreenArrowLeft.SetActive(true);         // 左向绿箭头
        //mat.EnableKeyword("_EMISSION");
        //mat.SetTexture("_EmissionMap", i_greenarrowleft);
        meshrd.materials[4].CopyPropertiesFromMaterial(m_greenarrowleft);
        meshrd.materials[4].EnableKeyword("_EMISSION");
        meshrd.materials[4].SetTexture("_EmissionMap", i_greenarrowleft);

        Status = 2;
    }

}
