using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelLocator : MonoBehaviour
{
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
        if (index - 1 < references.Length)
        {
            rotateTarget.eulerAngles = refAngle + new Vector3(0, 0, (float)(index - 1) * 10f);
        }
        else
        {
            Debug.Log("Index is out of range");
        }

        if (index < references.Length)
        {
            
        }
        else
        {
            Debug.Log("Index is out of range");
        }

    }
}
