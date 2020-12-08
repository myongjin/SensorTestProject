using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PressureMeasure : MonoBehaviour
{
    private bool init = false;
    public bool getValue = false;

    public byte sensitivity = 100;
    private int preSensitivity = 0;



    public byte[] test;


    [DllImport("PressurePad")]
    public static extern int Test(int a, int b);
    [DllImport("PressurePad")]
    public static extern bool InitDevice();

    [DllImport("PressurePad")]
    public static extern bool SetSensitivity(byte value);

    [DllImport("PressurePad")]
    public static extern byte[] GetPressureArray();

    [DllImport("PressurePad")]
    public static extern void CloseDevice();


    // Start is called before the first frame update
    void Start()
    {
        if (InitDevice())
        {
            Debug.Log("Pressure pad was initialised successfully");
            Debug.Log(Test(2, 3));
            init = true;
            preSensitivity = sensitivity;
            SetSensitivity(sensitivity);
        }
        else
        {
            Debug.Log("Failed to initialise the Pressure pad");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Update sensitivity when it changed
        if(preSensitivity!=sensitivity)
        {
            SetSensitivity(sensitivity);
        }

        if (init && getValue)
        {
            test = GetPressureArray();
            //Debug.Log(test);
            getValue = false;
        }
        

        preSensitivity = sensitivity;
    }
}
