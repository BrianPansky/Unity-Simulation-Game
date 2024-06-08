using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    //put this script on camera
    //make camera a direct "child" of player gameobject
    //it will listen to enactionscript for movement.


    public virtualGamepad theVirtualGamePad;

    public float limitedPitchRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (theVirtualGamePad == null)
        {
            GameObject parent = this.transform.parent.gameObject;
            theVirtualGamePad = parent.GetComponent<virtualGamepad>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (conditionsMet() == false)
        {
            return;
        }


        updatePitch();
        //theVirtualGamePad.updateLookingVector();
        //                          this.transform.rotation = theVirtualGamePad.rotationFromLookingVector();
    }


    bool conditionsMet()
    {
        //if (thePlayerClickInteractionScript.inMenu == false)
        {
            //letsSeeIfICanFuckingTranslateThisGarbage();
            //return false;

        }

        //if (theVirtualGamePad.currentlyUsable.Contains("humanBody") != true)
        {
            //return false;
        }

        return true;
    }


    void updatePitch()
    {
        limitedPitchRotation -= theVirtualGamePad.pitchInput;

        //Debug.Log("limitedPitchRotation:  " + limitedPitchRotation);
        limitedPitchRotation = Mathf.Clamp(limitedPitchRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(limitedPitchRotation, 0f, 0f);
        //                  playerBody.Rotate(Vector3.up * theVirtualGamePad.mouseX);
    }

    void updateMouseLook()
    {

        //          xRotation -= theVirtualGamePad.mouseY;
        //          xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //          transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //          playerBody.Rotate(Vector3.up * theVirtualGamePad.mouseX);
    }
}
