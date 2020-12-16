using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;
using System.Text;

public class PressureMeasure : MonoBehaviour
{

    private StreamWriter outputFile;
    private bool init = false;

    public bool debugFlag = false;
    private int debugPressure = 1;

    public bool getValue = false;

    public byte sensitivity = 100;
    private int preSensitivity = 0;

    private GCHandle arrayHndl;

    public float convertingUnit=0.0f;


    public int[] PressureArray;
    public int[,] Pressure2Array=new int[44,52];



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
        if(!debugFlag)
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
        }
        else
        {
            Debug.Log("Debug Mode");
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
        if (init && getValue && !debugFlag)
        {
            if (GetPressureArray(arrayHndl.AddrOfPinnedObject()))
            {
                RearrangePressure();
                Debug.Log("Success to get data from the pressure pad");
            }
            else
            {
                Debug.Log("Failed to get data from the pressure pad");
            }
        }

        if(debugFlag)
        {
            int[,] original = Pressure2Array.Clone() as int[,];

            for (int i = 0; i < 43; i++)
            {
                for (int j = 0; j < 52; j++)
                {
                    Pressure2Array[i+1, j] = original[i,j];
                }
            }

            if (Pressure2Array[0,0] >= 255)
            {
                debugPressure = -1;
            }
            if (Pressure2Array[0, 0] < 1)
            {
                debugPressure = 1;
            }

            for (int j = 0; j < 52; j++)
            {
                Pressure2Array[0, j] += debugPressure;
            }
        }


        preSensitivity = sensitivity;
    }


    float Calibration(float reference)
    {
        float singleArea = 1f;

        float value=reference/TotalArray();

        return value;
    }

    float TotalArray()
    {
        float value = 0;
        for(int i=0;i<PressureArray.Length;i++)
        {
            value += (float)PressureArray[i];
        }

        return value;
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


        int index = 0;

        

        for(int i=0;i<44;i++)
        {
            for(int j=0;j<52;j++)
            {
                Pressure2Array[aRow[i], aCol[j]] = PressureArray[index++];
            }
        }
    }

   
}
