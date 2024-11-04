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


    public void addBody4ToObject(GameObject newObj)
    {
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
        makeEnactionsBody4(thePlayable);
        makeInteractionsBody4(thePlayable);


        inventory1 theirInventory = newObj.AddComponent<inventory1>();
    }
    private void addCube(GameObject inputObject, float scale = 1f, float xOffset = 0f, float yOffset = 0f, float zOffset = 0f)
    {
        Vector3 where = inputObject.transform.position + new Vector3(xOffset, yOffset, zOffset);
        GameObject newObj = Instantiate(repository2.singleton.placeHolderCubePrefab, where, Quaternion.identity);
        newObj.transform.localScale = scale * newObj.transform.localScale;

        newObj.transform.parent = inputObject.transform;

    }

    private void addArrowForward(GameObject inputObject, float scale = 1f, float xOffset = 0f, float yOffset = 0f, float zOffset = 0f)
    {
        Vector3 where = inputObject.transform.position + new Vector3(xOffset, yOffset, zOffset);
        GameObject newObj = Instantiate(repository2.singleton.arrowForward, where, Quaternion.identity);
        newObj.transform.localScale = scale * newObj.transform.localScale;

        newObj.transform.parent = inputObject.transform;

    }

    private void addArrowUp(GameObject inputObject, float scale = 1f, float xOffset = 0f, float yOffset = 0f, float zOffset = 0f)
    {
        Vector3 where = inputObject.transform.position + new Vector3(xOffset, yOffset, zOffset);
        GameObject newObj = Instantiate(repository2.singleton.arrowUp, where, Quaternion.identity);
        newObj.transform.localScale = scale * newObj.transform.localScale;

        newObj.transform.parent = inputObject.transform;

    }

    void makeEnactionsBody4(playable2 thePlayable)
    {
        hitscanEnactor.addHitscanEnactor(thePlayable.gameObject, thePlayable.enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.standardClick));


        vecTranslation.addVecTranslation(thePlayable.gameObject, thePlayable.speed, buttonCategories.vector1);

        navAgent.addNavAgentEnaction(thePlayable.gameObject);

        aimTarget.addAimTargetAndVecRotation(thePlayable.gameObject, thePlayable.lookSpeed, thePlayable.transform, thePlayable.enactionPoint1.transform, buttonCategories.vector2);
    }

    private void makeInteractionsBody4(interactable2 theInteractable)
    {
        theInteractable.dictOfInteractions = interactionCreator.singleton.addInteraction(theInteractable.dictOfInteractions, interType.shoot1, new numericalEffect(numericalVariable.health));




        //i'll put conditional effects in this function for now
        condition theCondition = new numericalCondition(numericalVariable.health, theInteractable.dictOfIvariables);
        Ieffect theEffect = new deathEffect(theInteractable.transform.gameObject);
        //List<Ieffect> list = new List<Ieffect> { theEffect };
        //theInteractable.conditionalEffects[theCondition] = list;
        addConditionalEffect(theInteractable.transform.gameObject, theCondition, theEffect);

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
