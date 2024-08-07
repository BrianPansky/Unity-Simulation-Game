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
    public interactionCreator.simpleSlot theEquippable2Type;


    //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:



    public static void genEquippable2()
    {

    }

    public void test()
    {

    }

    public void Awake()
    {
        Debug.Log("equippable2 awake");
        //needs to be in awake, otherwise noy properly initialized if it is put in inventory [and thus disabled] immediately upon its creation...
        theEquippable2Type = interactionCreator.simpleSlot.hands;


        if (tagging2.singleton == null)
        {
            Debug.Log("tagging2.singleton is null, cannot use it to add tags.  this happens for objects that have been added to the scene using the editor, because they exist before any scripts are called.  solution:  do not add prefabs to scene using editor.  generate them after singletons have initialized.");
        }

        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.interactable);
        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.equippable2);
        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.zoneable);

        dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.standardClick, new putInInventory());

    }

    // Start is called before the first frame update
    void Start()
    {
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
        if (thePlayable2.equipperSlotsAndContents.ContainsKey(theEquippable2Type) == false)
        {
            Debug.Log("can't, thePlayable2.equipperSlotsAndContents.ContainsKey(theEquippable2Type) == false, the type is:  " + theEquippable2Type);
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
        //Debug.Log("111111111111111-------------thePlayable2.equipperSlotsAndContents[theEquippable2Type] == null ? :  " + thePlayable2.equipperSlotsAndContents[theEquippable2Type]);

        thePlayable2.clearTheEquipperSlot(theEquippable2Type);
        thePlayable2.equipperSlotsAndContents[theEquippable2Type] = this.gameObject;
    }


    public void unequip(playable2 thePlayable2)
    {
        thePlayable2.equipperSlotsAndContents[theEquippable2Type] = null;

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

        return false;
    }
}
