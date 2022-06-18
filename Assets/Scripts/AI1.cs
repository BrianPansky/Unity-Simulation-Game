using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class AI1 : MonoBehaviour
{

    
    //the NPC "state":
    public Dictionary<string, List<stateItem>> state = new Dictionary<string, List<stateItem>>();

    public GameObject target;
    public GameObject roleLocation;

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
    public taggedWith thisIsTaggedWith;

    // Start is called before the first frame update
    void Start()
    {
        //get some other scripts I'll need:
        theFunctions = GetComponent<functionsForAI>();
        thisIsTaggedWith = GetComponent<taggedWith>();

        //add a "person" tag to this agent:
        thisIsTaggedWith.addTag("person");
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

            //theFunctions.print(other.gameObject.transform.parent.name);
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
            theFunctions.print("==================================================");
            theFunctions.printInventory(state["inventory"]);
            if (toDoList.Count > 0)
            {
                theFunctions.print(toDoList[0].name);
            }
        }
        */

        /*
        if (this.name == "NPC shopkeeper")
        {
            theFunctions.print("==================================================");
            theFunctions.printPlan(toDoList);
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

        
        //remove plan if it is impossible
        //(it can become impossible if a necessary prereq that was previously met
        //unexpectedly becomes UNMET, and if the plan doesn't fill it)
        if (toDoList.Count > 0)
        {

            int Z;
            Z = theFunctions.findFirstImpossibleAction(toDoList, knownActions, state);


            if(Z != -2)
            {
                
                toDoList.RemoveRange(0, toDoList.Count);
                //theFunctions.printPlan(toDoList);
                target = null;
            }
        }
        else
        {
            //well, if "toDoList" is of zero length, need to try to make a plan

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
            
            target = theFunctions.doNextAction(toDoList[0], state, target);

            
            
        }
        //theFunctions.printState(state);


        
    }

}





