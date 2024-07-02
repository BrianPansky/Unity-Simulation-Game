using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.XR;
using static virtualGamepad;

public class virtualGamepad : MonoBehaviour
{
    public GameObject theCamera = null;

    public playable playingAs;


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
    public Dictionary<buttonCategories, IEnactaBool> allCurrentBoolEnactables = new Dictionary<buttonCategories, IEnactaBool>();
    public Dictionary<buttonCategories, IEnactaVector> allCurrentVectorEnactables = new Dictionary<buttonCategories, IEnactaVector>();
    //public Dictionary<buttonCategories, IEnactByTargetVector> allCurrentTARGETbyVectorEnactables = new Dictionary<buttonCategories, IEnactByTargetVector>();
    public List<IEnactByTargetVector> allCurrentTARGETbyVectorEnactables = new List<IEnactByTargetVector>();

    

    playerMouseKeyboardInputs mouseKeyboardInputs;


    //uninformative?
    public enum buttonCategories
    {
        errorYouDidntSetEnumTypeForBUTTONCATEGORIES,
        primary,
        aux1,
        vector1,
        vector2,
        augment1
    }

    //need THIS instead/as well?
    public enum INFORMATIVEbuttonCategories
    {
        errorYouDidntSetEnumTypeForINFORMATIVEbuttonCategories,
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
        allBoolButtons.Add(buttonCategories.augment1);

        //Debug.Log("initializeDictionaries() for this object:  " + this.gameObject);

        foreach (buttonCategories thisButtonCategory in allBoolButtons)
        {
            //Debug.Log("has category:  " + thisButtonCategory);
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
            mouse
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
            //buttonMapping[realButton.g] = buttonCategories.aux...2???;
            buttonMapping[realButton.g] = buttonCategories.augment1;
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
            if (Input.GetKeyDown(KeyCode.G) == false) {

                //Debug.Log("Input.GetKeyDown(KeyCode.G) == false"); 
                return; }
            if (theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.g]] == null)
            {
                Debug.Log("theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.g]] == null"); 
                return; }

            Debug.Log("should enact??");
            theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.g]].enact();
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


public class playable: MonoBehaviour
{
    public bool occupied = false;


    public GameObject enactionPoint1;
    public Transform cameraMount;

    public List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
    public List<IEnactaVector> enactableVectorSet = new List<IEnactaVector>();
    public List<IEnactByTargetVector> enactableTARGETVectorSet = new List<IEnactByTargetVector>();
    
    public Dictionary<interactionCreator.simpleSlot, equippable> equipperSlotsAndContents = new Dictionary<interactionCreator.simpleSlot, equippable>();

    //attach to objects/entities you can "play as" [such as bodies and vehicles]
    //weapons and items too


    //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:

    public void defaultCameraMountGenerator()
    {
        GameObject newObject = new GameObject("cameraMount?????");

        //newObject.transform.parent = this.transform;
        newObject.transform.SetParent(this.transform, false);
        //newObject.transform.position = this.transform.position + this.transform.forward * 0.3f;
        //newObject.transform.rotation = this.transform.rotation;

        //GameObject newObject2 = genGen.singleton.returnPineTree1(this.transform.position + this.transform.forward * 2.3f);

        //newObject2.transform.SetParent(this.transform, false);
        //newObject2.transform.position = this.transform.position + this.transform.forward * 8f;
        //newObject2.transform.rotation = this.transform.rotation;

        cameraMount = newObject.transform;

    }




    public void plugIntoGamepadIfThereIsOne()
    {
        virtualGamepad gamepad = gameObject.GetComponent<virtualGamepad>();
        if (gamepad == null) {
            Debug.Log("gamepad == null for:  " + this.gameObject.name); 
            return; }

        equip(gamepad);

    }

    public void playAs(virtualGamepad gamepad)
    {
        //better way, just plug whole thing in, lol
        occupied = true;
        gamepad.playingAs = this;
    }

    public void equip(virtualGamepad gamepad)
    {
        if(occupied == true) { return; }
        occupied = true;


        //Debug.Log("is cameraMount  null:  " + cameraMount + "  for this object:  " + this.gameObject.name);
        //Debug.Log("is gamepad.theCamera null:  " + gamepad.theCamera + "  for this object:  " + this.gameObject.name);
        if (cameraMount != null && gamepad.theCamera != null)
        {

            gamepad.theCamera.transform.SetParent(cameraMount, false);
        }

        //Debug.Log("is this ever happening???????&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
        updateAnyGamepadButtons(gamepad);

        if (gamepad.theCamera == null) { return; }

        /*
        Debug.Log(cameraMount);
        if (cameraMount == null)
        {
            defaultCameraMountGenerator();
        }
        Debug.Log(cameraMount);
        gamepad.theCamera.transform.SetParent(cameraMount, false);
        */
    }

    public void unequip(virtualGamepad gamepad)
    {
        occupied = false;


        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            enactaBool.interInfo.enactionAuthor = null;
            gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = null;
        }



        foreach (IEnactaVector enactaV in enactableVectorSet)
        {
            //enactaV.enactionAuthor = gamepad.transform.gameObject;
            gamepad.allCurrentVectorEnactables[enactaV.gamepadButtonType] = null;
        }

        foreach (IEnactByTargetVector enactaTargetV in enactableTARGETVectorSet)
        {
            if (gamepad.allCurrentTARGETbyVectorEnactables.Contains(enactaTargetV))
            {

                Debug.Log("here?????????$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                gamepad.allCurrentTARGETbyVectorEnactables.Remove(enactaTargetV);
            }
        }
    }

    internal void updateAnyGamepadButtons(virtualGamepad gamepad)
    {
        //      virtualGamepad gamepad = this.gameObject.GetComponent<virtualGamepad>();
        if(gamepad ==null) { return; }
        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:

        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            //this "object is null" error is usually the only kind of error where it isn't clear which variable went wrong
            //and EVERY TIME it's a situation like this, where there are a TON of variables in a single line.
            //so i need to print sooooooo many....
            //Debug.Log("enactaBool:  " + enactaBool);
            //      Debug.Log("enactaBool.interInfo:  " + enactaBool.interInfo);
            //Debug.Log("enactaBool.interInfo.enactionAuthor:  " + enactaBool.interInfo.enactionAuthor);
            //Debug.Log("gamepad:  " + gamepad);
            //Debug.Log("gamepad.transform:  " + gamepad.transform);
            //ebug.Log("gamepad.transform.gameObject:  " + gamepad.transform.gameObject);
            enactaBool.interInfo.enactionAuthor = gamepad.transform.gameObject;
            gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
        }



        foreach (IEnactaVector enactaV in enactableVectorSet)
        {
            //enactaV.enactionAuthor = gamepad.transform.gameObject;
            gamepad.allCurrentVectorEnactables[enactaV.gamepadButtonType] = enactaV;
        }

        /*
        foreach (IEnactByTargetVector thisEnactByTargetVector in gamepad.allCurrentTARGETbyVectorEnactables)
        {
            Debug.Log("1111111  gamepad.allCurrentTARGETbyVectorEnactables----------------------thisEnactByTargetVector:  " + thisEnactByTargetVector);

        }
        foreach (IEnactByTargetVector thisEnactByTargetVector in enactableTARGETVectorSet)
        {
            Debug.Log("1111111  enactableTARGETVectorSet thisEnactByTargetVector:  " + thisEnactByTargetVector);

        }

        Debug.Log("[[[[[[[[[[[[[[[[[[[[[[[[[[ BEFORE    gamepad.allCurrentTARGETbyVectorEnactables.Count:  " + gamepad.allCurrentTARGETbyVectorEnactables.Count);
        */

        List<IEnactByTargetVector> newListToPreventShallowCopyError = new List<IEnactByTargetVector>();
        foreach (IEnactByTargetVector enactByTargetVector in enactableTARGETVectorSet)
        {
            newListToPreventShallowCopyError.Add(enactByTargetVector);
        }


        gamepad.allCurrentTARGETbyVectorEnactables.Clear();
        gamepad.allCurrentTARGETbyVectorEnactables = newListToPreventShallowCopyError;
        
        
        /*
        Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^ AFTER    gamepad.allCurrentTARGETbyVectorEnactables.Count:  " + gamepad.allCurrentTARGETbyVectorEnactables.Count);

        foreach (IEnactByTargetVector thisEnactByTargetVector in gamepad.allCurrentTARGETbyVectorEnactables)
        {
            Debug.Log("2222222  gamepad.allCurrentTARGETbyVectorEnactables----------------------thisEnactByTargetVector:  " + thisEnactByTargetVector);

        }
        foreach (IEnactByTargetVector thisEnactByTargetVector in enactableTARGETVectorSet)
        {
            Debug.Log("2222222  enactableTARGETVectorSet thisEnactByTargetVector:  " + thisEnactByTargetVector);

        }

        */


    }
}

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
