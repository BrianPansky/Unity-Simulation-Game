using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static enactionCreator;
using static interactionCreator;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class genGen : MonoBehaviour
{
    //[general generator]  hmmm, thigs here should either be in repository or combo gen maybe?
    public static genGen singleton;

    void Awake()
    {
        //Debug.Log("Awake:  " + this);
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


    // Start is called before the first frame update
    void Start()
    {
        
    }



    public GameObject createPrefabAtPointAndRETURN(GameObject thePrefab, Vector3 thePoint)
    {
        //GameObject newBuilding = new GameObject();
        //newBuilding = Instantiate(thePrefab, thePoint, Quaternion.identity);
        //return Instantiate(thePrefab, thePoint, Quaternion.identity);

        //just so i can keep the rotation of the object i input, for now:
        return Instantiate(thePrefab, thePoint, thePrefab.transform.rotation);
    }



    public GameObject returnNPC5(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.placeHolderCylinderPrefab, where, Quaternion.identity);

        Destroy(newObj.GetComponent<Collider>());

        addBody4ToObject(newObj);

        newObj.AddComponent<AIHub3>();


        newObj.AddComponent<CapsuleCollider>();
        return newObj;
    }


    public void addBody4ToObject(GameObject newObj)
    {

        playable2 thePlayable = newObj.AddComponent<playable2>();
        //newObj.AddComponent<navmeshAgentDebugging>();




        thePlayable.dictOfInteractions = new Dictionary<enactionCreator.interType, List<Ieffect>>();
        thePlayable.dictOfIvariables = new Dictionary<interactionCreator.numericalVariable, float>();

        thePlayable.dictOfIvariables[numericalVariable.health] = 2;
        thePlayable.equipperSlotsAndContents[interactionCreator.simpleSlot.hands] = null;
        thePlayable.initializeEnactionPoint1();
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);






        makeEnactionsBody4(thePlayable);
        makeInteractionsBody4(thePlayable);



        inventory1 theirInventory = newObj.AddComponent<inventory1>();
    }


    void makeEnactionsBody4(playable2 thePlayable)
    {
        hitscanEnactor.addHitscanEnactor(thePlayable.gameObject, thePlayable.enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.standardClick));


        vecTranslation.addVecTranslation(thePlayable.gameObject, thePlayable.speed, buttonCategories.vector1);

        navAgent.addNavAgentEnaction(thePlayable.gameObject);

        aimTarget.addAaimTargetAndAimTranslation(thePlayable.gameObject, thePlayable.lookSpeed, thePlayable.transform, thePlayable.enactionPoint1.transform, buttonCategories.vector2);


        /*
        thePlayable.enactableBoolSet.Add(new hitscanEnactor(thePlayable.enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.standardClick)));


        thePlayable.enactableVectorSet.Add(new vecTranslation(thePlayable.speed, thePlayable.transform, buttonCategories.vector1));

        thePlayable.enactableTARGETVectorSet.Add(new navAgent(thePlayable.gameObject));
        new aimTarget(
            new vecRotation(thePlayable.lookSpeed, thePlayable.transform, thePlayable.enactionPoint1.transform, buttonCategories.vector2)
            ).addToBothLists(thePlayable.enactableVectorSet, thePlayable.enactableTARGETVectorSet);

        */


    }

    private void makeInteractionsBody4(IInteractable theInteractable)
    {
        theInteractable.dictOfInteractions = interactionCreator.singleton.addInteraction(theInteractable.dictOfInteractions, interType.shoot1, new numericalEffect(numericalVariable.health));

        /*
        theInteractable.dictOfInteractions = interactionCreator.singleton.addInteraction(theInteractable.dictOfInteractions, interType.shoot1, new numericalEffect(numericalVariable.health));
        theInteractable.dictOfInteractions = interactionCreator.singleton.addInteraction(theInteractable.dictOfInteractions, interType.shootFlamethrower1, new damage());
        theInteractable.dictOfInteractions = interactionCreator.singleton.addInteraction(theInteractable.dictOfInteractions, enactionCreator.interType.tankShot, new damage());
        theInteractable.dictOfInteractions = interactionCreator.singleton.addInteraction(theInteractable.dictOfInteractions, enactionCreator.interType.tankShot, new damage());  //uhhh, double damage
        */

    }



    /*
    
    public GameObject returnSimpleTank2(Vector3 where)
    {

        GameObject bottomBit = Instantiate(repository2.singleton.simpleTankBottom, where, Quaternion.identity);

        tank2 tank2 = bottomBit.AddComponent<tank2>();
        bottomBit.AddComponent<NavMeshAgent>();
        bottomBit.AddComponent<CharacterController>();
        //bottomBit.AddComponent<AIHub2>();
        
        //interactionScript theInteractionScript = bottomBit.AddComponent<interactionScript>();
        //dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, "standardClick", "useVehicle");

        //genGen.singleton.rigid(bottomBit);


        //"Setting the parent of a transform which resides in a Prefab Asset is disabled to prevent data corruption (GameObject: 'simple tank turret without barrel')."
        //hmmm...
        //Debug.Log("genGen:  " + genGen);

        tank2.tankHead = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.simpleTankTurretWITHOUTBarrel, where);
        tank2.tankHead.transform.SetParent(bottomBit.transform, true);
        //tank2.tankBarrel = repository2.singleton.simpleTankBarrel;
        tank2.tankBarrel = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.simpleTankBarrel, where);
        tank2.tankBarrel.transform.SetParent(tank2.tankHead.transform, true);


        tank2.tankHead.transform.localPosition += new Vector3(0, 0.2f, 0);
        tank2.tankBarrel.transform.localPosition += new Vector3(0, 2.1f, 1.1f);

        return bottomBit;
    }
    */

    public GameObject returnPineTree1(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.pineTree1, where, Quaternion.identity);

        //                          simpleInteractable.genSimpleInteractable(newObj, enactionCreator.interType.shootFlamethrower1, new burn());
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        //newObj.AddComponent<simpleInteractable>();
        //simpleInteractable theInteractionScript = newObj.GetComponent<interactionScript>();

        //dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.shootFlamethrower1, new burn());




        //newObj.AddComponent<interactionScript>();
        //interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        //dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.shootFlamethrower1, new burn());\
        //      dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.standardClick, new playAsPlayable2());

        return newObj;
    }

    /*

    public GameObject returnNPC4(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.placeHolderCylinderPrefab, where, Quaternion.identity);

        Destroy(newObj.GetComponent<Collider>());

        newObj.AddComponent<body2>();
        newObj.AddComponent<AIHub3>();

        newObj.AddComponent<CapsuleCollider>();

        inventory1 theirInventory = newObj.GetComponent<inventory1>();

        //theirInventory.storedequippable2s.Add(returnGun1(where));
        //                  theirInventory.putInInventory(returnGun1(where));//.storedequippable2s.Add(returnGun1(where));

        //newObj.AddComponent<navmeshAgentDebugging>();

        //newObj.AddComponent<interactionScript>();
        //interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        //dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.shootFlamethrower1, interactionScript.effect.burn);

        return newObj;
    }
    */


    public GameObject returnGun1(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.simpleGun1, where, Quaternion.identity);


        //newObj.AddComponent<interactionScript>();
        //interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        //dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.shootFlamethrower1, interactionScript.effect.burn);

        return newObj;
    }
    
    public GameObject returnShotgun1(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.shotgun1, where, Quaternion.identity);


        //newObj.AddComponent<interactionScript>();
        //interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        //dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.shootFlamethrower1, interactionScript.effect.burn);

        return newObj;
    }



    public GameObject makeEmptyIntSphere(Vector3 position)
    {

        GameObject prefabToUse = repository2.singleton.interactionSphere;
        GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, position);

        return newProjectile;
    }


    public void projectileGenerator(projectileToGenerate theprojectileToGenerate, collisionEnaction theEnactable, Vector3 startPoint, Vector3 direction)//(rangedEnaction enInfo, interactionInfo interINFO, IEnactaBool theEnactable)
    {

        //Debug.Log("projectileGenerator");
        //Transform firePoint, enactionCreator.interType interactionType, bool sdOnCollision = true, int timeUntilSelfDestruct = 99, float growthSpeed = 0f, float magnitudeOfInteraction = 1f)

        //Vector3 startPoint = enInfo.firePoint.position + enInfo.firePoint.forward;

        GameObject newObjectForProjectile = makeEmptyIntSphere(startPoint);
        projectile1.genProjectile1(newObjectForProjectile, theprojectileToGenerate, direction);//enInfo, interINFO, theEnactable);
        growScript1.genGrowScript1(newObjectForProjectile, theprojectileToGenerate.growthSpeed);

        //should this use "interactionMate" isntead?
        //authorScript1.GENAuthorScript1(newObjectForProjectile, theEnactable);
        //interactionSpheres already have an author script!  use THIS function instead:
        //authorScript1.FILLAuthorScript1(newObjectForProjectile, theEnactable.interInfo, theEnactable);
        colliderInteractor.genColliderInteractor(newObjectForProjectile, theEnactable);
        selfDestructScript1 sds = newObjectForProjectile.GetComponent<selfDestructScript1>();
        sds.timeUntilSelfDestruct = theprojectileToGenerate.timeUntilSelfDestruct;

        /*

        //Debug.Log("projectile made supposedly");
        //mastLine(startPoint, Color.red);
        //mastLine(newProjectile.transform.position, Color.blue);


        //Debug.DrawLine(newProjectile.transform.position, new Vector3(), Color.red);

        //threatAlert(this);

        */
    }


    




    /*

    public interactionScript ensureInteractionScript(GameObject onThisObject)
    {
        interactionScript ensuredThing = onThisObject.GetComponent<interactionScript>();
        if (ensuredThing == null)
        {
            ensuredThing = onThisObject.AddComponent<interactionScript>();

        }

        ensuredThing.dictOfInteractions = new Dictionary<interType, List<Ieffect>>();//new Dictionary<string, List<string>>(); //for some reason it was saying it already had that key in it, but it should be blank.  so MAKING it blank.

        return ensuredThing;
    }

    */


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
