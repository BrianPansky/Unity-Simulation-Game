using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
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
        threat1
    }



    void Awake()
    {
        //Debug.Log("Awake:  " + this);
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

    

}

public class objectIdPair
{
    //      !!!!! SHOULD SWITCH TO USING ID NUMBERS IN DICTIONARY INSTEAD OF GAME OBJECT!  MAKES IT EASIER TO TRACK DOWN ERRORS WHEN OBJECT IS DESTROYED!  BECAUSE I CAN PRINT id NUMBER EVERYYYYY TIME I DESTROY ANY OBJECT!!!!!!!!!!!!!
    //buuuuuuuuuuut you can't get object if you have ID!!!!!  very annoying.  so use this instead
    public int theObjectIdNumber = 0;
    public GameObject theObject;
}
