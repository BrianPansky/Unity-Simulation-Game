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



    public float speed = 10;
    

    public stateItem work1 = new stateItem();
    public action goToWork = new action();

    public stateItem store1 = new stateItem();
    public action goToStore = new action();
    public stateItem money1 = new stateItem();
    public stateItem money0 = new stateItem();
    public action buyFood = new action();

    public stateItem home1 = new stateItem();
    public action goToHome = new action();

    public stateItem food1 = new stateItem();
    public action eat = new action();
    public stateItem food0 = new stateItem();

    public stateItem hungry0 = new stateItem();



    public functionsForAI theFunctions;// = GetComponent<functionsForAI>();


    // Start is called before the first frame update
    void Start()
    {
        theFunctions = GetComponent<functionsForAI>();


        inventory.Add(food1);
        inventory.Add(money1);

        feelings.Add(hungry0);

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
        work1.name = "workPlace";

        goToWork.name = "goToWork";
        goToWork.type = "goTo";
        goToWork.effects.Add(work1);
        goToWork.cost = 1;


        store1.stateCategory = "locationState";
        store1.inStateOrNot = true;
        store1.name = "store";

        goToStore.name = "goToStore";
        goToStore.type = "goTo";
        goToStore.effects.Add(store1);
        goToStore.cost = 1;


        home1.stateCategory = "locationState";
        home1.inStateOrNot = true;
        home1.name = "home";

        goToHome.name = "goToHome";
        goToHome.type = "goTo";
        goToHome.effects.Add(home1);
        goToHome.cost = 1;

        //toDoList.Add(goToHome);
        //toDoList.Add(goToWork);


        //toDoList.Add(goToStore);

        food1.inStateOrNot = true;
        food1.stateCategory = "inventory";
        food1.name = "food";

        food0.inStateOrNot = false;
        food0.stateCategory = "inventory";
        food0.name = "food";

        money1.inStateOrNot = true;
        money1.stateCategory = "inventory";
        money1.name = "money";

        money0.inStateOrNot = false;
        money0.stateCategory = "inventory";
        money0.name = "money";


        hungry0.inStateOrNot = false;
        hungry0.stateCategory = "feelings";
        hungry0.name = "hungry";

        eat.name = "eat";
        eat.cost = 1;
        eat.type = "use";
        eat.prereqs.Add(home1);
        eat.prereqs.Add(food1);
        eat.effects.Add(hungry0);
        eat.effects.Add(food0);

        buyFood.name = "buyFood";
        buyFood.cost = 1;
        buyFood.type = "socialTrade";
        buyFood.prereqs.Add(money1);
        buyFood.prereqs.Add(store1);
        buyFood.effects.Add(money0);
        buyFood.effects.Add(food1);




        knownActions.Add(goToHome);
        knownActions.Add(goToWork);


        knownActions.Add(goToStore);

        knownActions.Add(eat);
        knownActions.Add(buyFood);
        
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
            //printState(state);
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
                state["feelings"].Add(hungry0);
            }
            //print("need to find a plan:");
            List<List<action>> planList = new List<List<action>>();
            planList = theFunctions.problemSolver(hungry0, knownActions, state);
            //printPlan(planList[0]);
            //print("state before imagination:");
            //printState(state);
            planList = theFunctions.simulatingPlansToEnsurePrereqs(planList, knownActions, state);
            //print("the plan after imagination fix:");
            //printPlan(planList[0]);
            //print("state AFTER imagination:");
            //printState(state);
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
        //printState(state);
        
    }
}


public class stateItem
{
    //just the same as "prereq", just different name
    public string name;
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
