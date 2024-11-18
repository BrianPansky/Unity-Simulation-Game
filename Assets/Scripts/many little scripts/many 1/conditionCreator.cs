using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static enactionCreator;
using static interactionCreator;
using static UnityEngine.GraphicsBuffer;

public class conditionCreator : MonoBehaviour
{
    public static conditionCreator singleton;

    void Awake()
    {
        singletonify();
    }

    void singletonify()
    {
        if (singleton != null && singleton != this)
        {
            Debug.Log("this class is supposed to be a singleton, you should not be making another instance, destroying the new one");
            Destroy(this);
            return;
        }
        singleton = this;
    }



    ////////////////////    CONDITIONS  ///////////////////

    //proximity, time....what else.....various states?

    public bool isXCloserThanYToZ(GameObject objectX, GameObject objectY, GameObject objectZ)
    {
        float distanceToX = distanceBetween(objectX, objectZ);

        float distanceToY = distanceBetween(objectY, objectZ);


        if (distanceToX > distanceToY)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public float distanceBetween(GameObject object1, GameObject object2)
    {
        Vector3 theVectorBetweenXandZ = object1.transform.position - object2.transform.position;
        return theVectorBetweenXandZ.sqrMagnitude;
    }

    public float horizontalDistanceBetween(GameObject object1, GameObject object2)
    {
        Vector3 v1 = object1.transform.position;
        Vector3 v2 = object2.transform.position;
        //Vector3 theVectorBetweenXandZ = object1.transform.position - object2.transform.position;
        Vector3 theHorizontalVectorBetweenXandZ = new Vector3(v1.x - v2.x, 0, v1.z - v2.z);
        return theHorizontalVectorBetweenXandZ.sqrMagnitude;
    }


    public Quaternion rotationFromLookingVector()
    {

        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        return Quaternion.identity;
    }





















    //      NON-BOOLEAN FUNCTIONS



    public GameObject whichObjectOnListIsNearest(GameObject objectWeWantItClosestTo, List<GameObject> listOfObjects)
    {
        GameObject theClosestSoFar = null;

        foreach (GameObject thisObject in listOfObjects)
        {
            if (theClosestSoFar != null)
            {
                float distanceToThisObject = Vector3.Distance(thisObject.transform.position, objectWeWantItClosestTo.transform.position);
                float distanceToTheClosestSoFar = Vector3.Distance(theClosestSoFar.transform.position, objectWeWantItClosestTo.transform.position);

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

    public GameObject whichObjOnIDPAIRListIsNearest(GameObject objectWeWantItClosestTo, List<objectIdPair> listOfObjects)
    {
        return whichObjectOnListIsNearest(objectWeWantItClosestTo, tagging2.singleton.listInObjectFormat(listOfObjects));
    }

    internal static bool objectHasInteraction(GameObject thisObject, interactionCreator.numericalVariable variableThatEquippableInteractsWith, bool shouldItBeAddition)
    {
        throw new NotImplementedException();

        var enactions = thisObject.GetComponents<collisionEnaction>();
        if (enactions.Count() < 1) { return false; }

        foreach (collisionEnaction enaction in enactions)
        {
            //numericalEffect
            //numericalVariable
        }


        return true;
    }

    internal static bool objectHasInteractionType(GameObject thisObject, enactionCreator.interType theInterType)
    {
        throw new NotImplementedException();
    }
}



public abstract class condition
{

    //string failureReport = "";

    public abstract bool met();


    public abstract string asText();
    public abstract string asTextSHORT();
    //string whyDidItFail();



    public condition returnInverseBool()
    {
        return new reverseCondition(this);
    }

    public string standardAsText()
    {
        string stringToReturn = "";

        stringToReturn += "met?  " + met() + ", ";

        stringToReturn += this.ToString();

        return stringToReturn;
    }

    public string standardAsTextSHORT()
    {

        string stringToReturn = "[condition:  ";

        stringToReturn += this.ToString();
        stringToReturn += "]";

        return stringToReturn;
    }
}


public class reverseCondition : condition
{
    condition theConditionToReverse;

    public reverseCondition(condition theConditionToReverseIn)
    {
        this.theConditionToReverse = theConditionToReverseIn;
    }

    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }

    public override bool met()
    {
        bool originalBool = theConditionToReverse.met();

        if(originalBool==true) {return false;}
        return true;
    }
}

public class multicondition : condition
{
    List<condition> conditionList;


    public multicondition(condition c1)
    {
        conditionList = new List<condition>();
        conditionList.Add(c1);
    }
    public multicondition(condition c1, condition c2)
    {
        conditionList = new List<condition>();
        conditionList.Add(c1);
        conditionList.Add(c2);
    }
    public multicondition(condition c1, condition c2, condition c3)
    {
        conditionList = new List<condition>();
        conditionList.Add(c1);
        conditionList.Add(c2);
        conditionList.Add(c3);
    }





    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }

    public override bool met()
    {
        foreach (condition condition in conditionList)
        {
            if (condition.met() == false) { return false; }
        }

        return true;
    }
}


//public class useDynamicInputConditionAsStaticCondition



//static/preset variables?????????
public class autoCondition : condition
{

    public override bool met()
    {
        return true;
    }
    
    public override string asText()
    {
        return "this is ''autoCondition'' (it is always true/met)";
    }

    public override string asTextSHORT()
    {
        return "this is ''autoCondition'' (it is always true/met)";
    }
}

public class enacted : condition
{
    int howManyTimesToEnact = 1;
    planEXE2 theEnactionEXE;

    public enacted(planEXE2 theEnactionEXE, int inputHowManyTimesToEnact = 1)
    {
        this.howManyTimesToEnact = inputHowManyTimesToEnact;
        this.theEnactionEXE = theEnactionEXE;
    }

    public override bool met()
    {
        if (theEnactionEXE.numberOfTimesExecuted >= howManyTimesToEnact) { return true; }

        return false;
    }

    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }
}

public class cooldown : condition
{
    public int cooldownMax = 130;
    public int cooldownTimer = 0;

    public cooldown(int cooldownMax = 130)
    {
        this.cooldownMax = cooldownMax;
    }

    public override bool met()
    {
        if (cooldownTimer < 1) { return true; }

        return false;
    }

    public void cooling()
    {
        cooldownTimer--;
    }

    public void fire()
    {
        cooldownTimer = cooldownMax;
    }


    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }
}

public class planListComplete : condition
{
    List<planEXE2> planList;

    public planListComplete(List<planEXE2> planList)
    {
        this.planList = planList;
    }

    public override bool met()
    {
        //Debug.Log("planList.Count:  " + planList.Count);
        foreach (planEXE2 planEXE in planList)
        {
            if (planEXE == null) { continue; } //messy annoying for now
            if (planEXE.endConditionsMet() == false) { return false; }
        }
        return true;
    }


    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }
}

public class adHocHasNoGunCondition : condition
{

    GameObject theObject;
    bool returnTrueIfThereIsNoGun = true;


    public adHocHasNoGunCondition(GameObject theObjectIn, bool returnTrueIfThereIsNoGunIn = true)
    {
        theObject = theObjectIn;
        returnTrueIfThereIsNoGun = returnTrueIfThereIsNoGunIn;
    }

    public override bool met()
    {
        AIHub3 theHub = theObject.GetComponent<AIHub3>();

        bool hasNoGun = theHub.hasNoGun();
        if (hasNoGun == returnTrueIfThereIsNoGun) { return true; }
        return false;
    }


    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        string stringToReturn = "";
        if (returnTrueIfThereIsNoGun)
        {
            stringToReturn += "do IF this object has NO gun";
        }
        else
        {
            stringToReturn += "do if this object HAS a gun";
        }

        return stringToReturn;
    }
}

public class stickyCondition : condition
{
    condition nestedCondition;

    int countdown = 0;
    int maxTimer = 2;


    public stickyCondition(condition nestedConditionIn, int maxTimerIn =2)
    {
        nestedCondition = nestedConditionIn;
        maxTimer = maxTimerIn;
    }


    public override bool met()
    {
        if (nestedCondition.met())
        {
            countdown = maxTimer;
            return true;
        }

        countdown--;
        
        
        if (countdown < 1)
        {
            return false;
        }

        return true;
    }





    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }

}





//dynamic/input variables??????????
public class proximity : condition
{
    //for when we want the objects to be CLOSER than the desired distance

    GameObject object1;
    //GameObject object2;
    //Vector3 targetLocation2;
    targetCalculator targetCalc;
    float desiredDistance = 4f;
    float allowedMargin = 2f;
    public bool debugPrint = false;

    conditionalEffects2 adHocConditionalEffects;

    public proximity(GameObject object1, GameObject object2, float desiredDistance = 4f, float allowedMargin = 2f)
    {
        this.object1 = object1;
        //this.object2 = object2;
        targetCalc = new movableObjectTargetCalculator(object1, object2, desiredDistance);
        this.desiredDistance = desiredDistance;
        this.allowedMargin = allowedMargin;
    }

    public proximity(GameObject object1, Vector3 targetLocation2In, float desiredDistance = 4f)
    {
        this.object1 = object1;
        //this.targetLocation2 = targetLocation2In;

        targetCalc = new staticVectorTargetCalculator(object1, targetLocation2In, desiredDistance);
        this.desiredDistance = desiredDistance;
    }

    public override bool met()
    {
        //return false;
        Vector3 position1 = object1.transform.position;
        Vector3 position2 = targetCalc.targetPosition();// object2.transform.position;
        Vector3 vectorBetween = position1 - position2;
        float distance = vectorBetween.magnitude;

        //Debug.Log("condition:  " + this);
        //Debug.Log("distance:  " + distance);
        //Debug.Log("desiredDistance:  " + desiredDistance);
        //Debug.Log("(desiredDistance + allowedMargin):  " + (desiredDistance + allowedMargin));
        //Debug.DrawLine(position1, position2, Color.blue, 0.1f);


        if (debugPrint)
        {
            Debug.Log("distance:  " + distance);
            Debug.Log("desiredDistance:  " + desiredDistance);
            Debug.Log("(desiredDistance + allowedMargin):  " + (desiredDistance + allowedMargin));
            Debug.DrawLine((position1 + Vector3.up), (position2 + Vector3.up), Color.blue, 7f);

            Debug.DrawLine(position1, position1 + (Vector3.up*105), Color.white, 7f);

            Debug.DrawLine(position2, position2 + (Vector3.up * 105), Color.black, 7f);
        }


        if (distance > (desiredDistance + allowedMargin)) { return false; }



        //Debug.Log("YESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
        return true;
    }
    public string metAsText()
    {
        string stringToReturn = "";
        bool theBool = false;
        Vector3 position1 = object1.transform.position;
        Vector3 position2 = targetCalc.targetPosition();// object2.transform.position;
        Vector3 vectorBetween = position1 - position2;
        float distance = vectorBetween.magnitude;

        //Debug.Log("condition:  " + this);
        //Debug.Log("distance:  " + distance);
        //Debug.Log("desiredDistance:  " + desiredDistance);
        //Debug.DrawLine(position1, position2, Color.blue, 0.1f);


        if (debugPrint)
        {
            Debug.Log("distance:  " + distance);
            Debug.Log("desiredDistance:  " + desiredDistance);
            Debug.DrawLine((position1 + Vector3.up), (position2 + Vector3.up), Color.blue, 7f);

            Debug.DrawLine(position1, position1 + (Vector3.up * 105), Color.white, 7f);

            Debug.DrawLine(position2, position2 + (Vector3.up * 105), Color.black, 7f);
        }


        if (distance > desiredDistance) 
        { theBool= false; }
        else
        {
            theBool = true;
        }










        stringToReturn += "met?  " + theBool;
        stringToReturn += ", distance:  " + distance;
        stringToReturn += ", desiredDistance:  " + desiredDistance;

        return stringToReturn;
    }

    public override string asText()
    {
        string stringToReturn = "";
        stringToReturn += "proximity between 1)  " + object1 + ", and 2)  " + targetCalc.asText();// object2;
        stringToReturn += ", [desiredDistance = " + desiredDistance + "]";
        stringToReturn += ", [metAsText() = " + metAsText() + "]";


        return stringToReturn;

    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }
}


public class proximityRef : condition
{
    //for when we want the objects to be CLOSER than the desired distance

    GameObject object1;
    //GameObject object2;
    //Vector3 targetLocation2;
    //targetCalculator targetCalc;
    float desiredDistance = 4f;
    float allowedMargin = 2f;
    vect3EXE2 theTargetHolder;
    public bool debugPrint = false;

    conditionalEffects2 adHocConditionalEffects;

    public proximityRef(GameObject object1, vect3EXE2 theTargetHolderIn, float desiredDistance = 4f, float allowedMarginIn = 2f)
    {
        this.object1 = object1;
        theTargetHolder = theTargetHolderIn;
        //this.object2 = object2;
        //targetCalc = new movableObjectTargetCalculator(object1, object2, desiredDistance);
        this.desiredDistance = desiredDistance;
        this.allowedMargin = allowedMarginIn;
    }


    public override bool met()
    {
        //return false;
        Vector3 position1 = object1.transform.position;
        Vector3 position2 = theTargetHolder.theTargetCalculator.targetPosition();// object2.transform.position;
        Vector3 vectorBetween = position1 - position2;
        float distance = vectorBetween.magnitude;

        //Debug.Log("condition:  " + this);
        //Debug.Log("distance:  " + distance);
        //Debug.Log("desiredDistance:  " + desiredDistance);
        //Debug.Log("theTargetHolder.theTargetCalculator.GetHashCode():  " + theTargetHolder.theTargetCalculator.GetHashCode());
        //Debug.Log("theTargetHolder.theTargetCalculator.targetPosition():  " + theTargetHolder.theTargetCalculator.targetPosition());
        //Debug.Log("theTargetHolder.theTargetCalculator.targetPosition():  " + theTargetHolder.theTargetCalculator.tar);
        Debug.DrawLine(position1, position2, Color.magenta, 0.1f);


        if (debugPrint)
        {
            Debug.Log("distance:  " + distance);
            Debug.Log("desiredDistance:  " + desiredDistance);
            Debug.DrawLine((position1 + Vector3.up), (position2 + Vector3.up), Color.blue, 7f);

            Debug.DrawLine(position1, position1 + (Vector3.up * 105), Color.white, 7f);

            Debug.DrawLine(position2, position2 + (Vector3.up * 105), Color.black, 7f);
        }


        if (distance > (desiredDistance+ allowedMargin)) { return false; }

        return true;
    }
    public string metAsText()
    {
        string stringToReturn = "";
        bool theBool = false;
        Vector3 position1 = object1.transform.position;
        Vector3 position2 = theTargetHolder.theTargetCalculator.targetPosition();// object2.transform.position;
        Vector3 vectorBetween = position1 - position2;
        float distance = vectorBetween.magnitude;

        //Debug.Log("condition:  " + this);
        //Debug.Log("distance:  " + distance);
        //Debug.Log("desiredDistance:  " + desiredDistance);
        //Debug.DrawLine(position1, position2, Color.blue, 0.1f);


        if (debugPrint)
        {
            Debug.Log("distance:  " + distance);
            Debug.Log("desiredDistance:  " + desiredDistance);
            Debug.DrawLine((position1 + Vector3.up), (position2 + Vector3.up), Color.blue, 7f);

            Debug.DrawLine(position1, position1 + (Vector3.up * 105), Color.white, 7f);

            Debug.DrawLine(position2, position2 + (Vector3.up * 105), Color.black, 7f);
        }


        if (distance > desiredDistance)
        { theBool = false; }
        else
        {
            theBool = true;
        }










        stringToReturn += "met?  " + theBool;
        stringToReturn += ", distance:  " + distance;
        stringToReturn += ", desiredDistance:  " + desiredDistance;

        return stringToReturn;
    }

    public override string asText()
    {
        string stringToReturn = "";
        stringToReturn += "proximity between 1)  " + object1 + ", and 2)  " + theTargetHolder.theTargetCalculator.asText();// object2;
        stringToReturn += ", [desiredDistance = " + desiredDistance + "]";
        stringToReturn += ", [metAsText() = " + metAsText() + "]";


        return stringToReturn;

    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }
}


public class adocThreatLineOfSightCondition : condition
{

    GameObject theObject;
    bool returnTrueIfAThreatCanSeeThisObject = true;

    public adocThreatLineOfSightCondition(GameObject theObjectIn, bool returnTrueIfAThreatCanSeeThisObjectIn = true)
    {
        theObject = theObjectIn;
        returnTrueIfAThreatCanSeeThisObject = returnTrueIfAThreatCanSeeThisObjectIn;
    }

    public override bool met()
    {
        AIHub3 theHub = theObject.GetComponent<AIHub3>();

        bool threatLineOfSightBool = theHub.threatLineOfSight();
        //Debug.Log("threatLineOfSightBool:  " + threatLineOfSightBool);
        //Debug.Log("returnTrueIfAThreatCanSeeThisObject:  " + returnTrueIfAThreatCanSeeThisObject);
        if (threatLineOfSightBool == returnTrueIfAThreatCanSeeThisObject)
        {
            //Debug.Log("(threatLineOfSightBool == returnTrueIfAThreatCanSeeThisObject), so return true"); 
            return true;
        }

        //Debug.Log("(threatLineOfSightBool == returnTrueIfAThreatCanSeeThisObject) is FALSE, so return false");
        return false;
    }

    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        string stringToReturn = "";
        if (returnTrueIfAThreatCanSeeThisObject)
        {
            stringToReturn += "do IF this object can be seen";
        }
        else
        {
            stringToReturn += "do if this object CANNOT be seen";
        }

        return stringToReturn;
    }
}

public class targetMatchesHitscanOutput : condition
{
    GameObject intendedTarget;
    //GameObject whatRaycastHit = null;
    targetCalculator theTargetCalculator;
    public hitscanEnactor theHitScanner;
    //targetCalculator whatRaycastHit;
    //bool firingIsDone = false;
    public adHocBooleanDeliveryClass firingIsDone;

    public targetMatchesHitscanOutput(targetCalculator theTargetCalculatorIn)
    {
        //intendedTarget = intendedTargetIn;
        //whatRaycastHit = null;
        theTargetCalculator = theTargetCalculatorIn;
        //whatRaycastHit = theHitCalculatorIn;
    }


    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }

    public override bool met()
    {
        //Debug.Log("]]]]]]]]]]]]]]]]]]]]]]]]]]   firingIsDone.theBoolSignal:  "+ firingIsDone.theBoolSignal);
        if (firingIsDone.theBoolSignal == false) { return false; }

        //Debug.Log("theTargetCalculator:  " + theTargetCalculator);
        //Debug.Log("whatRaycastHit:  " + theHitScanner.theHitCalculatorOut);
        if (didRaycastHitCorrectTarget(theTargetCalculator.targetPosition(), theHitScanner.theHitCalculatorOut.targetPosition()) == true)
        {

            //Debug.Log("]]]]]]]]]]]]]]]]]]]]]]]]]    failed because (didRaycastHitCorrectTarget(theTargetCalculator.targetPosition(), whatRaycastHit.targetPosition()) == true)"); 
            return false;
        }

        //Debug.Log("]]]]]]]]]]]]]]]]]]]]]]]]]    condition met, should do debug report!!!!!!!!!!!!!!!!!!!");
        return true;

        /*


        if (firingIsDone.theBoolSignal == false) { return true; }  //yaaaa, bit of a duct tape here.  need non-binary logic?
        
        return didRaycastHitCorrectTarget(theTargetCalculator.targetPosition(), whatRaycastHit.targetPosition());

        */
    }






    public bool didRaycastHitCorrectTarget(GameObject intendedTarget, GameObject whatRaycastHit)
    {
        //if(firingIsDone == false) { return; }
        //if(whatRaycastHit == null) { return; }  //no, wait, if raycast has indeed fired but didn't hit anything, might return null.  can't use null to detect whether it has been fired or not.....
        if (firingIsDone.theBoolSignal == true && whatRaycastHit != intendedTarget) { return false; }

        return true;  //maybe i shouldn't have binary logic?  before firing, the condition is NEITHER met NOR unmet....met is neither true nor false, exactly.  not in the same way.  it's unmet and met is false, but i want it to count as "true" here...
    }

    public void didRaycastHitCorrectTarget(GameObject intendedTarget, Vector3 whatRaycastHit, float marginOfError = 3f)
    {

    }

    public bool didRaycastHitCorrectTarget(Vector3 intendedTargetVector, Vector3 whatRaycastHitVector, float marginOfError = 3f)
    {
        float distance = distanceBetweenPositions(intendedTargetVector, whatRaycastHitVector);

        if (distance < marginOfError) { return true; }

        return false;

    }


    public float distanceBetweenPositions(Vector3 position1, Vector3 position2)
    {
        Vector3 theVectorBetweenXandZ = position1 - position2;
        return theVectorBetweenXandZ.sqrMagnitude;
    }



}




//either?  unsure?
public class numericalCondition : condition
{

    numericalVariable theVariableType;
    float conditionValue = 0f;
    //float variableReference;
    Dictionary<interactionCreator.numericalVariable, float> dictOfIvariables;

    public numericalCondition(numericalVariable theVariableTypeIn, Dictionary<interactionCreator.numericalVariable, float> dictOfIvariablesIn, float conditionValueIn = 0f)
    {
        this.theVariableType = theVariableTypeIn;
        this.dictOfIvariables = dictOfIvariablesIn;
        this.conditionValue = conditionValueIn;
    }


    public override bool met()
    {
        //Debug.Log("theVariableType:  " + theVariableType);
        //Debug.Log("conditionValue:  " + conditionValue);
        //Debug.Log("dictOfIvariables[theVariableType]:  " + dictOfIvariables[theVariableType]);
        //Debug.Log("(dictOfIvariables[theVariableType] < conditionValue):  " + (dictOfIvariables[theVariableType] < conditionValue));
        if (dictOfIvariables[theVariableType] > conditionValue) { return false; }

        return true;
    }


    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }
}




