using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RockVR.Video;


public class CamPushingStream : MonoBehaviour
{
    private VideoCaptureCtrl myScript;
    public GameObject CtrlUI;
    private sz.ui.CamCtrlUI UIScript;
    private bool stay_status = false;
    public GameObject stream_cam;
    public GameObject main_cam;



    // Start is called before the first frame update
    void Start()
    {
        myScript = gameObject.GetComponent<VideoCaptureCtrl>();
        myScript.StartCapture();

        UIScript = CtrlUI.GetComponent<sz.ui.CamCtrlUI>();

        stay_status = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (UIScript.AccStatus && !stay_status)
        {
            stay_status = true;
            stream_cam.transform.position = main_cam.transform.position + Vector3.right*3;
        }

        if (!UIScript.AccStatus && stay_status)
        {
            stay_status = false;
        }


        if (!stay_status)
        {
            stream_cam.transform.position = main_cam.transform.position;
            stream_cam.transform.rotation = main_cam.transform.rotation;
        }

    }


    void OnDestroy()
    {
        myScript.StopCapture();
    }
}
