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
    public stateItem home = new stateItem();
    public stateItem store = new stateItem();
    public stateItem workPlace = new stateItem();
    public stateItem checkout = new stateItem();
    public stateItem cashierZone = new stateItem();

    //inventory items:
    public stateItem money = new stateItem();
    public stateItem food = new stateItem();

    public stateItem gun = new stateItem();

    //feelings
    public stateItem profitMotive = new stateItem();
    public stateItem hungry = new stateItem();

    //organizationState stuff:
    public stateItem employee = new stateItem();

    public stateItem victim = new stateItem();

    public stateItem shopOwnership = new stateItem();

    public stateItem anyStore = new stateItem();

    public stateItem homeOwnership = new stateItem();

    public stateItem anyHome = new stateItem();

    public stateItem hiringZone = new stateItem();


    public stateItem threat = new stateItem();



    ////////////////////////////////////////////////
    //               ACTIONS
    ////////////////////////////////////////////////

    public action buyFood = new action();
    //public action sellFood = new action();
    public action doTheWork = new action();
    public action eat = new action();
    public action restock = new action();
    
    public action seekVictim = new action();
    public action pickVictimsPocket = new action();

    public action hireSomeone = new action();
    public action workAsCashier = new action();

    public action beBoss = new action();

    public action buyShop = new action();
    public action buyHome = new action();


    public action handleSecurityMild = new action();
    public action handleSecurityEscalationOne = new action();


    public action buyGun = new action();
    public action giftGun = new action();

    public action extort = new action();


    ////////////////////////////////////////////////
    //                   LISTS
    ////////////////////////////////////////////////

    public List<action> toDoList = new List<action>();
    public List<action> knownActions = new List<action>();

    public List<stateItem> goals = new List<stateItem>();


    //do I use this any more???
    //map
    public Dictionary<string, stateItem> map = new Dictionary<string, stateItem>();




    //Start is called before the first frame update, but too late
    //Awake is used to avoid issues, it's called even earlier than Start, I think (see notes somewhere):
    void Awake()
    {

        //stateItems:
        {
            
            home = stateItemCreator("home", "locationState");
            food = stateItemCreator("food", "inventory");
            money = stateItemCreator("money", "inventory");
            store = stateItemCreator("store", "locationState");
            workPlace = stateItemCreator("workPlace", "locationState");
            hungry = stateItemCreator("hungry", "feelings");
            checkout = stateItemCreator("checkout", "locationState");
            checkout.locationType = "any";

            profitMotive = stateItemCreator("profitMotive", "feelings");
            cashierZone = stateItemCreator("cashierZone", "locationState");
            cashierZone.locationType = "roleLocation";

            employee = stateItemCreator("employee", "organizationState");

            victim = stateItemCreator("victim", "target");
            victim.locationType = "mobile";

            shopOwnership = stateItemCreator("shopOwnership", "property");

            anyStore = stateItemCreator("anyStore", "locationState");
            anyStore.locationType = "any";

            homeOwnership = stateItemCreator("homeOwnership", "property");
            anyHome = stateItemCreator("anyHome", "locationState");
            anyHome.locationType = "any";

            hiringZone = stateItemCreator("hiringZone", "locationState");

            threat = stateItemCreator("threat", "threatState");

            gun = stateItemCreator("gun", "inventory");

        }

        //actions:
        {

            //wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(), 
            eat = actionCreator("eat", "use", wantedPrereqsLister(food, homeOwnership), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(hungry, food), 1, home);
            //eat = actionCreator("eat", "use", createListOfStateItems(food1, homeOwnership1), createListOfStateItems(hungry0, food0), 1, home1);
            buyFood = actionCreator("buyFood", "buyFromStore", wantedPrereqsLister(money), UNwantedPrereqsLister(), wantedEffectsLister(food), UNwantedEffectsLister(money), 1, checkout);
            
            doTheWork = actionCreator("doTheWork", "work", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(money), UNwantedEffectsLister(), 4, workPlace);
            //restock = actionCreator("restock", "ad-hoc", createListOfStateItems(money1), createListOfStateItems(money0, food1), 1);

            //sellFood = actionCreator("sellFood", "work", createListOfStateItems(food1), createListOfStateItems(money1, food0), 1, cashierZone1);
            workAsCashier = actionCreator("workAsCashier", "work", wantedPrereqsLister(), UNwantedPrereqsLister(threat), wantedEffectsLister(money), UNwantedEffectsLister(), 1, cashierZone);
            hireSomeone = actionCreator("hireSomeone", "work", wantedPrereqsLister(shopOwnership), UNwantedPrereqsLister(threat), wantedEffectsLister(employee), UNwantedEffectsLister(), 1, hiringZone);
            beBoss = actionCreator("beBoss", "ad-hoc", wantedPrereqsLister(employee, homeOwnership), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(profitMotive), 1, home);

            buyShop = actionCreator("buyShop", "buyThisProperty", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(shopOwnership), UNwantedEffectsLister(), 1, anyStore);

            buyHome = actionCreator("buyHome", "buyThisProperty", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(homeOwnership), UNwantedEffectsLister(), 1, anyHome);


            handleSecurityMild = actionCreator("handleSecurityMild", "security", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(threat), 1);
            handleSecurityEscalationOne = actionCreator("handleSecurityEscalationOne", "security", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(threat), 4);
            
            pickVictimsPocket = actionCreator("pickVictimsPocket", "ad-hoc", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(money, food), UNwantedEffectsLister(), 1, victim);

            buyGun = actionCreator("buyGun", "buyFromStore", wantedPrereqsLister(money), UNwantedPrereqsLister(), wantedEffectsLister(gun), UNwantedEffectsLister(money), 1, checkout);
            giftGun = actionCreator("buyGun", "buyFromStore", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(gun), 1);

            extort = actionCreator("extort", "crime", wantedPrereqsLister(gun), UNwantedPrereqsLister(), wantedEffectsLister(money), UNwantedEffectsLister(), 0, checkout);
        }
    }



    ////////////////////////////////////////////////////
    //              Pre-Made NPC STATES
    ////////////////////////////////////////////////////
    
    //regular default NPC:
    public Dictionary<string, List<stateItem>> createNPCstate1()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();

        //addToState(food1, state);
        addToState(money, state);
        addToState(hungry, state);

        return state;
    }

    //shopkeeper:
    public Dictionary<string, List<stateItem>> createShopkeeperState()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();

        addToState(food, state);
        addToState(profitMotive, state);

        return state;
    }

    //pickpocket:
    public Dictionary<string, List<stateItem>> createPickpocketState()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();

        addToState(hungry, state);

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
        List<stateItem> propertyState = new List<stateItem>();
        List<stateItem> threatState = new List<stateItem>();


        state.Add("locationState", locationState);
        state.Add("feelings", feelings);
        state.Add("inventory", inventory);
        state.Add("organizationState", organizationState);
        state.Add("property", propertyState);
        state.Add("threatState", threatState);
        

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
        knownActions.Add(buyHome);

        knownActions.Add(handleSecurityMild);
        knownActions.Add(handleSecurityEscalationOne);

        knownActions.Add(eat);
        knownActions.Add(buyFood);
        


        return knownActions;
    }

    public List<action> createShopkeeperKnownActions()
    {
        knownActions.Add(hireSomeone);
        knownActions.Add(beBoss);
        knownActions.Add(buyShop);
        knownActions.Add(buyHome);

        knownActions.Add(handleSecurityMild);
        knownActions.Add(handleSecurityEscalationOne);
        

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
        knownActions.Add(buyHome);

        knownActions.Add(doTheWork);
        

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

        map1.Add("workPlace", workPlace);
        map1.Add("store", store);
        map1.Add("home", home);
        map1.Add("checkout", checkout);
        map1.Add("cashierZone", cashierZone);

        //effect work1 = new effect();
        //w = GameObject.Find("workPlace");}

        return map1;
    }




    ////////////////////////////////////////////////////
    //  Functions for making actions and StateItems
    ////////////////////////////////////////////////////

    action actionCreator(string name, string type, List<stateItem> wantedPrereqs, List<stateItem> UNwantedPrereqs, List<stateItem> wantedEffects, List<stateItem> UNwantedEffects, int cost, stateItem locationPrereq = null)
    {
        action thisAction = new action();

        thisAction.name = name;
        thisAction.type = type;
        thisAction.cost = cost;
        thisAction.locationPrereq = locationPrereq;


        thisAction.prereqs = makePrereqsOrEffects(wantedPrereqs, UNwantedPrereqs);
        thisAction.effects = makePrereqsOrEffects(wantedEffects, UNwantedEffects);


        return thisAction;
    }

    public stateItem stateItemCreator(string name, string stateCategory)
    {
        stateItem thisStateItem = new stateItem();

        thisStateItem.stateCategory = stateCategory;
        thisStateItem.name = name;

        return thisStateItem;
    }

    //wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(), 

    public List<stateItem> wantedPrereqsLister(params stateItem[] listofStateItems)
    {
        List<stateItem> aNewList = new List<stateItem>();

        foreach (stateItem x in listofStateItems)
        {
            aNewList.Add(x);
        }

        return aNewList;
    }

    public List<stateItem> UNwantedPrereqsLister(params stateItem[] listofStateItems)
    {
        List<stateItem> aNewList = new List<stateItem>();

        foreach (stateItem x in listofStateItems)
        {
            aNewList.Add(x);
        }

        return aNewList;
    }
    
    public List<stateItem> wantedEffectsLister(params stateItem[] listofStateItems)
    {
        List<stateItem> aNewList = new List<stateItem>();

        foreach (stateItem x in listofStateItems)
        {
            aNewList.Add(x);
        }

        return aNewList;
    }
    
    public List<stateItem> UNwantedEffectsLister(params stateItem[] listofStateItems)
    {
        List<stateItem> aNewList = new List<stateItem>();

        foreach (stateItem x in listofStateItems)
        {
            aNewList.Add(x);
        }

        return aNewList;
    }


    public List<actionItem> makePrereqsOrEffects(List<stateItem> wantedOnes, List<stateItem> UNwantedOnes)
    {
        //input the wanted and unwanted stateItems
        //returns "actionItems" in a list that capture that info
        //used to make my lists of prereqs and effects

        List<actionItem> theList = new List<actionItem>();
        theList = convertingStateItemsToActionItems(theList, wantedOnes, 1);
        theList = convertingStateItemsToActionItems(theList, UNwantedOnes, 0);

        return theList;

    }

    public List<actionItem> convertingStateItemsToActionItems(List<actionItem> listToAddTo, List<stateItem> theStartList, int wantedOrNot)
    {
        foreach (stateItem thisStateItem in theStartList)
        {
            
            listToAddTo.Add(convertToActionItem(thisStateItem, wantedOrNot));
        }
        
        return listToAddTo;
    }

    public actionItem convertToActionItem(stateItem inputItem, int wantedOrNot)
    {
        actionItem newActionItem = new actionItem();
        newActionItem.item = inputItem;
        newActionItem.inStateOrNot = intToBool(wantedOrNot);


        //copy from stateItem:
        newActionItem.name = inputItem.name;
        newActionItem.stateCategory = inputItem.stateCategory;
        newActionItem.locationType = inputItem.locationType;

        return newActionItem;
    }

    public bool intToBool(int number)
    {
        //takes a 1 or a 0, converts it to boolean
        //careful to never give it any other number!  It will simply return false!

        if (number == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class stateItem
{
    //just the same as "prereq", just different name //[huh??? what's that comment talking about???]
    public string name;
    public string stateCategory;

    //bit ad-hoc seeming:
    public string locationType;
    //public int quantity;
}

public class actionItem
{
    public stateItem item;
    public bool inStateOrNot;

    //copy from stateItem for convenience while refactoring my code:
    public string name;
    public string stateCategory;
    public string locationType;
}

public class action
{
    public string name;
    public string type;

    public List<actionItem> prereqs = new List<actionItem>();
    public List<actionItem> effects = new List<actionItem>();
    public stateItem locationPrereq;  //start as null?

    public int cost;

    //maybe have methods here???
    //I can fill them in somehow???
    public void doThisAction()
    {

    }
}