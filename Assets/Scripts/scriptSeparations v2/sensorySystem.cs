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
using static UnityEditor.ShaderData;
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



public class findRelativeShadowBrightnessPercentage
{
    public float calculate(Transform theViewer, float viewerSightRange, GameObject theShadowcasterObject, List<lightIlluminationCalculator> theLights)
    {
        //100% means no shadow, same brigthness. 95% is faint shadow, not much difference in brightness.
        //then return as decimal instead of "percent"

        //raycast from armature parts away from light sources it has line of sight to
        //get point on environment or whatever where that shadow would fall
        //calculate:
        //      current illumination of that shadow point
        //      calculate what illumination that point WOULD have if it wasn't for this current individual light source...ehhh but what if it's a bunch of dim lights clustered together making same shadow?  do i need to do all together???  how?  hmmm...
        //          maybe for each "shadow point" we DO need to construct from each light source?  wait, WOULD the above problem cause a false result EVER?  well, faint ones makes relative small....umm...
        //          RIGHT!  just add up the relative difference for ALL light sources.  then that FINAL relative difference is the answer? will it always be between zero and one?  will it all be the same scale?  
        //i dunno, could i cut corners for now and just do a bool, not a float?

        //hmmm, but i don't have a way to calculate hypothetical lighting.  i need a way to calculate what it WOULD be with no shadows.  easy to add to lights?
        //buuuut no, it's not "without shadows".  other shadows matter!  just not the shadows of the stealth object.  hmm.
        //so, make an "evaluate EXCLUDING input object"?  i guess that's what i need...
        //mmm, but, as with their pathfinding, i don't know how to disable one object from raycasts.
        //soo, have to FIRST ensure light DOES hit the armature object?  ya.  of course.  ok

        //so:
        //      make sure there is line of sight from light to armature object [or did i do that in a previous filter step?]
        //      calculate "shadowless"
        //      calculate WITH shadow
        //      get percent of one from the other
        //      do that for all lights, add up the percentages?  do these percentages add up?  just assume they do
        //      return aggregate percent [as decimal], ya.

        //wait, i'm adding them UP starting from ZERO?
        //so, invisible faint shadow will return a result closer to zero than to 1?
        //aaaaand can't do percent, because shadow can have zero illumination.  just add illumination?  why was i bothering with percent?

        Vector3 shadowPoint;
        float totalBrightnessChange = 0;
        float illuminationWithShadow = 0;
        float illuminationWITHOUTShadow = 0;

        foreach (var light in theLights)
        {
            //Debug.Log(":::::::::::::::::::::::::::::::::::::::");
            if (light.boolIllumination(theShadowcasterObject) == false) { continue; }

            shadowPoint = light.shadowPoint(theShadowcasterObject);
            //Debug.Log("shadowPoint :  " + shadowPoint);
            if (shadowPoint == new Vector3(43210, -1234, -5678)) { continue; }  //this means "null"

            //draw line to shadow point:
            //      Debug.DrawLine(theViewer.transform.position, shadowPoint,Color.magenta,2f);

            //need line of sight from viewer to shadow!
            bool theLineOfSightFromViewerToShadow = new lineOfSight(theViewer.gameObject, viewerSightRange).evaluatePosition(shadowPoint);
            //Debug.Log("theLineOfSightFromViewerToShadow :  " + theLineOfSightFromViewerToShadow);
            if (theLineOfSightFromViewerToShadow == false) { continue; }

            illuminationWithShadow = light.evaluate(shadowPoint);
            illuminationWITHOUTShadow = light.evaluateShadowless(shadowPoint);

            //Debug.Log("illuminationWithShadow :  " + illuminationWithShadow);
            //Debug.Log("illuminationWITHOUTShadow :  " + illuminationWITHOUTShadow);
            totalBrightnessChange += illuminationWITHOUTShadow-illuminationWithShadow;
            //Debug.Log("totalBrightnessChange :  " + totalBrightnessChange);
        }

        //Debug.Log("----------totalBrightnessChange :  " + totalBrightnessChange);
        return totalBrightnessChange;
    }
    
}

public class findSilhouetteBrightness
{
    public float calculate(Transform theViewer, GameObject theDarkObject, List<lightIlluminationCalculator> theLights)
    {
        //so:
        //      [assume line of sight to the observer is already established]
        //      AND at least one light source,
        //      within range of both?


        RaycastHit hitOffTheEdgeOfDarkObject = findAfterEdgeHit(theViewer.transform.position, theDarkObject);

        float totalIllumination = evaluatePosition(hitOffTheEdgeOfDarkObject.point, theLights);


        //Debug.Log("^^^^^^^^^^^totalIllumination:  " + totalIllumination);
        return totalIllumination;
    }

    private RaycastHit findAfterEdgeHit(Vector3 observerPosition, GameObject theDarkObject)
    {
        RaycastHit myHit = new RaycastHit();

        
        Vector3 theDirection = theDarkObject.transform.position - observerPosition;
        Vector3 offsetDirection = horizontalPerpendicular(observerPosition, theDarkObject.transform.position).normalized;


        int tries = 20;
        int currentTry = 0;
        float initialOffsetStep = 0.01f;
        int directionChanger = 1;

        Vector3 offset = offsetDirection * (initialOffsetStep + (initialOffsetStep * currentTry * currentTry));
        theDirection = (theDarkObject.transform.position+ offset) - observerPosition;
        Ray myRay = new Ray(observerPosition, theDirection);

        while (currentTry < tries)
        {
            Physics.Raycast(myRay, out myHit, 1000, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

            if(myHit.collider == null || myHit.collider.gameObject != theDarkObject)
            {
                //now, bisect [or other numerical methods] until edge threshhold is razor thin
                Vector3 previousOffset = offsetDirection * (initialOffsetStep + (initialOffsetStep * (currentTry-1) * (currentTry - 1)*-directionChanger));

                return findHitAfterEdgeUsingBisection(theDarkObject, observerPosition, offset, previousOffset);
            }

            currentTry++;
            directionChanger = -directionChanger;
            offset = offsetDirection * (initialOffsetStep + (initialOffsetStep * currentTry * currentTry* directionChanger)); //double negative for reversal
            theDirection = (theDarkObject.transform.position + offset) - observerPosition;
            myRay = new Ray(observerPosition, theDirection);
        }

        return myHit;
    }



    private Vector3 findMidpoint(Vector3 startPoint, Vector3 endpoint)
    {
        Vector3 lineBetween = endpoint - startPoint;
        float spanLength = lineBetween.magnitude / 2;
        Vector3 span = (lineBetween.normalized * spanLength);
        Vector3 midpoint = startPoint + span;

        return midpoint;
    }



    private RaycastHit findHitAfterEdgeUsingBisection(GameObject theDarkObject, Vector3 observerPosition, Vector3 offset, Vector3 previousOffset)
    {
        RaycastHit myHit = new RaycastHit();

        int tries = 20;
        int currentTry = 0;



        //so.  what do we do?
        //      test midpoint
        //          if its the input object, test next midpoint between that point and the point that didn't hit that object
        //          if not, test between it and the one that DID hit the object
        //      measure remaining gap
        //      stop after number of tries OR gap becomes negligible in magnitude
        Vector3 thePointThatDIDHitTheDarkObject = theDarkObject.transform.position + previousOffset;
        Vector3 thePointThatWentPastTheEdge = theDarkObject.transform.position + offset;



        while (currentTry < tries)
        {
            Vector3 midpoint = findMidpoint(thePointThatDIDHitTheDarkObject, thePointThatWentPastTheEdge);
            //Vector3 offset = offsetDirection * (initialOffsetStep + (initialOffsetStep * currentTry * currentTry));
            Vector3 theDirection = midpoint - observerPosition;
            Ray myRay = new Ray(observerPosition, theDirection);

            Physics.Raycast(myRay, out myHit, 1000, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

            if (myHit.collider == null || myHit.collider.gameObject != theDarkObject)
            {
                //so we went past the edge
                //so this will be our NEW "went past the edge" point:
                thePointThatWentPastTheEdge = midpoint;

                //now repeat
            }
            else
            {
                //so we hit the dark object
                //so this will be our NEW "hit dark object" point:
                thePointThatDIDHitTheDarkObject = midpoint;

                //now repeat
            }


            if (successfulEdgeEstimation(thePointThatWentPastTheEdge, thePointThatDIDHitTheDarkObject) == true) 
            { 
                return myHit; 
            }

            currentTry++;
        }


        return myHit;
    }

    private bool successfulEdgeEstimation(Vector3 thePointThatWentPastTheEdge, Vector3 thePointThatDIDHitTheDarkObject)
    {
        //see if distance between the two points is smaller than the desired threshhold:
        float threshhold = 0.01f;

        return ((thePointThatWentPastTheEdge- thePointThatDIDHitTheDarkObject).magnitude < threshhold);
    }

    private Vector3 horizontalPerpendicular(Vector3 startPoint, Vector3 endPoint)
    {
        //https://docs.unity3d.com/2019.3/Documentation/Manual/ComputingNormalPerpendicularVector.html
        Vector3 perpendicular = new Vector3();

        perpendicular = Vector3.Cross(endPoint - startPoint, Vector3.up);

        return perpendicular;
    }

    public float evaluatePosition(Vector3 thePosition, List<lightIlluminationCalculator> theLights)
    {
        float totalIllumination = 0f;




        //Debug.Log(",,,,,,,,,,,,,,,,,,,,,,,,evaluateObject");
        foreach (lightIlluminationCalculator thisLight in theLights)
        {
            //Debug.Log("thisLight:  " + thisLight);
            float intensityToAdd = thisLight.evaluate(thePosition);
            //Debug.Log("intensityToAdd:  " + intensityToAdd);
            totalIllumination += intensityToAdd;
        }


        //so:
        //      [assume line of sight to the observer is already established]
        //      AND at least one light source,
        //      within range of both


        /*

        float totalIllumination = 0f;

        //Debug.Log(",,,,,,,,,,,,,,,,,,,,,,,,evaluateObject");
        foreach (GameObject thisLight in lightSourceGrabber.grab())
        {
            //Debug.Log("thisLight:  " + thisLight);
            float intensityToAdd = thisLight.GetComponent<lightIlluminationCalculator>().evaluate(thePosition);
            //Debug.Log("intensityToAdd:  " + intensityToAdd);
            totalIllumination += intensityToAdd;
        }


        */


        //Debug.Log("^^^^^^^^^^^totalIllumination:  " + totalIllumination);
        return totalIllumination;
    }
}


















public class boolDataSet
{
    List<Vector3> spatialPointSet = new List<Vector3>();
    List<bool> boolSet = new List<bool>();

    public void add(Vector3 point, bool theBool)
    {
        spatialPointSet.Add(point);
        boolSet.Add(theBool);
    }
}


public class spatialDataSet
{
    //want to be able to:
    //      have specified pattern of points [grid, line, circle/radial, etc?]?
    //      gather multiple types of data [perhaps with dependencies between them] at each point
    //      somehow be useable [but hard to "return" values of different types like bool, vector, float, etc]
    //          welllll, just need different "spatial datum" types?  like regular data types?  something?
    //ok, you plug in the "sample procedure" that computes stuff.  like a sensor.  separate from the set of points themselves.
    //then ya it returns a certain regular list of variables of a certain regular data type?

    List<Vector3> spatialPointSet = new List<Vector3>();

    public spatialDataSet(List<Vector3> spatialPointSetIn)
    {
        spatialPointSet = spatialPointSetIn;
    }

    public List<bool> sample(boolSampleProcedure theSampleProcedure)
    {
        return theSampleProcedure.sample(spatialPointSet);
    }
    public List<float> sample(floatSampleProcedure theSampleProcedure)
    {
        return theSampleProcedure.sample(spatialPointSet);
    }
    public List<Vector3> sample(vectorSampleProcedure theSampleProcedure)
    {
        return theSampleProcedure.sample(spatialPointSet);
    }
}


public class spatialDataPoint
{

}

public class spatialDatum
{
    //well, 3 types?
    //      bool
    //      float
    //      vector
    //???
    //orrr don't use this
}



/*
public interface sampleProcedure
{
    //........not even needed?  can have just totally separate classes????????
}
*/

public interface boolSampleProcedure// : sampleProcedure
{
    List<bool> sample(List<Vector3> spatialPointSet);

    bool sampleOne(Vector3 spatialPoint);
}


public interface floatSampleProcedure// : sampleProcedure
{
    List<float> sample(List<Vector3> spatialPointSet);
}


public interface vectorSampleProcedure// : sampleProcedure
{
    List<Vector3> sample(List<Vector3> spatialPointSet);
}





public class debugField
{
    public debugField(List<Vector3> spatialPointSet, List<bool> theSamples, float duration = 14f)
    {
        int index = 0;
        Color theColor;
        foreach (var spatialPoint in spatialPointSet)
        {
            if (theSamples[index] == true)
            {
                theColor = Color.green;
            }
            else
            {
                theColor = Color.red;
            }

            //Debug.Log("duration:  "+duration);
            //Debug.DrawLine(new Vector3(), spatialPoint, Color.blue, duration);
            Debug.DrawLine(spatialPoint, spatialPoint + Vector3.up*3, theColor, duration);
            index++;
        }
    }
}

public class debugFieldUpdateable: MonoBehaviour
{
    internal boolSampleProcedure theSampleProcedure;
    private List<Vector3> spatialPointFieldShape;

    internal static debugFieldUpdateable addThisComponent(GameObject theObject, boolSampleProcedure theSampleProcedureIn)
    {
        debugFieldUpdateable theComponent = theObject.AddComponent<debugFieldUpdateable>();
        theComponent.theSampleProcedure = theSampleProcedureIn;
        theComponent.spatialPointFieldShape = new gridOfPoints(new Vector3(-60,-3,-60),28, 28, 2f, 2f).returnIt();

        return theComponent;
    }

    void Update() 
    { 
        List<Vector3> currentPoints = new relativePointSet(transform.position, spatialPointFieldShape);
        new debugField(currentPoints, theSampleProcedure.sample(currentPoints), 0.00001f);
        //theSampleProcedure.sample(currentPoints);
    }
}

public abstract class spatialDataSampleProcedure
{
    public List<bool> sample(List<Vector3> spatialPointSet)
    {
        List<bool> newList = new List<bool>();

        foreach (Vector3 thisPoint in spatialPointSet)
        {
            newList.Add(sampleOnePoint(thisPoint));
        }

        //      new debugField(spatialPointSet, newList);
        return newList;
    }

    public abstract bool sampleOnePoint(Vector3 spatialPoint);

}

public abstract class stealthArmaturableSampleProcedure: spatialDataSampleProcedure
{
    internal objectSetGrabber theSet;
    //List<Vector3> setOfOffsets = new List<Vector3>();
    internal stealthArmature theArmature;

    /*
    public lineOfSightFromASetMember(objectSetGrabber theSetIn)
    {
        theSet = theSetIn;
        //setOfOffsets.Add(new Vector3());
    }
    public lineOfSightFromASetMember(objectSetGrabber theSetIn, stealthArmature theArmatureIn)
    {
        theSet = theSetIn;
        theArmature = theArmatureIn;
    }
    */


    public override bool sampleOnePoint(Vector3 spatialPoint)
    {
        if (theArmature == null) { return oneSubPoint(spatialPoint); }

        foreach (GameObject thisArmatureNode in theArmature.theListOfParts)
        {
            if (oneSubPoint(spatialPoint + armatureNodeOffset(thisArmatureNode)) == true) { return true; }
        }


        return false;
    }

    public bool sampleOne(Vector3 spatialPoint)
    {
        return sampleOnePoint(spatialPoint);
    }

    internal abstract bool oneSubPoint(Vector3 subPoint);

    private Vector3 armatureNodeOffset(GameObject thisArmatureNode)
    {
        return thisArmatureNode.transform.localPosition;
    }
}

public class lineOfSightFromASetMember : stealthArmaturableSampleProcedure, boolSampleProcedure
{
    //"true" = yes, line of sight, visible

    public lineOfSightFromASetMember(objectSetGrabber theSetIn)
    {
        theSet = theSetIn;
        //setOfOffsets.Add(new Vector3());
    }
    public lineOfSightFromASetMember(objectSetGrabber theSetIn, stealthArmature theArmatureIn)
    {
        theSet = theSetIn;
        theArmature = theArmatureIn;
    }

    internal override bool oneSubPoint(Vector3 subPoint)
    {
        //"true" = yes, line of sight, visible

        //Debug.Log("theSet.grab().Count:  " + theSet.grab().Count);
        foreach (GameObject thisObserver in theSet.grab())
        {
            //Debug.Log("thisObserver:  " + thisObserver);
            if (baseCalculation(subPoint, thisObserver.transform.position) == true) { return true; }
        }

        return false;
    }
    internal bool baseCalculation(Vector3 subPoint, Vector3 observerPosition)
    {
        //"true" = yes, line of sight, visible
        RaycastHit myHit;

        //new Ray(this.transform.position, theBody.theWorldScript.theTagScript.semiRandomUsuallyNearTargetPickerFromList(theBody.theLocalMapZoneScript.theList, this.gameObject).transform.position);
        Vector3 theDirection = subPoint-observerPosition;
        Ray myRay = new Ray(observerPosition, theDirection);

        //we don;t have objects for collision, soooo just see if LENGTH of the ray goes full distance???  or if NULL collider, ya
        //Debug.Log("theDirection.magnitude:  "+ theDirection.magnitude);
        if (Physics.Raycast(myRay, out myHit, theDirection.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log("myHit:  " + myHit);
            //Debug.Log("myHit.collider:  " + myHit.collider);
            //Debug.Log("myHit.transform:  " + myHit.transform);
            //Debug.Log("myHit.transform.gameObject:  " + myHit.transform.gameObject);
            //Debug.DrawLine(observerPosition, observerPosition+ theDirection,Color.yellow,3f);
            return false; 
        }

        return true;
    }

}

public class meetsIlluminationThreshhold : stealthArmaturableSampleProcedure, boolSampleProcedure
{
    float illuminationIntensityThresholdForDetection = 0.15f;
    private detectabilityIlluminationEvaluator1 detectabilityEvaluator;

    public meetsIlluminationThreshhold(GameObject objectForZone)
    {

        detectabilityEvaluator = new detectabilityIlluminationEvaluator1(objectForZone);
    }

    public meetsIlluminationThreshhold(GameObject objectForZone, float illuminationIntensityThresholdForDetectionIn) : this(objectForZone)
    {
        illuminationIntensityThresholdForDetection = illuminationIntensityThresholdForDetectionIn;
    }

    internal override bool oneSubPoint(Vector3 subPoint)
    {
        //"true" = yes, line of sight, visible

        //Debug.Log("theSet.grab().Count:  " + theSet.grab().Count);

        if (baseCalculation(subPoint) == true) { return true; }

        return false;
    }
    internal bool baseCalculation(Vector3 subPoint)
    {
        float intensity = detectabilityEvaluator.evaluatePosition(subPoint);
        //Debug.Log("intensity:  " + intensity);
        //Debug.DrawLine(theVisualSenseApparatus.position, thisPart.transform.position, new Color(intensity, intensity, intensity), 20f);

        return (intensity > illuminationIntensityThresholdForDetection);
    }
}


public class visibleToThreatSet : stealthArmaturableSampleProcedure, boolSampleProcedure
{
    float illuminationIntensityThresholdForDetection = 0.0015f;
    private detectabilityIlluminationEvaluator1 detectabilityEvaluator;

    public visibleToThreatSet(GameObject objectForZone, objectSetGrabber theSetIn)
    {
        theSet = theSetIn;
        detectabilityEvaluator = new detectabilityIlluminationEvaluator1(objectForZone);
    }

    public visibleToThreatSet(GameObject objectForZone, objectSetGrabber theSetIn, float illuminationIntensityThresholdForDetectionIn) : this(objectForZone, theSetIn)
    {
        illuminationIntensityThresholdForDetection = illuminationIntensityThresholdForDetectionIn;
    }

    internal override bool oneSubPoint(Vector3 subPoint)
    {
        //"true" = yes, line of sight, visible


        //      Debug.Log("theSet.grab().Count:  " + theSet.grab().Count);
        if (oneSubPointLineOfSightFilter(subPoint) == false) { return false; }

        if (baseCalculation(subPoint) == true) { return true; }

        return false;
    }
    
    internal bool baseCalculation(Vector3 subPoint)
    {
        float intensity = detectabilityEvaluator.evaluatePosition(subPoint);
        //Debug.Log("intensity:  " + intensity);
        //Debug.DrawLine(theVisualSenseApparatus.position, thisPart.transform.position, new Color(intensity, intensity, intensity), 20f);

        return (intensity > illuminationIntensityThresholdForDetection);
    }



    internal bool oneSubPointLineOfSightFilter(Vector3 subPoint)
    {
        //"true" = yes, line of sight, visible

        //Debug.Log("theSet.grab().Count:  " + theSet.grab().Count);
        foreach (GameObject thisObserver in theSet.grab())
        {
            //Debug.Log("thisObserver:  " + thisObserver);
            if (baseCalculationLineOfSightFilter(subPoint, thisObserver.transform.position) == true) { return true; }
        }

        return false;
    }
    
    internal bool baseCalculationLineOfSightFilter(Vector3 subPoint, Vector3 observerPosition)
    {
        //"true" = yes, line of sight, visible
        RaycastHit myHit;

        //new Ray(this.transform.position, theBody.theWorldScript.theTagScript.semiRandomUsuallyNearTargetPickerFromList(theBody.theLocalMapZoneScript.theList, this.gameObject).transform.position);
        Vector3 theDirection = subPoint - observerPosition;
        Ray myRay = new Ray(observerPosition, theDirection);

        //Debug.DrawLine(observerPosition, subPoint, Color.cyan, 100f);

        //we don;t have objects for collision, soooo just see if LENGTH of the ray goes full distance???  or if NULL collider, ya
        //Debug.Log("theDirection.magnitude:  "+ theDirection.magnitude);
        if (Physics.Raycast(myRay, out myHit, theDirection.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log("myHit:  " + myHit);
            //Debug.Log("myHit.collider:  " + myHit.collider);
            //Debug.Log("myHit.transform:  " + myHit.transform);
            //Debug.Log("myHit.transform.gameObject:  " + myHit.transform.gameObject);
            //Debug.DrawLine(observerPosition, observerPosition+ theDirection,Color.yellow,3f);
            return true;
        }

        return false;
    }


}



//visibleToThreatSetUsingVisualSensor1  hmmm, gonna need way to predict then evaluate IMAGINARY silhouette and shadow!!!












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
    float illuminationIntensityThresholdForDetection = 0.15f;//0.017f;  //also eventually make it harder to see dark areas if eyes adjusted to sunlight!!!//eventually can lower this if i get "wearing dark clothes" to make a difference?
    private Transform theVisualSenseApparatus;  //hmmm, should be built-in to the criteria etc?
    
    public objectSetGrabber baseSetGrabberBeforeSensing;  //just a list to "whittle down", sort, etc
    private objectCriteria basicTagProxAndFOVFilterCriteria;  //use this to FILTER before bothering with more complex computations of "detectability"?  include a condition/criteria to check if the object has "request stealth" set to true ...no that will be a pivot criteria, a "do full computation" criteria
    private objectCriteria lineOfSightCriteria;
    private objectCriteria lineOfSightToShadowCriteria;
    private objectCriteria visibleSilhouetteCriteria;
    private objectCriteria advancedSensingRequestingStealthCriteria;
    private objectEvaluator objectIlluminationDetectabilityEvaluator;  //the more complex calculations will likely go here?
    //private objectSetGrabber setOfSensedObjects;  //is this a good way to do things???????  buuuut it's "boolean"?  well, ya, a cutoff of evaluation float numbers either adds it here or doesn't.  you don't half-react to a threat....or do you?  if you are unsure WHAT it is, etc.......that's INTERPRETATION, happens LATER.  but does mean we may want to output/store the evaluator float, and any other data, not just the objects
    //public sensoryOutput theOutput;//????
    private beleifs theBeleifs;
    private perceptions thePerceptions;
    
    private tag2 excludeThisTag;

    float umDontIHaveARANGE = 450f;

    public visualSensor1(GameObject theObject, beleifs theBeleifsIn, tag2 excludeThisTagIn, float theVisualRangeIn = 60f)
    {
        theVisualSenseApparatus = theObject.transform;
        excludeThisTag = excludeThisTagIn;
        theBeleifs = theBeleifsIn;
        thePerceptions = theObject.GetComponent<perceptions>();
        baseSetGrabberBeforeSensing = new setOfAllObjectsInZone(theObject);
        basicTagProxAndFOVFilterCriteria = new objectMeetsAllCriteria(
            new reverseCriteria(new objectHasTag(excludeThisTagIn)),
            new proximityCriteriaBool(theObject, theVisualRangeIn),
            new objectVisibleInFOV(theObject.transform)
            ); 
        lineOfSightCriteria = new lineOfSight(theObject, theVisualRangeIn);
        advancedSensingRequestingStealthCriteria = new requestingStealthCriteria();
        objectIlluminationDetectabilityEvaluator = new detectabilityIlluminationEvaluator1(theObject);
    }

    public void sense()
    {
        //detection:
        //    BEFORE any complex calculations are run:
        //	run FILTER conditions[can either result in simple detection, or complete ignorance]
        //        line of sight for vision[then distance, then field of view]
        //        boolean "request stealth" on target / "theif"
        //THEN[if requersted] run some more advanced light / dark / camoflage code[in an "interface" plug -in]
        //    for now, collisions with light - source volumes[like with map zones] and simultaneous distance / line of sight[from mlultiple body parts] to lights AND guard's vision

        List<GameObject> newList = new List<GameObject>();

        List < lightIlluminationCalculator > listOfLights = getLights();

        foreach (GameObject thisObject in baseSetGrabberBeforeSensing.grab())
        {
            if (objectDetection(thisObject, listOfLights))
            {
                //Debug.Log("newList.Add(thisObject);");
                newList.Add(thisObject);
            }
        }

        thePerceptions.sensoryInput(newList);
        theBeleifs.sensoryInput(newList);
    }

    private bool objectDetection(GameObject thisObject, List<lightIlluminationCalculator> listOfLights)
    {

        //basic filter [tag, range, FOV]
        //then, different ways to detect, starting with computationally easiest to eliminate?????:
        //[for each stealth armature peice]
        //      line of sight:
        //          not requesting stealth = detected
        //          requesting stealth:                                         [ADD TO ADVANCED LINE OF SIGHT LIST (no don't use list that would result in calculating items on list even if other steps already detected them?) AND PROCESS AFTER SHADOWS, WHICH ARE EASIER I THINK???  AT LEAST FOR SILHOUETTE, MAYBE TINY BIT EASIER THAN ILLUMINATION BECAUSE NO QUANTITY CALCULATION WELL NO I HAVE TO COMPARE DIFFERENCE IN BRIGHTNESS FOR SHADOW SOOO MAYBE DO REGULAR ILLUMINATION BEFORE SHADOWS]
        //              direct illumination = detected
        //              shadow visible = detected
        //              silhouette = detected
        //              CONTINUE LOOP [don't do shadows AGAIN if they requested stealth]
        //      shadow visible = detected


        //Debug.Log("foreach");

        if (basicTagProxAndFOVFilterCriteria.evaluateObject(thisObject) == false) { return false; }

        if (advancedSensingRequestingStealthCriteria.evaluateObject(thisObject) == false)//hmm, doesn't work for armature PART???
        {
            //not requesting stealth = detected
            //Debug.Log("return true, advancedSensingRequestingStealthCriteria = FALSE");
            return true;
        }


        List<GameObject> theStealthArmature = getStealthArmature(thisObject);
        foreach (GameObject thisPart in theStealthArmature)
        {
            bool theBool = evaluateArmaturePart(thisPart, listOfLights);
            //Debug.Log("theBool:  "+ theBool);

            if (theBool == true) {  return true; }
        }
        //Debug.Log("return false;");
        return false;
    }

    private bool evaluateArmaturePart(GameObject thisPart, List<lightIlluminationCalculator> listOfLights)
    {
        //[for each stealth armature peice]
        //      line of sight:
        //          not requesting stealth = detected
        //          requesting stealth:                                         [ADD TO ADVANCED LINE OF SIGHT LIST (no don't use list that would result in calculating items on list even if other steps already detected them?) AND PROCESS AFTER SHADOWS, WHICH ARE EASIER I THINK???  AT LEAST FOR SILHOUETTE, MAYBE TINY BIT EASIER THAN ILLUMINATION BECAUSE NO QUANTITY CALCULATION WELL NO I HAVE TO COMPARE DIFFERENCE IN BRIGHTNESS FOR SHADOW SOOO MAYBE DO REGULAR ILLUMINATION BEFORE SHADOWS]
        //              direct illumination = detected
        //              shadow visible = detected
        //              silhouette = detected
        //              CONTINUE LOOP [don't do shadows AGAIN if they requested stealth]
        //      shadow visible = detected




        if (lineOfSightCriteria.evaluateObject(thisPart) == false)
        {
            //      line of sight:  no
            //      shadow visible = detected
            //Debug.Log("return shadowVisible(thisPart,listOfLights);:  " + shadowVisible(thisPart, listOfLights));
            return shadowVisible(thisPart,listOfLights);//lineOfSightToShadowCriteria.evaluateObject(thisPart);
        }




        //          requesting stealth:                                         [ADD TO ADVANCED LINE OF SIGHT LIST(no don't use list that would result in calculating items on list even if other steps already detected them?) AND PROCESS AFTER SHADOWS, WHICH ARE EASIER I THINK???  AT LEAST FOR SILHOUETTE, MAYBE TINY BIT EASIER THAN ILLUMINATION BECAUSE NO QUANTITY CALCULATION WELL NO I HAVE TO COMPARE DIFFERENCE IN BRIGHTNESS FOR SHADOW SOOO MAYBE DO REGULAR ILLUMINATION BEFORE SHADOWS]
        //              direct illumination = detected
        //              shadow visible = detected
        //              silhouette = detected
        //              CONTINUE LOOP [don't do shadows AGAIN if they requested stealth]

        if (directIllumination(thisPart, listOfLights) == true) { return true; }
        if (shadowVisible(thisPart, listOfLights) == true) { return true; }
        if (silhouetteVisible(thisPart, listOfLights) == true) { return true; }



        //Debug.Log("reached end, return FALSE");
        return false;
    }

    private bool shadowVisible(GameObject thisPart, List<lightIlluminationCalculator> listOfLights)
    {
        //so, if we use this to "detect" objects, they will go into "current perceptions" DESPITE there being no line of sight
        //kinda makes sense.  can obviously happen for SOUND.  but that means need to filter line of sight AGAIN for shooting behavior etc
        //return lineOfSightToShadowCriteria.evaluateObject(thisPart);

        //project a shadow point from armature part
        //look for line of sight to shadow
        //maybe calculate the relative darkness of shadow compared to that point if there WAS no shadow

        bool theBool = (new findRelativeShadowBrightnessPercentage().calculate(theVisualSenseApparatus, umDontIHaveARANGE, thisPart, listOfLights) > 0.02f);//< 0.98f);
        //Debug.Log("return :  " + theBool);

        return theBool;
    }

    private bool silhouetteVisible(GameObject thisPart, List<lightIlluminationCalculator> listOfLights)
    {
        //return visibleSilhouetteCriteria.evaluateObject(thisPart);

        //raycast to the sides in tiny (exponential?) increments
        //[add small vector to armature part position, DON'T use angles]
        //first thing you hit that is NOT the same game object, test illumination THERE
        //[well, not first thing.  FIRST make sure you go back to fine-grained to make SURE you found THE edge]
        //if bright enough, that means silhouette! [assume object is dark]

        bool theBool = (new findSilhouetteBrightness().calculate(theVisualSenseApparatus, thisPart, listOfLights) > 0.2f);
        //Debug.Log("return silhouetteVisible:  " +theBool);

        return theBool;
    }

    private bool directIllumination(GameObject thisPart, List<lightIlluminationCalculator> listOfLights)
    {
        float intensity = 0f;

        //for now, just "detect" if any ONE "body part" reaches detectability threshhold
        intensity = objectIlluminationDetectabilityEvaluator.evaluateObject(thisPart);
        //Debug.Log("intensity:  " + intensity);
        //Debug.DrawLine(theVisualSenseApparatus.position, thisPart.transform.position, new Color(intensity, intensity, intensity), 20f);


        //Debug.Log("return (intensity > illuminationIntensityThresholdForDetection)" + (intensity > illuminationIntensityThresholdForDetection));

        return (intensity > illuminationIntensityThresholdForDetection);
    }


    private List<lightIlluminationCalculator> getLights()
    {
        //for now:
        List < GameObject > lightObjects = new setOfAllObjectsWithTag(tag2.lightSource).grab();

        List<lightIlluminationCalculator> newList = new List<lightIlluminationCalculator>();

        foreach(GameObject thisObject in lightObjects)
        {
            newList.Add(thisObject.GetComponent<lightIlluminationCalculator>());
        }

        return newList;
    }

    private List<GameObject> getStealthArmature(GameObject thisObject)
    {
        List<GameObject> newList = new List<GameObject>();

        stealthArmature theStealthArmature = thisObject.GetComponent<stealthArmature>();
        if(theStealthArmature == null)
        {
            newList.Add(thisObject);
            return newList;
        }

        newList = theStealthArmature.theListOfParts;

        return newList;
    }
}





public class perceptions : MonoBehaviour
{
    //public updateableSetGrabber theSet;
    //public updateableSetGrabber threatPerceptionSet;
    public updateableSetGrabber setOfAllCurrentlySensedObjects;



    internal static perceptions addThisComponent(GameObject theObject)
    {
        perceptions theComponent = theObject.AddComponent<perceptions>();
        //theComponent.theSet = new threatBeleifSet1(theObject);
        //theComponent.threatPerceptionSet = new threatPerceptionSet1(theObject);
        theComponent.setOfAllCurrentlySensedObjects = new perceptionSet1();


        return theComponent;
    }

    internal static perceptions ensureComponent(GameObject theObject)
    {
        perceptions theComponent = theObject.GetComponent<perceptions>();
        if (theComponent == null)
        {
            theComponent = perceptions.addThisComponent(theObject);
        }

        return theComponent;
    }

    internal void sensoryInput(List<GameObject> inputList)
    {
        //threatPerceptionSet.updateSet(inputList);
        setOfAllCurrentlySensedObjects.updateSet(inputList);
    }

    internal void weDetectedThisObject(GameObject thisObject)//or do we want to have a class/interface called "beleif", and we just add or update a "beleif" to a list or something?????
    {
        //threatPerceptionSet.updateSetWithOneObject(thisObject);
        //hmm how to reset the list each frame......... setOfAllCurrentlySensedObjects.updateSetWithOneObject(thisObject);
    }
}



public class perceptionSet1 : updateableSetGrabber
{
    public override List<GameObject> grab()
    {
        return convertToObjects(theStoredSet);

    }
    public override void updateSet(List<GameObject> inputList)
    {
        //and RESET
        theStoredSet = convertToIds(inputList);
    }

    public override void updateSet(List<objectIdPair> inputList)
    {
        theStoredSet = inputList;
    }

    internal override void updateSetWithOneObject(GameObject thisObject)
    {
        theStoredSet.Add(tagging2.singleton.idPairGrabify(thisObject));
    }
}















public class threatPerceptionSet1 : updateableSetGrabber
{
    objectCriteria theCriteria;  //criteria for inclusion in this set

    public threatPerceptionSet1(tag2 teamIn)
    {

        objectCriteria theThreatObjectCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(teamIn))
            );

        theCriteria = theThreatObjectCriteria;
    }
    public threatPerceptionSet1(GameObject theObject)
    {

        tag2 team = tagging2.singleton.teamOfObject(theObject);

        objectCriteria theThreatObjectCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team))
            );

        theCriteria = theThreatObjectCriteria;
    }

    public override List<GameObject> grab()
    {
        return convertToObjects(theStoredSet);

    }
    public override void updateSet(List<GameObject> inputList)
    {
        foreach (objectIdPair thisID in convertToIds(inputList))
        {
            updateSetWithOneID(thisID, theCriteria);
        }
    }

    private void updateSetWithOneID(objectIdPair thisID, objectCriteria theCriteria)
    {
        //if (theStoredSet.Contains(thisID)){ continue; }

        //theStoredSet.Add(thisID);

        //Debug.Log("updateSetWithOneID:  " + thisID.theObject);
        if (theCriteria.evaluateObject(thisID.theObject) == false)
        {
            //Debug.Log("criteria NOT met"); 
            return;
        }

        //Debug.Log("criteria met:  " + thisID.theObject);
        theStoredSet.Add(thisID);
    }

    internal override void updateSetWithOneObject(GameObject thisObject)
    {
        updateSetWithOneID(tagging2.singleton.idPairGrabify(thisObject), theCriteria);
    }

    public override void updateSet(List<objectIdPair> inputList)
    {
        foreach (objectIdPair thisID in inputList)
        {
            updateSetWithOneID(thisID, theCriteria);
        }
    }
}








public class stealthArmature : MonoBehaviour
{
    public List<GameObject> theListOfParts = new List<GameObject>();
    public float simpleSizeOfObject = 0;


    public static stealthArmature addThisComponent(GameObject theObject, GameObject part1)
    {
        stealthArmature theComponent = theObject.AddComponent<stealthArmature>();
        theComponent.theListOfParts.Add(part1);


        return theComponent;
    }
    public static stealthArmature addThisComponent(GameObject theObject, GameObject part1, GameObject part2)
    {
        stealthArmature theComponent = theObject.AddComponent<stealthArmature>();
        theComponent.theListOfParts.Add(part1);
        theComponent.theListOfParts.Add(part2);


        return theComponent;
    }

    public static stealthArmature addThisComponent(GameObject theObject, GameObject part1, GameObject part2, GameObject part3)
    {
        stealthArmature theComponent = theObject.AddComponent<stealthArmature>();
        theComponent.theListOfParts.Add(part1);
        theComponent.theListOfParts.Add(part2);
        theComponent.theListOfParts.Add(part3);


        return theComponent;
    }

    public static stealthArmature addThisComponent(GameObject theObject, GameObject part1, GameObject part2, GameObject part3, GameObject part4)
    {
        stealthArmature theComponent = theObject.AddComponent<stealthArmature>();
        theComponent.theListOfParts.Add(part1);
        theComponent.theListOfParts.Add(part2);
        theComponent.theListOfParts.Add(part3);
        theComponent.theListOfParts.Add(part4);


        return theComponent;
    }

    public static stealthArmature addThisComponent(GameObject theObject, GameObject part1, GameObject part2, GameObject part3, GameObject part4, GameObject part5)
    {
        stealthArmature theComponent = theObject.AddComponent<stealthArmature>();
        theComponent.theListOfParts.Add(part1);
        theComponent.theListOfParts.Add(part2);
        theComponent.theListOfParts.Add(part3);
        theComponent.theListOfParts.Add(part4);
        theComponent.theListOfParts.Add(part5);


        return theComponent;
    }


    internal float calculateSizeOfObject()
    {
        //just take average of distance between armature bits!
        //can use this to make larger objects more likely to be noticed!
        //although that fails if only a tiny part is illuminated
        //so i'll have to "average" the illumination.  and i can use THIS SIZE to help that calculation!
        //or something similar to this.

        int numberOfComparisons = 0;
        float totalDistance = 0f;
        foreach (GameObject thisObject1 in theListOfParts)
        {
            foreach (GameObject thisObject2 in theListOfParts)
            {
                if(thisObject1 == thisObject2) { continue; }
                totalDistance += (thisObject1.transform.position - thisObject2.transform.position).magnitude;
                numberOfComparisons++;
            }
        }

        return totalDistance/numberOfComparisons;
    }
}



public class detectabilityIlluminationEvaluator1 : objectEvaluator, positionEvaluation
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


        //Debug.Log("^^^^^^^^^^^totalIllumination:  " + totalIllumination);
        return totalIllumination;
    }

    public float evaluatePosition(Vector3 thePosition)
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
            float intensityToAdd = thisLight.GetComponent<lightIlluminationCalculator>().evaluate(thePosition);
            //Debug.Log("intensityToAdd:  " + intensityToAdd);
            totalIllumination += intensityToAdd;
        }


        //Debug.Log("^^^^^^^^^^^totalIllumination:  " + totalIllumination);
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





public class OldSpatialDataPointFragment
{
    //basically, i hate having anything significant nested inside of my "foreach" loops.
    //and if there IS much in there, it should be easy to "find" when i'm debugging.  not digging through layers of functions.
    //so put stuff HERE, in a different class object.  a base level, where i will know to look if that's what the error seems to be
    //[same for adding or modifying stuff, not just debugging]

    //so, right now "OldSpatialDataPoint" has multiple "threat" gameobjects.
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

    public OldSpatialDataPointFragment(GameObject target, Vector3 location)
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

public class OldSpatialDataPoint
{
    //this code should only ever be about one single point
    //anything plural [like "distances"] should still be about one sample point, and how far away other things [like threats] are from that one point

    public List<OldSpatialDataPointFragment> fragmentList = new List<OldSpatialDataPointFragment>();

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

        foreach (OldSpatialDataPointFragment thisFragment in fragmentList)
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

        foreach (OldSpatialDataPointFragment thisFragment in fragmentList)
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

    public OldSpatialDataPoint(List<GameObject> targetList, Vector3 inputPoint)
    {
        //should just be done in constructor?
        thisPoint = inputPoint;

        //printAllIdNumbers(targetList);

        foreach (GameObject thisTarget in targetList)
        {
            OldSpatialDataPointFragment newFragment = new OldSpatialDataPointFragment(thisTarget, thisPoint);
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
        foreach (OldSpatialDataPointFragment thisFragment in fragmentList)
        {
            if(thisFragment.lineOfSightBool == true) {return true;}
        }
        return false;
    }
}

public class OldSpatialDataSet
{
    //this code should be about using more than one "OldSpatialDataPoint" class object

    public List<OldSpatialDataPoint> theDataSet = null;// new List<OldSpatialDataPoint>();

    public List<Vector3> thePoints = new List<Vector3>();
    public List<GameObject> threatList = new List<GameObject>();
    public List<float> combinedData;
    public Vector3 middlePoint;




    //      graphing

    public void appleGraph()
    {
        appleField();

        foreach (OldSpatialDataPoint thisDataPoint in theDataSet)
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
        theDataSet = new List<OldSpatialDataPoint>();

        foreach (Vector3 thisPoint in thePoints)
        {
            OldSpatialDataPoint aDataPoint = new OldSpatialDataPoint(threatList, thisPoint);
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
        OldSpatialDataPoint middleDataPoint = new OldSpatialDataPoint(threatList, middlePoint);

        return middleDataPoint.applePattern();
    }
    public void appleField()
    {
        int currentIndex = 0;

        foreach (OldSpatialDataPoint thisDataPoint in theDataSet)
        {
            //endPointsToGraph.Add(thisDataPoint.pattern1ForFightingArmedThreat());
            thisDataPoint.dontIneedACOMBINEDEndpoint = thisDataPoint.applePattern();
            currentIndex++;
        }
    }


    public void graphFieldAdHoc()
    {
        //just do regular blue vector field of the "end points" for now.

        foreach (OldSpatialDataPoint thisDataPoint in theDataSet)
        {
            thisDataPoint.graphBetweenThisPointAndCOMBINEDpoint();
        }
    }



}

