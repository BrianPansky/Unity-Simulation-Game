using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class premadeStuffForAI : MonoBehaviour
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

    //organizationState stuff:
    public stateItem employee1 = new stateItem();



    ////////////////////////////////////////////////
    //               ACTIONS
    ////////////////////////////////////////////////

    public action buyFood = new action();
    //public action sellFood = new action();
    public action doTheWork = new action();
    public action eat = new action();
    public action restock = new action();

    //public action findVictim = new action();
    //public action goToVictim = new action();
    public action seekVictim = new action();
    public action pickVictimsPocket = new action();

    public action hireSomeone = new action();
    public action workAsCashier = new action();

    public action beBoss = new action();
    






    public List<action> toDoList = new List<action>();
    public List<action> knownActions = new List<action>();

    public List<stateItem> goals = new List<stateItem>();


    //map
    public Dictionary<string, stateItem> map = new Dictionary<string, stateItem>();




    // Start is called before the first frame update
    //Awake is used to avoid issues, it's called even earlier than Start, I think (see notes somewhere):
    void Awake()
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

            employee1 = stateItemCreator("employee", "organizationState", 1);

        }

        //actions:
        {
            
            
            eat = actionCreator("eat", "use", createListOfStateItems(food1), createListOfStateItems(hungry0, food0), 1, home1);
            buyFood = actionCreator("buyFood", "socialTrade", createListOfStateItems(money1), createListOfStateItems(money0, food1), 1, checkout1);
            
            doTheWork = actionCreator("doTheWork", "work", createListOfStateItems(), createListOfStateItems(money1), 4, work1);
            //restock = actionCreator("restock", "ad-hoc", createListOfStateItems(money1), createListOfStateItems(money0, food1), 1);

            //sellFood = actionCreator("sellFood", "work", createListOfStateItems(food1), createListOfStateItems(money1, food0), 1, cashierZone1);
            workAsCashier = actionCreator("workAsCashier", "work", createListOfStateItems(), createListOfStateItems(money1), 1, cashierZone1);
            hireSomeone = actionCreator("hireSomeone", "ad-hoc", createListOfStateItems(), createListOfStateItems(employee1), 1, cashierZone1);
            beBoss = actionCreator("beBoss", "ad-hoc", createListOfStateItems(employee1), createListOfStateItems(profitMotive0), 1, home1);

            //pickpocket, under construction
            //findVictim = actionCreator("findVictim", "ad-hoc", createListOfStateItems(), createListOfStateItems(money0, food1), 1);
            //goToVictim = actionCreator("goToVictim", "ad-hoc", createListOfStateItems(), createListOfStateItems(money0, food1), 1);
            //seekVictim = actionCreator("seekVictim", "seek", createListOfStateItems(), createListOfStateItems(victim1), 1);
            pickVictimsPocket = actionCreator("pickVictimsPocket", "ad-hoc", createListOfStateItems(), createListOfStateItems(money1, food1), 1);
        }
    }



    ////////////////////////////////////////////////////
    //              Pre-Made NPC STATES
    ////////////////////////////////////////////////////
    
    //regular default NPC:
    public Dictionary<string, List<stateItem>> createNPCstate1()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();

        addToState(food1, state);
        addToState(money1, state);
        addToState(hungry0, state);

        return state;
    }

    //shopkeeper:
    public Dictionary<string, List<stateItem>> createShopkeeperState()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();

        addToState(food1, state);
        addToState(profitMotive0, state);

        return state;
    }

    //pickpocket:
    public Dictionary<string, List<stateItem>> createPickpocketState()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();

        addToState(hungry0, state);

        return state;
    }

    //player:
    public Dictionary<string, List<stateItem>> createPLAYERstate()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();

        //addToState(hungry0, state);
        
        return state;
    }






    //empty state, all others will call this first, then fill it
    //could I just move this up to "start"?
    //whatever, it seems more organized if it's here, 
    //very little (if any) additional computation cost
    public Dictionary<string, List<stateItem>> createEmptyState()
    {
        Dictionary<string, List<stateItem>> state = new Dictionary<string, List<stateItem>>();
        List<stateItem> feelings = new List<stateItem>();
        List<stateItem> inventory = new List<stateItem>();
        List<stateItem> locationState = new List<stateItem>();
        List<stateItem> organizationState = new List<stateItem>();

        state.Add("locationState", locationState);
        state.Add("feelings", feelings);
        state.Add("inventory", inventory);
        state.Add("organizationState", organizationState);

        return state;
    }

    public Dictionary<string, List<stateItem>> addToState(stateItem item, Dictionary<string, List<stateItem>> state)
    {
        //simply adds item:
        state[item.stateCategory].Add(item);

        return state;
    }




    ////////////////////////////////////////////////////
    //               NPC KNOWN ACTIONS
    ////////////////////////////////////////////////////

    public List<action> createKnownActions1()
    {
        
        knownActions.Add(doTheWork);

        

        knownActions.Add(eat);
        knownActions.Add(buyFood);
        


        return knownActions;
    }

    public List<action> createShopkeeperKnownActions()
    {
        knownActions.Add(hireSomeone);
        knownActions.Add(beBoss); 

        //knownActions.Add(sellFood);

        knownActions.Add(eat);
        //knownActions.Add(buyFood);
        
        knownActions.Add(restock);

        return knownActions;
    }

    public List<action> createPickpocketKnownActions()
    {
        

        knownActions.Add(eat);
        knownActions.Add(buyFood);
        

        //knownActions.Add(findVictim);
        //knownActions.Add(goToVictim);
        knownActions.Add(pickVictimsPocket);


        return knownActions;
    }
    

    public void addKnownAction(action theAction, List<action> listToModify)
    {
        //hmm, that's it?  Actually seems like this funciton is redundant
        //takes less writing to just use its CONTENTS

        listToModify.Add(theAction);
    }

    ////////////////////////////////////////////////////
    //                 NPC KNOWN MAPS
    ////////////////////////////////////////////////////

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

    action actionCreator(string name, string type, List<stateItem> prereqs, List<stateItem> effects, int cost, stateItem locationPrereq = null)
    {
        action thisAction = new action();

        thisAction.name = name;
        thisAction.type = type;

        thisAction.locationPrereq = locationPrereq;
        thisAction.prereqs = prereqs;

        thisAction.effects = effects;
        thisAction.cost = cost;

        return thisAction;
    }

    public stateItem stateItemCreator(string name, string stateCategory, int inStateOrNot)
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
    public stateItem locationPrereq;  //start as null?

    public int cost;

}