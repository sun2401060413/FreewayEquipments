using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sz.tools;

public class AutoBarrierStopper : MonoBehaviour
{
    public int status;
    public GameObject stopper;

    private AutoBarrierControl controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = transform.GetComponent<AutoBarrierControl>();
        status = controller.Status;
    }

    // Update is called once per frame
    void Update()
    {
        Process();
    }

    private void Process()
    {
        status = controller.Status;

        if (status == 0)
            Enable();
        if (status == 1)
            Disable();
    }

    private void Enable()
    {
        stopper.SetActive(true);
    }

    private void Disable()
    {
        stopper.SetActive(false);
    }

}
