using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class premadeStuffForAI2 : MonoBehaviour
{
    //here I define the actions for the AI

    //initializing:


    ////////////////////////////////////////////////
    //               STATE ITEMS
    ////////////////////////////////////////////////

    //locations:
    public stateItem home1 = new stateItem();
    public stateItem store1 = new stateItem();
    public stateItem work1 = new stateItem();
    public stateItem checkout1 = new stateItem();
    public stateItem cashierZone1 = new stateItem();

    //inventory items:
    public stateItem money1 = new stateItem();
    public stateItem money0 = new stateItem();
    public stateItem food1 = new stateItem();
    public stateItem food0 = new stateItem();

    //feelings
    public stateItem profitMotive0 = new stateItem();
    public stateItem hungry0 = new stateItem();



    ////////////////////////////////////////////////
    //               ACTIONS
    ////////////////////////////////////////////////
    public action goToWork = new action();
    public action goToStore = new action();
    public action buyFood = new action();
    public action sellFood = new action();
    public action goToHome = new action();
    public action doTheWork = new action();
    public action eat = new action();
    public action goToCashierZone = new action();
    public action goToCheckout = new action();
    public action restock = new action();




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

        //stateItems:
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

            profitMotive0 = stateItemCreator("profitMotive", "feelings", 0);
            cashierZone1 = stateItemCreator("cashierZone", "locationState", 1);

        }

        //actions:
        {
            //"goTO" actions:
            //[I really need to automate these at least.  They are all the same.  Locaitons exist, you can go to them.]
            //[except locaiton subsets, like goToCheckout, which require going to another locaiton first......but navMesh handles that actually]
            goToStore = actionCreator("goToStore", "goTo", createListOfStateItems(), createListOfStateItems(store1), 1);
            goToWork = actionCreator("goToWork", "goTo", createListOfStateItems(), createListOfStateItems(work1), 1);
            goToHome = actionCreator("goToHome", "goTo", createListOfStateItems(), createListOfStateItems(home1), 1);
            goToCashierZone = actionCreator("cashierZone", "goTo", createListOfStateItems(), createListOfStateItems(cashierZone1), 1);
            goToCheckout = actionCreator("checkout", "goTo", createListOfStateItems(store1), createListOfStateItems(checkout1), 1);

            //other actions:
            eat = actionCreator("eat", "use", createListOfStateItems(home1, food1), createListOfStateItems(hungry0, food0), 1);
            buyFood = actionCreator("buyFood", "socialTrade", createListOfStateItems(money1, checkout1), createListOfStateItems(money0, food1), 1);
            sellFood = actionCreator("sellFood", "socialTrade", createListOfStateItems(food1, cashierZone1), createListOfStateItems(money1, food0, profitMotive0), 1);
            doTheWork = actionCreator("doTheWork", "action", createListOfStateItems(work1), createListOfStateItems(money1), 4);
            restock = actionCreator("restock", "ad-hoc", createListOfStateItems(money1), createListOfStateItems(money0, food1), 1);
        }
    }



    ////////////////////////////////////////////////////
    //                 Pre-Made NPCs
    ////////////////////////////////////////////////////

    //regular default NPC:
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
        knownActions.Add(goToCheckout);
        

        return knownActions;
    }

    public Dictionary<string, stateItem> createMap1()
    {
        //annoyingly and-hoc/hand-crafted for now...

        Dictionary<string, stateItem> map1 = new Dictionary<string, stateItem>();

        map1.Add("workPlace", work1);
        map1.Add("store", store1);
        map1.Add("home", home1);
        map1.Add("checkout", checkout1);
        map1.Add("cashierZone", cashierZone1);

        //effect work1 = new effect();
        //w = GameObject.Find("workPlace");}

        return map1;
    }


    //shopkeeper:

    public Dictionary<string, List<stateItem>> createShopkeeperState()
    {
        inventory.Add(food1);
        //inventory.Add();

        feelings.Add(profitMotive0);

        state.Add("locationState", locationState);
        state.Add("feelings", feelings);
        state.Add("inventory", inventory);

        return state;
    }

    public List<action> createShopkeeperKnownActions()
    {
        knownActions.Add(goToHome);
        //knownActions.Add(goToWork);
        knownActions.Add(doTheWork);

        knownActions.Add(goToStore);
        knownActions.Add(sellFood);

        knownActions.Add(eat);
        //knownActions.Add(buyFood);
        knownActions.Add(goToCashierZone);
        knownActions.Add(restock);

        return knownActions;
    }


    ////////////////////////////////////////////////////
    //  Functions for making actions and StateItems
    ////////////////////////////////////////////////////

    public List<stateItem> createListOfStateItems(params stateItem[] listofStateItems)
    {
        List<stateItem> aNewList = new List<stateItem>();

        foreach (stateItem x in listofStateItems)
        {
            aNewList.Add(x);
        }

        return aNewList;
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