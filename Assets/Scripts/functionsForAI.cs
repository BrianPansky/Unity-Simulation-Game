using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


public class functionsForAI : MonoBehaviour
{
    [SerializeField]
    Transform _destination;
    public NavMeshAgent _navMeshAgent;

    public GameObject storePrefab;

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
    public taggedWith theTagScript;

    //ad hoc test thing
    public bool testTime;

    public Dictionary<string, List<GameObject>> globalTags;

    void Awake()
    {
        thisAI = GetComponent<AI1>();
        premadeStuff = GetComponent<premadeStuffForAI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        testTime = false;

        

        stopwatch = 0;
        effectivenessTimer = 0;
        workerCount = 0;

        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        //getting the "global" tags:
        GameObject theWorldObject = GameObject.Find("World");
        worldScript theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        globalTags = theWorldScript.taggedStuff;
        theTagScript = GetComponent<taggedWith>();
    }



    ////////////////////////////////////////////////
    //                  SENSING
    ////////////////////////////////////////////////

    //handles ALL sensing:
    public void sensing(Dictionary<string, List<stateItem>> state)
    {
        //function used to have "action nextAction, GameObject target, " as additional inputs.
        //that seems badly entangled, caused an error when there IS NO next action in the list of actions (index out of range error)
        //and i can see those variables were only used in code below which has already been commented out, not used.

        


        if (areAllForbiddenZonesClear() == false)
        {
            //print("yo");

            //so, sensed someone in the forbiddenZone
            //but

            //ok, so found a threat in a forbiddenZone
            //but before we record that threat, first check if we ALREADY have that threat recorded:
            //SEEMS GARBAGE AND AD HOC AND HAD A SHALLOW COPY ERROR
            //deep copy just in case
            actionItem threatActionItem = premadeStuff.deepActionItemCopier(premadeStuff.convertToActionItem(premadeStuff.threat, 1));
            if (isStateAccomplished(threatActionItem, state) == false)
            {
                //SEEMS GARBAGE AND AD HOC AND HAD A SHALLOW COPY ERROR
                //DEFINITELY need to deep copy these ones!
                //ok, now we know we won't be adding a duplicate, here we go:
                state["threatState"].Add(deepStateItemCopier(premadeStuff.threat));
                thisAI.planningState["threatState"].Add(deepStateItemCopier(premadeStuff.threat));
                //"incrementItem"??????????
                

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
        //this is the ENACTION phase.  the DOING of an aciton.
        //testSwitch();
        //alert();

        //List<GameObject> allPotentialTargets = new List<GameObject>();

        //now to find suitable targets using my new tagging system:
        //NOTE: RIGHT NOW THIS LIST WILL INCLUDE EVERYONE, EVEN THE PERSON DOING THE ACTION, SO THEY MIGHT TARGET THEMSELVES!
        //allPotentialTargets = globalTags["person"];
        //print("ya every one leofjrjfir");
        //print(allPotentialTargets.Count);
        



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
                //print("the chooseTarget function returned null, for the following action:");
                //print(nextAction.name);

                //mark this plan as failed?
                //i think to do that, just add to list of ineffective actions:
                ineffectiveActions.Add(nextAction);

                //dump plan:
                target = dumpAction(target);
            }
            else
            {
                //the following shoudl only happen if target is NOT null, right?
                travelToTargetObject(target);
            }
            


            //I still currently use a "actionItem", but it could perhaps
            //be replaced with mere text?  The name of the location?

            //travelToactionItem(nextAction.locationPrereq);
        }
        


        //actions with ALL prereqs met (including location prereq) can proceed below:
        if (target != null && whicheverprereqStateChecker(nextAction, state, target) == true)
        {
            
            //if(gameObject.name == "NPC")

            //I SHOULD REALLY BUNDLE THESE INSIDE THE DEFINITIONS OF THE ACTIONS?  OR HAVE THEM "LINKED" IN THERE...

            if (nextAction.type == "buyFromStore")
            {
                testSwitch();
                //startTest();
                //print("ssssssssssssssssss action should have prereqs met in state ssssssssssssssssssssssssssss");
                //printState(state);
                //print(actionToTextDeep(nextAction));

                //need this stuff to check if cashier is there.  Otherwise, can't buy anything.
                //if casheir isn't there, shouldn't wait forever, that is unrealistic, but right now that's what will happen

                //GameObject customerLocation = getLocationObject(state["locationState"][0].name);
                GameObject customerLocation = target;

                //print("target name is:::::::::::::::::::::::::::::");
                //print(target.name);

                GameObject cashierMapZone = getCashierMapZone(customerLocation);

                GameObject cashier = whoIsTrader(cashierMapZone);

                //printState(state);

                //but that cashier variable might come back null (if no one is there), check:
                if (cashier != null)
                {

                    //ad-hoc update of state:
                    //state = implementALLEffectsREAL(nextAction, state);

                    GameObject shopInventory = getShopInventory(cashierMapZone);

                    testSwitch();
                    //print(actionToTextDeep(nextAction));
                    //lol

                    if (TRYincrementInventoriesOfThisAndTargetFromEffects(shopInventory, nextAction))
                    {

                        target = dumpAction(target);

                        //print("got fooooooooooooooooooooooooooooooood////////////////////////////////////////////////////////////////////////////////////////////////////////");
                        //printState(state);

                    }
                    else
                    {
                        //lol
                        target = dumpAction(target);
                        print("FAILED TO GET FOOD !!!!!!!!!!!!!!!!!!!////////////////////////////////////////////////////////////////////////////////////////////////////////");
                        //printState(state);
                    }

                    testSwitch();
                    //thisAI.toDoList.RemoveAt(0);


                    //alert();

                }

                //endTest();
                testSwitch();


            }
            else if (nextAction.name == "createSoldier")
            {
                //eventually do "hiring", i guess.  but for now:
                incrementItem(thisAI.factionState[premadeStuff.soldier.stateCategory], premadeStuff.soldier, 1);
                incrementItem(thisAI.state["inventory"], premadeStuff.resource1, -1);

                //maybe ad-hoc [see 3456819]:
                incrementItem(thisAI.state[premadeStuff.soldier.stateCategory], premadeStuff.soldier, 1);

                //NOTE THAT "TARGET" IS NOT BEING USED AS AN INPUT FOR NOW!!!!! BECAUSE I AM USING AN AD-HOC TARGET!!!
                //incrementInventoriesOfThisAndTargetFromEffects(thisAI., nextAction);

                target = dumpAction(target);

            }
            else if (nextAction.name == "orderAttack")
            {

                target = dumpAction(target);
            }
            else if (nextAction.name == "hireResourceGatherer")
            {
                if (hiring(target, premadeStuff.resource1GatheringJob, "storage"))
                {
                    //Debug.Log("hired..........");
                    //ad-hoc update of state:
                    //state = implementALLEffectsREAL(nextAction, state);
                }
                else
                {
                    //Debug.Log("FAILED TO HIRE");
                }

                target = dumpAction(target);
            }
            else if (nextAction.name == "resource1Dropoff")
            {

                //using "stopwatch" as a QUOTA rather than a timer:

                //AI1 theTargetState = target.GetComponent("AI1") as AI1;

                //incrementTwoInventoriesFromActionEffects(state["inventory"], theTargetState.state["inventory"], nextAction);
                //incrementInventoriesOfThisAndTargetFromEffects(target, nextAction);
                //AD-HOC FOR NOW, BECAUSE LEADER CANNOT CURRENTLY PLAN WITH FACTION INVENTORY:
                //incrementInventoriesOfThisAndTargetFromEffects(thisAI.leader, nextAction);
                if(TRYincrementInventoriesOfThisAndTargetFromEffects(thisAI.leader, nextAction))
                {

                }


                //now, update factionState inventory record-keeping system:
                inventoryInspection(target);
                
                //if quota not met, COMMAND the NPC to do another delivery round!
                if (stopwatch < thisAI.currentJob.quota)
                {
                    stopwatch += 1;

                    //going to blank out their to-do list, and fill it with "orders":

                    thisAI.inputtedToDoList.Add(nextAction);
                    
                    //do i need this?
                    target = dumpAction(target);

                    //AD HOC, SHOULD NOT DO THIS?!?!?
                    thisAI.toDoList.Clear();
                }
                else
                {
                    //NOW we're done quota!
                    //reset counter, and dump action!
                    stopwatch = 0;
                    target = dumpAction(target);
                }

                


            }
            else if (nextAction.name == "shootSpree")
            {
                //print("yo");
                //don't kill player for now:
                if(target.name != "Player")
                {
                    //print("a killer just shot " + target.name);
                    //print("object to be destroyed:");
                    //print(target);
                    Destroy(target);
                }
                else
                {
                    //print("you are shot!");
                    target = null;
                }
            }
            else if (nextAction.name == "landLording")
            {

            }
            else if (nextAction.type == "deliverAnyXtoLeader")
            {
                //startTest();
                
                //

                //ad-hoc for now
                //should generalize to be able to deliver to any target

                //just "gift" the delivery item to the target

                AI1 theTargetState = target.GetComponent("AI1") as AI1;
                //steal(state["inventory"], theTargetState.state["inventory"], nextAction);



                //print("===============================surely here START=============================");
                //printState(state);
                //printKnownActionsDeeply(thisAI.knownActions);
                if(state["inventory"] == null)
                {
                    print("state inv = null");
                }
                if (theTargetState == null)
                {
                    print("theTargetState hub = null");
                    print(target.gameObject.name);
                }
                if (theTargetState.state["inventory"] == null)
                {
                    print("target inv = null");
                }
                if (nextAction == null)
                {
                    print("state inv = null");
                }

                incrementTwoInventoriesFromActionEffects(state["inventory"], theTargetState.state["inventory"], nextAction);

                //print("----------------------mid---------------------------");
                //printState(state);
                //printKnownActionsDeeply(thisAI.knownActions);
                //print("xxxxxxxxxxxxxxxxxtttttttttttttttttttttttttt");
                //print("----------------------mid---------------------------");

                //IF I ALREADY ALTERED THE INVENTORIES ABOVE, WHY AM I IMPLEMENTING ALL EFFECTS HERE?  ISN'T THAT DONE?
                //state = implementALLEffectsREAL(nextAction, state);

                //printState(state);
                //printKnownActionsDeeply(thisAI.knownActions);
                //print("xxxxxxxxxxxxxxxxxtttttttttttttttttttttttttt");
                //print("===============================surely here END=============================");

                target = dumpAction(target);

                //endTest();

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
                //GameObject whoToHire, job theJob, string jobLocationTypeTag
                if (customer != null && customer.name != "NPC shopkeeper" && customer.name != "NPC shopkeeper (1)" && hiring(customer, premadeStuff.cashierJob, "shop"))
                {
                    //successfully hired them!

                    //ad-hoc way to hire more than one employee for now:
                    listOfCashiers.Add(customer);
                    //print(customer.name);





                    //FOR INVESTIGATING/TESTING:
                    AI1 targetAI = customer.GetComponent("AI1") as AI1;
                    targetAI.masterPrintControl = true;
                    targetAI.npcx = targetAI.gameObject.name;
                    Debug.Log("updated ''npcx''");





                    workerCount += 1;
                    
                    //ad-hoc way to hire more than one worker for now:
                    if (workerCount > 0)
                    {
                        //also, to easily put the "employee1" actionItem in the organizationState:
                        state = implementALLEffectsREAL(nextAction, state);
                        //^^^^^^^^^^that will ALSO be seen, and thus the "hire" aciton will be removed from to-do list


                        //and ad hoc strike this action off the to-do list:
                        //thisAI.toDoList.RemoveAt(0);

                        target = dumpAction(target);
                    }
                }
            }
            else if (nextAction.name == "pickVictimsPocket")
            {
                //ad hoc for now



                //if we have a target, use that one
                //(so always blank out targets BEFORE this action happens, I guess)
                //WY IS THIS HERE?  HOW DO YOU GET HERE WITH NO TARGET?
                if (target == null)
                {
                    //print("uuuuuuuuuuuuuuuuuuuuuuhhhhhhhhhhhhhhhhhhhhhhhhhhh");
                    target = whoToTarget();
                }
                GameObject victim;
                victim = target;



                //now do the pickpocketing
                AI1 theTargetState = victim.GetComponent("AI1") as AI1;
                incrementTwoInventoriesFromActionEffects(state["inventory"], theTargetState.state["inventory"], nextAction);


                //ad-hoc action completion:
                //thisAI.toDoList.RemoveAt(0);

                target = dumpAction(target);

                //state = implementALLEffectsREAL(nextAction, state);

                //print(target.name);
            }
            else if (nextAction.name == "recruit")
            {
                //print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                succeedAtRecruitment(target);
                state = implementALLEffectsREAL(nextAction, state);
                target = dumpAction(target);
            }
            else if (nextAction.name == "askMemberForMoney")
            {
                //ad hoc timer:
                thisAI.goalWait = 1100;

                //print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                commandToDoFetchXAction(premadeStuff.bringLeaderX(premadeStuff.deepStateItemCopier(premadeStuff.money)), target);
                
                target = dumpAction(target);
            }
            else if (nextAction.name == "workAsCashier")
            {
                //THIS HAS BEEN MOVED TO:  "jobCheckFunction"



                /*

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
                    if (stopwatch > thisAI.currentJob.duration)
                    {
                        state = implementALLEffectsREAL(nextAction, state);
                        //thisAI.toDoList.RemoveAt(0);
                        target = dumpAction(target);
                        stopwatch = 0;

                        //ya this doesn't work because my check in AI1 still sees ...prereqs are done?
                        //print("ggggggggggggggggggggggggggggggggggggggggggggggggggggg");
                    }
                }

                */
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
                    state = implementALLEffectsREAL(nextAction, state);

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
                //thisAI.masterPrintControl = true;

                printAlways("SECURITY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

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
                    state = implementALLEffectsREAL(nextAction, state);
                    target = dumpAction(target);

                    effectivenessTimer = 0;
                }

                
                //thisAI.masterPrintControl = false;
            }
            else if (nextAction.name == "handleSecurityEscalationOne")
            {
                //printAlways("SECURITY22222!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                //printAlways(actionToTextDeep(nextAction));
                

                if (effectivenessTimer == 0)
                {
                    print("handling security ESCALATON");
                    printAlways(this.gameObject.name);
                }
                effectivenessTimer += 1;

                if (areAllForbiddenZonesClear() == true)
                {
                    
                    state = implementALLEffectsREAL(nextAction, state);
                    target = dumpAction(target);

                    effectivenessTimer = 0;

                    print("escalation over...");
                }

                //printAlways(actionToTextDeep(nextAction));

            }
            else if (nextAction.name == "createShop")
            {
                //print(actionToTextDeep(newAction));
                //printKnownActionsDeeply(knownActions);

                createBuildingX(storePrefab);

                //printKnownActionsDeeply(knownActions);

                target = dumpAction(target);

                //printKnownActionsDeeply(knownActions);

                //ad-hoc update of state:
                state = implementALLEffectsREAL(nextAction, state);
                
            }
            else if (nextAction.name == "createStorage")
            {
                createBuildingX(premadeStuff.storagePrefab);
                

                target = dumpAction(target);

                //ad-hoc update of state:
                state = implementALLEffectsREAL(nextAction, state);

            }
            else if (nextAction.type == "realInventoryChanges")
            {

                AI1 theTargetState = target.GetComponent("AI1") as AI1;

                incrementTwoInventoriesFromActionEffects(state["inventory"], theTargetState.state["inventory"], nextAction);

                target = dumpAction(target);


            }

            else
            {
                //thisAI.masterPrintControl = true;
                //print("?????????????????????????????????????????????????");
                //for actions like "eat" that currently just need a quick
                //ad-hoc update of state:
                state = implementALLEffectsREAL(nextAction, state);
                //thisAI.toDoList.RemoveAt(0);
                target = dumpAction(target);

                //thisAI.masterPrintControl = false;
                //alert();
            }

            
        }

        //ad hoc for now:
        //testSwitch();
        return target;
    }

    public void jobCheckFunction()
    {
        //checks if jobs are done
        //can implement some external effects here, like being paid for the job
        //this is different from enaction phase, which is about DOING the actions

        //for now JUST check the known actions in their currentJob?  i guess...

        if(thisAI.currentJob != null)
        {

            //So, first get the job actions, i guess:
            //or cycle throgh them [should be few on list, not too many]:
            foreach (action thisAction in thisAI.currentJob.theKnownActions)
            {
                if (thisAction.name == "workAsCashier")
                {
                    //so, gotta do the whole "work shift" timer thing here
                    //then give money if it's done
                    //NOT SURE HOW THIS MESHES WITH COMPLETING AN ACTION FOR AN NPC

                    //can use the ROLE LOCATION built into currentJob to check locations i guess!

                    //ad-hoc check if someone else is the cashier right now:
                    GameObject thisNPC = gameObject;
                    //GameObject theCashierZone = getLocationObject("cashierZone");
                    //but the actual "mapZone" is a CHILD of this casheirZone object:
                    //GameObject cashierMapZone = theCashierZone.GetChild(0).gameObject;
                    //GameObject theCashierZone = getLocationObject("cashierZone");

                    //casheirZone object:
                    //GameObject cashierMapZone = getCashierMapZone(target.transform.parent.gameObject);
                    GameObject cashierMapZone = getCashierMapZoneOfStore(thisAI.currentJob.roleLocation);



                    GameObject currentCashier = getWhoeverIsHereFirst(cashierMapZone);
                    if (currentCashier == thisNPC)
                    {
                        stopwatch += 1;

                        //ad-hoc work shift timer:
                        if (stopwatch > thisAI.currentJob.duration)
                        {

                            thisAI.state = implementALLEffectsREAL(thisAction, thisAI.state);
                            //thisAI.toDoList.RemoveAt(0);
                            //target = dumpAction(target);
                            if(thisAI.target != null)
                            {
                                //printAlways("dddddddddddddddddddddddddddddddddddddddddddddd");
                                thisAI.target = dumpAction(thisAI.target);
                            }
                            stopwatch = 0;

                            //ya this doesn't work because my check in AI1 still sees ...prereqs are done?
                            //printAlways("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
                            //print("ggggggggggggggggggggggggggggggggggggggggggggggggggggg");
                        }
                    }
                }
            }
        }
    }

    //============================================================

    //functions to handle important finishing steps of specific actions:

    //DANGER OF MALFUNCTION IF CLASS OBJECTS ARE NOT DEEP COPIED!!!!

    //modify state:
    
    public void incrementItem(List<stateItem> inventory, stateItem theItem, int amount)
    {
        //"theItem" is actually an effect built into an action.  
        //it is not any realy item in any inventory.  as such it should not be modified
        //(but can be copied, we will know how much it is in quantity)

        //adds an amount of an item to someone's inventory

        //if inventory aready has this item, just increment the quantity
        //if inventory has none of the item, add it as a stateItem, then increment the quantity
        bool foundItem = false;
        stateItem itemToIncrement = null;

        //find item to increment:
        foreach (stateItem item in inventory)
        {
            if (item.name == theItem.name)
            {
                foundItem = true;
                itemToIncrement = item;
                //print("111111111111111111111111111111111111");

            }
        }

        

        if (foundItem == false)
        {
            if (this.name == "NPC")
            {
                //print("222222222222222222222222222222222222");
                //print("XXXXXXXXXXXX      this planned action is deemed impossible: ");
                //print(currentAction.name);
                //print(newItem.quantity);
                //print(amount);
            }



            //startTest();

            //need to add money to inventory
            //will this deepcopy or not???  do we want to?  won't it have wrong quantities if it does deep copy, but break if it doesn't deep copy???
            stateItem newItem = deepStateItemCopier(theItem);
            //but, this money will already start with 1 quantity.  will need to subtract one so it starts at zero...
            //no, since we know in this logic branch of "if" statemetn that we are starting from ZERO
            //just set it EQUAL to "amount".  don't subtract anything
            //well, to avoid any deep copy nightmare, will use "+=" as follows:
            newItem.quantity = 0;
            newItem.quantity += amount;

            //endTest();

            if (this.name == "NPC")
            {
                //print(newItem.quantity);  //this should be JUST EQUAL TO "amount", no more, no less
            }
            //print(newItem.quantity);  //this should be JUST EQUAL TO "amount", no more, no less

            inventory.Add(newItem);
            

        }
        else
        {
            
            //startTest();

            //just add, don't need to zero it out:
            itemToIncrement.quantity += amount;
            


            //endTest();

            //alert();


            //if there are zero left in inventory, due to subtraction, completely remove the stateItem:
            //if (itemToIncrement.quantity == 0)
            if (itemToIncrement.quantity == 0)
            {
                
                //imaginaryState = removeStateItem(eachEffect, imaginaryState);

                //  SHOULD THIS BE "ITEM TO INCREMENT", NOT "THEITEM"????????
                //removeInventoryItem(theItem, inventory);
                removeInventoryItem(itemToIncrement, inventory);
                
            }
            if (itemToIncrement.quantity < 0)
            {
                //!!!  R E A D   T H I S  !!!!!!S
                //when subtracting "hunger" from state, if the "food" increment is more than one, it can make the "hunger" go into negatives.how to handle ?
                //---should never do that.  food should just increment 1.  my current larger numbers were for testing only
                //---check after incrementing.  if ANYWEHRE BELOW 1, count as zero, remove item from state.  yes this "wastes" all those extra food increments.  could maybe just subtract whatever was needed, have "leftovers" of food.
                //print("!!!!>>>  Increment went BELOW ZERO!  should this EVER happen?? how to handle???");

                //print(this.name);

                //print(itemToIncrement.quantity);
                //print(itemToIncrement.name);
                //print(theItem.quantity);
                //printState(thisAI.state);
                //printKnownActionsDeeply(thisAI.knownActions);

                //imaginaryState = removeStateItem(eachEffect, imaginaryState);

                //  SHOULD THIS BE "ITEM TO INCREMENT", NOT "THEITEM"????????
                //removeInventoryItem(theItem, inventory);
                //removeInventoryItem(itemToIncrement, inventory);
                
            }


        }

        
    }

    public void incrementTwoInventoriesFromActionEffects(List<stateItem> actionerInventory, List<stateItem> inventory2, action nextAction)
    {
        //so, to make it simple to work, assume:
        //1)---any gained item (in action effect) is taken from 2nd inventory
        //2)---any lost item is GIVEN to 2nd inventory
        //3)---just swap the sign on the quantity if the bool is false

        //actioner is the one doing the nextAction

        //https://stackoverflow.com/a/605390
        List<actionItem> deepCopyVERIFIEDeffectsList = new List<actionItem>();
        List<actionItem> actionerGives = new List<actionItem>();
        List<actionItem> actionerReceives = new List<actionItem>();

        //look to gift EACH item in the "effects" of the gift action
        //but only LOOK and take note, don't modify inventories YET (can lead to error)
        foreach (actionItem effect in nextAction.effects)
        {
            //must only give items if they exist in the giver's inventory!
            //but to get here we should assume that such prereqs are already met
            //BUT PREREQS HAVE NOT CHECKED THE 2ND INVENTORY

            //just deep copy the effects list so i can use them without messing anything up:
            //deepCopyVERIFIEDeffectsList.Add(premadeStuff.deepActionItemCopier(effect));

            //first, determine whose inventory to VERIFY the contents of, based on bool, and the assumptions listed at start of funciton:
            if (effect.inStateOrNot == false)
            {
                //"false" means it comes out of actioner's inventory, so need to check to make sure it is IN the actioner's inventory to start with:
                foreach (stateItem itemInInventory in actionerInventory)
                {
                    if (itemInInventory.name == effect.name)
                    {
                        //actionerGives.Add(itemInInventory);

                        //need to deep copy this one so we don't modify stuff in "knownActions":
                        //otherInventoryReceives.Add(deepStateItemCopier(effect.item));
                        deepCopyVERIFIEDeffectsList.Add(premadeStuff.deepActionItemCopier(effect));
                    }
                }
            }
            if (effect.inStateOrNot == true)
            {
                //"true" means it comes out of the2nd inventory, so need to check to make sure it is IN the 2nd inventory to start with:
                foreach (stateItem itemInInventory2 in inventory2)
                {
                    if (itemInInventory2.name == effect.name)
                    {
                        //actionerReceives.Add(deepStateItemCopier(effect));
                        //otherInventoryLoses.Add(itemInInventory2);
                        deepCopyVERIFIEDeffectsList.Add(premadeStuff.deepActionItemCopier(effect));

                    }

                }

            }
        }

        //NOW modify inventories
        int amount = 1;
        //actioner inventory:
        foreach (actionItem effect in deepCopyVERIFIEDeffectsList)
        {
            if(effect.inStateOrNot == false)
            {
                amount = -1;
            }
            incrementItem(actionerInventory, effect.item, amount);
            incrementItem(thisAI.planningState["inventory"], effect.item, amount);
        }
        //target's inventory:
        amount = -1;
        foreach (actionItem effect in deepCopyVERIFIEDeffectsList)
        {
            if (effect.inStateOrNot == false)
            {
                amount = 1;
            }
            incrementItem(inventory2, effect.item, amount);
        }


    }

    public void incrementInventoriesOfThisAndTargetFromEffects(GameObject theTargetPerson, action theAction)
    {
        //print(theTargetPerson.name);
        //just more clean and tidy than having to fetch the AI inventory and plug it in all the time!
        //make sure inputted target has an inventory script!
        //requires "state" of current person, but that's accounted for at start of this script, i think?
        //well, start gives us "thisAI", i can use that...
        AI1 theTargetState = theTargetPerson.GetComponent("AI1") as AI1;
        
        incrementTwoInventoriesFromActionEffects(thisAI.state["inventory"], theTargetState.state["inventory"], theAction);

    }




    //TRY INVENTORY MODIFICATION:
    public bool TRYincrementInventoriesOfThisAndTargetFromEffects(GameObject theTargetPerson, action theAction)
    {
        //print(theTargetPerson.name);
        //just more clean and tidy than having to fetch the AI inventory and plug it in all the time!
        //make sure inputted target has an inventory script!
        //requires "state" of current person, but that's accounted for at start of this script, i think?
        //well, start gives us "thisAI", i can use that...
        AI1 theTargetState = theTargetPerson.GetComponent("AI1") as AI1;

        return TRYincrementTwoInventoriesFromActionEffects(thisAI.state["inventory"], theTargetState.state["inventory"], theAction);

    }

    public bool TRYincrementTwoInventoriesFromActionEffects(List<stateItem> actionerInventory, List<stateItem> inventory2, action nextAction)
    {
        //so, to make it simple to work, assume:
        //1)---any gained item (in action effect) is taken from 2nd inventory
        //2)---any lost item is GIVEN to 2nd inventory
        //3)---just swap the sign on the quantity if the "inStateOrNot" bool is false

        //actioner is the one doing the nextAction

        //https://stackoverflow.com/a/605390
        List<actionItem> deepCopyVERIFIEDeffectsList = new List<actionItem>();
        List<actionItem> actionerGives = new List<actionItem>();
        List<actionItem> actionerReceives = new List<actionItem>();

        bool theTRY = false;

        //look to gift EACH item in the "effects" of the gift action
        //but only LOOK and take note, don't modify inventories YET (can lead to error)
        foreach (actionItem effect in nextAction.effects)
        {
            //must only give items if they exist in the giver's inventory!
            //but to get here we should assume that such prereqs are already met?
            //BUT PREREQS HAVE NOT CHECKED THE 2ND INVENTORY
            //also check quantities

            //just deep copy the effects list so i can use them without messing anything up:
            //deepCopyVERIFIEDeffectsList.Add(premadeStuff.deepActionItemCopier(effect));

            //bool to return if inventories don't have proper contents [assuming effects mean SWAP, any listed effect comes OUT of ONE inventory]:
            //will also use this to track whether there is enough QUANTITY
            bool found = false;

            //first, determine whose inventory to VERIFY the contents of, based on bool, and the assumptions listed at start of funciton:
            if (effect.inStateOrNot == false)
            {
                //"false" means it comes out of actioner's inventory, so need to check to make sure it is IN the actioner's inventory to start with:
                foreach (stateItem itemInInventory in actionerInventory)
                {
                    if (itemInInventory.name == effect.name)
                    {
                        //item found, but is there enough QUANTITY?

                        if(itemInInventory.quantity < effect.item.quantity)
                        {
                            //not enough quantity
                            found = false;
                        }
                        else
                        {
                            //there is enough quantity

                            //actionerGives.Add(itemInInventory);

                            //need to deep copy this one so we don't modify stuff in "knownActions":
                            //otherInventoryReceives.Add(deepStateItemCopier(effect.item));
                            deepCopyVERIFIEDeffectsList.Add(premadeStuff.deepActionItemCopier(effect));

                            found = true;
                        }
                            

                        
                    }
                }
            }
            if (effect.inStateOrNot == true)
            {
                //"true" means it comes out of the2nd inventory, so need to check to make sure it is IN the 2nd inventory to start with:
                foreach (stateItem itemInInventory2 in inventory2)
                {
                    if (itemInInventory2.name == effect.name)
                    {
                        //item found, but is there enough QUANTITY?

                        if (itemInInventory2.quantity < effect.item.quantity)
                        {
                            //not enough quantity
                            found = false;
                        }
                        else
                        {
                            //there is enough quantity

                            //actionerReceives.Add(deepStateItemCopier(effect));
                            //otherInventoryLoses.Add(itemInInventory2);
                            deepCopyVERIFIEDeffectsList.Add(premadeStuff.deepActionItemCopier(effect));

                            found = true;
                        }

                        
                    }

                }

            }

            if(found == false)
            {
                //means one item is not found, so no swap can take place,!

                return false;
            }
        }


        //remember, to make it simple to work, assume:
        //1)---any gained item (in action effect) is taken from 2nd inventory
        //2)---any lost item is GIVEN to 2nd inventory
        //3)---just swap the sign on the quantity if the "inStateOrNot" bool is false

        //actioner is the one doing the nextAction


        //print(actionToTextDeep(nextAction));
        //printInventoryDeep(actionerInventory);
        //printInventoryDeep(inventory2);
        //printInventoryDeep(thisAI.planningState["inventory"]);

        //print("..................");

        //NOW modify inventories
        //WHY DO I HAVE TWO FOR LOOPS, TWO DIFFERENT "AMOUNMTS" AND I STILL 
        //***AFTERWARDS*** CHECK "inStateOrNot"???????????
        //[supposedly "actioner" VS" target", but both are iterating through the same "deepCopyVERIFIEDeffectsList"!!!]
        //and the "amount" is only set to be the "item quantity" if it's NOT in state????? why?
        int amount = 1;
        //actioner inventory:
        foreach (actionItem effect in deepCopyVERIFIEDeffectsList)
        {
            theTRY = true;
            if (effect.inStateOrNot == false)
            {
                amount = (effect.item.quantity)*(-1);
            }
            incrementItem(actionerInventory, effect.item, amount);
            incrementItem(thisAI.planningState["inventory"], effect.item, amount);
        }
        //target's inventory:
        amount = -1;
        foreach (actionItem effect in deepCopyVERIFIEDeffectsList)
        {
            theTRY = true;
            if (effect.inStateOrNot == false)
            {
                amount = effect.item.quantity;
            }
            incrementItem(inventory2, effect.item, amount);
            
        }


        //printInventoryDeep(actionerInventory);
        //printInventoryDeep(inventory2);
        //printInventoryDeep(thisAI.planningState["inventory"]);
        //thisAI.stateDiagnosis(thisAI.planningState);

        return theTRY;


    }




    //change knownActions and such:
    public void addKnownActionToGameObject(GameObject agent, action theAction)
    {
        //first, go from "GameObject" to it's script that has knownActions:
        AI1 hubScript = getHubScriptFromGameObject(agent);

        //now add the knownAction:
        hubScript.knownActions.Add(premadeStuff.deepActionCopier(theAction));
        
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

    public bool hiring(GameObject whoToHire, job theJob, string jobLocationTypeTag)
    {
        //for now, ad-hoc enter "jobLocationType" string.  used to find location using tags.  later, pull that info from the boss automatically somehow....
        //Has to return bool to show if it worked or no.  clunky, but oh well?


        //ad-hoc way to hire more than one employee for now:
        //if (listOfCashiers.Contains(customer) == false)
        AI1 targetAI = whoToHire.GetComponent("AI1") as AI1;
        if (targetAI.jobSeeking == true)
        {
            

            //listOfCashiers.Add(customer);
            //changeRoles(whotoHire, premadeStuff.workAsCashier, premadeStuff.doTheWork);

            //print(customer.name);


            //workerCount += 1;

            //print(workerCount);

            //need the worker to show up at the correct store for their shift:
            //customerAI.roleLocation = thisAI.roleLocation;
            string ownershipTag = "owned by " + this.name;
            //need cashierZone of the owned store:
            GameObject roleLocation = randomTaggedWithMultiple(jobLocationTypeTag, ownershipTag);

            if(roleLocation == null)
            {
                print("cannot find a role location, probably trying to hire someone before you've made a business, or my system is unfinished");

                return false;
            }
            else
            {
                doSuccsessfulHiring(targetAI, theJob, roleLocation);
            }

            



            return true;
        }
        else
        {
            return false;
        }
    }

    public void doSuccsessfulHiring(AI1 targetAI, job theJob, GameObject roleLocation)
    {
        //print(roleLocation);
        targetAI.jobSeeking = false;
        targetAI.leader = this.gameObject;

        //record in factionState:
        //mmm, need it to INCREMENT...
        incrementItem(thisAI.factionState["unitState"], premadeStuff.employee, 1);
        //thisAI.factionState["unitState"].Add(deepStateItemCopier(premadeStuff.employee));
        

        //Increase the "clearance level" of the worker:
        //BIT ad-hoc.  Characters might have different clearance levels for different places/factions etc.  Right now I just have one.
        targetAI.clearanceLevel = 1;

        //now...to finish and deliver "theJob" class object...
        targetAI.currentJob = premadeStuff.jobFinisher(theJob, this.gameObject, roleLocation);

        //but still have to add the known actions to their known actions!  sigh.
        foreach (action x in theJob.theKnownActions)
        {
            targetAI.knownActions.Add(premadeStuff.deepActionCopier(x));
            //addKnownActionToGameObject(whoToHire, x);
        }

        //updateFactionState?
    }

    public void commandToDoFetchXAction(action theBringLeaderXAction, GameObject whoToCommand)
    {
        //commandList.Add(premadeStuff.bringLeaderX(premadeStuff.deepStateItemCopier(premadeStuff.food)));

        //need their AI1 script:
        AI1 NPChubScript = whoToCommand.GetComponent("AI1") as AI1;

        //going to blank out their to-do list, and fill it with test "orders":
        //AD HOC, SHOULD NOT DO THIS?!?!?
        NPChubScript.toDoList.Clear();


        NPChubScript.inputtedToDoList.Add(theBringLeaderXAction);
    }

    public void createBuildingX(GameObject buildingX)
    {
        //input a prefab
        //will instantiate it RIGHT WHERE npc IS STANDING
        //and will update ownership tags
        //some ad-hoc junk too, alas


        //create store:
        GameObject newBuilding = new GameObject();
        //newShop = Instantiate(storePrefab, new Vector3(5, 0, -11), Quaternion.identity);

        //---get XYZ values from gameObject.transform somehow
        //---adjust Y value
        //---combine XYZ values into...a 3Vector or whatever...somehow
        //---plug into instantiate function
        //Vector3 whereToPlace = new Vector3(gameObject.transform.x, (gameObject.transform.y - 113), gameObject.transform.z);

        newBuilding = Instantiate(buildingX, new Vector3(gameObject.transform.position.x, (gameObject.transform.position.y - 1), gameObject.transform.position.z), Quaternion.identity);


        //now "buy" it:

        //check if it's for sale:
        //get other script I need:
        taggedWith otherIsTaggedWith = newBuilding.GetComponent<taggedWith>() as taggedWith;

        string ownershipTag = "owned by " + this.name;
        otherIsTaggedWith.foreignAddTag(ownershipTag, newBuilding);

        //need to remember in the future WHICH store is theirs
        //so they ca go to it, and sned their employees there:
        //thisAI.roleLocation = target;

        //ad-hoc action completion:
        //thisAI.toDoList.RemoveAt(0);


        //make this NPC self employed, so i will have role-location for buying from them...bit ad-hoc...
        //SHOULD MOVE THIS TO "CREATE STORE" ENACTION, BUT
        //REQUIRES THE "NEWBUILDING" GAME OBJECT, NOT JUST THE PREFAB!
        //so, should RETURN this generated building object, to be used further
        //[or have OTHER way to "find" it for use]
        if(buildingX.name == "storeToCreate")
        {
            doSuccsessfulHiring(thisAI, premadeStuff.cashierJob, newBuilding);
        }
        

    }

    //other:
    public void travelToactionItem(actionItem X)
    {

        string name1 = X.name;
        t1 = GameObject.Find(name1);

        Vector3 targetVector = t1.GetComponent<Transform>().position;
        _navMeshAgent.SetDestination(targetVector);
    }

    public void succeedAtRecruitment(GameObject whoIsRecruited)
    {
        //ok, recruitment suceeds
        Debug.Log("recruitment successful");

        //for now ad-hoc
        //just tagging an NPC with "playersGang" when you click it
        taggedWith foreignTagScript = whoIsRecruited.GetComponent<taggedWith>();
        foreignTagScript.foreignAddTag(gangTag(this.gameObject), whoIsRecruited);

        //Debug.Log("should be recruited to gang now, by tagging");

        //but they need to be able to FIND me, their leader, to deliver money to me
        //so, for now, fill their leader variable:
        //need their AI1 script:
        AI1 NPChubScript = whoIsRecruited.GetComponent("AI1") as AI1;
        NPChubScript.leader = this.gameObject;

        //ALSO NEED TO BLANK-OUT THEIR TARGET!!!
        NPChubScript.target = null;
    }
    //============================================================



    ////////////////////////////////////////////////
    //         Misc functions for ACTIONS
    ////////////////////////////////////////////////

    public GameObject dumpAction(GameObject target)
    {

        //when action is done, remove the action from the plan, and set target to null
        target = null;
        thisAI.toDoList.RemoveAt(0);

        //maybe check if plan is complete.
        //if so, should ALSO blank out "planList"
        //because those plans were made on the assumption that the stuff of this current plan were not complete
        if (thisAI.toDoList.Count == 0)
        {
            //blank out the planList:
            thisAI.planList.Clear();
        }

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

    public void inventoryInspection(GameObject containerObject)
    {
        //check container
        //update faction inventory
        //[for now, assume this container is the ONLY inventory the faction has
        //thus, faction inventory = this container's contents
        //later, handle multiple containers, with an abstraction of each one?]


        //get inventory of container
        //get factionState inventory
        //....deep copy contents of container?
        //paste into faction....


        AI1 contHub = containerObject.GetComponent("AI1") as AI1;

        //for faction one....need to find it.....it is in leader game object.  
        //get leader game object....from job class object
        AI1 leaderHub = thisAI.currentJob.boss.GetComponent("AI1") as AI1;

        //now deep copy...paste into faction....
        leaderHub.factionState["inventory"] = deepStateCategoryCopyer("inventory", contHub.state);

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
                foreach (GameObject thisListedNPC in thisListOfTouchingNPCs.theList)
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

    public Dictionary<string, List<stateItem>> implementALLEffectsREAL(action currentAction, Dictionary<string, List<stateItem>> state)
    {
        //this version of the funciton is different from non-"real" version because
        //it ALSO modifies "planningState"!!!!!!!!!!!!!!!
        foreach (actionItem FIXeachEffect in currentAction.effects)
        {
            if (this.name == "NPC" && FIXeachEffect.name == "money")
            {
                //print("amount of money to implement, should never be zero i don't think:");
                //printPlanListForSpecificNPC();
                //print(FIXeachEffect.item.quantity);
            }
            state = implementTHISEffectREAL(FIXeachEffect, state);
        }
        return state;
    }

    public Dictionary<string, List<stateItem>> implementTHISEffectREAL(actionItem FIXeachEffect, Dictionary<string, List<stateItem>> state)
    {
        //this version of the funciton is different from non-"real" version because
        //it ALSO modifies "planningState"
        //just have to get the "stateItem" from the actionItem:
        stateItem eachEffect = FIXeachEffect.item;

        //print(stateItemToTextDeep(eachEffect));
        //printState(state);
        //printInventoryDeep(state["inventory"]);
        //printState(thisAI.planningState);

        if (FIXeachEffect.inStateOrNot == true)
        {

            if (eachEffect.stateCategory == "locationState")
            {
                //no longer needed/used???
                state["locationState"].Clear();
            }

            //imaginaryState[eachEffect.stateCategory].Add(eachEffect);
            incrementItem(state[eachEffect.stateCategory], eachEffect, eachEffect.quantity);
            incrementItem(thisAI.planningState[eachEffect.stateCategory], eachEffect, eachEffect.quantity);

            //print("helooooooooooooooooooooooooo11111111111111111111");
            //printState(state);
            //printInventoryDeep(state["inventory"]);
            //printState(thisAI.planningState);

        }
        else
        {
            //print("ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss");
            //print(actionItemToTextDeep(FIXeachEffect));
            //printState(imaginaryState);


            //imaginaryState = removeStateItem(eachEffect, imaginaryState);
            //imaginaryState[eachEffect.stateCategory].Remove(eachEffect);
            incrementItem(state[eachEffect.stateCategory], eachEffect, (-1) * eachEffect.quantity);
            incrementItem(thisAI.planningState[eachEffect.stateCategory], eachEffect, (-1) * eachEffect.quantity);
            //print("helooooooooooooooooooooooooo22222222222222222222222222");
            //print("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
            //printState(imaginaryState);
            //printState(state);
            //printInventoryDeep(state["inventory"]);
            //printState(thisAI.planningState);

            //if there are zero left in inventory, completely remove the stateItem:
            //well, this should technically be handled over in "incrementItem, i think
            //if ()

        }

        if (FIXeachEffect.name == "food")
        {
            //print("AFTER incrementing food:");
            //printInv
            //printState(imaginaryState);
        }

        return state;
    }


    ////////////////////////////////////////////////
    //                TARGETING
    ////////////////////////////////////////////////

    public GameObject chooseTarget(stateItem criteria)
    {
        //takes criteria, returns one target
        //for now, input is a actionItem from nextAction.locationPrereq
        //output is a GameObject


        string name1 = criteria.name;
        GameObject target;
        target = null;

        if (criteria.locationType == "mobile")
        {
            //for now, just the pickpocket action?  Move that stuff here...
            target = whoToTarget();

        }
        else if (criteria.locationType == "deliverTo")
        {

            if(criteria.name == "storagePlace")
            {
                target = randomTaggedWithMultiple("storage", thisLeadersOwnerTag());
            }
            else
            {
                //for now ad hoc, just deliver to me, the leader
                //should have a "deliveryTarget" variable?  But could have multiple...need it embedded in the "deliver" action...
                target = thisAI.leader;
            }
            


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
            else if (criteria.name == "anyResource1")
            {
                target = randomTaggedWith("resource1");
            }
            else if (criteria.name == "checkout")
            {
                //print("checkout");
                //get any store:
                //print("hello?????????????????????????????");
                target = anyCheckout();
            }
            else if (criteria.name == "anyGroupMember")
            {
                target = randomTaggedWith(this.name + "sGang");
            }
            else if (criteria.name == "anyLandPlot")
            {
                //ad hoc for now, just select self lol?
                target = gameObject;
            }
            else if(criteria.name == "toRecruit")
            {

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
            target = getCashierMapZoneOfStore(thisAI.currentJob.roleLocation);

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


    //find objects using tags [should these be moved to tag script?]:
    public GameObject randomTaggedWith(string theTag)
    {
        //should return ONE random GameObject that is tagged with the inputted tag

        List<GameObject> allPotentialTargets = new List<GameObject>();

        if (globalTags.ContainsKey(theTag))
        {
            allPotentialTargets = globalTags[theTag];
        }
        

        /*
        if (theTag == "shop")
        {
            print("choosing a shop...");

            print(allPotentialTargets.Count);

            foreach(GameObject item in allPotentialTargets)
            {
                print(item.transform.position);
            }
        }
        */


        if (allPotentialTargets.Count > 0)
        {
            GameObject thisObject;
            thisObject = null;
            int randomIndex = Random.Range(0, allPotentialTargets.Count);
            thisObject = allPotentialTargets[randomIndex];

            /*
            if (theTag == "shop")
            {
                print("FOUND a shop...");
                print(randomIndex);
                print(thisObject.name);

                //print coordinates?  how?
                print(thisObject.transform.position);
            }
            */

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

        if (globalTags.ContainsKey(theTag))
        {
            allPotentialTargets = globalTags[theTag];
        }

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

        /*
        if (theTag == "shop")
        {
            print("choosing a shop...");
            //print(length);
        }
        */
            


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

                        //set thisObject back to null:
                        thisObject = null;
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
                /*
                if (theTag == "shop")
                {
                    print("FOUND a shop...");
                    print(myIndex);
                    print(thisObject.name);
                }
                */

                return thisObject;
            }
        }

        //this will be null if the above loop didn't find one:
        return thisObject;
    }


    //misc tag stuff
    public string generateGangName(GameObject leader)
    {
        //input eader object?  or string of leader's name?  object for now
        //input leader name string "X"
        //output string "XsGang"

        return leader.name + "sGang";

    }

    public string gangTag(GameObject leader)
    {
        string theGangTag = leader.name + "sGang";
        return theGangTag;
    }

    public bool isThisMyLeader(GameObject maybeLeader)
    {
        //check if FOLLOWER ["me"] has faction tag of this talked-to NPC i think...

        //so:
        //-need faction tag
        //-need to check my tags for it


        //-need faction tag
        //gangTag(maybeLeader)


        //now
        //-need to check my tags for it

        if (theTagScript.tags.Contains(gangTag(maybeLeader)))
        {
            return true;
        }
        else
        {
            return false;
        }
        


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
            if (listOfNPCs.theList[0] != null && listOfNPCs.theList[0].name != "NPC pickpocket")
            {
                customer = listOfNPCs.theList[0];
            }

        }


        return customer;

    }

    public string thisLeadersOwnerTag()
    {
        return leadersOwnerTag(thisAI.leader);
    }


    public string leadersOwnerTag(GameObject leader)
    {
        return "owned by " + leader.name;
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

        //print("sooooooooooooooooooooooo");
        thisShop = randomTaggedWith("shop");
        //print("---------done----------");

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

    public GameObject getShopInventory(GameObject cashierMapZone)
    {
        //FROM CASHIER MAP ZONE
        //YOU CAN USE getCashierMapZoneOfStore TO GET IT FROM STORE
        //For now, just getting the cashier inventory by grabbing the parent object of the cashier map Zone. 

        return cashierMapZone.transform.parent.gameObject.transform.parent.gameObject;
    }


    //re-used bits for getting locations

    public GameObject namedChild(GameObject theHighestParent, string nameOfChild)
    {
        //recursive function to simply search ALL child objects, brute force
        //return the ONLY child with the given name (or first one found)

        //make sure object isn't null:
        if (theHighestParent == null)
        {
            return null;
        }

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

        if (thisShop != null)
        {
            //now get the checkoutZone of that shop:
            GameObject thisCheckout;
            thisCheckout = null;
            //print("here, right??????????????????????????");
            thisCheckout = namedChild(thisShop, "checkout");


            return thisCheckout;
        }
        else
        {
            return null;
        }
        
    }

    //old targeting:
    public GameObject whoToTarget()
    {
        //ad-hoc for now, this is being used in pickpocketing action
        //should return ONE NPC GameObject as a target

        List<GameObject> allPotentialTargets = new List<GameObject>();

        //now to find suitable targets using my new tagging system:
        //NOTE: RIGHT NOW THIS LIST WILL INCLUDE EVERYONE, EVEN THE PERSON DOING THE ACTION, SO THEY MIGHT TARGET THEMSELVES!
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
        //print(">>>>>>>>>>>>>>>>>>");
        //print(allPotentialTargets.Count);

        while (doWeHaveGoodTarget == false && allPotentialTargets.Count > 0)
        {
            int randomIndex = Random.Range(0, allPotentialTargets.Count);
            thisNPC = allPotentialTargets[randomIndex];
            //but, criteria, ad-hoc for now
            //if it's the shopkeeper, remove that item from the array (will that leave a "null" hole in array???)
            //and choose again
            //print("during iteration:");
            //print(allPotentialTargets.Count);
            if (thisNPC != null)
            { 
                if (thisNPC.name == "NPC shopkeeper" || thisNPC.name == "NPC pickpocket")
                {
                    allPotentialTargets.RemoveAt(randomIndex);
                    thisNPC = null;
                }
                else
                {
                    doWeHaveGoodTarget = true;
                }
            

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
        foreach (int thisCost in unsortedCosts)
        {
            copyCosts.Add(thisCost);
        }

        //just sorting the costs, not fancy for now:
        copyCosts.Sort();

        //NOW FOR THE TRICKY PART

        //add the indexes as a value in a dictionary wher the keys are in order, 1 2 3 4 etc.
        foreach (int thisCost in copyCosts)
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

        foreach (action thisAction in plan)
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
        
        if (this.name == thisAI.npcx && thisAI.masterPrintControl != false)
        {
            Debug.Log(text);
        }
        
    }

    public void printInt(int number)
    {

        if (this.name == "NPC" && thisAI.masterPrintControl != false)
        {
            Debug.Log(number);
        }

    }

    public void printAlways(string text)
    {
        //or "printAnyone"

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

        printout += "[ ";

        foreach (action listItem in plan)
        {
            printout += listItem.name + ' ';
        }

        printout += "]";

        return printout;
    }

    public string planListToText(List<List<action>> planList)
    {
        print("number of plans on planList = " + planList.Count());
        string printout = string.Empty;

        printout += "[ ";

        foreach (List<action> list in planList)
        {
            printout += "[ " + planToText(list) + " ] ";
        }

        printout += "]";

        return printout;
    }

    public string listOfPlanListsToText(List<List<List<action>>> listofPlanLists)
    {
        string printout = string.Empty;

        foreach (List<List<action>> planList in listofPlanLists)
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
        string text = string.Empty;
        text += "{";
        foreach (string key in state.Keys)
        {


            text += "{ " + key + ": ";
            //text = string.Concat(text, key, ", ");
            foreach (stateItem content in state[key])
            {
                text += content.quantity + " " + content.name + ", ";
                //text = string.Concat(text, content.name, ", ");
            }
            //text = string.Concat(text, "} ");
            text += "}";
        }
        text += "}";
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

    public void printPlanList(List<List<action>> planList)
    {
        if (planList == null)
        {
            print("this planList is null");
        }
        else
        {
            print(planListToText(planList));
        }

    }


    public void printPlanListForSpecificNPC(List<List<action>> planList)
    {
        //List<List<action>>


        //to help, encapsulating this:

        if (this.name == "NPC 4")
        {
            print("00000000000000000  Plan List:   000000000000000000");
            printPlanList(planList);
        }


        /*
            if (this.name == "NPC pickpocket")
            {
                theFunctions.print("00000000000000000  Plan List:   000000000000000000");
                printPlanList(planList);
            }

            //constantlyCheckLocationState();
            //theFunctions.printInventory(state["inventory"]);


            if (this.name == "NPC")
            {
                theFunctions.print("==================================================");
                theFunctions.printInventory(state["inventory"]);


                if (toDoList.Count > 0)
                {
                    theFunctions.print(toDoList[0].name);
                }

            }
            */

        /*
        //if (this.name == "NPC pickpocket")
        if (this.name == "NPC shopkeeper")
        {
            theFunctions.print("==================================================");
            theFunctions.printPlan(toDoList);
        }
        */


        /*
        //make sure list isn't empty, remove completed action:
        if (toDoList.Count > 0)
        {
            //ad hoc for now, remove action if it is done
            if (theFunctions.isThisActionDone(toDoList[0], state))
            {
                toDoList.Remove(toDoList[0]);
            }
        }
        */

        //theFunctions.printState(state);
        //printPlan(toDoList);
        /*
        if (this.name == "NPC")
        {
            theFunctions.print("---------------------END OF UPDATE----------------------");
        }
        */
    }

    public void printActionForSpecificNPC(action x)
    {
        //List<List<action>>


        //to help, encapsulating this:

        if (this.name == "NPC")
        {
            print("00000000000000000  Action:   000000000000000000");
            print(x.name);
        }

        
    }

    public void printNumberForSpecificNPC(int x)
    {
        if (this.name == "NPC 4")
        {
            print(x);
            print("no plan found for NPC 4??");
        }
        
    }

    public void printForSpecificNPC(string x)
    {
        if (this.name == "NPC 4")
        {
            print(x);
        }
    }


    public void printPlanWithQuantities(List<action> plan)
    {
        //print quantities in effects, for now

        string printout = string.Empty;

        printout += "[ ";

        foreach (action listItem in plan)
        {
            printout += actionToTextDeep(listItem);
        }

        printout += "]";

        print(printout);
    }

    public string actionToTextDeep(action thisAction)
    {
        string printout = string.Empty;

        printout += thisAction.name + ": ";

        printout += "(PREREQS: ";
        foreach (actionItem effect in thisAction.prereqs)
        {
            //printout += "( ";
            //printout += effect.name + ": " + effect.item.quantity;
            //printout += ")";
            printout += actionItemToTextDeep(effect);
            printout += ", ";
        }
        printout += "), ";

        printout += "(EFFECTS: ";
        foreach (actionItem effect in thisAction.effects)
        {
            //printout += "( ";
            //printout += effect.name + ": " + effect.item.quantity;
            //printout += ")";
            printout += actionItemToTextDeep(effect);
            printout += ", ";
        }
        printout += ")";

        return printout;
    }

    public string stateItemToTextDeep(stateItem thisStateItem)
    {
        string printout = string.Empty;


        printout += "( ";
        printout += thisStateItem.name + ": +" + thisStateItem.quantity;
        printout += ")";

        return printout;
    }

    public string actionItemToTextDeep(actionItem thisActionItem)
    {
        string printout = string.Empty;

        
        printout += "( ";
        if (thisActionItem.inStateOrNot == true)
        {
            printout += thisActionItem.name + ": +" + thisActionItem.item.quantity;
        }
        else
        {
            printout += thisActionItem.name + ": -" + thisActionItem.item.quantity;
        }
        //printout += thisActionItem.name + ": " + thisActionItem.item.quantity;
        printout += ")";

        return printout;
    }

    public void printKnownActionsDeeply(List<action> knownActions)
    {
        //print quantities in effects, for now

        printPlanWithQuantities(knownActions);


    }

    public void printInventoryDeep(List<stateItem> inv)
    {
        //actionItemToTextDeep

        string printout = string.Empty;

        foreach (stateItem item in inv)
        {
            printout += stateItemToTextDeep(item) + ' ';
        }

        print(printout);
    }

    ////////////////////////////////////////////////
    //                Planning
    ////////////////////////////////////////////////

    public List<List<action>> planningPhase(actionItem goal, List<action> knownActions, Dictionary<string, List<stateItem>> state)
    {
        //full planning phase
        //NO ITERATION of this function
        //able to modify inputs to problemSolver

        //so
        //first check if goal is accomplished in state
        //if it's accomplished, we don't need a plan [we should discard this goal from AI1, but i think it should not give this function 
        //goals that are finished.  but still this can be a useful place to check]
        if(isStateAccomplished(goal, state) == false)
        {
            //good, now to handle PARTIAL completion of quantities.
            //if state has part of the goal quantity, "expend" that state quantity to zero, and subtract it from the goal
            //[what if a goal involves more than one quantity?  is that ever possible?  ex:  need both apples AND oranges?  dunno...]
            //[both of these modified versions must be DEEP COPIES]




            //print("/////////////////////////////////////////////////////////////////////");
            //print("/////////////////////////////////////////////////////////////////////");
            //print("/////////////////////////////////////////////////////////////////////");
            //print("/////////////////////// STARTING INVESTIGATION ///////////////////////");
            //print("/////////////////////////////////////////////////////////////////////");
            //print("/////////////////////////////////////////////////////////////////////");
            //print("/////////////////////////////////////////////////////////////////////");

            //printState(state);

            //so, first find how much of the desired quantity is in "state"
            //only do deep copies etc. if it is greater than zero.
            //kinda a repeat.  should do this BEFORE checking if state is accomplished.  this is duplicating code.

            //first, create deep copies here:
            actionItem goal2 = premadeStuff.deepActionItemCopier(goal);
            Dictionary<string, List<stateItem>> state2 = deepStateCopyer(state);

            //now, find item in state, 
            //record its quantity in another variable [or subtract from goal immediately]
            //set it to zero in state
            //subtract quantity from goal

            //wait i'm gonna do all that in the function:


            List<List<action>> planList = new List<List<action>>();
            planList = problemSolver(goal2, knownActions, state2);

            if (planList == null)
            {
                //maybe should return null, but for now, imagination step (after this) works fine with returning blanks, i think.  
                //so just return a blank
                List<List<action>> blank = new List<List<action>>();
                return blank;
            }
            else
            {
                return planList;
            }
        }
        else
        {
            //so, goal is already accomplished.  i don't think this should ever happen
            print("goal is already accomplished here, i don't think this should happen");
            //return a blank list
            List<List<action>> blankList = new List<List<action>>();
            return blankList;
        }

        
    }
    
    public List<List<action>> problemSolver(actionItem goal, List<action> knownActions, Dictionary<string, List<stateItem>> state)
    {
        //AKA:  planMapper
        //problem solver MUST be fed deep copies [of state AND goal] the first time it is called
        //this is because of the state-zeroing step that modifies state and goal
        
        
        
        //need a LIST of plans because there can be all kinds of different ways to acheive a goal
        //in fact, every single step of one plan can be absent from another plan
        //one list for each of phases 2 and 3.  then stitch together into final list later.
        //should initialize as null for my failure handling system???
        List<List<action>> planList = new List<List<action>>();
        List<List<action>> prereqsPlanList = new List<List<action>>();
        List<List<action>> quantsPlanList = new List<List<action>>();

        //eventually needs to be an action LIST:
        List<action> actionOptions = new List<action>();
        //for now, ad-hoc, need an action
        action newAction = null;


        //first just make sure we need a plan at all (could remove this?):
        //indeed, for state-zeroing to work, and not go negative, this should be handled either
        //outside, or earlier, or simultaneously with state-zeroing
        if (isStateAccomplished(goal, state) == true)
        {
            printAlways("SHOULD NEVER HAPPEN!!!!!!!!!!!!!!!!!!!!!");
            return planList;


        }



        //first phase, find an action:
        //[currently only finds first useful action?  not ALL useful actions?]
        newAction = actionFindingPhase(goal, knownActions, state);
        //return null if previous phase failed:
        if (newAction == null)
        {
            return null;
        }

        //print("before 2nd phase, prereqs:");
        //printPlanList(planList);
        //print(actionToTextDeep(newAction));

        //so if we have an action, we can do 2nd phase:  prereqs:
        prereqsPlanList = thePrereqPhase(newAction, knownActions, state, prereqsPlanList); 
        //return null if previous phase failed:
        if (prereqsPlanList == null)
        {
            return null;
        }

        //print("AFTER 2nd phase, prereqs, and BEFORE 3rd phase, quantities:");
        //printPlanList(prereqsPlanList);


        //so prereqs should be handled, now for 3rd step [quantities]
        quantsPlanList = quantitiesPhase(newAction, goal, knownActions, state);
        //return null if previous phase failed:
        if (quantsPlanList == null)
        {
            return null;
        }



        //print("AFTER 3rd phase, quantities:");
        //printPlanList(quantsPlanList);

        //so, now 4th phase, MERGING
        planList = mergingPhase(newAction, planList, prereqsPlanList, quantsPlanList);
        
        //should be no way for 4th phase to fail.

        return planList;
    }

    public action actionFindingPhase(actionItem goal, List<action> knownActions, Dictionary<string, List<stateItem>> state)
    {
        //does this need "state"?


        //eventually needs to be an action LIST:
        List<action> actionOptions = new List<action>();
        //for now, ad-hoc, need an action
        action newAction = null;
        //should feed empty one in???
        //should feed empty one in???

        //[currently only finds first useful action?  not ALL useful actions?]
        //first check if it's a type we can just quickly generate, rather than searching for:
        if (goal.stateCategory == "inventory" && goal.name != "money" && goal.name != "resource1")
        {
            //printNumberForSpecificNPC(1);
            //so we need an inventory item
            //and it's NOT money
            //so here we can generate a "buy" or "steal" or whatever type of action to fill that

            newAction = generateActionOnTheFly(goal, knownActions, state);

            //print(actionToTextDeep(newAction));

            //BUT NEED TO DEEP COPY NEW ACTION BEFORE FEEDING IT IN???  BECAUSE OF STATE ZEROING???
            //AND WAHT ABOUT THE WAY IT MODIFIES STATE AS WELL?  DOES THAT NEED TO BE DEEP COPIED OR
            //IS IT GOOD TO KEEP THAT CHANGE FOR FUTURE PLANNING???
            //!!!!!!!!!!!!WHY IS THIS HERE???  SHOULDN'T IT BE IN THE "so if we have an action, we can do the final step" BIT BELOW?????????
            //planList = thePrereqPhase(premadeStuff.deepActionCopier(newAction), knownActions, state, planList);

            //printNumberForSpecificNPC(2);
        }
        else
        {
            //ok, can't generate action, have to look for it in knownActions:
            //cycle through every known action, so we can check if any accomplish the goal:
            foreach (action thisAction in knownActions)
            {
                print("fffffffffffffffffffffffffffffffffffffffffffffffffff");
                print(actionToTextDeep(thisAction));
                //also have to look at each of their effects individually, see if the effect is to 
                foreach (actionItem thisEffect in thisAction.effects)
                {
                    //print(thisEffect.name);
                    //print(goal.name);
                    //finally, check if this action effect acheives the goal:
                    if (goal.name == thisEffect.name & goal.inStateOrNot == thisEffect.inStateOrNot)
                    {
                        //ADD QUANTITY CHECK HERE?!?!

                        //ok cool, we have an action that would acheive the goal
                        //but do we have a prereq to DO that aciton?  Have to check/try:

                        //what if we can't fill the prereqs??????????
                        newAction = thisAction;

                        print(actionToTextDeep(newAction));
                        //printKnownActionsDeeply(knownActions);
                    }
                    else
                    {
                        print("^^^^^^not a match");
                    }
                }
            }

        }

        if (newAction != null && newAction.type == "work")
        {
            //if it's part of faction, now allow use of faction inventory, and signal that...NPC is in work mode.....
            //[should turn OFF work mode after planning done, outside where it was called from, and 
            //turn it back on again...like during ENACTMENT phase?  possibly even before enactment prereqs are complete?]
            thisAI.atWork = true;

        }

        return newAction;
    }

    public List<List<action>> quantitiesPhase(action newAction, actionItem goal, List<action> knownActions, Dictionary<string, List<stateItem>> state)
    {
        //should return a plan to fill the LEFTOVER quantities.
        //in other words, this plan, combined with the newAction, will together 
        //fulfill the quantity requirement
        //the plan will be complete [including all prereqs it needs for its own completion]
        //but this plan will not fill the prereqs for the newAction, that is done in a separate phase
        //and the newAction will not be on the plan.
        //thus, this phase really does only take care of the "leftover" quantities

        //what if plan is NEEDED, but no plan can be found?  should really signal to the problemSolver function that 
        //entire planning needs to be scrapped? probably, eventually...


        //should feed empty one in???
        List<List<action>> planList = new List<List<action>>();

        //state-zeroing step
        //would be nice to only do this when it's needed, not every (problemSolver?) function call.  but, ad-hoc for now
        //now:
        //find item in state, 
        //record its quantity in another variable [or subtract from goal immediately]
        //set it to zero in state
        //subtract quantity from goal
        int quant = quantityToReach(goal, newAction);
        foreach (stateItem stateI in state[goal.stateCategory])
        {
            if (stateI.name == goal.name)
            {
                //found the item in state
                //now, modify the quantities:
                goal.item.quantity -= stateI.quantity;  //do i need to deep copy here?
                stateI.quantity = 0;

            }
        }

        //printState(state);

        //so now we have an action
        //handle quantities here???  but kinda tangled with needing to also fill prereqs...
        quant = quantityToReach(goal, newAction);
        //print(newAction.name + " " + goal.name + " " + quant);

        //set the quant check to be >1, instead of >0 [seems wrong though][see journal]
        //AHAH! quantityToReach SHOULD TAKE STATE INTO ACCOUNT!  DUH!
        //should really be "if quant is greater than the amount the action gives you"
        //...which would be "> 0"?
        if (quant > 0)
        {
            //need more action to fill goal quantity

            //printState(state);

            //create the leftover goal:
            actionItem newGoal = newLeftoverGoal(goal, quant);
            //print(newGoal.name + newGoal.item.quantity);
            //printPlanList(planList);
            
            //List<List<action>> planListToFillQuantity = new List<List<action>>();
            //should give us everything finished for the remaining quantity?
            //what if it doesn't?
            planList = problemSolver(newGoal, knownActions, state);

            //printPlanList(planList);


        }

        return planList;
    }

    public List<List<action>> thePrereqPhase(action thisAction, List<action> knownActions, Dictionary<string, List<stateItem>> state, List<List<action>> prereqsPlanList)
    {
        //take an action that we want to do, and consider its prereqs
        //try to fill them if needed
        //mostly just uses prereqFiller
        //ADDS THE ACTION TO THE PLANS IT RETURNS

        //what if we can't fill the prereqs??????????

        //print(actionToTextDeep(thisAction));
        //printState(state);
        //printInventoryDeep(state["inventory"]);

        //ok cool, we have an action that would acheive the goal
        //but do we have a prereq to DO that action?  Have to check:
        if (prereqStateChecker(thisAction, state) != true)
        {
            //So no, we don't have the prereqs for this action
            //so we'll see if we can FILL the prereqs!

            //print("2222222222222222222222222222222222222222222222222");
            //printPlanList(planList);


            //so, I think we get a bunch of COMPLETE plans from the prereq filling function [below]
            //and they should be totally finished and ready to simply add to the planList:
            //[...but what if not?  what if they are empty?  should NOT merge?...sigh....]
            //[wait, ADD to the planList??????  as in, the planList already has plans on it
            //that DON'T fill the prereqs?  shouldn't we REPLACE the planList????]
            //so...check planList.  if it's NOT empty, print a warning:
            
            //maybe it shouldn't always be empty?  then need the combinatorial merging step, too.  that's all.  
            //but....make sure i know what's going on here first...except:  what level of planList is this?
            //will it already have a plan of quantities or something?  do i need to know?
            List<List<action>> completedPlans = new List<List<action>>();
            prereqsPlanList = prereqFiller(thisAction, knownActions, state);


            //print("the planList itself before merging");
            //printPlanList(planList);

            //print("'''''completedPlans''''':");
            //printPlanList(completedPlans);

            //combinitorialMergingOfPrereqFillers;
            //planList = mergePlanLists(planList, completedPlans);

            //print("merged:");
            //printPlanList(planList);

        }

        return prereqsPlanList;
    }

    public List<List<action>> mergingPhase(action thisAction, List<List<action>> planList, List<List<action>> prereqsPlanList, List<List<action>> quantsPlanList)
    {
        //create a plan with the new action [there will always be one at this point]
        //if prereq plan(s) exists[use "foreach" to handle "if not"], add them to the start of the plan(s)
        //if a quantities plan(s) exists[use "foreach" to handle "if not"], add it to the start of the plan(s)
        //return result

        //does quantities last, which adds them to START of plan, which ensures if it's a series of duplicate actions, 
        //they can be done all together.  this is more efficient than goign back and forth between different actions.



        //create initial plan from new action:
        List<action> initialPlan = new List<action>();
        initialPlan.Add(thisAction);
        planList.Add(initialPlan);

        //foreach(List<action> wayToFillPrereqs in prereqsPlanList)

        //append initial plan to end of each plan:
        //wayToFillPrereqs = appendPlanToEndOfOtherPlan(wayToFillPrereqs, initialPlan);



        print("===================================START OF MERGING=======[action, quant filler, then prereq filler]======================");
        //printPlanList(planList);
        //printPlanList(quantsPlanList);
        //printPlanList(prereqsPlanList);


        //if a quantities plan(s) exists [use "foreach" to handle "if not"], add it to the start of the plan(s)
        if (quantsPlanList.Count() > 0)
        {
            //append each plan in planslist to end of each quantities plan:
            planList = combinatorialPlanMerging(quantsPlanList, planList);
        }


        //if prereq plan(s) exists [use "foreach" to handle "if not"], add them to the start of the plan(s)
        if (prereqsPlanList.Count() > 0)
        {
            //append initial plan to end of each prereq plan:
            planList = combinatorialPlanMerging(prereqsPlanList, planList);
        }


        printPlanList(planList);
        print("===================================END OF MERGING====================================");
        


        return planList;
    }

    public int quantityToReach(actionItem goal, action thisAction)
    {
        //take goal/prereq and an action [effect?]
        //output how much is left of goal AFTER doing that action
        //.....AND after taking state into account...sorta [not fully simulated, so...could be issue...]
        //[or, don't modify state here???]

        int amount = 0;

        foreach (actionItem thisEffect in thisAction.effects)
        {
            //find correct effect
            if (goal.name == thisEffect.name & goal.inStateOrNot == thisEffect.inStateOrNot)
            {
                amount = goal.item.quantity - thisEffect.item.quantity;
            }
        }
        
        return amount;
    }

    public actionItem newLeftoverGoal(actionItem oldGoal, int amount)
    {
        //given how much of the old goal IS LEFT OVER,
        //create a new goal to meet that quantity
        //this should sorta be like a "deep copy", right?  the original input is not modified???

        actionItem newGoal = premadeStuff.convertToActionItemBoolVersion(premadeStuff.quantityOfItemGenerator(oldGoal.item, amount), oldGoal.inStateOrNot);

        return newGoal;
    }

    public List<List<action>> prereqFiller(action thisAction, List<action> knownActions, Dictionary<string, List<stateItem>> state)
    {
        //this function takes an action with at least some unfilled prereqs
        //and returns the set of all plans that fill the unfilled prereqs
        //each ending with that action we wanted to do in the first place  NO NOT ANY MORE!
        //THESE PLANS WILL NOT CONTAIN "thisAction"!

        //what if we can't fill the prereqs??????????  seems to handle that, returns blank.


        List<List<action>> planList = new List<List<action>>();

        //one planList for each prereq, then merge later:
        List<List<List<action>>> plansForEachPrereq = new List<List<List<action>>>();

        //go through ALL prereqs, need to make sure they're ALL filled:
        //BUT DEEP COPY
        foreach (actionItem eachPrereq in thisAction.prereqs)
        {
            
            
            //only make plans to fill prereqs that aren't ALREADY filled:
            if (isStateAccomplished(eachPrereq, state) == false)
            {
                
                //so, found a prereq that isn't filled.  Need plans to fill it!
                List<List<action>> plansForThisPrereq = new List<List<action>>();

                plansForThisPrereq = problemSolver(premadeStuff.deepActionItemCopier(eachPrereq), knownActions, state);
                //if this ever fails, it will be null.  that means we've failed, just stop now:
                if (plansForThisPrereq == null)
                {
                    //printNumberForSpecificNPC(5);
                    //what is goal??????
                    //print("goal (eachPrereq.name) was:");
                    //print(eachPrereq.name);

                    //break;
                    return null;
                }
                else
                {
                    //this "else" means we have plans...
                    //(at least for THIS prereq)
                    //so add them to the list:
                    plansForEachPrereq.Add(plansForThisPrereq);

                    //printPlanListForSpecificNPC(plansForEachPrereq[0]);
                    //printNumberForSpecificNPC(6);
                }


                //if we've found zero plans, we've failed, just stop now:





            }
            
        }
        //recursionCounter -= 1
        
        
        //ok, now should be a simple matter of creating all combinations of plans
        //[[[[also, should be checking if things are empty or null or whatever...]]]]
        planList = combinitorialMergingOfPrereqFillers(plansForEachPrereq);
        
        //now, add thisAction to the end of ALL these plans (or make a plan if there are none):
        //planList = addActionToEndOfAllPlans(planList, thisAction);

        //printForSpecificNPC("jjjjjjjjjjjjjjjjjjjjjjj");
        //printPlanListForSpecificNPC(planList);
        //printForSpecificNPC("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");

        return planList;
    }

    public List<List<action>> mergePlanLists(List<List<action>> list1, List<List<action>> list2)
    {
        //merges plan LISTS.  leaves the plans themselves unchanged!

        foreach(List<action> plan in list2)
        {
            //was debugging:
            /*
            if(list1 == list2)
            {
                print("saaaaaaaaaaaaaaaaaaaaameeeeeeeeeeeeeeeeeeeee");
                Debug.Log(list1);
                Debug.Log(list2);
            }
            else
            {
                print("nooooooooooooooooooooooope?????????????????????????????");
                Debug.Log(list1);
                Debug.Log(list2);
            }
            */

            list1.Add(plan);
        }

        return list1;
    }

    public List<action> mergePlans(List<action> plan1, List<action> plan2)
    {
        //should add "plan2" on to the END of "plan1"

        foreach (action thisAction in plan2)
        {
            plan1.Add(thisAction);
        }

        return plan1;
    }

    public List<List<action>> combinitorialMergingOfPrereqFillers(List<List<List<action>>> plansForEachRegularPrereq)
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
        allPossiblePlansFromRemainingPrereqs = combinitorialMergingOfPrereqFillers(plansForEachRegularPrereq);

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

    public List<List<action>> combinatorialPlanMerging(List<List<action>> startingPlans, List<List<action>> endingPlans)
    {
        //ya this should be easier than the othe rcombinatorial function because we KNOW there are only TWO lists to combine.

        //create [blank] final list
        //so, really, just 2 for loops.
        //	for list 1
        //		for list 2
        //			create new plan
        //			add one to the other
        //			add to final list
        //return final list

        //create [blank] final list
        List<List<action>> allCombosPlanList = new List<List<action>>();

        foreach(List<action> startHalfOfPlan in startingPlans)
        {
            foreach (List<action> endHalfOfPlan in endingPlans)
            {
                //create new plan
                //add one to the other
                //add to final list
                List<action> thisFullPlan = new List<action>();
                thisFullPlan = mergePlans(startHalfOfPlan,endHalfOfPlan);
                allCombosPlanList.Add(thisFullPlan);
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
    
    public List<List<action>> addActionToEndOfAllPlans(List<List<action>> planList, action thisAction)
    {
        if (planList.Count > 0)
        {
            foreach (List<action> plan in planList)
            {
                plan.Add(thisAction);
            }
        }
        else
        {
            //just need to MAKE one plan, add it to the planList:
            List<action> newPlan = new List<action>();
            newPlan.Add(thisAction);
            planList.Add(newPlan);
        }
        

        return planList;
    }

    public action generateActionOnTheFly(actionItem thisPrereq, List<action> knownActions, Dictionary<string, List<stateItem>> state)
    {
        //no, I should probably have this just in the regular "problemSolver" function...I think?
        //sigh.
        //actions generated HERE will ALSO have their own prereqs to fill...
        //but I guess for those I can just call the "prereqFiller" function, and pass it this newly generated action...

        action newAction = new action();

        //so, need to figure out what type of action to generate
        //for now, it's just for "inventory" type prereqs
        //so check for that, and generate appropriate action:
        if (thisPrereq.stateCategory == "inventory")
        {
            //could buy or steal or ask for items for inventory
            //for now, I'll just buy...WAIT WHAT ABOUT PICKPOCKETING???
            //uhhh, ad-hoc check if this is the pickpocket NPC
            //WAIT BUYING WORKS FOR FOOD, BUT NOT FOR MONEY!!!
            //so, I should exclude money ones for now

            string generatedName = "buy" + thisPrereq.name;
            newAction = premadeStuff.actionCreator(generatedName, "buyFromStore", premadeStuff.wantedPrereqsLister(premadeStuff.moneyFromItem(thisPrereq.item)), premadeStuff.UNwantedPrereqsLister(), premadeStuff.wantedEffectsLister(thisPrereq.item), premadeStuff.UNwantedEffectsLister(premadeStuff.moneyFromItem(thisPrereq.item)), 1, premadeStuff.checkout);
            
            
        }
        else
        {
            print("failed to generate an action, code currently does not handle stateCategories of this type");
        }

        return newAction;

    }

    //Prereq and goal checking stuff:

    public bool isStateAccomplished(actionItem goal, Dictionary<string, List<stateItem>> state)
    {
        //assume false, then check and change to true where needed
        bool tf;
        tf = false;
        //print("3:  should see ''inStateOrNot'' == false");
        //Debug.Log(goal.name);
        //print(goal.inStateOrNot);

        //do stuff as if "wantedInstateOrNot" == true, then can reverse it later if it's false


        if (this.name == "NPC")
        {
            //print("XXXXXXXXXXXX      this planned action is deemed impossible: ");
            //print(currentAction.name);
            //printState(state);

            //print("3333333333333333333333333333333333");
            //print("need: " + goal.item.name + " = " + goal.item.quantity);

            //printState(state);
        }
        


        foreach (stateItem stateI in state[goal.stateCategory])
        {


            //////////////here's the C# way to say "if this item is in this list"...ya, it won't let you use the word "in" here:
            //oh no this is text, "name" needs to be just a actionItem!  Or something!
            //print("1111111111111111111111111111111111111111");
            //print(goal.name);
            //print(stateI.name);
            if (stateI.name == goal.name)
            {
                tf = true;
                

                //but false if it's not enough money:
                //"money"??? what about other items???
                if (stateI.quantity < goal.item.quantity)
                {

                    


                    tf = false;
                }
                else
                {
                    if (this.name == "NPC")
                    {
                        //print("2222222222222222222222222222");
                        //print("have: " + stateI.name + " = " + stateI.quantity);
                        //print("need: " + goal.item.name + " = " + goal.item.quantity);
                        //printState(state);

                    }

                    if (this.name == "NPC shopkeeper")
                    {
                        if (stateI.quantity > 2)
                        {
                            //print("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq");
                            //print("have: " + stateI.name + " = " + stateI.quantity);
                            //print("need: " + goal.item.name + " = " + goal.item.quantity);
                            //printState(thisAI.state);
                        }
                        

                    }
                }
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
            foreach (actionItem stateI in state[goal.stateCategory])
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

        if (this.name == "NPC")
        {
            //print("XXXXXXXXXXXX      this planned action is deemed impossible: ");
            //print(currentAction.name);
            //printState(state);

            //print("5555555555555555555555555555555555555");
            //print(actionToTextDeep(thisAction));

            
            //printState(state);
        }



        foreach (actionItem prereqX in thisAction.prereqs)
        {
            //print("don't count this");

            //print("need: " + prereqX.item.name + " = " + prereqX.item.quantity);

            if (this.name == "NPC")
            {
                //print("XXXXXXXXXXXX      this planned action is deemed impossible: ");
                //print(currentAction.name);
                //printState(state);

                //print("444444444444444444444444444444444444");
                //print("need: " + prereqX.item.name + " = " + prereqX.item.quantity);

                //printState(state);
            }

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

        if (this.name == "NPC")
        {
            //print("XXXXXXXXXXXX      this planned action is deemed impossible: ");
            //print(currentAction.name);
            //printState(state);

            //print("66666666666666666666666666666666666666666666");
            //print(actionToTextDeep(thisAction));

            //print("need: " + prereqX.item.name + " = " + prereqX.item.quantity);
            //printState(state);
        }
        

        //first [for some reason i don't recall, this one has to be first, otherwise it crashes], just check the regular prereqs using my regular prereqStateChecker:
        if (prereqStateChecker(thisAction, state) == false)
        {
            return false;
        }

        //now, check the location Prereq, IF THERE IS ONE:
        if (thisAction.locationPrereq != null)
        {
            if (locationPrereqChecker(target, thisAction) == false)
            {
                return false;
            }
        }
        //print(thisAction.name);
        
        //if the above didn't fail, return true:
        return true;

    }

    public bool locationPrereqChecker(GameObject target, action thisAction)
    {
        //for now just copy the proximity check from pickpocket action?

        //check distance
        GameObject thisNPC = gameObject;
        float distance = Vector3.Distance(thisNPC.transform.position, target.transform.position);
        //print("distance, for this target:");
        //print(target.name);
        //print(distance);

        //some actions need a bigger or smaller range.  Ad-hoc adding that here for now...
        //default is basically zero range:
        float theRange = 3.2f;

        //change range for some things:
        if(thisAction.name == "shootSpree")
        {
            theRange = 15;
        }


        if (distance < theRange)
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

    public Dictionary<string, List<stateItem>> deepStateCopyer(Dictionary<string, List<stateItem>> state)
    {
        Dictionary<string, List<stateItem>> newState = new Dictionary<string, List<stateItem>>();

        foreach (string categoryName in state.Keys)
        {
            newState[categoryName] = deepStateCategoryCopyer(categoryName, state);
        }
        return newState;
    }

    public List<stateItem> deepStateCategoryCopyer(string categoryName, Dictionary<string, List<stateItem>> state)
    {
        List<stateItem> newStateList = new List<stateItem>();
        //newState[keyString] = state[keyString];
        foreach (stateItem item in state[categoryName])
        {
            //but...kinda need to ALSO make a deep copy of EVERY INDIVIDUAL ITEM
            //including all their quantities etc...sigh...
            //so, here we go:

            newStateList.Add(deepStateItemCopier(item));
        }

        return newStateList;
    }

    public stateItem deepStateItemCopier(stateItem item)
    {
        stateItem newItem = null;
        newItem = premadeStuff.stateItemCreator(item.name, item.stateCategory);
        newItem.locationType = item.locationType;

        newItem.quantity = 0; //just zero it out first
        newItem.quantity += item.quantity;

        return newItem;
    }

    public Dictionary<string, List<stateItem>> removeStateItem(stateItem thisActionItem, Dictionary<string, List<stateItem>> state)
    {
        //just because they are not quite identical enough for .Remove to work properly I don't think

        //EDIT, turns out C# already has a way to do this.
        //though it is ugly and I don't know the syntax:
        //inventory.RemoveAll(x => x.name == myItem.name);


        //find the correct actionItem, then remove it
        foreach (stateItem eachActionItem in state[thisActionItem.stateCategory])
        {
            if (eachActionItem.name == thisActionItem.name)
            {
                //*****EVENTUALLY might want to check MORE than just the name
                //because I might need multiple actionItems with same name, but differences in other fields?

                state[thisActionItem.stateCategory].Remove(eachActionItem);
                break;
            }
        }

        return state;
    }

    public List<stateItem> removeInventoryItem(stateItem thisActionItem, List<stateItem> inventory)
    {
        //just because they are not quite identical enough for .Remove to work properly I don't think

        //EDIT, turns out C# already has a way to do this.
        //though it is ugly and I don't know the syntax:
        //inventory.RemoveAll(x => x.name == myItem.name);


        //find the correct actionItem, then remove it
        foreach (stateItem eachActionItem in inventory)
        {
            if (eachActionItem.name == thisActionItem.name)
            {
                //*****EVENTUALLY might want to check MORE than just the name
                //because I might need multiple actionItems with same name, but differences in other fields?

                inventory.Remove(eachActionItem);
                break;
            }
        }

        return inventory;
    }

    public Dictionary<string, List<stateItem>> implementALLEffects(action currentAction, Dictionary<string, List<stateItem>> imaginaryState)
    {
        foreach (actionItem FIXeachEffect in currentAction.effects)
        {
            if (this.name == "NPC" && FIXeachEffect.name == "money")
            {
                //print("amount of money to implement, should never be zero i don't think:");
                //printPlanListForSpecificNPC();
                //print(FIXeachEffect.item.quantity);
            }
            imaginaryState = implementTHISEffect(FIXeachEffect, imaginaryState);
        }
        return imaginaryState;
    }

    public Dictionary<string, List<stateItem>> implementTHISEffect(actionItem FIXeachEffect, Dictionary<string, List<stateItem>> imaginaryState)
    {
        //just have to get the "stateItem" from the actionItem:
        stateItem eachEffect = FIXeachEffect.item;

        if (this.name == "NPC" && eachEffect.name == "money")
        {
            //print("is this money effect zero???: ");
            //print(currentAction.name);
            //print(eachEffect.quantity);
        }
        

        if (FIXeachEffect.name == "food")
        {
            //print("before incrementing food:");
            //printInv
            //printState(imaginaryState);
        }

        if (FIXeachEffect.inStateOrNot == true)
        {

            if (eachEffect.stateCategory == "locationState")
            {
                imaginaryState["locationState"].Clear();
            }

            //imaginaryState[eachEffect.stateCategory].Add(eachEffect);
            incrementItem(imaginaryState[eachEffect.stateCategory], eachEffect, eachEffect.quantity);

            
        }
        else
        {
            //print("ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss");
            //print(actionItemToTextDeep(FIXeachEffect));
            //printState(imaginaryState);


            //imaginaryState = removeStateItem(eachEffect, imaginaryState);
            //imaginaryState[eachEffect.stateCategory].Remove(eachEffect);
            incrementItem(imaginaryState[eachEffect.stateCategory], eachEffect, (-1)*eachEffect.quantity);

            //print("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff");
            //printState(imaginaryState);

            //if there are zero left in inventory, completely remove the stateItem:
            //well, this should technically be handled over in "incrementItem, i think
            //if ()

        }

        if (FIXeachEffect.name == "food")
        {
            //print("AFTER incrementing food:");
            //printInv
            //printState(imaginaryState);
        }

        return imaginaryState;
    }

    public List<action> deepCopyPlan(List<action> thisPlan)
    {
        //uhhhh, is this enough to count as a "deep copy"????????
        //hardly seems like it

        List<action> copyPlan = new List<action>();

        foreach(action thisAction in thisPlan)
        {
            copyPlan.Add(thisAction);
        }

        return copyPlan;
    }


    public List<List<action>> simulatingPlansToEnsurePrereqs(List<List<action>> planList, List<action> knownActions, Dictionary<string, List<stateItem>> realState, int countdown)
    {
        //technically, each time I fix a plan, should have to imagine it again to be sure the fix ITSELF doesn't contain any impossible actions
        //keep working on all impossible plans until they are fixed...or give up and discard them
        

        //print("================================START simulation main function====================================");
        //printPlanList(planList);
        //printInt(countdown);
        //print(countdown);


        countdown -= 1;


        List<List<action>> fixedPlanList = new List<List<action>>();

        if (countdown < 1)
        {
            print("xxxxxxxxxxxxxxxxxxxxxxxxx 111 end of countdown reached 111 xxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            return fixedPlanList;
        }

        //printPlanList(planList);
        foreach (List<action> eachPlan in planList)
        {
            //print("LLLLLLLLLLLLL   1111111  LLLLLLLLLLLLLL");

            //yes, to fix ONE plan, might need to have a whole LIST of plans
            //because if it needs to be fixed, there can be MULTIPLE possible ways to fix it
            //it's still only one plan that was fixed
            List<List<action>> fixedPlan = new List<List<action>>();
            fixedPlan = simulateOnePlanFillPrereqs(eachPlan, knownActions, realState, countdown);

            //print("/////////////// done here 1 ////////////////");

            if(fixedPlan != null && fixedPlan.Count() > 0)
            {
                //print("/////////////// done here 2 ////////////////");
                
                fixedPlanList = mergePlanLists(fixedPlanList, fixedPlan);

                //printPlanList(fixedPlanList);
            }
        }
        
        return fixedPlanList;
    }

    public List<List<action>> simulateOnePlanFillPrereqs(List<action> thisPlan, List<action> knownActions, Dictionary<string, List<stateItem>> realState, int countdown)
    {
        //yes, to fix ONE plan, might need to have a whole LIST of plans
        //because if it needs to be fixed, there can be MULTIPLE possible ways to fix it
        //it's still only one plan that was fixed

        //technically, each time I fix a plan, should have to imagine it again to be sure the fix ITSELF doesn't contain any impossible acitons
        //keep working on all impossible plans until they are fixed...or give up and discard them


        //print("---------------------starting individual plan simulation----------------------");
        //printPlan(thisPlan);
        //print(countdown);
        //printInt(countdown);



        List<List<action>> constructingFixedPlansToReturn = new List<List<action>>();
        List<action> dummyPlan = new List<action>();
        constructingFixedPlansToReturn.Add(dummyPlan);

        countdown -= 1;

        if(countdown < 1)
        {
            print("xxxxxxxxxxxxxxxxxxxxxxxxx 222 end of countdown reached 222 xxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            return constructingFixedPlansToReturn;
        }

        bool soFarSoGood = true;

        Dictionary<string, List<stateItem>> imaginaryState = new Dictionary<string, List<stateItem>>();
        imaginaryState = deepStateCopyer(realState);

        List<action> staticIteratorPlan = new List<action>();
        staticIteratorPlan = deepCopyPlan(thisPlan);


        //setup done, now for the actual simulation etc.
        foreach (action currentAction in staticIteratorPlan)
        {
            if(soFarSoGood == true)
            {
                if (prereqStateChecker(currentAction, imaginaryState) == true)
                {
                    //print("an action is fine..................");
                    //so this action is fine

                    //add it to plan
                    //[there should only be one plan if we are here
                    //multiple plans only happen after a problem
                    //which causes bool to never come here again]
                    //[but it's a list, so this is still easiest way to add it]:
                    //SHOULD JUST REPLACE THIS WITH "addActionToEndOfAllPlans", but simplify that function so it's jus this simple, no if check, no creating blank list
                    foreach (List<action> planInProgress in constructingFixedPlansToReturn)
                    {
                        planInProgress.Add(currentAction);
                    }

                    

                    //now implement effects before moving on to next action:
                    imaginaryState = implementALLEffects(currentAction, imaginaryState);

                }
                else
                {
                    soFarSoGood = false;
                    //this "else" means prereqs are NOT met.
                    //so, need to try to find a plan to meet them,
                    //and that's literally the job of "prereqFiller", right?
                    //function returns plans that INCLUDE this current action
                    List<List<action>> waysToFillPrereqs;

                    //print("prereqs are NOT met.............");
                    //print("here is current action:");
                    //print(actionToTextDeep(currentAction));
                    //actionToTextDeep(currentAction);
                    //print("here is current imaginaryState:");
                    //printState(imaginaryState);

                    waysToFillPrereqs = prereqFiller(currentAction, knownActions, deepStateCopyer(imaginaryState));
                    
                    //print("here is waysToFillPrereqs:");
                    //printPlanList(waysToFillPrereqs);
                    //print("here is current imaginaryState:");
                    //printState(imaginaryState);

                    if (waysToFillPrereqs == null)
                    {
                        //maybe should return null, but for now, imagination step works fine with returning blanks, i think.  
                        //so just return a blank
                        List<List<action>> blank = new List<List<action>>();
                        return blank;
                    }

                    //will there ever be an "else"?
                    if (waysToFillPrereqs.Count > 0)
                    {
                        //now need to make all plans
                        //one for each way to fix current action

                        //print("here is constructingFixedPlansToReturn:");
                        //printPlanList(constructingFixedPlansToReturn);

                        //first just add current action here i guess:
                        waysToFillPrereqs = addActionToEndOfAllPlans(waysToFillPrereqs, currentAction);

                        //hmm, multipying with "constructingFixedPlansToReturn"?  but we aren't done contrsucting it, shouldn't we
                        //wait until the end or something?  or else restart right after this?  sorta do with recursion
                        constructingFixedPlansToReturn = multiplyPlansByAddingFixes(constructingFixedPlansToReturn, waysToFillPrereqs);

                        //print("here is constructingFixedPlansToReturn AGAIN:");
                        //printPlanList(constructingFixedPlansToReturn);

                        
                    }
                    else
                    {
                        print("this ever happen?");

                        //does this mean the plan is impossible?  return...blank???
                        List<List<action>> failPlanList = new List<List<action>>();
                        return failPlanList;
                    }
                }


            }
            else
            {
                //print("this else");
                //just add it to ALL plans [do not implement effects]
                //this needs to be inside the else so that we don't add the action twice if we filled prereqs
                //the plan-fixing branch already includes that action

                foreach (List<action> planInProgress in constructingFixedPlansToReturn)
                {
                    planInProgress.Add(currentAction);
                }
            }

        }

        if(soFarSoGood == false)
        {
            //print("THIS SEEMS DUPLICATE SHOULD THIS EVER HAPPEN?!?!?!?!?!? oh right, does need to do recursion i think");
            //do recursion
            constructingFixedPlansToReturn = simulatingPlansToEnsurePrereqs(constructingFixedPlansToReturn, knownActions, realState, countdown);

            return constructingFixedPlansToReturn;
        }
        else
        {
            //print("fine i guess?");
            //this "else" means ORIGINAL  plan should be fine
            //return thisPlan;
            //but, we need to return a LIST data structure, so easiest for now:
            return constructingFixedPlansToReturn;
        }
        

    }
    
    public int findFirstImpossibleAction(List<action> plan, List<action> knownActions, Dictionary<string, List<stateItem>> realState)
    {
        //why is "knownActions" an input?  probably accidentally kept from a copy paste
        alert();
        //imagines it's way through a plan list
        //returns the index number of the first action on that list that CANNOT be completed
        //if all actions can be completed fine, it returns negative two

        int noProblem;
        noProblem = -2;

        int counter;
        counter = 0;

        Dictionary<string, List<stateItem>> imaginaryState = new Dictionary<string, List<stateItem>>();
        imaginaryState = deepStateCopyer(realState);

        

        foreach(action currentAction in plan)
        {
            if (impossibleActionprereqStateChecker(currentAction, imaginaryState) != true)
            {
                //print("XXXXXXXXXXXX      this planned action is deemed impossible: ");
                //print(currentAction.name);
                    
                return counter;
            }

            //print("bbbbbbbbbbbbbbbbbbbbbbbbbbbb here??");
            //print("--------------imaginaryState at START of implementALLEffects");
            //printPlan(plan);
            //printState(imaginaryState);

            imaginaryState = implementALLEffects(currentAction, imaginaryState);
            //print("is this not updating state???");
            //printState(imaginaryState);
            counter += 1;

            //print("bbbbbbbbbbbbbbbbbbbbbbbbbbbb end");
            //print("--------------imaginaryState at START of implementALLEffects");
            //printState(imaginaryState);
            //printPlan(plan);
        }


        return noProblem;
    }

    public bool impossibleActionprereqStateChecker(action thisAction, Dictionary<string, List<stateItem>> state)
    {
        //assume true, then check and change to false where needed
        bool tf;
        tf = true;

        foreach (actionItem prereqX in thisAction.prereqs)
        {
            //print("don't count this");
            if (isStateAccomplished(prereqX, state) == false)
            {
                
                //print("mmmmmmmmmmmmmmmmmmmmmmm      this planned action is deemed impossible: ");
                //print(thisAction.name);
                //print("BECAUSE of this prereq:");
                //print(prereqX.name);
                //printState(state);
                //printState(thisAI.planningState);
                //print(gameObject.name);




                return false;
            }
        }

        return tf;
    }


    public List<List<action>> multiplyPlansByAddingFixes(List<List<action>> oldPlanList, List<List<action>> theFixes)
    {
        //must not input an empty or null "theFixes"!

        //-if my "fixedPlansToReturn" is EMPTY and I have fixes, just add one EMPTY plan to it
        //-for each plan in ""fixedPlansToReturn", multiply them by making each version of each of them.

        //so, how to do that "multiply" step ?
        //-make all versions of it
        //-add all versions to a new planList
        //-return that NEW planList

        List<List<action>> constructingNewPlanList = new List<List<action>>();

        //first make sure there's at least one plan:
        if (oldPlanList.Count == 0)
        {
            List<action> dummyPlan = new List<action>();
            oldPlanList.Add(dummyPlan);
        }

        //now, multiply each plan on the oldPlanList:
        foreach(List<action> oldPlan in oldPlanList)
        {
            
            //now multiply, make every combination of this old plan and the new fixes to add:
            foreach(List<action> fix in theFixes)
            {
                List<action> constructingOneNewPlan = new List<action>();

                //fill this plan with the old plan thus far:
                constructingOneNewPlan = deepCopyPlan(oldPlan);

                constructingOneNewPlan = mergePlans(constructingOneNewPlan, fix);
                constructingNewPlanList.Add(constructingOneNewPlan);
            }

        }

        return constructingNewPlanList;
    }




    //other diagnostics:
    public void alert()
    {
        //check an error condition, print only when it happens
        //scatter this funciton throughout the code, 
        //then can check the traceback to see which one first printed it
        //and that is aproximately wher ethe trouble started

        if(conditionX() == true)
        {
            print("!!!!!!!!!! F O U N D !!!!!!!!!!!");
        }
    }

    public bool conditionX()
    {
        //return true if ALL conditions are met
        //that is if it is right NPC and the error is found
        //so, return false if it is wrong NPC, return false if error condition is not met

        //start with true
        //then if any ONE desired condition is false, it falsifies it
        bool tf = true;

        //junk:
        //print("===============================surely here START=============================");
        //printState(thisAI.state);
        //printKnownActionsDeeply(thisAI.knownActions);
        //print(itemToIncrement.quantity);
        //print(theItem.quantity);


        if (this.name != "NPC")
        {
            return false;
        }

        if(isActionModified() != true)
        {
            return false;
        }

        return tf;
    }

    public bool isActionModified()
    {
        //check knownActions
        //if (for the moment) food has hunger = 0, then return 
        //...true i guess.  "is action modified?  yes, true, it is"

        //junk:
        //print("===============================surely here START=============================");
        //printState(thisAI.state);
        //printKnownActionsDeeply(thisAI.knownActions);
        //print(itemToIncrement.quantity);
        //print(theItem.quantity);
        //printKnownActionsDeeply(thisAI.knownActions);
        foreach (action thisAction in thisAI.knownActions)
        {
            foreach(actionItem thisActionItem in thisAction.effects)
            {
                //print(thisActionItem.item.name);
                //print(thisActionItem.item.quantity);
                if(thisActionItem.item.name == "hungry" && thisActionItem.item.quantity == 0)
                {
                    //print("=======???hewwwwwwoooo????==========");
                    return true;
                }
                
            }
        }

        //print("=======???????????????????==========");

        return false;

    }


    public void startTest()
    {
        //NPC pickpocket
        if (testTime == true && this.name == "NPC")
        {
            print("===============================surely here START=============================");
            //print("??????????????????????????????????????????????");
            //printState(thisAI.state);
            //printKnownActionsDeeply(thisAI.knownActions);
            //print(itemToIncrement.quantity);
            //print(theItem.quantity);
        }
    }
    
    public void endTest()
    {
        //NPC pickpocket
        if (testTime == true && this.name == "NPC")
        {
            //print(itemToIncrement.quantity);
            //print(theItem.quantity);
            //printState(thisAI.state);
            //printKnownActionsDeeply(thisAI.knownActions);
            print("===============================surely here END=============================");
        }
    }

    public void testSwitch()
    {
        if(testTime == false)
        {
            testTime = true;
        }
        else
        {
            testTime = false;
        }
    }



    public void testOn()
    {
        testTime = true;
    }
    public void testOff()
    {
        testTime = false;
    }

}
