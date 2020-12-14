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

    public int[] PressureArray;
    private StreamWriter outputFile;

    // Start is called before the first frame update
    void Start()
    {
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
            //get pressure
            PressureArray = GetPressureArray();
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
        //DWORD devNum = GetDevicesNum();

        return true;
    }

    //will be filled 
    private void SetSensitivity()
    {

    }

    //generate random pressure array for a test
    private int[] GetPressureArray()
    {
        int[] array = new int[2288];
        Random r = new Random();

        //assign random values for a test
        for(int i=0;i< array.Length; i++)
        {
            array[i] = Random.Range(0, 255);
        }
        return array;
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

}
