using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PressureMeasure : MonoBehaviour
{
    private bool init = false;
    public bool getValue = false;

    public int sensitivity = 100;
    private int preSensitivity = 0;



    public int[] test;
    [DllImport("PressurePad")]
    public static extern bool InitDevice();

    [DllImport("PressurePad")]
    public static extern bool SetSensitivity(int value);

    [DllImport("PressurePad")]
    public static extern float[] GetPressureArray();

    [DllImport("PressurePad")]
    public static extern void CloseDevice();


    // Start is called before the first frame update
    void Start()
    {
        if (InitDevice())
        {
            Debug.Log("Pressure pad was initialised successfully");
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
        if(preSensitivity!=sensitivity)
        {
            SetSensitivity(sensitivity);
        }
        if (init && getValue)
        {
            //test = GetPressureArray();
            //Debug.Log(test);
            getValue = false;
        }
        

        preSensitivity = sensitivity;
    }
}
