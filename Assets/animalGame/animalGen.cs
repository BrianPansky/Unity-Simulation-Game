using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR;
using static enactionCreator;
using static interactionCreator;

public class animalGen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 location = Vector3.zero;

        location = new Vector3(6, 0, -15);
        makeEmptyZones(1,520);

        genGen.singleton.returnShotgun1(new Vector3(-36, 5, 13));
        //returnArrowForward(location,3);
        returnBasicAnimal1(location, stuffType.fruit, 1);
        //genGen.singleton.returnNPC5(location);

        returnBasicGrabbable(stuffType.fruit, new Vector3(-5, 2, 8));
        returnBasicGrabbable(stuffType.meat1, new Vector3(-17, 2, 23));
        returnBasicGrabbable(stuffType.fruit, new Vector3(-15, 2, -4));

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public GameObject returnBasicAnimal1(Vector3 where, stuffType stuffTypeX, float scale = 1f)
    {
        GameObject newObj = returnArrowForward(where, scale);

        genGen.singleton.ensureVirtualGamePad(newObj);

        addAnimalBody1ToObject(newObj);
        genGen.singleton.addArrowForward(newObj, 1f, 0f, 1.2f);
        //wander1.addWander1(newObj);
        //grabGun1.addGrabGun1(newObj, interType.shoot1);

        newObj.AddComponent<animalUpdate>();
        //grabStuffStuff.addGrabStuffStuff(newObj, stuffTypeX);

        return newObj;
    }

    public GameObject returnBasicGrabbable(stuffType stuffX, Vector3 where, float scale = 1f)
    {

        GameObject newObj = genGen.singleton.returnPineTree1(where);

        newObj.transform.localScale = scale * newObj.transform.localScale;

        tagging2.singleton.addTag(newObj, tagging2.tag2.interactable);
        tagging2.singleton.addTag(newObj, tagging2.tag2.equippable2);
        tagging2.singleton.addTag(newObj, tagging2.tag2.zoneable);

        stuffStuff.addStuffStuff(newObj, stuffX);

        return newObj;
    }


    public GameObject returnArrowForward(Vector3 where, float scale =1f)
    {
        GameObject newObj = Instantiate(repository2.singleton.placeHolderCubePrefab, where, Quaternion.identity);
        //      newObj.transform.localScale = new Vector3(128, 1, 8);
        newObj.transform.localScale = scale * newObj.transform.localScale;

        return newObj;
    }


    public void addAnimalBody1ToObject(GameObject newObj)
    {
        playable2 thePlayable = newObj.AddComponent<playable2>();


        //thePlayable.dictOfInteractions = new Dictionary<enactionCreator.interType, List<Ieffect>>();
        //thePlayable.dictOfIvariables = new Dictionary<interactionCreator.numericalVariable, float>();

        thePlayable.dictOfIvariables[numericalVariable.health] = 2;
        thePlayable.equipperSlotsAndContents[interactionCreator.simpleSlot.hands] = null;
        thePlayable.initializeEnactionPoint1();
        //addArrowForward(thePlayable.enactionPoint1);
        //genGen.singleton.addCube(thePlayable.enactionPoint1, 0.1f);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        //addArrowForward(newObj, 5f, 0f, 1.2f);
        genGen.singleton.makeEnactionsBody4(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);


        inventory1 theirInventory = newObj.AddComponent<inventory1>();
    }





    void makeEmptyZones(int howManyZones, int theZSpacing)
    {

        List<Vector3> zonePositions = patternScript2.singleton.makeLinePattern1(howManyZones, theZSpacing);


        foreach (Vector3 thisPoint in zonePositions)
        {
            //Instantiate(prefab, thisPoint, default);
            GameObject newObj = Instantiate(repository2.singleton.mapZone2, thisPoint, Quaternion.identity);
            newObj.transform.localScale = new Vector3(400f, 10f, 1f * theZSpacing);

        }
    }
}

public class animalUpdate:MonoBehaviour
{

    depletablePlan plan;
    permaPlan2 perma1;

    void Start()
    {
        setupTest2();
    }

    void Update()
    {
        plan.doTheDepletablePlan();
    }

    private singleEXE makeNavAgentPlanEXE(Vector3 staticTargetPosition, float offsetRoom = 0f)
    {

        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();


        singleEXE theEXE = new vect3EXE2(theNavAgent, staticTargetPosition);//placeholderTarget1);
        //theEXE.debugPrint = printThisNPC;


        proximity condition = new proximity(this.gameObject, staticTargetPosition, 2f);// offsetRoom * 1.4f);
        //condition.debugPrint = theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        return theEXE;
    }
    
    void setupTest2()
    {
        //then, to test it, call "plan.doTheDepletablePlan();" in the update
        singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));
        singleEXE step2 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));
        singleEXE step3 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));

        perma1 = new permaPlan2(step1, step2, step3);

        //plan = new depletablePlan(step1, step2);
        plan = perma1.convertToDepletable();



        //now
        //      how to make random wander take...ONE step?  but generate new location each time?
        //      generate location OUTSIDE the simple class, and input it........?
        //          and, what, that input........always takes vector3?  or sometimes GameObject?  so use "target calculator" as input?
        //              i' doing that in animalFSM?  the switch conditions ALSO provide stuff like threats/target/navpoint?


    }
}





public class simpleExactRepeatOfPerma
{
    permaPlan thePerma;
    depletablePlan theDepletablePlan;


    void doThisThing()
    {
        refillIfNeeded();

        theDepletablePlan.doTheDepletablePlan();

        refillIfNeeded();
    }

    private void refillIfNeeded()
    {
        if (theDepletablePlan.endConditionsMet())
        {
            theDepletablePlan = thePerma.convertToDepletable();
        }
    }
}




public class reproduce : IEnactaBool
{
    public override void enact(inputData theInput)
    {
        throw new System.NotImplementedException();
    }
}

public class meleeAttack : IEnactaBool
{
    public override void enact(inputData theInput)
    {
        throw new System.NotImplementedException();
    }
}

public class sleep : IEnactaBool
{
    public override void enact(inputData theInput)
    {
        throw new System.NotImplementedException();
    }
}



public abstract class behavior:MonoBehaviour
{
    planEXE2 thePlan;


    void Update()
    {
        doBehavior();
    }

    void doBehavior()
    {
        if (thePlan == null || thePlan.error())
        { 
            thePlan = replacementPlan(); 
        }

        //thePlan.debugPrint = printThisNPC;
        thePlan.execute();
    }

    public abstract planEXE2 replacementPlan();
}

public class wander1: behavior
{


    public static wander1 addWander1(GameObject objectToAddItTo)
    {

        wander1 newWanderBehavior = objectToAddItTo.AddComponent<wander1>();

        //newWanderBehavior.gamepadButtonType = theButtonType;
        //newWanderBehavior.theEnactions.Add(enaction1);

        return newWanderBehavior;
    }


    public override planEXE2 replacementPlan()
    {
        return FIXEDgoToTargetForSTATIONARYtargets(randomNearbyVector(this.transform.position));
    }



    public planEXE2 FIXEDgoToTargetForSTATIONARYtargets(Vector3 stationaryTargetPosition, float offset = 1.8f)
    {
        planEXE2 firstShell = new seriesEXE();
        //firstShell.debugPrint = printThisNPC;
        firstShell.Add(makeNavAgentPlanEXE(stationaryTargetPosition, offset));
        firstShell.untilListFinished();

        return firstShell;
    }

    private planEXE2 makeNavAgentPlanEXE(Vector3 staticTargetPosition, float offsetRoom = 0f)
    {

        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();


        planEXE2 theEXE = new vect3EXE2(theNavAgent, staticTargetPosition);//placeholderTarget1);
        //theEXE.debugPrint = printThisNPC;


        proximity condition = new proximity(this.gameObject, staticTargetPosition, offsetRoom * 1.4f);
        condition.debugPrint = theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        return theEXE;
    }
    public Vector3 randomNearbyVector(Vector3 positionToBeNear)
    {
        Vector3 vectorToReturn = positionToBeNear;
        float initialDistance = 0f;
        float randomAdditionalDistance = UnityEngine.Random.Range(-20, 20);
        vectorToReturn += new Vector3(initialDistance + randomAdditionalDistance, 0, 0);
        randomAdditionalDistance = UnityEngine.Random.Range(-20, 20);
        vectorToReturn += new Vector3(0, 0, initialDistance + randomAdditionalDistance);

        return vectorToReturn;
    }

}



public class grabGun1 : behavior
{
    interType interTypeToGrab;

    public static grabGun1 addGrabGun1(GameObject objectToAddItTo, interType interTypeToGrabIn)
    {

        grabGun1 newGrabGun1 = objectToAddItTo.AddComponent<grabGun1>();

        newGrabGun1.interTypeToGrab = interTypeToGrabIn;
        //newWanderBehavior.gamepadButtonType = theButtonType;
        //newWanderBehavior.theEnactions.Add(enaction1);

        return newGrabGun1;
    }


    public override planEXE2 replacementPlan()
    {

        planEXE2 zerothShell = new seriesEXE(goGrabPlan2(interTypeToGrab));
        return zerothShell;
    }

    private planEXE2 goGrabPlan2(interType interTypeX)
    {
        //ad-hoc hand-written plan
        GameObject target = repository2.singleton.pickRandomObjectFromList(allNearbyEquippablesWithInterTypeX(interTypeX));

        if (target == null) { return null; }

        planEXE2 firstShell = new seriesEXE();
        firstShell.Add(FIXEDgoToTargetForMOBILEtargets(target, 1.8f));
        firstShell.Add(aimTargetPlan2(target));




        hitscanEnactor theHitscanEnactor = grabHitscanEnaction(this.gameObject, interType.standardClick); //hitscanClickPlan(interType.standardClick, target);

        Debug.Assert(theHitscanEnactor != null);
        planEXE2 hitscanEXE = theHitscanEnactor.standardEXEconversion();
        firstShell.Add(hitscanEXE);
        firstShell.untilListFinished();


        return firstShell;
    }



    public List<GameObject> allNearbyEquippablesWithInterTypeX(interType theInterType)
    {

        List<GameObject> theListOfALL = new find().allObjectsInObjectsZone(this.gameObject);  //lol forgot, this is ONE way to grab functions
        List<GameObject> theListOfEquippables = new List<GameObject>();

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


    private planEXE2 aimTargetPlan2(GameObject target)
    {
        aimTarget testE1 = this.gameObject.GetComponent<aimTarget>();

        planEXE2 exe1 = testE1.toEXE(target);
        exe1.atLeastOnce();

        return exe1;
    }



    private hitscanEnactor grabHitscanEnaction(GameObject theObject, interType interTypeX)
    {

        foreach (hitscanEnactor thisEnaction in listOfHitscansOnObject(theObject))
        {

            if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
        }



        return null;
    }

    private List<hitscanEnactor> listOfHitscansOnObject(GameObject theObject)
    {
        //hmm:
        //List<IEnactaBool> theList = [.. theObject.GetComponents<collisionEnaction>()];


        List<hitscanEnactor> theList = new List<hitscanEnactor>();

        foreach (hitscanEnactor thisEnaction in theObject.GetComponents<hitscanEnactor>())
        {
            theList.Add(thisEnaction);
        }


        return theList;
    }



    public planEXE2 FIXEDgoToTargetForMOBILEtargets(GameObject possiblyMobileActualTarget, float offset = 1.8f)
    {
        planEXE2 firstShell = new seriesEXE();
        //firstShell.debugPrint = printThisNPC;
        firstShell.Add(makeNavAgentPlanEXE(possiblyMobileActualTarget, offset));
        firstShell.untilListFinished();

        return firstShell;
    }


    public planEXE2 FIXEDgoToTargetForSTATIONARYtargets(Vector3 stationaryTargetPosition, float offset = 1.8f)
    {
        planEXE2 firstShell = new seriesEXE();
        //firstShell.debugPrint = printThisNPC;
        firstShell.Add(makeNavAgentPlanEXE(stationaryTargetPosition, offset));
        firstShell.untilListFinished();

        return firstShell;
    }

    private planEXE2 makeNavAgentPlanEXE(Vector3 staticTargetPosition, float offsetRoom = 0f)
    {

        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();


        planEXE2 theEXE = new vect3EXE2(theNavAgent, staticTargetPosition);//placeholderTarget1);
        //theEXE.debugPrint = printThisNPC;


        proximity condition = new proximity(this.gameObject, staticTargetPosition, offsetRoom * 1.4f);
        condition.debugPrint = theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        return theEXE;
    }


    private planEXE2 makeNavAgentPlanEXE(GameObject possiblyMobileActualTarget, float offsetRoom = 0f)
    {
        if (possiblyMobileActualTarget == null)
        {
            Debug.Log("target is null, so plan to walk to target is null");
            Debug.Log(possiblyMobileActualTarget.GetInstanceID());
            return null;
        }
        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();


        planEXE2 theEXE = new vect3EXE2(theNavAgent, possiblyMobileActualTarget);//placeholderTarget1);
        //theEXE.debugPrint = printThisNPC;


        proximity condition = new proximity(this.gameObject, possiblyMobileActualTarget, offsetRoom * 1.4f);
        condition.debugPrint = theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        return theEXE;
    }



    public Vector3 randomNearbyVector(Vector3 positionToBeNear)
    {
        Vector3 vectorToReturn = positionToBeNear;
        float initialDistance = 0f;
        float randomAdditionalDistance = UnityEngine.Random.Range(-20, 20);
        vectorToReturn += new Vector3(initialDistance + randomAdditionalDistance, 0, 0);
        randomAdditionalDistance = UnityEngine.Random.Range(-20, 20);
        vectorToReturn += new Vector3(0, 0, initialDistance + randomAdditionalDistance);

        return vectorToReturn;
    }

}



public class grabStuffStuff : behavior
{
    stuffType theStuffTypeToGrab;

    public static grabStuffStuff addGrabStuffStuff(GameObject objectToAddItTo, stuffType theStuffTypeToGrabIn)
    {

        grabStuffStuff newComponent = objectToAddItTo.AddComponent<grabStuffStuff>();

        newComponent.theStuffTypeToGrab = theStuffTypeToGrabIn;
        //newWanderBehavior.gamepadButtonType = theButtonType;
        //newWanderBehavior.theEnactions.Add(enaction1);

        return newComponent;
    }


    public override planEXE2 replacementPlan()
    {

        planEXE2 zerothShell = new seriesEXE(goGrabPlan2(theStuffTypeToGrab));
        return zerothShell;
    }

    private planEXE2 goGrabPlan2(stuffType theStuffTypeX)
    {
        //ad-hoc hand-written plan
        GameObject target = repository2.singleton.pickRandomObjectFromList(allNearbyObjectsWithStuffTypeX(theStuffTypeX));


        Debug.Assert(target!=null);

        if (target == null) 
        {
            return null; 
        }

        planEXE2 firstShell = new seriesEXE();
        firstShell.Add(FIXEDgoToTargetForMOBILEtargets(target, 1.8f));
        firstShell.Add(aimTargetPlan2(target));




        hitscanEnactor theHitscanEnactor = grabHitscanEnaction(this.gameObject, interType.standardClick); //hitscanClickPlan(interType.standardClick, target);

        Debug.Assert(theHitscanEnactor != null);
        planEXE2 hitscanEXE = theHitscanEnactor.standardEXEconversion();
        firstShell.Add(hitscanEXE);
        firstShell.untilListFinished();


        return firstShell;
    }



    public List<GameObject> allNearbyObjectsWithStuffTypeX(stuffType theStuffTypeX)
    {

        List<GameObject> theListOfALL = new find().allObjectsInObjectsZone(this.gameObject);  //lol forgot, this is ONE way to grab functions
        List<GameObject> theListOfObjects = new List<GameObject>();

        //Debug.Log("theListOfALL.Count:  "+theListOfALL.Count);

        foreach (GameObject thisObject in theListOfALL)
        {

            //Debug.Log("thisObject:  " + thisObject);
            stuffStuff theComponent = thisObject.GetComponent<stuffStuff>();
            
            if (theComponent == null) 
            {

                //Debug.Log("(theComponent == null)");
                continue; 
            }

            if (theComponent.theTypeOfStuff == theStuffTypeX)
            {
                //Debug.Log("(theComponent.theTypeOfStuff == theStuffTypeX),   so:  theListOfObjects.Add(thisObject);");
                theListOfObjects.Add(thisObject);
            }
        }

        return theListOfObjects;
    }


    private planEXE2 aimTargetPlan2(GameObject target)
    {
        aimTarget testE1 = this.gameObject.GetComponent<aimTarget>();

        planEXE2 exe1 = testE1.toEXE(target);
        exe1.atLeastOnce();

        return exe1;
    }



    private hitscanEnactor grabHitscanEnaction(GameObject theObject, interType interTypeX)
    {

        foreach (hitscanEnactor thisEnaction in listOfHitscansOnObject(theObject))
        {

            if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
        }



        return null;
    }

    private List<hitscanEnactor> listOfHitscansOnObject(GameObject theObject)
    {
        //hmm:
        //List<IEnactaBool> theList = [.. theObject.GetComponents<collisionEnaction>()];


        List<hitscanEnactor> theList = new List<hitscanEnactor>();

        foreach (hitscanEnactor thisEnaction in theObject.GetComponents<hitscanEnactor>())
        {
            theList.Add(thisEnaction);
        }


        return theList;
    }



    public planEXE2 FIXEDgoToTargetForMOBILEtargets(GameObject possiblyMobileActualTarget, float offset = 1.8f)
    {
        planEXE2 firstShell = new seriesEXE();
        //firstShell.debugPrint = printThisNPC;
        firstShell.Add(makeNavAgentPlanEXE(possiblyMobileActualTarget, offset));
        firstShell.untilListFinished();

        return firstShell;
    }


    public planEXE2 FIXEDgoToTargetForSTATIONARYtargets(Vector3 stationaryTargetPosition, float offset = 1.8f)
    {
        planEXE2 firstShell = new seriesEXE();
        //firstShell.debugPrint = printThisNPC;
        firstShell.Add(makeNavAgentPlanEXE(stationaryTargetPosition, offset));
        firstShell.untilListFinished();

        return firstShell;
    }

    private planEXE2 makeNavAgentPlanEXE(Vector3 staticTargetPosition, float offsetRoom = 0f)
    {

        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();


        planEXE2 theEXE = new vect3EXE2(theNavAgent, staticTargetPosition);//placeholderTarget1);
        //theEXE.debugPrint = printThisNPC;


        proximity condition = new proximity(this.gameObject, staticTargetPosition, offsetRoom * 1.4f);
        condition.debugPrint = theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        return theEXE;
    }


    private planEXE2 makeNavAgentPlanEXE(GameObject possiblyMobileActualTarget, float offsetRoom = 0f)
    {
        if (possiblyMobileActualTarget == null)
        {
            Debug.Log("target is null, so plan to walk to target is null");
            Debug.Log(possiblyMobileActualTarget.GetInstanceID());
            return null;
        }
        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();


        planEXE2 theEXE = new vect3EXE2(theNavAgent, possiblyMobileActualTarget);//placeholderTarget1);
        //theEXE.debugPrint = printThisNPC;


        proximity condition = new proximity(this.gameObject, possiblyMobileActualTarget, offsetRoom * 1.4f);
        condition.debugPrint = theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        return theEXE;
    }



    public Vector3 randomNearbyVector(Vector3 positionToBeNear)
    {
        Vector3 vectorToReturn = positionToBeNear;
        float initialDistance = 0f;
        float randomAdditionalDistance = UnityEngine.Random.Range(-20, 20);
        vectorToReturn += new Vector3(initialDistance + randomAdditionalDistance, 0, 0);
        randomAdditionalDistance = UnityEngine.Random.Range(-20, 20);
        vectorToReturn += new Vector3(0, 0, initialDistance + randomAdditionalDistance);

        return vectorToReturn;
    }

}






public class animalFSM
{
    Dictionary<multicondition, animalFSM> switchBoard = new Dictionary<multicondition, animalFSM>();

    //planEXE2 repeatingPlan;
    //planEXE2 currentPlan;;

    List<permaPlan> repeatingPlans;
    List<depletablePlan> currentPlans = new List<depletablePlan>();


    public animalFSM()
    {
        //      new permaPlan();
    }

    public animalFSM doAFrame()
    {
        animalFSM toSwitchTo = null;

        toSwitchTo = firstMeSwitchtCondition();
        if(toSwitchTo != null)
        {
            return toSwitchTo;
        }



        if(currentPlans.Count <1)
        {
            currentPlans = refillPlans();
        }

        executeCurrentPlans();

        return this;
    }

    private void executeCurrentPlans()
    {

        List<depletablePlan> newList = new List<depletablePlan>();

        foreach (depletablePlan plan in currentPlans)
        {
            if (plan.endConditionsMet())
            {
                continue;
            }

            if (plan.startConditionsMet())
            {
                plan.doTheDepletablePlan();
            }

            if (plan.endConditionsMet())
            {
                continue;
            }

            newList.Add(plan);
        }

        currentPlans = newList;
    }

    private List<depletablePlan> refillPlans()
    {
        List<depletablePlan> newList = new List<depletablePlan>();
        foreach (permaPlan plan in repeatingPlans)
        {
            newList.Add(plan.convertToDepletable());
        }

        return newList;
    }

    /*
    private List<permaPlan> duplicateTheRepeatingPlans()
    {
        List<permaPlan> newList = new List<permaPlan>();
        foreach (permaPlan plan in repeatingPlans)
        {
            newList.Add(plan.duplicate());
        }

        return newList;
    }
    */


    private animalFSM firstMeSwitchtCondition()
    {
        foreach(multicondition theseConditions in switchBoard.Keys)
        {
            if (theseConditions.met())
            {
                return switchBoard[theseConditions];
            }
        }

        return null;
    }

}


public class permaPlan2
{
    //boil things down so it's EASY to generate [other] abstract classes
    //by simply inputting just [lists of?] enactions and conditions?

    //and make a simple class [this one] that JUST holds one set of enactions and conditions?
    //[then can easily copy from it]


    //what?  just START conditions for the set?  or ALSO end conditions?  [and is there anything else to consider?]

    List<singleEXE> thePlan = new List<singleEXE>();

    List<condition> startConditions;
    List<condition> endConditions;


    public permaPlan2(singleEXE step1)
    {
        thePlan.Add(step1);
    }
    public permaPlan2(singleEXE step1, singleEXE step2)
    {
        thePlan.Add(step1);
        thePlan.Add(step2);
    }


    public permaPlan2(singleEXE step1, singleEXE step2, singleEXE step3)
    {

        thePlan.Add(step1);
        thePlan.Add(step2);
        thePlan.Add(step3);
    }

    public permaPlan2(singleEXE step1, singleEXE step2, singleEXE step3, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(step1);
        thePlan.Add(step2);
        thePlan.Add(step3);
    }
    public permaPlan2(singleEXE step1, singleEXE step2, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(step1);
        thePlan.Add(step2);
    }
    public permaPlan2(singleEXE step1, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(step1);
    }

    public permaPlan2(singleEXE step1, singleEXE step2, condition startCondition1, condition startCondition2)
    {
        startConditions.Add(startCondition1);
        startConditions.Add(startCondition2);
        thePlan.Add(step1);
        thePlan.Add(step2);
    }
    public permaPlan2(singleEXE step1, singleEXE step2, singleEXE step3, condition startCondition1, condition startCondition2)
    {
        startConditions.Add(startCondition1);
        startConditions.Add(startCondition2);
        thePlan.Add(step1);
        thePlan.Add(step2);
        thePlan.Add(step3);
    }



    List<singleEXE> outPutCopyOfThePlan()
    {
        List<singleEXE> newList = new List<singleEXE>();

        foreach (singleEXE thisOne in thePlan)
        {
            newList.Add(thisOne);
        }

        return newList;
    }
    List<condition> outPutCopyOfStartConditions()
    {
        List<condition> newList = new List<condition>();

        foreach (condition thisOne in startConditions)
        {
            newList.Add(thisOne);
        }

        return newList;
    }
    List<condition> outPutCopyOfEndConditions()
    {
        List<condition> newList = new List<condition>();

        foreach (condition thisOne in endConditions)
        {
            newList.Add(thisOne);
        }

        return newList;
    }

    internal depletablePlan convertToDepletable()
    {
        depletablePlan newThing = new depletablePlan();

        foreach (singleEXE thisOne in thePlan)
        {
            newThing.add(thisOne);
        }

        return newThing;
    }



}


public class permaPlan
{
    //boil things down so it's EASY to generate [other] abstract classes
    //by simply inputting just [lists of?] enactions and conditions?

    //and make a simple class [this one] that JUST holds one set of enactions and conditions?
    //[then can easily copy from it]


    //what?  just START conditions for the set?  or ALSO end conditions?  [and is there anything else to consider?]

    List<enaction> thePlan = new List<enaction>();

    List<condition> startConditions;
    List<condition> endConditions;


    public permaPlan(enaction enaction1)
    {
        thePlan.Add(enaction1);
    }
    public permaPlan(enaction enaction1, enaction enaction2)
    {
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
    }
    public permaPlan(enaction enaction1, enaction enaction2, enaction enaction3)
    {
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
        thePlan.Add(enaction3);
    }

    public permaPlan(enaction enaction1, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(enaction1);
    }
    public permaPlan(enaction enaction1, enaction enaction2, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
    }
    public permaPlan(enaction enaction1, enaction enaction2, enaction enaction3, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
        thePlan.Add(enaction3);
    }

    public permaPlan(enaction enaction1, condition startCondition1, condition startCondition2)
    {
        startConditions.Add(startCondition1);
        startConditions.Add(startCondition2);

        thePlan.Add(enaction1);
    }
    public permaPlan(enaction enaction1, enaction enaction2, condition startCondition1, condition startCondition2)
    {
        startConditions.Add(startCondition1);
        startConditions.Add(startCondition2);
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
    }
    public permaPlan(enaction enaction1, enaction enaction2, enaction enaction3, condition startCondition1, condition startCondition2)
    {
        startConditions.Add(startCondition1);
        startConditions.Add(startCondition2);
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
        thePlan.Add(enaction3);
    }

    List<enaction> outPutCopyOfThePlan()
    {
        List<enaction> newList = new List<enaction>();

        foreach (enaction thisOne in thePlan)
        {
            newList.Add(thisOne);
        }

        return newList;
    }
    List<condition> outPutCopyOfStartConditions()
    {
        List<condition> newList = new List<condition>();

        foreach (condition thisOne in startConditions)
        {
            newList.Add(thisOne);
        }

        return newList;
    }
    List<condition> outPutCopyOfEndConditions()
    {
        List<condition> newList = new List<condition>();

        foreach (condition thisOne in endConditions)
        {
            newList.Add(thisOne);
        }

        return newList;
    }

    internal depletablePlan convertToDepletable()
    {
        throw new NotImplementedException();
    }
}



public class depletablePlan
{
    //AKA "planEXE", basically?  no!  this REPLACES series exe, and ABOVE this [animalFSM] holds PARALLEL.
    //so here:  just use singleEXE instead of enaction?  [for "inputData" for enacting]
    //List<enaction> thePlan = new List<enaction>();
    List<singleEXE> thePlan = new List<singleEXE>();  //should make a "SUPERsingleEXE" that CANNOT have any mess of holding series in it?
                                                      //no further layers below the single enaction within it?  we'll see.  
    //List<enactable> thePlan = new List<enactable>();  //call it an enactable?

    List<condition> startConditions = new List<condition>();
    List<condition> endConditions = new List<condition>();

    public depletablePlan()
    {

        endConditions.Add(new depletableSingleEXEListComplete(thePlan));
    }

    public depletablePlan(singleEXE step1)
    {
        thePlan.Add(step1);

        //always need "empty plan list" as an end condition, for animalFSM or something?

        endConditions.Add(new depletableSingleEXEListComplete(thePlan));
    }
    public depletablePlan(singleEXE step1, singleEXE step2)
    {
        thePlan.Add(step1);
        thePlan.Add(step2);

        //always need "empty plan list" as an end condition, for animalFSM or something?

        endConditions.Add(new depletableSingleEXEListComplete(thePlan));
    }
    public depletablePlan(singleEXE step1, singleEXE step2, singleEXE step3)
    {
        thePlan.Add(step1);
        thePlan.Add(step2);
        thePlan.Add(step3);

        //always need "empty plan list" as an end condition, for animalFSM or something?

        endConditions.Add(new depletableSingleEXEListComplete(thePlan));
    }



    public void add(singleEXE toAdd)
    {
        thePlan.Add(toAdd);
    }

    //public abstract void doTheDepletablePlan();
    public void doTheDepletablePlan()
    {
        executeSequential();
    }

    public void executeSequential()
    {

        //conditionalPrint("executeSequential()");
        //conditionalPrint("x1 nestedPlanCountToText():  " + nestedPlanCountToText());
        //this function is here because i want the lists to be private so that parallel and sequential EXEs initialize correctly

        //sequential concerns:
        //      only execute 1st one
        //      remove item from list when its end conditions are met

        if (thePlan == null)
        { 
            //Debug.Log("null.....that's an error!"); 
            return;
        }

        if (thePlan.Count < 1)
        {
            //Debug.Log("exeList.Count < 1       shouldn't happen?  or means this plan has reached the end"); 
            return;
        }


        if (thePlan[0] == null)
        { 
            //Debug.Log("null.....that's an error!"); 
            return;
        }

        //exeList[0].grabberDebug = grabberDebug;
        //      conditionalPrint("5555555555555555555555555555grabberDebug.GetInstanceID():  " + grabberDebug.GetInstanceID());
        //      grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);
        //conditionalPrint("x2 nestedPlanCountToText():  " + nestedPlanCountToText());

        if (startConditionsMet()&& thePlan[0].startConditionsMet())
        {
            //Debug.Log("start conitions met, should do this:  " + thePlan[0].staticEnactionNamesInPlanStructure());
            //conditionalPrint(" grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);...........");
            //conditionalPrint("??????????????????????????????? exeList[0].theEnaction:  " + exeList[0].theEnaction);
            //grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);
            //conditionalPrint("///////////////////////////////////////////////////////////////////////");
            thePlan[0].execute();
        }

        //conditionalPrint("x3 nestedPlanCountToText():  " + nestedPlanCountToText());

        if (thePlan[0].endConditionsMet())
        {
            //Debug.Log("exeList[0].endConditionsMet()  for:  " + thePlan[0]);
            
            //conditionalPrint("x4 nestedPlanCountToText():  " + nestedPlanCountToText());
            //conditionalPrint("endConditionsMet, so:  exeList.RemoveAt(0)");
            thePlan.RemoveAt(0);

            //conditionalPrint("x5 nestedPlanCountToText():  " + nestedPlanCountToText());

            return;
        }

        
        //????????
        if (endConditionsMet())
        {
            //Debug.Log("exeList[0].endConditionsMet()  for:  " + this);

            //conditionalPrint("x4 nestedPlanCountToText():  " + nestedPlanCountToText());
            //conditionalPrint("endConditionsMet, so:  exeList.RemoveAt(0)");
            thePlan.Clear();

            //conditionalPrint("x5 nestedPlanCountToText():  " + nestedPlanCountToText());

            return;
        }

        //conditionalPrint("x6 nestedPlanCountToText():  " + nestedPlanCountToText());
    }


    public bool startConditionsMet()
    {
        //grabberDebug.debugPrintBool = debugPrint;
        //Debug.Log("tartConditions.Count:  " + startConditions.Count);
        foreach (condition thisCondition in startConditions)
        {
            //Debug.Log("thisCondition:  " + thisCondition);
            //Debug.Log("thisCondition.met():  " + thisCondition.met());
            if (thisCondition.met() == false)
            {

                //if (debugPrint == true) { Debug.Log("this start condition not met:  " + thisCondition); }
                //      grabberDebug.rep
                return false;
            }
        }

        //Debug.Log("no start conditions remain unfulfilled!");
        //Debug.Log("no conditions remain unfulfilled!");
        return true;
    }

    public bool endConditionsMet()
    {
        //Debug.Log("looking at end conditions for:  " + this);

        //if (theEnaction != null) { Debug.Log("looking at end conditions for:  " + theEnaction); }
        foreach (condition thisCondition in endConditions)
        {
            //conditionalPrint("thisCondition:  " + thisCondition);
            //Debug.Log("thisCondition:  " + thisCondition);
            //if (theEnaction != null) { Debug.Log("thisCondition:  " + thisCondition); }
            if (thisCondition.met() == false)
            {
                //conditionalPrint("this end condition not met:  "+ thisCondition);
                return false;
            }
        }
        //Debug.Log("no conditions remain unfulfilled!");

        //conditionalPrint("no end conditions remain unfulfilled!");
        //if (theEnaction != null) { Debug.Log("so this enaction is finished:  " + theEnaction); }

        return true;
    }

}



public class depletableSingleEXEListComplete : condition
{
    List<singleEXE> planList;

    public depletableSingleEXEListComplete(List<singleEXE> planList)
    {
        this.planList = planList;
    }

    public override bool met()
    {
        //Debug.Log("planList.Count:  " + planList.Count);
        foreach (singleEXE planEXE in planList)
        {
            if (planEXE == null) { continue; } //messy annoying for now
            if (planEXE.endConditionsMet() == false) { return false; }
        }
        return true;
    }


    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }
}


/*

internal class enactable
{
    public enaction theEnaction;
}

public abstract class planEXE2
{
    public enaction theEnaction;
    //public inputData theInputData;

    public bool debugPrint = false;
    //nestedLayerDebug debug;
    //public adHocDebuggerForGoGrabPlan grabberDebug;// = new adHocDebuggerForGoGrabPlan();

    //      !!!!!!!!!!!!!!!  was supposed to be private so that constructors inputs guarantee it's never null...but then i changed the constructors again...
    public List<planEXE2> exeList;

    public List<condition> startConditions = new List<condition>();
    public List<condition> endConditions = new List<condition>();

    public int numberOfTimesExecuted = 0;  //don't do it for things that are called every frame, though?

    public abstract void execute();

    public abstract bool error();


    public void doConditionalEffectsAdHocDebugThing(targetCalculator theTargetCalculatorIn, hitscanEnactor theHitscanEnactorIn, adHocDebuggerForGoGrabPlan grabberDebugIn, Dictionary<condition, List<Ieffect>> conditionalEffectsIn, adHocBooleanDeliveryClass signalThatFiringIsDone)
    {
        //okSuperAdhocPlaceToDoThisDebugNonsense

        targetMatchesHitscanOutput theCondition = new targetMatchesHitscanOutput(theTargetCalculatorIn);//, theHitCalculatorIn);

        theCondition.firingIsDone = signalThatFiringIsDone;
        theCondition.theHitScanner = theHitscanEnactorIn;

        Ieffect theEffect = new adHocDebugEffect(grabberDebugIn, theCondition);
        List<Ieffect> theEffects = new List<Ieffect>();
        theEffects.Add(theEffect);
        conditionalEffectsIn[theCondition] = theEffects;
    }




    public string nestedPlanCountToText()
    {
        string stringToReturn = "";

        if (exeList == null) { return "[(exeList == null), no nested plans to count]"; }
        if (exeList.Count == 0) { return "[(exeList.Count == 0), no nested plans to count]"; }

        stringToReturn += "[exeList.Count = " + exeList.Count;

        foreach (planEXE2 thisPlan in exeList)
        {
            if (thisPlan != null)
            {

                stringToReturn += thisPlan.nestedPlanCountToText();
            }
        }


        stringToReturn += "]";

        return stringToReturn;
    }


    public bool standardExecuteErrors()
    {
        if (theEnaction == null) { Debug.Log("null.....that's an error!"); return true; } //is it, though?

        //conditionalPrint("startConditionsMet():  "+ startConditionsMet());
        if (startConditionsMet() == false) { return true; }

        return false;
    }

    public void executeSequential()
    {

        //conditionalPrint("executeSequential()");
        //conditionalPrint("x1 nestedPlanCountToText():  " + nestedPlanCountToText());
        //this function is here because i want the lists to be private so that parallel and sequential EXEs initialize correctly

        //sequential concerns:
        //      only execute 1st one
        //      remove item from list when its end conditions are met

        if (exeList == null)
        { //Debug.Log("null.....that's an error!"); 
            return;
        }

        if (exeList.Count < 1)
        {
            //Debug.Log("exeList.Count < 1       shouldn't happen?"); 
            return;
        }


        if (exeList[0] == null)
        { //Debug.Log("null.....that's an error!"); 
            return;
        }

        exeList[0].debugPrint = debugPrint;

        //exeList[0].grabberDebug = grabberDebug;
        //      conditionalPrint("5555555555555555555555555555grabberDebug.GetInstanceID():  " + grabberDebug.GetInstanceID());
        //      grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);
        //conditionalPrint("x2 nestedPlanCountToText():  " + nestedPlanCountToText());

        if (startConditionsMet())
        {
            //Debug.Log("start conitions met, should do this:  " + exeList[0].staticEnactionNamesInPlanStructure());
            //conditionalPrint(" grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);...........");
            //conditionalPrint("??????????????????????????????? exeList[0].theEnaction:  " + exeList[0].theEnaction);
            //grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);
            //conditionalPrint("///////////////////////////////////////////////////////////////////////");
            exeList[0].execute();
        }

        //conditionalPrint("x3 nestedPlanCountToText():  " + nestedPlanCountToText());

        if (exeList[0].endConditionsMet())
        {
            //Debug.Log("exeList[0].endConditionsMet()  for:  " + exeList[0]);
            if (exeList[0].theEnaction != null)
            {
                //Debug.Log("exeList[0].endConditionsMet()  for theEnaction:  " + exeList[0].theEnaction);
            }


            //conditionalPrint("x4 nestedPlanCountToText():  " + nestedPlanCountToText());
            //conditionalPrint("endConditionsMet, so:  exeList.RemoveAt(0)");
            exeList.RemoveAt(0);

            //conditionalPrint("x5 nestedPlanCountToText():  " + nestedPlanCountToText());

            return;
        }

        //conditionalPrint("x6 nestedPlanCountToText():  " + nestedPlanCountToText());
    }

    public void executeParallel()
    {
        if (exeList == null) { Debug.Log("null.....that's an error!"); ; return; }

        //if null.....that's an error!


        List<planEXE2> completedItems = new List<planEXE2>();

        foreach (planEXE2 plan in exeList)
        {

            //plan.grabberDebug = grabberDebug;
            plan.execute();
            if (plan.endConditionsMet()) { completedItems.Add(plan); }
        }

        foreach (planEXE2 plan in completedItems)
        {
            exeList.Remove(plan);
        }
    }





    public bool startConditionsMet()
    {
        //grabberDebug.debugPrintBool = debugPrint;
        //Debug.Log("tartConditions.Count:  " + startConditions.Count);
        foreach (condition thisCondition in startConditions)
        {
            //Debug.Log("thisCondition:  " + thisCondition);
            //Debug.Log("thisCondition.met():  " + thisCondition.met());
            if (thisCondition.met() == false)
            {

                //if (debugPrint == true) { Debug.Log("this start condition not met:  " + thisCondition); }
                //      grabberDebug.rep
                return false;
            }
        }

        //Debug.Log("no start conditions remain unfulfilled!");
        //Debug.Log("no conditions remain unfulfilled!");
        return true;
    }

    public bool endConditionsMet()
    {
        //Debug.Log("looking at end conditions for:  " + this);

        if (debugPrint == true)
        {
            if (theEnaction != null)
            {
                //  conditionalPrint("-----------------------------looking at end conditions for a single enaction:  " + theEnaction.ToString());
            }
            else if (exeList != null)
            {

                //conditionalPrint("...............................looking at end conditions for an exeList???  the count of the list:  " + exeList.Count);
            }
            else
            {
                conditionalPrint("uhhhhhhhhh.....???????????? both the enaction AND the exeList are null...........");
            }
        }
        //if (theEnaction != null) { Debug.Log("looking at end conditions for:  " + theEnaction); }
        foreach (condition thisCondition in endConditions)
        {
            //conditionalPrint("thisCondition:  " + thisCondition);
            //Debug.Log("thisCondition:  " + thisCondition);
            //if (theEnaction != null) { Debug.Log("thisCondition:  " + thisCondition); }
            if (thisCondition.met() == false)
            {
                //conditionalPrint("this end condition not met:  "+ thisCondition);
                return false;
            }
        }
        //Debug.Log("no conditions remain unfulfilled!");

        //conditionalPrint("no end conditions remain unfulfilled!");
        //if (theEnaction != null) { Debug.Log("so this enaction is finished:  " + theEnaction); }

        return true;
    }


    public void atLeastOnce()
    {
        condition thisCondition = new enacted(this);
        endConditions.Add(thisCondition);
    }

    public void untilListFinished()
    {
        if (exeList == null) { exeList = new List<planEXE2>(); }
        condition thisCondition = new planListComplete(exeList);  //should be fine?  lists are references, so will work even if items are added after this??
        endConditions.Add(thisCondition);
    }


    public string asText()
    {

        string theString = "";

        theString += staticEnactionNamesInPlanStructure();

        return theString;
    }


    public string infoString3()
    {
        string theString = "";

        theString += this.ToString();
        theString += ":  ";
        theString += theEnaction;

        theString += conditionsAsText();

        if (exeList == null) { return theString; }


        theString += "[ ";
        foreach (planEXE2 plan in exeList)
        {
            if (plan == null) { theString += "(plan == null)"; continue; }
            theString += plan.asText();
            theString += ", ";
        }

        theString += "]";
        return theString;
    }



    public string staticEnactionNamesInPlanStructure()
    {
        string theString = "";

        theString += this.ToString();
        theString += ":  ";
        theString += theEnaction;

        if (exeList == null) { return theString; }


        theString += "[ ";
        foreach (planEXE2 plan in exeList)
        {
            if (plan == null) { theString += "(plan == null)"; continue; }
            theString += plan.staticEnactionNamesInPlanStructure();
            theString += ", ";
        }

        theString += "]";
        return theString;
    }



    public string conditionsAsText()
    {
        string stringToReturn = "";

        stringToReturn += "number of START conditions:  " + startConditions.Count;

        foreach (condition condition in startConditions)
        {
            stringToReturn += ", ";
            stringToReturn += condition.asText();
        }
        stringToReturn += ", number of END conditions:  " + endConditions.Count;

        foreach (condition condition in endConditions)
        {
            stringToReturn += ", ";
            stringToReturn += condition.asText();
        }


        return stringToReturn;
    }


    internal void conditionalPrint(string thingToPrint)
    {
        if (debugPrint == false) { return; }


        Debug.Log(thingToPrint);

    }





    public void Add(planEXE2 itemToAdd)
    {
        if (exeList == null) { exeList = new List<planEXE2>(); }
        exeList.Add(itemToAdd);
    }

    internal void Add(List<planEXE2> addFromList)
    {

        if (exeList == null) { exeList = new List<planEXE2>(); }
        foreach (planEXE2 item in addFromList)
        {
            exeList.Add(item);
        }
    }
}


public abstract class singleEXE : planEXE2
{
    //private GameObject target;



    public abstract override void execute();
}

*/

public class stuffStuff : MonoBehaviour
{
    public stuffType theTypeOfStuff;

    public static stuffStuff addStuffStuff(GameObject theObject, stuffType theTypeOfStuffToAdd)
    {
        stuffStuff newStuffStuff = theObject.AddComponent<stuffStuff>();

        newStuffStuff.theTypeOfStuff = theTypeOfStuffToAdd;



        return newStuffStuff;
    }
}