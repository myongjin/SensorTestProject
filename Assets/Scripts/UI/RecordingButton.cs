using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordingButton : MonoBehaviour
{

    [SerializeField]
    private bool isClicked = false;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            ToggleButton();
        }
    }


    public void ToggleButton()
    {
        isClicked = !isClicked;
        if (isClicked)
        {
            image.color = Color.red;
            text.text = "Stop Recording";
            text.color = Color.white;
        }
        else
        {
            image.color = Color.white;
            text.text = "Record";
            text.color = Color.black;
        }

    }


}
