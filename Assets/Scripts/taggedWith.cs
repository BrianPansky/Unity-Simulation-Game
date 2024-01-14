using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taggedWith : MonoBehaviour
{
    //my own tagging system, not using the unity tagging system

    public List<string> tags = new List<string>();

    //public GameObject theWorldObject;



    //kind of ad-hoc, but hopefully good enough most of the time:
    public string tag1;
    public string tag2;
    public string tag3;
    public string tag4;

    List<string> tagsToAdd = new List<string>();

    worldScript theWorldScript;


    public Dictionary<string, List<GameObject>> globalTags;

    // Start is called before the first frame update
    void Awake()
    {
        //getting the "global" tags:
        //(hopefully will allow me to modify them here
        //SHOULD be a shallow copy, NOT a deep copy)
        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        globalTags = theWorldScript.taggedStuff;



    }

    void Start()
    {
        //this tag1 tag2 stuff is just my quick way to add a few tags to an object

        //add all tags to the tag list:
        tagsToAdd.Add(tag1);
        tagsToAdd.Add(tag2);
        tagsToAdd.Add(tag3);
        tagsToAdd.Add(tag4);

        //add name:
        addTag(this.gameObject.name);


        //add all tags to this GameObject:
        foreach (string thisTag in tagsToAdd)
        {
            //make sure it's not null:
            if (thisTag != null)
            {
                addTag(thisTag);
            }
        }
    }

    
    //funcitons that modify current object's tags [the object this script is attached to]
    public void addTag(string tag)
    {
        //this funciton updates BOTH lists of tags
        //the "local" list, and the "global"...Dicitonary of objects

        //update "local" tags
        tags.Add(tag);

        //update "global" tags...
        //but dictionaries are tricky objects, and must be checked and all that:
        if(globalTags.ContainsKey(tag))
        {
            //add the game object to the list of objects tagged with that tag:
            globalTags[tag].Add(gameObject);
        }
        else
        {
            //sigh, need to add the key first, which means the list it unlocks as well...
            List<GameObject> needsList = new List<GameObject>();
            needsList.Add(gameObject);
            globalTags.Add(tag, needsList);
        }

        
    }

    public void removeTag(string tag)
    {
        //this funciton updates BOTH lists of tags
        //the "local" list, and the "global"...Dicitonary of objects

        //should I add checks here to make sure the tag is present?
        //because it will have errors if I try to remove a tag that's basicaly already removed...

        //update "local" tags
        tags.Remove(tag);

        //update "global" tags...
        //remove the game object to the list of objects tagged with that tag:
        globalTags[tag].Remove(gameObject);
    }

    public void removeALLtags()
    {
        //REMOVES this object from ALL tag lists
        //necessary when destroying objects, 
        //otherwise there will be "null" object references on those lists!

        foreach(string tag in tags)
        {
            removeTag(tag);
        }
    }


    //funcitons that modify a foreign object's tags
    public void foreignAddTag(string tag, GameObject addToThis)
    {
        //this funciton updates BOTH lists of tags
        //the "local" list, and the "global"...Dicitonary of objects

        //update "local" tags
        tags.Add(tag);

        //update "global" tags...
        //but dictionaries are tricky objects, and must be checked and all that:
        if (globalTags.ContainsKey(tag))
        {
            //add the game object to the list of objects tagged with that tag:
            globalTags[tag].Add(addToThis);
        }
        else
        {
            //sigh, need to add the key first, which means the list it unlocks as well...
            List<GameObject> needsList = new List<GameObject>();
            needsList.Add(addToThis);
            globalTags.Add(tag, needsList);
        }


    }

    public void foreignRemoveGlobalTag(string tag, GameObject removeFromThis)
    {
        //update "global" tags...
        //the "global"...Dicitonary of objects

        //should I add checks here to make sure the tag is present?
        //because it will have errors if I try to remove a tag that's basicaly already removed...
        if (globalTags.ContainsKey(tag))
        {
            //remove the game object from the list of objects tagged with that tag:
            globalTags[tag].Remove(removeFromThis);
        }

        
        
    }

    public void foreignRemoveBOTHTags(string tag, GameObject foreignGameObject)
    {
        //this funciton updates BOTH lists of tags
        //the "local" list, and the "global"...Dicitonary of objects


        taggedWith foreignGameObjectsTagScript = foreignGameObject.GetComponent<taggedWith>();

        //update "local" tags
        foreignGameObjectsTagScript.tags.Remove(tag);

        //should I add checks here to make sure the tag is present?
        //because it will have errors if I try to remove a tag that's basicaly already removed...
        if (globalTags.ContainsKey(tag))
        {
            //remove the game object from the list of objects tagged with that tag:
            globalTags[tag].Remove(foreignGameObject);
        }



    }


    public void foreignRemoveALLtags(GameObject foreignGameObject)
    {

        //REMOVES this object from ALL tag lists
        //necessary when destroying objects, 
        //otherwise there will be "null" object references on those lists!
        
        taggedWith foreignGameObjectsTagScript = foreignGameObject.GetComponent<taggedWith>();

        //remove object from all global tag lists:
        foreach (string tag in foreignGameObjectsTagScript.tags)
        {
            foreignRemoveGlobalTag(tag, foreignGameObject);
            //globalTags[tag].Remove(thisGameObject);
        }

        //delete local tags:
        foreignGameObjectsTagScript.tags.Clear();
    }

    //diagnostic:
    public void printAllTags()
    {


        string printout = string.Empty;

        printout += "Tags:  [ ";

        foreach (string tag in tags)
        {
            printout += tag + ", ";
        }

        printout += "]";

        Debug.Log(printout);

        
    }

    public string allForeignTagsAsText(GameObject thisForeignObject)
    {
        taggedWith thisForeignSocialScript = thisForeignObject.GetComponent<taggedWith>();

        if(thisForeignSocialScript != null)
        {
            string printout = string.Empty;

            printout += "Tags:  [ ";



            foreach (string tag in thisForeignSocialScript.tags)
            {
                printout += tag + ", ";
            }

            printout += "]";

            return printout;
        }

        else
        {
            return "no tag script";
        }
        
    }



    //find objects using tags:
    
    public GameObject pickRandomObjectFromListEXCEPT(List<GameObject> theList, GameObject notTHISObject)
    {
        //use with my "ALLTaggedWithMultiple" funcitons etc., they return a list of objects

        //Debug.Log("///////////////// here is the list:");
        //foreach (GameObject obj in theList)
        {
            //Debug.Log(obj.name);
        }


        int numberOfTries = 10; //easy ad hoc way to terminate a potentially infinate loop for now lol
        GameObject thisObject;
        thisObject = null;


        while (numberOfTries > 0)
        {
            int randomIndex = Random.Range(0, theList.Count);
            thisObject = theList[randomIndex];

            if (thisObject != notTHISObject)
            {
                return thisObject;
            }

            numberOfTries--;
        }
        
        


        return thisObject;

    }
    
    public GameObject randomTaggedWith(string theTag)
    {
        //should return ONE random GameObject that is tagged with the inputted tag

        List<GameObject> allPotentialTargets = new List<GameObject>();

        if (globalTags.ContainsKey(theTag))
        {
            allPotentialTargets = globalTags[theTag];
        }


        /*
        if (theTag == "shop")
        {
            print("choosing a shop...");

            print(allPotentialTargets.Count);

            foreach(GameObject item in allPotentialTargets)
            {
                print(item.transform.position);
            }
        }
        */


        if (allPotentialTargets.Count > 0)
        {
            GameObject thisObject;
            thisObject = null;
            int randomIndex = Random.Range(0, allPotentialTargets.Count);
            thisObject = allPotentialTargets[randomIndex];


            return thisObject;
        }
        else
        {
            return null;
        }
    }

    public GameObject randomTaggedWithMultiple(string theTag, string tag2 = null, string tag3 = null, string tag4 = null)
    {
        //should return ONE random GameObject that is tagged with ALL inputted tags


        //ad-hoc for debug
        functionsForAI theFunctions = GetComponent<functionsForAI>();
        //theFunctions.print("---------------------------- start randomTaggedWithMultiple -----------------------------");
        //theFunctions.print(theTag + ", " + tag2  + ", " + tag3  + ", " + tag4);




        List<GameObject> allPotentialTargets = new List<GameObject>();

        if (globalTags.ContainsKey(theTag))
        {
            allPotentialTargets = globalTags[theTag];
        }
        
        //foreach(GameObject thisListItem in allPotentialTargets)
        {
            //theFunctions.print(thisListItem.name);
        }

        //BUT THAT'S A SHALLOW COPY!
        //so I need to make a corrosponding list of indices to use, to prevent messing with it:
        List<int> listOfIndices = new List<int>();
        int length = 0;
        //WILL THIS MAKE IT THE RIGHT NUMBER?  OR ONE TOO MANY?  ONE TOO FEW???
        //I think it's correct now?
        while (length < allPotentialTargets.Count)
        {
            listOfIndices.Add(length);
            length += 1;
        }

        /*
        if (theTag == "shop")
        {
            print("choosing a shop...");
            //print(length);
        }
        */



        //put the optional other tags in a list:
        List<string> otherTags = new List<string>();
        otherTags.Add(tag2);
        otherTags.Add(tag3);
        otherTags.Add(tag4);



        GameObject thisObject;
        thisObject = null;
        bool doWeHaveGoodTarget = false;

        int randomNumber;
        int myIndex;

        //theFunctions.print("now the while loop-------------");
        //print("do we have even ONE of these????");
        //print(theTag);
        //print(allPotentialTargets.Count);
        while (doWeHaveGoodTarget == false && listOfIndices.Count > 0)
        {
            //print("yes at least one");
            //grab a randomn object from the list:
            randomNumber = Random.Range(0, listOfIndices.Count);
            myIndex = listOfIndices[randomNumber];

            thisObject = allPotentialTargets[myIndex];

            //theFunctions.print(thisObject.name);
            //theFunctions.print("should NOT be null");

            //now, check all the other tags on that^ object
            //if it lacks a needed tag, remove that item from the array
            //and choose again
            //to do that, need to grab the tags on that object:
            taggedWith theTagScript = allPotentialTargets[myIndex].GetComponent("taggedWith") as taggedWith;

            //assume this object will correctly have ALL the tags
            //then falsify by checking:
            doWeHaveGoodTarget = true;

            //theFunctions.print("has these tags:");
            //foreach (string thisTag in theTagScript.tags)
            {
                //theFunctions.print(thisTag);
            }

            foreach (string thisTag in otherTags)
            {
                //make sure it's not null:
                if (thisTag != null)
                {
                    //theFunctions.print(thisTag);

                    if (theTagScript.tags.Contains(thisTag) == false)
                    {
                        //theFunctions.print("doesn't contain this tag");
                        //print("grrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
                        //print(thisTag);

                        doWeHaveGoodTarget = false;
                        listOfIndices.RemoveAt(randomNumber);

                        //set thisObject back to null:
                        //theFunctions.print("should be null");
                        thisObject = null;
                        break;
                    }
                    /*
                    else
                    {
                        print("YAAAAAAAAAAAYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
                        print(thisTag);
                    }
                    */
                }
            }

            //see if the object passed the test:
            if (doWeHaveGoodTarget == true)
            {
                //theFunctions.print("we have a good target");
                //theFunctions.print(thisObject.name);
                /*
                if (theTag == "shop")
                {
                    print("FOUND a shop...");
                    print(myIndex);
                    print(thisObject.name);
                }
                */

                return thisObject;
            }
        }

        //theFunctions.print("ends up here, meaning that no good object was found");
        //this will be null if the above loop didn't find one:
        return thisObject;
    }

    public List<GameObject> ALLTaggedWithMultiple(string theTag, string tag2 = null, string tag3 = null, string tag4 = null)
    {
        //INPUTS setup
        //STARTING place for contructing answer to return
        List<GameObject> starterList = new List<GameObject>();
        if (globalTags.ContainsKey(theTag))
        {
            starterList = globalTags[theTag];
        }

        //put the optional other tags in a list:
        List<string> otherTags = new List<string>();
        otherTags.Add(tag2);
        otherTags.Add(tag3);
        otherTags.Add(tag4);


        //OUTPUT initializing
        //only add items to following list when they are CONFIRMED to fit ALL criteria:
        List<GameObject> allThatFit = new List<GameObject>();


        bool thisObjectFitsTheCriteria = false;



        //print(theTag);
        //print(allPotentialTargets.Count);
        foreach (GameObject thisObject in starterList)
        {

            //now, check all the other tags on that^ object
            //if it lacks a needed tag, don't add it to the output
            //to do that, need to grab the tags on that object:
            taggedWith theTagScript = thisObject.GetComponent("taggedWith") as taggedWith;

            //assume this object will correctly have ALL the tags
            //then falsify by checking:
            thisObjectFitsTheCriteria = true;


            foreach (string thisTag in otherTags)
            {
                //make sure it's not null:
                //[CAN BE NULL BECAUSE THESE ARE THE OPTIONAL INPUTS, WHICH DEFAULT TO "NULL"]
                if (thisTag != null)
                {

                    if (theTagScript.tags.Contains(thisTag) == false)
                    {
                        //print("grrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
                        //print(thisTag);

                        thisObjectFitsTheCriteria = false;

                        break;
                    }
                    /*
                    else
                    {
                        print("YAAAAAAAAAAAYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
                        print(thisTag);
                    }
                    */
                }
            }

            //see if the object passed the test:
            if (thisObjectFitsTheCriteria == true)
            {
                allThatFit.Add(thisObject);
            }
        }

        return allThatFit;
    }

    public List<GameObject> ALLTaggedWithANY(List<string> tagList)
    {
        //takes a list of tags
        //returns ALL gameObjects that have ANY of those tags
        //difficlut part is handing DUPLICATES

        List<GameObject> objectList = new List<GameObject>();

        foreach (string thisTag in tagList)
        {
            if (globalTags.ContainsKey(thisTag))
            {
                foreach (GameObject thisObject in globalTags[thisTag])
                {
                    //only add it if we don't already have it:
                    if (objectList.Contains(thisObject) != true)
                    {
                        objectList.Add(thisObject);
                    }
                }
            }
        }

        return objectList;
    }

    public GameObject randomTaggedWithMIX(List<GameObject> positiveList, string tag1, string tag2 = null, string tag3 = null, string tag4 = null)
    {
        //"mix" of positive and negatives
        //positive means it HAS the tag, negative means it EXCLUDES the tag
        //the first input is everything that contains the positive tags [found using "ALLTaggedWithMultiple"]
        //the other string inputs are optional, and they FILTER OUT tags


        //INPUTS setup

        //put all unwanted tags in a list:
        List<string> unwantedTags = new List<string>();
        unwantedTags.Add(tag1);
        unwantedTags.Add(tag2);
        unwantedTags.Add(tag3);
        unwantedTags.Add(tag4);


        //OUTPUT initializing
        //only add items to following list when they are CONFIRMED to fit ALL criteria:
        List<GameObject> allThatFit = new List<GameObject>();




        //so I need to make a corrosponding list of indices to use, to prevent messing with it:
        List<int> listOfIndices = new List<int>();
        int length = 0;
        //WILL THIS MAKE IT THE RIGHT NUMBER?  OR ONE TOO MANY?  ONE TOO FEW???
        //I think it's correct now?
        while (length < positiveList.Count)
        {
            listOfIndices.Add(length);
            length += 1;
        }






        //print(theTag);
        //print(allPotentialTargets.Count);


        GameObject thisObject;
        thisObject = null;
        bool doWeHaveGoodTarget = false;

        int randomNumber;
        int myIndex;

        //print("do we have even ONE of these????");
        //print(theTag);
        //print(allPotentialTargets.Count);
        while (doWeHaveGoodTarget == false && listOfIndices.Count > 0)
        {
            //print("yes at least one");
            //grab a randomn object from the list:
            randomNumber = Random.Range(0, listOfIndices.Count);
            myIndex = listOfIndices[randomNumber];

            thisObject = positiveList[myIndex];


            //now, check all the other tags on that^ object
            //if it has unwanted tag, remove that item from the array
            //and choose again
            //to do that, need to grab the tags on that object:
            taggedWith theTagScript = thisObject.GetComponent("taggedWith") as taggedWith;

            //assume this object will correctly have NO UNWANTED tags
            //then falsify by checking:
            doWeHaveGoodTarget = true;


            foreach (string thisUnwantedTag in unwantedTags)
            {
                //make sure it's not null:
                if (thisUnwantedTag != null)
                {

                    if (theTagScript.tags.Contains(thisUnwantedTag) == true)
                    {
                        //print("grrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
                        //print(thisTag);

                        doWeHaveGoodTarget = false;
                        listOfIndices.RemoveAt(randomNumber);

                        //set thisObject back to null:
                        thisObject = null;
                        break;
                    }
                    /*
                    else
                    {
                        print("YAAAAAAAAAAAYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
                        print(thisTag);
                    }
                    */
                }
            }

            //see if the object passed the test:
            if (doWeHaveGoodTarget == true)
            {

                return thisObject;
            }
        }

        //this will be null if the above loop didn't find one:
        return thisObject;


    }

    public List<GameObject> narrowingTaggedWithAll(List<GameObject> objectList, string tag1, string tag2 = null, string tag3 = null, string tag4 = null)
    {
        //takes a list of objects
        //returns all objects from the list that fit a set of tags

        //so, create a NEW list to return
        //iterate through OLD list, add objects to new list if they fit criteria
        //return new list

        //INPUTS setup


        //OUTPUT initializing
        //only add items to following list when they are CONFIRMED to fit ALL criteria:
        List<GameObject> allThatFit = new List<GameObject>();

        foreach (GameObject thisObject in objectList)
        {
            if (hasAllTags(thisObject, tag1, tag2, tag3, tag4))
            {
                allThatFit.Add(thisObject);
            }
        }

        return allThatFit;

    }

    public GameObject randomObjectFromList(List<GameObject> objectList)
    {
        //but check to be sure list is not empty:

        if (objectList.Count > 0)
        {
            int randomNumber = Random.Range(0, objectList.Count);

            return objectList[randomNumber];
        }
        else
        {
            return null;
        }
    }

    public List<GameObject> narrowingTaggedWithNoneOnList(List<GameObject> objectList, List<string> unwantedTags)
    {
        //takes a list of objects, and a list of tags
        //returns all objects from the list that have NONE of the tags

        //so, create a NEW list to return
        //iterate through OLD list, add objects to new list if they fit criteria
        //return new list

        //OUTPUT initializing
        //only add items to following list when they are CONFIRMED to have NONE of the tags:
        List<GameObject> allThatFit = new List<GameObject>();


        //print("who fits??????????????????");

        foreach (GameObject thisObject in objectList)
        {
            //we don't want it to have ANY of the tags on that list:
            if (hasANYTagsOnList(thisObject, unwantedTags) == false)
            {
                //print(thisObject.name);
                allThatFit.Add(thisObject);
            }
        }


        return allThatFit;
    }

    public bool hasAllTagsOnList(GameObject thisObject, List<string> wantedTags)
    {
        //should be moved to tagging script or something!
        //returns true if this object has all tags

        //need to grab the tags on that object:
        taggedWith theTagScript = thisObject.GetComponent("taggedWith") as taggedWith;

        foreach (string tag in wantedTags)
        {
            if (theTagScript.tags.Contains(tag) == false)
            {
                return false;
            }
        }

        //if we reach this point, it means the object does indeed have all the tags we want!
        return true;
    }

    public bool hasANYTagsOnList(GameObject thisObject, List<string> wantedTags)
    {
        //should be moved to tagging script or something!
        //returns true if this object has ANY tags

        //need to grab the tags on that object:
        taggedWith theTagScript = thisObject.GetComponent("taggedWith") as taggedWith;

        foreach (string tag in wantedTags)
        {
            if (theTagScript.tags.Contains(tag) == true)
            {
                return true;
            }
        }

        //if we reach this point, it means the object does NOT have ANY the tags we want!
        return false;
    }

    public bool hasAllTags(GameObject thisObject, string tag1, string tag2 = null, string tag3 = null, string tag4 = null)
    {
        //should be moved to tagging script or something!
        //returns true if this object has all tags

        //put all unwanted tags in a list:
        List<string> wantedTags = removeNullStrings(tag1, tag2, tag3, tag4);

        //need to grab the tags on that object:
        taggedWith theTagScript = thisObject.GetComponent("taggedWith") as taggedWith;

        foreach (string tag in wantedTags)
        {
            if (theTagScript.tags.Contains(tag) == false)
            {
                return false;
            }
        }

        //if we reach this point, it means the object does indeed have all the tags we want!
        return true;
    }

    public List<string> removeNullStrings(string tag1, string tag2, string tag3, string tag4)
    {
        //input 4 strings
        //get backa  list of all of them that are NOT null

        List<string> allStrings = new List<string>();
        allStrings.Add(tag1);
        allStrings.Add(tag2);
        allStrings.Add(tag3);
        allStrings.Add(tag4);

        List<string> nonNullStrings = new List<string>();

        foreach (string thisString in allStrings)
        {
            if (thisString != null)
            {
                nonNullStrings.Add(thisString);
            }
        }

        return nonNullStrings;
    }




    public string randomStringFromList(List<string> theList)
    {

        string thisString;
        thisString = null;


        if (theList.Count > 0)
        {
            int randomIndex = Random.Range(0, theList.Count);
            thisString = theList[randomIndex];

        }
        else
        {
            Debug.Log("LIST OF STRINGS IS EMPTY!");
        }
        return thisString;
    }



    //not needed???
    public List<GameObject> ALLTaggedWithMIX(List<GameObject> positiveList, string tag1, string tag2 = null, string tag3 = null, string tag4 = null)
    {
        //"mix" of positive and negatives
        //positive means it HAS the tag, negative means it EXCLUDES the tag
        //the first input is everything that contains the positive tags [found using "ALLTaggedWithMultiple"]
        //the other string inputs are optional, and they FILTER OUT tags


        //INPUTS setup

        //put all unwanted tags in a list:
        List<string> otherTags = new List<string>();
        otherTags.Add(tag1);
        otherTags.Add(tag2);
        otherTags.Add(tag3);
        otherTags.Add(tag4);


        //OUTPUT initializing
        //only add items to following list when they are CONFIRMED to fit ALL criteria:
        List<GameObject> allThatFit = new List<GameObject>();


        bool thisObjectFitsTheCriteria = false;



        //print(theTag);
        //print(allPotentialTargets.Count);
        foreach (GameObject thisObject in positiveList)
        {

            //now, check all the other tags on that^ object
            //if it has unwanted tag, don't add it to ooutput list
            //to do that, need to grab the tags on that object:
            taggedWith theTagScript = thisObject.GetComponent("taggedWith") as taggedWith;

            //assume this object will correctly have ALL the tags
            //then falsify by checking:
            thisObjectFitsTheCriteria = true;


            foreach (string thisTag in otherTags)
            {
                //make sure it's not null:
                //[CAN BE NULL BECAUSE THESE ARE THE OPTIONAL INPUTS, WHICH DEFAULT TO "NULL"]
                if (thisTag != null)
                {

                    if (theTagScript.tags.Contains(thisTag) == true)
                    {
                        //print("grrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
                        //print(thisTag);

                        thisObjectFitsTheCriteria = false;

                        break;
                    }
                }
            }

            //see if the object passed the test:
            if (thisObjectFitsTheCriteria == true)
            {
                allThatFit.Add(thisObject);
            }
        }

        return allThatFit;


    }


    //start with an object, THEN see if it has the desired tags:
    public bool doesObjectHaveALLTags(GameObject someGameObject, string tag1, string tag2 = null, string tag3 = null, string tag4 = null)
    {
        //INPUTS setup

        //put all wanted tags in a list:
        List<string> allTags = new List<string>();
        allTags.Add(tag1);
        allTags.Add(tag2);
        allTags.Add(tag3);
        allTags.Add(tag4);

        taggedWith theTagScript = someGameObject.GetComponent("taggedWith") as taggedWith;

        foreach (string thisTag in allTags)
        {
            //make sure it's not null:
            if (thisTag != null)
            {
                //theFunctions.print(thisTag);

                if (theTagScript.tags.Contains(thisTag) == false)
                {
                    return false;
                }
            }
        }

        return true;
    }




    //bit beyond mere "which objects have these tags", but basically same "find the right object" functions:
    public GameObject findNearestX(string tagToLookFor)
    {
        //one tag input for now
        //return nearest object with that tag
        //other funciton can be called "nearest XYZ" or something lol


        //stackoverflow.com/questions/63106256/find-and-return-nearest-gameobject-with-tag-unity
        //var sorted = NearGameobjects.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        List<GameObject> allPotentialTargets = ALLTaggedWithMultiple(tagToLookFor);
        //List<GameObject> sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        //var sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        return whichObjectOnListIsNearest(allPotentialTargets);
    }

    public GameObject findXNearestToY(string tagToLookFor, GameObject objectWeWantItClosestTo)
    {
        //one tag input for now
        //return nearest object with that tag
        //other funciton can be called "nearest XYZ" or something lol


        //stackoverflow.com/questions/63106256/find-and-return-nearest-gameobject-with-tag-unity
        //var sorted = NearGameobjects.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        List<GameObject> allPotentialTargets = ALLTaggedWithMultiple(tagToLookFor);
        //List<GameObject> sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        //var sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        return whichObjectOnListIsNearestToInputtedObject(allPotentialTargets, objectWeWantItClosestTo);
    }


    public GameObject findXNearestToYExceptY(string tagToLookFor, GameObject objectWeWantItClosestTo)
    {
        //one tag input for now
        //return nearest object with that tag
        //other funciton can be called "nearest XYZ" or something lol


        //stackoverflow.com/questions/63106256/find-and-return-nearest-gameobject-with-tag-unity
        //var sorted = NearGameobjects.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        List<GameObject> allPotentialTargets = ALLTaggedWithMultiple(tagToLookFor);
        //List<GameObject> sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        //var sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        return whichObjectOnListIsNearestToInputtedObjectExceptThatObject(allPotentialTargets, objectWeWantItClosestTo);
    }


    public GameObject whichObjectOnListIsNearest(List<GameObject> listOfObjects)
    {
        //closest to what?  to THIS object, i suppose

        GameObject theClosestSoFar = null;

        foreach (GameObject thisObject in listOfObjects)
        {
            if (theClosestSoFar != null)
            {
                float distanceToThisObject = Vector3.Distance(thisObject.transform.position, this.gameObject.transform.position);
                float distanceToTheClosestSoFar = Vector3.Distance(theClosestSoFar.transform.position, this.gameObject.transform.position);

                if (distanceToThisObject < distanceToTheClosestSoFar)
                {
                    theClosestSoFar = thisObject;
                }
            }
            else
            {
                theClosestSoFar = thisObject;
            }
        }

        return theClosestSoFar;
    }

    public GameObject whichObjectOnListIsNearestToInputtedObject(List<GameObject> listOfObjects, GameObject objectWeWantItClosestTo)
    {

        GameObject theClosestSoFar = null;

        foreach (GameObject thisObject in listOfObjects)
        {
            if (theClosestSoFar != null)
            {
                float distanceToThisObject = Vector3.Distance(thisObject.transform.position, objectWeWantItClosestTo.transform.position);
                float distanceToTheClosestSoFar = Vector3.Distance(theClosestSoFar.transform.position, objectWeWantItClosestTo.transform.position);

                if (distanceToThisObject < distanceToTheClosestSoFar)
                {
                    theClosestSoFar = thisObject;
                }
            }
            else
            {
                theClosestSoFar = thisObject;
            }
        }

        return theClosestSoFar;
    }

    public GameObject whichObjectOnListIsNearestToInputtedObjectExceptThatObject(List<GameObject> listOfObjects, GameObject objectWeWantItClosestTo)
    {
        //how to make it not return the inputted object?

        GameObject theClosestSoFar = null;

        foreach (GameObject thisObject in listOfObjects)
        {
            //thisObject.name != objectWeWantItClosestTo.name

            //Debug.Log("===================================================");
            //Debug.Log(thisObject.name);
            //Debug.Log(objectWeWantItClosestTo.name);



            if (thisObject.name != objectWeWantItClosestTo.name)
            {
                if (theClosestSoFar != null)
                {
                    float distanceToThisObject = Vector3.Distance(thisObject.transform.position, objectWeWantItClosestTo.transform.position);
                    float distanceToTheClosestSoFar = Vector3.Distance(theClosestSoFar.transform.position, objectWeWantItClosestTo.transform.position);

                    if (distanceToThisObject < distanceToTheClosestSoFar)
                    {
                        theClosestSoFar = thisObject;
                    }
                }
                else
                {
                    theClosestSoFar = thisObject;
                }
            }
            else
            {
                //Debug.Log("333333333333333333333333333333333333333333333");
            }
            
        }

        return theClosestSoFar;
    }

    public GameObject findSECONDNearestXToYExceptY(string tagToLookFor, GameObject objectWeWantItClosestTo)
    {
        //one tag input for now
        //return nearest object with that tag
        //other funciton can be called "nearest XYZ" or something lol


        //stackoverflow.com/questions/63106256/find-and-return-nearest-gameobject-with-tag-unity
        //var sorted = NearGameobjects.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        List<GameObject> allPotentialTargets = ALLTaggedWithMultiple(tagToLookFor);
        foreach(GameObject obj in allPotentialTargets)
        {
            Debug.Log(obj);
        }
        //List<GameObject> sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        //var sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        return whichObjectOnListIsSECONDNearestToInputtedObjectExceptThatObject(allPotentialTargets, objectWeWantItClosestTo);
    }

    public GameObject whichObjectOnListIsSECONDNearestToInputtedObjectExceptThatObject(List<GameObject> listOfObjects, GameObject objectWeWantItClosestTo)
    {
        //how to make it not return the inputted object?

        GameObject theClosestSoFar = null;
        GameObject theSECONDClosestSoFar = null;

        List<GameObject> validObjects = null;
        List<float> allDistancesToValidObjects = new List<float>();


        foreach (GameObject thisObject in listOfObjects)
        {
            //thisObject.name != objectWeWantItClosestTo.name

            //Debug.Log(thisObject.name);
            //Debug.Log(objectWeWantItClosestTo.name);




            if (thisObject.name != objectWeWantItClosestTo.name)
            {
                if (theClosestSoFar == null)
                {
                    theClosestSoFar = thisObject;
                }
                else if (theSECONDClosestSoFar == null)
                {
                    if (theWorldScript.theRespository.isXCloserThanYToZ(thisObject, theClosestSoFar, objectWeWantItClosestTo))
                    {
                        theSECONDClosestSoFar = theClosestSoFar;
                        theClosestSoFar = thisObject;
                    }
                    else
                    {
                        theSECONDClosestSoFar = thisObject;
                    }
                }
                else if (theWorldScript.theRespository.isXCloserThanYToZ(thisObject, theSECONDClosestSoFar, objectWeWantItClosestTo))
                {

                    if (theWorldScript.theRespository.isXCloserThanYToZ(thisObject, theClosestSoFar, objectWeWantItClosestTo))
                    {
                        theSECONDClosestSoFar = theClosestSoFar;
                        theClosestSoFar = thisObject;
                    }
                    else
                    {
                        theSECONDClosestSoFar = thisObject;
                    }
                }
                else
                {
                    //Debug.Log("bbbbbbbbbbbbbbbbbbbbbbb says 2nd is closer, so do nothing??? bbbbbbbbbbbbbbbbbbbbbbbbb");

                }


            }
        }

        return theSECONDClosestSoFar;
    }


    

}
