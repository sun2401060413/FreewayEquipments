using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sz.tools;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class LampControl : _baseCtrlCodeControl
{
    public Texture2D t_lighton, t_lightoff;
    public Material m_lighton, m_lightoff;

    private Transform t_light;
    private MeshRenderer meshrd;

    protected override void ExtendStart()
    {
        t_light = CodeEditTools.GetChildTransformByName(transform, "SpotLight");
        meshrd = transform.GetChild(0).GetComponent<MeshRenderer>();
        
    }

    protected override int Process()
    {
        switch (CtrlCode)
        {
            case 0:
                Disable();
                return 0;

            case 1:
                Enable();
                return 1;

            default:
                return -1;

        }
    }

    protected void Enable()
    {
        t_light.gameObject.SetActive(true);
        meshrd.materials[3].CopyPropertiesFromMaterial(m_lighton);
        meshrd.materials[3].EnableKeyword("_EMISSION");
        meshrd.materials[3].SetTexture("_EmissionMap", t_lighton);
        //Status = 1;
    }

    protected void Disable()
    {
        t_light.gameObject.SetActive(false);
        meshrd.materials[3].CopyPropertiesFromMaterial(m_lightoff);
        meshrd.materials[3].DisableKeyword("_EMISSION");
        //meshrd.materials[3].SetTexture("_EmissionMap", t_lightoff);
        //Status = 0;
    }


}
