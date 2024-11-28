using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.WSA;
using static enactionCreator;
using static interactionCreator;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class genGen : MonoBehaviour
{
    //[general generator]  hmmm, thigs here should either be in repository or combo gen maybe?
    public static genGen singleton;

    void Awake()
    {
        singletonify();
    }

    void singletonify()
    {
        if (singleton != null && singleton != this)
        {
            Debug.Log("this class is supposed to be a singleton, you should not be making another instance, destroying the new one");
            Destroy(this);
            return;
        }
        singleton = this;
    }






    public void createPrefabAtPoint(GameObject thePrefab, Vector3 thePoint)
    {
        //just so i can keep the rotation of the object i input, for now:
        Instantiate(thePrefab, thePoint, thePrefab.transform.rotation);
    }

    public GameObject createPrefabAtPointAndRETURN(GameObject thePrefab, Vector3 thePoint)
    {
        //just so i can keep the rotation of the object i input, for now:
        return Instantiate(thePrefab, thePoint, thePrefab.transform.rotation);
    }

    public GameObject createAndReturnPrefabAtPointWITHNAME(GameObject thePrefab, Vector3 thePoint, string theName)
    {

        //just so i can keep the rotation of the object i input, for now:
        GameObject newObject = Instantiate(thePrefab, thePoint, thePrefab.transform.rotation);

        newObject.name = theName;

        return newObject;
    }








    

    public GameObject makeEmptyIntSphere(Vector3 position)
    {
        GameObject prefabToUse = repository2.singleton.interactionSphere;
        GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, position);

        return newProjectile;
    }

    public void addCube(GameObject inputObject, float scale = 1f, float xOffset = 0f, float yOffset = 0f, float zOffset = 0f)
    {
        Vector3 where = inputObject.transform.position + new Vector3(xOffset, yOffset, zOffset);
        GameObject newObj = Instantiate(repository2.singleton.placeHolderCubePrefab, where, Quaternion.identity);
        newObj.transform.localScale = scale * newObj.transform.localScale;

        newObj.transform.parent = inputObject.transform;

    }

    public void addArrowForward(GameObject inputObject, float scale = 1f, float xOffset = 0f, float yOffset = 0f, float zOffset = 0f)
    {
        Vector3 where = inputObject.transform.position + new Vector3(xOffset, yOffset, zOffset);
        GameObject newObj = Instantiate(repository2.singleton.arrowForward, where, Quaternion.identity);
        newObj.transform.localScale = scale * newObj.transform.localScale;

        newObj.transform.parent = inputObject.transform;

    }

    public void addArrowUp(GameObject inputObject, float scale = 1f, float xOffset = 0f, float yOffset = 0f, float zOffset = 0f)
    {
        Vector3 where = inputObject.transform.position + new Vector3(xOffset, yOffset, zOffset);
        GameObject newObj = Instantiate(repository2.singleton.arrowUp, where, Quaternion.identity);
        newObj.transform.localScale = scale * newObj.transform.localScale;

        newObj.transform.parent = inputObject.transform;

    }

    public void projectileGenerator(projectileToGenerate theprojectileToGenerate, collisionEnaction theEnactable, Vector3 startPoint, Vector3 direction)//(rangedEnaction enInfo, interactionInfo interINFO, IEnactaBool theEnactable)
    {

        GameObject newObjectForProjectile = makeEmptyIntSphere(startPoint);
        projectile1.genProjectile1(newObjectForProjectile, theprojectileToGenerate, direction);//enInfo, interINFO, theEnactable);
        growScript1.genGrowScript1(newObjectForProjectile, theprojectileToGenerate.growthSpeed);

        colliderInteractor.genColliderInteractor(newObjectForProjectile, theEnactable);
        selfDestructScript1 sds = newObjectForProjectile.GetComponent<selfDestructScript1>();
        sds.timeUntilSelfDestruct = theprojectileToGenerate.timeUntilSelfDestruct;
    }







    public GameObject returnNPC5(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.placeHolderCylinderPrefab, where, Quaternion.identity);

        Destroy(newObj.GetComponent<Collider>());

        addBody4ToObject(newObj);

        newObj.AddComponent<AIHub3>();


        newObj.AddComponent<CapsuleCollider>();


        //newObj.transform.localScale = new Vector3(1, 0.5f, 1);
        return newObj;
    }

    public GameObject returnPineTree1(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.pineTree1, where, Quaternion.identity);

        return newObj;
    }

    public GameObject returnGun1(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.simpleGun1, where, Quaternion.identity);

        return newObj;
    }

    public GameObject returnShotgun1(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.shotgun1, where, Quaternion.identity);
        //      newObj.transform.localScale = new Vector3(128, 1, 8);

        return newObj;
    }




    public void addBody4ToObject(GameObject newObj)
    {
        tagging2.singleton.addTag(newObj, tagging2.tag2.threat1);
        playable2 thePlayable = newObj.AddComponent<playable2>();


        //thePlayable.dictOfInteractions = new Dictionary<enactionCreator.interType, List<Ieffect>>();
        //thePlayable.dictOfIvariables = new Dictionary<interactionCreator.numericalVariable, float>();

        thePlayable.dictOfIvariables[numericalVariable.health] = 2;
        thePlayable.equipperSlotsAndContents[interactionCreator.simpleSlot.hands] = null;
        thePlayable.initializeEnactionPoint1();
        addArrowForward(thePlayable.enactionPoint1);
        addCube(thePlayable.enactionPoint1, 0.1f);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        addArrowForward(newObj, 5f, 0f, 1.2f);
        makeBasicEnactions(thePlayable);
        makeInteractionsBody4(thePlayable);


        inventory1 theirInventory = newObj.AddComponent<inventory1>();
    }

    public void makeBasicEnactions(playable2 thePlayable)
    {
        hitscanEnactor.addHitscanEnactor(thePlayable.gameObject, thePlayable.enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.standardClick));


        vecTranslation.addVecTranslation(thePlayable.gameObject, thePlayable.speed, buttonCategories.vector1);

        navAgent.addNavAgentEnaction(thePlayable.gameObject);

        aimTarget.addAimTargetAndVecRotation(thePlayable.gameObject, thePlayable.lookSpeed, thePlayable.transform, thePlayable.enactionPoint1.transform, buttonCategories.vector2);
    }

    public void makeInteractionsBody4(interactable2 theInteractable)
    {
        createWeaponLevels(theInteractable, interType.peircing, 0, 4);
        createWeaponLevels(theInteractable, interType.melee, 0, 4);
        //createWeaponLevels(theInteractable, interType.shootFlamethrower1, 0, 5);
        //createWeaponLevels(theInteractable, interType.tankShot, 0, 0);

        deathWhenHealthIsZero(theInteractable);
    }

    public void standardGun(equippable2 theEquippable, float magnitudeOfInteraction = 1f, int level = 0, bool sdOnCollision = true, float speed = 1f)
    {
        theEquippable.dictOfIvariables[numericalVariable.cooldown] = 0f;

        /*
        projectileLauncher theShooter = new projectileLauncher(theEquippable.enactionPoint1.transform, 
            buttonCategories.errorYouDidntSetEnumTypeForBUTTONCATEGORIES, 
            new interactionInfo(interType.peircing, magnitudeOfInteraction, level),
            new projectileToGenerate(speed, sdOnCollision, 999, 0));
        */


        //bit messy?  made "thisButtonCategoryIntentionallyLeftBlank" so that i can add component to object [thus easy search object for component of that type] WITHOUT having it plug into a gamepad button when equipped......
        projectileLauncher.addProjectileLauncher(theEquippable.transform.gameObject,
            theEquippable.enactionPoint1.transform,
            buttonCategories.thisButtonCategoryIntentionallyLeftBlank,
            new interactionInfo(interType.peircing, magnitudeOfInteraction, level),
            new projectileToGenerate(speed, sdOnCollision, 99, 0),
            20);

        numericalEffect(theEquippable, numericalVariable.cooldown, 90);

        projectileLauncher theShooter = theEquippable.transform.gameObject.GetComponent<projectileLauncher>();
        enactEffect theFiringEffectOnCooldown = theEquippable.transform.gameObject.GetComponent<enactEffect>();

        //IEnactaBool theFiringEffectOnCooldown = enactEffect.returnEnactEffect(new numericalEffect(theEquippable, numericalVariable.cooldown));





        //Debug.Assert(enactEffect.returnEnactEffect(new deathEffect(theEquippable.transform.gameObject)) != null);
        //Debug.Assert(theFiringEffectOnCooldown != null);
        Debug.Assert(theFiringEffectOnCooldown != null);

        condition cooldownCondition = new numericalCondition(numericalVariable.cooldown, theEquippable.dictOfIvariables);

        compoundEnactaBool.addCompoundEnactaBool(theEquippable.transform.gameObject, buttonCategories.primary, theShooter, theFiringEffectOnCooldown, cooldownCondition);






        /*
        projectileLauncher.addProjectileLauncher(theEquippable.transform.gameObject,
            theEquippable.enactionPoint1.transform,
            buttonCategories.primary,
            new interactionInfo(interType.peircing, magnitudeOfInteraction, level),
            new projectileToGenerate(speed, sdOnCollision, 999, 0),
            20);

        */

    }

    private static void numericalEffect(equippable2 theEquippable, numericalVariable numVarX, int amountToSubtract = 1)
    {
        enactEffect.addEnactEffect(theEquippable.transform.gameObject, new numericalEffect(theEquippable, numericalVariable.cooldown, amountToSubtract));
    }

    void deathWhenHealthIsZero(interactable2 theInteractable)
    {

        //i'll put conditional effects in this function for now
        condition theCondition = new numericalCondition(numericalVariable.health, theInteractable.dictOfIvariables);
        Ieffect theEffect = new deathEffect(theInteractable.transform.gameObject);
        //List<Ieffect> list = new List<Ieffect> { theEffect };
        //theInteractable.conditionalEffects[theCondition] = list;
        addConditionalEffect(theInteractable.transform.gameObject, theCondition, theEffect);
    }

    private static void createWeaponLevels(interactable2 theInteractable, interType theInteractionType, int levelWhenDamageStarts, int instantKillLevel)
    {
        theInteractable.dictOfInteractions = interactionCreator.singleton.addInteraction(theInteractable.dictOfInteractions,
                    theInteractionType,
                    new conditionalInteraction(new interactionLevel(levelWhenDamageStarts),
                    new numericalInteraction(numericalVariable.health)));



        theInteractable.dictOfInteractions = interactionCreator.singleton.addInteraction(theInteractable.dictOfInteractions,
                    theInteractionType,
                    new conditionalInteraction(new interactionLevel(instantKillLevel),
                    new interactionEffect(new deathEffect(theInteractable.transform.gameObject))));


    }





    public void addConditionalEffect(GameObject theObject, condition theCondition, Ieffect theEffect)
    {
        conditionalEffects2 theComponent = theObject.GetComponent<conditionalEffects2>();
        if(theComponent == null)
        {
            theComponent = theObject.AddComponent<conditionalEffects2>();
        }


        theComponent.add(theCondition, theEffect);


    }








    //plan EXE2s


    public singleEXE makeNavAgentPlanEXE(GameObject theObjectDoingTheEnaction, Vector3 staticTargetPosition, float offsetRoom = 0f)
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


    public singleEXE makeNavAgentPlanEXE(GameObject theObjectDoingTheEnaction, GameObject possiblyMobileActualTarget, float offsetRoom = 0f)
    {
        if (possiblyMobileActualTarget == null)
        {
            Debug.Log("target is null, so plan to walk to target is null");
            Debug.Log(possiblyMobileActualTarget.GetInstanceID());
            return null;
        }
        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = theObjectDoingTheEnaction.GetComponent<navAgent>();


        vect3EXE2 theEXE = new vect3EXE2(theNavAgent, possiblyMobileActualTarget);//placeholderTarget1);
                                                                                  //theEXE.debugPrint = printThisNPC;


        //proximity condition = new proximity(theObjectDoingTheEnactions, possiblyMobileActualTarget, offsetRoom * 1.4f);
        proximityRef condition = new proximityRef(theObjectDoingTheEnaction, theEXE, offsetRoom);// * 1.4f);
        //condition.debugPrint = theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        return theEXE;
    }












    public repeatWithTargetPicker meleeDodge(GameObject theObjectDoingTheEnaction)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new objectHasTag(tagging2.tag2.threat1),
            //new stickyTrueCriteria(new lineOfSight(theObjectDoingTheEnaction), 40),
            new stickyTrueCriteria(new proximityCriteriaBool(theObjectDoingTheEnaction, 40))
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        objectSetGrabber theObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);

        targetPicker theTargetPicker = new radialFleeingTargeter(theObjectDoingTheEnaction, theObjectSet);


        //whew!!


        permaPlan2 thePerma = new permaPlan2(genGen.singleton.makeNavAgentPlanEXE(
                theObjectDoingTheEnaction,
                theTargetPicker.pickNext().realPositionOfTarget()
                ));

        repeatWithTargetPicker theRep = new repeatWithTargetPicker(thePerma, theTargetPicker);

        return theRep;
    }
    public repeatWithTargetPicker meleeDodge(GameObject theObjectDoingTheEnaction, objectListCacheSetter theCacher)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new objectHasTag(tagging2.tag2.threat1),
            new lineOfSight(theObjectDoingTheEnaction),
            new proximityCriteriaBool(theObjectDoingTheEnaction,120)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        objectSetGrabber theObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        theCacher.add(theObjectSet);

        targetPicker theTargetPicker = new radialFleeingTargeter(theObjectDoingTheEnaction, theCacher);


        //whew!!


        permaPlan2 thePerma = new permaPlan2(genGen.singleton.makeNavAgentPlanEXE(
                theObjectDoingTheEnaction,
                theTargetPicker.pickNext().realPositionOfTarget()
                ));

        repeatWithTargetPicker theRep = new repeatWithTargetPicker(thePerma, theTargetPicker);


        //try to shorten?
        //repeatWithTargetPicker theRep = meleeDodge(theObjectDoingTheEnaction);
        //theRep.theTargetPicker

        return theRep;
    }










    public enactEffect addAdhocReproduction1(GameObject theObjectToAddItTo, objectGen theObjectToGenerate)
    {

        Ieffect e1 = new generateObject(theObjectToGenerate);//new adhocAnimal1Gen();

        enactEffect theEnaction = enactEffect.addEnactEffectAndReturn(theObjectToAddItTo,e1);//; theEnaction = theObjectToAddItTo.AddComponent<enactEffect>();

        return theEnaction;
    }


    internal repeater reproRepeater(GameObject theObjectDoingTheEnaction, adhocAnimal1Gen adhocAnimal1Gen, stuffType stuffX)
    {
        enactEffect theEnaction = genGen.singleton.addAdhocReproduction1(theObjectDoingTheEnaction, new adhocAnimal1Gen(theObjectDoingTheEnaction, stuffX));

        //gahhhhhhhhh, need to be able to cut through this garbage?
        //repeater newRep = new agnostRepeater(new permaPlan2(new singleEXE( theEnaction);

        throw new NotImplementedException();
    }








    internal inventory1 ensureInventory1Script(GameObject onThisObject)
    {
        inventory1 ensuredThing = onThisObject.GetComponent<inventory1>();
        if (ensuredThing == null)
        {
            ensuredThing = onThisObject.AddComponent<inventory1>();
        }

        return ensuredThing;
    }

    internal virtualGamepad ensureVirtualGamePad(GameObject onThisObject)
    {
        virtualGamepad ensuredThing = onThisObject.GetComponent<virtualGamepad>();
        if (ensuredThing == null)
        {
            ensuredThing = onThisObject.AddComponent<virtualGamepad>();
        }

        return ensuredThing;
    }

    internal NavMeshAgent ensureNavmeshAgent(GameObject onThisObject)
    {
        NavMeshAgent ensuredThing = onThisObject.GetComponent<NavMeshAgent>();
        if (ensuredThing == null)
        {
            ensuredThing = onThisObject.AddComponent<NavMeshAgent>();
        }

        return ensuredThing;
    }



    public void ensureSafetyForDeletion(GameObject theObject)
    {
        if (theObject.GetComponent<safeDestroy>() == null)
        {
            theObject.AddComponent<safeDestroy>();
        }
    }

}



public interface objectGen
{
    GameObject generate();
}

public class genPrefabAndModify : objectGen
{
    GameObject thePrefab;
    Vector3 thePoint = new Vector3();

    List<objectModifier> theModifiers = new List<objectModifier>();


    public genPrefabAndModify(GameObject thePrefabIn, objectModifier mod1)
    {
        thePrefab = thePrefabIn;
        theModifiers.Add(mod1);
    }
    public genPrefabAndModify(GameObject thePrefabIn, objectModifier mod1, objectModifier mod2)
    {
        thePrefab = thePrefabIn;
        theModifiers.Add(mod1);
        theModifiers.Add(mod2);
    }


    public GameObject generate()
    {
        GameObject theObject = genGen.singleton.createPrefabAtPointAndRETURN(thePrefab, thePoint);

        foreach (objectModifier modifier in theModifiers)
        {
            modifier.modify(theObject);
        }

        return theObject;
    }

    internal void spawn(Vector3 positionInput)
    {
        GameObject theObject = generate();

        theObject.transform.position = positionInput;
    }






    /*
    public void createPrefabAtPoint(GameObject thePrefab, Vector3 thePoint)
    {
        //just so i can keep the rotation of the object i input, for now:
        Instantiate(thePrefab, thePoint, thePrefab.transform.rotation);
    }

    public GameObject createPrefabAtPointAndRETURN(GameObject thePrefab, Vector3 thePoint)
    {
        //just so i can keep the rotation of the object i input, for now:
        return Instantiate(thePrefab, thePoint, thePrefab.transform.rotation);
    }

    public GameObject createAndReturnPrefabAtPointWITHNAME(GameObject thePrefab, Vector3 thePoint, string theName)
    {

        //just so i can keep the rotation of the object i input, for now:
        GameObject newObject = Instantiate(thePrefab, thePoint, thePrefab.transform.rotation);

        newObject.name = theName;

        return newObject;
    }
    */
}

public class adhocAnimal1Gen : objectGen
{
    GameObject parent;
    stuffType stuffTypeX;
    float scale = 1f;

    public adhocAnimal1Gen(GameObject parentIn, stuffType stuffTypeXIn, float scaleIn = 1f)
    {
        parent = parentIn;
        stuffTypeX = stuffTypeXIn;
        scale = scaleIn;
    }

    public GameObject generate()
    {
        return returnBasicAnimal1(parent.transform.position, stuffTypeX, scale);
    }


    internal void spawn(Vector3 positionInput)
    {
        GameObject theObject = generate();

        theObject.transform.position = positionInput;
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
        animalUpdate theUpdate = newObj.AddComponent<animalUpdate>();
        //Debug.Log("?????????????????????    2   ??????????????????????????");
        theUpdate.theFSM = herbavoreForagingBehavior1(newObj, stuffTypeX);


        newObj.GetComponent<interactable2>().dictOfIvariables[numericalVariable.cooldown] = 0f;

        //Debug.Log("????????????????????      3   ???????????????????????");
        //grabStuffStuff.addGrabStuffStuff(newObj, stuffTypeX);

        return newObj;
    }




    public GameObject returnArrowForward(Vector3 where, float scale = 1f)
    {

        GameObject newObj = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.placeHolderCubePrefab, where);// Instantiate(repository2.singleton.placeHolderCubePrefab, where, Quaternion.identity);
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


    animalFSM herbavoreForagingBehavior1(GameObject theObjectDoingTheEnaction, stuffType stuffX)
    {




        //enactEffect theEnaction = genGen.singleton.addAdhocReproduction1(theObjectDoingTheEnaction, new adhocAnimal1Gen(stuffX));

        condition switchCondition1 = new stickyCondition(new canSeeStuffStuff(theObjectDoingTheEnaction, stuffX), 90);

        animalFSM theFSM = new animalFSM(randomWanderRepeatable(theObjectDoingTheEnaction));
        animalFSM getFood = new animalFSM(grabTheStuff(theObjectDoingTheEnaction, stuffX));
        animalFSM flee = new animalFSM(genGen.singleton.meleeDodge(theObjectDoingTheEnaction));



        //animalFSM repro = new animalFSM(genGen.singleton.reproRepeater(theObjectDoingTheEnaction, new adhocAnimal1Gen(stuffX)));

        animalFSM repro = repro1(theObjectDoingTheEnaction,stuffX);



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

        //objectSetGrabber theFleeFromObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        objectSetGrabber theFleeFromObjectSet = new allObjectsInSetThatMeetCriteria(new excludeX(new allObjectsInZone(theObjectDoingTheEnaction), theObjectDoingTheEnaction), theCriteria);



        //condition switchCondition1 = new canSeeStuffStuff(theObjectDoingTheEnaction, stuffX);

        condition switchToFlee = new stickyCondition(
            new isThereAtLeastOneObjectInSet(theFleeFromObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);



        objectCriteria reproCriteria = new objectMeetsAllCriteria(
            new proximityCriteriaBool(theObjectDoingTheEnaction, 40)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        //objectSetGrabber theFleeFromObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        objectSetGrabber theReproSet = new allObjectsInSetThatMeetCriteria(
            new excludeX(new allNearbyNumericalVariable(theObjectDoingTheEnaction, numericalVariable.health), theObjectDoingTheEnaction),
            reproCriteria);

        condition switchCondition3 = new reverseCondition(
            new isThereAtLeastOneObjectInSet(theReproSet));


        //wander.addSwitchAndReverse(new stickyCondition(switchCondition1, 90), grabMeat);
        theFSM.addSwitchAndReverse(new stickyCondition(switchToFlee, 10), flee);
        theFSM.addSwitchAndReverse(new stickyCondition(switchCondition1, 10), getFood);
        theFSM.addSwitchAndReverse(switchCondition3, repro);
        getFood.addSwitch(new stickyCondition(switchToFlee, 10), flee);
        getFood.addSwitch(switchCondition3, repro);





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

        objectSetGrabber theFleeFromObjectSet = new allObjectsInSetThatMeetCriteria(new excludeX(new allObjectsInZone(theObjectDoingTheEnaction), theObjectDoingTheEnaction), theCriteria);



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

        objectSetGrabber theFleeFromObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);



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

    animalFSM repro1(GameObject theObjectDoingTheEnaction, stuffType stuffX)
    {


        Ieffect e1 = new generateObject(new adhocAnimal1Gen(theObjectDoingTheEnaction, stuffX));//new adhocAnimal1Gen();

        enactEffect theEnaction = enactEffect.addEnactEffectAndReturn(theObjectDoingTheEnaction, e1);//; theEnaction = theObjectToAddItTo.AddComponent<enactEffect>();


        singleEXE step1 = (singleEXE)theEnaction.standardEXEconversion();
        step1.untilListFinished();

        //singleEXE step1 = new boolEXE2(theEnaction);

        animalFSM repro = new animalFSM(theObjectDoingTheEnaction, step1);

        //animalFSM repro = new animalFSM(genGen.singleton.reproRepeater(theObjectDoingTheEnaction, new adhocAnimal1Gen(stuffX)));





        //enactEffect theEnaction = genGen.singleton.addAdhocReproduction1(theObjectDoingTheEnaction, new adhocAnimal1Gen(stuffX));

        //gahhhhhhhhh, need to be able to cut through this garbage?
        //repeater newRep = new agnostRepeater(new permaPlan2(new singleEXE(theEnaction);



        /*
        public animalFSM(GameObject theObjectDoingTheEnaction, singleEXE step1)

        animalFSM repro = new animalFSM(genGen.singleton.reproRepeater(theObjectDoingTheEnaction, new adhocAnimal1Gen(stuffX)));




        enactEffect theEnaction = genGen.singleton.addAdhocReproduction1(theObjectDoingTheEnaction, new adhocAnimal1Gen(stuffX));

        gahhhhhhhhh, need to be able to cut through this garbage?
        repeater newRep = new agnostRepeater(new permaPlan2(new singleEXE(theEnaction);

        throw new NotImplementedException();

        */

        return repro;
    }

    private repeatWithTargetPicker randomWanderRepeatable(GameObject theObjectDoingTheEnaction)
    {
        singleEXE step1 = genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnaction, patternScript2.singleton.randomNearbyVector(theObjectDoingTheEnaction.transform.position));
        permaPlan2 perma1 = new permaPlan2(step1);
        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();
        //simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);
        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new pickRandomNearbyLocation(theObjectDoingTheEnaction));

        return repeatWithTargetPickerTest;
    }
    private repeatWithTargetPicker grabTheStuff(GameObject theObjectDoingTheEnaction, stuffType stuffX)
    {
        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));
        //perma1 = new permaPlan2(step1);

        //repeatWithTargetPicker otherBehavior = new repeatWithTargetPicker(perma1, new pickRandomNearbyLocation(this.gameObject));


        return new ummAllThusStuffForGrab(theObjectDoingTheEnaction, stuffX).returnTheRepeatTargetThing();
    }






}










public interface objectModifier
{
    void modify(GameObject theObject);
}

public class multiModify : objectModifier
{

    List<objectModifier> theModifiers;

    public void modify(GameObject theObject)
    {
        foreach (objectModifier modifier in theModifiers)
        {
            modifier.modify(theObject);
        }
    }
}