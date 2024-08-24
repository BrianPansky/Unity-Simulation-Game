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

    public NavMeshAgent currentNavMeshAgent;
    virtualGamepad vGpad;

    GameObject placeholderTarget1;

    int adhocCooldown = 0;
    public bool printThisNPC = false;
    bool test = true;


    void Awake()
    {
        placeholderTarget1 = new GameObject();
        vGpad = genGen.singleton.ensureVirtualGamePad(this.gameObject);
    }

    public List<IupdateCallable> currentUpdateList { get; set; }
    //public Dictionary<buttonCategories, List<planEXE>> multiPlan = new Dictionary<buttonCategories, List<planEXE>>();

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("=================================      START      ===============================");

        //start plan:  walk to gun, aim at it, click on it, equip it [can do all SEQUENTIAL! or at least equip comes AFTER grabbing gun]
        //plan after that:  dodge while aim-fire at enemies

        //so, first, how about i make 3 lists [1st sequential plan, 2nd two parallel plans of walk and aim-fire][walking isn't a list, just an enaction in a planEXE]

        //then encapsulate

        fullPlan = plan0();
        
        //fullPlan = walkToTarget2(target);

        /*
        Dictionary<buttonCategories, List<planEXE>> multiPlan = new Dictionary<buttonCategories, List<planEXE>>();

        currentNavMeshAgent = genGen.singleton.ensureNavmeshAgent(this.gameObject);


        GameObject target = pickRandomObjectFromList(allNearbyEquippablesWithInterTypeX(interType.shoot1));


        enaction anEnaction = walkToTarget(target).theEnaction;
        buttonCategories theButtonCategory = anEnaction.gamepadButtonType;
        multiPlanAdd(walkToTarget(target), multiPlan);

        //      anEnaction = aimTargetPlan1(target).theEnaction;
        //      theButtonCategory = anEnaction.gamepadButtonType;
        
        //      multiPlanAdd(aimTargetPlan1(target));
        //multiPlanAdd(mouseClickPlan());

        //anEnaction = firePlan2(interType.standardClick).theEnaction;
        //theButtonCategory = anEnaction.gamepadButtonType;
        multiPlanAdd(firePlan2(interType.standardClick, target), multiPlan);


        Dictionary<buttonCategories, List<planEXE>> multiPlan2 = new Dictionary<buttonCategories, List<planEXE>>();
        //multiPlanAdd(clickOnTarget(target));
        multiPlanAdd(equipX(interType.shoot1), multiPlan2);

        planOfMultiPlans.Add(multiPlan);
        planOfMultiPlans.Add(multiPlan2);

        */


        /*
        anEnaction = equipX(interType.shoot1).theEnaction;
        theButtonCategory = anEnaction.gamepadButtonType;
        multiPlanAdd(equipX(interType.shoot1));

        */
    }

    public planEXE2 plan0()
    {



        GameObject target = pickRandomObjectFromList(allNearbyEquippablesWithInterTypeX(interType.shoot1));


        List<planEXE2> firstSequential = new List<planEXE2>();
        List<planEXE2> sequential2 = new List<planEXE2>();

        firstSequential.Add(walkToTarget2(target, 1.9f));
        firstSequential.Add(aimTargetPlan2(target));
        firstSequential.Add(firePlan2nd2(interType.standardClick, target));
        planEXE2 shellOnePointFive = equipX2(interType.shoot1);
        /*
        planEXE2 shellOnePointFive = equipX2(interType.shoot1);
        //firstSequential.Add(equipX2(interType.shoot1));
        firstSequential.Add(equipX2(target));


        GameObject target2 = pickRandomObjectFromList(threatListWithoutSelf());
        sequential2.Add(aimTargetPlan2(target2));
        sequential2.Add(firePlan2nd2(interType.shoot1, target2));

        //now......
        //shell for whole plan, containing [ZEROTH SEQUENCE]:
        //      shell for 1st sequence
        //      shell for 2nd paralllel, containing:
        //          shell for 2nd sequence
        //          shell for 2nd walk


        planEXE2 theDodge = combatDodgeEXE2();
        planEXE2 zerothShell = new seriesEXE();
        planEXE2 firstShell = new seriesEXE();
        planEXE2 secondShell = new parallelEXE();
        planEXE2 secondSequence = new seriesEXE();

        secondSequence.sequentialSet = sequential2;
        secondShell.parallelSet.Add(secondSequence);
        secondShell.parallelSet.Add(theDodge);

        firstShell.sequentialSet = firstSequential;

        zerothShell.sequentialSet.Add(firstShell);
        zerothShell.sequentialSet.Add(shellOnePointFive);
        zerothShell.sequentialSet.Add(secondShell);

        */

        //planEXE2 zerothShell = new seriesEXE();
        planEXE2 firstShell = new seriesEXE(firstSequential);
        //firstShell.sequentialSet = firstSequential;
        //zerothShell.sequentialSet.Add(firstShell);

        List<planEXE2> sequential0 = new List<planEXE2>();
        sequential0.Add(firstShell);
        sequential0.Add(shellOnePointFive);
        planEXE2 zerothShell = new seriesEXE(sequential0);
        //zerothShell.sequentialSet.Add(shellOnePointFive);


        //need a stop condition.......


        return zerothShell;
    }


    public planEXE2 plan2()
    {

        planEXE2 shellOnePointFive = equipX2(interType.shoot1);
        //firstSequential.Add(equipX2(interType.shoot1));
        //firstSequential.Add(equipX2(target));

        return shellOnePointFive;
    }
    
    public planEXE2 plan3()
    {

        GameObject target2 = pickRandomObjectFromList(threatListWithoutSelf());
        //GameObject target2 = pickRandomObjectFromList(allNearbyEquippablesWithInterTypeX(interType.shoot1));

        if(target2 == null) { return new singleEXE(); }

        planEXE2 aim = aimTargetPlan2(target2);
        planEXE2 fire = firePlan2nd2(interType.shoot1, target2);


        List<planEXE2> sequential2 = new List<planEXE2>();
        sequential2.Add(aim);
        sequential2.Add(fire);
        planEXE2 secondShell = new seriesEXE(sequential2);


        List<planEXE2> parallelList = new List<planEXE2>();








        parallelList.Add(combatDodgeEXE2());









        parallelList.Add(secondShell);
        planEXE2 parallel = new parallelEXE(parallelList);

        //now......
        //shell for whole plan, containing [ZEROTH SEQUENCE]:
        //      shell for 1st sequence
        //      shell for 2nd paralllel, containing:
        //          shell for 2nd sequence
        //          shell for 2nd walk

        /*
        planEXE2 theDodge = combatDodgeEXE2();
        //planEXE2 zerothShell = new seriesEXE();
        planEXE2 firstShell = new seriesEXE();
        planEXE2 secondShell = new parallelEXE();
        planEXE2 secondSequence = new seriesEXE();

        //secondSequence.sequentialSet = sequential2;
        secondSequence.sequentialSet.Add(aim);
        secondSequence.sequentialSet.Add(fire);
        secondShell.parallelSet.Add(secondSequence);
        secondShell.parallelSet.Add(theDodge);

        */

        /*
        firstShell.sequentialSet = firstSequential;

        zerothShell.sequentialSet.Add(firstShell);
        zerothShell.sequentialSet.Add(shellOnePointFive);
        zerothShell.sequentialSet.Add(secondShell);
        */


        //Debug.Log("need to fix this");

        return parallel;
        //return null;
    }

    private planEXE2 combatDodgeEXE2()
    {
        //ad-hoc hand-coded plan

        //calculate a position to "dodge" towards
        //place an empty "nav point" object there
        //then just use walkToTarget(target)

        //look at simpleDodge
        List<GameObject> threatList = threatListWithoutSelf();

        //if (threatList.Count < 1){return null;}

        //from my older code:
        spatialDataPoint myData = new spatialDataPoint();
        myData.initializeDataPoint(threatListWithoutSelf(), this.transform.position);

        Vector3 adHocThreatAvoidanceVector = myData.applePattern();

        //GameObject target = new GameObject();
        placeholderTarget1.transform.position = this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized * 4.7f;
        planEXE2 exe1 = walkToTarget2(placeholderTarget1, 1.9f);
        //      condition thisCondition = new proximity(placeholderTarget1,this.gameObject, 0.6f);
        //      exe1.endConditions.Add(thisCondition);


        return exe1;
    }

    private planEXE2 equipX2(interType interTypeX)
    {
        GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, getInventory());

        //IEnactaBool testE1 = new takeFromAndPutBackIntoInventory(this.gameObject);

        IEnactaBool testE1 = this.gameObject.GetComponent<takeFromAndPutBackIntoInventory>();

        //planEXE2 exe1 = new singleEXE(testE1, theItemWeWant);
        planEXE2 exe1 = testE1.toEXE(theItemWeWant);
        condition thisCondition = new enacted(exe1);
        exe1.endConditions.Add(thisCondition);
        return exe1;
    }
    
    private planEXE2 equipX2(GameObject theItemWeWant)
    {
        //GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, getInventory());

        //IEnactaBool testE1 = new takeFromAndPutBackIntoInventory(this.gameObject);

        IEnactaBool testE1 = this.gameObject.GetComponent<takeFromAndPutBackIntoInventory>();

        //planEXE2 exe1 = new singleEXE(testE1, theItemWeWant);
        planEXE2 exe1 = testE1.toEXE(theItemWeWant);
        condition thisCondition = new enacted(exe1);
        exe1.startConditions.Add(thisCondition);
        return exe1;
    }

    private planEXE2 firePlan2nd2(interType interTypeX, GameObject target)
    {

        //either playable will already have the type, or it might be in equipper slots
        rangedEnaction grabEnact1;
        grabEnact1 = enactionWithInterTypeXOnObjectsPlayable(this.gameObject, interTypeX);

        //Debug.Log("grabEnact1:  " + grabEnact1);

        if (grabEnact1 == null)
        {
            //Debug.Log("equipperContents().Count:  " + equipperContents().Count);
            GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, equipperContents());

            //oh no it can be null
            if(theItemWeWant == null)
            {
                //Debug.DrawLine(Vector3.zero, this.transform.position, Color.magenta, 6f);
                //return combatDodgeEXE2();  //easy way to fail gracefully here for now
                return plan0();
            }
            

            //Debug.Assert(theItemWeWant != null);

            grabEnact1 = theItemWeWant.GetComponent<rangedEnaction>();
        }



        //boolEXE exe1 = new boolEXE(grabEnact1, null);
        //planEXE2 exe1 = new singleEXE(grabEnact1, null);
        planEXE2 exe1 = grabEnact1.toEXE(null);
        Debug.Assert(exe1 != null);
        Debug.Assert(exe1.theEnaction != null);
        //          enactTimes2(exe1, 3);
        //          if (cooldownMax > 0) { addCooldownConditionToSTART(exe1, cooldownMax); }

        Debug.Assert(exe1 != null);
        Debug.Assert(exe1.theEnaction != null);

        enaction anEnact = exe1.theEnaction;

        Debug.Assert(anEnact != null);
        //Debug.Log("exe1:  " + exe1);

        /*
        int cooldownMax = grabEnact1.firingCooldownMax;
        Debug.Log("cooldownMax:  " + cooldownMax);
        if (cooldownMax > 0)
        {
            cooldown condition2 = new cooldown(cooldownMax);
            exe1.startConditions.Add(condition2);
        }//{ addCooldownConditionToSTART(exe1, cooldownMax); }
        */

        Debug.Assert(grabEnact1.theCooldown !=null);

        //Debug.Log("grabEnact1.theCooldown:  " + grabEnact1.theCooldown);
        //Debug.Log("grabEnact1.theCooldown.cooldownMax:  " + grabEnact1.theCooldown.cooldownMax);
        //Debug.Log("grabEnact1.theCooldown.cooldownTimer:  " + grabEnact1.theCooldown.cooldownTimer);
        exe1.startConditions.Add(grabEnact1.theCooldown);

        //foreach (condition x in exe1.startConditions)
        {

            //Debug.Log("condition x in exe1.startConditions:  " + x);
        }




        //proximity condition = new proximity(this.gameObject, target, 3.8f);

        //?????????????????????
        //          proximity condition = new proximity(this.gameObject, target, grabEnact1.range * 0.9f);
        //proximity condition = new proximity(this.gameObject, target, 2f);

        //Debug.Log("grabEnact1.range * 0.7f:  " + grabEnact1.range * 0.7f);
        //should also have a line of sight condition here, and allowance for innacuracy causing friendly fire
        //          exe1.startConditions.Add(condition);

        condition thisCondition = new enacted(exe1);
        exe1.endConditions.Add(thisCondition);

        //exe1.microPlan.Add(aimTargetPlan1(target));

        //Debug.Log("exe1.theEnaction:  " + exe1.theEnaction);
        return exe1;
    }

    private planEXE2 aimTargetPlan2(GameObject target)
    {
        playable2 thePlayable = this.gameObject.GetComponent<playable2>();

        //some of my classes inherit from monobehavior, so you can't use "new" constructor to return the object.
        //have to think of monobehavior classes as COMPONENTS attached to an OBJECT [because that's what they are]
        //so......how to get the correct one?

        aimTarget testE1 = this.gameObject.GetComponent<aimTarget>();

        Debug.Assert(testE1 != null);


        //now, put it in a planEXE, with the target [and a specified proximity condition for ending the enaction?]






        //vectEXE exe1 = new vectEXE(testE1, target);
        //singleEXE exe1 = new vect3EXE2(testE1, target);
        Debug.Assert(target != null);
        planEXE2 exe1 = testE1.toEXE(target);
        condition thisCondition = new enacted(exe1);
        exe1.endConditions.Add(thisCondition);

        Debug.Assert(exe1.theEnaction != null);
        return exe1;
    }

    private planEXE2 walkToTarget2(GameObject target, float offsetRoom = 0f)
    {
        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();
        Debug.Assert(theNavAgent != null);

        if (theNavAgent == null) { return null; }

        //now, put it in a planEXE, with the target [and a specified proximity condition for ending the enaction?]

        Vector3 targetPosition = target.transform.position;
        Vector3 between = targetPosition - this.transform.position;

        placeholderTarget1.transform.position = targetPosition - between.normalized * offsetRoom;


        //planEXE theEXE = new vectEXE(theNavAgent, target);
        Debug.Assert(target != null);
        planEXE2 theEXE = new vect3EXE2(theNavAgent, placeholderTarget1);
        Debug.Assert(theEXE.theEnaction != null);

        proximity condition = new proximity(this.gameObject, placeholderTarget1, offsetRoom*1.4f);
        theEXE.endConditions.Add(condition);
        //is that it?  is it already set up to work?  [aside from the proximity condition...]
        return theEXE;
    }












    // Update is called once per frame
    void Update()
    {
        adhocCooldown++;
    }



    public void callableUpdate()
    {
       //Debug.Log("=======================callableUpdate()............");

        //Debug.Log("333      multiPlan.Keys.Count:  " + multiPlan.Keys.Count);


        if (fullPlan == null || fullPlan.endConditionsMet() ) { //Debug.Log("fullPlan == null || fullPlan.endConditionsMet()");
                                                                fullPlan = plan3(); return; }

        //Debug.Log("we have a plan, fullPlan.execute()");
        fullPlan.execute();

        /*
        if(planOfMultiPlans.Count < 1) { refillPlanOfMultiPlans();  return; }
        if (noPlansInMultiplan(planOfMultiPlans[0])) { planOfMultiPlans.RemoveAt(0); return; }
        
        doCurrentMultiPlanSteps(planOfMultiPlans[0]);
        */

        /*
        if (adhocCooldown < 44) { return; }

        adhocCooldown = 0;

        repeatAdHocPlanWhenItsDone();

        */
    }

    private void refillPlanOfMultiPlans()
    {
        throw new System.NotImplementedException();

        //one dodge plan beside one aim and shoot plan


        Dictionary<buttonCategories, List<planEXE>> multiPlan = new Dictionary<buttonCategories, List<planEXE>>();

        //combatDodgePlan();

        /*
        multiPlanAdd(returnCombatDodgePlan(), multiPlan);
        GameObject target = pickRandomObjectFromList(threatListWithoutSelf());
        //Debug.Log("pickRandomObjectFromList(threatListWithoutSelf()):  " + target);
        //enaction anEnaction = aimTargetPlan1(target).theEnaction;
        //buttonCategories theButtonCategory = anEnaction.gamepadButtonType;

        List<planEXE> sidePlan = new List<planEXE>();
        sidePlan.Add(aimTargetPlan1(target));
        sidePlan.Add(firePlan2(interType.shoot1, target));
        multiPlanAddLIST(, multiPlan);

        multiPlanAdd(firePlan2(interType.shoot1, target), multiPlan);



        planOfMultiPlans.Add(multiPlan);

        */

        //  planEXE anExe = firePlan2(interType.shoot1, 30);
        //  Debug.Log("anExe:  " + anExe);
        //  enaction anEnact = anExe.theEnaction;
        //  anEnaction = anEnact;
        //  theButtonCategory = anEnaction.gamepadButtonType;











        currentNavMeshAgent = genGen.singleton.ensureNavmeshAgent(this.gameObject);


        GameObject target = pickRandomObjectFromList(allNearbyEquippablesWithInterTypeX(interType.shoot1));


        enaction anEnaction = walkToTarget(target).theEnaction;
        buttonCategories theButtonCategory = anEnaction.gamepadButtonType;
        multiPlanAdd(walkToTarget(target), multiPlan);

        //      anEnaction = aimTargetPlan1(target).theEnaction;
        //      theButtonCategory = anEnaction.gamepadButtonType;

        //      multiPlanAdd(aimTargetPlan1(target));
        //multiPlanAdd(mouseClickPlan());

        //anEnaction = firePlan2(interType.standardClick).theEnaction;
        //theButtonCategory = anEnaction.gamepadButtonType;
        multiPlanAdd(firePlan2(interType.standardClick, target), multiPlan);


        Dictionary<buttonCategories, List<planEXE>> multiPlan2 = new Dictionary<buttonCategories, List<planEXE>>();
        //multiPlanAdd(clickOnTarget(target));
        multiPlanAdd(equipX(interType.shoot1), multiPlan2);

        //      planOfMultiPlans.Add(multiPlan);
        //      planOfMultiPlans.Add(multiPlan2);

        /*
        anEnaction = equipX(interType.shoot1).theEnaction;
        theButtonCategory = anEnaction.gamepadButtonType;
        multiPlanAdd(equipX(interType.shoot1));

        */
    }

    void repeatAdHocPlanWhenItsDone()
    {

        /*
        foreach (var key in multiPlan.Keys)
        {
            List<planEXE> plan = multiPlan[key];
            if (plan.Count > 0) { return; }

            Debug.Log("plan.Count > 0 is FALSE");
            //makeAdHocPlanToDo();
            //randomWanderPlan();

            if (threatLineOfSight())
            {
                combatDodgePlan();
                combatDodgePlan();
                combatDodgePlan();
                GameObject target = pickRandomObjectFromList(threatListWithoutSelf());
                //Debug.Log("pickRandomObjectFromList(threatListWithoutSelf()):  " + target);
                enaction anEnaction = aimTargetPlan1(target).theEnaction;
                buttonCategories theButtonCategory = anEnaction.gamepadButtonType;
                multiPlanAdd(aimTargetPlan1(target));

                
                //  planEXE anExe = firePlan2(interType.shoot1, 30);
                //  Debug.Log("anExe:  " + anExe);
                //  enaction anEnact = anExe.theEnaction;
                //  anEnaction = anEnact;
                //  theButtonCategory = anEnaction.gamepadButtonType;
                

                multiPlanAdd(clickOnTarget(target));
            }
            else
            {
                randomWanderPlan();
            }
        }

        */
    }

    void doCurrentMultiPlanSteps(Dictionary<buttonCategories, List<planEXE>> multiPlan)
    {
        
        //Debug.Log("doCurrentMultiPlanSteps");
        foreach (var key in multiPlan.Keys)
        {
            //Debug.Log("foreach (var key in multiPlan.Keys)");
            List<planEXE> plan = multiPlan[key];
            doCurrentPlanStep(plan);
        }

        
    }

    void doCurrentPlanStep(List<planEXE> plan)
    {
        if (plan.Count < 1) { //Debug.Log("plan.Count < 1"); 
            return; }
        if (plan[0] == null)
        {
            Debug.Log("how to handle this");
            plan.RemoveAt(0);
            return;
        }

        if (plan[0].areSTARTconditionsFulfilled())
        {
            //Debug.Log("plan[0].areSTARTconditionsFulfilled() for:  " + plan[0]);
            plan[0].executePlan();
        }

        if (plan[0].areENDconditionsFulfilled())
        {
            Debug.Log("ENDconditionsFulfilled() for:  " + plan[0].theEnaction);
            plan.RemoveAt(0);
        }

    }






    void randomWanderPlan()
    {
        //ad-hoc hand-coded plan

        //k, they go to random navpoints, great
        //[though...........those navpoints are never DELETED............]
        //[they should have just ONE "nextNav" object, and just MOVE it around ???]

        GameObject target = createNavpointInRandomDirection();
        enaction anEnaction = walkToTarget(target).theEnaction;
        buttonCategories theButtonCategory = anEnaction.gamepadButtonType;
        multiPlanAdd(walkToTarget(target), blankMultiPlan());
    }

    void combatDodgePlan()
    {
        //ad-hoc hand-coded plan

        enaction anEnaction = combatDodgeEXE().theEnaction;
        buttonCategories theButtonCategory = anEnaction.gamepadButtonType;
        multiPlanAdd(combatDodgeEXE(), blankMultiPlan());
    }

    planEXE combatDodgeEXE()
    {
        //ad-hoc hand-coded plan

        //calculate a position to "dodge" towards
        //place an empty "nav point" object there
        //then just use walkToTarget(target)

        //look at simpleDodge
        List<GameObject> threatList = threatListWithoutSelf();

        //if (threatList.Count < 1){return null;}

        //from my older code:
        spatialDataPoint myData = new spatialDataPoint();
        myData.initializeDataPoint(threatListWithoutSelf(), this.transform.position);

        Vector3 adHocThreatAvoidanceVector = myData.applePattern();

        GameObject target = new GameObject();
        target.transform.position = this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized * 3.7f;



        return walkToTarget(target);
    }

    public GameObject createNavpointInRandomDirection()
    {
        GameObject theNavpoint = new GameObject();

        float initialDistance = 2f;
        float randomAdditionalDistance = UnityEngine.Random.Range(0, 33);
        theNavpoint.transform.position += new Vector3(initialDistance+ randomAdditionalDistance, 0,0);
        randomAdditionalDistance = UnityEngine.Random.Range(0, 33);
        theNavpoint.transform.position += new Vector3(0,0, initialDistance + randomAdditionalDistance);

        return theNavpoint;
    }


    public bool threatLineOfSight()
    {
        //super ad-hoc for now

        spatialDataPoint myData = new spatialDataPoint();
        myData.initializeDataPoint(threatListWithoutSelf(), this.transform.position);



        return myData.threatLineOfSightBool();
    }



    void makeAdHocPlanToDo()
    {
        int count = 0;
        while (count < 15)
        {
            GameObject target = pickRandomObjectFromList(threatListWithoutSelf());
            //Debug.Log("pickRandomObjectFromList(threatListWithoutSelf()):  " + target);
            enaction anEnaction = aimTargetPlan1(target).theEnaction;
            buttonCategories theButtonCategory = anEnaction.gamepadButtonType;
            multiPlanAdd(aimTargetPlan1(target), blankMultiPlan());

            anEnaction = firePlan2(interType.shoot1, target, 30).theEnaction;
            theButtonCategory = anEnaction.gamepadButtonType;
            multiPlanAdd(firePlan2(interType.shoot1, target, 30), blankMultiPlan());
            count++;
        }

        //enactALLtimes(plan, 9);

        /*
        
        multiPlanAdd(clickOnTarget(target));  //not gonna work, i haven't fixed picking up yet....
        multiPlanAdd(equipX(interType.shoot1));
        //multiPlanAdd(aimPlan1());

        target = pickRandomObjectFromList(threatListWithoutSelf());
        multiPlanAdd(aimTargetPlan1(target));
        multiPlanAdd(firePlan2(interType.shoot1));
        */


    }

    


    private planEXE equipX(interType interTypeX)
    {
        GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, getInventory());

        //IEnactaBool testE1 = new takeFromAndPutBackIntoInventory(this.gameObject);

        IEnactaBool testE1 = this.gameObject.GetComponent<takeFromAndPutBackIntoInventory>();

        boolEXE exe1 = new boolEXE(testE1, theItemWeWant);
        return exe1;
    }



    planEXE walkToTarget(GameObject target)
    {
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();
        Debug.Assert(theNavAgent != null);

        if (theNavAgent == null) { return null; }

        //now, put it in a planEXE, with the target [and a specified proximity condition for ending the enaction?]

        planEXE theEXE = new vectEXE(theNavAgent, target);
        Debug.Assert(theEXE.theEnaction != null);
        proximity condition = new proximity(this.gameObject, target, 3.8f);
        theEXE.endConditions.Add(condition);
        //is that it?  is it already set up to work?  [aside from the proximity condition...]
        return theEXE;
    }

    planEXE clickOnTarget(GameObject target)
    {
        throw new System.NotImplementedException();
    }

    planEXE aimTargetPlan1(GameObject target)
    {
        playable2 thePlayable = this.gameObject.GetComponent<playable2>();


        /*
        float theLookSpeed = thePlayable.lookSpeed;
        Transform theTransform = thePlayable.transform;
        Transform enactionPointTransform = thePlayable.enactionPoint1.transform;
        buttonCategories theButtonCategory = buttonCategories.vector2;
        //vecRotation theVecRotation = new vecRotation(thePlayable.lookSpeed, thePlayable.transform, thePlayable.enactionPoint1.transform, buttonCategories.vector2)
        vecRotation theVecRotation = new vecRotation(theLookSpeed, theTransform, enactionPointTransform, theButtonCategory) as vecRotation;
        */

        //my classes inherit from monobehavior, so you can't use "new" constructor to return the object.
        //have to think of monobehavior classes as COMPONENTS attached to an OBJECT [because that's what they are]
        //so......how to get the correct one?

        /*
        vecRotation theVecRotation = this.gameObject.GetComponent<vecRotation>();

        Debug.Assert(theVecRotation != null);
        IEnactByTargetVector testE1 = new aimTarget(theVecRotation);
        */

        aimTarget testE1 = this.gameObject.GetComponent<aimTarget>();

        Debug.Assert(testE1 != null);


        //now, put it in a planEXE, with the target [and a specified proximity condition for ending the enaction?]






        vectEXE exe1 = new vectEXE(testE1, target);

        Debug.Assert(exe1.theEnaction != null);
        return exe1;
    }



    planEXE aimPlan1()
    {
        //pick random enemy target
        //aim

        GameObject target = pickRandomObjectFromList(threatListWithoutSelf());

        //playable2 thePlayable = this.gameObject.GetComponent<playable2>();
        //IEnactByTargetVector testE1 = new aimTarget(new vecRotation(thePlayable.lookSpeed, thePlayable.transform, thePlayable.enactionPoint1.transform, buttonCategories.vector2));

        IEnactByTargetVector testE1 = this.gameObject.GetComponent<aimTarget>();

        vectEXE exe1 = new vectEXE(testE1, target);
        return exe1;
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

    planEXE firePlan2(interType interTypeX, GameObject target, int cooldownMax = 0)
    {
        //either playable will already have the type, or it might be in equipper slots
        rangedEnaction grabEnact1;
        grabEnact1 = enactionWithInterTypeXOnObjectsPlayable(this.gameObject, interTypeX);

        //Debug.Log("grabEnact1:  " + grabEnact1);

        if (grabEnact1 == null) 
        {
            Debug.Log("equipperContents().Count:  " + equipperContents().Count);
            GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, equipperContents());

            //oh no it can be null
            if (theItemWeWant == null)
            {
                Debug.Log("theItemWeWant == null"); 
                return null; }//???

            grabEnact1 = theItemWeWant.GetComponent<rangedEnaction>();
        }



        boolEXE exe1 = new boolEXE(grabEnact1, null);
        Debug.Assert(exe1 != null);
        Debug.Assert(exe1.theEnaction != null);
        enactTimes(exe1, 3);
        if (cooldownMax > 0) { addCooldownConditionToSTART(exe1, cooldownMax); }

        Debug.Assert(exe1 != null);
        Debug.Assert(exe1.theEnaction != null);

        enaction anEnact = exe1.theEnaction;

        Debug.Assert(anEnact != null);
        //Debug.Log("exe1:  " + exe1);


        //proximity condition = new proximity(this.gameObject, target, 3.8f);
        proximity condition = new proximity(this.gameObject, target, grabEnact1.range*0.1f);
        //should also have a line of sight condition here, and allowance for innacuracy causing friendly fire
        exe1.endConditions.Add(condition);

        exe1.microPlan.Add(aimTargetPlan1(target));

        //Debug.Log("exe1.theEnaction:  " + exe1.theEnaction);
        return exe1;
    }


    public void addCooldownConditionToSTART(planEXE theEXE, int cooldownMax)
    {
        cooldown condition = new cooldown(cooldownMax);
        theEXE.startConditions.Add(condition);
    }

    public void enactALLtimes(List<planEXE> theEXEList, int times = 1)
    {
        foreach (planEXE theEXE in theEXEList) { enactTimes(theEXE, times); }
    }


    public void enactTimes(planEXE theEXE, int times = 1)
    {
        if(theEXE == null) { Debug.Log("theEXE == null"); return; }
        Debug.Log("i'm not using planEXE anymore!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //          condition theCondition = new enacted(theEXE, times);
        //Debug.Log("theEXE.endConditions:  " + theEXE.endConditions);
        theEXE.endConditions = new List<condition>();
        //          theEXE.endConditions.Add(theCondition);
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
        /*
        foreach (GameObject thisObject in theList)
        {
            if (conditionCreator.objectHasInteractionType(thisObject, theInterType))
            {
                theItemWeWant = thisObject;
                break;
            }
        }
        */

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

    void simpleDodge()
    {
        //Debug.Log("try simpleDodge()");
        //Know positions of nearby threats
        //      For now, just log the position of NPCs[and player], which will include any occupied tanks.
        //          Soooo, tag everything with a virtualGamepad?
        //Calculate simple movement vector
        //Place waypoint in that direction, go with nav agent ?

        List<GameObject> threatList = threatListWithoutSelf();

        //need to combine/balance dodging with other navigation goals.
        //for now, at least allow normal navigation if there are zero threats
        //[and save some calculation that is pointless if there are no threats]: 
        if (threatList.Count < 1) {
            return; }

        //from my older code:
        spatialDataPoint myData = new spatialDataPoint();
        myData.initializeDataPoint(threatListWithoutSelf(), this.transform.position);

        Vector3 adHocThreatAvoidanceVector = myData.applePattern();

        if(currentNavMeshAgent.enabled == true)
        {
            currentNavMeshAgent.SetDestination(this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized * 12f);
        }


        //Debug.Log("-----------------------------------end of dodge............");
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
                Vector3 threatPosition = threat.gameObject.transform.position;
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

}
