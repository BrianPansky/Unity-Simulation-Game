using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class AI1 : MonoBehaviour
{

    
    //the NPC "state":
    public Dictionary<string, List<stateItem>> state = new Dictionary<string, List<stateItem>>();

    public GameObject target;

    public GameObject roleLocation;
    public GameObject homeLocation;

    public List<action> toDoList = new List<action>();
    public List<List<action>> planList = new List<List<action>>();

    List<action> ineffectiveActions = new List<action>();

    


    //wait do I use this:
    public List<action> knownActions = new List<action>();
    
    //is this not used right now?  I'm using "recurringGoal" instead?
    public List<actionItem> goals = new List<actionItem>();


    //map [UNUSED NOW???]
    public Dictionary<string, stateItem> map = new Dictionary<string, stateItem>();


    //ad-hoc for now:
    public bool jobSeeking;
    public bool inConversation;

    public int clearanceLevel;



    public actionItem recurringGoal = new actionItem();

    public functionsForAI theFunctions;// = GetComponent<functionsForAI>();
    //public stateForAI state;
    public taggedWith thisIsTaggedWith;

    // Start is called before the first frame update
    void Start()
    {
        //get some other scripts I'll need:
        theFunctions = GetComponent<functionsForAI>();
        thisIsTaggedWith = GetComponent<taggedWith>();

        //add a "person" tag to this agent:
        thisIsTaggedWith.addTag("person");

        //ad-hoc for now:
        jobSeeking = true;
        inConversation = false;

        homeLocation = null;

        clearanceLevel = 0;
        
}

    

    // Update is called once per frame
    void Update()
    {
        //diagnostic stuff I use sometimes:
        {

            /*
            if (this.name == "NPC pickpocket")
            {
                theFunctions.print("00000000000000000  Plan List:   000000000000000000");
                printPlanList(planList);
            }

            //constantlyCheckLocationState();
            //theFunctions.printInventory(state["inventory"]);


            if (this.name == "NPC")
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
            //if (this.name == "NPC pickpocket")
            if (this.name == "NPC shopkeeper")
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
            if (this.name == "NPC")
            {
                theFunctions.print("---------------------END OF UPDATE----------------------");
            }
            */
        }


        if (inConversation == false)
        {
            //get NPC moving again if it was stopped by conversation:
            getGoingAgan();

            //fine time to re-fill goals, I guess...
            refillGoals();

            //fine time to do sensing, I guess...
            doSensing();

            //remove all plans that contain "ineffective actions":
            removeIneffectiveActions();

            if (toDoList.Count > 0)
            {
                //blank to-do list if it is impossible:
                blankImpossibleToDoList();
            }
            else
            {
                //this "else" means the "toDoList" is of zero length, so we need a plan:
                getPlan();
                
            }
            
            //doing the to-do list (checks if it's not zero length):
            doToDoList();
        }
        else
        {
            //this "else" means we are in conversation, need to stop:
            theFunctions._navMeshAgent.isStopped = true;
            //UnityEngine.AI.NavMeshAgent.Stop();
            
        }

        
    }

    ////////////////////////////////////////////////////
    //       Stuff for the Update function:
    ////////////////////////////////////////////////////

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
        if (target != null)
        {
            theFunctions.sensing(toDoList[0], target, state);
        }
    }
    
    public void removeIneffectiveActions()
    {
        //using our global variables ("ineffectiveActions" and "planList"
        //check ALL plans, remove that plan if it has ANY ineffective actions

        //presumably never has to check to-do list?

        //doesn't need to check if lists are empty, because "foreach" handles that gracefully???

        //first have a list of all plans to remove, because it's bad to modify a list while iterating over it in effing C# apparently
        List<List<action>> plansToRemove = new List<List<action>>();
        foreach (action thisIneffectiveAction in ineffectiveActions)
        {
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
        }
    }

    public void blankImpossibleToDoList()
    {
        //only checks CURRENT to-do list.
        //blanks it out if it has ANY impossible action.

        //my checking function returns an index integer
        //why am I doing it this way?  What if there's MORE THAN ONE impossible action?
        //very weird, need to re-write this somehow...
        int Z;
        Z = theFunctions.findFirstImpossibleAction(toDoList, knownActions, state);

        
        if (Z != -2)
        {
            //so, remove the ENTIRE to-do list:

            //print("says this plan is imposible:");
            //theFunctions.printPlan(toDoList);
            toDoList.RemoveRange(0, toDoList.Count);
            target = null;

        }
    }

    public void getPlan()
    {
        //choose next one from planList, unless planlist is "null" or empty:
        //so check if it's null or empty, fill it up if so:
        if (planList == null || planList.Count == 0)
        {
            //need to make planList:
            
            planList = theFunctions.problemSolver(recurringGoal, knownActions, state);
            planList = theFunctions.simulatingPlansToEnsurePrereqs(planList, knownActions, state);

            //now to rank the plans by cost:
            if (planList != null && planList.Count > 0)
            {
                planList = theFunctions.planRanker(planList);
            }

            //also, blank out the list of "ineffective actions":
            //[I think this code could/should be moved elsewhere...]
            clearIneffectiveActions();

        }



        
        //now, pick top-ranked plan (if there are any)
        if (planList != null && planList.Count > 0)
        {
            //choose first one:
            toDoList = deepCopyFirstPlan(planList);
            //and REMOVE that first one from the planList:
            planList.RemoveAt(0);
            
        }

    }

    public void doToDoList()
    {
        //make sure list isn't empty AGAIN:
        if (toDoList.Count > 0)
        {
            //but don't do the action if it is already done
            //if it's done, remove it:
            if (checkIfEffectsAreDone(toDoList[0], state) == false)
            {
                target = theFunctions.doNextAction(toDoList[0], state, target, ineffectiveActions);
            }
            else
            {
                //this "else" means the nextAction is redundant, already done.
                //so dump the action:
                target = theFunctions.dumpAction(target);
            }



        }
    }
            
    public void refillGoals()
    {
        if (state["feelings"].Count == 0)
        {
            if (recurringGoal.inStateOrNot == false)
            {
                state["feelings"].Add(recurringGoal.item);
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
            theFunctions.print(theFunctions.planListToText(planList));
        }
        
    }

    public List<action> deepCopyFirstPlan(List<List<action>> planList)
    {
        List<action> thisDeepCopy = new List<action>();

        foreach(action thisAction in planList[0])
        {
            thisDeepCopy.Add(thisAction);
        }

        return thisDeepCopy;
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
}





