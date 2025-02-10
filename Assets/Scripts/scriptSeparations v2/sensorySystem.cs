using System;
using System.Collections;
using System.Collections.Generic;

//using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.UI;
//using System.Drawing;
//using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking.Types;
using UnityEngine.Profiling;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static enactionCreator;
using static tagging2;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class sensorySystem : MonoBehaviour
{
    public GameObject target;
    public Ray lookingRay;


    public List<equippable2> toolsWithIntertypeX(interType intertypeX, List<equippable2> equippable2s)
    {
        List < equippable2 > toolsWithIntertypeX = new List < equippable2 >();

        foreach (equippable2 thisTool in equippable2s)
        {
            if (thisTool.containsIntertype(intertypeX)) { toolsWithIntertypeX.Add( thisTool); }
        }


        return toolsWithIntertypeX;
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




public class sensorySystemComponent : MonoBehaviour //callable update?
{
    internal List<sensor> theSenses = new List<sensor>();

    public static sensorySystemComponent addThisComponent(GameObject theObject, sensor sense1)
    {
        sensorySystemComponent theComponent = theObject.AddComponent<sensorySystemComponent>();
        theComponent.theSenses.Add(sense1);


        return theComponent;
    }

    void Update()
    {
        foreach (sensor sensor in theSenses)
        {
            sensor.sense();
        }
    }
}


public interface sensor
{
    //the specifics of what they sense, how they sense it is left up to other classes implementing this interface
    //i guess they also decide what to do with the resulting data, where to put it, or impacts/effects
    
    
    //for collision-based sensing, just use "on collision" to update a set [or set grabber] which will AFTERWARDS be filtered further by "sense" and the results shared to beleifs?
    void sense();
}

public class visualSensor1 : sensor
{
    private Transform theVisualSenseApparatus;  //hmmm, should be built-in to the criteria etc?
    
    public objectSetGrabber baseSetGrabberBeforeSensing;  //just a list to "whittle down", sort, etc
    private objectCriteria basicSensingFilterCriteria;  //use this to FILTER before bothering with more complex computations of "detectability"?  include a condition/criteria to check if the object has "request stealth" set to true ...no that will be a pivot criteria, a "do full computation" criteria
    private objectCriteria advancedSensingAdditionalFilterCriteria;
    private objectEvaluator detectabilityEvaluator;  //the more complex calculations will likely go here?
    //private objectSetGrabber setOfSensedObjects;  //is this a good way to do things???????  buuuut it's "boolean"?  well, ya, a cutoff of evaluation float numbers either adds it here or doesn't.  you don't half-react to a threat....or do you?  if you are unsure WHAT it is, etc.......that's INTERPRETATION, happens LATER.  but does mean we may want to output/store the evaluator float, and any other data, not just the objects
    //public sensoryOutput theOutput;//????
    private beleifs theBeleifs;
    float illuminationIntensityThresholdForDetection = 0.017f;

    private tag2 excludeThisTag;

    public visualSensor1(GameObject theObject, beleifs theBeleifsIn, tag2 excludeThisTagIn, float theVisualRangeIn = 60f)
    {
        theVisualSenseApparatus = theObject.transform;
        excludeThisTag = excludeThisTagIn;
        theBeleifs = theBeleifsIn;
        baseSetGrabberBeforeSensing = new setOfAllObjectsInZone(theObject);
        basicSensingFilterCriteria = new objectMeetsAllCriteria(
            new reverseCriteria(new objectHasTag(excludeThisTagIn)),
            new lineOfSight(theObject, theVisualRangeIn));
        advancedSensingAdditionalFilterCriteria = new requestingStealthCriteria();
        detectabilityEvaluator = new detectabilityIlluminationEvaluator1(theObject);
    }

    public void sense()
    {
        //detection:
        //    BEFORE any complex calculations are run:
        //	run FILTER conditions[can either result in simple detection, or complete ignorance]
        //        line of sight for vision[then distance, then feild of view]
        //        boolean "request stealth" on target / "theif"
        //THEN[if requersted] run some more advanced light / dark / camoflage code[in an "interface" plug -in]
        //    for now, collisions with light - source volumes[like with map zones] and simultaneous distance / line of sight[from mlultiple body parts] to lights AND guard's vision

        List<GameObject> newList = new List<GameObject>();


        foreach (GameObject thisObject in baseSetGrabberBeforeSensing.grab())
        {
            //basic filter [line of sight]
            //then choice of what next
            //      either nothing, auto-detection,
            //      or (if "requested"), stealth cmputatiions

            //Debug.Log("foreach");

            if (basicSensingFilterCriteria.evaluateObject(thisObject) == false) { continue; }


            //Debug.Log("basicSensingFilterCriteria met");

            if (advancedSensingAdditionalFilterCriteria.evaluateObject(thisObject) == false) { continue; }

            //Debug.Log("advancedSensingAdditionalFilterCriteria met");

            float intensity = detectabilityEvaluator.evaluateObject(thisObject);
            Debug.Log("intensity:  " + intensity);
            Debug.DrawLine(theVisualSenseApparatus.position, thisObject.transform.position, new Color(intensity, intensity, intensity), 20f);

            if (intensity < illuminationIntensityThresholdForDetection)
            {

                continue;
            }

            //newList.Add(tagging2.singleton.idPairGrabify(thisObject));
            theBeleifs.sensoryInput(newList);
        }
    }
}



public class detectabilityIlluminationEvaluator1 : objectEvaluator
{
    objectSetGrabber lightSourceGrabber;

    public detectabilityIlluminationEvaluator1(GameObject theObjectWeUseForZone)
    {
        /*
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new objectHasTag(tag2.lightSource)
            );

        lightSourceGrabber = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsInZone(theObjectWeUseForZone), theCriteria);
        */

        lightSourceGrabber = new setOfAllObjectsWithTag(tag2.lightSource);
    }


    public override float evaluateObject(GameObject theObject)
    {
        //so:
        //      [assume line of sight to the observer is already established]
        //      AND at least one light source,
        //      within range of both

        float totalIllumination = 0f;

        //Debug.Log(",,,,,,,,,,,,,,,,,,,,,,,,evaluateObject");
        foreach (GameObject thisLight in lightSourceGrabber.grab())
        {
            //Debug.Log("thisLight:  " + thisLight);
            float intensityToAdd = thisLight.GetComponent<lightIlluminationCalculator>().evaluate(theObject);
            //Debug.Log("intensityToAdd:  " + intensityToAdd);
            totalIllumination += intensityToAdd;
        }


        Debug.Log("^^^^^^^^^^^totalIllumination:  " + totalIllumination);
        return totalIllumination;
    }
}



public class stealthModule : MonoBehaviour
{
    public bool requestingStealth = false;
    public List<GameObject> evaluatableParts = new List<GameObject>();


}




public class requestingStealthCriteria : objectCriteria
{
    public override bool evaluateObject(GameObject theObject)
    {
        stealthModule theComponent = theObject.GetComponent<stealthModule>();
        if (theComponent == null) { return false; }
        return theComponent.requestingStealth;
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


        float orbitCoefficient = 1;
        float entryCoefficient = 1;

        

        if (lookingAngle < crossoverAngle)
        {
            //have i made sure these andles only go between 0 and 180 degrees??  is that the default?

            entryCoefficient = 0;
            orbitCoefficient = 1;

            theOutputVector += (entryCoefficient * awayFromTarget.normalized + orbitCoefficient * lookingAnglePerpendicular.normalized);
        }
        else
        {
            entryCoefficient = (lookingAngle - crossoverAngle) / (180 - crossoverAngle);
            orbitCoefficient = (180 - lookingAngle) / (180 - crossoverAngle);
            theOutputVector += ((entryCoefficient * (towardsTarget.normalized)) + (orbitCoefficient * orbitCoefficient* orbitCoefficient* orbitCoefficient * (lookingAnglePerpendicular.normalized)));
        }


        return theOutputVector;
    }

    public Vector3 simpleRadialPattern()
    {

        //generalFragmentData1();



        //Ray threatLookingRay = targetObject.GetComponent<sensorySystem>().lookingRay;
        //Ray targetLookingRay = new Ray(targetObject.transform.position, targetObject.transform.forward);//targetObject.GetComponent<sensorySystem>().lookingRay;
        lineBetweenTargetAndThisPoint = originLocation - targetObject.transform.position;

        //do i have these correct?
        awayFromTarget = lineBetweenTargetAndThisPoint;

        return awayFromTarget;
    }

    //      initializing

    public spatialDataPointFragment(GameObject target, Vector3 location)
    {
        //should just be done in constructor?
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

        Ray targetLookingRay = new Ray(targetObject.transform.position, targetObject.transform.forward);//targetObject.GetComponent<sensorySystem>().lookingRay;
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
        //Ray threatLookingRay = targetObject.GetComponent<sensorySystem>().lookingRay;
        Ray targetLookingRay = new Ray(targetObject.transform.position, targetObject.transform.forward);//targetObject.GetComponent<sensorySystem>().lookingRay;
        lineBetweenTargetAndThisPoint = originLocation - targetObject.transform.position;

        //do i have these correct?
        awayFromTarget = lineBetweenTargetAndThisPoint;
        towardsTarget = -lineBetweenTargetAndThisPoint;


        float theAngle = Vector3.SignedAngle(targetLookingRay.direction, lineBetweenTargetAndThisPoint, Vector3.up);
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
}

public class spatialDataPoint
{
    //this code should only ever be about one single point
    //anything plural [like "distances"] should still be about one sample point, and how far away other things [like threats] are from that one point

    public List<spatialDataPointFragment> fragmentList = new List<spatialDataPointFragment>();

    public int listLenghts = 0;


    public float combinedMeasures = 0;  //hmmmm....don't i want a VECTOR i can GRAPH?
    public Vector3 thisPoint;
    public Vector3 dontIneedACOMBINEDEndpoint;



    public bool debugPrint = false;


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
        Vector3 endV = whereToEnd;

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

        //          if (debugPrint == true) { Debug.Log("fragmentList.Count:  " + fragmentList.Count); }

        foreach (spatialDataPointFragment thisFragment in fragmentList)
        {
            if (debugPrint == true) { Debug.Log("thisFragment.lineOfSightBool:  " + thisFragment.lineOfSightBool); }
            if (thisFragment.lineOfSightBool == true)
            {
                Vector3 fragmentVector = new Vector3();
                fragmentVector = thisFragment.applePattern();

                if (debugPrint == true) { Debug.Log("fragmentVector:  " + fragmentVector); }
                theOutputVector += fragmentVector;

                if (debugPrint == true) { Debug.Log("theOutputVector:  " + theOutputVector); }
            }
            
        }

        return theOutputVector;
    }

    public Vector3 weightedRadialPattern()
    {
        //straight away from threats
        //their "center of mass"???
        //weight nearer ones more heavily???



        //well, how to do "weighting?"
        //      set all relative to nearest one and furthest one?  normalize that?
        //      hmm, nah, absolute value can matter more than relative?
        //      and "furthest away" might be just a few CENTIMETERS further away, hardly worth setting to "zero" weight!
        // just divide by distance
        // and maybe multiply by an offset or whatever




        Vector3 theOutputVector = new Vector3();


        //generalPointData1();

        //          if (debugPrint == true) { Debug.Log("fragmentList.Count:  " + fragmentList.Count); }

        foreach (spatialDataPointFragment thisFragment in fragmentList)
        {
            if (thisFragment.lineOfSightBool == false) { continue; }


            Vector3 fragmentVector = new Vector3();
            fragmentVector = thisFragment.simpleRadialPattern();

            float distance = thisFragment.distanceAsFloat;

            theOutputVector += fragmentVector.normalized/distance;
        }

        return theOutputVector;
    }



    //      initializing

    public spatialDataPoint(List<GameObject> targetList, Vector3 inputPoint)
    {
        //should just be done in constructor?
        thisPoint = inputPoint;

        //printAllIdNumbers(targetList);

        foreach (GameObject thisTarget in targetList)
        {
            spatialDataPointFragment newFragment = new spatialDataPointFragment(thisTarget, thisPoint);
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

    internal bool threatLineOfSightBool()
    {
        foreach (spatialDataPointFragment thisFragment in fragmentList)
        {
            if(thisFragment.lineOfSightBool == true) {return true;}
        }
        return false;
    }
}

public class spatialDataSet
{
    //this code should be about using more than one "spatialDataPoint" class object

    public List<spatialDataPoint> theDataSet = null;// new List<spatialDataPoint>();

    public List<Vector3> thePoints = new List<Vector3>();
    public List<GameObject> threatList = new List<GameObject>();
    public List<float> combinedData;
    public Vector3 middlePoint;




    //      graphing

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
            spatialDataPoint aDataPoint = new spatialDataPoint(threatList, thisPoint);
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
        spatialDataPoint middleDataPoint = new spatialDataPoint(threatList, middlePoint);

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
