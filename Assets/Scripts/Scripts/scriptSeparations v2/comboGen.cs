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

        colliderInteractor theCollisionInteraction = newProjectile.AddComponent<colliderInteractor>();
        List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
        theCollisionInteraction.interactionType = enactionCreator.interType.tankShot;
        theCollisionInteraction.enactionAuthor = this.gameObject;  //uhhhhh super messy for now...
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
        newProjectile.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        growScript1 growScript = newProjectile.AddComponent<growScript1>();
        growScript.growthSpeed = growthSpeed;

        return newProjectile;
    }

    public GameObject flyingDebris(Vector3 theLocation)
    {
        GameObject prefabToUse = repository2.singleton.interactionSphere;
        GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, theLocation);


        projectile1 projectileScript = newProjectile.AddComponent<projectile1>();
        projectileScript.speed = 2.4f;
        projectileScript.selfDestructOnCollision = false;
        projectileScript.gravity = true;

        float randomX = Random.Range(-0.5f, 0.5f);
        float randomZ = Random.Range(-0.5f, 0.5f);
        projectileScript.Direction = new Vector3(randomX, 1 , randomZ);// Vector3.up + 
        selfDestructScript1 killScript = newProjectile.GetComponent<selfDestructScript1>();

        newProjectile.GetComponent<selfDestructScript1>().timeUntilSelfDestruct = 90;

        newProjectile.transform.localScale = new Vector3(1,1,1);
        Rigidbody aRigidBody = newProjectile.GetComponent<Rigidbody>();
        aRigidBody.useGravity = true;
        aRigidBody.isKinematic = false;
        aRigidBody.mass = 55;


        return newProjectile;
    }
}
