using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using static tagging2;
using static enactionCreator;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static interactionCreator;
using System.Threading;
using static UnityEditor.PlayerSettings;
using UnityEngine.XR;
using UnityEngine.Assertions;


public class AIHub3 : planningAndImagination, IupdateCallable
{
    nestedLayerDebug debug;
    public NavMeshAgent currentNavMeshAgent;
    virtualGamepad vGpad;

    GameObject placeholderTarget1;

    int adhocCooldown = 0;
    public bool printThisNPC = false;
    string storedMessage = "";
    bool test = true;


    public List<IupdateCallable> currentUpdateList { get; set; }

    //public Dictionary<List<condition>, List<planEXE2>> adHocMultiGoalConditionDictionary = new Dictionary<List<condition>, List<planEXE2>>();
    //List<adHocPlanRefillThing> adHocParallelPlanTypeThing = new List<adHocPlanRefillThing>();
    List<planEXE2> SUPERadHocParallelPlanList = new List<planEXE2>();


    void Awake()
    {
        //placeholderTarget1 = new GameObject();
        vGpad = genGen.singleton.ensureVirtualGamePad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("=================================      START      ===============================");

        placeholderTarget1 = new GameObject();
        placeholderTarget1.transform.parent = this.transform;

        //      SUPERadHocParallelPlanList.Add(new seriesEXE(grabAndEquipPlan2(interType.shoot1), safeGunless()));
        //adHocParallelPlanTypeThing.Add(new adHocRefillThingGeneral(safeGunless(), grabAndEquipPlan2(interType.shoot1)));
        //adHocParallelPlanTypeThing.Add(new adHocGoGrabAndEquipRefill(safeGunless(), this));
        //adHocParallelPlanTypeThing.Add(new adHocRandomWanderRefill(safeWithGun(), randomWanderPlan(), this.gameObject));
        //adHocParallelPlanTypeThing.Add(new adHocRefillThingGeneral(unsafeGunless(), combatDodgeWithoutGun()));
        //adHocParallelPlanTypeThing.Add(new adHocRefillThingGeneral(unsafeWithGun(), combatDodgeWithGun()));
        //printThisNPC = true;
        /*
        adHocParallelPlanTypeThing.Add(new adHocPlanRefillThing(unsafeGunless(), combatDodgeWithoutGun()));
        adHocParallelPlanTypeThing.Add(new adHocPlanRefillThing(unsafeWithGun(), combatDodgeWithGun()));
        */

        /*
        threatLineOfSight()
        hasNoGun()
        adHocGoalConditionDictionary[safeGunless()] = grabAndEquipPlan2(interType.shoot1);
        adHocGoalConditionDictionary[safeWithGun()] = randomWanderPlan();
        adHocGoalConditionDictionary[unsafeGunless()] = combatDodgeWithoutGun();
        adHocGoalConditionDictionary[unsafeWithGun()] = combatDodgeWithGun();

        */

        //fullPlan = goGrabThenEquip(interType.shoot1);
    }

    public void SUPERadHocRefillThing()
    {

    }


    private List<condition> unsafeWithGun()
    {
        List<condition> theList = new List<condition>();
        theList.Add(new adocThreatLineOfSightCondition(this.gameObject));
        theList.Add(new adHocHasNoGunCondition(this.gameObject, false));

        return theList;
    }

    private List<condition> unsafeGunless()
    {
        List<condition> theList = new List<condition>();
        theList.Add(new adocThreatLineOfSightCondition(this.gameObject));
        theList.Add(new adHocHasNoGunCondition(this.gameObject));


        return theList;
    }

    private List<condition> safeWithGun()
    {
        List<condition> theList = new List<condition>();
        theList.Add(new adocThreatLineOfSightCondition(this.gameObject, false));
        theList.Add(new adHocHasNoGunCondition(this.gameObject, false));


        return theList;
    }

    private List<condition> safeGunless()
    {
        List<condition> theList = new List<condition>();
        theList.Add(new adocThreatLineOfSightCondition(this.gameObject, false));
        theList.Add(new adHocHasNoGunCondition(this.gameObject));


        return theList;
    }




    // Update is called once per frame
    void Update()
    {
        //conditionalPrint("=======================     REGULAR     Update()............");
        adhocCooldown++;
    }



    public void callableUpdate()
    {
        //Debug.Log("=======================callableUpdate()............");
        //  conditionalPrint("======================================================callableUpdate()............");

        int whichOne = 1;


        SUPERadHocParallelPlanList.Add(null);
        SUPERadHocParallelPlanList.Add(null);
        SUPERadHocParallelPlanList.Add(null);
        SUPERadHocParallelPlanList.Add(null);
        SUPERadHocParallelPlanList.Add(null);

        if (SUPERadHocParallelPlanList[whichOne - 1] ==null || SUPERadHocParallelPlanList[whichOne-1].error()) 
        { SUPERadHocParallelPlanList[whichOne-1] = new seriesEXE(grabAndEquipPlan2(interType.shoot1), safeGunless()); }
        SUPERadHocParallelPlanList[whichOne-1].execute();

        whichOne++;


        if (SUPERadHocParallelPlanList[whichOne - 1] == null || SUPERadHocParallelPlanList[whichOne - 1].error())
        { SUPERadHocParallelPlanList[whichOne - 1] = new seriesEXE(combatDodgeWithoutGun(), unsafeGunless()); }
        SUPERadHocParallelPlanList[whichOne - 1].execute();

        whichOne++;

        
        if (SUPERadHocParallelPlanList[whichOne - 1] == null || SUPERadHocParallelPlanList[whichOne - 1].error())
        { SUPERadHocParallelPlanList[whichOne - 1] = new seriesEXE(randomWanderPlan(), safeWithGun()); }
        SUPERadHocParallelPlanList[whichOne - 1].execute();

        whichOne++;
        


        combatBehaviorPlan1();
        //adHocParallelPlanTypeThing.Add(new adHocRefillThingGeneral(unsafeWithGun(), combatDodgeWithGun()));

        if (SUPERadHocParallelPlanList[whichOne - 1] == null || SUPERadHocParallelPlanList[whichOne - 1].error())
        { SUPERadHocParallelPlanList[whichOne - 1] = new seriesEXE(combatBehaviorPlan11(), unsafeWithGun()); }
        SUPERadHocParallelPlanList[whichOne - 1].execute();

        whichOne++;


        if (SUPERadHocParallelPlanList[whichOne - 1] == null || SUPERadHocParallelPlanList[whichOne - 1].error())
        { SUPERadHocParallelPlanList[whichOne - 1] = new seriesEXE(combatBehaviorPlan12(), unsafeWithGun()); }
        SUPERadHocParallelPlanList[whichOne - 1].execute();

        whichOne++;


        //adHocParallelPlanTypeThing.Add(new adHocRefillThingGeneral(safeGunless(), grabAndEquipPlan2(interType.shoot1)));
        //adHocParallelPlanTypeThing.Add(new adHocGoGrabAndEquipRefill(safeGunless(), this));
        //adHocParallelPlanTypeThing.Add(new adHocRandomWanderRefill(safeWithGun(), randomWanderPlan(), this.gameObject));
        //adHocParallelPlanTypeThing.Add(new adHocRefillThingGeneral(unsafeGunless(), combatDodgeWithoutGun()));

        /*
        foreach (adHocPlanRefillThing thing in adHocParallelPlanTypeThing)
        {
            thing.debugPrint = this.printThisNPC;
            //debugPlanRefillThing(thing);
            //  conditionalPrint(whichOne + ")  >>>>>>>>>>>>>>>>>>>>time to print an adhoc refill thing, we can identify it by its conditions:  ");
            //  conditionalPrint(thing.conditionsAsText());



            //      conditionalPrint("thing.doUpdate()");
            thing.doUpdate();


            //  conditionalPrint("///////////////////////////////////////////////////END of printouts for #" + whichOne);
            whichOne++;
        }
        */


        /*

        //plan or no plan
        //threat line of sight or no threat line of sight
        //has gun or no gun

        if(fullPlan == null)
        {
            storedMessage = "(fullPlan == null),  ";
            storedMessage += "threatLineOfSight(): " + threatLineOfSight() + ",  ";
            storedMessage += "hasNoGun(): " + hasNoGun() + ",  ";
            if (threatLineOfSight())
            {
                if (hasNoGun())
                {
                    storedMessage += "thus combatDodgeWithoutGun()";
                    fullPlan = combatDodgeWithoutGun();
                }
                else
                {
                    storedMessage += "thus combatDodgeWithGun()";
                    fullPlan = combatDodgeWithGun();
                }
            }
            else
            {
                if (hasNoGun())
                {
                    storedMessage += "thus grabAndEquipPlan2(interType.shoot1)";
                    fullPlan = grabAndEquipPlan2(interType.shoot1);
                }
                else
                {
                    storedMessage += "thus randomWanderPlan()";
                    fullPlan = randomWanderPlan();
                }
            }
        }
        else
        {
            //fullPlan.conditionalPrint(printThisNPC);
            if (fullPlan.error()) { fullPlan = null;

                conditionalPrint("fullPlan.error()) { fullPlan = null;"); 
                return; }
            if (fullPlan.endConditionsMet())
            { //Debug.Log("fullPlan.endConditionsMet()");
                conditionalPrint("fullPlan.endConditionsMet()");
                //storedMessage = "erroneous plan insertion, combatBehaviorPlan1()";
                //fullPlan = combatBehaviorPlan1();
                fullPlan = null;
                return;
            }

            conditionalPrint(storedMessage);
            //conditionalPrint("we have a plan, fullPlan.execute()");
            //fullPlan.conditionalPrint(printThisNPC);
            fullPlan.execute();
        }


        */


        /*
        if (fullPlan == null)
        {


            conditionalPrint("fullPlan == null");
            if (hasNoGun())
            {
                fullPlan = goGrabThenEquip(interType.shoot1);
                return;
            }
            fullPlan = combatBehaviorPlan1();
            return;
        }

        fullPlan.conditionalPrint(printThisNPC);
        if (fullPlan.error()) { fullPlan = null; return; }
        if (fullPlan.endConditionsMet())
        { //Debug.Log("fullPlan.endConditionsMet()");
            conditionalPrint("fullPlan.endConditionsMet()");
            fullPlan = combatBehaviorPlan1(); return;
        }


        conditionalPrint("we have a plan, fullPlan.execute()");
        fullPlan.conditionalPrint(printThisNPC);
        fullPlan.execute();

        */


    }




    public planEXE2 combatBehaviorPlan11()
    {
        //ad-hoc hand-written plan
        GameObject target2 = pickRandomObjectFromList(threatListWithoutSelf());

        if (target2 == null)
        {
            //Debug.Log("can't do combat behavior plan, (target2 == null), probably this means the NPC is alone in the map zone, no one else is there to count as a ''threat''");
            return null;
        }

        planEXE2 secondShell = new seriesEXE();
        secondShell.Add(aimTargetPlan2(target2));
        secondShell.Add(firePlan4(interType.shoot1, target2));
        secondShell.untilListFinished();

        //planEXE2 parallel = new parallelEXE(secondShell, combatDodgeEXE2());
        //parallel.untilListFinished();
        return secondShell;
    }

    public planEXE2 combatBehaviorPlan12()
    {
        return combatDodgeEXE2();
    }











    private void debugPlanRefillThing(adHocPlanRefillThing thing)
    {
        //Debug.Log("looooooooooooooop   thing:  " + thing);
        //      conditionalPrint((":::::::::::::::::::    debugPlanRefillThing    ::::::::::::::::::::::  " + thing));
        //      conditionalPrint(thing.conditionsAsText());
        if (thing == null)
        {
            conditionalPrint("(thing == null)");
            //Debug.Log("(thing == null)"); continue;
        }
        //Debug.Log("thing.doUpdate()");

        if (thing == null) { return; }
        if(thing.theCurrentPlan == null) { return; }
        if (thing.theCurrentPlan.exeList == null) { return; }
        //      conditionalPrint(("thing.theCurrentPlan.exeList.Count:  " + thing.theCurrentPlan.exeList.Count));

        foreach (planEXE2 thisPlanEXE in thing.theCurrentPlan.exeList)
        {

            //      conditionalPrint(thisPlanEXE.asText());
        }

    }

    public planEXE2 grabAndEquipPlan2(interType interTypeX)
    {

        planEXE2 zerothShell = new seriesEXE(goGrabPlan2(interTypeX));
        zerothShell.Add(equipX2(interTypeX));
        zerothShell.untilListFinished();

        return zerothShell;
    }


    private planEXE2 walkToObject(GameObject target, float offsetRoom = 0f)
    {
        if (target == null)
        {
            Debug.Log("target is null, so plan to walk to target is null");
            Debug.Log(target.GetInstanceID());
            return null;
        }
        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();
        Debug.Assert(theNavAgent != null);


        planEXE2 theEXE = new vect3EXE2(theNavAgent, target);
        proximity condition = new proximity(this.gameObject, target, offsetRoom * 1.4f);
        condition.debugPrint = theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        theEXE.debugPrint = theNavAgent.debugPrint;

        return theEXE;
    }


    private planEXE2 combatDodgeWithoutGun()
    {
        return combatDodgeEXE2();
    }

    private planEXE2 combatDodgeWithGun()
    {
        return combatBehaviorPlan1();
    }







    private planEXE2 goGrabPlan2(interType interTypeX)
    {

        //ad-hoc hand-written plan
        GameObject target = pickRandomObjectFromList(allNearbyEquippablesWithInterTypeX(interTypeX));

        if (target == null) { return null; }
        //Debug.DrawLine(this.gameObject.transform.position, target.transform.position, Color.magenta, 44f);


        debugTargetDistance(this.gameObject, target);

        planEXE2 firstShell = new seriesEXE();
        firstShell.Add(walkToTarget2(target, 1.8f));
        firstShell.Add(aimTargetPlan2(target));
        firstShell.Add(firePlan4(interType.standardClick, target));
        firstShell.untilListFinished();

        return firstShell;
    }

    private planEXE2 firePlan4(interType interTypeX, GameObject target)
    {

        //either playable will already have the type, or it might be in equipper slots
        rangedEnaction grabEnact1;
        grabEnact1 = enactionWithInterTypeXOnObjectsPlayable(this.gameObject, interTypeX);

        if (grabEnact1 == null)
        {
            return getFireEnactionFromEquipperSlots(interTypeX);
        }

        planEXE2 exe1 = grabEnact1.toEXE(null);

        //Debug.Log("grabEnact1.theCooldown:  " + grabEnact1.theCooldown);
        //Debug.Log("grabEnact1.theCooldown.cooldownMax:  " + grabEnact1.theCooldown.cooldownMax);
        //Debug.Log("grabEnact1.theCooldown.cooldownTimer:  " + grabEnact1.theCooldown.cooldownTimer);
        //      exe1.startConditions.Add(grabEnact1.theCooldown);
        exe1.atLeastOnce();
        //condition thisCondition = new enacted(exe1);
        //exe1.endConditions.Add(thisCondition);

        return exe1;
    }

    private planEXE2 getFireEnactionFromEquipperSlots(interType interTypeX)
    {
        GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, equipperContents());

        //oh no it can ALSO be null
        if (theItemWeWant == null)
        {
            conditionalPrint("(theItemWeWant == null)");
            //Debug.DrawLine(Vector3.zero, this.transform.position, Color.magenta, 6f);
            return null;
            //return goGrabPlan1(interType.shoot1);
        }


        //Debug.Assert(theItemWeWant != null);

        //grabEnact1 = theItemWeWant.GetComponent<rangedEnaction>();
        return theItemWeWant.GetComponent<equippable2>().planshell;
    }

    public planEXE2 goGrabThenEquip(interType interTypeX)
    {
        planEXE2 zerothShell = new seriesEXE(goGrabPlan1(interTypeX));
        zerothShell.Add(equipX2(interTypeX));
        zerothShell.untilListFinished();

        return zerothShell;
    }

    public planEXE2 goGrabPlan1(interType interTypeX)
    {
        //ad-hoc hand-written plan
        GameObject target = pickRandomObjectFromList(allNearbyEquippablesWithInterTypeX(interTypeX));

        if (target == null) { return null; }

        debugTargetDistance(this.gameObject, target);

        planEXE2 firstShell = new seriesEXE();
        firstShell.Add(walkToTarget2(target, 0.8f));
        firstShell.Add(aimTargetPlan2(target));
        firstShell.Add(firePlan3(interType.standardClick, target));
        firstShell.untilListFinished();

        return firstShell;
    }

    public planEXE2 combatBehaviorPlan1()
    {
        //ad-hoc hand-written plan
        GameObject target2 = pickRandomObjectFromList(threatListWithoutSelf());

        if(target2 == null) 
        {
            //Debug.Log("can't do combat behavior plan, (target2 == null), probably this means the NPC is alone in the map zone, no one else is there to count as a ''threat''");
            return null; 
        }

        planEXE2 secondShell = new seriesEXE();
        secondShell.Add(aimTargetPlan2(target2));
        secondShell.Add(firePlan4(interType.shoot1, target2));
        secondShell.untilListFinished();

        planEXE2 parallel = new parallelEXE(secondShell, combatDodgeEXE2());
        parallel.untilListFinished();
        return parallel;
    }

    private planEXE2 combatDodgeEXE2()
    {
        //ad-hoc hand-coded plan

        //calculate a position to "dodge" towards
        //place an empty "nav point" object there
        //then just use walkToTarget(target)

        //look at simpleDodge
        List<GameObject> threatList = threatListWithoutSelf();


        //conditionalPrint("inputs, threatList.Count:  " + threatList.Count);
        //conditionalPrint("inputs, this.transform.position:  " + this.transform.position);
        spatialDataPoint dataPoint = new spatialDataPoint(threatList, this.transform.position);
        dataPoint.debugPrint = printThisNPC;

        conditionalPrint("threatLineOfSight():  " + threatLineOfSight());
        Vector3 adHocThreatAvoidanceVector = dataPoint.applePattern();

        //conditionalPrint("output adHocThreatAvoidanceVector:  " + adHocThreatAvoidanceVector);
        //GameObject placeholderTarget1 = new GameObject();
        placeholderTarget1.transform.position = this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized * 44.7f;
        //debugTargetDistance(this.gameObject, placeholderTarget1);

        return walkToTarget2(placeholderTarget1, 1.9f);
    }

    private planEXE2 equipX2(interType interTypeX)
    {
        GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, getInventory());

        //IEnactaBool testE1 = new takeFromAndPutBackIntoInventory(this.gameObject);

        IEnactaBool testE1 = this.gameObject.GetComponent<takeFromAndPutBackIntoInventory>();

        //planEXE2 exe1 = new singleEXE(testE1, theItemWeWant);
        planEXE2 exe1 = testE1.toEXE(theItemWeWant);
        exe1.atLeastOnce();
        //condition thisCondition = new enacted(exe1);
        //exe1.endConditions.Add(thisCondition);
        return exe1;
    }


    public void debugTargetDistance(GameObject object1, GameObject object2)
    {

        Vector3 position1 = object1.transform.position;
        Vector3 position2 = object2.transform.position;
        Vector3 vectorBetween = position1 - position2;
        float distance = vectorBetween.magnitude;

        //Debug.Log("condition:  " + this);
        //Debug.Log("distance:  " + distance);
        //Debug.Log("desiredDistance:  " + desiredDistance);
        //Debug.DrawLine(position1, position2, Color.blue, 0.1f);


        if (printThisNPC)
        {
            Debug.Log("distance:  " + distance);
            Debug.DrawLine((position1 + Vector3.up), (position2 + Vector3.up), Color.green, 7f);

            Debug.DrawLine(position1, position1 + (Vector3.up * 105), Color.red, 7f);

            Debug.DrawLine(position2, position2 + (Vector3.up * 105), Color.magenta, 7f);
        }


    }



    public bool hasNoGun()
    {
        rangedEnaction grabEnact1;
        grabEnact1 = enactionWithInterTypeXOnObjectsPlayable(this.gameObject, interType.shoot1);



        if (grabEnact1 == null)
        {
            //ummm sloppy for now
            grabEnact1 = getFireEnactionFromEquipperSlotsToSeeIfNPCHasAGUn(interType.shoot1);
        }

        if (grabEnact1 == null) { return true; }

        return false;
    }

    private rangedEnaction getFireEnactionFromEquipperSlotsToSeeIfNPCHasAGUn(interType interTypeX)
    {
        GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, equipperContents());

        //oh no it can ALSO be null
        if (theItemWeWant == null)
        {
            //      conditionalPrint("(theItemWeWant == null)");
            //Debug.DrawLine(Vector3.zero, this.transform.position, Color.magenta, 6f);
            return null;
            //return goGrabPlan1(interType.shoot1);
        }


        //Debug.Assert(theItemWeWant != null);

        //grabEnact1 = theItemWeWant.GetComponent<rangedEnaction>();
        return theItemWeWant.GetComponent<rangedEnaction>();
    }

    private planEXE2 firePlan3(interType interTypeX, GameObject target)
    {

        //either playable will already have the type, or it might be in equipper slots
        rangedEnaction grabEnact1;
        grabEnact1 = enactionWithInterTypeXOnObjectsPlayable(this.gameObject, interTypeX);

        if (grabEnact1 == null)
        {
            GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, equipperContents());

            //oh no it can ALSO be null
            if (theItemWeWant == null)
            {
                //Debug.DrawLine(Vector3.zero, this.transform.position, Color.magenta, 6f);
                return goGrabPlan1(interType.shoot1);
            }


            //Debug.Assert(theItemWeWant != null);

            //grabEnact1 = theItemWeWant.GetComponent<rangedEnaction>();
            return theItemWeWant.GetComponent<equippable2>().planshell;
        }

        planEXE2 exe1 = grabEnact1.toEXE(null);

        //Debug.Log("grabEnact1.theCooldown:  " + grabEnact1.theCooldown);
        //Debug.Log("grabEnact1.theCooldown.cooldownMax:  " + grabEnact1.theCooldown.cooldownMax);
        //Debug.Log("grabEnact1.theCooldown.cooldownTimer:  " + grabEnact1.theCooldown.cooldownTimer);
        //      exe1.startConditions.Add(grabEnact1.theCooldown);
        exe1.atLeastOnce();
        //condition thisCondition = new enacted(exe1);
        //exe1.endConditions.Add(thisCondition);

        return exe1;
    }

    private planEXE2 firePlan2nd2(interType interTypeX, GameObject target)
    {

        //either playable will already have the type, or it might be in equipper slots
        rangedEnaction grabEnact1;
        grabEnact1 = enactionWithInterTypeXOnObjectsPlayable(this.gameObject, interTypeX);

        if (grabEnact1 == null)
        {
            GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, equipperContents());

            //oh no it can ALSO be null
            if(theItemWeWant == null)
            {
                //Debug.DrawLine(Vector3.zero, this.transform.position, Color.magenta, 6f);
                return goGrabPlan1(interType.shoot1);
            }
            

            //Debug.Assert(theItemWeWant != null);

            grabEnact1 = theItemWeWant.GetComponent<rangedEnaction>();
        }

        planEXE2 exe1 = grabEnact1.toEXE(null);

        //Debug.Log("grabEnact1.theCooldown:  " + grabEnact1.theCooldown);
        //Debug.Log("grabEnact1.theCooldown.cooldownMax:  " + grabEnact1.theCooldown.cooldownMax);
        //Debug.Log("grabEnact1.theCooldown.cooldownTimer:  " + grabEnact1.theCooldown.cooldownTimer);
        exe1.startConditions.Add(grabEnact1.theCooldown);
        exe1.atLeastOnce();
        //condition thisCondition = new enacted(exe1);
        //exe1.endConditions.Add(thisCondition);

        return exe1;
    }

    private planEXE2 aimTargetPlan2(GameObject target)
    {
        aimTarget testE1 = this.gameObject.GetComponent<aimTarget>();

        planEXE2 exe1 = testE1.toEXE(target);
        exe1.atLeastOnce();

        return exe1;
    }

    private planEXE2 walkToTarget2(GameObject target, float offsetRoom = 0f)
    {
        if (target == null) {
            Debug.Log("target is null, so plan to walk to target is null");
            Debug.Log(target.GetInstanceID());
            return null; }
        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();
        Debug.Assert(theNavAgent != null);

        Vector3 targetPosition = target.transform.position;
        Vector3 between = targetPosition - this.transform.position;
        //GameObject placeholderTarget1 = new GameObject();
        placeholderTarget1.transform.position = targetPosition - between.normalized * offsetRoom;


        planEXE2 theEXE = new vect3EXE2(theNavAgent, placeholderTarget1);
        proximity condition = new proximity(this.gameObject, placeholderTarget1, offsetRoom*1.4f);
        condition.debugPrint =theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        theEXE.debugPrint = theNavAgent.debugPrint;

        return theEXE;
    }











    public planEXE2 randomWanderPlan()
    {
        //ad-hoc hand-coded plan

        //k, they go to random navpoints, great
        //[though...........those navpoints are never DELETED............]
        //[they should have just ONE "nextNav" object, and just MOVE it around ???]

        //  GameObject target = createNavpointInRandomDirection();
        placeholderTarget1.transform.position=this.transform.position;
        moveToRandomNearbyLocation(placeholderTarget1);
        //              enaction anEnaction = walkToTarget(target).theEnaction;
        //              buttonCategories theButtonCategory = anEnaction.gamepadButtonType;
        //              multiPlanAdd(walkToTarget(target), blankMultiPlan());

        return walkToTarget2(placeholderTarget1);
    }


    public GameObject createNavpointInRandomDirection()
    {
        GameObject theNavpoint = new GameObject();
        moveToRandomNearbyLocation(theNavpoint);

        return theNavpoint;
    }

    private static void moveToRandomNearbyLocation(GameObject theObject)
    {
        float initialDistance = 0.1f;
        float randomAdditionalDistance = UnityEngine.Random.Range(20, 443);
        theObject.transform.position += new Vector3(initialDistance + randomAdditionalDistance, 0, 0);
        randomAdditionalDistance = UnityEngine.Random.Range(0, 133);
        theObject.transform.position += new Vector3(0, 0, initialDistance + randomAdditionalDistance);
    }

    public bool threatLineOfSight()
    {
        //super ad-hoc for now

        spatialDataPoint myData = new spatialDataPoint(threatListWithoutSelf(), this.transform.position);



        return myData.threatLineOfSightBool();
    }





    private GameObject pickRandomObjectFromList(List<GameObject> theList)
    {

        if (theList.Count == 0)
        {
            //Debug.Log("there are zero objects on the list of objects entered into ''pickRandomObjectFromListEXCEPT''");
            return null;
        }


        int numberOfTries = 10; //easy ad hoc way to terminate a potentially infinate loop for now lol
        GameObject thisObject;
        thisObject = null;


        while (numberOfTries > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, theList.Count);
            thisObject = theList[randomIndex];

            if (thisObject != null)
            {
                return thisObject;
            }

            numberOfTries--;
        }




        return thisObject;

    }




    public rangedEnaction enactionWithInterTypeXOnObjectsPlayable(GameObject theObject, interType intertypeX)
    {
        foreach (rangedEnaction thisEnaction in listOfIEnactaBoolsOnObject(theObject))
        {

            if (thisEnaction.interInfo.interactionType == intertypeX) { return thisEnaction; }
        }



        return null;
    }

    private List<IEnactaBool> listOfIEnactaBoolsOnObject(GameObject theObject)
    {
        //hmm:
        //List<IEnactaBool> theList = [.. theObject.GetComponents<collisionEnaction>()];


        List<IEnactaBool> theList = new List<IEnactaBool>();

        foreach (collisionEnaction thisEnaction in theObject.GetComponents<collisionEnaction>())
        {
            theList.Add(thisEnaction);
        }


        return theList;
    }




    private List<GameObject> equipperContents()
    {
        playable2 thePlayable = this.gameObject.GetComponent<playable2>();

        List<GameObject> theList = new List<GameObject>();
        foreach (var x in thePlayable.equipperSlotsAndContents.Keys)
        {
            if (thePlayable.equipperSlotsAndContents[x] == null) { continue; }
            theList.Add(thePlayable.equipperSlotsAndContents[x]);
        }

        return theList;
    }

    

    public List<GameObject> getInventory()
    {
        inventory1 inventory = this.gameObject.GetComponent<inventory1>();
        return inventory.inventoryItems;
    }

    GameObject firstObjectOnListWIthInterTypeX(interType interTypeX, List<GameObject> theList)
    {
        //looking at the INTERACTION TYPES of their enactions

        GameObject theItemWeWant = null;

        foreach (GameObject thisObject in theList)
        {

            equippable2 equip = thisObject.GetComponent<equippable2>();
            if (equip == null) { continue; }

            if (equip.containsIntertype(interTypeX))
            {
                theItemWeWant = thisObject;
                break;
            }
        }

        return theItemWeWant;
    }




    public List<GameObject> allNearbyEquippablesWithInterTypeX(interType theInterType)
    {

        List<GameObject> theListOfALL = new find().allObjectsInObjectsZone(this.gameObject);
        List<GameObject> theListOfEquippables = new List<GameObject> ();

        foreach (GameObject thisObject in theListOfALL)
        {

            equippable2 equip = thisObject.GetComponent<equippable2>();
            if (equip == null) { continue; }

            if (equip.containsIntertype(theInterType))
            {
                theListOfEquippables.Add(thisObject);
            }
        }

        return theListOfEquippables;
    }
    //justSenseNearbyEquipables


    public GameObject firstNearbyEquipableWithInteractionType(interType theInterType)
    {

        GameObject theItemWeWant = null;

        List<GameObject> theList = new find().allObjectsInObjectsZone(this.gameObject);

        theItemWeWant = firstObjectOnListWIthInterTypeX(theInterType,theList);

        return theItemWeWant;
    }


    void justDoRandomByINPUT()
    {
        int bools = vGpad.allCurrentBoolEnactables.Count;
        int vectors = vGpad.allCurrentVectorEnactables.Count;

        int whichToPick = Random.Range(0, bools + vectors);


        int dictionaryEntryCount = 0;

        if (whichToPick <= bools-1)
        {
            foreach (var item in vGpad.allCurrentBoolEnactables.Values)
            {
                if(dictionaryEntryCount == whichToPick)
                {
                    if (item == null) { return; }
                    item.enact();
                }
                dictionaryEntryCount++;
            }
        }
        else
        {
            foreach (var item in vGpad.allCurrentVectorEnactables.Values)
            {
                if (dictionaryEntryCount == whichToPick- bools)
                {

                    if (item == null){return; }
                    int x = Random.Range(-8, 8);
                    int y = Random.Range(-8, 8);

                    Debug.Log("item.enact:  " + item);
                    item.enact(new Vector2(x,y));
                }
                dictionaryEntryCount++;
            }
        }

    }

    void justDoRandomByInputORVector()
    {
        int bools = vGpad.allCurrentBoolEnactables.Count;
        int vectors = vGpad.allCurrentVectorEnactables.Count;
        int byTargets = vGpad.allCurrentTARGETbyVectorEnactables.Count;

        int whichToPick = Random.Range(0, bools + vectors + byTargets);


        int indexCount = 0;

        if (whichToPick <= bools - 1)
        {
            foreach (var item in vGpad.allCurrentBoolEnactables.Values)
            {
                if (indexCount == whichToPick)
                {
                    if (item == null) { return; }
                    item.enact();
                }
                indexCount++;
            }
        }
        else if(whichToPick <= bools + vectors - 1)
        {
            foreach (var item in vGpad.allCurrentVectorEnactables.Values)
            {
                if (indexCount == whichToPick - bools)
                {

                    if (item == null) { return; }
                    int x = Random.Range(-8, 8);
                    int y = Random.Range(-8, 8);

                    item.enact(new Vector2(x, y));
                }
                indexCount++;
            }
        }
        else
        {
            foreach (var item in vGpad.allCurrentTARGETbyVectorEnactables)
            {
                if (indexCount == whichToPick - bools - vectors)
                {

                    if (item == null) { return; }


                    CharacterController controller = this.transform.GetComponent<CharacterController>();
                    if (controller != null)
                    {
                        controller.enabled = false;
                    }

                    //eh, should i store this elsewhere?  but where else would be best?  for all other objects?
                    //or just ALSO store it here on AIHub3, because AI will USE it often enough?
                    //but then when/how to update it?  there's the rub.
                    objectIdPair thisPair = tagging2.singleton.idPairGrabify(this.gameObject);

                    int currentZone = tagging2.singleton.zoneOfObject[thisPair];
                    GameObject target = pickRandomObjectFromListEXCEPT(tagging2.singleton.listInObjectFormat(tagging2.singleton.objectsInZone[currentZone]), this.gameObject);
                    //          Debug.DrawLine(this.transform.position, target.transform.position, Color.blue, 2f);
                    item.enact(target.transform.position);
                }
                indexCount++;
            }
        }

    }

    void justDoRandomByBoolORTarget()
    {
        int bools = vGpad.allCurrentBoolEnactables.Count;
        int byTargets = vGpad.allCurrentTARGETbyVectorEnactables.Count;

        int whichToPick = Random.Range(0, bools + byTargets);



        int indexCount = 0;

        if (whichToPick <= bools - 1)
        {
            IEnactaBool enactaBool = null;

            foreach (buttonCategories key in vGpad.allCurrentBoolEnactables.Keys)
            {
                if (indexCount == whichToPick)
                {
                    if (vGpad.allCurrentBoolEnactables[key] == null)
                    {
                        return;
                    }
                    enactaBool = vGpad.allCurrentBoolEnactables[key];
                    break;
                }
                indexCount++;
            }

            enactaBool.enact();
        }
        else
        {
            doRandomByTarget(whichToPick - bools);
        }

    }

    private void doRandomByTarget(int whichToPick)
    {
        //Debug.Log("_______________________________________doRandomByTarget:  " + whichToPick);

        objectIdPair thisId = tagging2.singleton.idPairGrabify(this.gameObject);
        int currentZone = tagging2.singleton.zoneOfObject[thisId];
        GameObject target = pickRandomObjectFromListEXCEPT(
            tagging2.singleton.listInObjectFormat(tagging2.singleton.objectsInZone[currentZone]), 
            this.gameObject);

        vGpad.allCurrentTARGETbyVectorEnactables[whichToPick].enact(target.transform.position);
    }



    public List<GameObject> threatListWithoutSelf()
    {
        List<GameObject> threatListWithoutSelf = new List<GameObject>();
        //Debug.Log("tagging2.singleton.whichZone(this.gameObject):  " + tagging2.singleton.whichZone(this.gameObject));
        List<objectIdPair> theList = new find().allInZone(tagging2.singleton.whichZone(this.gameObject));
        //Debug.Log("theList.Count:  " + theList.Count);
        List<GameObject> thisThreatList = tagging2.singleton.listInObjectFormat(
            new find().allWithOneTag(
                new find().allInZone(tagging2.singleton.whichZone(this.gameObject)), tagging2.tag2.gamepad));//tagging2.singleton.all

        //Debug.Log("thisThreatList.Count:  " + thisThreatList.Count);

        foreach (GameObject threat in thisThreatList)
        {
            //UnityEngine.Vector3 p1 = this.gameObject.transform.position;
            //UnityEngine.Vector3 p2 = threat.gameObject.transform.position;
            //Debug.DrawLine(p1, p2, new Color(0f, 0f, 1f), 1f);
            if (threat != null && threat != this.gameObject)
            {
                //UnityEngine.Vector3 p1 = this.gameObject.transform.position;
                //UnityEngine.Vector3 p2 = threat.gameObject.transform.position;
                //Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 1f);
                threatListWithoutSelf.Add(threat);
            }
        }

        //UnityEngine.Vector3 p3 = this.gameObject.transform.position;
        //UnityEngine.Vector3 p4 = Vector3.zero;
        //Debug.DrawLine(p3, p4, new Color(0f, 0f, 0f), 1f);
        //Debug.Assert(threatListWithoutSelf.Count > 0);
        return threatListWithoutSelf;
    }



    public GameObject pickRandomObjectFromListEXCEPT(List<GameObject> theList, GameObject notTHISObject)
    {
        if (theList.Count == 0)
        {
            Debug.Log("there are zero objects on the list of objects entered into ''pickRandomObjectFromListEXCEPT''");
            return null;
        }


        int numberOfTries = 10; //easy ad hoc way to terminate a potentially infinate loop for now lol
        GameObject thisObject;
        thisObject = null;


        while (numberOfTries > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, theList.Count);
            thisObject = theList[randomIndex];

            if (thisObject != notTHISObject)
            {
                return thisObject;
            }

            numberOfTries--;
        }




        return thisObject;

    }




    void someDebugLogs()
    {

        Debug.Log("CURRENT destination:  " + currentNavMeshAgent.destination);
        Debug.Log("hasPath:  " + currentNavMeshAgent.hasPath);
        Debug.Log("isOnNavMesh:  " + currentNavMeshAgent.isOnNavMesh);
        Debug.Log("isStopped:  " + currentNavMeshAgent.isStopped);
        Debug.Log("pathPending:  " + currentNavMeshAgent.pathPending);
        Debug.Log("remainingDistance:  " + (currentNavMeshAgent.destination - currentNavMeshAgent.nextPosition));

        Debug.Log("destination:  " + currentNavMeshAgent.destination);




        Debug.DrawLine(currentNavMeshAgent.nextPosition, currentNavMeshAgent.destination, Color.green);
        //Debug.DrawLine(this.gameObject.transform.position, theEnactionScript.thisNavMeshAgent.destination, Color.magenta);

    }


    void conditionalPrint(string toPrint)
    {
        if(printThisNPC == false) { return; }
        Debug.Log(toPrint);
    }
}
