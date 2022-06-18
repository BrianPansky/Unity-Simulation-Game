using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myTagging1 : MonoBehaviour
{
    public taggedWith thisIsTaggedWith;


    //kind of ad-hoc, but hopefully good enough most of the time:
    public string tag1;
    public string tag2;
    public string tag3;
    public string tag4;

    List<string> tagsToAdd = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //get other script I need:
        thisIsTaggedWith = GetComponent<taggedWith>();

        //add all tags to the tag list:
        tagsToAdd.Add(tag1);
        tagsToAdd.Add(tag2);
        tagsToAdd.Add(tag3);
        tagsToAdd.Add(tag4);


        //add all tags to this GameObject:
        foreach(string thisTag in tagsToAdd)
        {
            //make sure it's not null:
            if(thisTag != null)
            {
                thisIsTaggedWith.addTag(thisTag);
            }
        }
    }
    
}
