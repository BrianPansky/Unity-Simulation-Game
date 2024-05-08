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

        //isGrounded = isThisGrounded();

        //if (isGrounded && velocity.y < 0)
        {
            //velocity.y = -2f;
        }


        theEnactionScript.x = Input.GetAxis("Horizontal");
        theEnactionScript.z = Input.GetAxis("Vertical");

        //Vector3 move = transform.right * x + transform.forward * z;

        //controller.Move(move * speed * Time.deltaTime);


        if (Input.GetButtonDown("Jump"))
        {
            //Debug.Log("yes, pressed jump");

            //Debug.Log("theEnactionScript.jump:  " + theEnactionScript.jump);
            theEnactionScript.jump = true;

            //Debug.Log("theEnactionScript.jump:  " + theEnactionScript.jump);
        }

        //if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);
    }


    void mouseUpdate()
    {

        //theEnactionScript.lookingVector = rotateThisVectorByMouseInput(theEnactionScript.lookingVector).normalized;

        //Debug.Log("...................INPUTS...................");
        //Debug.Log("theEnactionScript.yawInput:  " + theEnactionScript.yawInput);
        theEnactionScript.yawInput = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        theEnactionScript.pitchInput = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        //ONLY can look around if we aren't in a menu!!!
        //              if (thePlayerClickInteractionScript.inMenu == false)
        {

            //                          theEnactionScript.mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            //                          theEnactionScript.mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

            //xRotation -= mouseY;
            //xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            //playerBody.Rotate(Vector3.up * mouseX);
        }

    }


    void soMyModifiedVersion()
    {
        //like...."xRotation -= mouseY;"?????  wtf are you doing?
        //
        float mouseHorizontal = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseVertical = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        verticalCameraRotation -= mouseVertical;
        verticalCameraRotation = Mathf.Clamp(verticalCameraRotation, -90f, 90f);  //so clearly, this is vertical.  this line stops you from doing "backflips" or "rolling forwards" when you are doing rotation to look up and down.  you look all the way up, and then STOP rotating up there.  same with down.  ok.

        //so, rotates vertical using euler, uses RELATIVE, around x-axis.  that's which axis the "verticalCameraRotation" variable is plugged into:
        transform.localRotation = Quaternion.Euler(verticalCameraRotation, 0f, 0f);

        //this multiplies by an absolute coordinates "Vector3.up" vector.  for some things [like vehicles] this is NOT what i want 
        playerBody.Rotate(Vector3.up * mouseHorizontal);


        //ah!  one is the camera rotating vertically [whic his relative to body?] other is whole player gameobject rotating.
        //because you don't want body rotating when you look up, or FAILING to rotate when you turn around!
        //transform.localRotation = Quaternion.Euler(0f, 0f, mouseHorizontalIThink);



    }


}
