using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSelectAndSender : MonoBehaviour {

    [SerializeField]
    private CustomMessages.TestMessageID testMessageId;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CustomMessages.Instance.SendTransform(testMessageId, transform.localPosition, transform.localRotation);
    }
}
