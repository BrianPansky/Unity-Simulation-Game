using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        grabStuffStuff.addGrabStuffStuff(newObj, stuffTypeX);

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

        Debug.Log("theListOfALL.Count:  "+theListOfALL.Count);

        foreach (GameObject thisObject in theListOfALL)
        {

            Debug.Log("thisObject:  " + thisObject);
            stuffStuff theComponent = thisObject.GetComponent<stuffStuff>();
            
            if (theComponent == null) 
            {

                Debug.Log("(theComponent == null)");
                continue; 
            }

            if (theComponent.theTypeOfStuff == theStuffTypeX)
            {
                Debug.Log("(theComponent.theTypeOfStuff == theStuffTypeX),   so:  theListOfObjects.Add(thisObject);");
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