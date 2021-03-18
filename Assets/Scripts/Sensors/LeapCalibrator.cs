using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapCalibrator : MonoBehaviour
{
    public GameObject target;
    public GameObject initPos;
    public GameObject fingerTip;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalibrateLeap()
    {
        Vector3 pos = initPos.transform.position;
        Vector3 posTip = fingerTip.transform.position;


        Vector3 difference = pos - posTip;
        target.transform.position = target.transform.position + difference;


        //Quaternion diffRot = Quaternion.Euler(initPos.transform.rotation.ToEuler() - fingerTip.transform.rotation.ToEuler());
        //target.transform.rotation *= diffRot;
    }
}
