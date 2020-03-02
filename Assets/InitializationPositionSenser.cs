using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationPositionSenser : MonoBehaviour {

    public Vector3 callTestVector;
    public Vector3 capturedPosition;


	// Use this for initialization
	void Start () {
       

	}
	
	// Update is called once per frame
	void Update () {

        callTestVector = GameObject.Find("FingerTip").GetComponent<FingerTip>().position;

        if (Input.GetKeyDown(KeyCode.S))
        {
            capturedPosition = callTestVector;
            Debug.Log(capturedPosition);
        }

    }
}
