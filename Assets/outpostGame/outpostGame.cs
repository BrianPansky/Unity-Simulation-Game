using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SocialPlatforms;
using UnityEngine.WSA;
using UnityEngine.XR;
using static enactionCreator;
using static interactionCreator;
using static tagging2;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class outpostGame : MonoBehaviour
{

    List<hoardeWaveGen> hoardes = new List<hoardeWaveGen>();



    // Start is called before the first frame update
    void Start()
    {

        //new makeTestBase(tag2.team3).doIt(new Vector3(-705, 1, -325));

        genGen.singleton.returnShotgun1(new Vector3(-7, 1, -6));

        /*
        new makeTestLeader(tag2.team2).doIt(new Vector3(430, 1, 30));
        new makeTestLeader(tag2.team4).doIt(new Vector3(420, 1, 30));
        new makeTestLeader(tag2.team5).doIt(new Vector3(-430, 1, -20));
        new makeTestLeader(tag2.team6).doIt(new Vector3(430, 1, -20));
        new makeTestLeader(tag2.team7).doIt(new Vector3(430, 1, -20));
        new makeTestLeader(tag2.team8).doIt(new Vector3(430, 1, -20));
        */

        //new makeTestLeader(tag2.team3).doIt(new Vector3(-430, 1, -30));
        /*
        new makeTestLeader(tag2.team4).doIt(new Vector3(420, 1, 30));
        new makeTestLeader(tag2.team5).doIt(new Vector3(-430, 1, -20));
        new makeTestLeader(tag2.team6).doIt(new Vector3(430, 1, -20));
        new makeTestLeader(tag2.team7).doIt(new Vector3(430, 1, -20));
        new makeTestLeader(tag2.team8).doIt(new Vector3(430, 1, -20));
        */
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






public class randomHidingLocationTargetPicker : targetPicker
{
    //randomly pick from sampled locations where they won't be seen

    //bool currentlyDoingMainBehavior = true;
    List<Vector3> nearbyHidingPositions = new List<Vector3>();
    
    GameObject objectToBeNear;
    float spreadFactor = 1.0f;
    visibleToThreatSet visibilityCalc;

    private tag2 team;

    public randomHidingLocationTargetPicker(GameObject objectToBeNearIn, tag2 teamIn, float spreadFactorIn = 1f)
    {
        objectToBeNear = objectToBeNearIn;
        this.team = teamIn;
        spreadFactor = spreadFactorIn;
        visibilityCalc = new visibleToThreatSet(objectToBeNearIn, threatSet(objectToBeNearIn));//objectToBeNearIn.GetComponent<beleifs>().beleifObjectSets[0]); //very ad-hoc
    }

    private objectSetGrabber threatSet(GameObject theObject)
    {
        //return theObjectDoingTheEnaction.GetComponent<beleifs>().threatMarkerSet;

        //so:
        //      for feet, use shadow object markers
        //      that are "threats" [meet threat criteria]
        //ALSO this is where i CREATE the "beleif set", beleif component just UPDATES it....

        updateableSetGrabber threatObjectPermanence = new objectsMeetingCriteriaBeleifSet1(theObject,
            new objectMeetsAllCriteria(
                new hasVirtualGamepad(),
                new reverseCriteria(new objectHasTag(team))
                ));

        beleifs theComponent = theObject.GetComponent<beleifs>();
        theComponent.beleifObjectSets.Add(threatObjectPermanence);

        //objectSetGrabber theGrabber = threatObjectPermanence;//????i think???? //new setOfAllObjectThatMeetCriteria(threatObjectPermanence);
        return threatObjectPermanence;//????i think????
    }

    public override agnosticTargetCalc pickNext()
    {

        //Vector3 target = patternScript2.singleton.randomNearbyVector(objectToBeNear.transform.position, spreadFactor);
        //Debug.Log(target);
        //agnosticTargetCalc targ = new agnosticTargetCalc(objectToBeNear, target);
        //return targ;

        Vector3 hidingSpot = new Vector3();
        int tries = 25;
        while(tries > 0)
        {
            hidingSpot = patternScript2.singleton.randomNearbyVector(objectToBeNear.transform.position, spreadFactor);

            if (thisIsAnUndetectableLocation(hidingSpot)==false) { return new agnosticTargetCalc(hidingSpot); }

            tries--;
        }

        return new agnosticTargetCalc(new Vector3(-1000,0,-1500)); //just to be obvious

        //int index = UnityEngine.Random.Range(0, nearbyHidingPositions.Count-1);
        //return new agnosticTargetCalc(nearbyHidingPositions[index]);
    }

    private bool thisIsAnUndetectableLocation(Vector3 hidingSpot)
    {
        return visibilityCalc.sampleOnePoint(hidingSpot);
    }

    internal void updateSetOfNearbyHidingPositions()
    {
        //so:
        //      sample field of points
        //the data:
        //      [armature?] line of sight to:
        //          object permanence threat marker set
        //          lights
        //[and if those are both "true"]:
        //      illumination level [or bool?]
        //                  spatialDataSet myData = new spatialDataSet();

    }
}








public class playerTeamGen : doAtPoint
{
    //So, for each team I need the following
    //      a base LOCATION
    //      a base object that is properly tagged
    //      a leader
    //      a hoarde wave callable update component  monobehavior I think? that can generate the waves at the correct times
    //      I need two of those.or I need two different ways and two different conditions.so two of those probably.
    //  one for attackers one for defenders.
    //      Attackers need to be tagged as attackers, Defenders need to be tagged as Defenders
    //      each of them needs to be able to receive orders
    //      the leader needs to give attackers and Defenders different orders based on their tagged classification. 
    //      And let's have one type of soldier generator, because I hate having to update two of them
    //  and check two of them and everything. just have one. [that i can somehow plug in the "attacker" or "defender" tags]

    tag2 team = tag2.team1;


    internal override void doIt(Vector3 thisPoint)
    {
        GameObject baseMarker = new makeBaseMarker(team).doIt(thisPoint);
        new makeTestRadar(team, thisPoint);
        new makeTestAircraft(thisPoint + new Vector3(9, 1, 7));
        makePLAYER(new Vector3(-5, 1, -1)+ thisPoint);

        /*
        new makeLeader(team).doIt(thisPoint * 8);
        List<objectSetInstantiator> attackerWaveSet = new makeWaveList(team, tag2.attackSquad).returnWaves();
        List<objectSetInstantiator> defenderWaveSet = new makeWaveList(team, tag2.defenseSquad).returnWaves();
        Dictionary<condition, List<objectSetInstantiator>> wavesAndConditionDict = new Dictionary<condition, List<objectSetInstantiator>>();
        condition attackerRespawnCondition = noUnitsWithThisTag(tag2.attackSquad);
        condition defenderRespawnCondition = noUnitsWithThisTag(tag2.defenseSquad);
        wavesAndConditionDict[attackerRespawnCondition] = attackerWaveSet;
        wavesAndConditionDict[defenderRespawnCondition] = defenderWaveSet;
        callableThatGeneratesWavesWhenTheirConditionIsMet.addThisComponent(baseMarker, relativeSpawnPoints(), wavesAndConditionDict); //can assume condition just checks their tags?  but then.....need to input those tags, or else hard wire them....
        */

    }
    
    void makePLAYER(Vector3 location)
    {
        GameObject player = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.player, location);
        //genGen.singleton.addBody4ToObject(player);
        addTestBodyToObject(player);
        tagging2.singleton.addTag(player, team);
        tagging2.singleton.addTag(player, tag2.player);
        //Debug.Log("???????????????????????????????????????????????");
        worldScript.singleton.thePlayer = player;
        //tagging2.singleton.addTag(player, tag2.defenseSquad);
        //addTeamColors(newObj);
        //addWeapon(newObj, weapon);

        stealthModule stealthModule = player.AddComponent<stealthModule>();
        stealthModule.requestingStealth = true;

        GameObject testPart1 = genGen.singleton.addCube(player, 0.4f, 0.6f, 1.5f, -1.2f); //repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);
        GameObject testPart2 = genGen.singleton.addCube(player, 0.4f, 1.1f, 0.3f, -0.4f);
        GameObject testPart3 = genGen.singleton.addCube(player, 0.4f, -0.6f, 1.0f, 1.2f);

        
        stealthArmature.addThisComponent(player, testPart1, testPart2, testPart3);

    }

    public void addTestBodyToObject(GameObject newObj)
    {
        tagging2.singleton.addTag(newObj, tagging2.tag2.threat1);
        playable2 thePlayable = newObj.AddComponent<playable2>();


        //thePlayable.dictOfInteractions = new Dictionary<enactionCreator.interType, List<Ieffect>>();
        //thePlayable.dictOfIvariables = new Dictionary<interactionCreator.numericalVariable, float>();

        thePlayable.dictOfIvariables[numericalVariable.health] = 2;
        thePlayable.equipperSlotsAndContents[interactionCreator.simpleSlot.hands] = null;



        thePlayable.initializeEnactionPoint1();

        genGen.singleton.addArrowForward(thePlayable.enactionPoint1);
        genGen.singleton.addCube(thePlayable.enactionPoint1, 0.1f);

        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform, new Vector3(0, 2, 0));

        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);
        makeBasicEnactionssss(thePlayable);
        //genGen.singleton.makeInteractionsBody4(thePlayable);


        inventory1 theirInventory = newObj.AddComponent<inventory1>();
    }
    public void makeBasicEnactionssss(playable2 thePlayable)
    {
        hitscanEnactor.addHitscanEnactor(thePlayable.gameObject, thePlayable.enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.standardClick));


        vecTranslation.addVecTranslation(thePlayable.gameObject, thePlayable.speed, buttonCategories.vector1);

        navAgent.addNavAgentEnaction(thePlayable.gameObject);

        aimTarget.addAimTargetAndVecRotation(thePlayable.gameObject, thePlayable.lookSpeed, thePlayable.transform, thePlayable.enactionPoint1.transform, buttonCategories.vector2);
    }

    private List<Vector3> relativeSpawnPoints()
    {
        List<Vector3> listOfOffsetSpawnLocations = new List<Vector3>();
        listOfOffsetSpawnLocations.Add(new Vector3(5, 0, 5));
        listOfOffsetSpawnLocations.Add(new Vector3(-5, 0, 3));
        listOfOffsetSpawnLocations.Add(new Vector3(-7, 0, -4));
        listOfOffsetSpawnLocations.Add(new Vector3(6, 0, -5));

        return listOfOffsetSpawnLocations;
    }

    private condition noUnitsWithThisTag(tag2 unitGroupIn)
    {

        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new objectHasTag(unitGroupIn),
            new reverseCriteria(new objectHasTag(tag2.teamLeader))
            );



        objectSetGrabber theTeamObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), theCriteria);

        condition theCondition = new reverseCondition(new isThereAtLeastOneObjectInSet(theTeamObjectSet));

        return theCondition;
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

}




public class makeTestAircraft
{
    public makeTestAircraft(Vector3 thisPoint)
    {
        /*
        -------------------------------------------------------------
        object "screen"
        grab set of all virtual gamepad objects
        sort by team?  [no, just use team colors!]
        updating every frame needs a component!  an updater!  maybe callable!
        create little objects[box ? ball ?] with that same color
        get info about their position in space
        scale that position info down
        flatten it into top view
        rotate it up vertical or whatever onto a vertical screen.
        -------------------------------------------------------------
        */

        GameObject aircraft = new GameObject();


        genGen.singleton.addArrowForward(aircraft);
        aircraft.transform.position = thisPoint;// + new Vector3(0, -5, 0);
        aircraft.transform.localScale = new Vector3(28, 28, 28);
        aircraft.name = "aircraft1";

        //aircraft.AddComponent<BoxCollider>();
        //GameObject.Destroy(newObj.GetComponent<Collider>());
        BoxCollider hitbox = aircraft.AddComponent<BoxCollider>();
        //hitbox.size += new Vector3(width-1, height-1, width-1);
        //hitbox.center += new Vector3(0, (height-1)/2, 0);
        hitbox.size = new Vector3(0.1f, 0.1f, 0.3f);
        hitbox.center = new Vector3(0, 0.37f, 0);

        //tagging2.singleton.addTag(aircraft, tag2.zoneable);


        testAircraftComponent.addThisComponent(aircraft);
        new makeImaginary(aircraft);
    }
}

public class testAircraftComponent : playable2
{

    public static testAircraftComponent addThisComponent(GameObject theObject)
    {
        //Debug.Log("-------------------aircraft object has hash code:  " + theObject + ", " + theObject.GetHashCode());
        testAircraftComponent theComponent = theObject.AddComponent<testAircraftComponent>();
        //theComponent.initializeEnactionPoint1();
        //theComponent.initializeEnactionPoint1(new Vector3(0, 0, 0));
        theComponent.initializeCustomEnactionPoint1(theComponent.gameObject, new Vector3(0, 10, 12));
        theComponent.initializeCameraMount(theComponent.enactionPoint1.transform, new Vector3(0,0,-5));






        //theComponent.theVerticalRotationTransform = theObject.transform;







        //addAirplaneFlightModel();

        theComponent.dictOfIvariables[numericalVariable.health] = 13;
        //thePlayable.equipperSlotsAndContents[simpleSlot.hands] = null;

        addEnactions(theComponent);
        addInteractions(theComponent);


        Debug.Log("-------------------aircraft maker END-----------------------------" + theObject + ", " + theObject.GetHashCode());
        return theComponent;
    }

    private static void addInteractions(testAircraftComponent theComponent)
    {
        interactionCreator.singleton.addInteraction(theComponent.gameObject, interType.standardClick, new occupyPlayable(theComponent));

        genGen.createWeaponLevels(theComponent, interType.peircing, 0, 4);
        genGen.createWeaponLevels(theComponent, interType.melee, 0, 4);
        //createWeaponLevels(theInteractable, interType.shootFlamethrower1, 0, 5);
        //createWeaponLevels(theInteractable, interType.tankShot, 0, 0);

        genGen.singleton.deathWhenHealthIsZero(theComponent);
    }

    private static void addEnactions(testAircraftComponent theComponent)
    {
        //vecTranslation.addVecTranslation(theComponent.gameObject, theComponent.speed, buttonCategories.vector1);
        airplaneMovementTest3.addThisComponent(theComponent.gameObject, theComponent.enactionPoint1.transform ,buttonCategories.vector1);

        aimTarget.addAimTargetAndVecRotation(theComponent.gameObject, theComponent.lookSpeed, theComponent.transform, theComponent.enactionPoint1.transform, buttonCategories.vector2);
        theComponent.addGun(theComponent);
    }

    public void addGun(playable2 theComponent, float magnitudeOfInteraction = 1f, int level = 0, bool sdOnCollision = true, float speed = 5f)
    {
        theComponent.dictOfIvariables[numericalVariable.cooldown] = 0f;

        /*
        projectileLauncher theShooter = new projectileLauncher(theEquippable.enactionPoint1.transform, 
            buttonCategories.errorYouDidntSetEnumTypeForBUTTONCATEGORIES, 
            new interactionInfo(interType.peircing, magnitudeOfInteraction, level),
            new projectileToGenerate(speed, sdOnCollision, 999, 0));
        */


        //bit messy?  made "thisButtonCategoryIntentionallyLeftBlank" so that i can add component to object [thus easy search object for component of that type] WITHOUT having it plug into a gamepad button when equipped......
        projectileLauncher.addProjectileLauncher(theComponent.transform.gameObject,
            theComponent.enactionPoint1.transform,
            buttonCategories.thisButtonCategoryIntentionallyLeftBlank,
            new interactionInfo(interType.peircing, magnitudeOfInteraction, level),
            new projectileToGenerate(speed, sdOnCollision, 99, 0),
            20);

        numericalEffect(theComponent, numericalVariable.cooldown, 40);

        projectileLauncher theShooter = theComponent.transform.gameObject.GetComponent<projectileLauncher>();
        enactEffect theFiringEffectOnCooldown = theComponent.transform.gameObject.GetComponent<enactEffect>();

        //IEnactaBool theFiringEffectOnCooldown = enactEffect.returnEnactEffect(new numericalEffect(theEquippable, numericalVariable.cooldown));





        //Debug.Assert(enactEffect.returnEnactEffect(new deathEffect(theEquippable.transform.gameObject)) != null);
        //Debug.Assert(theFiringEffectOnCooldown != null);
        Debug.Assert(theFiringEffectOnCooldown != null);

        condition cooldownCondition = new numericalCondition(numericalVariable.cooldown, theComponent.dictOfIvariables);

        //compoundEnactaBool.addCompoundEnactaBool(theEquippable.transform.gameObject, buttonCategories.primary, theShooter, theFiringEffectOnCooldown, cooldownCondition);
        theShooter.linkedEnactionAtoms.Add(theFiringEffectOnCooldown);//messy [but better than the above "compound" nonsense]
        theShooter.gamepadButtonType = buttonCategories.primary;

        /*
        objectGen theFlash =  new genObjectAndModify(new gunFlash(), new objectModifier[] { new simpleMovingMod(0.2f, false, 4) });

        enactEffect theGunFlash = enactEffect.addEnactEffectAndReturn(theEquippable.gameObject, new generateObjectAtLocation(theFlash, theEquippable.enactionPoint1));
        theShooter.linkedEnactionAtoms.Add(theGunFlash);//messy [but better than the above "compound" nonsense]
        


        objectGen theBulletGlow = new genObjectAndModify(new bulletGlow(), new objectModifier[] { new simpleMovingMod(speed, false, 99) });

        enactEffect theGlowEnactEffect = enactEffect.addEnactEffectAndReturn(theEquippable.gameObject, new generateObjectAtLocation(theBulletGlow, theEquippable.enactionPoint1));
        theShooter.linkedEnactionAtoms.Add(theGlowEnactEffect);//messy [but better than the above "compound" nonsense]
        */

        /*
        projectileLauncher.addProjectileLauncher(theEquippable.transform.gameObject,
            theEquippable.enactionPoint1.transform,
            buttonCategories.primary,
            new interactionInfo(interType.peircing, magnitudeOfInteraction, level),
            new projectileToGenerate(speed, sdOnCollision, 999, 0),
            20);

        */

    }
    
    public static void numericalEffect(playable2 theComponent, numericalVariable numVarX, int amountToSubtract = 1)
    {
        enactEffect.addEnactEffect(theComponent.transform.gameObject, new numericalEffect(theComponent, numericalVariable.cooldown, amountToSubtract));
    }

}





public class makeTestRadar
{
    public makeTestRadar(tag2 team, Vector3 thisPoint)
    {
        /*
        -------------------------------------------------------------
        object "screen"
        grab set of all virtual gamepad objects
        sort by team?  [no, just use team colors!]
        updating every frame needs a component!  an updater!  maybe callable!
        create little objects[box ? ball ?] with that same color
        get info about their position in space
        scale that position info down
        flatten it into top view
        rotate it up vertical or whatever onto a vertical screen.
        -------------------------------------------------------------
        */

        GameObject screen = new GameObject();
        screen.AddComponent<BoxCollider>();
        genGen.singleton.addCube(screen);
        screen.transform.position = thisPoint + new Vector3(0, 5, 0);
        screen.transform.localScale = new Vector3(0.1f, 4, 4);
        screen.name = "screen for this team:  " + team.ToString();
        Renderer theRenderer = screen.AddComponent<MeshRenderer>();
        theRenderer.material.color = Color.black;//teamColor;

        tagging2.singleton.addTag(screen, tag2.zoneable);


        testRadarComponent.addThisComponent(screen);
    }
}

public class testRadarComponent : MonoBehaviour, IupdateCallable
{
    public List<IupdateCallable> currentUpdateList { get; set; }
    public objectSetGrabber theGrabber;

    int playerBlipSpacing = 75;
    int playerBlipCountup = 0;

    int callableUpdateSpacing = 7;
    int callableUpdateCountup = 0;

    public static testRadarComponent addThisComponent(GameObject screen)
    {
        
        testRadarComponent theComponent = screen.AddComponent<testRadarComponent>();

        objectCriteria hasVirtualGamepad = new objectMeetsAllCriteria(new hasVirtualGamepad());

        objectSetGrabber setOfAllVirtualGamepadObjects = new setOfAllObjectThatMeetCriteria(
            new COMPUTATIONALLYEXPENSIVEsetOfAllObjectsInNearestXZones(screen, 20),
        hasVirtualGamepad);

        theComponent.theGrabber = setOfAllVirtualGamepadObjects;
        theComponent.playerBlipSpacing = 75/theComponent.callableUpdateSpacing;
        return theComponent;
    }

    public void callableUpdate()
    {
        //Debug.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
        /*
        -------------------------------------------------------------
        object "screen"
        grab set of all virtual gamepad objects
        sort by team?  [no, just use team colors!]
        updating every frame needs a component!  an updater!  maybe callable!
        create little objects[box ? ball ?] with that same color
        get info about their position in space
        scale that position info down
        flatten it into top view
        rotate it up vertical or whatever onto a vertical screen.
        -------------------------------------------------------------
        */
        callableUpdateCountup++;
        if (callableUpdateCountup < callableUpdateSpacing) { return; }
        callableUpdateCountup = 0;
        List<GameObject> allDetectedObjects = theGrabber.grab();
        renderObjectsToScreen(allDetectedObjects);
    }
    public void Update()
    {
        //Debug.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
        /*
        -------------------------------------------------------------
        object "screen"
        grab set of all virtual gamepad objects
        sort by team?  [no, just use team colors!]
        updating every frame needs a component!  an updater!  maybe callable!
        create little objects[box ? ball ?] with that same color
        get info about their position in space
        scale that position info down
        flatten it into top view
        rotate it up vertical or whatever onto a vertical screen.
        -------------------------------------------------------------
        */

        //List<GameObject> allDetectedObjects = theGrabber.grab();
        //renderObjectsToScreen(allDetectedObjects);
    }

    private void renderObjectsToScreen(List<GameObject> ListOfObjects)
    {
        ListOfObjects.Add(tagging2.singleton.allObjectsWithTag(tag2.player)[0]);
        //Debug.Log("ListOfObjects.Count:  "+ListOfObjects.Count);
        foreach (GameObject thisObject in ListOfObjects)
        {
            //Debug.Log(thisObject);
            //tagging2.singleton.printAllTags(thisObject);
            renderOneObjectToScreen(thisObject);
        }
    }

    private void renderOneObjectToScreen(GameObject theRealObject)
    {
        /*
        -------------------------------------------------------------
        create little objects[box ? ball ?] with its team color
        get info about their position in space
        scale that position info down [to the size of the screen]
        flatten it into top view [or could do 3d, lol]
        rotate it up vertical or whatever onto a vertical screen.
        -------------------------------------------------------------
        */


        GameObject theMapMarker = new GameObject();
        theMapMarker.name = "a map marker";
        genGen.singleton.addCube(theMapMarker);
        theMapMarker.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);

        Color teamColor = teamColorFinder(theRealObject);
        Renderer theRenderer = theMapMarker.AddComponent<MeshRenderer>();
        theRenderer.material.color = Color.yellow;//teamColor;

        theMapMarker.AddComponent<selfDestructScript1>();




        //player blip:
        playerBlipCountup++;
        if (playerBlipCountup > playerBlipSpacing && tagging2.singleton.allTagsOnObject(theRealObject).Contains(tag2.player))
        {
            theMapMarker.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
            theMapMarker.GetComponent<selfDestructScript1>().timeUntilSelfDestruct=1;
            playerBlipCountup = 0;
        }



        placeonCorrospondingScreenPosition(theRealObject, theMapMarker);
    }

    private void placeonCorrospondingScreenPosition(GameObject theRealObject, GameObject theMapMarker)
    {
        float realx = theRealObject.transform.position.x;
        float realy = theRealObject.transform.position.y;
        float realz = theRealObject.transform.position.z;
        Vector3 test3dPoint = (new Vector3(-80f, realz, realx) / 180) + (this.gameObject.transform.position)+new Vector3(0,0.6f,0);

        theMapMarker.transform.position = test3dPoint;
    }

    private Color teamColorFinder(GameObject thisObject)
    {
        List<tag2> allTheirTags = tagging2.singleton.allTagsOnObject(thisObject);

        foreach(tag2 thisTag in allTheirTags)
        {
            if (tagging2.singleton.teamColors.Keys.Contains(thisTag))
            {
                return tagging2.singleton.teamColors[thisTag];
            }
        }

        return new Color();
    }

}



public class teamGen:doAtPoint
{
    //So, for each team I need the following
    //      a base LOCATION
    //      a base object that is properly tagged
    //      a leader
    //      a hoarde wave callable update component  monobehavior I think? that can generate the waves at the correct times
    //      I need two of those.or I need two different ways and two different conditions.so two of those probably.
    //  one for attackers one for defenders.
    //      Attackers need to be tagged as attackers, Defenders need to be tagged as Defenders
    //      each of them needs to be able to receive orders
    //      the leader needs to give attackers and Defenders different orders based on their tagged classification. 
    //      And let's have one type of soldier generator, because I hate having to update two of them
    //  and check two of them and everything. just have one. [that i can somehow plug in the "attacker" or "defender" tags]

    tag2 team;

    public teamGen(tag2 teamIn)
    {
        team = teamIn;
    }

    internal override void doIt(Vector3 thisPoint)
    {
        GameObject baseMarker=  new makeBaseMarker(team).doIt(thisPoint);
        new makeLeader(team).doIt(thisPoint * 8);
        List<objectSetInstantiator> attackerWaveSet = new makeWaveList(team, tag2.attackSquad).returnWaves();
        List<objectSetInstantiator> defenderWaveSet = new makeWaveList(team, tag2.defenseSquad).returnWaves();
        Dictionary<condition, List<objectSetInstantiator>> wavesAndConditionDict = new Dictionary<condition, List<objectSetInstantiator>>();
        condition attackerRespawnCondition = noUnitsWithThisTag(tag2.attackSquad);
        condition defenderRespawnCondition = noUnitsWithThisTag(tag2.defenseSquad);
        wavesAndConditionDict[attackerRespawnCondition] = attackerWaveSet;
        wavesAndConditionDict[defenderRespawnCondition] = defenderWaveSet;
        callableThatGeneratesWavesWhenTheirConditionIsMet.addThisComponent(baseMarker, relativeSpawnPoints(), wavesAndConditionDict); //can assume condition just checks their tags?  but then.....need to input those tags, or else hard wire them....
    }

    private List<Vector3> relativeSpawnPoints()
    {
        List<Vector3> listOfOffsetSpawnLocations = new List<Vector3>();
        listOfOffsetSpawnLocations.Add(new Vector3(5, 0, 5));
        listOfOffsetSpawnLocations.Add(new Vector3(-5, 0, 3));
        listOfOffsetSpawnLocations.Add(new Vector3(-7, 0, -4));
        listOfOffsetSpawnLocations.Add(new Vector3(6, 0, -5));

        return listOfOffsetSpawnLocations;
    }

    private condition noUnitsWithThisTag(tag2 unitGroupIn)
    {

        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new objectHasTag(unitGroupIn),
            new reverseCriteria(new objectHasTag(tag2.teamLeader))
            );



        objectSetGrabber theTeamObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), theCriteria);

        condition theCondition = new reverseCondition(new isThereAtLeastOneObjectInSet(theTeamObjectSet));

        return theCondition;
    }
}





public class teamLeaderOldFSMMadeWithPlugAndPlayPeices1 //: OldFSM
{
    OldFSM theOldFSM;
    public teamLeaderOldFSMMadeWithPlugAndPlayPeices1(GameObject theObject)
    {
        //so:
        //      which orders
        //      to who
        //      under what conditions?


        //new OldFSMgen2(newObj, new randomWanderOldFSMgen(), new grabFoodFeetOldFSMGen(stuffType.fruit));
        //new OldFSMgen2(newObj, new grabFoodHandsOldFSMGen(stuffType.fruit));





        //so:
        //      which orders
        //          "goToTargetPicker" with a "loop list" target picker or something
        //      to who
        //          [criteriaX] defenders
        //      [can assume] under what conditions?
        //          [can assume] if the defenders have no other orders?

        //but full picture here:
        //      make the OldFSM orders, etc
        //      create COMMAND OldFSM
        //      give that command...etc...


        OldFSM theCommandToFollow = new OldFSM();

        OldFSM advancedCommandForDefenders = new OldFSM();

        //      new OldFSMgen2(theObject, new leaderStateGivingDefenderPatrol());
    }

    public OldFSM returnIt()
    { 
        return theOldFSM;
    }
}


/*
public class leaderStateGivingDefenderPatrol : OldSimpleOneStateAndReturn
{
    //so:
    //      which orders
    //          "goToTargetPicker" with a "loop list" target picker or something
    //      to who
    //          [criteriaX] defenders
    //      [can assume] under what conditions?
    //          [can assume] if the defenders have no other orders? [ya that's part of "who", sorta

    //but full picture here:
    //      make the OldFSM orders, etc
    //      create COMMAND OldFSM
    //      give that command...etc...

    tag2 team;

    public leaderStateGivingDefenderPatrol(tag2 teamIn)
    {
        team = teamIn;
    }

    public override OldFSM generateTheOldFSM(GameObject theObjectDoingTheEnaction)
    {
        plugInOldFSM theOrders = new defenderPatrolState1();
        objectCriteria whoToGiveItTo = allSquadMembersWithoutOrders(tag2.defenseSquad);

        OldFSM theLeaderStateGivingDefenderPatrol = new giveXCommandToY(team,theOrders,whoToGiveItTo).returnIt();

        return theLeaderStateGivingDefenderPatrol;
    }

    private objectCriteria allSquadMembersWithoutOrders(tag2 squadTag)
    {
        throw new NotImplementedException();
    }

    public override condition generateTheSwitchCondition(GameObject theObjectDoingTheEnaction)
    {
        //when this is nonzero:
        objectCriteria whoToGiveItTo = allSquadMembersWithoutOrders(tag2.defenseSquad);
        condition theCondition = new isThereAtLeastOneObjectThatMeetsCriteria(team, whoToGiveItTo);


        return theCondition;
    }
}
*/
/*
public class giveXCommandToY
{
    private tag2 team;
    private plugInOldFSM theOrders;
    private objectCriteria whoToGiveItTo;

    public giveXCommandToY(tag2 teamIn, plugInOldFSM theOrdersIn, objectCriteria whoToGiveItToIn)
    {
        this.team = teamIn;
        this.theOrders = theOrdersIn;
        this.whoToGiveItTo = whoToGiveItToIn;
    }

    internal OldFSM returnIt()
    {
        //fuck this.




        var fakeRepeater = new giveXRTSTargetsToYUnits(null, new randomTargetPicker(allEnemyBases()));
        objectSetGrabber allAttackersWithNoOrders = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), attackerHasNoOrders);
        objectSetGrabber allDefendersWithNoOrders = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), defenderHasNoOrders);

        fakeRepeater.MOREINFOgiveXRTSTargetsToYUnits(allAttackersWithNoOrders);
        OldFSM giveOrders = new generateOldFSM(fakeRepeater);//new goToX(newObj, hihgigigiygiyTargetPicker(), 10000).returnIt());

        giveOrders.name = "giveOrders";




        agnosticTargetCalc theTarget = rtsTargetPicker.pickNext();

        //Debug.Log("unitsToGiveOrdersTo.grab().Count:  " + unitsToGiveOrdersTo.grab().Count);
        foreach (GameObject unit in unitsToGiveOrdersTo.grab())
        {
            //Debug.Log("unit, unit.GetHashCode():  " + unit + ", " + unit.GetHashCode());
            //tagging2.singleton.printAllTags(unit);
            rtsModule theComponent = unit.GetComponent<rtsModule>();
            theComponent.currentReceivedCommand = theTarget;
        }



        return giveOrders;
    }
}
*/

/*
public class giveXAdvancedRTSOrdersToSetY : repeatWithTargetPicker
{
    OldFSM theOrders;
    objectSetGrabber unitsToGiveOrdersTo;

    public giveXAdvancedRTSOrdersToSetY(permaPlan2 thePermaIn, targetPicker theTargetPickerIn) : base(thePermaIn, theTargetPickerIn)
    {
    }

    public void giveXRTSTargetsToYUnits(permaPlan2 thePermaIn, OldFSM theOrdersIn)//,  whoToGiveOrdersTo) : base(thePermaIn, whoToGiveOrdersTo)//?????
    {
        //rtsTargetPicker = theOrdersIn;

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
            //Debug.Log("unit, unit.GetHashCode():  " + unit + ", " + unit.GetHashCode());
            //tagging2.singleton.printAllTags(unit);
            rtsModule theComponent = unit.GetComponent<rtsModule>();
            theComponent.currentReceivedCommand = theTarget;
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


}
*/



public class followCommandsFromAdvancedRTS : OldSimpleOneStateAndReturn
{
    public override OldFSM generateTheOldFSM(GameObject theObjectDoingTheEnaction)
    {
        throw new NotImplementedException();
    }

    public override condition generateTheSwitchCondition(GameObject theObjectDoingTheEnaction)
    {
        throw new NotImplementedException();
    }
}


public class defenderPatrolState1 : OldSimpleOneStateAndReturn
{
    //so:
    //      which orders
    //          "goToTargetPicker" with a "loop list" target picker or something

    public override OldFSM generateTheOldFSM(GameObject theObjectDoingTheEnaction)
    {
        throw new NotImplementedException();
    }

    public override condition generateTheSwitchCondition(GameObject theObjectDoingTheEnaction)
    {
        throw new NotImplementedException();
    }
}

















public class makeWaveList
{

    tag2 team;//this is annoying, messy
    public tag2 unitGroup;
    List<objectSetInstantiator> theWaves = new List<objectSetInstantiator>();

    public makeWaveList(tag2 teamIn, tag2 unitGroupIn)
    {
        team = teamIn;
        unitGroup = unitGroupIn;
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
        float magnitudeOfInteractionIn = 1f;
        int firingRateIn = 10;
        float projectileSizeIn = 0.2f;
        float projectileSpeedIn = 1f;
        bool sdOnCollisionIn = true;
        int levelIn = 0;
        float gunHeightIn = 0.3f;
        float gunWidthIn = 0.2f;
        float gunLengthIn = 1.4f;

        return unitMaker2(1, 2.9f, 0.9f, 5, 20,
            weaponMaker(1, 40, 4, 1, true, 0, 0.3f, 0.2f, 1.4f));
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
        theWaves.Add(new objectSetInstantiator(theWave));
    }

    /*
    objectGen unit(float health, float height, float width, float speed, float targetDetectionRange)
    {
        return new basicPaintByNumbersSoldierGeneratorG(team, health, height, width, speed, targetDetectionRange);
    }
    */

    objectGen unitMaker2(float health, float height, float width, float speed, float targetDetectionRange, objectGen weapon)
    {
        return new soldierGenWithCustomGunAndUnitGroup(team, unitGroup, health, height, width, speed, targetDetectionRange, weapon);
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


public class soldierGenWithCustomGunAndUnitGroup : objectGen
{
    tagging2.tag2 team;
    public tag2 unitGroup;
    private Vector3 thePosition;

    float health;
    float height;
    float width;
    objectGen weapon;
    //List<OldFSM> theBehavior;  //gah!  needs the object as an input!
    float speed;
    float targetDetectionRange;


    //(tagging2.tag2 theTeamIn, float health, float speed, float height, float width, float targetDetectionRange, objectGen weapon, List<OldFSM> theBehavior )

    // 
    //basicBodyProperties
    public soldierGenWithCustomGunAndUnitGroup(tag2 theTeamIn, tag2 unitGroupIn, float health, float height, float width, float speed, float targetDetectionRange, objectGen weaponGenIn)
    {
        this.team = theTeamIn;
        this.unitGroup = unitGroupIn;
        this.health = health;
        this.height = height;
        this.width = width;
        this.speed = speed;
        weapon = weaponGenIn;
    }

    public GameObject generate()
    {
        GameObject newObj = new makeBasicHuman(3, 1, 6).generate();
        addTeamColors(newObj);

        tagging2.singleton.addTag(newObj, team);
        tagging2.singleton.addTag(newObj, unitGroup);


        newObj.AddComponent<rtsModule>();

        addWeapon(newObj, weapon);
        addSoldierOldFSM(newObj);


        return newObj;
    }

    private void addWeapon(GameObject theObject, objectGen theWeaponIn)
    {
        inventory1 theirInventory = theObject.GetComponent<inventory1>();
        //GameObject gun = weapon.generate();
        GameObject gun = theWeaponIn.generate();//genGen.singleton.returnGun1(newObj.transform.position);
        theirInventory.inventoryItems.Add(gun);
        interactionCreator.singleton.dockXToY(gun, theObject);

    }

    private void addSoldierOldFSM(GameObject theObject)
    {
        OldFSMcomponent theOldFSMcomponent = theObject.AddComponent<OldFSMcomponent>();
        theOldFSMcomponent.theOldFSMList = new soldierWithUnitGroupOldFSM(theObject, team, unitGroup,speed).returnIt();
    }

    public void addTeamColors(GameObject theObject)
    {
        Renderer theRenderer = theObject.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];
        //Renderer theRenderer2 = torso.GetComponent<Renderer>();
        //theRenderer2.material.color = tagging2.singleton.teamColors[team];
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


public class soldierWithUnitGroupOldFSM : OldFSM
{

    //OldFSM theOldFSM;
    public List<OldFSM> theOldFSMList = new List<OldFSM>();

    //float combatRange = 40f;
    private GameObject newObj;
    private tag2 team;
    public tag2 unitGroup;
    private float speed;
    float weaponFiringRange;
    private float targetDetectionRange;

    public soldierWithUnitGroupOldFSM(GameObject theObjectDoingTheEnaction, tag2 team, tag2 unitGroupIn, float speed, float weaponFiringRangeIn = 70f, float targetDetectionRangeIn = 210f)
    {
        this.newObj = theObjectDoingTheEnaction;
        this.team = team;
        this.unitGroup = unitGroupIn;  //i don't think soldiers need this info though?  leave it up to the leaders?
        this.speed = speed;
        weaponFiringRange = weaponFiringRangeIn;
        this.targetDetectionRange = targetDetectionRangeIn;

        //Debug.Log("weaponFiringRange:  " + weaponFiringRange);
        theOldFSMList.Add(feetOldFSM(theObjectDoingTheEnaction, team));
        theOldFSMList.Add(handsOldFSM(theObjectDoingTheEnaction, team));

    }

    private OldFSM handsOldFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        OldFSM idle = new generateOldFSM();

        //objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction, team, weaponFiringRange);
        //objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        
        
        
        
        
        
        
        
        //USING "OBJECT PERMANENCE" FOR THE AIMING STATE JUST MAKES IT LOOK BAD AIMING AT ITSELF WHEN THE SET ACTUALLY HAS NO ONE TO AIM AT
        //condition switchToAttack = threatObjectPermanenceButFalseIfObjectIsTrulyAbsent(theObjectDoingTheEnaction,weaponFiringRange);//new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 110);// theObjectDoingTheEnaction, numericalVariable.health);




        objectCriteria theAttackCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            new lineOfSight(theObjectDoingTheEnaction, weaponFiringRange),
            //new proximityCriteriaBool(theObjectDoingTheEnaction, range)  //REDUNDANT!  line of sight does range better for this purpose!
            new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)

            );

        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theAttackCriteria);


        //worldScript.singleton.theContradictionInvestigator.handsAttackObjectSetForAiming = theAttackObjectSet;
        //worldScript.singleton.theContradictionInvestigator.handsSwitchToAttack = switchToAttack;

        //worldScript.singleton.theContradictionInvestigator.handsAttackObjectSet = theAttackObjectSet;
        //worldScript.singleton.theContradictionInvestigator.handsSwitchToAttack = switchToAttack;

        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);
        
        //OldFSM combat1 = new generateOldFSM(new aimAtXAndPressY(theObjectDoingTheEnaction, theAttackTargetPicker, buttonCategories.primary, weaponFiringRange).returnIt());//new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, targetDetectionRange).returnIt());
        OldFSM combat1 = new generateOldFSM(new aimAtXAndPressYOnSameFrame(theObjectDoingTheEnaction, theAttackTargetPicker, buttonCategories.primary, weaponFiringRange).returnIt());//new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, targetDetectionRange).returnIt());


       condition switchToAttack = new isThereAtLeastOneObjectInSet(theAttackObjectSet);







        idle.addSwitchAndReverse(switchToAttack, combat1);



        equipItemOldFSM equipGun = new equipItemOldFSM(theObjectDoingTheEnaction, interType.peircing);

        idle.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theOldFSM);
        //wander.addSwitchAndReverse(switchToAttack, equipGun.theOldFSM);
        combat1.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theOldFSM);//messy



        idle.name = "hands, idle";
        combat1.name = "hands, combat1";
        equipGun.theOldFSM.name = "hands, equipGun";
        return idle; ;
    }

    private objectCriteria createAttackCriteria(GameObject theObjectDoingTheEnaction, tag2 team, float weaponFiringRangeIn)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new stickyTrueCriteria(new lineOfSight(theObjectDoingTheEnaction), 200),
            new proximityCriteriaBool(theObjectDoingTheEnaction, weaponFiringRangeIn)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private targetPicker generateAttackTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theAttackObjectSet)
    {

        targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        return theAttackTargetPicker;
    }

    private OldFSM feetOldFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        //Debug.Log("targetDetectionRange:  " + targetDetectionRange);
        //Debug.Log("weaponFiringRange:  " + weaponFiringRange);
        OldFSM wander = new generateOldFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());


        objectCriteria theCriteria = attackObjectCriteria(theObjectDoingTheEnaction);
        //objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInNearestXZones(theObjectDoingTheEnaction, 4), theCriteria);
        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        //setOfAllObjectsInZone
        //targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        //targetPicker theTargetPicker = new applePatternTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        //Debug.Log("weaponFiringRange:  " + weaponFiringRange);
        //float goldilocksRange = weaponFiringRange * (2 / 3);
        float goldilocksRange = weaponFiringRange * (2f / 3f);
        //Debug.Log("goldilocksRange:  "+ goldilocksRange);
        //targetPicker theTargetPicker = new combatPositionPack3TargetPicker(theObjectDoingTheEnaction, theAttackObjectSet, goldilocksRange);
        targetPicker theTargetPicker = new combatPositionPack4TargetPicker(theObjectDoingTheEnaction, theAttackObjectSet, goldilocksRange);

        //float desiredProximity = weaponFiringRange * (2f / 3f);
        OldFSM combat1 = new generateOldFSM(new goToX(theObjectDoingTheEnaction, theTargetPicker, 0).returnIt());

        condition switchToAttack = threatObjectPermanenceButFalseIfObjectIsTrulyAbsent(theObjectDoingTheEnaction, targetDetectionRange);//new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        wander.addSwitchAndReverse(switchToAttack, combat1);




        //worldScript.singleton.theContradictionInvestigator.feetAttackObjectSet = theAttackObjectSet;
        //worldScript.singleton.theContradictionInvestigator.feetSwitchToAttack = switchToAttack;





        targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        OldFSM goToRTSTarget = rtsOldFSM1(theObjectDoingTheEnaction, team);//new generateOldFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

        condition switchFromWanderToRTS = makeSwitchFromWanderToRTS(theObjectDoingTheEnaction);

        wander.addSwitchAndReverse(switchFromWanderToRTS, goToRTSTarget);
        goToRTSTarget.addSwitchAndReverse(switchToAttack, combat1);

        //*/


        wander.name = "feet, wander";
        combat1.name = "feet, combat1";
        goToRTSTarget.name = "feet, goToRTSTarget";










        navAgent theNav = theObjectDoingTheEnaction.GetComponent<navAgent>();
        theNav.theAgent.speed = this.speed;
        theNav.theAgent.baseOffset = 0.5f;//theObjectDoingTheEnaction.transform.localScale.y;
        //theNav.theAgent.

        return wander;
    }

    private objectCriteria attackObjectCriteria(GameObject theObjectDoingTheEnaction)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team))
            //new lineOfSight(theObjectDoingTheEnaction),
            //new proximityCriteriaBool(theObjectDoingTheEnaction, targetDetectionRange)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    internal condition threatObjectPermanenceButFalseIfObjectIsTrulyAbsent(GameObject theObjectDoingTheEnaction, float range)
    {
        objectCriteria theRealObjectCriteria = theRealThreatObjectCriteria();

        objectSetGrabber theRealObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theRealObjectCriteria);

        objectCriteria theAdditionalSituationalCriteria = theAdditionalSituationalThreatCriteria(theObjectDoingTheEnaction, range);

        objectSetGrabber theVisibleNearObjectSet = new setOfAllObjectThatMeetCriteria(theRealObjectSet, theAdditionalSituationalCriteria);


        //worldScript.singleton.theContradictionInvestigator.theRealObjectSet = theRealObjectSet;
        //worldScript.singleton.theContradictionInvestigator.theVisibleNearObjectSet = theVisibleNearObjectSet;



        int stickyTimerInSeconds = 9;
        condition objectPermanenceKludgeUsingStickiness = new stickyCondition(new isThereAtLeastOneObjectInSet(theVisibleNearObjectSet), stickyTimerInSeconds);// theObjectDoingTheEnaction, numericalVariable.health);
        condition realObjectConditionWithNoStickiness = new isThereAtLeastOneObjectInSet(theRealObjectSet);

        //worldScript.singleton.theContradictionInvestigator.objectPermanenceKludgeUsingStickiness = objectPermanenceKludgeUsingStickiness;
        //worldScript.singleton.theContradictionInvestigator.realObjectConditionWithNoStickiness = realObjectConditionWithNoStickiness;

        //k, but, why didn't i just do one set of criteria and one grabber and one condition to begin with?  isn't it the same result?
        condition theCondition = new multicondition(realObjectConditionWithNoStickiness, objectPermanenceKludgeUsingStickiness);

        //return realObjectConditionWithNoStickiness;
        return theCondition;
    }

    private objectCriteria theAdditionalSituationalThreatCriteria(GameObject theObjectDoingTheEnaction, float range)
    {
        objectCriteria theAdditionalSituationalCriteria = new objectMeetsAllCriteria(
            new lineOfSight(theObjectDoingTheEnaction, range),
            //new proximityCriteriaBool(theObjectDoingTheEnaction, range)  //REDUNDANT!  line of sight does range better for this purpose!
            new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );
        return theAdditionalSituationalCriteria;
    }

    private objectCriteria theRealThreatObjectCriteria()
    {
        objectCriteria theRealObjectCriteria =  new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team))
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );
        return theRealObjectCriteria;
    }

    private OldFSM rtsOldFSM1(GameObject theObjectDoingTheEnaction, tag2 teamIn)
    {
        //OldFSM wander = new generateOldFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());

        targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        OldFSM goToRTSTarget = new generateOldFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

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
            new reverseCriteria(new hasNoOrders())
            );

        //how to apply it to themselves?
        individualObjectReturner theObject = new presetObject(theObjectDoingTheEnaction);


        condition switchFromWanderToRTS = new individualObjectMeetsAllCriteria(theObject, theCriteria);

        return switchFromWanderToRTS;
    }




















    public List<OldFSM> returnIt()
    {
        return theOldFSMList;
    }



}

public class combatPositionPack2TargetPicker : targetPicker
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





    GameObject theObjectDoingTheEnaction;
    objectSetGrabber theSet;
    public combatPositionPack2TargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theSetInput, float goldilocksRangeIn)
    {
        //Debug.Log("goldilocksRange:  " + goldilocksRangeIn);
        theSet = theSetInput;
        this.theObjectDoingTheEnaction = theObjectDoingTheEnaction;
        targetPicker goldilocksCircle = new nearestGoldilocksTargetPicker(theObjectDoingTheEnaction, theSetInput, goldilocksRangeIn);
        targetPicker applePattern = new applePatternTargetPicker(theObjectDoingTheEnaction, theSetInput);
        targetPicker randomNearby = new randomNearbyLocationTargetPicker(theObjectDoingTheEnaction);

        /*
        listOfTargetPickers.Add(applePattern);
        listOfTargetPickers.Add(new radialFleeingTargetPicker(theObjectDoingTheEnaction, theSetInput));
        listOfTargetPickers.Add(new nearestTargetPicker(theObjectDoingTheEnaction, theSetInput));  //this is "go TOWARD" i think
        listOfTargetPickers.Add(randomNearby);
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { randomNearby, applePattern }));
        */

        /*
        //listOfTargetPickers.Add(applePattern);
        listOfTargetPickers.Add(goldilocksCircle); //very good all on it's own!
        //listOfTargetPickers.Add(randomNearby);
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, randomNearby })); 
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, applePattern }));
        */
        listOfTargetPickers.Add(randomNearby);
        listOfTargetPickers.Add(goldilocksCircle); //very good all on it's own!
        listOfTargetPickers.Add(applePattern);
        listOfTargetPickers.Add(new radialFleeingTargetPicker(theObjectDoingTheEnaction, theSetInput));
        listOfTargetPickers.Add(new nearestTargetPicker(theObjectDoingTheEnaction, theSetInput));  //this is "go TOWARD" i think  //yup, this one is like normal combat in a game, nice
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, applePattern }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, randomNearby }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, applePattern, randomNearby }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { applePattern, randomNearby }));//this one is like kiting while dodging.  a very cautious fighting style.  nice
        
        //how to do "idle"? can do single object target picker, and insert self.
        //Debug.Log("goldilocksRange:  " + goldilocksRangeIn);
    }

    public override agnosticTargetCalc pickNext()
    {
        //Debug.Log("theSet.grab().Count:  " + theSet.grab().Count);
        if (theSet.grab().Count ==0)//SHOULDN'T HAPPEN!  CONDITION SHOULD CHECK BEFORE WE DO THIS!  WHY IS IT HAPPENING???
        {
            Debug.Log("(theSet.grab().Count ==0) SHOULDN'T HAPPEN!  CONDITION SHOULD CHECK THIS SET BEFORE WE USE IT!");
            return new agnosticTargetCalc(theObjectDoingTheEnaction);
        }
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

public class combatPositionPack3TargetPicker : targetPicker
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
    int behaviorChangeCountdownTimeLIMIT = 3;

    bool currentlyDoingMainBehavior = true;





    GameObject theObjectDoingTheEnaction;
    objectSetGrabber theSet;
    public combatPositionPack3TargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theSetInput, float goldilocksRangeIn)
    {
        //Debug.Log("goldilocksRange:  " + goldilocksRangeIn);
        theSet = theSetInput;
        this.theObjectDoingTheEnaction = theObjectDoingTheEnaction;
        targetPicker goldilocksCircle = new nearestGoldilocksTargetPicker(theObjectDoingTheEnaction, theSetInput, goldilocksRangeIn);
        targetPicker applePattern = new applePatternTargetPicker(theObjectDoingTheEnaction, theSetInput);
        targetPicker randomNearby = new randomNearbyLocationTargetPicker(theObjectDoingTheEnaction);

        
        
        listOfTargetPickers.Add(goldilocksCircle); //very good all on it's own!
        listOfTargetPickers.Add(applePattern);
        listOfTargetPickers.Add(new nearestTargetPicker(theObjectDoingTheEnaction, theSetInput));  //this is "go TOWARD" i think
        listOfTargetPickers.Add(randomNearby);
        //listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { randomNearby, applePattern }));
        //listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, randomNearby }));
        

        /*
        //listOfTargetPickers.Add(applePattern);
        listOfTargetPickers.Add(goldilocksCircle); //very good all on it's own!
        //listOfTargetPickers.Add(randomNearby);
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, randomNearby })); 
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, applePattern }));
        */
        /*
        listOfTargetPickers.Add(randomNearby);
        listOfTargetPickers.Add(goldilocksCircle); //very good all on it's own!
        listOfTargetPickers.Add(applePattern);
        listOfTargetPickers.Add(new radialFleeingTargetPicker(theObjectDoingTheEnaction, theSetInput));
        listOfTargetPickers.Add(new nearestTargetPicker(theObjectDoingTheEnaction, theSetInput));  //this is "go TOWARD" i think  //yup, this one is like normal combat in a game, nice
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, applePattern }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, randomNearby }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, applePattern, randomNearby }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { applePattern, randomNearby }));//this one is like kiting while dodging.  a very cautious fighting style.  nice
        */
        //how to do "idle"? can do single object target picker, and insert self.
        //Debug.Log("goldilocksRange:  " + goldilocksRangeIn);
    }

    public override agnosticTargetCalc pickNext()
    {
        //Debug.Log("theSet.grab().Count:  " + theSet.grab().Count);
        if (theSet.grab().Count == 0)//SHOULDN'T HAPPEN!  CONDITION SHOULD CHECK BEFORE WE DO THIS!  WHY IS IT HAPPENING???
        {
            Debug.Log("(theSet.grab().Count ==0) SHOULDN'T HAPPEN!  CONDITION SHOULD CHECK THIS SET BEFORE WE USE IT!");
            return new agnosticTargetCalc(theObjectDoingTheEnaction);
        }
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


public class combatPositionPack4TargetPicker : targetPicker
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
    int behaviorChangeCountdownTimeLIMIT = 3;

    bool currentlyDoingMainBehavior = true;





    GameObject theObjectDoingTheEnaction;
    objectSetGrabber theSet;
    public combatPositionPack4TargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theSetInput, float goldilocksRangeIn)
    {
        //Debug.Log("goldilocksRange:  " + goldilocksRangeIn);
        theSet = theSetInput;
        this.theObjectDoingTheEnaction = theObjectDoingTheEnaction;
        
        wowItsBeautifulNiceCombatBehavior(goldilocksRangeIn);








        /*
        //listOfTargetPickers.Add(applePattern);
        listOfTargetPickers.Add(goldilocksCircle); //very good all on it's own!
        //listOfTargetPickers.Add(randomNearby);
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, randomNearby })); 
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, applePattern }));
        */
        /*
        listOfTargetPickers.Add(randomNearby);
        listOfTargetPickers.Add(goldilocksCircle); //very good all on it's own!
        listOfTargetPickers.Add(applePattern);
        listOfTargetPickers.Add(new radialFleeingTargetPicker(theObjectDoingTheEnaction, theSetInput));
        listOfTargetPickers.Add(new nearestTargetPicker(theObjectDoingTheEnaction, theSetInput));  //this is "go TOWARD" i think  //yup, this one is like normal combat in a game, nice
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, applePattern }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, randomNearby }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, applePattern, randomNearby }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { applePattern, randomNearby }));//this one is like kiting while dodging.  a very cautious fighting style.  nice
        */
        //how to do "idle"? can do single object target picker, and insert self.
        //Debug.Log("goldilocksRange:  " + goldilocksRangeIn);
    }

    private void wowItsBeautifulNiceCombatBehavior(float goldilocksRangeIn)
    {
        targetPicker goldilocksCircle = new nearestGoldilocksTargetPicker(theObjectDoingTheEnaction, theSet, goldilocksRangeIn);
        targetPicker applePattern = new applePatternTargetPicker(theObjectDoingTheEnaction, theSet);
        targetPicker randomNearby = new randomNearbyLocationTargetPicker(theObjectDoingTheEnaction);
        targetPicker nearest = new randomNearbyLocationTargetPicker(theObjectDoingTheEnaction);


        //wow it's beautiful, nice combat behavior
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, applePattern, randomNearby }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { nearest, applePattern, randomNearby }));
        listOfTargetPickers.Add(new averageOfTargetPickers(new targetPicker[] { goldilocksCircle, nearest, randomNearby }));
    }

    public override agnosticTargetCalc pickNext()
    {
        //Debug.Log("theSet.grab().Count:  " + theSet.grab().Count);
        if (theSet.grab().Count == 0)//SHOULDN'T HAPPEN!  CONDITION SHOULD CHECK BEFORE WE DO THIS!  WHY IS IT HAPPENING???
        {
            Debug.Log("(theSet.grab().Count ==0) SHOULDN'T HAPPEN!  CONDITION SHOULD CHECK THIS SET BEFORE WE USE IT!");
            return new agnosticTargetCalc(theObjectDoingTheEnaction);
        }
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









/*
public class something
{
    int currentWaveNumber = 0;

    public tag2 team;
    public tag2 unitGroup;
    public condition newWaveCondition;

    public List<objectSetInstantiator> listOfWaves = new List<objectSetInstantiator>();

    List<Vector3> listOfSpawnPoints = new List<Vector3>();

    public GameObject theBaseGeneratorObject;


    public something(tag2 teamIn, tag2 unitGroupIn, List<Vector3> listOfSpawnPointsIn)
    {
        unitGroup = unitGroupIn;
        listOfSpawnPoints = listOfSpawnPointsIn;

        //newWaveCondition = zeroRemainingUnitGroupMembersAnywhere(unitGroupIn);

        //Debug.Log("listOfSpawnPoints.Count:  " + listOfSpawnPoints.Count);
    }
    public something(tag2 unitGroupIn, List<Vector3> listOfSpawnPointsIn, List<objectSetInstantiator> wavesIn)
    {
        unitGroup = unitGroupIn;
        listOfSpawnPoints = listOfSpawnPointsIn;

        //newWaveCondition = zeroRemainingUnitGroupMembersAnywhere(unitGroupIn);
        listOfWaves = wavesIn;

        //Debug.Log("listOfSpawnPoints.Count:  " + listOfSpawnPoints.Count);
    }

    public List<objectSetInstantiator> testWaves()
    {
        List<objectSetInstantiator> newList = new List<objectSetInstantiator>();

        newList.Add(new objectSetInstantiator(new objectGen[] { new solderGen3(team, unitGroup) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33), new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.4f, 1, 4, 33), new basicPaintByNumbersSoldierGeneratorG(team, 2, 4, 1.3f, 13, 99) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 1, 2.5f, 1.9f, 9, 33), new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team), new basicSoldierGeneratorG(team) }));
        newList.Add(new objectSetInstantiator(new objectGen[] { new basicPaintByNumbersSoldierGeneratorG(team, 3, 3.8f, 2.4f, 2, 99), new basicPaintByNumbersSoldierGeneratorG(team, 2, 4, 1.3f, 13, 99), new basicSoldierGeneratorG(team), new basicPaintByNumbersSoldierGeneratorG(team, 2, 4, 1.3f, 13, 99), new basicSoldierGeneratorG(team) }));



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
        if (listOfWaves.Count == 0)
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

    }

    private Vector3 randomTargetPickerSpawnPoint(List<Vector3> listOfSpawnPoints)
    {
        //Debug.Log("listOfSpawnPoints.Count:  "+listOfSpawnPoints.Count);
        return listOfSpawnPoints[repository2.singleton.randomTargetPickerInteger(listOfSpawnPoints.Count - 1)];
    }




    public condition zeroRemainingUnitGroupMembersAnywhere(tag2 unitGroupIn)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            //new objectHasTag(unitGroupIn),
            new reverseCriteria(new objectHasTag(tag2.teamLeader))
            //new objectHasTag(team)
            //new proximityCriteriaBool(thePlayer?, 25)
            );



        //Debug.Log("CONDITION MAKER | zone:  "+tagging2.singleton.whichZone(thePlaceholderObjectInZone)+", and id number:  " + thePlaceholderObjectInZone.GetHashCode());

        objectSetGrabber theTeamObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), theCriteria);

        condition theCondition = new reverseCondition(new stickyCondition(new isThereAtLeastOneObjectInSet(theTeamObjectSet), 0));// theObjectDoingTheEnaction, numericalVariable.health);

        return theCondition;
    }

}


*/












public class callableThatGeneratesWavesWhenTheirConditionIsMet : MonoBehaviour, IupdateCallable
{
    private List<Vector3> relativeSpawnPoints;
    private Dictionary<condition, List<objectSetInstantiator>> wavesAndConditionDict;
    int currentWaveNumber = 0;
    public List<IupdateCallable> currentUpdateList { get; set; }

    public static callableThatGeneratesWavesWhenTheirConditionIsMet addThisComponent(GameObject baseMarkerIn, List<Vector3> relativeSpawnPointsIn, Dictionary<condition, List<objectSetInstantiator>> wavesAndConditionDictIn)
    {
        callableThatGeneratesWavesWhenTheirConditionIsMet theCallable = baseMarkerIn.AddComponent<callableThatGeneratesWavesWhenTheirConditionIsMet>();

        theCallable.relativeSpawnPoints = relativeSpawnPointsIn;
        theCallable.wavesAndConditionDict = wavesAndConditionDictIn;

        return theCallable;
    }

    public void callableUpdate()
    {
        foreach (condition thisCondition in wavesAndConditionDict.Keys)
        {
            if (thisCondition.met() == false) { continue; }
            wavesAndConditionDict[thisCondition][currentWaveNumber].generate(this.transform.position);
            currentWaveNumber++;
        }
    }
}




public class makeBasicHuman: objectGen
{
    float height;
    float width;
    objectGen weapon;
    float speed;


    public makeBasicHuman(float height, float width, float speed)//, objectGen weapon)
    {
        this.height = height;
        this.width = width;
        this.speed = speed;
    }

    private playable2 playableWithNewTorso(GameObject newObj, GameObject torso)
    {

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

        return thePlayable;
    }

    private void newTorso(GameObject newObj, GameObject torso)
    {
        //torso.transform.localScale = new Vector3(width, height / 2, width);


        //torso.transform.position += new Vector3(0, (height / 2) - 1, 0);
        torso.transform.position += new Vector3(0, 1f, 0);


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

    public GameObject generate()
    {
        //GameObject newObj = new teamRankingOfficerGenerator(team, 2, 4, 2, 5, 99).generate();

        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);
        GameObject torso = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);

        newTorso(newObj, torso);
        playable2 thePlayable = playableWithNewTorso(newObj, torso);

        //newObj.transform.position = thePosition;


        genGen.singleton.ensureVirtualGamePad(newObj);

        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);


        genGen.singleton.makeEnactionsWithTorsoArticulation1(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);



        inventory1 theirInventory = newObj.AddComponent<inventory1>();
        //GameObject gun = weapon.generate();
        //GameObject gun = genGen.singleton.returnGun1(newObj.transform.position);//new paintByNumbersGun1(1,40, 10,0.5f, true, 0, 0.3f, 0.2f,0.5f).generate();//
        //theirInventory.inventoryItems.Add(gun);
        //interactionCreator.singleton.dockXToY(gun, newObj);





        //newObj.GetComponent<NavMeshAgent>().enabled = false;
        newObj.AddComponent<reactivationOfNavMeshAgent>();
        newObj.AddComponent<miscDebug>();

        return newObj;
    }
}

public class makeLeader
{
    private tag2 team;

    public makeLeader(tag2 teamIn)
    {
        team = teamIn;
    }

    internal void doIt(Vector3 vector3)
    {
        GameObject newObj = new makeBasicHuman(4,2,5).generate();//new teamRankingOfficerGenerator(team, 2, 4, 2, 5, 99).generate();
        tagging2.singleton.addTag(newObj, team);
        tagging2.singleton.addTag(newObj, tag2.teamLeader);
        addTeamColors(newObj);
        addLeaderOldFSM(newObj);
        newObj.transform.position = vector3;
    }

    private void addLeaderOldFSM(GameObject theObject)
    {
        OldFSMcomponent theOldFSMcomponent = theObject.AddComponent<OldFSMcomponent>();
        theOldFSMcomponent.theOldFSMList = new teamLeaderWithUnitGroupsOldFSM(theObject, team, 5, 44).returnIt();
    }

    public void addTeamColors(GameObject theObject)
    {
        Renderer theRenderer = theObject.GetComponent<Renderer>();
        theRenderer.material.color = tagging2.singleton.teamColors[team];
        //Renderer theRenderer2 = torso.GetComponent<Renderer>();
        //theRenderer2.material.color = tagging2.singleton.teamColors[team];
    }
}



public class teamLeaderWithUnitGroupsOldFSM : OldFSM
{

    //OldFSM theOldFSM;
    public List<OldFSM> theOldFSMList = new List<OldFSM>();

    //float combatRange = 40f;
    private GameObject newObj;
    private tag2 team;
    private float speed;
    private float targetDetectionRange;

    public teamLeaderWithUnitGroupsOldFSM(GameObject theObjectDoingTheEnaction, tag2 team, float speed, float targetDetectionRange = 40f)
    {
        this.newObj = theObjectDoingTheEnaction;
        this.team = team;
        this.speed = speed;
        this.targetDetectionRange = targetDetectionRange;


        //theOldFSMList.Add(feetOldFSM(theObjectDoingTheEnaction, team));
        //theOldFSMList.Add(handsOldFSM(theObjectDoingTheEnaction, team));
        theOldFSMList.Add(giveTeamCommandsOldFSM(theObjectDoingTheEnaction, team));

    }

    private OldFSM giveTeamCommandsOldFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        objectSetGrabber theEnemyBaseObjectSet = new setOfAllObjectThatMeetCriteria(
            new setOfAllObjectsWithTag(tag2.militaryBase), enemyBaseCriteria(theObjectDoingTheEnaction, team));

        //condition is any time there are units with no orders? [and units who complete orders should thus blank out their orders?]
        objectCriteria attackerHasNoOrders = unitWithNoOrdersCriteriaByUnitGroup(tag2.attackSquad);

        condition conditionToGiveOrdersToAttackers = new isThereAtLeastOneObjectInSet(
            new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), attackerHasNoOrders));





        objectCriteria defenderHasNoOrders = unitWithNoOrdersCriteriaByUnitGroup(tag2.defenseSquad);
        
        condition conditionToGiveOrdersToDefenders = new isThereAtLeastOneObjectInSet(
            new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), defenderHasNoOrders));


        //So:
        //      1)  pick a random enemy base
        //      2)  make the finite state for feet that goes to that base
        //      3)  give that state to all of the soldiers with no orders
        //      4)  then I need to make the soldiers have the state that allows them to follow
        //    orders if there are any orders and if they are in an idle state.
        //    and make transfer out of that order following state if there is combat threats.

        //objectSetGrabber allEnemyBasesGrabber = allEnemyBases();
        //          agnosticTargetCalc randomEnemyBase = new randomTargetPicker(allEnemyBases()).pickNext();

        //OldFSM feetGoToTheTarget = new generateOldFSM(new goToX(newObj, hihgigigiygiyTargetPicker(), 10000).returnIt());


        //no, i'll give them just targets for now, not OldFSMs
        //so, need leader to have a repeater and/or OldFSM that inputs SOMETHING into soldiers' rts modules
        var fakeRepeater = new giveXRTSTargetsToYUnits(null, new randomTargetPicker(allEnemyBases()));
        objectSetGrabber allAttackersWithNoOrders = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), attackerHasNoOrders);
        objectSetGrabber allDefendersWithNoOrders = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), defenderHasNoOrders);

        fakeRepeater.MOREINFOgiveXRTSTargetsToYUnits(allAttackersWithNoOrders);
        OldFSM giveOrders = new generateOldFSM(fakeRepeater);//new goToX(newObj, hihgigigiygiyTargetPicker(), 10000).returnIt());

        giveOrders.name = "giveOrders";

        return giveOrders;
    }



    public void stuff()
    {
        //();
        //(tag2.defenseSquad);
        

    }

    private condition noUnitsWithThisTag(tag2 unitGroupIn)
    {

        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new objectHasTag(unitGroupIn),
            new reverseCriteria(new objectHasTag(tag2.teamLeader))
            );



        objectSetGrabber theTeamObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), theCriteria);

        condition theCondition = new reverseCondition(new isThereAtLeastOneObjectInSet(theTeamObjectSet));

        return theCondition;
    }






    private objectSetGrabber allEnemyBases()
    {

        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new reverseCriteria(new objectHasTag(team))
            );

        objectSetGrabber allEnemyBasesGrabber = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(tag2.militaryBase), theCriteria);

        return allEnemyBasesGrabber;
    }

    private objectCriteria unitWithNoOrdersCriteriaByUnitGroup(tag2 unitGroup)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new objectHasTag(unitGroup),
            new reverseCriteria(new objectHasTag(tag2.teamLeader)),
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






    private OldFSM handsOldFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        OldFSM idle = new generateOldFSM();

        objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction, team);
        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 110);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        OldFSM combat1 = new generateOldFSM(new aimAtXAndPressY(theObjectDoingTheEnaction, theAttackTargetPicker, buttonCategories.primary, targetDetectionRange).returnIt());//new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, targetDetectionRange).returnIt());



        idle.addSwitchAndReverse(switchToAttack, combat1);



        equipItemOldFSM equipGun = new equipItemOldFSM(theObjectDoingTheEnaction, interType.peircing);

        idle.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theOldFSM);
        //wander.addSwitchAndReverse(switchToAttack, equipGun.theOldFSM);
        combat1.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theOldFSM);//messy



        idle.name = "hands, idle";
        combat1.name = "hands, combat1";
        equipGun.theOldFSM.name = "hands, equipGun";
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

    private OldFSM feetOldFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        OldFSM wander = new generateOldFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());




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

        OldFSM combat1 = new generateOldFSM(new goToX(theObjectDoingTheEnaction, theTargetPicker, targetDetectionRange).returnIt());

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




    public List<OldFSM> returnIt()
    {
        return theOldFSMList;
    }



}











public class makeBaseMarker
{
    //So, for each team I need the following
    //      a base LOCATION
    //      a base object that is properly tagged
    //      a leader
    //      a hoarde wave callable update component  monobehavior I think? that can generate the waves at the correct times
    //      I need two of those.or I need two different ways and two different conditions.so two of those probably.
    //  one for attackers one for defenders.
    //      Attackers need to be tagged as attackers, Defenders need to be tagged as Defenders
    //      each of them needs to be able to receive orders
    //      the leader needs to give attackers and Defenders different orders based on their tagged classification. 
    //      And let's have one type of soldier generator, because I hate having to update two of them
    //  and check two of them and everything. just have one. [that i can somehow plug in the "attacker" or "defender" tags]

    private tag2 team;

    public makeBaseMarker(tag2 teamIn)
    {
        team = teamIn;
    }

    internal GameObject doIt(Vector3 thisPoint)
    {
        GameObject emptyObject = new GameObject();
        emptyObject.transform.position = thisPoint;


        //needs a zone, otherwise it won't ever be called in "callableUpdate"
        emptyObject.AddComponent<BoxCollider>();
        tagging2.singleton.addTag(emptyObject, tag2.zoneable);
        tagging2.singleton.addTag(emptyObject, tag2.militaryBase);
        tagging2.singleton.addTag(emptyObject, team);

        return emptyObject;
    }
}







public class waveGen
{
    int currentWaveNumber = 0;

    public tag2 team;
    public tag2 unitGroup;
    public condition newWaveCondition;

    public List<objectSetInstantiator> listOfWaves = new List<objectSetInstantiator>();

    List<Vector3> listOfSpawnPoints = new List<Vector3>();

    public GameObject theBaseGeneratorObject;


    public waveGen(tag2 unitGroupIn,List<Vector3> listOfSpawnPointsIn)
    {
        unitGroup = unitGroupIn;
        listOfSpawnPoints = listOfSpawnPointsIn;

        //newWaveCondition = zeroRemainingUnitGroupMembersAnywhere(unitGroupIn);

        //Debug.Log("listOfSpawnPoints.Count:  " + listOfSpawnPoints.Count);
    }
    public waveGen(tag2 unitGroupIn, List<Vector3> listOfSpawnPointsIn, List<objectSetInstantiator> wavesIn)
    {
        unitGroup = unitGroupIn;
        listOfSpawnPoints = listOfSpawnPointsIn;

        //newWaveCondition = zeroRemainingUnitGroupMembersAnywhere(unitGroupIn);
        listOfWaves = wavesIn;

        //Debug.Log("listOfSpawnPoints.Count:  " + listOfSpawnPoints.Count);
    }

    public List<objectSetInstantiator> testWaves()
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
        if (newWaveCondition.met())
        {
            currentWaveNumber++;
            generateNextWave();
        }
    }


    public void generateNextWave()
    {
        if (listOfWaves.Count == 0)
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

    }

    private Vector3 randomTargetPickerSpawnPoint(List<Vector3> listOfSpawnPoints)
    {
        //Debug.Log("listOfSpawnPoints.Count:  "+listOfSpawnPoints.Count);
        return listOfSpawnPoints[repository2.singleton.randomTargetPickerInteger(listOfSpawnPoints.Count - 1)];
    }




    public condition zeroRemainingUnitGroupMembersAnywhere(tag2 unitGroupIn)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            //new objectHasTag(unitGroupIn),
            new reverseCriteria(new objectHasTag(tag2.teamLeader))
            //new objectHasTag(team)
            //new proximityCriteriaBool(thePlayer?, 25)
            );



        //Debug.Log("CONDITION MAKER | zone:  "+tagging2.singleton.whichZone(thePlaceholderObjectInZone)+", and id number:  " + thePlaceholderObjectInZone.GetHashCode());

        objectSetGrabber theTeamObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), theCriteria);

        condition theCondition = new reverseCondition(new stickyCondition(new isThereAtLeastOneObjectInSet(theTeamObjectSet), 0));// theObjectDoingTheEnaction, numericalVariable.health);

        return theCondition;
    }

}

public class hoardUpdater1_2 : MonoBehaviour, IupdateCallable
{
    public List<IupdateCallable> currentUpdateList { get; set; }


    void Start()
    {
        //Debug.Log("hoardUpdater | zone:  " + tagging2.singleton.whichZone(this.gameObject) + ", and id number:  " + this.gameObject.GetHashCode());

        //messy [using collider to add this generator to a zone]
        Destroy(this.gameObject.GetComponent<BoxCollider>());//.enabled = false;
    }

    public void callableUpdate()
    {
        //Debug.Log("hoardUpdater | zone:  " + tagging2.singleton.whichZone(this.gameObject) + ", and id number:  " + this.gameObject.GetHashCode());

        foreach (waveGen hoard in hoardes)
        {
            //Debug.Log("hoard.team:  " + hoard.team);
            hoard.doOnUpdate();
        }
    }
    internal List<waveGen> hoardes = new List<waveGen>();
}




public class twoUnitGeneratorsPerTeam : doAtPoint
{
    public tag2 team;
    public waveGen attackWaveGen;
    public waveGen defenseWaveGen;
    public twoUnitGeneratorsPerTeam(tag2 teamIn, waveGen attackWaveGenIn, waveGen defenseWaveGenIn)
    {
        team = teamIn;
        attackWaveGen = attackWaveGenIn;
        defenseWaveGen = defenseWaveGenIn;
        //Debug.Log("??????????????????????");
        attackWaveGen.team = team;
        defenseWaveGen.team = team;
        //messy, but otherwise the soldiers don't have the updated team that we set in the lines above:
        attackWaveGen.listOfWaves = attackWaveGenIn.testWaves();
        defenseWaveGen.listOfWaves = defenseWaveGen.testWaves();
        //Debug.Log("attackWaveGen.team:  "+ attackWaveGen.team);
        //aaaaaand same with condition:
        attackWaveGen.newWaveCondition = attackWaveGenIn.zeroRemainingUnitGroupMembersAnywhere(attackWaveGen.unitGroup);
        defenseWaveGen.newWaveCondition = defenseWaveGen.zeroRemainingUnitGroupMembersAnywhere(defenseWaveGen.unitGroup);
    }

    internal override void doIt(Vector3 thisPoint)
    {

        GameObject newObj = new makeTestBase(team).doIt(thisPoint); //new GameObject();
                                                                    //emptyObject.transform.position = thisPoint;


        hoardUpdater1_2 theUpdater = newObj.AddComponent<hoardUpdater1_2>();
        theUpdater.hoardes.Add(attackWaveGen);
        theUpdater.hoardes.Add(defenseWaveGen);


        //needs a zone, otherwise it won't ever be called in "callableUpdate"
        newObj.AddComponent<BoxCollider>();

        attackWaveGen.theBaseGeneratorObject = newObj;
        defenseWaveGen.theBaseGeneratorObject = newObj;
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

    internal GameObject doIt(Vector3 thisPoint)
    {
        GameObject emptyObject = new GameObject();
        emptyObject.transform.position = thisPoint;


        //needs a zone, otherwise it won't ever be called in "callableUpdate"
        emptyObject.AddComponent<BoxCollider>();
        tagging2.singleton.addTag(emptyObject, tag2.zoneable);
        tagging2.singleton.addTag(emptyObject, tag2.militaryBase);
        tagging2.singleton.addTag(emptyObject, team);

        return emptyObject;
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




        OldFSMcomponent theOldFSMcomponent = newObj.AddComponent<OldFSMcomponent>();
        theOldFSMcomponent.theOldFSMList = new testRTSOldFSM(newObj, team).returnIt();//new basicPaintByNumbersSoldierOldFSM(newObj, tag2.team3, 5, 33).returnIt();//theBehavior;


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




public class testRTSOldFSM
{
    List<OldFSM> theList = new List<OldFSM>();
    GameObject theObjectDoingTheEnaction;
    tag2 team;

    public testRTSOldFSM(GameObject theObjectDoingTheEnactionIn, tag2 teamIn)
    {
        theObjectDoingTheEnaction = theObjectDoingTheEnactionIn;
        team = teamIn;
        theList.Add(theTestOldFSM());
    }

    private OldFSM theTestOldFSM()
    {
        //use a pre-filled RTS script, get it to move
        //then get the condition to gate that behavior
        //then find a way for it to work while initially empty, and then being filled [currently, LOTS of null object errprs!]

        return rtsOldFSM1(theObjectDoingTheEnaction, team);
    }

    internal List<OldFSM> returnIt()
    {
        return theList;
    }



    private OldFSM rtsOldFSM1(GameObject theObjectDoingTheEnaction, tag2 teamIn)
    {
        //OldFSM wander = new generateOldFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());

        targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        OldFSM goToRTSTarget = new generateOldFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

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
    //List<OldFSM> theBehavior;  //gah!  needs the object as an input!
    float speed;
    float targetDetectionRange;


    //(tagging2.tag2 theTeamIn, float health, float speed, float height, float width, float targetDetectionRange, objectGen weapon, List<OldFSM> theBehavior )

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




        OldFSMcomponent theOldFSMcomponent = newObj.AddComponent<OldFSMcomponent>();
        theOldFSMcomponent.theOldFSMList = new teamRankingOfficerOldFSM(newObj, team, speed, targetDetectionRange).returnIt();//new basicPaintByNumbersSoldierOldFSM(newObj, team, speed, targetDetectionRange).returnIt();//theBehavior;


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

public class teamRankingOfficerOldFSM : OldFSM
{

    //OldFSM theOldFSM;
    public List<OldFSM> theOldFSMList = new List<OldFSM>();

    //float combatRange = 40f;
    private GameObject newObj;
    private tag2 team;
    private float speed;
    private float targetDetectionRange;

    public teamRankingOfficerOldFSM(GameObject theObjectDoingTheEnaction, tag2 team, float speed, float targetDetectionRange = 40f)
    {
        this.newObj = theObjectDoingTheEnaction;
        this.team = team;
        this.speed = speed;
        this.targetDetectionRange = targetDetectionRange;


        //theOldFSMList.Add(feetOldFSM(theObjectDoingTheEnaction, team));
        //theOldFSMList.Add(handsOldFSM(theObjectDoingTheEnaction, team));
        theOldFSMList.Add(giveTeamCommandsOldFSM(theObjectDoingTheEnaction, team));

    }








    private OldFSM giveTeamCommandsOldFSM(GameObject theObjectDoingTheEnaction, tag2 team)
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
        //          agnosticTargetCalc randomEnemyBase = new randomTargetPicker(allEnemyBases()).pickNext();

        //OldFSM feetGoToTheTarget = new generateOldFSM(new goToX(newObj, hihgigigiygiyTargetPicker(), 10000).returnIt());


        //no, i'll give them just targets for now, not OldFSMs
        //so, need leader to have a repeater and/or OldFSM that inputs SOMETHING into soldiers' rts modules
        var fakeRepeater = new giveXRTSTargetsToYUnits(null, new randomTargetPicker(allEnemyBases()));
        objectSetGrabber allUnitsWithNoOrders = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), hasNoOrders);

        fakeRepeater.MOREINFOgiveXRTSTargetsToYUnits(allUnitsWithNoOrders);
        OldFSM giveOrders = new generateOldFSM(fakeRepeater);//new goToX(newObj, hihgigigiygiyTargetPicker(), 10000).returnIt());

        return giveOrders;
    }

    private objectSetGrabber allEnemyBases()
    {

        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new reverseCriteria(new objectHasTag(team))
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






    private OldFSM handsOldFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        OldFSM idle = new generateOldFSM();

        objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction, team);
        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 110);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        OldFSM combat1 = new generateOldFSM(new aimAtXAndPressY(theObjectDoingTheEnaction, theAttackTargetPicker, buttonCategories.primary, targetDetectionRange).returnIt());//new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, targetDetectionRange).returnIt());



        idle.addSwitchAndReverse(switchToAttack, combat1);



        equipItemOldFSM equipGun = new equipItemOldFSM(theObjectDoingTheEnaction, interType.peircing);

        idle.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theOldFSM);
        //wander.addSwitchAndReverse(switchToAttack, equipGun.theOldFSM);
        combat1.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theOldFSM);//messy



        idle.name = "hands, idle";
        combat1.name = "hands, combat1";
        equipGun.theOldFSM.name = "hands, equipGun";
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

    private OldFSM feetOldFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        OldFSM wander = new generateOldFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());




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

        OldFSM combat1 = new generateOldFSM(new goToX(theObjectDoingTheEnaction, theTargetPicker, targetDetectionRange).returnIt());

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




    public List<OldFSM> returnIt()
    {
        return theOldFSMList;
    }



}












public class advancedRtsModule : MonoBehaviour
{
    public generateFSM currentOrdersToGive;
    internal Dictionary<objectIdPair, generateFSM> currentReceivedCommandAndWhoGaveThem = new Dictionary<objectIdPair, generateFSM>();  //important to know who gave orders!  in case it's from wrong chain of command, or an ENEMY, or there's a mutiny and someone BECOMES an enemy


    public List<objectIdPair> currentlySelectedUnits; //annoying, but what else to do...
    public advancedRtsModuleVersion theVersion;
    public generateFSM currentReceivedCommand = null;

    public static advancedRtsModule ensureObjectHasThisComponent(GameObject theObject, advancedRtsModuleVersion theVersion)
    {
        advancedRtsModule theRTSModule = theObject.GetComponent<advancedRtsModule>();
        if (theRTSModule == null)
        {
            theRTSModule = theObject.AddComponent<advancedRtsModule>();
        }

        theRTSModule.theVersion = theVersion;

        return theRTSModule;
    }

    internal void giveCurrentOrdersToCurrentlySelectedUnits()
    {
        theVersion.giveCurrentOrdersToCurrentlySelectedUnits(this);
    }


    internal void basicImplementation()
    {
        foreach (objectIdPair thisID in currentlySelectedUnits)
        {
            if (thisID.theObject == null) { continue; }
            advancedRtsModule theirRTSModule = thisID.theObject.GetComponent<advancedRtsModule>();
            theirRTSModule.currentReceivedCommandAndWhoGaveThem[tagging2.singleton.idPairGrabify(this.gameObject)] = currentOrdersToGive;
        }
    }

    /*
    public agnostRepeater translateOrdersIntoRepeaterPlan()
    {

    }
    */
}

public abstract class advancedRtsModuleVersion
{
    internal abstract void giveCurrentOrdersToCurrentlySelectedUnits(advancedRtsModule rtsModule);
}

public class playerVersionOfAdvancedRTS : advancedRtsModuleVersion
{
    objectSetGrabber theDefaultSelectionSet;  //buuut........
    internal override void giveCurrentOrdersToCurrentlySelectedUnits(advancedRtsModule rtsModule)
    {
        rtsModule.basicImplementation();


        resetCurrentOrdersToGive(rtsModule);
        resetCurrentlySelectedUnits(rtsModule);
    }


    private void resetCurrentOrdersToGive(advancedRtsModule rtsModule)
    {
        //make the orders be "go to player's waypoint1"
        //and then have a bundled...enaction, enaction Effect?...that moves
        //the player's waypoint1 to wherever they are pointing at that enaction moment.  should move it FIRST, really, to be sure....
    }

    private void resetCurrentlySelectedUnits(advancedRtsModule rtsModule)
    {
        List<objectIdPair> unitList = tagging2.singleton.listInIDPairFormat(theDefaultSelectionSet.grab());
        rtsModule.currentlySelectedUnits = unitList;
    }

}

public class npcVersionOfAdvancedRTS : advancedRtsModuleVersion
{
    internal override void giveCurrentOrdersToCurrentlySelectedUnits(advancedRtsModule rtsModule)
    {
        rtsModule.basicImplementation();
    }
}












public class rtsModule : MonoBehaviour
{
    //public OldFSM currentOrdersToGive;
    //OldFSM currentReceivedCommand;  //what about multiple missions from multiple sources?  list?
    //internal Dictionary<objectIdPair, OldFSM> currentReceivedCommandAndWhoGaveThem = new Dictionary<objectIdPair, OldFSM>();  //important to know who gave orders!  in case it's from wrong chain of command, or an ENEMY, or there's a mutiny and someone BECOMES an enemy
    //too complex for now.  start simple.
    //internal Dictionary<condition, OldFSM> currentReceivedCommand = new Dictionary<condition, OldFSM>();
    //List<GameObject> currentlySelectedUnits; //hmm, but will glitch when they die........

    //just do targets for now, not OldFSM
    public agnosticTargetCalc currentOrdersToGive;
    //OldFSM currentReceivedCommand;  //what about multiple missions from multiple sources?  list?
    internal Dictionary<objectIdPair, agnosticTargetCalc> currentReceivedCommandAndWhoGaveThem = new Dictionary<objectIdPair, agnosticTargetCalc>();  //important to know who gave orders!  in case it's from wrong chain of command, or an ENEMY, or there's a mutiny and someone BECOMES an enemy


    public List<objectIdPair> currentlySelectedUnits; //annoying, but what else to do...
    public rtsModuleVersion theVersion;
    internal agnosticTargetCalc currentReceivedCommand = null;

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
            theirRTSModule.currentReceivedCommandAndWhoGaveThem[tagging2.singleton.idPairGrabify(this.gameObject)] = currentOrdersToGive;
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
    tag2 team;
    public oneTeamBaseGenAtPoint(tag2 teamIn, hoardeWaveGen1_1 teamBaseGenIn)
    {
        team = teamIn;
        teamBaseGen = teamBaseGenIn;
    }

    internal override void doIt(Vector3 thisPoint)
    {

        GameObject newObj = new makeTestBase(team).doIt(thisPoint); //new GameObject();
        //emptyObject.transform.position = thisPoint;

        hoardUpdater1_1 theUpdater = newObj.AddComponent<hoardUpdater1_1>();


        List<Vector3> listOfSpawnPoints = new List<Vector3>();
        listOfSpawnPoints.Add(new Vector3(5, 0, 5) + newObj.transform.position);
        listOfSpawnPoints.Add(new Vector3(5, 0, -5) + newObj.transform.position);
        listOfSpawnPoints.Add(new Vector3(-5, 0, 5) + newObj.transform.position);
        listOfSpawnPoints.Add(new Vector3(-5, 0, -5) + newObj.transform.position);
        

        theUpdater.hoardes.Add(teamBaseGen);
        //theUpdater.hoardes.Add(new hoardeWaveGen(tag2.team8, listOfSpawnPoints, emptyObject, new factionEDYEOUF(tag2.team8).returnWaves())); //messy messy "tag2.team5" stuff TWICE



        //needs a zone, otherwise it won't ever be called in "callableUpdate"
        newObj.AddComponent<BoxCollider>();

        teamBaseGen.theBaseGeneratorObject = newObj;
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









public class playerTeamBase1_1
{

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
    //List<OldFSM> theBehavior;  //gah!  needs the object as an input!
    float speed;
    float targetDetectionRange;


    //(tagging2.tag2 theTeamIn, float health, float speed, float height, float width, float targetDetectionRange, objectGen weapon, List<OldFSM> theBehavior )

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




        OldFSMcomponent theOldFSMcomponent = newObj.AddComponent<OldFSMcomponent>();
        theOldFSMcomponent.theOldFSMList = new basicPaintByNumbersSoldierOldFSM(newObj,team,speed,targetDetectionRange).returnIt();//theBehavior;


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
    //List<OldFSM> theBehavior;  //gah!  needs the object as an input!
    float speed;
    float targetDetectionRange;


    //(tagging2.tag2 theTeamIn, float health, float speed, float height, float width, float targetDetectionRange, objectGen weapon, List<OldFSM> theBehavior )

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




        OldFSMcomponent theOldFSMcomponent = newObj.AddComponent<OldFSMcomponent>();
        theOldFSMcomponent.theOldFSMList = new basicPaintByNumbersSoldierOldFSM(newObj, team, speed, targetDetectionRange).returnIt();//theBehavior;


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


public class basicPaintByNumbersSoldierOldFSM : OldFSM
{

    //OldFSM theOldFSM;
    public List<OldFSM> theOldFSMList = new List<OldFSM>();

    //float combatRange = 40f;
    private GameObject newObj;
    private tag2 team;
    private float speed;
    private float targetDetectionRange;

    public basicPaintByNumbersSoldierOldFSM(GameObject theObjectDoingTheEnaction, tag2 team, float speed, float targetDetectionRange = 40f)
    {
        this.newObj = theObjectDoingTheEnaction;
        this.team = team;
        this.speed = speed;
        this.targetDetectionRange = targetDetectionRange;


        theOldFSMList.Add(feetOldFSM(theObjectDoingTheEnaction, team));
        theOldFSMList.Add(handsOldFSM(theObjectDoingTheEnaction, team));

    }

    private OldFSM handsOldFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        OldFSM idle = new generateOldFSM();

        objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction, team);
        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 110);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);
        
        OldFSM combat1 = new generateOldFSM(new aimAtXAndPressY(theObjectDoingTheEnaction, theAttackTargetPicker, buttonCategories.primary, targetDetectionRange).returnIt());//new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, targetDetectionRange).returnIt());



        idle.addSwitchAndReverse(switchToAttack, combat1);



        equipItemOldFSM equipGun = new equipItemOldFSM(theObjectDoingTheEnaction, interType.peircing);

        idle.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theOldFSM);
        //wander.addSwitchAndReverse(switchToAttack, equipGun.theOldFSM);
        combat1.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theOldFSM);//messy



        idle.name = "hands, idle";
        combat1.name = "hands, combat1";
        equipGun.theOldFSM.name = "hands, equipGun";
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

    private OldFSM feetOldFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        OldFSM wander = new generateOldFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());




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

        OldFSM combat1 = new generateOldFSM(new goToX(theObjectDoingTheEnaction, theTargetPicker, targetDetectionRange).returnIt());

        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        wander.addSwitchAndReverse(switchToAttack, combat1);

        wander.name = "feet, wander";
        combat1.name = "feet, combat1";










        targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        OldFSM goToRTSTarget = rtsOldFSM1(theObjectDoingTheEnaction, team);//new generateOldFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

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







    private OldFSM rtsOldFSM1(GameObject theObjectDoingTheEnaction, tag2 teamIn)
    {
        //OldFSM wander = new generateOldFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());

        targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        OldFSM goToRTSTarget = new generateOldFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

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




















    public List<OldFSM> returnIt()
    {
        return theOldFSMList;
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


        OldFSMcomponent theOldFSMcomponent = newObj.AddComponent<OldFSMcomponent>();
        theOldFSMcomponent.theOldFSMList = new basicSoldierOldFSM(newObj, team).returnIt();
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
        //Debug.Log(team);
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


        OldFSMcomponent theOldFSMcomponent = newObj.AddComponent<OldFSMcomponent>();
        theOldFSMcomponent.theOldFSMList = new basicSoldierOldFSM(newObj, team).returnIt();




        newObj.AddComponent<reactivationOfNavMeshAgent>();

        return newObj;
    }



}







public class basicSoldierOldFSM : OldFSM
{

    //OldFSM theOldFSM;
    public List<OldFSM> theOldFSMList = new List<OldFSM>();

    float combatRange = 40f;

    public basicSoldierOldFSM(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {


        theOldFSMList.Add(feetOldFSM(theObjectDoingTheEnaction, team));
        theOldFSMList.Add(handsOldFSM(theObjectDoingTheEnaction, team));

    }

    private OldFSM handsOldFSM(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {
        OldFSM idle = new generateOldFSM();

        objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction,team);
        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction,theAttackObjectSet);

        OldFSM combat1 = new generateOldFSM(new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, combatRange).returnIt());

        

        idle.addSwitchAndReverse(switchToAttack, combat1);



        equipItemOldFSM equipGun = new equipItemOldFSM(theObjectDoingTheEnaction, interType.peircing);

        idle.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theOldFSM);
        //wander.addSwitchAndReverse(switchToAttack, equipGun.theOldFSM);
        combat1.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theOldFSM);//messy



        idle.name = "hands, idle";
        combat1.name = "hands, combat1";
        equipGun.theOldFSM.name = "hands, equipGun";
        return idle;;
    }

    private objectCriteria createAttackCriteria(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new stickyTrueCriteria( new lineOfSight(theObjectDoingTheEnaction),200),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 45)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private targetPicker generateAttackTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theAttackObjectSet)
    {
        
        targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        return theAttackTargetPicker;
    }

    private OldFSM feetOldFSM(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {
        OldFSM wander = new generateOldFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());




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

        OldFSM combat1 = new generateOldFSM(new goToX(theObjectDoingTheEnaction, theTargetPicker, combatRange).returnIt());

        //condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 1);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        wander.addSwitchAndReverse(switchToAttack, combat1);


        //targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        OldFSM goToRTSTarget = rtsOldFSM1(theObjectDoingTheEnaction,team);//new generateOldFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

        condition switchFromWanderToRTS = makeSwitchFromWanderToRTS(theObjectDoingTheEnaction);

        wander.addSwitchAndReverse(switchFromWanderToRTS, goToRTSTarget);
        goToRTSTarget.addSwitchAndReverse(switchToAttack, combat1);


        wander.name = "feet, wander";
        combat1.name = "feet, combat1";
        goToRTSTarget.name = "feet, goToRTSTarget";
        return wander;
    }





    private OldFSM rtsOldFSM1(GameObject theObjectDoingTheEnaction, tag2 teamIn)
    {
        //OldFSM wander = new generateOldFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());

        targetPicker theRTSCommandTargetPicker = makeTheRTSCommandTargetPicker(theObjectDoingTheEnaction);

        OldFSM goToRTSTarget = new generateOldFSM(new goToXFromTargetPicker(theObjectDoingTheEnaction, theRTSCommandTargetPicker, 2f).returnIt());

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










    public List<OldFSM> returnIt()
    {
        return theOldFSMList;
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

        return theRTSModule.currentReceivedCommand;
    }
}


public class equipItemOldFSM
{

    public OldFSM theOldFSM;

    public equipItemOldFSM(GameObject theObjectDoingTheEnaction, interType interTypeX)
    {

        //equipObjectRepeater
        theOldFSM = new generateOldFSM(new equipObjectRepeater(theObjectDoingTheEnaction)); //hmm, this doesn't share cache with the conditions......
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


    public OldFSM returnIt()
    {
        return theOldFSM;
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
            //Debug.Log("unit, unit.GetHashCode():  " + unit + ", " + unit.GetHashCode());
            //tagging2.singleton.printAllTags(unit);
            rtsModule theComponent = unit.GetComponent<rtsModule>();
            theComponent.currentReceivedCommand = theTarget;
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

        Debug.Log("failed to find an enaction with this interaction:  "+ interTypeX+" on this object:  " + theObjectDoingTheEnactions);

        //if(theInteractionEnaction == null) { return null; }//hmmmmmmmm


        inventory1 theirInventory = theObjectDoingTheEnactions.GetComponent<inventory1>();
        if (theirInventory == null) 
        {
            Debug.Log("next thing to try was looking for interaction type in their INVENTORY, but failed to find an inventory on this object:  " +theObjectDoingTheEnactions);
            return null; 
        }  //or return "go find"?

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




public class aimAtXAndPressYOnSameFrame
{
    repeatWithTargetPicker theRepeater;

    public aimAtXAndPressYOnSameFrame(GameObject theObjectDoingTheEnactions, targetPicker thingXToAimAt, buttonCategories buttonY, float proximity)
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


        singleEXE theSimultaneousEXE = new simultaneousEXE(
            new aimTargetPlanGen(theObjectDoingTheEnactions, thingXToAimAt).returnIt(), //messy
                pressY(theObjectDoingTheEnactions, buttonY));

        //permaPlan2 perma1 = new permaPlan2(theEXE);

        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(theSimultaneousEXE, thingXToAimAt);


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
            //Debug.Log("theEnaction in gamepad button = null");
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
