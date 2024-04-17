using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Threading;
using System.Xml.Linq;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.AI;
using static interactionEffects1;
using static UnityEngine.GraphicsBuffer;

public class AIHub2 : MonoBehaviour
{
    public bool printWTFThisNPCDoin = false;

    //public CharacterController AIcontroller;

    public UnityEngine.Vector3 adHocThreatAvoidanceVector = new UnityEngine.Vector3(0,0,0);



    private worldScript theWorldScript;
    public NavMeshAgent thisNavMeshAgent;
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


    public List<interactionMate> adhocPrereqFillerTest = new List<interactionMate>();


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
        this.gameObject.AddComponent<body1>();
        body = this.gameObject.GetComponent<body1>();
        //body = this.gameObject.AddComponent<body1>();

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


        thisNavMeshAgent = GetComponent<NavMeshAgent>();
        thisNavMeshAgent.speed = 13f;
        mytest = this.transform.position + new UnityEngine.Vector3(0, 0, -15);

        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;

        //body.pointerPoint = pointerPointToPutOnBody;


        //no, no, no, don't "get" component, CREATE it!
        //an AIHub2 should probably come with a sensory system by DEFAULT,
        //so why not build that into its initialization?
        /// theSensorySystem = myTest2.GetComponent("sensorySystem") as sensorySystem;
        this.gameObject.AddComponent<sensorySystem>();
        theSensorySystem = this.gameObject.GetComponent("sensorySystem") as sensorySystem;
        theSensorySystem.body = body;
        //same for planning:
        this.gameObject.AddComponent<planningAndImagination>();
        thePlanner = this.gameObject.GetComponent("planningAndImagination") as planningAndImagination;

        //inventory1
        this.gameObject.AddComponent<inventory1>();
        theInventory = this.gameObject.GetComponent("inventory1") as inventory1;

        theEnactionScript = this.gameObject.GetComponent<enactionScript>();

        if (true == false)
        {
            //this should be moved to a "regular human body" script:
            interactionEffects1 interactionScriptOnThisObject = this.gameObject.AddComponent<interactionEffects1>();
            //interactionScriptOnGeneratedObject.generateInteractionFULL("grabTestKey1", atomLister(atoms["grabTestKey1Atom"]));
            //interactionsAvailable
            //initialGenerator2
            initialGenerator2 theGeneratorScript = theWorldObject.GetComponent("initialGenerator2") as initialGenerator2;
            //foreach (string thisKey in theGeneratorScript.atoms.Keys)
            {
                //Debug.Log("keys:  " + thisKey);
            }
            interactionScriptOnThisObject.generateInteractionFULL("proximity0FillerInteraction1",
                    theGeneratorScript.atomLister(theGeneratorScript.atoms["proximity0Atom"]),
                    0
                    );
        }

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

        if (true == false)
        {
            //Debug.Log("==================AAAAAAAAAAAA====================     START update for:  " + this.gameObject.name + "     ==================AAAAAAAAA=====================");

            if (forgetfulnessTimer == 0)
            {
                //forget whole plan, start again:
                stringListToEnact = new List<string>();

                //reset timer:
                forgetfulnessTimer = 10;
            }
            else
            {
                forgetfulnessTimer -= 1;
            }


            if (stringListToEnact.Count == 0)
            {
                //need to randomly pick something to enact
                stringListToEnact.Add(pickRandomEnactionONObject(this.gameObject));
            }

            //then enact it:
            body.theEnactionScript.stringEnaction(stringListToEnact[0]);






            if (true == false)
            {

                //ad hoc plan/action switching in some cases for now:
                if (forgetfulnessTimer == 0)
                {
                    //forget whole plan, start again:
                    adhocPrereqFillerTest = new List<interactionMate>();

                    //reset timer:
                    forgetfulnessTimer = 10;
                }
                else
                {
                    forgetfulnessTimer -= 1;
                }








                //Debug.Log("11111111111111111111111111111111111111111111111111111");

                if (adhocPrereqFillerTest == null)
                {
                    //Debug.Log("''adhocPrereqFillerTest'' is null for this NPC:  " + this.gameObject.name);
                    //pickRandomNearbyInteractionAndTryIt();
                    newInteractionFunction();

                    //Debug.Log("WHERE TEH FUCK IS THIS HAPPENEINGGGGGGGGGGGG AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    //Debug.Log(adhocPrereqFillerTest);
                    //Debug.Log(adhocPrereqFillerTest[0]);      right, this will ALWAYS be null error here!  we just tested for that!


                }
                else if (adhocPrereqFillerTest.Count == 0)
                {
                    //Debug.Log("''adhocPrereqFillerTest'' is EMPTY for this NPC:  " + this.gameObject.name);
                    //pickRandomNearbyInteractionAndTryIt();
                    newInteractionFunction();

                    //Debug.Log("WHERE TEH FUCK IS THIS HAPPENEINGGGGGGGGGGGG BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
                    //Debug.Log(adhocPrereqFillerTest);
                    //Debug.Log(adhocPrereqFillerTest[0]);
                    //Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction);
                    //Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction.name);
                }
                else
                {
                    //Debug.Log("''adhocPrereqFillerTest'' has prereqFiller for the following NPC:  " + this.gameObject.name + ", so they should do them or plan to do them...");
                    //      ohhh this doesn't work.  the input to this function needs to be an OBJECT.
                    //      but what i have is a specific INTERACTION.
                    //      so, i need to grab what's needed, and maybe make a new function to do it properly
                    //      since the functions i made to find the interaction already presumably have access to the object, just modify those to return objects
                    //      then make an enaction "use" function that uses some interaction from an object.  somehow.  however that works.

                    //k i guess i sorta fixed that at some point?  i'm using an object now, right?  it works fine?
                    //Debug.Log(this.gameObject.name);
                    //Debug.Log(adhocPrereqFillerTest[0].interactionAuthor.name);

                    //interactionMate thisMate = new interactionMate();
                    //thisMate.interactionAuthor = this.gameObject;
                    //thisMate.target1 = randomInteractionTarget;

                    //tryRandomInteractionWithTarget(adhocPrereqFillerTest[0].target1, adhocPrereqFillerTest[0]);
                    //doThisInteractionOrPlanToDoIt(adhocPrereqFillerTest[0].forInteraction, adhocPrereqFillerTest[0]);
                    //adhocPrereqFillerTest = null;

                    //Debug.Log(adhocPrereqFillerTest);
                    //Debug.Log(adhocPrereqFillerTest[0]);
                    //Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction);
                    //Debug.Log(adhocPrereqFillerTest[0].enactThisInteraction.name);

                    //if (theWorldScript.theRespository.theInteractionEffects1.modularPrereqCheckBool(adhocPrereqFillerTest[0].enactThisInteraction, adhocPrereqFillerTest[0]))





                }


                Debug.Log("prereqs met?");
                if (adhocPrereqFillerTest[0].checkInteractionMatePrereqs())
                {
                    Debug.Log("yes");
                    //ad-hoc aim for now
                    //theInteractionMate.target1

                    //Debug.Log(body);

                    if (adhocPrereqFillerTest[0].target1 != null)
                    {
                        //adhocPrereqFillerTest[0].printMate();
                        //Debug.DrawLine(new Vector3(0, 0, 0), adhocPrereqFillerTest[0].target1.transform.position, Color.green, 0.1f);
                        //Debug.DrawLine(this.transform.position, (adhocPrereqFillerTest[0].target1.transform.position - this.transform.position), Color.blue, 0.1f);
                        body.lookingRay = new Ray(this.transform.position, (adhocPrereqFillerTest[0].target1.transform.position - this.transform.position));
                        //Debug.DrawRay(this.transform.position, (adhocPrereqFillerTest[0].target1.transform.position - this.transform.position), Color.blue, 0.1f);

                        //Debug.Log();
                        //adhocPrereqFillerTest[0].printMate();

                        adhocPrereqFillerTest[0].doThisInteraction();
                        //randomInteractionTarget = null;
                    }
                    else
                    {
                        Debug.Log("this has no target:  " + adhocPrereqFillerTest[0].enactThisInteraction.name);
                        giveItARandomInteractableTarget(adhocPrereqFillerTest[0]);
                        //adhocPrereqFillerTest[0].printMate();
                        //Debug.DrawLine(new Vector3(0,0,0), adhocPrereqFillerTest[0].target1.transform.position, Color.green, 0.1f);

                        //Debug.DrawLine(this.transform.position, (adhocPrereqFillerTest[0].target1.transform.position - this.transform.position), Color.yellow, 0.1f);
                        body.lookingRay = new Ray(this.transform.position, (adhocPrereqFillerTest[0].target1.transform.position - this.transform.position));
                        //Debug.DrawRay(this.transform.position, (adhocPrereqFillerTest[0].target1.transform.position - this.transform.position), Color.yellow, 0.1f);

                        adhocPrereqFillerTest[0].doThisInteraction();

                    }

                }


                //goToWhicheverAvailableTarget();

            }


            //Debug.Log("-----------////////////////////////-----------     END update for:  " + this.gameObject.name + "     -------------////////////////////////----------");
        }

    }

    public List<GameObject> threatListWithoutSelf()
    {
        List<GameObject> threatListWithoutSelf = new List<GameObject>();
        List<GameObject> thisThreatList = body.theLocalMapZoneScript.threatList;

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
        return threatListWithoutSelf;
    }



    public void navmeshStopping1()
    {

        if (myDelay == 0)
        {
            thisNavMeshAgent.acceleration = 8;
            thisNavMeshAgent.speed = 7;
            myDelay = 2;
        }
        else if (myDelay == 4)
        {
            thisNavMeshAgent.ResetPath();
            thisNavMeshAgent.isStopped = true;
            myDelay = 0;
        }
        myDelay--;

    }

    public void navmeshSpeeding1()
    {
        thisNavMeshAgent.isStopped = false;
        myDelay = 6;
        thisNavMeshAgent.acceleration = 180;
        thisNavMeshAgent.speed = 70;
    }

    public void callableUpdate()
    {

        if (cooldownTimer > 11 && theEnactionScript.availableEnactions.Contains("shoot1"))
        {
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
        thisNavMeshAgent.SetDestination(newFinalPosition);//for some reason it doesn't work if you put the *8f here, even though it works in the comparable "SetDestination" line above, so i had to put that into the "newFinalPosition" variable....


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
            List<GameObject> threatListWithoutSelf = new List<GameObject>();
            foreach (GameObject threat in thisThreatList)
            {
                //UnityEngine.Vector3 p1 = this.gameObject.transform.position;
                //UnityEngine.Vector3 p2 = threat.gameObject.transform.position;
                //Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 1f);
                if(threat != null && threat != this.gameObject)
                {
                    threatListWithoutSelf.Add(threat);
                }
            }


            spatialData1 myData = new spatialData1();
            //combineVectorForOnePoint2
            myData.location = this.gameObject.transform.position;
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
                graphCooldown = 0;
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
            if (thisNavMeshAgent.velocity.sqrMagnitude == 0)
            {
                currentFramesNOTinTransit++;
                if (currentFramesNOTinTransit == framesNOTinTransitBeforeDumpingAction)
                {

                    if (currentPlan[0].enactionTarget.name == "returnTestLOCK1(Clone)" && currentPlan[0].enactThis == "standardClick")
                    {
                        //Debug.Log("currentFramesNOTinTransit == framesNOTinTransitBeforeDumpingAction           for standardClick on returnTestLOCK1(Clone)");
                    }

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
        GameObject theTarget = semiRandomTargetPicker();
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
        newPlan = planToDoInterActionWithObject(theTarget, randomInteractionTypeOnTarget);

        //Debug.Log("newPlan.Count:  " + newPlan.Count);

        return newPlan;
    }


    public List<enactionMate> planToDoInterActionWithObject(GameObject theTarget, string randomInteractionTypeOnTarget)
    {
        List<enactionMate> newPlan = new List<enactionMate>();

        //so, planning should depend on which type of interaction, and context [and any other criteria, etc.]
        //      wait a second, i have some things conflated!
        //      interaction script is currently putting EVERYTHING under the "interactionType1" category
        //      clicking key, dieing etc, all lumped together.
        //need to sort out:
        //      1) different interaction types [heat is different from being pushed, which is different from contact with a sharp object]
        //      2) the EFFECTS of those types [could hypothetically make fire cause burning/damage, water puts it out]
        //      3) but i want to SIMPLIFY with "ctandardClick".  it automatically picks some specific interaction, EVEN THOUGH in real life they would be quite different
        //soooooo, sort that out.....................
        //well, for now, let's keep it at "simplified interface" level.  ALL WE CARE ABOUT is game descision-making
        //that means, for interactions, we lump "standardClick" together.
        //so "interactionType1" probably shouldn't exist?  except as a useless shell i ignore all of the time.
        //but "clcikLock"???? that isn't an interaction type!  it's enaction!  i tried splitting interaction from enaction, and apparently FAILED?
        //i don't know, where the fuck is "standardClick"??????  i'm not seeing it there ANYWHERE????  i guess it's an ENACTION now?  ok....uhhhhh ok.....
        //but i need a way to separate "click on this" from "shoot this with a gun".  where is that distinction?  it's somehow only appears in ENACTION, i think?  or who knows.
        //what a mess.
        //so, i need two things.  somewhere , somehow:
        //      things you will need to click on
        //      things you will need to shoot with gun
        //and this difference should obviously be stored in their fucking interaction scripts!  but be "legible" to planning!
        //i THINK this is supposed to be the KEY of the dictionary i have in there.  just print those out and see what they are?
        //yes, that's what it is.  i see printouts of "standardClick" and "bullet1".  precisely.
        //so i can use those, for now.

        //Debug.Log("randomInteractionTypeOnTarget:  " + randomInteractionTypeOnTarget);
        if (randomInteractionTypeOnTarget == "standardClick")
        {
            //      [OLD ad hoc stuff making them ONLY go and do "Standard click" on things]
            //how to know if it has a proximity "prereq"?
            //for now, don't know.  just click POINTING at that?
            //whatever, very ad-hoc for now:
            if (theWorldScript.theTagScript.distanceBetween(theTarget, this.gameObject) > (body.standardClickDistance) * 0.7f)
            {
                //so, plan to walk there FIRST, thennnn click
                newPlan.Add(makeSimpleEnactionMate("navMeshWalk", theTarget));
                newPlan.Add(makeSimpleEnactionMate("aim", theTarget));
                newPlan.Add(makeSimpleEnactionMate("standardClick", theTarget));
            }
            else
            {
                //so, easy, just click it
                newPlan.Add(makeSimpleEnactionMate("aim", theTarget));
                newPlan.Add(makeSimpleEnactionMate("standardClick", theTarget));
            }

        }
        //else if (randomInteractionTypeOnTarget == "bullet1")
        else if (randomInteractionTypeOnTarget == "shoot1")
        {
            //do they have ability to shoot?  super hand-crafted ad-hoc....
            //enactionScript theEnactionScript = this.gameObject.GetComponent<enactionScript>();
            //theEnactionScript.availableEnactions.Add("shoot1");

            //Debug.Log("do they have shoot1?");
            if (theEnactionScript.availableEnactions.Contains("shoot1"))
            {
                //Debug.Log("yes they have shoot1");
                newPlan.Add(makeSimpleEnactionMate("aim", theTarget));
                //newPlan.Add(makeSimpleEnactionMate("shoot", theTarget));
                newPlan.Add(makeSimpleEnactionMate("shoot1", theTarget));

                //Debug.Log("newPlan[0].enactThis:  " + newPlan[0].enactThis);
                //Debug.Log("newPlan[1].enactThis:  " + newPlan[1].enactThis);
            }
            else
            {
                //Debug.Log("no they don't have shoot1");
            }

        }


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
            Debug.Log("anInteractionScript is null on randomInteractionTarget");
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



    public enactionMate makeSimpleEnactionMate(string enactThis, GameObject enactionTarget)
    {
        //should move this function to enaction script, so player can use it etc.
        enactionMate newEnactionMate = new enactionMate();
        newEnactionMate.enactionAuthor = this.gameObject;
        newEnactionMate.enactionBody = body;
        newEnactionMate.enactThis = enactThis;
        newEnactionMate.enactionTarget = enactionTarget;

        return newEnactionMate;
    }


    public GameObject semiRandomTargetPicker()
    {
        //List<GameObject> potentialTargets = theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable");

        List<List<GameObject>> ALLpotentialTargetsSORTED = theWorldScript.theTagScript.nearestXNumberOfYToZExceptYAndTheRemainder(4, body.theLocalMapZoneScript.theList, this.gameObject);
        //theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(body.theLocalMapZoneScript.theList, this.gameObject);
        List<GameObject> potentialTargets = new List<GameObject>();
        //potentialTargets = theWorldScript.theTagScript.nearestXNumberOfYToZExceptYAndTheRemainder(4, potentialTargets, this.gameObject)[0];

        //just to allow a far away one sometimes:

        //potentialTargets.Add(theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(potentialTargets, this.gameObject));
        potentialTargets.Add(theWorldScript.theTagScript.randomObjectFromList(ALLpotentialTargetsSORTED[0]));
        potentialTargets.Add(theWorldScript.theTagScript.randomObjectFromList(ALLpotentialTargetsSORTED[0]));
        //potentialTargets.Add(theWorldScript.theTagScript.randomObjectFromList(ALLpotentialTargetsSORTED[1]));
        potentialTargets.Add(theWorldScript.theTagScript.randomObjectFromList(ALLpotentialTargetsSORTED[1]));
        potentialTargets.Add(theWorldScript.theTagScript.randomObjectFromList(ALLpotentialTargetsSORTED[1]));
        GameObject theTarget = theWorldScript.theTagScript.randomObjectFromList(potentialTargets);
        //GameObject theTarget = theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(body.theLocalMapZoneScript.theList, this.gameObject);

        //Debug.Log("theTarget:  " + theTarget);
        

        int loopTries = 4;
        while(loopTries > 0)
        {
            loopTries--;
            if (theTarget == null)
            {
                //Debug.Log("this is null:  " + theTarget);
                return null;
            }
            else
            {
                //Debug.Log("this is NOT null:  " + theTarget);
            }
            //Debug.Log("loopTries:  " + loopTries);
            //Debug.Log("theTarget:  " + theTarget);
            //Debug.Log("theTarget:  " + theTarget.GetComponent<interactionScript>());
            interactionScript anInteractionScript = theTarget.GetComponent<interactionScript>();
            if( anInteractionScript != null )
            {
                break;
            }
            else
            {
                theTarget = theWorldScript.theTagScript.randomObjectFromList(potentialTargets);
            }
        }



        return theTarget;


        //Debug.Log("body.theLocalMapZoneScript:  " + body.theLocalMapZoneScript);
        //theWorldScript.theTagScript
        //List<GameObject> ALLTaggedWithMultiple
        //nearestXNumberOfYToZExceptYAndTheRemainder
        //randomObjectFromList
    }



    void justPickAnAbilityAndFireIt()
    {
        if (stringListToEnact.Count == 0)
        {
            //need to randomly pick something to enact
            stringListToEnact.Add(pickRandomEnactionONObject(this.gameObject));
        }

        //then enact it:
        body.theEnactionScript.stringEnaction(stringListToEnact[0]);

    }






    public string pickRandomEnactionONObject(GameObject objectWithEnactions)
    {
        List<string> theAvailableEnactions = objectWithEnactions.GetComponent<enactionScript>().availableEnactions;




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















    private void giveItARandomInteractableTarget(interactionMate interactionMate)
    {
        //[besides itself]
        interactionMate.target1 = theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable"), this.gameObject);


    }

    private void newInteractionFunction()
    {
        //Debug.Log("222222222222222222222222222222222222222222222222222222222");
        //pick random nearby interactable
        //if it's not themselves, "try" the standard "click" interaction aimed at it
        //if it IS themselves, should be walk function, pick a random TARGET and walk to it [i already have this working]

        GameObject randomInteractableObject = pickRandomNearbyInteractableObject();

        if (randomInteractableObject != null)
        {
            //Debug.Log("33333333333333333333333333333333333333333333333333333");
            if (randomInteractableObject.name == this.gameObject.name)
            {
                //Debug.Log("4444444444444444444444444444444444444444444444");
                //if it IS themselves, should be walk function, pick a random TARGET and walk to it [i already have this working] = "walkSomewhere"
                interactionMate newMate = new interactionMate();

                newMate.interactionAuthor = this.gameObject;

                //ad-hoc for now:
                testInteraction theInteractionToEnact = pickRandomInteractionONObject(randomInteractableObject);
                newMate.enactThisInteraction = theInteractionToEnact;


                //super ad-hoc nonsense for now:
                if (theInteractionToEnact.name == "walkSomewhere")
                {
                    Debug.Log("55555555555555555555555555555555555555555555555555");
                    //literally just look for a random interactable BESIDES themselves to walk to:
                    //theWorldScript.taggedStuff.ALLTaggedWithMultiple("intera");
                    List<GameObject> allObjectsThatAreInteractable = theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable");
                    newMate.target1 = theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(allObjectsThatAreInteractable, this.gameObject);
                }


                adhocPrereqFillerTest.Add(newMate);

            }
            else
            {
                //if it's not themselves, "try" the standard "click" interaction aimed at it
                //      need to AIM [make a function that automatically aligns their body's "ray" at the target object?  sure, for now
                //      then need to do:  "standardInteraction1"


                string nameOfCurrentClickInteraction = "standardInteraction1";
                interactionMate theInteractionMate = new interactionMate();
                theInteractionMate.interactionAuthor = this.gameObject;
                theInteractionMate.target1 = randomInteractableObject;


                //      "interactionScript" is no longer defined
                //theInteractionMate.enactThisInteraction = body.interactionScript.interactionDictionary["doARegularClick"];





                //theInteractionMate.enactThisInteraction.doInteraction(theInteractionMate);

                //Debug.Log(adhocPrereqFillerTest);

                adhocPrereqFillerTest.Add(theInteractionMate);


            }
        }


    }

    public interactionMate pickRandomNearbyInteractionReturnMate()
    {
        GameObject randomInteractableObject = pickRandomNearbyInteractableObject();

        testInteraction thisInteraction = pickRandomInteractionONObject(randomInteractableObject);

        interactionMate thisMate = createBasicInteractionMate(this.gameObject, randomInteractableObject, thisInteraction);


        return thisMate;
    }

    public interactionMate createBasicInteractionMate(GameObject author, GameObject target1, testInteraction theInteractionToEnact)
    {
        //move this to interactionEffects1?

        interactionMate newMate = new interactionMate();

        newMate.interactionAuthor = author;
        newMate.target1 = target1;
        newMate.enactThisInteraction = theInteractionToEnact;


        //super ad-hoc nonsense for now:
        if (theInteractionToEnact.name == "walkSomewhere")
        {
            //literally just look for a random interactable BESIDES themselves to walk to:
            //theWorldScript.taggedStuff.ALLTaggedWithMultiple("intera");
            List<GameObject> allObjectsThatAreInteractable = theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable");
            newMate.target1 = theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(allObjectsThatAreInteractable, this.gameObject);
        }


        return newMate;
    }


    public void pickRandomNearbyInteractionAndTryIt()
    {
        //step 1:  find some nearby interactions, and randomly pick one
        //step 2:  "try" it [not necessarily succeed, though i'm not sure how failure will work yet]

        interactionMate thisMate = pickRandomNearbyInteractionReturnMate();

        List<interactionMate> listOfActions = planForPrereqsIfNeeded(thisMate);

        if (areThereAnyErrorsWithThisListOfActions(listOfActions) == false)
        {
            listOfActions[0].doThisInteraction();
        }

    }




    public bool areThereAnyErrorsWithThisListOfActions(List<interactionMate> listOfActions)
    {
        //returns TRUE if there's an ERROR

        if (listOfActions == null)
        {
            Debug.Log("apparently we don't have a plan to fill the unfilled prereq here  (it's NULL)");
            return true;
        }
        else if (listOfActions.Count == 0)
        {
            Debug.Log("apparently we don't have a plan to fill the unfilled prereq here (it's count is ZERO)");
            return true;
        }
        else if (listOfActions[0].enactThisInteraction == null)
        {
            Debug.Log("the first item in the prereq filling plan has the ''enactThisInteraction'' variable = NULL");
            return true;
        }

        return false;
    }


    public List<interactionMate> planForPrereqsIfNeeded(interactionMate thisMate)
    {
        //if prereqs are met, do it
        //if not, plan to fill them

        List<interactionMate> thePlan = new List<interactionMate>();
        testInteraction thisInteraction = thisMate.enactThisInteraction;

        //if (theWorldScript.theRespository.theInteractionEffects1.modularPrereqCheckBool(thisInteraction, thisMate))

        if (thisMate.checkInteractionMatePrereqs())
        {
            //Debug.Log("prereqs met for this interaction:  " + thisInteraction.name);

            thePlan.Add(thisMate);
            return thePlan;
        }
        else
        {
            //Debug.Log("prereqs NOT met for this interaction:  " + thisInteraction.name);



            //      THIS IS PROBABLY REDUNDANT NOW, JUST USE THE MATE I ALREADY HAVE
            //thisMate.printMate();
            interactionMate newMate = new interactionMate();
            newMate.interactionAuthor = this.gameObject;
            newMate.target1 = thisMate.target1;

            return returnPlanToFillPrereqs(thisInteraction, newMate);
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
    public List<interactionMate> returnPlanToFillPrereqs(testInteraction theInteraction, interactionMate thisMate)
    {
        //so.  prereqs are not filled.
        //so how to fill them?
        //"look around" in environment [tags, actually] for objects with interactions that can fulfil it.
        //so:
        //      need to know WHICH prereq(s) is/are not filled
        //      need to fill it/each
        //      have a plan somewhere so it can be DONE next frame.....[or i just do it THIS frame for now?]


        //which prereqs are not filled:
        List<string> unfilledPrereqs = theInteraction.returnAllUnfilledPrereqs(thisMate);

        //how to fill each:
        List<interactionMate> prereqFillers = returnPlansToFillPrereqs(unfilledPrereqs, thisMate);

        return prereqFillers;
    }

    public List<interactionMate> returnPlansToFillPrereqs(List<string> unfilledPrereqs, interactionMate thisMate)
    {
        List<interactionMate> prereqFillers = new List<interactionMate>();

        //should be able to check just the string to see if other available interactions in the area have that effect?
        //are they in legibstrata?  or what?
        //for ALL [or until a suitable one is found?] interactions "in the area" [everywhere for now][so, find them ALL with "interactable" tag, then iterate through their availableInteractions]
        //      see if this interaction can fulfill the specific prereq
        //      if so, for now just return the FIRST one that works?

        foreach (string thisUnfilledPrereq in unfilledPrereqs)
        {
            //doesn't need objects or context???  well, for now?  context will be found by the tagging system inside this next function, and perhaps that can be specified from outside of it.  but for now just ALL
            prereqFillers.AddRange(returnPlansToFillONEPrereq(thisUnfilledPrereq, thisMate));
        }




        return prereqFillers;
    }









    public GameObject pickRandomNearbyInteractableObject()
    {
        //well this seems redundant:
        return pickRandomNearbyInteractable();
    }

    public testInteraction pickRandomInteractionONObject(GameObject randomInteractionTarget)
    {
        List<testInteraction> availableIntertactions = randomInteractionTarget.GetComponent<interactionEffects1>().interactionsAvailable;




        if (availableIntertactions.Count > 0)
        {
            return availableIntertactions[UnityEngine.Random.Range(0, availableIntertactions.Count)];
        }
        else
        {
            Debug.Log("there are zero items on the interactions available list for this object, but it's supposed to have interactions:  " + randomInteractionTarget.name);
            Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, randomInteractionTarget.GetComponent<Transform>().position, Color.red, 0.1f);
            return null;
        }
    }






    public List<interactionMate> returnPlansToFillONEPrereq(string unfilledPrereq, interactionMate thisMate)
    {
        List<interactionMate> prereqFillers = new List<interactionMate>();

        prereqFillers.AddRange(privatePrereqFillers(unfilledPrereq, thisMate));
        prereqFillers.AddRange(onPersonPrereqFillers(unfilledPrereq, thisMate));

        if (prereqFillers == null || prereqFillers.Count == 0)
        {
            prereqFillers.AddRange(worldPrereqFillers(unfilledPrereq, thisMate));
        }

        return prereqFillers;
    }

    private List<interactionMate> privatePrereqFillers(string unfilledPrereq, interactionMate thisMate)
    {
        List<interactionMate> prereqFillers = new List<interactionMate>();



        testInteraction isThereAnInteraction = theWorldScript.theRespository.theInteractionEffects1.returnFirstPrivateInteractionOnObjectWithThisEffect(unfilledPrereq, this.gameObject);
        if (isThereAnInteraction != null)
        {
            prereqFillers.Add(basicPackup(isThereAnInteraction, thisMate));
        }

        return prereqFillers;
    }

    private List<interactionMate> onPersonPrereqFillers(string unfilledPrereq, interactionMate thisMate)
    {
        //this will be for stuff like INVENTORIES, which i don't have totally implemented yet!
        //technically the "key" currently goes in inventory, but the lock just works if the key is there, no actual "interaction" has to happen between lock and key.
        //i guess i should change that!  key should "unlock' the lock, and then a SEPARATE interaction should "open" it.



        List<interactionMate> prereqFillers = new List<interactionMate>();

        return prereqFillers;
    }

    private List<interactionMate> worldPrereqFillers(string unfilledPrereq, interactionMate thisMate)
    {
        //currently returns "first" prereq filler, not ALL possible ones

        List<interactionMate> prereqFillers = new List<interactionMate>();

        //this is HUGELY computationally wasteful, and the legibstrata should have a dictionary to EASILY look up ONLY the objects etc that can fulfill a SPECIFIC prereq!
        List<GameObject> allObjectsThatAreInteractable = theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable");

        foreach (GameObject thisObject in allObjectsThatAreInteractable)
        {
            //Debug.Log("does this object fulfill prereq we are looking for?:  " + thisObject.name);
            //if (theWorldScript.theRespository.theInteractionEffects1.doesThisObjectHaveThisEffect(unfilledPrereq, thisObject) == true)
            testInteraction isThereAnInteraction = theWorldScript.theRespository.theInteractionEffects1.returnFirstInteractionOnObjectWithThisEffect(unfilledPrereq, thisObject);
            if (thisObject != this.gameObject && isThereAnInteraction != null)
            {
                prereqFillers.Add(basicPackup(isThereAnInteraction, thisMate));
            }
        }

        return prereqFillers;
    }

    private interactionMate basicPackup(testInteraction isThereAnInteraction, interactionMate thisMate)
    {

        //interactionMate
        interactionMate newMate = new interactionMate();
        newMate.interactionAuthor = this.gameObject;
        newMate.target1 = thisMate.target1;
        newMate.enactThisInteraction = isThereAnInteraction;

        return newMate;
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
