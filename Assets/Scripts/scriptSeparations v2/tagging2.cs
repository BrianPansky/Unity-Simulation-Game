using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        interactable,
        mapZone
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




    //funcitons that modify an object's tags
    public void addTag(GameObject theObject, tag2 tag)
    {
        //this funciton updates BOTH lists of tags
        //the "local" list, and the "global"...Dicitonary of .....objectIdPairs

        //update "local" tags
        //tags.Add(tag);

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


        //objectIdPair newIdPair = idPairify(theObject);
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
    }
    public void removeALLtags(GameObject theObject)
    {
        //REMOVES this object from ALL tag lists
        //necessary when destroying objects, 
        //otherwise there will be "null" object references on those lists!

        //which data to modify?
        //      tagsOnObject
        //      objectsWithTag
        //  what about:
        //          objectsInZone
        //          zoneOfObject
        //          pairsById
        //???????


        objectIdPair newIdPair = idPairify(theObject);

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








    public GameObject findXNearestToY(GameObject objectWeWantItClosestTo, tag2 tagToLookFor)
    {
        //      EXCEPT for the input object itself!!!


        //one tag input for now
        //return nearest object with that tag
        //other funciton can be called "nearest XYZ" or something lol


        //stackoverflow.com/questions/63106256/find-and-return-nearest-gameobject-with-tag-unity
        //var sorted = NearGameobjects.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        //  List<GameObject> allPotentialTargets = listInObjectFormat(objectsWithTag[tagToLookFor]);
        List<objectIdPair> allPotentialTargets = objectsWithTag[tagToLookFor];
        //List<GameObject> sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        //var sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        return whichOBJECTOnObjectIdPairListIsNearestToInputtedObject(objectWeWantItClosestTo, allPotentialTargets);
    }


    public GameObject whichOBJECTOnObjectIdPairListIsNearestToInputtedObject(GameObject objectWeWantItClosestTo, List<objectIdPair> allPotentialTargets)
    {
        //      EXCEPT for the input object itself!!!

        //how to make it not return the inputted object?

        GameObject theClosestSoFar = null;
        //Debug.Log("===================================================");
        //Debug.Log("objectWeWantItClosestTo.GetInstanceID():  " + objectWeWantItClosestTo.GetInstanceID());

        foreach (objectIdPair thisObjectIdPair in allPotentialTargets)
        {
            //Debug.DrawLine(objectWeWantItClosestTo.transform.position, thisObjectIdPair.theObject.transform.position, Color.green, 12f);

            //Debug.Log(":::::::::::::::::::::::::::::::::::::::::::");

            //Debug.Log("thisObjectIdPair.theObjectIdNumber:  " + thisObjectIdPair.theObjectIdNumber);
            
            if (thisObjectIdPair.theObjectIdNumber == objectWeWantItClosestTo.GetInstanceID())
            {
                //Debug.Log("1111111111111111111111111111111111111111");
                continue;
            }

            if (theClosestSoFar == null)
            {
                //Debug.Log("22222222222222222222222222222222222222222222222");
                theClosestSoFar = thisObjectIdPair.theObject;
                continue;
            }

            float distanceToThisObject = Vector3.Distance(thisObjectIdPair.theObject.transform.position, objectWeWantItClosestTo.transform.position);
            float distanceToTheClosestSoFar = Vector3.Distance(theClosestSoFar.transform.position, objectWeWantItClosestTo.transform.position);

            //Debug.Log("distanceToThisObject:  " + distanceToThisObject);
            //Debug.Log("distanceToTheClosestSoFar:  " + distanceToTheClosestSoFar);
            if (distanceToThisObject > distanceToTheClosestSoFar)
            {

                //Debug.Log("distanceToThisObject > distanceToTheClosestSoFar!!!!!!!!!!");
                continue;
            }

            //Debug.Log("444444444444444444444444444444444444");
            theClosestSoFar = thisObjectIdPair.theObject;

        }


        //Debug.DrawLine(objectWeWantItClosestTo.transform.position, theClosestSoFar.transform.position, Color.red, 2f);

        return theClosestSoFar;
    }










    public objectIdPair idPairGrabify(GameObject theObject)
    {
        objectIdPair newIdPair = idPairify(theObject);
        if (pairsById.ContainsKey(newIdPair.theObjectIdNumber))
        {
            return pairsById[newIdPair.theObjectIdNumber];
        }
        else
        {
            pairsById[newIdPair.theObjectIdNumber] = newIdPair;
            return newIdPair;
        }

    }

    public objectIdPair idPairify(GameObject theObject)
    {
        objectIdPair newobjIdPair = new objectIdPair();
        newobjIdPair.theObject = theObject;
        newobjIdPair.theObjectIdNumber = theObject.GetHashCode();  //this???  or other id number???
        return newobjIdPair;


    }


    public void setObjectAsMemberOfZone(GameObject theObject, int zoneNumber)
    {
        //tagging2.singleton.objectsInZone[zoneNumber].Add(theObject);
        //tagging2.singleton.zoneOfObject

    }
}



public class objectIdPair
{
    //      !!!!! SHOULD SWITCH TO USING ID NUMBERS IN DICTIONARY INSTEAD OF GAME OBJECT!  MAKES IT EASIER TO TRACK DOWN ERRORS WHEN OBJECT IS DESTROYED!  BECAUSE I CAN PRINT id NUMBER EVERYYYYY TIME I DESTROY ANY OBJECT!!!!!!!!!!!!!
    //buuuuuuuuuuut you can't get object if you have ID!!!!!  very annoying.  so use this instead
    public int theObjectIdNumber = 0;
    public GameObject theObject;
}
