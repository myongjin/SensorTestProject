using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordDRE : MonoBehaviour
{
    public bool IsRecording = false;
  //  public RecordAnimation recordAnimation1;
 //   public RecordAnimation recordAnimation2;

    private RecordingButton recordingButton;

    // Use this for initialization
    void Start()
    {
        recordingButton = GetComponent<RecordingButton>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleRecord()
    {
        IsRecording = !IsRecording;

        // set button appearance
        //recordingButton.IsRecordingButton(IsRecording);

        // start record the finger movement
     //   recordAnimation1.record = IsRecording;
      //  recordAnimation2.record = IsRecording;
    }
}
