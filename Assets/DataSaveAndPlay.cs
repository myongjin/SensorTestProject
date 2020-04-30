using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;



public class DataSaveAndPlay : MonoBehaviour {

    public bool isRecording = false;
    public bool isReplaying = false;
    public bool isPasuing = false;
    public bool isRepeat = false;
    private bool setWritefile = false;
    private bool readFile = false;

    public string recordText;
    public string replayText;
    public int replayFrame;
    private int localFrame = 0;

    //for replay
    private string[] lines;
    private int indexRead = 0;


    private Vector3[] readVector;
    public GameObject[] fingerObj = new GameObject[2];
    public ForceSender forceSender;


    private StreamWriter outputFile;
    
    private int nbOfFinger;
    

    // Use this for initialization
    void Start () {
        
        //set the number of finger tip
        nbOfFinger = fingerObj.Length;
        //Debug.Log(nbOfFinger);

        recordText = System.DateTime.Now.ToString("f");
    }
	


	// Update is called once per frame
	void LateUpdate () {
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
            SetTextFile(recordText);
        }

        //write down data
        if(isRecording && setWritefile)
        {
            string data = "";

            //data = AppendPosOriForce(data, fingerObj[0], Time.frameCount);

            data = AppendPosOriForce(data, fingerObj[0], forceSender.Force1);
            data = AppendPosOriForce(data, fingerObj[1], forceSender.Force2);


            //Write down
            outputFile.WriteLine(data);
        }

        //when recording is stopped and there is a file to save
        if (!isRecording && setWritefile)
        {
            Debug.Log("Write complete");
            if (setWritefile)
            {
                outputFile.Close();
            }

            setWritefile = false;
        }



        //replay
        //once reading flag is on and there is a file to read
        if (isReplaying && readFile)
        {
            if (indexRead < lines.Length)
            {
                ReplayData();
                //to Next frame
                if(!isPasuing)
                {
                    indexRead++;
                }    
            }
            else
            {
                indexRead = 0;
                isReplaying = false;
                if(isRepeat)
                {
                    isReplaying = true;
                }
            }

        }
    }

    private void ReplayData()
    {
        Vector3 pos;
        Quaternion ori;
        float[] forces = new float[2];

        for (int j = 0; j < 2; j++)
        {
            var pt = lines[indexRead].Split(" "[0]); // gets 3 parts of the vector as separated strings
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

        forceSender.Force1 = forces[0];
        forceSender.Force2 = forces[1];

    }
    private void SetTextFile(string name)
    {
        //make a text file if there is the same file, then delete it and create new one
        FileStream fs = new FileStream(@"Data/" + name + ".txt", FileMode.Create);
        outputFile = new StreamWriter(fs);
        Debug.Log("Make a text file for recording");

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
