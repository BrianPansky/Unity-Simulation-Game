using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static equippableSetup;

public class inventory1 : MonoBehaviour, IInteractable
{

    public Dictionary<interType, List<Ieffect>> dictOfInteractions { get; set; }


    //Debug.Log("hello:  " + this);
    //              helloTest h1 = new helloTest();  //will helloTest's "awake" be called before "inventory1" awake?!?!?!

    //public List<string> testInventory1 = new List<string>();

    public List<equippable> storedEquippables = new List<equippable>();

    void Awake()
    {

        playable thePlayable = this.gameObject.GetComponent<playable>();
        if (thePlayable == null)
        {
            Debug.Log("this shouldn't be null, on this object:  " + this.gameObject);
        }

        thePlayable.enactableBoolSet.Add(new takeFromAndPutBackIntoInventory(this.gameObject));

        //foreach (IEnactaBool enactaBool in thePlayable.enactableBoolSet)
        {
            //Debug.Log(enactaBool);
        }

        //.....this looks like a bad way to do things:
        //              thePlayable.updateALLGamepadButtonsFromPlayable(this.gameObject.GetComponent<virtualGamepad>());
    }
    
    void Start()
    {



    }




    public void putInInventory(GameObject theObjectToPutInInventory)
    {
        //should i just be storing a game object?  because maybe someday an inventory item won't be equippable?
        //i'm doing this so it's easy to write code to equip it.  can worry about edge cases later.
        equippable theEquippable = theObjectToPutInInventory.GetComponent<equippable>();
        putInInventory(theEquippable);

        /*

        storedEquippables.Add(theEquippable);

        //dockThisToOtherObject(theAuthorScript.enacting.enactionAuthor, (1.5f * theAuthorScript.enacting.enactionAuthor.transform.forward) + new Vector3(0.9f, 0.2f, 0));
        interactionCreator.singleton.dockXToY(theObjectToPutInInventory, this.gameObject);
        theEquippable.gameObject.SetActive(false);


        */



        //Debug.Log("will it put in inventory???");
        //virtualGamepad gamepad = theAuthorScript.enacting.enactionAuthor.GetComponent<virtualGamepad>();
        //playable thePlayable = theAuthorScript.enacting.enactionAuthor.GetComponent<playable>();
        //gun1 theGun = this.GetComponent<gun1>();
        //if (theGun.occupied == true) { return; }
        //          theGun.equipThisEquippable(thePlayable);


        //          interactionCreator.singleton.makeThisObjectDisappear(theObjectToPutInInventory);
        //dockThisToOtherObject(thePlayable.enactionPoint1, new Vector3(0.6f, 0.1f, 0));
    }


    public void putInInventory(equippable theEquippable)
    {
        //should i just be storing a game object?  because maybe someday an inventory item won't be equippable?
        //i'm doing this so it's easy to write code to equip it.  can worry about edge cases later.
        //equippable theEquippable = theObjectToPutInInventory.GetComponent<equippable>();
        storedEquippables.Add(theEquippable);

        //dockThisToOtherObject(theAuthorScript.enacting.enactionAuthor, (1.5f * theAuthorScript.enacting.enactionAuthor.transform.forward) + new Vector3(0.9f, 0.2f, 0));
        interactionCreator.singleton.dockXToY(theEquippable.gameObject, this.gameObject);
        theEquippable.gameObject.SetActive(false);






        //Debug.Log("will it put in inventory???");
        //virtualGamepad gamepad = theAuthorScript.enacting.enactionAuthor.GetComponent<virtualGamepad>();
        //playable thePlayable = theAuthorScript.enacting.enactionAuthor.GetComponent<playable>();
        //gun1 theGun = this.GetComponent<gun1>();
        //if (theGun.occupied == true) { return; }
        //          theGun.equipThisEquippable(thePlayable);


        //          interactionCreator.singleton.makeThisObjectDisappear(theObjectToPutInInventory);
        //dockThisToOtherObject(thePlayable.enactionPoint1, new Vector3(0.6f, 0.1f, 0));
    }


}

public class equippable : MonoBehaviour, IInteractable
{

    public Dictionary<interType, List<Ieffect>> dictOfInteractions { get; set; }

    //......should thes be an IEnactabool with toggling equipped/unequipped as the enaction????  how else am i doing it again???
    public interactionCreator.simpleSlot theEquippableType;


    public List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
    //public List<gamePadButtonable> enactableSet = new List<gamePadButtonable>();
    public List<IEnactaVector> enactableVectorSet = new List<IEnactaVector>();
    public List<IEnactByTargetVector> enactableTARGETVectorSet = new List<IEnactByTargetVector>();

    //aren't i supposed to NOT have these references to other scripts floating around???
    //          public interactionScript theInteractionScript;  //shouldn't this be in the IEnactaBools???

    public void callableAwake()
    {
        //gotta tag it!  err....maybe not so vitally important...?
        //ohhhh, right, i meant add the INTERACTION....to be picked up....hmm....i mean,
        //maybe just do a "get component" in interactions....but...but no, interaction script handles collision interactions,
        //so i would still NEED an interaction script for this.................
        //well, looks like i HAVE this code in "gun1", just move it HERE, simple.
        //      theInteractionScript = genGen.singleton.ensureInteractionScript(this.gameObject);


        //ya, obviously this should go here:
        //     List<Ieffect> thing = new List<Ieffect>();
        //thing.Add(interactionScript.effect.equip);

        //well, change to a "put in inventory" effect?  probably, ya.
        //i dunno, i don't really like this.  an enum for "put in inventory"???  what if i just made ALL objects [of some type]
        //have required/interface functions for "what to do if this get's clicked"?  right?
        //buuut i've got this system for now.............
        //      thing.Add(new putInInventory());
        //theInteractionScript.dictOfInteractions[interType.standardClick] = thing;

        dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.standardClick, new putInInventory());

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

        equipThisEquippable(thePlayable);

    }



    public void equipThisEquippable(playable thePlayable)
    {
        //Debug.Log("trying to equip...");

        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:
        if (thePlayable.equipperSlotsAndContents.ContainsKey(theEquippableType) == false)
        {
            Debug.Log("can't, thePlayable.equipperSlotsAndContents.ContainsKey(theEquippableType) == false, the type is:  " + theEquippableType);
            return;
        }

        equipToEquipperSlots(thePlayable);
        equipToGamepadButtons(thePlayable);
        equipVisiblyToLocation(thePlayable.enactionPoint1);

    }

    private void equipVisiblyToLocation(GameObject locationObjectToEquipTo)
    {
        interactionCreator.singleton.dockXToY(this.gameObject, locationObjectToEquipTo, new Vector3(0.47f, 0.2f, 0.4f));
        this.gameObject.SetActive(true);
    }

    private void equipToGamepadButtons(playable thePlayable)
    {

        //super ad hoc for now:
        virtualGamepad gamepad = thePlayable.transform.gameObject.GetComponent<virtualGamepad>();
        gamepad.receiveAnyNonNullEnactionsForButtons(this);
    }

    private void equipToEquipperSlots(playable thePlayable)
    {
        //Debug.Log("111111111111111-------------thePlayable.equipperSlotsAndContents[theEquippableType] == null ? :  " + thePlayable.equipperSlotsAndContents[theEquippableType]);

        thePlayable.clearTheEquipperSlot(theEquippableType);
        thePlayable.equipperSlotsAndContents[theEquippableType] = this;

        //Debug.Log("222222222222222-------------thePlayable.equipperSlotsAndContents[theEquippableType] == null ? :  " + thePlayable.equipperSlotsAndContents[theEquippableType]);

    }


    public void unequip(playable thePlayable)
    {
        Debug.Log("trying to UNequip... for this object:  " + thePlayable.gameObject);
        //Debug.Log("trying to UNequip...");

        thePlayable.equipperSlotsAndContents[theEquippableType] = null;


        //Debug.Log("after thePlayable.equipperSlotsAndContents[theEquippableType]" + thePlayable.equipperSlotsAndContents[theEquippableType]);



        //super ad hoc for now:
        virtualGamepad gamepad = thePlayable.transform.gameObject.GetComponent<virtualGamepad>();
        gamepad.removeFromGamepadButtons(this);
        //      thePlayable.replace(this);
        thePlayable.refilUnusedSlotsAndButtonsFromThisPlayable();


        disappearIntoPocket(thePlayable.gameObject);

    }

    private void disappearIntoPocket(GameObject thePocketObject)
    {
        interactionCreator.singleton.dockXToY(this.gameObject, thePocketObject);
        this.gameObject.SetActive(false);
    }

}

public class takeFromAndPutBackIntoInventory : IEnactaBool
{
    public buttonCategories gamepadButtonType { get; set; }
    //public interactionInfo interInfo { get; set; }  //hmmm.......
    public GameObject enactionAuthor { get; set; }

    public inventory1 theInventory;
    public playable thePlayable;

    public interactionCreator.simpleSlot theSlotTypeToCycle; //[typically "hands"]


    public takeFromAndPutBackIntoInventory(GameObject theObjectWithAnInventory, interactionCreator.simpleSlot theSlotTypeToCycle = interactionCreator.simpleSlot.hands)
    {
        gamepadButtonType = buttonCategories.augment1;
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

        //interInfo = new interactionInfo(interType.self, 1f);
    }

    public void enact()
    {
        
        equippable theEquippableToPutAway = thePlayable.equipperSlotsAndContents[theSlotTypeToCycle]; 
        //Debug.Log("::::::::::::::::::::::::::::::thePlayable.equipperSlotsAndContents[theSlotTypeToCycle] == null ? :  " + thePlayable.equipperSlotsAndContents[theSlotTypeToCycle]);

        //Debug.Log("::::::::::::::::::::::::::::::theEquippableToPutAway == null ? :  " + theEquippableToPutAway);

        if (theEquippableToPutAway == null)
        {

            //Debug.Log("trying to equip something from inventory");
            if (theInventory.storedEquippables.Count < 1) { return; }

            //Debug.Log("so far so good");
            //theInventory.storedEquippables[0].equipThisEquippable(thePlayable);
            theInventory.storedEquippables[0].equipThisEquippable(thePlayable);
            theInventory.storedEquippables.RemoveAt(0);
        }
        else
        {
            //Debug.Log("trying to puy away something into inventory");
            
            theEquippableToPutAway.unequip(thePlayable);
            theInventory.putInInventory(theEquippableToPutAway);
            //theInventory.storedEquippables[0].unequip(thePlayable);
            //rotateInventoryList();
            //theInventory.putInInventory();
            
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
        theInventory.storedEquippables[0].equipThisEquippable(thePlayable);
    }


}

