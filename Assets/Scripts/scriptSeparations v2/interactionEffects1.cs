using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class interactionEffects1 : MonoBehaviour
{
    //so, interactions, two things:
    //impactee selection / conditions (proximity, area of effect, line of sight, linked objects, etc.)
    //effect
    //impactee selection is basically targeting, but with an area of effect, that would seem like the wrong word






    public bool inputBoolSignal;



    //public Dictionary<>
    public List<testInteraction> interactionsAvailable = new List<testInteraction>();
    public List<testInteraction> privateInteractionsAvailable = new List<testInteraction>();

    //is this all interactions?  only public ones?  only private ones?
    public Dictionary<string, testInteraction> interactionDictionary = new Dictionary<string, testInteraction>();






    //preselected host, slot sequence

    public GameObject preselectedHost;

    public List<GameObject> slotObjects = new List<GameObject>();
    public Dictionary<int, bool> slotAvailability = new Dictionary<int, bool>();
    public List<List<Vector3>> slotLocations = new List<List<Vector3>>();


    public string targetToSelect;
    public string targetPriority;
    public string typeOfInteractionOutcome;

    worldScript theWorldScript;

    // Start is called before the first frame update
    void Start()
    {

        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        //theWorldScript.interactionLegibstration.Add(this.gameObject.name);

        theWorldScript.theTagScript.foreignAddTag("interactionEffects1", this.gameObject);

        //just a GENERAL tag:
        if(interactionsAvailable.Count > 0)
        {
            theWorldScript.theTagScript.foreignAddTag("interactable", this.gameObject);
        }
        


        //just...wtf is going on here?
        //Debug.Log("???????????????????????????      start looking at keys in dictionary     ??????????????????????????????");
        List<string> myKeyList = new List<string>(this.theWorldScript.interactionLegibstration.Keys);
        //foreach(string key in myKeyList)
        {
            //Debug.Log("HELLLOOOOOOOOOOOOOOOOOOOOO?????????????????????????????????????????????????????????");
            //Debug.Log(key);
        }
        //Debug.Log("???????????????????????????      END looking at keys in dictionary, start looking at contents for THIS object's key     ??????????????????????????????");

        

        //Debug.Log(interactionsAvailable);
        //Debug.Log(theWorldScript.interactionLegibstration[this.gameObject.name]);
        //theWorldScript.interactionLegibstration[this.gameObject.name] = interactionsAvailable;
        //Debug.Log(theWorldScript.interactionLegibstration[this.gameObject.name]);
        //Debug.Log(interactionsAvailable);


    }

    public testInteraction interactionTest2()
    {
        //          NOW I NEED TO FILL THEIR NAMES AND ALL THAT
        //          THEN ADD TO LEGIBSTRATA TO BE FOUND
        return generateInteraction("testName2");
    }

    public testInteraction interactionTest1()
    {
        return generateInteraction("testName1");
    }


    public testInteraction generateInteractionFULL(string theName, List<interactionAtom> theAtoms, int zeroIfPrivate = 1)
    {
        testInteraction oneToGenerate = new testInteraction();
        //public List<interactionAtom> atoms = new List<interactionAtom>();



        oneToGenerate.name = theName;
        oneToGenerate.atoms = theAtoms;

        if (theWorldScript == null)
        {
            GameObject theWorldObject = GameObject.Find("World");
            theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        }

        //when "generating" an interaction, should ALSO add it to "legibstrata" and THIS script's "interactionsAvailable"???

        //Debug.Log("should NOT just be world object:  " + this.gameObject);

        //theWorldScript.theRespository.theLegibstrata.legibstrate(this.gameObject, oneToGenerate);
        
        if(zeroIfPrivate == 0) 
        {
            privateInteractionsAvailable.Add(oneToGenerate);
        }
        else
        {
            interactionsAvailable.Add(oneToGenerate);
        }
        

        return oneToGenerate;
    }

    public testInteraction generateInteraction(string theName)
    {
        testInteraction oneToGenerate = new testInteraction();

        


        oneToGenerate.name = theName;
        oneToGenerate.atoms.Add(generateInteractionAtom(theName));

        //Debug.Log("------------------------     start      ----------------------------");
        
        if (theWorldScript == null)
        {
            //Debug.Log("44444444444444444444444444444444444444444");
            GameObject theWorldObject = GameObject.Find("World");
            theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        }


        //when "generating" an interaction, should ALSO add it to "legibstrata" and THIS script's "interactionsAvailable"???
        
        Debug.Log("should NOT just be world object:  " + this.gameObject);
        //Debug.Log(theName);
        //Debug.Log("-------------------------    end    ---------------------------");
        //this.gameObject.GetComponent<interactionEffects1>().interactionsAvailable.Add(oneToGenerate);
        //theWorldScript.theRespository.theLegibstrata.legibstrate(this.gameObject, oneToGenerate);
        interactionsAvailable.Add(oneToGenerate);

        return oneToGenerate;
    }
    public interactionAtom generateInteractionAtom(string theName)
    {
        interactionAtom oneToGenerate = new interactionAtom();

        oneToGenerate.subAtoms.Add(generateInteractionSUBAtom(theName));

        return oneToGenerate;
    }

    public interactionSubAtom generateInteractionSUBAtom(string theName)
    {
        interactionSubAtom oneToGenerate = new interactionSubAtom();

        oneToGenerate.effects.Add(theName);

        return oneToGenerate;
    }


    // Update is called once per frame
    void Update()
    {
        if (inputBoolSignal == true)
        {
            inputBoolSignal = false;
            this.transform.Rotate(30.0f, 0.0f, 0.0f, Space.World);
        }
    }



    public void initilaizeSlotAvailability()
    {
        //can't be done in start, beucase the slot list won't have been filled yet i don't think???  i could try???

        generateInteraction("slotAvailabilityInitializationThing, presumably should change to the way this object interacts WITH the slots that are either available or unavailalbe or something.  stuff IN the slots.");

        //foreach (var slot in slotObjects)
        {
            //Debug.Log(slot);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("YYYYYY     START onTriggerEnter for:  " + this.gameObject.name + "     YYYYYY");


        //Debug.Log(other.gameObject.name);
        if (other.tag == "interactionType1")
        {

            //      or should these "author" and "target" be reversed?????
            interactionMate thisMate = new interactionMate();

            authorScript1 theAuthorScript = other.gameObject.GetComponent<authorScript1>();
            thisMate.interactionAuthor = theAuthorScript.theAuthor;
            thisMate.enactThisInteraction = theAuthorScript.enactThisInteraction;
            //thisMate.target1 = other.gameObject;
            thisMate.target1 = this.gameObject;

            Debug.Log("my interaction type:  " + theAuthorScript.interactionType);

            //doInteraction1(other.GetComponent<authorScript1>().theAuthor, thisMate);

            //thisMate.enactThisInteraction = interactionsAvailable[0];

            //sigh
            Debug.Log(interactionDictionary.Keys.Count());
            foreach (string aaaaa in interactionDictionary.Keys)
            {
                //Debug.Log("the dictionary key:  " + aaaaa);
                if (aaaaa == "walkSomewhere")
                {
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("the dictionary key:  " + aaaaa);
                }
                if (aaaaa == theAuthorScript.interactionType)
                {
                    Debug.Log("yes good, the dictionary key is the same as the interaction type.  now DO it");
                    Debug.Log("so do this:  " + interactionDictionary[theAuthorScript.interactionType].name);
                    //nteractionDictionary[theAuthorScript.interactionType].

                    Debug.Log("on this object:  " + this.gameObject.name);
                    Debug.Log("targeted by this author:  " + theAuthorScript.theAuthor.name);
                    interactionDictionary[theAuthorScript.interactionType].doInteraction(thisMate);
                }
                if(aaaaa == thisMate.enactThisInteraction.name)
                {
                    //thisMate.printMate();
                    //thisMate.doThisInteractionIfPrereqsMet(thisMate);
                }

                Debug.Log(interactionDictionary.Keys.Count());
            }


        }
        if (other.tag == "inputOutput1")
        {
            //Destroy(this.gameObject);


        }



        Debug.Log("ZZZZZZZ     END onTriggerEnter for:  " + this.gameObject.name + "     ZZZZZZZ");

    }


    void doInteraction1(GameObject theObjectInitiatingTheInteraction, interactionMate thisMate)
    {
        //for now, just do the FIRST interaction available:

        //      FOR MATE theObjectInitiatingTheInteraction, this.gameObject
        //modularInteractions1(interactionsAvailable[0], thisMate);
        
        //foreach (testInteraction thisAvailableInteraction in interactionsAvailable)
        {
            //if (thisAvailableInteraction.name == "pickUpItem")
            {
                //pickUp1();

            }
        }
    }




    public void clonify(interactionEffects1 newInputScript)
    {
        //"deep copy" all important variables from this current script into this new input script.

        //public List<testInteraction> interactionsAvailable = new List<testInteraction>();

        List<testInteraction> cloningOldInteractionsAvailable = new List<testInteraction>();
        foreach (testInteraction thisTestInteraction in interactionsAvailable)
        {
            cloningOldInteractionsAvailable.Add(thisTestInteraction.deepCopy());
        }

        newInputScript.interactionsAvailable = cloningOldInteractionsAvailable;
    }



    public bool doesObjectXHaveThisEffect(string thisEffect, GameObject objectX)
    {
        //i need something to check whether something has a specific EFFECT

        interactionEffects1 interactionScript = objectX.GetComponent<interactionEffects1>();

        return interactionScript.doesThisScriptHaveInteractionsAvailableWithThisEffect(thisEffect);
    }

    public bool doesThisScriptHaveInteractionsAvailableWithThisEffect(string thisEffect)
    {
        //i need something to check whether something has a specific EFFECT

        foreach (var theInteraction in interactionsAvailable)
        {
            if (theInteraction.doesThisInteractionContainEffectX(thisEffect) == true)
            {
                return true;
            }
        }

        return false;
    }


    public testInteraction returnFirstInteractionOnObjectWithThisEffect(string thisEffect, GameObject thisObject)
    {
        //i need something to check whether something has a specific EFFECT

        interactionEffects1 interactionScript = thisObject.GetComponent<interactionEffects1>();

        return returnFirstIntearctionOnListWithEffectX(thisEffect, interactionScript.interactionsAvailable);
    }

    public testInteraction returnFirstPrivateInteractionOnObjectWithThisEffect(string thisEffect, GameObject thisObject)
    {
        interactionEffects1 interactionScript = thisObject.GetComponent<interactionEffects1>();

        return returnFirstIntearctionOnListWithEffectX(thisEffect, interactionScript.privateInteractionsAvailable);
    }

    public testInteraction returnFirstIntearctionOnListWithEffectX(string thisEffect, List<testInteraction> listOfInteracitons)
    {
        foreach (var theInteraction in listOfInteracitons)
        {
            if (theInteraction.doesThisInteractionContainEffectX(thisEffect))
            {
                return theInteraction;
            }
        }

        return null;
    }
}

public class interactionSubAtom
{
    //modular parts i can build "Atoms" out of
    //these are not atoms, becausue atoms in the game have to make sense
    //which requires hand-crafting
    //so why do sub-atoms exist?
    //to make the construction of atoms easier and modular

    //presumably THIS is the level where ALL prereqs and effects exist

    public List<string> prereqs = new List<string>();
    public List<string> effects = new List<string>();



    public string name;

    //public void doSubAtom(GameObject theObjectInitiatingTheInteraction, GameObject theObjectBeingInteractedWith)




    public void doSubAtom(interactionMate theInteractionMate)
    {
        Debug.Log(effects.Count());
        foreach (var thisEffect in effects)
        {
            Debug.Log(effects.Count());
            doThisSubAtomEffect(thisEffect, theInteractionMate);
        }
    }

    public void doThisSubAtomEffect(string theSubAtomEffect, interactionMate theInteractionMate)
    {
        bool aTestBool = false;

        //respository of test printouts:
        if (true == false)
        {
            if (theInteractionMate == null)
            {
                Debug.Log("''theInteractionMate'' is NULL in enaction phase");
                aTestBool = true;
            }
            if (theInteractionMate.interactionAuthor == null)
            {
                Debug.Log("''theInteractionMate.interactionAuthor'' is NULL in enaction phase");
                aTestBool = true;
            }
            if (theInteractionMate.interactionAuthor.name == null)
            {
                Debug.Log("''theInteractionMateinteractionAuthor.name'' is NULL in enaction phase");
                aTestBool = true;
            }
            if (theInteractionMate.target1 == null)
            {
                Debug.Log("''theInteractionMate.target1'' is NULL in enaction phase");
                aTestBool = true;
            }
        }





        if (theSubAtomEffect == "testName1")
        {
            //Debug.Log("the testName11111 effect has occured");

            //objectToChangeColorOf.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            //so, i need TARGET object here.
            //or, should i set it up so that this function is always called ON the target?  how is this working again?
            //this.gameObject.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            //right now, this script is attached to the "world object" which has no render component and thus has an error.
            //where is this being called from or caused?  how is it getting here from the AI NPC2?
            //it's just calling it like this:
            //theWorldScript.theRespository.theInteractionEffects1.modularInteractions1(availableIntertactions[0]);
            //so, for now i can change it to plug in the target object?


            theInteractionMate.target1.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
        else if(theSubAtomEffect == "standardInteraction1")
        {
            RaycastHit myHit;
            //Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            body1 authorBody = theInteractionMate.interactionAuthor.GetComponent<body1>();
            Ray myRay = authorBody.lookingRay;
            //Ray myRay = theInteractionMate.interactionAuthor.GetComponent<body1>().lookingRay;

            if (Physics.Raycast(myRay, out myHit, 7.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if (myHit.transform != null)
                {
                    //Debug.Log(myHit.transform.gameObject);

                    //so, the following line basically magically RETURNS the interactionMate.  like, updates it so it can be used AFTER the current function call.  is that wise?  no.
                    theInteractionMate.clickedOn = myHit.transform.gameObject;

                    GameObject anInteractionSphere = authorBody.theWorldScript.theRespository.interactionSphere;

                    GameObject thisObject = authorBody.theWorldScript.theRespository.createPrefabAtPointAndRETURN(anInteractionSphere, myHit.point);

                    //      should this use "interactionMate" isntead?
                    authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
                    theAuthorScript.theAuthor = theInteractionMate.interactionAuthor;
                    theAuthorScript.enactThisInteraction = theInteractionMate.enactThisInteraction;
                    theAuthorScript.interactionType = "standardInteraction1";

                }
            }
        }
        else if (theSubAtomEffect == "justGrabIt")
        {
            theInteractionMate.target1.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f);

            theInteractionMate.interactionAuthor.GetComponent<inventory1>().testInventory1.Add("testKey1");
        }
        else if (theSubAtomEffect == "justActivateLock")
        {
            if (theInteractionMate.interactionAuthor.GetComponent<inventory1>().testInventory1.Contains("testKey1") == true)
            {
                theInteractionMate.target1.GetComponent<Renderer>().material.color = new Color(1f, 0f, 1f);
            }
        }
        else if (theSubAtomEffect == "walkSomewhere")
        {
            Vector3 targetVector = theInteractionMate.target1.GetComponent<Transform>().position;
            theInteractionMate.interactionAuthor.GetComponent<AIHub2>().thisNavMeshAgent.SetDestination(targetVector);
        }
        else if (theSubAtomEffect == "testName2")
        {
            //Debug.Log("the testName22222 effect has occured");
            theInteractionMate.target1.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
        }
        else if (theSubAtomEffect == "pickUpItem")
        {
            //Debug.Log("the pickUpItem effect has occured");
            theInteractionMate.target1.GetComponent<Renderer>().material.color = new Color(0, 111, 255);
        }
        else if (theSubAtomEffect == "subAtomslotAvailabilityInitializationThing")
        {
            //Debug.Log("the subAtomslotAvailabilityInitializationThing effect has occured");
            theInteractionMate.target1.GetComponent<Renderer>().material.color = new Color(255, 111, 55);
        }
        else if (theSubAtomEffect == "grabTestKeySubAtom")
        {

            if (theInteractionMate == null)
            {
                Debug.Log("''theInteractionMate'' is NULL in enaction phase");
                aTestBool = true;
            }
            if (theInteractionMate.interactionAuthor == null)
            {
                Debug.Log("''theInteractionMate.interactionAuthor'' is NULL in enaction phase");
                aTestBool = true;
            }
            if (theInteractionMate.target1 == null)
            {
                Debug.Log("''theInteractionMate.target1'' is NULL in enaction phase");
                aTestBool = true;
            }



            if (aTestBool == false)
            {
                //Debug.Log("the grabTestKeySubAtomEffect1 effect has occured");
                //theObjectBeingInteractedWith.GetComponent<Renderer>().material.color = new Color(155, 111, 55);
                theInteractionMate.target1.GetComponent<Renderer>().material.color = new Color(0.4f, 0.8f, 0.5f);

                //testInventory1
                //inventory1
                //need the object DOING the interaction.  or the NPC doing the interaction.
                //Debug.Log(theObjectInitiatingTheInteraction.name);
                theInteractionMate.interactionAuthor.GetComponent<inventory1>().testInventory1.Add("testKey1");
            }
            
        }
        else if (theSubAtomEffect == "grabTestLOCKSubAtomEffect1")
        {
            //Debug.Log("the grabTestLOCKSubAtomEffect1 effect has occured");
            //theObjectBeingInteractedWith.GetComponent<Renderer>().material.color = new Color(155, 111, 55);


            if (theInteractionMate == null)
            {
                Debug.Log("''theInteractionMate'' is NULL in enaction phase");
                aTestBool = true;
            }
            if (theInteractionMate.target1 == null)
            {
                Debug.Log("''theInteractionMate.target1'' is NULL in enaction phase");
                aTestBool = true;
            }



            if (aTestBool == false)
            {
                theInteractionMate.target1.GetComponent<Renderer>().material.color = new Color(0.7f, 0.2f, 0.9f);
            }
            
        }
        else if (theSubAtomEffect == "proximity0")
        {


            if (theInteractionMate == null)
            {
                Debug.Log("''theInteractionMate'' is NULL in enaction phase");
                aTestBool = true;
            }
            if (theInteractionMate.interactionAuthor == null)
            {
                Debug.Log("''theInteractionMate.interactionAuthor'' is NULL in enaction phase");
                aTestBool = true;
            }



            if (aTestBool == false)
            {
                //Debug.Log("should be going from " + theInteractionMate.interactionAuthor.name + " to:  " + theInteractionMate.target1.name);

                Vector3 targetVector = theInteractionMate.target1.GetComponent<Transform>().position;
                theInteractionMate.interactionAuthor.GetComponent<AIHub2>().thisNavMeshAgent.SetDestination(targetVector);
            }
            
            
        }

        else
        {
            Debug.Log(theInteractionMate.target1.name + " is trying to enact this subAtom effect" + theSubAtomEffect + "   but there is no ''if'' statement to catch it in modular interactions' ''doSubAtomEffects''");
            theInteractionMate.target1.GetComponent<Renderer>().material.color = new Color(55, 111, 222);
        }
    }

    public List<string> listUnfilledPrereqs(interactionMate thisMate)
    {
        List<string> unfilledPrereqs = new List<string>();

        foreach (var thisPrereq in prereqs)
        {
            if (checkOnePrereq(thisPrereq, thisMate) == false)
            {
                unfilledPrereqs.Add(thisPrereq);
            }

        }

        return unfilledPrereqs;
    }


    public bool checkOnePrereq(string thisPrereq, interactionMate theInteractionMate)
    {
        //returns TRUE if prereqs are met

        //grabTestLOCKSubAtomPrereq1
        if (thisPrereq == "grabTestLOCKSubAtomPrereq1")
        {
            //Debug.Log("the testName11111 effect has occured");
            if (theInteractionMate.interactionAuthor.GetComponent<inventory1>().testInventory1.Contains("testKey1") != true)
            {
                return false;
            }

        }
        else if (thisPrereq == "proximity0")
        {
            //needs SMALL distance between theObjectInitiatingTheInteraction and theObjectBeingInteractedWith


            if (areThereErrorsWithInteractionMate(theInteractionMate) == false)
            {
                //Debug.Log(theInteractionMate.thisIsTheDamnLock);
                float distance = Vector3.Distance(theInteractionMate.interactionAuthor.transform.position, theInteractionMate.target1.transform.position);
                //some actions need a bigger or smaller range.  Ad-hoc adding that here for now...
                //default is basically zero range:
                float theRange = 3.2f;


                if (distance > theRange)
                {
                    return false;
                }


            }



        }
        else
        {
            Debug.Log("no ''if'' statement catches this prereq to see if it's met:  " + thisPrereq);
        }


        return true;
    }

    public bool checkAllPrereqs(interactionMate theInteractionMate)
    {
        //returns TRUE if prereqs are met

        foreach(string thisPrereq in prereqs)
        {
            if(checkOnePrereq(thisPrereq, theInteractionMate) == false)
            {
                return false;
            }
        }


        return true;
    }


    public bool areThereErrorsWithInteractionMate(interactionMate theInteractionMate)
    {
        //this function is here [or at least STARTS here] instead of inside the interactionMate class definition
        //because one error is it could be null.  at which point i don't think it can even do those functions.

        //return false if there are no errors

        if (theInteractionMate == null)
        {
            Debug.Log("theInteractionMate in my enaction phase is NULL");
            return true;
        }

        return theInteractionMate.anyErrors();
    }

}

public class interactionAtom
{
    //this is the smallest unit of interaction in the game that makes sense
    //thus it requires some hand crafting

    public string name;
    public List<interactionSubAtom> subAtoms = new List<interactionSubAtom>();

    public interactionAtom deepCopy()
    {
        interactionAtom newCopy = new interactionAtom();

        newCopy.name = name;
        
        //does this need deeper copying?  maybe EVENTUALLY
        //List<interactionSubAtom> deepCopiedSubAtoms = new List<interactionSubAtom>();
        //foreach (var x in subAtoms)
        {
            //does this need deeper copying?  maybe EVENTUALLY
            //deepCopiedSubAtoms.Add(x.deepCopy);
        }
        //newCopy.subAtoms = deepCopiedSubAtoms;
        newCopy.subAtoms = subAtoms;

        return newCopy;
    }

    public void enactAtom(interactionMate theInteractionMate)
    {
        foreach(interactionSubAtom subAtom in subAtoms)
        {
            subAtom.doSubAtom(theInteractionMate);
        }
    }
}

public class testInteraction
{
    //do i need this level of abstraction above atoms for now?  i'm not sure
    //i mean, i GUESS.  that's the only reason modularizing interactions into atoms could serve, right?
    //well, no, modularizing them also helps with ENACTION, even if there were no larger abstraction?  i dunno

    public string name;
    public List<interactionAtom> atoms = new List<interactionAtom>();

    public void doInteraction(interactionMate theInteractionMate)
    {
        foreach(interactionAtom atom in atoms)
        {
            atom.enactAtom(theInteractionMate);
        }
    }





    public List<interactionSubAtom> allSubAtoms()
    {
        List<interactionSubAtom> listOfSubAtoms = new List<interactionSubAtom>();

        //first get all the sub-atoms, simple so i don't have to look at this nested code:
        foreach (var thisAtom in atoms)
        {
            foreach (var thisSubAtom in thisAtom.subAtoms)
            {
                listOfSubAtoms.Add(thisSubAtom);
            }
        }

        return listOfSubAtoms;
    }
    public testInteraction deepCopy()
    {
        testInteraction newCopy = new testInteraction();

        newCopy.name = name;

        List<interactionAtom> deepCopiedAtoms = new List<interactionAtom>();
        foreach(var thisAtom in atoms)
        {
            deepCopiedAtoms.Add(thisAtom.deepCopy());
        }
        newCopy.atoms = deepCopiedAtoms;

        return newCopy;
    }

    public bool doesThisInteractionContainEffectX(string effectX)
    {
        foreach(string thisEffect in allEffects())
        {
            //Debug.Log("compare the desired effectX:  " + effectX + ", to the effect this other object has:  " + thisEffect);

            if (thisEffect == effectX)
            {
                return true;
            }
        }

        return false;
    }

    public List<string> allEffects()
    {
        List<string> theList = new List<string>();

        foreach (var thisAtom in atoms)
        {
            foreach (var thisSubAtom in thisAtom.subAtoms)
            {
                foreach (var thisSubAtomEffect in thisSubAtom.effects)
                {
                    theList.Add(thisSubAtomEffect);
                }
            }
        }

        return theList;
    }

    public List<string> returnAllUnfilledPrereqs(interactionMate thisMate)
    {
        List<string> unfilledPrereqs = new List<string>();
        List<interactionSubAtom> listOfSubAtoms = allSubAtoms();

        foreach (interactionSubAtom thisSubAtom in listOfSubAtoms)
        {
            unfilledPrereqs.AddRange(thisSubAtom.listUnfilledPrereqs(thisMate));
        }

        return unfilledPrereqs;
    }

    public bool checkPrereqs(interactionMate interactionMate)
    {
        //return TRUE if prereqs are met

        foreach(interactionSubAtom thisSubAtom in allSubAtoms())
        {
            if (thisSubAtom.checkAllPrereqs(interactionMate) == false)
            {
                return false;
            }
        }

        return true;
    }

    public void printUnfilledPrereqs(interactionMate thisMate)
    {
        Debug.Log("here are unfilled prereqs:  ");

        foreach(string thisUnfilledPrereq in returnAllUnfilledPrereqs(thisMate))
        {
            Debug.Log(thisUnfilledPrereq);
        }

        Debug.Log("-------- END of unfilled prereqs:  ");

    }
}

public class interactionMate
{
    public GameObject interactionAuthor;

    public GameObject target1;
    public GameObject target2;
    public GameObject target3;

    public GameObject clickedOn;

    public testInteraction enactThisInteraction;



    public void doThisInteraction()
    {
        //      are we assuming the prereqs have been MET here?
        //get all sub atoms
        //do all sub atoms


        List<interactionSubAtom> allSubatoms = enactThisInteraction.allSubAtoms();

        foreach (var thisSubAtom in allSubatoms)
        {
            thisSubAtom.doSubAtom(this);
        }
    }


    public bool checkInteractionMatePrereqs()
    {
        return enactThisInteraction.checkPrereqs(this);
    }



    


    public bool areThereErrorsWithInteractionMate(interactionMate theInteractionMate)
    {
        //this function is here [or at least STARTS here] instead of inside the interactionMate class definition
        //because one error is it could be null.  at which point i don't think it can even do those functions.

        //return false if there are no errors

        if (theInteractionMate == null)
        {
            Debug.Log("theInteractionMate in my enaction phase is NULL");
            return true;
        }

        return theInteractionMate.anyErrors();
    }



    public void printMate()
    {
        Debug.Log("MMMMMMMMMMMMMMMMMMMMMMMMMMMM    printing interaction mate    MMMMMMMMMMMMMMMMMMMMMMMMMMMM");
        Debug.Log("interactionAuthor:  " + interactionAuthor);
        Debug.Log("target1:  " + target1);
        Debug.Log("target2:  " + target2);
        Debug.Log("target3:  " + target3);
        Debug.Log("clickedOn:  " + clickedOn);
        Debug.Log("enactThisInteraction:  " + enactThisInteraction.name);
        Debug.Log("WWWWWWWWWWWWWWWWW    END printing interaction mate    WWWWWWWWWWWWWWWWW");
    }

    public bool anyErrors()
    {
        //returns TRUE if there's an ERROR


        //Debug.Log(theInteractionMate);
        //Debug.Log(theInteractionMate.interactionAuthor);
        //Debug.Log(theInteractionMate.interactionAuthor.transform.position);
        //Debug.Log(theInteractionMate.target1);
        //Debug.Log(theInteractionMate.target1.transform.position);


        if (interactionAuthor == null)
        {
            Debug.Log("''theInteractionMate.interactionAuthor'' in my enaction phase is NULL");
            return true;
        }
        else if (target1 == null)
        {
            Debug.Log("''theInteractionMate.target1'' in my enaction phase is NULL");
            return true;
        }

        return false;
    }

    public void doThisInteractionIfPrereqsMet(interactionMate thisMate)
    {
        if (enactThisInteraction.checkPrereqs(thisMate) == false)
        {
            enactThisInteraction.printUnfilledPrereqs(thisMate);
            return;
        }

        enactThisInteraction.doInteraction(thisMate);
    }

}
