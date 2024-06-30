using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionCreator : MonoBehaviour
{

    public static interactionCreator singleton;


    //public enum interType



    public enum simpleSlot
    {
        //XYZ Controller buttons,
        storage,
        seat,
        hands,
        liquidHolder,
        topOfHead,
        face,
        torso

    }

    public enum slot //not sure where to put these //can be "Slots"/equipperSlots OR the things that go in those slots, right?  no, no, just slots.  so rename as slots, just for easy human reading of program...or "equipperSlot"?
    {
        hands,
        feet,
        topOfHead,
        eyewear,
        mask,
        back,
        upperBody,
        lowerBody,
        seat1,
        seat2,
        seat3,
        seat4

    }


    void Awake()
    {
        singletonify();

    }

    void singletonify()
    {
        if (singleton != null && singleton != this)
        {
            Debug.Log("this class is supposed to be a singleton, you should not be making another instance, destroying the new one");
            Destroy(this);
            return;
        }
        singleton = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class interactionInfo
{
    public GameObject enactionAuthor { get; set; }
    public enactionCreator.interType interactionType { get; set; }
    public float magnitudeOfInteraction = 1f;

    public interactionInfo(enactionCreator.interType interactionType, float magnitudeOfInteraction = 1f)
    {
        this.interactionType = interactionType;
        this.magnitudeOfInteraction = magnitudeOfInteraction;
    }
}












public abstract class simpleVehicleItemWeaponOrBodyEtc
{
    public List<simpleEquippable> equipperSlotList;

    public void equippableSlotsInitializer(List<interactionCreator.slot> availableParts)
    {
        //(List<buttonCategories> availableButtonCategories) hmmmmmmmmmmm, no, need to do this in terms of how they can be equipped [held one one of your free hands, worn on head, etc]


        foreach (var part in availableParts)
        {
            //      this.availabilityOfParts[part] = false;
        }
    }

}

public class simplePlayable : gamepadable
{
    //these can also be an individual "seat" on a vehicle?  which might not have any enactions at all?  except exit
    //public bool occupied = false;
    //public Dictionary<buttonCategories, equippable> currentlyEquipped = new Dictionary<buttonCategories, equippable>();

    public Transform cameraMount;

    //attach to objects/entities you can "play as" [such as bodies and vehicles]
    //weapons and items too


    //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:



    //need to get clear.  is this just a slot/seat?  or is this a full vehicle/body?
    //"playable" sounds like a vehcile/body.
    //so i need something ELSE to be the slots/seats.....and that is the dictionary entries.


}

public class simpleSlot
{
    public interactionCreator.simpleSlot typeOfSlot;
    public simpleEquippable whatIsInThisSlot;

    public void successfullyEquip(equippable thingToEquip)
    {
        //!!!!!!!!!!!!!!!!!     all conditions must be handled BEFORE this function is called.      //!!!!!!!!!!!!!!!!!
        //if (occupied == true) { return; }
        //occupied = true;

        //virtualGamepad gamepad = equippedBy.theVirtualGamepad;  //no, equip enactions to non-spatial slot component ON virtualGamepad....just like equipping ANYTHING

        //fillequipperSlotSLots(gamepad.equipperSlotsAndTheirEquipment);

        /*

        //Debug.Log("is cameraMount  null:  " + cameraMount + "  for this object:  " + this.gameObject.name);
        //Debug.Log("is gamepad.theCamera null:  " + gamepad.theCamera + "  for this object:  " + this.gameObject.name);
        if (theequipperSlot.cameraMount != null && gamepad.theCamera != null)
        {

            gamepad.theCamera.transform.SetParent(theequipperSlot.cameraMount, false);
        }



        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:

        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            //this "object is null" error is usually the only kind of error where it isn't clear which variable went wrong
            //and EVERY TIME it's a situation like this, where there are a TON of variables in a single line.
            //so i need to print sooooooo many....
            //Debug.Log("enactaBool:  " + enactaBool);
            //Debug.Log("enactaBool.interInfo:  " + enactaBool.interInfo);
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

        gamepad.allCurrentTARGETbyVectorEnactables.Clear();
        gamepad.allCurrentTARGETbyVectorEnactables = enactableTARGETVectorSet;


        if (gamepad.theCamera == null) { return; }

        */

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

    public void unequip()
    {
        //      occupied = false;


        /*



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
                gamepad.allCurrentTARGETbyVectorEnactables.Remove(enactaTargetV);
            }
        }

        */

    }




    public void tryToEquip(equippable thingToEquip)
    {
        //rethinking how to start.  conditions.  how about do those LAST.  will be clearer.
        //for now just simple successfully equip WHATEVER IT IS....regardless of type of slot etc even!?






        //Can’t just plug enactions into gamepad anymore:
        //      You DO plug them in like that most of the time.And at the same time,
        //      have to UNEQUIP anything that occupies the same button OR equipperSlot slot
        //          Oh, and unequipping something obviously simultaneously removes it from the buttons AND the equipperSlot slots.
        //      The exception is if slots are occupied by a “playAs” [body or vehicle],
        //      then they cannot be repurposed until player manually exits vehicle
        //          It could tell the player “you can’t do that while in this vehicle”



        //free up "whatIsRequiredToEquipThisPart" so that it can equip this part
        //but this also requires knowing WHAT [if anything] is currently filling that slot
        //if it's a vehicle, don't proceed
        //otherwise, unequip anything in this slot, to equip the current thing

        //so first check if slots are a vehicle or null (null will mean that there is no such slot);
        //      foreach (interactionCreator.slot thisSlot in thingToEquip.whatPartsAreRequiredToEquipThis)
        {
            //gamepadable thisGamePadable = virtualGamepad.equipperSlotsAndTheirEquipment[thisSlot];
            //if (thisGamePadable != null || thisGamePadable.TryReplace() == false) { return; }
        }


        //..............i should move ALL of this logic over onto the virtual gamepad, right?
        //well, equipping maybe sounds more like something an equippable should do, or an equipperSlot.  not something a gamepad does....

        successfullyEquip(thingToEquip);


        //thisEquippablePart;  //like items
        //whatIsRequiredToEquipThisPart;  //like hands

    }



}

public class simpleEquippable : gamepadable
{
    //interactionCreator.slot thisEquippablePart;  //like items //hmm, infinite variety of items, an object of this class will suffice to describe itself, no need for enum
    //List<interactionCreator.slot> whatPartsAreRequiredToEquipThis;  //like hands
    //playable equippedBy;

    interactionCreator.simpleSlot typeOfSlotRequired;
    //simpleSlot equippedBy;





}









//just put these here for now:
//[[[[[Whether something has contents, WHAT contents, and how much does/can it have]]]]]
public class slot
{

    public interactionCreator.slot typeOfSlot;
    //public Dictionary<interactionCreator.slot, bool> availabilityOfSlots = new Dictionary<interactionCreator.slot, bool>();
    //public List<interactionCreator.slot> whatIsInThisSeat = new List<interactionCreator.slot>();

    //      only need ONE slot, right?
    //public equippable whatIsInThisSlot;
    //      Errrr, no, some might be general, like POCKETS/containers,
    //      and be able to hold more than one thing at the same time…….
    public List<equippable> whatIsInThisSlot = new List<equippable>();
    //      but, since some can only fit one thing [or....a certain quantity/size of thing...]
    //      need some way to limit it.  integer, for now?
    //public int maximumNumberOfThingsInThisSlot = 1;

    //should "equip" be on equippables, or equipperSlots?  let's say equipperSlots have the function to equip other things, change if i need to
    public void successfullyEquip(equippable thingToEquip)
    {
        //!!!!!!!!!!!!!!!!!     all conditions must be handled BEFORE this function is called.      //!!!!!!!!!!!!!!!!!
        //if (occupied == true) { return; }
        //occupied = true;

        //virtualGamepad gamepad = equippedBy.theVirtualGamepad;  //no, equip enactions to non-spatial slot component ON virtualGamepad....just like equipping ANYTHING

        //fillequipperSlotSLots(gamepad.equipperSlotsAndTheirEquipment);

        /*

        //Debug.Log("is cameraMount  null:  " + cameraMount + "  for this object:  " + this.gameObject.name);
        //Debug.Log("is gamepad.theCamera null:  " + gamepad.theCamera + "  for this object:  " + this.gameObject.name);
        if (theequipperSlot.cameraMount != null && gamepad.theCamera != null)
        {

            gamepad.theCamera.transform.SetParent(theequipperSlot.cameraMount, false);
        }



        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:

        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            //this "object is null" error is usually the only kind of error where it isn't clear which variable went wrong
            //and EVERY TIME it's a situation like this, where there are a TON of variables in a single line.
            //so i need to print sooooooo many....
            //Debug.Log("enactaBool:  " + enactaBool);
            //Debug.Log("enactaBool.interInfo:  " + enactaBool.interInfo);
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

        gamepad.allCurrentTARGETbyVectorEnactables.Clear();
        gamepad.allCurrentTARGETbyVectorEnactables = enactableTARGETVectorSet;


        if (gamepad.theCamera == null) { return; }

        */

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

    public void unequip()
    {
        //      occupied = false;


        /*



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
                gamepad.allCurrentTARGETbyVectorEnactables.Remove(enactaTargetV);
            }
        }

        */

    }



    public void tryToEquip(equippable thingToEquip)
    {
        //rethinking how to start.  conditions.  how about do those LAST.  will be clearer.
        //for now just simple successfully equip WHATEVER IT IS....regardless of type of slot etc even!?






        //Can’t just plug enactions into gamepad anymore:
        //      You DO plug them in like that most of the time.And at the same time,
        //      have to UNEQUIP anything that occupies the same button OR equipperSlot slot
        //          Oh, and unequipping something obviously simultaneously removes it from the buttons AND the equipperSlot slots.
        //      The exception is if slots are occupied by a “playAs” [body or vehicle],
        //      then they cannot be repurposed until player manually exits vehicle
        //          It could tell the player “you can’t do that while in this vehicle”



        //free up "whatIsRequiredToEquipThisPart" so that it can equip this part
        //but this also requires knowing WHAT [if anything] is currently filling that slot
        //if it's a vehicle, don't proceed
        //otherwise, unequip anything in this slot, to equip the current thing

        //so first check if slots are a vehicle or null (null will mean that there is no such slot);
        //      foreach (interactionCreator.slot thisSlot in thingToEquip.whatPartsAreRequiredToEquipThis)
        {
            //gamepadable thisGamePadable = virtualGamepad.equipperSlotsAndTheirEquipment[thisSlot];
            //if (thisGamePadable != null || thisGamePadable.TryReplace() == false) { return; }
        }


        //..............i should move ALL of this logic over onto the virtual gamepad, right?
        //well, equipping maybe sounds more like something an equippable should do, or an equipperSlot.  not something a gamepad does....

        successfullyEquip(thingToEquip);


        //thisEquippablePart;  //like items
        //whatIsRequiredToEquipThisPart;  //like hands

    }


}
//[[[[[If something HAS a “door” to ever let contents in/out]]]]]
//[[[[[What state [open or closed] that entrance is currently in]]]]]
public class slotDoor
{

}
//[[[[[Multiple ways in/out]]]]]  just list slot doors



//[[[[[Solid LOCK]]]]]
public class slotWithSolidLock:slot
{

}
//[[[[[“Solid tight”]]]]]
public class solidTightSlot:slotWithSolidLock
{
    //only IF the "door" is CLOSED
}
//[[[[[Can hold water]]]]]
public class waterHoldableSlot:solidTightSlot
{

}
//[[[[[Can be sealed air/ watertight]]]]]
public class fluidTightSlot:waterHoldableSlot
{

}
//[[[[[Non-....spatial?]]]]]
public class nonSpatialSlot : slot
{

}


