using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class enactionScript : MonoBehaviour
{
    //      just make strings for now [can modularize into sub-atoms and all of that later]:
    //public List<string> availableEnactions = new List<string>();


    public List<string> currentlyUsable = new List<string>();

    public bool bodyCanBeUsed = false;

    //ad-hoc inputs:
    public float x = 0f;
    public float z = 0f;
    public float yawInput = 0f;
    public float pitchInput = 0f;
    //public Vector3 lookingVector = Vector3.zero;

    //could call it "primary auxilliary" or something?
    public bool jump = false;






    public List<enactionMate> availableEnactions = new List<enactionMate>();

    //adhoc for now:
    public int firingCooldown = 0;


    public NavMeshAgent thisNavMeshAgent;

    public GameObject enactionBody;




    //gamepad outputs:

    public virtualGamepad theGamePad;

    public Quaternion rotationFromLookingVector()
    {

        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        return Quaternion.identity;
    }







    //public body1 theBody;

    void Start()
    {
        //theBody = this.gameObject.GetComponent<body1>();

        if (theGamePad == null)
        {
            //this.gameObject.AddComponent<virtualGamepad>();
            //theGamePad = this.gameObject.GetComponent<virtualGamepad>();

            theGamePad = this.gameObject.AddComponent<virtualGamepad>();
        }
    }


    void Update()
    {
        //Debug.Log("...................enactionScript...................");

    }







}

public class enactionMate
{
    public GameObject enactionAuthor;
    //public body1 enactionBody;
    //public enactionScript authorEnactionScript;
    public sensorySystem authorSensorySystem;



    public bool navmeshResume = false;
    public NavMeshAgent thisNavMeshAgent;
    public Vector3 navmeshTarget;


    public string enactThis;
    public GameObject enactionTarget;


    //public GameObject target1;
    //public GameObject target2;
    //public GameObject target3;

    public GameObject returnClickedOn;

    public bool deleteThisEnaction = false;
    public bool didATry = false;
    public bool inTransit = false;

    //adhoc for now [should be part of a weapon or other item or whatever]:
    public int firingCooldown = 0;
    public int firingCooldownMax = 0;
    public bool glueGunFirstShotDoneBool = false;
    public GameObject currentGlueStartPoint = null;
    public float range = 0f;


    public bool projectileSelfDestructOnCollision = true;
    public int timeUntilProjectileSelfDestruct = 99;
    public float growthSpeed = 0f;
    public float magnitudeOfInteraction = 1f;

    //can i do without the following?
    public float enactionCost = 1f;
    //public string enactionCostType = "default";





    public void enact()
    {
        //Debug.Log("%%%%%%%%%%%%%%%%    ENACTION PHASE       %%%%%%%%%%%%%%%");

        //Debug.DrawLine(enactionAuthor.transform.position, enactionBody.pointerPoint.transform.position, Color.green, 0.6f);
        //Debug.DrawLine(enactionAuthor.transform.position, enactionTarget.transform.position, Color.white, 0.1f);

        //Debug.DrawRay(enactionBody.lookingRay.origin, enactionBody.lookingRay.direction, Color.red);
        //printMate();
        if(inTransit == true)
        {
            deleteThisEnaction = true;
            
            return;
        }
        


        if(navmeshResume == true)
        {
            if(enactionTarget == null)
            {
                return;
            }

            Vector3 lineBetweenNormalized = enactionTarget.transform.position.normalized - enactionAuthor.transform.position.normalized;
            Vector3 positionNEARtheTarget = enactionTarget.transform.position - lineBetweenNormalized * 1.5f;

            thisNavMeshAgent.SetDestination(positionNEARtheTarget);
            inTransit = true;
        }
        else if (enactThis == "firingByRaycastHit")
        {
            firingByRaycastHit("standardClick", range);
        }
        else if (enactThis == "fireProjectile")
        {
            if (firingCooldown == 0)
            {
                firingCooldown = firingCooldownMax;
                projectileGenerator(enactionAuthor, enactThis, projectileStartPoint1(), authorSensorySystem.lookingRay.direction, projectileSelfDestructOnCollision, timeUntilProjectileSelfDestruct, growthSpeed, magnitudeOfInteraction);
            }
        }











        if (enactThis == "walk")
        {

            //Debug.Log("walk");
            float speed = 0f;
            //Vector3 Direction = new Vector3(0, 0, 0);
            speed = 0.11f;
            //Ray myRay = this.gameObject.GetComponent<body1>().lookingRay;
            //body1 theBody = enactionAuthor.GetComponent<body1>();
            Ray myRay = authorSensorySystem.lookingRay;     //not "enactionTarget"???
            //Direction = this.gameObject.GetComponent<body1>();
            Vector3 Direction = new Vector3(myRay.direction.x, 0, myRay.direction.z);
            enactionAuthor.transform.position = enactionAuthor.transform.position + Direction * speed;

        }
        else if (enactThis == "shoot1")
        {
            //Debug.Log("shoot1");
            if (firingCooldown == 0)
            {
                firingCooldown = 5;

                Renderer objectsRenderer = enactionAuthor.GetComponent<Renderer>();
                if(objectsRenderer != null)
                {
                    //objectsRenderer.material.color = new Color(1f, 0f, 0f);
                }
                
                //this.gameObject.transform.scale = new Vector3(1f, 22, 1f);
                //this.gameObject.transform.

                //body1 authorBody = this.gameObject.GetComponent<body1>();
                //GameObject makeThis = authorBody.theWorldScript.theRespository.placeHolderCubePrefab;
                GameObject makeThis = authorSensorySystem.theWorldScript.theRespository.interactionSphere;


                GameObject thisObject = authorSensorySystem.theWorldScript.theRespository.createPrefabAtPointAndRETURN(makeThis, enactionAuthor.transform.position);
                //UnityEngine.Object.Destroy(thisObject.GetComponent<selfDestructScript1>());
                thisObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                thisObject.transform.position += authorSensorySystem.lookingRay.direction;
                //theInteractionMate.interactionAuthor.transform.position + new Vector3(0, 0, 0)
                projectile1 projectileScript = thisObject.AddComponent<projectile1>();
                projectileScript.Direction = authorSensorySystem.lookingRay.direction;
                selfDestructScript1 killScript = thisObject.GetComponent<selfDestructScript1>();
                killScript.timeUntilSelfDestruct = 830;

                //      should this use "interactionMate" isntead?
                
                authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
                theAuthorScript.theAuthor = enactionAuthor;
                //theAuthorScript.enactThisInteraction = theInteractionMate.enactThisInteraction;
                //theAuthorScript.interactionType = "bullet1";
                theAuthorScript.interactionType = "shoot1";

                theAuthorScript.magnitudeOfInteraction = 100;
                //Debug.Log("11111111111111the interaction type is:  " + theAuthorScript.interactionType);
                //theAuthorScript.theAuthor.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);

                threatAlert(theAuthorScript.theAuthor);
                //Vector3 p1 = theAuthorScript.theAuthor.transform.position;
                //Vector3 p2 = new Vector3(p1.x, p1.y + 22, p1.z);
                //Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 9999f);

            }
            else
            {
                firingCooldown -= 1;
            }


        }
        else if (enactThis == "shootFlamethrower1")
        {
            //Debug.Log("shootFlamethrower1");
            if (firingCooldown == 0)
            {
                firingCooldown = 3;

                Renderer objectsRenderer = enactionAuthor.GetComponent<Renderer>();
                if (objectsRenderer != null)
                {
                    //objectsRenderer.material.color = new Color(1f, 0f, 0f);
                }

                //this.gameObject.transform.scale = new Vector3(1f, 22, 1f);
                //this.gameObject.transform.

                //body1 authorBody = this.gameObject.GetComponent<body1>();
                //GameObject makeThis = authorBody.theWorldScript.theRespository.placeHolderCubePrefab;

                //Debug.Log("11111111111111the interaction type is:  " + theAuthorScript.interactionType);
                //theAuthorScript.theAuthor.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);


                projectileGenerator(enactionAuthor, "shootFlamethrower1", projectileStartPoint1(), authorSensorySystem.lookingRay.direction, false, 10, 0.3f, 10);

                //Vector3 p1 = theAuthorScript.theAuthor.transform.position;
                //Vector3 p2 = new Vector3(p1.x, p1.y + 22, p1.z);
                //Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 9999f);

            }
            else
            {
                firingCooldown -= 1;
            }


        }
        else if (enactThis == "glueGun")
        {
            if (glueGunFirstShotDoneBool == false)
            {
                //means we haven't fired first shot in the sequence yet.  the starting point.
                //so fire our starting point



                RaycastHit myHit;
                Ray myRay = authorSensorySystem.lookingRay; //Camera.main.ScreenPointToRay(Input.mousePosition);  //  COULD use this ray to ALSO update the "body1" lookingRay.  if i want to be more equivalent to NPCs

                //Debug.Log("enaction is firing");
                if (Physics.Raycast(myRay, out myHit, 7.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    //Debug.Log("raycast done");

                    if (myHit.transform != null)
                    {
                        //Debug.Log("myHit.transform != null:  " + myHit.transform);
                        //GameObject thisObject = createPrefabAtPointAndRETURN(theInteractionSphere, myHit.point);

                        currentGlueStartPoint = fireGlueStartingPoint(myHit.point);
                        currentGlueStartPoint.transform.SetParent(myHit.transform);
                        Renderer objectsRenderer = currentGlueStartPoint.GetComponent<Renderer>();
                        if (objectsRenderer != null)
                        {
                            objectsRenderer.material.color = new Color(0f, 1f, 0f);
                        }
                        glueGunFirstShotDoneBool = true;

                        //authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
                        //theAuthorScript.theAuthor = this.gameObject;
                        //theAuthorScript.interactionType = "glue";

                    }
                }







                

            }
            else
            {





                RaycastHit myHit;
                Ray myRay = authorSensorySystem.lookingRay; //Camera.main.ScreenPointToRay(Input.mousePosition);  //  COULD use this ray to ALSO update the "body1" lookingRay.  if i want to be more equivalent to NPCs

                //Debug.Log("enaction is firing");
                if (Physics.Raycast(myRay, out myHit, 7.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    //Debug.Log("raycast done");

                    if (myHit.transform != null)
                    {
                        //Debug.Log("myHit.transform != null:  " + myHit.transform);
                        //GameObject thisObject = createPrefabAtPointAndRETURN(theInteractionSphere, myHit.point);

                        GameObject newGlue = fireGlueEndPoint(myHit.point, currentGlueStartPoint);

                        newGlue.transform.SetParent(myHit.transform);
                        currentGlueStartPoint = null;
                        Renderer objectsRenderer = newGlue.GetComponent<Renderer>();
                        if (objectsRenderer != null)
                        {
                            objectsRenderer.material.color = new Color(0f, 0f, 1f);
                        }
                        glueGunFirstShotDoneBool = false;

                        //authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
                        //theAuthorScript.theAuthor = this.gameObject;
                        //theAuthorScript.interactionType = "glue";

                    }
                }

            }


            playerClickInteraction forPlayer = enactionAuthor.GetComponent<playerClickInteraction>();
            forPlayer.glueGunFirstShotDoneBool = glueGunFirstShotDoneBool;
            forPlayer.currentGlueStartPoint = currentGlueStartPoint;

        }



    }



    public void firingByRaycastHit(string theInteractionType, float theRange)
    {
        //Vector3 startPoint = authorSensorySystem.pointerOrigin();
        //Vector3 endPoint = enactionTarget.gameObject.transform.position;
        //authorSensorySystem.lookingRay = new Ray(startPoint, (endPoint - startPoint));


        RaycastHit myHit;
        Ray myRay = authorSensorySystem.lookingRay;


        if (Physics.Raycast(myRay, out myHit, theRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (myHit.transform != null && myHit.transform.gameObject == enactionTarget)
            {
                GameObject anInteractionSphere = authorSensorySystem.theWorldScript.theRespository.interactionSphere;
                GameObject thisObject = authorSensorySystem.theWorldScript.theRespository.createPrefabAtPointAndRETURN(anInteractionSphere, myHit.point);

                //      should this use "interactionMate" isntead?
                authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
                theAuthorScript.theAuthor = enactionAuthor;
                theAuthorScript.interactionType = theInteractionType;


                //see how far interactionSphere is from it's supposed target:
                //Debug.DrawLine(thisObject.transform.position, enactionTarget.transform.position, Color.red, 0.9f);
                deleteThisEnaction = true;
            }


        }


        firingCooldown--;
    }


    public GameObject fireGlueStartingPoint(Vector3 startPoint)
    {
        GameObject newGlue = newGlueGlob(startPoint, null);
        return newGlue;
    }

    public GameObject fireGlueEndPoint(Vector3 startPoint, GameObject otherGLue)
    {
        GameObject newGlue = newGlueGlob(startPoint, otherGLue);
        glueScript thisGlueScript = newGlue.GetComponent<glueScript>();
        thisGlueScript.otherGlue = otherGLue;

        return newGlue;
    }

    public GameObject newGlueGlob(Vector3 startPoint, GameObject otherGLue)
    {

        GameObject prefabToUse = authorSensorySystem.theWorldScript.theRespository.interactionSphere;


        GameObject newGlue = authorSensorySystem.theWorldScript.theRespository.createPrefabAtPointAndRETURN(prefabToUse, startPoint);
        selfDestructScript1 killScript = newGlue.GetComponent<selfDestructScript1>();
        UnityEngine.Object.Destroy(killScript);

        glueScript theGlueScript = newGlue.AddComponent<glueScript>();
        theGlueScript.otherGlue = otherGLue;

        return newGlue;
    }


    public void projectileGenerator(GameObject author, string interactionType, Vector3 startPoint, Vector3 direction, bool sdOnCollision = true, int timeUntilSelfDestruct = 99, float growthSpeed = 0f, float magnitudeOfInteraction = 1f)
    {
        GameObject prefabToUse = authorSensorySystem.theWorldScript.theRespository.interactionSphere;


        GameObject newProjectile = authorSensorySystem.theWorldScript.theRespository.createPrefabAtPointAndRETURN(prefabToUse, startPoint);
        //UnityEngine.Object.Destroy(thisObject.GetComponent<selfDestructScript1>());
        
        //newProjectile.transform.position += enactionBody.lookingRay.direction;
        //theInteractionMate.interactionAuthor.transform.position + new Vector3(0, 0, 0)
        projectile1 projectileScript = newProjectile.AddComponent<projectile1>();
        projectileScript.Direction = direction;
        projectileScript.selfDestructOnCollision = sdOnCollision;
        selfDestructScript1 killScript = newProjectile.GetComponent<selfDestructScript1>();
        killScript.timeUntilSelfDestruct = timeUntilSelfDestruct;
        //killScript.

        if(growthSpeed > 0f)
        {
            newProjectile.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            growScript1 growScript = newProjectile.AddComponent<growScript1>();
            growScript.growthSpeed = growthSpeed;
        }

        //      should this use "interactionMate" isntead?

        authorScript1 theAuthorScript = newProjectile.GetComponent<authorScript1>();
        theAuthorScript.theAuthor = author;
        //theAuthorScript.enactThisInteraction = theInteractionMate.enactThisInteraction;
        //theAuthorScript.interactionType = "bullet1";
        theAuthorScript.interactionType = interactionType;
        theAuthorScript.magnitudeOfInteraction = magnitudeOfInteraction;


        threatAlert(theAuthorScript.theAuthor);
    }

    public Vector3 projectileStartPoint1()
    {



        Debug.Log("enactionAuthor:  " + enactionAuthor);
        Debug.Log("enactionAuthor.transform.position:  " + enactionAuthor.transform.position);
        Debug.Log("authorSensorySystem:  " + authorSensorySystem);
        Debug.Log("authorSensorySystem.lookingRay:  " + authorSensorySystem.lookingRay);
        Debug.Log("authorSensorySystem.lookingRay.direction:  " + authorSensorySystem.lookingRay.direction);
        Debug.Log("enactionAuthor.transform.position + authorSensorySystem.lookingRay.direction:  " + enactionAuthor.transform.position + authorSensorySystem.lookingRay.direction);





        Debug.Log("enactionAuthor.transform.position + authorSensorySystem.lookingRay.direction:  " + enactionAuthor.transform.position + authorSensorySystem.lookingRay.direction);
        return enactionAuthor.transform.position + authorSensorySystem.lookingRay.direction;
    }


    void threatAlert(GameObject theThreat)
    {
        //take the threat object, add them to the threat list in this/their "map zone"
        //try not to add them as duplicate if they are already added

        //      #1, access their map zone:
        //AIHub2 thisHub = theThreat.GetComponent<AIHub2>();
        body1 thisBody = theThreat.GetComponent<body1>();
        //hmm, lists like this always go bad though if the object is destryed......but....ad-hoc.....[and i have such a list on map zones ALREADY]
        List<GameObject> thisThreatList = thisBody.theLocalMapZoneScript.threatList;

        //      #2, add them to a "list of threats" if they aren't already
        //[hmmmm, would be easier to use tags?  easier to code it, but that system KILLS game performance.....]
        //[what about adding a child object, with a UNITY tag that is relevant?  is that faster?  one way to find out.......?]
        if (isFuckingThingOnListAlready(theThreat,thisThreatList))
        {
            //do nothing, they are already on the list
        }
        else
        {
            thisThreatList.Add(theThreat);
        }


    }

    public bool isFuckingThingOnListAlready(GameObject fuckingThing, List<GameObject> theList)
    {

        foreach (GameObject thisItem in theList)
        {
            if(thisItem == fuckingThing)
            {
                return true;
            }
        }

        return false;
    }


    void mastLine(Vector3 startPoint, Color theColor, float theHeight)
    {
        Vector3 p1 = startPoint;
        Vector3 p2 = new Vector3(p1.x, p1.y + theHeight, p1.z);
        Debug.DrawLine(p1, p2, theColor, 22f);
    }

    public float distanceBetween(GameObject object1, GameObject object2)
    {
        //for now, just use "horizontal" distance
        Vector3 v1 = object1.transform.position;
        Vector3 v2 = object2.transform.position;
        //Vector3 theVectorBetweenXandZ = object1.transform.position - object2.transform.position;
        Vector3 theHorizontalVectorBetweenXandZ = new Vector3(v1.x - v2.x, 0, v1.z - v2.z);
        return theHorizontalVectorBetweenXandZ.sqrMagnitude;
    }




    public void printMate()
    {
        Debug.Log("MMMMMMMMMMMMMMMMMMMMMMMMMMMM    printing enaction mate    MMMMMMMMMMMMMMMMMMMMMMMMMMMM");
        Debug.Log("enactionAuthor:  " + enactionAuthor);
        Debug.Log("returnClickedOn:  " + returnClickedOn);
        Debug.Log("enactThis:  " + enactThis);
        Debug.Log("WWWWWWWWWWWWWWWWW    END printing enaction mate    WWWWWWWWWWWWWWWWW");
    }

}
