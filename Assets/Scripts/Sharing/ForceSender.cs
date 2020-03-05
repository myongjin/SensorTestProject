using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

public class ForceSender : Singleton<ForceSender>
{
    LiveStream livestream;

    public float Force1;
    public float Force2;
    private float offset1 = 0;
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
        Force1 = livestream.forcePPS1 + offset1;
        Force2 = livestream.forcePPS2 + offset2;
        CustomMessages.Instance.SendForce(Force1);
        CustomMessages.Instance.SendForce(Force2);
    }

    public void ResetForce()
    {
        offset1 = -livestream.forcePPS1;
        offset2 = -livestream.forcePPS2;
    }
}
