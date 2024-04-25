using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class enactionScript : MonoBehaviour
{
    //      just make strings for now [can modularize into sub-atoms and all of that later]:
    public List<string> availableEnactions = new List<string>();


    //adhoc for now:
    public int firingCooldown = 0;


    public body1 theBody;

    void Start()
    {
        //theBody = this.gameObject.GetComponent<body1>();
    }


    void Update()
    {
        
    }







}

public class enactionMate
{
    public GameObject enactionAuthor;
    public body1 enactionBody;

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
            if (true == false)
            {

                //Debug.Log("in transit");
                //Debug.Log("enactionBody.standardClickDistance:  " + enactionBody.standardClickDistance);
                //Debug.Log("distance remaining:  " + distanceBetween(enactionAuthor, enactionTarget));
                if (distanceBetween(enactionAuthor, enactionTarget) < enactionBody.standardClickDistance * 0.9f)
                {

                    //Debug.Log("DONE transit");
                    //instant stop for testing:
                    enactionAuthor.GetComponent<AIHub2>().thisNavMeshAgent.SetDestination(enactionAuthor.transform.position);
                    deleteThisEnaction = true;
                }
            }
            return;
        }
        

        if (enactThis == "walk")
        {

            //Debug.Log("walk");
            float speed = 0f;
            //Vector3 Direction = new Vector3(0, 0, 0);
            speed = 0.11f;
            //Ray myRay = this.gameObject.GetComponent<body1>().lookingRay;
            //body1 theBody = enactionAuthor.GetComponent<body1>();
            Ray myRay = enactionBody.lookingRay;     //not "enactionTarget"???
            //Direction = this.gameObject.GetComponent<body1>();
            Vector3 Direction = new Vector3(myRay.direction.x, 0, myRay.direction.z);
            enactionAuthor.transform.position = enactionAuthor.transform.position + Direction * speed;

        }
        else if (enactThis == "aim")
        {
            //Debug.Log("aim");
            //set lookingRay to a random target
            //body1 theBody = enactionAuthor.GetComponent<body1>();

            //theBody.lookingRay = new Ray(this.transform.position, (theBody.theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theBody.theWorldScript.theTagScript.findXNearestToY("mapZone", this.gameObject).GetComponent<mapZoneScript>().theList, this.gameObject).transform.position - this.transform.position));
            //semiRandomUsuallyNearTargetPicker
            //enactionBody.lookingRay = new Ray(enactionBody.pointerPoint.transform.position, (enactionTarget.transform.position - enactionBody.pointerPoint.transform.position));

            //Vector3 startPoint = enactionBody.pointerOrigin();
            //Vector3 endPoint = enactionTarget.transform.position;
            Vector3 startPoint = enactionBody.pointerOrigin();
            //Vector3 endPoint = startPoint + new Vector3(6,7,8);

            Vector3 endPoint = enactionTarget.gameObject.transform.position;
            //mastLine(startPoint, Color.white, 1f);
            //mastLine(endPoint, Color.green, 1f);

            //          WHICH OF THESE IS THE CORRECT WAY TO DO IT????
            //enactionBody.lookingRay = new Ray(startPoint, (startPoint - endPoint));
            enactionBody.lookingRay = new Ray(startPoint, (endPoint - startPoint));

            //Debug.DrawRay(enactionBody.lookingRay.origin, enactionBody.lookingRay.direction, Color.magenta, 77f);

            //new Ray(this.transform.position, (adhocPrereqFillerTest[0].target1.transform.position - this.transform.position));
            deleteThisEnaction = true;

        }
        else if (enactThis == "standardClick")
        {
            //didATry = true;


            Vector3 startPoint = enactionBody.pointerOrigin();
            //Vector3 endPoint = startPoint + new Vector3(6,7,8);

            Vector3 endPoint = enactionTarget.gameObject.transform.position;
            //mastLine(startPoint, Color.white, 1f);
            //mastLine(endPoint, Color.green, 1f);

            //          WHICH OF THESE IS THE CORRECT WAY TO DO IT????
            //enactionBody.lookingRay = new Ray(startPoint, (startPoint - endPoint));
            enactionBody.lookingRay = new Ray(startPoint, (endPoint - startPoint));


            //Vector3 p1 = enactionAuthor.transform.position;
            //Vector3 p2 = new Vector3(p1.x, p1.y + 3, p1.z);
            //Debug.DrawLine(p1, p2, Color.yellow, 1f);

            //for testing
            //enactionAuthor.GetComponent<AIHub2>().thisNavMeshAgent.Stop();

            //Debug.Log("standardClick");




            if (true == true)
            {

                RaycastHit myHit;
                //body1 theBody = this.gameObject.GetComponent<body1>();

                //Debug.Log("enactionTarget:  " + enactionTarget);
                Ray myRay = enactionBody.lookingRay;
                //Debug.DrawRay(enactionBody.lookingRay.origin, enactionBody.lookingRay.direction, Color.blue);

                //Debug.DrawLine(enactionAuthor.transform.position, enactionBody.lookingRay.origin, Color.red, 0.8f);
                //Debug.DrawLine(enactionAuthor.transform.position, enactionAuthor.transform.position + enactionBody.lookingRay.direction, Color.blue, 22f);

                //Debug.Log("enactionBody.standardClickDistance:  " + enactionBody.standardClickDistance);
                if (Physics.Raycast(myRay, out myHit, enactionBody.standardClickDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    if (myHit.transform != null && myHit.transform.gameObject == enactionTarget)
                    {
                        if (enactionTarget.name == "returnTestLOCK1(Clone)")
                        {
                            //Debug.Log("click worked            enactionTarget.name == returnTestLOCK1(Clone)");
                            //if (myHit.name == "returnTestLOCK1(Clone)")
                            if (myHit.transform.gameObject.name == "returnTestLOCK1(Clone)")
                            {
                                //Debug.Log("click worked            myHit.transform.gameObject.name == returnTestLOCK1(Clone)");

                            }
                        }

                        //Debug.Log("click worked?????");

                        //Debug.Log("click hit:  " + myHit.transform.gameObject);
                        GameObject anInteractionSphere = enactionBody.theWorldScript.theRespository.interactionSphere;

                        GameObject thisObject = enactionBody.theWorldScript.theRespository.createPrefabAtPointAndRETURN(anInteractionSphere, myHit.point);

                        //      should this use "interactionMate" isntead?
                        authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
                        theAuthorScript.theAuthor = enactionAuthor;
                        theAuthorScript.interactionType = "standardClick";


                        //Debug.DrawLine(enactionAuthor.transform.position, enactionTarget.transform.position, Color.green, 0.9f);
                        //see how far interactionSphere is from it's supposed target:
                        //Debug.DrawLine(thisObject.transform.position, enactionTarget.transform.position, Color.red, 0.9f);
                        deleteThisEnaction = true;
                    }
                    else
                    {
                        //Debug.Log("click returned null");
                        if (myHit.transform == null)
                        {
                            //Vector3 p11 = enactionAuthor.transform.position;
                            //Vector3 p22 = new Vector3(p1.x, p1.y + 8, p1.z);
                            //Debug.DrawLine(p1, p2, Color.blue, 22f);


                            //mastLine(enactionAuthor.transform.position, Color.blue, 88f);
                        }
                        if (myHit.transform.gameObject != enactionTarget)
                        {
                            //Debug.Log("this author:  " + enactionAuthor + "  hit this object:  " + myHit.transform.gameObject);

                            //Debug.DrawLine(enactionAuthor.transform.position, myHit.point, Color.blue, 8f);

                            //mastLine(enactionAuthor.transform.position, Color.red, 1f);

                            //mastLine(myHit.point, Color.yellow, 7f);
                            //mastLine(myHit.transform.position, Color.cyan, 3f);





                            //Vector3 p11 = enactionAuthor.transform.position;
                            //Vector3 p22 = new Vector3(p1.x, p1.y + 8, p1.z);
                            //Debug.DrawLine(p1, p2, Color.yellow, 22f);
                        }
                            
                    }

                }
                else
                {

                    //mastLine(enactionAuthor.transform.position, Color.cyan, 88f);






                    //Vector3 p11 = enactionAuthor.transform.position;
                    //Vector3 p22 = new Vector3(p1.x, p1.y + 3, p1.z);
                    //Debug.DrawLine(p1, p2, Color.red, 1f);
                    //Debug.DrawLine(thisObject.transform.position, enactionTarget.transform.position, Color.red, 0.9f);
                    //Debug.Log("click didn't work");
                }

            }

            firingCooldown--;
        }
        else if (enactThis == "navMeshWalk")
        {
            //Debug.Log("navMeshWalk");
            //body1 theBody = this.gameObject.GetComponent<body1>();
            //theBody.lookingRay = new Ray(this.transform.position, (theBody.theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theBody.theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable"), this.gameObject).transform.position - this.transform.position));

            //Vector3 targetVector = theBody.theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theBody.theWorldScript.theTagScript.findXNearestToY("mapZone", this.gameObject).GetComponent<mapZoneScript>().theList, this.gameObject).transform.position;
            //Vector3 targetVector = theBody.theWorldScript.theTagScript.semiRandomUsuallyNearTargetPickerFromList(theBody.theLocalMapZoneScript.theList, this.gameObject).transform.position;

            enactionAuthor.GetComponent<AIHub2>().thisNavMeshAgent.Resume();

            //set an endpoint not QUITE at the target object, so there's room
            //Vector3 currentNPCPosition = enactionAuthor.transform.position;
            //Vector3 targetObjectPosition = enactionTarget.transform.position;
            //Vector3 lineBetween = targetObjectPosition - currentNPCPosition;
            //Vector3 lineBetweenNormalized = lineBetween.normalized;
            //Vector3 lineBetweenNormalized = targetObjectPosition.normalized - currentNPCPosition.normalized;
            //Vector3 positionNEARtheTarget = targetObjectPosition - lineBetweenNormalized*2;

            //  just in case fewer lines and variables is faster:
            Vector3 lineBetweenNormalized = enactionTarget.transform.position.normalized - enactionAuthor.transform.position.normalized;
            Vector3 positionNEARtheTarget = enactionTarget.transform.position - lineBetweenNormalized*1.5f;
            enactionAuthor.GetComponent<AIHub2>().thisNavMeshAgent.SetDestination(positionNEARtheTarget);

            //this.gameObject.GetComponent<AIHub2>().thisNavMeshAgent.isStopped = true;
            inTransit = true;
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
                GameObject makeThis = enactionBody.theWorldScript.theRespository.interactionSphere;


                GameObject thisObject = enactionBody.theWorldScript.theRespository.createPrefabAtPointAndRETURN(makeThis, enactionAuthor.transform.position);
                //UnityEngine.Object.Destroy(thisObject.GetComponent<selfDestructScript1>());
                thisObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                thisObject.transform.position += enactionBody.lookingRay.direction;
                //theInteractionMate.interactionAuthor.transform.position + new Vector3(0, 0, 0)
                projectile1 projectileScript = thisObject.AddComponent<projectile1>();
                projectileScript.Direction = enactionBody.lookingRay.direction;
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
                GameObject makeThis = enactionBody.theWorldScript.theRespository.interactionSphere;


                GameObject thisObject = enactionBody.theWorldScript.theRespository.createPrefabAtPointAndRETURN(makeThis, enactionAuthor.transform.position);
                //UnityEngine.Object.Destroy(thisObject.GetComponent<selfDestructScript1>());
                thisObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                thisObject.transform.position += enactionBody.lookingRay.direction;
                //theInteractionMate.interactionAuthor.transform.position + new Vector3(0, 0, 0)
                projectile1 projectileScript = thisObject.AddComponent<projectile1>();
                projectileScript.Direction = enactionBody.lookingRay.direction;
                projectileScript.selfDestructOnCollision = false;
                selfDestructScript1 killScript = thisObject.GetComponent<selfDestructScript1>();
                killScript.timeUntilSelfDestruct = 10;
                //killScript.

                growScript1 growScript = thisObject.AddComponent<growScript1>();
                growScript.growthSpeed = 0.3f;

                //      should this use "interactionMate" isntead?

                authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
                theAuthorScript.theAuthor = enactionAuthor;
                //theAuthorScript.enactThisInteraction = theInteractionMate.enactThisInteraction;
                //theAuthorScript.interactionType = "bullet1";
                theAuthorScript.interactionType = "shootFlamethrower1";
                theAuthorScript.magnitudeOfInteraction = 10;
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
