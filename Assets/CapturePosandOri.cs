using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;



public class CapturePosandOri : MonoBehaviour {

    
    public string fileName;
    public float posScale = 1.0f;
    public GameObject[] posCapture;
    public GameObject[] oriCapture;
    public GameObject[] direcCapture;


    public Vector3[] worldPos;
    public Vector3[] worldOri;
    public Vector3[] anglesToAxis;

    private StreamWriter outputFile;
    private bool setWritefile;

    public bool capture=false;

    private void SetTextFile(string name)
    {
        //make a text file if there is the same file, then delete it and create new one
        FileStream fs = new FileStream(@"Data/" + name + ".txt", FileMode.Create);
        outputFile = new StreamWriter(fs);
        Debug.Log("Make a text file for capture");
        setWritefile = true;
    }



    // Use this for initialization
    void Start () {
        //set text file
        SetTextFile(fileName);

        //set variables
      
        worldPos = new Vector3[posCapture.Length];
        worldOri = new Vector3[posCapture.Length];
        anglesToAxis = new Vector3[posCapture.Length];
    }
	
	// Update is called once per frame
	void Update () {
		//update variavles;

        for(int i=0;i<worldPos.Length;i++)
        {
            worldPos[i] = posCapture[i].transform.position * posScale;
            worldOri[i] = oriCapture[i].transform.eulerAngles;

            
            //Compute new Z axis by a rotation matrix
            Matrix4x4 rot = oriCapture[i].transform.worldToLocalMatrix;
            Vector3 zDirec = rot.MultiplyVector(new Vector3(0, 0, 1));
            zDirec = direcCapture[i].transform.position - posCapture[i].transform.position;
            zDirec = zDirec.normalized;
            Vector3 _zDirec = zDirec;

            //Debug.Log(zDirec);
            //Compute angle to each axis on a projected plan
            //x-y
            zDirec[2] = 0;
            zDirec = zDirec.normalized;
            anglesToAxis[i][0] = Vector3.Angle(zDirec, new Vector3(1, 0, 0));

            //y-z
            zDirec = _zDirec;
            zDirec[0] = 0;
            zDirec = zDirec.normalized;
            anglesToAxis[i][1] = Vector3.Angle(zDirec, new Vector3(0, 1, 0));

            //z-x
            zDirec = _zDirec;
            zDirec[1] = 0;
            zDirec = zDirec.normalized;
            anglesToAxis[i][2] = Vector3.Angle(zDirec, new Vector3(0, 0, 1));
        }

        //when capture is on
        if(capture && setWritefile)
        {
            capture = false;
            
            //write down every pos ans pri line by line
            for (int i=0;i<posCapture.Length;i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(i).Append(" ");
                string data = sb.ToString();
                data = SerializeVector3(data, worldPos[i]);
                data = SerializeVector3(data, worldOri[i]);
                data = SerializeVector3(data, anglesToAxis[i]);
                outputFile.WriteLine(data);
            }
            Debug.Log("Write a line");
        }
	}

    private static string SerializeVector3(string data, Vector3 v)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(data);
        sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append(" ");
        return sb.ToString();
    }


    void ToggleCapture()
    {
        capture = !capture;
    }

    private void OnDestroy()
    {
        Debug.Log("Write complete");
        if (setWritefile)
        {
            outputFile.Close();
        }

    }

}
