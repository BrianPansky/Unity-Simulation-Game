using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class premadeStuffForAI : MonoBehaviour
{
    //here I define the actions for the AI

    //initializing:

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

    public action doTheWork = new action();

    public stateItem hungry0 = new stateItem();



    //the NPC "state":
    public Dictionary<string, List<stateItem>> state = new Dictionary<string, List<stateItem>>();// { { "locationState", {{ }} }, { 2, "World" }, { 2, "World" } };

    public List<stateItem> feelings = new List<stateItem>();
    public List<stateItem> inventory = new List<stateItem>();
    public List<stateItem> locationState = new List<stateItem>();


    public List<action> toDoList = new List<action>();
    public List<action> knownActions = new List<action>();

    public List<stateItem> goals = new List<stateItem>();


    //map
    public Dictionary<string, stateItem> map = new Dictionary<string, stateItem>();




    // Start is called before the first frame update
    void Start()
    {

        //actions and stateItems:
        {

            //maybe change so state items DON'T have the boolean, 
            //I can ADD that as an extra peice when I
            //input them as prereqs and effects in the action maker??
            home1 = stateItemCreator("home", "locationState", 1);
            food1 = stateItemCreator("food", "inventory", 1);
            food0 = stateItemCreator("food", "inventory", 0);
            money1 = stateItemCreator("money", "inventory", 1);
            money0 = stateItemCreator("money", "inventory", 0);
            store1 = stateItemCreator("store", "locationState", 1);
            work1 = stateItemCreator("workPlace", "locationState", 1);
            hungry0 = stateItemCreator("hungry", "feelings", 0);
            checkout1 = stateItemCreator("checkout", "locationState", 1);
            cashierZone1 = stateItemCreator("cashierZone", "locationState", 1);


            goToWork.name = "goToWork";
            goToWork.type = "goTo";
            goToWork.effects.Add(work1);
            goToWork.cost = 1;

            doTheWork.name = "doTheWork";
            doTheWork.type = "action";
            doTheWork.prereqs.Add(work1);
            doTheWork.effects.Add(money1);
            doTheWork.cost = 4;

            

            //goToStore.name = "goToStore";
            //goToStore.type = "goTo";
            //goToStore.effects.Add(store1);
            //goToStore.cost = 1;
            List<stateItem> prereqsx = new List<stateItem>();
            List<stateItem> effectsx = new List<stateItem>();
            effectsx.Add(store1);

            goToStore = actionCreator("goToStore", "goTo", prereqsx, effectsx, 1);




            

            goToHome.name = "goToHome";
            goToHome.type = "goTo";
            goToHome.effects.Add(home1);
            goToHome.cost = 1;

            //toDoList.Add(goToHome);
            //toDoList.Add(goToWork);


            //toDoList.Add(goToStore);

            


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
        }

    }
    public Dictionary<string, List<stateItem>> createNPCstate1()
    {
        inventory.Add(food1);
        inventory.Add(money1);

        feelings.Add(hungry0);

        state.Add("locationState", locationState);
        state.Add("feelings", feelings);
        state.Add("inventory", inventory);

        return state;
    }


    public List<action> createKnownActions1()
    {
        knownActions.Add(goToHome);
        knownActions.Add(goToWork);
        knownActions.Add(doTheWork);

        knownActions.Add(goToStore);

        knownActions.Add(eat);
        knownActions.Add(buyFood);

        return knownActions;
    }

    public Dictionary<string, stateItem> createMap1()
    {
        Dictionary<string, stateItem> map1 = new Dictionary<string, stateItem>();

        map1.Add("workPlace", work1);
        map1.Add("store", store1);
        map1.Add("home", home1);
        //effect work1 = new effect();
        //w = GameObject.Find("workPlace");}

        return map1;
    }

    action actionCreator(string name, string type, List<stateItem> prereqs, List<stateItem> effects, int cost)
    {
        action thisAction = new action();

        thisAction.name = name;
        thisAction.type = type;

        thisAction.prereqs = prereqs;

        thisAction.effects = effects;
        thisAction.cost = cost;

        return thisAction;
    }

    stateItem stateItemCreator(string name, string stateCategory, int inStateOrNot)
    {
        stateItem thisStateItem = new stateItem();

        bool b;
        if (inStateOrNot == 1)
        {
            b = true;
        }
        else
        {
            b = false;
        }

        thisStateItem.stateCategory = stateCategory;
        thisStateItem.inStateOrNot = b;
        thisStateItem.name = name;

        return thisStateItem;
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


