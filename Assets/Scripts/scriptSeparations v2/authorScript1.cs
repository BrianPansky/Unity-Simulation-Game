using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class authorScript1 : MonoBehaviour
{
    //to track who does an interaction
    //      should this use "interactionMate" isntead?


    public IEnactaBool enacting;


    public enactionCreator.interType interactionType;

    /*
    public GameObject theAuthor;
    public bool isInteractionTypeLiteral = true; //literal is like "fire" and "water".  non-literal is like "standardClick" etc.
    //public enactionCreator.interType interactionType;

    public float magnitudeOfInteraction = 1f;

    */


    public static void FILLAuthorScript1(GameObject theObject, IEnactaBool enactingThis) //rangedEnaction enInfo, interactionInfo interINFO, 
    {

        authorScript1 theAuthScript = theObject.GetComponent<authorScript1>();

        if (theAuthScript == null)
        {
            GENAuthorScript1(theObject, enactingThis);
            return;
        }

        //does this work
        //https://discussions.unity.com/t/passing-arguments-into-a-constructor-via-addcomponent/138288/2
        //like this?  work?
        //oh, like THIS:
        //authorScript1.GENAuthorScript1(thisObject, enactThis);
        //https://forum.unity.com/threads/solved-member-cannot-be-accessed-with-an-instance-reference-qualify-it-with-a-type-name-instead.842914/
        //NOT like this:
        //authorScript1 theAuthorScript = thisObject.AddComponent<authorScript1>().GENAuthorScript1(thisObject, enactThis);
        //hopefully......

        //Debug.Log("theObject.GetInstanceID():  " + theObject.GetInstanceID());
        theObject.GetInstanceID();

        if (enactingThis == null)
        {
            //Debug.Log("enactingThis == null");
        }
        else
        {
            //Debug.Log("enactingThis is ok");
        }

        //authorScript1 theAuthScript = theObject.AddComponent<authorScript1>();
        theAuthScript.enacting = enactingThis;
        theAuthScript.interactionType = enactingThis.interInfo.interactionType;



        //hmmmmm, i should probably stick to using more basic data types etc. wherever possible?
        //so back to:
        //theAuthorScript.theAuthor = enactionAuthor;
        //theAuthorScript.interactionType = theInteractionType;
        //finish refactoring "intSpherAtor" first, especially the variables ["feilds", """parameters""", whatever]
    }


    public static void GENAuthorScript1(GameObject theObject, IEnactaBool enactingThis) //rangedEnaction enInfo, interactionInfo interINFO, 
    {

        authorScript1 theAuthScript = theObject.GetComponent<authorScript1>();
        if (theAuthScript != null)
        {
            Debug.Log("this object already has an authorScript1, use FILLAuthorScript1 instead of GENAuthorScript1");
        }

        //does this work
        //https://discussions.unity.com/t/passing-arguments-into-a-constructor-via-addcomponent/138288/2
        //like this?  work?
        //oh, like THIS:
        //authorScript1.GENAuthorScript1(thisObject, enactThis);
        //https://forum.unity.com/threads/solved-member-cannot-be-accessed-with-an-instance-reference-qualify-it-with-a-type-name-instead.842914/
        //NOT like this:
        //authorScript1 theAuthorScript = thisObject.AddComponent<authorScript1>().GENAuthorScript1(thisObject, enactThis);
        //hopefully......

        //Debug.Log("theObject.GetInstanceID():  "+ theObject.GetInstanceID());
        theObject.GetInstanceID();

        if (enactingThis == null)
        {
            //Debug.Log("enactingThis == null");
        }
        else
        {
            //Debug.Log("enactingThis is ok");
        }

        theAuthScript = theObject.AddComponent<authorScript1>();
        theAuthScript.enacting = enactingThis;
        theAuthScript.interactionType = enactingThis.interInfo.interactionType;



        //hmmmmm, i should probably stick to using more basic data types etc. wherever possible?
        //so back to:
        //theAuthorScript.theAuthor = enactionAuthor;
        //theAuthorScript.interactionType = theInteractionType;
        //finish refactoring "intSpherAtor" first, especially the variables ["feilds", """parameters""", whatever]
    }

    public static void OLDGENAuthorScript1(GameObject theObject, IEnactaBool enactingThis)
    {
        /*

        //does this work
        //https://discussions.unity.com/t/passing-arguments-into-a-constructor-via-addcomponent/138288/2
        //like this?  work?
        //oh, like THIS:
        //authorScript1.GENAuthorScript1(thisObject, enactThis);
        //https://forum.unity.com/threads/solved-member-cannot-be-accessed-with-an-instance-reference-qualify-it-with-a-type-name-instead.842914/
        //NOT like this:
        //authorScript1 theAuthorScript = thisObject.AddComponent<authorScript1>().GENAuthorScript1(thisObject, enactThis);
        //hopefully......


        authorScript1 theAuthScript = theObject.AddComponent<authorScript1>();
        theAuthScript.enacting = enactingThis;
        theAuthScript.interactionType = enactingThis.interactionType;



        //hmmmmm, i should probably stick to using more basic data types etc. wherever possible?
        //so back to:
        //theAuthorScript.theAuthor = enactionAuthor;
        //theAuthorScript.interactionType = theInteractionType;
        //finish refactoring "intSpherAtor" first, especially the variables ["feilds", """parameters""", whatever]



        */
    }


    // Start is called before the first frame update
    void Start()
    {
        //redunfant because i do this in "GENAuthorScript1"?
        //interactionType = enacting.theInfo.interactionType;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("theObject.GetInstanceID():  " + this.gameObject.GetInstanceID());
        if (enacting == null)
        {
            //Debug.Log("enacting == null");
        }
        else
        {
            //Debug.Log("enacting is ok");
        }

        //Debug.Log("the author is:  "  + theAuthor.name);
        //Debug.Log("the author is:  " + theAuthor);
        //Debug.Log("authorScript is ON this object:  " + this.gameObject.name);
        //Debug.Log("33333333333333333333333333333333333333333 the interactionType is:  " + interactionType);

        Vector3 thisBit = (this.gameObject.transform.position + this.gameObject.transform.up);
        Color whatColor = Color.white;

        //change if it's a bullet.............tankShot:
        //Debug.Log("enacting:  " + enacting);
        //Debug.Log("enacting.interactionType:  " + enacting.interactionType);
        //Debug.Log("enacting:  " + enacting);
        //Debug.Log("enactionCreator.interType.tankShot:  " + enactionCreator.interType.tankShot);
        if (enacting != null && interactionType == enactionCreator.interType.tankShot)
        {
            thisBit = (this.gameObject.transform.position + this.gameObject.transform.forward);
            whatColor = Color.red;
        }
        else if (enacting != null && interactionType == enactionCreator.interType.shoot1)
        {
            thisBit = (this.gameObject.transform.position - this.gameObject.transform.up);
            whatColor = Color.blue;
        }

        Debug.DrawLine(this.gameObject.transform.position, thisBit, whatColor, 0.1f);
    }
}
