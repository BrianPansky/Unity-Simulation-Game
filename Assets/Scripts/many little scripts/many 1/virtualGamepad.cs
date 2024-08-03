using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static enactionCreator;
using UnityEngine.XR;


public class virtualGamepad : MonoBehaviour
{
    public GameObject theCamera = null;
    public playable2 playingAs;
    public bool isPlayer = false;
    playerMouseKeyboardInputs mouseKeyboardInputs;


    public Dictionary<buttonCategories, IEnactaBool> allCurrentBoolEnactables = new Dictionary<buttonCategories, IEnactaBool>();
    public Dictionary<buttonCategories, IEnactaVector> allCurrentVectorEnactables = new Dictionary<buttonCategories, IEnactaVector>();
    public List<IEnactByTargetVector> allCurrentTARGETbyVectorEnactables = new List<IEnactByTargetVector>();

    

    void Awake()
    {
        //Debug.Log("Awake:  " + this);
        //doThisPrintoutByInputString("0");
        initializeDictionaries();
        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.gamepad);

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
        if (isPlayer != true) { return; }  //should probably.....move this to "player inputs" or something....
        mouseKeyboardInputs.updateAll();

    }



    /*

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

    */

    internal void receiveAnyNonNullEnactionsForButtons(equippable2 equippable2)
    {

        foreach (IEnactaBool enactaBool in equippable2.GetComponents<IEnactaBool>())
        {
            //                  Debug.Log("1111111111111111111 allCurrentBoolEnactables[enactaBool.gamepadButtonType]:  " + allCurrentBoolEnactables[enactaBool.gamepadButtonType]);

            enactaBool.enactionAuthor = this.transform.gameObject;
            allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
            //                  Debug.Log("222222222222222222222 allCurrentBoolEnactables[enactaBool.gamepadButtonType]:  " + allCurrentBoolEnactables[enactaBool.gamepadButtonType]);
        }


        foreach (IEnactaVector enactaV in equippable2.GetComponents<IEnactaVector>())
        {
            //enactaV.enactionAuthor = gamepad.transform.gameObject;
            allCurrentVectorEnactables[enactaV.gamepadButtonType] = enactaV;
        }



        /*
        //wait, is this allowed????  i GUESS so..........since the execution is conditional on whether there ARE any such items on the list???  or it's just not getting angry at me YET???
        foreach (collisionEnaction enactaBool in equippable2.enactableBoolSet)
        {
            //                  Debug.Log("1111111111111111111 allCurrentBoolEnactables[enactaBool.gamepadButtonType]:  " + allCurrentBoolEnactables[enactaBool.gamepadButtonType]);

            enactaBool.enactionAuthor = this.transform.gameObject;
            allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
            //                  Debug.Log("222222222222222222222 allCurrentBoolEnactables[enactaBool.gamepadButtonType]:  " + allCurrentBoolEnactables[enactaBool.gamepadButtonType]);
        }


        foreach (IEnactaVector enactaV in equippable2.enactableVectorSet)
        {
            //enactaV.enactionAuthor = gamepad.transform.gameObject;
            allCurrentVectorEnactables[enactaV.gamepadButtonType] = enactaV;
        }
        */

    }

    internal void updateALLGamepadButtonsFromplayable2(playable2 thePlayable2)
    {
        //      virtualGamepad gamepad = this.gameObject.GetComponent<virtualGamepad>();
        if (thePlayable2 == null) { Debug.Log("thePlayable2 ==null"); return; }
        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:

        foreach (IEnactaBool enactaBool in thePlayable2.GetComponents<IEnactaBool>())
        {
            enactaBool.enactionAuthor = this.transform.gameObject;
            this.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
        }


        foreach (IEnactaVector enactaV in thePlayable2.GetComponents<IEnactaVector>())
        {
            this.allCurrentVectorEnactables[enactaV.gamepadButtonType] = enactaV;
        }


        List<IEnactByTargetVector> newListToPreventShallowCopyError = new List<IEnactByTargetVector>();
        foreach (IEnactByTargetVector enactByTargetVector in thePlayable2.GetComponents<IEnactByTargetVector>())
        {
            newListToPreventShallowCopyError.Add(enactByTargetVector);
        }


        this.allCurrentTARGETbyVectorEnactables.Clear();
        this.allCurrentTARGETbyVectorEnactables = newListToPreventShallowCopyError;


    }


    internal void removeFromGamepadButtons(equippable2 equippable2)
    {
        //merely remove, do not replace with any defaults in this function

        foreach (IEnactaBool enactaBool in equippable2.GetComponents<IEnactaBool>())
        {
            allCurrentBoolEnactables[enactaBool.gamepadButtonType] = null;
        }



        /*

        foreach (IEnactaBool enactaBool in equippable2.enactableBoolSet)
        {
            //merely remove, do not replace with any defaults in this function
            allCurrentBoolEnactables[enactaBool.gamepadButtonType] = null;
//          Debug.Log("222222222222222222222 gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType]:  " + gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType]);
        }

        foreach (IEnactaVector enactaV in equippable2.enactableVectorSet)
        {
            //merely remove, do not replace with any defaults in this function
            allCurrentBoolEnactables[enactaV.gamepadButtonType] = null;
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
            buttonMapping[realButton.g] = buttonCategories.augment1;
            //buttonMapping[realButton.click1] = buttonCategories.;
        }

        void initializeDefaultButtonMapDictioinary()
        {
            buttonMapping[realButton.click1] = buttonCategories.primary;
            buttonMapping[realButton.space] = buttonCategories.aux1;
        }

        public void updateAll()
        {

            mouseUpdate();

            keyboardUpdate();

        }

        void keyboardUpdate()
        {
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

            //Debug.Log("should enact??  what is there:  "+ theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.g]]);
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


public abstract class gamePadButtonable
{
    public buttonCategories gamepadButtonType;
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
