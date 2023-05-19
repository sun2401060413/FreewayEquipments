using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sz.tools;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class LightAlarmControl : _baseCtrlCodeControl
{
    private MeshRenderer meshlight;
    private Material originmat, mat_1, mat_2;


    // Start is called before the first frame update
    override protected void ExtendStart()
    {
        ani = gameObject.GetComponent<Animation>();
        meshlight = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();

        originmat = meshlight.material;

        mat_1 = new Material(Shader.Find("Standard"));
        mat_2 = new Material(Shader.Find("Unlit/Color"));

        Disable();
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
        ani.Play("shining");
        originmat = mat_2;
        Status = 1;
    }

    protected void Disable()
    {

        ani.Stop("shining");
        originmat = mat_1;
        Status = 0;

    }


}
