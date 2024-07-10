using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;

public class comboGen : MonoBehaviour
{
    //for making partially pre-defined combinatios of things more easily

    public static comboGen singleton;

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



    public GameObject instantInteractionSphere(Vector3 theLocation)
    {
        GameObject anInteractionSpherePrefab = repository2.singleton.interactionSphere;
        GameObject newInstantInteractionSphere = genGen.singleton.createPrefabAtPointAndRETURN(anInteractionSpherePrefab, theLocation);
        selfDestructScript1 sds = newInstantInteractionSphere.GetComponent<selfDestructScript1>();
        sds.timeUntilSelfDestruct = 0;

        return newInstantInteractionSphere;
    }




    public void tankShotExplosion(Vector3 theLocation)
    {

        GameObject newProjectile = expandingSphere(theLocation, 3, 4);

        MeshRenderer theRenderer = newProjectile.GetComponent<MeshRenderer>();
        theRenderer.material.color = new Color(1f, 1f, 0f);

        authorScript1 auth = newProjectile.GetComponent<authorScript1>();
        List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
        auth.interactionType = enactionCreator.interType.tankShot;
        auth.enacting = new hitscanEnactor(this.transform, buttonCategories.aux1, new interactionInfo(interType.tankShot, 0.1f), 1f);
        //      enactableBoolSet.Add(new intSpherAtor(this.transform, interType.tankShot, buttonCategories.aux1, 1f, false));
        //          auth.enacting = enactableBoolSet[0];
        //          auth.enacting.interInfo = new interactionInfo(interType.tankShot);
        auth.enacting.enactionAuthor = this.gameObject;  //uhhhhh super messy for now...
    }

    public void tankDeathExplosion(Vector3 theLocation)
    {
        flyingDebris(theLocation);
        flyingDebris(theLocation);
        flyingDebris(theLocation);

        GameObject mainExplosion = expandingSphere(theLocation, 4.1f, 3);
        mainExplosion.GetComponent<Renderer>().material = repository2.singleton.explosion1;

        GameObject smoke = expandingSphere(theLocation +4*Vector3.up, 0.7f, 28);
        smoke.GetComponent<Renderer>().material = repository2.singleton.smoke1;
        Destroy(smoke.GetComponent<Collider>());
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
        //killScript.
        newProjectile.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        growScript1 growScript = newProjectile.AddComponent<growScript1>();
        growScript.growthSpeed = growthSpeed;

        return newProjectile;
    }

    public GameObject flyingDebris(Vector3 theLocation)
    {
        GameObject prefabToUse = repository2.singleton.interactionSphere;
        GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, theLocation);



        //projectile1.genProjectile1(newProjectile, null, new Vector3(0, 0, 0), true);//newProjectile.AddComponent<projectile1>();
        //projectile1 projectileScript = newProjectile.GetComponent<projectile1>();
        projectile1 projectileScript = newProjectile.AddComponent<projectile1>();
        projectileScript.speed = 2.4f;
        projectileScript.selfDestructOnCollision = false;
        projectileScript.gravity = true;

        float randomX = Random.Range(-0.5f, 0.5f);
        float randomZ = Random.Range(-0.5f, 0.5f);
        //Debug.Log("randomX:  " + randomX);
        //Debug.Log("randomZ:  " + randomZ);
        projectileScript.Direction = new Vector3(randomX, 1 , randomZ);// Vector3.up + 
        selfDestructScript1 killScript = newProjectile.GetComponent<selfDestructScript1>();
        //      killScript.timeUntilSelfDestruct = timeUntilSelfDestruct;

        newProjectile.GetComponent<selfDestructScript1>().timeUntilSelfDestruct = 90;

        //killScript.
        newProjectile.transform.localScale = new Vector3(1,1,1);//new Vector3(0.2f, 0.2f, 0.2f);

        //..............why isn't THIS working?????
        //Rigidbody aRigidBody = newProjectile.AddComponent<Rigidbody>();
        //      newProjectile.AddComponent<Rigidbody>();
        Rigidbody aRigidBody = newProjectile.GetComponent<Rigidbody>();
        //Debug.Log("ummmmmm aRigidBody:  " + aRigidBody);
        //Debug.Log("ummmmmm newProjectile.GetComponent<Rigidbody>():  " + newProjectile.GetComponent<Rigidbody>());
        aRigidBody.useGravity = true;
        aRigidBody.isKinematic = false;
        aRigidBody.mass = 55;


        return newProjectile;
    }

}
