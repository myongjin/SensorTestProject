using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class PressureMeasure : MonoBehaviour
{
    public bool setText = false;
    public bool recording = false;
    public bool saveData = false;

    private bool initDevice = false;

    public string fileName;

    public string recordText;

    public int[] PressureArray ;
    public int[,] Pressure2Array ;

    private StreamWriter outputFile;
    private int debugPressure = 1;

    // Start is called before the first frame update
    void Start()
    {
        PressureArray = new int[2288];
        Pressure2Array = new int[44, 52];
        //Initilise device

        if (InitDevice())
            {
                Debug.Log("Device successfully loaded");
                initDevice = true;
            }
            else
            {
                Debug.Log("failed to load the device");
            }

        //Set sensitivity of sensor
        SetSensitivity();
    }

    // Update is called once per frame
    void Update()
    {
        if(initDevice)
        {
            //get pressure - two options
            //GetPressureArray();
            GetPressureArrayV2();

            //filter data - Jessica
            FilterData();

            //set text file
            if (setText && !recording)
            {
                SetTextFile(fileName + System.DateTime.Now.ToString(" dd-MM-yyyy H-mm"));
                Debug.Log("Text is created");
                recording = true;
            }


            //if text file exist and recording started
            if (setText && recording)
            {
                StringBuilder sb = new StringBuilder();

                //for example if you need to write down a vector
                //you need to put 

                //write down pressure
                for (int i = 0; i < PressureArray.Length; i++)
                {
                    sb.Append(PressureArray[i]).Append(",");
                    outputFile.WriteLine(sb.ToString());
                }
            }

            //save a text file here
            if (setText && recording && saveData)
            {
                outputFile.Close();
                Debug.Log("Data is saved");
                recording = false;
                setText = false;
                saveData = false;
            }
        }




    }

    private void SetTextFile(string name)
    {
        //make a text file if there is the same file, then delete it and create new one
        FileStream fs = new FileStream(@"Data/" + name + ".txt", FileMode.Create);
        outputFile = new StreamWriter(fs);
        Debug.Log("Make a text file for recording");
    }

    //you will need the below functions for you UI
    public void ToggleRecord()
    {
        recording = !recording;
    }

    public void ToggleWrite()
    {
        setText = !setText;
    }

    public void ToggleSave()
    {
        saveData = !saveData;
    }


    //device functions
    private bool InitDevice()
    {
        //For debug
        return true;
    }

    //will be filled 
    private void SetSensitivity()
    {

    }

    //generate random pressure array for a test
    private void GetPressureArray()
    { 
        Random r = new Random();

        //assign random values for a test
        for (int i = 0; i < PressureArray.Length; i++)
        {
            PressureArray[i] = Random.Range(0, 255);
        }

        RearrangePressure();


    }

    private void GetPressureArrayV2()
    {
        int[,] original = Pressure2Array.Clone() as int[,];
        for (int i = 0; i < 43; i++)
        {
            for (int j = 0; j < 52; j++)
            {
                Pressure2Array[i + 1, j] = original[i, j];
            }
        }

        if (Pressure2Array[0, 0] >= 255)
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

    private void TurnOffSensor()
    {

    }

    private void OnDisable()
    {
        if(initDevice)
        {
            TurnOffSensor();
        }
    }

    void FilterData()
    {
        //Jessica
    }

    //Convert 1 dimensional array to 2 dimensional array
    //if you need 2 dim array, use the below function
    void RearrangePressure()
    {
        int index = 0;

        for (int i = 0; i < 44; i++)
        {
            for (int j = 0; j < 52; j++)
            {
                Pressure2Array[i, j] = PressureArray[index++];
            }
        }
    }
}
