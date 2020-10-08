using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelector : MonoBehaviour
{
    public Camera[] cameras;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectView(int index)
    {
        if (index == 0)
        {
            Debug.Log("Nothing is selected");
        }
        else
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                if (i == index - 1)
                {
                    cameras[i].enabled=true;
                }
                else
                {
                    cameras[i].enabled=false;
                }
            }
        }
    }
}

