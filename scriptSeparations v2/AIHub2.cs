using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Threading;
using System.Xml.Linq;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class AIHub2 : MonoBehaviour
{
    public bool printWTFThisNPCDoin = false;

    //public CharacterController AIcontroller;

    public UnityEngine.Vector3 adHocThreatAvoidanceVector = new UnityEngine.Vector3(0,0,0);

    public NavMeshAgent currentNavMeshAgent;

    private worldScript theWorldScript;
    public GameObject savedNavMeshTarget;
    public planningAndImagination thePlanner;

    public body1 body;
    int myDelay = 0;
    int graphCooldown = 0;

    //i should move these to the BODY:
    public sensorySystem theSensorySystem;
    public inventory1 theInventory;

    public List<enactionMate> currentPlan = new List<enactionMate>();

    private UnityEngine.Vector3 mytest;
    public GameObject testTarget;




    public List<string> stringListToEnact = new List<string>();



    GameObject randomInteractionTarget;

    int forgetfulnessTimerEndpoint = 1112;
    int triesBeforeDeletingClickActionsEndPoint = 812;
    int currentTry = 0;
    public int forgetfulnessTimerCurrent = 0;
    int framesNOTinTransitBeforeDumpingAction = 2;  //super ad hoc, just don't want them sitting there if they targeted an NPC to click on, but the NPC has since moved to a different location.  yes maybe they should follow, but whatever
    int currentFramesNOTinTransit = 0;

    int shortPeriodicalEndpoint = 7;
    int currentPeriodicalPoint = 0;

    enactionScript theEnactionScript;

    //          NO LONGER USED:
    public int forgetfulnessTimer = 1;

    public int cooldownTimer = 0;

    void Awake()
    {


        if (body == null)
        {
            body = this.GetComponent<body1>();
            if (body == null)
            {
                body = this.gameObject.AddComponent<body1>();
            }
        }

        //AddComponent<AIHub2>();


        //if (this.gameObject.GetComponent<CharacterController>() == null)
        {
            //this.gameObject.AddComponent<CharacterController>();
            //AIcontroller = this.gameObject.GetComponent<CharacterController>();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if(currentNavMeshAgent == null)
        {
            currentNavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        }

        mytest = this.transform.position + new UnityEngine.Vector3(0, 0, -15);

        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;

        //body.pointerPoint = pointerPointToPutOnBody;


        //no, no, no, don't "get" component, CREATE it!
        //an AIHub2 should probably come with a sensory system by DEFAULT,
        //so why not build that into its initialization?
        
        if (theSensorySystem == null)
        {
            theSensorySystem = this.GetComponent<sensorySystem>();
            if (theSensorySystem == null)
            {
                theSensorySystem = this.gameObject.AddComponent<sensorySystem>();
            }

            theSensorySystem.body = body;
        }

        //same for planning:
        if (thePlanner == null)
        {
            thePlanner = this.GetComponent<planningAndImagination>();
            if (thePlanner == null)
            {
                thePlanner = this.gameObject.AddComponent<planningAndImagination>();
            }
        }

        //inventory1
        if (theInventory == null)
        {
            theInventory = this.GetComponent<inventory1>();
            if (theInventory == null)
            {
                theInventory = this.gameObject.AddComponent<inventory1>();
            }
        }


        theEnactionScript = this.gameObject.GetComponent<enactionScript>();

        theSensorySystem.lookingRay = startLookingRay();
    }

    public Ray startLookingRay()
    {
        return new Ray(this.transform.position + this.transform.forward, this.transform.forward);
    }

    // Update is called once per frame
    void Update()
    {


        //spatialDataSet myData = new spatialDataSet();
        //myData.threatList = threatListWithoutSelf();
        //myData.middlePoint = this.transform.position;
        //                      myData.thePoints = myData.generate2Xby2YNearPoints(myData.middlePoint, 15, 15);
        //                      myData.gatherData(myData.listOfStringsWithoutNulls("distancesToThreats", "threatAngles", "linesOfSight"));
        //myData.combineData(myData.listOfStringsWithoutNulls("distancesToThreats", "threatAngles", "linesOfSight"));
        //          myData.adhocVectorCreationForAttackDodge1();
        //adHocThreatAvoidanceVector = myData.bestMiddlePoint();
        //Debug.DrawLine(myData.middlePoint, myData.middlePoint+ adHocThreatAvoidanceVector, Color.green, 0.1f);



        spatialDataPoint myData = new spatialDataPoint();
        myData.initializeDataPoint(threatListWithoutSelf(), this.transform.position);

        adHocThreatAvoidanceVector = myData.applePattern();
        Debug.DrawLine(myData.thisPoint, myData.thisPoint + adHocThreatAvoidanceVector, Color.green, 0.1f);


        //                      myData.appleField();
        //myData.torusField1();
        //myData.graphFeild("default");
        //                      myData.graphFeildAdHoc();







        //              thisNavMeshAgent.SetDestination(this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized*8f);
        //navmeshStopping1();
        //Debug.Log("body:  " + body);
        //Debug.Log("body.theLocalMapZoneScript:  " + body.theLocalMapZoneScript);
        //if (body.theLocalMapZoneScript != null && body.theLocalMapZoneScript.isItThisZonesTurn)
        {
            //Debug.Log("???????????????????????????????????????????????????????????????????");
            //callableUpdate();
        }


    }

    //void OnDestroy()
    //{
        //does work when they die! .......but ALSO when you stop program.  which could be....a bit much.......
        //Debug.Log("does this do anything when they die?  or only when you stop the whole game or whatever???????????");
    //}

    public List<GameObject> threatListWithoutSelf()
    {
        List<GameObject> threatListWithoutSelf = new List<GameObject>();
        List<GameObject> thisThreatList = body.theLocalMapZoneScript.threatList;

        //Debug.Log("body.theLocalMapZoneScript.threatList.Count:  " + body.theLocalMapZoneScript.threatList.Count);
        //printAllIdNumbers(body.theLocalMapZoneScript.threatList);

        foreach (GameObject threat in thisThreatList)
        {
            //UnityEngine.Vector3 p1 = this.gameObject.transform.position;
            //UnityEngine.Vector3 p2 = threat.gameObject.transform.position;
            //Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 1f);
            if (threat != null && threat != this.gameObject)
            {
                threatListWithoutSelf.Add(threat);
            }
        }
        //Debug.Log("threatListWithoutSelf.Count:  " + threatListWithoutSelf.Count);
        //printAllIdNumbers(threatListWithoutSelf);
        return threatListWithoutSelf;
    }




    public void callableUpdate()
    {
        //Debug.Log("==============================================================");




        quickNewEnactionTestingEtc();









        //if (cooldownTimer > 11 && theEnactionScript.availableEnactions.Contains("shoot1"))
        if (cooldownTimer > 11)
        {
            //Debug.Log("111111111111111111111111111111111");
            cooldownTimer = 0;
            doingAThreatThing();


        }
        else
        {
            cooldownTimer++;
        }
        Vector3 newFinalPosition = this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized  * 8f;// * 0.2f;
        //          this.gameObject.transform.position = this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized * 0.2f;
        //AIcontroller.Move(adHocThreatAvoidanceVector.normalized * 0.2f);
        //                  thisNavMeshAgent.SetDestination(this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized * 8f);
        //NavMeshAgent thisNavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        currentNavMeshAgent.SetDestination(newFinalPosition);//for some reason it doesn't work if you put the *8f here, even though it works in the comparable "SetDestination" line above, so i had to put that into the "newFinalPosition" variable....


        if (printWTFThisNPCDoin)
        {
            Debug.Log("$$$$$$$$$$$$$$$$$$$$$$       BEGIN CALLABLE UPDATE       $$$$$$$$$$$$$$$$$$$$$$$$");
        }

        //navmeshSpeeding1();
        if (forgetfulnessTimerCurrent == 0)
        {
            //forget whole plan, start again:
            stringListToEnact = new List<string>();

            //reset timer:
            forgetfulnessTimerCurrent = forgetfulnessTimerEndpoint;
            //ya, ad-hoc:
            if (currentPlan.Count > 0)
            {

                if (currentPlan[0].enactionTarget == null) 
                {
                    //this happens if game object is destroyed, but still referenced in this variable
                    //ad-hoc solution for now is to do this!
                    currentPlan.Remove(currentPlan[0]);
                    return; 
                }
                if (currentPlan[0].enactionTarget.name == "returnTestLOCK1(Clone)" && currentPlan[0].enactThis == "standardClick")
                {
                    //Debug.Log("forgetfulnessTimerCurrent hit zero           for standardClick on returnTestLOCK1(Clone)");
                }
                currentPlan.RemoveAt(0);
            }
        }
        else
        {
            forgetfulnessTimerCurrent -= 1;
        }





        //justPickAnAbilityAndFireIt();

        //Debug.Log("do planFiller1");
        if (printWTFThisNPCDoin)
        {
            Debug.Log("do planFiller1");
        }
        planFiller1();


        //Debug.Log("currentPlan.Count:  " + currentPlan.Count);

        //Debug.Log("do enactNext");
        if (printWTFThisNPCDoin)
        {
            Debug.Log("do enactNext");
        }
        enactNext();
        //navmeshStopping1();


        //Debug.Log("//////////////////////////////////////");
    }



    public void quickNewEnactionTestingEtc()
    {
        //for now:
        //      the NPC looks for other objects 
        //      it picks one
        //      it looks at the interactions on the object
        //      it picks one
        //      Then it looks at its own actions to find
        //          which has the same interaction type as
        //              the interaction of the one from the object
        //      it picks THAT one
        //      And does it


        //I should already have the basic outline of this in a different function right now that just uses a different data type or something. 


        GameObject theTarget = pickRandomNearbyInteractable();
        string randomInteractionTypeOnTarget = NEWpickRandomInteractionONObject(theTarget);
        intSpherAtor thing = theEnactionScript.matchInteractionType(randomInteractionTypeOnTarget);

        if(thing !=null)
        {
            thing.enact(startLookingRay(), theSensorySystem);
        }
        


        if (true == false)
        {

            //SUPER ad hoc for now:
            //theEnactionScript.interactionSphereList[0].enact(theSensorySystem.lookingRay, theSensorySystem);
            //mastLine(startLookingRay().origin, Color.white);
            //mastLine(startLookingRay().direction, Color.green);
            theEnactionScript.interactionSphereList[0].enact(startLookingRay(), theSensorySystem);
        }


    }
    void mastLine(Vector3 startPoint, Color theColor, float theHeight = 10f)
    {
        Vector3 p1 = startPoint;
        Vector3 p2 = new Vector3(p1.x, p1.y + theHeight, p1.z);
        Debug.DrawLine(p1, p2, theColor, 22f);
    }


    public void planFiller1()
    {
        //Debug.Log("planFiller1 START");
        if (printWTFThisNPCDoin)
        {
            Debug.Log("planFiller1 START");
        }


        if (currentPlan.Count == 0)
        {

            //doingAThreatThing();


            currentPlan = pickSomethingToInteractWithAndPlanToTryIt();
        }

        //Debug.Log("currentPlan.Count:  " + currentPlan.Count);

        //Debug.Log("planFiller1 END");
        if (printWTFThisNPCDoin)
        {
            Debug.Log("planFiller1 END");
        }
    }


    public void doingAThreatThing()
    {
        //testing for threat detection:
        //Vector3 p1 = this.gameObject.transform.position;
        //Vector3 p2 = new Vector3(p1.x, p1.y + 22, p1.z);
        //Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 9999f);
        List<GameObject> thisThreatList = body.theLocalMapZoneScript.threatList;
        if(thisThreatList.Count > 0)
        {








            //                          spatialData1 myData = new spatialData1();
            //combineVectorForOnePoint2
            //                          myData.location = this.gameObject.transform.position;
            //myData.combineVectorForOnePoint2(myData.location);
            //myData.generateValidNearPoints1(myData.location);
            //      myData.gatherALLThreatDataTypes(thisThreatList);
            //myData.graphLookingAnglesSq1(myData.location);
            //      myData.combine2into1();
            //      myData.NEWgatherALLThreatDataTypes(threatListWithoutSelf);
            //myData.graphLookingAnglesSq1(myData.location);
            //      myData.NEWcombine2into1();
            if (graphCooldown < 0)
            {
                graphCooldown++;
            }
            else
            {
                graphCooldown = -2;


                List<GameObject> threatListWithoutSelf = new List<GameObject>();
                foreach (GameObject threat in thisThreatList)
                {
                    //UnityEngine.Vector3 p1 = this.gameObject.transform.position;
                    //UnityEngine.Vector3 p2 = threat.gameObject.transform.position;
                    //Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 1f);
                    if (threat != null && threat != this.gameObject)
                    {
                        threatListWithoutSelf.Add(threat);
                    }
                }


                if (threatListWithoutSelf.Count > 0)
                {
                    //spatialDataSet mySpatialDataSet = new spatialDataSet();
                    //mySpatialDataSet.InitializeFeild(this.gameObject.transform.position, threatListWithoutSelf, 9);
                    //mySpatialDataSet.appleGraph();
                }




                //myData.general8DirectionGraphEXAGGERATED(myData.location, myData.lookingAngleSamples);
                //      myData.general8DirectionGraphEXAGGERATED(myData.location, myData.combinedMeasures);
            }

            //myData.graphBetweenTwoPoints(myData.location, myData.pointAwayFromThreats(thisThreatList));
            //adHocThreatAvoidanceVector = (this.gameObject.transform.position - myData.pointAwayFromThreats(thisThreatList));
            //      adHocThreatAvoidanceVector = (this.gameObject.transform.position - myData.thePoints[myData.whichIndexIsHighest(myData.combinedMeasures)]);
            //          adHocThreatAvoidanceVector = myData.combineVectorForOnePoint2(myData.location, threatListWithoutSelf);
            //adHocThreatAvoidanceVector = myData.combineVectorForOnePoint2(myData.location, thisThreatList);
            //adHocThreatAvoidanceVector = (this.gameObject.transform.position + myData.pointAwayFromThreats(thisThreatList)).normalized;




            //now, some kind of sampling of points:
            //List<UnityEngine.Vector3> listOfSamplePoints = new List<UnityEngine.Vector3>();
            //listOfSamplePoints = theSensorySystem.returnPossibleNearMeshPoints();

            //      List<UnityEngine.Vector3> listOfSamplePoints = theSensorySystem.returnPossibleNearMeshPoints();
            //Debug.Log("listOfSamplePoints.Count:  " + listOfSamplePoints.Count);
            //foreach (UnityEngine.Vector3 aPoint in listOfSamplePoints)
            {
                //UnityEngine.Vector3 p1 = this.gameObject.transform.position;
                //UnityEngine.Vector3 p2 = aPoint;
                //Debug.DrawLine(p1, p2, new Color(0f, 0f, 1f), 1f);
            }



            //      spatialData1 myData = theSensorySystem.shooterThreatInfoSetBetterFunction(listOfSamplePoints, thisThreatList);

            //Debug.Log("myData.distancePointsSq.Count:  " + myData.distancePointsSq.Count);

            //Debug.Log("myData.thePoints.Count:  " + myData.thePoints.Count);

            //Debug.Log("myData.lookingAnglesSq.Count:  " + myData.lookingAnglesSq.Count);

            //myData.graphLineOfSightData1(this.gameObject.transform.position);
            //myData.graphLookingAnglesSq1(this.gameObject.transform.position);
            //myData.graphDistanceSq1(this.gameObject.transform.position);
            //      myData.pickBestPointFromData1();
            //myData.graphBetweenTwoPoints(this.gameObject.transform.position, myData.bestPoint[0]);

            //UnityEngine.Vector3 finalPoint = pickBestPointFromData1(myData);

            //super ad hoc test:
            //this.gameObject.transform.position = this.gameObject.transform.position + myData.bestPoint[0].normalized * 1;
            //      adHocThreatAvoidanceVector = myData.bestPoint[0].normalized;

            //myData.pickBestPointFromData1();

            //Debug.Log("myData.distancePointsSq.Count:  " + myData.distancePointsSq.Count);
            //myData.graphBetweenTwoPoints(this.gameObject.transform.position, myData.bestPoint[0]);


        }

    }


    












    public void enactNext()
    {
        if (printWTFThisNPCDoin)
        {
            Debug.Log("enactNext START");
            Debug.Log("currentPlan:  " + currentPlan);
            Debug.Log("currentPlan.Count:  " + currentPlan.Count);
        }
        //Debug.Log("enactNext START");

        //Debug.Log("currentPlan:  " + currentPlan);
        //Debug.Log("currentPlan.Count:  " + currentPlan.Count);
        if (currentPlan == null || currentPlan.Count == 0)
        {
            //Debug.Log("no plan for enaction phase");
            return; //no plan
        }

            if (currentPlan[0] != null)
        {

            //int shortPeriodicalEndpoint = 5;
            //int currentPeriodicalPoint = 0;


            //if (currentPlan[0].inTransit == true)


            //Debug.Log("currentPlan.Count:  " + currentPlan.Count);

            if (printWTFThisNPCDoin)
            {
                Debug.Log("currentPlan[0] != null, so go ahead and enact it");
            }

            //Debug.Log("currentPlan[0].enactThis:  " + currentPlan[0].enactThis);
            currentPlan[0].enact();
            if (printWTFThisNPCDoin)
            {
                Debug.Log("should be done enacting, deleteThisEnaction? " + currentPlan[0].deleteThisEnaction);
            }
            //triesBeforeDeletingClickActions
            if (currentPlan[0].deleteThisEnaction == true)
            {
                if (currentPlan[0].enactionTarget.name == "returnTestLOCK1(Clone)" && currentPlan[0].enactThis == "standardClick")
                {
                    //Debug.Log("currentPlan[0].deleteThisEnaction == true           for standardClick on returnTestLOCK1(Clone)");
                }

                if (printWTFThisNPCDoin)
                {
                    Debug.Log("yes");
                    Debug.Log("currentPlan.Count:  " + currentPlan.Count);
                }
                currentPlan.RemoveAt(0);
                currentPeriodicalPoint = 0;
                if (printWTFThisNPCDoin)
                {
                    Debug.Log("currentPlan.Count:  " + currentPlan.Count);
                }
            }
            else if (currentPlan[0].didATry == true)
            {
                currentPlan[0].didATry = false;
                currentTry++;
                if (currentTry == triesBeforeDeletingClickActionsEndPoint)
                {
                    if (currentPlan[0].enactionTarget.name == "returnTestLOCK1(Clone)" && currentPlan[0].enactThis == "standardClick")
                    {
                        //Debug.Log("currentTry == triesBeforeDeletingClickActionsEndPoint           for standardClick on returnTestLOCK1(Clone)");
                    }

                    currentTry = 0;
                    currentPlan.RemoveAt(0);
                    currentPeriodicalPoint = 0;

                }
                //int triesBeforeDeletingClickActionsEndPoint = 12;
                //int currentTry = 0;
            }
        }



        if (currentPeriodicalPoint == shortPeriodicalEndpoint)
        {
            currentPeriodicalPoint = 0;
            if (currentNavMeshAgent.velocity.sqrMagnitude == 0)
            {
                currentFramesNOTinTransit++;
                if (currentFramesNOTinTransit == framesNOTinTransitBeforeDumpingAction)
                {
                    currentPlan.RemoveAt(0);

                    currentFramesNOTinTransit = 0;
                    //currentPlan[0].deleteThisEnaction = true;
                }
            }
        }
        else
        {
            currentPeriodicalPoint++;
        }


        //Debug.Log("currentPlan:  " + currentPlan);
        //Debug.Log("currentPlan.Count:  " + currentPlan.Count);
        //Debug.Log("enactNext END");
        if (printWTFThisNPCDoin)
        {
            Debug.Log("enactNext END");
            Debug.Log("currentPlan:  " + currentPlan);
            Debug.Log("currentPlan.Count:  " + currentPlan.Count);
        }
    }


    public List<enactionMate> pickSomethingToInteractWithAndPlanToTryIt()
    {
        GameObject theTarget = semiRandomTargetPickerMZ();
        List<enactionMate> newPlan = new List<enactionMate>();

        if (theTarget == null)
        {
            return newPlan;
        }


        //it’s picking TARGETS
        //so, then pick an interaction ON that target
        //then plan a way to do that interaction


        //pick an interaction ON that target
        //i have "pickRandomInteractionONObject", but that is for old system.  need NEW system.....
        //what ARE the interactions now?  just strings?  yes, there is a dictionary with TWO strings.  one is interaction TYPE, the other is basically EFFECT
        //so right now i'm lookijng for interaction TYPES, not effects:
        string randomInteractionTypeOnTarget = NEWpickRandomInteractionONObject(theTarget);

        //      then plan a way to do that interaction.................
        
        
        //                  newPlan = planToDoInterActionWithObject(theTarget, randomInteractionTypeOnTarget);

        //Debug.Log("newPlan.Count:  " + newPlan.Count);

        return newPlan;
    }



    public string NEWpickRandomInteractionONObject(GameObject randomInteractionTarget)
    {
        //Debug.Log("looking at all available interactions on object");
        List<string> availableIntertactions = new List<string>();
        interactionScript anInteractionScript = randomInteractionTarget.GetComponent<interactionScript>();
        //Debug.Log("randomInteractionTarget:  " + randomInteractionTarget);
        //Debug.Log("anInteractionScript:  " + anInteractionScript);
        //Debug.Log("anInteractionScript.dictOfInteractions:  " + anInteractionScript.dictOfInteractions);
        //Debug.Log("anInteractionScript.dictOfInteractions.Keys:  " + anInteractionScript.dictOfInteractions.Keys);

        if (anInteractionScript == null)
        {
            //Debug.Log("anInteractionScript is null on randomInteractionTarget");
            return null;
        }


        foreach (string thisKey in anInteractionScript.dictOfInteractions.Keys)
        {
            //Debug.Log(thisKey);
            availableIntertactions.Add(thisKey);  //effing dictionary.Keys gives a weird data type, not a regular list, so i'm converting
        }




        if (availableIntertactions.Count > 0)
        {
            return availableIntertactions[UnityEngine.Random.Range(0, availableIntertactions.Count)];
        }
        else
        {
            //does this happen when an NPC try to interact with an NPC that is DEAD, has no interactions anymore?  hmmm, i dunno, i thought that only gets rid of their "aihub2", not interaction script, oddly enough.....
            Debug.Log("there are zero items on the interactions available list for this object, but it's supposed to have interactions:  " + randomInteractionTarget.name);
            Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, randomInteractionTarget.GetComponent<Transform>().position, Color.red, 0.1f);
            return null;
        }
    }




    public GameObject semiRandomTargetPickerMZ()
    {
        //theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(body.theLocalMapZoneScript.theList, this.gameObject);
        List<GameObject> potentialTargets = body.theLocalMapZoneScript.theList;
        //potentialTargets = theWorldScript.theTagScript.nearestXNumberOfYToZExceptYAndTheRemainder(4, potentialTargets, this.gameObject)[0];

        //GameObject theTarget = theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(body.theLocalMapZoneScript.theList, this.gameObject);

        //Debug.Log("theTarget:  " + theTarget);


        int randomIndex = UnityEngine.Random.Range(0, potentialTargets.Count);
        GameObject theTarget = potentialTargets[randomIndex];
        //GameObject theTarget = randomObjectFromList(listOfObjects);
        return theTarget;



        //Debug.Log("body.theLocalMapZoneScript:  " + body.theLocalMapZoneScript);
        //theWorldScript.theTagScript
        //List<GameObject> ALLTaggedWithMultiple
        //nearestXNumberOfYToZExceptYAndTheRemainder
        //randomObjectFromList
    }







    public enactionMate pickRandomEnactionONObject(GameObject objectWithEnactions)
    {
        List<enactionMate> theAvailableEnactions = objectWithEnactions.GetComponent<enactionScript>().availableEnactions;




        if (theAvailableEnactions.Count > 0)
        {
            return theAvailableEnactions[UnityEngine.Random.Range(0, theAvailableEnactions.Count)];
        }
        else
        {
            Debug.Log("there are zero items on the enactions available list for this object, but it's supposed to have enactions:  " + objectWithEnactions.name);
            Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, objectWithEnactions.GetComponent<Transform>().position, Color.red, 0.1f);
            return null;
        }
    }


















    public void printStringListWithPreface(string preface, List<string> theList)
    {
        //"preface" isn't quite the correct word.  that would imply it only appears once, at the start
        //instead, it appears before EVERY entry.
        //or could change it so i DO only print a preface at start. and maybe a dividine line at the end...

        foreach (string thisItem in theList)
        {
            Debug.Log(preface + thisItem);
        }
    }






    //          ARE THESE THE SAME?  REDUNDANT?
   






    public GameObject pickRandomNearbyInteractableObject()
    {
        //well this seems redundant:
        return pickRandomNearbyInteractable();
    }


    private GameObject pickRandomNearbyInteractable()
    {
        //grab all objects within some nearby range
        //from them, select those with appropriate tag
        //from those, select one at random


        //findSECONDNearestXToYExceptY
        //findXNearestToYExceptY

        //well, for now, skip the proximity condition complication:
        //return theWorldScript.theTagScript.randomTaggedWith("interactable");


        //return theWorldScript.theTagScript.findXNearestToYExceptY("interactable", this.gameObject);
        //pickRandomObjectFromListEXCEPT



        //GameObject thisSelection = theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable"), this.gameObject);
        GameObject thisSelection = theWorldScript.theTagScript.randomTaggedWithMultiple("interactable");


        //Debug.Log("ok it's '''randomly''' picking this object:  " + thisSelection.name);
        return thisSelection;

    }



}
