using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;

public class equippable2 : stateHolder, IInteractable
{
    //plug in peices to make each specific weapon/item


    public GameObject enactionPoint1;

    public Dictionary<interType, List<Ieffect>> dictOfInteractions { get; set; }

    //......should thes be an IEnactabool with toggling equipped/unequipped as the enaction????  how else am i doing it again???
    public interactionCreator.simpleSlot theequippable2Type;

    /*
    public List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
    //public List<gamePadButtonable> enactableSet = new List<gamePadButtonable>();
    public List<IEnactaVector> enactableVectorSet = new List<IEnactaVector>();
    public List<IEnactByTargetVector> enactableTARGETVectorSet = new List<IEnactByTargetVector>();
    */

    //aren't i supposed to NOT have these references to other scripts floating around???
    //          public interactionScript theInteractionScript;  //shouldn't this be in the IEnactaBools???


    //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:



    public static void genEquippable2()
    {

    }

    void Awake()
    {
        //needs to be in awake, otherwise noy properly initialized if it is put in inventory [and thus disabled] immediately upon its creation...
        theequippable2Type = interactionCreator.simpleSlot.hands;


        if (tagging2.singleton == null)
        {
            Debug.Log("tagging2.singleton is null, cannot use it to add tags.  this happens for objects that have been added to the scene using the editor, because they exist before any scripts are called.  solution:  do not add prefabs to scene using editor.  generate them after singletons have initialized.");
        }

        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.interactable);
        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.equippable2);
        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.zoneable);


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

        Debug.Log("equippable2, add interType.standardClick");
        dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.standardClick, new putInInventory());

        //Debug.Log("equippable2 awake");


    }

    

    // Start is called before the first frame update
    void Start()
    {
        //tag self in start or awake


        initializeEnactionPoint1();



    }


    void initializeEnactionPoint1()
    {
        enactionPoint1 = new GameObject("enactionPoint1 in initializeEnactionPoint1() line 91, equippable2 script");
        enactionPoint1.transform.parent = transform;
        enactionPoint1.transform.position = this.transform.position + this.transform.forward * 0.2f + this.transform.up * 0.3f;
        enactionPoint1.transform.rotation = this.transform.rotation;

    }

    public void plugIntoGamepadIfThereIsOne()
    {

        playable2 thePlayable2 = this.gameObject.GetComponent<playable2>();
        if (thePlayable2 == null)
        {
            Debug.Log("thePlayable2 == null for:  " + this.gameObject.name);
            return;
        }

        equipThisequippable2(thePlayable2);

    }



    public void equipThisequippable2(playable2 thePlayable2)
    {
        //Debug.Log("trying to equip...");

        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:
        if (thePlayable2.equipperSlotsAndContents.ContainsKey(theequippable2Type) == false)
        {
            Debug.Log("can't, thePlayable2.equipperSlotsAndContents.ContainsKey(theequippable2Type) == false, the type is:  " + theequippable2Type);
            return;
        }

        equipToEquipperSlots(thePlayable2);
        equipToGamepadButtons(thePlayable2);
        equipVisiblyToLocation(thePlayable2.enactionPoint1);

    }

    private void equipVisiblyToLocation(GameObject locationObjectToEquipTo)
    {
        interactionCreator.singleton.dockXToY(this.gameObject, locationObjectToEquipTo, new Vector3(0.47f, 0.2f, 0.4f));
        this.gameObject.SetActive(true);
    }

    private void equipToGamepadButtons(playable2 thePlayable2)
    {

        //super ad hoc for now:
        virtualGamepad gamepad = thePlayable2.transform.gameObject.GetComponent<virtualGamepad>();
        gamepad.receiveAnyNonNullEnactionsForButtons(this);
    }

    private void equipToEquipperSlots(playable2 thePlayable2)
    {
        //Debug.Log("111111111111111-------------thePlayable2.equipperSlotsAndContents[theequippable2Type] == null ? :  " + thePlayable2.equipperSlotsAndContents[theequippable2Type]);

        thePlayable2.clearTheEquipperSlot(theequippable2Type);
        thePlayable2.equipperSlotsAndContents[theequippable2Type] = this;

        //Debug.Log("222222222222222-------------thePlayable2.equipperSlotsAndContents[theequippable2Type] == null ? :  " + thePlayable2.equipperSlotsAndContents[theequippable2Type]);

    }


    public void unequip(playable2 thePlayable2)
    {
        //Debug.Log("trying to UNequip... for this object:  " + thePlayable2.gameObject);
        //Debug.Log("trying to UNequip...");

        thePlayable2.equipperSlotsAndContents[theequippable2Type] = null;


        //Debug.Log("after thePlayable2.equipperSlotsAndContents[theequippable2Type]" + thePlayable2.equipperSlotsAndContents[theequippable2Type]);



        //super ad hoc for now:
        virtualGamepad gamepad = thePlayable2.transform.gameObject.GetComponent<virtualGamepad>();
        gamepad.removeFromGamepadButtons(this);
        //      thePlayable2.replace(this);
        thePlayable2.refilUnusedSlotsAndButtonsFromThisplayable2();


        disappearIntoPocket(thePlayable2.gameObject);

    }

    private void disappearIntoPocket(GameObject thePocketObject)
    {
        interactionCreator.singleton.dockXToY(this.gameObject, thePocketObject);
        this.gameObject.SetActive(false);
    }

    internal bool containsIntertype(interType intertypeX)
    {
        foreach (collisionEnaction thisEnaction in this.gameObject.GetComponents<collisionEnaction>())
        {
            if (thisEnaction.interInfo.interactionType == intertypeX) { return true; }
        }

        /*
        foreach(IEnactaBool thisEnaction in enactableBoolSet)
        {
            if(thisEnaction.GetType() != collisionEnaction) { continue; }
            collisionEnaction cE = (collisionEnaction)thisEnaction;
            if (thisEnaction.interInfo.interactionType == intertypeX) { return true;}
        }
        */
        return false;
    }
}
