using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelLocator : MonoBehaviour
{
    private int currentStation = 0;
    public Transform transformTarget;
    public Transform rotateTarget;

    public Transform[] references;

    private Vector3 refAngle;


    private void Start()
    {
        refAngle = rotateTarget.eulerAngles;
    }

    public void SelectTransform(int index)
    {
        if(index>0)
        {
            if (index-1 < references.Length)
            {
                currentStation = index - 1;
                transformTarget.position = references[index-1].position;
                transformTarget.rotation = references[index - 1].rotation;
            }
            else
            {
                Debug.Log("Index is out of range");
            }
        }
        
        
    }

    public void SelectAngle(int index)
    {
        
        rotateTarget.eulerAngles = references[currentStation].eulerAngles + new Vector3(0, 0, (float)(index - 1) * 5f);

        //Debug.Log("Index is out of range");
    }
}
