using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sz.tools;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class TrafficLightControl : _baseCtrlCodeControl
{
    
    //[Header("控制码:0-green,1-yellow,2-red,3-arrow")]
    //[Range(-1, 3)]
    //public int CtrlCode = -1;                       // 0-green, 1-yellow, 2-red, 3-arrow; -1-Notwork
    //public int LightStatus = -1;                    //


    public Texture2D i_green, i_yellow, i_red, i_arrow;
    public MeshRenderer mesh_green, mesh_yellow, mesh_red, mesh_arrow;

    private Transform t_green, t_yellow, t_red, t_arrow;
    private Material green, yellow, red, arrow;

    // Start is called before the first frame update
    protected override void ExtendStart()
    {
        t_green = CodeEditTools.GetChildTransformByName(transform, "Green");
        t_yellow = CodeEditTools.GetChildTransformByName(transform, "Yellow");
        t_red = CodeEditTools.GetChildTransformByName(transform, "Red");
        t_arrow = CodeEditTools.GetChildTransformByName(transform, "Green_arrow");

        // 获取MeshRenderer下的materials,[1]第二个为Material;
        green = mesh_green.materials[1];            // material 和 materials 不一样
        yellow = mesh_yellow.materials[1];
        red = mesh_red.materials[1];
        arrow = mesh_arrow.materials[1];

        Status = -1;
    }



    protected override int Process()
    {
        switch (CtrlCode)
        {
            case 0:
                EnableGreen();
                DisableYellow();
                DisableRed();
                DisableArrow();
                return 0;
            case 1:
                DisableGreen();
                EnableYellow();
                DisableRed();
                DisableArrow();
                return 1;
            case 2:
                DisableGreen();
                DisableYellow();
                EnableRed();
                DisableArrow();
                return 2;
            case 3:
                DisableGreen();
                DisableYellow();
                DisableRed();
                EnableArrow();
                return 3;
            default:
                DisableGreen();
                DisableYellow();
                DisableRed();
                DisableArrow();
                Status = -1;
                return -1;
        }
    }


    // Green Light
    void EnableGreen()
    {
        green.EnableKeyword("_EMISSION");
        green.SetTexture("_EmissionMap", i_green);
        Status = 0;
    }

    void DisableGreen()
    {
        green.DisableKeyword("_EMISSION");
        green.SetTexture("_EmissionMap", i_green);
    }

    // Yellow Light
    void EnableYellow()
    {
        yellow.EnableKeyword("_EMISSION");
        yellow.SetTexture("_EmissionMap", i_yellow);
        Status = 1;
    }

    void DisableYellow()
    {
        yellow.DisableKeyword("_EMISSION");
    }

    // Red Light
    void EnableRed()
    {
        red.EnableKeyword("_EMISSION");
        red.SetTexture("_EmissionMap", i_red);
        Status = 2;
    }

    void DisableRed()
    {
        red.DisableKeyword("_EMISSION");
    }

    // Arrow Light
    void EnableArrow()
    {
        arrow.EnableKeyword("_EMISSION");
        arrow.SetTexture("_EmissionMap", i_arrow);
        Status = 3;
    }

    void DisableArrow()
    {
        arrow.DisableKeyword("_EMISSION");
        //green.SetTexture("_EmissionMap", i_arrow);
    }
}
