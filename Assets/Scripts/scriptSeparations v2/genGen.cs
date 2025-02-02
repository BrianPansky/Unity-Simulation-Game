using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static enactionCreator;
using static interactionCreator;
using static tagging2;
using static UnityEngine.GraphicsBuffer;
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
    
    public void makeEnactionsWithTorsoArticulation1(playable2 thePlayable)
    {
        hitscanEnactor.addHitscanEnactor(thePlayable.gameObject, thePlayable.enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.standardClick));


        vecTranslation.addVecTranslation(thePlayable.gameObject, thePlayable.speed, buttonCategories.vector1);

        navAgent.addNavAgentEnaction(thePlayable.gameObject);

        aimTarget.addAimTargetAndVecRotation(thePlayable.gameObject, thePlayable.lookSpeed, thePlayable.theHorizontalRotationTransform, thePlayable.theVerticalRotationTransform, buttonCategories.vector2);
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

        numericalEffect(theEquippable, numericalVariable.cooldown, 40);

        projectileLauncher theShooter = theEquippable.transform.gameObject.GetComponent<projectileLauncher>();
        enactEffect theFiringEffectOnCooldown = theEquippable.transform.gameObject.GetComponent<enactEffect>();

        //IEnactaBool theFiringEffectOnCooldown = enactEffect.returnEnactEffect(new numericalEffect(theEquippable, numericalVariable.cooldown));





        //Debug.Assert(enactEffect.returnEnactEffect(new deathEffect(theEquippable.transform.gameObject)) != null);
        //Debug.Assert(theFiringEffectOnCooldown != null);
        Debug.Assert(theFiringEffectOnCooldown != null);

        condition cooldownCondition = new numericalCondition(numericalVariable.cooldown, theEquippable.dictOfIvariables);

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

    public static void numericalEffect(equippable2 theEquippable, numericalVariable numVarX, int amountToSubtract = 1)
    {
        enactEffect.addEnactEffect(theEquippable.transform.gameObject, new numericalEffect(theEquippable, numericalVariable.cooldown, amountToSubtract));
    }

    public void deathWhenHealthIsZero(interactable2 theInteractable)
    {

        //i'll put conditional effects in this function for now
        condition theCondition = new numericalCondition(numericalVariable.health, theInteractable.dictOfIvariables);
        Ieffect theEffect = new deathEffect(theInteractable.transform.gameObject);
        //List<Ieffect> list = new List<Ieffect> { theEffect };
        //theInteractable.conditionalEffects[theCondition] = list;
        addConditionalEffect(theInteractable.transform.gameObject, theCondition, theEffect);
    }

    public static void createWeaponLevels(interactable2 theInteractable, interType theInteractionType, int levelWhenDamageStarts, int instantKillLevel)
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
    public singleEXE makeNavAgentPlanEXE(GameObject theObjectDoingTheEnaction, targetPicker theTargetPicker, float offsetRoom = 0f)
    {
        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = theObjectDoingTheEnaction.GetComponent<navAgent>();


        vect3EXE2 theEXE = new vect3EXE2(theNavAgent, theTargetPicker);//placeholderTarget1);
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

        objectSetGrabber theObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);

        targetPicker theTargetPicker = new radialFleeingTargetPicker(theObjectDoingTheEnaction, theObjectSet);


        //whew!!


        permaPlan2 thePerma = new permaPlan2(genGen.singleton.makeNavAgentPlanEXE(
                theObjectDoingTheEnaction,
                theTargetPicker.pickNext().realPositionOfTarget()
                ));

        repeatWithTargetPicker theRep = new repeatWithTargetPicker(thePerma, theTargetPicker);

        return theRep;
    }
    public repeatWithTargetPicker meleeDodge(GameObject theObjectDoingTheEnaction, objectSetGrabberAndCacheSetter theCacher)//??? surely thic should be cache RECEIVER????
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new objectHasTag(tagging2.tag2.threat1),
            new lineOfSight(theObjectDoingTheEnaction),
            new proximityCriteriaBool(theObjectDoingTheEnaction,120)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        objectSetGrabber theObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        theCacher.add(theObjectSet);

        targetPicker theTargetPicker = new radialFleeingTargetPicker(theObjectDoingTheEnaction, theCacher);


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


    /*

    public repeater reproRepeater(GameObject theObjectDoingTheEnaction, adhocAnimal1Gen adhocAnimal1Gen, stuffType stuffX)
    {
        enactEffect theEnaction = genGen.singleton.addAdhocReproduction1(theObjectDoingTheEnaction, new adhocAnimal1Gen(theObjectDoingTheEnaction, stuffX));

        //gahhhhhhhhh, need to be able to cut through this garbage?
        //repeater newRep = new agnostRepeater(new permaPlan2(new singleEXE( theEnaction);

        throw new NotImplementedException();
    }
    */







    public inventory1 ensureInventory1Script(GameObject onThisObject)
    {
        inventory1 ensuredThing = onThisObject.GetComponent<inventory1>();
        if (ensuredThing == null)
        {
            ensuredThing = onThisObject.AddComponent<inventory1>();
        }

        return ensuredThing;
    }

    public virtualGamepad ensureVirtualGamePad(GameObject onThisObject)
    {
        virtualGamepad ensuredThing = onThisObject.GetComponent<virtualGamepad>();
        if (ensuredThing == null)
        {
            ensuredThing = onThisObject.AddComponent<virtualGamepad>();
        }

        return ensuredThing;
    }

    public NavMeshAgent ensureNavmeshAgent(GameObject onThisObject)
    {
        NavMeshAgent ensuredThing = onThisObject.GetComponent<NavMeshAgent>();
        if (ensuredThing == null)
        {
            ensuredThing = onThisObject.AddComponent<NavMeshAgent>();
        }

        return ensuredThing;
    }

    public FSMcomponent ensureFSMcomponent(GameObject onThisObject)
    {
        FSMcomponent ensuredThing = onThisObject.GetComponent<FSMcomponent>();
        if (ensuredThing == null)
        {
            ensuredThing = onThisObject.AddComponent<FSMcomponent>();
        }

        return ensuredThing;
    }


    public interactable2 ensureInteractable2(GameObject theObject)
    {
        interactable2 theComponent = theObject.GetComponent<interactable2>();
        if (theComponent == null)
        {
            theComponent = theObject.AddComponent<interactable2>();
        }

        return theComponent;
    }

    public void ensureSafetyForDeletion(GameObject theObject)
    {
        if (theObject.GetComponent<safeDestroy>() == null)
        {
            theObject.AddComponent<safeDestroy>();
        }
    }

}






































public class gunComponent1: equippable2
{
    public void Awake()
    {
        this.GetComponent<equippable2>().Awake();

        initializeStandardEnactionPoint1(this, 0.2f, 0.3f);
    }

    void Update()
    {
        doCooldown();
    }
}



public class paintByNumbersGun1 :  objectGen //equippable2
{
    //annoying, messy variable initialization silliness.  just input into a function that uses it?  but want to STORE it to generate LATER, sighhhh
    float magnitudeOfInteraction;
    int firingRate;
    float projectileSpeed;
    float projectileSize;
    bool sdOnCollision = true;
    int level = 0;
    float gunHeight = 1;
    float gunWidth = 1;
    float gunLength = 1;
    
    public paintByNumbersGun1(float magnitudeOfInteractionIn, int firingRateIn, float projectileSpeedIn, float projectileSizeIn, bool sdOnCollisionIn = true, int levelIn=0, float gunHeightIn=1, float gunWidthIn = 1, float gunLengthIn = 1)
    {
        magnitudeOfInteraction = magnitudeOfInteractionIn;
        firingRate = firingRateIn;
        projectileSpeed = projectileSpeedIn;
        projectileSize = projectileSizeIn;
        sdOnCollision = sdOnCollisionIn;
        level = levelIn;
        gunHeight = gunHeightIn;
        gunWidth = gunWidthIn;
        gunLength = gunLengthIn;
    }

    public GameObject generate()
    {
        return gunGen2(magnitudeOfInteraction,firingRate,projectileSpeed,projectileSize,sdOnCollision,level,gunHeight,gunWidth,gunLength);
    }


    public GameObject gunGen2(float magnitudeOfInteraction, int firingRate, float projectileSpeed, float projectileSize, bool sdOnCollision = true, int level = 0, float gunHeight = 1, float gunWidth = 1, float gunLength = 1)
    {
        GameObject theObject = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);
        theObject.transform.localScale = new Vector3(gunWidth, gunHeight, gunLength);
        equippable2 theEquippable = theObject.AddComponent<gunComponent1>();
        theEquippable.dictOfIvariables[numericalVariable.cooldown] = 0f;
        addNumericalEffect(theEquippable, numericalVariable.cooldown, firingRate);

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
            new projectileToGenerate(projectileSpeed, sdOnCollision, 99, 0),
            20);


        projectileLauncher theShooter = theEquippable.transform.gameObject.GetComponent<projectileLauncher>();
        enactEffect theFiringEffectOnCooldown = theEquippable.transform.gameObject.GetComponent<enactEffect>();

        //IEnactaBool theFiringEffectOnCooldown = enactEffect.returnEnactEffect(new numericalEffect(theEquippable, numericalVariable.cooldown));





        //Debug.Assert(enactEffect.returnEnactEffect(new deathEffect(theEquippable.transform.gameObject)) != null);
        //Debug.Assert(theFiringEffectOnCooldown != null);
        Debug.Assert(theFiringEffectOnCooldown != null);

        condition cooldownCondition = new numericalCondition(numericalVariable.cooldown, theEquippable.dictOfIvariables);

        //compoundEnactaBool.addCompoundEnactaBool(theEquippable.transform.gameObject, buttonCategories.primary, theShooter, theFiringEffectOnCooldown, cooldownCondition);
        theShooter.linkedEnactionAtoms.Add(theFiringEffectOnCooldown);//messy [but better than the above "compound" nonsense]
        theShooter.gamepadButtonType = buttonCategories.primary;




        /*
        projectileLauncher.addProjectileLauncher(theEquippable.transform.gameObject,
            theEquippable.enactionPoint1.transform,
            buttonCategories.primary,
            new interactionInfo(interType.peircing, magnitudeOfInteraction, level),
            new projectileToGenerate(speed, sdOnCollision, 999, 0),
            20);

        */

        return theObject;
    }
    public static void addNumericalEffect(equippable2 theEquippable, numericalVariable numVarX, int amountToSubtract = 1)
    {
        enactEffect.addEnactEffect(theEquippable.transform.gameObject, new numericalEffect(theEquippable, numericalVariable.cooldown, amountToSubtract));
    }

}


public class newGunGen : objectGen //equippable2
{
    //annoying, messy variable initialization silliness.  just input into a function that uses it?  but want to STORE it to generate LATER, sighhhh
    //AHA!  use CLASSES!  my "object modifiers"

    /*
    float magnitudeOfInteraction;
    int firingRate;
    float projectileSpeed;
    float projectileSize;
    bool sdOnCollision = true;
    int level = 0;
    float gunHeight = 1;
    float gunWidth = 1;
    float gunLength = 1;
    */

    genObjectAndModify mainGunGenerator;
    genObjectAndModify projectileType;
    int firingRate = 40;

    public newGunGen(genObjectAndModify projectileTypeIn, int firingRateIn = 40)
    {
        //theBullets = new genObjectAndModify(, projectileModifiers);
        //theGun = new genObjectAndModify(new thingShell(projectileType), gunModifiers);
        projectileType = projectileTypeIn;
        firingRate = firingRateIn;
        mainGunGenerator = new genObjectAndModify(new cubeGen());
    }
    public newGunGen(genObjectAndModify projectileTypeIn, objectModifier[] gunModifiers, int firingRateIn = 40)
    {
        //theBullets = new genObjectAndModify(, projectileModifiers);
        //theGun = new genObjectAndModify(new thingShell(projectileType), gunModifiers);
        projectileType = projectileTypeIn;
        firingRate = firingRateIn;
        mainGunGenerator = new genObjectAndModify(new cubeGen(), gunModifiers);
    }


    public GameObject generate()
    {
        //generateObjectAtLocation

        return gunGen3();
    }


    public GameObject gunGen3()
    {
        GameObject theObject = mainGunGenerator.generate();//repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, new Vector3(), Quaternion.identity);

        /*
        theObject.transform.localScale = new Vector3(gunWidth, gunHeight, gunLength);
        equippable2 theEquippable = theObject.AddComponent<gunComponent1>();
        theEquippable.dictOfIvariables[numericalVariable.cooldown] = 0f;
        addNumericalEffect(theEquippable, numericalVariable.cooldown, firingRate);
        */

        /*
        projectileLauncher theShooter = new projectileLauncher(theEquippable.enactionPoint1.transform, 
            buttonCategories.errorYouDidntSetEnumTypeForBUTTONCATEGORIES, 
            new interactionInfo(interType.peircing, magnitudeOfInteraction, level),
            new projectileToGenerate(speed, sdOnCollision, 999, 0));
        */

        /*
        //bit messy?  made "thisButtonCategoryIntentionallyLeftBlank" so that i can add component to object [thus easy search object for component of that type] WITHOUT having it plug into a gamepad button when equipped......
        projectileLauncher.addProjectileLauncher(theEquippable.transform.gameObject,
            theEquippable.enactionPoint1.transform,
            buttonCategories.thisButtonCategoryIntentionallyLeftBlank,
            new interactionInfo(interType.peircing, magnitudeOfInteraction, level),
            new projectileToGenerate(projectileSpeed, sdOnCollision, 99, 0),
            20);

        */


        //hard to turn gun component and cooldown and enaction point generator into "object modifiers" because they have to be LINKED, not totally separate/atomized/independent components???  or can i plug one into another?  one modifier into another?  hmmm, ehh....
        gunComponent1 theEquippable = theObject.AddComponent<gunComponent1>();
        theEquippable.initializeStandardEnactionPoint1(theObject, theEquippable.transform.localScale.x, 0.1f);

        theEquippable.dictOfIvariables[numericalVariable.cooldown] = 0f;
        addNumericalEffect(theEquippable, numericalVariable.cooldown, firingRate);

        //enactEffect theEnaction = theObject.AddComponent<enactEffect>();
        //theEnaction.theEffect = new generateObjectAtLocation(projectileType, theEquippable.enactionPoint1);

        createAuthoredObject theEnaction = createAuthoredObject.addThisEnactionAndReturn(theObject,projectileType, theEquippable.enactionPoint1);




        //projectileLauncher theShooter = theEquippable.transform.gameObject.GetComponent<projectileLauncher>();
        enactEffect theFiringEffectOnCooldown = theEquippable.transform.gameObject.GetComponent<enactEffect>();

        //IEnactaBool theFiringEffectOnCooldown = enactEffect.returnEnactEffect(new numericalEffect(theEquippable, numericalVariable.cooldown));





        //Debug.Assert(enactEffect.returnEnactEffect(new deathEffect(theEquippable.transform.gameObject)) != null);
        //Debug.Assert(theFiringEffectOnCooldown != null);
        Debug.Assert(theFiringEffectOnCooldown != null);

        condition cooldownCondition = new numericalCondition(numericalVariable.cooldown, theEquippable.dictOfIvariables);

        //compoundEnactaBool.addCompoundEnactaBool(theEquippable.transform.gameObject, buttonCategories.primary, theShooter, theFiringEffectOnCooldown, cooldownCondition);
        theEnaction.linkedEnactionAtoms.Add(theFiringEffectOnCooldown);//messy [but better than the above "compound" nonsense]
        theEnaction.gamepadButtonType = buttonCategories.primary;




        /*
        projectileLauncher.addProjectileLauncher(theEquippable.transform.gameObject,
            theEquippable.enactionPoint1.transform,
            buttonCategories.primary,
            new interactionInfo(interType.peircing, magnitudeOfInteraction, level),
            new projectileToGenerate(speed, sdOnCollision, 999, 0),
            20);

        */

        return theObject;
    }
    
    public static void addNumericalEffect(equippable2 theEquippable, numericalVariable numVarX, int amountToSubtract = 1)
    {
        enactEffect.addEnactEffect(theEquippable.transform.gameObject, new numericalEffect(theEquippable, numericalVariable.cooldown, amountToSubtract));
    }

}


public class sphereGen : objectGen
{
    /*
    public GameObject makeEmptyIntSphere(Vector3 position)
    {
        GameObject prefabToUse = repository2.singleton.interactionSphere;
        GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, position);

        return newProjectile;
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

    */

    public GameObject generate()
    {
        GameObject prefabToUse = repository2.singleton.interactionSphere;
        GameObject newObj = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, new Vector3());

        return newObj;
    }
}
public class cubeGen : objectGen
{
    public GameObject generate()
    {
        GameObject prefabToUse = repository2.singleton.placeHolderCubePrefab;
        GameObject newObj = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, new Vector3());

        return newObj;
    }
}

public class paintByNumbersProjectile : objectGen
{
    Vector3 direction = new Vector3();

    public GameObject generate()
    {
        throw new System.NotImplementedException();
    }
}





public class FSM
{
    public bool debugBool = false;
    public string name;
    //internal Dictionary<multicondition, FSM> switchBoard = new Dictionary<multicondition, FSM>();
    internal Dictionary<condition, FSM> switchBoard = new Dictionary<condition, FSM>();

    internal List<repeater> repeatingPlans = new List<repeater>();



    public FSM doAFrame()
    {
        if(debugBool == true)
        {
            Debug.Log("the name of this FSM:  " + name);
        }
        //Debug.Log("the name of this FSM:  " + name);

        //Debug.Log("the base enactions of this FSM:  " + baseEnactionsAsText());  //null error because "nested" repeaters don't store a perma plan in the top shell

        FSM toSwitchTo = null;

        toSwitchTo = firstMetSwitchtCondition();
        if (toSwitchTo != null)
        {
            //Debug.Log("switch");
            //Debug.Log("toSwitchTo:  " + toSwitchTo);
            //Debug.Log("(toSwitchTo==this):  " + (toSwitchTo==this));
            toSwitchTo.refillAllRepeaters();
            return toSwitchTo;
        }





        //Debug.Log("repeatingPlans.count:  " + repeatingPlans.Count);
        foreach (repeater plan in repeatingPlans)
        {
            //Debug.Log("plan:  " + plan);
            plan.doThisThing();
        }




        return this;
    }


    public void refillAllRepeaters()
    {
        foreach (repeater plan in repeatingPlans)
        {
            plan.refill();
        }
    }


    private FSM firstMetSwitchtCondition()
    {
        foreach (condition thisCondition in switchBoard.Keys)
        {
            //Debug.Log(".................thisCondition:  " + thisCondition + " " + thisCondition.GetHashCode());
            //Debug.Log("thisCondition.asTextSHORT():  " + thisCondition.asTextSHORT() + " " + thisCondition.GetHashCode());
            //Debug.Log("thisCondition.asText():  " + thisCondition.asText() + " " + thisCondition.GetHashCode());
            //Debug.Log("thisCondition.asTextBaseOnly():  " + thisCondition.asTextBaseOnly() + " " + thisCondition.GetHashCode());
            if (thisCondition.met())
            {
                //Debug.Log("thisCondition is met:  " + thisCondition+" "+thisCondition.GetHashCode());
                return switchBoard[thisCondition];
            }
        }

        return null;
    }







    public void addSwitch(condition switchCondition, repeater doThisAfterSwitchCondition)
    {

        FSM otherFSM = new generateFSM(doThisAfterSwitchCondition);

        switchBoard[switchCondition] = otherFSM;
    }
    public void addSwitch(condition switchCondition, FSM otherFSM)
    {
        switchBoard[switchCondition] = otherFSM;
    }

    public void addSwitchAndReverse(condition switchCondition, repeater doThisAfterSwitchCondition)
    {

        FSM otherFSM = new generateFSM(doThisAfterSwitchCondition);

        switchBoard[switchCondition] = otherFSM;
        otherFSM.switchBoard[new reverseCondition(switchCondition)] = this;
    }
    public void addSwitchAndReverse(condition switchCondition, FSM otherFSM)
    {
        switchBoard[switchCondition] = otherFSM;
        otherFSM.switchBoard[new reverseCondition(switchCondition)] = this;
    }


    public string baseEnactionsAsText()
    {
        string newString = "";
        //newString += "the base enactions of this FSM:  ";
        newString += "[";
        foreach (repeater thisRep in repeatingPlans)
        {

            //newString += thisRep.thePerma.printBaseEnactions();
            //Debug.Assert(thisRep != null);
            //Debug.Assert(thisRep.thePerma != null);
            newString += thisRep.baseEnactionsAsText();
        }

        newString += "]";
        //Debug.Log(newString);
        return newString;
    }
}

public class generateFSM : FSM
{

    public generateFSM()
    {
        //      new permaPlan();
    }

    public generateFSM(repeater doThisImmediately)
    {
        //justDoThisForNow = doThisImmediately;

        repeatingPlans.Add(doThisImmediately);

    }

    public generateFSM(GameObject theObjectDoingTheEnaction, singleEXE step1)
    {
        permaPlan2 perma1 = new permaPlan2(step1);
        agnostRepeater repeatWithTargetPickerTest = new agnostRepeater(perma1);
        repeatingPlans.Add(repeatWithTargetPickerTest);

    }
    public generateFSM(GameObject theObjectDoingTheEnaction, singleEXE step1, singleEXE step2)
    {
        permaPlan2 perma1 = new permaPlan2(step1, step2);
        agnostRepeater repeatWithTargetPickerTest = new agnostRepeater(perma1);
        repeatingPlans.Add(repeatWithTargetPickerTest);

    }

    public generateFSM(GameObject theObjectDoingTheEnaction, singleEXE step1, singleEXE step2, singleEXE step3)
    {
        permaPlan2 perma1 = new permaPlan2(step1, step2, step3);
        agnostRepeater repeatWithTargetPickerTest = new agnostRepeater(perma1);
        repeatingPlans.Add(repeatWithTargetPickerTest);

    }
    public generateFSM(GameObject theObjectDoingTheEnaction, singleEXE step1, singleEXE step2, singleEXE step3, singleEXE step4)
    {
        permaPlan2 perma1 = new permaPlan2(step1, step2, step3, step4);
        agnostRepeater repeatWithTargetPickerTest = new agnostRepeater(perma1);
        repeatingPlans.Add(repeatWithTargetPickerTest);

    }

    public generateFSM(repeater doThisImmediately, condition switchCondition, repeater doThisAfterSwitchCondition)
    {
        //justDoThisForNow = doThisImmediately;

        repeatingPlans.Add(doThisImmediately);



        FSM otherFSM = new generateFSM(doThisAfterSwitchCondition, new reverseCondition(switchCondition), this);

        switchBoard[switchCondition] = otherFSM;
    }

    public generateFSM(repeater doThisImmediately, condition switchCondition, FSM doThisAfterSwitchCondition)
    {
        //justDoThisForNow = doThisImmediately;

        repeatingPlans.Add(doThisImmediately);


        //animalFSM otherFSM = new animalFSM(repeatWithTargetPicker2, switchCondition, repeatWithTargetPicker1);

        switchBoard[switchCondition] = doThisAfterSwitchCondition;
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


        vect3EXE2 theEXE = new vect3EXE2(theNavAgent, staticTargetPosition);
        proximityRef condition = new proximityRef(theObjectDoingTheEnaction, theEXE, offsetRoom);
        theEXE.endConditions.Add(condition);

        return theEXE;
    }

}



public class FSMcomponent : MonoBehaviour, IupdateCallable
{
    public bool debugBool = false;
    //public FSM theFSM;
    public List<FSM> theFSMList = new List<FSM>();// = new List<FSM>();  //correct way to do parallel!  right at the top level!!!  one for walking/feet, one for hands/equipping/using items etc.

    public List<IupdateCallable> currentUpdateList { get; set; }
    public void Update()
    {
        if (debugBool == true)
        {
            Debug.Log("theFSMList.Count:  " + theFSMList.Count);
            Debug.Log("currentUpdateList:  " + currentUpdateList);
        }
        //Debug.Log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&     regular Update()        &&&&&&&&&&&&&&&&&&&");
    }

    public void callableUpdate()
    {
        if (debugBool == true)
        {
            Debug.Log("???????????????????????????????????????????????????????????????");
        }
        //Debug.Log("============================     callableUpdate()        =================");
        //Debug.Log("this.gameObject:  " + this.gameObject);
        //tagging2.singleton.printAllTags(this.gameObject);
        foreach (FSM theFSM in theFSMList)
        {
            //Debug.Log("theFSM:  "+ theFSM);
            //theFSM = theFSM.doAFrame();  //will this be an issue?  or only if i add/remove items from list?
        }

        for (int index = 0; index < theFSMList.Count; index++)
        {
            theFSMList[index].debugBool = debugBool;
            theFSMList[index] = theFSMList[index].doAFrame();
        }

    }
}

public class reactivationOfNavMeshAgent : MonoBehaviour
{
    //a quick fix for "reposition errors", where things don't spawn in correct location due to nav mesh agent, somehow
    void Awake()
    {
        this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
    }

    void Start()
    {
        this.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        Destroy(this);
    }
}




public abstract class plugInFSM
{
    public condition switchCondition;
    public FSM theFSM;

    public abstract void addPlugin(FSM theFSMToAddItTo, GameObject theObjectDoingTheEnaction);
}

public abstract class simpleOneStateAndReturn : plugInFSM
{
    public override void addPlugin(FSM theFSMToAddItTo, GameObject theObjectDoingTheEnaction)
    {
        theFSMToAddItTo.addSwitchAndReverse(generateTheSwitchCondition(theObjectDoingTheEnaction), generateTheFSM(theObjectDoingTheEnaction));
    }
    public abstract condition generateTheSwitchCondition(GameObject theObjectDoingTheEnaction);
    public abstract FSM generateTheFSM(GameObject theObjectDoingTheEnaction);
}

public  class skippableProcess : plugInFSM
{
    List<plugInFSM> theStepPlugins = new List<plugInFSM>();
    public skippableProcess(plugInFSM step1)
    {
        theStepPlugins.Add(step1);
    }
    public override void addPlugin(FSM theFSMToAddItTo, GameObject theObjectDoingTheEnaction)
    {
        //so, all steps have their switch to [but not switch back!] conditions from all states below/before them in the list
        //so, we're building a chain of FSMs
        //then adding "switch to" to each of the previous ones on the list so far
        //and then....done?

        List<FSM> theChainOfFSMs = new List<FSM>();
        theChainOfFSMs.Add(theFSMToAddItTo);
        foreach(plugInFSM thisPlugin in theStepPlugins)
        {
            foreach (FSM thisFSM in theChainOfFSMs)
            {
                thisPlugin.addPlugin(thisFSM,theObjectDoingTheEnaction);
            }

            theChainOfFSMs.Add(thisPlugin.theFSM);
        }
    }
}

/*
public class layer : plugInFSM
{
    public override void addPlugin(FSM theFSMToAddItTo, GameObject theObjectDoingTheEnaction)
    {
        fSMgen2.addSwitchAndReverse(switchCondition, theFSM);

        foreach (var thisKey in fSMgen2.switchBoard.Keys)
        {
            fSMgen2.switchBoard[thisKey].addSwitch(switchCondition, theFSM);
        }
    }
}
*/


public class nestedFSM_AsRepeater : repeater//FSM
{
    FSM theFSM;

    public nestedFSM_AsRepeater(FSM theFSMIn)
    {
        theFSM = theFSMIn;
    }

    public override string baseEnactionsAsText()
    {
        return theFSM.baseEnactionsAsText();
    }

    public override void doThisThing()
    {
        theFSM.doAFrame();
    }

    public override void refill()
    {
        //not needed!  the only part of this tiny class that is not slick
    }
}



public class FSMgen2
{
    FSM theBaseFSM;
    //GameObject theObjectDoingTheEnaction;

    public FSMgen2(GameObject theObjectDoingTheEnactionIn)
    {
        //theObjectDoingTheEnaction = theObjectDoingTheEnactionIn;
        theBaseFSM = new generateFSM(); //idle
        theBaseFSM.name = "idle";
        addItToTheirFSMComponent(theObjectDoingTheEnactionIn);
    }

    public FSMgen2(GameObject theObjectDoingTheEnactionIn, plugInFSM addon1)
    {
        //theObjectDoingTheEnaction = theObjectDoingTheEnactionIn;
        //      addon1.addPlugin(this);
        //addItToTheirFSMComponent(theObjectDoingTheEnactionIn);
        theBaseFSM = new generateFSM(); //idle
        theBaseFSM.name = "idle";
        addon1.addPlugin(theBaseFSM, theObjectDoingTheEnactionIn);
        addItToTheirFSMComponent(theObjectDoingTheEnactionIn);

    }
    public FSMgen2(GameObject theObjectDoingTheEnactionIn, baseState state0)
    {
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //theObjectDoingTheEnaction = theObjectDoingTheEnactionIn;
        theBaseFSM = state0.makeState(theObjectDoingTheEnactionIn);
        addItToTheirFSMComponent(theObjectDoingTheEnactionIn);
    }

    public FSMgen2(GameObject theObjectDoingTheEnactionIn, baseState state0, plugInFSM addon1) : this(theObjectDoingTheEnactionIn, state0)
    {

        theBaseFSM = state0.makeState(theObjectDoingTheEnactionIn);
        addon1.addPlugin(theBaseFSM, theObjectDoingTheEnactionIn);

        addItToTheirFSMComponent(theObjectDoingTheEnactionIn);
    }

    public FSMgen2(GameObject theObjectDoingTheEnactionIn, baseState state0, plugInFSM[] plugInFSMs) : this(theObjectDoingTheEnactionIn, state0)
    {
        theBaseFSM = state0.makeState(theObjectDoingTheEnactionIn);

        foreach (plugInFSM thisPlugin in plugInFSMs)
        {
            thisPlugin.addPlugin(theBaseFSM, theObjectDoingTheEnactionIn);
        }

        addItToTheirFSMComponent(theObjectDoingTheEnactionIn);
    }

    public FSMgen2(GameObject theObjectDoingTheEnactionIn,  plugInFSM[] plugInFSMs)
    {
        theBaseFSM = new generateFSM(); //idle
        theBaseFSM.name = "idle";

        foreach (plugInFSM thisPlugin in plugInFSMs)
        {
            thisPlugin.addPlugin(theBaseFSM, theObjectDoingTheEnactionIn);
        }

        addItToTheirFSMComponent(theObjectDoingTheEnactionIn);
    }

    private void addItToTheirFSMComponent(GameObject theObjectDoingTheEnactionIn)
    {
        FSMcomponent theFSMComponent = genGen.singleton.ensureFSMcomponent(theObjectDoingTheEnactionIn);
        theFSMComponent.theFSMList.Add(theBaseFSM);
    }

    public FSM returnIt()
    {
        return theBaseFSM;
    }
    /*
    public void addLayer(plugInFSM outerLayer)
    {
    }
    */



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


        vect3EXE2 theEXE = new vect3EXE2(theNavAgent, staticTargetPosition);
        proximityRef condition = new proximityRef(theObjectDoingTheEnaction, theEXE, offsetRoom);
        theEXE.endConditions.Add(condition);

        return theEXE;
    }
}






public class testNewFSMGenerator
{
    private Vector3 vector3;

    public testNewFSMGenerator(Vector3 vector3)
    {
        this.vector3 = vector3;
    }

    internal void doIt()
    {
        GameObject newObj = new makeBasicHuman(8, 1, 6).generate();
        newObj.transform.position = vector3;




        new FSMgen2(newObj, new randomWanderFSMgen(), new grabFoodFeetFSMGen(stuffType.fruit));
        new FSMgen2(newObj, new grabFoodHandsFSMGen(stuffType.fruit));
    }
}

public abstract class baseState
{

    public FSM theFSM;

    public abstract void addPlugin(GameObject theObjectDoingTheEnaction, FSMgen2 fSMgen2);

    internal abstract FSM makeState(GameObject theObjectDoingTheEnaction);
}

public class randomWanderFSMgen : baseState
{
    public override void addPlugin(GameObject theObjectDoingTheEnaction, FSMgen2 fSMgen2)
    {
    }

    internal override FSM makeState(GameObject theObjectDoingTheEnaction)
    {
        FSM wander = new generateFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());

        wander.name = "feet, wander";
        theFSM = wander;

        return theFSM;
    }
}


public class grabFoodFeetFSMGen : simpleOneStateAndReturn
{
    private stuffType stuffToGrab;

    public grabFoodFeetFSMGen(stuffType stuffToGrabIn)
    {
        stuffToGrab = stuffToGrabIn;
    }

    public override condition generateTheSwitchCondition(GameObject theObjectDoingTheEnaction)
    {
        return grabFoodCondition(theObjectDoingTheEnaction, stuffToGrab);
    }

    private condition grabFoodCondition(GameObject theObjectDoingTheEnaction, stuffType stuffTypeX)
    {

        //Debug.Log("stuffTypeX:  " + stuffTypeX);
        objectSetGrabber theFoodObjectSet = new setOfAllNearbyStuffStuff(theObjectDoingTheEnaction, stuffTypeX);

        condition switchToGrabFood = new stickyCondition(new isThereAtLeastOneObjectInSet(theFoodObjectSet), 7);// theObjectDoingTheEnaction, numericalVariable.health);

        return switchToGrabFood;
    }

    public override FSM generateTheFSM(GameObject theObjectDoingTheEnaction)
    {
        return grabFoodFeetFSM(theObjectDoingTheEnaction, stuffToGrab, 330);
    }


    private FSM grabFoodFeetFSM(GameObject theObjectDoingTheEnaction, stuffType stuffTypeX, float targetDetectionRange = 40)
    {
        FSM grabFood = new generateFSM(
            new goToX(theObjectDoingTheEnaction, grabFoodTargetPicker(theObjectDoingTheEnaction, stuffTypeX), targetDetectionRange).returnIt());

        grabFood.name = "feet, grabFood";
        return grabFood;
    }



    private void changeNavSpeed(GameObject theObject, float speed)
    {
        navAgent theNav = theObject.GetComponent<navAgent>();
        theNav.theAgent.speed = speed;
        theNav.theAgent.baseOffset = 0.5f;//theObjectDoingTheEnaction.transform.localScale.y;
                                          //theNav.theAgent.

        //Debug.Log("targetDetectionRange:  " + targetDetectionRange);


    }



    private targetPicker grabFoodTargetPicker(GameObject theObjectDoingTheEnaction, stuffType stuffTypeX)
    {
        targetPicker theTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, new setOfAllNearbyStuffStuff(theObjectDoingTheEnaction, stuffTypeX));

        return theTargetPicker;
    }


}


public class grabFoodHandsFSMGen: simpleOneStateAndReturn
{
    private stuffType stuffToGrab;

    public grabFoodHandsFSMGen(stuffType stuffToGrabIn)
    {
        stuffToGrab = stuffToGrabIn;
    }

    public override condition generateTheSwitchCondition(GameObject theObjectDoingTheEnaction)
    {
        return new proximityFromTargetPicker(theObjectDoingTheEnaction, grabFoodTargetPicker(theObjectDoingTheEnaction,stuffToGrab), 40);
    }

    /*
    private condition grabFoodCondition(GameObject theObjectDoingTheEnaction, stuffType stuffTypeX)
    {

        //Debug.Log("stuffTypeX:  " + stuffTypeX);
        objectSetGrabber theFoodObjectSet = new setOfAllNearbyStuffStuff(theObjectDoingTheEnaction, stuffTypeX);

        condition switchToGrabFood = new isThereAtLeastOneObjectInSet(theFoodObjectSet));// theObjectDoingTheEnaction, numericalVariable.health);

        return switchToGrabFood;
    }
    */

    public override FSM generateTheFSM(GameObject theObjectDoingTheEnaction)
    {
        return grabFoodHandsFSM(theObjectDoingTheEnaction, stuffToGrab, 40);
    }

    private FSM grabFoodHandsFSM(GameObject theObjectDoingTheEnaction, stuffType stuffTypeX, float distance = 3f)
    {
        FSM grabFood = new generateFSM(grabTheStuff(theObjectDoingTheEnaction, stuffTypeX));

        grabFood.name = "hands, grabFood";
        return grabFood;
    }

    private repeatWithTargetPicker grabTheStuff(GameObject theObjectDoingTheEnaction, stuffType stuffX, float distance = 3f)
    {
        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));
        //perma1 = new permaPlan2(step1);

        //repeatWithTargetPicker otherBehavior = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(this.gameObject));


        //return new ummAllThusStuffForGrab(theObjectDoingTheEnaction, stuffX).returnTheRepeatTargetThing();

        return new aimAtXAndInteractWithY(theObjectDoingTheEnaction, grabFoodTargetPicker(theObjectDoingTheEnaction,stuffX), interType.standardClick, distance).returnIt();
    }




    private targetPicker grabFoodTargetPicker(GameObject theObjectDoingTheEnaction, stuffType stuffTypeX)
    {
        targetPicker theTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, new setOfAllNearbyStuffStuff(theObjectDoingTheEnaction, stuffTypeX));

        return theTargetPicker;
    }

}










public class testNewFSMGenerator2
{

    private Vector3 vector3;

    public testNewFSMGenerator2(Vector3 vector3)
    {
        this.vector3 = vector3;
    }

    internal void doIt()
    {
        GameObject newObj = new makeBasicHuman(8, 1, 6).generate();
        newObj.transform.position = vector3;




        new FSMgen2(newObj, new randomWanderFSMgen(), new plugInFSM[] { new goToPreyFeetFSMGen(), new grabFoodFeetFSMGen(stuffType.meat1) });
        new FSMgen2(newObj, new plugInFSM[] { new killPreyHandsFSMGen(), new grabFoodHandsFSMGen(stuffType.meat1) });
    }
}





public class goToPreyFeetFSMGen : simpleOneStateAndReturn
{
    float targetDetectionRange;
    public goToPreyFeetFSMGen(float targetDetectionRangeIn=90f)
    {
        targetDetectionRange = targetDetectionRangeIn;
    }

    public override condition generateTheSwitchCondition(GameObject theObjectDoingTheEnaction)
    {
        return goToPreyCondition(theObjectDoingTheEnaction);
    }

    private condition goToPreyCondition(GameObject theObjectDoingTheEnaction)
    {

        //Debug.Log("stuffTypeX:  " + stuffTypeX);
        objectSetGrabber theFoodObjectSet = preySet(theObjectDoingTheEnaction);

        condition switchToGrabFood = new isThereAtLeastOneObjectInSet(theFoodObjectSet);// theObjectDoingTheEnaction, numericalVariable.health);

        return switchToGrabFood;
    }

    public override FSM generateTheFSM(GameObject theObjectDoingTheEnaction)
    {
        return goToPreyFeetFSM(theObjectDoingTheEnaction);
    }

    private FSM goToPreyFeetFSM(GameObject theObjectDoingTheEnaction, float targetDetectionRange = 40)
    {
        FSM grabFood = new generateFSM(
            new goToX(theObjectDoingTheEnaction, killPreyTargetPicker(theObjectDoingTheEnaction), targetDetectionRange).returnIt());

        grabFood.name = "feet, goToPrey";
        return grabFood;
    }





    /*
    private condition switchToKillPreyConditionHands()
    {
        //new stickyCondition(switchCondition2, 90)
        //condition switchCondition2 = new canSeeNumericalVariable(theObjectDoingTheEnaction, numericalVariable.health);



        //ok this doesn't work yet because it doesn't ever 'return' "unmet" AKA "FALSE" when there is no target, 
        //beacuse target picker has agnostic calc, ummmm, STUFF..........
        //soooo, need to nest? FIRST see if there IS a target that is returned from picker[or one that meets criteria].
        //.if so, THEN evaluate it on proximity criteria.  and [for the CONDITION] return bool result
        //      condition killPreyCondition = new proximityFromTargetPicker(newObj, killPreyTargetPicker());



        objectCriteria prox = new proximityCriteriaBool(newObj, 0);
        condition killPreyCondition = new stickyCondition(new isThereAtLeastOneObjectInSet(new setOfAllObjectThatMeetCriteria(preySet(), prox)), 0);


        return killPreyCondition;
    }
    */
    private targetPicker killPreyTargetPicker(GameObject theObjectDoingTheEnaction)
    {
        targetPicker theTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, preySet(theObjectDoingTheEnaction));

        return theTargetPicker;
    }

    private objectSetGrabber preySet(GameObject theObjectDoingTheEnaction)
    {
        return new excludeX(new setOfAllNearbyNumericalVariable(theObjectDoingTheEnaction, numericalVariable.health), theObjectDoingTheEnaction);
    }
}


public class killPreyHandsFSMGen : simpleOneStateAndReturn
{

    public override condition generateTheSwitchCondition(GameObject theObjectDoingTheEnaction)
    {
        return new proximityFromTargetPicker(theObjectDoingTheEnaction, killPreyTargetPicker(theObjectDoingTheEnaction), 3);
    }


    public override FSM generateTheFSM(GameObject theObjectDoingTheEnaction)
    {
        return killPreyFSM(theObjectDoingTheEnaction);
    }

    private FSM killPreyFSM(GameObject theObjectDoingTheEnaction)
    {
        //FSM getFood = new generateFSM(grabTheStuff(newObj, stuffTypeX));
        FSM killPrey = new generateFSM(
            new aimAtXAndInteractWithY(theObjectDoingTheEnaction, killPreyTargetPicker(theObjectDoingTheEnaction), interType.melee, 1.7f).returnIt()
            );//new animalFSM(returnTheGoToThingWithNumericalVariableXAndInteractWithTypeY(newObj, numericalVariable.health, interType.melee));

        killPrey.name = "hands, kellPrey";
        return killPrey;
    }



    private targetPicker killPreyTargetPicker(GameObject theObjectDoingTheEnaction)
    {
        targetPicker theTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, preySet(theObjectDoingTheEnaction));

        return theTargetPicker;
    }

    private objectSetGrabber preySet(GameObject theObjectDoingTheEnaction)
    {
        return new excludeX(new setOfAllNearbyNumericalVariable(theObjectDoingTheEnaction, numericalVariable.health), theObjectDoingTheEnaction);
    }



}












public class predatorGen2 : objectGen
{
    tagging2.tag2 team;
    private Vector3 thePosition;

    float health;
    float height;
    float length;
    float width;

    float speed;
    float targetDetectionRange;

    //(tagging2.tag2 theTeamIn, float health, float speed, float height, float width, float targetDetectionRange, objectGen weapon, List<FSM> theBehavior )

    // 
    //basicBodyProperties
    public predatorGen2(tag2 theTeamIn, float health, float height, float length, float width, float speed, float targetDetectionRange)//, objectGen weapon)
    {
        this.team = theTeamIn;
        this.health = health;
        this.height = height;
        this.length = length;
        this.width = width;
        this.speed = speed;
        this.targetDetectionRange = targetDetectionRange;
        //this.weapon = weapon;
        //this.theBehavior = theBehavior;
        thePosition = new Vector3();
    }

    public GameObject generate()
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thePosition, Quaternion.identity);
        //GameObject torso = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thePosition, Quaternion.identity);

        //oldTorso(newObj,torso);

        //newTorso(newObj, torso);

        genGen.singleton.ensureVirtualGamePad(newObj);


        tagging2.singleton.addTag(newObj, team);
        playable2 thePlayable = newObj.AddComponent<playable2>();
        thePlayable.theNavMeshTransform = newObj.transform;
        thePlayable.theHorizontalRotationTransform = newObj.transform;
        thePlayable.initializeEnactionPoint1();
        thePlayable.theVerticalRotationTransform = thePlayable.enactionPoint1.transform;
        thePlayable.enactionPoint1.transform.SetParent(thePlayable.theHorizontalRotationTransform, true);

        hitscanEnactor.addHitscanEnactor(thePlayable.gameObject, thePlayable.enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.melee));

        tagging2.singleton.addTag(newObj, tagging2.tag2.threat1);  //note link between this and threatening enactions.  but also choice of targets.



        thePlayable.dictOfIvariables[numericalVariable.health] = health;
        //thePlayable.equipperSlotsAndContents[simpleSlot.hands] = null;

        genGen.singleton.addArrowForward(thePlayable.enactionPoint1);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);


        //      genGen.singleton.makeEnactionsWithTorsoArticulation1(thePlayable);
        genGen.singleton.makeBasicEnactions(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);




        //Debug.Log("?????????????????????????????????????????????????????");
        




        FSMcomponent theFSMcomponent = newObj.AddComponent<FSMcomponent>();
        theFSMcomponent.theFSMList = new predatorFSMGen2(newObj, team, speed, stuffType.meat1, targetDetectionRange).returnIt();//theBehavior;




        MeshRenderer theRenderer = newObj.GetComponent<MeshRenderer>();
        theRenderer.material.color = new Color(1f, 0f, 0f);


        //NavMeshAgent navMeshAgent = newObj.GetComponent<NavMeshAgent>();
        //navMeshAgent.speed += 1.7f;



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


    /*
    public GameObject returnBasicPredator1(Vector3 where, stuffType stuffTypeX, float scale = 1f)
    {
        GameObject newObj = returnBasicAnimal1(where, stuffTypeX, scale);

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
    */

}



public class predatorFSMGen2 : FSM
{

    //FSM theFSM;
    public List<FSM> theFSMList = new List<FSM>();

    //float combatRange = 40f;
    private GameObject newObj;
    private tag2 team;
    private float speed;
    private float targetDetectionRange;

    stuffType stuffTypeX;
    objectCriteria defaultThreatCriteriaInAbsoluteTerms;

    public predatorFSMGen2(GameObject theObjectDoingTheEnaction, tag2 team, float speed, stuffType stuffTypeX = stuffType.meat1, float targetDetectionRange = 40f)
    {
        this.newObj = theObjectDoingTheEnaction;
        this.team = team;
        this.speed = speed;
        this.targetDetectionRange = targetDetectionRange;
        this.stuffTypeX = stuffTypeX;
        //Debug.Log("targetDetectionRange:  "+targetDetectionRange);

        defaultThreatCriteriaInAbsoluteTerms = threatCriteriaInAbsolute();

        theFSMList.Add(feetFSM(theObjectDoingTheEnaction, team));
        theFSMList.Add(handsFSM(theObjectDoingTheEnaction, team));

    }



    private FSM handsFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        FSM idle = new generateFSM();
        FSM grabFood = grabFoodHandsFSM();

        //idle.addSwitchAndReverse(switchToGrabFoodConditionHands(), grabFood);



        FSM killPrey = killPreyFSM(); 
        idle.addSwitchAndReverse(switchToKillPreyConditionHands(), killPrey);
        //killPrey.addSwitchAndReverse(switchToGrabFoodConditionHands(), grabFood);




        /*
        objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction, team);
        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 110);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        FSM combat1 = new generateFSM(new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, targetDetectionRange).returnIt());



        idle.addSwitchAndReverse(switchToAttack, combat1);

        */


        idle.name = "hands, idle";
        grabFood.name = "hands, grabFood";
        killPrey.name = "hands, killPrey";
        //combat1.name = "hands, combat1";
        //equipGun.theFSM.name = "hands, equipGun";
        return idle;
    }

    private FSM killPreyFSM()
    {
        //FSM getFood = new generateFSM(grabTheStuff(newObj, stuffTypeX));
        FSM killPrey = new generateFSM(
            new aimAtXAndInteractWithY(newObj, killPreyTargetPicker(), interType.melee, 1.7f).returnIt()
            );//new animalFSM(returnTheGoToThingWithNumericalVariableXAndInteractWithTypeY(newObj, numericalVariable.health, interType.melee));


        return killPrey;
    }





    private condition switchToKillPreyConditionHands()
    {
        //new stickyCondition(switchCondition2, 90)
        //condition switchCondition2 = new canSeeNumericalVariable(theObjectDoingTheEnaction, numericalVariable.health);



        //ok this doesn't work yet because it doesn't ever 'return' "unmet" AKA "FALSE" when there is no target, 
        //beacuse target picker has agnostic calc, ummmm, STUFF..........
        //soooo, need to nest? FIRST see if there IS a target that is returned from picker[or one that meets criteria].
        //.if so, THEN evaluate it on proximity criteria.  and [for the CONDITION] return bool result
        //      condition killPreyCondition = new proximityFromTargetPicker(newObj, killPreyTargetPicker());



        objectCriteria prox = new proximityCriteriaBool(newObj,0);
        condition killPreyCondition = new stickyCondition(new isThereAtLeastOneObjectInSet(new setOfAllObjectThatMeetCriteria(preySet(), prox)), 0);


        return killPreyCondition;
    }

    private targetPicker killPreyTargetPicker()
    {
        targetPicker theTargetPicker = new nearestTargetPicker(newObj, preySet());

        return theTargetPicker;
    }

    private objectSetGrabber preySet()
    {
        return new excludeX(new setOfAllNearbyNumericalVariable(newObj, numericalVariable.health), newObj);
    }

    private condition switchToGrabFoodConditionHands()
    {
        condition grabCondition = new proximityFromTargetPicker(newObj, grabFoodTargetPicker());

        return grabCondition;
    }

    private FSM grabFoodHandsFSM()
    {
        FSM getFood = new generateFSM(grabTheStuff(newObj, stuffTypeX));

        return getFood;
    }

    private repeatWithTargetPicker grabTheStuff(GameObject theObjectDoingTheEnaction, stuffType stuffX)
    {
        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));
        //perma1 = new permaPlan2(step1);

        //repeatWithTargetPicker otherBehavior = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(this.gameObject));


        //return new ummAllThusStuffForGrab(theObjectDoingTheEnaction, stuffX).returnTheRepeatTargetThing();

        return new aimAtXAndInteractWithY(newObj, grabFoodTargetPicker(), interType.standardClick, 1.7f).returnIt();
    }




    /*
    private animalFSM predatorForagingBehavior1(GameObject theObjectDoingTheEnaction, stuffType stuffX)
    {


        animalFSM wander = new animalFSM(randomWanderRepeatable(theObjectDoingTheEnaction));
        animalFSM grabMeat = new animalFSM(returnTheGoToThingOfTypeXAndInteractWithTypeY(theObjectDoingTheEnaction, stuffX, interType.standardClick));//new animalFSM(new repeatWithTargetPicker(new permaPlan2(goGrabPlan2(theObjectDoingTheEnaction,stuffX)), new setOfAllNearbyStuffStuff(stuffX)));
        

        condition switchCondition1 = new canSeeStuffStuff(theObjectDoingTheEnaction, stuffX);

        

        //wander.addSwitchAndReverse(new stickyCondition(switchCondition1, 90), grabMeat);

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

    */






    private FSM feetFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        FSM wander = wanderFSM();
        FSM grabFood = grabFoodFeetFSM();
        FSM killPrey = killPreyFeetFSM();
        //FSM flee = fleeFSM();

        //condition switchToFlee = fleeCondition();

        //wander.addSwitchAndReverse(switchToFlee, flee);
        //grabFood.addSwitchAndReverse(switchToFlee, flee);



        //condition switchToGrabFood = grabFoodCondition();

        wander.addSwitchAndReverse(grabFoodCondition(), grabFood);
        wander.addSwitchAndReverse(killPreyCondition(), killPrey);
        killPrey.addSwitchAndReverse(grabFoodCondition(), grabFood);




        changeNavSpeed(theObjectDoingTheEnaction, speed);

        wander.name = "feet, wander";
        grabFood.name = "feet, grabFood";
        killPrey.name = "feet, killPrey";
        //flee.name = "feet, flee";

        return wander;
    }

    private condition killPreyCondition()
    {
        objectSetGrabber theObjectSet = preySet();//new excludeX((new setOfAllNearbyNumericalVariable(newObj, numericalVariable.health)),newObj);

        condition switchCondition = new stickyCondition(new isThereAtLeastOneObjectInSet(theObjectSet), 7);// theObjectDoingTheEnaction, numericalVariable.health);

        return switchCondition;
    }

    private FSM killPreyFeetFSM()
    {
        FSM killPrey = new generateFSM(
            new goToX(newObj, killPreyTargetPicker(), targetDetectionRange).returnIt());

        return killPrey;
    }

    private void changeNavSpeed(GameObject theObject, float speed)
    {
        navAgent theNav = theObject.GetComponent<navAgent>();
        theNav.theAgent.speed = speed;
        theNav.theAgent.baseOffset = 0.5f;//theObjectDoingTheEnaction.transform.localScale.y;
                                          //theNav.theAgent.

        //Debug.Log("targetDetectionRange:  " + targetDetectionRange);


    }

    private FSM fleeFSM()
    {
        FSM flee = new generateFSM(
            new goToX(newObj, fleeingPathTargetPicker(), targetDetectionRange).returnIt()
            );


        return flee;
    }

    private FSM grabFoodFeetFSM()
    {
        FSM grabFood = new generateFSM(
            new goToX(newObj, grabFoodTargetPicker(), targetDetectionRange).returnIt());

        return grabFood;
    }

    private FSM wanderFSM()
    {
        FSM wander = new generateFSM(
            new randomWanderRepeatable(newObj).returnIt());

        return wander;
    }






    private targetPicker grabFoodTargetPicker()
    {
        targetPicker theTargetPicker = new nearestTargetPicker(newObj, new excludeX(new setOfAllNearbyStuffStuff(newObj, stuffTypeX), newObj));

        return theTargetPicker;
    }

    private condition grabFoodCondition()
    {

        //Debug.Log("stuffTypeX:  " + stuffTypeX);
        objectSetGrabber theFoodObjectSet = new setOfAllNearbyStuffStuff(newObj, stuffTypeX);

        condition switchToGrabFood = new stickyCondition(new isThereAtLeastOneObjectInSet(theFoodObjectSet), 7);// theObjectDoingTheEnaction, numericalVariable.health);

        return switchToGrabFood;
    }





    private objectCriteria createAttackCriteria(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new stickyTrueCriteria(new lineOfSight(theObjectDoingTheEnaction), 200),
            new proximityCriteriaBool(theObjectDoingTheEnaction, targetDetectionRange)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private targetPicker generateAttackTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theAttackObjectSet)
    {

        targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        return theAttackTargetPicker;
    }




    private objectCriteria threatCriteriaInAbsolute()
    {
        //Debug.Log("targetDetectionRange:  " + targetDetectionRange);
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team))
            //new lineOfSight(theObjectDoingTheEnaction),
            ////////////////////new proximityCriteriaBool(theObjectDoingTheEnaction, targetDetectionRange)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private objectCriteria fleeingSituationalCriteria()
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            //new hasVirtualGamepad(),
            //new reverseCriteria( new objectHasTag(team)),
            //new lineOfSight(theObjectDoingTheEnaction),
            new proximityCriteriaBool(newObj, targetDetectionRange)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private objectSetGrabber fleeingSetGrabber()
    {
        objectSetGrabber setGrabber = new setOfAllObjectThatMeetCriteria(
            new excludeX(
                new setOfAllObjectsInZone(newObj), newObj), defaultThreatCriteriaInAbsoluteTerms);

        return setGrabber;
    }

    private targetPicker fleeingPathTargetPicker()
    {
        targetPicker theTargetPicker = new radialFleeingTargetPicker(newObj, fleeingSetGrabber());

        return theTargetPicker;
    }

    private condition fleeCondition()
    {
        objectSetGrabber theThreatObjectSet = allExceptSelfInZoneThatMeetBothAbsoluteAndSituationalCriteria(defaultThreatCriteriaInAbsoluteTerms, fleeingSituationalCriteria());

        condition switchToFlee = new stickyCondition(new isThereAtLeastOneObjectInSet(theThreatObjectSet), 4);// theObjectDoingTheEnaction, numericalVariable.health);

        return switchToFlee;
    }

    private objectSetGrabber allExceptSelfInZoneThatMeetBothAbsoluteAndSituationalCriteria(objectCriteria criteria1, objectCriteria criteria2)
    {

        objectSetGrabber allExceptSelfInZone = new excludeX(new setOfAllObjectsInZone(newObj), newObj);

        objectSetGrabber objectSubsetMeetingCriteria1 = new setOfAllObjectThatMeetCriteria(allExceptSelfInZone, criteria1);

        objectSetGrabber objectSubsetMeetingCriteria2 = new setOfAllObjectThatMeetCriteria(objectSubsetMeetingCriteria1, criteria2);

        return objectSubsetMeetingCriteria2;
    }



    public List<FSM> returnIt()
    {
        return theFSMList;
    }

}
















public class animalGen2 : objectGen
{
    tagging2.tag2 team;
    private Vector3 thePosition;

    float health;
    float height;
    float length;
    float width;

    float speed;
    float targetDetectionRange;

    //(tagging2.tag2 theTeamIn, float health, float speed, float height, float width, float targetDetectionRange, objectGen weapon, List<FSM> theBehavior )

    // 
    //basicBodyProperties
    public animalGen2(tag2 theTeamIn, float health, float height, float length, float width, float speed, float targetDetectionRange)//, objectGen weapon)
    {
        this.team = theTeamIn;
        this.health = health;
        this.height = height;
        this.length = length;
        this.width = width;
        this.speed = speed;
        this.targetDetectionRange = targetDetectionRange;
        //this.weapon = weapon;
        //this.theBehavior = theBehavior;
        thePosition = new Vector3();
    }

    public GameObject generate()
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thePosition, Quaternion.identity);
        //GameObject torso = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thePosition, Quaternion.identity);

        //oldTorso(newObj,torso);

        //newTorso(newObj, torso);

        genGen.singleton.ensureVirtualGamePad(newObj);


        tagging2.singleton.addTag(newObj, team);
        playable2 thePlayable = newObj.AddComponent<playable2>();
        thePlayable.theNavMeshTransform = newObj.transform;
        thePlayable.theHorizontalRotationTransform = newObj.transform;
        thePlayable.initializeEnactionPoint1();
        thePlayable.theVerticalRotationTransform = thePlayable.enactionPoint1.transform;
        thePlayable.enactionPoint1.transform.SetParent(thePlayable.theHorizontalRotationTransform, true);

        thePlayable.dictOfIvariables[numericalVariable.health] = health;
        //thePlayable.equipperSlotsAndContents[simpleSlot.hands] = null;

        genGen.singleton.addArrowForward(thePlayable.enactionPoint1);
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);


        //      genGen.singleton.makeEnactionsWithTorsoArticulation1(thePlayable);
        genGen.singleton.makeBasicEnactions(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);





        FSMcomponent theFSMcomponent = newObj.AddComponent<FSMcomponent>();
        theFSMcomponent.theFSMList = new animalFSMGen2(newObj, team, speed, stuffType.fruit, targetDetectionRange).returnIt();//theBehavior;

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

public class animalFSMGen2 : FSM
{

    //FSM theFSM;
    public List<FSM> theFSMList = new List<FSM>();

    //float combatRange = 40f;
    private GameObject newObj;
    private tag2 team;
    private float speed;
    private float targetDetectionRange;

    stuffType stuffTypeX;
    objectCriteria defaultThreatCriteriaInAbsoluteTerms;

    public animalFSMGen2(GameObject theObjectDoingTheEnaction, tag2 team, float speed, stuffType stuffTypeX = stuffType.fruit,float targetDetectionRange = 40f)
    {
        this.newObj = theObjectDoingTheEnaction;
        this.team = team;
        this.speed = speed;
        this.targetDetectionRange = targetDetectionRange;
        this.stuffTypeX = stuffTypeX;
        //Debug.Log("targetDetectionRange:  "+targetDetectionRange);

        defaultThreatCriteriaInAbsoluteTerms = threatCriteriaInAbsolute();

        theFSMList.Add(feetFSM(theObjectDoingTheEnaction, team));
        theFSMList.Add(handsFSM(theObjectDoingTheEnaction, team));

    }

    

    private FSM handsFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        FSM idle = new generateFSM();
        FSM grabFood = grabFoodHandsFSM();

        idle.addSwitchAndReverse(switchToGrabFoodConditionHands(), grabFood);

        /*
        objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction, team);
        objectSetGrabber theAttackObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 110);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        FSM combat1 = new generateFSM(new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, targetDetectionRange).returnIt());



        idle.addSwitchAndReverse(switchToAttack, combat1);

        */


        idle.name = "hands, idle";
        //combat1.name = "hands, combat1";
        //equipGun.theFSM.name = "hands, equipGun";
        return idle; ;
    }

    private condition switchToGrabFoodConditionHands()
    {
        condition grabCondition = new proximityFromTargetPicker(newObj, grabFoodTargetPicker());

        return grabCondition;
    }

    private FSM grabFoodHandsFSM()
    {
        FSM getFood = new generateFSM(grabTheStuff(newObj, stuffTypeX));

        return getFood;
    }

    private repeatWithTargetPicker grabTheStuff(GameObject theObjectDoingTheEnaction, stuffType stuffX)
    {
        //singleEXE step1 = makeNavAgentPlanEXE(patternScript2.singleton.randomNearbyVector(this.transform.position));
        //perma1 = new permaPlan2(step1);

        //repeatWithTargetPicker otherBehavior = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(this.gameObject));


        //return new ummAllThusStuffForGrab(theObjectDoingTheEnaction, stuffX).returnTheRepeatTargetThing();

        return new aimAtXAndInteractWithY(newObj, grabFoodTargetPicker(), interType.standardClick, 1.7f).returnIt();
    }













    private FSM feetFSM(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        FSM wander = wanderFSM();
        FSM grabFood = grabFoodFeetFSM();
        FSM flee = fleeFSM();

        condition switchToFlee = fleeCondition();

        wander.addSwitchAndReverse(switchToFlee, flee);
        grabFood.addSwitchAndReverse(switchToFlee, flee);



        condition switchToGrabFood = grabFoodCondition(); 

        wander.addSwitchAndReverse(switchToGrabFood, grabFood);




        changeNavSpeed(theObjectDoingTheEnaction, speed);

        wander.name = "feet, wander";
        grabFood.name = "feet, grabFood";
        flee.name = "feet, flee";

        return wander;
    }

    private void changeNavSpeed(GameObject theObject, float speed)
    {
        navAgent theNav = theObject.GetComponent<navAgent>();
        theNav.theAgent.speed = speed;
        theNav.theAgent.baseOffset = 0.5f;//theObjectDoingTheEnaction.transform.localScale.y;
                                          //theNav.theAgent.

        //Debug.Log("targetDetectionRange:  " + targetDetectionRange);


    }

    private FSM fleeFSM()
    {
        FSM flee = new generateFSM(
            new goToX(newObj, fleeingPathTargetPicker(), targetDetectionRange).returnIt()
            );


        return flee;
    }

    private FSM grabFoodFeetFSM()
    {
        FSM grabFood = new generateFSM(
            new goToX(newObj, grabFoodTargetPicker(),targetDetectionRange).returnIt());

        return grabFood;
    }

    private FSM wanderFSM()
    {
        FSM wander = new generateFSM(
            new randomWanderRepeatable(newObj).returnIt());
        
        return wander;
    }






    private targetPicker grabFoodTargetPicker()
    {
        targetPicker theTargetPicker = new nearestTargetPicker(newObj, new setOfAllNearbyStuffStuff(newObj, stuffTypeX));

        return theTargetPicker;
    }

    private condition grabFoodCondition()
    {

        //Debug.Log("stuffTypeX:  " + stuffTypeX);
        objectSetGrabber theFoodObjectSet = new setOfAllNearbyStuffStuff(newObj, stuffTypeX);

        condition switchToGrabFood = new stickyCondition(new isThereAtLeastOneObjectInSet(theFoodObjectSet), 7);// theObjectDoingTheEnaction, numericalVariable.health);

        return switchToGrabFood;
    }





    private objectCriteria createAttackCriteria(GameObject theObjectDoingTheEnaction, tag2 team)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new stickyTrueCriteria(new lineOfSight(theObjectDoingTheEnaction), 200),
            new proximityCriteriaBool(theObjectDoingTheEnaction, targetDetectionRange)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private targetPicker generateAttackTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theAttackObjectSet)
    {

        targetPicker theAttackTargetPicker = new nearestTargetPicker(theObjectDoingTheEnaction, theAttackObjectSet);

        return theAttackTargetPicker;
    }




    private objectCriteria threatCriteriaInAbsolute()
    {
        //Debug.Log("targetDetectionRange:  " + targetDetectionRange);
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team))
            //new lineOfSight(theObjectDoingTheEnaction),
            ////////////////////new proximityCriteriaBool(theObjectDoingTheEnaction, targetDetectionRange)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }
    
    private objectCriteria fleeingSituationalCriteria()
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            //new hasVirtualGamepad(),
            //new reverseCriteria( new objectHasTag(team)),
            //new lineOfSight(theObjectDoingTheEnaction),
            new proximityCriteriaBool(newObj, targetDetectionRange)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private objectSetGrabber fleeingSetGrabber()
    {
        objectSetGrabber setGrabber = new setOfAllObjectThatMeetCriteria(
            new excludeX(
                new setOfAllObjectsInZone(newObj), newObj), defaultThreatCriteriaInAbsoluteTerms);

        return setGrabber;
    }
    
    private targetPicker fleeingPathTargetPicker()
    {
        targetPicker theTargetPicker = new radialFleeingTargetPicker(newObj, fleeingSetGrabber());

        return theTargetPicker;
    }

    private condition fleeCondition()
    {
        objectSetGrabber theThreatObjectSet = allExceptSelfInZoneThatMeetBothAbsoluteAndSituationalCriteria(defaultThreatCriteriaInAbsoluteTerms, fleeingSituationalCriteria());

        condition switchToFlee = new stickyCondition(new isThereAtLeastOneObjectInSet(theThreatObjectSet), 4);// theObjectDoingTheEnaction, numericalVariable.health);

        return switchToFlee;
    }

    private objectSetGrabber allExceptSelfInZoneThatMeetBothAbsoluteAndSituationalCriteria(objectCriteria criteria1, objectCriteria criteria2)
    {

        objectSetGrabber allExceptSelfInZone = new excludeX(new setOfAllObjectsInZone(newObj), newObj);

        objectSetGrabber objectSubsetMeetingCriteria1 = new setOfAllObjectThatMeetCriteria(allExceptSelfInZone, criteria1);

        objectSetGrabber objectSubsetMeetingCriteria2 = new setOfAllObjectThatMeetCriteria(objectSubsetMeetingCriteria1, criteria2);

        return objectSubsetMeetingCriteria2;
    }



    public List<FSM> returnIt()
    {
        return theFSMList;
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

public class genObjectAndModify : objectGen
{
    objectGen theObjectGen;

    List<objectModifier> theModifiers = new List<objectModifier>();


    public genObjectAndModify(objectGen theObjectGenIn, objectModifier[] modifiers)
    {
        theObjectGen = theObjectGenIn;

        foreach (objectModifier mod in modifiers)
        {
            theModifiers.Add(mod);
        }
    }
    public genObjectAndModify(objectGen theObjectGenIn)
    {
        theObjectGen = theObjectGenIn;
    }


    public GameObject generate()
    {
        GameObject theObject = theObjectGen.generate();

        foreach (objectModifier modifier in theModifiers)
        {
            modifier.modify(theObject);
        }

        return theObject;
    }
}


/*
public class paintByNumbersNPCGenerator1 : objectGen
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

    //tag2 theTeamIn, float health, float height, float width, float speed, float targetDetectionRange,attackingrangshouldbebuiltintoenactionbutfornow...


    public paintByNumbersNPCGenerator1(visibleAppearance theVisibleAppearanceIn, bodyProperties theBodyPropertiesIn, behaviorSet theBehaviorSetIn)//, objectGen weapon)
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
        theFSMcomponent.theFSMList = new basicPaintByNumbersSoldierFSM(newObj, team, speed, targetDetectionRange).returnIt();//theBehavior;

        return newObj;
    }

}

public class paintByNumbersNPCFSMGenerator1 : FSM
{

    //FSM theFSM;
    public List<FSM> theFSMList = new List<FSM>();

    //float combatRange = 40f;
    private GameObject newObj;
    private tag2 team;
    private float speed;
    private float targetDetectionRange;

    public paintByNumbersNPCFSMGenerator1(GameObject theObjectDoingTheEnaction, tag2 team, float speed, float targetDetectionRange = 40f)
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


*/











public class gunFlash : objectGen
{
    public GameObject generate()
    {
        GameObject newProjectile = expandingSphere(new Vector3(), 0.2f, 4);
        GameObject.Destroy(newProjectile.GetComponent<Collider>());

        //MeshRenderer theRenderer = newProjectile.GetComponent<MeshRenderer>();
        //theRenderer.material.color = new Color(1f, 1f, 0f);
        newProjectile.GetComponent<Renderer>().material = repository2.singleton.explosion1;

        return newProjectile;
    }


    public GameObject expandingSphere(Vector3 theLocation, float growthSpeed, int timeUntilSelfDestruct)
    {
        GameObject prefabToUse = repository2.singleton.interactionSphere;
        GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, theLocation);


        projectile1 projectileScript = newProjectile.AddComponent<projectile1>();
        projectileScript.speed = 0.17f;
        projectileScript.selfDestructOnCollision = false;
        selfDestructScript1 killScript = newProjectile.GetComponent<selfDestructScript1>();
        killScript.timeUntilSelfDestruct = timeUntilSelfDestruct;
        newProjectile.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        growScript1 growScript = newProjectile.AddComponent<growScript1>();
        growScript.growthSpeed = growthSpeed;

        return newProjectile;
    }
}



public class bulletGlow : objectGen
{
    public GameObject generate()
    {

        GameObject newProjectile = sphere(new Vector3(), 99);
        GameObject.Destroy(newProjectile.GetComponent<Collider>());

        //MeshRenderer theRenderer = newProjectile.GetComponent<MeshRenderer>();
        //theRenderer.material.color = new Color(1f, 1f, 0f);
        newProjectile.GetComponent<Renderer>().material = repository2.singleton.bulletGlow1;

        return newProjectile;
    }


    public GameObject sphere(Vector3 theLocation, int timeUntilSelfDestruct)
    {
        GameObject prefabToUse = repository2.singleton.interactionSphere;
        GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, theLocation);


        projectile1 projectileScript = newProjectile.AddComponent<projectile1>();
        projectileScript.speed = 0.17f;
        projectileScript.selfDestructOnCollision = false;
        selfDestructScript1 killScript = newProjectile.GetComponent<selfDestructScript1>();
        killScript.timeUntilSelfDestruct = timeUntilSelfDestruct;
        newProjectile.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);


        return newProjectile;
    }


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

        animalFSM repro = repro1(theObjectDoingTheEnaction, stuffX);



        //switchCondition, grabTheStuff(theObjectDoingTheEnaction,stuffX)

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



        objectCriteria reproCriteria = new objectMeetsAllCriteria(
            new proximityCriteriaBool(theObjectDoingTheEnaction, 40)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        //objectSetGrabber theFleeFromObjectSet = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        objectSetGrabber theReproSet = new setOfAllObjectThatMeetCriteria(
            new excludeX(new setOfAllNearbyNumericalVariable(theObjectDoingTheEnaction, numericalVariable.health), theObjectDoingTheEnaction),
            reproCriteria);

        condition switchCondition3 = new reverseCondition(
            new isThereAtLeastOneObjectInSet(theReproSet));


        //wander.addSwitchAndReverse(new stickyCondition(switchCondition1, 90), grabMeat);
        theFSM.addSwitchAndReverse(new stickyCondition(switchToFlee, 10), flee);
        theFSM.addSwitchAndReverse(new stickyCondition(switchCondition1, 10), getFood);
        theFSM.addSwitchAndReverse(switchCondition3, repro);
        getFood.addSwitch(new stickyCondition(switchToFlee, 10), flee);
        getFood.addSwitch(switchCondition3, repro);







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



        return repro;
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






}




public class randomWanderRepeatable
{
    repeatWithTargetPicker madeIt;

    public randomWanderRepeatable(GameObject theObjectDoingTheEnaction)
    {
        madeIt = makeAndReturnRandomWanderRepeatable(theObjectDoingTheEnaction);
    }

    public repeatWithTargetPicker returnIt()
    {
        return madeIt;
    }

    private repeatWithTargetPicker makeAndReturnRandomWanderRepeatable(GameObject theObjectDoingTheEnaction)
    {
        singleEXE step1 = genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnaction, patternScript2.singleton.randomNearbyVector(theObjectDoingTheEnaction.transform.position));
        permaPlan2 perma1 = new permaPlan2(step1);
        //plan = new depletablePlan(step1, step2);
        //plan = perma1.convertToDepletable();
        //simpleRepeat1 = new simpleExactRepeatOfPerma(perma1);
        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, new randomNearbyLocationTargetPicker(theObjectDoingTheEnaction));

        return repeatWithTargetPickerTest;
    }

}


public class paintByNumbersAnimalBody1 : objectGen
{
    GameObject generatedObject;
    GameObject parent;

    public GameObject generate()
    {
        generatedObject.transform.position = parent.transform.position;
        return generatedObject;
    }


    public paintByNumbersAnimalBody1(GameObject parentIn, stuffStuff foodTypeIn, float speedIn, int healthIn, int minDamageLevelIn, int maxDamageLevelIn, float sightRangeIn)
    {
        //throw new NotImplementedException();
    }



    public GameObject returnBasicAnimal1(Vector3 where, stuffType stuffTypeX, float scale = 1f)
    {
        GameObject newObj = returnBodyModel(where, scale);
        genGen.singleton.addArrowForward(newObj, 1f, 0f, 1.2f);

        addAnimalBody1PlayableToObject(newObj);
        //              stuffStuff.addStuffStuff(newObj, stuffType.meat1);

        //                          animalUpdate theUpdate = newObj.AddComponent<animalUpdate>();
        //                          theUpdate.theFSM = herbavoreForagingBehavior1(newObj, stuffTypeX);


        newObj.GetComponent<interactable2>().dictOfIvariables[numericalVariable.cooldown] = 0f;  //do they need this?  bad sign that i can't tell just by looking here

        return newObj;
    }



    public GameObject returnBodyModel(Vector3 where, float scale = 1f)
    {

        GameObject newObj = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.placeHolderCubePrefab, where);// Instantiate(repository2.singleton.placeHolderCubePrefab, where, Quaternion.identity);
        //      newObj.transform.localScale = new Vector3(128, 1, 8);
        newObj.transform.localScale = scale * newObj.transform.localScale;

        return newObj;
    }


    public void addAnimalBody1PlayableToObject(GameObject newObj)
    {
        genGen.singleton.ensureVirtualGamePad(newObj);

        playable2 thePlayable = newObj.AddComponent<playable2>();
        thePlayable.initializeEnactionPoint1();
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        genGen.singleton.makeBasicEnactions(thePlayable);
        makeInteractions(thePlayable);


        thePlayable.dictOfIvariables[numericalVariable.health] = 2;



        inventory1 theirInventory = newObj.AddComponent<inventory1>();
    }


    public void makeInteractions(interactable2 theInteractable)
    {
        genGen.createWeaponLevels(theInteractable, interType.peircing, 0, 4);
        genGen.createWeaponLevels(theInteractable, interType.melee, 0, 4);
        //createWeaponLevels(theInteractable, interType.shootFlamethrower1, 0, 5);
        //createWeaponLevels(theInteractable, interType.tankShot, 0, 0);

        genGen.singleton.deathWhenHealthIsZero(theInteractable);
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


public class modifyScale : objectModifier
{
    float xScale;
    float yScale;
    float zScale;

    public modifyScale(float xScaleIn, float yScaleIn, float zScaleIn)
    {
        xScale = xScaleIn;
        yScale = yScaleIn;
        zScale = zScaleIn;
    }

    public void modify(GameObject theObject)
    {
        theObject.transform.localScale = new Vector3(xScale, yScale, zScale);
    }
}


public class collisionInteractionMod : objectModifier
{
    private interType theIntertype;
    float magnitudeOfInteraction;
    int level;

    public collisionInteractionMod(interType theIntertypeIn, float magnitudeOfInteractionIn, int levelIn)
    {
        this.theIntertype = theIntertypeIn;
        magnitudeOfInteraction = magnitudeOfInteractionIn;
        this.level = levelIn;
    }

    public void modify(GameObject theObject)
    {
        colliderInteractor2.addColliderInteractor2(theObject, theIntertype, magnitudeOfInteraction,level);

    }
}

public class simpleMovingMod : objectModifier
{
    float speed;
    bool sdOnCollision = true;
    int timeUntilSelfDestruct;

    //hmmm, what about direction?  just go FORWARD, then match rotation to that of the gun when firing it?
    public simpleMovingMod(float speedIn, bool sdOnCollisionIn = true, int timeUntilSelfDestructIn = 99)
    {
        //Debug.Log("speedIn:  " + speedIn);
        speed = speedIn;
        sdOnCollision = sdOnCollisionIn;
        timeUntilSelfDestruct = timeUntilSelfDestructIn;
    }

    public void modify(GameObject theObject)
    {
        //Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>speed:  " + speed);
        projectile2.addProjectile2(theObject,speed,timeUntilSelfDestruct,sdOnCollision);
    }
}




public class projectile2 : MonoBehaviour
{
    float speed = 0f;
    bool selfDestructOnCollision = true;
    

    public static void addProjectile2(GameObject theObject, float speedIn, int timeUntilSelfDestructIn, bool sdOnCollisionIn = true)//IEnactaBool enactingThis)//(GameObject theObject, int timeUntilSelfDestruct, Vector3 direction = new Vector3(), bool sdOnCollision = true, bool explodeOnDestroy = false)//, IEnactaBool enactingThis)
    {
        //use like THIS:
        //projectile2.addProjectile1(x, y, z);

        projectile2 projectileScript = theObject.AddComponent<projectile2>();

        //Debug.Log("speedIn:  " + speedIn);
        projectileScript.speed = speedIn;

        projectileScript.selfDestructOnCollision = sdOnCollisionIn;
        selfDestructScript1 killScript = theObject.GetComponent<selfDestructScript1>();
        killScript.timeUntilSelfDestruct = timeUntilSelfDestructIn;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("speed:  " + speed);
        this.gameObject.transform.position = this.gameObject.transform.position + this.gameObject.transform.forward * speed;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("projectile collision self destruct????????" + Time.fixedTime + this.GetInstanceID());
        //technically, this is an interaction.  but whatever?  just put it here.
        //Debug.Log("other.gameObject.tag:  "+ other.gameObject.tag);


        if (selfDestructOnCollision == false) { return; }
        if (other.CompareTag("aMapZone")) { return; }//other.CompareTag(String tagName)  //https://stackoverflow.com/questions/46248022/not-destroying-objects-fast-enough-and-causing-another-collision



        Destroy(this.gameObject);

        /*
        if (selfDestructOnCollision && other.gameObject.tag != "aMapZone")
        {
            //selfDestructOnCollision = false;
            //enabled = false;
            //Debug.Log("YES projectile collision self destruct!!!" + Time.fixedTime + this.GetInstanceID());
            Destroy(this.gameObject);
        }
        else
        {

            //Debug.Log("no  " + Time.fixedTime + this.GetInstanceID());
        }
        */
    }

}








public class objectSetInstantiator
{
    List<objectGen> theSet = new List<objectGen>();




    public objectSetInstantiator(objectGen[] objectGenArray)
    {
        foreach(objectGen thisObjectGen in objectGenArray) { theSet.Add(thisObjectGen); }
    }



    internal void generate(Vector3 spawnPoint)
    {
        foreach(objectGen objGen in theSet)
        {
           GameObject newObj =  objGen.generate();
            //Debug.Log("spawnPoint:  "+spawnPoint);
            //newObj.transform.position = spawnPoint;
            newObj.transform.position = patternScript2.singleton.randomNearbyVector(spawnPoint);
        }
    }
}