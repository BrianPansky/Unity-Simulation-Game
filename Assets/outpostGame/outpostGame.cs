using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.WSA;
using static enactionCreator;
using static interactionCreator;
using static tagging2;
using static UnityEngine.UI.GridLayoutGroup;

public class outpostGame : MonoBehaviour
{

    List<hoardeWaveGen> hoardes = new List<hoardeWaveGen>();



    // Start is called before the first frame update
    void Start()
    {
        genGen.singleton.returnGun1(new Vector3(-2, 0.7f, 6));

        new makeTestLeader(tag2.team2).doIt(new Vector3(430, 1, 30));
        new makeTestLeader(tag2.team3).doIt(new Vector3(-430, 1, -30));
        new makeTestLeader(tag2.team4).doIt(new Vector3(420, 1, 30));
        new makeTestLeader(tag2.team5).doIt(new Vector3(-430, 1, -20));
        /*
        new makeSimpleTestsToFixMyRTSStuff(tag2.team2).doIt(new Vector3(20,1,20));
        new makeSimpleTestsToFixMyRTSStuff(tag2.team3).doIt(new Vector3(-20, 1, -20));
        new makeTestBase(tag2.team2).doIt(new Vector3(25, 1, 25));
        new makeTestBase(tag2.team3).doIt(new Vector3(-25, 1, -25));
        new makeTestLeader(tag2.team2).doIt(new Vector3(30, 1, 30));
        new makeTestLeader(tag2.team3).doIt(new Vector3(-30, 1, -30));
        */
        //new makeTestBase(tag2.team2).doIt(new Vector3(25, 1, 25));
        /*
        new makeTestBase(tag2.team3).doIt(new Vector3(-25, 1, -25));

        new makeTestLeader(tag2.team2).doIt(new Vector3(30, 1, 30));
        new basicSoldierGeneratorG(tag2.team2).generate(); 
        new basicPaintByNumbersSoldierGeneratorG(tag2.team2, 1, 2.4f, 1, 4, 33).generate();

        List<Vector3> listOfSpawnPoints = new List<Vector3>();
        listOfSpawnPoints.Add(new Vector3(30,0,20));
        listOfSpawnPoints.Add(new Vector3(45,0,-20));
        listOfSpawnPoints.Add(new Vector3(-25,0,25));
        listOfSpawnPoints.Add(new Vector3(-30,0,-30));

        new oneTeamBaseGenAtPoint(new hoardeWaveGen1_1(tag2.team2, listOfSpawnPoints));
        */

        ///hoardes.Add(new hoardeWaveGen(tag2.team2, listOfSpawnPoints));
        //hoardes.Add(new hoardeWaveGen(tag2.team3, listOfSpawnPoints));
        //hoardes.Add(new hoardeWaveGen(tag2.team4, listOfSpawnPoints));


        /*

        int number = 0;
        while(number < 3)
        {

            new basicSoldierGenerator(tag2.team2).doIt(new Vector3(7, 0, 10 * number));
            new basicSoldierGenerator(tag2.team3).doIt(new Vector3(7, 0, -10 * number));
            number++;
        }
        */



        /*
        GameObject anim1 = new animalGen2(tag2.team2, 2, 1, 1, 1, 9, 24).generate();
        anim1.transform.position = new Vector3(-23, 0, 35);
        GameObject anim2 = new animalGen2(tag2.team2, 2, 1, 1, 1, 9, 24).generate();
        anim2.transform.position = new Vector3(-13, 0, 25);

        GameObject pred1 = new predatorGen2(tag2.team3, 2, 1.5f, 1.5f, 1.5f, 13, 74).generate();
        pred1.transform.position = new Vector3(23, 0, -15);


        returnBasicGrabbable(stuffType.fruit, new Vector3(-5, 2, 8));
        //returnBasicGrabbable(stuffType.meat1, new Vector3(-17, 2, 2));
        returnBasicGrabbable(stuffType.fruit, new Vector3(15, 2, -4));
        returnBasicGrabbable(stuffType.fruit, new Vector3(-15, 1, 8));
        //returnBasicGrabbable(stuffType.meat1, new Vector3(-17, 2, 2));
        returnBasicGrabbable(stuffType.fruit, new Vector3(25, 1, -14));
        returnBasicGrabbable(stuffType.fruit, new Vector3(-25, 1, 18));
        //returnBasicGrabbable(stuffType.meat1, new Vector3(-17, 2, 2));
        returnBasicGrabbable(stuffType.fruit, new Vector3(-15, 1, -24));
        
        */

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("---------------------------------------------------------");
        foreach (hoardeWaveGen hoard in hoardes)
        {
            hoard.doOnUpdate();
        }
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

}



public class makeTestLeader
{
    private tag2 team;

    public makeTestLeader(tag2 teamIn)
    {
        team = teamIn;
    }

    internal void doIt(Vector3 vector3)
    {
        GameObject newObj = new teamRankingOfficerGenerator(team, 2, 4,2,5,99).generate();
        newObj.transform.position = vector3;
    }
}


public class makeTestBase
{
    private tag2 team;

    public makeTestBase(tag2 teamIn)
    {
        team = teamIn;
    }

    internal void doIt(Vector3 thisPoint)
    {
        GameObject emptyObject = new GameObject();
        emptyObject.transform.position = thisPoint;


        //needs a zone, otherwise it won't ever be called in "callableUpdate"
        emptyObject.AddComponent<BoxCollider>();
        tagging2.singleton.addTag(emptyObject, tag2.zoneable);
        tagging2.singleton.addTag(emptyObject, tag2.militaryBase);
        tagging2.singleton.addTag(emptyObject, team);
    }
}






public class makeSimpleTestsToFixMyRTSStuff
{
    private tag2 team;

    public makeSimpleTestsToFixMyRTSStuff(tag2 teamIn)
    {
        this.team = teamIn;
    }

    public void doIt(Vector3 vector3)
    {
        //make simple block with a pre-filled RTS script, get it to move
        //then get the condition to gate that behavior
        //then find a way for it to work while initially empty, and then being filled [currently, LOTS of null object errprs!]

        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);
        GameObject torso = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);

        newObj.AddComponent<rtsModule>();
        //oldTorso(newObj,torso);

        newTorso(newObj, torso);

        genGen.singleton.ensureVirtualGamePad(newObj);


        tagging2.singleton.addTag(newObj, team);
        playable2 thePlayable = newObj.AddComponent<playable2>();
        thePlayable.theNavMeshTransform = newObj.transform;
        thePlayable.theHorizontalRotationTransform = torso.transform;
        thePlayable.initializeEnactionPoint1();
        thePlayable.theVerticalRotationTransform = thePlayable.enactionPoint1.transform;
        thePlayable.enactionPoint1.transform.SetParent(thePlayable.theHorizontalRotationTransform, true);

        thePlayable.dictOfIvariables[numericalVariable.health] = 2;
        thePlayable.equipperSlotsAndContents[simpleSlot.hands] = null;

        genGen.singleton.addArrowForward(thePlayable.enactionPoint1);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);


        genGen.singleton.makeEnactionsWithTorsoArticulation1(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);



        inventory1 theirInventory = newObj.AddComponent<inventory1>();
        //GameObject gun = weapon.generate();
        GameObject gun = genGen.singleton.returnGun1(newObj.transform.position);//new paintByNumbersGun1(1,40, 10,0.5f, true, 0, 0.3f, 0.2f,0.5f).generate();//
        theirInventory.inventoryItems.Add(gun);
        interactionCreator.singleton.dockXToY(gun, newObj);




        FSMcomponent theFSMcomponent = newObj.AddComponent<FSMcomponent>();
        theFSMcomponent.theFSMList = new testRTSFSM(newObj, team).returnIt();//new basicPaintByNumbersSoldierFSM(newObj, tag2.team3, 5, 33).returnIt();//theBehavior;


        //newObj.GetComponent<NavMeshAgent>().enabled = false;
        newObj.AddComponent<reactivationOfNavMeshAgent>();

        newObj.transform.position = vector3;
        //return newObj;

    }


    private void newTorso(GameObject newObj, GameObject torso)
    {
        //torso.transform.localScale = new Vector3(width, height / 2, width);


        //torso.transform.position += new Vector3(0, (height / 2) - 1, 0);
        torso.transform.position += new Vector3(0, 1f, 0);

        Renderer theRenderer = newObj.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];
        Renderer theRenderer2 = torso.GetComponent<Renderer>();
        //theRenderer2.material.color = tagging2.singleton.teamColors[team];

        GameObject.Destroy(newObj.GetComponent<Collider>());
        BoxCollider hitbox = newObj.AddComponent<BoxCollider>();
        //hitbox.size += new Vector3(width-1, height-1, width-1);
        //hitbox.center += new Vector3(0, (height-1)/2, 0);
        hitbox.size += new Vector3(0, 1f, 0);
        hitbox.center += new Vector3(0, 0.5f, 0);


        torso.transform.SetParent(newObj.transform, false);
        newObj.transform.localScale = new Vector3(1, (4 - 1) / 2, 1);
        //newObj.transform.position += new Vector3(0, ((height - 1) / 2), 0);
    }
}




public class testRTSFSM
{
    List<FSM> theList = new List<FSM>();
    GameObject theObjectDoingTheEnaction;
    tag2 team;

    public testRTSFSM(GameObject theObjectDoingTheEnactionIn, tag2 teamIn)
    {
        theObjectDoingTheEnaction = theObjectDoingTheEnactionIn;
        team = teamIn;
        theList.Add(theTestFSM());
    }

    private FSM theTestFSM()
    {
        //use a pre-filled RTS script, get it to move
        //then get the condition to gate that behavior
        //then find a way for it to work while initially empty, and then being filled [currently, LOTS of null object errprs!]

        return rtsFSM1(theObjectDoingTheEnaction, team);
    }

    internal List<FSM> returnIt()
    {
        return theList;
    }



    private FSM rtsFSM1(GameObject theObjectDoingTheEnaction, tag2 teamIn)
    {
        //FSM wander = new generateFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());

        targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        FSM goToRTSTarget = new generateFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

        //condition switchFromWanderToRTS = makeSwitchFromWanderToRTS(theObjectDoingTheEnaction);

        //wander.addSwitchAndReverse(switchFromWanderToRTS, goToRTSTarget);


        //wander.name = "feet, wander";
        goToRTSTarget.name = "feet, goToRTSTarget";
        return goToRTSTarget;
    }

    private targetPicker makeTheRTSCommandTargetPicker(GameObject theObjectDoingTheEnaction)
    {
        //bit messy ad-hoc sorta way to do this for now
        //need target picker that just copies from their RTS module lol

        //individualObjectReturner theObject = new presetObject(theObjectDoingTheEnaction);
        targetPicker theRTSCommandTargetPicker = new targetPickerFromRTSModule(theObjectDoingTheEnaction);
        return theRTSCommandTargetPicker;
    }

    private condition makeSwitchFromWanderToRTS(GameObject theObjectDoingTheEnaction)
    {
        //if there's more than zero orders
        //opposite of leader's condition

        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasRtsModule()
            //new reverseCriteria( new hasNoOrders())
            );

        //how to apply it to themselves?
        individualObjectReturner theObject = new presetObject(theObjectDoingTheEnaction);


        condition switchFromWanderToRTS = new individualObjectMeetsAllCriteria(theObject, theCriteria);

        return switchFromWanderToRTS;
    }

}



public class goToXFromTargetPicker
{
    repeatWithTargetPicker theRepeater;

    public goToXFromTargetPicker(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, float proximity)
    {
        theRepeater = thinggggg(theObjectDoingTheEnactions, thingXToGoTo, proximity);
    }

    public repeatWithTargetPicker returnIt()
    {
        return theRepeater;
    }

    private repeatWithTargetPicker thinggggg(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, float proximity)
    {

        //targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions,
        //    new setOfAllNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

        //Debug.Assert(genGen.singleton != null);
        //Debug.Assert(thingXToGoTo != null);
        //Debug.Assert(thingXToGoTo.pickNext() != null);
        //Debug.Assert(thingXToGoTo.pickNext().realPositionOfTarget() != null);

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(
                genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions,
                thingXToGoTo,
                //thingXToGoTo.pickNext().realPositionOfTarget(), //messy
                proximity)
                //new aimTargetPlanGen(theObjectDoingTheEnactions, thingXToGoTo).returnIt(), //messy
                //interactWithType(theObjectDoingTheEnactions, theIntertypeY)
                );

        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, thingXToGoTo);


        return repeatWithTargetPickerTest;
    }



}








public class teamRankingOfficerGenerator : objectGen
{
    tagging2.tag2 team;
    private Vector3 thePosition;

    float health;
    float height;
    float width;
    objectGen weapon;
    //List<FSM> theBehavior;  //gah!  needs the object as an input!
    float speed;
    float targetDetectionRange;


    //(tagging2.tag2 theTeamIn, float health, float speed, float height, float width, float targetDetectionRange, objectGen weapon, List<FSM> theBehavior )

    // 
    //basicBodyProperties
    public teamRankingOfficerGenerator(tag2 theTeamIn, float health, float height, float width, float speed, float targetDetectionRange)//, objectGen weapon)
    {
        this.team = theTeamIn;
        this.health = health;
        this.height = height;
        this.width = width;
        this.speed = speed;
    }

    public GameObject generate()
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);
        GameObject torso = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);

        //oldTorso(newObj,torso);

        newTorso(newObj, torso);

        genGen.singleton.ensureVirtualGamePad(newObj);

        tagging2.singleton.addTag(newObj, tag2.teamLeader);

        tagging2.singleton.addTag(newObj, team);
        playable2 thePlayable = newObj.AddComponent<playable2>();
        thePlayable.theNavMeshTransform = newObj.transform;
        thePlayable.theHorizontalRotationTransform = torso.transform;
        thePlayable.initializeEnactionPoint1();
        thePlayable.theVerticalRotationTransform = thePlayable.enactionPoint1.transform;
        thePlayable.enactionPoint1.transform.SetParent(thePlayable.theHorizontalRotationTransform, true);

        thePlayable.dictOfIvariables[numericalVariable.health] = health;
        thePlayable.equipperSlotsAndContents[simpleSlot.hands] = null;

        genGen.singleton.addArrowForward(thePlayable.enactionPoint1);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);


        genGen.singleton.makeEnactionsWithTorsoArticulation1(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);



        inventory1 theirInventory = newObj.AddComponent<inventory1>();
        //GameObject gun = weapon.generate();
        GameObject gun = genGen.singleton.returnGun1(newObj.transform.position);//new paintByNumbersGun1(1,40, 10,0.5f, true, 0, 0.3f, 0.2f,0.5f).generate();//
        theirInventory.inventoryItems.Add(gun);
        interactionCreator.singleton.dockXToY(gun, newObj);




        FSMcomponent theFSMcomponent = newObj.AddComponent<FSMcomponent>();
        theFSMcomponent.theFSMList = new teamRankingOfficerFSM(newObj, team, speed, targetDetectionRange).returnIt();//new basicPaintByNumbersSoldierFSM(newObj, team, speed, targetDetectionRange).returnIt();//theBehavior;


        //newObj.GetComponent<NavMeshAgent>().enabled = false;
        newObj.AddComponent<reactivationOfNavMeshAgent>();

        return newObj;
    }

    private void newTorso(GameObject newObj, GameObject torso)
    {
        //torso.transform.localScale = new Vector3(width, height / 2, width);


        //torso.transform.position += new Vector3(0, (height / 2) - 1, 0);
        torso.transform.position += new Vector3(0, 1f, 0);

        Renderer theRenderer = newObj.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];
        Renderer theRenderer2 = torso.GetComponent<Renderer>();
        //theRenderer2.material.color = tagging2.singleton.teamColors[team];

        GameObject.Destroy(newObj.GetComponent<Collider>());
        BoxCollider hitbox = newObj.AddComponent<BoxCollider>();
        //hitbox.size += new Vector3(width-1, height-1, width-1);
        //hitbox.center += new Vector3(0, (height-1)/2, 0);
        hitbox.size += new Vector3(0, 1f, 0);
        hitbox.center += new Vector3(0, 0.5f, 0);


        torso.transform.SetParent(newObj.transform, false);
        newObj.transform.localScale = new Vector3(width, (height - 1) / 2, width);
        //newObj.transform.position += new Vector3(0, ((height - 1) / 2), 0);
    }

    private void oldTorso(GameObject newObj, GameObject torso)
    {
        torso.transform.SetParent(newObj.transform, false);
        torso.transform.position += new Vector3(0, 1f, 0);


        Renderer theRenderer = newObj.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];

        GameObject.Destroy(newObj.GetComponent<Collider>());
        BoxCollider hitbox = newObj.AddComponent<BoxCollider>();
        hitbox.size += new Vector3(0, 1f, 0);
        hitbox.center += new Vector3(0, 0.5f, 0);
    }
}

public class teamRankingOfficerFSM : FSM
{

    //FSM theFSM;
    public List<FSM> theFSMList = new List<FSM>();

    //float combatRange = 40f;
    private GameObject newObj;
    private tag2 team;
    private float speed;
    private float targetDetectionRange;

    public teamRankingOfficerFSM(GameObject theObjectDoingTheEnaction, tag2 team, float speed, float targetDetectionRange = 40f)
    {
        this.newObj = theObjectDoingTheEnaction;
        this.team = team;
        this.speed = speed;
        this.targetDetectionRange = targetDetectionRange;


        //theFSMList.Add(feetFSM(theObjectDoingTheEnaction, team));
        //theFSMList.Add(handsFSM(theObjectDoingTheEnaction, team));
        theFSMList.Add(giveTeamCommandsFSM(theObjectDoingTheEnaction, team));

    }

    private FSM giveTeamCommandsFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        objectSetGrabber theEnemyBaseObjectSet = new setOfAllObjectThatMeetCriteria(
            new setOfAllObjectsWithTag(tag2.militaryBase), enemyBaseCriteria(theObjectDoingTheEnaction, team));

        //condition is any time there are units with no orders? [and units who complete orders should thus blank out their orders?]
        objectCriteria hasNoOrders = unitWithNoOrdersCriteria();

        condition conditionToGiveOrders = new isThereAtLeastOneObjectInSet(new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), hasNoOrders));



        //So:
        //      1)  pick a random enemy base
        //      2)  make the finite state for feet that goes to that base
        //      3)  give that state to all of the soldiers with no orders
        //      4)  then I need to make the soldiers have the state that allows them to follow
        //    orders if there are any orders and if they are in an idle state.
        //    and make transfer out of that order following state if there is combat threats.

        //objectSetGrabber allEnemyBasesGrabber = allEnemyBases();
        agnosticTargetCalc randomEnemyBase = new randomTargetPicker(allEnemyBases()).pickNext();

        //FSM feetGoToTheTarget = new generateFSM(new goToX(newObj, hihgigigiygiyTargetPicker(), 10000).returnIt());


        //no, i'll give them just targets for now, not FSMs
        //so, need leader to have a repeater and/or FSM that inputs SOMETHING into soldiers' rts modules
        var fakeRepeater = new giveXRTSTargetsToYUnits(null, new randomTargetPicker(allEnemyBases()));
        objectSetGrabber allUnitsWithNoOrders = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), hasNoOrders);

        fakeRepeater.MOREINFOgiveXRTSTargetsToYUnits(allUnitsWithNoOrders);
        FSM giveOrders = new generateFSM(fakeRepeater);//new goToX(newObj, hihgigigiygiyTargetPicker(), 10000).returnIt());

        return giveOrders;
    }

    private objectSetGrabber allEnemyBases()
    {

        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new reverseCriteria( new objectHasTag(team))
            );

        objectSetGrabber allEnemyBasesGrabber = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(tag2.militaryBase), theCriteria);

        return allEnemyBasesGrabber;
    }

    private objectCriteria unitWithNoOrdersCriteria()
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new reverseCriteria( new objectHasTag(tag2.teamLeader)),
            new hasRtsModule(),
            new hasNoOrders()
            );


        return theCriteria;
    }

    private objectCriteria enemyBaseCriteria(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            //new objectHasTag(tag2.militaryBase),
            new reverseCriteria(new objectHasTag(team))
            );

        return theCriteria;
    }

    private targetPicker enemyBaseTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theEnemyBaseObjectSet)
    {

        targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theEnemyBaseObjectSet);

        return theAttackTargetPicker;
    }






    private FSM handsFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        FSM idle = new generateFSM();

        objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction, team);
        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 110);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        FSM combat1 = new generateFSM(new aimAtXAndPressY(theObjectDoingTheEnaction, theAttackTargetPicker, buttonCategories.primary, targetDetectionRange).returnIt());//new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, targetDetectionRange).returnIt());



        idle.addSwitchAndReverse(switchToAttack, combat1);



        equipItemFSM equipGun = new equipItemFSM(theObjectDoingTheEnaction, interType.peircing);

        idle.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theFSM);
        //wander.addSwitchAndReverse(switchToAttack, equipGun.theFSM);
        combat1.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theFSM);//messy



        idle.name = "hands, idle";
        combat1.name = "hands, combat1";
        equipGun.theFSM.name = "hands, equipGun";
        return idle; ;
    }

    private objectCriteria createAttackCriteria(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new stickyTrueCriteria(new lineOfSight(theObjectDoingTheEnaction), 200),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 25)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private targetPicker generateAttackTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theAttackObjectSet)
    {

        targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        return theAttackTargetPicker;
    }

    private FSM feetFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        FSM wander = new generateFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());




        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new lineOfSight(theObjectDoingTheEnaction),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 25)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);

        //targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        //targetPicker theTargetPicker = new applePatternTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);
        targetPicker theTargetPicker = new combatDodgeVarietyPack1TargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        FSM combat1 = new generateFSM(new goToX(theObjectDoingTheEnaction, theTargetPicker, targetDetectionRange).returnIt());

        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        wander.addSwitchAndReverse(switchToAttack, combat1);

        wander.name = "feet, wander";
        combat1.name = "feet, combat1";

        navAgent theNav = theObjectDoingTheEnaction.GetComponent<navAgent>();
        theNav.theAgent.speed = this.speed;
        theNav.theAgent.baseOffset = 0.5f;//theObjectDoingTheEnaction.transform.localScale.y;
        //theNav.theAgent.

        return wander;
    }




    public List<FSM> returnIt()
    {
        return theFSMList;
    }



}





public class rtsModule : MonoBehaviour
{
    //public FSM currentOrdersToGive;
    //FSM currentReceivedOrders;  //what about multiple missions from multiple sources?  list?
    //internal Dictionary<objectIdPair, FSM> currentReceivedOrdersAndWhoGaveThem = new Dictionary<objectIdPair, FSM>();  //important to know who gave orders!  in case it's from wrong chain of command, or an ENEMY, or there's a mutiny and someone BECOMES an enemy
    //too complex for now.  start simple.
    //internal Dictionary<condition, FSM> currentReceivedOrders = new Dictionary<condition, FSM>();
    //List<GameObject> currentlySelectedUnits; //hmm, but will glitch when they die........

    //just do targets for now, not FSM
    public agnosticTargetCalc currentOrdersToGive;
    //FSM currentReceivedOrders;  //what about multiple missions from multiple sources?  list?
    internal Dictionary<objectIdPair, agnosticTargetCalc> currentReceivedOrdersAndWhoGaveThem = new Dictionary<objectIdPair, agnosticTargetCalc>();  //important to know who gave orders!  in case it's from wrong chain of command, or an ENEMY, or there's a mutiny and someone BECOMES an enemy


    public List<objectIdPair> currentlySelectedUnits; //annoying, but what else to do...
    public rtsModuleVersion theVersion;
    internal agnosticTargetCalc currentReceivedOrders = null;

    public static rtsModule ensureObjectHasThisComponent(GameObject theObject, rtsModuleVersion theVersion)
    {
        rtsModule theRTSModule = theObject.GetComponent<rtsModule>();
        if(theRTSModule == null)
        {
            theRTSModule = theObject.AddComponent<rtsModule>();
        }

        return theRTSModule;
    }

    internal void giveCurrentOrdersToCurrentlySelectedUnits()
    {
        theVersion.giveCurrentOrdersToCurrentlySelectedUnits(this);
    }


    internal void basicImplementation()
    {
        foreach(objectIdPair thisID in currentlySelectedUnits)
        {
            if(thisID.theObject == null) { continue; }
            rtsModule theirRTSModule = thisID.theObject.GetComponent<rtsModule>();
            theirRTSModule.currentReceivedOrdersAndWhoGaveThem[tagging2.singleton.idPairGrabify(this.gameObject)] = currentOrdersToGive;
        }
    }

    /*
    public agnostRepeater translateOrdersIntoRepeaterPlan()
    {

    }
    */
}

public abstract class rtsModuleVersion
{
    internal abstract void giveCurrentOrdersToCurrentlySelectedUnits(rtsModule rtsModule);
}

public class playerVersion : rtsModuleVersion
{
    objectSetGrabber theDefaultSelectionSet;  //buuut........
    internal override void giveCurrentOrdersToCurrentlySelectedUnits(rtsModule rtsModule)
    {
        rtsModule.basicImplementation();


        resetCurrentOrdersToGive(rtsModule);
        resetCurrentlySelectedUnits(rtsModule);
    }

    
    private void resetCurrentOrdersToGive(rtsModule rtsModule)
    {
        //make the orders be "go to player's waypoint1"
        //and then have a bundled...enaction, enaction Effect?...that moves
        //the player's waypoint1 to wherever they are pointing at that enaction moment.  should move it FIRST, really, to be sure....
    }

    private void resetCurrentlySelectedUnits(rtsModule rtsModule)
    {
        List<objectIdPair> unitList = tagging2.singleton.listInIDPairFormat(theDefaultSelectionSet.grab());
        rtsModule.currentlySelectedUnits =unitList;
    }
    
}

public class npcVersion : rtsModuleVersion
{
    internal override void giveCurrentOrdersToCurrentlySelectedUnits(rtsModule rtsModule)
    {
        rtsModule.basicImplementation();
    }
}




public class oneTeamBaseGenAtPoint : doAtPoint
{
    hoardeWaveGen1_1 teamBaseGen;

    public oneTeamBaseGenAtPoint(hoardeWaveGen1_1 teamBaseGenIn)
    {
        teamBaseGen = teamBaseGenIn;
    }

    internal override void doIt(Vector3 thisPoint)
    {
        GameObject emptyObject = new GameObject();
        emptyObject.transform.position = thisPoint;

        hoardUpdater1_1 theUpdater = emptyObject.AddComponent<hoardUpdater1_1>();


        List<Vector3> listOfSpawnPoints = new List<Vector3>();
        listOfSpawnPoints.Add(new Vector3(5, 0, 5) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(5, 0, -5) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(-5, 0, 5) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(-5, 0, -5) + emptyObject.transform.position);
        

        theUpdater.hoardes.Add(teamBaseGen);
        //theUpdater.hoardes.Add(new hoardeWaveGen(tag2.team8, listOfSpawnPoints, emptyObject, new factionEDYEOUF(tag2.team8).returnWaves())); //messy messy "tag2.team5" stuff TWICE



        //needs a zone, otherwise it won't ever be called in "callableUpdate"
        emptyObject.AddComponent<BoxCollider>();
        tagging2.singleton.addTag(emptyObject, tagging2.tag2.zoneable);
        tagging2.singleton.addTag(emptyObject, tagging2.tag2.militaryBase);

        teamBaseGen.theBaseGeneratorObject = emptyObject;
    }
}






public class makeHoardeGenAtPoint : doAtPoint
{
    private float xScale;
    private float yScale;
    private float zScale;
    Vector3 zoneOffset;

    public makeHoardeGenAtPoint(float xScale, float yScale, float zScale, Vector3 zoneOffsetIn = new Vector3())
    {
        zoneOffset = zoneOffsetIn;
        this.xScale = xScale;
        this.yScale = yScale;
        this.zScale = zScale;
    }

    internal override void doIt(Vector3 thisPoint)
    {
        /*
        GameObject newObj = repository2.Instantiate(repository2.singleton.invisibleCubePrefab, thisPoint, Quaternion.identity);
        newObj.transform.localScale = new Vector3(xScale, yScale, zScale);

        Collider theCollider = newObj.GetComponent<Collider>();
        theCollider.isTrigger = true;

        Rigidbody rigidbody = newObj.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;


        newObj.AddComponent<mapZoneScript>();
        */

        //empty object
        //set map zone position plus offset
        //add hoard updater
        //add hoard gen to BE updated
        //connect zone number to the "no more soldiers" condition, for testing

        GameObject emptyObject = new GameObject();
        emptyObject.transform.position = thisPoint + zoneOffset;

        hoardUpdater theUpdater = emptyObject.AddComponent<hoardUpdater>();


        List<Vector3> listOfSpawnPoints = new List<Vector3>();
        listOfSpawnPoints.Add(new Vector3(5, 0, 5) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(5, 0, -5) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(-5, 0, 5) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(-5, 0, -5) + emptyObject.transform.position);
        /*
        listOfSpawnPoints.Add(new Vector3(30, 0, 20) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(45, 0, -20) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(-45, 0, 15) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(-30, 0, -30) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(-25, 0, 25) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(-10, 0, -50) + emptyObject.transform.position);
        

        */

        //listOfSpawnPoints.Add(new Vector3(0, 0, 0) + emptyObject.transform.position);
        /*
        listOfSpawnPoints.Add(new Vector3(5, 0, 5) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(5, 0, -5) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(-5, 0, 5) + emptyObject.transform.position);
        listOfSpawnPoints.Add(new Vector3(-5, 0, -5) + emptyObject.transform.position);
        */

        theUpdater.hoardes.Add(new hoardeWaveGen(tag2.team2, listOfSpawnPoints, emptyObject));
        //theUpdater.hoardes.Add(new hoardeWaveGen(tag2.team3, listOfSpawnPoints, emptyObject));
        //theUpdater.hoardes.Add(new hoardeWaveGen(tag2.team4, listOfSpawnPoints, emptyObject));
        theUpdater.hoardes.Add(new hoardeWaveGen(tag2.team5, listOfSpawnPoints, emptyObject, new factionEDYEOUF(tag2.team5).returnWaves()));
        //theUpdater.hoardes.Add(new hoardeWaveGen(tag2.team6, listOfSpawnPoints, emptyObject, new factionHNCURF(tag2.team6).returnWaves()));
        //theUpdater.hoardes.Add(new hoardeWaveGen(tag2.team7, listOfSpawnPoints, emptyObject, new factionJKLHYGFIEG(tag2.team7).returnWaves()));
        //theUpdater.hoardes.Add(new hoardeWaveGen(tag2.team8, listOfSpawnPoints, emptyObject, new factionEDYEOUF(tag2.team8).returnWaves())); //messy messy "tag2.team5" stuff TWICE


        emptyObject.AddComponent<BoxCollider>();
        tagging2.singleton.addTag(emptyObject, tagging2.tag2.zoneable);
        //tagging2.singleton.setObjectAsMemberOfZone(emptyObject, );

    }
}


public class hoardUpdater : MonoBehaviour, IupdateCallable
{
    public List<IupdateCallable> currentUpdateList { get; set; }
    public List<hoardeWaveGen> hoardes = new List<hoardeWaveGen>();


    void Start()
    {
        //Debug.Log("hoardUpdater | zone:  " + tagging2.singleton.whichZone(this.gameObject) + ", and id number:  " + this.gameObject.GetHashCode());

        //messy [using collider to add this generator to a zone]
        Destroy(this.gameObject.GetComponent<BoxCollider>());//.enabled = false;
    }

    public void callableUpdate()
    {
        foreach (hoardeWaveGen hoard in hoardes)
        {
            hoard.doOnUpdate();
        }
    }

    /*
    public void Update()
    {
        int zone = tagging2.singleton.whichZone(this.gameObject);
        Debug.Log("hoarde zone:  " + zone);
        //Debug.Log("hoardes.Count" + hoardes.Count);
        foreach (hoardeWaveGen hoard in hoardes)
        {
            hoard.doOnUpdate();
        }
    }
    */
}


public class hoardUpdater1_1 : MonoBehaviour, IupdateCallable
{
    public List<IupdateCallable> currentUpdateList { get; set; }
    public List<hoardeWaveGen1_1> hoardes = new List<hoardeWaveGen1_1>();


    void Start()
    {
        //Debug.Log("hoardUpdater | zone:  " + tagging2.singleton.whichZone(this.gameObject) + ", and id number:  " + this.gameObject.GetHashCode());

        //messy [using collider to add this generator to a zone]
        Destroy(this.gameObject.GetComponent<BoxCollider>());//.enabled = false;
    }

    public void callableUpdate()
    {
        //Debug.Log("hoardUpdater | zone:  " + tagging2.singleton.whichZone(this.gameObject) + ", and id number:  " + this.gameObject.GetHashCode());

        foreach (hoardeWaveGen1_1 hoard in hoardes)
        {
            hoard.doOnUpdate();
        }
    }
}















public class hoardeWaveGen
{
    int currentWaveNumber = 0;

    tag2 team;

    condition newWaveCondition;
    GameObject thePlaceholderObjectInZone;

    List<objectSetInstantiator> listOfWaves = new List<objectSetInstantiator>();

    List<Vector3> listOfSpawnPoints = new List<Vector3>();



    public hoardeWaveGen(tag2 teamIn, List<Vector3> listOfSpawnPointsIn, GameObject thePlaceholderObjectInZoneIn)
    {
        this.thePlaceholderObjectInZone = thePlaceholderObjectInZoneIn;
        team = teamIn;
        listOfSpawnPoints = listOfSpawnPointsIn;

        newWaveCondition = zeroRemainingTeamMembersAnywhere();
        listOfWaves = testWaves();
    }
    public hoardeWaveGen(tag2 teamIn, List<Vector3> listOfSpawnPointsIn, GameObject thePlaceholderObjectInZoneIn, List<objectSetInstantiator> wavesIn)
    {
        this.thePlaceholderObjectInZone = thePlaceholderObjectInZoneIn;
        team = teamIn;
        listOfSpawnPoints = listOfSpawnPointsIn;

        newWaveCondition = zeroRemainingTeamMembersAnywhere();
        listOfWaves = wavesIn;
    }

    private List<objectSetInstantiator> testWaves()
    {
        List<objectSetInstantiator> newList = new List<objectSetInstantiator>();


        newList.Add(new objectSetInstantiator(new objectGen[] { new basicSoldierGeneratorG(team) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33), new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33), new basicPaintByNumbersSoldierGeneratorG(team, 2, 4, 1.3f, 13, 99) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.5f, 1.9f, 9, 33), new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 3, 3.8f, 2.4f, 2, 99), new basicPaintByNumbersSoldierGeneratorG(team, 2, 4, 1.3f, 13, 99), new basicSoldierGeneratorG(team), new basicPaintByNumbersSoldierGeneratorG(team, 2, 4, 1.3f, 13, 99), new basicSoldierGeneratorG(team) }));



        return newList;
    }




    public void doOnUpdate()
    {
        //Debug.Log("???????????????????????????????????????????");

        //Debug.Log("newWaveCondition" + newWaveCondition);
        //Debug.Log("newWaveCondition.asTextAllTheWayDown()" + newWaveCondition.asTextAllTheWayDown());
        if (newWaveCondition.met())
        {
            //Debug.Log("SAYS IT'S MET???????????????????????????????????????????");
            currentWaveNumber++;
            generateNextWave();
        }
        else
        {

            //Debug.Log("NOT MET1111111");
        }
    }









    public void generateNextWave()
    {
        /*
        //has reposition error in far zones:
        GameObject thing1 = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.placeHolderCubePrefab, new Vector3());
        //would have NO reposition error in far zones:
        //GameObject thing1 = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.placeHolderCubePrefab,thePlaceholderObjectInZone.transform.position);
        thing1.AddComponent<NavMeshAgent>();

        thing1.transform.position = thePlaceholderObjectInZone.transform.position;
        */






        //GameObject thing1 = new TESTsoldierGenerator(team, thePlaceholderObjectInZone.transform.position).generate();
        //GameObject thing1 = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.placeHolderCubePrefab,
        //thePlaceholderObjectInZone.transform.position);
        //thing1.AddComponent<NavMeshAgent>();

        //thing1.transform.position = thePlaceholderObjectInZone.transform.position;
        //Debug.Log("spawnPoint:  "+spawnPoint);
        //newObj.transform.position = spawnPoint;
        //newObj.transform.position = patternScript2.singleton.randomNearbyVector(thePlaceholderObjectInZone.transform.position);



        //new objectSetInstantiator(new objectGen[] { new basicSoldierGeneratorG(team) }).generate(thePlaceholderObjectInZone.transform.position);

        /*
        GameObject thing1= genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.placeHolderCylinderPrefab, thePlaceholderObjectInZone.transform.position);
        thing1.AddComponent<NavMeshAgent>();
        thing1.AddComponent<navAgent>();

        Debug.Log("thePlaceholderObjectInZone.transform.position:  " + thePlaceholderObjectInZone.transform.position);
        Debug.Log("current and count:  " + currentWaveNumber + ", " + listOfWaves.Count);
        */
        //if (currentWaveNumber > -1) { return; }
        if (listOfWaves.Count == 0)
        {
            Debug.Log("(listOfWaves.Count == 0)  for this team:  " + team);
            return;
        }

        if (currentWaveNumber - 1 < listOfWaves.Count)
        {

            listOfWaves[currentWaveNumber - 1].generate(thePlaceholderObjectInZone.transform.position);
            return;
        }

        listOfWaves[currentWaveNumber - listOfWaves.Count - 1].generate(thePlaceholderObjectInZone.transform.position);
        listOfWaves[currentWaveNumber - listOfWaves.Count].generate(thePlaceholderObjectInZone.transform.position);

        /*
        //currentWaveNumber starts at 1
        Debug.Log("current and count:  "+ currentWaveNumber+", "+ listOfWaves.Count);
        if(currentWaveNumber-1 < listOfWaves.Count)
        {

            listOfWaves[currentWaveNumber -1].generate(randomTargetPickerSpawnPoint(listOfSpawnPoints));
            return;
        }

        listOfWaves[currentWaveNumber - listOfWaves.Count-1].generate(randomTargetPickerSpawnPoint(listOfSpawnPoints));
        listOfWaves[currentWaveNumber - listOfWaves.Count].generate(randomTargetPickerSpawnPoint(listOfSpawnPoints));

        */
    }

    private Vector3 randomTargetPickerSpawnPoint(List<Vector3> listOfSpawnPoints)
    {
        return listOfSpawnPoints[repository2.singleton.randomTargetPickerInteger(listOfSpawnPoints.Count - 1)];
    }




    condition zeroRemainingTeamMembersAnywhere()
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad()
            //new objectHasTag(team)
            //new objectHasTag(team)
            //new proximityCriteriaBool(thePlayer?, 25)
            );



        //Debug.Log("CONDITION MAKER | zone:  "+tagging2.singleton.whichZone(thePlaceholderObjectInZone)+", and id number:  " + thePlaceholderObjectInZone.GetHashCode());

        objectSetGrabber theTeamObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), theCriteria);

        condition theCondition = new reverseCondition(new stickyCondition(new isThereAtLeastOneObjectInSet(theTeamObjectSet), 0));// theObjectDoingTheEnaction, numericalVariable.health);

        return theCondition;
    }
    condition zeroRemainingTeamMembersInZone()
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new objectHasTag(team)
            //new objectHasTag(team)
            //new proximityCriteriaBool(thePlayer?, 25)
            );



        //Debug.Log("CONDITION MAKER | zone:  "+tagging2.singleton.whichZone(thePlaceholderObjectInZone)+", and id number:  " + thePlaceholderObjectInZone.GetHashCode());

        objectSetGrabber theTeamObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(thePlaceholderObjectInZone), theCriteria);

        condition theCondition = new reverseCondition(new stickyCondition(new isThereAtLeastOneObjectInSet(theTeamObjectSet), 0));// theObjectDoingTheEnaction, numericalVariable.health);

        return theCondition;
    }
}



public class hoardeWaveGen1_1
{
    int currentWaveNumber = 0;

    tag2 team;

    condition newWaveCondition;

    List<objectSetInstantiator> listOfWaves = new List<objectSetInstantiator>();

    List<Vector3> listOfSpawnPoints = new List<Vector3>();

    public GameObject theBaseGeneratorObject;


    public hoardeWaveGen1_1(tag2 teamIn, List<Vector3> listOfSpawnPointsIn)
    {
        team = teamIn;
        listOfSpawnPoints = listOfSpawnPointsIn;

        newWaveCondition = zeroRemainingTeamMembersAnywhere();
        listOfWaves = testWaves();

        //Debug.Log("listOfSpawnPoints.Count:  " + listOfSpawnPoints.Count);
    }
    public hoardeWaveGen1_1(tag2 teamIn, List<Vector3> listOfSpawnPointsIn, List<objectSetInstantiator> wavesIn)
    {
        team = teamIn;
        listOfSpawnPoints = listOfSpawnPointsIn;

        newWaveCondition = zeroRemainingTeamMembersAnywhere();
        listOfWaves = wavesIn;

        //Debug.Log("listOfSpawnPoints.Count:  " + listOfSpawnPoints.Count);
    }

    private List<objectSetInstantiator> testWaves()
    {
        List<objectSetInstantiator> newList = new List<objectSetInstantiator>();

        newList.Add(new objectSetInstantiator(new objectGen[] { new basicSoldierGeneratorG(team) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33), new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33), new basicPaintByNumbersSoldierGeneratorG(team, 2, 4, 1.3f, 13, 99) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.5f, 1.9f, 9, 33), new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 3, 3.8f, 2.4f, 2, 99), new basicPaintByNumbersSoldierGeneratorG(team, 2, 4, 1.3f, 13, 99), new basicSoldierGeneratorG(team), new basicPaintByNumbersSoldierGeneratorG(team, 2, 4, 1.3f, 13, 99), new basicSoldierGeneratorG(team) }));
        


        return newList;
    }




    public void doOnUpdate()
    {
        //Debug.Log("???????????????????????????????????????????");

        //Debug.Log("newWaveCondition" + newWaveCondition);
        //Debug.Log("newWaveCondition.asTextAllTheWayDown()" + newWaveCondition.asTextAllTheWayDown());
        if (newWaveCondition.met())
        {
            //Debug.Log("SAYS IT'S MET???????????????????????????????????????????");
            currentWaveNumber++;
            generateNextWave();
        }
        else
        {

            //Debug.Log("NOT MET1111111");
        }
    }





    /*
    public targetPicker pickEnemyTargetToAttack()
    {
        objectCriteria
    }
    */

    public void generateNextWave()
    {
        if(listOfWaves.Count == 0)
        {
            Debug.Log("(listOfWaves.Count == 0)  for this team:  " + team);
            return;
        }

        if (currentWaveNumber - 1 < listOfWaves.Count)
        {

            listOfWaves[currentWaveNumber - 1].generate(theBaseGeneratorObject.transform.position + randomTargetPickerSpawnPoint(listOfSpawnPoints));
            return;
        }

        listOfWaves[currentWaveNumber - listOfWaves.Count - 1].generate(theBaseGeneratorObject.transform.position + randomTargetPickerSpawnPoint(listOfSpawnPoints));
        listOfWaves[currentWaveNumber - listOfWaves.Count].generate(theBaseGeneratorObject.transform.position + randomTargetPickerSpawnPoint(listOfSpawnPoints));

        /*
        //currentWaveNumber starts at 1
        Debug.Log("current and count:  "+ currentWaveNumber+", "+ listOfWaves.Count);
        if(currentWaveNumber-1 < listOfWaves.Count)
        {

            listOfWaves[currentWaveNumber -1].generate(randomTargetPickerSpawnPoint(listOfSpawnPoints));
            return;
        }

        listOfWaves[currentWaveNumber - listOfWaves.Count-1].generate(randomTargetPickerSpawnPoint(listOfSpawnPoints));
        listOfWaves[currentWaveNumber - listOfWaves.Count].generate(randomTargetPickerSpawnPoint(listOfSpawnPoints));

        */
    }

    private Vector3 randomTargetPickerSpawnPoint(List<Vector3> listOfSpawnPoints)
    {
        //Debug.Log("listOfSpawnPoints.Count:  "+listOfSpawnPoints.Count);
        return listOfSpawnPoints[repository2.singleton.randomTargetPickerInteger(listOfSpawnPoints.Count-1)];
    }




    condition zeroRemainingTeamMembersAnywhere()
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria( new objectHasTag(tag2.teamLeader))
            //new objectHasTag(team)
            //new proximityCriteriaBool(thePlayer?, 25)
            );



        //Debug.Log("CONDITION MAKER | zone:  "+tagging2.singleton.whichZone(thePlaceholderObjectInZone)+", and id number:  " + thePlaceholderObjectInZone.GetHashCode());

        objectSetGrabber theTeamObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), theCriteria);

        condition theCondition = new reverseCondition(new stickyCondition(new isThereAtLeastOneObjectInSet(theTeamObjectSet), 0));// theObjectDoingTheEnaction, numericalVariable.health);

        return theCondition;
    }
    
    /*
    condition zeroRemainingTeamMembersInZone()
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new objectHasTag(team)
            //new objectHasTag(team)
            //new proximityCriteriaBool(thePlayer?, 25)
            );



        //Debug.Log("CONDITION MAKER | zone:  "+tagging2.singleton.whichZone(thePlaceholderObjectInZone)+", and id number:  " + thePlaceholderObjectInZone.GetHashCode());

        objectSetGrabber theTeamObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(thePlaceholderObjectInZone), theCriteria);

        condition theCondition = new reverseCondition(new stickyCondition(new isThereAtLeastOneObjectInSet(theTeamObjectSet), 0));// theObjectDoingTheEnaction, numericalVariable.health);

        return theCondition;
    }
    */
}



public class factionEDYEOUF
{

    tag2 team;//this is annoying, messy
    List<objectSetInstantiator> theWaves = new List<objectSetInstantiator>();

    public factionEDYEOUF(tag2 teamIn)
    {
        team = teamIn;
    }


    public List<objectSetInstantiator> returnWaves()
    {
        addWave(
            new objectGen[]
                {
                    unit1()//,unit2(),unit2(),unit4()
                }
            );


        addWave(
            new objectGen[]
                {
                    unit1(),unit1()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit1()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit2()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit3()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit2(),unit2(),unit3()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit3(),unit3(),unit3()
                }
            );


        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit1(),unit1(),unit1(),
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit2(),unit2(),unit4()
                }
            );


        addWave(
            new objectGen[]
                {
                    unit2(),unit2(),unit4()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit3(),unit3(),unit4()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit2(),unit4()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit2(),unit3(),unit4()
                }
            );

        return theWaves;
    }

    public objectGen unit1()
    {
        interType theIntertype = interType.peircing;
        float magnitudeOfInteractionIn =1f;
        int firingRateIn = 10;
        float projectileSizeIn = 0.2f;
        float projectileSpeedIn = 1f;
        bool sdOnCollisionIn = true;
        int levelIn = 0;
        float gunHeightIn = 0.3f;
        float gunWidthIn = 0.2f;
        float gunLengthIn = 1.4f;

        return unitMaker2(1, 2.9f, 0.9f, 5, 20, 
            weaponMaker(1, 10, 4, 1, true, 0, 0.3f, 0.2f, 1.4f));
        //return new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33);//unit(1, 0.8f, 0.9f, 5, 20);
    }
    public objectGen unit2()
    {
        interType theIntertype = interType.peircing;
        float magnitudeOfInteractionIn = 2;
        int firingRateIn = 50;
        float projectileSpeedIn = 3;
        float projectileSizeIn = 1;
        bool sdOnCollisionIn = true;
        int levelIn = 0;
        float gunHeightIn = 0.4f;
        float gunWidthIn = 0.4f;
        float gunLengthIn = 1.7f;
    
            return unitMaker2(1, 3f, 1f, 3.7f, 33,

            weaponMaker(magnitudeOfInteractionIn, firingRateIn, projectileSpeedIn, projectileSizeIn, sdOnCollisionIn, levelIn, gunHeightIn, gunWidthIn, gunLengthIn));

    }
    public objectGen unit3()
    {
        interType theIntertype = interType.peircing;
        float magnitudeOfInteractionIn = 2;
            int firingRateIn = 20;
            float projectileSpeedIn = 4;
            float projectileSizeIn = 1;
            bool sdOnCollisionIn = true;
            int levelIn = 1;
            float gunHeightIn = 0.5f;
            float gunWidthIn = 0.3f;
            float gunLengthIn = 0.8f;
            return unitMaker2(3, 3.3f, 1f, 3.7f, 28,

            weaponMaker(magnitudeOfInteractionIn, firingRateIn, projectileSpeedIn, projectileSizeIn, sdOnCollisionIn, levelIn, gunHeightIn, gunWidthIn, gunLengthIn));

    }
    public objectGen unit4()
    {
        interType theIntertype = interType.peircing;
        float magnitudeOfInteractionIn = 5;
        int firingRateIn = 70;
        float projectileSpeedIn = 3;
        float projectileSizeIn = 1;
        bool sdOnCollisionIn = true;
        int levelIn = 2;
        float gunHeightIn = 0.6f;
        float gunWidthIn = 0.7f;
        float gunLengthIn = 1.3f;
        return unitMaker2(3, 3.9f, 1f, 7f, 25,

        weaponMaker(magnitudeOfInteractionIn, firingRateIn, projectileSpeedIn, projectileSizeIn, sdOnCollisionIn, levelIn, gunHeightIn, gunWidthIn, gunLengthIn));
    }


    /*
    
    public objectGen unit1()
    {
        interType theIntertype = interType.peircing;
        float magnitudeOfInteractionIn =1f;
        int firingRateIn = 10;
        float projectileSizeIn = 0.2f;
        float projectileSpeedIn = 1f;
        bool sdOnCollisionIn = true;
        int levelIn = 0;
        float gunHeightIn = 0.3f;
        float gunWidthIn = 0.2f;
        float gunLengthIn = 1.4f;

        return unitMaker2(1, 2.9f, 0.9f, 5, 20, 
            weaponMaker2(firingRateIn, projectileMaker(
                new objectModifier[] {
            new simpleMovingMod(projectileSpeedIn, sdOnCollisionIn, 99),
            new collisionInteractionMod(theIntertype, magnitudeOfInteractionIn, levelIn),
            new modifyScale(projectileSizeIn, projectileSizeIn, projectileSizeIn)
            }), new objectModifier[]
            {
                new modifyScale(gunLengthIn, gunHeightIn, gunWidthIn)
            }
            ));//weaponMaker(1, 10, 4, 1, true, 0, 0.3f, 0.2f, 1.4f//(1,10,4,1,true,0,0.3f,0.2f,1.4f));
        //return new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33);//unit(1, 0.8f, 0.9f, 5, 20);
    }
    public objectGen unit2()
    {
        interType theIntertype = interType.peircing;
        float magnitudeOfInteractionIn = 2;
        int firingRateIn = 50;
        float projectileSpeedIn = 3;
        float projectileSizeIn = 1;
        bool sdOnCollisionIn = true;
        int levelIn = 0;
        float gunHeightIn = 0.4f;
        float gunWidthIn = 0.4f;
        float gunLengthIn = 1.7f;
    
            return unitMaker2(1, 3f, 1f, 3.7f, 33,

            weaponMaker2(firingRateIn, projectileMaker(
                new objectModifier[] {
            new simpleMovingMod(projectileSpeedIn, sdOnCollisionIn, 99),
            new collisionInteractionMod(theIntertype, magnitudeOfInteractionIn, levelIn),
            new modifyScale(projectileSizeIn, projectileSizeIn, projectileSizeIn)
            }), new objectModifier[]
            {
                new modifyScale(gunLengthIn, gunHeightIn, gunWidthIn)
            }
            ));
        
    }
    public objectGen unit3()
    {
        interType theIntertype = interType.peircing;
        float magnitudeOfInteractionIn = 2;
            int firingRateIn = 20;
            float projectileSpeedIn = 4;
            float projectileSizeIn = 1;
            bool sdOnCollisionIn = true;
            int levelIn = 1;
            float gunHeightIn = 0.5f;
            float gunWidthIn = 0.3f;
            float gunLengthIn = 0.8f;
            return unitMaker2(3, 3.3f, 1f, 3.7f, 28,

            weaponMaker2(firingRateIn, projectileMaker(
                new objectModifier[] {
            new simpleMovingMod(projectileSpeedIn, sdOnCollisionIn, 99),
            new collisionInteractionMod(theIntertype, magnitudeOfInteractionIn, levelIn),
            new modifyScale(projectileSizeIn, projectileSizeIn, projectileSizeIn)
            }), new objectModifier[]
            {
                new modifyScale(gunLengthIn, gunHeightIn, gunWidthIn)
            }
            ));

    }
    public objectGen unit4()
    {
        interType theIntertype = interType.peircing;
        float magnitudeOfInteractionIn = 5;
        int firingRateIn = 70;
        float projectileSpeedIn = 3;
        float projectileSizeIn = 1;
        bool sdOnCollisionIn = true;
        int levelIn = 2;
        float gunHeightIn = 0.6f;
        float gunWidthIn = 0.7f;
        float gunLengthIn = 1.3f;
        return unitMaker2(3, 3.9f, 1f, 7f, 25,

        weaponMaker2(firingRateIn, projectileMaker(
                new objectModifier[] {
            new simpleMovingMod(projectileSpeedIn, sdOnCollisionIn, 99),
            new collisionInteractionMod(theIntertype, magnitudeOfInteractionIn, levelIn),
            new modifyScale(projectileSizeIn, projectileSizeIn, projectileSizeIn)
            }), new objectModifier[]
            {
                new modifyScale(gunLengthIn, gunHeightIn, gunWidthIn)
            }
            ));
    }
    















    
    
    public objectGen unit1()
    {
        return unit(1, 2.9f, 0.9f, 5, 20);
        //return new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33);//unit(1, 0.8f, 0.9f, 5, 20);
    }
    public objectGen unit2()
    {
        return unit(1, 3f, 1f, 3.7f, 33);
    }
    public objectGen unit3()
    {
        return unit(3, 3.3f, 1f, 3.7f, 28);
    }
    public objectGen unit4()
    {
        return unit(3, 3.9f, 1f, 7f, 25);
    }




    */





    public void addWave(objectGen[] theWave)
    {
        theWaves.Add( new objectSetInstantiator(theWave));
    }

    objectGen unit(float health, float height, float width, float speed, float targetDetectionRange)
    {
        return new basicPaintByNumbersSoldierGeneratorG(team, health, height, width, speed, targetDetectionRange);
    }

    objectGen unitMaker2(float health, float height, float width, float speed, float targetDetectionRange, objectGen weapon)
    {
        return new soldierGeneratorWithCustomGun(team, health, height, width, speed, targetDetectionRange, weapon);
    }

    objectGen weaponMaker(float magnitudeOfInteractionIn, int firingRateIn, float projectileSpeedIn, float projectileSizeIn, bool sdOnCollisionIn = true, int levelIn = 0, float gunHeightIn = 1, float gunWidthIn = 1, float gunLengthIn = 1)
    {
        return new paintByNumbersGun1(magnitudeOfInteractionIn, firingRateIn, projectileSpeedIn, projectileSizeIn, sdOnCollisionIn, levelIn, gunHeightIn, gunWidthIn, gunLengthIn);
    }
    objectGen weaponMaker2(int firingRateIn, genObjectAndModify projectileType)
    {
        
        return new newGunGen(projectileType, new objectModifier[]{
        }, 
        firingRateIn);
    }
    objectGen weaponMaker2(int firingRateIn, genObjectAndModify projectileType, objectModifier[] gunItselfMods)
    {

        return new newGunGen(projectileType, gunItselfMods,
        firingRateIn);
    }

    genObjectAndModify projectileMaker(objectModifier[] projectileModifiers) //should be a class?  like newGunGen?
    { 
        //float magnitudeOfInteractionIn, int firingRateIn,
        //float projectileSpeedIn, float projectileSizeIn,
        //bool sdOnCollisionIn = true, int levelIn = 0)
    
        return new genObjectAndModify(new sphereGen(), projectileModifiers);
        

    }


}

public class factionHNCURF
{

    tag2 team;//this is annoying, messy
    List<objectSetInstantiator> theWaves = new List<objectSetInstantiator>();

    public void addWave(objectGen[] theWave)
    {
        theWaves.Add(new objectSetInstantiator(theWave));
    }

    public factionHNCURF(tag2 teamIn)
    {
        team = teamIn;
    }
    public List<objectSetInstantiator> returnWaves()
    {
        addWave(
            new objectGen[]
                {
                    unit1(),unit2(),unit2(),unit4()
                }
            );


        addWave(
            new objectGen[]
                {
                    unit1(),unit1()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit1()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit2()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit3()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit2(),unit2(),unit3()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit3(),unit3(),unit3()
                }
            );


        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit1(),unit1(),unit1(),
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit2(),unit2(),unit4()
                }
            );


        addWave(
            new objectGen[]
                {
                    unit2(),unit2(),unit4()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit3(),unit3(),unit4()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit2(),unit4()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit2(),unit3(),unit4()
                }
            );

        return theWaves;
    }

    public objectGen unit1()
    {
        return unit(1, 2.6f, 0.7f, 6, 20);
    }
    public objectGen unit2()
    {
        return unit(1, 2.7f, 0.8f, 7.7f, 26);
    }
    public objectGen unit3()
    {
        return unit(2, 3.3f, 1f, 4f, 38);
    }
    public objectGen unit4()
    {
        return unit(3, 3.9f, 1f, 6f, 45);
    }








    objectGen unit(float health, float height, float width, float speed, float targetDetectionRange)
    {
        return new basicPaintByNumbersSoldierGeneratorG(team, health, height, width, speed, targetDetectionRange);
    }



}

public class factionJKLHYGFIEG
{

    tag2 team;//this is annoying, messy
    List<objectSetInstantiator> theWaves = new List<objectSetInstantiator>();

    public void addWave(objectGen[] theWave)
    {
        theWaves.Add(new objectSetInstantiator(theWave));
    }

    public factionJKLHYGFIEG(tag2 teamIn)
    {
        team = teamIn;
    }
    public List<objectSetInstantiator> returnWaves()
    {
        addWave(
            new objectGen[]
                {
                    unit1(),unit2(),unit3(),unit4()
                }
            );


        addWave(
            new objectGen[]
                {
                    unit1(),unit1()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit1()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit2()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit3()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit2(),unit2(),unit3()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit3(),unit3(),unit3()
                }
            );


        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit1(),unit1(),unit1(),
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit2(),unit2(),unit4()
                }
            );


        addWave(
            new objectGen[]
                {
                    unit2(),unit2(),unit4()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit3(),unit3(),unit4()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit1(),unit2(),unit4()
                }
            );

        addWave(
            new objectGen[]
                {
                    unit1(),unit1(),unit2(),unit3(),unit4()
                }
            );

        return theWaves;
    }

    public objectGen unit1()
    {
        return unit(1, 3.6f, 1.3f, 4, 20);
    }
    public objectGen unit2()
    {
        return unit(2, 2.95f, 1.13f, 4.7f, 26);
    }
    public objectGen unit3()
    {
        return unit(3, 3.9f, 1f, 5f, 38);
    }
    public objectGen unit4()
    {
        return unit(4, 4.1f, 1.6f, 4f, 45);
    }








    objectGen unit(float health, float height, float width, float speed, float targetDetectionRange)
    {
        return new basicPaintByNumbersSoldierGeneratorG(team, health, height, width, speed, targetDetectionRange);
    }
    objectGen unitMaker2(float health, float height, float width, float speed, float targetDetectionRange, objectGen weapon)
    {
        return new soldierGeneratorWithCustomGun(team, health, height, width, speed, targetDetectionRange, weapon);
    }

    objectGen weaponMaker(float magnitudeOfInteractionIn, int firingRateIn, float projectileSpeedIn, float projectileSizeIn, bool sdOnCollisionIn = true, int levelIn = 0, float gunHeightIn = 1, float gunWidthIn = 1, float gunLengthIn = 1)
    {
        return new paintByNumbersGun1(magnitudeOfInteractionIn,firingRateIn,projectileSpeedIn,projectileSizeIn, sdOnCollisionIn, levelIn, gunHeightIn, gunWidthIn, gunLengthIn);
    }


}











public class basicPaintByNumbersSoldierGeneratorG : objectGen
{
    tagging2.tag2 team;
    private Vector3 thePosition;

    float health;
    float height;
    float width;
    objectGen weapon;
    //List<FSM> theBehavior;  //gah!  needs the object as an input!
    float speed;
    float targetDetectionRange;


    //(tagging2.tag2 theTeamIn, float health, float speed, float height, float width, float targetDetectionRange, objectGen weapon, List<FSM> theBehavior )

    // 
    //basicBodyProperties
    public basicPaintByNumbersSoldierGeneratorG(tag2 theTeamIn, float health, float height, float width, float speed, float targetDetectionRange)//, objectGen weapon)
    {
        this.team = theTeamIn;
        this.health = health;
        this.height = height;
        this.width = width;
        this.speed = speed;
    }

    public GameObject generate()
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);
        GameObject torso = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);


        newObj.AddComponent<rtsModule>();
        //oldTorso(newObj,torso);

        newTorso(newObj, torso);

        genGen.singleton.ensureVirtualGamePad(newObj);

        rtsModule theRTScomponent = newObj.AddComponent<rtsModule>();

        tagging2.singleton.addTag(newObj, team);
        playable2 thePlayable = newObj.AddComponent<playable2>();
        thePlayable.theNavMeshTransform = newObj.transform;
        thePlayable.theHorizontalRotationTransform = torso.transform;
        thePlayable.initializeEnactionPoint1();
        thePlayable.theVerticalRotationTransform = thePlayable.enactionPoint1.transform;
        thePlayable.enactionPoint1.transform.SetParent(thePlayable.theHorizontalRotationTransform, true);

        thePlayable.dictOfIvariables[numericalVariable.health] = health;
        thePlayable.equipperSlotsAndContents[simpleSlot.hands] = null;

        genGen.singleton.addArrowForward(thePlayable.enactionPoint1);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);


        genGen.singleton.makeEnactionsWithTorsoArticulation1(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);



        inventory1 theirInventory = newObj.AddComponent<inventory1>();
        //GameObject gun = weapon.generate();
        GameObject gun = genGen.singleton.returnGun1(newObj.transform.position);//new paintByNumbersGun1(1,40, 10,0.5f, true, 0, 0.3f, 0.2f,0.5f).generate();//
        theirInventory.inventoryItems.Add(gun);
        interactionCreator.singleton.dockXToY(gun, newObj);




        FSMcomponent theFSMcomponent = newObj.AddComponent<FSMcomponent>();
        theFSMcomponent.theFSMList = new basicPaintByNumbersSoldierFSM(newObj,team,speed,targetDetectionRange).returnIt();//theBehavior;


        //newObj.GetComponent<NavMeshAgent>().enabled = false;
        newObj.AddComponent<reactivationOfNavMeshAgent>();
        return newObj;
    }

    private void newTorso(GameObject newObj, GameObject torso)
    {
        //torso.transform.localScale = new Vector3(width, height / 2, width);


        //torso.transform.position += new Vector3(0, (height / 2) - 1, 0);
        torso.transform.position += new Vector3(0, 1f, 0);

        Renderer theRenderer = newObj.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];
        Renderer theRenderer2 = torso.GetComponent<Renderer>();
        //theRenderer2.material.color = tagging2.singleton.teamColors[team];

        GameObject.Destroy(newObj.GetComponent<Collider>());
        BoxCollider hitbox = newObj.AddComponent<BoxCollider>();
        //hitbox.size += new Vector3(width-1, height-1, width-1);
        //hitbox.center += new Vector3(0, (height-1)/2, 0);
        hitbox.size += new Vector3(0, 1f, 0);
        hitbox.center += new Vector3(0, 0.5f, 0);


        torso.transform.SetParent(newObj.transform, false);
        newObj.transform.localScale = new Vector3(width, (height-1)/2, width);
        //newObj.transform.position += new Vector3(0, ((height - 1) / 2), 0);
    }

    private void oldTorso(GameObject newObj, GameObject torso)
    {
        torso.transform.SetParent(newObj.transform, false);
        torso.transform.position += new Vector3(0, 1f, 0);


        Renderer theRenderer = newObj.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];

        GameObject.Destroy(newObj.GetComponent<Collider>());
        BoxCollider hitbox = newObj.AddComponent<BoxCollider>();
        hitbox.size += new Vector3(0, 1f, 0);
        hitbox.center += new Vector3(0, 0.5f, 0);
    }
}

public class soldierGeneratorWithCustomGun : objectGen
{
    tagging2.tag2 team;
    private Vector3 thePosition;

    float health;
    float height;
    float width;
    objectGen weapon;
    //List<FSM> theBehavior;  //gah!  needs the object as an input!
    float speed;
    float targetDetectionRange;


    //(tagging2.tag2 theTeamIn, float health, float speed, float height, float width, float targetDetectionRange, objectGen weapon, List<FSM> theBehavior )

    // 
    //basicBodyProperties
    public soldierGeneratorWithCustomGun(tag2 theTeamIn, float health, float height, float width, float speed, float targetDetectionRange, objectGen weaponGenIn)
    {
        this.team = theTeamIn;
        this.health = health;
        this.height = height;
        this.width = width;
        this.speed = speed;
        weapon = weaponGenIn;
    }

    public GameObject generate()
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);
        GameObject torso = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);

        //oldTorso(newObj,torso);

        newTorso(newObj, torso);

        genGen.singleton.ensureVirtualGamePad(newObj);


        tagging2.singleton.addTag(newObj, team);
        playable2 thePlayable = newObj.AddComponent<playable2>();
        thePlayable.theNavMeshTransform = newObj.transform;
        thePlayable.theHorizontalRotationTransform = torso.transform;
        thePlayable.initializeEnactionPoint1();
        thePlayable.theVerticalRotationTransform = thePlayable.enactionPoint1.transform;
        thePlayable.enactionPoint1.transform.SetParent(thePlayable.theHorizontalRotationTransform, true);

        thePlayable.dictOfIvariables[numericalVariable.health] = health;
        thePlayable.equipperSlotsAndContents[simpleSlot.hands] = null;

        genGen.singleton.addArrowForward(thePlayable.enactionPoint1);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);


        genGen.singleton.makeEnactionsWithTorsoArticulation1(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);



        inventory1 theirInventory = newObj.AddComponent<inventory1>();
        //GameObject gun = weapon.generate();
        GameObject gun = weapon.generate();//genGen.singleton.returnGun1(newObj.transform.position);
        theirInventory.inventoryItems.Add(gun);
        interactionCreator.singleton.dockXToY(gun, newObj);




        FSMcomponent theFSMcomponent = newObj.AddComponent<FSMcomponent>();
        theFSMcomponent.theFSMList = new basicPaintByNumbersSoldierFSM(newObj, team, speed, targetDetectionRange).returnIt();//theBehavior;


        //newObj.GetComponent<NavMeshAgent>().enabled = false;
        newObj.AddComponent<reactivationOfNavMeshAgent>();

        return newObj;
    }

    private void newTorso(GameObject newObj, GameObject torso)
    {
        //torso.transform.localScale = new Vector3(width, height / 2, width);


        //torso.transform.position += new Vector3(0, (height / 2) - 1, 0);
        torso.transform.position += new Vector3(0, 1f, 0);

        Renderer theRenderer = newObj.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];
        Renderer theRenderer2 = torso.GetComponent<Renderer>();
        //theRenderer2.material.color = tagging2.singleton.teamColors[team];

        GameObject.Destroy(newObj.GetComponent<Collider>());
        BoxCollider hitbox = newObj.AddComponent<BoxCollider>();
        //hitbox.size += new Vector3(width-1, height-1, width-1);
        //hitbox.center += new Vector3(0, (height-1)/2, 0);
        hitbox.size += new Vector3(0, 1f, 0);
        hitbox.center += new Vector3(0, 0.5f, 0);


        torso.transform.SetParent(newObj.transform, false);
        newObj.transform.localScale = new Vector3(width, (height - 1) / 2, width);
        //newObj.transform.position += new Vector3(0, ((height - 1) / 2), 0);
    }

    private void oldTorso(GameObject newObj, GameObject torso)
    {
        torso.transform.SetParent(newObj.transform, false);
        torso.transform.position += new Vector3(0, 1f, 0);


        Renderer theRenderer = newObj.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];

        GameObject.Destroy(newObj.GetComponent<Collider>());
        BoxCollider hitbox = newObj.AddComponent<BoxCollider>();
        hitbox.size += new Vector3(0, 1f, 0);
        hitbox.center += new Vector3(0, 0.5f, 0);
    }
}


public class basicPaintByNumbersSoldierFSM : FSM
{

    //FSM theFSM;
    public List<FSM> theFSMList = new List<FSM>();

    //float combatRange = 40f;
    private GameObject newObj;
    private tag2 team;
    private float speed;
    private float targetDetectionRange;

    public basicPaintByNumbersSoldierFSM(GameObject theObjectDoingTheEnaction, tag2 team, float speed, float targetDetectionRange = 40f)
    {
        this.newObj = theObjectDoingTheEnaction;
        this.team = team;
        this.speed = speed;
        this.targetDetectionRange = targetDetectionRange;


        theFSMList.Add(feetFSM(theObjectDoingTheEnaction, team));
        theFSMList.Add(handsFSM(theObjectDoingTheEnaction, team));

    }

    private FSM handsFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        FSM idle = new generateFSM();

        objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction, team);
        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 110);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);
        
        FSM combat1 = new generateFSM(new aimAtXAndPressY(theObjectDoingTheEnaction, theAttackTargetPicker, buttonCategories.primary, targetDetectionRange).returnIt());//new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, targetDetectionRange).returnIt());



        idle.addSwitchAndReverse(switchToAttack, combat1);



        equipItemFSM equipGun = new equipItemFSM(theObjectDoingTheEnaction, interType.peircing);

        idle.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theFSM);
        //wander.addSwitchAndReverse(switchToAttack, equipGun.theFSM);
        combat1.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theFSM);//messy



        idle.name = "hands, idle";
        combat1.name = "hands, combat1";
        equipGun.theFSM.name = "hands, equipGun";
        return idle; ;
    }

    private objectCriteria createAttackCriteria(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new stickyTrueCriteria(new lineOfSight(theObjectDoingTheEnaction), 200),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 25)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private targetPicker generateAttackTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theAttackObjectSet)
    {

        targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        return theAttackTargetPicker;
    }

    private FSM feetFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        FSM wander = new generateFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());




        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new lineOfSight(theObjectDoingTheEnaction),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 25)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);

        //targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        //targetPicker theTargetPicker = new applePatternTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);
        targetPicker theTargetPicker = new combatDodgeVarietyPack1TargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        FSM combat1 = new generateFSM(new goToX(theObjectDoingTheEnaction, theTargetPicker, targetDetectionRange).returnIt());

        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        wander.addSwitchAndReverse(switchToAttack, combat1);

        wander.name = "feet, wander";
        combat1.name = "feet, combat1";










        targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        FSM goToRTSTarget = rtsFSM1(theObjectDoingTheEnaction, team);//new generateFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

        condition switchFromWanderToRTS = makeSwitchFromWanderToRTS(theObjectDoingTheEnaction);

        wander.addSwitchAndReverse(switchFromWanderToRTS, goToRTSTarget);
        goToRTSTarget.addSwitchAndReverse(switchToAttack, combat1);

        goToRTSTarget.name = "feet, goToRTSTarget";









        navAgent theNav = theObjectDoingTheEnaction.GetComponent<navAgent>();
        theNav.theAgent.speed = this.speed;
        theNav.theAgent.baseOffset = 0.5f;//theObjectDoingTheEnaction.transform.localScale.y;
        //theNav.theAgent.

        return wander;
    }







    private FSM rtsFSM1(GameObject theObjectDoingTheEnaction, tag2 teamIn)
    {
        //FSM wander = new generateFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());

        targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        FSM goToRTSTarget = new generateFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

        //condition switchFromWanderToRTS = makeSwitchFromWanderToRTS(theObjectDoingTheEnaction);

        //wander.addSwitchAndReverse(switchFromWanderToRTS, goToRTSTarget);


        //wander.name = "feet, wander";
        goToRTSTarget.name = "feet, goToRTSTarget";
        return goToRTSTarget;
    }

    private targetPicker makeTheRTSCommandTargetPicker(GameObject theObjectDoingTheEnaction)
    {
        //bit messy ad-hoc sorta way to do this for now
        //need target picker that just copies from their RTS module lol

        //individualObjectReturner theObject = new presetObject(theObjectDoingTheEnaction);
        targetPicker theRTSCommandTargetPicker = new targetPickerFromRTSModule(theObjectDoingTheEnaction);
        return theRTSCommandTargetPicker;
    }

    private condition makeSwitchFromWanderToRTS(GameObject theObjectDoingTheEnaction)
    {
        //if there's more than zero orders
        //opposite of leader's condition

        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasRtsModule(),
            new reverseCriteria( new hasNoOrders())
            );

        //how to apply it to themselves?
        individualObjectReturner theObject = new presetObject(theObjectDoingTheEnaction);


        condition switchFromWanderToRTS = new individualObjectMeetsAllCriteria(theObject, theCriteria);

        return switchFromWanderToRTS;
    }




















    public List<FSM> returnIt()
    {
        return theFSMList;
    }



}














public class basicSoldierGenerator : doAtPoint
{
    tagging2.tag2 team;

    public basicSoldierGenerator(tagging2.tag2 theTeamIn)
    {
        this.team = theTeamIn;
    }


    internal override void doIt(Vector3 thisPoint)
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCylinderPrefab, thisPoint, Quaternion.identity);
        Renderer theRenderer = newObj.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];
        GameObject.Destroy(newObj.GetComponent<Collider>());
        newObj.AddComponent<CapsuleCollider>();

        genGen.singleton.ensureVirtualGamePad(newObj);



        tagging2.singleton.addTag(newObj, team);
        playable2 thePlayable = newObj.AddComponent<playable2>();
        thePlayable.dictOfIvariables[numericalVariable.health] = 2;
        thePlayable.equipperSlotsAndContents[simpleSlot.hands] = null;
        thePlayable.initializeEnactionPoint1();
        genGen.singleton.addArrowForward(thePlayable.enactionPoint1); 
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);


        genGen.singleton.makeBasicEnactions(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);


        inventory1 theirInventory = newObj.AddComponent<inventory1>();
        //theirInventory.startingItem = genGen.singleton.returnGun1(newObj.transform.position);
        GameObject gun = genGen.singleton.returnGun1(newObj.transform.position);
        theirInventory.inventoryItems.Add(gun);
        interactionCreator.singleton.dockXToY(gun, newObj);


        FSMcomponent theFSMcomponent = newObj.AddComponent<FSMcomponent>();
        theFSMcomponent.theFSMList = new basicSoldierFSM(newObj, team).returnIt();
    }



}


public class basicSoldierGeneratorG : objectGen
{
    tagging2.tag2 team;
    private Vector3 thePosition;

    public basicSoldierGeneratorG(tagging2.tag2 theTeamIn)
    {
        this.team = theTeamIn;
        thePosition = new Vector3();// (1,0,100);
    }

    public GameObject generate()
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thePosition, Quaternion.identity);
        GameObject torso = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thePosition, Quaternion.identity);

        newObj.AddComponent<rtsModule>();


        //newObj.transform.localScale = new Vector3(1, 0.5f, 1);
        //torso.transform.localScale = new Vector3(1, 0.5f, 1); 
        torso.transform.SetParent(newObj.transform, false);
        torso.transform.position += new Vector3(0, 1f, 0);


        Renderer theRenderer = newObj.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];

        GameObject.Destroy(newObj.GetComponent<Collider>());
        BoxCollider hitbox = newObj.AddComponent<BoxCollider>();
        hitbox.size += new Vector3(0, 1f, 0);
        hitbox.center += new Vector3(0, 0.5f, 0);
        //hitbox.transform.localScale += new Vector3(0, 1f, 0);
        //newObj.transform.localScale += new Vector3(0, -1f, 0);

        genGen.singleton.ensureVirtualGamePad(newObj);




        tagging2.singleton.addTag(newObj, team);
        playable2 thePlayable = newObj.AddComponent<playable2>();
        thePlayable.theNavMeshTransform = newObj.transform;
        thePlayable.theHorizontalRotationTransform = torso.transform;
        thePlayable.initializeEnactionPoint1();
        thePlayable.theVerticalRotationTransform = thePlayable.enactionPoint1.transform;
        thePlayable.enactionPoint1.transform.SetParent(thePlayable.theHorizontalRotationTransform,true);

        thePlayable.dictOfIvariables[numericalVariable.health] = 1;
        thePlayable.equipperSlotsAndContents[simpleSlot.hands] = null;

        genGen.singleton.addArrowForward(thePlayable.enactionPoint1);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);


        genGen.singleton.makeEnactionsWithTorsoArticulation1(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);


        inventory1 theirInventory = newObj.AddComponent<inventory1>();
        //theirInventory.startingItem = genGen.singleton.returnGun1(newObj.transform.position);
        GameObject gun = genGen.singleton.returnGun1(newObj.transform.position);
        theirInventory.inventoryItems.Add(gun);
        interactionCreator.singleton.dockXToY(gun, newObj);


        FSMcomponent theFSMcomponent = newObj.AddComponent<FSMcomponent>();
        theFSMcomponent.theFSMList = new basicSoldierFSM(newObj, team).returnIt();




        newObj.AddComponent<reactivationOfNavMeshAgent>();

        return newObj;
    }



}







public class basicSoldierFSM : FSM
{

    //FSM theFSM;
    public List<FSM> theFSMList = new List<FSM>();

    float combatRange = 40f;

    public basicSoldierFSM(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {


        theFSMList.Add(feetFSM(theObjectDoingTheEnaction, team));
        theFSMList.Add(handsFSM(theObjectDoingTheEnaction, team));

    }

    private FSM handsFSM(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {
        FSM idle = new generateFSM();

        objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction,team);
        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction,theAttackObjectSet);

        FSM combat1 = new generateFSM(new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, combatRange).returnIt());

        

        idle.addSwitchAndReverse(switchToAttack, combat1);



        equipItemFSM equipGun = new equipItemFSM(theObjectDoingTheEnaction, interType.peircing);

        idle.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theFSM);
        //wander.addSwitchAndReverse(switchToAttack, equipGun.theFSM);
        combat1.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theFSM);//messy



        idle.name = "hands, idle";
        combat1.name = "hands, combat1";
        equipGun.theFSM.name = "hands, equipGun";
        return idle;;
    }

    private objectCriteria createAttackCriteria(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new stickyTrueCriteria( new lineOfSight(theObjectDoingTheEnaction),200),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 25)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private targetPicker generateAttackTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theAttackObjectSet)
    {
        
        targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        return theAttackTargetPicker;
    }

    private FSM feetFSM(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {
        FSM wander = new generateFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());




        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new lineOfSight(theObjectDoingTheEnaction),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 25)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);

        //targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        //targetPicker theTargetPicker = new applePatternTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);
        targetPicker theTargetPicker = new combatDodgeVarietyPack1TargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        FSM combat1 = new generateFSM(new goToX(theObjectDoingTheEnaction, theTargetPicker, combatRange).returnIt());

        //condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 1);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        wander.addSwitchAndReverse(switchToAttack, combat1);


        //targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        FSM goToRTSTarget = rtsFSM1(theObjectDoingTheEnaction,team);//new generateFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

        condition switchFromWanderToRTS = makeSwitchFromWanderToRTS(theObjectDoingTheEnaction);

        wander.addSwitchAndReverse(switchFromWanderToRTS, goToRTSTarget);
        goToRTSTarget.addSwitchAndReverse(switchToAttack, combat1);


        wander.name = "feet, wander";
        combat1.name = "feet, combat1";
        goToRTSTarget.name = "feet, goToRTSTarget";
        return wander;
    }





    private FSM rtsFSM1(GameObject theObjectDoingTheEnaction, tag2 teamIn)
    {
        //FSM wander = new generateFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());

        targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        FSM goToRTSTarget = new generateFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

        //condition switchFromWanderToRTS = makeSwitchFromWanderToRTS(theObjectDoingTheEnaction);

        //wander.addSwitchAndReverse(switchFromWanderToRTS, goToRTSTarget);


        //wander.name = "feet, wander";
        goToRTSTarget.name = "feet, goToRTSTarget";
        return goToRTSTarget;
    }

    private targetPicker makeTheRTSCommandTargetPicker(GameObject theObjectDoingTheEnaction)
    {
        //bit messy ad-hoc sorta way to do this for now
        //need target picker that just copies from their RTS module lol

        //individualObjectReturner theObject = new presetObject(theObjectDoingTheEnaction);
        targetPicker theRTSCommandTargetPicker = new targetPickerFromRTSModule(theObjectDoingTheEnaction);
        return theRTSCommandTargetPicker;
    }

    private condition makeSwitchFromWanderToRTS(GameObject theObjectDoingTheEnaction)
    {
        //if there's more than zero orders
        //opposite of leader's condition

        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasRtsModule(),
            new reverseCriteria( new hasNoOrders())
            );

        //how to apply it to themselves?
        individualObjectReturner theObject = new presetObject(theObjectDoingTheEnaction);


        condition switchFromWanderToRTS = new individualObjectMeetsAllCriteria(theObject, theCriteria);

        return switchFromWanderToRTS;
    }










    public List<FSM> returnIt()
    {
        return theFSMList;
    }



}

public class combatDodgeVarietyPack1TargetPicker : targetPicker
{
    //randomly pick from:
    //      apple pattern
    //      idle
    //      flee
    //      go towards
    //      random wander
    //can maybe bias in favor of some?  but for now, simple random.

    List<targetPicker> listOfTargetPickers = new List<targetPicker>();
    int currentPick = 0;
    int behaviorChangeCountdownTimeCurrent = 0;
    int behaviorChangeCountdownTimeLIMIT = 40;

    bool currentlyDoingMainBehavior = true;

    public combatDodgeVarietyPack1TargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theSetInput)
    {

        listOfTargetPickers.Add(new applePatternTargetPicker(theObjectDoingTheEnaction, theSetInput));
        listOfTargetPickers.Add(new radialFleeingTargetPicker(theObjectDoingTheEnaction, theSetInput));
        listOfTargetPickers.Add(new nearestTargetPicker(theObjectDoingTheEnaction, theSetInput));  //this is "go TOWARD" i think
        listOfTargetPickers.Add(new randomNearbyLocationTargetPicker(theObjectDoingTheEnaction));
        //how to do "idle"?
    }

    public override agnosticTargetCalc pickNext()
    {
        randomlyPickDifferentStrategySometimes();
        //Debug.Log("currentPick:  " + currentPick);



        return listOfTargetPickers[currentPick].pickNext();
    }

    private void randomlyPickDifferentStrategySometimes()
    {
        behaviorChangeCountdownTimeCurrent++;
        //Debug.Log("behaviorChangeCountdownTimeCurrent:  "+behaviorChangeCountdownTimeCurrent);

        if (behaviorChangeCountdownTimeCurrent < behaviorChangeCountdownTimeLIMIT) { return; }


        if (currentlyDoingMainBehavior)
        {
            currentlyDoingMainBehavior = false;
            currentPick = repository2.singleton.randomTargetPickerInteger(listOfTargetPickers.Count);

            behaviorChangeCountdownTimeLIMIT = repository2.singleton.randomTargetPickerInteger(270) + 110;

        }
        else
        {
            currentlyDoingMainBehavior = true;
            currentPick = 0;

            behaviorChangeCountdownTimeLIMIT = repository2.singleton.randomTargetPickerInteger(300) + 210;
        }

        behaviorChangeCountdownTimeCurrent = 0;
    }
}

public class targetPickerFromRTSModule : targetPicker
{
    //bit ugle messy ad hoc???


    private GameObject theObjectDoingTheEnaction;
    rtsModule theRTSModule;

    public targetPickerFromRTSModule(GameObject theObjectDoingTheEnactionIn)
    {
        this.theObjectDoingTheEnaction = theObjectDoingTheEnactionIn;
        theRTSModule = theObjectDoingTheEnaction.GetComponent<rtsModule>();
    }

    public override agnosticTargetCalc pickNext()
    {
        theRTSModule = theObjectDoingTheEnaction.GetComponent<rtsModule>();
        if(theRTSModule == null) { return new agnosticTargetCalc(theObjectDoingTheEnaction, theObjectDoingTheEnaction); }//???

        return theRTSModule.currentReceivedOrders;
    }
}


public class equipItemFSM
{

    public FSM theFSM;

    public equipItemFSM(GameObject theObjectDoingTheEnaction, interType interTypeX)
    {

        //equipObjectRepeater
        theFSM = new generateFSM(new equipObjectRepeater(theObjectDoingTheEnaction)); //hmm, this doesn't share cache with the conditions......
        //MAKE SURE TO RETURN IT!!!!!!  
    }

    public condition theNotEquippedButCanEquipSwitchCondition(GameObject theObjectDoingTheEnaction, interType interTypeX)
    {

        //switch condition:
        //      has item we want in inventory
        //      but not in playable or equipped items [just search in virtual gamepad???]
        //  AND we want to CACHE that item if possible, so we can equip it without searching for it again



        objectCriteria hasInterTypeX = new intertypeXisOnObject(interTypeX);

        condition hasInPlayable = new individualObjectMeetsAllCriteria(new presetObject(theObjectDoingTheEnaction), hasInterTypeX);







        objectSetGrabber thesetOfAllEquippedObjects = new setOfAllEquippedObjects(theObjectDoingTheEnaction);
        //objectCriteria hasInterTypeX = new intertypeXisOnObject(interTypeX);
        pickFirstObjectXFromListY theEquippedObjectPicker = new pickFirstObjectXFromListY(hasInterTypeX, thesetOfAllEquippedObjects);


        condition hasEquipped = new nonNullObject(theEquippedObjectPicker);








        //      has item we want in inventory [AND we want to CACHE that item if possible]
        //condition:  object returner returns non-null object
        //object returner [main]:  object on list that meets criteria
        //          the list:   inventory on [set?] object
        //          the criteria: has intertypeX
        //caching etc
        //  plug cache setter into condition checker [so, wrap the above returner]
        //  have cache observer as a variable we can plug into "equipping" enaction

        setOfAllInventoryObjects theInvGrabber = new setOfAllInventoryObjects(theObjectDoingTheEnaction);
        //objectCriteria hasInterTypeX = new intertypeXisOnObject(interTypeX);
        pickFirstObjectXFromListY theInvObjectPicker = new pickFirstObjectXFromListY(hasInterTypeX, theInvGrabber);


        objectCacheSetter theInvCacheSetter = new objectCacheSetter(theInvObjectPicker);
        objectCacheReceiver theInvCacheReceiver = new objectCacheReceiver(theInvCacheSetter);

        condition hasInInventory = new nonNullObject(theInvCacheSetter);






        condition notEquippedButCanEquip = new multicondition(new reverseCondition(hasInPlayable), new reverseCondition(hasEquipped), hasInInventory);


        //new intertypeXisOnObject();
        //new intertypeXisInEquipperSlots();

        //new targetCacheSetter
        //new targetCacheReceiver



        //      but not in playable or equipped items [just search in virtual gamepad???]
        //similar to above.
        //but the object lists are equipper slot contents, and ...playable is just one object, sooo a set of ONE?
        //does this need cache too???
        //new intertypeXisInInventory();

        //new equipObjectRepeater();


        return notEquippedButCanEquip;
    }


    public FSM returnIt()
    {
        return theFSM;
    }



}



public class equipObjectRepeater : repeater
{
    repeater theRepeater; //hmm, nesting makes problems for printing/debugging contents.....
    individualObjectReturner theIndividualObjectReturner;

    public equipObjectRepeater(GameObject theObjectDoingTheEnaction)
    {

        theRepeater = new simpleExactRepeatOfPerma(new permaPlan2(equipX2(theObjectDoingTheEnaction)));
    }

    //for now
    /*
    public equipObjectRepeater(individualObjectReturner theIndividualObjectReturner, GameObject theObjectDoingTheEnaction, interType interTypeX)
    {

        theRepeater = new simpleExactRepeatOfPerma(new permaPlan2(equipX2(theObjectDoingTheEnaction)));
    }
    */


    public override void refill()
    {
        theRepeater.refill();
    }

    public override void doThisThing()
    {
        theRepeater.doThisThing();
    }


    private singleEXE equipX2(GameObject theObjectDoingTheEnaction)//, interType interTypeX)
    {
        GameObject theItemWeWant = null; //for now.....//theIndividualObjectReturner.returnObject();//new find().objectInObjectsInventoryWithIntertypeX(theObjectDoingTheEnaction, interTypeX);//firstObjectOnListWIthInterTypeX(interTypeX, getInventory());

        //IEnactaBool testE1 = new takeFromAndPutBackIntoInventory(this.gameObject);

        IEnactaBool testE1 = theObjectDoingTheEnaction.GetComponent<takeFromAndPutBackIntoInventory>();

        //planEXE2 exe1 = new singleEXE(testE1, theItemWeWant);
        singleEXE exe1 = (singleEXE)testE1.toEXE(theItemWeWant);
        exe1.atLeastOnce();
        //condition thisCondition = new enacted(exe1);
        //exe1.endConditions.Add(thisCondition);
        return exe1;
    }

    public override string baseEnactionsAsText()
    {
        return theRepeater.baseEnactionsAsText();
    }
}





public class goToXAndInteractWithY
{
    repeatWithTargetPicker theRepeater;

    public goToXAndInteractWithY(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, interType theIntertypeY, float proximity)
    {
        theRepeater = thinggggg(theObjectDoingTheEnactions,thingXToGoTo,theIntertypeY,proximity);
    }

    public repeatWithTargetPicker returnIt()
    {
        return theRepeater;
    }

    private repeatWithTargetPicker thinggggg(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, interType theIntertypeY, float proximity)
    {

        //targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions,
        //    new setOfAllNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(
            //genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions,
                //thingXToGoTo.pickNext().realPositionOfTarget(), //messy
                //proximity),
                //new aimTargetPlanGen(theObjectDoingTheEnactions, thingXToGoTo).returnIt(), //messy
                interactWithType(theObjectDoingTheEnactions, theIntertypeY));
        
        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, thingXToGoTo);


        return repeatWithTargetPickerTest;
    }
    public singleEXE interactWithType(GameObject theObjectDoingTheEnactions, interType interTypeX)
    {
        //have to see any available interactions:
        //where?
        //          in current "equipped" enactions in gamepad
        //          in inventory [at which point, need to equip it!  ..............doesn't fit current "singleEXE" paradigm.....
        //just add a step before this in the plan that equips/preps IF necessary???
        //
        //what kind of enaction?
        //      ranged
        //          hitscan
        //          non-hitscan
        //      [no others currently?]
        
        enaction theInteractionEnaction = new find().enactionWithInteractionX(theObjectDoingTheEnactions,interTypeX);
        if (theInteractionEnaction != null)
        {
            singleEXE theSingle = (singleEXE)theInteractionEnaction.standardEXEconversion();
            theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

            return theSingle;
        }


        //if(theInteractionEnaction == null) { return null; }//hmmmmmmmm


        inventory1 theirInventory = theObjectDoingTheEnactions.GetComponent<inventory1>();
        if (theirInventory == null) { return null; }  //or return "go find"?
        
        foreach(GameObject thisObject in theirInventory.inventoryItems)
        {
            theInteractionEnaction = new find().enactionWithInteractionX(thisObject, interTypeX);
            if (theInteractionEnaction != null)
            {
                singleEXE theSingle = (singleEXE)theInteractionEnaction.standardEXEconversion();
                theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

                return theSingle;
            }
            //foreach (collisionEnaction thisEnaction in listOfCollisionEnactionsOnObject(theObject))
            {
                //Debug.Log(thisEnaction.interInfo.interactionType);
                //if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
            }
        }
        



        return null;
    }


    
}


public class goToX
{
    repeatWithTargetPicker theRepeater;

    public goToX(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, float proximity)
    {
        theRepeater = thinggggg(theObjectDoingTheEnactions, thingXToGoTo, proximity);
    }

    public repeatWithTargetPicker returnIt()
    {
        return theRepeater;
    }

    private repeatWithTargetPicker thinggggg(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, float proximity)
    {

        //targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions,
        //    new setOfAllNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

        //Debug.Assert(genGen.singleton != null);
        //Debug.Assert(thingXToGoTo != null);
        //Debug.Assert(thingXToGoTo.pickNext() != null);
        //Debug.Assert(thingXToGoTo.pickNext().realPositionOfTarget() != null);

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(
                genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions,
                theObjectDoingTheEnactions,
                //thingXToGoTo.pickNext().realPositionOfTarget(), //messy
                proximity)
                //new aimTargetPlanGen(theObjectDoingTheEnactions, thingXToGoTo).returnIt(), //messy
                //interactWithType(theObjectDoingTheEnactions, theIntertypeY)
                );

        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, thingXToGoTo);


        return repeatWithTargetPickerTest;
    }
    


}

public class giveXRTSTargetsToYUnits: repeatWithTargetPicker
{
    targetPicker rtsTargetPicker;
    objectSetGrabber unitsToGiveOrdersTo;

    public giveXRTSTargetsToYUnits(permaPlan2 thePermaIn, targetPicker theTargetPickerIn) : base(thePermaIn, theTargetPickerIn)//?????
    {
        rtsTargetPicker = theTargetPickerIn;

    }

    //agnosticTargetCalc rtsTarget
    public void MOREINFOgiveXRTSTargetsToYUnits(objectSetGrabber unitsToGiveOrdersToIn)
    {
        unitsToGiveOrdersTo = unitsToGiveOrdersToIn;
    }


    public override void doThisThing() //a bit of a kludge, but maybe good???
    {
        //Debug.Log("?????????????????????????????????????");
        agnosticTargetCalc theTarget = rtsTargetPicker.pickNext();

        //Debug.Log("unitsToGiveOrdersTo.grab().Count:  " + unitsToGiveOrdersTo.grab().Count);
        foreach (GameObject unit in unitsToGiveOrdersTo.grab())
        {
            rtsModule theComponent = unit.GetComponent<rtsModule>();
            theComponent.currentReceivedOrders = theTarget;
        }
    }

    /*
    private repeatWithTargetPicker thinggggg(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, float proximity)
    {

        //targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions,
        //    new setOfAllNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(
                genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions,
                thingXToGoTo.pickNext().realPositionOfTarget(), //messy
                proximity)
                //new aimTargetPlanGen(theObjectDoingTheEnactions, thingXToGoTo).returnIt(), //messy
                //interactWithType(theObjectDoingTheEnactions, theIntertypeY)
                );

        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, thingXToGoTo);


        return repeatWithTargetPickerTest;
    }
    */


}

//public class applePattern  //no!  just swap out TARTGETpICKER and use it in "goToX"


public class aimAtXAndInteractWithY
{
    repeatWithTargetPicker theRepeater;

    public aimAtXAndInteractWithY(GameObject theObjectDoingTheEnactions, targetPicker thingXToAimAt, interType theIntertypeY, float proximity)
    {
        theRepeater = thinggggg(theObjectDoingTheEnactions, thingXToAimAt, theIntertypeY, proximity);
    }

    public repeatWithTargetPicker returnIt()
    {
        return theRepeater;
    }

    private repeatWithTargetPicker thinggggg(GameObject theObjectDoingTheEnactions, targetPicker thingXToAimAt, interType theIntertypeY, float proximity)
    {

        //targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions,
        //    new setOfAllNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(
                //genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions,
                //thingXToGoTo.pickNext().realPositionOfTarget(), //messy
                //proximity),
                new aimTargetPlanGen(theObjectDoingTheEnactions, thingXToAimAt).returnIt(), //messy
                interactWithType(theObjectDoingTheEnactions, theIntertypeY));

        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, thingXToAimAt);


        return repeatWithTargetPickerTest;
    }



    public singleEXE interactWithType(GameObject theObjectDoingTheEnactions, interType interTypeX)
    {
        //have to see any available interactions:
        //where?
        //          in current "equipped" enactions in gamepad
        //          in inventory [at which point, need to equip it!  ..............doesn't fit current "singleEXE" paradigm.....
        //just add a step before this in the plan that equips/preps IF necessary???
        //
        //what kind of enaction?
        //      ranged
        //          hitscan
        //          non-hitscan
        //      [no others currently?]

        enaction theInteractionEnaction = new find().enactionWithInteractionX(theObjectDoingTheEnactions, interTypeX);
        if (theInteractionEnaction != null)
        {
            singleEXE theSingle = (singleEXE)theInteractionEnaction.standardEXEconversion();
            theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

            return theSingle;
        }


        //if(theInteractionEnaction == null) { return null; }//hmmmmmmmm


        inventory1 theirInventory = theObjectDoingTheEnactions.GetComponent<inventory1>();
        if (theirInventory == null) { return null; }  //or return "go find"?

        foreach (GameObject thisObject in theirInventory.inventoryItems)
        {
            theInteractionEnaction = new find().enactionWithInteractionX(thisObject, interTypeX);
            if (theInteractionEnaction != null)
            {
                singleEXE theSingle = (singleEXE)theInteractionEnaction.standardEXEconversion();
                theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

                return theSingle;
            }
            //foreach (collisionEnaction thisEnaction in listOfCollisionEnactionsOnObject(theObject))
            {
                //Debug.Log(thisEnaction.interInfo.interactionType);
                //if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
            }
        }




        return null;
    }

}



public class aimAtXAndPressY
{
    repeatWithTargetPicker theRepeater;

    public aimAtXAndPressY(GameObject theObjectDoingTheEnactions, targetPicker thingXToAimAt, buttonCategories buttonY, float proximity)
    {
        theRepeater = thinggggg(theObjectDoingTheEnactions, thingXToAimAt, buttonY, proximity);
    }

    public repeatWithTargetPicker returnIt()
    {
        return theRepeater;
    }

    private repeatWithTargetPicker thinggggg(GameObject theObjectDoingTheEnactions, targetPicker thingXToAimAt, buttonCategories buttonY, float proximity)
    {

        //targetPicker getter = new nearestTargetPickerExceptSelf(theObjectDoingTheEnactions,
        //    new setOfAllNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(
                //genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions,
                //thingXToGoTo.pickNext().realPositionOfTarget(), //messy
                //proximity),
                new aimTargetPlanGen(theObjectDoingTheEnactions, thingXToAimAt).returnIt(), //messy
                pressY(theObjectDoingTheEnactions, buttonY));

        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, thingXToAimAt);


        return repeatWithTargetPickerTest;
    }



    public singleEXE pressY(GameObject theObjectDoingTheEnactions, buttonCategories buttonY)
    {
        //have to see any available interactions:
        //where?
        //          in current "equipped" enactions in gamepad
        //          in inventory [at which point, need to equip it!  ..............doesn't fit current "singleEXE" paradigm.....
        //just add a step before this in the plan that equips/preps IF necessary???
        //
        //what kind of enaction?
        //      ranged
        //          hitscan
        //          non-hitscan
        //      [no others currently?]

        virtualGamepad theVirtualGamepad = theObjectDoingTheEnactions.GetComponent<virtualGamepad>();

        enaction theEnaction = theVirtualGamepad.allCurrentBoolEnactables[buttonY];//new find().enactionWithInteractionX(theObjectDoingTheEnactions, interTypeX);
        if (theEnaction != null)
        {
            singleEXE theSingle = (singleEXE)theEnaction.standardEXEconversion();
            theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

            return theSingle;
        }
        else
        {
            Debug.Log("theEnaction in gamepad button = null");
            //return null;
        }


        inventory1 theirInventory = theObjectDoingTheEnactions.GetComponent<inventory1>();
        if (theirInventory == null) { return null; }  //or return "go find"?

        foreach (GameObject thisObject in theirInventory.inventoryItems)
        {
            theEnaction = new find().enactionWithButtonX(thisObject, buttonY);
            if (theEnaction != null)
            {
                singleEXE theSingle = (singleEXE)theEnaction.standardEXEconversion();
                theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

                return theSingle;
            }
            //foreach (collisionEnaction thisEnaction in listOfCollisionEnactionsOnObject(theObject))
            {
                //Debug.Log(thisEnaction.interInfo.interactionType);
                //if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
            }
        }
        //if(theInteractionEnaction == null) { return null; }//hmmmmmmmm

        /*
        inventory1 theirInventory = theObjectDoingTheEnactions.GetComponent<inventory1>();
        if (theirInventory == null) { return null; }  //or return "go find"?

        foreach (GameObject thisObject in theirInventory.inventoryItems)
        {
            theEnaction = new find().enactionWithInteractionX(thisObject, interTypeX);
            if (theEnaction != null)
            {
                singleEXE theSingle = (singleEXE)theEnaction.standardEXEconversion();
                theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

                return theSingle;
            }
            //foreach (collisionEnaction thisEnaction in listOfCollisionEnactionsOnObject(theObject))
            {
                //Debug.Log(thisEnaction.interInfo.interactionType);
                //if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
            }
        }

        */



        return null;
    }

}
