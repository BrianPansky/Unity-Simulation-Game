using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legibstration : MonoBehaviour
{
    //making things legible [to NPCs, mostly]
    //it's a bit like tags, but more detailed?  more structured?  something.

    //has names of objects as top keys, then each object has various sub-categories, and each sub-category has its own list of items.....
    //public Dictionary<string, Dictionary<string, List<string>>> legibictionary = new Dictionary<string, Dictionary<string, List<string>>>();
    //na, that's stilly.  i should know each individual category, and hand craft a variable for it?  i think?
    //so this one is JUST for interactions.....but there can still be different categories?  i dunno, how about no.....
    //          public Dictionary<string, List<testInteraction>> interactionLegibstration = new Dictionary<string, List<testInteraction>>();
    //so this DOES look a lot like tags.  but the topic is well-defined.  is that good?  necessary?  helpful?

    //ah!  maybe it SHOULD use category names!
    //the reason i'm not just using tags is because i want it to be easy to find like a LIST of things in a certain category
    //so the tag is the category name!
    //still, i don't have full perspective on wtf i'm doing.  continue with ad-hoc methods, figure out system AFTER some of "it" is working
    //NO WAIT, i should have something more like my action/actionItem class objects?  and set up for modular condition/trigger implementation?

    public Dictionary<string, List<testInteraction>> globalInteractionLegibstration;

    worldScript theWorldScript;


    // Start is called before the first frame update
    void Start()
    {
        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;

        globalInteractionLegibstration = theWorldScript.interactionLegibstration;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void legibstrate(GameObject theObject, testInteraction theInteraction)
    {
        //globalInteractionLebistration

        //update "global" legibstrata, don't bother with a "local" one for now?
        //[basically, "local" one is the "interactionsAvailable" on an object's interaction script, which it can update itself?

        if(globalInteractionLegibstration == null)
        {
            Debug.Log("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb this is dumb that i need to do this here");

            GameObject theWorldObject = GameObject.Find("World");
            theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;

            globalInteractionLegibstration = theWorldScript.interactionLegibstration;
        }

        //but dictionaries are tricky objects, and must be checked and all that:
        if (globalInteractionLegibstration.ContainsKey(theObject.name))
        {
            //add the game object to the list of objects tagged with that tag:
            globalInteractionLegibstration[theObject.name].Add(theInteraction);
        }
        else
        {
            //sigh, need to add the key first, which means the list it unlocks as well...
            List<testInteraction> needsList = new List<testInteraction>();
            needsList.Add(theInteraction);
            globalInteractionLegibstration.Add(theObject.name, needsList);
        }
    }



}

