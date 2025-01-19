using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using static enactionCreator;
using static interactionCreator;
using static tagging2;

public class tagging2 : MonoBehaviour
{

    public static tagging2 singleton;


    //by tag, and by object:
    public Dictionary<objectIdPair, List<tag2>> tagsOnObject = new Dictionary<objectIdPair, List<tag2>>();
    public Dictionary<tag2, List<objectIdPair>> objectsWithTag = new Dictionary<tag2, List<objectIdPair>>();

    //and zones are integers, put them here?  or elsewhere?
    public Dictionary<int, List<objectIdPair>> objectsInZone = new Dictionary<int, List<objectIdPair>>();
    public Dictionary<objectIdPair, int> zoneOfObject = new Dictionary<objectIdPair, int>();

    //object id Pairs:
    public Dictionary<int, objectIdPair> pairsById = new Dictionary<int, objectIdPair>();



    public enum tag2
    {
        errorYouDidntSetEnumTypeForTAG2,
        interactable,
        equippable2,
        playable2,
        mapZone,
        gamepad,
        zoneable,
        threat1,
        herbivore,
        predator,
        militaryBase,
        teamLeader,
        attackSquad, 
        defenseSquad,
        team1,
        team2,
        team3,
        team4,
        team5,
        team6,
        team7,
        team8 //just use integers for teams????
    }


    public Dictionary<tagging2.tag2, Color> teamColors = new Dictionary<tagging2.tag2, Color>();


    void Awake()
    {
        //Debug.Log("Awake:  " + this);
        singletonify();


        teamColors[tag2.team2] = Color.red;
        teamColors[tag2.team3] = Color.green;
        teamColors[tag2.team4] = Color.blue;
        teamColors[tag2.team5] = Color.black;
        teamColors[tag2.team6] = Color.cyan;
        teamColors[tag2.team7] = Color.yellow;
        teamColors[tag2.team8] = Color.magenta;
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



    //funcitons that modify an object's tags
    public void addTag(GameObject theObject, tag2 tag)
    {
        genGen.singleton.ensureSafetyForDeletion(theObject);

        //this funciton updates BOTH lists of tags
        //the "local" list, and the "global"...Dicitonary of .....objectIdPairs

        //update "local" tags
        if (tagsOnObject.ContainsKey(idPairGrabify(theObject)) == false)
        {
            List<tag2> tagList = new List<tag2>();
            tagList.Add(tag);
            tagsOnObject[idPairGrabify(theObject)] = tagList;
        }
        else
        {
            List<tag2> tagList = tagsOnObject[idPairGrabify(theObject)];
            if (tagList.Contains(tag) == true)
            {
                return; 
            }

            tagList.Add(tag);
        }



        //update "global" tags...
        //but dictionaries are tricky objects, and must be checked and all that:

        //objectsWithTag
        if (objectsWithTag.ContainsKey(tag))
        {

            //Debug.Log("zzzzzzzzzzzzzzzzzzzzzobjectsWithTag[tag].Count:  " + objectsWithTag[tag].Count);
            List<objectIdPair> theList = objectsWithTag[tag];
            //Debug.Log("how many tags:  " + theList.Count);
            if (theList.Contains(idPairGrabify(theObject))) {

                //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!already on list");

                return;
            }
            //Debug.Log("not on list, adding it");
            //add the game object to the list of objects tagged with that tag:
            objectsWithTag[tag].Add(idPairGrabify(theObject));
        }
        else
        {
            //sigh, need to add the key first, which means the list it unlocks as well...
            List<objectIdPair> needsList = new List<objectIdPair>();
            needsList.Add(idPairGrabify(theObject));
            objectsWithTag.Add(tag, needsList);
        }
    }

    

    public void removeTag(GameObject theObject, tag2 tag)
    {
        //this funciton updates BOTH lists of tags
        //the "local" list, and the "global"...Dicitonary of objects

        //should I add checks here to make sure the tag is present?
        //because it will have errors if I try to remove a tag that's basicaly already removed...

        //update "local" tags
        //tags.Remove(tag);

        if (tagsOnObject.ContainsKey(idPairGrabify(theObject)) == false)
        {
            //should be done already then
            return;
        }


        List<tag2> tagList = tagsOnObject[idPairGrabify(theObject)];
        if (tagList.Contains(tag) == false)
        {
            //should be done already then
            return;
        }


        tagList.Remove(tag);

        //update "global" tags...
        //remove the game object to the list of objects tagged with that tag:
        //          globalTags[tag].Remove(idNumberrrr);

        trickyObjectsWithTagDictionaryRemoval(theObject, tag);
    }
    
    public void trickyObjectsWithTagDictionaryRemoval(GameObject theObject, tag2 tag)
    {
        if (objectsWithTag[tag] == null || objectsWithTag[tag].Count == 0) { return; }

        int currentIndex = 0;


        objectIdPair theIdPair = idPairGrabify(theObject);
        if(objectsWithTag[tag] !=null && objectsWithTag[tag].Contains(theIdPair))
        {
            objectsWithTag[tag].Remove(theIdPair);
        }
    }
    
    public void removeAllTagsAndZone(GameObject theObject)
    {
        //REMOVES this object from ALL tag lists
        //necessary when destroying objects, 
        //otherwise there will be "null" object references on those lists!

        //which data to modify?
        //      tagsOnObject
        //      objectsWithTag
        //      objectsInZone
        //      zoneOfObject
        //  what about:
        //          pairsById



        objectIdPair newIdPair = idPairGrabify(theObject);

        removeFromItsZone(newIdPair);
        zoneOfObject.Remove(newIdPair);
        //zoneOfObject[thisObjectIdPair].Remove(thisObjectIdPair);

        //      objectsWithTag
        //      do this FIRST.  need to know which tags the object will be filed under
        //tagsOnObject.Remove(newIdPair);  //no, "removeALLtags" needs that to remove it from "objectsWithTag"

        removeALLtags(theObject, newIdPair);
    }

    private void removeALLtags(GameObject theObject, objectIdPair newIdPair)
    {

        //can't modify list while using it in a for loop, so need to make a copy list:
        List<tag2> copyListOfTags = new List<tag2>();

        List<tag2> originalListOfTags = tagsOnObject[pairsById[newIdPair.theObjectIdNumber]];
        foreach (tag2 tag in originalListOfTags)
        {
            copyListOfTags.Add(tag);
        }

        foreach (tag2 tag in copyListOfTags)
        {
            removeTag(theObject, tag);
        }
    }

    public void addToZone(GameObject theObject, int zone)
    {

        genGen.singleton.ensureSafetyForDeletion(theObject);
        objectIdPair thisObjectIdPair = idPairGrabify(theObject);
        initializeObjectEntriesIfNecessary(thisObjectIdPair);

        if (zoneOfObject[thisObjectIdPair] == zone && objectsInZone[zone].Contains(thisObjectIdPair))
        {
            //no modification necessary
            return;
        }

        removeFromZone(theObject, zoneOfObject[thisObjectIdPair]);


        zoneOfObject[thisObjectIdPair] = zone;

        //remove from [all should be handled in remove from zone function]:
        //      zoneOfObject
        //      objectsInZone
        //WHEN to remove from a zone?
        //      could do it always
        //      or skip if new zone is same as old zone......so ya, check this FIRST
        //add to:
        //      zoneOfObject



        //add to new zone:
        objectsInZone[zone].Add(thisObjectIdPair);
    }

    

    public void removeFromZone(GameObject theObject, int zone)
    {
        /*
        if (zoneOfObject.ContainsKey(newIdPair) == false)
        {
            Debug.Log("zoneOfObject.ContainsKey(newIdPair) is FALSE, object has no zone");
            return;
        }


        if (objectsInZone[zoneOfObject[newIdPair]].Contains(newIdPair) == false)
        {
            Debug.Log("objectsInZone[zoneOfObject[newIdPair]].Contains(newIdPair) is FALSE, object is not in the zone list, zone list does not contain the object, well the idpair");
            return;
        }
        */


        objectIdPair thisObjectIdPair = idPairGrabify(theObject);


        if (zoneOfObject[thisObjectIdPair] != zone) { return; }

        objectsInZone[zone].Remove(thisObjectIdPair);
    }


    private void removeFromItsZone(objectIdPair thisObjectIdPair)
    {
        objectsInZone[zoneOfObject[thisObjectIdPair]].Remove(thisObjectIdPair);
    }


    public objectIdPair idPairGrabify(GameObject theObject)
    {
        objectIdPair newIdPair = new objectIdPair();
        newIdPair.theObject = theObject;
        Debug.Assert(newIdPair != null);
        Debug.Assert(theObject != null);
        newIdPair.theObjectIdNumber = theObject.GetHashCode();
        if (pairsById.ContainsKey(newIdPair.theObjectIdNumber))
        {
            initializeObjectEntriesIfNecessary(pairsById[newIdPair.theObjectIdNumber]);
            return pairsById[newIdPair.theObjectIdNumber];
        }
        else
        {
            initializeObjectEntriesIfNecessary(newIdPair);
            pairsById[newIdPair.theObjectIdNumber] = newIdPair;
            return newIdPair;
        }

        //initialize other dictionaries?  if needed?  sure.  but through another function.
        

    }

    public void initializeObjectEntriesIfNecessary(objectIdPair thisIdPair)
    {



        if (zoneOfObject.ContainsKey(thisIdPair) == false)
        {
            zoneOfObject[thisIdPair] = 0;
        }
        if (tagsOnObject.ContainsKey(thisIdPair) == false)
        {
            tagsOnObject[thisIdPair] = new List<tag2>();
        }


    }

    public void initializeTagListsIfNecessary(tag2 thisTag)
    {


        /*
        if (objectsInZone.ContainsKey(thisTag) == false)
        {
            zoneOfObject[thisTag] = 0;
        }
        */

        if (objectsWithTag.ContainsKey(thisTag) == false)
        {
            objectsWithTag[thisTag] = new List<objectIdPair>();
        }


    }

    public List<GameObject> listInObjectFormat(List<objectIdPair> pairFormatList)
    {
        List<GameObject> newList = new List<GameObject>();

        foreach (objectIdPair thisPair in pairFormatList)
        {
            if (thisPair.theObject != null)
            {
                newList.Add(thisPair.theObject);
            }
            else
            {
                Debug.Log("this object was deleted without being removed from the list!  id# = " + thisPair.theObjectIdNumber);
            }
        }

        return newList;
    }

    
        public List<objectIdPair> listInIDPairFormat(List<GameObject> objectFormatList)
    {
        List<objectIdPair> newList = new List<objectIdPair>();

        foreach (GameObject theObject in objectFormatList)
        {
            if (theObject != null)
            {
                newList.Add(idPairGrabify(theObject));
            }
        }

        return newList;
    }


    public void setObjectAsMemberOfZone(GameObject theObject, int zoneNumber)
    {
        //tagging2.singleton.objectsInZone[zoneNumber].Add(theObject);
        //tagging2.singleton.zoneOfObject

    }

    internal int whichZone(GameObject gameObject)
    {
        return zoneOfObject[idPairGrabify(gameObject)];
    }

    public List<tag2> allTagsOnObject(GameObject gameObject)
    {
        return tagsOnObject[idPairGrabify(gameObject)];
    }

    internal void printAllTags(GameObject theObject)
    {
        string x = "tags:  [ ";
        foreach(tag2 thisTag in allTagsOnObject(theObject))
        {
            x += thisTag.ToString() + ", ";
        }
        x += "]";
        Debug.Log(x);
    }
    public List<GameObject> allObjectsWithTag(tag2 theTag)
    {
        return listInObjectFormat(objectsWithTag[theTag]);
    }
    internal void printAllObjectsWithTag(tag2 theTag)
    {
        foreach (GameObject thisObj in allObjectsWithTag(theTag))
        {
            Debug.Log(thisObj);
        }
    }
}

public class find
{
    public find()
    {

    }

    public List<GameObject> allMultipleInZone(int zone, List<tag2> theTags)
    {
        List<GameObject> newList = new List<GameObject> ();

        foreach(objectIdPair thisId in allInZone(zone))
        {
            if(hasAllTagsOnList(thisId, theTags) == false) { continue; }

            newList.Add(thisId.theObject);
        }

        return newList;
    }

    public List<GameObject> allObjectsInObjectsZone(GameObject theObject)
    {
        int zone = tagging2.singleton.whichZone(theObject);
        List<objectIdPair> pairs = allInZone(zone);
        return tagging2.singleton.listInObjectFormat(pairs);
    }


    public List<objectIdPair> allInZone(int zone)
    {
        return tagging2.singleton.objectsInZone[zone];
    }

    public List<objectIdPair> allWithOneTag(List<objectIdPair> theList,tag2 theTag)
    {
        List<objectIdPair> newList = new List<objectIdPair>();

        //return tagging2.singleton.objectsInZone[zone];

        foreach(objectIdPair thisId in theList)
        {
            //if (tagging2.singleton.tagsOnObject.Keys.Contains(thisId) == false) {Debug.Log("this object lacks this key:  "+ thisId.theObject + "key:  wait, what?");  continue; }
            if (tagging2.singleton.tagsOnObject[thisId].Contains(theTag) == false) { continue; }

            newList.Add(thisId);
        }

        return newList;
    }


    public bool hasAllTagsOnList(objectIdPair thisId, List<tag2> wantedTags)
    {
        //should be moved to tagging script or something!
        //returns true if this object has all tags

        //need to grab the tags on that object:

        foreach (tag2 tag in wantedTags)
        {
            if (tagging2.singleton.tagsOnObject[thisId].Contains(tag) == false) { return false; }
        }

        //if we reach this point, it means the object does indeed have all the tags we want!
        return true;
    }






    public hitscanEnactor grabHitscanEnaction(GameObject theObject, interType interTypeX)
    {

        foreach (hitscanEnactor thisEnaction in listOfHitscansOnObject(theObject))
        {

            if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
        }



        return null;
    }
    public List<hitscanEnactor> listOfHitscansOnObject(GameObject theObject)
    {
        //hmm:
        //List<IEnactaBool> theList = [.. theObject.GetComponents<collisionEnaction>()];


        List<hitscanEnactor> theList = new List<hitscanEnactor>();

        foreach (hitscanEnactor thisEnaction in theObject.GetComponents<hitscanEnactor>())
        {
            theList.Add(thisEnaction);
        }


        return theList;
    }



    internal enaction enactionWithButtonX(GameObject theObject, buttonCategories buttonX)
    {
        foreach (enaction thisEnaction in listOfAllnEnactionsOnObject(theObject))
        {
            //Debug.Log(thisEnaction.interInfo.interactionType);
            if (thisEnaction.gamepadButtonType == buttonX) { return thisEnaction; }
        }




        return null;
    }

    internal enaction enactionWithInteractionX(GameObject theObject, interType interTypeX)
    {
        //have to see any available interactions:
        //where?
        //          in current "equipped" enactions in gamepad
        //          in inventory [at which point, need to equip it!  ..............doesn't fit current "singleEXE" paradigm.....
        //just add a step before this in the plan that equips/preps IF necessary???
        //
        //what kind of enaction?
        //      ranged
        //          hitscan
        //          non-hitscan
        //      [no others currently?]


        //what about child objects/inventory objects?  does this copde find them? [no, you have to feed each inventory object into this function]
        //Debug.Log(listOfCollisionEnactionsOnObject(theObject).Count);
        foreach (collisionEnaction thisEnaction in listOfCollisionEnactionsOnObject(theObject))
        {
            //Debug.Log(thisEnaction.interInfo.interactionType);
            if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
        }




        return null;
        }

    internal enaction enactionOnObjectItselfWithIntertypeX(GameObject theObject, interType interTypeX)
    {
        //what about child objects/inventory objects?  does this copde find them? [no, you have to feed each inventory object into this function]
        //Debug.Log(listOfCollisionEnactionsOnObject(theObject).Count);
        foreach (collisionEnaction thisEnaction in listOfCollisionEnactionsOnObject(theObject))
        {
            //Debug.Log(thisEnaction.interInfo.interactionType);
            if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
        }




        return null;
    }

    internal enaction enactionEquippedByObjectWithIntertypeX(GameObject theObject, interType interTypeX)
    {
        //what about child objects/inventory objects?  does this copde find them? [no, you have to feed each inventory object into this function]
        //Debug.Log(listOfCollisionEnactionsOnObject(theObject).Count);
        foreach (collisionEnaction thisEnaction in listOfCollisionEnactionsOnObject(theObject))
        {
            //Debug.Log(thisEnaction.interInfo.interactionType);
            if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
        }

        playable2 thePlayable = theObject.GetComponent<playable2>();

        foreach (var key in thePlayable.equipperSlotsAndContents.Keys)
        {
            GameObject equippedObject = thePlayable.equipperSlotsAndContents[key];
            enaction thisEnaction = enactionOnObjectItselfWithIntertypeX(equippedObject, interTypeX);

            if (thisEnaction != null) { return thisEnaction; }
        }




        return null;
    }
    internal GameObject ObjectEquippedByObjectWithIntertypeX(GameObject theObject, interType interTypeX)
    {

        playable2 thePlayable = theObject.GetComponent<playable2>();

        foreach (var key in thePlayable.equipperSlotsAndContents.Keys)
        {
            GameObject equippedObject = thePlayable.equipperSlotsAndContents[key];

            if (new find().enactionOnObjectItselfWithIntertypeX(equippedObject, interTypeX) != null) { return equippedObject; }
        }




        return null;
    }

    internal enaction enactionInObjectsInventoryWithIntertypeX(GameObject theObject, interType interTypeX)
    {

        inventory1 theInventory = theObject.GetComponent<inventory1>();

        foreach (GameObject inventoryItem in theInventory.inventoryItems)
        {

            enaction thisEnaction = enactionOnObjectItselfWithIntertypeX(inventoryItem, interTypeX);
            if (new find().enactionOnObjectItselfWithIntertypeX(inventoryItem, interTypeX) != null) { return thisEnaction; }
        }



        return null;
    }

    internal GameObject objectInObjectsInventoryWithIntertypeX(GameObject theObject, interType interTypeX)
    {

        inventory1 theInventory = theObject.GetComponent<inventory1>();

        foreach (GameObject inventoryItem in theInventory.inventoryItems)
        {

            enaction thisEnaction = enactionOnObjectItselfWithIntertypeX(inventoryItem, interTypeX);
            if (new find().enactionOnObjectItselfWithIntertypeX(inventoryItem, interTypeX) != null) { return inventoryItem; }
        }



        return null;
    }


    public List<enaction> listOfAllnEnactionsOnObject(GameObject theObject)
    {
        //what about child objects/inventory objects?  does this code find them?

        List<enaction> theList = new List<enaction>();

        foreach (enaction thisEnaction in theObject.GetComponents<enaction>())
        {
            theList.Add(thisEnaction);
        }


        return theList;
    }
    public List<collisionEnaction> listOfCollisionEnactionsOnObject(GameObject theObject)
    {
        //what about child objects/inventory objects?  does this code find them?

        List<collisionEnaction> theList = new List<collisionEnaction>();

        foreach (collisionEnaction thisEnaction in theObject.GetComponents<collisionEnaction>())
        {
            theList.Add(thisEnaction);
        }


        return theList;
    }

}


public class objectIdPair
{
    //      !!!!! SHOULD SWITCH TO USING ID NUMBERS IN DICTIONARY INSTEAD OF GAME OBJECT!  MAKES IT EASIER TO TRACK DOWN ERRORS WHEN OBJECT IS DESTROYED!  BECAUSE I CAN PRINT id NUMBER EVERYYYYY TIME I DESTROY ANY OBJECT!!!!!!!!!!!!!
    //buuuuuuuuuuut you can't get object if you have ID!!!!!  very annoying.  so use this instead
    public int theObjectIdNumber = 0;
    public GameObject theObject;
}








//targeting

public abstract class targetPicker
{

    //internal GameObject objectToBeNear; ?????????????

    public abstract agnosticTargetCalc pickNext();  //hmm, should just return object??
}

public class aimOffsetterTargetPicker : targetPicker
{
    private targetPicker targetPicker;
    private aimTarget theEnaction;

    public aimOffsetterTargetPicker(targetPicker targetPicker, aimTarget theEnaction)
    {
        this.targetPicker = targetPicker;
        this.theEnaction = theEnaction;
    }

    public override agnosticTargetCalc pickNext()
    {
        throw new NotImplementedException();
    }
}

public class randomNearbyLocationTargetPicker : targetPicker
{
    GameObject objectToBeNear;
    float spreadFactor = 1.0f;

    public randomNearbyLocationTargetPicker(GameObject objectToBeNearIn, float spreadFactorIn = 1f)
    {
        objectToBeNear = objectToBeNearIn;
        spreadFactor = spreadFactorIn;
    }

    public override agnosticTargetCalc pickNext()
    {
        Vector3 target = patternScript2.singleton.randomNearbyVector(objectToBeNear.transform.position, spreadFactor);
        //Debug.Log(target);
        agnosticTargetCalc targ = new agnosticTargetCalc(objectToBeNear, target);
        return targ;
    }

}


//nahh let's split this up:

/*
public class pickNextVisibleStuffStuff : targetPicker
{
    GameObject objectToBeNear;
    stuffType theType;

    public pickNextVisibleStuffStuff(GameObject objectToBeNearIn, stuffType theTypeIn)
    {
        //objectToBeNear = objectToBeNearIn;
        theType = theTypeIn;
    }

    public override agnosticTargetCalc pickNext()
    {
        //Vector3 target = patternScript2.singleton.randomNearbyVector(objectToBeNear.transform.position, spreadFactor);
        GameObject target = repository2.singleton.randomTargetPickerObjectFromList(new setOfAllNearbyStuffStuff(objectToBeNear, theType).grab());
        //nearestTargetPicker???
        agnosticTargetCalc targ = new agnosticTargetCalc(objectToBeNear, target);

        return targ;
    }
}
*/


//"nearest visible" is two conditions.....
//      all visible
//      nearest

//public class allVisibleInFOV : objectSetGrabber
//no, just combine "setOfAllObjectThatMeetCriteria" with"objectVisibleInFOV", i love this

//[i already have "setOfAllNearbyStuffStuff"]

public class randomTargetPicker : targetPicker
{
    objectSetGrabber theObjectSetGrabber;

    public randomTargetPicker(objectSetGrabber objectSetGrabberIn)
    {
        //objectToBeNear = objectToBeNearIn;
        theObjectSetGrabber = objectSetGrabberIn;
    }

    public override agnosticTargetCalc pickNext()
    {
        //      tagging2.singleton.printAllObjectsWithTag(tag2.militaryBase);
        //Vector3 target = patternScript2.singleton.randomNearbyVector(objectToBeNear.transform.position, spreadFactor);
        GameObject target = repository2.singleton.randomTargetPickerObjectFromList(theObjectSetGrabber.grab());
        //what if it's null?  no target????  zero on list??????
        //check that condition BEFORE using this?  probably?  bit....tenuous, though.....risky....could forget.......
        //Debug.Log("'''SUCCESSFUL PICK''' target, target.GetHashCode():  " + target + ", " + target.GetHashCode());
        //tagging2.singleton.printAllTags(target);

        agnosticTargetCalc targ = new agnosticTargetCalc(target);

        return targ;
    }
}

public class nearestTargetPicker : targetPicker
{
    GameObject objectToBeNear;
    objectSetGrabber theObjectSetGrabber;

    public nearestTargetPicker(GameObject objectToBeNearIn, objectSetGrabber objectSetGrabberIn)
    {
        objectToBeNear = objectToBeNearIn;
        theObjectSetGrabber = objectSetGrabberIn;
    }

    public override agnosticTargetCalc pickNext()
    {
        //Vector3 target = patternScript2.singleton.randomNearbyVector(objectToBeNear.transform.position, spreadFactor);
        GameObject target = conditionCreator.singleton.whichObjectOnListIsNearest(objectToBeNear, theObjectSetGrabber.grab());
        //Debug.Log("target:  "+target);
        agnosticTargetCalc targ = new agnosticTargetCalc(objectToBeNear, target);

        return targ;
    }
}

public class nearestTargetPickerExceptSelf : targetPicker
{
    GameObject objectToBeNear;
    objectSetGrabber theObjectSetGrabber;

    public nearestTargetPickerExceptSelf(GameObject objectToBeNearIn, objectSetGrabber objectSetGrabberIn)
    {
        objectToBeNear = objectToBeNearIn;
        theObjectSetGrabber = objectSetGrabberIn;
    }

    public override agnosticTargetCalc pickNext()
    {
        //Vector3 target = patternScript2.singleton.randomNearbyVector(objectToBeNear.transform.position, spreadFactor);
        GameObject target = conditionCreator.singleton.whichObjectOnListIsNearestExceptSELF(objectToBeNear, theObjectSetGrabber.grab());

        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! target:  " + target);

        agnosticTargetCalc targ = new agnosticTargetCalc(objectToBeNear, target);

        return targ;
    }
}

/*
public class targetCacheSetter : targetPicker
{
    //maybe:
    //      don't re-calculate if the target is non-null
    //      have some time or condition under which the target is set to null again???  [how?]
    // no, do 2 diff classes.  a setter, and a receiver/bserver.



    targetPicker theTargetPicker; 

    GameObject objectToBeNear;
    GameObject theCachedObject;


    public targetCacheSetter(GameObject objectToBeNearIn, targetPicker theTargetPickerIn)
    {
        objectToBeNear = objectToBeNearIn;
        theTargetPicker = theTargetPickerIn;
    }




    public agnosticTargetCalc observeCache()
    {
        if (theCachedObject == null)
        {
            return pickNext();
        }

        return new agnosticTargetCalc(objectToBeNear, theCachedObject);
    }

    public override agnosticTargetCalc pickNext()
    {
        theCachedObject = theTargetPicker.pickNext().ta;
        return new agnosticTargetCalc(objectToBeNear, theCachedObject);
    }

}


public class targetCacheReceiver : targetPicker
{
    targetCacheSetter theCache;
    GameObject objectToBeNear;

    public targetCacheReceiver(GameObject objectToBeNear)
    {

    }

    public override agnosticTargetCalc pickNext()
    {
        return theCache.observeCache();
    }
}

*/

public class averageOfTargetPickers : targetPicker
{
    private targetPicker[] targetPickers;

    public averageOfTargetPickers(targetPicker[] targetPickersIn)
    {
        this.targetPickers = targetPickersIn;
    }

    public override agnosticTargetCalc pickNext()
    {
        Vector3 averageVector = new Vector3();
        foreach(targetPicker thisTargetPicker in targetPickers)
        {
            averageVector += thisTargetPicker.pickNext().targetPosition();
        }

        return new agnosticTargetCalc(averageVector/(targetPickers.Length));
    }
}



public class firstFromListTargetPicker : targetPicker
{

    objectSetGrabber theListGenerator;

    public firstFromListTargetPicker(objectSetGrabber theListGeneratorIn)
    {
        theListGenerator = theListGeneratorIn;
    }

    public override agnosticTargetCalc pickNext()
    {
        foreach (GameObject thisObject in theListGenerator.grab())
        {

            agnosticTargetCalc targ = new agnosticTargetCalc(thisObject);

            return targ;
        }

        return null;
    }
}

public class firstXFromListYTargetPicker : targetPicker
{

    objectCriteria theCriteria;
    objectSetGrabber theListGenerator;

    public firstXFromListYTargetPicker(objectCriteria theCriteriaIn, objectSetGrabber theListGeneratorIn)
    {
        theCriteria = theCriteriaIn;
        theListGenerator = theListGeneratorIn;
    }

    public override agnosticTargetCalc pickNext()
    {
        foreach (GameObject thisObject in theListGenerator.grab())
        {
            //Debug.Log("thisObject:  " + thisObject);

            if (theCriteria.evaluateObject(thisObject))
            {
                agnosticTargetCalc targ = new agnosticTargetCalc(thisObject);

                return targ;
            }
        }

        return null;
    }
}

public class pickMostXFromListYTargetPicker : targetPicker
{

    objectEvaluator theEvaluator;
    objectSetGrabber theListGenerator;

    public override agnosticTargetCalc pickNext()
    {
        agnosticTargetCalc targ = new agnosticTargetCalc(whichObjectOnListIsMostX(theListGenerator.grab()));

        return targ;
    }




    public GameObject whichObjectOnListIsMostX(List<GameObject> listOfObjects)
    {
        GameObject bestSoFar = null;
        float bestValueSoFar = 0;

        foreach (GameObject thisObject in listOfObjects)
        {
            if (bestSoFar == null)
            {
                bestSoFar = thisObject;
                bestValueSoFar = theEvaluator.evaluateObject(thisObject);
                continue;
            }


            float currentValue = theEvaluator.evaluateObject(thisObject);
            if (currentValue < bestValueSoFar)
            {
                bestSoFar = thisObject;
                bestValueSoFar = currentValue;
            }
        }

        return bestSoFar;
    }

}




//object set grabbers


public abstract class objectSetGrabber
{
    public abstract List<GameObject> grab();
}



public class setOfAllObjectThatMeetCriteria : objectSetGrabber
{

    objectSetGrabber theObjectSetGrabber;
    //List<objectCriteria> theCriteria;  //no no, use a multi-criteria
    objectCriteria theCriteria;

    public setOfAllObjectThatMeetCriteria(objectSetGrabber theObjectSetGrabberIn, objectCriteria theCriteriaIn)
    {
        theObjectSetGrabber = theObjectSetGrabberIn;
        theCriteria = theCriteriaIn;
    }

    public override List<GameObject> grab()
    {
        List<GameObject> newList = new List<GameObject>();

        //Debug.Log(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");

        foreach (GameObject thisObject in theObjectSetGrabber.grab())
        {
            if (thisObject == null)
            {
                continue;
            }

            //Debug.Log(thisObject);

            if (theCriteria.evaluateObject(thisObject) == false)
            {

                //Debug.Log("NOT MET");
                continue;
            }


            //Debug.Log("met");
            newList.Add(thisObject);
        }

        return newList;
    }
}



public class setOfAllObjectsInZone : objectSetGrabber
{
    GameObject theObjectWhoseZoneWeWantToLookIn;

    public setOfAllObjectsInZone(GameObject theObjectWhoseZoneWeWantToLookInInput)
    {
        theObjectWhoseZoneWeWantToLookIn = theObjectWhoseZoneWeWantToLookInInput;
    }

    public override List<GameObject> grab()
    {
        //List<GameObject> theListOfALL = new find().allObjectsInObjectsZone(theObjectWhoseZoneWeWantToLookIn); //lol

        //return theListOfALL;
        return allObjectsInObjectsZone(theObjectWhoseZoneWeWantToLookIn);
    }


    public List<GameObject> allObjectsInObjectsZone(GameObject theObject)
    {
        int zone = tagging2.singleton.whichZone(theObject);

        List<objectIdPair> pairs = tagging2.singleton.objectsInZone[zone]; //= allInZone(zone);
        return tagging2.singleton.listInObjectFormat(pairs);
    }

    /*
    public List<objectIdPair> allInZone(int zone)
    {
        return tagging2.singleton.objectsInZone[zone];
    }
    */

}

public class setOfAllObjectsWithTag : objectSetGrabber
{
    private tag2 tag;

    public setOfAllObjectsWithTag(tag2 tag)
    {
        this.tag = tag;
        tagging2.singleton.initializeTagListsIfNecessary(tag);
    }

    public override List<GameObject> grab()
    {
        List<objectIdPair> pairs = tagging2.singleton.objectsWithTag[tag]; //= allInZone(zone);
        return tagging2.singleton.listInObjectFormat(pairs);
    }
}


public class objectSetGrabberAndCacheSetter : objectSetGrabber
{
    //maybe:
    //      don't re-calculate if the set is non-null
    //      have some time or condition under which the list is set to null again???  [how?]
    // no, do 2 diff classes.  a setter, and a receiver/bserver.



    List<GameObject> theList = new List<GameObject>();

    objectSetGrabber theSetToCache;  //??????  how to do this???




    public override List<GameObject> grab()
    {
        //this function always generates a FRESH list

        theList = theSetToCache.grab();

        return theList;
    }

    public List<GameObject> observeCache()
    {
        if (theList == null)
        {
            return grab();
        }

        return theList;
    }

    internal void add(objectSetGrabber theObjectSetIn)
    {
        theSetToCache = theObjectSetIn;
    }
}


public class objectListCacheReceiver : objectSetGrabber     //look at how cute and simple this is!
{
    objectSetGrabberAndCacheSetter theCache;

    public override List<GameObject> grab()
    {
        return theCache.observeCache();
    }
}


public class excludeX : objectSetGrabber
{
    GameObject toExclude;
    objectSetGrabber nestedSet;

    public excludeX(objectSetGrabber nestedSetIn, GameObject toExcludeIn)
    {
        nestedSet = nestedSetIn;
        toExclude = toExcludeIn;
    }

    public override List<GameObject> grab()
    {
        List<GameObject> newList = new List<GameObject>();

        foreach (GameObject obj in nestedSet.grab())
        {
            if (obj != toExclude)
            {
                newList.Add(obj);
            }
        }

        return newList;
    }
}




//should switch stuff to just grabbing/picking objects with CRITERIA?  ya, that's better way to do it.

public class setOfAllNearbyStuffStuff : objectSetGrabber
{
    GameObject theObjectWeWantStuffNear;
    stuffType theStuffTypeX;

    public setOfAllNearbyStuffStuff(GameObject theObjectWeWantStuffNearIn, stuffType theStuffTypeXIn)
    {
        theObjectWeWantStuffNear = theObjectWeWantStuffNearIn;
        theStuffTypeX = theStuffTypeXIn;

        //Debug.Log("theStuffTypeX:  " + theStuffTypeX);
    }


    public override List<GameObject> grab()
    {
        return allNearbyObjectsWithStuffTypeX(theStuffTypeX);
    }


    public List<GameObject> allNearbyObjectsWithStuffTypeX(stuffType theStuffTypeX)
    {

        List<GameObject> theListOfALL = new find().allObjectsInObjectsZone(theObjectWeWantStuffNear);  //lol forgot, this is ONE way to grab functions
        List<GameObject> theListOfObjects = new List<GameObject>();

        //Debug.Log("theListOfALL.Count:  "+theListOfALL.Count);

        foreach (GameObject thisObject in theListOfALL)
        {

            //Debug.Log("thisObject:  " + thisObject);
            stuffStuff theComponent = thisObject.GetComponent<stuffStuff>();

            if (theComponent == null)
            {

                //Debug.Log("(theComponent == null)");
                continue;
            }

            //Debug.Log("theStuffTypeX:  " + theStuffTypeX);
            //Debug.Log("theComponent.theTypeOfStuff:  " + theComponent.theTypeOfStuff);
            if (theComponent.theTypeOfStuff == theStuffTypeX)
            {
                //Debug.Log("(theComponent.theTypeOfStuff == theStuffTypeX),   so:  theListOfObjects.Add(thisObject);");
                theListOfObjects.Add(thisObject);
            }
        }

        return theListOfObjects;
    }
}
public class setOfAllNearbyNumericalVariable : objectSetGrabber
{
    numericalVariable theVariableType;


    GameObject theObjectThatIsLooking;

    public setOfAllNearbyNumericalVariable(GameObject theObjectThatIsLookingIn, numericalVariable theVariableTypeIn)
    {
        theObjectThatIsLooking = theObjectThatIsLookingIn;
        theVariableType = theVariableTypeIn;
    }

    public override List<GameObject> grab()
    {
        return allNearbyObjectsWithVariableX(theVariableType);
    }


    public List<GameObject> allNearbyObjectsWithVariableX(numericalVariable theVariableTypeIn)
    {

        List<GameObject> theListOfALL = new find().allObjectsInObjectsZone(theObjectThatIsLooking);  //lol forgot, this is ONE way to grab functions
        List<GameObject> theListOfObjects = new List<GameObject>();

        //Debug.Log("theListOfALL.Count:  "+theListOfALL.Count);

        foreach (GameObject thisObject in theListOfALL)
        {

            //Debug.Log("thisObject:  " + thisObject);
            interactable2 theComponent = thisObject.GetComponent<interactable2>();

            if (theComponent == null)
            {

                //Debug.Log("(theComponent == null)");
                continue;
            }

            if (theComponent.dictOfIvariables.ContainsKey(theVariableType))
            {
                //Debug.Log("(theComponent.theTypeOfStuff == theStuffTypeX),   so:  theListOfObjects.Add(thisObject);");
                theListOfObjects.Add(thisObject);
            }
        }

        return theListOfObjects;
    }
}



public class setOfAllInventoryObjects : objectSetGrabber
{
    //GameObject theObjectWithInventory;
    inventory1 theInventory;


    public setOfAllInventoryObjects(GameObject theObjectWithInventory)
    {
        theInventory = theObjectWithInventory.GetComponent<inventory1>();
    }

    public override List<GameObject> grab()
    {
        return theInventory.inventoryItems;
    }
}
public class setOfAllEquippedObjects : objectSetGrabber
{
    //GameObject theObjectWithInventory;
    playable2 thePlayable;


    public setOfAllEquippedObjects(GameObject theObjectWithEquipperSlots)
    {
        thePlayable = theObjectWithEquipperSlots.GetComponent<playable2>();
    }

    public override List<GameObject> grab()
    {
        List<GameObject> newList = new List<GameObject>();

        foreach (var key in thePlayable.equipperSlotsAndContents.Keys)
        {
            GameObject thisObject = thePlayable.equipperSlotsAndContents[key];
            if (thisObject != null)
            {
                newList.Add(thisObject);
            }
        }


        return newList;
    }
}


//grabbing INDIVIDUAL OBJECTS

public abstract class individualObjectReturner
{
    public abstract GameObject returnObject();
}

public class objectCacheSetter : individualObjectReturner
{
    //maybe:
    //      don't re-calculate if the target is non-null
    //      have some time or condition under which the target is set to null again???  [how?]
    // no, do 2 diff classes.  a setter, and a receiver/bserver.



    individualObjectReturner theNestedReturner;

    GameObject theCachedObject;


    public objectCacheSetter(individualObjectReturner theNestedReturnerIn)
    {
        theNestedReturner = theNestedReturnerIn;
    }




    public GameObject observeCache()
    {
        if (theCachedObject == null)
        {
            return returnObject();
        }

        return theCachedObject;
    }

    public override GameObject returnObject()
    {
        theCachedObject = theNestedReturner.returnObject();
        return theCachedObject;
    }
}


public class objectCacheReceiver : individualObjectReturner
{
    objectCacheSetter theCache;

    public objectCacheReceiver(objectCacheSetter theCacheIn)
    {
        //"theCacheIn" is the SETTER?  i think
        theCache = theCacheIn;
    }


    public override GameObject returnObject()
    {
        return theCache.observeCache();
    }
}

public class pickFirstObjectXFromListY : individualObjectReturner
{

    objectCriteria theCriteria;
    objectSetGrabber theListGenerator;

    public pickFirstObjectXFromListY(objectCriteria theCriteriaIn, objectSetGrabber theListGeneratorIn)
    {
        theCriteria = theCriteriaIn;
        theListGenerator = theListGeneratorIn;
    }

    public override GameObject returnObject()
    {

        //Debug.Log("theListGenerator:  "+theListGenerator);
        foreach (GameObject thisObject in theListGenerator.grab())
        {
            //Debug.Log("thisObject:  " + thisObject);
            //Debug.Log("theCriteria:  " + theCriteria);
            if (theCriteria.evaluateObject(thisObject))
            {
                return thisObject;
            }
        }

        return null;
    }
}


public class presetObject : individualObjectReturner
{
    GameObject theObject;

    public presetObject(GameObject theObjectIn)
    {
        theObject = theObjectIn;
    }



    public override GameObject returnObject()
    {
        return theObject;
    }
}
