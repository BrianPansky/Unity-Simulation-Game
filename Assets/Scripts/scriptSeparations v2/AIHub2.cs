using System;
using System.Collections;
using System.Collections.Generic;
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
        interactionEffects1 interactionScriptOnGeneratedObject = this.gameObject.AddComponent<interactionEffects1>();
        //interactionScriptOnGeneratedObject.generateInteractionFULL("grabTestKey1", atomLister(atoms["grabTestKey1Atom"]));
        //interactionsAvailable
        //initialGenerator2
        initialGenerator2 theGeneratorScript = theWorldObject.GetComponent("initialGenerator2") as initialGenerator2;
        foreach (string thisKey in theGeneratorScript.atoms.Keys)
        {
            Debug.Log("keys:  " + thisKey);
        }
        interactionScriptOnGeneratedObject.interactionsAvailable.Add(
            interactionScriptOnGeneratedObject.generateInteractionFULL("proximity0FillerInteraction1",
                theGeneratorScript.atomLister(theGeneratorScript.atoms["proximity0Atom"])
                ));
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

            if (theWorldScript.theRespository.theInteractionEffects1.modularPrereqCheckBool(adhocPrereqFillerTest[0].enactThisInteraction, adhocPrereqFillerTest[0]))
            {
                //Debug.Log("prereqs met for:  " + thisInteraction.name);
                //travelToTargetObject(randomInteractionTarget);
                //      FOR MATE  this.gameObject, randomInteractionTarget
                theWorldScript.theRespository.theInteractionEffects1.modularInteractions1(adhocPrereqFillerTest[0].enactThisInteraction, adhocPrereqFillerTest[0]);
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


        interactionMate thisMate = new interactionMate();
        thisMate.interactionAuthor = this.gameObject;
        thisMate.target1 = randomInteractableObject;
        thisMate.enactThisInteraction = thisInteraction;

        return thisMate;
    }




    public void pickRandomNearbyInteractionAndTryIt()
    {
        //step 1:  find some nearby interactions, and randomly pick one
        //step 2:  "try" it [not necessarily succeed, though i'm not sure how failure will work yet]

        //but how to not select new every frame?  how to skip work tha is done?
        //need to be saving variables, generating plan from bottom-up?

        //for now, just pick object.  have interaction info stored in the object:

        interactionMate thisMate = pickRandomNearbyInteractionReturnMate();




        //doThisInteractionMateOrPlanToDoIt(thisMate);


        List<interactionMate> listOfActions = planForPrereqsIfNeeded(thisMate);


        if(areThereAnyErrorsWithThisListOfActions(listOfActions) == false)
        {
            theWorldScript.theRespository.theInteractionEffects1.modularInteractions1(listOfActions[0].enactThisInteraction, listOfActions[0]);
            //randomInteractionTarget = null;
        }






        if (true == false)
        {
            randomInteractionTarget = pickRandomNearbyInteractableObject();
            //Debug.Log("ok so is it still this object???:  " + randomInteractionTarget.name);

            testInteraction thisInteraction = pickRandomInteractionONObject(randomInteractionTarget);
            //tryRandomInteractionWithTarget(randomInteractionTarget);
            Debug.Log("kay, picked an interaction on it:  " + thisInteraction.name);

            interactionMate newMate = new interactionMate();
            newMate.interactionAuthor = this.gameObject;
            newMate.target1 = randomInteractionTarget;

            doThisInteractionOrPlanToDoIt(thisInteraction, newMate);

        }



        //if(true == false)
        {
            //if (randomInteractionTarget == null)
            {
                //Debug.Log(this.gameObject.name + "22222222 nulllllllllllllllllllllllllllllllllllll");

            }
            //else
            {
                //Debug.Log(this.gameObject.name + "22222 NOT null:  " + randomInteractionTarget.name);
            }
        }
        

    }

    


    public bool areThereAnyErrorsWithThisListOfActions(List<interactionMate> listOfActions)
    {
        //returns true if there's an error

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
        //SIMILAR FUNCTION TO "whatIsThisAdhocEnactionStuff" [among others]
        //if prereqs are met, do it
        //if not, plan to fill them....
        //      .....then presumably the "fill them" check in update function will catch that and handle it.....

        List<interactionMate> thePlan = new List<interactionMate>();

        //      FOR MATE  this.gameObject, randomInteractionTarget
        //Debug.Log("yyyyyyyyyyyyy  checking prereqs before doing action");
        testInteraction thisInteraction = thisMate.enactThisInteraction;
        if (theWorldScript.theRespository.theInteractionEffects1.modularPrereqCheckBool(thisInteraction, thisMate))
        {
            Debug.Log("prereqs met for this interaction:  " + thisInteraction.name);
            thePlan.Add(thisMate);
            return thePlan;
        }
        else
        {
            Debug.Log("prereqs NOT met for this interaction:  " + thisInteraction.name);
            List<string> unfilledPrereqs = returnAllUnfilledPrereqs(thisInteraction, thisMate);
            foreach(string prereq in unfilledPrereqs)
            {
                Debug.Log("one of the unfilled prereqs is:  " + prereq);
            }

            //is this the right function to use?
            thisMate.printMate();
            interactionMate newMate = new interactionMate();
            newMate.interactionAuthor = this.gameObject;
            newMate.target1 = thisMate.target1;
            newMate.thisIsTheDamnLock = thisMate.target1;
            return returnPlanToFillPrereqs(thisInteraction, newMate);
            //randomInteractionTarget = null;
        }
    }

    public void doThisInteractionMateOrPlanToDoIt(interactionMate thisMate)
    {
        //SIMILAR FUNCTION TO "whatIsThisAdhocEnactionStuff"
        //if prereqs are met, do it
        //if not, plan to fill them....
        //      .....then presumably the "fill them" check in update function will catch that and handle it.....

        //      FOR MATE  this.gameObject, randomInteractionTarget
        Debug.Log("yyyyyyyyyyyyy  checking prereqs before doing action");
        testInteraction thisInteraction = thisMate.enactThisInteraction;
        if (theWorldScript.theRespository.theInteractionEffects1.modularPrereqCheckBool(thisInteraction, thisMate))
        {
            Debug.Log("  tttttttt  prereqs met for:  " + thisInteraction.name);
            //travelToTargetObject(randomInteractionTarget);
            //      FOR MATE  this.gameObject, randomInteractionTarget
            theWorldScript.theRespository.theInteractionEffects1.modularInteractions1(thisInteraction, thisMate);
            randomInteractionTarget = null;
        }
        else
        {
            Debug.Log("wwwwwww  prereqs NOT met for:  " + thisInteraction.name);
            //is this the right function to use?
            interactionMate newMate = new interactionMate();
            newMate.interactionAuthor = this.gameObject;
            newMate.target1 = randomInteractionTarget;
            returnPlanToFillPrereqs(thisInteraction, newMate);
            //randomInteractionTarget = null;
        }

    }

    List<interactionMate> returnPlanToFillPrereqs(testInteraction theInteraction, interactionMate thisMate)
    {

        //kay.
        //so, for now, just do everything HERE.  can sort out where i want to put all this code and handle everything later.
        //so.  prereqs are not filled.
        //so how to fill them?
        //"look around" in environment [tags, actually] for objects with interactions that can fulfil it.
        //so:
        //      need to know WHICH prereq(s) is/are not filled
        //      need to fill it/each
        //      have a plan somewhere so it can be DONE next frame.....


        //which prereqs are not filled:
        List<string> unfilledPrereqs = returnAllUnfilledPrereqs(theInteraction, thisMate);

        //how to fill each:
        List<interactionMate> prereqFillers = returnPlansToFillPrereqs(unfilledPrereqs, thisMate);

        if(true == false)
        {
            Debug.Log("here are prereqFillers for:  " + theInteraction.name);
            foreach (interactionMate theInteractionMate in prereqFillers)
            {
                //Debug.Log(prereq.name);
                theInteractionMate.fillsPrereqFor = theInteraction;
                Debug.Log("11111here is the [prereq filling] interaction mate author:  " + theInteractionMate.interactionAuthor.name + ", and the ''target1'':  " + theInteractionMate.target1.name);
            }


            //at the end....        PUT THE PLAN SOMEWHERE SO IT CAN BE DONE:
            adhocPrereqFillerTest = prereqFillers;
            Debug.Log("WHERE TEH FUCK IS THIS HAPPENEINGGGGGGGGGGGG CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC");
            Debug.Log(adhocPrereqFillerTest);
            Debug.Log(adhocPrereqFillerTest[0]);
            Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction);
            Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction.name);
            Debug.Log("now ''adhocPrereqFillerTest'' should NOT be null....if there are prereq fillers");
            foreach (interactionMate theInteractionMate in prereqFillers)
            {
                //Debug.Log(prereq.name);
                Debug.Log("zzzzzzhere is the [prereq filling] interaction mate author:  " + theInteractionMate.interactionAuthor.name + ", and the ''target1'':  " + theInteractionMate.target1.name);
            }
            //or, ad hoc, just do it NOW:

        }




        return prereqFillers;
    }





    public void doThisInteractionOrPlanToDoIt(testInteraction thisInteraction, interactionMate theInteractionMate)
    {
        //SIMILAR FUNCTION TO "whatIsThisAdhocEnactionStuff"
        //if prereqs are met, do it
        //if not, plan to fill them....
        //      .....then presumably the "fill them" check in update function will catch that and handle it.....

        //      FOR MATE  this.gameObject, randomInteractionTarget
        Debug.Log("yyyyyyyyyyyyy  checking prereqs before doing action");
        if (theWorldScript.theRespository.theInteractionEffects1.modularPrereqCheckBool(thisInteraction, theInteractionMate))
        {
            Debug.Log("  tttttttt  prereqs met for:  " + thisInteraction.name);
            //travelToTargetObject(randomInteractionTarget);
            //      FOR MATE  this.gameObject, randomInteractionTarget
            theWorldScript.theRespository.theInteractionEffects1.modularInteractions1(thisInteraction, theInteractionMate);
            randomInteractionTarget = null;
        }
        else
        {
            Debug.Log("wwwwwww  prereqs NOT met for:  " + thisInteraction.name);
            //is this the right function to use?
            interactionMate thisMate = new interactionMate();
            thisMate.interactionAuthor = this.gameObject;
            thisMate.target1 = randomInteractionTarget;
            doingStuffWhenPrereqsNotMetHereForNow(thisInteraction, thisMate);
            //randomInteractionTarget = null;
        }
    
    }

    public void goToWhicheverAvailableTarget()
    {
        //[for initial testing]

        if (theSensorySystem.target != null)
        {
            travelToTargetObject(theSensorySystem.target);
        }
        else
        {
            //travelToTargetPoint(mytest);
            travelToTargetObject(testTarget);
        }
    }


    public GameObject pickRandomNearbyInteractableObject()
    {
        //step 1:  find some nearby interactions, and randomly pick one
        //step 2:  "try" it [not necessarily succeed, though i'm not sure how failure will work yet]

        //but how to not select new every frame?  how to skip work tha is done?
        //need to be saving variables, generating plan from bottom-up?

        //for now, just pick object.  have interaction info stored in the object:

        if (randomInteractionTarget == null)
        {
            //Debug.Log(this.gameObject.name + "111111 nulllllllllllllllllllllllllllllllllllll");
            return pickRandomNearbyInteractable();
            //Debug.Log(randomInteractionTarget.name);

            if (randomInteractionTarget != null)
            {
                //Debug.Log(randomInteractionTarget.name);
            }
            else
            {
                //Debug.Log("nulllllllllllllllllllllllllllllllllllll");
            }
        }
        else
        {
            //Debug.Log(this.gameObject.name + "111111 NOT null:  " + randomInteractionTarget.name);
        }

        return randomInteractionTarget;
    }

    public testInteraction pickRandomInteractionONObject(GameObject randomInteractionTarget)
    {
        List<testInteraction> availableIntertactions = randomInteractionTarget.GetComponent<interactionEffects1>().interactionsAvailable;
        
        if (availableIntertactions.Count > 0)
        {
            //right now it's returning FIRST one!  not random one!
            return availableIntertactions[0];
        }
        else
        {
            Debug.Log("there are zero items on the interactions available list for this object, but it's supposed to have interactions:  " + randomInteractionTarget.name);
            Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, randomInteractionTarget.GetComponent<Transform>().position, Color.red, 0.1f);
            return null;
        }
    }


    public void whatIsThisAdhocEnactionStuff(List<testInteraction> availableIntertactions, interactionMate theInteractionMate)
    {
        //SIMILAR FUNCTION TO "doThisInteractionOrPlanToDoIt"
        //      indeed, just do ENACTION here as well for now.  don't bother with proximity checks or "nextAction" or anything.  just get modular enaction going.  then get same for player interaction.
        //now i have a prereq checker.  so do that for "trying", for now:
        //      FOR MATE  this.gameObject, randomInteractionTarget
        if (theWorldScript.theRespository.theInteractionEffects1.modularPrereqCheckBool(availableIntertactions[0], theInteractionMate))
        {
            travelToTargetObject(randomInteractionTarget);
            //      for mate this.gameObject, randomInteractionTarget
            theWorldScript.theRespository.theInteractionEffects1.modularInteractions1(availableIntertactions[0], theInteractionMate);
        }
        else
        {
            interactionMate thisMate = new interactionMate();
            thisMate.interactionAuthor = this.gameObject;
            thisMate.target1 = randomInteractionTarget;
            doingStuffWhenPrereqsNotMetHereForNow(availableIntertactions[0], thisMate);
            randomInteractionTarget = null;

        }
    }

    
    public void tryRandomInteractionWithTargetDONTUSETHIS(GameObject randomInteractionTarget, interactionMate theInteractionMate)
    {
        //randomly select an available interaction "on" the object
        //check prereqs, fill them?  [including PROXIMITY prereq!]
        //do interaction



        //so, need to detect
        //      what interactions are availalbe on the object
        //      how to do them [what is required to perform the interaction]
        //      [for now we are ignoring the effects, we don't care abou tconsequences]
        //and THEN need to actually "try" DOING them

        //so, step 1, pick random interaction ON the object:
        //so, i need a "legibstration" of such info.  or something.
        //possibly a class object that needs to be [sigh] initialized for each object
        //but it's a trade off between the flexibility of a dictionary's STRING keys, but the inflexibility of their contents
        //with the flexibility of a class object's contents, but the inflexibility of calling for them by name
        //for now, i can't think of anything for it to contain other than strings like tags, so just do it that way?
        //and if i need to store objects, a SEPARATE dictionary can store objects, using keys related to legibstration info?




        //NEED TO FIX!  NOT INITIALIZED WITH KEYS AND CONTENTS!!!  SEE CHECKLIST BELOW!!!
        //List<string> availableIntertactions = theWorldScript.theRespository.theLegibstrata.legibictionary[this.gameObject.name]["interactions"];
        
        //get the list of legible "sub-tags" from the lebistrata dictionary:
        //[also needs to be initialized in the interaction script, along with the rest of the actual interaction stuff for each object]
        
        //foreach(string str in availableIntertactions)
        {
            //Debug.Log(str);
            //it works!

            //now:
            //be able to pick one at random
            //decide what to do based on modular info associated with that? [again, maybe these should be like "action" class objects, not mere strings]
        }

        //need to add these to lebictionary:
        //      name of object key
        //          "interactions" category key
        //              some strings on the resulting list, that name interactions


        //now pick a random item from that list somehow

        //now class objects:
        //Debug.Log(randomInteractionTarget.name);
        //List<string> myKeyList = new List<string>(this.theWorldScript.interactionLegibstration.Keys);
        //foreach(string key in myKeyList)
        {
            //Debug.Log("HELLLOOOOOOOOOOOOOOOOOOOOO?????????????????????????????????????????????????????????");
            //Debug.Log(key);
        }
        //List<testInteraction> availableIntertactions = theWorldScript.interactionLegibstration[randomInteractionTarget.name];
        List<testInteraction> availableIntertactions = randomInteractionTarget.GetComponent<interactionEffects1>().interactionsAvailable;
        //foreach (testInteraction thisTestInteraction in availableIntertactions)
        {
            //Debug.Log(thisTestInteraction.name);
        }

        //picking random is easy, so skip it FOR NOW.  just pick first one if list is not empty?
        //now what?  "try" it somehow, for some reason.  magic reason for now?  the reasons for calling this funciton should exist OUTSIDE this function, don't worry about that here.
        //so, what does "trying" an interaction entail?  for simple "doing", basically you do the whole "check prereqs and fill them" thing.
        //should i start there?  or go for whatever else this "trying" stuff is?  i don't have a clear idea or example of trying.
        //think of ho things fail, or are thwarted by others, i guess.  pretty important.  but does that belong HERE?  not really.
        //either way, basically need to hand this desired interaction over to some kind of PLANNER, and go from there.
        //but i don't care about much actual PLANNING right now.  but i do care about modular enaction/interaction system, which the planner will presumably need to handle
        //          so, start with the interactions and enaction phase [build planner later]

        //      indeed, just do ENACTION here as well for now.  don't bother with proximity checks or "nextAction" or anything.  just get modular enaction going.  then get same for player interaction.
        if(availableIntertactions.Count > 0)
        {
            //now i have a prereq checker.  so do that for "trying", for now:
            //      FOR MATE this.gameObject, randomInteractionTarget
            if (theWorldScript.theRespository.theInteractionEffects1.modularPrereqCheckBool(availableIntertactions[0], theInteractionMate))
            {
                //for testing:
                //GameObject objectFromLegibstrata = 
                //well, START with object from tags:
                //GameObject randomInteractableObject = theWorldScript.theTagScript.randomTaggedWith("interactable");
                //wait, that's what i'm already doing.  so now i need to take this object i already have, and pick an interaction inside of it.....basicdally....
                travelToTargetObject(randomInteractionTarget);


                //      FOR MATE this.gameObject, randomInteractionTarget
                theWorldScript.theRespository.theInteractionEffects1.modularInteractions1(availableIntertactions[0], theInteractionMate);
            }
            else
            {
                //randomInteractionTarget = null;  //uhhhh this doesn't work!!!!!!!!!!!!!!!  because this function doesn't "Return" it???
                //so, for now, just do everything HERE.  can sort out where i want to put all this code and handle everything later.
                //so.  prereqs are not filled.
                //so how to fill them?
                //"look around" in environment [tags, actually] for objects with interactions that can fulfil it.
                //so:
                //      need to know WHICH prereq(s) is/are not filled
                //      need to fill it/each
                //      have a plan somewhere so it can be DONE next frame.....
                interactionMate thisMate = new interactionMate();
                thisMate.interactionAuthor = this.gameObject;
                thisMate.target1 = randomInteractionTarget;
                doingStuffWhenPrereqsNotMetHereForNow(availableIntertactions[0], thisMate);
                randomInteractionTarget = null;



                //Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                //is that all i need to do?  will find new interactions?  i think it will actually keep trying the SAME one, forever....
                //because my "random" interaction picker isn't actually random.  picks the same one EVERY SINGLE FRAME right now, i think
            }
            
        }
        else
        {
            Debug.Log("there are zero items on the interactions available list for this object, but it's supposed to have interactions:  " + randomInteractionTarget.name);
            Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, randomInteractionTarget.GetComponent<Transform>().position, Color.red, 0.1f);
        }


        //very nice, it works




        if (randomInteractionTarget == null)
        {
            //Debug.Log("nulllllllllllllllllllllllllllllllllllll");

        }
        else
        {
            //Debug.Log("NOT null:  " + randomInteractionTarget.name);
        }


    }




    public void doingStuffWhenPrereqsNotMetHereForNow(testInteraction theInteraction, interactionMate thisMate)
    {
        //kay.
        //so, for now, just do everything HERE.  can sort out where i want to put all this code and handle everything later.
        //so.  prereqs are not filled.
        //so how to fill them?
        //"look around" in environment [tags, actually] for objects with interactions that can fulfil it.
        //so:
        //      need to know WHICH prereq(s) is/are not filled
        //      need to fill it/each
        //      have a plan somewhere so it can be DONE next frame.....


        //which prereqs are not filled:
        List<string> unfilledPrereqs = new List<string>();
        unfilledPrereqs = returnAllUnfilledPrereqs(theInteraction, thisMate);

        //how to fill each:
        List<interactionMate> prereqFillers = new List<interactionMate>();
        prereqFillers = returnPlansToFillPrereqs(unfilledPrereqs, thisMate);
        Debug.Log("here are prereqFillers for:  " + theInteraction.name);
        foreach(interactionMate theInteractionMate in prereqFillers)
        {
            //Debug.Log(prereq.name);
            theInteractionMate.fillsPrereqFor = theInteraction;
            Debug.Log("11111here is the [prereq filling] interaction mate author:  " + theInteractionMate.interactionAuthor.name + ", and the ''target1'':  " + theInteractionMate.target1.name);
        }

        
        //at the end....        PUT THE PLAN SOMEWHERE SO IT CAN BE DONE:
        adhocPrereqFillerTest = prereqFillers;
        Debug.Log("WHERE TEH FUCK IS THIS HAPPENEINGGGGGGGGGGGG CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC");
        Debug.Log(adhocPrereqFillerTest);
        Debug.Log(adhocPrereqFillerTest[0]);
        Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction);
        Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction.name);
        Debug.Log("now ''adhocPrereqFillerTest'' should NOT be null....if there are prereq fillers");
        foreach (interactionMate theInteractionMate in prereqFillers)
        {
            //Debug.Log(prereq.name);
            Debug.Log("zzzzzzhere is the [prereq filling] interaction mate author:  " + theInteractionMate.interactionAuthor.name + ", and the ''target1'':  " + theInteractionMate.target1.name);
        }
        //or, ad hoc, just do it NOW:


    }

    public List<string> returnAllUnfilledPrereqs(testInteraction theInteraction, interactionMate thisMate)
    {
        List<string> unfilledPrereqs = new List<string>();

        foreach (var thisAtom in theInteraction.atoms)
        {
            foreach (var thisSubAtom in thisAtom.subAtoms)
            {
                List<string> thisBunch = listUnfilledPrereqsForOneSubAtom(thisSubAtom, thisMate);

                if (thisBunch != null)
                {
                    unfilledPrereqs.AddRange(thisBunch);
                }

            }
        }


        return unfilledPrereqs;
    }


    public List<string> listUnfilledPrereqsForOneSubAtom(interactionSubAtom theSubAtom, interactionMate thisMate)
    {
        List<string> unfilledPrereqs = new List<string>();

        foreach (var thisPrereq in theSubAtom.prereqs)
        {
            if(theWorldScript.theRespository.theInteractionEffects1.checkOnePrereqFromSubAtom(thisPrereq, thisMate) == false)
            {
                unfilledPrereqs.Add(thisPrereq);
            }

        }

        return unfilledPrereqs;
    }


    public List<interactionMate> returnPlansToFillPrereqs(List<string> unfilledPrereqs, interactionMate thisMate)
    {
        List<interactionMate> prereqFillers = new List<interactionMate>();

        //should be able to check just the string to see if other available interactions in the area have that effect?
        //are they in legibstrata?  or what?
        //for ALL [or until a suitable one is found?] interactions "in the area" [everywhere for now][so, find them ALL with "interactable" tag, then iterate through their availableInteractions]
        //      see if this interaction can fulfill the specific prereq
        //      if so, for now just return the FIRST one that works?

        foreach(string thisUnfilledPrereq in unfilledPrereqs)
        {
            //doesn't need objects or context???  well, for now?  context will be found by the tagging system inside this next function, and perhaps that can be specified from outside of it.  but for now just ALL
            prereqFillers.AddRange(returnPlansToFillONEPrereq(thisUnfilledPrereq, thisMate));
        }




        return prereqFillers;
    }


    public List<interactionMate> returnPlansToFillONEPrereq(string unfilledPrereq, interactionMate thisMate)
    {
        List<interactionMate> prereqFillers = new List<interactionMate>();




        //this is HUGELY computationally wasteful, and the legibstrata should have a dictionary to EASILY look up ONLY the objects etc that can fulfill a SPECIFIC prereq!
        List<GameObject> allObjectsThatAreInteractable = theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable");

        foreach (GameObject thisObject in allObjectsThatAreInteractable)
        {
            Debug.Log("does this object fulfill prereq we are looking for?:  " + thisObject.name);
            //if (theWorldScript.theRespository.theInteractionEffects1.doesThisObjectHaveThisEffect(unfilledPrereq, thisObject) == true)
            testInteraction isThereAnInteraction = theWorldScript.theRespository.theInteractionEffects1.returnFirstInteractionOnObjectWithThisEffect(unfilledPrereq, thisObject);
            if (isThereAnInteraction != null)
            {
                Debug.Log("yes");

                //interactionMate
                interactionMate newMate = new interactionMate();
                newMate.interactionAuthor = this.gameObject;
                newMate.target1 = thisMate.target1;
                //newMate.fillsPrereqFor = unfilledPrereq;
                //Debug.Log("is this null???  " + thisMate.target1 + ", for this npc:  " + this.gameObject.name);
                newMate.thisIsTheDamnLock = thisMate.target1;
                newMate.enactThisInteraction = isThereAnInteraction;


                Debug.Log("WWWWWWWWWWWWWWWWWWWWWWWWWW       if this is to fill the lock prereq, which of these variables has the lock???       WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW");
                Debug.Log(thisMate.target1);
                Debug.Log(thisMate.forInteraction);
                Debug.Log(thisMate.immediateInteraction);
                Debug.Log(thisMate.enactThisInteraction);
                //newMate.thisIsTheDamnLock = ;





                //Debug.Log("WWWWWWWWWWWWWWWWWWWWWWWWWW       that's right, it' snot fucking null, is it???       WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW");
                //Debug.Log(isThereAnInteraction);
                //Debug.Log(isThereAnInteraction.name);
                //Debug.Log(isThereAnInteraction.atoms);



                prereqFillers.Add(newMate);

                //Debug.Log(prereqFillers);
                //Debug.Log(prereqFillers[0]);
                //Debug.Log(prereqFillers[0].enactThisInteraction);
                //Debug.Log(prereqFillers[0].enactThisInteraction.name);

                //foreach (interactionMate theInteractionMate in prereqFillers)
                {
                    //Debug.Log(prereq.name);
                    //Debug.Log("here is the [prereq filling] interaction mate author:  " + theInteractionMate.interactionAuthor.name + ", and the ''target1'':  " + theInteractionMate.target1.name);
                }

            }
            else
            {
                Debug.Log("says no");
            }

            //i think i don't need this now:
            if (true == false)
            {
                //iterate through their availableInteractions
                //      see if this interaction can fulfill the specific prereq
                //      if so, for now just return the FIRST one that works?
                foreach (testInteraction thisInteraction in thisObject.GetComponent<interactionEffects1>().interactionsAvailable)
                {
                    //modularPrereqCheckBool
                    //if (thisInteraction)


                    //if (theWorldScript.theRespository.theInteractionEffects1.doesThisObjectHaveThisEffect(thisInteraction, thisObject) == true)
                    {
                        //prereqFillers.Add(thisObject);

                    }
                }
            }
            
        }

        foreach (interactionMate theInteractionMate in prereqFillers)
        {
            //Debug.Log(prereq.name);
            Debug.Log("zzzzzzhere is the [prereq filling] interaction mate author:  " + theInteractionMate.interactionAuthor.name + ", and the ''target1'':  " + theInteractionMate.target1.name);
        }

        return prereqFillers;
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

    public void travelToTargetObject(GameObject target)
    {

        Vector3 targetVector = target.GetComponent<Transform>().position;
        travelToTargetPoint(targetVector);
    }

    public void travelToTargetPoint(Vector3 targetPoint)
    {
        thisNavMeshAgent.SetDestination(targetPoint);
    }

}
