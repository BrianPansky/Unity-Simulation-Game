using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerInputs : MonoBehaviour
{


    enactionScript theEnactionScript;






    //      mouse look stuff:
    public float mouseSpeed = 290f;
    public Transform playerBody;
    float xRotation = 0f;
    //need the "playerClickInteraction" script 
    //(in order to check if we are in a menu or not)
    public playerClickInteraction thePlayerClickInteractionScript;



    float verticalCameraRotation = 0f;





    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (theEnactionScript == null)
        {
            theEnactionScript = this.gameObject.GetComponent<enactionScript>();
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
        theEnactionScript.x = Input.GetAxis("Horizontal");
        theEnactionScript.z = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            //Debug.Log("Input.GetButtonDown jump:  " + Input.GetButtonDown("Jump"));
            theEnactionScript.jump = true;

            //Debug.Log("theEnactionScript.jump:  " + theEnactionScript.jump);
        }
    }

    void mouseUpdate()
    {
        //Debug.Log("...................INPUTS...................");
        theEnactionScript.yawInput = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        theEnactionScript.pitchInput = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
    }

}
