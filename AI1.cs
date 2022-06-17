using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI1 : MonoBehaviour
{

    //the NPC "state":
    public Dictionary<string, List<stateItem>> state = new Dictionary<string, List<stateItem>>();// { { "locationState", {{ }} }, { 2, "World" }, { 2, "World" } };
    
    public List<action> toDoList = new List<action>();
    public List<action> knownActions = new List<action>();
    //public string locationState;
    public List<stateItem> locationState = new List<stateItem>();
    public List<stateItem> feelings = new List<stateItem>();
    public List<stateItem> inventory = new List<stateItem>();
    public List<stateItem> goals = new List<stateItem>();


    //map
    public Dictionary<string, stateItem> map = new Dictionary<string, stateItem>();



    private float speed = 10;
    private GameObject w;

    public stateItem work1 = new stateItem();
    public action goToWork = new action();

    public stateItem store1 = new stateItem();
    public action goToStore = new action();

    public stateItem home1 = new stateItem();
    public action goToHome = new action();

    bool isThisActionDone(action thisAction)
    {
        //assume true, then check and change to false where needed
        bool tf;
        tf = true;

        foreach (stateItem effectX in thisAction.effects)
        {
            if (isGoalAccomplished(effectX) == false)
            {
                return false;
            }
        }

        return tf;
    }

    bool isGoalAccomplished(stateItem goal)
    {
        //assume false, then check and change to true where needed
        bool tf;
        tf = false;

        if (goal.inStateOrNot == true)
        {
            foreach (stateItem stateI in state[goal.stateCategory])
            {


                //////////////here's the C# way to say "if this item is in this list"...ya, i twon't let you use the word "in" here:
                //oh no this is text, "stateItemName" needs to be just a stateItem!  Or something!
                if (stateI.stateItemName == goal.stateItemName)
                {
                    tf = true;
                }
            }
        }
        if (goal.inStateOrNot == false)
        {
            foreach (stateItem stateI in state[goal.stateCategory])
            {
                //actually here I have to reverse it?
                //assume false, then if I find it, change to true?
                //ya I had to do that in my C++ code too
                tf = true;

                if (stateI.stateItemName == goal.stateItemName)
                {
                    tf = false;
                }
            }
        }
        return tf;
    }

    bool prereqChecker(action thisAction)
    {
        //assume true, then check and change to false where needed
        bool tf;
        tf = true;

        foreach (stateItem prereqX in thisAction.prereqs)
        {
            if (isGoalAccomplished(prereqX) == false)
            {
                return false;
            }
        }

        return tf;
    }
    void constantlyCheckLocationState()
    {

    }

    void doNextAction(action nextAction)
    {
        if (nextAction.type == "goTo")
        {
            
            //Debug.Log("hello this is where one thing is printing");
            //Debug.Log(nextAction.effects[0]);
            //Debug.Log("done printing");
            stateItem stateItemX = nextAction.effects[0];
            string name1 = stateItemX.stateItemName;
            w = GameObject.Find(name1);
            transform.position = Vector3.MoveTowards(transform.position, w.GetComponent<Transform>().position, speed * Time.deltaTime);
        }
    }


    List<List<action>> problemSolver(stateItem goal, List<action> knownActions, Dictionary<string, List<stateItem>> state)
    {
        List<List<action>> planList = new List<List<action>>();

        if (isGoalAccomplished(goal) == false)
        {
            foreach(action thisAction in knownActions)
            {
                foreach(stateItem thisEffect in thisAction.effects)
                {
                    if (goal.stateItemName == thisEffect.stateItemName & goal.inStateOrNot == thisEffect.inStateOrNot)
                    {
                        if (prereqChecker(thisAction))
                        {
                            List<action> shortPlan = new List<action>();
                            shortPlan.Add(thisAction);
                            planList.Add(shortPlan);
                        }
                        else
                        {
                            List<List<action>> plansUnderConstruction = new List<List<action>>();

                            foreach (stateItem eachPrereq in thisAction.prereqs)
                            {
                                if (isGoalAccomplished(eachPrereq) == false)
                                {
                                    List<List<action>> plansForThisPrereq = new List<List<action>>();
                                    plansForThisPrereq = problemSolver(eachPrereq, knownActions, state);
                                    
                                    if (plansForThisPrereq.Count == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (plansUnderConstruction.Count == 0)
                                        {
                                            plansUnderConstruction = plansForThisPrereq;
                                        }
                                        else
                                        {
                                            List<List<action>> nextStepInConstructingPlans = new List<List<action>>();
                                            foreach (List<action> eachPLanUnderConstruction in plansUnderConstruction)
                                            {
                                                foreach (List<action> eachPLanForThisPrereq in plansForThisPrereq)
                                                {
                                                    eachPLanUnderConstruction.AddRange(eachPLanForThisPrereq);
                                                    nextStepInConstructingPlans.Add(eachPLanUnderConstruction);
                                                }
                                            }
                                            plansUnderConstruction = nextStepInConstructingPlans;
                                        }
                                    }
                                }
                            }
                            //recursionCounter -= 1

                            foreach(List<action> eachWay in plansUnderConstruction)
                            {
                                List<action> eachPlan = new List<action>();
                                eachPlan = eachWay;
                                eachPlan.Add(thisAction);
                                planList.Add(eachPlan);
                            }
                        }
                    }
                }
            }
        }

        return planList;
    }

    // Start is called before the first frame update
    void Start()
    {
        state.Add("locationState", locationState);
        state.Add("feelings", feelings);
        state.Add("inventory", inventory);

        map.Add("workPlace", work1);
        map.Add("store", store1);
        map.Add("home", home1);
        //effect work1 = new effect();
        //w = GameObject.Find("workPlace");

        work1.stateCategory = "locationState";
        work1.inStateOrNot = true;
        work1.stateItemName = "workPlace";

        goToWork.name = "goToWork";
        goToWork.type = "goTo";
        goToWork.effects.Add(work1);
        goToWork.cost = 1;


        store1.stateCategory = "locationState";
        store1.inStateOrNot = true;
        store1.stateItemName = "store";

        goToStore.name = "goToStore";
        goToStore.type = "goTo";
        goToStore.effects.Add(store1);
        goToStore.cost = 1;


        home1.stateCategory = "locationState";
        home1.inStateOrNot = true;
        home1.stateItemName = "home";

        goToHome.name = "goToHome";
        goToHome.type = "goTo";
        goToHome.effects.Add(home1);
        goToHome.cost = 1;

        //toDoList.Add(goToHome);
        //toDoList.Add(goToWork);


        //toDoList.Add(goToStore);

        knownActions.Add(goToHome);
        knownActions.Add(goToWork);


        knownActions.Add(goToStore);
    }

    // Update is called once per frame
    void Update()
    {
        //constantlyCheckLocationState();
        
        //make sure list isn't empty:
        if (toDoList.Count > 0)
        {
            //ad hoc for now
            if (isThisActionDone(toDoList[0]))
            {
                toDoList.Remove(toDoList[0]);
            } 
        }
        else
        {
            Debug.Log("need to find a plan");
            List<List<action>> planList = new List<List<action>>();
            planList = problemSolver(work1, knownActions, state);
            //ad hoc for now
            if (planList.Count > 0)
            {
                Debug.Log("plan found");
                toDoList = planList[0];
            }
        }

        //make sure list isn't empty AGAIN:
        if (toDoList.Count > 0)
        {
            doNextAction(toDoList[0]);
        }
        

    }
}


public class stateItem
{
    //just the same as "prereq", just different name
    public string stateItemName;
    public string stateCategory;
    public bool inStateOrNot;
    //public int quantity;
    //public float coords[3];  //gotta fix this, should be vector?  WHAT IS THIS FOR???
    //public float valueEach;  //value for each item.  Needed for cost calcualtions.
}

public class action
{
    public string name;
    public string type;

    public List<stateItem> prereqs = new List<stateItem>();
    public List<stateItem> effects = new List<stateItem>();

    public int cost;

}

public class stateOfNPC
{
    public List<action> toDoList = new List<action>();
    public List<action> knownActions = new List<action>();

    public List<stateItem> locationState = new List<stateItem>();
    public List<stateItem> feelings = new List<stateItem>();
    public List<stateItem> inventory = new List<stateItem>();


}
