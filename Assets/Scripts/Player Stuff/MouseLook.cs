using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSpeed = 290f;

    public Transform playerBody;

    float xRotation = 0f;

    //need the "playerClickInteraction" script 
    //(in order to check if we are in a menu or not)
    public playerClickInteraction thePlayerClickInteractionScript;






    //trying to translate this mess:
    float verticalCameraRotation = 0f;






    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //need the "playerClickInteraction" script 
        //(in order to check if we are in a menu or not)
        //but that script is on the PARENT object, so first need to get the parent object:
        GameObject thisParent = this.transform.parent.gameObject;
        //now can get the script:
        thePlayerClickInteractionScript = thisParent.GetComponent<playerClickInteraction>(); //this.gameObject.GetComponent("playerClickInteraction") as playerClickInteraction;

        if (thePlayerClickInteractionScript == null)
        {
            Debug.Log("wtffffffffffffffffffffffffffffffffffffffffffffff");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ONLY can look around if we aren't in a menu!!!
        //theThingIgotFromATutorial();
        if (thePlayerClickInteractionScript.inMenu == false)
        {
            //letsSeeIfICanFuckingTranslateThisGarbage();
            soMyModifiedVersion();
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






    void theThingIgotFromATutorial()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

    }


    void letsSeeIfICanFuckingTranslateThisGarbage()
    {
        //like...."xRotation -= mouseY;"?????  wtf are you doing?
        //
        float mouseHorizontalIThink = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseVerticalIThink = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        verticalCameraRotation -= mouseVerticalIThink;
        verticalCameraRotation = Mathf.Clamp(xRotation, -90f, 90f);  //so clearly, this is vertical.  this line stops you from doing "backflips" or "rolling forwards" when you are doing rotation to look up and down.  you look all the way up, and then STOP rotating up there.  same with down.  ok.

        //so, rotates vertical using euler, uses RELATIVE, around x-axis.  that's which axis the "verticalCameraRotation" variable is plugged into:
        transform.localRotation = Quaternion.Euler(verticalCameraRotation, 0f, 0f);

        //this multiplies by an absolute coordinates "Vector3.up" vector.  for some things [like vehicles] this is NOT what i want 
        playerBody.Rotate(Vector3.up * mouseHorizontalIThink);

        //why does one use "transform.localRotation" and the other uses "playerBody"? maybe one of those is only available one way?  relative available for transform?  who fucking knows.
    
        //ah!  one is the camera rotating vertically [whic his relative to body?] other is whole player gameobject rotating.
        //because you don't want body rotating when you look up, or FAILING to rotate when you turn around!
    }
}
