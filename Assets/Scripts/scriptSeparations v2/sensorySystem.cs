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
    public worldScript theWorldScript;
    public GameObject target;
    public Ray lookingRay;

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










    private void OnTriggerEnter(Collider other)
    {
        //Debug.DrawRay(this.gameObject.GetComponent<Transform>().position, newVector, Color.white, 0.06f);
        //Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, other.gameObject.GetComponent<Transform>().position, Color.white, 6f);
        sensedObjects.Add(other.gameObject);
    }




    public Vector3 pointerOrigin()
    {
        //couldn't store this relative position, soooo just generate it when needed >.<
        return this.gameObject.transform.position + new Vector3(0, 0, 0.69f);
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

    float crossoverAngle = 110;


    public GameObject targetObject;
    public Vector3 originLocation;
    public Vector3 lineBetweenTargetAndThisPoint = new Vector3();
    public Vector3 towardsTarget = new Vector3();
    public Vector3 awayFromTarget = new Vector3();

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

        //Vector3 awayOrTowardsFinal = towardsIfBehindAwayIfInFront();
        //Vector3 theOrbitFinal = orbitAwayFromLookingLine();

        float orbitCoefficient = 1;
        float entryCoefficient = 1;

        

        if (lookingAngle < crossoverAngle)
        {
            //have i made sure these andles only go between 0 and 180 degrees??  is that the default?

            //entryCoefficient = lookingAngles[currentIndex]/120;
            entryCoefficient = 0;
            orbitCoefficient = 1;

            theOutputVector += (entryCoefficient * awayFromTarget.normalized + orbitCoefficient * lookingAnglePerpendicular.normalized);
        }
        else
        {
            //entryCoefficient = lookingAngles[currentIndex] / 180;
            //orbitCoefficient = (180 - lookingAngles[currentIndex])/180;
            //entryCoefficient = 1;
            //orbitCoefficient = 0;
            entryCoefficient = (lookingAngle - crossoverAngle) / (180 - crossoverAngle);
            orbitCoefficient = (180 - lookingAngle) / (180 - crossoverAngle);
            //Debug.Log("towardsTarget.normalized:  " + towardsTarget.normalized);
            //Debug.Log("-lookingAnglePerpendicular.normalized:  " + -lookingAnglePerpendicular.normalized);
            //Debug.Log("(entryCoefficient * towardsTarget.normalized):  " + (entryCoefficient * towardsTarget.normalized));
            //Debug.Log("(orbitCoefficient * (-lookingAnglePerpendicular.normalized)):  " + (orbitCoefficient * (-lookingAnglePerpendicular.normalized)));
            //Debug.Log("((entryCoefficient * towardsTarget.normalized) + (orbitCoefficient * (-lookingAnglePerpendicular.normalized))):  " + ((entryCoefficient * towardsTarget.normalized) + (orbitCoefficient * (-lookingAnglePerpendicular.normalized))));
            
            
            //entryCoefficient = 1;
            //orbitCoefficient = 0;

            theOutputVector += ((entryCoefficient * (towardsTarget.normalized)) + (orbitCoefficient * orbitCoefficient* orbitCoefficient* orbitCoefficient * (lookingAnglePerpendicular.normalized)));
            
        }



        //Debug.Log("lookingAngle:  " + lookingAngle);
        //Debug.Log("awayFromTarget:  " + awayFromTarget);
        //Debug.Log("towardsTarget:  " + towardsTarget);
        //Debug.Log("lookingAnglePerpendicular:  " + lookingAnglePerpendicular);
        //Debug.Log("theOutputVector:  " + theOutputVector);
        //Debug.Log("lookingAngle:  " + lookingAngle);


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
        if (targetObject == null)
        {
            Debug.Log("gameObject is null");
            
        }

        Ray targetLookingRay = targetObject.GetComponent<sensorySystem>().lookingRay;
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
        Ray threatLookingRay = targetObject.GetComponent<sensorySystem>().lookingRay;
        lineBetweenTargetAndThisPoint = originLocation - targetObject.transform.position;

        //do i have these correct?
        awayFromTarget = lineBetweenTargetAndThisPoint;
        towardsTarget = -lineBetweenTargetAndThisPoint;


        float theAngle = Vector3.SignedAngle(threatLookingRay.direction, lineBetweenTargetAndThisPoint, Vector3.up);
        //Debug.Log("theAngle:  " + theAngle);

        if (theAngle < 0f)
        {
            perpendicular = Quaternion.Euler(0, -90, 0) * lineBetweenTargetAndThisPoint;
        }
        else
        {
            perpendicular = Quaternion.Euler(0, 90, 0) * lineBetweenTargetAndThisPoint;
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
        graphBetweenTwoPoints2(thisPoint, (thisPoint + dontIneedACOMBINEDEndpoint.normalized), 0.5f);
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
                Vector3 fragmentVector = new Vector3();
                fragmentVector = thisFragment.applePattern();
                theOutputVector += fragmentVector;
                
            }
        }

        return theOutputVector;
    }



    //      initializing

    public void initializeDataPoint(List<GameObject> targetList, Vector3 inputPoint)
    {
        thisPoint = inputPoint;

        //printAllIdNumbers(targetList);

        foreach (GameObject thisTarget in targetList)
        {
            spatialDataPointFragment newFragment = new spatialDataPointFragment();
            newFragment.initializeFragment(thisTarget, thisPoint);
            fragmentList.Add(newFragment);
        }
    }


    public void printAllIdNumbers(List<GameObject> listOfGameObjects)
    {

        string X = "printAllIdNumbers:  ";

        foreach (GameObject thisItem in listOfGameObjects)
        {
            X += thisItem + ", ";
        }

        Debug.Log(X);
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
    public List<GameObject> threatList = new List<GameObject>();
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

    public void appleGraph()
    {
        appleField();

        foreach (spatialDataPoint thisDataPoint in theDataSet)
        {

            Vector3 endPointAdditionp = new Vector3();

            endPointAdditionp = thisDataPoint.applePattern().normalized;



            thisDataPoint.graphBetweenTwoPoints2(thisDataPoint.thisPoint, thisDataPoint.thisPoint + endPointAdditionp, 0.1f);
            thisDataPoint.graphBetweenTwoPoints2(thisDataPoint.thisPoint, thisDataPoint.thisPoint + new Vector3(0,0.2f,0), 0.1f);

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
