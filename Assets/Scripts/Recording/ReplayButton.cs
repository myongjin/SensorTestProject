using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour {

    //
    public bool isClicked = false;
    public Image image;
    public Text text;
    public DataSaveAndPlay dataSaveAndPlay;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {  
        //if replay is done
        if(isClicked && !dataSaveAndPlay.isReplaying)
        {
            isClicked = !isClicked;
            setReplayImageandText();
        }
    }

    public void ToggleReplayButton()
    {
        isClicked = !isClicked;
        if (isClicked)
        {
            Debug.Log("Button is pushed");
            //if replay is not activated and the button is pressed
            if (!dataSaveAndPlay.isReplaying)
            {
                dataSaveAndPlay.isReplaying = true;
            }
        }else
        {
            Debug.Log("Button is released");
            //if replay is activated and the button is pressed again
            //stop replaying
            if (dataSaveAndPlay.isReplaying)
            {
                dataSaveAndPlay.isReplaying = false;
            }
        }

        setReplayImageandText();
    }

    public void TogglePauseButton()
    {
        dataSaveAndPlay.isPasuing = 
            !dataSaveAndPlay.isPasuing;
        setPauseImageandText();
    }

    void setReplayImageandText()
    {
        if (isClicked)
        {
            image.color = Color.green;
            text.text = "Stop Replay";
            text.color = Color.white;
        }
        else
        {
            image.color = Color.white;
            text.text = "Replay";
            text.color = Color.black;
        }
    }

    void setPauseImageandText()
    {
        if (dataSaveAndPlay.isPasuing)
        {
            image.color = Color.red;
        }
        else
        {
            image.color = Color.white;
        }
    }
}
