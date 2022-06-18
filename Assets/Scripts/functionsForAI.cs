using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class functionsForAI : MonoBehaviour
{
    [SerializeField]
    Transform _destination;
    NavMeshAgent _navMeshAgent;

    //I don't remember making this, why is it here instead of in the function where I use it?
    //probably carried over from some tutorial?  On navmesh?
    private GameObject t1;


    public AI1 thisAI;// = GetComponent<AI1>();
    public premadeStuffForAI premadeStuff;

    public Dictionary<string, List<GameObject>> globalTags;

    // Start is called before the first frame update
    void Start()
    {

        thisAI = GetComponent<AI1>();
        premadeStuff = GetComponent<premadeStuffForAI>();

        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        //getting the "global" tags:
        GameObject theWorldObject = GameObject.Find("World");
        worldScript theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        globalTags = theWorldScript.taggedStuff;
    }






    ////////////////////////////////////////////////
    //                  ACTIONS
    ////////////////////////////////////////////////

    //handles the enactment of ALL actions:
    public GameObject doNextAction(action nextAction, Dictionary<string, List<stateItem>> state, GameObject target)
    {

        //handle the travel prereqs here:
        if (nextAction.locationPrereq != null)
        {
            //here we automatically try to fill all travel prereqs

            //I still currently use a "stateItem", but it could perhaps
            //be replaced with mere text?  The name of the location?

            travelToStateItem(nextAction.locationPrereq);
        }


        //actions with ALL prereqs met (including location prereq) can proceed below:
        if (whicheverPrereqChecker(nextAction, state) == true)
        {
            if (nextAction.type == "socialTrade")
            {
                //print(nextAction.name);
                //GameObject theTarget = GameObject.Find("NPC shopkeeper");
                //AI1 theTargetState = theTarget.GetComponent("AI1") as AI1;
                //print(theTargetState.state["locationState"][0].name);
                GameObject customerLocation = getLocationObject(state["locationState"][0].name);

                GameObject cashierMapZone = getCashierMapZone(customerLocation);
                //print(casheirMapZone.name);

                //print(locationRoot.name);

                GameObject cashier = whoIsTrader(cashierMapZone);

                //but that cashier variable might come back null (if no one is there), check:
                if(cashier != null)
                {
                    AI1 theTargetState = cashier.GetComponent("AI1") as AI1;

                    //now implement trade
                    //state["inventory"].Remove(nextAction.effects[0]);
                    trade(state["inventory"], theTargetState.state["inventory"], nextAction);
                }
                
            }
            else if (nextAction.name == "hireSomeone")
            {
                //ad-hoc for now

                //find someone (and NPC, for now) to hire,
                //then change their knownActions, remove "doTheWork", add "workAsCashier"

                //get the cashierLocation:
                GameObject cashierLocation = getLocationObject("cashierZone");

                //get the checkoutZone:
                GameObject checkoutZone = getCheckoutMapZone(cashierLocation);

                //check for an NPC customer in the checkoutZone:
                GameObject customer = checkForCustomer(checkoutZone);
                if (customer != null)
                {
                    changeRoles(customer, premadeStuff.workAsCashier, premadeStuff.doTheWork);

                    //and ad hoc strike this action off the to-do list:
                    thisAI.toDoList.RemoveAt(0);

                    //also, to easily put the "employee1" stateItem in the organizationState:
                    state = implementALLEffects(nextAction, state);
                }


            }
            else if (nextAction.name == "pickVictimsPocket")
            {
                //ad hoc for now


                //if we have a target, use that one
                //(so always blank out targets BEFORE this action happens, I guess)
                if (target == null)
                {
                    //print("uuuuuuuuuuuuuuuuuuuuuuhhhhhhhhhhhhhhhhhhhhhhhhhhh");
                    target = whoToTarget();
                }
                GameObject victim;
                victim = target;

                //navigation
                //transform.position = Vector3.MoveTowards(transform.position, t1.GetComponent<Transform>().position, thisAI.speed * Time.deltaTime);
                Vector3 targetVector = victim.GetComponent<Transform>().position;
                _navMeshAgent.SetDestination(targetVector);


                //now check distance
                //GameObject thePickpocket = GameObject.Find("NPC pickpocket");
                GameObject thePickpocket = GameObject.Find(this.name);
                float distance = Vector3.Distance(thePickpocket.transform.position, victim.transform.position);
                if (distance < 2.0f)
                {


                    //now do the pickpocketing
                    AI1 theTargetState = victim.GetComponent("AI1") as AI1;
                    steal(state["inventory"], theTargetState.state["inventory"], nextAction);


                    //ad-hoc action completion:
                    thisAI.toDoList.RemoveAt(0);
                    target = null;

                    //state = implementALLEffects(nextAction, state);
                }


                //print(target.name);
            }
            else
            {
                //for actions like "eat" that currently just need a quick
                //ad-hoc update of state:
                state = implementALLEffects(nextAction, state);
            }
        }
        

        //ad hoc for now:
        return target;
    }


    //..................................................................
    //functions to handle important finishing steps of specific actions:

    public void trade(List<stateItem> actionerInventory, List<stateItem> inventory2, action nextAction)
    {
        //actioner is the one doing the nextAction

        //need to initialize:
        stateItem actionerGives = new stateItem();
        stateItem actionerReceives = new stateItem();

        //set up items to trade:
        foreach (stateItem effect in nextAction.effects)
        {
            if (effect.inStateOrNot == false)
            {
                actionerGives = effect;
            }
            if (effect.inStateOrNot == true)
            {
                actionerReceives = effect;
            }
        }


        //do the trade:
        actionerInventory.RemoveAll(x => x.name == actionerGives.name);
        actionerInventory.Add(actionerReceives);

        inventory2.RemoveAll(y => y.name == actionerReceives.name);
        inventory2.Add(actionerGives);

    }

    public void steal(List<stateItem> actionerInventory, List<stateItem> inventory2, action nextAction)
    {
        //probably ad-hoc for now

        //actioner is the one doing the nextAction

        //https://stackoverflow.com/a/605390
        List<stateItem> actionerReceives = new List<stateItem>();
        List<stateItem> otherInventoryLoses = new List<stateItem>();

        //look to steal EACH item in the "effects" of the steal aciton
        //but only LOOK and take note, don't modify inventories YET (can lead to error)
        foreach (stateItem effect in nextAction.effects)
        {
            //actionerReceives = effect;
            //actionerInventory.Add(actionerReceives);
            //inventory2.RemoveAll(y => y.name == actionerReceives.name);

            //but must only steal items if they exist in the victim's inventory!
            foreach (stateItem itemInInventory2 in inventory2)
            {
                if(itemInInventory2.name == effect.name)
                {
                    actionerReceives.Add(effect);
                    otherInventoryLoses.Add(itemInInventory2);
                }
            }
        }

        //NOW modify inventories
        foreach (stateItem item in actionerReceives)
        {
            actionerInventory.Add(item);
        }
        foreach (stateItem item in otherInventoryLoses)
        {
            inventory2.Remove(item);
        }
    }

    public void addKnownActionToGameObject(GameObject agent, action theAction)
    {
        //first, go from "GameObject" to it's script that has knownActions:
        AI1 hubScript = getHubScriptFromGameObject(agent);

        //now add the knownAction:
        hubScript.knownActions.Add(theAction);
    }

    public void removeKnownActionFromGameObject(GameObject agent, action theAction)
    {
        //first, go from "GameObject" to it's script that has knownActions:
        AI1 hubScript = getHubScriptFromGameObject(agent);

        //now add the knownAction:
        hubScript.knownActions.RemoveAll(y => y.name == theAction.name);
    }

    public void changeRoles(GameObject agent, action roleToAdd, action roleToRemove)
    {
        //maybe in future the inputs can be some role class object
        //for now, it's just a quick way to add one action and remove another

        addKnownActionToGameObject(agent, roleToAdd);
        removeKnownActionFromGameObject(agent, roleToRemove);

    }

    public void travelToStateItem(stateItem X)
    {

        string name1 = X.name;
        t1 = GameObject.Find(name1);

        Vector3 targetVector = t1.GetComponent<Transform>().position;
        _navMeshAgent.SetDestination(targetVector);
    }

    //..................................................................



    ////////////////////////////////////////////////
    //         Misc functions for ACTIONS
    ////////////////////////////////////////////////


    public GameObject getLocationObject(string nameOfLocation)
    {
        //is this redundant?  Whatever.  I might change how it works later...
        //ya, later I want this to get the object based on TOUCH, not name...

        GameObject locationObject = GameObject.Find(nameOfLocation);



        return locationObject;

    }

    public GameObject getCashierMapZone(GameObject customerLocation)
    {
        GameObject cashierMapZone;
        cashierMapZone = null; //just in case none is found
        GameObject locationParent = customerLocation.transform.parent.gameObject;

        //now search for the correct "child" object:
        foreach(Transform child in locationParent.transform)
        {
            if(child.name == "cashierZone")
            {
                //but the actual "mapZone" is a CHILD of this casheirZone object:
                cashierMapZone = child.GetChild(0).gameObject;
                return cashierMapZone;
            }

        }

        //should suceed in the loop and NOT execute the following code
        //thus the following code prints an error:
        print("cashierZone not found, perhaps it is not a child of the checkout zone's parent object");
        return cashierMapZone;

    }

    public GameObject getCheckoutMapZone(GameObject cashierLocation)
    {
        GameObject checkoutZone;
        checkoutZone = null; //just in case none is found, the above won't work, trying to return it will have compile error
        GameObject locationParent = cashierLocation.transform.parent.gameObject;

        //now search for the correct "child" object:
        //wait, doesn't Unity have a way to do this?  Haven't I done this elsewhere???
        foreach (Transform child in locationParent.transform)
        {
            if (child.name == "checkout")
            {
                //but the actual "mapZone" is a CHILD of this checkout object:
                checkoutZone = child.GetChild(0).gameObject;
                return checkoutZone;
            }

        }

        //should suceed in the loop and NOT execute the following code
        //thus the following code prints an error:
        print("checkout zone not found, perhaps it is not a child of the cashier zone's parent object");
        return checkoutZone;

    }

    public GameObject whoIsTrader(GameObject cashierZone)
    {
        //get the "listOfTouchingNPCs" script on the casheirZone
        //get first item on that list, it should be be the cashier
        //return that item

        GameObject cashier;
        cashier = null;

        listOfTouchingNPCs listOfNPCs = cashierZone.GetComponent<listOfTouchingNPCs>();

        //check to make sure the list isn't empty:
        if (listOfNPCs.theList.Count > 0)
        {
            cashier = listOfNPCs.theList[0];
        }
        

        return cashier;


    }

    public GameObject checkForCustomer(GameObject checkoutZone)
    {
        GameObject customer;
        customer = null; //required to compile
        listOfTouchingNPCs listOfNPCs = checkoutZone.GetComponent<listOfTouchingNPCs>();

        //check if there ARE any NPCs there at all:
        if(listOfNPCs.theList.Count > 0)
        {
            customer = listOfNPCs.theList[0];
        }
        

        return customer;

    }

    public GameObject whoToTarget()
    {
        //ad-hoc for now, this is being used in pickpocketing action
        //should return ONE NPC GameObject as a target

        List<GameObject> allPotentialTargets = new List<GameObject>();

        //now to find suitable targets using my new tagging system:
        allPotentialTargets = globalTags["person"];

        //....................................................................
        //old way to get stuff, using old stupid Unity tag system, and stupid "Array" data type for no reason, good riddence:
        //GameObject[] allNPCsArray;
        //allNPCsArray = GameObject.FindGameObjectsWithTag("anNPC");

        //convert this stupid fucking array data type to a list:
        /*
        List<GameObject> allPotentialTargets = new List<GameObject>();
        foreach (GameObject g in allNPCsArray)
        {
            allPotentialTargets.Add(g);
        }
        */
        //....................................................................


        //choose one randomly
        //Random rnd = new Random();

        GameObject thisNPC;
        thisNPC = null;
        bool doWeHaveGoodTarget = false;

        while (doWeHaveGoodTarget == false && allPotentialTargets.Count > 0)
        {
            int randomIndex = Random.Range(0, allPotentialTargets.Count);
            thisNPC = allPotentialTargets[randomIndex];
            //but, criteria, ad-hoc for now
            //if it's the shopkeeper, remove that item from the array (will that leave a "null" hole in array???)
            //and choose again
            if (thisNPC.name == "NPC shopkeeper")
            {
                allPotentialTargets.RemoveAt(randomIndex);
                thisNPC = null;
            }
            else
            {
                doWeHaveGoodTarget = true;
            }
        }
        

        return thisNPC;
    }

    public AI1 getHubScriptFromGameObject(GameObject NPC)
    {
        AI1 theHub = NPC.GetComponent("AI1") as AI1;

        return theHub;
    }


    ////////////////////////////////////////////////
    //         Misc diagnostic functions
    ////////////////////////////////////////////////

    public void print(string text)
    {
        Debug.Log(text);
    }

    public void printStateItemList(List<stateItem> theList)
    {
        string printout = string.Empty;

        foreach (stateItem item in theList)
        {
            printout += item.name + ' ';
        }

        print(printout);
    }

    public void printInventory(List<stateItem> inv)
    {
        printStateItemList(inv);
    }

    public string planToText(List<action> plan)
    {
        string printout = string.Empty;

        foreach (action listItem in plan)
        {
            printout += listItem.name + ' ';
        }

        return printout;
    }

    public string planListToText(List<List<action>> planList)
    {
        string printout = string.Empty;

        foreach(List<action> list in planList)
        {
            printout += "[ " + planToText(list) + " ] ";
        }

        return printout;
    }

    public string listOfPlanListsToText(List<List<List<action>>> listofPlanLists)
    {
        string printout = string.Empty;

        foreach(List<List<action>> planList in listofPlanLists)
        {
            printout += "[ " + planListToText(planList) + " ] ";
        }

        return printout;
    }

    public void printPlan(List<action> plan)
    {
        print(planToText(plan));
    }

    public void printState(Dictionary<string, List<stateItem>> state)
    {
        string text;
        text = "{";
        foreach (string key in state.Keys)
        {
            text = string.Concat(text, key, ", ");
            foreach (stateItem content in state[key])
            {
                text = string.Concat(text, content.name, ", ");
            }
            text = string.Concat(text, "} ");
        }
        text = string.Concat(text, "}");
        print(text);
    }




    ////////////////////////////////////////////////
    //                Planning
    ////////////////////////////////////////////////

    public List<List<action>> problemSolver(stateItem goal, List<action> knownActions, Dictionary<string, List<stateItem>> state)
    {
        //need a LIST of plans because there can be all kinds of different ways to acheive a goal
        //in fact, every single step of one plan can be absent from another plan
        List<List<action>> planList = new List<List<action>>();

        //first just make sure we need a plan at all (could remove this?):
        if (isGoalAccomplished(goal, state) == false)
        {
            //cycle through every known action, so we can check if any accomplish the goal:
            foreach (action thisAction in knownActions)
            {
                //also have to look at each of their effects individually, see if the effect is to 
                foreach (stateItem thisEffect in thisAction.effects)
                {
                    //finally, check if this action effect acheives the goal:
                    if (goal.name == thisEffect.name & goal.inStateOrNot == thisEffect.inStateOrNot)
                    {
                        //ok cool, we have an action that would acheive the goal
                        //but do we have a prereq to DO that aciton?  Have to check:
                        if (prereqChecker(thisAction, state))
                        {
                            //yup!  we can do this action and acheive the goal!
                            //so our entire "plan" is just this one action:
                            List<action> shortPlan = new List<action>();
                            shortPlan.Add(thisAction);
                            planList.Add(shortPlan);
                        }
                        else
                        {
                            //So no, we don't have the prereqs for this action
                            //so we'll see if we can FILL the prereqs!

                            //so, I think we get a bunch of COMPLETE plans from the prereq filling funciton
                            //and they should be totally finished and ready to simply add to the planList:
                            planList = mergePlanLists(planList, prereqFiller(thisAction, knownActions, state));

                        }
                    }
                }
            }
        }

        return planList;
    }





    public List<List<action>> prereqFiller(action thisAction, List<action> knownActions, Dictionary<string, List<stateItem>> state)
    {
        //this function takes an action with at least some unfilled prereqs
        //and returns the set of all plans that fill the unfilled prereqs
        //each ending with that action we wanted to do in the first place

        List<List<action>> planList = new List<List<action>>();

        //one planList for each prereq, then merge later:
        List<List<List<action>>> plansForEachRegularPrereq = new List<List<List<action>>>();
        List<List<action>> thePlansForLocationStatePrereq = new List<List<action>>();

        //go thoruhg ALL prereqs, need to make sure they're ALL filled:
        foreach (stateItem eachPrereq in thisAction.prereqs)
        {
            //only make plans to fill prereqs that aren't ALREADY filled:
            if (isGoalAccomplished(eachPrereq, state) == false)
            {
                //so, found a prereq that isn't filled.  Need plans to fill it!





                



                List<List<action>> plansForThisPrereq = new List<List<action>>();
                plansForThisPrereq = problemSolver(eachPrereq, knownActions, state);
                
                //if we've found zero plans, we've failed, just stop now:
                if (plansForThisPrereq.Count == 0)
                {
                    break;
                }




                //if we have plans, good, now...what?
                //well, can't add those results to the planList just yet
                //because they only fill ONE prereq
                //still need to at least CHECK that other prereqs CAN be filled
                //and also find plans to fill thos eother prereqs
                //and combine the plans in the correct way
                //so I'm guessing that's what I'm doing here?
                ////////////////////////////////////////////////////////////
                else
                {

                    //I'll sort the plans based on whether this prereq is a locationState
                    //so that later I can ensure that prereq is the one that get's fulfilled LAST
                    if (eachPrereq.stateCategory == "locationState")
                    {
                        //so add these found plans to locationStatePlans
                        //hopefully this doesn't need deep copy...
                        thePlansForLocationStatePrereq = plansForThisPrereq;
                    }
                    else
                    {
                        plansForEachRegularPrereq.Add(plansForThisPrereq);
                    }
                }
                ////////////////////////////////////////////////////////////







            }
        }
        //recursionCounter -= 1
        
        

        //ok, now should be a simple matter of creating all combinations of regular plans
        //then handle locationState plans by adding them to the ends
        //again in a combinitorial way
        //so I'll create two funcitons, one to do each of those steps
        //[[[[also, should be checking if things are empty or null or whatever...]]]]
        planList = mergePlanLists(planList, combinitorialMergingOfRegularPrereqFillers(plansForEachRegularPrereq));
        planList = combinitorialMergingOfLocationStatePrereqFillers(thePlansForLocationStatePrereq, planList);


        //now, add thisAction to the end of ALL these plans:
        planList = addActionToEndOfAllPlans(planList, thisAction);


        return planList;
    }

    public List<List<action>> mergePlanLists(List<List<action>> list1, List<List<action>> list2)
    {
        foreach(List<action> plan in list2)
        {
            list1.Add(plan);
        }

        return list1;
    }

    public List<List<action>> combinitorialMergingOfRegularPrereqFillers(List<List<List<action>>> plansForEachRegularPrereq)
    {
        //input sets of plans (that is, lists (plural) of planLists)
        //(each set fulfills a separate prereq)
        //output all possible plans, each of which fulfills ALL those prereqs

        //the top list is on item per prereq
        //so each plan needs one selection from each of these

        //if fed empty list, just return an empty dummy thing I guess?:
        if (plansForEachRegularPrereq.Count == 0)
        {
            List<List<action>> dummy = new List<List<action>>();
            return dummy;
        }


        //if there's only one prereq, then we're done already:
        if (plansForEachRegularPrereq.Count == 1)
        {
            return plansForEachRegularPrereq[0];
        }

        //if not, then we have to go recursive:
        List<List<action>> allCombosPlanList = new List<List<action>>();

        List<List<action>> plansForFirstPrereq = new List<List<action>>();
        List<List<action>> allPossiblePlansFromRemainingPrereqs = new List<List<action>>();


        //now remove first set, find all combos of the remaining prereqs
        //until base case, when only one prereq remains
        plansForFirstPrereq = plansForEachRegularPrereq[0];
        plansForEachRegularPrereq.RemoveAt(0);

        //now, do recursion, then merge:
        allPossiblePlansFromRemainingPrereqs = combinitorialMergingOfRegularPrereqFillers(plansForEachRegularPrereq);

        //now, merge.
        //each of the ways to fill the first prereq (each item in "plansForFirstPrereq")
        //is combined with EACH possible plan from allPossiblePLansFromRemainingPrereqs
        //and then we're done

        foreach (List<action> planForFirstPrereq in plansForFirstPrereq)
        {
            printPlan(planForFirstPrereq);
            foreach(List<action> planForAllOtherPrereqs in allPossiblePlansFromRemainingPrereqs)
            {
                printPlan(planForAllOtherPrereqs);
                allCombosPlanList.Add(appendPlanToEndOfOtherPlan(planForFirstPrereq,planForAllOtherPrereqs));
                
            }
        }

        return allCombosPlanList;
    }

    public List<action> appendPlanToEndOfOtherPlan(List<action> plan1, List<action> plan2)
    {
        //the second input will always end up at the end of the plan
        //maybe I don't need this function, but helps clear code I think

        List<action> completedPlan = new List<action>();
        completedPlan = plan1;

        completedPlan.AddRange(plan2);

        return completedPlan;

    }

    public List<List<action>> combinitorialMergingOfLocationStatePrereqFillers(List<List<action>> thePlansForLocationStatePrereq, List<List<action>> planList)
    {
        //so, we don't know which list is bigger
        //so we'll initialize a NEW list, and just add results to it as we go
        List<List<action>> newPlanList = new List<List<action>>();

        //[[[[also, should be checking if things are empty or null or whatever...]]]]
        //yes, so if thePlansForLocationStatePrereq is empty, just return the planList unchanged:
        if(thePlansForLocationStatePrereq.Count == 0)
        {
            return planList;
        }

        //if planList is empty, just return the other plans we have:
        if(planList.Count == 0)
        {
            return thePlansForLocationStatePrereq;
        }

        foreach (List<action> regularPlan in planList)
        {
            foreach(List<action> locationStatePlan in thePlansForLocationStatePrereq)
            {
                //now add merged result into newPLanList, making sure the locationState
                //is fulfilled at the END of each plan
                newPlanList.Add(appendPlanToEndOfOtherPlan(regularPlan, locationStatePlan));

            }
        }

        return newPlanList;
    }

    public List<List<action>> addActionToEndOfAllPlans(List<List<action>> planList, action thisAction)
    {
        foreach(List<action> plan in planList)
        {
            plan.Add(thisAction);
        }

        return planList;
    }

    public bool isThisActionDone(action thisAction, Dictionary<string, List<stateItem>>  state)
    {
        //I might eventually want to change this so that "state" is one of the inputs???

        //assume true, then check and change to false where needed
        bool tf;
        tf = true;

        //print("========================1==========================");
        //print(thisAction.name);

        int howMany;
        howMany = 1;

        foreach (stateItem effectX in thisAction.effects)
        {
            //Debug.Log("how many freaking effects?????????????");
            //Debug.Log(howMany);
            //print("2------checks if no more ______ is in _______, confirm false:");
            //print(effectX.name);
            //print(effectX.stateCategory);
            //print(effectX.inStateOrNot);
            if (isGoalAccomplished(effectX, state) == false)
            {
                //Debug.Log(effectX.name);
                //print("yyyyyyyyyyyyyyyyyyyyyyyyyyyyy");
                return false;
            }
            //print("do we get here?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????");
            howMany += 1;
        }
        //print("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        return tf;
    }

    public bool isGoalAccomplished(stateItem goal, Dictionary<string, List<stateItem>> state)
    {
        //assume false, then check and change to true where needed
        bool tf;
        tf = false;
        //print("3:  should see ''inStateOrNot'' == false");
        //Debug.Log(goal.name);
        //print(goal.inStateOrNot);
        if (goal.inStateOrNot == true)
        {
            foreach (stateItem stateI in state[goal.stateCategory])
            {


                //////////////here's the C# way to say "if this item is in this list"...ya, i twon't let you use the word "in" here:
                //oh no this is text, "name" needs to be just a stateItem!  Or something!
                //print("1111111111111111111111111111111111111111");
                //print(goal.name);
                //print(stateI.name);
                if (stateI.name == goal.name)
                {
                    tf = true;
                }
            }
        }
        if (goal.inStateOrNot == false)
        {
            //actually here I have to reverse it?
            //assume false, then if I find it, change to true?
            //ya I had to do that in my C++ code too
            tf = true;

            //print("4:  should look through each item in list of________:");
            //print(goal.name);
            //print(goal.stateCategory);
            foreach (stateItem stateI in state[goal.stateCategory])
            {

                //print("222222222222222222222222222222222222222");
                //print(goal.name);
                //print(stateI.name);
                if (stateI.name == goal.name)
                {
                    //print("whaaaaaat>>>>>>>>>>>>>>>>");
                    //print(stateI.name);
                    //print(goal.name);
                    tf = false;
                }
            }
        }
        return tf;
    }

    public bool prereqChecker(action thisAction, Dictionary<string, List<stateItem>> state)
    {
        //assume true, then check and change to false where needed
        bool tf;
        tf = true;

        foreach (stateItem prereqX in thisAction.prereqs)
        {
            //print("don't count this");
            if (isGoalAccomplished(prereqX, state) == false)
            {
                return false;
            }
        }

        return tf;
    }

    public bool prereqAndLocationChecker(action thisAction, Dictionary<string, List<stateItem>> state)
    {
        //since location is now a separate prereq, and is not always checked,
        //this funciton checks both regular prereqs AND the location one

        //first, just check the regular prereqs using my regular prereqChecker:
        if (prereqChecker(thisAction, state) == false)
        {
            return false;
        }

        //now, check the location Prereq:
        //(to be safe, check that it's not null, print error if it is)
        if (thisAction.locationPrereq != null)
        {
            if (isGoalAccomplished(thisAction.locationPrereq, state) == false)
            {
                return false;
            }
        }
        else
        {
            //hmm, it's null, print error:
            print("hmm, the locationPrereq is null, but it's being checked, is that an error?");
        }

        //if the above didn't fail, return true:
        return true;

    }

    public bool whicheverPrereqChecker(action thisAction, Dictionary<string, List<stateItem>> state)
    {
        //this funciton checks whatever prereqs an action has
        //doesn't matter if the action has a locationPrereq or not
        //it can handle both types of action

        //first, just check the regular prereqs using my regular prereqChecker:
        if (prereqChecker(thisAction, state) == false)
        {
            return false;
        }

        //now, check the location Prereq, IF THERE IS ONE:
        if (thisAction.locationPrereq != null)
        {
            if (isGoalAccomplished(thisAction.locationPrereq, state) == false)
            {
                return false;
            }
        }
        
        //if the above didn't fail, return true:
        return true;

    }





    public Dictionary<string, List<stateItem>> stateCopyer(Dictionary<string, List<stateItem>> state)
    {
        Dictionary<string, List<stateItem>> newState = new Dictionary<string, List<stateItem>>();

        foreach (string keyString in state.Keys)
        {
            List<stateItem> emptyList = new List<stateItem>();
            //newState[keyString] = state[keyString];
            newState[keyString] = emptyList;
            foreach (stateItem item in state[keyString])
            {
                newState[keyString].Add(item);
            }
        }
        return newState;
    }

    public Dictionary<string, List<stateItem>> removeStateItem(stateItem thisStateItem, Dictionary<string, List<stateItem>> state)
    {
        //just because they are not quite identical enough for .Remove to work properly I don't think

        //EDIT, turns out C# already has a way to do this.
        //though it is ugly and I don't know the syntax:
        //inventory.RemoveAll(x => x.name == myItem.name);


        //find the correct stateItem, then remove it
        foreach (stateItem eachStateItem in state[thisStateItem.stateCategory])
        {
            if (eachStateItem.name == thisStateItem.name)
            {
                state[thisStateItem.stateCategory].Remove(eachStateItem);
                break;
            }
        }

        return state;
    }

    public Dictionary<string, List<stateItem>> implementALLEffects(action currentAction, Dictionary<string, List<stateItem>> imaginaryState)
    {
        foreach (stateItem eachEffect in currentAction.effects)
        {
            if (eachEffect.inStateOrNot == true)
            {
                if (eachEffect.stateCategory == "locationState")
                {
                    imaginaryState["locationState"].Clear();
                }
                imaginaryState[eachEffect.stateCategory].Add(eachEffect);
            }
            else
            {
                imaginaryState = removeStateItem(eachEffect, imaginaryState);
                //imaginaryState[eachEffect.stateCategory].Remove(eachEffect);
            }
        }
        return imaginaryState;
    }

    public List<List<action>> simulatingPlansToEnsurePrereqs(List<List<action>> planList, List<action> knownActions, Dictionary<string, List<stateItem>> realState)
    {
        //print("111111111111111111111111111111111111111");
        foreach (List<action> eachPlan in planList)
        {
            Dictionary<string, List<stateItem>> imaginaryState = new Dictionary<string, List<stateItem>>();
            imaginaryState = stateCopyer(realState);
            //print("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            //printState(realState);
            //imaginaryState.Clear();
            //Debug.Log(realState["inventory"]);
            //imaginaryState["inventory"].Remove(money1);
            //printState(imaginaryState);
            //print("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
            int counter;
            int halt;

            counter = 0;
            halt = 0;
            //print("222222222222222222222222222222222222222222222");
            while ((counter + 1) <= eachPlan.Count)
            {
                halt += 1;
                if (halt > 20)
                {
                    break;
                }
                action currentAction;
                currentAction = eachPlan[counter];
                //print("333333333333333333333333333333333333333333333");
                //print(currentAction.name);
                if (prereqChecker(currentAction, imaginaryState) != true)
                {
                    //print("yes this should happen for ''eat'':");
                    //print(currentAction.name);
                    foreach (stateItem eachPrereq in currentAction.prereqs)
                    {
                        if (isGoalAccomplished(eachPrereq, imaginaryState) != true)
                        {
                            //print("and this should happen for ''home''");
                            //print(eachPrereq.name);
                            List<List<action>> prereqFillerList;
                            prereqFillerList = problemSolver(eachPrereq, knownActions, imaginaryState);
                            //print("should have found this plan to fill the prereq:");
                            //printPlan(prereqFillerList[0]);


                            if (prereqFillerList.Count > 0)
                            {
                                //see python code for why this part is unfinished code
                                foreach (action eachAction in prereqFillerList[0])
                                {
                                    eachPlan.Insert(counter, eachAction);
                                }
                                counter += 1;
                            }
                            else
                            {
                                counter += 1;
                                planList.Remove(eachPlan);

                            }
                        }
                    }
                }

                imaginaryState = implementALLEffects(currentAction, imaginaryState);
                counter += 1;
            }

        }
        //print("xxxxxxxxxxxxxxxxxxxxxTHE MOMENT OF TRUTHxxxxxxxxxxxxxxxxxxxxxxxxx");
        //printState(realState);
        //print("xxxxxxxxxxxxxxxxxxxxxvvvvvvvvvvvvvvvvvvvvvvvvvvvvxxxxxxxxxxxxxxxxxxxxxxxx");
        return planList;
    }

    public int findFirstImpossibleAction(List<action> plan, List<action> knownActions, Dictionary<string, List<stateItem>> realState)
    {
        //imagines it's way through a plan list
        //returns the index number of the first aciton on that list that CANNOT be completed
        //if all actions can be completed fine, it returns negative two

        int noProblem;
        noProblem = -2;

        int counter;
        counter = 0;

        Dictionary<string, List<stateItem>> imaginaryState = new Dictionary<string, List<stateItem>>();
        imaginaryState = stateCopyer(realState);

        

        foreach(action currentAction in plan)
        {
            if (prereqChecker(currentAction, imaginaryState) != true)
            {
                //print("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                //print(currentAction.name);
                return counter;
            }

            imaginaryState = implementALLEffects(currentAction, imaginaryState);
            counter += 1;
        }


        return noProblem;
    }

    

}
