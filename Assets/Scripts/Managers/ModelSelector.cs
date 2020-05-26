using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelector : MonoBehaviour
{
    public GameObject[] targets;


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectOne(int index)
    {
        if(index == 0)
        {
            Debug.Log("Nothing is selected");
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (i == index-1)
                {
                    targets[i].SetActive(true);
                }
                else
                {
                    targets[i].SetActive(false);
                }
            }
        }
        
    }
}
