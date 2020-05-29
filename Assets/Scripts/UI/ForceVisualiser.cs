using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceVisualiser : MonoBehaviour
{

    private int NbofForce;
    [SerializeField]
    private Image foregroundImage1;
    [SerializeField]
    private Image foregroundImage2;

    [SerializeField]
    private InputField forceText1;
    [SerializeField]
    private InputField forceText2;
    [SerializeField]
    private float forceMax = 10;

    public LiveStream liveStream;

    public float force1;
    public float force2;

    // Use this for initialization
    private void Start()
    {
        liveStream = GetComponent<LiveStream>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        // Get force
        force1 = liveStream.forcePPS1;
        force2 = liveStream.forcePPS2;

        // Set force text
        forceText1.text = force1.ToString("F2") + " N";
        forceText2.text = force2.ToString("F2") + " N";

        // Set force bar
        foregroundImage1.fillAmount = force1 / forceMax;
        foregroundImage2.fillAmount = force2 / forceMax;
    }
}