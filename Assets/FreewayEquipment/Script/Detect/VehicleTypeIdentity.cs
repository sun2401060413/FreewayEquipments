using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTypeIdentity : MonoBehaviour
{
    [Header("尺寸类型:0-小型,1-中型,2-大型,3-特大型")]
    public int SizeType = 0;        // 尺寸类型; 0-小型，1-中型，2-大型，3-特大型
    [Header("运输类型:0-客车,1-货车,3-其他")]
    public int TransType = 0;       // 运输类型: 0-客车，1-货车，2-其他
}
