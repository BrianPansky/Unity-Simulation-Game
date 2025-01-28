using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
using static enactionCreator;
using static interactionCreator;
using static UnityEngine.GraphicsBuffer;

public class animalGen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 location = Vector3.zero;

        location = new Vector3(20, 0, -15);
        makeEmptyZones(1,520);

        
        genGen.singleton.returnShotgun1(new Vector3(-36, 5, 13));


        adhocAnimal1Gen animalGen1 = new adhocAnimal1Gen(this.gameObject, stuffType.fruit);

        animalGen1.spawn(new Vector3(0, 0.5f, 5));



        /*
        //returnArrowForward(location,3);
        returnBasicAnimal1(location, stuffType.fruit, 1);
        
        returnBasicAnimal1(new Vector3(20, 0, 5), stuffType.fruit, 1);
        returnBasicAnimal1(new Vector3(10, 0, 15), stuffType.fruit, 1);
        returnBasicAnimal1(new Vector3(15, 0, -5), stuffType.fruit, 1);
        returnBasicAnimal1(new Vector3(-10, 0, -15), stuffType.fruit, 1);
        */
        location = new Vector3(10, 0, 5);
        returnBasicPredator1(new Vector3(70, 0, -35), stuffType.meat1, 2f);
        
        genGen.singleton.returnNPC5(new Vector3(-30, 0, 35));
        genGen.singleton.returnShotgun1(new Vector3(-36, 1, 36));
        
        returnBasicGrabbable(stuffType.fruit, new Vector3(-5, 2, 8));
        //returnBasicGrabbable(stuffType.meat1, new Vector3(-17, 2, 2));
        returnBasicGrabbable(stuffType.fruit, new Vector3(-15, 2, -4));

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public GameObject fullAnimalGen1(Vector3 where, stuffType stuffTypeX, float scale = 1f)
    {
        //GameObject newObj = returnArrowForward(where, scale);

        GameObject newObj = Instantiate(repository2.singleton.placeHolderCubePrefab, where, Quaternion.identity);
        //      newObj.transform.localScale = new Vector3(128, 1, 8);
        newObj.transform.localScale = scale * newObj.transform.localScale;









        genGen.singleton.ensureVirtualGamePad(newObj);







        //addAnimalBody1ToObject(newObj);

        playable2 thePlayable = newObj.AddComponent<playable2>();


        //thePlayable.dictOfInteractions = new Dictionary<enactionCreator.interType, List<Ieffect>>();
        //thePlayable.dictOfIvariables = new Dictionary<interactionCreator.numericalVariable, float>();

        thePlayable.dictOfIvariables[numericalVariable.health] = 2;
        //thePlayable.equipperSlotsAndContents[interactionCreator.simpleSlot.hands] = null;
        thePlayable.initializeEnactionPoint1();
        //addArrowForward(thePlayable.enactionPoint1);
        //genGen.singleton.addCube(thePlayable.enactionPoint1, 0.1f);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        //addArrowForward(newObj, 5f, 0f, 1.2f);
        genGen.singleton.makeBasicEnactions(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);


        inventory1 theirInventory = newObj.AddComponent<inventory1>();












        genGen.singleton.addArrowForward(newObj, 1f, 0f, 1.2f);
        //wander1.addWander1(newObj);
        //grabGun1.addGrabGun1(newObj, interType.peircing);

        //              stuffStuff.addStuffStuff(newObj, stuffType.meat1);

        //Debug.Log("?????????????????????????????????????????????????????");
        animalUpdate theUpdate = newObj.AddComponent<animalUpdate>();
        //Debug.Log("?????????????????????    2   ??????????????????????????");
        theUpdate.theFSM = herbavoreForagingBehavior1(newObj, stuffTypeX);


        newObj.GetComponent<interactable2>().dictOfIvariables[numericalVariable.cooldown] = 0f;

        //Debug.Log("????????????????????      3   ???????????????????????");
        //grabStuffStuff.addGrabStuffStuff(newObj, stuffTypeX);

        return newObj;
    }







    public GameObject returnBasicAnimal1(Vector3 where, stuffType stuffTypeX, float scale = 1f)
    {
        GameObject newObj = returnArrowForward(where, scale);

        genGen.singleton.ensureVirtualGamePad(newObj);

        addAnimalBody1ToObject(newObj);
        genGen.singleton.addArrowForward(newObj, 1f, 0f, 1.2f);
        //wander1.addWander1(newObj);
        //grabGun1.addGrabGun1(newObj, interType.peircing);

        //              stuffStuff.addStuffStuff(newObj, stuffType.meat1);

        //Debug.Log("?????????????????????????????????????????????????????");
        animalUpdate theUpdate= newObj.AddComponent<animalUpdate>();
        //Debug.Log("?????????????????????    2   ??????????????????????????");
        theUpdate.theFSM = herbavoreForagingBehavior1(newObj, stuffTypeX);


        newObj.GetComponent<interactable2>().dictOfIvariables[numericalVariable.cooldown] = 0f;

        //Debug.Log("????????????????????      3   ???????????????????????");
        //grabStuffStuff.addGrabStuffStuff(newObj, stuffTypeX);

        return newObj;
    }
    
    public GameObject returnBasicPredator1(Vector3 where, stuffType stuffTypeX, float scale = 1f)
    {
        GameObject newObj = returnBasicAnimal1(where,stuffTypeX ,scale);

        tagging2.singleton.addTag(newObj, tagging2.tag2.threat1);

        playable2 thePlayable = newObj.GetComponent<playable2>();
        //Debug.Log("?????????????????????????????????????????????????????");
        hitscanEnactor.addHitscanEnactor(thePlayable.gameObject, thePlayable.enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.melee));



        //genGen.singleton.ensureVirtualGamePad(newObj);
        animalUpdate theUpdate = newObj.GetComponent<animalUpdate>();
        theUpdate.theFSM = predatorForagingBehavior1(newObj, stuffTypeX);

        //      theUpdate.theFSM = predatorForagingBehavior1(newObj, stuffTypeX);


        MeshRenderer theRenderer = newObj.GetComponent<MeshRenderer>();
        theRenderer.material.color = new Color(1f, 0f, 0f);


        NavMeshAgent navMeshAgent = newObj.GetComponent<NavMeshAgent>();
        navMeshAgent.speed += 1.7f;

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

        interactionCreator.singleton.addInteraction(newObj, interType.standardClick, new interactionEffect(new deathEffect(newObj)));


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
        //thePlayable.equipperSlotsAndContents[interactionCreator.simpleSlot.hands] = null;
        thePlayable.initializeEnactionPoint1();
        //addArrowForward(thePlayable.enactionPoint1);
        //genGen.singleton.addCube(thePlayable.enactionPoint1, 0.1f);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        //addArrowForward(newObj, 5f, 0f, 1.2f);
        genGen.singleton.makeBasicEnactions(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);


        inventory1 theirInventory = newObj.AddComponent<inventory1>();
    }










    //behavior

    /*
    void setupTest2()
    {
        //then, to test it, call "plan.doTheDepletablePlan();" in the update
        singleEXE step1 = genGen.singleton.makeNavAgentPlanEXE(this.gameObject, patternScript2.singleton.randomNearbyVector(this.transform.position));
        singleEXE step2 = genGen.singleton.makeNavAgentPlanEXE(this.gameObject, patternScript2.singleton.randomNearbyVector(this.transform.position));
        singleEXE step3 = genGen.singleton.makeNavAgentPlanEXE(this.gameObject, patternScript2.singleton.randomNearbyVector(this.transform.position));

        perma1 = new permaPlan2(step1, step2, step3);

        //plan = new depletablePlan(step1, step2);
        plan = perma1.convertToDepletable();



        //now
        //      how to make random wander take...ONE step?  but generate new location each time?
        //      generate location OUTSIDE the simple class, and input it........?
        //          and, what, that input........always takes vector3?  or sometimes GameObject?  so use "target calculator" as input?
        //              i' doing that in animalFSM?  the switch conditions ALSO provide stuff like threats/target/navpoint?


    }

    void setupTest3()
    {
        //then do "simpleRepeat1.doThisThing();" in update
        singleEXE exe = genGen.singleton.makeNavAgentPlanEXE(this.gameObject, patternScript2.singleton.randomNearbyVector(this.transform.position));

        perma1 = new permaPlan2(exe);

        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();

        simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);

    }

    void setupTest4()
    {
        //then, to test it, call "repeatWithTargetPickerTest.doThisThing();" in the update
        singleEXE step1 = genGen.singleton.makeNavAgentPlanEXE(this.gameObject, patternScript2.singleton.randomNearbyVector(this.transform.position));


        perma1 = new permaPlan2(step1);

        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();

        //simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);

        repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(this.gameObject));

    }

    void setupTest5()
    {
        repeatWithTargetPickerTest = goToStuff(stuffType.fruit);
    }

    void setupTest5_2()
    {
        repeatWithTargetPickerTest = grabTheStuff(stuffType.fruit);
    }

    */


    private animalFSM predatorForagingBehavior1(GameObject theObjectDoingTheEnaction, stuffType stuffX)
    {


        animalFSM wander = new animalFSM(randomWanderRepeatable(theObjectDoingTheEnaction));
        animalFSM grabMeat = new animalFSM(returnTheGoToThingOfTypeXAndInteractWithTypeY(theObjectDoingTheEnaction, stuffX, interType.standardClick));//new animalFSM(new repeatWithTargetPicker(new permaPlan2(goGrabPlan2(theObjectDoingTheEnaction,stuffX)), new setOfAllNearbyStuffStuff(stuffX)));
        animalFSM killPrey = new animalFSM(returnTheGoToThingWithNumericalVariableXAndInteractWithTypeY(theObjectDoingTheEnaction, numericalVariable.health, interType.melee));


        condition switchCondition1 = new canSeeStuffStuff(theObjectDoingTheEnaction, stuffX);

        condition switchCondition2 = new canSeeNumericalVariable(theObjectDoingTheEnaction, numericalVariable.health);


        //wander.addSwitchAndReverse(new stickyCondition(switchCondition1, 90), grabMeat);
        wander.addSwitchAndReverse(new stickyCondition(switchCondition2, 90), killPrey);

        //killPrey.addSwitch(new stickyCondition(switchCondition1, 90), grabMeat);


        return wander;
    }

    private repeater returnTheGoToThingWithNumericalVariableXAndInteractWithTypeY(GameObject theObjectDoingTheEnactions, numericalVariable numVarX, interType interTypeX)
    {

        targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions, 
            new setOfAllNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(
            genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions, 
                getter.pickNext().realPositionOfTarget()), 
                aimTargetPlan2(theObjectDoingTheEnactions, theObjectDoingTheEnactions), 
                fireHitscan(theObjectDoingTheEnactions, interTypeX));
        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();
        //simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);
        //repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(theObjectDoingTheEnactions));
        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, getter);


        return repeatWithTargetPickerTest;

    }

    animalFSM herbavoreForagingBehavior1(GameObject theObjectDoingTheEnaction, stuffType stuffX)
    {
        condition switchCondition1 = new stickyCondition(new canSeeStuffStuff(theObjectDoingTheEnaction, stuffX), 90);

        animalFSM theFSM = new animalFSM(randomWanderRepeatable(theObjectDoingTheEnaction));
        animalFSM getFood = new animalFSM(grabTheStuff(theObjectDoingTheEnaction, stuffX));
        animalFSM flee = new animalFSM(genGen.singleton.meleeDodge(theObjectDoingTheEnaction));
        //switchCondition, grabTheStuff(theObjectDoingTheEnaction,stuffX)

        /*
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new objectHasTag(tagging2.tag2.threat1),
            new stickyTrueCriteria(new lineOfSight(theObjectDoingTheEnaction), 30),
            new stickyTrueCriteria(new proximityCriteriaBool(theObjectDoingTheEnaction, 40)),
            new stickyTrueCriteria(new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform), 90)
            );
        */
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new objectHasTag(tagging2.tag2.threat1),
            new lineOfSight(theObjectDoingTheEnaction),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 25)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        //objectSetGrabber theFleeFromObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        objectSetGrabber theFleeFromObjectSet = new setOfAllObjectThatMeetCriteria(new excludeX(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theObjectDoingTheEnaction), theCriteria);



        //condition switchCondition1 = new canSeeStuffStuff(theObjectDoingTheEnaction, stuffX);

        condition switchToFlee = new stickyCondition(
            new isThereAtLeastOneObjectInSet(theFleeFromObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        //wander.addSwitchAndReverse(new stickyCondition(switchCondition1, 90), grabMeat);
        theFSM.addSwitchAndReverse(new stickyCondition(switchToFlee, 10), flee);
        theFSM.addSwitchAndReverse(new stickyCondition(switchCondition1, 10), getFood);
        getFood.addSwitch(new stickyCondition(switchToFlee, 10), flee);





        /*

        //condition switchCondition1 = new stickyCondition(new canSeeStuffStuff(theObjectDoingTheEnaction, stuffX), 90);

        animalFSM theFSM = new animalFSM(randomWanderRepeatable(theObjectDoingTheEnaction));
        //animalFSM getFood = new animalFSM(grabTheStuff(theObjectDoingTheEnaction, stuffX));
        animalFSM flee = new animalFSM(genGen.singleton.meleeDodge(theObjectDoingTheEnaction));
        //switchCondition, grabTheStuff(theObjectDoingTheEnaction,stuffX)


        objectCriteria theCriteria = new objectMeetsAllCriteria(
            //new objectHasTag(tagging2.tag2.threat1),
            //new stickyTrueCriteria(new lineOfSight(theObjectDoingTheEnaction), 60),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 8)
            //new stickyTrueCriteria(new proximityCriteriaBool(theObjectDoingTheEnaction, 40))
            //new stickyTrueCriteria(new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform), 90)
            );

        objectSetGrabber theFleeFromObjectSet = new setOfAllObjectThatMeetCriteria(new excludeX(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theObjectDoingTheEnaction), theCriteria);



        //condition switchCondition1 = new canSeeStuffStuff(theObjectDoingTheEnaction, stuffX);

        //condition switchToFlee = new stickyCondition(new isThereAtLeastOneObjectInSet(theFleeFromObjectSet), 10);
        condition switchToFlee = new isThereAtLeastOneObjectInSet(theFleeFromObjectSet);// theObjectDoingTheEnaction, numericalVariable.health);


        //wander.addSwitchAndReverse(new stickyCondition(switchCondition1, 90), grabMeat);
        //theFSM.addSwitchAndReverse(new stickyCondition(switchToFlee, 10), flee);
        theFSM.addSwitchAndReverse(switchToFlee, flee);
        //theFSM.addSwitchAndReverse(new stickyCondition(switchCondition1, 10), getFood);
        //getFood.addSwitch(new stickyCondition(switchToFlee, 10), flee);


        
        condition switchCondition1 = new stickyCondition(new canSeeStuffStuff(theObjectDoingTheEnaction, stuffX), 90);

        animalFSM theFSM = new animalFSM(randomWanderRepeatable(theObjectDoingTheEnaction));
        animalFSM getFood = new animalFSM(grabTheStuff(theObjectDoingTheEnaction, stuffX));
        animalFSM flee = new animalFSM(genGen.singleton.meleeDodge(theObjectDoingTheEnaction));
        //switchCondition, grabTheStuff(theObjectDoingTheEnaction,stuffX)


        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new objectHasTag(tagging2.tag2.threat1),
            new stickyTrueCriteria(new lineOfSight(theObjectDoingTheEnaction), 60),
            new stickyTrueCriteria(new proximityCriteriaBool(theObjectDoingTheEnaction, 40)),
            new stickyTrueCriteria(new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform), 90)
            );

        objectSetGrabber theFleeFromObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);



        //condition switchCondition1 = new canSeeStuffStuff(theObjectDoingTheEnaction, stuffX);

        condition switchToFlee = new stickyCondition(
            new isThereAtLeastOneObjectInSet(theFleeFromObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        //wander.addSwitchAndReverse(new stickyCondition(switchCondition1, 90), grabMeat);
        theFSM.addSwitchAndReverse(new stickyCondition(switchToFlee, 10), flee);
        theFSM.addSwitchAndReverse(new stickyCondition(switchCondition1, 10), getFood);
        getFood.addSwitch(new stickyCondition(switchToFlee, 10), flee);

        */



        return theFSM;
    }

    private repeatWithTargetPicker randomWanderRepeatable(GameObject theObjectDoingTheEnaction)
    {
        singleEXE step1 = genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnaction, patternScript2.singleton.randomNearbyVector(theObjectDoingTheEnaction.transform.position));
        permaPlan2 perma1 = new permaPlan2(step1);
        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();
        //simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);
        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(theObjectDoingTheEnaction));

        return repeatWithTargetPickerTest;
    }
    private repeatWithTargetPicker grabTheStuff(GameObject theObjectDoingTheEnaction, stuffType stuffX)
    {
        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));
        //perma1 = new permaPlan2(step1);

        //repeatWithTargetPicker otherBehavior = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(this.gameObject));


        return new ummAllThusStuffForGrab(theObjectDoingTheEnaction, stuffX).returnTheRepeatTargetThing();
    }





    public repeatWithTargetPicker returnTheGoToThingOfTypeXAndInteractWithTypeY(GameObject theObjectDoingTheEnactions, stuffType stuffX, interType interTypeX)
    {
        

        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));


        //targetPicker getter = new pickNextVisibleStuffStuff(theObjectDoingTheEnactions, stuffX);


        targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions,
            new setOfAllObjectThatMeetCriteria(new setOfAllNearbyStuffStuff(theObjectDoingTheEnactions, stuffX),
            new objectVisibleInFOV(theObjectDoingTheEnactions.transform)
            ));



        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions, getter.pickNext().realPositionOfTarget()), aimTargetPlan2(theObjectDoingTheEnactions, theObjectDoingTheEnactions), fireHitscan(theObjectDoingTheEnactions, interTypeX));
        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();
        //simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);
        //repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(theObjectDoingTheEnactions));
        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, getter);


        return repeatWithTargetPickerTest;
    }



    public repeatWithTargetPicker returnTheRepeatTargetThing(GameObject theObjectDoingTheEnactions, stuffType stuffX, interType interTypeX)
    {
        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));



        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions, theObjectDoingTheEnactions.transform.position), aimTargetPlan2(theObjectDoingTheEnactions, theObjectDoingTheEnactions), fireHitscan(theObjectDoingTheEnactions, interTypeX));
        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();
        //simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);
        //repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(theObjectDoingTheEnactions));


        targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions,
            new setOfAllObjectThatMeetCriteria(new setOfAllNearbyStuffStuff(theObjectDoingTheEnactions, stuffX),
            new objectVisibleInFOV(theObjectDoingTheEnactions.transform)
            ));

        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1,
            getter//new pickNextVisibleStuffStuff(theObjectDoingTheEnactions, theStuffTypeToGrab)
            );

        //                  repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new pickNextVisibleStuffStuff(theObjectDoingTheEnactions, stuffX));


        return repeatWithTargetPickerTest;
    }







    private planEXE2 goGrabPlan2(GameObject theObjectDoingTheEnactions, stuffType theStuffTypeX)
    {
        //ad-hoc hand-written plan
        GameObject target = repository2.singleton.randomTargetPickerObjectFromList(new setOfAllNearbyStuffStuff(theObjectDoingTheEnactions, theStuffTypeX).grab());


        Debug.Assert(target != null);

        if (target == null)
        {
            return null;
        }

        planEXE2 firstShell = new seriesEXE();
        firstShell.Add(genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions, target, 1.8f));
        firstShell.Add(aimTargetPlan2(theObjectDoingTheEnactions, target));




        hitscanEnactor theHitscanEnactor = grabHitscanEnaction(theObjectDoingTheEnactions, interType.standardClick); //hitscanClickPlan(interType.standardClick, target);

        Debug.Assert(theHitscanEnactor != null);
        planEXE2 hitscanEXE = theHitscanEnactor.standardEXEconversion();
        firstShell.Add(hitscanEXE);
        firstShell.untilListFinished();


        return firstShell;
    }

    public singleEXE fireHitscan(GameObject theObjectDoingTheEnactions, interType interTypeX)
    {

        hitscanEnactor theHitscanEnactor = grabHitscanEnaction(theObjectDoingTheEnactions, interTypeX); //hitscanClickPlan(interType.standardClick, target);

        singleEXE theSingle = (singleEXE)theHitscanEnactor.standardEXEconversion();
        theSingle.untilListFinished();

        return theSingle;
    }



    public List<GameObject> allNearbyObjectsWithStuffTypeX(GameObject theObjectDoingTheEnactions, stuffType theStuffTypeX)
    {

        List<GameObject> theListOfALL = new find().allObjectsInObjectsZone(theObjectDoingTheEnactions);  //lol forgot, this is ONE way to grab functions
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


    private vect3EXE2 aimTargetPlan2(GameObject theObjectDoingTheEnactions, GameObject target)
    {
        aimTarget testE1 = theObjectDoingTheEnactions.GetComponent<aimTarget>();

        vect3EXE2 exe1 = (vect3EXE2)testE1.toEXE(target);
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
    public FSM theFSM;

    void Start()
    {

        //Debug.Log("sssssssssssssssssssssssssssssssss");
    }

    void Update()
    {
        //plan.doTheDepletablePlan();
        //simpleRepeat1.doThisThing();
        //repeatWithTargetPickerTest.doThisThing();
        theFSM = theFSM.doAFrame();
    }


}








public class animalFSM: FSM
{
    //Dictionary<multicondition, animalFSM> switchBoard = new Dictionary<multicondition, animalFSM>();


    //planEXE2 repeatingPlan;
    //planEXE2 currentPlan;;

    //these are lists, AKA simultaneous plans
    //List<permaPlan> repeatingPlans = new List<permaPlan>();
    //List<repeater> repeatingPlans = new List<repeater>();
    //List<depletablePlan> currentPlans = new List<depletablePlan>();


    //repeatWithTargetPicker justDoThisForNow;


    public animalFSM()
    {
        //      new permaPlan();
    }

    public animalFSM(repeater doThisImmediately)
    {
        //justDoThisForNow = doThisImmediately;

        repeatingPlans.Add(doThisImmediately);

    }

    public animalFSM(GameObject theObjectDoingTheEnaction, singleEXE step1)
    {
        permaPlan2 perma1 = new permaPlan2(step1);
        agnostRepeater repeatWithTargetPickerTest = new agnostRepeater(perma1);
        repeatingPlans.Add(repeatWithTargetPickerTest);

    }



    agnostRepeater singleExeToRepeater(GameObject theObjectDoingTheEnaction, singleEXE step1)
    {
        permaPlan2 perma1 = new permaPlan2(step1);
        agnostRepeater repeatWithTargetPickerTest = new agnostRepeater(perma1);
        return repeatWithTargetPickerTest;
    }


    agnostRepeater singleExeToRepeater(singleEXE exe1, targetPicker getter)
    {
        permaPlan2 perma1 = new permaPlan2(exe1);
        agnostRepeater repeatWithTargetPickerTest = new agnostRepeater(perma1, getter);
        return repeatWithTargetPickerTest;
    }

    //single nav!  [should be standard conversion???????]
    singleEXE singleNav(GameObject theObjectDoingTheEnaction, Vector3 staticTargetPosition, float offsetRoom = 0f)
    {
        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = theObjectDoingTheEnaction.GetComponent<navAgent>();


        vect3EXE2 theEXE = new vect3EXE2(theNavAgent, staticTargetPosition);//placeholderTarget1);
                                                                            //theEXE.debugPrint = printThisNPC;
                                                                            //singleEXE theEXEsingle = theEXE;

        //proximity condition = new proximity(this.gameObject, staticTargetPosition, 2f);// offsetRoom * 1.4f);
        proximityRef condition = new proximityRef(theObjectDoingTheEnaction, theEXE, offsetRoom);// offsetRoom * 1.4f);
        //condition.debugPrint = theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        return theEXE;
    }

    repeatWithTargetPicker grabTheStuffgdtjtxdetjt(GameObject theObjectDoingTheEnaction, stuffType stuffX)
    {
        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));
        //perma1 = new permaPlan2(step1);

        //repeatWithTargetPicker otherBehavior = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(this.gameObject));


        return new ummAllThusStuffForGrab(theObjectDoingTheEnaction, stuffX).returnTheRepeatTargetThing();
    }






    public animalFSM(repeater doThisImmediately, condition switchCondition, repeater doThisAfterSwitchCondition)
    {
        //justDoThisForNow = doThisImmediately;

        repeatingPlans.Add(doThisImmediately);



        animalFSM otherFSM = new animalFSM(doThisAfterSwitchCondition, new reverseCondition(switchCondition), this);

        switchBoard[switchCondition] = otherFSM;
    }

    public animalFSM(repeater doThisImmediately, condition switchCondition, animalFSM doThisAfterSwitchCondition)
    {
        //justDoThisForNow = doThisImmediately;

        repeatingPlans.Add(doThisImmediately);


        //animalFSM otherFSM = new animalFSM(repeatWithTargetPicker2, switchCondition, repeatWithTargetPicker1);

        switchBoard[switchCondition] = doThisAfterSwitchCondition;
    }

    



    public void addSwitch(condition switchCondition, repeater doThisAfterSwitchCondition)
    {

        animalFSM otherFSM = new animalFSM(doThisAfterSwitchCondition);

        switchBoard[switchCondition] = otherFSM;
    }
    public void addSwitch(condition switchCondition, animalFSM otherFSM)
    {
        switchBoard[switchCondition] = otherFSM;
    }

    public void addSwitchAndReverse(condition switchCondition, repeater doThisAfterSwitchCondition)
    {

        animalFSM otherFSM = new animalFSM(doThisAfterSwitchCondition);

        switchBoard[switchCondition] = otherFSM;
        otherFSM.switchBoard[new reverseCondition(switchCondition)] = this;
    }
    public void addSwitchAndReverse(condition switchCondition, animalFSM otherFSM)
    {
        switchBoard[switchCondition] = otherFSM;
        otherFSM.switchBoard[new reverseCondition(switchCondition)] = this;
    }


    /*
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
        Debug.Log(repeatingPlans.Count.ToString());
        Debug.Log(repeatingPlans.Count);
        foreach (permaPlan plan in repeatingPlans)
        {
            newList.Add(plan.convertToDepletable());
        }

        return newList;
    }


    */

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


}




























//new combat dodge/fleeing behavior


//start fleeing condition(s)
//public class nonNullObjectWithAllCriteria : condition
//{
//nonononono, make a function that either returns the FIRST such object, or ALL of them,
//then pain with OTHER condition thing that simply looks at whether the output is null or not
//[any easy way to cache this set/object in case i want to use it later?

//objectSetGrabber theObjectSetGrabber;
//List<objectCriteria> theCriteria;

//public override bool met()
//{
//foreach(GameObject thisObject in theObjectSetGrabber.grab())
//  {

//    }

//  }

//}



//no!  split into evaluator for single object!  [and then just use setOfAllObjectThatMeetCriteria]

/*
public class allTargetsInSetThatAreThreats : objectSetGrabber
{
    //use tags for now?  later other rags for "teams"?  and [either before or after this step] exclude "self" from list?  
    objectSetGrabber theObjectSetGrabber;

    public override List<GameObject> grab()
    {
        return allInObjectListWithTagX(theObjectSetGrabber.grab(), tagging2.tag2.);
    }

    private List<GameObject> allInObjectListWithTagX(List<GameObject> theList, tagging2.tag2 theTag)
    {
        List<GameObject> newList = new List<GameObject>();

        foreach (GameObject thisObject in )
        {
            if (theCriteria.evaluateObject(thisObject) == false)
            {
                continue;
            }

            newList.Add(thisObject);
        }

        return newList;
    }
}

*/

//[and then also a "prox".......criteria.........








//enaction





/*
public class meleeDodge : repeatWithTargetPicker
{
    //simple radial pattern?  well, can have multiple different threats....

    objectSetGrabber theSetGrabber;


    public meleeDodge(GameObject theObjectDoingTheEnaction)
    {
        theTargetPicker = null;



        thePerma = new permaPlan2(
            genGen.singleton.makeNavAgentPlanEXE(
                theObjectDoingTheEnaction,
                theTargetPicker.pickNext().realPositionOfTarget()
                ));


        theDepletablePlan = convertToDepletableWithNextTarget();
    }




    /*

    public repeatWithTargetPicker returnTheGoToThing()
    {

        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));


        targetPicker getter = new pickNextVisibleStuffStuff(theObjectDoingTheEnactions, theStuffTypeToGrab);

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions, getter.pickNext().realPositionOfTarget()));
        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();
        //simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);
        //repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(theObjectDoingTheEnactions));
        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, getter);


        return repeatWithTargetPickerTest;
    }

    */

//}

//*/



public class radialFleeingTargetPicker : targetPicker
{
    //so...take a set of objects........AND the position of "flee-er"?
    //then.......weighted average the vectors running away from each object?
    //i think?
    //and i recently made that in "weightedRadialPattern()" in spatialDataPoint.

    objectSetGrabber theSet;
    GameObject theFleeer;


    public radialFleeingTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theSetInput)
    {
        theFleeer = theObjectDoingTheEnaction;
        theSet = theSetInput;
    }


    public override agnosticTargetCalc pickNext()
    {
        return new agnosticTargetCalc(weightedRadialFleeingPoint());
    }


    Vector3 weightedRadialFleeingPoint()
    {
        spatialDataPoint myData = new spatialDataPoint(theSet.grab(), theFleeer.transform.position);

        Vector3 newDirection = myData.weightedRadialPattern();
        //Debug.Log(newDirection - Vector3.zero);

        return theFleeer.transform.position + (newDirection.normalized*20);
    }
}

public class applePatternTargetPicker : targetPicker
{
    //so...take a set of objects........AND the position of "flee-er"?
    //then.......weighted average the vectors running away from each object?
    //i think?
    //and i recently made that in "weightedRadialPattern()" in spatialDataPoint.

    objectSetGrabber theSet;
    GameObject theFleeer;


    public applePatternTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theSetInput)
    {
        theFleeer = theObjectDoingTheEnaction;
        theSet = theSetInput;
    }


    public override agnosticTargetCalc pickNext()
    {
        return new agnosticTargetCalc(applePatternPoint());
    }


    Vector3 applePatternPoint()
    {
        spatialDataPoint myData = new spatialDataPoint(theSet.grab(), theFleeer.transform.position);

        Vector3 newDirection = myData.applePattern();
        //Debug.Log(newDirection - Vector3.zero);

        return theFleeer.transform.position + (newDirection * 20);
    }
}

public class nearestGoldilocksTargetPicker : targetPicker
{
    //so...take a set of objects........AND the position of "flee-er"?
    //then.......weighted average the vectors running away from each object?
    //i think?
    //and i recently made that in "weightedRadialPattern()" in spatialDataPoint.

    objectSetGrabber theSet;
    GameObject theObjectDoingThePicking;
    private float goldilocksRange;
    string myConstructorTrace;

    public nearestGoldilocksTargetPicker(GameObject theObjectDoingThePickingIn, objectSetGrabber theSetInput, float goldilocksRangeIn)
    {
        //Debug.Log("goldilocksRange:  " + goldilocksRange + "goldilocksRangeIn:  "+ goldilocksRangeIn);
        theObjectDoingThePicking = theObjectDoingThePickingIn;
        theSet = theSetInput;
        this.goldilocksRange = goldilocksRangeIn;

        myConstructorTrace = new System.Diagnostics.StackTrace(true).ToString();
        //Debug.Log("goldilocksRange:  " + goldilocksRange + "goldilocksRangeIn:  " + goldilocksRangeIn);

    }

    public override agnosticTargetCalc pickNext()
    {
        //Debug.Log("goldilocksRange:  " + goldilocksRange);

        return new agnosticTargetCalc(nearestGoldilocksPoint());
    }


    Vector3 nearestGoldilocksPoint()
    {
        /*
        spatialDataPointFragment thisFragment = new spatialDataPointFragment();
        {
            if (thisFragment.lineOfSightBool == false) { continue; }


            Vector3 fragmentVector = new Vector3();
            fragmentVector = thisFragment.simpleRadialPattern();

            float distance = thisFragment.distanceAsFloat;

            theOutputVector += fragmentVector.normalized / distance;

            return theFleeer.transform.position + (newDirection * 20);
    }
        */

        //Debug.Log("new nearestTargetPickerExceptSelf(theObjectDoingThePicking, theSet).pickNext().realPositionOfTarget()"+new nearestTargetPickerExceptSelf(
        //theObjectDoingThePicking, theSet).pickNext().realPositionOfTarget());
        //                  Debug.Log("goldilocksRange:  "+goldilocksRange + ", "+myConstructorTrace);
        Vector3 centerOfGoldilocksRadius = new nearestTargetPickerExceptSelf(theObjectDoingThePicking, theSet).pickNext().realPositionOfTarget();
        Vector3 currentLineFromTheGoldilocksCenterTargetAndTheTargeter = theObjectDoingThePicking.transform.position - centerOfGoldilocksRadius;
            //transform.position;

        Vector3 normalized = currentLineFromTheGoldilocksCenterTargetAndTheTargeter.normalized;

        //Debug.Log(",,,,,,,,,,,,,,,,,goldilocksRange:  " + goldilocksRange);
        //Debug.Log("normalized:  " + normalized);
        //Debug.Log("normalized*goldilocksRange:  " + normalized * goldilocksRange);
        return normalized * goldilocksRange + centerOfGoldilocksRadius;// theObjectDoingThePicking.transform.position;

    }

    /*
    Vector3 weightedRadialFleeingPoint()
    {
        spatialDataPoint myData = new spatialDataPoint(theSet.grab(), theFleeer.transform.position);

        Vector3 newDirection = myData.weightedRadialPattern();
        //Debug.Log(newDirection - Vector3.zero);

        return theFleeer.transform.position + (newDirection.normalized * 20);
    }
    */
}



//end condition






//[i'll make this later]:

//public class pigging : repeater
//{
//creatures can eat non-food items, trash, guns, whatever lol [have to kill them to retreive?
//or......induce vomiting???  other methods?   yank from their jaws like a puppy]





//}























public class ummAllThusStuffForGrab
{

    GameObject theObjectDoingTheEnactions;

    stuffType theStuffTypeToGrab;


    public ummAllThusStuffForGrab(GameObject theObjectDoingTheEnactionsIn, stuffType theStuffTypeToGrabIn)
    {
        theObjectDoingTheEnactions = theObjectDoingTheEnactionsIn;
        theStuffTypeToGrab = theStuffTypeToGrabIn;
    }




    public repeatWithTargetPicker returnTheGoToThing()
    {

        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));


        //targetPicker getter = new pickNextVisibleStuffStuff(theObjectDoingTheEnactions, theStuffTypeToGrab);
        targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions, 
            new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnactions), 
            new objectVisibleInFOV(theObjectDoingTheEnactions.transform)
            ));

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions, getter.pickNext().realPositionOfTarget()));
        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();
        //simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);
        //repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(theObjectDoingTheEnactions));
        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, getter);


        return repeatWithTargetPickerTest;
    }


    public repeatWithTargetPicker returnTheRepeatTargetThing()
    {
        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));



        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions, theObjectDoingTheEnactions.transform.position), aimTargetPlan2(theObjectDoingTheEnactions), fireHitscanClick());
        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();
        //simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);
        //repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(theObjectDoingTheEnactions));

        targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions,
            new setOfAllObjectThatMeetCriteria(new setOfAllNearbyStuffStuff(theObjectDoingTheEnactions, theStuffTypeToGrab),
            new objectVisibleInFOV(theObjectDoingTheEnactions.transform)
            ));

        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1,
            getter//new pickNextVisibleStuffStuff(theObjectDoingTheEnactions, theStuffTypeToGrab)
            );
        

        return repeatWithTargetPickerTest;
    }




    public planEXE2 replacementPlan()
    {

        planEXE2 zerothShell = new seriesEXE(goGrabPlan2(theStuffTypeToGrab));
        return zerothShell;
    }

    private planEXE2 goGrabPlan2(stuffType theStuffTypeX)
    {
        //ad-hoc hand-written plan
        GameObject target = repository2.singleton.randomTargetPickerObjectFromList(new setOfAllNearbyStuffStuff(theObjectDoingTheEnactions, theStuffTypeX).grab());


        Debug.Assert(target != null);

        if (target == null)
        {
            return null;
        }

        planEXE2 firstShell = new seriesEXE();
        firstShell.Add(genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions, target, 1.8f));
        firstShell.Add(aimTargetPlan2(target));




        hitscanEnactor theHitscanEnactor = grabHitscanEnaction(theObjectDoingTheEnactions, interType.standardClick); //hitscanClickPlan(interType.standardClick, target);

        Debug.Assert(theHitscanEnactor != null);
        planEXE2 hitscanEXE = theHitscanEnactor.standardEXEconversion();
        firstShell.Add(hitscanEXE);
        firstShell.untilListFinished();


        return firstShell;
    }

    public singleEXE fireHitscanClick()
    {

        hitscanEnactor theHitscanEnactor = grabHitscanEnaction(theObjectDoingTheEnactions, interType.standardClick); //hitscanClickPlan(interType.standardClick, target);

        singleEXE theSingle = (singleEXE)theHitscanEnactor.standardEXEconversion();
        theSingle.untilListFinished();

        return theSingle;
    }



    public List<GameObject> allNearbyObjectsWithStuffTypeX(GameObject theObjectDoingTheEnactions, stuffType theStuffTypeX)
    {

        List<GameObject> theListOfALL = new find().allObjectsInObjectsZone(theObjectDoingTheEnactions);  //lol forgot, this is ONE way to grab functions
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


    private singleEXE aimTargetPlan2(GameObject target)
    {
        aimTarget testE1 = theObjectDoingTheEnactions.GetComponent<aimTarget>();

        singleEXE exe1 = (singleEXE)testE1.toEXE(target);
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





}
















//repeaters [can make a non-repeating super class, and then it can replace my "seriesEXE"???

public abstract class repeater
{

    public permaPlan2 thePerma;
    public depletablePlan theDepletablePlan;

    public abstract void doThisThing();

    public abstract string baseEnactionsAsText();

    public abstract void refill();
}

public class agnostRepeater: repeater
{
    //agnostic as to whether target may change or not
    repeater theRepeater; //hmm, nesting makes problems for printing/debugging contents.....

    public agnostRepeater(permaPlan2 thePermaIn)
    {
        theRepeater = new simpleExactRepeatOfPerma(thePermaIn);
    }

    public agnostRepeater(permaPlan2 thePermaIn, targetPicker theTargetPickerIn)
    {
        theRepeater = new repeatWithTargetPicker(thePermaIn, theTargetPickerIn);
    }

    public override string baseEnactionsAsText()
    {
        return theRepeater.baseEnactionsAsText();
    }

    public override void doThisThing()
    {
        theRepeater.doThisThing();
    }

    public override void refill()
    {
        theRepeater.refill();
    }
}

public class repeatWithTargetPicker:repeater
{
    internal targetPicker theTargetPicker;


    public repeatWithTargetPicker(permaPlan2 thePermaIn, targetPicker theTargetPickerIn)
    {
        this.thePerma = thePermaIn;
        theTargetPicker = theTargetPickerIn;
        //theDepletablePlan = convertToDepletableWithNextTarget();
    }
    public repeatWithTargetPicker(singleEXE anEXE, targetPicker theTargetPickerIn)
    {
        this.thePerma = new permaPlan2(anEXE);
        theTargetPicker = theTargetPickerIn;
        //theDepletablePlan = convertToDepletableWithNextTarget();
    }

    public override void doThisThing()
    {
        //Debug.Log("======================================================================");
        refillIfNeeded();

        theDepletablePlan.doTheDepletablePlan();

        refillIfNeeded();
    }

    private void refillIfNeeded()
    {
        //Debug.Log("refillIfNeeded()");
        if (theDepletablePlan == null || theDepletablePlan.endConditionsMet())
        {
            //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!endConditionsMet() == true");
            refill();
            return;
        }

        //Debug.Log("NOT met.........");
    }

    public override void refill()
    {
        theDepletablePlan = convertToDepletableWithNextTarget();
    }




    internal depletablePlan convertToDepletableWithNextTarget()
    {
        depletablePlan newThing = new depletablePlan();
        agnosticTargetCalc newTarget = theTargetPicker.pickNext();

        //Debug.Log("newTarget:  " + newTarget);
        //Debug.Log("newTarget.targetPosition():  " + newTarget.targetPosition());
        //Debug.Log("newTarget.GetHashCode():  " + newTarget.GetHashCode());


        Debug.Assert(thePerma != null);
        Debug.Assert(thePerma.convertToDepletable() != null);
        Debug.Assert(thePerma.convertToDepletable().thePlan != null);
        //Debug.Log("thePerma.convertToDepletable().thePlan.Count:  " + thePerma.convertToDepletable().thePlan.Count);
        foreach (singleEXE thisOne in thePerma.convertToDepletable().thePlan)
        {
            //thisOne.theEnaction.targ//ohhhhhhhhh, not all enactions HAVE a target, i see.....how to handle....
            //Debug.Log("ooooooooooooooo????????????");
            thisOne.setTarget(newTarget);

            newThing.add(thisOne);
        }


        //Debug.Log("newThing.Count:  " + newThing.thePlan.Count);



        return newThing;
    }

    public override string baseEnactionsAsText()
    {
        string newString = "";
        //newString += "the base enactions of this FSM:  ";
        newString += "[";

        newString += thePerma.baseEnactionsAsText();

        newString += "]";
        //Debug.Log(newString);
        return newString;
    }
}

public class simpleExactRepeatOfPerma : repeater
{

    public simpleExactRepeatOfPerma(permaPlan2 thePermaIn)
    {
        this.thePerma = thePermaIn;
        theDepletablePlan = thePerma.convertToDepletable();
    }

    public override void doThisThing()
    {
        refillIfNeeded();

        theDepletablePlan.doTheDepletablePlan();

        refillIfNeeded();
    }

    private void refillIfNeeded()
    {
        if (theDepletablePlan.endConditionsMet())
        {
            refill();
        }
    }
    public override string baseEnactionsAsText()
    {
        string newString = "";
        //newString += "the base enactions of this FSM:  ";
        newString += "[";

        newString += thePerma.baseEnactionsAsText();

        newString += "]";
        //Debug.Log(newString);
        return newString;
    }


    public override void refill()
    {
        theDepletablePlan = thePerma.convertToDepletable();
    }


}

public class repeatWithObjectReturner : repeater
{
    internal individualObjectReturner theObjectReturner;


    public repeatWithObjectReturner(permaPlan2 thePermaIn, individualObjectReturner theObjectReturnerIn)
    {
        this.thePerma = thePermaIn;
        theObjectReturner = theObjectReturnerIn;
        theDepletablePlan = convertToDepletableWithNextTarget();
    }

    public override void doThisThing()
    {
        //Debug.Log("======================================================================");
        refillIfNeeded();

        theDepletablePlan.doTheDepletablePlan();

        refillIfNeeded();
    }

    private void refillIfNeeded()
    {
        //Debug.Log("refillIfNeeded()");
        if (theDepletablePlan.endConditionsMet())
        {
            //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!endConditionsMet() == true");
            refill();
            return;
        }

        //Debug.Log("NOT met.........");
    }


    public override void refill()
    {
        theDepletablePlan = convertToDepletableWithNextTarget();
    }




    internal depletablePlan convertToDepletableWithNextTarget()
    {
        depletablePlan newThing = new depletablePlan();
        //          agnosticTargetCalc newTarget = theTargetPicker.pickNext();

        //Debug.Log("newTarget:  " + newTarget);
        //Debug.Log("newTarget.targetPosition():  " + newTarget.targetPosition());
        //Debug.Log("newTarget.GetHashCode():  " + newTarget.GetHashCode());

        //Debug.Log("thePerma.convertToDepletable().thePlan.Count:  " + thePerma.convertToDepletable().thePlan.Count);

        foreach (singleEXE thisOne in thePerma.convertToDepletable().thePlan)
        {
            //thisOne.theEnaction.targ//ohhhhhhhhh, not all enactions HAVE a target, i see.....how to handle....
            //Debug.Log("ooooooooooooooo????????????");
           //            thisOne.setTarget(newTarget);

            newThing.add(thisOne);
        }


        //Debug.Log("newThing.Count:  " + newThing.thePlan.Count);



        return newThing;
    }



    public override string baseEnactionsAsText()
    {
        string newString = "";
        //newString += "the base enactions of this FSM:  ";
        newString += "[";

        newString += thePerma.baseEnactionsAsText();

        newString += "]";
        //Debug.Log(newString);
        return newString;
    }

}











//enactions

public class reproduce : IEnactaBool
{
    public override void enact(inputData theInput)
    {
        throw new System.NotImplementedException();
    }

    public override void enactJustThisIndividualEnaction(inputData theInput)
    {
        throw new NotImplementedException();
    }
}

public class meleeAttack : IEnactaBool
{
    public override void enact(inputData theInput)
    {
        throw new System.NotImplementedException();
    }

    public override void enactJustThisIndividualEnaction(inputData theInput)
    {
        throw new NotImplementedException();
    }
}

public class sleep : IEnactaBool
{
    public override void enact(inputData theInput)
    {
        throw new System.NotImplementedException();
    }

    public override void enactJustThisIndividualEnaction(inputData theInput)
    {
        throw new NotImplementedException();
    }
}







//"behaviors" [maybe scrap]

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
        return genGen.singleton.makeNavAgentPlanEXE(this.gameObject,patternScript2.singleton.randomNearbyVector(this.transform.position));
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
        GameObject target = repository2.singleton.randomTargetPickerObjectFromList(allNearbyEquippablesWithInterTypeX(interTypeX));

        if (target == null) { return null; }

        planEXE2 firstShell = new seriesEXE();
        firstShell.Add(genGen.singleton.makeNavAgentPlanEXE(this.gameObject,target, 1.8f));
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
        GameObject target = repository2.singleton.randomTargetPickerObjectFromList(allNearbyObjectsWithStuffTypeX(theStuffTypeX));


        Debug.Assert(target!=null);

        if (target == null) 
        {
            return null; 
        }

        planEXE2 firstShell = new seriesEXE();
        firstShell.Add(genGen.singleton.makeNavAgentPlanEXE(this.gameObject, target, 1.8f));
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