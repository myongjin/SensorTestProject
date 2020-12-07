using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PressureMeasure : MonoBehaviour
{
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
            Debug.Log("Device was initialised successfully");
        }
        else
        {
            Debug.Log("Failed to initialise the device");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
