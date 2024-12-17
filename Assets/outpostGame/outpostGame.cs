using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.WSA;
using static enactionCreator;
using static interactionCreator;
using static tagging2;

public class outpostGame : MonoBehaviour
{

    List<hoardeWaveGen> hoardes = new List<hoardeWaveGen>();



    // Start is called before the first frame update
    void Start()
    {
        genGen.singleton.returnGun1(new Vector3(-2, 0.7f, 6));



        List<Vector3> listOfSpawnPoints = new List<Vector3>();
        listOfSpawnPoints.Add(new Vector3(30,0,20));
        listOfSpawnPoints.Add(new Vector3(45,0,-20));
        listOfSpawnPoints.Add(new Vector3(-25,0,25));
        listOfSpawnPoints.Add(new Vector3(-30,0,-30));


        hoardes.Add(new hoardeWaveGen(tag2.team2, listOfSpawnPoints));
        hoardes.Add(new hoardeWaveGen(tag2.team3, listOfSpawnPoints));
        hoardes.Add(new hoardeWaveGen(tag2.team4, listOfSpawnPoints));


        /*

        int number = 0;
        while(number < 3)
        {

            new basicSoldierGenerator(tag2.team2).doIt(new Vector3(7, 0, 10 * number));
            new basicSoldierGenerator(tag2.team3).doIt(new Vector3(7, 0, -10 * number));
            number++;
        }
        */


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("---------------------------------------------------------");
        foreach (hoardeWaveGen hoard in hoardes)
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

    List<objectSetInstantiator> listOfWaves = new List<objectSetInstantiator>();

    List<Vector3> listOfSpawnPoints = new List<Vector3>();



    public hoardeWaveGen(tag2 teamIn, List<Vector3> listOfSpawnPointsIn)
    {
        team = teamIn;
        listOfSpawnPoints = listOfSpawnPointsIn;

        newWaveCondition = zeroRemainingTeamMembers();
        listOfWaves = testWaves();
    }

    private List<objectSetInstantiator> testWaves()
    {
        List<objectSetInstantiator> newList = new List<objectSetInstantiator>();


        objectSetInstantiator o1 = new objectSetInstantiator(new objectGen[] { new basicSoldierGeneratorG(team) });
        objectSetInstantiator o2 = new objectSetInstantiator(new objectGen[] { new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team) });
        objectSetInstantiator o3 = new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33), new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33), new basicPaintByNumbersSoldierGeneratorG(team, 5, 4, 1.3f, 13, 99) });
        objectSetInstantiator o4 = new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.5f, 1.9f, 9, 33), new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team) });
        objectSetInstantiator o5 = new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 11, 3.8f, 2.4f, 2, 99), new basicPaintByNumbersSoldierGeneratorG(team, 5, 4, 1.3f, 13, 99), new basicSoldierGeneratorG(team), new basicPaintByNumbersSoldierGeneratorG(team, 5, 4, 1.3f, 13, 99), new basicSoldierGeneratorG(team) });

        newList.Add(o1);
        newList.Add(o2);
        newList.Add(o3);
        newList.Add(o4);
        newList.Add(o5);

        return newList;
    }

    public void doOnUpdate()
    {
        if (newWaveCondition.met())
        {
            currentWaveNumber++;
            generateNextWave();
        }
    }









    public void generateNextWave()
    {
        //currentWaveNumber starts at 1
        Debug.Log("current and count:  "+ currentWaveNumber+", "+ listOfWaves.Count);
        if(currentWaveNumber-1 < listOfWaves.Count)
        {

            listOfWaves[currentWaveNumber -1].generate(pickRandomSpawnPoint(listOfSpawnPoints));
            return;
        }

        listOfWaves[currentWaveNumber - listOfWaves.Count-1].generate(pickRandomSpawnPoint(listOfSpawnPoints));
        listOfWaves[currentWaveNumber - listOfWaves.Count].generate(pickRandomSpawnPoint(listOfSpawnPoints));
    }

    private Vector3 pickRandomSpawnPoint(List<Vector3> listOfSpawnPoints)
    {
        return listOfSpawnPoints[repository2.singleton.pickRandomInteger(listOfSpawnPoints.Count-1)];
    }




    condition zeroRemainingTeamMembers()
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad()
            //new objectHasTag(team)
            //new proximityCriteriaBool(thePlayer?, 25)
            );


        objectSetGrabber theTeamObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsWithTag(team), theCriteria);

        condition theCondition =new reverseCondition( new stickyCondition(new isThereAtLeastOneObjectInSet(theTeamObjectSet), 120));// theObjectDoingTheEnaction, numericalVariable.health);

        return theCondition;
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
        //this.weapon = weapon;
        //this.theBehavior = theBehavior;
        thePosition = new Vector3();
    }

    public GameObject generate()
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thePosition, Quaternion.identity);
        GameObject torso = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thePosition, Quaternion.identity);

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
        GameObject gun = genGen.singleton.returnGun1(newObj.transform.position);
        theirInventory.inventoryItems.Add(gun);
        interactionCreator.singleton.dockXToY(gun, newObj);




        FSMcomponent theFSMcomponent = newObj.AddComponent<FSMcomponent>();
        theFSMcomponent.theFSMList = new basicPaintByNumbersSoldierFSM(newObj,team,speed,targetDetectionRange).returnIt();//theBehavior;

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
        objectSetGrabber theAttackObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 110);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        FSM combat1 = new generateFSM(new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, targetDetectionRange).returnIt());



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

        targetPicker theAttackTargetPicker = new pickNearest(theObjectDoingTheEnaction, theAttackObjectSet);

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

        objectSetGrabber theAttackObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);

        //targetPicker theAttackTargetPicker = new pickNearest(theObjectDoingTheEnaction, theAttackObjectSet);

        //targetPicker theTargetPicker = new applePatternTargeter(theObjectDoingTheEnaction, theAttackObjectSet);
        targetPicker theTargetPicker = new combatDodgeVarietyPack1(theObjectDoingTheEnaction, theAttackObjectSet);

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
        thePosition = new Vector3();
    }

    public GameObject generate()
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thePosition, Quaternion.identity);
        GameObject torso = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thePosition, Quaternion.identity);

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

        thePlayable.dictOfIvariables[numericalVariable.health] = 2;
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

        return newObj;
    }



}






public class FSMcomponent:MonoBehaviour,IupdateCallable
{
    //public FSM theFSM;
    public List<FSM> theFSMList;  //correct way to do parallel!  right at the top level!!!  one for walking/feet, one for hands/equipping/using items etc.

    public List<IupdateCallable> currentUpdateList {  get; set; }
    public void Update()
    {

        //Debug.Log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&     regular Update()        &&&&&&&&&&&&&&&&&&&");
    }

    public void callableUpdate()
    {
        //Debug.Log("============================     callableUpdate()        =================");
        foreach (FSM theFSM in theFSMList)
        {
            //Debug.Log("theFSM:  "+ theFSM);
            //theFSM = theFSM.doAFrame();  //will this be an issue?  or only if i add/remove items from list?
        }

        for (int index = 0; index < theFSMList.Count; index++)
        {
            theFSMList[index] = theFSMList[index].doAFrame();
        }

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
        objectSetGrabber theAttackObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);
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
        
        targetPicker theAttackTargetPicker = new pickNearest(theObjectDoingTheEnaction, theAttackObjectSet);

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

        objectSetGrabber theAttackObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);

        //targetPicker theAttackTargetPicker = new pickNearest(theObjectDoingTheEnaction, theAttackObjectSet);

        //targetPicker theTargetPicker = new applePatternTargeter(theObjectDoingTheEnaction, theAttackObjectSet);
        targetPicker theTargetPicker = new combatDodgeVarietyPack1(theObjectDoingTheEnaction, theAttackObjectSet);

        FSM combat1 = new generateFSM(new goToX(theObjectDoingTheEnaction, theTargetPicker, combatRange).returnIt());

        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        wander.addSwitchAndReverse(switchToAttack, combat1);

        wander.name = "feet, wander";
        combat1.name = "feet, combat1";
        return wander;
    }




    public List<FSM> returnIt()
    {
        return theFSMList;
    }



}

public class combatDodgeVarietyPack1 : targetPicker
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

    public combatDodgeVarietyPack1(GameObject theObjectDoingTheEnaction, objectSetGrabber theSetInput)
    {

        listOfTargetPickers.Add(new applePatternTargeter(theObjectDoingTheEnaction, theSetInput));
        listOfTargetPickers.Add(new radialFleeingTargeter(theObjectDoingTheEnaction, theSetInput));
        listOfTargetPickers.Add(new pickNearest(theObjectDoingTheEnaction, theSetInput));  //this is "go TOWARD" i think
        listOfTargetPickers.Add(new pickRandomNearbyLocation(theObjectDoingTheEnaction));
        //how to do "idle"?
    }

    public override agnosticTargetCalc pickNext()
    {
        randomlyPickDifferentStrategySometimes();
        Debug.Log("currentPick:  " + currentPick);



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
            currentPick = repository2.singleton.pickRandomInteger(listOfTargetPickers.Count);

            behaviorChangeCountdownTimeLIMIT = repository2.singleton.pickRandomInteger(270) + 110;

        }
        else
        {
            currentlyDoingMainBehavior = true;
            currentPick = 0;

            behaviorChangeCountdownTimeLIMIT = repository2.singleton.pickRandomInteger(300) + 210;
        }

        behaviorChangeCountdownTimeCurrent = 0;
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







        objectSetGrabber theEquippedObjectsGrabber = new equippedObjectsGrabber(theObjectDoingTheEnaction);
        //objectCriteria hasInterTypeX = new intertypeXisOnObject(interTypeX);
        pickFirstObjectXFromListY theEquippedObjectPicker = new pickFirstObjectXFromListY(hasInterTypeX, theEquippedObjectsGrabber);


        condition hasEquipped = new nonNullObject(theEquippedObjectPicker);








        //      has item we want in inventory [AND we want to CACHE that item if possible]
        //condition:  object returner returns non-null object
        //object returner [main]:  object on list that meets criteria
        //          the list:   inventory on [set?] object
        //          the criteria: has intertypeX
        //caching etc
        //  plug cache setter into condition checker [so, wrap the above returner]
        //  have cache observer as a variable we can plug into "equipping" enaction

        inventoryGrabber theInvGrabber = new inventoryGrabber(theObjectDoingTheEnaction);
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

        //targetPicker getter = new pickNearestExceptSelf(theObjectDoingTheEnactions,
        //    new allNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

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

        //targetPicker getter = new pickNearestExceptSelf(theObjectDoingTheEnactions,
        //    new allNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

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

        //targetPicker getter = new pickNearestExceptSelf(theObjectDoingTheEnactions,
        //    new allNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

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