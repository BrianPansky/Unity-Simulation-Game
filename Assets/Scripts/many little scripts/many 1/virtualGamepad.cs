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

        foreach (buttonCategories thisButtonCategory in allBoolButtons)
        {
            allCurrentBoolEnactables[thisButtonCategory] = null;
        }

        List<buttonCategories> allVectorButtons = new List<buttonCategories>();
        allVectorButtons.Add(buttonCategories.vector1);
        allVectorButtons.Add(buttonCategories.vector2);

        foreach (buttonCategories thisButtonCategory in allVectorButtons)
        {
            allCurrentVectorEnactables[thisButtonCategory] = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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

    internal void receiveAnyNonNullEnactionsForButtons(equippable2 equippable2)
    {
        foreach (IEnactaBool enactaBool in equippable2.GetComponents<IEnactaBool>())
        {
            enactaBool.enactionAuthor = this.transform.gameObject;
            allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
        }


        foreach (IEnactaVector enactaV in equippable2.GetComponents<IEnactaVector>())
        {
            allCurrentVectorEnactables[enactaV.gamepadButtonType] = enactaV;
        }
    }

    internal void updateALLGamepadButtonsFromplayable2(playable2 thePlayable2)
    {
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
                return; }
            if (theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.g]] == null)
            {
                Debug.Log("theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.g]] == null"); 
                return; }

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
                if (theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.click1]] == null) {return; }
                theVirtualGamePad.allCurrentBoolEnactables[buttonMapping[realButton.click1]].enact();
            }
        }


        void mouseMove()
        {
            if (theVirtualGamePad.allCurrentVectorEnactables[buttonMapping[realButton.mouse]] == null) { return; }

            float yawInput = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            float pitchInput = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

            theVirtualGamePad.allCurrentVectorEnactables[buttonMapping[realButton.mouse]].enact(new Vector2(yawInput, pitchInput));

        }

    }


}


public abstract class gamePadButtonable
{
    public buttonCategories gamepadButtonType;
    public IEnactaBool theEnaction;
}