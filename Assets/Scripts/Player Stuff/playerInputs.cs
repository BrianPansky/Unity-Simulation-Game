using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerInputs : MonoBehaviour
{


    virtualGamepad theVirtualGamePad;






    //      mouse look stuff:
    public float mouseSpeed = 290f;
    public Transform playerBody;
    float xRotation = 0f;
    //need the "playerClickInteraction" script 
    //(in order to check if we are in a menu or not)
    //public playerClickInteraction thePlayerClickInteractionScript;



    float verticalCameraRotation = 0f;





    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (theVirtualGamePad == null)
        {
            theVirtualGamePad = this.gameObject.GetComponent<virtualGamepad>();
        }

        initializeMouseLookStuff();
    }

    void initializeMouseLookStuff()
    {
        //                          Cursor.lockState = CursorLockMode.Locked;

        //need the "playerClickInteraction" script 
        //(in order to check if we are in a menu or not)
        //but that script is on the PARENT object, so first need to get the parent object:
        //                          GameObject thisParent = this.transform.parent.gameObject;
        //now can get the script:
        //                          thePlayerClickInteractionScript = thisParent.GetComponent<playerClickInteraction>(); //this.gameObject.GetComponent("playerClickInteraction") as playerClickInteraction;

        //                          if (thePlayerClickInteractionScript == null)
        {
            //                          Debug.Log("wtffffffffffffffffffffffffffffffffffffffffffffff");
        }
    }



    // Update is called once per frame
    void Update()
    {

        mouseUpdate();

        keyboardUpdate();

    }

    void keyboardUpdate()
    {
        theVirtualGamePad.x = Input.GetAxis("Horizontal");
        theVirtualGamePad.z = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            //Debug.Log("Input.GetButtonDown jump:  " + Input.GetButtonDown("Jump"));
            //      theVirtualGamePad.jump = true;

            //Debug.Log("theVirtualGamePad.jump:  " + theVirtualGamePad.jump);
        }
    }

    void mouseUpdate()
    {
        //Debug.Log("...................INPUTS...................");
        theVirtualGamePad.yawInput = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        theVirtualGamePad.pitchInput = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        //          theVirtualGamePad.primary = Input.GetMouseButtonDown(0);
    }

}
