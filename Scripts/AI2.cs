using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class AI2 : MonoBehaviour
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
        // this is my shopkeeper creation, should be in my AI2 file
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

    // Update is called once per frame
    void Update()
    {
        //constantlyCheckLocationState();
        
        //make sure list isn't empty:
        if (toDoList.Count > 0)
        {

            //ad hoc for now
            //Debug.Log("are we CHECKING???");
            //theFunctions.printState(state);
            if (theFunctions.isThisActionDone(toDoList[0], state))
            {
                //Debug.Log("yes good, on we go!!!!!!!!!!!!!!!!!!");
                toDoList.Remove(toDoList[0]);
            }
            //print("and how about here?.................................................................................................................................................................................................................................................");
        }
        else
        {
            if (state["feelings"].Count == 0)
            {
                //print("heloooooooooooooooooooooo");
                state["feelings"].Add(recurringGoal);
            }
            //print("need to find a plan:");
            List<List<action>> planList = new List<List<action>>();
            
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





