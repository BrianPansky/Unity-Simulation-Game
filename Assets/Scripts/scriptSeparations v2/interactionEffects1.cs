using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    [SerializeField] public List<testInteraction> interactionsAvailable = new List<testInteraction>();
    public int testThisFuckingInt = new int();
    public List<int> testThisFuckingIntList = new List<int>();
    [SerializeField] public fuckingTestClass fuckingTestClassObject = new fuckingTestClass();
    [SerializeField] public List<fuckingTestClass> fuckingTestClassObjectList = new List<fuckingTestClass>();
    [SerializeField] public List<testInteraction> fuckThis = new List<testInteraction>();









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
        theWorldScript.theTagScript.foreignAddTag("interactable", this.gameObject);

        //if(interactionsAvailable.Count == 0)
        {
            //is this a good idea?  this is for handling clones.  but by definition....ya will never work because name of clone is different!
            //theWorldScript.interactionLegibstration[this.gameObject.name] = interactionsAvailable;
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
        theWorldScript.interactionLegibstration[this.gameObject.name] = interactionsAvailable;
        //Debug.Log(theWorldScript.interactionLegibstration[this.gameObject.name]);
        //Debug.Log(interactionsAvailable);


        List<testInteraction> myInteractionList = theWorldScript.interactionLegibstration[this.gameObject.name];
        //Debug.Log(myInteractionList);
        foreach (testInteraction x in myInteractionList)
        {
            //Debug.Log("HELLLOOOOOOOOOOOOOOOOOOOOO?????????????????????????????????????????????????????????");
            Debug.Log("interactions on this object: " + this.gameObject.name);
            Debug.Log(x);
        }
        //Debug.Log("???????????????????????????      END looking at contents for this object's key     ??????????????????????????????");

        //foreach(var x in interactionsAvailable)
        {

        }
        //      [switch these over to be interaction class objects, not strings]
        //List<string> availableIntertactions = new List<string>();
        //availableIntertactions.Add("interactiontest1");
        //availableIntertactions.Add("interactionTest2");
        //theWorldScript.theRespository.theLegibstrata.interactionLebistration[this.gameObject.name] = availableIntertactions;
        //List<testInteraction> availableIntertactions = new List<testInteraction>();
        //availableIntertactions.Add(interactionTest1());
        //availableIntertactions.Add(interactionTest2());
        //theWorldScript.theRespository.theLegibstrata.interactionLebistration[this.gameObject.name] = availableIntertactions;


        //foreach (var slot in slotObjects)
        {
            //Debug.Log(slot);
        }

        //if(preselectedHost != null)
        {
            //preselectedHost.GetComponent<interactionEffects1>().slotObjects.Add(this.gameObject);
        }


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


    public testInteraction generateInteractionFULL(string theName, List<interactionAtom> theAtoms)
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

        theWorldScript.theRespository.theLegibstrata.legibstrate(this.gameObject, oneToGenerate);
        interactionsAvailable.Add(oneToGenerate);

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
        theWorldScript.theRespository.theLegibstrata.legibstrate(this.gameObject, oneToGenerate);
        interactionsAvailable.Add(oneToGenerate);
        fuckThis.Add(oneToGenerate);

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

        //Debug.Log(other.gameObject.name);
        if (other.tag == "interactionType1")
        {

            //      or should these "author" and "target" be reversed?????
            interactionMate thisMate = new interactionMate();
            thisMate.interactionAuthor = other.gameObject.GetComponent<authorScript1>().theAuthor;
            //thisMate.target1 = other.gameObject;
            thisMate.target1 = this.gameObject;

            doInteraction1(other.GetComponent<authorScript1>().theAuthor, thisMate);


        }
        if (other.tag == "inputOutput1")
        {
            //Destroy(this.gameObject);


        }

    }


    void doInteraction1(GameObject theObjectInitiatingTheInteraction, interactionMate thisMate)
    {
        //for now, just do the FIRST interaction available:

        //      FOR MATE theObjectInitiatingTheInteraction, this.gameObject
        modularInteractions1(interactionsAvailable[0], thisMate);

        //foreach (testInteraction thisAvailableInteraction in interactionsAvailable)
        {
            //if (thisAvailableInteraction.name == "pickUpItem")
            {
                //pickUp1();
                
            }
        }
    }

    public bool modularPrereqCheckBool(testInteraction theInteraction, interactionMate theInteractionMate)
    {
        //Debug.Log(theInteraction);
        //Debug.Log(theInteraction.name);
        //Debug.Log(theInteraction.atoms);
        foreach (var thisAtom in theInteraction.atoms)
        {
            //Debug.Log("1?");
            foreach (var thisSubAtom in thisAtom.subAtoms)
            {
                //Debug.Log("2?");
                if (thisSubAtom.prereqs.Count == 0)
                {
                    //Debug.Log("3?");
                    return true;
                }

                foreach (var thisPrereq in thisSubAtom.prereqs)
                {
                    //Debug.Log("4) we are checking this, right?");
                    if (checkOnePrereqFromSubAtom(thisPrereq, theInteractionMate) != true)
                    {
                        //Debug.Log("5?");
                        return false;
                    }
                }
                
            }
        }

        return true;
    }

    public bool checkOnePrereqFromSubAtom(string theSubAtomPrereq, interactionMate theInteractionMate)
    {
        //grabTestLOCKSubAtomPrereq1
        if (theSubAtomPrereq == "grabTestLOCKSubAtomPrereq1")
        {
            //Debug.Log("the testName11111 effect has occured");
            if (theInteractionMate.interactionAuthor.GetComponent<inventory1>().testInventory1.Contains("testKey1") != true)
            {
                return false;
            }

        }
        if(theSubAtomPrereq == "needsthisIsTheDamnLock")
        {
            //ad-hoc check if this variable is null:
            if (theInteractionMate.thisIsTheDamnLock == null)
            {
                Debug.Log("ddddddddddddddddddd says it's null:");
                return false;
            }
            else
            {
                Debug.Log("pppppppppppppp says not null:");
                Debug.Log(theInteractionMate.thisIsTheDamnLock);
            }
        }
        if (theSubAtomPrereq == "proximity0")
        {
            //needs SMALL distance between theObjectInitiatingTheInteraction and theObjectBeingInteractedWith


            if(areThereErrorsWithInteractionMate(theInteractionMate) == false)
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
        if (theSubAtomPrereq == "needsthisIsTheDamnLock")
        {
            if(theInteractionMate.thisIsTheDamnLock == null)
            {
                return false;
            }
        }
        


        //else if (theSubAtomPrereq == "testName2")
        {
            //Debug.Log("the testName22222 effect has occured");
            //theObjectBeingInteractedWith.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
        }

        return true;
    }



    public bool areThereErrorsWithInteractionMate(interactionMate theInteractionMate)
    {
        //return false if there are no errors

        //Debug.Log(theInteractionMate);
        //Debug.Log(theInteractionMate.interactionAuthor);
        //Debug.Log(theInteractionMate.interactionAuthor.transform.position);
        //Debug.Log(theInteractionMate.target1);
        //Debug.Log(theInteractionMate.target1.transform.position);


        if (theInteractionMate == null)
        {
            Debug.Log("theInteractionMate in my enaction phase is NULL");
            return true;
        }
        else if (theInteractionMate.interactionAuthor == null)
        {
            Debug.Log("''theInteractionMate.interactionAuthor'' in my enaction phase is NULL");
            return true;
        }
        else if (theInteractionMate.target1 == null)
        {
            Debug.Log("''theInteractionMate.target1'' in my enaction phase is NULL");
            return true;
        }

        return false;
    }



    public bool doesThisObjectHaveThisEffect(string thisEffect, GameObject thisObject)
    {
        //i need something to check whether something has a specific EFFECT

        interactionEffects1 interactionScript = thisObject.GetComponent<interactionEffects1>();

        foreach(var theInteraction in interactionScript.interactionsAvailable)
        {
            foreach (var thisAtom in theInteraction.atoms)
            {
                foreach (var thisSubAtom in thisAtom.subAtoms)
                {
                    foreach (var thisOTHEREffect in thisSubAtom.effects)
                    {
                        Debug.Log("compare the desired effect:  " + thisEffect + ", to the effect this other object has:  " + thisOTHEREffect);
                        if (thisOTHEREffect == thisEffect)
                        {
                            return true;
                        }
                    }

                }
            }
        }

        

        return false;
    }


    public testInteraction returnFirstInteractionOnObjectWithThisEffect(string thisEffect, GameObject thisObject)
    {
        //i need something to check whether something has a specific EFFECT

        interactionEffects1 interactionScript = thisObject.GetComponent<interactionEffects1>();

        foreach (var theInteraction in interactionScript.interactionsAvailable)
        {
            foreach (var thisAtom in theInteraction.atoms)
            {
                foreach (var thisSubAtom in thisAtom.subAtoms)
                {
                    foreach (var thisOTHEREffect in thisSubAtom.effects)
                    {
                        Debug.Log("compare the desired effect:  " + thisEffect + ", to the effect this other object has:  " + thisOTHEREffect);
                        if (thisOTHEREffect == thisEffect)
                        {
                            return theInteraction;
                        }
                    }

                }
            }
        }



        return null;
    }


    public void modularInteractions1(testInteraction theInteraction, interactionMate theInteractionMate)
    {
        //      ok so this is silly, i currently (somewhere) "unshell" an interaction mate, but now i'm gonna shell it AGAIN?  fix that

        //should the object be referenced inside the interaction class object?
        //or some other way?
        //what if interaction affects MULTIPLE targets?

        if (modularPrereqCheckBool(theInteraction, theInteractionMate) == false)
        {
            return;
        }

        foreach(var thisAtom in theInteraction.atoms)
        {
            foreach(var thisSubAtom in thisAtom.subAtoms)
            {
                //ok so this is silly, i currently (somewhere) "unshell" an interaction mate, but now i'm gonna shell it AGAIN?  fix that
                thisSubAtom.doSubAtom(theInteractionMate);
            }
        }
    }

    

    void doInteraction1OLD()
    {
        this.transform.Rotate(0.0f, 30.0f, 0.0f, Space.World);
        //Destroy(this.gameObject);
        //this.gameObject.
        //if (connectedObjects.Count > 0)
        {
            //foreach (GameObject thisObject in connectedObjects)
            {
                //interactive1 interactionScript = thisObject.GetComponent<interactive1>();

                //move this to other script
                //thisObject.transform.Rotate(30.0f, 00.0f, 0.0f, Space.World);
            }
        }
        //if (docked == true  && connectedObjects.Count > 0)
        {
            //output signal
            //basicInputOutput1 inOutScript = input1.GetComponent<basicInputOutput1>();
            //inOutScript.outBoolSignal = true;
        }
        //else
        {
            //this.transform.Rotate(0.0f, 30.0f, 0.0f, Space.World);
        }
    }


    void pickUp1()
    {
        //turn off the visible part of the object
        //move it to the location of the "inventory"
        //create parent-child relationship
        //.....add it to an inventory object list!



        Debug.Log("picked up item");

        //...................now i need to have access to which person is DOING the interaction.............hmmm.....
    }




    void fire()
    {
        this.transform.Rotate(0.0f, 30.0f, 0.0f, Space.World);
    }





    public void clonify(interactionEffects1 newInputScript)
    {
        //"deep copy" all important variables from this current script into this new input script.

        //public List<testInteraction> interactionsAvailable = new List<testInteraction>();

        List<testInteraction> cloningOldInteractionsAvailable = new List<testInteraction>();
        foreach(testInteraction thisTestInteraction in interactionsAvailable)
        {
            cloningOldInteractionsAvailable.Add(thisTestInteraction.deepCopy());
        }

        newInputScript.interactionsAvailable = cloningOldInteractionsAvailable;
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
        foreach (var thisEffect in effects)
        {
            doSubAtomEffects(thisEffect, theInteractionMate);
        }
    }

    public void doSubAtomEffects(string theSubAtomEffect, interactionMate theInteractionMate)
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
            if (theInteractionMate.thisIsTheDamnLock == null)
            {
                Debug.Log("''theInteractionMate.thisIsTheDamnLock'' is NULL in enaction phase");
                aTestBool = true;
            }
            if (theInteractionMate.thisIsTheDamnLock.name == null)
            {
                Debug.Log("''theInteractionMate.thisIsTheDamnLock.name'' is NULL in enaction phase");
                aTestBool = true;
            }
            if (theInteractionMate.whereTheFuckIsThisMade == null)
            {
                Debug.Log("''theInteractionMate.whereTheFuckIsThisMade'' is NULL in enaction phase");
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
        else if (theSubAtomEffect == "grabTestLOCKSubAtomPrereq1")
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
            if (theInteractionMate.thisIsTheDamnLock == null)
            {
                Debug.Log("''theInteractionMate.thisIsTheDamnLock'' is NULL in enaction phase");
                aTestBool = true;
            }



            if (aTestBool == false)
            {
                Debug.Log("should be going from " + theInteractionMate.interactionAuthor.name + " to:  " + theInteractionMate.thisIsTheDamnLock.name);
                //Debug.Log(theInteractionMate.target1);
                Debug.Log(theInteractionMate.thisIsTheDamnLock.name);
                //Debug.Log(theInteractionMate.target1.GetComponent<Transform>());
                //Debug.Log(theInteractionMate.target1.GetComponent<Transform>().position);

                Vector3 targetVector = theInteractionMate.thisIsTheDamnLock.GetComponent<Transform>().position;
                theInteractionMate.interactionAuthor.GetComponent<AIHub2>().thisNavMeshAgent.SetDestination(targetVector);
            }
            
            
        }

        else
        {
            Debug.Log(theInteractionMate.target1.name + " is trying to enact this subAtom effect" + theSubAtomEffect + "   but there is no ''if'' statement to catch it in modular interactions' ''doSubAtomEffects''");
            theInteractionMate.target1.GetComponent<Renderer>().material.color = new Color(55, 111, 222);
        }
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
}

public class testInteraction
{
    //do i need this level of abstraction above atoms for now?  i'm not sure
    //i mean, i GUESS.  that's the only reason modularizing interactions into atoms could serve, right?
    //well, no, modularizing them also helps with ENACTION, even if there were no larger abstraction?  i dunno

    [SerializeField] public string name;
    [SerializeField] public List<interactionAtom> atoms = new List<interactionAtom>();

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
}

public class interactionMate
{
    public GameObject interactionAuthor;

    public GameObject target1;
    public GameObject target2;
    public GameObject target3;

    public testInteraction enactThisInteraction;
    public testInteraction forInteraction;
    public testInteraction immediateInteraction;
    public testInteraction fillsPrereqFor;
    public GameObject objectFillsPrereq;

    public GameObject thisIsTheDamnLock;

    public string whereTheFuckIsThisMade;


    public void printMate()
    {
        Debug.Log("MMMMMMMMMMMMMMMMMMMMMMMMMMMM    printing interaction mate    MMMMMMMMMMMMMMMMMMMMMMMMMMMM");
        Debug.Log("interactionAuthor:  " + interactionAuthor);
        Debug.Log("target1:  " + target1);
        Debug.Log("target2:  " + target2);
        Debug.Log("target3:  " + target3);
        Debug.Log("enactThisInteraction:  " + enactThisInteraction.name);
        Debug.Log("forInteraction:  " + forInteraction);
        Debug.Log("immediateInteraction:  " + immediateInteraction);
        Debug.Log("fillsPrereqFor:  " + fillsPrereqFor);
        Debug.Log("objectFillsPrereq:  " + objectFillsPrereq);
        Debug.Log("thisIsTheDamnLock:  " + thisIsTheDamnLock);
        Debug.Log("whereTheFuckIsThisMade:  " + whereTheFuckIsThisMade);
        Debug.Log("WWWWWWWWWWWWWWWWW    END printing interaction mate    WWWWWWWWWWWWWWWWW");
    }

}

[SerializeField] public class fuckingTestClass
{
    [SerializeField] public int fuckingInt;
}
