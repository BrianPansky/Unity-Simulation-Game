using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.AI;
using static interactionEffects1;
using static UnityEngine.GraphicsBuffer;

public class AIHub2 : MonoBehaviour
{
    private worldScript theWorldScript;
    public NavMeshAgent thisNavMeshAgent;
    private Vector3 mytest;
    public GameObject testTarget;
    private sensorySystem theSensorySystem;
    public planningAndImagination thePlanner;
    public inventory1 theInventory;
    public List<interactionMate> adhocPrereqFillerTest;
    public List<GameObject> imGoinGTOLoseMyMInd;

    GameObject randomInteractionTarget;

    public int forgetfulnessTimer = 10;


    // Start is called before the first frame update
    void Start()
    {
        thisNavMeshAgent = GetComponent<NavMeshAgent>();
        mytest = this.transform.position + new Vector3(0, 0, -15);

        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;



        //no, no, no, don't "get" component, CREATE it!
        //an AIHub2 should probably come with a sensory system by DEFAULT,
        //so why not build that into its initialization?
        /// theSensorySystem = myTest2.GetComponent("sensorySystem") as sensorySystem;
        this.gameObject.AddComponent<sensorySystem>();
        theSensorySystem = this.gameObject.GetComponent("sensorySystem") as sensorySystem;
        //same for planning:
        this.gameObject.AddComponent<planningAndImagination>();
        thePlanner = this.gameObject.GetComponent("planningAndImagination") as planningAndImagination;

        //inventory1
        this.gameObject.AddComponent<inventory1>();
        theInventory = this.gameObject.GetComponent("inventory1") as inventory1;


        //this should be moved to a "regular human body" script:
        interactionEffects1 interactionScriptOnThisObject = this.gameObject.AddComponent<interactionEffects1>();
        //interactionScriptOnGeneratedObject.generateInteractionFULL("grabTestKey1", atomLister(atoms["grabTestKey1Atom"]));
        //interactionsAvailable
        //initialGenerator2
        initialGenerator2 theGeneratorScript = theWorldObject.GetComponent("initialGenerator2") as initialGenerator2;
        foreach (string thisKey in theGeneratorScript.atoms.Keys)
        {
            Debug.Log("keys:  " + thisKey);
        }
        interactionScriptOnThisObject.generateInteractionFULL("proximity0FillerInteraction1",
                theGeneratorScript.atomLister(theGeneratorScript.atoms["proximity0Atom"]),
                0
                );
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("========================     START update for:  " + this.gameObject.name + "     =====================");


        //ad hoc plan/action switching in some cases for now:
        if (forgetfulnessTimer == 0)
        {
            //forget whole plan, start again:
            adhocPrereqFillerTest = null;

            //reset timer:
            forgetfulnessTimer = 10;
        }
        else
        {
            forgetfulnessTimer -= 1;
        }




        if (adhocPrereqFillerTest == null)
        {
            //Debug.Log("''adhocPrereqFillerTest'' is null for this NPC:  " + this.gameObject.name);
            pickRandomNearbyInteractionAndTryIt();

            //Debug.Log("WHERE TEH FUCK IS THIS HAPPENEINGGGGGGGGGGGG AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            //Debug.Log(adhocPrereqFillerTest);
            //Debug.Log(adhocPrereqFillerTest[0]);      right, this will ALWAYS be null error here!  we just tested for that!
            

        }
        else if (adhocPrereqFillerTest.Count == 0)
        {
            //Debug.Log("''adhocPrereqFillerTest'' is EMPTY for this NPC:  " + this.gameObject.name);
            pickRandomNearbyInteractionAndTryIt();

            //Debug.Log("WHERE TEH FUCK IS THIS HAPPENEINGGGGGGGGGGGG BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
            //Debug.Log(adhocPrereqFillerTest);
            //Debug.Log(adhocPrereqFillerTest[0]);
            //Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction);
            //Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction.name);
        }
        else
        {
            //Debug.Log("''adhocPrereqFillerTest'' has prereqFiller for the following NPC:  " + this.gameObject.name + ", so they should do them or plan to do them...");
            //      ohhh this doesn't work.  the input to this function needs to be an OBJECT.
            //      but what i have is a specific INTERACTION.
            //      so, i need to grab what's needed, and maybe make a new function to do it properly
            //      since the functions i made to find the interaction already presumably have access to the object, just modify those to return objects
            //      then make an enaction "use" function that uses some interaction from an object.  somehow.  however that works.

            //k i guess i sorta fixed that at some point?  i'm using an object now, right?  it works fine?
            //Debug.Log(this.gameObject.name);
            //Debug.Log(adhocPrereqFillerTest[0].interactionAuthor.name);

            //interactionMate thisMate = new interactionMate();
            //thisMate.interactionAuthor = this.gameObject;
            //thisMate.target1 = randomInteractionTarget;

            //tryRandomInteractionWithTarget(adhocPrereqFillerTest[0].target1, adhocPrereqFillerTest[0]);
            //doThisInteractionOrPlanToDoIt(adhocPrereqFillerTest[0].forInteraction, adhocPrereqFillerTest[0]);
            //adhocPrereqFillerTest = null;

            //Debug.Log(adhocPrereqFillerTest);
            //Debug.Log(adhocPrereqFillerTest[0]);
            //Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction);
            //Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction.name);

            //if (theWorldScript.theRespository.theInteractionEffects1.modularPrereqCheckBool(adhocPrereqFillerTest[0].enactThisInteraction, adhocPrereqFillerTest[0]))
            
            if (adhocPrereqFillerTest[0].checkInteractionMatePrereqs())
            {
                adhocPrereqFillerTest[0].doThisInteraction();
                randomInteractionTarget = null;
            }



        }
        //goToWhicheverAvailableTarget();


        Debug.Log("----------------------     END update for:  " + this.gameObject.name + "     -----------------------");
    }

    public interactionMate pickRandomNearbyInteractionReturnMate()
    {
        GameObject randomInteractableObject = pickRandomNearbyInteractableObject();

        testInteraction thisInteraction = pickRandomInteractionONObject(randomInteractableObject);

        interactionMate thisMate = createBasicInteractionMate(this.gameObject, randomInteractableObject, thisInteraction);
        

        return thisMate;
    }

    public interactionMate createBasicInteractionMate(GameObject author, GameObject target1, testInteraction theInteractionToEnact)
    {
        interactionMate newMate = new interactionMate();

        newMate.interactionAuthor = author;
        newMate.target1 = target1;
        newMate.enactThisInteraction = theInteractionToEnact;

        return newMate;
    }


    public void pickRandomNearbyInteractionAndTryIt()
    {
        //step 1:  find some nearby interactions, and randomly pick one
        //step 2:  "try" it [not necessarily succeed, though i'm not sure how failure will work yet]

        interactionMate thisMate = pickRandomNearbyInteractionReturnMate();

        List<interactionMate> listOfActions = planForPrereqsIfNeeded(thisMate);

        if(areThereAnyErrorsWithThisListOfActions(listOfActions) == false)
        {
            listOfActions[0].doThisInteraction();
        }

    }

    


    public bool areThereAnyErrorsWithThisListOfActions(List<interactionMate> listOfActions)
    {
        //returns TRUE if there's an ERROR

        if(listOfActions == null)
        {
            Debug.Log("apparently we don't have a plan to fill the unfilled prereq here  (it's NULL)");
            return true;
        }
        else if(listOfActions.Count == 0)
        {
            Debug.Log("apparently we don't have a plan to fill the unfilled prereq here (it's count is ZERO)");
            return true;
        }
        else if(listOfActions[0].enactThisInteraction == null)
        {
            Debug.Log("the first item in the prereq filling plan has the ''enactThisInteraction'' variable = NULL");
            return true;
        }

        return false;
    }


    public List<interactionMate> planForPrereqsIfNeeded(interactionMate thisMate)
    {
        //if prereqs are met, do it
        //if not, plan to fill them

        List<interactionMate> thePlan = new List<interactionMate>();
        testInteraction thisInteraction = thisMate.enactThisInteraction;

        //if (theWorldScript.theRespository.theInteractionEffects1.modularPrereqCheckBool(thisInteraction, thisMate))

        if (thisMate.checkInteractionMatePrereqs())
        {
            //Debug.Log("prereqs met for this interaction:  " + thisInteraction.name);
            
            thePlan.Add(thisMate);
            return thePlan;
        }
        else
        {
            //Debug.Log("prereqs NOT met for this interaction:  " + thisInteraction.name);



            //      THIS IS PROBABLY REDUNDANT NOW, JUST USE THE MATE I ALREADY HAVE
            //thisMate.printMate();
            interactionMate newMate = new interactionMate();
            newMate.interactionAuthor = this.gameObject;
            newMate.target1 = thisMate.target1;

            return returnPlanToFillPrereqs(thisInteraction, newMate);
        }
    }



    public void printStringListWithPreface(string preface, List<string> theList)
    {
        //"preface" isn't quite the correct word.  that would imply it only appears once, at the start
        //instead, it appears before EVERY entry.
        //or could change it so i DO only print a preface at start. and maybe a dividine line at the end...

        foreach (string thisItem in theList)
        {
            Debug.Log(preface + thisItem);
        }
    }






    //          ARE THESE THE SAME?  REDUNDANT?
    public List<interactionMate> returnPlanToFillPrereqs(testInteraction theInteraction, interactionMate thisMate)
    {
        //so.  prereqs are not filled.
        //so how to fill them?
        //"look around" in environment [tags, actually] for objects with interactions that can fulfil it.
        //so:
        //      need to know WHICH prereq(s) is/are not filled
        //      need to fill it/each
        //      have a plan somewhere so it can be DONE next frame.....[or i just do it THIS frame for now?]


        //which prereqs are not filled:
        List<string> unfilledPrereqs = theInteraction.returnAllUnfilledPrereqs(thisMate);

        //how to fill each:
        List<interactionMate> prereqFillers = returnPlansToFillPrereqs(unfilledPrereqs, thisMate);

        return prereqFillers;
    }

    public List<interactionMate> returnPlansToFillPrereqs(List<string> unfilledPrereqs, interactionMate thisMate)
    {
        List<interactionMate> prereqFillers = new List<interactionMate>();

        //should be able to check just the string to see if other available interactions in the area have that effect?
        //are they in legibstrata?  or what?
        //for ALL [or until a suitable one is found?] interactions "in the area" [everywhere for now][so, find them ALL with "interactable" tag, then iterate through their availableInteractions]
        //      see if this interaction can fulfill the specific prereq
        //      if so, for now just return the FIRST one that works?

        foreach (string thisUnfilledPrereq in unfilledPrereqs)
        {
            //doesn't need objects or context???  well, for now?  context will be found by the tagging system inside this next function, and perhaps that can be specified from outside of it.  but for now just ALL
            prereqFillers.AddRange(returnPlansToFillONEPrereq(thisUnfilledPrereq, thisMate));
        }




        return prereqFillers;
    }









    public GameObject pickRandomNearbyInteractableObject()
    {
        //well this seems redundant:
        return pickRandomNearbyInteractable();
    }

    public testInteraction pickRandomInteractionONObject(GameObject randomInteractionTarget)
    {
        List<testInteraction> availableIntertactions = randomInteractionTarget.GetComponent<interactionEffects1>().interactionsAvailable;
        
        if (availableIntertactions.Count > 0)
        {
            return availableIntertactions[UnityEngine.Random.Range(0, availableIntertactions.Count)];
        }
        else
        {
            Debug.Log("there are zero items on the interactions available list for this object, but it's supposed to have interactions:  " + randomInteractionTarget.name);
            Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, randomInteractionTarget.GetComponent<Transform>().position, Color.red, 0.1f);
            return null;
        }
    }






    public List<interactionMate> returnPlansToFillONEPrereq(string unfilledPrereq, interactionMate thisMate)
    {
        List<interactionMate> prereqFillers = new List<interactionMate>();

        prereqFillers.AddRange(privatePrereqFillers(unfilledPrereq, thisMate));
        prereqFillers.AddRange(onPersonPrereqFillers(unfilledPrereq, thisMate));

        if(prereqFillers == null || prereqFillers.Count == 0)
        {
            prereqFillers.AddRange(worldPrereqFillers(unfilledPrereq, thisMate));
        }

        return prereqFillers;
    }

    private List<interactionMate> privatePrereqFillers(string unfilledPrereq, interactionMate thisMate)
    {
        List<interactionMate> prereqFillers = new List<interactionMate>();



        testInteraction isThereAnInteraction = theWorldScript.theRespository.theInteractionEffects1.returnFirstPrivateInteractionOnObjectWithThisEffect(unfilledPrereq, this.gameObject);
        if (isThereAnInteraction != null)
        {
            prereqFillers.Add(basicPackup(isThereAnInteraction, thisMate));
        }

        return prereqFillers;
    }

    private List<interactionMate> onPersonPrereqFillers(string unfilledPrereq, interactionMate thisMate)
    {
        //this will be for stuff like INVENTORIES, which i don't have totally implemented yet!
        //technically the "key" currently goes in inventory, but the lock just works if the key is there, no actual "interaction" has to happen between lock and key.
        //i guess i should change that!  key should "unlock' the lock, and then a SEPARATE interaction should "open" it.



        List<interactionMate> prereqFillers = new List<interactionMate>();

        return prereqFillers;
    }

    private List<interactionMate> worldPrereqFillers(string unfilledPrereq, interactionMate thisMate)
    {
        //currently returns "first" prereq filler, not ALL possible ones

        List<interactionMate> prereqFillers = new List<interactionMate>();

        //this is HUGELY computationally wasteful, and the legibstrata should have a dictionary to EASILY look up ONLY the objects etc that can fulfill a SPECIFIC prereq!
        List<GameObject> allObjectsThatAreInteractable = theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable");

        foreach (GameObject thisObject in allObjectsThatAreInteractable)
        {
            //Debug.Log("does this object fulfill prereq we are looking for?:  " + thisObject.name);
            //if (theWorldScript.theRespository.theInteractionEffects1.doesThisObjectHaveThisEffect(unfilledPrereq, thisObject) == true)
            testInteraction isThereAnInteraction = theWorldScript.theRespository.theInteractionEffects1.returnFirstInteractionOnObjectWithThisEffect(unfilledPrereq, thisObject);
            if (thisObject != this.gameObject && isThereAnInteraction != null)
            {
                prereqFillers.Add(basicPackup(isThereAnInteraction, thisMate));
            }
        }

        return prereqFillers;
    }

    private interactionMate basicPackup(testInteraction isThereAnInteraction, interactionMate thisMate)
    {

        //interactionMate
        interactionMate newMate = new interactionMate();
        newMate.interactionAuthor = this.gameObject;
        newMate.target1 = thisMate.target1;
        newMate.enactThisInteraction = isThereAnInteraction;

        return newMate;
    }



    private GameObject pickRandomNearbyInteractable()
    {
        //grab all objects within some nearby range
        //from them, select those with appropriate tag
        //from those, select one at random


        //findSECONDNearestXToYExceptY
        //findXNearestToYExceptY

        //well, for now, skip the proximity condition complication:
        //return theWorldScript.theTagScript.randomTaggedWith("interactable");


        //return theWorldScript.theTagScript.findXNearestToYExceptY("interactable", this.gameObject);
        //pickRandomObjectFromListEXCEPT
        GameObject thisSelection = theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable"), this.gameObject);
        //Debug.Log("ok it's '''randomly''' picking this object:  " + thisSelection.name);
        return thisSelection;

    }



}
