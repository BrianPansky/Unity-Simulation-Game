﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;

public class playable2 : stateHolder, IInteractable
{



    //attach to objects/entities you can "play as" [such as bodies and vehicles], NOT weapons and items [for them use "equippable2"]
    public bool occupied = false;


    public GameObject enactionPoint1;
    public Transform cameraMount;

    /*
    public List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
    //public List<collisionEnaction> enactableBoolSet = new List<collisionEnaction>();
    public List<IEnactaVector> enactableVectorSet = new List<IEnactaVector>();
    public List<IEnactByTargetVector> enactableTARGETVectorSet = new List<IEnactByTargetVector>();
    */

    //public Dictionary<interactionCreator.simpleSlot, equippable2> equipperSlotsAndContents = new Dictionary<interactionCreator.simpleSlot, equippable2>();
    public Dictionary<interactionCreator.simpleSlot, GameObject> equipperSlotsAndContents = new Dictionary<interactionCreator.simpleSlot, GameObject>();

    //public Dictionary<interType, List<Ieffect>> dictOfInteractions = new Dictionary<enactionCreator.interType, List<Ieffect>>();
    //public Dictionary<interactionCreator.numericalVariable, float> dictOfIvariables = new Dictionary<interactionCreator.numericalVariable, float>();
    public Dictionary<interType, List<Ieffect>> dictOfInteractions { get; set;}





    
    public float lookSpeed = 290f;
    public float standardClickDistance = 7.0f;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    public bool isGrounded;
    


    void Awake()
    {

        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.interactable);
        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.playable2);
        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.zoneable);
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("plugIntoGamepadIfThereIsOne, " + this.gameObject.name);
        plugIntoGamepadIfThereIsOne();
    }


    static public playable2 genPlayable2AndReturn(GameObject theObject)
    {
        playable2 thePlayable = theObject.GetComponent<playable2>();
        if (thePlayable != null) { return thePlayable; }

        thePlayable.dictOfInteractions = new Dictionary<enactionCreator.interType, List<Ieffect>>();
        thePlayable.dictOfIvariables = new Dictionary<interactionCreator.numericalVariable, float>();

        return thePlayable;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public static void genPlayable2()
    {

    }



    //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:
    public void initializeEnactionPoint1()
    {
        enactionPoint1 = new GameObject("enactionPoint1 in initializeEnactionPoint1(), playable2 script"); //hmm, shouldn't playable2 AND equippable2s both have this?  a class encompassing them both?
        enactionPoint1.transform.parent = transform;
        enactionPoint1.transform.position = this.transform.position + this.transform.forward * 0.5f;
        enactionPoint1.transform.rotation = this.transform.rotation;

    }

    public void initializeCustomEnactionPoint1(GameObject parent, Vector3 offset)
    {
        enactionPoint1 = new GameObject("enactionPoint1 in initializeEnactionPoint1(), playable2 script"); //hmm, shouldn't playable2 AND equippable2s both have this?  a class encompassing them both?
        //enactionPoint1.transform.SetParent(parent, false, )
        enactionPoint1.transform.parent = parent.transform;
        enactionPoint1.transform.position = parent.transform.position + offset;
        enactionPoint1.transform.rotation = this.transform.rotation;

        enactionPoint1.transform.localScale = -parent.transform.localScale;

    }

    public void defaultCameraMountGenerator()
    {
        GameObject newObject = new GameObject("cameraMount");


        newObject.transform.SetParent(this.transform, false);
        cameraMount = newObject.transform;

    }

    public void initializeCameraMount(Transform attachNewObjectForCameraOnThisInputTransform, Vector3 offset = new Vector3())
    {
        cameraMount = new GameObject("cameraMount in initializeCamera(), playable2 script").transform;

        //Debug.Log("tankBarrel:  " + tankBarrel);
        cameraMount.transform.SetParent(attachNewObjectForCameraOnThisInputTransform, false); //has to be child of ENACTION point for this body!  because THAT is the point which the gamepad rotates!!!
        cameraMount.transform.position += offset;
    }


    public void plugIntoGamepadIfThereIsOne()
    {
        virtualGamepad gamepad = gameObject.GetComponent<virtualGamepad>();
        if (gamepad == null)
        {
            Debug.Log("gamepad == null for:  " + this.gameObject.name);
            return;
        }

        playAsPlayable2(gamepad);

    }

    public void playAs(virtualGamepad gamepad)
    {
        //better way, just plug whole thing in, lol
        occupied = true;
        gamepad.playingAs = this;
    }

    public void playAsPlayable2(virtualGamepad gamepad)
    {
        if (occupied == true) { return; }
        //Debug.Log("equipping/occupying this:  " + this.gameObject + "for this object's gamepad:  " + gamepad.gameObject); 
        occupied = true;

        //Debug.Log("4444444444444444444444444444444444444444444");
        //Debug.Log("is cameraMount  null:  " + cameraMount + "  for this object:  " + this.gameObject.name);
        //Debug.Log("is gamepad.theCamera null:  " + gamepad.theCamera + "  for this object:  " + this.gameObject.name);
        if (cameraMount != null && gamepad.theCamera != null)
        {

            gamepad.theCamera.transform.SetParent(cameraMount, false);
        }

        //Debug.Log("5555555555555555555555555555555555555555555");
        gamepad.updateALLGamepadButtonsFromplayable2(this);

    }

    public void UNplayAsPlayable2(virtualGamepad gamepad)
    {
        //redundant?  entering a different playable2 already overwrites everything?
        //what about exiting a vehicle back into your own body?

        //right now i'm using the function in "equippable2" instead?  and virtual gamepad?
        //Debug.Log("unequip, for this object:  " + gamepad.gameObject);



        occupied = false;


        foreach (IEnactaBool enactaBool in this.GetComponents<IEnactaBool>())
        {
            enactaBool.enactionAuthor = null;
            gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = null;
        }



        foreach (IEnactaVector enactaV in this.GetComponents<IEnactaVector>())
        {
            //enactaV.enactionAuthor = gamepad.transform.gameObject;
            gamepad.allCurrentVectorEnactables[enactaV.gamepadButtonType] = null;
        }

        foreach (IEnactByTargetVector enactaTargetV in this.GetComponents<IEnactByTargetVector>())
        {
            if (gamepad.allCurrentTARGETbyVectorEnactables.Contains(enactaTargetV))
            {

                Debug.Log("here?????????$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                gamepad.allCurrentTARGETbyVectorEnactables.Remove(enactaTargetV);
            }
        }
    }

    internal void clearTheEquipperSlot(interactionCreator.simpleSlot theequippable2Type)
    {
        if (equipperSlotsAndContents[theequippable2Type] != null)
        {
            GameObject item1 = equipperSlotsAndContents[theequippable2Type];
            equippable2 equip = item1.GetComponent<equippable2>();
            equip.unequip(this);
        }
    }


    internal void refilUnusedSlotsAndButtonsFromThisplayable2()
    {
        //equip ONLY those things that are not blocked by other things in those equipper slots or buttons

        virtualGamepad gamepad = this.gameObject.GetComponent<virtualGamepad>();
        if (gamepad == null) { return; }


        foreach (IEnactaBool enactaBool in this.GetComponents<IEnactaBool>())
        {
            if (gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] != null) { continue; }
            enactaBool.enactionAuthor = gamepad.transform.gameObject;
            gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
        }


        foreach (IEnactaVector enactaV in this.GetComponents<IEnactaVector>())
        {
            if (gamepad.allCurrentVectorEnactables[enactaV.gamepadButtonType] != null) { continue; }
            gamepad.allCurrentVectorEnactables[enactaV.gamepadButtonType] = enactaV;
        }

        //hmmm, no idea what to do with these for now, should be fine without for now:
        /*

        List<IEnactByTargetVector> newListToPreventShallowCopyError = new List<IEnactByTargetVector>();
        foreach (IEnactByTargetVector enactByTargetVector in enactableTARGETVectorSet)
        {
            newListToPreventShallowCopyError.Add(enactByTargetVector);
        }


        gamepad.allCurrentTARGETbyVectorEnactables.Clear();
        gamepad.allCurrentTARGETbyVectorEnactables = newListToPreventShallowCopyError;

        */

    }














    internal void replace(GameObject equippable2)
    {

        virtualGamepad gamepad = gameObject.GetComponent<virtualGamepad>();


        //look at all spaces [buttons AND equipper slots] that this equippable2 was taking up
        //find what things the ...playable2 has to fill those by default, and fill them........[seems anoying.....]
        //orr.....just.....ignore the equippable2, and have a function to equip only those things that are not blocked?  will be similar...?
        //sure, that seems more general

        //so.  
        refilUnusedSlotsAndButtonsFromThisplayable2();
    }




    public void printEnactables()
    {
        //enactableBoolSet = new List<IEnactaBool>();
        //enactableVectorSet = new List<IEnactaVector>();
        //enactableTARGETVectorSet = new List<IEnactByTargetVector>();

        string printout = "printEnactables:  " ;
        foreach(var x in this.GetComponents<IEnactaBool>())
        {
            printout += x.ToString();
            printout += ", ";
        }
        foreach (var x in this.GetComponents<IEnactaVector>())
        {
            printout += x.ToString();
            printout += ", ";
        }
        foreach (var x in this.GetComponents<IEnactByTargetVector>())
        {
            printout += x.ToString();
            printout += ", ";
        }

        Debug.Log(printout);
    }
    public void printSlots()
    {
        //equipperSlotsAndContents = new Dictionary<interactionCreator.simpleSlot, equippable2>();


        string printout = "printSlots:  ";
        foreach (var x in equipperSlotsAndContents.Keys)
        {
            printout += x.ToString();
            printout += " (";
            if(equipperSlotsAndContents[x] == null)
            {
                printout += "null/(empty)";
            }
            else
            {
                printout += equipperSlotsAndContents[x].ToString();
            }

            printout += "),  ";
        }

        Debug.Log(printout);
    }
    public void printInteractions()
    {
        //dictOfInteractions
        //Dictionary<interType, List<Ieffect>>


        string printout = "printInteractions:  ";
        foreach (var x in dictOfInteractions.Keys)
        {
            printout += x.ToString();

            printout += ":  (";
            foreach (var y in dictOfInteractions[x])
            {
                printout += y.ToString();

                printout += ", ";
            }
            printout += "),  ";
        }

        Debug.Log(printout);
    }



}