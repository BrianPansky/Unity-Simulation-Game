using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static virtualGamepad;

public class projectile1 : MonoBehaviour
{
    //just quick test for now, mostly so that tank shot interactions are VISIBLE:
    public bool explode = false;

    public float speed = 0f;
    public Vector3 Direction = new Vector3(0,0,0);
    public bool selfDestructOnCollision = true;

    void Awake()
    {
        speed = 1.61f;
        //float speed;
        //Vector3 Direction = new Vector3(0, 0, 1);
        Vector3 Direction;
    }

    public static void genProjectile1(GameObject theObject,projectileInfo inputProjectileInfo, Vector3 direction = new Vector3())//IEnactaBool enactingThis)//(GameObject theObject, int timeUntilSelfDestruct, Vector3 direction = new Vector3(), bool sdOnCollision = true, bool explodeOnDestroy = false)//, IEnactaBool enactingThis)
    {
        //use like THIS:
        //authorScript1.genProjectile1(x, y, z);

        projectile1 projectileScript = theObject.AddComponent<projectile1>();

        //i dunno, at this point, why not merely store "intSpherAtor interactionSphereCreator" itself on this script
        //instead of all these details from it?
        //should i....reverse it?  you ....
        //...no can't create free floating components that aren't attached to anything, i don't think...

        //hmm, i don't have an input that modifies speed yet?
        //      projectileScript.speed = 1f;

        projectileScript.speed = inputProjectileInfo.speed;
        projectileScript.Direction = direction;
        projectileScript.selfDestructOnCollision = inputProjectileInfo.sdOnCollision;
        selfDestructScript1 killScript = theObject.GetComponent<selfDestructScript1>();
        killScript.timeUntilSelfDestruct = inputProjectileInfo.timeUntilSelfDestruct;

        projectileScript.explode = inputProjectileInfo.explodeOnDestroy;

        //if (interactionType == interType.tankShot)
        {
            //projectileScript.explode = true;
        }

    }

    public static void OLDgenProjectile1(GameObject theObject, intSpherAtor interactionSphereCreator)//, IEnactaBool enactingThis)
    {
        //use like THIS:
        //authorScript1.genProjectile1(x, y, z);

        projectile1 projectileScript = theObject.AddComponent<projectile1>();

        //i dunno, at this point, why not merely store "intSpherAtor interactionSphereCreator" itself on this script
        //instead of all these details from it?
        //should i....reverse it?  you ....
        //...no can't create free floating components that aren't attached to anything, i don't think...

        //hmm, i don't have an input that modifies speed yet?
        projectileScript.speed = 1f;

        projectileScript.Direction = interactionSphereCreator.firePoint.transform.forward;
        projectileScript.selfDestructOnCollision = interactionSphereCreator.sdOnCollision;
        selfDestructScript1 killScript = theObject.GetComponent<selfDestructScript1>();
        killScript.timeUntilSelfDestruct = interactionSphereCreator.timeUntilSelfDestruct;

        if (interactionSphereCreator.interactionType == interType.tankShot)
        {
            projectileScript.explode = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = this.gameObject.transform.position + Direction*speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //technically, this is an interaction.  but whatever?  just put it here.



        if (selfDestructOnCollision && other.gameObject.tag != "aMapZone")
        {
            //Debug.Log("projectile1, selfDestructOnCollision, this.gameObject.name:  " + this.gameObject.name);

            Destroy(this.gameObject);

            //if (delay == 0)
            {
                //Destroy(this.gameObject);
            }
            //delay -= 1;
        }
    }

    void OnDestroy()
    {

        //mm, doesn't work:
        if (Application.isPlaying == false) { return; }
        //should put this somewhere else?  just putting it here FOR NOW:
        if (explode == true)
        {
            //new intSpherAtor(transform, enactionCreator.interType.tankShot, virtualGamepad.buttonCategories.aux1)




            GameObject prefabToUse = repository2.singleton.interactionSphere;
            Vector3 startPoint = this.transform.position;

            GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, startPoint);


            projectile1 projectileScript = newProjectile.AddComponent<projectile1>();
            projectileScript.speed = 0.17f;
            //projectileScript.Direction = this.firePoint.transform.forward;
            projectileScript.selfDestructOnCollision = false;
            selfDestructScript1 killScript = newProjectile.GetComponent<selfDestructScript1>();
            killScript.timeUntilSelfDestruct = 4;
            //killScript.
            newProjectile.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            growScript1 growScript = newProjectile.AddComponent<growScript1>();
            growScript.growthSpeed = 3f;

            //newProjectile.GetComponent<Renderer>().material.color = new Color(1f, 1f, 0f);
            MeshRenderer theRenderer = newProjectile.GetComponent<MeshRenderer>();
            theRenderer.material.color = new Color(1f, 1f, 0f);
            //theRenderer.material.shader.
            //theRenderer.

            //don't bother with this for now.  need to later though.
            //intSpherAtor thing =
            


            authorScript1 auth = newProjectile.GetComponent<authorScript1>();
            //auth.enacting = new intSpherAtor(this.transform, enactionCreator.interType.tankShot, virtualGamepad.buttonCategories.aux1, 1).genAuthorScript2(newProjectile, this.gameObject);
            List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
            enactableBoolSet.Add(new intSpherAtor(this.transform, interType.tankShot, buttonCategories.aux1, 1f, false));
            auth.enacting = enactableBoolSet[0];
            auth.enacting.interInfo = new interactionInfo(interType.tankShot);
            auth.enacting.interInfo.enactionAuthor = this.gameObject;  //uhhhhh super messy for now...
        }
    }
}

public class projectileInfo
{
    //stuff to keep track of about the projectile BEFORE it is generated
    public float speed = 1f;
    public bool sdOnCollision = true;
    public int timeUntilSelfDestruct = 99;
    public float growthSpeed = 0f;
    public bool affectedByGravity = false;
    public bool explodeOnDestroy = false;

    public projectileInfo(float speed, bool sdOnCollision = true, int timeUntilSelfDestruct = 99, float growthSpeed = 0f,
            bool affectedByGravity = false, bool explodeOnDestroy = false)
    {
        this.speed = speed;
        this.sdOnCollision = sdOnCollision;
        this.explodeOnDestroy = explodeOnDestroy;
        this.affectedByGravity = affectedByGravity;
        this.growthSpeed = growthSpeed;
        this.timeUntilSelfDestruct = timeUntilSelfDestruct;
    }
}