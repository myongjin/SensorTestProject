using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;

public class SaveDataForMLBreast : MonoBehaviour
{
    public bool isRecording = false;
    public bool isReplaying = false;
    public bool isPasuing = false;
    public bool isRepeat = false;

    private bool csvSave = false;
    private bool setWritefile = false;
    private bool readFile = false;



    public InputField recordFileName;
    public string recordText;
    public string replayText;
    public int innerFrame = 0;
    public int replayFrame = 0;
    public int replaySlower = 1;
    
    private int localFrame = 0;

  

    //for replay
    private string[] lines;
    private int indexRead = 0;

    private Vector3[] readVector;
    public GameObject[] fingerObj;



    //Get force and pos
    public LiveStream liveStream;

    //Get pressure
    public PressureMeasure pressureData;

    private StreamWriter outputFile;

    private int nbOfFinger;

    private float startTime = 0.0f;

    //predict data
    public bool isPredicting = false;
    public string predictText;
    private string[] plines;
    public GameObject predictedFinger;


    // Start is called before the first frame update
    void Start()
    {
        liveStream = GetComponent<LiveStream>();

        //set the number of finger tip
        nbOfFinger = fingerObj.Length;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Hot key
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isRecording = !isRecording;
        }


        //when replay is on, read text file
        if (isReplaying)
        {
            isRecording = false;
            //if no file has been read
            if (!readFile)
            {
                //read file
                if (csvSave)
                {
                    lines = ReadCSVFile(replayText);
                }
                else
                {
                    lines = ReadTextFile(replayText);
                }
                
                if(isPredicting)
                {
                    if (csvSave)
                    {
                        plines = ReadCSVFile(predictText);
                    }
                    else
                    {
                        plines = ReadTextFile(predictText);
                    }
                }
            }
        }

        //record
        //make a file
        if (isRecording && !setWritefile)
        {
            //make a text file if there is the same file, then delete it and create new one
            recordText = recordFileName.text;
            SetCSVFile(recordText + " " + System.DateTime.Now.ToString("dd-MM-yyyy H-mm"));
            startTime = Time.realtimeSinceStartup;
        }

        //write down data
        if (isRecording && setWritefile)
        {
            WriteDownData();
        }

        //when recording is stopped and there is a file to save
        if (!isRecording && setWritefile)
        {
            Debug.Log("Write complete");
            if (setWritefile)
            {
                outputFile.Close();
                setWritefile = false;
            }

            //Set replay text
            replayText = recordText;

            //Set new recording text
            //recordText = System.DateTime.Now.ToString("dd-MM-yyyy H-mm");
        }



        //replay
        //once reading flag is on and there is a file to read
        if (isReplaying && readFile)
        {
            if (replayFrame < lines.Length)
            {
                ReplayData();
                //to Next frame
                if (!isPasuing)
                {
                    innerFrame++;
                    replayFrame = (int)(Math.Truncate((double)(innerFrame / replaySlower)));
                }
            }
            else
            {
                innerFrame = 0;
                replayFrame = 0;
                isReplaying = false;
                if (isRepeat)
                {
                    isReplaying = true;
                }
            }

        }
    }

    private void WriteDownData()
    {
        string data = "";
        StringBuilder sb = new StringBuilder();
        //write time
        
        data = (Time.realtimeSinceStartup - startTime).ToString();
        sb.Append(data).Append(",");
        data = sb.ToString();
        sb.Clear();

        for(int i=0;i<nbOfFinger;i++)
        {
            float force = 0;
            if(i==0)
            {
                force = liveStream.forcePPS1;
            }
            if(i==1)
            {
                force = liveStream.forcePPS2;
            }

            if (i == 2)
            {
                force = liveStream.forcePPS3;
            }

            if (i == 3)
            {
                force = liveStream.forcePPS4;
            }
            data = AppendPosOriForce(data, fingerObj[i], force);
        }
        //write pos and force of finger1
        //data = AppendPosOriForce(data, fingerObj[0], liveStream.forcePPS1);
        ////write pos and force of finger2
        //data = AppendPosOriForce(data, fingerObj[1], liveStream.forcePPS2);
        //wirte pressure 
        
        sb.Append(data);

        float[,] pressure= pressureData.Pressure2Array;
        for (int i=0;i< pressure.GetLength(0);i++)
        {
            for(int j=0;j<pressure.GetLength(1);j++)
            {
                sb.Append(pressure[i,j]).Append(",");
            }
        }
        data = sb.ToString();

        //Write down
        outputFile.WriteLine(data);
    }


    private void ReplayData()
    {
        Vector3 pos;
        Quaternion ori;
        float[] forces = new float[nbOfFinger];

        for (int j = 0; j < nbOfFinger; j++)
        {
            

            if (csvSave)
            {
                var pt = lines[replayFrame].Split(","[0]); // gets 3 parts of the vector as separated strings
                var x = float.Parse(pt[1 + 8 * j]);
                var y = float.Parse(pt[2 + 8 * j]);
                var z = float.Parse(pt[3 + 8 * j]);
                pos = new Vector3(x, y, z);
                //Debug.Log(pos);

                x = float.Parse(pt[4 + 8 * j]);
                y = float.Parse(pt[5 + 8 * j]);
                z = float.Parse(pt[6 + 8 * j]);
                var w = float.Parse(pt[7 + 8 * j]);
                ori = new Quaternion(x, y, z, w);
                //Debug.Log(ori);

                x = float.Parse(pt[8 + 8 * j]);
                forces[j] = x;
            }
            else
            {
                var pt = lines[replayFrame].Split(" "[0]); // gets 3 parts of the vector as separated strings
                var x = float.Parse(pt[1 + 8 * j]);
                var y = float.Parse(pt[2 + 8 * j]);
                var z = float.Parse(pt[3 + 8 * j]);
                pos = new Vector3(x, y, z);
                //Debug.Log(pos);

                x = float.Parse(pt[4 + 8 * j]);
                y = float.Parse(pt[5 + 8 * j]);
                z = float.Parse(pt[6 + 8 * j]);
                var w = float.Parse(pt[7 + 8 * j]);
                ori = new Quaternion(x, y, z, w);
                //Debug.Log(ori);

                x = float.Parse(pt[8 + 8 * j]);
                forces[j] = x;
            }
            

            //Set position and orientation
            //Debug.Log(forces[j]);
            fingerObj[j].transform.position = pos;
            fingerObj[j].transform.rotation = ori;
        }

        liveStream.forcePPS1 = forces[0];
        liveStream.forcePPS2 = forces[1];

        var pt2 = lines[replayFrame].Split(" "[0]); // pressure
        int localIndex = 0;
        float[,] pressure = pressureData.Pressure2Array;
        for (int i = 0; i < pressure.GetLength(0); i++)
        {
            for (int j = 0; j < pressure.GetLength(1); j++)
            {
                var x = float.Parse(pt2[localIndex++ + 17]);
                //Debug.Log(x);
                pressure[i, j] = (float)x;
            }
        }

        if(isPredicting)
        {
            var pt3 = plines[replayFrame].Split(" "[0]);
            Vector3 position = new Vector3(0, 0, 0);
            Quaternion rot = new Quaternion(0, 0, 0, 0);
            for (int i=0;i<3;i++)
            {
                var x = float.Parse(pt3[i]);
                position[i] = x;
            }

            for (int i = 0; i < 4; i++)
            {
                var x = float.Parse(pt3[i] + 3);
                rot[i] = x;
            }
            predictedFinger.transform.position = position;
            predictedFinger.transform.rotation = rot;
        }
        


    }
    private void SetTextFile(string name)
    {
        //make a text file if there is the same file, then delete it and create new one
        FileStream fs = new FileStream(@"Data/" +  name + ".txt", FileMode.Create);


        outputFile = new StreamWriter(fs);
        Debug.Log("Make a text file for recording");

        //Write down head info.
        //WriteDownHeadInfo();

        setWritefile = true;
    }

    private void SetCSVFile(string name)
    {
        //make a text file if there is the same file, then delete it and create new one
        FileStream fs = new FileStream(@"Data/" + name + ".csv", FileMode.Create);


        outputFile = new StreamWriter(fs);
        Debug.Log("Make a text file for recording");

        //Write down head info.
        //WriteDownHeadInfo();

        setWritefile = true;
        csvSave = true;
    }

    private string[] ReadTextFile(string name)
    {
        readFile = true;
        Debug.Log("Read file");

        return File.ReadAllLines(@"Data/" + name + ".txt");
    }

    private string[] ReadCSVFile(string name)
    {
        readFile = true;
        Debug.Log("Read file");

        return File.ReadAllLines(@"Data/" + name + ".csv");
    }

    public void ToggleRecording()
    {
        isRecording = !isRecording;
    }

    public void ToggleReplaying()
    {
        isReplaying = !isReplaying;
    }

    private void OnDestroy()
    {
        Debug.Log("Write complete");
        if (setWritefile)
        {
            outputFile.Close();
        }

    }


    public string SerializeVector3Array(Vector3[] aVectors)
    {
        StringBuilder sb = new StringBuilder();
        foreach (Vector3 v in aVectors)
        {
            sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append("\n");
        }
        if (sb.Length > 0) // remove last "|"
            sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }


    public string AppendPosOriForce(string _data, GameObject obj, float force)
    {
        StringBuilder sb = new StringBuilder();
        string data = _data;

        //position
        data = SerializeVector3(data, obj.transform.position);

        //orientation
        data = SerializeVector3(data, obj.transform.rotation);

        //Force
        sb.Append(data).Append(force).Append(",");


        return sb.ToString();
    }

    public string AppendPos(string _data, GameObject obj)
    {
        StringBuilder sb = new StringBuilder();
        string data = _data;

        //position
        data = SerializeVector3(data, obj.transform.position);


        return sb.ToString();
    }

    public string AppendOri(string _data, GameObject obj)
    {
        StringBuilder sb = new StringBuilder();
        string data = _data;

        //orientation
        data = SerializeVector3(data, obj.transform.eulerAngles);

        return sb.ToString();
    }


    private static string SerializeVector3(Vector3 v)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z);
        return sb.ToString();
    }

    private string SerializeVector3(string data, Vector3 v)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(data);
        if (csvSave)
        {
            sb.Append(v.x).Append(",").Append(v.y).Append(",").Append(v.z).Append(",");
        }
        else
        {
            sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append(" ");
        }
        
        return sb.ToString();
    }


    private string SerializeVector3(string data, Quaternion v)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(data);
        if (csvSave)
        {
            sb.Append(v.x).Append(",").Append(v.y).Append(",").Append(v.z).Append(",").Append(v.w).Append(",");
        }
        else
        {
            sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append(" ").Append(v.w).Append(" ");
        }

        return sb.ToString();
    }

    public void WriteDownLine(string data)
    {
        outputFile.WriteLine(data);
    }
}
