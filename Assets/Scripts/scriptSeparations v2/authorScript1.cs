using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class authorScript1 : MonoBehaviour
{
    //to track who does an interaction
    //      should this use "interactionMate" isntead?


    //public collisionEnaction enacting;
    public IEnactaBool enacting;
    public interactionInfo interactionInfo;


    public enactionCreator.interType interactionType;

    /*
    public GameObject theAuthor;
    public bool isInteractionTypeLiteral = true; //literal is like "fire" and "water".  non-literal is like "standardClick" etc.
    //public enactionCreator.interType interactionType;

    public float magnitudeOfInteraction = 1f;

    */


    public static void FILLAuthorScript1(GameObject theObject, interactionInfo interInfo, IEnactaBool enactingThis) //rangedEnaction enInfo, interactionInfo interINFO, 
    {

        authorScript1 theAuthScript = theObject.GetComponent<authorScript1>();

        if (theAuthScript == null)
        {
            theAuthScript = theObject.AddComponent<authorScript1>();
            //GENAuthorScript1(theObject, interInfo, enactingThis);
            //return;
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
        theAuthScript.interactionType = interInfo.interactionType;



        //hmmmmm, i should probably stick to using more basic data types etc. wherever possible?
        //so back to:
        //theAuthorScript.theAuthor = enactionAuthor;
        //theAuthorScript.interactionType = theInteractionType;
        //finish refactoring "intSpherAtor" first, especially the variables ["feilds", """parameters""", whatever]



    }


    public static void GENAuthorScript1(GameObject theObject, interactionInfo interInfo, IEnactaBool enactingThis) //rangedEnaction enInfo, interactionInfo interINFO, 
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





































        //ah.  thi means..............the enactable ITSELF needs to input this variable?
        //theAuthScript.interactionType = enactingThis.interInfo.interactionType;
        theAuthScript.interactionType = interInfo.interactionType;







































        //hmmmmm, i should probably stick to using more basic data types etc. wherever possible?
        //so back to:
        //theAuthorScript.theAuthor = enactionAuthor;
        //theAuthorScript.interactionType = theInteractionType;
        //finish refactoring "intSpherAtor" first, especially the variables ["feilds", """parameters""", whatever]
    }

    public static void OLDGENAuthorScript1(GameObject theObject, collisionEnaction enactingThis)
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






    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("testing!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        IInteractable theInteractable = other.gameObject.GetComponent<IInteractable>();
        if (conditionsMet(other, theInteractable) == false) { return; }


        //authorScript1 theAuthorScript = other.gameObject.GetComponent<authorScript1>();

        foreach (Ieffect thisEffect in theInteractable.dictOfInteractions[interactionType])
        {
            //thisEffect.implementEffect(enacting.enactionAuthor, this.gameObject);
            thisEffect.implementEffect(other.gameObject, enacting.enactionAuthor);

            //Debug.Log("thisEffect is not implemented:  " + thisEffect);


        }



        //Debug.Log("ZZZZZZZ     END onTriggerEnter for:  " + this.gameObject.name + "     ZZZZZZZ");

    }


    private bool conditionsMet(Collider other, IInteractable theInteractable)
    {
        //Debug.Log("YYYYYYYYYYYYYYYYYYY     START onTriggerEnter for:  " + this.gameObject.name + "     YYYYYYYYYYYYYYYYYYYY");



        /*
        if (other.tag != "interactionType1")
        {
            Debug.Log("other.tag != \"interactionType1\"");
            return false;
        }
        */

        if(theInteractable == null) {  return false; }


        //authorScript1 theAuthorScript = other.gameObject.GetComponent<authorScript1>();

        if (this.enacting == null)
        {
            Debug.Log("i don't think this should ever happen:   theAuthorScript.enacting == null");

            //will probably be the error type, since enums can't be null:
            Debug.Log("theAuthorScript.interactionType:  " + this.interactionType);

            return false;
        }

        //Debug.Log("theAuthorScript.enacting.interInfo:  " + theAuthorScript.enacting.interInfo);
        //Debug.Log("theAuthorScript.enacting.enactionAuthor:  " + theAuthorScript.enacting.enactionAuthor);
        if (this.enacting.enactionAuthor == null)
        {

            Debug.Log("theAuthorScript.enacting.enactionAuthor == null");
            return false;
        }

        if(theInteractable.dictOfInteractions == null)
        {

            Debug.Log("i don't think this should ever happen:   theInteractable.dictOfInteractions == null");
            return false; }

        if (theInteractable.dictOfInteractions.ContainsKey(this.interactionType) != true)
        {
            Debug.Log("dictOfInteractions.ContainsKey(theAuthorScript.interactionType) != true, object being interacted with doesn't have this key:  " + this.interactionType);
            Debug.Log("instead, it only has the following:  ");

            foreach (var key in theInteractable.dictOfInteractions.Keys)
            {
                Debug.Log(key);
            }
            Debug.Log("..........................................................");
            return false;
        }


        //Debug.Log("great, what interaction effect is it???");

        return true;
    }





}
