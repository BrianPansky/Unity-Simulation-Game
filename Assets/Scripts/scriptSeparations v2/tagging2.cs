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
        zoneable
    }



    void Awake()
    {
        //Debug.Log("Awake:  " + this);
        singletonify();
    }

    void singletonify()
    {
        //Debug.Log("singleton, right?  " + this);


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
        //this funciton updates BOTH lists of tags
        //the "local" list, and the "global"...Dicitonary of .....objectIdPairs

        //update "local" tags
        //tags.Add(tag);
        genGen.singleton.ensureSafetyForDeletion(theObject);


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

        //int idNumberrrr = this.gameObject.GetInstanceID();

        

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
        //          globalTags[tag].Remove(idNumberrrr);

        if (objectsWithTag[tag] == null || objectsWithTag[tag].Count == 0) { return; }

        //int lengthOfList = objectsWithTag[tag].Count;
        int currentIndex = 0;


        objectIdPair theIdPair = idPairGrabify(theObject);
        if(objectsWithTag[tag] !=null && objectsWithTag[tag].Contains(theIdPair))
        {
            objectsWithTag[tag].Remove(theIdPair);
        }

        /*
        //need to find the object in the list of tagged objects, then remove it from that list
        foreach (objectIdPair pair in objectsWithTag[tag])
        {

            //if (pair.theObject == null)

            if (pair.theObject == theObject)
            {
                //.............wait, can't modify because we are using it in for loop?
                //solution:
                //      have the list of pairs
                //      the "foreach" can go through a range of indexes
                //      find the index.....no wait, that won't delete it from the...yes it will?
                //      it will work if i use the list that is in the dictionary, NOT a new different list.  yes.
                break;
            }

            currentIndex++;
        }

        objectsWithTag[tag].RemoveAt(currentIndex);

        */
    }
    public void removeALLtags(GameObject theObject)
    {
        //REMOVES this object from ALL tag lists
        //necessary when destroying objects, 
        //otherwise there will be "null" object references on those lists!

        //ok, but i need to remove it from pairsById, tagsOnObject, AND zoneOfObject as well then, and probably rename the function to be clearer

        //which data to modify?
        //      tagsOnObject
        //      objectsWithTag
        //  what about:
        //          objectsInZone
        //          zoneOfObject
        //          pairsById
        //???????


        objectIdPair newIdPair = idPairGrabify(theObject);


        
        //          !!!!!!!!!!!  ZONES  !!!!!!!!!!!!!

        if (zoneOfObject.ContainsKey(newIdPair))
        {
            if (objectsInZone[zoneOfObject[newIdPair]].Contains(newIdPair))
            {

                /*
                Debug.Log("111111111so....zone _______with id________ contains pair with id ________ for object with id________:  "
            + zoneOfObject[newIdPair] + "zone id:  " + zoneOfObject[newIdPair].GetHashCode()
            + "contains pair (boolean):  " + tagging2.singleton.objectsInZone[zoneOfObject[newIdPair]].Contains(newIdPair)
            + "pair id:  " + newIdPair.GetHashCode() + "object id:  " + newIdPair.theObjectIdNumber);
                */

                objectsInZone[zoneOfObject[newIdPair]].Remove(newIdPair);
            }
            else
            {
                Debug.Log("objectsInZone[zoneOfObject[newIdPair]].Contains(newIdPair) is FALSE, object is not in the zone list, zone list does not contain the object, well the idpair");

            }

            removeFromZone(theObject, zoneOfObject[newIdPair]);
        }
        else
        {
            Debug.Log("zoneOfObject.ContainsKey(newIdPair) is FALSE, object has no zone");

        }







        //      objectsWithTag
        //      do this FIRST.  need to know which tags the object will be filed under

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

        //      tagsOnObject, just clear its list?
        //          also remove the object itself from dictionary maybe?  so just remove the entire KEY from tagsOnObject?
        //          i didn't do this with previous system because they were on object?  get deleted when you delete object?  maybe?  i think?

        //now i need to remove it from all the OTHER dictionaries too?  or at least make functions to modify those dictionaries similarly.
        //fuck, it's the SAME SHAPE.  i should NOT have to re-make the function for each!
        //i should be able to use the same shape, then just plug in ....gahhh.
        //i guess i can with copy and paste, but that's annoying!
        //can i have some kind of abstract variable/dictionary that i can use somehow, that can do it for ALL of them?
        //well, the id number is the same in all cases.  just.....use that and do stuff for allthe others at once?  eh....
    }

    void waitIsThisUseless(GameObject theObject)
    {
        //can't i just CLEAR the list if i'm removing ALL tags?????

        
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

        objectIdPair thisObjectIdPair = idPairGrabify(theObject);


        if (zoneOfObject[thisObjectIdPair] != zone) { return; }

        objectsInZone[zone].Remove(thisObjectIdPair);
    }















    public objectIdPair idPairGrabify(GameObject theObject)
    {
        //objectIdPair newIdPair = idPairify(theObject);
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
