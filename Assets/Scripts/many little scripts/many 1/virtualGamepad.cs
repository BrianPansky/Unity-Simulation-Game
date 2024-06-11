using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.XR;
using static virtualGamepad;

public class virtualGamepad : MonoBehaviour
{

    public static virtualGamepad singleton;

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



    //                              public bool primary = false;
    //ad-hoc inputs:
    public float x = 0f;
    public float z = 0f;
    public float yawInput = 0f;
    public float pitchInput = 0f;
    //public Vector3 lookingVector = Vector3.zero;

    //could call it "primary auxilliary" or something?
    //                              public bool jump = false;



    //plug in the enactables:
    //IEnactaBool primaryEnactable = null;
    //IEnactaBool AuxEnactable1 = null;
    //IEnactaBool mouseOrThumb1 = null;
    //IEnactaBool wasdOrThumb2 = null;


    //Dictionary<buttonCategories, bool> allCurrentBoolInputs = new Dictionary<buttonCategories, bool>();
    public Dictionary<buttonCategories, IEnactaBool> allCurrentBoolEnactables = new Dictionary<buttonCategories, IEnactaBool>();
    public Dictionary<buttonCategories, IEnactaVector> allCurrentVectorEnactables = new Dictionary<buttonCategories, IEnactaVector>();

    playerMouseKeyboardInputs mouseKeyboardInputs;


    public enum buttonCategories
    {
        primary,
        aux1,
        vector1,
    }

    



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



    void Awake()
    {
        initializeDictionaries();
        singletonify();  //so that i can use "enum buttonCategories" when making enactables

    }

    void singletonify()
    {
        if (singleton != null && singleton != this)
        {
            Debug.Log("this class is supposed to be a singleton, you should not be making another instance, destroying the new one");
            Destroy(this);
            return;
        }
        singleton = this;
    }



    void initializeDictionaries()
    {
        List<buttonCategories> allBoolButtons = new List<buttonCategories>();
        allBoolButtons.Add(buttonCategories.primary);
        allBoolButtons.Add(buttonCategories.aux1);




        foreach (buttonCategories thisButtonCategory in allBoolButtons)
        {
            //allCurrentBoolInputs[thisButtonCategory] = false;
            allCurrentBoolEnactables[thisButtonCategory] = null;
        }




        List<buttonCategories> allVectorButtons = new List<buttonCategories>();
        allVectorButtons.Add(buttonCategories.vector1);




        foreach (buttonCategories thisButtonCategory in allVectorButtons)
        {
            //allCurrentBoolInputs[thisButtonCategory] = false;
            allCurrentVectorEnactables[thisButtonCategory] = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initializeMouseKeyboardInputs();
    }

    void initializeMouseKeyboardInputs()
    {
        mouseKeyboardInputs = new playerMouseKeyboardInputs();
        mouseKeyboardInputs.initializeMouseKeyboard(this);
    }

    // Update is called once per frame
    void Update()
    {
        mouseKeyboardInputs.updateAll();

        /*
        foreach (buttonCategories thisButton in allCurrentBoolEnactables.Keys)
        {
            if (allCurrentBoolInputs[thisButton] == false)
            {
                continue;
            }

            allCurrentBoolEnactables[thisButton].enact();
            allCurrentBoolInputs[thisButton] = false;

        }
        */
    }







    public class playerMouseKeyboardInputs
    {
        public virtualGamepad theVirtualGamePad;

        Dictionary<realButton, buttonCategories> buttonMapping = new Dictionary<realButton, buttonCategories>();

        //      mouse look stuff:
        public float mouseSpeed = 290f;
        public Transform playerBody;
        float xRotation = 0f;
        //need the "playerClickInteraction" script 
        //(in order to check if we are in a menu or not)
        //public playerClickInteraction thePlayerClickInteractionScript;

        float verticalCameraRotation = 0f;


        enum realButton
        {
            click1,
            space,
            g,
            wasd
        }


        public void initializeMouseKeyboard(virtualGamepad inputVirtualGamePad)
        {
            theVirtualGamePad = inputVirtualGamePad;
            Cursor.lockState = CursorLockMode.Locked;
            defaultButtonMapping();
        }

        public void defaultButtonMapping()
        {
            buttonMapping[realButton.click1] = buttonCategories.primary;
            buttonMapping[realButton.space] = buttonCategories.aux1;
            buttonMapping[realButton.wasd] = buttonCategories.vector1;
            //buttonMapping[realButton.g] = buttonCategories.aux...2???;
            //buttonMapping[realButton.click1] = buttonCategories.;
        }

        void initializeDefaultButtonMapDictioinary()
        {
            buttonMapping[realButton.click1] = buttonCategories.primary;
            buttonMapping[realButton.space] = buttonCategories.aux1;
            //buttonMapping[realButton.click1] = buttonCategories.primary;
            //buttonMapping[realButton.click1] = buttonCategories.primary;
        }

        public void updateAll()
        {

            mouseUpdate();

            keyboardUpdate();

        }

        void keyboardUpdate()
        {
            //theVirtualGamePad.x = Input.GetAxis("Horizontal");
            //theVirtualGamePad.z = Input.GetAxis("Vertical");

            Vector2 wasdVactor= new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            
            if(wasdVactor.magnitude > 0.05f)
            {

                theVirtualGamePad.allCurrentVectorEnactables[buttonMapping[realButton.wasd]].enact(wasdVactor);
            }



            if (Input.GetButtonDown("Jump"))
            {
                theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.space]].enact();
            }
        }

        void mouseUpdate()
        {
            //Debug.Log("...................INPUTS...................");
            //      theVirtualGamePad.yawInput = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            //      theVirtualGamePad.pitchInput = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

            //theVirtualGamePad.primary = Input.GetMouseButtonDown(0);

            if (Input.GetMouseButtonDown(0))
            {
                theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.click1]].enact();
            }
            
        }

    }


}


public interface Iplayable
{
    //attach to objects/entities you can "play as" [such as bodies and vehicles]
    //weapons and items too


    //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:
    void equip(virtualGamepad gamepad);

}


public interface IbuttonObservable
{
    //enum ButtonUseType
    //{
        //primary,
        //secondary,
        //aux1
    //}

    //ButtonUseType useType { get; set; }


    void observeButton(IEnactaBool whatItDoes);
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
