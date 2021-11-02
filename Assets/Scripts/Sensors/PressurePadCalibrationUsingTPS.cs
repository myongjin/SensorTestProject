using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;




//우선 압력판 초기값은 건들지 않는다 즉 오리지널은 놔두고
//여기서 재생산해서 사용한다



public class PressurePadCalibrationUsingTPS : MonoBehaviour
{
    

    //이게 켜지면 live calibration 시작
    public bool calibrationOn = false;

    private bool initPreGet = true;
    //calibration 되는 상수
    public float coefficient = 0f;
    
    //average size
    public int meanWindow = 1;
    public List<float> values = new List<float>();

    //force applied to the breast model (N)
    public float appliedForce = 0f;

    //variable to check the difference from the beginning
    private float initialPressureSum = 0f;

    //get pressure class
    public PressureMeasure pressureClass;

    //get force TPS class
    public LiveStream liveStream;

    public Image foregroundImage;
    public InputField forceText;



    //variables for save and load function
    private StreamWriter outputFile;

    public bool saveCalibratedValue = false;
    public string saveName = "Pad_TPS_Calibration";
    public bool loadCalibrationFlag = false;
    public string loadName = "Pad_TPS_Calibration";


    // Start is called before the first frame update
    void Start()
    {
        values.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (calibrationOn)
        {
            if (initPreGet)
            {
                //set initial pressure sum
                initialPressureSum = pressureClass.totalWeight;
                initPreGet = false;
            }

            values.Add(pressureClass.totalWeight - initialPressureSum);
            if (values.Count > meanWindow)
            {
                //처음 값 제거
                values.RemoveAt(0);
            }
            float sum = 0;
            for(int i=0;i<values.Count;i++)
            {
                sum += values[i];
            }
            if(values.Count > 0 && sum > 0)
            {
                float refForce = liveStream.forcePPS1;

                coefficient = refForce /( sum / values.Count);
            }
            
        }
        else
        {
            appliedForce = coefficient * (pressureClass.totalWeight - initialPressureSum);
            initPreGet = true;
        }

        if (saveCalibratedValue)
        {
            SaveCalibration();
            saveCalibratedValue = false;
        }

        if (loadCalibrationFlag)
        {
            initialPressureSum = pressureClass.totalWeight;
            LoadCalibration(loadName);
            loadCalibrationFlag = false;
        }
    }

    void SaveCalibration()
    {
        SetTextFile(saveName);
        StringBuilder sb = new StringBuilder();
        sb.Append(coefficient);
        String str;
        str = String.Format("{0:F10}", sb.ToString());
        outputFile.WriteLine(str);
        outputFile.Close();
        Debug.Log("Save Pad_TPS Calibration");
    }

    void LoadCalibration(string name)
    {
        string[] lines;
        lines = File.ReadAllLines(@"Data/" + name + ".txt");
        var pt = lines[0];
        var value = float.Parse(pt);

        coefficient = value;
        Debug.Log("Load Pad_TPS Calibration");
    }

    private void SetTextFile(string name)
    {
        //make a text file if there is the same file, then delete it and create new one
        FileStream fs = new FileStream(@"Data/" + name + ".txt", FileMode.Create);

        outputFile = new StreamWriter(fs);
    }

    private void LateUpdate()
    {
        // Set force text
        forceText.text = appliedForce.ToString("F2") + " N";

        // Set force bar
        float forceMax = 10;
        foregroundImage.fillAmount = appliedForce / forceMax;
    }

}
