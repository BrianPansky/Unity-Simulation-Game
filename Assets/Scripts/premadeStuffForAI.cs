using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class premadeStuffForAI : MonoBehaviour
{
    public GameObject storagePrefab;

    //HMMM, INSTED OF GENERATING EVERYTHING, SHOULD JUST HAVE "ITEM" AND "TARGET" AND STUFF???
    //THEN ***FILL*** IT WITH ALL RELEVANT NEEDED INFO???
    //would still have to do steps to fill them with different info, but would be LESS work, i think
    //i dunno.  seems similar to uhhh whatever that class object way of doing things is
    //where you have....like, sub-species of the class.  
    //and the issues that can have if something doesn't fit the speciation tree....
    //but better than EVERYTHING had done????

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
    public stateItem resource1 = new stateItem();
    public stateItem gun = new stateItem();

    //feelings
    public stateItem profitMotive = new stateItem();
    public stateItem hungry = new stateItem();

    //organizationState stuff:
    public stateItem employee = new stateItem();
    public stateItem rentalProperty = new stateItem();
    public stateItem groupMember = new stateItem();
    public stateItem soldier = new stateItem();
    


    public stateItem victim = new stateItem();
    public stateItem toRecruit = new stateItem();
    

    public stateItem shopOwnership = new stateItem();

    public stateItem storageOwnership = new stateItem();

    public stateItem anyStore = new stateItem();

    public stateItem homeOwnership = new stateItem();

    public stateItem anyHome = new stateItem();

    public stateItem hiringZone = new stateItem();


    public stateItem threat = new stateItem();

    public stateItem myLeader = new stateItem();

    public stateItem anyLandPlot = new stateItem();

    public stateItem anyResource1 = new stateItem();

    public stateItem storagePlace = new stateItem();

    public stateItem anyGroupMember = new stateItem();

    public stateItem placeHolderFactionGoal = new stateItem();

    


    ////////////////////////////////////////////////
    //               ACTIONS
    ////////////////////////////////////////////////

    //public action buyFood = new action();
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

    public action giveMoneyToLeader = new action();

    public action landLording = new action();

    public action shootSpree = new action();

    public action createShop = new action();

    public action gatherResource1 = new action();

    public action resource1Dropoff = new action();

    public action recruit = new action();
    public action askMemberForMoney = new action();
    public action hireResourceGatherer = new action();

    public action createSoldier = new action();
    public action orderAttack = new action();

    public action createStorage = new action();

    



    //jobs
    public job cashierJob = new job();
    public job resource1GatheringJob = new job();






    ////////////////////////////////////////////////
    //                   LISTS
    ////////////////////////////////////////////////

    public List<action> toDoList = new List<action>();
    public List<action> knownActions = new List<action>();

    public List<stateItem> goals = new List<stateItem>();


    //do I use this any more???
    //map
    public Dictionary<string, stateItem> map = new Dictionary<string, stateItem>();


    public functionsForAI theFunctions;// = GetComponent<functionsForAI>();

    //Start is called before the first frame update, but too late
    //Awake is used to avoid issues, it's called even earlier than Start, I think (see notes somewhere):
    void Awake()
    {
        theFunctions = GetComponent<functionsForAI>();
        //stateItems:
        {
            rentalProperty = stateItemCreator("rentalProperty", "property");

            placeHolderFactionGoal = stateItemCreator("placeHolderFactionGoal", "organizationState");

            shopOwnership = stateItemCreator("shopOwnership", "property");
            storageOwnership = stateItemCreator("storageOwnership", "property");

            myLeader = stateItemCreator("myLeader", "target");
            myLeader.locationType = "deliverTo";

            storagePlace = stateItemCreator("storagePlace", "target");
            storagePlace.locationType = "deliverTo";

            home = stateItemCreator("home", "locationState");



            food = stateItemCreator("food", "inventory", 5);
            money = stateItemCreator("money", "inventory");
            resource1 = stateItemCreator("resource1", "inventory");

            store = stateItemCreator("store", "locationState");
            workPlace = stateItemCreator("workPlace", "locationState");
            hungry = stateItemCreator("hungry", "feelings");
            checkout = stateItemCreator("checkout", "locationState");
            checkout.locationType = "any";

            profitMotive = stateItemCreator("profitMotive", "feelings");
            cashierZone = stateItemCreator("cashierZone", "locationState");
            cashierZone.locationType = "roleLocation";

            employee = stateItemCreator("employee", "organizationState");
            groupMember = stateItemCreator("groupMember", "organizationState");
            soldier = stateItemCreator("soldier", "unitState");

            victim = stateItemCreator("victim", "target"); //outdated category???
            victim.locationType = "mobile";

            toRecruit = stateItemCreator("toRecruit", "target");
            toRecruit.locationType = "any";


            anyStore = stateItemCreator("anyStore", "locationState");
            anyStore.locationType = "any";

            //I SHOULD JUST MAKE MY ACTION CREATOR FUNCTIONS AUTOMATICALLY GENERATE THESE TARGET THING SBASED ON SOME INPUT(S)
            anyLandPlot = stateItemCreator("anyLandPlot", "locationState");
            anyLandPlot.locationType = "any";

            anyResource1 = stateItemCreator("anyResource1", "locationState");
            anyResource1.locationType = "any";
            

            homeOwnership = stateItemCreator("homeOwnership", "property");
            anyHome = stateItemCreator("anyHome", "locationState");
            anyHome.locationType = "any";

            hiringZone = stateItemCreator("hiringZone", "locationState");

            threat = stateItemCreator("threat", "threatState");

            gun = stateItemCreator("gun", "inventory");

            anyGroupMember = stateItemCreator("anyGroupMember", "locationState");
            anyGroupMember.locationType = "any";

        }

        //actions:
        {
            //NOTE AD-HOC TARGET RIGHT NOW IS "anyLandPlot"!!!!!!!!!!!!!!!
            createSoldier = actionCreator("createSoldier", "work", wantedPrereqsLister(resource1), UNwantedPrereqsLister(), wantedEffectsLister(soldier), UNwantedEffectsLister(resource1), 1, anyLandPlot);
            orderAttack = actionCreator("orderAttack", "work", wantedPrereqsLister(soldier), UNwantedPrereqsLister(), wantedEffectsLister(placeHolderFactionGoal), UNwantedEffectsLister(soldier), 1, anyLandPlot);


            createShop = actionCreator("createShop", "createProperty", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(shopOwnership), UNwantedEffectsLister(), 7, anyLandPlot);
            createStorage = actionCreator("createStorage", "createProperty", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(storageOwnership), UNwantedEffectsLister(), 7, anyLandPlot);

            //needs a target!  locationPrereq??!?!?!  do ALL actions need a target?  do all targets need a "LOCATION"prereq?????
            landLording = actionCreator("landLording", "capitalism", wantedPrereqsLister(homeOwnership), UNwantedPrereqsLister(), wantedEffectsLister(rentalProperty), UNwantedEffectsLister(homeOwnership, profitMotive), 1);
            shootSpree = actionCreator("shootSpree", "ad-hoc", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(money), UNwantedEffectsLister(), 1, victim);

            giveMoneyToLeader = actionCreator("giveMoneyToLeader", "deliver", wantedPrereqsLister(money), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(money), 1, myLeader);

            resource1Dropoff = actionCreator("resource1Dropoff", "realInventoryChanges", wantedPrereqsLister(resource1), UNwantedPrereqsLister(), wantedEffectsLister(money), UNwantedEffectsLister(resource1), 0, storagePlace);

            
            //wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(), 
            eat = actionCreator("eat", "use", wantedPrereqsLister(food, homeOwnership), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(hungry, food), 1, home);
            //eat = actionCreator("eat", "use", createListOfStateItems(food1, homeOwnership1), createListOfStateItems(hungry0, food0), 1, home1);
            
            doTheWork = actionCreator("doTheWork", "work", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(money), UNwantedEffectsLister(), 99, workPlace);
            //restock = actionCreator("restock", "ad-hoc", createListOfStateItems(money1), createListOfStateItems(money0, food1), 1);

            //sellFood = actionCreator("sellFood", "work", createListOfStateItems(food1), createListOfStateItems(money1, food0), 1, cashierZone1);
            workAsCashier = actionCreator("workAsCashier", "work", wantedPrereqsLister(), UNwantedPrereqsLister(threat), wantedEffectsLister(money), UNwantedEffectsLister(), 35, cashierZone);
            
            beBoss = actionCreator("beBoss", "ad-hoc", wantedPrereqsLister(employee, homeOwnership), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(profitMotive), 1, home);

            buyShop = actionCreator("buyShop", "buyThisProperty", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(shopOwnership), UNwantedEffectsLister(), 1, anyStore);

            buyHome = actionCreator("buyHome", "buyThisProperty", wantedPrereqsLister(money), UNwantedPrereqsLister(), wantedEffectsLister(homeOwnership), UNwantedEffectsLister(), 1, anyHome);

            //need target/location??
            handleSecurityMild = actionCreator("handleSecurityMild", "security", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(threat), 1);
            handleSecurityEscalationOne = actionCreator("handleSecurityEscalationOne", "security", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(threat), 4);
            
            pickVictimsPocket = actionCreator("pickVictimsPocket", "ad-hoc", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(money, food, resource1), UNwantedEffectsLister(), 1, victim);
            

            //target??????? location????
            giftGun = actionCreator("giftGun", "........", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(gun), 1);

            extort = actionCreator("extort", "crime", wantedPrereqsLister(gun), UNwantedPrereqsLister(), wantedEffectsLister(money), UNwantedEffectsLister(), 0, checkout);
            
            gatherResource1 = actionCreator("gatherResource1", "work", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(resource1), UNwantedEffectsLister(), 1, anyResource1);

            recruit = actionCreator("recruit", "organizing", wantedPrereqsLister(), UNwantedPrereqsLister(), wantedEffectsLister(groupMember), UNwantedEffectsLister(), 1, victim);


            //SHOULD OBVIOUSLY BE AUTOMATICALLY GENERATED:
            askMemberForMoney = actionCreator("askMemberForMoney", "commanding", wantedPrereqsLister(groupMember), UNwantedPrereqsLister(), wantedEffectsLister(money), UNwantedEffectsLister(), 1, anyGroupMember);
            hireSomeone = actionCreator("hireSomeone", "work", wantedPrereqsLister(shopOwnership), UNwantedPrereqsLister(threat), wantedEffectsLister(employee), UNwantedEffectsLister(), 1, hiringZone);
            hireResourceGatherer = actionCreator("hireResourceGatherer", "work", wantedPrereqsLister(storageOwnership), UNwantedPrereqsLister(), wantedEffectsLister(resource1), UNwantedEffectsLister(), 1, victim);

            //done automating these?????  can delete????????
            buyGun = actionCreator("buyGun", "buyFromStore", wantedPrereqsLister(money), UNwantedPrereqsLister(), wantedEffectsLister(gun), UNwantedEffectsLister(money), 1, checkout);
            //buyFood = actionCreator("buyFood", "buyFromStore", wantedPrereqsLister(money), UNwantedPrereqsLister(), wantedEffectsLister(food), UNwantedEffectsLister(money), 1, checkout);




        }

        //jobs:
        {
            //hmm, so much to fill in during hiring phase...is it even worth it to make this here???
            cashierJob = jobCreator(null, null, actionListCreator(workAsCashier), 1000, 0, 1);
            resource1GatheringJob = jobCreator(null, null, actionListCreator(gatherResource1, resource1Dropoff), 0, 3, 1);
            
            
        }
    }

    void Start()
    {
        
    }

    ////////////////////////////////////////////////////
    //              Pre-Made NPC STATES
    ////////////////////////////////////////////////////
    
    //regular default NPC:
    public Dictionary<string, List<stateItem>> createNPCstate1()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();
        
        //addToState(deepStateItemCopier(food), state);
        addToState(deepStateItemCopier(money), state);
        addToState(deepStateItemCopier(money), state);


        addToState(deepStateItemCopier(hungry), state);

        return state;
    }

    //shopkeeper:
    public Dictionary<string, List<stateItem>> createShopkeeperState()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();

        addToState(food, state);
        addToState(profitMotive, state);

        addToState(money, state);

        return state;
    }

    //pickpocket:
    public Dictionary<string, List<stateItem>> createPickpocketState()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();

        addToState(deepStateItemCopier(hungry), state);

        //AD-HOC while leader cannot plan with faction inventory, needs the inventory in their pocket:
        theFunctions.incrementItem(state["inventory"], money, 555);

        return state;
    }

    //player:
    public Dictionary<string, List<stateItem>> createPLAYERstate()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();

        //addToState(hungry0, state);

        //ad-hoc, [see 395478]:
        theFunctions.incrementItem(state["unitState"], soldier, 555);

        return state;
    }

    

    //money stockpile:
    public Dictionary<string, List<stateItem>> createMoneyStockpileState()
    {
        Dictionary<string, List<stateItem>> state = createEmptyState();
        
        theFunctions.incrementItem(state["inventory"], money, 555);

        //also store inventory for now?
        theFunctions.incrementItem(state["inventory"], food, 555);

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

        //maybe ad-hoc [see 3456819]:
        List<stateItem> unitState = new List<stateItem>();

        state.Add("locationState", locationState);
        state.Add("feelings", feelings);
        state.Add("inventory", inventory);
        state.Add("organizationState", organizationState);
        state.Add("property", propertyState);
        state.Add("threatState", threatState);

        //maybe ad-hoc [see 3456819]:
        state.Add("unitState", unitState);
        



        return state;
    }

    public Dictionary<string, List<stateItem>> addToState(stateItem item, Dictionary<string, List<stateItem>> state)
    {
        //state[item.stateCategory].Add(deepStateItemCopier(item));

        //is that correct?  the quantity in the item doesn't matter, just the integer we input???
        theFunctions.incrementItem(state[item.stateCategory], item, 1);

        return state;
    }




    ////////////////////////////////////////////////////
    //               NPC KNOWN ACTIONS
    ////////////////////////////////////////////////////

    public List<action> createKnownActions1()
    {
        List<action> newList = new List<action>();


        //newList.Add(gatherResource1);
        //newList.Add(resource1Dropoff);


        newList.Add(doTheWork);
        newList.Add(buyHome);
        
        

        //knownActions.Add(handleSecurityMild);
        //knownActions.Add(handleSecurityEscalationOne);

        newList.Add(eat);
        //newList.Add(buyFood);



        //knownActions.Add(doTheWork);
        //knownActions.Add(buyHome);

        //knownActions.Add(handleSecurityMild);
        //knownActions.Add(handleSecurityEscalationOne);

        //knownActions.Add(eat);
        //knownActions.Add(buyFood);
        //theFunctions.testSwitch();
        //Debug.Log("////////////////////////// surely here START /////////////////////////");

        knownActions = deepCopyActionList(newList);
        
        //theFunctions.testSwitch();


        //theFunctions.printKnownActionsDeeply(knownActions);
        //Debug.Log("//////////////////////////////// surely here END ///////////////////////////////");
        return knownActions;
    }

    public List<action> createShopkeeperKnownActions()
    {
        knownActions.Add(hireSomeone);
        //REMOVED FOR TEST:
        knownActions.Add(beBoss);
        knownActions.Add(buyShop);
        knownActions.Add(buyHome);
        //knownActions.Add(landLording);
        knownActions.Add(createShop);
        



        knownActions.Add(handleSecurityMild);
        knownActions.Add(handleSecurityEscalationOne);
        

        //knownActions.Add(sellFood);

        //knownActions.Add(eat);
        //knownActions.Add(buyFood);
        
        knownActions.Add(restock);

        return deepCopyActionList(knownActions);
    }

    public List<action> createPickpocketKnownActions()
    {
        List<action> newList = new List<action>();


        newList.Add(eat);
        //knownActions.Add(buyFood);
        newList.Add(buyHome);

        //knownActions.Add(doTheWork);

        
        //newList.Add(pickVictimsPocket);
        newList.Add(recruit);
        //newList.Add(askMemberForMoney);
        newList.Add(hireResourceGatherer);
        newList.Add(createStorage);
        newList.Add(createSoldier);

        newList.Add(orderAttack);
        



        //knownActions.Add(shootSpree); 


        //theFunctions.testSwitch();
        //Debug.Log("////////////////////////// surely here START /////////////////////////");

        knownActions = deepCopyActionList(newList);

        //theFunctions.testSwitch();


        //theFunctions.printKnownActionsDeeply(knownActions);
        //Debug.Log("//////////////////////////////// surely here END ///////////////////////////////");



        return knownActions;
    }
    

    public void addKnownAction(action theAction, List<action> listToModify)
    {
        //hmm, that's it?  Actually seems like this funciton is redundant
        //takes less writing to just use its CONTENTS

        listToModify.Add(theAction);
    }
    
    public List<action> deepCopyActionList(List<action> theList)
    {
        //required to prevent quantities from screwing up i think

        List<action> newList = new List<action>();

        foreach (action thisAction in theList)
        {
            newList.Add(deepActionCopier(thisAction));
        }


        if (theFunctions.testTime == true && newList.Count > 0)
        {
            Debug.Log(newList[0].effects[0].item.name);
            Debug.Log(newList[0].effects[0].item.quantity);
        }

        return newList;

    }
    
    public action deepActionCopier(action thisAction)
    {

        action newAction = new action();

        //these can be directly copied, they are never dynamically modified?  will cause no problems??
        newAction.name = thisAction.name;
        newAction.type = thisAction.type;
        newAction.cost = thisAction.cost;
        newAction.locationPrereq = thisAction.locationPrereq;


        newAction.prereqs = deepPrereqEffectCopier(thisAction.prereqs);
        newAction.effects = deepPrereqEffectCopier(thisAction.effects);

        if (theFunctions.testTime == true && newAction.effects.Count > 0)
        {
            //Debug.Log(newAction.effects[0].item.name);
            //Debug.Log(newAction.effects[0].item.quantity);
        }

        return newAction;
        
    }

    public List<actionItem> deepPrereqEffectCopier(List<actionItem> thisPrereqOrEffectList)
    {
        List<actionItem> newPrereqOrEffectList = new List<actionItem>();

        foreach(actionItem thisActionItem in thisPrereqOrEffectList)
        {
            newPrereqOrEffectList.Add(deepActionItemCopier(thisActionItem));
        }

        if (theFunctions.testTime == true  && newPrereqOrEffectList.Count > 0)
        {
            //Debug.Log(newPrereqOrEffectList[0].item.name);
            //Debug.Log(newPrereqOrEffectList[0].item.quantity);
        }


        return newPrereqOrEffectList;
    }

    public actionItem deepActionItemCopier(actionItem thisActionItem)
    {
        actionItem newActionItem = new actionItem();

        newActionItem.item = deepStateItemCopier(thisActionItem.item);

        //the rest can be shallow copy?  i never dynamically modify them, so no problems?
        newActionItem.inStateOrNot = thisActionItem.inStateOrNot;
        newActionItem.name = thisActionItem.name;
        newActionItem.stateCategory = thisActionItem.stateCategory;
        newActionItem.locationType = thisActionItem.locationType;

        if (theFunctions.testTime == true)
        {
            //Debug.Log(newActionItem.item.name);
            //Debug.Log(newActionItem.item.quantity);
        }

        return newActionItem;
    }

    public stateItem deepStateItemCopier(stateItem item)
    {
        stateItem newItem = null;
        newItem = stateItemCreator(item.name, item.stateCategory);
        newItem.locationType = item.locationType;

        newItem.quantity = 0; //just zero it out first
        newItem.quantity += item.quantity;

        newItem.value = 0; //just zero it out first
        newItem.value += item.value;

        //Debug.Log(newItem.name);
        //Debug.Log(newItem.quantity);

        if (theFunctions.testTime == true)
        {
            //Debug.Log(newItem.name);
            //Debug.Log(newItem.quantity);
        }


        return newItem;
    }


    ////////////////////////////////////////////////////
    //  Functions for making actions and StateItems
    ////////////////////////////////////////////////////

    public action actionCreator(string name, string type, List<stateItem> wantedPrereqs, List<stateItem> UNwantedPrereqs, List<stateItem> wantedEffects, List<stateItem> UNwantedEffects, int cost, stateItem locationPrereq = null)
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

    public stateItem stateItemCreator(string name, string stateCategory, int price = 1)
    {
        stateItem thisStateItem = new stateItem();

        thisStateItem.stateCategory = stateCategory;
        thisStateItem.name = name;
        thisStateItem.value = price;

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

    public actionItem convertToActionItemBoolVersion(stateItem inputItem, bool wantedOrNot)
    {
        actionItem newActionItem = new actionItem();
        newActionItem.item = inputItem;
        newActionItem.inStateOrNot = wantedOrNot;


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


    public stateItem quantityOfItemGenerator(stateItem item, int quantity)
    {
        //deepcopies item, modifies quantity
        stateItem thisStateItem = new stateItem();

        thisStateItem = deepStateItemCopier(item);
        thisStateItem.quantity = quantity;

        return thisStateItem;
    }

    public stateItem moneyFromItem(stateItem item)
    {
        //gives amount of money based on value of item
        //seems to translate "value" of item into "quantity" of money?
        //Debug.Log(item.name);
        //Debug.Log(item.value);
        return quantityOfItemGenerator(money, item.value);
    }

    //common action types to auto-generate:
    //public action bringMeX(stateItem itemX)
    //not handling dynamic locationprereq yet:
    public action bringLeaderX(stateItem itemX)
    {
        //will generate an action to bring any item X to "myLeader":
        return actionCreator("give" + itemX.name + "ToLeader", "deliverAnyXtoLeader", wantedPrereqsLister(itemX), UNwantedPrereqsLister(), wantedEffectsLister(), UNwantedEffectsLister(itemX), 1, myLeader);

    }



    //generate JOBS:
    public job jobCreator(GameObject boss, GameObject roleLocation, List<action> theKnownActions, int duration, int quota, int paymentQuantity = 1)
    {
        job thisJob = new job();

        thisJob.boss = boss;  //just make it null [by input] when initializing, then fill it in when actual hiring event happens?
        thisJob.roleLocation = roleLocation;  //ok to reuse this name?  gonna delete old one?
        thisJob.theKnownActions = theKnownActions;

        thisJob.duration = duration;
        thisJob.quota = quota;

        thisJob.paymentQuantity = paymentQuantity;


        return thisJob;
    }

    public List<action> actionListCreator(params action[] listofActions)
    {
        List<action> aNewList = new List<action>();

        foreach (action x in listofActions)
        {
            aNewList.Add(x);
        }

        return aNewList;
    }

    public job jobDeepCopier(job theJob)
    {
        job deepCopiedJob = new job();

        //game objects do not need any special treatment because they don't need to be deep copied:
        deepCopiedJob.boss = theJob.boss;
        deepCopiedJob.roleLocation = theJob.roleLocation;


        deepCopiedJob.theKnownActions = deepCopyActionList(theJob.theKnownActions);

        deepCopiedJob.duration = 0;
        deepCopiedJob.quota = 0;
        deepCopiedJob.duration += theJob.duration;
        deepCopiedJob.quota += theJob.quota;
        deepCopiedJob.paymentQuantity = 0;
        deepCopiedJob.paymentQuantity += theJob.paymentQuantity;



        return deepCopiedJob;
    }

    public job jobFinisher(job theJob, GameObject boss, GameObject roleLocation)
    {
        //finishes the details of a job during the hiring process

        //first, deep copy
        job finishedJob = new job();
        finishedJob = jobDeepCopier(theJob);

        //now, modify the two gameObject parameters:
        finishedJob.boss = boss;
        finishedJob.roleLocation = roleLocation;




        return finishedJob;
    }
}

public class stateItem
{
    //just the same as "prereq", just different name //[huh??? what's that comment talking about???]
    public string name;
    public string stateCategory;

    //bit ad-hoc seeming:
    public string locationType;
    public int quantity = 1;  //default 1 is ok?
    //basically, how much each one is worth, AKA value per quantity:
    public int value = 1;  //default 1 is ok?
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

public class job
{
    public GameObject boss;
    public GameObject roleLocation;  //ok to reuse this name?  gonna delete old one?
    public List<action> theKnownActions;

    public int duration;
    public int quota;

    public int paymentQuantity = 1;  //default
}