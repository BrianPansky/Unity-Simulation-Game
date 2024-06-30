using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.XR;
using UnityEngine.Playables;
//using static virtualGamepad;
//using System.Numerics;

public class virtualGamepad : MonoBehaviour
{
    public GameObject theCamera = null;

    //public playable playingAs;
    public Dictionary<buttonCategories, IEnactaBool> allCurrentBoolEnactables = new Dictionary<buttonCategories, IEnactaBool>();
    public Dictionary<buttonCategories, IEnactaVector> allCurrentVectorEnactables = new Dictionary<buttonCategories, IEnactaVector>();
    public List<IEnactByTargetVector> allCurrentTARGETbyVectorEnactables = new List<IEnactByTargetVector>();


    public Dictionary<interactionCreator.slot, gamepadable> equipperSlotsAndTheirEquipment = new Dictionary<interactionCreator.slot, gamepadable>();
    //public Dictionary<interactionCreator.slot, bool> availabilityOfParts = new Dictionary<interactionCreator.slot, bool>();



    public bool isPlayer = false;
    //public static virtualGamepad singleton;

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
    

    playerMouseKeyboardInputs mouseKeyboardInputs;


    //uninformative?
    public enum buttonCategories
    {
        primary,
        aux1,
        interact,
        vector1,
        vector2
    }

    //need THIS instead/as well?
    public enum INFORMATIVEbuttonCategories
    {
        fire1,
        jump,
        move1,
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
        //doThisPrintoutByInputString("0");
        initializeDictionaries();
        //singletonify();  //so that i can use "enum buttonCategories" when making enactables

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
        allVectorButtons.Add(buttonCategories.vector2);




        foreach (buttonCategories thisButtonCategory in allVectorButtons)
        {
            //allCurrentBoolInputs[thisButtonCategory] = false;
            allCurrentVectorEnactables[thisButtonCategory] = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log("this.gameObject:  " + this.gameObject);
        //Debug.DrawLine(new Vector3(), this.gameObject.transform.position, Color.green, 22f);
        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.gamepad);

        if (isPlayer)
        {
            initializeMouseKeyboardInputs();
        }


    }

    void initializeMouseKeyboardInputs()
    {
        mouseKeyboardInputs = new playerMouseKeyboardInputs();
        mouseKeyboardInputs.initializeMouseKeyboard(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer != true) { return; }
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

    void equipSelf()
    {
        //          this.gameObject.GetComponent<playable>().equip(this);
    }


    void tryToChangeWhichEnactablesGamepadActivatesAndWhichequipperSlotsAreOccupied(gamepadable thisGamepadable)
    {
        //[this is the generic function.  it will detect whether it needs to use "playAs" or "equip"
        //of course, there should be an interface for that?  not quite, i DO want to pre-design the functions.
        //err...ya?  i guess specific items/vehicles can INHERIT from "playable" and "equippable"
        //still need to define ABSTRACT function in.....gamepadable.  right.  so THAT is the data type this function
        //will take as input.

        //          thisGamepadable.tryToChangeWhichEnactablesGamepadActivatesAndWhichequipperSlotsAreOccupied(this);

    }

    void playAs(playable thisPlayable)
    {
        //transfer ALL enactables
        //if(thisPlayable.occupied == true) { return;}

        //playingAs = thisPlayable;
        //thisPlayable.occupied = true;
    }

    void equip(playable thisPlayable)
    {
        //only transfer the non-null enactables


    }




    public void plugIntoGamepadIfThereIsOne()
    {
        virtualGamepad gamepad = gameObject.GetComponent<virtualGamepad>();
        if (gamepad == null)
        {
            Debug.Log("gamepad == null for:  " + this.gameObject.name);
            return;
        }

        //          equip(gamepad);

    }

    public void playAs(virtualGamepad gamepad)
    {
        //better way, just plug whole thing in, lol
        //      occupied = true;
        //      gamepad.playingAs = this;
    }

    



    public void doThisPrintoutByInputString(string inputString)
    {
        Debug.Log(inputString + inputString + inputString + inputString + inputString + inputString + inputString + inputString + inputString + inputString + inputString + inputString);

        foreach (var thisKey in allCurrentBoolEnactables.Keys)
        {
            Debug.Log(thisKey);
            Debug.Log(allCurrentBoolEnactables[thisKey]);
            if(allCurrentBoolEnactables[thisKey] == null) { continue; }

            //if (allCurrentBoolEnactables[thisKey].interactionType == null) { continue; }
            Debug.Log(allCurrentBoolEnactables[thisKey].interInfo.interactionType);
        }
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

        float mouseThreshhold = 0.2f;


        enum realButton
        {
            click1,
            space,
            g,
            wasd,
            mouse,
            e
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
            buttonMapping[realButton.mouse] = buttonCategories.vector2;
            buttonMapping[realButton.e] = buttonCategories.interact;
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


            updateWASD();

            updateJump();

            updateMiscKeyboard();

        }

        private void updateMiscKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                //          theVirtualGamePad.playingAs.enactableBoolSet[buttonMapping[realButton.space]].enact();
            }
        }

        void updateWASD()
        {
            Vector2 wasdVactor = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


            if (wasdVactor.magnitude < 0.05f) { return; }
            if (theVirtualGamePad.allCurrentVectorEnactables[buttonMapping[realButton.wasd]] == null) { return; }
            theVirtualGamePad.allCurrentVectorEnactables[buttonMapping[realButton.wasd]].enact(wasdVactor);
        }

        void updateJump()
        {

            if (Input.GetButtonDown("Jump") == false) { return; }
            if (theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.space]] == null) { return; }
            theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.space]].enact();
        }

        void mouseUpdate()
        {

            mouseClick();
            mouseMove();
            //Debug.Log("...................INPUTS...................");
            //      theVirtualGamePad.yawInput = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            //      theVirtualGamePad.pitchInput = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

            //theVirtualGamePad.primary = Input.GetMouseButtonDown(0);



            /*
            theVirtualGamePad.yawInput = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            theVirtualGamePad.pitchInput = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
            */



        }

        void mouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("...................Input.GetMouseButtonDown(0)");
                if (theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.click1]] == null) {

                    //Debug.Log("theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.click1]] == null");
                    //Debug.Log("buttonMapping[realButton.click1]:  " + buttonMapping[realButton.click1]);
                    return; }
                theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.click1]].enact();
            }
        }


        void mouseMove()
        {
            if (theVirtualGamePad.allCurrentVectorEnactables[buttonMapping[realButton.mouse]] == null) { return; }

            float yawInput = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            float pitchInput = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

            //if (yawInput < mouseThreshhold) { return; }
            //if (pitchInput < mouseThreshhold) { return; }

            theVirtualGamePad.allCurrentVectorEnactables[buttonMapping[realButton.mouse]].enact(new Vector2(yawInput, pitchInput));

        }

    }


}















public abstract class vehicleItemWeaponOrBodyEtc
{
    public List<slot> equipperSlotList;

    public void equippableSlotsInitializer(List<interactionCreator.slot> availableParts)
    {
        //(List<buttonCategories> availableButtonCategories) hmmmmmmmmmmm, no, need to do this in terms of how they can be equipped [held one one of your free hands, worn on head, etc]


        foreach (var part in availableParts)
        {
            //      this.availabilityOfParts[part] = false;
        }
    }

}

public abstract class gamepadable : MonoBehaviour
{
    //not all items/equippables require a gamepad button, some are passive

    

    //Can’t just plug enactions into gamepad anymore:
    //      You DO plug them in like that most of the time.And at the same time,
    //      have to UNEQUIP anything that occupies the same button OR equipperSlot slot
    //          Oh, and unequipping something obviously simultaneously removes it from the buttons AND the equipperSlot slots.
    //      The exception is if slots are occupied by a “playAs” [body or vehicle],
    //      then they cannot be repurposed until player manually exits vehicle
    //          It could tell the player “you can’t do that while in this vehicle”





    //public List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
    //public List<IEnactaVector> enactableVectorSet = new List<IEnactaVector>();
    //public List<IEnactByTargetVector> enactableTARGETVectorSet = new List<IEnactByTargetVector>();


    //public Dictionary<buttonCategories, IEnactaBool> enactableBoolSet = new Dictionary<buttonCategories, IEnactaBool>();
    //public Dictionary<buttonCategories, IEnactaVector> enactableVectorSet = new Dictionary<buttonCategories, IEnactaVector>();
    //public List<IEnactByTargetVector> enactableTARGETVectorSet = new List<IEnactByTargetVector>();


    //all should have exit by default.  yes, it has to be an IEnactaBool, not a mere function, because it needs to occupy a BUTTON!
    public List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
    public List<IEnactaVector> enactableVectorSet = new List<IEnactaVector>();
    public List<IEnactByTargetVector> enactableTARGETVectorSet = new List<IEnactByTargetVector>();


    
}

public abstract class playable : gamepadable
{
    //these can also be an individual "seat" on a vehicle?  which might not have any enactions at all?  except exit
    //public bool occupied = false;
    //public Dictionary<buttonCategories, equippable> currentlyEquipped = new Dictionary<buttonCategories, equippable>();
    
    public Transform cameraMount;

    //attach to objects/entities you can "play as" [such as bodies and vehicles]
    //weapons and items too


    //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:



    //need to get clear.  is this just a slot/seat?  or is this a full vehicle/body?
    //"playable" sounds like a vehcile/body.
    //so i need something ELSE to be the slots/seats.....and that is the dictionary entries.


}

public abstract class equippable: gamepadable
{
    //interactionCreator.slot thisEquippablePart;  //like items //hmm, infinite variety of items, an object of this class will suffice to describe itself, no need for enum
    List<interactionCreator.slot> whatPartsAreRequiredToEquipThis;  //like hands
    playable equippedBy;


    



}

/*

public class equipperSlot
{
    no, go see the "slots" i defined elsewhere [they are in interactionCreator, for now]
    



}

*/




















//??????????  do i need this?
public abstract class gamePadButtonable
{
    public virtualGamepad.buttonCategories gamepadButtonType;
    public IEnactaBool theEnaction;
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
