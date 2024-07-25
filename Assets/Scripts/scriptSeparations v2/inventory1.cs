using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static equippable2Setup;

public class inventory1 : MonoBehaviour, IInteractable
{

    public Dictionary<interType, List<Ieffect>> dictOfInteractions { get; set; }


    //Debug.Log("hello:  " + this);
    //              helloTest h1 = new helloTest();  //will helloTest's "awake" be called before "inventory1" awake?!?!?!

    //public List<string> testInventory1 = new List<string>();

    public List<equippable2> storedequippable2s = new List<equippable2>();

    void Awake()
    {

        playable2 thePlayable2 = this.gameObject.GetComponent<playable2>();
        if (thePlayable2 == null)
        {
            Debug.Log("this shouldn't be null, on this object:  " + this.gameObject);
        }

        //thePlayable2.enactableBoolSet.Add(new takeFromAndPutBackIntoInventory(this.gameObject));
        takeFromAndPutBackIntoInventory.addTakeFromAndPutBackIntoInventory(this.gameObject);

        //foreach (IEnactaBool enactaBool in thePlayable2.enactableBoolSet)
        {
            //Debug.Log(enactaBool);
        }

        //.....this looks like a bad way to do things:
        //              thePlayable2.updateALLGamepadButtonsFromplayable2(this.gameObject.GetComponent<virtualGamepad>());

        GameObject gun = genGen.singleton.returnGun1(this.transform.position);
        putInInventory(gun);
    }
    
    void Start()
    {



    }




    public void putInInventory(GameObject theObjectToPutInInventory)
    {
        //should i just be storing a game object?  because maybe someday an inventory item won't be equippable2?
        //i'm doing this so it's easy to write code to equip it.  can worry about edge cases later.
        equippable2 theequippable2 = theObjectToPutInInventory.GetComponent<equippable2>();
        putInInventory(theequippable2);

        /*

        storedequippable2s.Add(theequippable2);

        //dockThisToOtherObject(theAuthorScript.enacting.enactionAuthor, (1.5f * theAuthorScript.enacting.enactionAuthor.transform.forward) + new Vector3(0.9f, 0.2f, 0));
        interactionCreator.singleton.dockXToY(theObjectToPutInInventory, this.gameObject);
        theequippable2.gameObject.SetActive(false);


        */



        //Debug.Log("will it put in inventory???");
        //virtualGamepad gamepad = theAuthorScript.enacting.enactionAuthor.GetComponent<virtualGamepad>();
        //playable2 thePlayable2 = theAuthorScript.enacting.enactionAuthor.GetComponent<playable2>();
        //gun1 theGun = this.GetComponent<gun1>();
        //if (theGun.occupied == true) { return; }
        //          theGun.equipThisequippable2(thePlayable2);


        //          interactionCreator.singleton.makeThisObjectDisappear(theObjectToPutInInventory);
        //dockThisToOtherObject(thePlayable2.enactionPoint1, new Vector3(0.6f, 0.1f, 0));
    }


    public void putInInventory(equippable2 theequippable2)
    {
        //should i just be storing a game object?  because maybe someday an inventory item won't be equippable2?
        //i'm doing this so it's easy to write code to equip it.  can worry about edge cases later.
        //equippable2 theequippable2 = theObjectToPutInInventory.GetComponent<equippable2>();
        storedequippable2s.Add(theequippable2);

        //dockThisToOtherObject(theAuthorScript.enacting.enactionAuthor, (1.5f * theAuthorScript.enacting.enactionAuthor.transform.forward) + new Vector3(0.9f, 0.2f, 0));
        interactionCreator.singleton.dockXToY(theequippable2.gameObject, this.gameObject);
        theequippable2.gameObject.SetActive(false);






        //Debug.Log("will it put in inventory???");
        //virtualGamepad gamepad = theAuthorScript.enacting.enactionAuthor.GetComponent<virtualGamepad>();
        //playable2 thePlayable2 = theAuthorScript.enacting.enactionAuthor.GetComponent<playable2>();
        //gun1 theGun = this.GetComponent<gun1>();
        //if (theGun.occupied == true) { return; }
        //          theGun.equipThisequippable2(thePlayable2);


        //          interactionCreator.singleton.makeThisObjectDisappear(theObjectToPutInInventory);
        //dockThisToOtherObject(thePlayable2.enactionPoint1, new Vector3(0.6f, 0.1f, 0));
    }



    
}

public class takeFromAndPutBackIntoInventory :MonoBehaviour, IEnactaBool
{
    public buttonCategories gamepadButtonType { get; set; }
    //public interactionInfo interInfo { get; set; }  //hmmm.......
    public GameObject enactionAuthor { get; set; }

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

        //interInfo = new interactionInfo(interType.self, 1f);
    }

    public void enact()
    {
        
        equippable2 theequippable2ToPutAway = thePlayable2.equipperSlotsAndContents[theSlotTypeToCycle]; 
        //Debug.Log("::::::::::::::::::::::::::::::thePlayable2.equipperSlotsAndContents[theSlotTypeToCycle] == null ? :  " + thePlayable2.equipperSlotsAndContents[theSlotTypeToCycle]);

        //Debug.Log("::::::::::::::::::::::::::::::theequippable2ToPutAway == null ? :  " + theequippable2ToPutAway);

        if (theequippable2ToPutAway == null)
        {

            //Debug.Log("trying to equip something from inventory");
            if (theInventory.storedequippable2s.Count < 1) { return; }

            //Debug.Log("so far so good");
            //theInventory.storedequippable2s[0].equipThisequippable2(thePlayable2);
            theInventory.storedequippable2s[0].equipThisequippable2(thePlayable2);
            theInventory.storedequippable2s.RemoveAt(0);
        }
        else
        {
            //Debug.Log("trying to puy away something into inventory");
            
            theequippable2ToPutAway.unequip(thePlayable2);
            theInventory.putInInventory(theequippable2ToPutAway);
            //theInventory.storedequippable2s[0].unequip(thePlayable2);
            //rotateInventoryList();
            //theInventory.putInInventory();
            
        }
        
    }

    void rotateInventoryList()
    {
        if (theInventory.storedequippable2s.Count < 2) { return; }

        //remove one from end, put it at start:
        //equippable2 endOne = theInventory.storedequippable2s.FindLast();
        //orrr easier to remove FIRST one, and ADD it to the list [which will put it at end i think?]
        equippable2 firstOne = theInventory.storedequippable2s[0];
        theInventory.storedequippable2s.RemoveAt(0);
        theInventory.storedequippable2s.Add(firstOne);

    }

    void equipFirstItemInInventoryList()
    {
        if (theInventory.storedequippable2s.Count < 1) { return; }
        theInventory.storedequippable2s[0].equipThisequippable2(thePlayable2);
    }


}

