using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class AI1 : MonoBehaviour
{

    
    //the NPC "state":
    public Dictionary<string, List<stateItem>> state = new Dictionary<string, List<stateItem>>();

    public List<action> toDoList = new List<action>();
    public List<action> knownActions = new List<action>();
    //public string locationState;
    
    //is this not used right now?  I'm using "recurringGoal" instead?
    public List<stateItem> goals = new List<stateItem>();


    //map
    public Dictionary<string, stateItem> map = new Dictionary<string, stateItem>();


    public stateItem recurringGoal = new stateItem();

    public functionsForAI theFunctions;// = GetComponent<functionsForAI>();
    //public stateForAI state;

    // Start is called before the first frame update
    void Start()
    {
        //this is my regular NPC, shoudl be in my AI1 file

        theFunctions = GetComponent<functionsForAI>();

    }




    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("it works");
        //Debug.Log(other);
        if (other.gameObject.tag == "aMapZone")
        {
            //print("it works");


            List<stateItem> newLocationList = new List<stateItem>();
            //Debug.Log(f);
            //print(this.transform.parent.name);
            //print("------------------start printing map--------------------");
            //mapPrinter(scriptX.map);
            //print("------------------done printing map--------------------");
            newLocationList.Add(map[other.gameObject.transform.parent.name]);
            state["locationState"] = newLocationList;

            //Component theScript = other.GetComponent("Script");
            //Debug.Log(theScript);  //returns Null, because there is no component called "script", the script is called "AI1"
            //.locationState = this.name;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "aMapZone")
        {
            //just blank out the locationState, I think:
            List<stateItem> newLocationList = new List<stateItem>();
            state["locationState"] = newLocationList;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //constantlyCheckLocationState();
        //theFunctions.printInventory(state["inventory"]);

        /*
        if (this.name == "NPC")
        {
            theFunctions.print("==============================================================");
        }
        */

        //make sure list isn't empty, remove completed action:
        if (toDoList.Count > 0)
        {
            //ad hoc for now, remove action if it is done
            if (theFunctions.isThisActionDone(toDoList[0], state))
            {
                toDoList.Remove(toDoList[0]);
            }
        }

        //remove plan if prereqs that should be filled already are not filled
        //will somehow need to tell which prereqs should already be filled
        //probably by adding up effects (using imagination simulation of plan), basically
        //but would be nice if it was simpler than that function...
        if (toDoList.Count > 0)
        {

            int Z;
            Z = theFunctions.findFirstImpossibleAction(toDoList, knownActions, state);


            if(Z != -2)
            {
                
                toDoList.RemoveRange(0, toDoList.Count);
                theFunctions.printPlan(toDoList);
            }



            /*

            while (Z != -2)
            {
                toDoList.RemoveAt(Z);
                Z = theFunctions.findFirstImpossibleAction(toDoList, knownActions, state);
            }

            


            List<List<action>> planListx = new List<List<action>>();
            planListx.Add(toDoList);

            //ad hoc for now
            if (theFunctions.simulatingPlansToEnsurePrereqs(planListx, knownActions, state).Count > 0)
            {
                //this will (stil) be true basically every frame for every NPC, though...

                //print("plan found");
                toDoList = planListx[0];
                theFunctions.print("is this a weird plan?11111111111111111111111111111111111111");
                theFunctions.printPlan(toDoList);
            }
            else
            {
                toDoList.RemoveRange(0, toDoList.Count);
            }

            

            //since "goTo" actions don't have prereqs, we need to skip past them to check if prereqs are failed:
            int actionToCheck;
            actionToCheck = 0;
            while (toDoList[actionToCheck].type == "goTo")
            {
                actionToCheck += 1;
            }


            
            //check if prereqs are failed:
            if (theFunctions.prereqChecker(toDoList[actionToCheck], state) == false)
            {
                //ok but now it deletes the plan EVERY FRAME, 
                //because the action after "goTo" ALWAYS has its prereqs unmet!
                //Because being at a location IS ONE OF THE PREREQS!
                //(thus, the above "if" statement is ALWAYS TRUE)
                toDoList.RemoveRange(0, toDoList.Count);
            }
            */
        }
        else
        {
            //ad-hoc adding the goal once it is completed, to create behavior loop
            if (state["feelings"].Count == 0)
            {
                state["feelings"].Add(recurringGoal);
            }
            //print("need to find a plan:");
            List<List<action>> planList = new List<List<action>>();
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
            //ad hoc for now
            if (planList.Count > 0)
            {
                //print("plan found");
                toDoList = planList[0];
            }
        }

        //printPlan(toDoList);

        
        //make sure list isn't empty AGAIN:
        if (toDoList.Count > 0)
        {
            theFunctions.doNextAction(toDoList[0], state);
        }
        //theFunctions.printState(state);

    }

}





