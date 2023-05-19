using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using sz.tools;
using System;

[RequireComponent(typeof(sz.device.DeviceTypeIdentity))]
public class VMSControl : MonoBehaviour
{
    protected virtual void extendStart() { }
    protected virtual void extendUpdate() { }

    public bool Enable = true;             // 是否启动
    public string display_text = "";        // 显示文字
    //[Header("文字色彩码:0-红色;1-黄色;2-绿色;3-白色;4-任意")]
    //[Range(0, 4)]
    public string color_code = "green";              // 显示文字色彩
    [Header("字体大小范围20-120")]
    [Range(20, 120)]
    public int font_size = 50;              // 字体大小

    private Transform obj_text;
    private TextMeshPro component_text;

    private bool Status = true;
    private string text_displaying = "";
    private string color_displaying = "green";
    private int size_displaying = 50;

    // Start is called before the first frame update
    void Start()
    {
        obj_text = CodeEditTools.GetChildTransformByName(transform, "DisplayText");
        component_text = obj_text.GetComponentInChildren<TextMeshPro>();

        // 初始化文字颜色[绿色]
        component_text.color = new Color(0, 255, 0);

        // 继承类的扩展入口
        extendStart();
    }

    // Update is called once per frame
    void Update()
    {
        // VMS 开启
        if (Enable && !Status)
            EnableVMS();

        // VMS 关闭
        if (!Enable && Status)
            DisableVMS();

        // 设置文字内容
        if (display_text != text_displaying)
            SetDisplayText();

        // 设置文字色彩
        if (color_code != color_displaying)
            SetTextColor();

        // 设置文字尺寸
        if (font_size != size_displaying)
            SetTextSize();

        // 继承类的扩展入口
        extendUpdate();

    }

    void EnableVMS()
    {
        obj_text.gameObject.SetActive(true);
        Status = true;
    }

    void DisableVMS()
    {
        obj_text.gameObject.SetActive(false);
        Status = false;
    }

    void SetDisplayText()
    {
        //component_text.text = display_text;
        component_text.text = display_text.Replace("\\n", "\n");
        text_displaying = display_text;
    }


    void SetTextColor()
    {
        if (color_code == "red")
        {
            component_text.color = Color.red;        // 红色文字
            color_displaying = "red";
        }

        if (color_code == "yellow")
        {
            component_text.color = Color.yellow;      // 黄色文字
            color_displaying = "yellow";
        }

        if (color_code == "green")
        {
            component_text.color = Color.green;      // 黄色文字
            color_displaying = "green";
        }

        if (color_code == "white")
        {
            component_text.color = Color.white;         // 白色文字
            color_displaying = "white";
        }

        if (color_code.Contains("#"))
        {
            Color new_color;
            ColorUtility.TryParseHtmlString(color_code, out new_color);
            component_text.color = new_color;
            color_displaying = color_code;
        }

        

    }

    void SetTextSize()
    {
        component_text.fontSize = font_size;
        size_displaying = font_size;
    }
    
}
