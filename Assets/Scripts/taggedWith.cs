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



    public Dictionary<string, List<GameObject>> globalTags;

    // Start is called before the first frame update
    void Awake()
    {
        //getting the "global" tags:
        //(hopefully will allow me to modify them here
        //SHOULD be a shallow copy, NOT a deep copy)
        GameObject theWorldObject = GameObject.Find("World");
        worldScript theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
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

    public void foreignRemoveTag(string tag, GameObject removeFromThis)
    {
        //this funciton updates BOTH lists of tags
        //the "local" list, and the "global"...Dicitonary of objects

        //should I add checks here to make sure the tag is present?
        //because it will have errors if I try to remove a tag that's basicaly already removed...

        //update "local" tags
        tags.Remove(tag);

        //update "global" tags...
        //remove the game object to the list of objects tagged with that tag:
        globalTags[tag].Remove(removeFromThis);
    }

    public void foreignRemoveALLtags(GameObject thisGameObject)
    {

        //REMOVES this object from ALL tag lists
        //necessary when destroying objects, 
        //otherwise there will be "null" object references on those lists!

        //UHH, THIS IS TAG SCRIPT, NOT "SOCIAL" SCRIPT, THAT"S SOMETHING ELSE!!!
        taggedWith thisGameObjectsSocialScript = thisGameObject.GetComponent<taggedWith>();

        foreach (string tag in thisGameObjectsSocialScript.tags)
        {
            foreignRemoveTag(tag, thisGameObject);
        }
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
    
}
