using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public class LoadPredictData : MonoBehaviour
{
    public bool isReplaying = false;
    public int replayFrame = 0;

    private bool readFile = false;

    public string fileName;

    //for replay
    private string[] lines;
    private int indexRead = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //when replay is on, read text file
        if (isReplaying)
        {
            //if no file has been read
            if (!readFile)
            {
                //read file
                ReadTextFile(fileName);
            }
        }

        //replay
        //once reading flag is on and there is a file to read
        //if (isReplaying && readFile)
        //{
        //    if (replayFrame < lines.Length)
        //    {
        //        ReplayData();
        //        //to Next frame
        //        replayFrame++;
        //    }
        //    else
        //    {
        //        replayFrame = 0;
        //        isReplaying = false;
        //    }
        //}
    }

    private void ReadTextFile(string name)
    {
        lines = File.ReadAllLines(@"Data/" + name + ".txt");
        readFile = true;
        Debug.Log("Read file");
    }
}
