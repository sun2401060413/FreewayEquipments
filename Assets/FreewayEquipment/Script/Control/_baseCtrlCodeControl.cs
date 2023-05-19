using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 警报设备基类
/// </summary>
public class _baseCtrlCodeControl : MonoBehaviour
{
    public int CtrlCode;                // 控制码
    public int Status;                  // 控制状态

    protected Animation ani;            // 动画


    // 扩展功能虚函数
    protected virtual void ExtendStart() { }
    protected virtual void ExtendUpdate() { }

    protected virtual int Process() { return CtrlCode; }

    // Start is called before the first frame update
    void Start()
    {
        Status = -1;

        ExtendStart();
    }

    // Update is called once per frame
    void Update()
    {

        if (CtrlCode != Status)
            Status = Process();

        ExtendUpdate();
    }


    
}
