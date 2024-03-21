using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
//using System.Drawing;
//using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
using static UnityEngine.GraphicsBuffer;

public class sensorySystem : MonoBehaviour
{
    private worldScript theWorldScript;
    public GameObject target;

    List<GameObject> sensedObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("==================== START OF SENSORY UPDATE ======================");
        target = theWorldScript.theTagScript.findXNearestToYExceptY("interactionEffects1", this.gameObject);
        //Debug.Log(target);
        //Debug.Log(theWorldScript);
        if (target != null && theWorldScript.isThereAParentChildRelationshipHere(target, this.gameObject))
        {

            target = theWorldScript.theTagScript.findSECONDNearestXToYExceptY("interactionEffects1", this.gameObject);
        }
       

        //Debug.Log(target);
        //Debug.Log(target.name);
        if(target != null)
        {
            Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, target.GetComponent<Transform>().position, Color.white, 0.1f);
        }
        
        //theWorldScript.theTagScript.findNearestX("interactive1");
        foreach (GameObject obj in sensedObjects)
        {
            //Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, obj.gameObject.GetComponent<Transform>().position, Color.white, 0.1f);
        }


        //Debug.Log("xxxxxxxxxxxxxxxxxxx END of sensory update xxxxxxxxxxxxxxxxxxxxx");
    }




    private void OnTriggerEnter(Collider other)
    {
        //Debug.DrawRay(this.gameObject.GetComponent<Transform>().position, newVector, Color.white, 0.06f);
        //Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, other.gameObject.GetComponent<Transform>().position, Color.white, 6f);
        sensedObjects.Add(other.gameObject);
    }



    public GameObject whichObjectOnListIsNearest(List<GameObject> listOfObjects)
    {
        //closest to what?  to THIS object, i suppose

        GameObject theClosestSoFar = null;

        foreach (GameObject thisObject in listOfObjects)
        {
            if (theClosestSoFar != null)
            {
                float distanceToThisObject = Vector3.Distance(thisObject.transform.position, this.gameObject.transform.position);
                float distanceToTheClosestSoFar = Vector3.Distance(theClosestSoFar.transform.position, this.gameObject.transform.position);

                if (distanceToThisObject < distanceToTheClosestSoFar)
                {
                    theClosestSoFar = thisObject;
                }
            }
            else
            {
                theClosestSoFar = thisObject;
            }
        }

        return theClosestSoFar;
    }

    public List<Vector3> returnPossibleNearMeshPoints()
    {
        List<Vector3> listOfNearMeshPoints = new List<Vector3>();

        List<Vector3> just8PointsAroundMiddle = new List<Vector3>();
        just8PointsAroundMiddle = justReturn8PointsAroundMiddle(this.gameObject.transform.position);

        //Debug.Log("just8PointsAroundMiddle.Count:  " + just8PointsAroundMiddle.Count);

        foreach (Vector3 thisPoint in just8PointsAroundMiddle)
        {
            //if they are "on" the navmesh [or near enough], add them to the "listOfNearMeshPoints"
            NavMeshHit hit;
            //Debug.Log("is this point on navmesh?");
            if (NavMesh.SamplePosition(thisPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                //Debug.Log("no, this point is NOT on navmesh");
                listOfNearMeshPoints.Add(thisPoint);
            }
            else
            {
                //Debug.Log("yes, this point is on navmesh");
            }
        }


        //Debug.Log("listOfNearMeshPoints.Count:  " + listOfNearMeshPoints.Count);

        return listOfNearMeshPoints;
    }

    public List<Vector3> justReturn8PointsAroundMiddle(Vector3 theMiddlePoint)
    {
        List<Vector3> just8PointsAroundMiddle = new List<Vector3>();

        //soooo:
        //-1x +1z       0x +1z      +1x +1z
        //
        //-1x 0z       [N/A]      +1x 0z
        //
        //-1x -1z       0x -1z      +1x -1z

        int theXValue = -1;
        int theYValue = -1;
        while(theXValue < 2)
        {
            //while (theYValue < 2)
            {
                //Vector3 newPoint = theMiddlePoint + new Vector3(theXValue, 0, theYValue);
                //just8PointsAroundMiddle.Add(newPoint);
                //theYValue++;
            }

            Vector3 newPoint = theMiddlePoint + new Vector3(theXValue, 0, theYValue);
            just8PointsAroundMiddle.Add(newPoint);
            theYValue++;
            Vector3 newPoint2 = theMiddlePoint + new Vector3(theXValue, 0, theYValue);
            just8PointsAroundMiddle.Add(newPoint2);
            theYValue++;
            Vector3 newPoint3 = theMiddlePoint + new Vector3(theXValue, 0, theYValue);
            just8PointsAroundMiddle.Add(newPoint3);
            theYValue++;

            theYValue = -1;
            theXValue++;
        }



        return just8PointsAroundMiddle;
    }




    public spatialData1 shooterThreatInfoSet(List<Vector3> setOfPoints, Vector3 goalPoint, GameObject theThreat)
    {
        spatialData1 gatheredData = new spatialData1();

        foreach(Vector3 aPoint in setOfPoints)
        {
            // distance to threat
            // line of sight to threat
            // angle between where shooter is facing and where the point is
            // proximity to end goal

            distanceData1(aPoint, theThreat, gatheredData);
            lineOfSightData1(aPoint, theThreat, gatheredData);
            lookingAngleData1(aPoint, theThreat, gatheredData);
            distancePointsData1(aPoint, goalPoint, gatheredData);

        }

        return gatheredData;
    }

    public spatialData1 shooterThreatInfoSetBetterFunction(List<Vector3> setOfPoints, List<GameObject> theThreatList)
    {
        spatialData1 gatheredData = new spatialData1();

        gatheredData.numberOfThreats = theThreatList.Count;
        gatheredData.numberOfSamplePoints = setOfPoints.Count;
        gatheredData.thePoints = setOfPoints;

        foreach (GameObject aThreat in theThreatList )
        {
            foreach (Vector3 aPoint in setOfPoints)
            {
                // distance to threat
                // line of sight to threat
                // angle between where shooter is facing and where the point is
                // proximity to end goal

                distanceData1(aPoint, aThreat, gatheredData);
                lineOfSightData1(aPoint, aThreat, gatheredData);
                lookingAngleData1(aPoint, aThreat, gatheredData);
                //distancePointsData1(aPoint, goalPoint, gatheredData);

            }
        }
        

        return gatheredData;
    }





    public void distancePointsData1(Vector3 aPoint, Vector3 goalPoint, spatialData1 gatheredData)
    {
        float halfSquaredThing = aPoint.sqrMagnitude - goalPoint.sqrMagnitude;
        gatheredData.distanceToGoalDataSq.Add(halfSquaredThing* halfSquaredThing);
    }

    public void lookingAngleData1(Vector3 aPoint, GameObject theThreat, spatialData1 gatheredData)
    {
        //need the direction the threat is LOOKING
        //Ray threatLookingRay = theThreat.GetComponent<body1>().lookingRay;
        //or just get their transform rotation?  sure
        Quaternion QUATERNIONthreatLookingRotation = theThreat.transform.rotation;
        float threatLookingRotation = oneDamnQUATERNIONrotation(QUATERNIONthreatLookingRotation);
        float angleOfAPoint = oneDamnAngle(aPoint);
        float theAngleHalfSquared = ((threatLookingRotation * threatLookingRotation) - (angleOfAPoint* angleOfAPoint));
        gatheredData.lookingAnglesSq.Add(theAngleHalfSquared* theAngleHalfSquared);
    }

    public float oneDamnAngle(Vector3 oneVector)
    {
        float oneAngle = Vector3.Angle(oneVector, new Vector3(0,0,0));
        return oneAngle;
    }

    public float oneDamnQUATERNIONrotation(Quaternion oneQuaternion)
    {
        float oneAngle = Quaternion.Angle(oneQuaternion, Quaternion.identity);
        return oneAngle;
    }

    public void lineOfSightData1(Vector3 aPoint, GameObject theThreat, spatialData1 gatheredData)
    {
        bool theBool = false;
        RaycastHit myHit;

        //new Ray(this.transform.position, theBody.theWorldScript.theTagScript.semiRandomUsuallyNearTargetPickerFromList(theBody.theLocalMapZoneScript.theList, this.gameObject).transform.position);
        Vector3 theDirection = theThreat.transform.position - aPoint;
        Ray myRay = new Ray(aPoint, theDirection);
        ;

        if (Physics.Raycast(myRay, out myHit, 20f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            theBool = true;
            //if (myHit.transform != null)
            {

            }
        }

        gatheredData.lineOfSightData.Add(theBool);
    }

    public void distanceData1(Vector3 aPoint, GameObject theThreat, spatialData1 gatheredData)
    {
        //Debug.Log("?????????????????????????????????????????????????????????????????????????");
        float halfSquaredThing = aPoint.sqrMagnitude - theThreat.transform.position.sqrMagnitude;
        gatheredData.distanceToGoalDataSq.Add(halfSquaredThing * halfSquaredThing);
    }
}

public class spatialData1
{
    //using square magnitude on stuff like distances, i suppose
    //and they are lumped together.  does each list do every point for one threat, then move on to next threat?
    //or ever threat for one point, then move onto next point?
    //the former.  for one threat, all points.  then next threat.
    public List<float> distancePointsSq = new List<float>();
    public List<float> distanceSamples = new List<float>();
    public List<float> lookingAnglesSq = new List<float>();
    public List<float> lookingAngleSamples = new List<float>();
    public List<bool> lineOfSightData = new List<bool>();  //a LIST of BOOL....i don't think i've ever seen such a thing
    public List<float> distanceToGoalDataSq = new List<float>();

    public List<float> combinedMeasures = new List<float>();

    //ya, the data is all combined together,
    //the following two integers basically tells you how many rows and columns there are so you can separate it easily:
    public int numberOfThreats = 0;
    public int numberOfSamplePoints = 0;
    public List<Vector3> thePoints = new List<Vector3>();

    int cooldown = 0;

    public List<Vector3> bestPoint = new List<Vector3>(); //yes, it's a list, but a vector can't be null, so need a way to have none
    public Vector3 location;


    public void NEWgatherALLThreatDataTypes(List<GameObject> theThreatList)
    {
        if (thePoints.Count == 0)
        {
            generateValidNearPoints1(location);
        }


        numberOfThreats = theThreatList.Count;
        numberOfSamplePoints = thePoints.Count;


        //aquireDistanceSamples(theThreatList);


        foreach (Vector3 aPoint in thePoints)
        {
            float currentAddedDistance = 0f;
            float currentAddedAngles = 0f;
            foreach (GameObject thisThreat in theThreatList)
            {

                // distance to threat
                // line of sight to threat
                // angle between where shooter is facing and where the point is
                // proximity to end goal

                //distanceData1(aPoint, aThreat);
                //lineOfSightData1(aPoint, aThreat);
                //lookingAngleData1(aPoint, aThreat);
                //distancePointsData1(aPoint, goalPoint, gatheredData);
                currentAddedDistance += aquireONEDistanceSampleOnePointOneThreat(aPoint, thisThreat);
                currentAddedAngles += NEWaquireONEAngleSampleOnePointONeThreat(aPoint, thisThreat);
            }
            distanceSamples.Add(currentAddedDistance);
            lookingAngleSamples.Add(currentAddedAngles / (180 * theThreatList.Count));
        }

    }

    public float NEWaquireONEAngleSampleOnePointONeThreat(Vector3 thePoint, GameObject theThreat)
    {
        Ray threatLookingRay = theThreat.GetComponent<body1>().lookingRay;
        Vector3 lineBetweenThreatAndPoint = thePoint - theThreat.transform.position;
        float theAngle = Vector3.Angle(threatLookingRay.direction, lineBetweenThreatAndPoint);
        if(theAngle < 90f)
        {
            return theAngle;
        }
        else
        {
            return -theAngle;
        }
        
    }


    public void NEWcombine2into1()
    {
        //combine the distance and angle measures
        //if i have it correct, high distance = good, and high angle equals good?
        //so, could multiply them together

        int theIndex = 0;
        foreach (float thisFloat in distanceSamples)
        {
            combinedMeasures.Add(thisFloat * lookingAngleSamples[theIndex]);
            theIndex++;
        }

    }













    public void gatherALLThreatDataTypes(List<GameObject> theThreatList)
    {
        if(thePoints.Count == 0)
        {
            generateValidNearPoints1(location);
        }
        

        numberOfThreats = theThreatList.Count;
        numberOfSamplePoints = thePoints.Count;


        //aquireDistanceSamples(theThreatList);


        foreach (Vector3 aPoint in thePoints)
        {
            float currentAddedDistance = 0f;
            float currentAddedAngles = 0f;
            foreach (GameObject thisThreat in theThreatList)
            {

                // distance to threat
                // line of sight to threat
                // angle between where shooter is facing and where the point is
                // proximity to end goal

                //distanceData1(aPoint, aThreat);
                //lineOfSightData1(aPoint, aThreat);
                //lookingAngleData1(aPoint, aThreat);
                //distancePointsData1(aPoint, goalPoint, gatheredData);
                currentAddedDistance += aquireONEDistanceSampleOnePointOneThreat(aPoint, thisThreat);
                currentAddedAngles += aquireONEAngleSampleOnePointONeThreat(aPoint, thisThreat);
            }
            distanceSamples.Add(currentAddedDistance);
            lookingAngleSamples.Add(currentAddedAngles/(180*theThreatList.Count));
        }

    }


    public void combine2into1()
    {
        //combine the distance and angle measures
        //if i have it correct, high distance = good, and high angle equals good?
        //so, could multiply them together

        int theIndex = 0;
        foreach(float thisFloat in distanceSamples)
        {
            combinedMeasures.Add(thisFloat* lookingAngleSamples[theIndex]);
            theIndex++;
        }

    }


    public float aquireONEAngleSampleOnePointONeThreat(Vector3 thePoint, GameObject theThreat)
    {
        Ray threatLookingRay = theThreat.GetComponent<body1>().lookingRay;
        Vector3 lineBetweenThreatAndPoint = thePoint - theThreat.transform.position;
        float theAngle = Vector3.Angle(threatLookingRay.direction, lineBetweenThreatAndPoint);
        return theAngle;
    }








    public void generateValidNearPoints1(Vector3 pointToBeNear)
    {
        List<Vector3> listOfNearMeshPoints = new List<Vector3>();

        List<Vector3> just8PointsAroundMiddle = new List<Vector3>();
        just8PointsAroundMiddle = generate8NearPoints(pointToBeNear);

        //Debug.Log("just8PointsAroundMiddle.Count:  " + just8PointsAroundMiddle.Count);

        foreach (Vector3 thisPoint in just8PointsAroundMiddle)
        {
            //if they are "on" the navmesh [or near enough], add them to the "listOfNearMeshPoints"
            NavMeshHit hit;
            //Debug.Log("is this point on navmesh?");
            if (NavMesh.SamplePosition(thisPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                //Debug.Log("no, this point is NOT on navmesh");
                listOfNearMeshPoints.Add(thisPoint);
            }
            else
            {
                //Debug.Log("yes, this point is on navmesh");
            }
        }


        //Debug.Log("listOfNearMeshPoints.Count:  " + listOfNearMeshPoints.Count);

        thePoints = listOfNearMeshPoints;
    }


    //public Vector3 aquireDistanceSamples(List<GameObject> thisThreatList)
    //{
        //so, find point with shortest cumulative distance from threats?
        //maybe with some way of making near threats worse

        //List<float> addedDistancesPerPointList = new List<float>();

        //Debug.Log("0000000000000000000000000000000000000000000");
        //foreach (Vector3 aPoint in thePoints)
        //{
            //float currentAddedDistance = 0f;
            //foreach (GameObject thisThreat in thisThreatList)
            //{
                //currentAddedDistance += aquireONEDistanceSampleOnePointOneThreat(aPoint, thisThreat);
            //}
            //addedDistancesPerPointList.Add(currentAddedDistance);
        //}

        //distanceSamples = addedDistancesPerPointList;
    //}

    public float aquireONEDistanceSampleOnePointOneThreat(Vector3 thePoint, GameObject theThreat)
    {
        return (thePoint - theThreat.transform.position).magnitude;
    }



    public Vector3 pointAwayFromThreats(List<GameObject> thisThreatList)
    {
        //so, find point with shortest cumulative distance from threats?
        //maybe with some way of making near threats worse

        List<float> addedDistancesPerPointList = new List<float>();

        //Debug.Log("0000000000000000000000000000000000000000000");
        foreach (Vector3 aPoint in thePoints)
        {
            float currentAddedDistance = 0f;
            foreach(GameObject thisThreat in thisThreatList)
            {
                //Debug.Log("location.magnitude:  " + location.magnitude);
                //Debug.Log("thisThreat.transform.position.magnitude:  " + thisThreat.transform.position.magnitude);
                currentAddedDistance += (aPoint - thisThreat.transform.position).magnitude;
                //Debug.Log(currentAddedDistance);
            }
            //Debug.Log("INDEX:  " + "n/a" + "currentAddedDistance:  " + currentAddedDistance);
            //currentAddedDistance = currentAddedDistance / 10000;
            //Debug.Log(currentAddedDistance);
            //currentAddedDistance = currentAddedDistance / 100000;
            //currentAddedDistance = currentAddedDistance / 100000;
            //currentAddedDistance = currentAddedDistance / 100000;
            //currentAddedDistance = currentAddedDistance / 100000;
            //graphBetweenTwoPoints(location, aPoint * -currentAddedDistance/100000000);
            //graphBetweenTwoPoints2(location, aPoint, currentAddedDistance/10);
            addedDistancesPerPointList.Add(currentAddedDistance);
        }

        //Debug.Log("PICK THIS INDEX  whichIndexIsHighest(addedDistancesPerPointList):  " + whichIndexIsHighest(addedDistancesPerPointList));

        return thePoints[whichIndexIsHighest(addedDistancesPerPointList)];
    }


    public List<Vector3> generate8NearPoints(Vector3 theMiddlePoint)
    {
        List<Vector3> just8PointsAroundMiddle = new List<Vector3>();

        //soooo:
        //-1x +1z       0x +1z      +1x +1z
        //
        //-1x 0z       [N/A]      +1x 0z
        //
        //-1x -1z       0x -1z      +1x -1z

        int theXValue = -1;
        int theYValue = -1;
        while (theXValue < 2)
        {
            //while (theYValue < 2)
            {
                //Vector3 newPoint = theMiddlePoint + new Vector3(theXValue, 0, theYValue);
                //just8PointsAroundMiddle.Add(newPoint);
                //theYValue++;
            }

            Vector3 newPoint = theMiddlePoint + new Vector3(theXValue, 0, theYValue);
            just8PointsAroundMiddle.Add(newPoint);
            theYValue++;
            Vector3 newPoint2 = theMiddlePoint + new Vector3(theXValue, 0, theYValue);
            just8PointsAroundMiddle.Add(newPoint2);
            theYValue++;
            Vector3 newPoint3 = theMiddlePoint + new Vector3(theXValue, 0, theYValue);
            just8PointsAroundMiddle.Add(newPoint3);
            theYValue++;

            theYValue = -1;
            theXValue++;
        }



        return just8PointsAroundMiddle;
    }


    public void graphLookingAnglesSq1(Vector3 whereToStart)
    {
        general8DirectionGraph1(whereToStart, makeAngleDataManageable(addTheAnglesPerPoint()));
    }

    public void graphDistanceSq1(Vector3 whereToStart)
    {
        general8DirectionGraph1(whereToStart, makeAngleDataManageable(addTheAnglesPerPoint()));
    }

    public void graphBetweenTwoPoints(Vector3 whereToStart, Vector3 whereToEnd)
    {
        Vector3 startV = whereToStart;
        Vector3 endV = whereToEnd;//.operator(2);// * someBoolsAdded[thePointIndex];
        Vector3 diffV = (endV - startV);
        Vector3 drawV = endV + diffV;
        Debug.DrawLine(startV, drawV, new Color(0f, 0f, 1f), 1f);
    }

    public void graphBetweenTwoPoints2(Vector3 whereToStart, Vector3 whereToEnd, float lengthMultiplier)
    {
        Vector3 startV = whereToStart;
        Vector3 endV = whereToEnd;//.operator(2);// * someBoolsAdded[thePointIndex];
        Vector3 diffV = (endV - startV);
        Vector3 drawV = endV + diffV*lengthMultiplier* lengthMultiplier * diffV.sqrMagnitude * diffV.sqrMagnitude/10;
        Debug.DrawLine(startV, drawV, new Color(0f, 0f, 1f), 1f);
    }

    public void pickBestPointFromData1()
    {
        //how to do this?
        //specifically, how to COMBINE the data from different data types?
        //working with angle from sight, line of sight, and distance
        //how about multiply all by line of sight, and favor distance over angle [don't go TOWARDS danger to avoid angle!  think of enemy in a corner position, or far away]
        //UnityEngine.Vector3 bestVector = null;

        //int indexForBest = whichIndexIsLowest();

        int indexForBest = whichIndexIsHighest(combine3into1());

        //return myData.thePoints[whichPoint];
        //bestPoint = thePoints[whichPoint];
        //bestPoint.Add(thePoints[whichPoint]);
        //Debug.Log("whichPoint:  " + whichPoint);
        //Debug.Log("indexForBest:  " + indexForBest);

        if (bestPoint.Count > 0 )
        {
            bestPoint[0] = thePoints[indexForBest];
        }
        else
        {
            bestPoint.Add(thePoints[indexForBest]);
        }
    }


    public int whichIndexIsLowest()
    {
        int indexOfLowest = 0;


        float bestFloat = 0f;
        int indexForLoop = 0;

        List<float> combinedList = combine3into1();
        //Debug.Log("combinedList.Count:  " + combinedList.Count);

        foreach (float thisPosition in combinedList)
        {
            //i want LOWEST?  i think? well, they vary from one data type to another.
            //let's set it up so that lowest is best?  like lowest risk?  sure
            //Debug.Log("BEFORE IF thisPosition < bestFloat OR indexForLoop == 0:  " + thisPosition + ", " + bestFloat + ", " + indexForLoop);
            if (indexForLoop == 0 || thisPosition > bestFloat)
            {
                //Debug.Log("AFTER IF thisPosition < bestFloat OR indexForLoop == 0:  " + thisPosition + ", " + bestFloat + ", " + indexForLoop);
                bestFloat = thisPosition;
                indexOfLowest = indexForLoop;
            }
            indexForLoop++;
        }



        return indexOfLowest;
    }







    public int whichIndexIsHighest(List<float> listOfFloats)
    {
        int indexOfHighest = 0;


        float bestFloat = 0f;
        int indexForLoop = 0;

        //      List<float> combinedList = combine3into1();
        //Debug.Log("combinedList.Count:  " + combinedList.Count);

        foreach (float thisPosition in listOfFloats)
        {
            //i want LOWEST?  i think? well, they vary from one data type to another.
            //let's set it up so that lowest is best?  like lowest risk?  sure
            //Debug.Log("BEFORE IF thisPosition < bestFloat OR indexForLoop == 0:  " + thisPosition + ", " + bestFloat + ", " + indexForLoop);
            if (indexForLoop == 0 || thisPosition < bestFloat)
            {
                //Debug.Log("AFTER IF thisPosition < bestFloat OR indexForLoop == 0:  " + thisPosition + ", " + bestFloat + ", " + indexForLoop);
                bestFloat = thisPosition;
                indexOfHighest = indexForLoop;
            }
            indexForLoop++;
        }



        return indexOfHighest;
    }

    public int whichIndexIsACTUALLYhighest(List<float> listOfFloats)
    {
        int indexOfHighest = 0;


        float bestFloat = 0f;
        int indexForLoop = 0;

        //      List<float> combinedList = combine3into1();
        //Debug.Log("combinedList.Count:  " + combinedList.Count);

        foreach (float thisPosition in listOfFloats)
        {
            //i want LOWEST?  i think? well, they vary from one data type to another.
            //let's set it up so that lowest is best?  like lowest risk?  sure
            //Debug.Log("BEFORE IF thisPosition < bestFloat OR indexForLoop == 0:  " + thisPosition + ", " + bestFloat + ", " + indexForLoop);
            if (indexForLoop == 0 || thisPosition > bestFloat)
            {
                //Debug.Log("AFTER IF thisPosition < bestFloat OR indexForLoop == 0:  " + thisPosition + ", " + bestFloat + ", " + indexForLoop);
                bestFloat = thisPosition;
                indexOfHighest = indexForLoop;
            }
            indexForLoop++;
        }



        return indexOfHighest;
    }


    public List<float> combine3into1()
    {
        //how to do this?
        //specifically, how to COMBINE the data from different data types?
        //working with angle from sight, line of sight, and distance
        //how about multiply all by line of sight, and favor distance over angle [don't go TOWARDS danger to avoid angle!  think of enemy in a corner position, or far away]

        List<float> combine3into1List = new List<float>();

        lookingAnglesSq = makeAngleDataManageable(lookingAnglesSq);

        int pointPositionIndexPlusOne = 1;

        foreach (Vector3 thisPoint in thePoints)
        {
            float aCombinedPoint = combineONEpoint3into1(pointPositionIndexPlusOne);
            combine3into1List.Add(aCombinedPoint);
            pointPositionIndexPlusOne++;
        }

        return combine3into1List;
    }

    public float combineONEpoint3into1(int whichPoint)
    {
        //"whichPoint" should be starting at "1", NOT at "0" [because i need to multiply it]

        //how to do this?
        //specifically, how to COMBINE the data from different data types?
        //working with angle from sight, line of sight, and distance
        //how about multiply all by line of sight, and favor distance over angle [don't go TOWARDS danger to avoid angle!  think of enemy in a corner position, or far away]
        

        float combinedONEpoint3into1ForThisPoint = 0;
        int threatIndexPlusOne = 1;

        while (threatIndexPlusOne < numberOfThreats + 1)
        {
            //      float thedistanceData = addedDistancesForONEPoint(whichPoint);  //i don't have this yet
            float theAngleData = addedAnglesForONEPoint(whichPoint);
            float theLineOfSightData = addedBoolsForONEPoint(whichPoint) + 1; //add one, so that it doesn't actually ever multiply by zero?  i don't want that to get rid of the other variation?

            //      combinedONEpoint3into1ForThisPoint = theLineOfSightData * (theAngleData / thedistanceData);
            combinedONEpoint3into1ForThisPoint = theLineOfSightData * (theAngleData);


            //Debug.Log("threatIndexPlusOne:  " + threatIndexPlusOne);
            //Debug.Log("theAngleData:  " + theAngleData);
            //Debug.Log("theLineOfSightData:  " + theLineOfSightData);
            //Debug.Log("combinedONEpoint3into1ForThisPoint:  " + combinedONEpoint3into1ForThisPoint);
            


            threatIndexPlusOne++;
        }

        return combinedONEpoint3into1ForThisPoint;
    }


    public List<float> distanceDataAddedTogether()
    {
        List<float> addedDistancesPerPointList = new List<float>();


        int pointPositionIndexPlusOne = 1;

        foreach (Vector3 aPoint in thePoints)
        {
            float addedDistancesForThisPoint = addedDistancesForONEPoint(pointPositionIndexPlusOne);
            addedDistancesPerPointList.Add(addedDistancesForThisPoint);
            pointPositionIndexPlusOne++;
        }


        return addedDistancesPerPointList;
    }

    public float addedDistancesForONEPoint(int whichPoint)
    {
        //"whichPoint" should be starting at "1", NOT at "0" [because i need to multiply it]

        float addedDistancesForThisPoint = 0;
        int threatIndexPlusOne = 1;

        while (threatIndexPlusOne < numberOfThreats + 1)
        {
            //Debug.Log("whichPoint:  " + whichPoint);
            //Debug.Log("threatIndexPlusOne:  " + threatIndexPlusOne);
            //Debug.Log("distancePointsSq:  " + distancePointsSq);
            //Debug.Log("distancePointsSq.Count:  " + distancePointsSq.Count);
            //Debug.Log("distancePointsSq[(whichPoint * threatIndexPlusOne) - 1]:  " + distancePointsSq[(whichPoint * threatIndexPlusOne) - 1]);
            addedDistancesForThisPoint += distancePointsSq[(whichPoint * threatIndexPlusOne) - 1];

            threatIndexPlusOne++;
        }

        return addedDistancesForThisPoint/(numberOfThreats*100);
    }

    public List<float> makeAngleDataManageable(List<float> theData)
    {
        List<float> smallerData = new List<float>();

        foreach (float anAngle in theData)
        {
            float newFloat = anAngle / numberOfThreats;
            smallerData.Add(newFloat/(360f*360f*360f));
        }

        return smallerData;
    }

    public void graphLineOfSightData1(Vector3 whereToStart)
    {
        general8DirectionGraph1(whereToStart, addTheBools());
    }

    public void general8DirectionGraph1(Vector3 whereToStart, List<float> theDataToGraph)
    {
        int thePointIndex = 0;

        foreach (Vector3 aPoint in thePoints)
        {
            Vector3 startV = whereToStart;
            Vector3 endV = aPoint;//.operator(2);// * someBoolsAdded[thePointIndex];
            Vector3 diffV = (endV - startV);
            Vector3 drawV = endV + diffV * theDataToGraph[thePointIndex];
            Debug.DrawLine(startV, drawV, new Color(0f, 0f, 1f), 1f);

            thePointIndex++;
        }
    }

    public void general8DirectionGraphEXAGGERATED(Vector3 whereToStart, List<float> theDataToGraph)
    {
        int thePointIndex = 0;

        //Debug.Log("----------------------------------------------------");
        float highestPoint = theDataToGraph[whichIndexIsACTUALLYhighest(theDataToGraph)];
        float LowestPoint = theDataToGraph[whichIndexIsHighest(theDataToGraph)];
        //Debug.Log("highestPoint:  " + highestPoint);
        //Debug.Log("LowestPoint:  " + LowestPoint);
        float diffBetweenHighAndLow = highestPoint - LowestPoint;
        float incrementBetweenPoints = diffBetweenHighAndLow/theDataToGraph.Count;
        float normalizeFactor = theDataToGraph[0];
        float exaggeratedFactor = theDataToGraph[1] / normalizeFactor;

        float theSum = 0f;
        foreach(float thisFloat in theDataToGraph)
        {
            theSum += thisFloat;
        }

        float theAverage = theSum/theDataToGraph.Count;

        foreach (Vector3 aPoint in thePoints)
        {
            //Debug.Log(theDataToGraph[thePointIndex]);
            Vector3 startV = whereToStart;
            Vector3 endV = aPoint;//.operator(2);// * someBoolsAdded[thePointIndex];
            Vector3 diffV = (endV - startV);
            float exaggeratedFactor2 = theDataToGraph[thePointIndex]  / normalizeFactor;
            float thisPointNormalizedToLowest = theDataToGraph[thePointIndex]/LowestPoint;
            float thisPointNormalizedToHighest = theDataToGraph[thePointIndex] / highestPoint;
            //Vector3 drawV = endV + diffV * (exaggeratedFactor2) * 10 * (1  - exaggeratedFactor2);
            //Vector3 drawV = endV + diffV * (10 + 10 * (thisPointNormalizedToLowest - 1));
            //(1-x/lowest)/upper bound???
            float fractionOfTotal = (theDataToGraph[thePointIndex] - LowestPoint) / (diffBetweenHighAndLow);
            Vector3 drawV = endV + diffV * 1*(1 + 2* fractionOfTotal* fractionOfTotal);
            Debug.DrawLine(startV, drawV, new Color(0f, 0f, 1f), 1f);

            thePointIndex++;
        }
    }


    public List<float> addTheBools()
    {
        //[the LINE OF SIGHT bools]


        List<float> addedBoolsList = new List<float>();

        //make ONE interger for each SAMPLE POINT by adding the bools [as ones and zeroes]


        int pointPositionIndexPlusOne = 1;

        foreach(Vector3 aPoint in thePoints)
        {
            int addedBoolsForThisPoint = addedBoolsForONEPoint(pointPositionIndexPlusOne);
            addedBoolsList.Add(addedBoolsForThisPoint);
            pointPositionIndexPlusOne++;
        }


        return addedBoolsList;
    }

    public List<float> addTheAnglesPerPoint()
    {

        List<float> addedAnglesPerPointList = new List<float>();


        int pointPositionIndexPlusOne = 1;

        foreach (Vector3 aPoint in thePoints)
        {
            float addedAnglesForThisPoint = addedAnglesForONEPoint(pointPositionIndexPlusOne);
            addedAnglesPerPointList.Add(addedAnglesForThisPoint);
            pointPositionIndexPlusOne++;
        }


        return addedAnglesPerPointList;
    }

    public int addedBoolsForONEPoint(int whichPoint)
    {
        //"whichPoint" should be starting at "1", NOT at "0" [because i need to multiply it]
        int addedBoolsForThisPoint = 0;
        int threatIndexPlusOne = 1;

        while (threatIndexPlusOne < numberOfThreats + 1)
        {
            if (lineOfSightData[(whichPoint * threatIndexPlusOne) - 1] == true)
            {
                addedBoolsForThisPoint++;
            }
            threatIndexPlusOne++;
        }

        return addedBoolsForThisPoint;
    }


    public float addedAnglesForONEPoint(int whichPoint)
    {
        //"whichPoint" should be starting at "1", NOT at "0" [because i need to multiply it]

        float addedAnglesForThisPoint = 0;
        int threatIndexPlusOne = 1;

        while (threatIndexPlusOne < numberOfThreats + 1)
        {
            addedAnglesForThisPoint += lookingAnglesSq[(whichPoint * threatIndexPlusOne) - 1];

            threatIndexPlusOne++;
        }

        return addedAnglesForThisPoint;
    }

}
