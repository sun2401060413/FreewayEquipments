using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sz.tools;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class SpinAlarmControl : _baseCtrlCodeControl
{
    private Transform t_Alarm;              // alarm base
    private Transform t_light_1;            // spot_light_1
    private Transform t_light_2;            // spot_light_2

    private Material m1;                    // normal shader
    private Material m2;                    // alarm shader


    // Start is called before the first frame update
    override protected void ExtendStart()
    {

        t_Alarm = CodeEditTools.GetChildTransformByName(transform, "Alarm");
        t_light_1 = CodeEditTools.GetChildTransformByName(t_Alarm, "SpotLight_1");
        t_light_2 = CodeEditTools.GetChildTransformByName(t_Alarm, "SpotLight_2");
 
        ani = t_Alarm.GetComponent<Animation>();

        m1 = new Material(Shader.Find("Standard"));
        m2 = new Material(Shader.Find("Unlit/Color"));
        m2.color = new Color(255, 0, 0);

    }

    protected override int Process()
    {
        switch(CtrlCode)
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
        // 设置警报渲染效果
        t_Alarm.gameObject.GetComponent<Renderer>().material = m2;
        t_Alarm.gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        // 开启spotlight
        t_light_1.gameObject.SetActive(true);
        t_light_2.gameObject.SetActive(true);
        // 开启旋转动画
        ani.Play("Spin360");
        // ani.Play();
        // 设置状态
        this.Status = 1;
    }

    protected void Disable()
    {
        // 恢复报警器渲染效果
        t_Alarm.gameObject.GetComponent<Renderer>().material = m1;
        t_Alarm.gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        // 关闭spotlight
        t_light_1.gameObject.SetActive(false);
        t_light_2.gameObject.SetActive(false);
        // 关闭旋转动画
        ani.Stop("Spin360");
        // ani.Stop();
        // 设置状态
        this.Status = 0;
    }

}
