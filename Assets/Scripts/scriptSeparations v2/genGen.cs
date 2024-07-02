using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static enactionCreator;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class genGen : MonoBehaviour
{
    //[general generator]
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





    public GameObject returnSimpleTank2(Vector3 where)
    {

        GameObject bottomBit = Instantiate(repository2.singleton.simpleTankBottom, where, Quaternion.identity);

        tank2 tank2 = bottomBit.AddComponent<tank2>();
        bottomBit.AddComponent<NavMeshAgent>();
        bottomBit.AddComponent<CharacterController>();
        //bottomBit.AddComponent<AIHub2>();
        
        interactionScript theInteractionScript = bottomBit.AddComponent<interactionScript>();
        //theInteractionScript.addInteraction("standardClick", "useVehicle");

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

    public GameObject returnPineTree1(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.pineTree1, where, Quaternion.identity);


        newObj.AddComponent<interactionScript>();
        interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        theInteractionScript.addInteraction(enactionCreator.interType.shootFlamethrower1, interactionScript.effect.burn);

        return newObj;
    }

    public GameObject returnNPC4(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.placeHolderCylinderPrefab, where, Quaternion.identity);



        newObj.AddComponent<AIHub3>();
        newObj.AddComponent<body2>();


        inventory1 theirInventory = newObj.GetComponent<inventory1>();

        //theirInventory.storedEquippables.Add(returnGun1(where));
        //                  theirInventory.putInInventory(returnGun1(where));//.storedEquippables.Add(returnGun1(where));

        //newObj.AddComponent<navmeshAgentDebugging>();

        //newObj.AddComponent<interactionScript>();
        //interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        //theInteractionScript.addInteraction(enactionCreator.interType.shootFlamethrower1, interactionScript.effect.burn);

        return newObj;
    }

    public GameObject returnGun1(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.simpleGun1, where, Quaternion.identity);


        //newObj.AddComponent<interactionScript>();
        //interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        //theInteractionScript.addInteraction(enactionCreator.interType.shootFlamethrower1, interactionScript.effect.burn);

        return newObj;
    }




    public GameObject makeEmptyIntSphere(Vector3 position)
    {

        GameObject prefabToUse = repository2.singleton.interactionSphere;
        GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, position);

        return newProjectile;
    }


    public void projectileGenerator(projectileInfo theProjectileInfo, IEnactaBool theEnactable, Vector3 startPoint, Vector3 direction)//(rangedEnaction enInfo, interactionInfo interINFO, IEnactaBool theEnactable)
    {

        //Debug.Log("projectileGenerator");
        //Transform firePoint, enactionCreator.interType interactionType, bool sdOnCollision = true, int timeUntilSelfDestruct = 99, float growthSpeed = 0f, float magnitudeOfInteraction = 1f)

        //Vector3 startPoint = enInfo.firePoint.position + enInfo.firePoint.forward;

        GameObject newObjectForProjectile = makeEmptyIntSphere(startPoint);
        projectile1.genProjectile1(newObjectForProjectile, theProjectileInfo, direction);//enInfo, interINFO, theEnactable);
        growScript1.genGrowScript1(newObjectForProjectile, theProjectileInfo.growthSpeed);

        //should this use "interactionMate" isntead?
        //authorScript1.GENAuthorScript1(newObjectForProjectile, theEnactable);
        //interactionSpheres already have an author script!  use THIS function instead:
        authorScript1.FILLAuthorScript1(newObjectForProjectile, theEnactable);
        selfDestructScript1 sds = newObjectForProjectile.GetComponent<selfDestructScript1>();
        sds.timeUntilSelfDestruct = theProjectileInfo.timeUntilSelfDestruct;

        /*

        //Debug.Log("projectile made supposedly");
        //mastLine(startPoint, Color.red);
        //mastLine(newProjectile.transform.position, Color.blue);


        //Debug.DrawLine(newProjectile.transform.position, new Vector3(), Color.red);

        //threatAlert(this);

        */
    }


    public void OLDprojectileGenerator(intSpherAtor theEnactable)
    {
        /*
        //Transform firePoint, enactionCreator.interType interactionType, bool sdOnCollision = true, int timeUntilSelfDestruct = 99, float growthSpeed = 0f, float magnitudeOfInteraction = 1f)

        Vector3 startPoint = theEnactable.firePoint.position + theEnactable.firePoint.forward;

        GameObject newObjectForProjectile = makeEmptyIntSphere(startPoint);
        projectile1.genProjectile1(newObjectForProjectile, theEnactable);
        growScript1.genGrowScript1(newObjectForProjectile, theEnactable);

        //should this use "interactionMate" isntead?
        authorScript1.GENAuthorScript1(newObjectForProjectile, theEnactable);


        /*

        //Debug.Log("projectile made supposedly");
        //mastLine(startPoint, Color.red);
        //mastLine(newProjectile.transform.position, Color.blue);


        //Debug.DrawLine(newProjectile.transform.position, new Vector3(), Color.red);

        //threatAlert(this);

        */
    }

    internal GameObject returnShotgun1(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.shotgun1, where, Quaternion.identity);


        //newObj.AddComponent<interactionScript>();
        //interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        //theInteractionScript.addInteraction(enactionCreator.interType.shootFlamethrower1, interactionScript.effect.burn);

        return newObj;
    }
}
