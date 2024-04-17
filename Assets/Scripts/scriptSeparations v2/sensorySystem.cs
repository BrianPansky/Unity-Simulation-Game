using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Xml.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.UI;
//using System.Drawing;
//using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class sensorySystem : MonoBehaviour
{
    private worldScript theWorldScript;
    public GameObject target;


    public body1 body;

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

        
        //makeVectorFeild(threatListWithoutSelf());

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

    public void makeVectorFeild(List<GameObject> thisThreatList)
    {
        //make grid of points around them
        //for EACH point:
        //      find the direction that would be "picked"
        //      display it at that point

        spatialData1Field myField = new spatialData1Field();

        myField.theMiddlePoint = this.gameObject.transform.position;
        myField.thisThreatList = thisThreatList;
        //myField.thisThreatList = body.theLocalMapZoneScript.threatList;
        myField.vectorField2();


        foreach(GameObject obj in thisThreatList)
        {
            //myField.graphBetweenTwoPoints2(myField.theMiddlePoint, obj.transform.position, 1f);

            Vector3 startV = myField.theMiddlePoint;
            Vector3 endV = obj.transform.position;//.operator(2);// * someBoolsAdded[thePointIndex];
            //Vector3 diffV = (endV - startV);
            //Vector3 drawV = endV + diffV * lengthMultiplier * lengthMultiplier * diffV.sqrMagnitude * diffV.sqrMagnitude / 10;
            //Debug.DrawLine(startV, drawV, new Color(0f, 0f, 1f), 1f);
            Debug.DrawLine(startV, endV, new Color(1f, 0f, 0f), 1f);
        }

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




public class spatialDataPointFragment
{
    //basically, i hate having anything significant nested inside of my "foreach" loops.
    //and if there IS much in there, it should be easy to "find" when i'm debugging.  not digging through layers of functions.
    //so put stuff HERE, in a different class object.  a base level, where i will know to look if that's what the error seems to be
    //[same for adding or modifying stuff, not just debugging]

    //so, right now "spatialDataPoint" has multiple "threat" gameobjects.
    //i'm replacing that with a list of THIS class object.  each of these will have one of those possible targets.
    //could i go the other way?  START with something like this, THEN gather them and even RE-USE them for different npcs?  i dunno.

    public GameObject targetObject;
    public Vector3 originLocation;
    public Vector3 lineBetweenTargetAndThisPoint = new Vector3();

    public float distanceAsFloat = 0f;
    public float lookingAngle = 0f;
    public bool lineOfSightBool = true;  //initialize as true???

    public Vector3 distanceAsVector = new Vector3();
    public Vector3 lookingAnglePerpendicular = new Vector3();  //should i have one left AND right???


    //????????????????
    public float combinedMeasures = 0;  //hmmmm....don't i want a VECTOR i can GRAPH?
    
    public Vector3 dontIneedACOMBINEDEndpoint;






    //      graphing






    //useful "patterns" etc.
    public Vector3 applePattern()
    {
        //need to make it easy to make versions for UNARMED npcs simply FLEEING from threat.
        //that shouldn’t require making much new stuff.
        //indeed, it should be BUILT IN somehow, INHERENT to WHAT is even CALCULATED at each point.
        //an inflection point where “risk of death” is LOWER if you go in for the kill?  or something.  


        //return zero length / no direction if there is no line of sight
        //but if there IS a line of sight, do the following:
        //      when in front of enemy:
        //              use vector pointing AWAY from enemy
        //      when BEHIND an enemy
        //              vector points TOWARDS enemy
        //      and then, in either case:
        //              use vector ORBITING enemy, to their side
        //              simply combine both of these
        Vector3 theOutputVector = new Vector3();

        generalFragmentData1();


        //          Vector3 awayOrTowardsUndecided = distanceAsVector;
        //          Vector3 theOrbitUndecided = lookingAnglePerpendicular;

        Vector3 awayOrTowardsFinal = towardsIfBehindAwayIfInFront();
        Vector3 theOrbitFinal = orbitAwayFromLookingLine();

        float orbitCoefficient = 1;
        float entryCoefficient = 1;

        float crossoverAngle = 110;

        if (lookingAngle < crossoverAngle)
        {
            //have i made sure these andles only go between 0 and 180 degrees??  is that the default?

            //entryCoefficient = lookingAngles[currentIndex]/120;
            entryCoefficient = 0;
            orbitCoefficient = 1;

            theOutputVector += (entryCoefficient * awayOrTowardsFinal + orbitCoefficient * theOrbitFinal);
        }
        else
        {
            //entryCoefficient = lookingAngles[currentIndex] / 180;
            //orbitCoefficient = (180 - lookingAngles[currentIndex])/180;
            //entryCoefficient = 1;
            //orbitCoefficient = 0;
            entryCoefficient = (lookingAngle - crossoverAngle) / (180 - crossoverAngle);
            orbitCoefficient = (180 - lookingAngle) / 180;
            theOutputVector += (entryCoefficient * awayOrTowardsFinal + orbitCoefficient * theOrbitFinal);
        }


        return theOutputVector;
    }


    //      initializing

    public void initializeFragment(GameObject target, Vector3 location)
    {
        targetObject = target;
        originLocation = location;
        generalFragmentData1();
    }

    public void generalFragmentData1()
    {
        gatherLookingAngleData();
        gatherDistanceData();
        gatherLineOfSightData();
        findIntermediateDataModularly();
    }


    public void gatherDistanceData()
    {
        Vector3 thisDistance = targetObject.transform.position - originLocation;
        distanceAsFloat = thisDistance.magnitude;
        distanceAsVector = thisDistance;
    }
    public void gatherLookingAngleData()
    {
        Ray targetLookingRay = targetObject.GetComponent<body1>().lookingRay;
        Vector3 lineBetweenThreatAndPoint = originLocation - targetObject.transform.position;
        float theAngle = Vector3.Angle(targetLookingRay.direction, lineBetweenThreatAndPoint);

        lookingAngle = theAngle;
    }
    public void gatherLineOfSightData()
    {
        bool theBool = false;
        RaycastHit myHit;

        //new Ray(this.transform.position, theBody.theWorldScript.theTagScript.semiRandomUsuallyNearTargetPickerFromList(theBody.theLocalMapZoneScript.theList, this.gameObject).transform.position);
        Vector3 theDirection = targetObject.transform.position - originLocation;
        Ray myRay = new Ray(originLocation, theDirection);


        if (Physics.Raycast(myRay, out myHit, 60f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (myHit.collider.gameObject == targetObject)
            {
                theBool = true;
            }
        }

        lineOfSightBool = theBool;
    }
    public void findIntermediateDataModularly()
    {
        //so i should already have basic data [distances, angles, lines of sight]
        //now i want to get:
        //      perpendiculars [symmetrical]

        lookingAnglePerpendicular = findOnePerpendicularVectorSYMMETRICAL();
    }
    public Vector3 findOnePerpendicularVectorSYMMETRICAL()
    {
        Vector3 perpendicular = new Vector3();
        Ray threatLookingRay = targetObject.GetComponent<body1>().lookingRay;
        lineBetweenTargetAndThisPoint = originLocation - targetObject.transform.position;

        float theAngle = Vector3.SignedAngle(threatLookingRay.direction, lineBetweenTargetAndThisPoint, Vector3.up);
        //Debug.Log("theAngle:  " + theAngle);

        if (theAngle < 0f)
        {
            if (theAngle > -120f)
            {
                perpendicular = Quaternion.Euler(0, -90, 0) * lineBetweenTargetAndThisPoint;
            }
            else
            {
            }
        }
        else
        {
            if (theAngle < 120f)
            {
                perpendicular = Quaternion.Euler(0, 90, 0) * lineBetweenTargetAndThisPoint;
            }
            else
            {
            }
        }





        return perpendicular;
    }



    //      utility
    public Vector3 towardsIfBehindAwayIfInFront()
    {
        //      when in front of enemy:
        //              use vector pointing AWAY from enemy
        //      when BEHIND an enemy
        //              vector points TOWARDS enemy

        //so, need to FIRST determine whether we are in front of or behind enemy.  we should have the angles for that:
        if (lookingAngle < 110)
        {
            //have i made sure these andles only go between 0 and 180 degrees??  is that the default?

            return -distanceAsVector;
        }
        else
        {
            return distanceAsVector;
        }
    }
    public Vector3 orbitAwayFromLookingLine()
    {
        //not sure i know how to ensure these point the correct direction:
        return lookingAnglePerpendicular;
    }




    //      ad-hoc, try to get rid of

}

public class spatialDataPoint
{
    //this code should only ever be about one single point
    //anything plural [like "distances"] should still be about one sample point, and how far away other things [like threats] are from that one point

    public List<spatialDataPointFragment> fragmentList = new List<spatialDataPointFragment>();

    public int listLenghts = 0;

    //public List<float> combinedMeasures = new List<float>();
    public float combinedMeasures = 0;  //hmmmm....don't i want a VECTOR i can GRAPH?
    public Vector3 thisPoint;
    public Vector3 dontIneedACOMBINEDEndpoint;



    //          user manual?
    //      when to add threat list?  is it ALWAYS necessary?  well, i might use this for something other than "threats"...
    //      




    //      graphing
    public void graphBetweenThisPointAndCOMBINEDpoint()
    {
        graphBetweenTwoPoints2(thisPoint, (thisPoint + dontIneedACOMBINEDEndpoint.normalized), 0.01f);
    }
    public void graphBetweenTwoPoints2(Vector3 whereToStart, Vector3 whereToEnd, float lengthMultiplier)
    {
        Vector3 startV = whereToStart;
        Vector3 endV = whereToEnd;//.operator(2);// * someBoolsAdded[thePointIndex];
        //      Vector3 diffV = (endV - startV);
        //      Vector3 drawV = endV + diffV * lengthMultiplier * diffV.sqrMagnitude * diffV.sqrMagnitude / 10;
        //      Debug.DrawLine(startV, drawV, new Color(0f, 0f, 1f), 1f);
        Debug.DrawLine(startV, endV, new Color(0f, 0f, 1f), 1f);
    }


    //useful "patterns" etc.
    public Vector3 applePattern()
    {
        //return zero length / no direction if there is no line of sight
        //but if there IS a line of sight, do the following:
        //      when in front of enemy:
        //              use vector pointing AWAY from enemy
        //      when BEHIND an enemy
        //              vector points TOWARDS enemy
        //      and then, in either case:
        //              use vector ORBITING enemy, to their side
        //              simply combine both of these
        Vector3 theOutputVector = new Vector3();

        //generalPointData1();

        foreach (spatialDataPointFragment thisFragment in fragmentList)
        {
            if (thisFragment.lineOfSightBool == true)
            {
                
                theOutputVector += thisFragment.applePattern();
            }
        }

        return theOutputVector;
    }



    //      initializing

    public void initializeDataPoint(List<GameObject> targetList, Vector3 inputPoint)
    {
        thisPoint = inputPoint;

        foreach (GameObject thisTarget in targetList)
        {
            spatialDataPointFragment newFragment = new spatialDataPointFragment();
            newFragment.initializeFragment(thisTarget, thisPoint);
            fragmentList.Add(newFragment);
        }
    }






    //      utility

    //      ad-hoc, try to get rid of
   
    

}

public class spatialDataSet
{
    //this code should be about using more than one "spatialDataPoint" class object

    public List<spatialDataPoint> theDataSet = null;// new List<spatialDataPoint>();

    public List<Vector3> thePoints = new List<Vector3>();
    //public List<Vector3> endPointsToGraph = new List<Vector3>();
    public List<GameObject> threatList;
    public List<float> combinedData;
    public Vector3 middlePoint;




    //      graphing
    public void graphFeild(string graphInBlue, string graphInGreen = null, string graphInRed = null, string graphInYellow = null, string graphInMagenta = null)
    {
        //what are the strings supposed to be?  tell it what data to graph?  something?  i guess?
        //so how to do this?
        //first, use the string to set up the data to graph
        //then graph it in the corrosponding color
        //and be able to graph multiple things at once
        List<string> treatmentList = new List<string>();
        treatmentList = listOfStringsWITHNulls(graphInBlue, graphInGreen, graphInRed, graphInYellow, graphInMagenta);

        int treatmentCounter = 0;

        foreach (string thisTreatment in treatmentList)
        {

        }

    }


    

    //      initializing
    public List<Vector3> generate2Xby2YNearPoints(Vector3 theMiddlePoint, int X, int Y)
    {
        List<Vector3> xByYpointsAroundMiddle = new List<Vector3>();

        //soooo:
        //-1x +1z       0x +1z      +1x +1z
        //
        //-1x 0z       [N/A]      +1x 0z
        //
        //-1x -1z       0x -1z      +1x -1z

        int theXValue = -X;
        int theYValue = -Y;
        while (theXValue < X)
        {
            while (theYValue < Y)
            {
                Vector3 newPoint = theMiddlePoint + new Vector3(theXValue * 2, 0, theYValue * 2);
                xByYpointsAroundMiddle.Add(newPoint);
                theYValue++;
            }

            theYValue = -Y;
            theXValue++;
        }



        return xByYpointsAroundMiddle;
    }

    public void createBlankDataSetFromPoints()
    {
        theDataSet = new List<spatialDataPoint>();

        foreach (Vector3 thisPoint in thePoints)
        {
            spatialDataPoint aDataPoint = new spatialDataPoint();
            aDataPoint.initializeDataPoint(threatList, thisPoint);
            theDataSet.Add(aDataPoint);
        }
    }



    //      utility
    public List<string> listOfStringsWithoutNulls(string s1, string s2 = null, string s3 = null, string s4 = null, string s5 = null)
    {
        List<string> inputList = new List<string>();
        inputList.Add(s1);
        inputList.Add(s2);
        inputList.Add(s3);
        inputList.Add(s4);
        inputList.Add(s5);

        List<string> outputList = new List<string>();

        foreach(string thisString in inputList)
        {
            if(thisString != null)
            {
                outputList.Add(thisString);
            }
        }

        return outputList;
    }

    public List<string> listOfStringsWITHNulls(string s1, string s2 = null, string s3 = null, string s4 = null, string s5 = null)
    {
        List<string> outputList = new List<string>();
        outputList.Add(s1);
        outputList.Add(s2);
        outputList.Add(s3);
        outputList.Add(s4);
        outputList.Add(s5);

        return outputList;
    }





    //      ad-hoc, try to get rid of
    public Vector3 bestMiddlePoint()
    {
        spatialDataPoint middleDataPoint = new spatialDataPoint();
        middleDataPoint.initializeDataPoint(threatList, middlePoint);

        return middleDataPoint.applePattern();
    }
    public void appleField()
    {
        int currentIndex = 0;

        foreach (spatialDataPoint thisDataPoint in theDataSet)
        {
            //endPointsToGraph.Add(thisDataPoint.pattern1ForFightingArmedThreat());
            thisDataPoint.dontIneedACOMBINEDEndpoint = thisDataPoint.applePattern();
            currentIndex++;
        }
    }


    public void graphFeildAdHoc()
    {
        //just do regular blue vector feild of the "end points" for now.

        foreach (spatialDataPoint thisDataPoint in theDataSet)
        {
            thisDataPoint.graphBetweenThisPointAndCOMBINEDpoint();
        }
    }



}




public class spatialData1Field
{
    public List<Vector3> thePoints = new List<Vector3>();
    public List<spatialData1> theData = new List<spatialData1>();
    public Vector3 theMiddlePoint;
    public List<GameObject> thisThreatList;



    int graphCooldown = 0;
    int currentGraphRow = 0;

    public void vectorField1()
    {
        //make grid of points around them
        //for EACH point:
        //      find the direction that would be "picked"
        //      display it at that point

        int howManyInEachRow = 5;

        thePoints = generate2Xby2YNearPoints(theMiddlePoint, howManyInEachRow, howManyInEachRow);
        theData = getDataForAllPoints();


        if (graphCooldown < 0)
        {
            graphCooldown++;
        }
        else
        {
            graphCooldown = 0;
            graphFieldPartial(howManyInEachRow, currentGraphRow);
            if(currentGraphRow == howManyInEachRow)
            {
                currentGraphRow = 0;
            }
            else
            {
                currentGraphRow++;
            }
            


        }

        //adHocThreatAvoidanceVector = (this.gameObject.transform.position - myData.thePoints[myData.whichIndexIsHighest(myData.combinedMeasures)]);


    }

    public void vectorField2()
    {
        //make grid of points around them
        //for EACH point:
        //      

        int howManyInEachRow = 5;

        thePoints = generate2Xby2YNearPoints(theMiddlePoint, howManyInEachRow, howManyInEachRow);
        //List<Vector3> theField = vectorsForField2();
        theData = getDataForAllPoints();
        //theData = vectorsForField2();

        if (graphCooldown < 0)
        {
            graphCooldown++;
        }
        else
        {
            graphCooldown = 0;
            //graphFieldPartial(howManyInEachRow, currentGraphRow);
            graphField1();
            if (currentGraphRow == howManyInEachRow)
            {
                currentGraphRow = 0;
            }
            else
            {
                currentGraphRow++;
            }
        }
    }


    

    public List<Vector3> vectorsForField2()
    {
        List<Vector3> theList = new List<Vector3>();

        foreach (Vector3 thisPoint in thePoints)
        {

            spatialData1 myData = new spatialData1();
            myData.location = theMiddlePoint;
            theList.Add(myData.combineVectorForOnePoint2(thisPoint, thisThreatList));

            //      theList.Add(combineVectorForOnePoint2(thisPoint));


        }


        return theList;
    }



    public void graphBetweenTwoPoints2(Vector3 whereToStart, Vector3 whereToEnd, float lengthMultiplier)
    {
        Vector3 startV = whereToStart;
        Vector3 endV = whereToEnd;//.operator(2);// * someBoolsAdded[thePointIndex];
        Vector3 diffV = (endV - startV);
        Vector3 drawV = endV + diffV * lengthMultiplier * lengthMultiplier * diffV.sqrMagnitude * diffV.sqrMagnitude / 10;
        Debug.DrawLine(startV, drawV, new Color(0f, 0f, 1f), 1f);
    }



    public float aquireONEAngleSampleOnePointONeThreat(Vector3 thePoint, GameObject theThreat)
    {
        Ray threatLookingRay = theThreat.GetComponent<body1>().lookingRay;
        Vector3 lineBetweenThreatAndPoint = thePoint - theThreat.transform.position;
        float theAngle = Vector3.Angle(threatLookingRay.direction, lineBetweenThreatAndPoint);
        return theAngle;
    }


    public float aquireONEDistanceSampleOnePointOneThreat(Vector3 thePoint, GameObject theThreat)
    {
        return (thePoint - theThreat.transform.position).magnitude;
    }



    public void graphField1()
    {
        foreach(spatialData1 thisDataPoint in theData)
        {
            //myData.general8DirectionGraphEXAGGERATED(myData.location, myData.combinedMeasures);
            //Debug.Log("000000000000000000000000000000000");
            //Debug.Log("thisDataPoint.bestPoint[0]:  " + thisDataPoint.bestPoint[0]);
            thisDataPoint.graphBetweenTwoPoints2(thisDataPoint.location, thisDataPoint.bestPoint[0], 1);
        }
    }

    public void graphFieldPartial(int howManyInEachRow, int whichRow)
    {
        int currentIndex = 0;
        foreach (spatialData1 thisDataPoint in theData)
        {

            //myData.general8DirectionGraphEXAGGERATED(myData.location, myData.combinedMeasures);
            if(currentIndex > whichRow*howManyInEachRow && currentIndex < (whichRow * howManyInEachRow + howManyInEachRow))
            {
                thisDataPoint.graphBetweenTwoPoints2(thisDataPoint.location, thisDataPoint.bestPoint[0], 1);
            }
            
            currentIndex++;
        }
    }

    public List<spatialData1> getDataForAllPoints()
    {
        List < spatialData1 > listOfData = new List<spatialData1 >();
        foreach (Vector3 thisPoint in thePoints)
        {
            //create a spatialData1 for this point
            //treat this point as a "center" for the 8 point method in spatialData1
            //gather the data

            //          listOfData.Add(getDataForOnePoint(thisPoint));
            spatialData1 myData = new spatialData1();
            myData.location = theMiddlePoint;
            //                  listOfData.Add(myData.combineVectorForOnePoint2(theMiddlePoint, thisThreatList));
            //myData.combineVectorForOnePoint2(myData.location, threatListWithoutSelf);
        }

        return listOfData;
    }


    public spatialData1 getDataForOnePoint(Vector3 thisPoint)
    {
        spatialData1 myData = new spatialData1();
        if (thisThreatList.Count > 0)
        {

            
            myData.location = thisPoint;

            myData.NEWgatherALLThreatDataTypes(thisThreatList);
            myData.NEWcombine2into1();
            Vector3 myBestPoint = myData.thePoints[myData.whichIndexIsHighest(myData.combinedMeasures)];
            myData.bestPoint.Add(myBestPoint);
            if (myData.bestPoint.Count == 0)
            {
                Debug.Log("no best point found?");
            }
            else
            {
                myData.graphBetweenTwoPoints2(myData.location, myData.bestPoint[0], 0.00000001f);
            }
            
            
        }

        return myData;
    }

    public List<Vector3> generate2Xby2YNearPoints(Vector3 theMiddlePoint, int X, int Y)
    {
        List<Vector3> xByYpointsAroundMiddle = new List<Vector3>();

        //soooo:
        //-1x +1z       0x +1z      +1x +1z
        //
        //-1x 0z       [N/A]      +1x 0z
        //
        //-1x -1z       0x -1z      +1x -1z

        int theXValue = -X;
        int theYValue = -Y;
        while (theXValue < X)
        {
            while (theYValue < Y)
            {
                Vector3 newPoint = theMiddlePoint + new Vector3(theXValue*2, 0, theYValue*2);
                xByYpointsAroundMiddle.Add(newPoint);
                theYValue++;
            }

            theYValue = -Y;
            theXValue++;
        }



        return xByYpointsAroundMiddle;
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
            return -(360*360)/ (theAngle* theAngle);
        }
        
    }


    public Vector3 combineVectorForOnePoint2(Vector3 thisPoint, List<GameObject> theThreatList)
    {
        Vector3 theVector = new Vector3();

        //Debug.Log("thisThreatList.Count:  " + thisThreatList.Count);

        foreach (GameObject thisThreat in theThreatList)
        {

            // distance to threat
            // line of sight to threat
            // angle between where shooter is facing and where the point is
            // proximity to end goal?

            //add vector perpendicular to looking angle?
            //add vector towards or away from threat, depending on angle?

            
            //currentAddedDistance += aquireONEDistanceSampleOnePointOneThreat(aPoint, thisThreat);
            //currentAddedAngles += aquireONEAngleSampleOnePointONeThreat(aPoint, thisThreat);
            if (thisThreat != null)
            {
                if(lineOfSightBool(thisThreat) == false)
                {
                    float currentDistance = aquireONEDistanceSampleOnePointOneThreat(thisPoint, thisThreat);
                    Vector3 perpendicular = perpedicularToOneThreat(thisPoint, thisThreat);
                    theVector += perpendicular * currentDistance;
                }
                
            }

            //float Distance = (thisPoint - thisThreat.transform.position).magnitude;
        }
        //Debug.Log("theVector.z:  " + theVector.z);
        //Debug.Log("thisPoint.z:  " + thisPoint.z);
        //Debug.Log("(thisPoint + theVector).z:  " + (thisPoint + theVector).z);
        
        
        //graphBetweenTwoPoints2(thisPoint, (thisPoint + theVector.normalized), 1f);

        return theVector;
    }

    public bool lineOfSightBool(GameObject thisThreat)
    {
        bool theBool = false;
        RaycastHit myHit;

        Vector3 theDirection = thisThreat.transform.position - location;
        Ray myRay = new Ray(location, theDirection);
        ;

        if (Physics.Raycast(myRay, out myHit, 20f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            theBool = true;
            
        }
        return theBool;
    }

    public Vector3 perpedicularToOneThreat(Vector3 thePoint, GameObject theThreat)
    {
        Vector3 perpendicular = new Vector3();
        Ray threatLookingRay = theThreat.GetComponent<body1>().lookingRay;
        Vector3 lineBetweenThreatAndPoint = thePoint - theThreat.transform.position;

        float theAngle = Vector3.SignedAngle(threatLookingRay.direction, lineBetweenThreatAndPoint, Vector3.up);
        //Debug.Log("theAngle:  " + theAngle);

        if (theAngle < 0f)
        {
            if (theAngle > -120f)
            {
                //Vector3 perpendicular = Quaternion.Euler(0, 90, 0) * threatLookingRay.direction;
                //Vector3 perpendicular = Quaternion.AngleAxis(90, Vector3.up) * threatLookingRay.direction;
                //          perpendicular = Quaternion.AngleAxis(-90, Vector3.up + location) * lineBetweenThreatAndPoint;
                perpendicular = Quaternion.Euler(0, -90, 0) * lineBetweenThreatAndPoint;
                //Debug.Log("perpendicular.z:  " + perpendicular.z);
            }
            else
            {
                //so from -120 tp - 180
                perpendicular = -lineBetweenThreatAndPoint;
                //perpendicular = Quaternion.AngleAxis(-theAngle, Vector3.up + location) * lineBetweenThreatAndPoint;
            }
        }
        else
        {
            if (theAngle < 120f)
            {
                //perpendicular = Quaternion.AngleAxis(90, Vector3.up + location) * lineBetweenThreatAndPoint;
                perpendicular = Quaternion.Euler(0, 90, 0) * lineBetweenThreatAndPoint;
            }
            else
            {
                perpendicular = lineBetweenThreatAndPoint;
                //perpendicular = Quaternion.AngleAxis(theAngle, Vector3.up + location) * lineBetweenThreatAndPoint;
            }
        }





        return perpendicular;
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


    public float aquireONEDistanceSampleOnePointOneThreat(Vector3 thePoint, GameObject theThreat)
    {
        return (thePoint - theThreat.transform.position).magnitude;
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
