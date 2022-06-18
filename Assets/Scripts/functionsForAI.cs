using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class functionsForAI : MonoBehaviour
{
    [SerializeField]
    Transform _destination;
    public NavMeshAgent _navMeshAgent;

    //I don't remember making this, why is it here instead of in the function where I use it?
    //probably carried over from some tutorial?  On navmesh?
    private GameObject t1;

    //maybe ad-hoc for now:
    public int stopwatch;
    public int effectivenessTimer;

    //VERY ad-hoc for now:
    public int workerCount;
    public List<GameObject> listOfCashiers = new List<GameObject>();


    public AI1 thisAI;// = GetComponent<AI1>();
    public premadeStuffForAI premadeStuff;

    public Dictionary<string, List<GameObject>> globalTags;

    // Start is called before the first frame update
    void Start()
    {
        

        thisAI = GetComponent<AI1>();
        premadeStuff = GetComponent<premadeStuffForAI>();

        stopwatch = 0;
        effectivenessTimer = 0;
        workerCount = 0;

        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        //getting the "global" tags:
        GameObject theWorldObject = GameObject.Find("World");
        worldScript theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        globalTags = theWorldScript.taggedStuff;
    }



    ////////////////////////////////////////////////
    //                  SENSING
    ////////////////////////////////////////////////

    //handles ALL sensing:
    public void sensing(action nextAction, GameObject target, Dictionary<string, List<stateItem>> state)
    {
        
        if (areAllForbiddenZonesClear() == false)
        {
            //print("yo");

            //so, sensed someone in the forbiddenZone
            //but

            //ok, so found a threat in a forbiddenZone
            //but before we record that threat, first check if we ALREADY have that threat recorded:
            if(isStateAccomplished(premadeStuff.threat1, state) == false)
            {
                //ok, now we know we won't be adding a duplicate, here we go:
                state["threatState"].Add(premadeStuff.threat1);



                //print("yo");
            }
        }




        /*
        if(nextAction.name == "workAsCashier" || nextAction.name == "hireSomeone")
        {
            //characters doing htese actions must check the "forbiddenZone" of the store they are working at
            //so I need to get that zone to check

            //for now maybe ad-hoc, these characters will be targeting part of their store.  
            //Can use that to find the other part of the store we are interested in
            GameObject mapZoneToCheck = getMapZoneOfForbiddenZone(target);

            //then, need to check if ANYONE is in that zone:
            listOfTouchingNPCs listOfAnyone = mapZoneToCheck.GetComponent<listOfTouchingNPCs>();
            if (listOfAnyone.theList.Count > 0)
            {
                //print("yo");
                //whoever = listOfNPCs.theList[0];

                //so, sensed someone in the forbiddenZone
                //need to update threatState with threat1:
                //soooo, how to do that...
                //THIS WILL ADD THEM AN INFINIE NUMBER OF TIMES, NEED TO ONLY ADD IT IF IT IS ABSENT!
                state["threatState"].Add(premadeStuff.threat1);
                //print("in sensing phase:  " + target.name);
            }
            
        }
        */


    }




    ////////////////////////////////////////////////
    //                  ACTIONS
    ////////////////////////////////////////////////

    //handles the enactment of ALL actions:
    public GameObject doNextAction(action nextAction, Dictionary<string, List<stateItem>> state, GameObject target, List<action> ineffectiveActions)
    {

        //handle the travel prereqs here:
        if (nextAction.locationPrereq != null)
        {
            //here we automatically try to fill all travel prereqs

            //will go to navigationTarget, so check if we have one:  chooseTarget
            if (target == null)
            {
                //need to get a target:
                target = chooseTarget(nextAction.locationPrereq);
            }
            if (target == null)
            {
                print("the chooseTarget function returned null, for the following action:");
                print(nextAction.name);
            }
            travelToTargetObject(target);
            

            //I still currently use a "stateItem", but it could perhaps
            //be replaced with mere text?  The name of the location?

            //travelToStateItem(nextAction.locationPrereq);
        }



        //actions with ALL prereqs met (including location prereq) can proceed below:
        if (whicheverprereqStateChecker(nextAction, state, target) == true)
        {

            //if(gameObject.name == "NPC")


            
            if (nextAction.type == "buyFromStore")
            {
                //print("sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss");
                //need this stuff to check if cashier is there.  Otherwise, can't buy anything.
                //if casheir isn't there, shouldn't wait forever, that is unrealistic, but right now that's what will happen

                //GameObject customerLocation = getLocationObject(state["locationState"][0].name);
                GameObject customerLocation = target;

                //print("target name is:::::::::::::::::::::::::::::");
                //print(target.name);

                GameObject cashierMapZone = getCashierMapZone(customerLocation);
                
                GameObject cashier = whoIsTrader(cashierMapZone);

                //but that cashier variable might come back null (if no one is there), check:
                if (cashier != null)
                {
                    //ad-hoc update of state:
                    state = implementALLEffects(nextAction, state);

                    //thisAI.toDoList.RemoveAt(0);
                    target = dumpAction(target);
                }
                
            }
            else if (nextAction.name == "hireSomeone")
            {
                //ad-hoc for now

                //find someone (and NPC, for now) to hire,
                //then change their knownActions, remove "doTheWork", add "workAsCashier"


                //**********UMMMMM, how does this know WHICH STORE???  Is it wrong???*****************
                //get the cashierLocation:
                //print("1) " + target.name);
                //GameObject cashierLocation = getLocationObject("cashierZone");  //**********UMMMMM, how does this know WHICH STORE???  Is it wrong???*****************
                //print("2) " + cashierLocation.name);
                //**********UMMMMM, how does this know WHICH STORE???  Is it wrong???*****************

                //**********UMMMMM, how does this know WHICH STORE???  Is it wrong???*****************


                //get the checkoutZone:
                GameObject checkoutZone = getCheckoutMapZone(target.transform.parent.gameObject);
                //print("3) " + checkoutZone.name);

                //check for an NPC customer in the checkoutZone:
                GameObject customer = checkForCustomer(checkoutZone);
                if (customer != null && customer.name != "NPC shopkeeper" && customer.name != "NPC shopkeeper (1)")
                {
                    //ad-hoc way to hire more than one employee for now:
                    //if (listOfCashiers.Contains(customer) == false)
                    AI1 customerAI = customer.GetComponent("AI1") as AI1;
                    if (customerAI.jobSeeking == true)
                    {
                        customerAI.jobSeeking = false;

                        listOfCashiers.Add(customer);
                        changeRoles(customer, premadeStuff.workAsCashier, premadeStuff.doTheWork);

                        print(customer.name);
                        

                        workerCount += 1;

                        //print(workerCount);

                        //need the worker to show up at the correct store for their shift:
                        //customerAI.roleLocation = thisAI.roleLocation;
                        string ownershipTag = "owned by " + this.name;
                        //need cashierZone of the owned store:
                        customerAI.roleLocation = randomTaggedWithMultiple("shop", ownershipTag);

                        //Increase the "clearance level" of the worker:
                        //BIT ad-hoc.  Characters might have different clearance levels for different places/factions etc.  Right now I just have one.
                        customerAI.clearanceLevel = 1;


                        //ad-hoc way to hire more than one worker for now:
                        if (workerCount > 1)
                        {
                            //also, to easily put the "employee1" stateItem in the organizationState:
                            state = implementALLEffects(nextAction, state);
                            //^^^^^^^^^^that will ALSO be seen, and thus the "hire" aciton will be removed from to-do list

                            
                            //and ad hoc strike this action off the to-do list:
                            //thisAI.toDoList.RemoveAt(0);

                            target = dumpAction(target);
                        }
                    }
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


                
                //now do the pickpocketing
                AI1 theTargetState = victim.GetComponent("AI1") as AI1;
                steal(state["inventory"], theTargetState.state["inventory"], nextAction);


                //ad-hoc action completion:
                //thisAI.toDoList.RemoveAt(0);

                target = dumpAction(target);

                //state = implementALLEffects(nextAction, state);





                /*
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
                */


                //print(target.name);
            }
            else if (nextAction.name == "workAsCashier")
            {
                //very ad-hoc for now
                //just want the worker to wait there for a while
                //"doing their work shift"
                

                
                //ad-hoc check if someone else is the cashier right now:
                GameObject thisNPC = gameObject;
                //GameObject theCashierZone = getLocationObject("cashierZone");
                //but the actual "mapZone" is a CHILD of this casheirZone object:
                //GameObject cashierMapZone = theCashierZone.GetChild(0).gameObject;
                //GameObject theCashierZone = getLocationObject("cashierZone");
                //but the actual "mapZone" is a CHILD of this casheirZone object:
                GameObject cashierMapZone = getCashierMapZone(target.transform.parent.gameObject);

                    
                GameObject currentCashier = getWhoeverIsHereFirst(cashierMapZone);
                if (currentCashier == thisNPC)
                {
                    stopwatch += 1;

                    //ad-hoc work shift timer:
                    if (stopwatch > 1000)
                    {
                        state = implementALLEffects(nextAction, state);
                        //thisAI.toDoList.RemoveAt(0);
                        target = dumpAction(target);
                        stopwatch = 0;

                        //ya this doesn't work because my check in AI1 still sees ...prereqs are done?
                        //print("ggggggggggggggggggggggggggggggggggggggggggggggggggggg");
                    }
                }
                
                
            }
            else if (nextAction.type == "buyThisProperty")
            {
                
                //kinda ad-hoc
                //"buy" the property that the NPC has arrived at
                //can use for both buying shops, and buying homes, maybe

                //get this property:
                GameObject thisProperty;
                thisProperty = null;
                thisProperty = target;

                //print("cccccccccccccccccccccccccccccccccccccccccccccccccccccccccc");

                //check if it's for sale:
                //get other script I need:
                taggedWith otherIsTaggedWith = target.GetComponent<taggedWith>() as taggedWith;
                if (otherIsTaggedWith.tags.Contains("forSale"))
                {
                    //ok, it's for sale, now can buy it

                    //printTextList(otherIsTaggedWith.tags);
                    //remove the "for sale" tag:
                    otherIsTaggedWith.foreignRemoveTag("forSale", target);
                    //printTextList(otherIsTaggedWith.tags);
                    //add the "owned by _____" tag...:
                    string ownershipTag = "owned by " + this.name;
                    otherIsTaggedWith.foreignAddTag(ownershipTag, target);

                    //need to remember in the future WHICH store is theirs
                    //so they ca go to it, and sned their employees there:
                    //thisAI.roleLocation = target;

                    //ad-hoc action completion:
                    //thisAI.toDoList.RemoveAt(0);

                    target = dumpAction(target);

                    //ad-hoc update of state:
                    state = implementALLEffects(nextAction, state);

                }
                
                
                else
                {
                    //well, this one is NOT for sale, so need to scrap WHOLE plan, I think...
                    //AND make sure not to just generate the same plan infinitely, somehow.  
                    //Not sure how...memory?  Add to list of temporarily unworkable plans?
                    //then can use that info in the "choosing among plans" phase I don't yet have?

                    //for now, just treat it as a completed action
                    //this will simply remove the action, and next frame the AI will 
                    //detect an impossible plan, and try again
                    //not ideal if there were several to choose from, could end up going back to
                    //ones that are not for sale several times
                    //needs to LEARN?  Or prevent this with better foreknowledge.  But already has foreknowledge using tags, I think
                    //when the plan was formed, the foreknowledge was correct.  Became incorrect on the way there.
                    //so if tags are knowledge, they will have ALREADY "learned" due to the tag being removed.
                    //thisAI.toDoList.RemoveRange(0, thisAI.toDoList.Count);
                    target = dumpAction(target);
                }
                


            }
            else if (nextAction.name == "handleSecurityMild")
            {
                //print("yoooooooo");
                if (effectivenessTimer == 0)
                {
                    print("handling security mild");
                }

                //for escalation:
                effectivenessTimer += 1;
                if (effectivenessTimer > 200)
                {
                    print("INEFFECTIVE!!!");

                    //mark this as an "ineffective action"
                    ineffectiveActions.Add(nextAction);

                    //then end this current plan/action:
                    target = dumpAction(target);


                    //MAYBE DOESN'T MAKE SENSE TO RESET THIS RIGHT AWAY?  Dunno.  But for now:
                    effectivenessTimer = 0;
                }

                if (areAllForbiddenZonesClear() == true)
                {
                    print("DONE handling mild security");
                    state = implementALLEffects(nextAction, state);
                    target = dumpAction(target);

                    effectivenessTimer = 0;
                }
            }
            else if (nextAction.name == "handleSecurityEscalationOne")
            {
                if (effectivenessTimer == 0)
                {
                    print("handling security ESCALATON");
                }
                effectivenessTimer += 1;

                if (areAllForbiddenZonesClear() == true)
                {
                    print("escalation over...");
                    state = implementALLEffects(nextAction, state);
                    target = dumpAction(target);

                    effectivenessTimer = 0;
                }
            }
            else
            {
                //for actions like "eat" that currently just need a quick
                //ad-hoc update of state:
                state = implementALLEffects(nextAction, state);
                //thisAI.toDoList.RemoveAt(0);
                target = dumpAction(target);
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

    public GameObject dumpAction(GameObject target)
    {
        //when action is done, remove the action from the plan, and set target to null
        target = null;
        thisAI.toDoList.RemoveAt(0);

        return target;
    }
    
    public GameObject getLocationObject(string nameOfLocation)
    {
        //is this redundant?  Whatever.  I might change how it works later...
        //ya, later I want this to get the object based on TOUCH, not name...

        GameObject locationObject = GameObject.Find(nameOfLocation);



        return locationObject;

    }

    public void travelToTargetObject(GameObject target)
    {

        Vector3 targetVector = target.GetComponent<Transform>().position;
        _navMeshAgent.SetDestination(targetVector);
    }

    public AI1 getHubScriptFromGameObject(GameObject NPC)
    {
        AI1 theHub = NPC.GetComponent("AI1") as AI1;

        return theHub;
    }

    public GameObject getWhoeverIsHereFirst(GameObject locationZone)
    {
        //looks at a location
        //if someone is there, return them as a GameObject
        //(if more than one is there, return FIRST one)

        GameObject whoever;
        whoever = null;

        listOfTouchingNPCs listOfNPCs = locationZone.GetComponent<listOfTouchingNPCs>();

        //check to make sure the list isn't empty:
        if (listOfNPCs.theList.Count > 0)
        {
            whoever = listOfNPCs.theList[0];
        }


        return whoever;
    }

    
    //pretty ad-hoc

    public bool areAllForbiddenZonesClear()
    {
        //check all forbiddenZones in senseZone, return true or false

        //ALSO USED IN "SENSING" MAYBE!!!

        //first gett he senseZone:
        GameObject thisSenseZone = namedChild(this.gameObject, "senseZone");

        //then get the script on the senseZone:
        senseZoneScript thisSenseZoneScript = thisSenseZone.GetComponent<senseZoneScript>();

        //if(thisSenseZoneScript.listOfForbiddenZones.Count > 0)


        //check each forbiddenZone:
        foreach (GameObject thisForbiddenZone in thisSenseZoneScript.listOfForbiddenZones)
        {
            //now need to check the "listOfTouchingNPCs" script on the "mapZone" object that is the CHILD of this forbiddenZone...

            //get the mapZone that is the child of this forbiddenZone
            GameObject thismapZone = namedChild(thisForbiddenZone, "mapZone");

            //get the "listOfTouchingNPCs" script that is attached to the mapZone:
            listOfTouchingNPCs thisListOfTouchingNPCs = thismapZone.GetComponent<listOfTouchingNPCs>();

            //whew!

            //now, check if ANYONE is in that map zone:
            if (thisListOfTouchingNPCs.theList.Count > 0)
            {
                //BUT need to check if they have CLEARANCE:
                foreach(GameObject thisListedNPC in thisListOfTouchingNPCs.theList)
                {
                    //need to check their clearance level...

                    AI1 thisListedNPCAI = thisListedNPC.GetComponent("AI1") as AI1;

                    //only detect threat if their clearance level is less than one:
                    if (thisListedNPCAI.clearanceLevel < 1)
                    {
                        return false;
                    }
                }
                
            }
        }

        //if that didn't find anything, then it's clear.  Return true:
        return true;
    }

    ////////////////////////////////////////////////
    //                TARGETING
    ////////////////////////////////////////////////

    public GameObject chooseTarget(stateItem criteria)
    {
        //takes criteria, returns one target
        //for now, input is a stateItem from nextAction.locationPrereq
        //output is a GameObject


        string name1 = criteria.name;
        GameObject target;
        target = null;

        if (criteria.locationType == "mobile")
        {
            //for now, just the pickpocket action?  Move that stuff here...
            target = whoToTarget();

        }
        else if (criteria.locationType == "any")
        {
            //print("any");
            //sorta ad-hoc for now...
            if (criteria.name == "anyStore")
            {
                //print("anyStore");
                //get any store:
                //target = anyStoreForSale();
                target = randomTaggedWithMultiple("shop", "forSale");
            }
            else if (criteria.name == "anyHome")
            {
                //print("anyHome");
                //get any store:
                //target = anyStoreForSale();
                target = randomTaggedWithMultiple("home", "forSale");
            }
            else if (criteria.name == "checkout")
            {
                //print("checkout");
                //get any store:
                //print("hello?????????????????????????????");
                target = anyCheckout();
            }
        }
        else if (criteria.locationType == "roleLocation")
        {
            //print("else if (criteria.name == roleLocation)");
            //just target their role location variable?
            //target = thisAI.roleLocation;
            //no, that variable just goes to the store building
            //I need them to go to the cashierZone IN that store
            //so:
            target = getCashierMapZoneOfStore(thisAI.roleLocation);

        }
        else if (criteria.name == "hiringZone")
        {
            string ownershipTag = "owned by " + this.name;
            //need cashierZone of the owned store:
            target = getCashierMapZoneOfStore(randomTaggedWithMultiple("shop", ownershipTag));
        }
        else if (criteria.name == "home")
        {
            //print("else if (criteria.name == home)");

            //dummy test:
            //string ownershipTag = "owned by " + this.name;
            //target = DUMMYrandomTaggedWithMultipleDUMMY(criteria.name, ownershipTag);

            //just target their homelocation variable
            //target = thisAI.homeLocation;


            //new method based on tagging
            //find the home they own:
            string ownershipTag = "owned by " + this.name;
            target = randomTaggedWithMultiple(criteria.name, ownershipTag);  //yes, using "random" is redundant here, but it's my only function right now
        }
        else
        {
            //print("else");
            //for now, "else" should all be mapZone things we can find like this?
            target = GameObject.Find(name1);
        }
        

        //stuff to get the target...




        return target;
    }


    //find objects using tags:
    public GameObject randomTaggedWith(string theTag)
    {
        //should return ONE random GameObject that is tagged with the inputted tag

        List<GameObject> allPotentialTargets = new List<GameObject>();

        allPotentialTargets = globalTags[theTag];



        if (allPotentialTargets.Count > 0)
        {
            GameObject thisObject;
            thisObject = null;
            int randomIndex = Random.Range(0, allPotentialTargets.Count);
            thisObject = allPotentialTargets[randomIndex];
            return thisObject;
        }
        else
        {
            return null;
        }
    }
    
    public GameObject randomTaggedWithMultiple(string theTag, string tag2 = null, string tag3 = null, string tag4 = null)
    {
        //should return ONE random GameObject that is tagged with ALL inputted tags
        List<GameObject> allPotentialTargets = new List<GameObject>();
        allPotentialTargets = globalTags[theTag];

        //BUT THAT'S A SHALLOW COPY!
        //so I need to make a corrosponding list of indices to use, to prevent messing with it:
        List<int> listOfIndices = new List<int>();
        int length = 0;
        //WILL THIS MAKE IT THE RIGHT NUMBER?  OR ONE TOO MANY?  ONE TOO FEW???
        //I think it's correct now?
        while (length < allPotentialTargets.Count)
        {
            listOfIndices.Add(length);
            length += 1;
        }

        //put the optional other tags in a list:
        List<string> otherTags = new List<string>();
        otherTags.Add(tag2);
        otherTags.Add(tag3);
        otherTags.Add(tag4);

        

        GameObject thisObject;
        thisObject = null;
        bool doWeHaveGoodTarget = false;

        int randomNumber;
        int myIndex;

        //print("do we have even ONE of these????");
        //print(theTag);
        //print(allPotentialTargets.Count);
        while (doWeHaveGoodTarget == false && listOfIndices.Count > 0)
        {
            //print("yes at least one");
            //grab a randomn object from the list:
            randomNumber = Random.Range(0, listOfIndices.Count);
            myIndex = listOfIndices[randomNumber];

            thisObject = allPotentialTargets[myIndex];


            //now, check all the other tags on that^ object
            //if it lacks a needed tag, remove that item from the array
            //and choose again
            //to do that, need to grab the tags on that object:
            taggedWith theTagScript = thisObject.GetComponent("taggedWith") as taggedWith;

            //assume this object will correctly have ALL the tags
            //then falsify by checking:
            doWeHaveGoodTarget = true;

            
            foreach (string thisTag in otherTags)
            {
                //make sure it's not null:
                if (thisTag != null)
                {
                    
                    if (theTagScript.tags.Contains(thisTag) == false)
                    {
                        //print("grrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
                        //print(thisTag);

                        doWeHaveGoodTarget = false;
                        listOfIndices.RemoveAt(randomNumber);
                        break;
                    }
                    /*
                    else
                    {
                        print("YAAAAAAAAAAAYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
                        print(thisTag);
                    }
                    */
                }
            }

            //see if the object passed the test:
            if (doWeHaveGoodTarget == true)
            {
                return thisObject;
            }
        }

        //this will be null if the above loop didn't find one:
        return thisObject;
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
        if (listOfNPCs.theList.Count > 0)
        {
            if (listOfNPCs.theList[0].name != "NPC pickpocket")
            {
                customer = listOfNPCs.theList[0];
            }

        }


        return customer;

    }


    //ad-hoc:
    public GameObject anyStoreForSale()
    {
        //ad-hoc for now, this is being used in buyStore action
        //should return ONE random shop GameObject as a target that is tagged with "forSale"

        List<GameObject> allPotentialTargets = new List<GameObject>();

        //now to find suitable targets using my new tagging system:
        allPotentialTargets = globalTags["shop"];

        GameObject thisShop;
        thisShop = null;
        bool doWeHaveGoodTarget = false;

        while (doWeHaveGoodTarget == false && allPotentialTargets.Count > 0)
        {
            int randomIndex = Random.Range(0, allPotentialTargets.Count);
            thisShop = allPotentialTargets[randomIndex];

            //print(thisShop.name);

            //but, criteria, ad-hoc for now
            //if it's wrong criteria, remove that item from the array (will that leave a "null" hole in array???)
            //and choose again
            //check if it is for sale
            taggedWith theTagScript = thisShop.GetComponent("taggedWith") as taggedWith;
            if (theTagScript.tags.Contains("forSale") == false)
            {
                allPotentialTargets.RemoveAt(randomIndex);
                thisShop = null;
            }
            else
            {
                doWeHaveGoodTarget = true;
            }
        }


        return thisShop;
    }

    public GameObject anyStore()
    {
        //should return ONE random shop GameObject as a target
        
        GameObject thisShop;
        thisShop = null;

        thisShop = randomTaggedWith("shop");

        return thisShop;
    }


    //getting locations:

    public GameObject getOppositeZone(GameObject target)
    {
        //returns the "mapZone" object on the opposite side of the counter
        //needs to know two things:
        //1) which store to look in
        //2) which side of the counter are we on right now?
        //I think the current "target" will ALWAYS be the answer to both simultaneously
        return null;

    }

    public GameObject getCashierMapZone(GameObject customerLocation)
    {
        GameObject cashierMapZone;
        cashierMapZone = null; //just in case none is found
        GameObject locationParent = customerLocation.transform.parent.gameObject;


        //gets the named object ("checkout") itself, 
        //then uses that to get the "mapZone" that is a "child" of that checkout:
        cashierMapZone = getMapZoneChildOfObject(namedChild(locationParent, "cashierZone"));

        //check results:
        if (cashierMapZone == null)
        {
            print("cashier mapZone not found, perhaps it is not a child of the customer location's parent object");
        }


        return cashierMapZone;

    }

    public GameObject getCashierMapZoneOfStore(GameObject theStore)
    {
        //this function just needs to know which store to look in, then finds the cashierZone in that store
        //print(theStore.name);

        GameObject cashierMapZone;
        cashierMapZone = null; //just in case none is found

        //gets the named object ("cashierZone"):
        //then uses that to get the "mapZone" that is a "child" of that cashierZone:
        cashierMapZone = getMapZoneChildOfObject(namedChild(theStore, "cashierZone"));

        //check results:
        if (cashierMapZone == null)
        {
            print("cashierMapZone not found, ...");
        }


        return cashierMapZone;

    }


    public GameObject getCheckoutMapZone(GameObject cashierLocation)
    {
        GameObject checkoutZone;
        checkoutZone = null; //just in case none is found, the above won't work, trying to return it will have compile error
        GameObject locationParent = cashierLocation.transform.parent.gameObject;

        //now search for the correct "child" object:
        //wait, doesn't Unity have a way to do this?  Haven't I done this elsewhere???

        //gets the named object ("checkout") itself, 
        //then uses that to get the "mapZone" that is a "child" of that checkout:
        checkoutZone = getMapZoneChildOfObject(namedChild(locationParent, "checkout"));

        //check results:
        if (checkoutZone == null)
        {
            print("checkout zone not found, perhaps it is not a child of the cashier zone's parent object");
        }

        return checkoutZone;

    }



    //re-used bits for getting locations

    public GameObject namedChild(GameObject theHighestParent, string nameOfChild)
    {
        //recursive function to simply search ALL child objects, brute force
        //return the ONLY child with the given name (or first one found)

        foreach (Transform child in theHighestParent.transform)
        {
            if (child.name == nameOfChild)
            {
                return child.gameObject;
            }
            else
            {
                //depth-first search, go recursive (AKA "deeper") immediately:
                GameObject deeperSearch = namedChild(child.gameObject, nameOfChild);
                if (deeperSearch != null)
                {
                    return deeperSearch;
                }
            }
        }

        //if that search hasn't returned anyting, we can only return null:
        return null;
    }

    public GameObject getMapZoneChildOfObject(GameObject theObject)
    {
        if (theObject != null)
        {
            return theObject.transform.GetChild(0).gameObject;
        }
        else
        {
            return null;
        }
    }



    public GameObject getMapZoneOfForbiddenZone(GameObject targetInStore)
    {
        //input is any target that is in the store
        //this function will crawl up the parent tree and find the "forbiddenZone"
        //then it will get the "mapZone" ATTACHED TO that "forbiddenZone", and return it


        //so, FIRST, crawl ALL THE WAY up the ladder of parent objects until you reach the one called "store":
        GameObject ladderClimb;
        ladderClimb = targetInStore;  //hopefully this won't have "shallow copy" issues...
        bool atTop = false;

        //CURRENT DANGER OF INFINITE LOOP
        //THE BOOLEAN IS NEVER SWITCHED
        //ASSUMES IT WILL **ALWAYS** FIND WHAT IT IS LOOKING FOR
        while (atTop == false)
        {
            //is there a better way to do this?  I'm afraid of an infinite loop here...

            //each loop, look if object has a "Tagged With" script
            //if so, see if it is tagged with "shop"
            //if so, that's the object we want, it's the store object (SET atTop to TRUE!)

            //look if object has a "Tagged With" script:
            //if (ladderClimb.GetComponent<TaggedWith>() != null)
            //so, see if it is tagged with "shop"
            //need to get the tags to check


            //nevermind all that, I'll just use my "get named child" function:
            GameObject thisResult = namedChild(ladderClimb, "forbiddenZone");
            if (thisResult != null)
            {
                //found it

                //now return the mapZone ATTACHED to this:
                return getMapZoneChildOfObject(thisResult);
            }
            else
            {
                //didn't find it, go up one in object tree
                ladderClimb = ladderClimb.transform.parent.gameObject;
            }
            
        }

        //second, find the child object called "forbiddenZone":

        //third, get the mapZone attached to that "forbiddenZone":


        //if nothing is found:
        return null;
    }


    public GameObject anyCheckout()
    {
        //ad-hoc for now, this is being used in buyFood action
        //should return ONE random checkoutZone GameObject as a target

        //first, use anyStore() to find a store, then use namedChild
        //to get the checkout...


        GameObject thisShop;
        thisShop = null;
        thisShop = anyStore();


        //now get the checkoutZone of that shop:
        GameObject thisCheckout;
        thisCheckout = null;
        //print("here, right??????????????????????????");
        thisCheckout = namedChild(thisShop, "checkout");


        return thisCheckout;
    }

    //old targeting:
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


    ////////////////////////////////////////////////
    //             POST-PLANNING PHASE
    ////////////////////////////////////////////////

    public List<List<action>> planRanker(List<List<action>> unsortedPlans)
    {
        //takes a list of unsorted plans
        //evaluates the "cost" of each plan
        //organizes the plans from cheapest to most costly

        List<List<action>> rankedPlans = new List<List<action>>();

        List<int> unsortedCosts = new List<int>();

        foreach (List<action> thisPlan in unsortedPlans)
        {
            //calculate the cost of this plan

            int cost = costFinder(thisPlan);
            unsortedCosts.Add(cost);
        }

        //now I have two lists (unsortedPlans and unsortedCosts)
        //they are unsorted but in the SAME order
        //how to sort them?
        //make a function that creates a NEW list, a list of index integers in the correct order
        //then go through each item in THAT list, use it to call indexed items from
        //the unsorted plans list, add them in correct order to a new list

        List<int> sortedIndexes = findIndexOrder(unsortedCosts);

        //now just add the PLANS to the sorted list ("rankedPlans"), called by index from unsortedPlans, in order of the sortedIndexes:
        foreach (int thisIndexToCall in sortedIndexes)
        {
            rankedPlans.Add(unsortedPlans[thisIndexToCall]);
        }


        return rankedPlans;
    }


    public List<int> findIndexOrder(List<int> unsortedCosts)
    {
        //given unsorted list of costs
        //find out the sequence they SHOULD be in
        //return a list of indexes in that order

        List<int> unsortedListOfIndexes = new List<int>();
        List<int> sortedListOfIndexes = new List<int>();

        Dictionary<int, int> indexAligner = new Dictionary<int, int>();

        //make this list of indexes the 

        //"deep copy" of the unsorted costs:
        List<int> copyCosts = new List<int>();
        foreach(int thisCost in unsortedCosts)
        {
            copyCosts.Add(thisCost);
        }

        //just sorting the costs, not fancy for now:
        copyCosts.Sort();

        //NOW FOR THE TRICKY PART

        //add the indexes as a value in a dictionary wher the keys are in order, 1 2 3 4 etc.
        foreach(int thisCost in copyCosts)
        {
            //so, adding this to the dictionary will be done in order
            //so we'll have a counter so I always know the key to give the 
            int thisIndex = unsortedCosts.IndexOf(thisCost);

            //need to check if this is duplicate
            while (sortedListOfIndexes.Contains(thisIndex))
            {
                //ok, need to find NEXT instance of thisCost
                int offset = thisIndex + 1;
                thisIndex = unsortedCosts.IndexOf(thisCost, offset);
            }

            sortedListOfIndexes.Add(thisIndex);

        }



        /*
        //now, create the sorted list of indexes:
        foreach (int thisCost in copyCosts)
        {
            //check which index it is in the UNsorted list
            //then add that index to the sorted list of indexes

            int thisIndex = unsortedCosts.IndexOf(thisCost);

            //try adding

            //but there might be duplicates, so... 
            while ()
            if(sortedListOfIndexes.Contains()

            sortedListOfIndexes.Add();
        }
        */

        return sortedListOfIndexes;
    }

    public int costFinder(List<action> plan)
    {
        //finds the cost of a plan
        //plan is input, integer cost is output

        int cost = 0;

        foreach(action thisAction in plan)
        {
            cost += thisAction.cost;
        }

        return cost;
    }






    ////////////////////////////////////////////////
    //                  TESTS
    ////////////////////////////////////////////////

    public void testIndexListSortedThing()
    {
        //create my test list
        //print results of my function
        //then I can check if it matches the example(s) I did by hand

        //test list:
        List<int> testList = new List<int>();
        testList.Add(8);
        testList.Add(3);
        testList.Add(3);
        testList.Add(3);
        testList.Add(5);
        testList.Add(6);

        List<int> testResult = findIndexOrder(testList);
        printNumberList(testResult);

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

    public void printTextList(List<string> theList)
    {
        string printout = string.Empty;

        foreach (string listItem in theList)
        {
            printout += listItem + ", ";
        }

        print(printout);
    }

    public void printNumberList(List<int> numberList)
    {
        string printout = string.Empty;
        printout += "[";

        foreach (int number in numberList)
        {
            printout += " " + number + " ";
        }
        printout += "]";

        print(printout);
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
        if (isStateAccomplished(goal, state) == false)
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
                        if (prereqStateChecker(thisAction, state))
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
            if (isStateAccomplished(eachPrereq, state) == false)
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

        //the top list is one item per prereq
        //so each plan needs one selection from each of these

        //if fed empty list, just return an empty dummy thing I guess?:
        if (plansForEachRegularPrereq.Count == 0)
        {
            List<List<action>> dummy = new List<List<action>>();
            return dummy;
        }


        //if there's only one prereq, then we're done already (base case):
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
            //printPlan(planForFirstPrereq);
            foreach(List<action> planForAllOtherPrereqs in allPossiblePlansFromRemainingPrereqs)
            {
                //printPlan(planForAllOtherPrereqs);
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
        //this function is unused now that I don't plan to fill locationPrereqs


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


    //Prereq and goal checking stuff:
    
    public bool isStateAccomplished(stateItem goal, Dictionary<string, List<stateItem>> state)
    {
        //assume false, then check and change to true where needed
        bool tf;
        tf = false;
        //print("3:  should see ''inStateOrNot'' == false");
        //Debug.Log(goal.name);
        //print(goal.inStateOrNot);

        //do stuff as if "wantedInstateOrNot" == true, then can reverse it later if it's false
        
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
        
        //just reverse that result if it's not wanted
        if (goal.inStateOrNot == false)
        {
            //here I can just reverse WHATEVER result the other check gave?
            if (tf == false)
            {
                tf = true;
            }
            else
            {
                tf = false;
            }

            /*
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
            */
        }

        return tf;
    }
    
    public bool prereqStateChecker(action thisAction, Dictionary<string, List<stateItem>> state)
    {
        //assume true, then check and change to false where needed
        bool tf;
        tf = true;

        foreach (stateItem prereqX in thisAction.prereqs)
        {
            //print("don't count this");
            if (isStateAccomplished(prereqX, state) == false)
            {
                
                return false;
            }
        }

        return tf;
    }
    
    public bool whicheverprereqStateChecker(action thisAction, Dictionary<string, List<stateItem>> state, GameObject target)
    {
        //this function checks whatever prereqs an action has
        //doesn't matter if the action has a locationPrereq or not
        //it can handle both types of action

        //first, just check the regular prereqs using my regular prereqStateChecker:
        if (prereqStateChecker(thisAction, state) == false)
        {
            
            return false;
        }

        //now, check the location Prereq, IF THERE IS ONE:
        if (thisAction.locationPrereq != null)
        {
            if (locationPrereqChecker(target) == false)
            {
                
                return false;
            }
        }
        //print(thisAction.name);
        
        //if the above didn't fail, return true:
        return true;

    }

    public bool locationPrereqChecker(GameObject target)
    {
        //for now just copy the proximity check from pickpocket action?

        //check distance
        GameObject thisNPC = gameObject;
        float distance = Vector3.Distance(thisNPC.transform.position, target.transform.position);
        //print("distance, for this target:");
        //print(target.name);
        //print(distance);
        if (distance < 3.5f)
        {
            return true;
        }
        else
        {
            return false;
        }

        /*
        if (goal.inStateOrNot == false)
        {
            //here I can just reverse WHATEVER result the other check gave?
            if (tf == false)
            {
                tf = true;
            }
            else
            {
                tf = false;
            }
        }
        */
    }



    //"imagination" stuff:

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
                //*****EVENTUALLY might want to check MORE than just the name
                //because I might need multiple stateItems with same name, but differences in other fields?

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
                if (prereqStateChecker(currentAction, imaginaryState) != true)
                {
                    //print("yes this should happen for ''eat'':");
                    //print(currentAction.name);
                    foreach (stateItem eachPrereq in currentAction.prereqs)
                    {
                        if (isStateAccomplished(eachPrereq, imaginaryState) != true)
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
        //returns the index number of the first action on that list that CANNOT be completed
        //if all actions can be completed fine, it returns negative two

        int noProblem;
        noProblem = -2;

        int counter;
        counter = 0;

        Dictionary<string, List<stateItem>> imaginaryState = new Dictionary<string, List<stateItem>>();
        imaginaryState = stateCopyer(realState);

        

        foreach(action currentAction in plan)
        {
            if (impossibleActionprereqStateChecker(currentAction, imaginaryState) != true)
            {
                //print("XXXXXXXXXXXX      this planned action is deemed impossible: ");
                //print(currentAction.name);
                return counter;
            }

            imaginaryState = implementALLEffects(currentAction, imaginaryState);
            counter += 1;
        }


        return noProblem;
    }

    public bool impossibleActionprereqStateChecker(action thisAction, Dictionary<string, List<stateItem>> state)
    {
        //assume true, then check and change to false where needed
        bool tf;
        tf = true;

        foreach (stateItem prereqX in thisAction.prereqs)
        {
            //print("don't count this");
            if (isStateAccomplished(prereqX, state) == false)
            {
                /*
                print("mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm      this planned action is deemed impossible: ");
                print(thisAction.name);
                print("BECAUSE of this prereq:");
                print(prereqX.name);
                print(gameObject.name);
                */

                return false;
            }
        }

        return tf;
    }


}
