using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static equippable2Setup;

public class inventory1 : interactable2
{
    public GameObject startingItem;
    public List<GameObject> inventoryItems = new List<GameObject>();

    void Awake()
    {
        playable2 thePlayable2 = this.gameObject.GetComponent<playable2>();
        if (thePlayable2 == null)
        {
            Debug.Log("this shouldn't be null, on this object:  " + this.gameObject);
        }

        takeFromAndPutBackIntoInventory.addTakeFromAndPutBackIntoInventory(this.gameObject);
        //startingItem = genGen.singleton.returnGun1(this.transform.position);
    }
    
    void Start()
    {

        if (startingItem == null) { return; }
        putInInventory(startingItem);
    }





    public void putInInventory(GameObject theObjectToPutInInventory)
    {
        if(theObjectToPutInInventory == null) { Debug.Log("theObjectToPutInInventory == null"); return; }
        //should i just be storing a game object?  because maybe someday an inventory item won't be equippable2?
        //i'm doing this so it's easy to write code to equip it.  can worry about edge cases later.
        inventoryItems.Add(theObjectToPutInInventory);

        interactionCreator.singleton.dockXToY(theObjectToPutInInventory, this.gameObject);
        theObjectToPutInInventory.SetActive(false);
    }



    
}

public class takeFromAndPutBackIntoInventory : IEnactaBool
{
    public inventory1 theInventory;
    public playable2 thePlayable2;

    public interactionCreator.simpleSlot theSlotTypeToCycle; //[typically "hands"]

    public static void addTakeFromAndPutBackIntoInventory(GameObject theObjectWithAnInventory, interactionCreator.simpleSlot theSlotTypeToCycle = interactionCreator.simpleSlot.hands)
    {
        takeFromAndPutBackIntoInventory enactionComponent = theObjectWithAnInventory.AddComponent<takeFromAndPutBackIntoInventory>();

        enactionComponent.gamepadButtonType = buttonCategories.augment1;
        enactionComponent.theSlotTypeToCycle = theSlotTypeToCycle;
        enactionComponent.theInventory = theObjectWithAnInventory.GetComponent<inventory1>();
        if (enactionComponent.theInventory == null)
        {
            enactionComponent.theInventory = theObjectWithAnInventory.AddComponent<inventory1>();
        }

        enactionComponent.thePlayable2 = theObjectWithAnInventory.GetComponent<playable2>();
        if (enactionComponent.thePlayable2 == null)
        {
            enactionComponent.thePlayable2 = theObjectWithAnInventory.AddComponent<playable2>();  //???
        }
    }



    public takeFromAndPutBackIntoInventory(GameObject theObjectWithAnInventory, interactionCreator.simpleSlot theSlotTypeToCycle = interactionCreator.simpleSlot.hands)
    {
        gamepadButtonType = buttonCategories.augment1;
        this.theSlotTypeToCycle = theSlotTypeToCycle;
        theInventory = theObjectWithAnInventory.GetComponent<inventory1>();
        if (theInventory == null)
        {
            theInventory = theObjectWithAnInventory.AddComponent<inventory1>();
        }

        thePlayable2 = theObjectWithAnInventory.GetComponent<playable2>();
        if (thePlayable2 == null)
        {
            thePlayable2 = theObjectWithAnInventory.AddComponent<playable2>();
        }
    }

    override public void enact(inputData theInput)
    {
        enactJustThisIndividualEnaction(theInput);
        enactAllLinkedEnactionAtoms(theInput);
    }

    void rotateInventoryList()
    {
        if (theInventory.inventoryItems.Count < 2) { return; }

        GameObject item1 = theInventory.inventoryItems[0];
        theInventory.inventoryItems.RemoveAt(0);
        theInventory.inventoryItems.Add(item1);
    }

    void equipFirstItemInInventoryList()
    {
        if (theInventory.inventoryItems.Count < 1) { return; }
        GameObject item1 = theInventory.inventoryItems[0];
        equippable2 equip = item1.GetComponent<equippable2>();
        equip.equipThisequippable2(thePlayable2);
    }

    public override void enactJustThisIndividualEnaction(inputData theInput)
    {
        //Debug.Log("YES WE ARE TRYING TO ENACT THE EQUIP ENACTION");
        GameObject theequippable2ToPutAway = thePlayable2.equipperSlotsAndContents[theSlotTypeToCycle];
        if (theequippable2ToPutAway == null)
        {
            if (theInventory.inventoryItems.Count < 1) { return; }

            GameObject item1 = theInventory.inventoryItems[0];
            equippable2 equip = item1.GetComponent<equippable2>();
            equip.equipThisequippable2(thePlayable2);
            theInventory.inventoryItems.RemoveAt(0);

        }
        else
        {
            equippable2 equip = theequippable2ToPutAway.GetComponent<equippable2>();
            equip.unequip(thePlayable2);
            theInventory.putInInventory(theequippable2ToPutAway);
        }
    }
}


/*
public class equipSpecificItemFromInventory : IEnactaBool
{
    public inventory1 theInventory;
    public playable2 thePlayable2;

    public interactionCreator.simpleSlot theSlotTypeToCycle; //[typically "hands"]

    public static void addEquipSpecificItemFromInventory(GameObject theObjectWithAnInventory, interactionCreator.simpleSlot theSlotTypeToCycle = interactionCreator.simpleSlot.hands)
    {
        takeFromAndPutBackIntoInventory enactionComponent = theObjectWithAnInventory.AddComponent<takeFromAndPutBackIntoInventory>();

        enactionComponent.gamepadButtonType = buttonCategories.augment1;
        enactionComponent.theSlotTypeToCycle = theSlotTypeToCycle;
        enactionComponent.theInventory = theObjectWithAnInventory.GetComponent<inventory1>();
        if (enactionComponent.theInventory == null)
        {
            enactionComponent.theInventory = theObjectWithAnInventory.AddComponent<inventory1>();
        }

        enactionComponent.thePlayable2 = theObjectWithAnInventory.GetComponent<playable2>();
        if (enactionComponent.thePlayable2 == null)
        {
            enactionComponent.thePlayable2 = theObjectWithAnInventory.AddComponent<playable2>();  //???
        }
    }



    public equipSpecificItemFromInventory(GameObject theObjectWithAnInventory, interactionCreator.simpleSlot theSlotTypeToCycle = interactionCreator.simpleSlot.hands)
    {
        gamepadButtonType = buttonCategories.augment1;
        this.theSlotTypeToCycle = theSlotTypeToCycle;
        theInventory = theObjectWithAnInventory.GetComponent<inventory1>();
        if (theInventory == null)
        {
            theInventory = theObjectWithAnInventory.AddComponent<inventory1>();
        }

        thePlayable2 = theObjectWithAnInventory.GetComponent<playable2>();
        if (thePlayable2 == null)
        {
            thePlayable2 = theObjectWithAnInventory.AddComponent<playable2>();
        }
    }

    override public void enact(inputData theInput)
    {
        //Debug.Log("YES WE ARE TRYING TO ENACT THE EQUIP ENACTION");
        GameObject theequippable2ToPutAway = thePlayable2.equipperSlotsAndContents[theSlotTypeToCycle];
        if (theequippable2ToPutAway == null)
        {
            if (theInventory.inventoryItems.Count < 1) { return; }

            GameObject item1 = theInventory.inventoryItems[0];
            equippable2 equip = item1.GetComponent<equippable2>();
            equip.equipThisequippable2(thePlayable2);
            theInventory.inventoryItems.RemoveAt(0);

        }
        else
        {
            equippable2 equip = theequippable2ToPutAway.GetComponent<equippable2>();
            equip.unequip(thePlayable2);
            theInventory.putInInventory(theequippable2ToPutAway);
        }

    }


    void equipItem(GameObject itemToEquip)
    {
        equippable2 equip = itemToEquip.GetComponent<equippable2>();
        equip.equipThisequippable2(thePlayable2);
    }


}

public class cycleCurrentInventoryItem : IEnactaBool
{
    public inventory1 theInventory;
    public playable2 thePlayable2;

    public interactionCreator.simpleSlot theSlotTypeToCycle; //[typically "hands"]

    public static void addTakeFromAndPutBackIntoInventory(GameObject theObjectWithAnInventory, interactionCreator.simpleSlot theSlotTypeToCycle = interactionCreator.simpleSlot.hands)
    {
        takeFromAndPutBackIntoInventory enactionComponent = theObjectWithAnInventory.AddComponent<takeFromAndPutBackIntoInventory>();

        enactionComponent.gamepadButtonType = buttonCategories.augment1;
        enactionComponent.theSlotTypeToCycle = theSlotTypeToCycle;
        enactionComponent.theInventory = theObjectWithAnInventory.GetComponent<inventory1>();
        if (enactionComponent.theInventory == null)
        {
            enactionComponent.theInventory = theObjectWithAnInventory.AddComponent<inventory1>();
        }

        enactionComponent.thePlayable2 = theObjectWithAnInventory.GetComponent<playable2>();
        if (enactionComponent.thePlayable2 == null)
        {
            enactionComponent.thePlayable2 = theObjectWithAnInventory.AddComponent<playable2>();  //???
        }
    }



    public takeFromAndPutBackIntoInventory(GameObject theObjectWithAnInventory, interactionCreator.simpleSlot theSlotTypeToCycle = interactionCreator.simpleSlot.hands)
    {
        gamepadButtonType = buttonCategories.augment1;
        this.theSlotTypeToCycle = theSlotTypeToCycle;
        theInventory = theObjectWithAnInventory.GetComponent<inventory1>();
        if (theInventory == null)
        {
            theInventory = theObjectWithAnInventory.AddComponent<inventory1>();
        }

        thePlayable2 = theObjectWithAnInventory.GetComponent<playable2>();
        if (thePlayable2 == null)
        {
            thePlayable2 = theObjectWithAnInventory.AddComponent<playable2>();
        }
    }

    override public void enact(inputData theInput)
    {
        //Debug.Log("YES WE ARE TRYING TO ENACT THE EQUIP ENACTION");
        GameObject theequippable2ToPutAway = thePlayable2.equipperSlotsAndContents[theSlotTypeToCycle];
        if (theequippable2ToPutAway == null)
        {
            if (theInventory.inventoryItems.Count < 1) { return; }

            GameObject item1 = theInventory.inventoryItems[0];
            equippable2 equip = item1.GetComponent<equippable2>();
            equip.equipThisequippable2(thePlayable2);
            theInventory.inventoryItems.RemoveAt(0);

        }
        else
        {
            equippable2 equip = theequippable2ToPutAway.GetComponent<equippable2>();
            equip.unequip(thePlayable2);
            theInventory.putInInventory(theequippable2ToPutAway);
        }

    }

    void rotateInventoryList()
    {
        if (theInventory.inventoryItems.Count < 2) { return; }

        GameObject item1 = theInventory.inventoryItems[0];
        theInventory.inventoryItems.RemoveAt(0);
        theInventory.inventoryItems.Add(item1);
    }

    void equipFirstItemInInventoryList()
    {
        if (theInventory.inventoryItems.Count < 1) { return; }
        GameObject item1 = theInventory.inventoryItems[0];
        equippable2 equip = item1.GetComponent<equippable2>();
        equip.equipThisequippable2(thePlayable2);
    }


}

*/