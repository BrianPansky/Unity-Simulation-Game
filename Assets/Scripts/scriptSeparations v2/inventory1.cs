using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;

public class inventory1 : MonoBehaviour
{

    //public List<string> testInventory1 = new List<string>();

    public List<equippable> storedEquippables = new List<equippable>();

    void Awake()
    {
        //need something blank so i can unequip things:
        //storedEquippables.Add(new equippable());
    }

    void Start()
    {


        playable thePlayable = this.gameObject.GetComponent<playable>();

        if (thePlayable == null)
        {
            Debug.Log("this shouldn't be null");
        }

        thePlayable.enactableBoolSet.Add(new cycleInventory(this.gameObject));

        foreach (IEnactaBool enactaBool in thePlayable.enactableBoolSet)
        {
            //Debug.Log(enactaBool);
        }

        //.....this looks like a bad way to do things:
        thePlayable.updateAnyGamepadButtons(this.gameObject.GetComponent<virtualGamepad>());

        //foreach (var item in testInventory1)
        {
            //theObjectBeingInteractedWith.GetComponent<inventory1>().testInventory1.Add("testKey1");
            //if (item == "testKey1")
            {
                //Debug.Log("yesssssssssssssssssssssssss");
            }
        }
    }




    public void putInInventory(GameObject theObjectToPutInInventory)
    {
        Debug.Log("will it put in inventory???");
        //virtualGamepad gamepad = theAuthorScript.enacting.interInfo.enactionAuthor.GetComponent<virtualGamepad>();
        //playable thePlayable = theAuthorScript.enacting.interInfo.enactionAuthor.GetComponent<playable>();
        //gun1 theGun = this.GetComponent<gun1>();
        //if (theGun.occupied == true) { return; }
        //          theGun.equip(thePlayable);

        //should i just be storing a game object?  because maybe someday an inventory item won't be equippable?
        //i'm doing this so it's easy to write code to equip it.  can worry about edge cases later.
        equippable theEquippable = theObjectToPutInInventory.GetComponent<equippable>();
        storedEquippables.Add(theEquippable);

        //dockThisToOtherObject(theAuthorScript.enacting.interInfo.enactionAuthor, (1.5f * theAuthorScript.enacting.interInfo.enactionAuthor.transform.forward) + new Vector3(0.9f, 0.2f, 0));
        interactionCreator.singleton.dockXToY(theObjectToPutInInventory, this.gameObject);
        theEquippable.gameObject.SetActive(false);
        //          interactionCreator.singleton.makeThisObjectDisappear(theObjectToPutInInventory);
        //dockThisToOtherObject(thePlayable.enactionPoint1, new Vector3(0.6f, 0.1f, 0));
    }

}

public class equippable : MonoBehaviour
{
    public interactionCreator.simpleSlot theEquippableType;

    public List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
    public List<IEnactaVector> enactableVectorSet = new List<IEnactaVector>();
    public List<IEnactByTargetVector> enactableTARGETVectorSet = new List<IEnactByTargetVector>();

    //attach to objects/entities you can "play as" [such as bodies and vehicles]
    //weapons and items too

    //aren't i supposed to NOT have these references to other scripts floating around???
    public interactionScript theInteractionScript;

    public void callableAwake()
    {
        //gotta tag it!  err....maybe not so vitally important...?
        //ohhhh, right, i meant add the INTERACTION....to be picked up....hmm....i mean,
        //maybe just do a "get component" in interactions....but...but no, interaction script handles collision interactions,
        //so i would still NEED an interaction script for this.................
        //well, looks like i HAVE this code in "gun1", just move it HERE, simple.
        if (theInteractionScript == null)
        {
            theInteractionScript = this.gameObject.GetComponent<interactionScript>();
            if (theInteractionScript == null)
            {
                theInteractionScript = this.gameObject.AddComponent<interactionScript>();
            }
            theInteractionScript.dictOfInteractions = new Dictionary<interType, List<interactionScript.effect>>();//new Dictionary<string, List<string>>(); //for some reason it was saying it already had that key in it, but it should be blank.  so MAKING it blank.
        }





        //ya, obviously this should go here:
        List<interactionScript.effect> thing = new List<interactionScript.effect>();
        //thing.Add(interactionScript.effect.equip);

        //well, change to a "put in inventory" effect?  probably, ya.
        //i dunno, i don't really like this.  an enum for "put in inventory"???  what if i just made ALL objects [of some type]
        //have required/interface functions for "what to do if this get's clicked"?  right?
        //buuut i've got this system for now.............
        thing.Add(interactionScript.effect.putInInventory);
        theInteractionScript.dictOfInteractions[interType.standardClick] = thing;

        //Debug.Log("equippable awake");


    }

    //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:



    public void plugIntoGamepadIfThereIsOne()
    {

        playable thePlayable = this.gameObject.GetComponent<playable>();
        if (thePlayable == null)
        {
            Debug.Log("thePlayable == null for:  " + this.gameObject.name);
            return;
        }

        equip(thePlayable);

    }



    public void equip(playable thePlayable)
    {
        Debug.Log("trying to equip...");

        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:
        if (thePlayable.equipperSlotsAndContents.ContainsKey(theEquippableType) == false)
        {
            Debug.Log("can't, thePlayable.equipperSlotsAndContents.ContainsKey(theEquippableType) == false, the type is:  " + theEquippableType); 
            return; }

        Debug.Log("still good....");


        //Debug.Log("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");return; }

        Debug.Log("before thePlayable.equipperSlotsAndContents[theEquippableType]" + thePlayable.equipperSlotsAndContents[theEquippableType]);

        if (thePlayable.equipperSlotsAndContents[theEquippableType] != null)
        {
            thePlayable.equipperSlotsAndContents[theEquippableType].unequip(thePlayable);
        }

        thePlayable.equipperSlotsAndContents[theEquippableType] = this;


        Debug.Log("after thePlayable.equipperSlotsAndContents[theEquippableType]" + thePlayable.equipperSlotsAndContents[theEquippableType]);



        //super ad hoc for now:
        virtualGamepad gamepad = thePlayable.transform.gameObject.GetComponent<virtualGamepad>();


        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            //this "object is null" error is usually the only kind of error where it isn't clear which variable went wrong
            //and EVERY TIME it's a situation like this, where there are a TON of variables in a single line.
            //so i need to print sooooooo many....
            Debug.Log("1111111111111111111 gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType]:  " + gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType]);
            //Debug.Log("enactaBool.interInfo:  " + enactaBool.interInfo);
            //Debug.Log("enactaBool.interInfo.enactionAuthor:  " + enactaBool.interInfo.enactionAuthor);
            //Debug.Log("gamepad:  " + gamepad);
            //Debug.Log("gamepad.transform:  " + gamepad.transform);
            //ebug.Log("gamepad.transform.gameObject:  " + gamepad.transform.gameObject);
            //          enactaBool.interInfo.enactionAuthor = gamepad.transform.gameObject;
            //          gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
            enactaBool.interInfo.enactionAuthor = gamepad.transform.gameObject;
            gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
            Debug.Log("222222222222222222222 gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType]:  " + gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType]);
        }



        foreach (IEnactaVector enactaV in enactableVectorSet)
        {
            //enactaV.enactionAuthor = gamepad.transform.gameObject;
            //          gamepad.allCurrentVectorEnactables[enactaV.gamepadButtonType] = enactaV;
        }

        //          gamepad.allCurrentTARGETbyVectorEnactables.Clear();
        //          gamepad.allCurrentTARGETbyVectorEnactables = enactableTARGETVectorSet;


        //          if (gamepad.theCamera == null) { return; }

        /*
        Debug.Log(cameraMount);
        if (cameraMount == null)
        {
            defaultCameraMountGenerator();
        }
        Debug.Log(cameraMount);
        gamepad.theCamera.transform.SetParent(cameraMount, false);
        */



        //interactionCreator.singleton.dockXToY(this.gameObject, thePlayable.gameObject);
        interactionCreator.singleton.dockXToY(this.gameObject, thePlayable.enactionPoint1, new Vector3(0.47f, 0.2f, 0.4f));

        this.gameObject.SetActive(true);

    }
    public void unequip(playable thePlayable)
    {


        Debug.Log("trying to UNequip...");

        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:
        //      if (thePlayable.equipperSlotsAndContents.ContainsKey(theEquippableType) == false)
        {
            
        }

        //Debug.Log("still good....");


        //Debug.Log("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");return; }

        //Debug.Log("before thePlayable.equipperSlotsAndContents[theEquippableType]" + thePlayable.equipperSlotsAndContents[theEquippableType]);

        //if (thePlayable.equipperSlotsAndContents[theEquippableType] != null)
        {
            //thePlayable.equipperSlotsAndContents[theEquippableType].unequip();
        }

        thePlayable.equipperSlotsAndContents[theEquippableType] = null;


        //Debug.Log("after thePlayable.equipperSlotsAndContents[theEquippableType]" + thePlayable.equipperSlotsAndContents[theEquippableType]);



        //super ad hoc for now:
        virtualGamepad gamepad = thePlayable.transform.gameObject.GetComponent<virtualGamepad>();


        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            //this "object is null" error is usually the only kind of error where it isn't clear which variable went wrong
            //and EVERY TIME it's a situation like this, where there are a TON of variables in a single line.
            //so i need to print sooooooo many....
            //                  Debug.Log("1111111111111111111 gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType]:  " + gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType]);
            //Debug.Log("enactaBool.interInfo:  " + enactaBool.interInfo);
            //Debug.Log("enactaBool.interInfo.enactionAuthor:  " + enactaBool.interInfo.enactionAuthor);
            //Debug.Log("gamepad:  " + gamepad);
            //Debug.Log("gamepad.transform:  " + gamepad.transform);
            //ebug.Log("gamepad.transform.gameObject:  " + gamepad.transform.gameObject);
            //          enactaBool.interInfo.enactionAuthor = gamepad.transform.gameObject;
            //          gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;




            //                  enactaBool.interInfo.enactionAuthor = gamepad.transform.gameObject;
            //      gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
            gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = thePlayable.enactableBoolSet[0];
            //      Debug.Log("222222222222222222222 gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType]:  " + gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType]);
        }



        foreach (IEnactaVector enactaV in enactableVectorSet)
        {
            //enactaV.enactionAuthor = gamepad.transform.gameObject;
            //          gamepad.allCurrentVectorEnactables[enactaV.gamepadButtonType] = enactaV;
        }

        //          gamepad.allCurrentTARGETbyVectorEnactables.Clear();
        //          gamepad.allCurrentTARGETbyVectorEnactables = enactableTARGETVectorSet;


        //          if (gamepad.theCamera == null) { return; }

        /*
        Debug.Log(cameraMount);
        if (cameraMount == null)
        {
            defaultCameraMountGenerator();
        }
        Debug.Log(cameraMount);
        gamepad.theCamera.transform.SetParent(cameraMount, false);
        */



        //interactionCreator.singleton.dockXToY(this.gameObject, thePlayable.enactionPoint1, new Vector3(0.47f, 0.2f, 0.4f));
        interactionCreator.singleton.dockXToY(this.gameObject, thePlayable.gameObject);

        this.gameObject.SetActive(false);

        /*

        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            enactaBool.interInfo.enactionAuthor = null;
            //          gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = null;
        }



        foreach (IEnactaVector enactaV in enactableVectorSet)
        {
            //enactaV.enactionAuthor = gamepad.transform.gameObject;
            //          gamepad.allCurrentVectorEnactables[enactaV.gamepadButtonType] = null;
        }

        foreach (IEnactByTargetVector enactaTargetV in enactableTARGETVectorSet)
        {
            //          if (gamepad.allCurrentTARGETbyVectorEnactables.Contains(enactaTargetV))
            {
                //          gamepad.allCurrentTARGETbyVectorEnactables.Remove(enactaTargetV);
            }
        }


        */
    }

    public void putInInventory(GameObject theObjectWithTheInventory)
    {
        //virtualGamepad gamepad = theAuthorScript.enacting.interInfo.enactionAuthor.GetComponent<virtualGamepad>();
        //playable thePlayable = theAuthorScript.enacting.interInfo.enactionAuthor.GetComponent<playable>();
        //gun1 theGun = this.GetComponent<gun1>();
        //if (theGun.occupied == true) { return; }
        //          theGun.equip(thePlayable);

        inventory1 theInventory = theObjectWithTheInventory.GetComponent<inventory1>();


        //dockThisToOtherObject(theAuthorScript.enacting.interInfo.enactionAuthor, (1.5f * theAuthorScript.enacting.interInfo.enactionAuthor.transform.forward) + new Vector3(0.9f, 0.2f, 0));
        interactionCreator.singleton.dockXToY(this.gameObject, theObjectWithTheInventory);
        //dockThisToOtherObject(thePlayable.enactionPoint1, new Vector3(0.6f, 0.1f, 0));
    }


}

public class cycleInventory : IEnactaBool
{
    public virtualGamepad.buttonCategories gamepadButtonType { get; set; }
    public interactionInfo interInfo { get; set; }

    public inventory1 theInventory;
    public playable thePlayable;

    public interactionCreator.simpleSlot theSlotTypeToCycle; //[typically "hands"]


    public cycleInventory(GameObject theObjectWithAnInventory, interactionCreator.simpleSlot theSlotTypeToCycle = interactionCreator.simpleSlot.hands)
    {
        gamepadButtonType = virtualGamepad.buttonCategories.augment1;
        this.theSlotTypeToCycle = theSlotTypeToCycle;
        theInventory = theObjectWithAnInventory.GetComponent<inventory1>();
        if (theInventory == null)
        {
            theInventory = theObjectWithAnInventory.AddComponent<inventory1>();
        }

        thePlayable = theObjectWithAnInventory.GetComponent<playable>();
        if (thePlayable == null)
        {
            thePlayable = theObjectWithAnInventory.AddComponent<playable>();
        }

        interInfo = new interactionInfo(interType.self, 1f);
    }

    public void enact()
    {
        if(theInventory.storedEquippables.Count < 1) { return; }

        if (thePlayable.equipperSlotsAndContents[theSlotTypeToCycle] == null)
        {
            Debug.Log("trying to equip something from inventory");

            if (theInventory.storedEquippables.Count < 1)
            {

                Debug.Log("......can't, theInventory.storedEquippables.Count < 1"); return;
            }
            Debug.Log("so far so good");
            //theInventory.storedEquippables[0].equip(thePlayable);
            theInventory.storedEquippables[0].equip(thePlayable);
        }
        else
        {
            theInventory.storedEquippables[0].unequip(thePlayable);
            rotateInventoryList();
            
        }
        
    }

    void rotateInventoryList()
    {
        if (theInventory.storedEquippables.Count < 2) { return; }

        //remove one from end, put it at start:
        //equippable endOne = theInventory.storedEquippables.FindLast();
        //orrr easier to remove FIRST one, and ADD it to the list [which will put it at end i think?]
        equippable firstOne = theInventory.storedEquippables[0];
        theInventory.storedEquippables.RemoveAt(0);
        theInventory.storedEquippables.Add(firstOne);

    }

    void equipFirstItemInInventoryList()
    {
        if (theInventory.storedEquippables.Count < 1) { return; }
        theInventory.storedEquippables[0].equip(thePlayable);
    }


}

