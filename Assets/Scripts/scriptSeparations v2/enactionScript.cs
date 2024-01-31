using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enactionScript : MonoBehaviour
{
    //      just make strings for now [can modularize into sub-atoms and all of that later]:
    public List<string> availableEnactions = new List<string>();


    //adhoc for now:
    public int firingCooldown = 0;

    void Start()
    {
        
    }


    void Update()
    {
        
    }












    public void stringEnaction(string stringToEnact)
    {
        //"walk"
        //"aim"
        //"standardClick"


        if(stringToEnact == "walk")
        {
            float speed = 0f;
            //Vector3 Direction = new Vector3(0, 0, 0);
            speed = 0.11f;
            Ray myRay = this.gameObject.GetComponent<body1>().lookingRay;
            //Direction = this.gameObject.GetComponent<body1>();
            Vector3 Direction = new Vector3(myRay.direction.x, 0, myRay.direction.z);
            this.gameObject.transform.position = this.gameObject.transform.position + Direction * speed;

        }
        if (stringToEnact == "aim")
        {
            //set lookingRay to a random target
            body1 theBody = this.gameObject.GetComponent<body1>();
            theBody.lookingRay = new Ray(this.transform.position, (theBody.theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theBody.theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable"), this.gameObject).transform.position - this.transform.position));
            //new Ray(this.transform.position, (adhocPrereqFillerTest[0].target1.transform.position - this.transform.position));

        }
        if (stringToEnact == "standardClick")
        {

            RaycastHit myHit;
            body1 theBody = this.gameObject.GetComponent<body1>();
            Ray myRay = theBody.lookingRay;

            if (Physics.Raycast(myRay, out myHit, 7.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if (myHit.transform != null)
                {

                    GameObject anInteractionSphere = theBody.theWorldScript.theRespository.interactionSphere;

                    GameObject thisObject = theBody.theWorldScript.theRespository.createPrefabAtPointAndRETURN(anInteractionSphere, myHit.point);

                    //      should this use "interactionMate" isntead?
                    authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
                    theAuthorScript.theAuthor = this.gameObject;
                    theAuthorScript.interactionType = "standardClick";

                }
            }
        }
        if(stringToEnact == "navMeshWalk")
        {
            body1 theBody = this.gameObject.GetComponent<body1>();
            //theBody.lookingRay = new Ray(this.transform.position, (theBody.theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theBody.theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable"), this.gameObject).transform.position - this.transform.position));

            Vector3 targetVector = theBody.theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theBody.theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable"), this.gameObject).transform.position;
            this.gameObject.GetComponent<AIHub2>().thisNavMeshAgent.SetDestination(targetVector);
        }
        if (stringToEnact == "shoot1")
        {
            if(firingCooldown == 0)
            {
                firingCooldown = 5;

                this.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);

                body1 authorBody = this.gameObject.GetComponent<body1>();
                //GameObject makeThis = authorBody.theWorldScript.theRespository.placeHolderCubePrefab;
                GameObject makeThis = authorBody.theWorldScript.theRespository.interactionSphere;


                GameObject thisObject = authorBody.theWorldScript.theRespository.createPrefabAtPointAndRETURN(makeThis, this.gameObject.transform.position);
                //UnityEngine.Object.Destroy(thisObject.GetComponent<selfDestructScript1>());
                thisObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                thisObject.transform.position += authorBody.lookingRay.direction;
                //theInteractionMate.interactionAuthor.transform.position + new Vector3(0, 0, 0)
                projectile1 projectileScript = thisObject.AddComponent<projectile1>();
                projectileScript.Direction = authorBody.lookingRay.direction;
                selfDestructScript1 killScript = thisObject.GetComponent<selfDestructScript1>();
                killScript.delay = 130;

                //      should this use "interactionMate" isntead?
                authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
                theAuthorScript.theAuthor = this.gameObject;
                //theAuthorScript.enactThisInteraction = theInteractionMate.enactThisInteraction;
                theAuthorScript.interactionType = "bullet1";
                //Debug.Log("11111111111111the interaction type is:  " + theAuthorScript.interactionType);


            }
            else
            {
                firingCooldown -= 1;
            }
            

        }


    }

}
