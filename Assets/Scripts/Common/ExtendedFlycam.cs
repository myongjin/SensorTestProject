﻿using UnityEngine;
using System.Collections;

public class ExtendedFlycam : MonoBehaviour
{

    /*
    EXTENDED FLYCAM
        Desi Quintans (CowfaceGames.com), 17 August 2012.
        Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
 
    LICENSE
        Free as in speech, and free as in beer.
 
    FEATURES
        WASD/Arrows:    Movement
                  Q:    Climb
                  E:    Drop
                      Shift:    Move faster
                    Control:    Move slower
                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
    */

    public float cameraSensitivity = 90;
    public float normalMoveSpeed = 10;
    public float fastMoveFactor = 3;
    public KeyCode dragBtn = KeyCode.Mouse1;

    public float rotationX = 0.0f;
    public float rotationY = 0.0f;
    

    private bool drag = false;
    void Start()
    {
        Screen.lockCursor = true;
        this.rotationX = transform.localRotation.eulerAngles.y;
        this.rotationY = transform.localRotation.eulerAngles.x;
    }

    void Update()
    {
        if (Input.GetKeyDown(this.dragBtn))
            this.drag = true;

        if (Input.GetKeyUp(this.dragBtn))
            this.drag = false;

         Screen.lockCursor =  this.drag;

       
        if (this.drag)
        {
            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            rotationY -= Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90);
        
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.right);
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKey(KeyCode.W)) { transform.position += transform.forward * normalMoveSpeed * fastMoveFactor * Time.deltaTime; }
            if (Input.GetKey(KeyCode.S)) { transform.position -= transform.forward * normalMoveSpeed * fastMoveFactor * Time.deltaTime; }
            if (Input.GetKey(KeyCode.D)) { transform.position += transform.right * normalMoveSpeed   * fastMoveFactor * Time.deltaTime; }
            if (Input.GetKey(KeyCode.A)) { transform.position -= transform.right * normalMoveSpeed   * fastMoveFactor * Time.deltaTime; }
            if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * normalMoveSpeed * fastMoveFactor * Time.deltaTime; }
            if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * normalMoveSpeed * fastMoveFactor * Time.deltaTime; }
        }
        else
        {
            if (Input.GetKey(KeyCode.W)) { transform.position += transform.forward * normalMoveSpeed * Time.deltaTime; }
            if (Input.GetKey(KeyCode.S)) { transform.position -= transform.forward * normalMoveSpeed * Time.deltaTime; }
            if (Input.GetKey(KeyCode.D)) { transform.position += transform.right * normalMoveSpeed * Time.deltaTime; }
            if (Input.GetKey(KeyCode.A)) { transform.position -= transform.right * normalMoveSpeed * Time.deltaTime;}
            if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * normalMoveSpeed * Time.deltaTime; }
            if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * normalMoveSpeed * Time.deltaTime; }
            
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
        }
    }
}