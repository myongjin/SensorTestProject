using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    public GameObject target;
    public bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        //target=this.GetComponent<GameObject>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(!done)
        {
            target.SetActive(false);
            done = true;
        }
    }
}
