using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    //put this script on camera
    //make camera a direct "child" of player gameobject
    //it will listen to enactionscript for movement.


    public enactionScript theEnactionScript;

    public float limitedPitchRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (theEnactionScript == null)
        {
            GameObject parent = this.transform.parent.gameObject;
            theEnactionScript = parent.GetComponent<enactionScript>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        updatePitch();
        //theEnactionScript.updateLookingVector();
        //                          this.transform.rotation = theEnactionScript.rotationFromLookingVector();
    }


    void updatePitch()
    {
        limitedPitchRotation -= theEnactionScript.pitchInput;

        //Debug.Log("limitedPitchRotation:  " + limitedPitchRotation);
        limitedPitchRotation = Mathf.Clamp(limitedPitchRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(limitedPitchRotation, 0f, 0f);
        //                  playerBody.Rotate(Vector3.up * theEnactionScript.mouseX);
    }

    void updateMouseLook()
    {

        //          xRotation -= theEnactionScript.mouseY;
        //          xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //          transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //          playerBody.Rotate(Vector3.up * theEnactionScript.mouseX);
    }
}
