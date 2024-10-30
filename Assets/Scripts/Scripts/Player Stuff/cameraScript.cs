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
        //hmm, maybe i'll just plug them in in the prefab....

        /*
        if (theVirtualGamePad == null)
        {
            GameObject parent = this.transform.parent.gameObject;
            theVirtualGamePad = parent.GetComponent<virtualGamepad>();
        }

        theVirtualGamePad.theCamera = this.gameObject;

        */
    }

    // Update is called once per frame
    void Update()
    {
        if (conditionsMet() == false)
        {
            return;
        }

        //Debug.Log("cccccccccccccccccccccccccccccccccccccccccccc   this.transform x+y+z:  X:  " + this.transform.rotation.x + "  Y:  " + this.transform.rotation.y + "  Z:  " + this.transform.rotation.z + "  W:  " + this.transform.rotation.w);

        //updatePitch();
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


}
