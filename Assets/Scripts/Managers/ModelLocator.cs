using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelLocator : MonoBehaviour
{
    public Transform target;

    public Transform[] references;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectTransform(int index)
    {
        if (index < references.Length)
        {
            target = references[index];
        }
        else
        {
            Debug.Log("Index is out of range");
        }
        
    }
}
