using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class ModelAssign : MonoBehaviour
{
    private GameObject thisObject;
    public GameObject[] targets;

    //for text read
    public int refIndex =0;
    private bool readFile = false;
    private string[] lines;
    private StreamWriter outputFile;

    //buttons
    public bool startReading = false;
    public bool setTextFile = false;
    public bool doneFlag = false;
    public bool readingHeadPos = false;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        //Read ref pos and apply it
        ReadReferences("headPos");
        thisObject = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(startReading)
        {
            SetTextFile("headPos");
        }

        if (readingHeadPos && setTextFile)
        {
            if (refIndex < targets.Length)
            {
                Debug.Log(this.transform.position);
                Debug.Log(SerializeVector3("", this.transform.position));
                outputFile.WriteLine(SerializeVector3("", this.transform.position));
                Debug.Log(refIndex + " write complete");
                readingHeadPos = false;
                refIndex++;
            }

            if (refIndex >= targets.Length)
            {
                refIndex = 0;
                readingHeadPos = false;
                doneFlag = true;
            }

            if(doneFlag)
            {
                outputFile.Close();
                Debug.Log("Write Complete and Data saved");
            }
        }
        
    }
    private static string SerializeVector3(string data, Vector3 v)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(data);
        sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append(" ");
        return sb.ToString();
    }

    private void SetTextFile(string name)
    {
        //make a text file if there is the same file, then delete it and create new one
        FileStream fs = new FileStream(@"Data/" + name + ".txt", FileMode.Create);
        outputFile = new StreamWriter(fs);
        Debug.Log("Make a text file for head pos");

        setTextFile = true;
        startReading = false;
    }

    private void ReadReferences(string name)
    {
        lines = File.ReadAllLines(@"Data/" + name + ".txt");
        if (lines.Length > 0)
        {
            readFile = true;
            Debug.Log("Read head pos references");

            //set
            for (int i = 0; i < lines.Length; i++)
            {
                var pt = lines[i].Split(" "[0]);
                var x = float.Parse(pt[0]);
                var y = float.Parse(pt[1]);
                var z = float.Parse(pt[2]);
                targets[i].transform.position = new Vector3(x, y, z);
            }
            Debug.Log("Set " + lines.Length +" head poses");
        }
        else
        {
            Debug.Log("No head pos references");
        }

    }

    private void OnDestroy()
    {
        Debug.Log("Write complete");
        if (setTextFile)
        {
            outputFile.Close();
        }

    }
}
