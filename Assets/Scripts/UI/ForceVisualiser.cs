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

    private float force1;
    private float force2;

    // Use this for initialization
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Send force to client
        force1 = ForceSender.Instance.Force1;
        force2 = ForceSender.Instance.Force2;

        // Set force text
        forceText1.text = force1.ToString("F2") + " N";
        forceText2.text = force2.ToString("F2") + " N";

        // Set force bar
        foregroundImage1.fillAmount = force1 / forceMax;
        foregroundImage2.fillAmount = force2 / forceMax;
    }
}