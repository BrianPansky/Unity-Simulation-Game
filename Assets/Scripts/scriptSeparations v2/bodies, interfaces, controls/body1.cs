using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class body1 : MonoBehaviour
{
    //different things can be controlled in the game [human, vehicle, menu]
    //this is for a human body
    //it contains the actions that could be done if you press buttons on a controller
    //      for now, only actions which are IDENTICAL for both NPC and player will be put here
    //note this doesn't need to LIMIT the NPC.  they can ALSO have OTHER ways of doing similar things [like walking with navmesh]

    //so, which actions does it have?
    //forward, back, strafing, aiming up down left/right
    //jump
    //grab?  i guess.  without a body, even an NPC can't grab anything?
    //use items?

    //public GameObject pointerPoint;
    //public Vector3 pointerOrigin;

    public Vector3 lookingVector;
    public Ray lookingRay;

    public float standardClickDistance = 7.0f;

    public worldScript theWorldScript;
    public enactionScript theEnactionScript;
    public interactionScript theInteractionScript;
    public mapZoneScript theLocalMapZoneScript;


    public float maxHealth = 100f;
    public float currentHealth = 0f;  //set to max health in "awake"


    void Awake()
    {
        currentHealth = maxHealth;
        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;





        if(theEnactionScript == null)
        {
            this.gameObject.AddComponent<enactionScript>();
            theEnactionScript = this.gameObject.GetComponent<enactionScript>();
            theEnactionScript.theBody = this;
        }
        
        //enactionScript.availableEnactions.Add("walk");
        theEnactionScript.availableEnactions.Add("navMeshWalk");
        theEnactionScript.availableEnactions.Add("aim");
        theEnactionScript.availableEnactions.Add("standardClick");


        
    }

    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log("pointerPoint:  " + pointerPoint);

        //GameObject myTest2 = theWorldScript.theRespository.createAndReturnPrefabAtPointWITHNAME(theWorldScript.theRespository.invisiblePoint, this.gameObject.transform.position + new Vector3(0, 0, 0.8f), this.gameObject.name + "pointer");

        //myTest2.transform.SetParent(this.gameObject.transform, true);
        //pointerPoint = myTest2;
        //pointerOrigin = this.gameObject.transform.position + new Vector3(0, 0, 0.8f);

        //Debug.Log("pointerPoint:  " + pointerPoint);

        //"bullet1"
        if(theInteractionScript == null)
        {
            this.gameObject.AddComponent<interactionScript>();
            theInteractionScript = this.gameObject.GetComponent<interactionScript>();
            theInteractionScript.dictOfInteractions = new Dictionary<string, List<string>>(); //for some reason it was saying it already had that key in it, but it should be blank.  so MAKING it blank.
        }

        //theInteractionScript.dictOfInteractions.Add("bullet1", "die");  shoot1
        theInteractionScript.addInteraction("shoot1", "damage");
        theInteractionScript.addInteraction("shootFlamethrower1", "damage");
        //.Add("walk");
    }

    // Update is called once per frame
    void Update()
    {
        //      Vector3 startV = lookingRay.origin;

        //Vector3 diffV = (endV - startV);
        //Vector3 drawV = endV + diffV * lengthMultiplier * lengthMultiplier * diffV.sqrMagnitude * diffV.sqrMagnitude / 10;





        //Vector3 startV = this.gameObject.transform.position;
        //Vector3 endV = startV + (4*lookingRay.direction);//.operator(2);// * someBoolsAdded[thePointIndex];
        //Debug.DrawLine(startV, endV, new Color(0f, 1f, 0f), 1f);

    }

    public void killThisBody()
    {
        //this.gameObject.SetActive(false);

        theLocalMapZoneScript.theList.Remove(this.gameObject);
        theLocalMapZoneScript.threatList.Remove(this.gameObject);



        theWorldScript.theTagScript.foreignRemoveALLtags(this.gameObject);

        Debug.Log("destroy this object:  " + this.gameObject.GetInstanceID() + this.gameObject);
        UnityEngine.Object.Destroy(this.gameObject);

    }


    public Vector3 pointerOrigin()
    {
        //couldn't store this relative position, soooo just generate it when needed >.<
        return this.gameObject.transform.position + new Vector3(0, 0, 0.69f);
    }


}
