using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.XR;

public class virtualGamepad : MonoBehaviour
{

    //WAIT I DON'T NEED WEIRD DOUBLE BOOLS HERE TO ENSURE OTHER SCRIPT GETS THE SIGNAL!
    //just have the OTHER script be the one responsible for resetting the bool after it has received it!  simple



    //how does a body know which controllers to listen to, and when?  need to be able to switch when you enter a vehicle etc…..
    //a modular script
    //      basically just stores data about current “controller button pressing” on that frame of gameplay
    //      can also be linked to other scripts OF THE SAME TYPE[no fiddling around with hundreds of unique body and vehicle scripts] and transmit all of its values to the other one.
    //      bodies [human body, vehicle “body” etc] are attached to game objects.those game objects will also have their own “game controller” script.the bodies find the controller script on their own gameObject, and listen to that one specifically
    //      has booleans to tell bodies whether it is currently controlling current body
    //          thus, getting in vehicle does this:
    //              “Get in” vehicle just by doing “standard click” to it
    //              that standard click activates a specific enaction[or wait, that’s an INTERACTION, right ? but “enaction script” is not the same as body script….nor same as game controller script ? or does my new system of controllers and bodies completely eliminate the enaction script ? hmmm…and then do i want a general abstract body to be able to build all the new ones bla bla bla….whatever, i think new system for now], which does the following:
    //                  links person’s controller script to the vehicle’s controller script, directional so the vehicle one receives from person
    //                  changes bool in person’s controller script
    //                      this makes it so that when the person’s human body looks at this controller script, it sees that it is not time to be controlled by it
    //                  changes bool in the vehicles’s controller script
    //                      makes it so that the vehicle “body” script knows that it is time to be controlled by it
    //                  [reverse for exiting vehicle]



    public bool primary = false;
    //ad-hoc inputs:
    public float x = 0f;
    public float z = 0f;
    public float yawInput = 0f;
    public float pitchInput = 0f;
    //public Vector3 lookingVector = Vector3.zero;

    //could call it "primary auxilliary" or something?
    public bool jump = false;














    //          some non-button stuff:
    bool transmitting = true;
    GameObject outputToObject;
    virtualGamepad outputToVirtualGamepad;


    //          the buttons and stuff:
    public Dpad theDpad = new Dpad();

    //like "Start" and "Select"
    public myButtonClass button1 = new myButtonClass();
    public myButtonClass button2 = new myButtonClass();

    //like the 4 main buttons
    public myButtonClass buttonA = new myButtonClass();
    public myButtonClass buttonB = new myButtonClass();
    public myButtonClass buttonC = new myButtonClass();
    public myButtonClass buttonD = new myButtonClass();

    //like thje bumpers, or white and black on x-box
    public myButtonClass buttonE = new myButtonClass();
    public myButtonClass buttonF = new myButtonClass();

    //two triggers:
    public trigger trigger1 = new trigger();
    public trigger trigger2 = new trigger();

    //two thumbsticks:
    public thumbstick thumbstick1 = new thumbstick();
    public thumbstick thumbstick2 = new thumbstick();


    //also have stuff that AI usually uses here?  like target?  no, target is in SENSORY
    //could maybe have the navmesh agent stuff here.  but that almost seems more like a body thing.



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this script is mostly passive.
        //      inputs will be scripted on other scripts [player or AI scripts]
        //      outputs will be "listened for" by other scripts [bodies, vehicles, etc]
        //so this script doesn't have to actually do any of those
        //maybe togle bools back to false and all that sort of thing?  will have to see what works there.
        //      but, in that case, will have to have an ADDITIONAL bool for all buttons and such.
        //      to keep track of whether the input has been allowed to stay on this script for at least one single frame [otherwise might not be read by body/vehicle etc]


        //but it WILL automatically pass on ALL inputs into "outputToVirtualGamepad" IF there is one connected
        if(outputToVirtualGamepad != null)
        {
            //pass on ALL inputs into "outputToVirtualGamepad"
        }
    }
}

public class myButtonClass
{
    //maybe togle bools back to false and all that sort of thing?  will have to see what works there.
    //      but, in that case, will have to have an ADDITIONAL bool for all buttons and such.
    //      to keep track of whether the input has been allowed to stay on this script for at least one single frame [otherwise might not be read by body/vehicle etc]

    //every button press must also set "turnOffNext" to false [otherwise input may be discarded that frame]
    //the update on THIS script will ensure that all "pressed" buttons get the "turnOffNext" bool set to "true"

    public bool pressed = false;
    public bool turnOffNext = false;
}

public class Dpad
{
    public myButtonClass up = new myButtonClass();
    public myButtonClass down = new myButtonClass();
    public myButtonClass left = new myButtonClass();
    public myButtonClass right = new myButtonClass();
}

public class trigger
{
    public float press = 0f;
    myButtonClass booleanButtonLogic = new myButtonClass();
}


public class thumbstick
{
    //could use Vector2?  would that be better somehow?  i dunno, if there's any specific moment where i need trignonometry or whatever, i can use the following floats to CREATE a vector2 right then and there for that use.  i think.  maybe.
    public float up = 0f;
    public float down = 0f;
    public float left = 0f;
    public float right = 0f;

    public myButtonClass booleanUp = new myButtonClass();
    public myButtonClass booleanDown = new myButtonClass();
    public myButtonClass boleanLeft = new myButtonClass();
    public myButtonClass boolRight = new myButtonClass();
}
