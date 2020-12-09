using System;
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

    private GCHandle arrayHndl;
    public int[] PressureArray;

    public byte[] test;

    [DllImport("PressurePad")]
    public static extern int Test(int a, int b);

    [DllImport("PressurePad")]
    public static extern int[] TestArray(int a, int b);

    [DllImport("PressurePad")]
    public static extern void TestArrayV2(IntPtr IntArray);

    [DllImport("PressurePad")]
    public static extern bool InitDevice();

    [DllImport("PressurePad")]
    public static extern bool SetSensitivity(byte value);

    [DllImport("PressurePad")]
    public static extern bool GetPressureArray(IntPtr array);


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


        //Set variables
        PressureArray = new int[2288];
        arrayHndl = GCHandle.Alloc(PressureArray, GCHandleType.Pinned);
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
            if(!(GetPressureArray(arrayHndl.AddrOfPinnedObject())))
            {
                Debug.Log("Failed to get data from the pressure pad");
            }
            
            getValue = false;
        }
        

        preSensitivity = sensitivity;
    }
}
