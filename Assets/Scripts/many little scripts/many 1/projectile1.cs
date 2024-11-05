using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;


public class projectile1 : MonoBehaviour
{
    //just quick test for now, mostly so that tank shot interactions are VISIBLE:
    public bool explode = false;

    public float speed = 0f;
    public Vector3 Direction = new Vector3(0,0,0);
    public bool selfDestructOnCollision = true;
    public bool gravity = false;
    public float gavityRamp = 0f; //ad hoc for now
    public bool canWeDebugThisOrWhat = false;

    void Awake()
    {
        speed = 1.61f;
        Vector3 Direction;
    }

    public static void genProjectile1(GameObject theObject,projectileToGenerate inputprojectileToGenerate, Vector3 direction = new Vector3(), bool gravity = false)//IEnactaBool enactingThis)//(GameObject theObject, int timeUntilSelfDestruct, Vector3 direction = new Vector3(), bool sdOnCollision = true, bool explodeOnDestroy = false)//, IEnactaBool enactingThis)
    {
        //use like THIS:
        //authorScript1.genProjectile1(x, y, z);

        projectile1 projectileScript = theObject.AddComponent<projectile1>();

        //Debug.Log("inputprojectileToGenerate.speed:  "+ inputprojectileToGenerate.speed);
        projectileScript.speed = inputprojectileToGenerate.speed;
        projectileScript.Direction = direction;
        projectileScript.selfDestructOnCollision = inputprojectileToGenerate.sdOnCollision;
        selfDestructScript1 killScript = theObject.GetComponent<selfDestructScript1>();
        killScript.timeUntilSelfDestruct = inputprojectileToGenerate.timeUntilSelfDestruct;

        projectileScript.explode = inputprojectileToGenerate.explodeOnDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        speed = 1.5f;

        this.gameObject.transform.position = this.gameObject.transform.position + Direction*speed;
        if (canWeDebugThisOrWhat)
        {

            Debug.Log("......this.gameObject:  " + this.gameObject);
            Debug.Log("Direction:  " + Direction);
            Debug.Log("speed:  " + speed);
        }
        if (gravity == true)
        {
            this.gameObject.transform.position += 9 * gavityRamp * new Vector3(0,-1,0);
            gavityRamp += 0.018f;
        }
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

    void OnDestroy()
    {
        //mm, doesn't work:
        if (Application.isPlaying == false) { return; }
        //should put this somewhere else?  just putting it here FOR NOW:
        if (explode == true)
        {
            comboGen.singleton.tankShotExplosion(this.transform.position);
        }
    }
}

public class projectileToGenerate
{
    //stuff to keep track of about the projectile BEFORE it is generated, otherwise attach a projectile COMPONENT


    public float speed = 1f;
    public bool sdOnCollision = true;
    public int timeUntilSelfDestruct = 99;
    public float growthSpeed = 0f;
    public bool affectedByGravity = false;
    public bool explodeOnDestroy = false;

    public projectileToGenerate(float speed, bool sdOnCollision = true, int timeUntilSelfDestruct = 99, float growthSpeed = 0f,
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