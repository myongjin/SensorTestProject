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

    public bool getValue = true;

    public byte sensitivity = 100;
    private int preSensitivity = 0;

    public float totalWeight =0; //Kg
    public float maximumPressure = 0; //Kg/(cm^2)
    private Vector2Int maxIndex;

    public bool calibration = false;
    public float referenceWeightKg = 0;
    public float calibratedUnit = 1.0f;
    private float totalUnit = 0;
    private int numberOfCalibration=0;

    public int filterRange = 1;

    public bool saveCalibratedValue=false;
    public bool loadCalibrationFlag = false;
    private string[] lines;

    private GCHandle arrayHndl;


    public int[] PressureArray;
    public float[,] Pressure2Array=new float[44,52];



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
        maxIndex = new Vector2Int(0, 0);
        if (!debugFlag)
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
        //
        if(calibration)
        {
            if(numberOfCalibration==0)
            {
                calibratedUnit = Calibration(referenceWeightKg);
            }
            else
            {
                calibratedUnit += Calibration(referenceWeightKg);
            }
            if (numberOfCalibration < 2)
                numberOfCalibration++;
            calibratedUnit /= (float)(numberOfCalibration);
            Debug.Log(numberOfCalibration);
            calibration = false;
        }

        if(saveCalibratedValue)
        {
            SaveCalibration();
            saveCalibratedValue = false;
        }

        if(loadCalibrationFlag)
        {
            LoadCalibration();
            loadCalibrationFlag = false;
        }

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
                totalWeight = calibratedUnit * TotalArray();
                float singleArea = 30.0f * 36.0f / 2288.0f;
                maximumPressure = MaxPressureValue()/ singleArea;
                //Debug.Log("Success to get data from the pressure pad");
            }
            else
            {
                Debug.Log("Failed to get data from the pressure pad");
                getValue = false;
            }
        }

        if(debugFlag)
        {
            float[,] original = Pressure2Array.Clone() as float[,];

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

        FilterData();

        preSensitivity = sensitivity;
    }


    void FilterData()
    {
        //Jessica
        float[,] newPressure2Array = new float[44, 52];
        

        // iterate through all cells in Pressure2Array
        for (int a = 0; a < 44; a++)
        {
            for (int b = 0; b < 52; b++)
            {
                float summation = 0;
                int count = 0;

                //find average of cells within filterRange distance
                for (int i = a - filterRange; i <= a + filterRange; i++)
                {
                    for (int j = b - filterRange; j <= b + filterRange; j++)
                    {
                        // if within limits
                        if (i >= 0 & i < 44 & j >= 0 & j < 52)
                        {
                            //sum of the values surrounding the actual value
                            summation += Pressure2Array[i, j];
                            count += 1;
                        }
                    }
                }

                float average = summation / (float)count;
                newPressure2Array[a, b] = average;
            }
        }

        // overwrite Pressure2Array with filtered version
        Pressure2Array = newPressure2Array;
    }

    void SaveCalibration()
    {
        SetTextFile();
        StringBuilder sb = new StringBuilder();
        sb.Append(sensitivity).Append(" ").Append(calibratedUnit);
        String str;
        str = String.Format("{0:F10}", sb.ToString());
        outputFile.WriteLine(str);
        outputFile.Close();
        Debug.Log("Save Calibration");
    }

    void LoadCalibration()
    {
        lines = File.ReadAllLines(@"Data/Calibration.txt");
        var pt = lines[0].Split(" "[0]); // gets 3 parts of the vector as separated strings
        var sens = byte.Parse(pt[0]);
        var value = float.Parse(pt[1]);

        sensitivity = sens;
        calibratedUnit = value;

        Debug.Log("Load Calibration");
    }

    private void SetTextFile()
    {
        //make a text file if there is the same file, then delete it and create new one
        FileStream fs = new FileStream(@"Data/Calibration.txt", FileMode.Create);
        outputFile = new StreamWriter(fs);
        Debug.Log("Save calibration");
    }

    float Calibration(float reference)
    {
        return reference / TotalArray();
        
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

    float MaxPressureValue()
    {
        float value = -1000000000000;
        for (int i = 0; i < Pressure2Array.GetLength(0); i++)
            
        {
            for(int j=0;j<Pressure2Array.GetLength(1);j++)
            {
                if (Pressure2Array[i,j]>value)
                {
                    value = Pressure2Array[i, j];

                    maxIndex[0] = i;
                    maxIndex[1] = j;
                }
            }
            
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


        for (int j = 0; j < 52; j++)
            
        {
            for (int i = 0; i < 44; i++)
            {
                Pressure2Array[i, j] = (float)PressureArray[index++];
            }
        }

        float[,] original = Pressure2Array.Clone() as float[,];

        for (int i = 0; i < 44; i++)
        {
            for (int j = 0; j < 52; j++)
            {
                Pressure2Array[aRow[i], aCol[j]] = original[i, j]* calibratedUnit;
            }
        }
    }

   
}
