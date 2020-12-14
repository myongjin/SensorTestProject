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


    //DLL Import
    [DllImport("PressurePad")]
    public static extern int Test(int a, int b);

    [DllImport("PressurePad")]
    public static extern int[] TestArray(int a, int b);

    [DllImport("PressurePad")]
    public static extern void TestArrayV2(IntPtr IntArray);

    [DllImport("PressurePad")]
    public static extern void TestArrayV3(IntPtr IntArray);

    [DllImport("PressurePad")]
    public static extern void TestArrayV4(IntPtr IntArray);

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
            Debug.Log("Pressure pad sensitivity: " + sensitivity);
        }
        else
        {
            Debug.Log("Failed to initialise the Pressure pad");
        }


        //Set variables
        PressureArray = new int[2288];
        arrayHndl = GCHandle.Alloc(PressureArray, GCHandleType.Pinned);


        //Debuging
        //RearrangePressure();
    }

    // Update is called once per frame
    void Update()
    {
        //Update sensitivity when it changed
        if(preSensitivity!=sensitivity)
        {
            SetSensitivity(sensitivity);
            Debug.Log("Pressure pad sensitivity: " + sensitivity);
        }

        //Get value
        if (init && getValue)
        {
            if (GetPressureArray(arrayHndl.AddrOfPinnedObject()))
            {
                Debug.Log("Success to get data from the pressure pad");
            }
            else
            {
                Debug.Log("Failed to get data from the pressure pad");
            }
        }
        

        preSensitivity = sensitivity;
    }

    void RearrangePressure()
    {
        int[] tempArray = PressureArray;
        int[] aRow = new int[44] {21, 22, 20, 23, 19, 24, 18, 25, 17, 26, 16, 27, 15, 28, 14, 29, 13, 30, 12, 31, 11, 32, 10, 33, 9, 34, 8, 35, 7, 36, 6, 37, 5, 38, 4, 39, 3, 40, 2, 41, 1, 42, 0, 43 };
        int[] aCol = new int[52];

        for(int i=0;i<26;i++)
        {
            aCol[i] = i * 2;
        }
        for(int i=26;i<52;i++)
        {
            aCol[i] = 53 - 2 * (i - 25);
        }

        for (int i = 0; i < 52; i++)
        {
            Debug.Log(aCol[i]);
        }


    }

   
}
