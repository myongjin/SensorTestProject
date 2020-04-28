using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHandTransformSender : MonoBehaviour {

    public int handSelector = 0;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(handSelector == 1)
        {
            CustomMessages.Instance.SendTransform(CustomMessages.TestMessageID.HandTransform, transform.localPosition, transform.localRotation);
        }
        else if(handSelector == 2)
        {
            CustomMessages.Instance.SendTransform(CustomMessages.TestMessageID.HandTransform2, transform.localPosition, transform.localRotation);
        }
        
    }
}
