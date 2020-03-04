using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

public class ForceSender : Singleton<ForceSender>
{
    LiveStream livestream;

    public float Force;
    public float Force2;
    private float offset = 0;
    private float offset2 = 0;

    // Use this for initialization
    private void Start ()
    {
        // live stream exists on same component
        // So, we can get it easily like this
        livestream = GetComponent<LiveStream>();
		
	}
	
	// Update is called once per frame
    private void Update ()
    {
        Force = livestream.forcePPS1 + offset;
        Force2 = livestream.forcePPS2 + offset2;
        CustomMessages.Instance.SendForce(Force);
        CustomMessages.Instance.SendForce(Force2);
    }

    public void ResetForce()
    {
        offset = -livestream.forcePPS1;
        offset2 = -livestream.forcePPS2;
    }
}
