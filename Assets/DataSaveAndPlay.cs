using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

//This is for VE project

public class DataSaveAndPlay : MonoBehaviour {

    public GameManager gameManager;
    public bool isRecording = false;
    public bool isReplaying = false;
    public bool isPasuing = false;
    public bool isRepeat = false;
    private bool setWritefile = false;
    private bool readFile = false;

    public string recordText;
    public string replayText;
    public int replayFrame =0;
    private int localFrame = 0;

    //for replay
    private string[] lines;
    private int indexRead = 0;


    private Vector3[] readVector;
    public GameObject[] fingerObj;

    public LiveStream liveStream;


    private StreamWriter outputFile;
    
    private int nbOfFinger;

    private float startTime =0.0f;
    


    // Use this for initialization
    void Start () {

        liveStream = GetComponent<LiveStream>();

        //set the number of finger tip
        nbOfFinger = fingerObj.Length;
        //Debug.Log(nbOfFinger);

        //recordText = System.DateTime.Now.ToString("dd-MM-yyyy H-mm");
    }
	


	// Update is called once per frame
	void LateUpdate () {
        //Hot key
        if(Input.GetKeyDown(KeyCode.F1))
        {
            isRecording = !isRecording;
        }


        //when replay is on, read text file
        if(isReplaying)
        {
            isRecording = false;
            //if no file has been read
            if(!readFile)
            {
                //read file
                ReadTextFile(replayText);
            } 
        }

        //record
        //make a file
        if(isRecording && !setWritefile)
        {
            //make a text file if there is the same file, then delete it and create new one
            SetTextFile(recordText + System.DateTime.Now.ToString("dd-MM-yyyy H-mm"));
            startTime = Time.realtimeSinceStartup;
        }

        //write down data
        if(isRecording && setWritefile)
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
            //Read Head Pose and Dialation first
            if(replayFrame == 0)
            {
                ReadHeadandSet();
                replayFrame++;
            }
            else if (replayFrame < lines.Length)
            {
                ReplayData();
                //to Next frame
                if(!isPasuing)
                {
                    replayFrame++;
                }    
            }
            else
            {
                replayFrame = 0;
                isReplaying = false;
                if(isRepeat)
                {
                    isReplaying = true;
                }
            }

        }
    }

    private void WriteDownData()
    {
        string data = "";

 
        data = (Time.realtimeSinceStartup- startTime).ToString();
        data = AppendPosOriForce(data, fingerObj[0], liveStream.forcePPS1);
        data = AppendPosOriForce(data, fingerObj[1], liveStream.forcePPS2);


        //Write down
        outputFile.WriteLine(data);
    }

    private void ReadHeadandSet()
    {
        var pt = lines[replayFrame].Split(" "[0]); // gets 3 parts of the vector as separated strings
        var head = int.Parse(pt[0]);
        var cervix = int.Parse(pt[1]);
        var angle = int.Parse(pt[2]);

        gameManager.SetModelInfo(head, cervix, angle);
    }

    private void ReplayData()
    {
        Vector3 pos;
        Quaternion ori;
        float[] forces = new float[nbOfFinger];

        for (int j = 0; j < nbOfFinger; j++)
        {
            var pt = lines[replayFrame].Split(" "[0]); // gets 3 parts of the vector as separated strings
            var x = float.Parse(pt[0 + 8 * j]);
            var y = float.Parse(pt[1 + 8 * j]);
            var z = float.Parse(pt[2 + 8 * j]);
            pos = new Vector3(x, y, z);
            //Debug.Log(pos);

            x = float.Parse(pt[3 + 8 * j]);
            y = float.Parse(pt[4 + 8 * j]);
            z = float.Parse(pt[5 + 8 * j]);
            var w = float.Parse(pt[6 + 8 * j]);
            ori = new Quaternion(x, y, z, w);
            //Debug.Log(ori);

            x = float.Parse(pt[7 + 8 * j]);
            forces[j] = x;

            //Set position and orientation
            //Debug.Log(forces[j]);
            fingerObj[j].transform.position = pos;
            fingerObj[j].transform.rotation = ori;

            //increase local frame
        }

        liveStream.forcePPS1 = forces[0];
        liveStream.forcePPS2 = forces[1];

    }
    private void SetTextFile(string name)
    {
        //make a text file if there is the same file, then delete it and create new one

        int examNumber = gameManager.exam;
        FileStream fs = new FileStream(@"Data/" + "Exam " + examNumber + "_" + name + ".txt", FileMode.Create);


        outputFile = new StreamWriter(fs);
        Debug.Log("Make a text file for recording");

        //Write down head info.
        //WriteDownHeadInfo();

        setWritefile = true;
    }

    private void ReadTextFile(string name)
    {
        lines = File.ReadAllLines(@"Data/" + name + ".txt");
        readFile = true;
        Debug.Log("Read file");
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
        if(setWritefile)
        {
            outputFile.Close();
        }
        
    }

    private void WriteDownHeadInfo()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(gameManager.exam);
        sb.Append(gameManager.headStation).Append(" ").Append(gameManager.cervix).Append(" ").Append(gameManager.angleIndex);
        outputFile.WriteLine(sb.ToString());
    }

    public static string SerializeVector3Array(Vector3[] aVectors)
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


    public static string AppendPosOriForce(string _data, GameObject obj, float force)
    {
        StringBuilder sb = new StringBuilder();
        string data = _data;

        //position
        data = SerializeVector3(data, obj.transform.position);
        
        //orientation
        data = SerializeVector3(data, obj.transform.rotation);

        //Force
        sb.Append(data).Append(force).Append(" ");


        return sb.ToString();
    }

    public static string AppendPos(string _data, GameObject obj)
    {
        StringBuilder sb = new StringBuilder();
        string data = _data;

        //position
        data = SerializeVector3(data, obj.transform.position);


        return sb.ToString();
    }

    public static string AppendOri(string _data, GameObject obj)
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

    private static string SerializeVector3(string data, Vector3 v)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(data);
        sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append(" ");
        return sb.ToString();
    }

    private static string SerializeVector3(string data, Quaternion v)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(data);
        sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append(" ").Append(v.w).Append(" ");
        return sb.ToString();
    }

    public void WriteDownLine(string data)
    {
        outputFile.WriteLine(data);
    }
}
