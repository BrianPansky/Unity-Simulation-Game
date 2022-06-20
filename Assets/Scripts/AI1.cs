using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class AI1 : MonoBehaviour
{

    
    //the NPC "state":
    public Dictionary<string, List<stateItem>> state = new Dictionary<string, List<stateItem>>();
    public Dictionary<string, List<stateItem>> planningState = new Dictionary<string, List<stateItem>>();
    public Dictionary<string, List<stateItem>> factionState = new Dictionary<string, List<stateItem>>();

    public GameObject target;

    public job currentJob;


    public GameObject roleLocation;
    public GameObject homeLocation;
    public GameObject leader;

    public List<action> toDoList = new List<action>();
    public List<List<action>> planList = new List<List<action>>();


    public List<action> inputtedToDoList = new List<action>();
    //eventually might want:
    //public List<List<action>> inputtedPlanList = new List<List<action>>();

    List<action> ineffectiveActions = new List<action>();

    


    //wait do I use this:
    public List<action> knownActions = new List<action>();
    
    //is this not used right now?  I'm using "recurringGoal" instead?
    public List<actionItem> goals = new List<actionItem>();


    //diagnostic
    public bool masterPrintControl;
    


    //ad-hoc for now:
    public bool jobSeeking;
    public bool inConversation;
    public bool atWork;

    public int clearanceLevel;



    public actionItem recurringGoal = new actionItem();

    public functionsForAI theFunctions;// = GetComponent<functionsForAI>();
    //public stateForAI state;
    public taggedWith taggedWith;
    public social social;

    //for easy debug printing
    public string npcx;


    //ad-hoc
    //this is so i can use this script on stuff like storage containers [which have an inventory] even though stuff won't work normally like AI
    //for these abnormal things, edit in in Unity side panel to be true
    //otherwise, i'll try to make it false in "start" function
    public bool ignore;
    public int goalWait;

    void Awake()
    {

        //get some other scripts I'll need:
        theFunctions = GetComponent<functionsForAI>();
        taggedWith = GetComponent<taggedWith>();
        social = GetComponent<social>();

        //need to initialize, don't want faction inventory etc. to be null/non-existent
        factionState = createEmptyFactionState();
    }

    // Start is called before the first frame update
    void Start()
    {
        //should move this elsewhere?
        if(this.name == "Player")
        {
            ignore = true;
        }
        //ad-hoc
        //this is so i can use this script on stuff like storage containers or PLAYER [which have an inventory] even though stuff won't work normally like AI
        //for these abnormal things, edit in in Unity side panel to be true
        //otherwise, i'll try to make it false in this "start" function
        if(ignore != true)
        {
            ignore = false;
        }

        atWork = false;




        //add a "person" tag to this agent:
        //ad hoc way to prevent storage containters from being called people:  check for the npc tag:
        if(this.gameObject.tag == "anNPC")
        {
            taggedWith.addTag("person");
        }
        
        

        //ad-hoc for now:
        jobSeeking = true;
        inConversation = false;

        homeLocation = null;

        clearanceLevel = 0;

        goalWait = 0;



        //for easy debug printing
        npcx = "NPC pickpocket";
        //npcx = "NPC";
        //diagnostic
        masterPrintControl = false;


        //i think this should work?
        planningState = theFunctions.deepStateCopyer(state);

        //theFunctions.printKnownActionsDeeply(knownActions);




        //Debug.Log(this.gameObject.name);
        //taggedWith.printAllTags();
    }



    // Update is called once per frame
    void Update()
    {
        
        //theFunctions.print("\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");

        //"ignore means this is not an AI, it doesn't DO anything.  just uses this script for inventory.  maybe a dumb idea...
        if (ignore == false)
        {
            //now handle the checking stuff.  like checking to pay PLAYER for work shift.  just JOB stuff for now i guess:
            checkJobs();

            if (inConversation == false)
            {

                //theFunctions.print("TESTTTTTTTTTTxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx!");

                //get NPC moving again if it was stopped by conversation:
                getGoingAgan();

                //printPlanListForSpecificNPC();

                //fine time to re-fill goals, I guess...
                refillGoals();

                //fine time to do sensing, I guess...
                doSensing();
                //remove all plans that contain "ineffective actions":
                removeIneffectiveActions();

                //theFunctions.print("TESTTTTTTTTTTzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz!");

                if (inputtedToDoList.Count > 0)
                {
                    //for now, do orders/favors and such first
                    //theFunctions.print("TESTTTTTTTTTT444444444444444444444444444444444444444444444!");

                    //NEED TO BLANK TARGET!
                    target = null;

                    getPlanToObeyOrders();

                    //printPlanList(planList);

                    //now, pick top-ranked plan (if there are any)
                    makeFirstPlanTheToDoList();
                    //printToDoList(toDoList);
                }
                else if (toDoList.Count == 0)
                {
                    //theFunctions.print("TESTTTTTTTTTT!!!!!!55555555555555555555555555555555555555555!!!");
                    //so we need a plan:
                    getPlan();

                    if (this.name == npcx)
                    {
                        //theFunctions.print("here are plans:");
                        //printPlanListForSpecificNPC();
                        //printPlanList(planList);
                        //printToDoList(toDoList);

                        //theFunctions.print("here was goal:");
                        //theFunctions.print(recurringGoal.name);
                        //theFunctions.print("state AFTER getting plan:");
                        //printPlanListForSpecificNPC();
                        //theFunctions.printState(state);

                    }
                    //printPlanListForSpecificNPC();
                    //printToDoListForSpecificNPC();


                    //now, pick top-ranked plan (if there are any)
                    makeFirstPlanTheToDoList();
                    //printToDoList(toDoList);

                }

                //printToDoList(toDoList);

                //printToDoListForSpecificNPC();
                //doing the to-do list (checks if it's not zero length):

                //theFunctions.print("TESTTTTTTTTTT before handle action");
                //printToDoList(toDoList);
                handleAnyNextAction();
                //printToDoList(toDoList);

                //theFunctions.print("TESTTTTTTTTTT22222222222222222222222222222222222");
                //masterPrintControl = false;
            }
            else
            {
                //this "else" means we are in conversation, need to stop:
                theFunctions._navMeshAgent.isStopped = true;
                //UnityEngine.AI.NavMeshAgent.Stop();
                

            }

            //theFunctions.print("TESTTTTTTTTTT!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //masterPrintControl = false;
        }


        //theFunctions.print(theFunctions.actionToTextDeep(theFunctions.premadeStuff.workAsCashier));
    }

    ////////////////    Stuff for the Update function:    ////////////////

    public void checkJobs()
    {
        //helps check if PLAYER has done a job shift.
        //probably just calls a more complex function over in functionsForAI to do it, really.

        theFunctions.jobCheckFunction();

    }

    public void getGoingAgan()
    {
        //get NPC moving again if it was stopped by conversation.


        //but player has no navmesh agent, so need to check if it's null:
        if (theFunctions._navMeshAgent != null)
        {
            if (theFunctions._navMeshAgent.isStopped == true)
            {
                theFunctions._navMeshAgent.isStopped = false;
            }
        }
    }
            
    public void doSensing()
    {
        //hmm, I can't do SENSING if target is null?!?!?
        //sounds like that could be a problem...why is that???
        //will have to check...see if I can fix it...
        //if (target != null && )
        
        theFunctions.sensing(state);
        
    }
    
    public void removeIneffectiveActions()
    {
        //THIS DOES GET CALLED EVERY FRAME
        //BUT THAT DOESN'T MEAN THERE ARE ANY INEFFECTIVE ACTIONS EVERY FRAME
        //IT IS JUST CHECKIGN IF THERE ARE

        //using our global variables ("ineffectiveActions" and "planList"
        //check ALL plans, remove that plan if it has ANY ineffective actions

        //presumably never has to check to-do list?
        
        //doesn't need to check if lists are empty, because "foreach" handles that gracefully???

        //first have a list of all plans to remove, because it's bad to modify a list while iterating over it in effing C# apparently
        List<List<action>> plansToRemove = new List<List<action>>();
        foreach (action thisIneffectiveAction in ineffectiveActions)
        {
            if (this.name == npcx)
            {
                theFunctions.print("11111111111111111there is an ineffective action to remove:");
                //printPlanListForSpecificNPC();
                //printPlanList(planList);
                //printToDoList(toDoList);

                theFunctions.print(thisIneffectiveAction.name);
                //theFunctions.print(recurringGoal.name);


            }
            //go through each plan
            foreach (List<action> thisPlan in planList)
            {
                //check each ACTION in this plan:
                foreach (action thisAction in thisPlan)
                {
                    //checked if this is the ineffective action:
                    if (thisAction.name == thisIneffectiveAction.name)
                    {
                        //if so, add this plan to list of plans to remove:
                        plansToRemove.Add(thisPlan);

                        //then break, so it doesn't do this plan again?
                        break;

                    }
                }
            }
        }

        //NOW can remove the plans with ineffective actions:
        //but what if the plan has been added to the list of plansToRemove TWICE???
        //I tried adding "break" above, hopefully that's the correct way to avoid duplicates...
        foreach (List<action> thisPlan in plansToRemove)
        {
            //remove the plan from the planList
            planList.Remove(thisPlan);

            if (this.name == npcx)
            {
                //NPC shopkeeper
                //theFunctions.print("this planList after removing ineffective plan:");
                //printPlanListForSpecificNPC();

            }
        }

        

        
    }

    public void blankImpossibleToDoList()
    {
        //theFunctions.print("=============================START check=====================================");
        //only checks CURRENT to-do list.
        //blanks it out if it has ANY impossible action.
        if (this.name == npcx)
        {
            //theFunctions.print("=============================START blankImpossibleToDoList check=====================================");

            //theFunctions.print(thisIneffectiveAction.name);
            //theFunctions.print(recurringGoal.name);
            //theFunctions.printPlanWithQuantities(toDoList);

        }
        
        //my checking function returns an index integer
        //why am I doing it this way?  What if there's MORE THAN ONE impossible action?
        //very weird, need to re-write this somehow...
        int Z;
        //returns the index number of the first action on that list that CANNOT be completed
        Z = theFunctions.findFirstImpossibleAction(toDoList, knownActions, state);
        //if all actions can be completed fine, it returns negative two

        //theFunctions.print(Z.ToString());
        
        if (Z != -2)
        {
            if (this.name == npcx)
            {
                theFunctions.print("22222222222222222there is an impossible toDoList to blank out");

                //printPlanListForSpecificNPC();
                //printPlanList(planList);
                //printToDoList(toDoList);
                


            }

            //so, remove the ENTIRE to-do list:
            /*
            if (this.name npcx)
            {
                theFunctions.print("00000000000000000  to do List:   000000000000000000");
                theFunctions.printPlan(toDoList);
                if (target != null)
                {
                    theFunctions.print("00000000000000000  TARGET:   000000000000000000");
                    theFunctions.print(target.name);
                }
                else
                {
                    theFunctions.print("TARGET IS NULLLLLLLLL");
                }

            }
            */

            theFunctions.print("says this plan is imposible:");
            theFunctions.printPlan(toDoList);
            theFunctions.printState(state);
            theFunctions.printState(factionState);
            theFunctions.printState(planningState);
            toDoList.RemoveRange(0, toDoList.Count);
            target = null;
            
        }
        if (this.name == npcx)
        {
            //theFunctions.print("========================================END blankImpossibleToDoList check=============================");

            //theFunctions.print(thisIneffectiveAction.name);
            //theFunctions.print(recurringGoal.name);


        }
        //theFunctions.print("========================================END check=============================");
    }

    public void getPlan()
    {
        target = null;
        //choose next one from planList, unless planlist is "null" or empty:
        //so check if it's null or empty, fill it up if so:
        if (planList == null || planList.Count == 0)
        {

            //theFunctions.printInventoryDeep(state["inventory"]);
            //theFunctions.printInventoryDeep(planningState["inventory"]);

            //masterPrintControl = false;


            //need to make planList:
            planList = theFunctions.planningPhase(recurringGoal, knownActions, planningState);

            //masterPrintControl = false;

            //printPlanListForSpecificNPC();
            //theFunctions.printState(state);
            //sometimes at this moment, there are zero plans?  but not always?
            //masterPrintControl = false;

            //masterPrintControl = true;
            //printPlanListForSpecificNPC();
            //masterPrintControl = false;

            planList = theFunctions.simulatingPlansToEnsurePrereqs(planList, knownActions, planningState, 20);

            //masterPrintControl = true;
            //printPlanListForSpecificNPC();
            //masterPrintControl = false;


            //also, blank out the list of "ineffective actions":
            //[I think this code could/should be moved elsewhere...]
            clearIneffectiveActions();
            //printPlanListForSpecificNPC();

            //why is this HERE??!?!?!?
            atWork = false;

        }
        else
        {
            theFunctions.print("so, no to-do-list for some reason, but nonzero planlist");
        }
        

    }

    public void getPlanToObeyOrders()
    {


        //theFunctions.print("is it this????????????????????????????");

        List<List<action>> newPlanList1 = new List<List<action>>();

        //should really check if output is null?  or does it do something else if it fails?  give blank?
        planList = theFunctions.simulateOnePlanFillPrereqs(inputtedToDoList, knownActions, planningState, 200);
        


        //....remove that action from "inputtedToDoList"?  I geuss for now
        //but eventually want to keep it remembered UNTIL it is completed or cancelled
        //but right now it isn't guaranteed that one of those outcomes will occurr
        inputtedToDoList.RemoveAt(0);
        //print("so, this is broken???");
        //printPlanList(planList);
    }

    public void makeFirstPlanTheToDoList()
    {
        if (planList != null && planList.Count > 0)
        {
            //now to rank the plans by cost:
            //[yes, it will re-do this redundantly every time it needs to 
            //choose a different plan from planList when toDoList is blank.  
            //But for now, easy enough to do it here.  Can re-arrange later.  
            //Used to have it right after generating new plans, but i want it to work
            //even if the planList is handed over by an outside entity, like the player
            planList = theFunctions.planRanker(planList);


            //theFunctions.print("this planList, after ranking plans");
            //printPlanListForSpecificNPC();

            //choose first one:
            toDoList = deepCopyFirstPlan(planList);
            //and REMOVE that first one from the planList:
            planList.RemoveAt(0);

            //printToDoListForSpecificNPC();

        }
    }

    public void handleAnyNextAction()
    {
        //theFunctions.print(theFunctions.actionToTextDeep(theFunctions.premadeStuff.workAsCashier));
        if (this.name == npcx)
        {
            //theFunctions.print("11111111111111111111111111111111111111");
            //theFunctions.printKnownActionsDeeply(knownActions);
            //theFunctions.print(thisIneffectiveAction.name);
            //theFunctions.print(recurringGoal.name);


        }
        //theFunctions.print(theFunctions.actionToTextDeep(theFunctions.premadeStuff.workAsCashier));
        //first, blank toDoList if it is impossible:
        blankImpossibleToDoList();
        //theFunctions.print(theFunctions.actionToTextDeep(theFunctions.premadeStuff.workAsCashier));
        if (this.name == npcx)
        {
            //theFunctions.print("22222222222222222222222222222222222222222");
            //theFunctions.printKnownActionsDeeply(knownActions);
            //theFunctions.print(thisIneffectiveAction.name);
            //theFunctions.print(recurringGoal.name);


        }
        //make sure list isn't empty AGAIN:
        if (toDoList.Count > 0)
        {
            //but don't do the action if it is already done
            //if it's done, remove it:
            if (checkIfActionIsNeeded(toDoList[0], state) == true)
            {
                if (goalWait < 1)
                {
                    //theFunctions.print(theFunctions.actionToTextDeep(theFunctions.premadeStuff.workAsCashier));
                    //theFunctions.printKnownActionsDeeply(knownActions);
                    //theFunctions.print(theFunctions.actionToTextDeep(toDoList[0]));
                    target = theFunctions.doNextAction(toDoList[0], state, target, ineffectiveActions);
                    //theFunctions.printKnownActionsDeeply(knownActions);
                    //theFunctions.print(theFunctions.actionToTextDeep(theFunctions.premadeStuff.workAsCashier));
                }
                else
                {
                    goalWait -= 1;
                }
                
            }
            else
            {
                //this "else" means the nextAction is redundant, already done.
                //so dump the action:
                print("dddddddddddddddduuuuuuuuuuuuuuummmmmmmmmmmmmmmmmmmmpppppppppppppp");
                target = theFunctions.dumpAction(target);
                goalWait = 0; //SHOULD INCORPORATE INTO "dumpAction"????
            }



            //masterPrintControl = false;
        }
    }
            
    public void refillGoals()
    {
        if (state["feelings"].Count == 0)
        {
            if (recurringGoal.inStateOrNot == false)
            {
                //maybe use a less ad-hoc function here so it is modified correctly any time i modify normal state changer functions
                //should use a REAL "incrementItem" thing...???
                state["feelings"].Add(theFunctions.deepStateItemCopier(recurringGoal.item));
                planningState["feelings"].Add(theFunctions.deepStateItemCopier(recurringGoal.item));
            }
            else
            {
                theFunctions.print("need a way to handle WANTED feeling goals, easy enough to check if actionItem goal is accomplished in state");
            }
            
        }
    }
                
    public void clearIneffectiveActions()
    {
        if (ineffectiveActions != null && ineffectiveActions.Count > 0)
        {
            ineffectiveActions.Clear();
        }
    }
            


    /////////////////////////////////////

    public void printPlanList(List<List<action>> planList)
    {
        if (planList == null)
        {
            theFunctions.print("this planList is null");
        }
        else
        {
            theFunctions.print("this should be planList:");
            theFunctions.print(theFunctions.planListToText(planList));
        }
        
    }

    public void printToDoList(List<action> toDoList)
    {
        if (toDoList == null)
        {
            theFunctions.print("this toDoList is null");
        }
        else
        {
            theFunctions.print("this should be the toDoList:");
            theFunctions.print(theFunctions.planToText(toDoList));
        }
    }

    public List<action> deepCopyPlan(List<action> plan)
    {
        List<action> thisDeepCopy = new List<action>();

        foreach (action thisAction in plan)
        {
            thisDeepCopy.Add(thisAction);
        }

        return thisDeepCopy;
    }

    public List<action> deepCopyFirstPlan(List<List<action>> planList)
    {
        return deepCopyPlan(planList[0]);
    }

    public bool checkIfEffectsAreDone(action thisAction, Dictionary<string, List<stateItem>> state)
    {
        //used to check if an action is redundant, if it's done already.
        //this is similar to the funciton that checks prereqs.  Could probably use that fact to cut down on duplicate code...


        //assume true, then check and change to false where needed
        bool tf;
        tf = true;
        

        foreach (actionItem effectX in thisAction.effects)
        {
            
            if (theFunctions.isStateAccomplished(effectX, state) == false)
            {

                return false;
            }
        }

        return tf;

    }

    public bool checkIfActionIsNeeded(action thisAction, Dictionary<string, List<stateItem>> state)
    {
        //NOT A PROPER TEST OF FULL PLAN
        //ALSO NOT A PROPER TEST IF THERE IS ONLY ONE ACTION
        //[SEE DEVJOURNAL "why don't NPCs dump ''redundant'' orders by plyer"
        //ASSUMES THE ACTION IS THE FIRST ACTION IN THE "toDoList" VARIABLE!
        //used to check if an action is redundant, if it's done already.
        //this is similar to the funciton that checks prereqs.  Could probably use that fact to cut down on duplicate code...


        //assume true, then check and change to false where needed
        bool tf;
        tf = true;  //"true" means "meeded", false means redundant.

        if(checkIfEffectsAreDone(thisAction, state) == true)
        {
            //action SEEMS done, but if it's still necessary
            //[such as if a quantity greater than 1 is needed]
            //it might still not be redundant
            //so, check if removing it causes plan to be impossible

            //copy plan
            //remove 1st action
            //if this new plan is impossible, then the action MAY NOT be considered redundant
            //(full unmodified plan gets checked for impossibility elsewhere)
            List<action> thisDeepCopy = new List<action>();
            thisDeepCopy = deepCopyPlan(toDoList);
            thisDeepCopy.RemoveAt(0);

            if(theFunctions.findFirstImpossibleAction(thisDeepCopy, knownActions, state) == -2)
            {
                //thus, no problem with this shorter plan
                //thus, the longer plan is redundant
                tf = false;  //"true" means "meeded", false means redundant.
            }
        }


        return tf;

    }


    public void printThisInventory()
    {

        if (this.name == npcx)
        {
            theFunctions.print("==================================================");
            theFunctions.printInventory(state["inventory"]);

            
        }
    }

    public void printPlanListForSpecificNPC()
    {
        //List<List<action>>


        //to help, encapsulating this:

        if (this.name == npcx)
        {
            //NPC shopkeeper
            theFunctions.print("00000000000000000  Plan List:   000000000000000000");
            printPlanList(planList);
        }




        /*
            if (this.name == npcx)
            {
                theFunctions.print("00000000000000000  Plan List:   000000000000000000");
                printPlanList(planList);
            }

            //constantlyCheckLocationState();
            //theFunctions.printInventory(state["inventory"]);


            if (this.name == npcx)
            {
                theFunctions.print("==================================================");
                theFunctions.printInventory(state["inventory"]);


                if (toDoList.Count > 0)
                {
                    theFunctions.print(toDoList[0].name);
                }

            }
            */

        /*
        //if (this.name npcx)
        if (this.name npcx)
        {
            theFunctions.print("==================================================");
            theFunctions.printPlan(toDoList);
        }
        */


        /*
        //make sure list isn't empty, remove completed action:
        if (toDoList.Count > 0)
        {
            //ad hoc for now, remove action if it is done
            if (theFunctions.isThisActionDone(toDoList[0], state))
            {
                toDoList.Remove(toDoList[0]);
            }
        }
        */

        //theFunctions.printState(state);
        //printPlan(toDoList);
        /*
        if (this.name npcx)
        {
            theFunctions.print("---------------------END OF UPDATE----------------------");
        }
        */
    }

    public void printToDoListForSpecificNPC()
    {
        //theFunctions.printKnownActionsDeeply(knownActions);
        //theFunctions.printPlanWithQuantities(planList[0]);
        if (this.name == npcx)
        {
            if (toDoList == null)
            {
                theFunctions.print("this toDoList is null");
            }
            else
            {
                theFunctions.print("this should be the toDoList:");
                theFunctions.print(theFunctions.planToText(toDoList));
            }
        }
        
    }


    public void stateDiagnosis(Dictionary<string, List<stateItem>> state)
    {
        //checks for a specified item and quantity in a state dictionary
        //eventually make the item/quantity into an input?
        //for now, manually...

        string theItemName = "money";
        int quant = -3;

        if(this.name == npcx)
        {

            //theFunctions.printState(planningState);
            //print("...1111111111111111111");
            //theFunctions.printStateItemList(state["inventory"]);
            //print("...22222222222222222222");

            foreach (stateItem item in state["inventory"])
            {
                if (item.name == theItemName && item.quantity == quant)
                {
                    theFunctions.print("now!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    masterPrintControl = false;
                }
            }
        }

    }


    public Dictionary<string, List<stateItem>> createEmptyFactionState()
    {
        Dictionary<string, List<stateItem>> newFactionState = new Dictionary<string, List<stateItem>>();
        //List<stateItem> feelings = new List<stateItem>();
        List<stateItem> inventory = new List<stateItem>();
        //List<stateItem> locationState = new List<stateItem>();
        //List<stateItem> organizationState = new List<stateItem>();
        List<stateItem> unitState = new List<stateItem>();
        List<stateItem> propertyState = new List<stateItem>();
        List<stateItem> threatState = new List<stateItem>();


        //newFactionState.Add("locationState", locationState);
        //newFactionState.Add("feelings", feelings);
        newFactionState.Add("inventory", inventory);
        //newFactionState.Add("organizationState", organizationState);
        newFactionState.Add("unitState", unitState);
        newFactionState.Add("property", propertyState);
        newFactionState.Add("threatState", threatState);



        return newFactionState;
    }

}
