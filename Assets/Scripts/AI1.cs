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
    //public string locationState;
    
    //is this not used right now?  I'm using "recurringGoal" instead?
    public List<stateItem> goals = new List<stateItem>();


    //map [UNUSED NOW???]
    public Dictionary<string, stateItem> map = new Dictionary<string, stateItem>();


    //ad-hoc for now:
    public bool jobSeeking;
    public bool inConversation;

    public int clearanceLevel;



    public stateItem recurringGoal = new stateItem();

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


        

        
        if(inConversation == false)
        {
            //get NPC moving again i it was stopped by conversation:
            //but player has no navmesh agent, so need to check if it's null:
            if(theFunctions._navMeshAgent != null)
            {
                if (theFunctions._navMeshAgent.isStopped == true)
                {
                    theFunctions._navMeshAgent.isStopped = false;
                }
            }
            



            //remove plan if it is impossible
            //(it can become impossible if a necessary prereq that was previously met
            //unexpectedly becomes UNMET, and if the plan doesn't fill it)
            if (toDoList.Count > 0)
            {
                //FIRST, do sensing:
                if (target != null)
                {
                    theFunctions.sensing(toDoList[0], target, state);
                }


                //theFunctions.print("////////////////WHAT IS IMPOSSIBLE?????????????????????");
                //theFunctions.printPlan(toDoList);


                //remove all plans that contain "ineffective actions":
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

                            }
                        }
                    }
                }

                //NOW can remove the plans with ineffective actions:
                foreach (List<action> thisPlan in plansToRemove)
                {
                    //remove the plan from the planList
                    planList.Remove(thisPlan);
                }



                //Now find and remove all IMPOSSIBLE plans
                int Z;
                Z = theFunctions.findFirstImpossibleAction(toDoList, knownActions, state);


                if (Z != -2)
                {
                    //print("says this plan is imposible:");
                    //theFunctions.printPlan(toDoList);
                    toDoList.RemoveRange(0, toDoList.Count);
                    target = null;

                }
            }
            else
            {
                //well, if "toDoList" is of zero length, need a plan

                //choose next one from planList, unless planlist is "null" or empty:
                //so check if it's null or empty, fill it up if so:
                if (planList == null || planList.Count == 0)
                {
                    //need to make planList:


                    //print(recurringGoal.name);
                    planList = theFunctions.problemSolver(recurringGoal, knownActions, state);
                    //theFunctions.printPlan(planList[0]);
                    //print("state before imagination:");
                    //theFunctions.printState(state);
                    planList = theFunctions.simulatingPlansToEnsurePrereqs(planList, knownActions, state);
                    //print("the plan after imagination fix:");
                    //theFunctions.printPlan(planList[0]);
                    //print("state AFTER imagination:");


                    //theFunctions.printState(state);

                    //now to rank the plans by cost:
                    if (planList != null && planList.Count > 0)
                    {
                        planList = theFunctions.planRanker(planList);
                    }

                    //also, blank out the list of "ineffective actions":
                    if (ineffectiveActions != null && ineffectiveActions.Count > 0)
                    {
                        ineffectiveActions.Clear();
                    }

                }




                //ad-hoc adding the goal once it is completed, to create behavior loop
                //WHY IS THIS HERE????  SHOULDN'T IT BE SOMEWHERE ELSE???  maybe at the END, or START of update???
                if (state["feelings"].Count == 0)
                {
                    state["feelings"].Add(recurringGoal);
                }




                /////////////////////////////////////////////////
                //          POST-PLANNING PHASE
                /////////////////////////////////////////////////

                //now to rank the plans by cost:
                //(first check we have any palns)
                if (planList != null && planList.Count > 0)
                {



                    //now, choose first one:
                    toDoList = deepCopyFirstPlan(planList);
                    //and REMOVE that first one from the planList:
                    planList.RemoveAt(0);

                    //am I generating impossible plans???
                    //int Z;
                    //Z = theFunctions.findFirstImpossibleAction(toDoList, knownActions, state);

                }

            }

            //printPlan(toDoList);







            //theFunctions.print("yes hello, this is a conversation");
            //inConversation = false;

            //make sure list isn't empty AGAIN:
            if (toDoList.Count > 0)
            {

                target = theFunctions.doNextAction(toDoList[0], state, target, ineffectiveActions);



            }
        }
        else
        {
            //in conversation, need to stop:
            theFunctions._navMeshAgent.isStopped = true;
            //UnityEngine.AI.NavMeshAgent.Stop();
            
        }

        
        //theFunctions.printState(state);

        /*
        if (this.name == "NPC")
        {
            theFunctions.print("---------------------END OF UPDATE----------------------");
        }
        */
        
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
}





