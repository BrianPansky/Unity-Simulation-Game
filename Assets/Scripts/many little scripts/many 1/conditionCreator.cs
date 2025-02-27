using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Xml.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.XR;
using static enactionCreator;
using static interactionCreator;
using static tagging2;
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
    public GameObject whichObjectOnListIsNearestExceptSELF(GameObject objectWeWantItClosestTo, List<GameObject> listOfObjects)
    {
        GameObject theClosestSoFar = null;

        //Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx  objectWeWantItClosestTo:  " + objectWeWantItClosestTo);
        foreach (GameObject thisObject in listOfObjects)
        {
            //Debug.Log("thisObject:  " + thisObject);
            if (thisObject == objectWeWantItClosestTo)
            {
                continue;
            }
            else if (theClosestSoFar != null)
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


    public GameObject randomTargetPickerObjectFromListEXCEPT(List<GameObject> theList, GameObject notTHISObject)
    {
        if (theList.Count == 0)
        {
            Debug.Log("there are zero objects on the list of objects entered into ''randomTargetPickerObjectFromListEXCEPT''");
            return null;
        }


        int numberOfTries = 10; //easy ad hoc way to terminate a potentially infinate loop for now lol
        GameObject thisObject;
        thisObject = null;


        while (numberOfTries > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, theList.Count);
            thisObject = theList[randomIndex];

            if (thisObject != notTHISObject)
            {
                return thisObject;
            }

            numberOfTries--;
        }




        return thisObject;

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
    public abstract string asTextAllTheWayDown();
    public abstract string asTextBaseOnly();
    //string whyDidItFail();



    public condition returnInverseBool()
    {
        return new reverseCondition(this);
    }

    public string standardAsText()
    {
        string stringToReturn = "";

        stringToReturn += "met?  " + met() + ", ";  //doesn't seem to work??????????  doesn't call the override "met()"?

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


public abstract class baseCondition : condition
{

    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }
    public override string asTextAllTheWayDown()
    {
        return asText();
    }

    public override string asTextBaseOnly()
    {
        Debug.Log("11111111111????????????????");
        return asText();
    }

}

public abstract class nesterCondition : condition
{

    public condition theNestedCondition;

    public override string asText()
    {
        return theNestedCondition.asText();
    }

    public override string asTextSHORT()
    {
        return theNestedCondition.asTextSHORT();
    }

    public override string asTextAllTheWayDown()
    {
        string newString = this.ToString() + "|>"+ theNestedCondition.asTextAllTheWayDown();
        return newString;
    }

    public override string asTextBaseOnly()
    {
        //Debug.Log("2222222222222????????????????");
        //Debug.Log("this:  " + this);
        //Debug.Log("asText():  " + asText());
        string newString = theNestedCondition.asTextBaseOnly();
        return newString;
    }
}

public class reverseCondition : nesterCondition
{
    //condition theConditionToReverse;

    public reverseCondition(condition theConditionToReverseIn)
    {
        //this.theConditionToReverse = theConditionToReverseIn;
        this.theNestedCondition = theConditionToReverseIn;
    }

    public override bool met()
    {
        //Debug.Log("reverseCondition met ????????????????  "+ theNestedCondition.ToString());
        bool originalBool = theNestedCondition.met();

        if(originalBool==true) {return false;}
        return true;
    }
}

public class multicondition : nesterCondition
{
    List<condition> conditionList;


    
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





    public override bool met()
    {
        foreach (condition condition in conditionList)
        {
            //Debug.Log(".................condition:  " + condition + ", hash code:  " + condition.GetHashCode());
            //Debug.Log("condition.asTextSHORT():  " + condition.asTextSHORT());
            //Debug.Log("condition.asText():  " + condition.asText());
            bool metttt = condition.met();
            //Debug.Log("metttt:  " + metttt + condition.GetHashCode());
            if (metttt == false)
            {
                //Debug.Log("condition is false/NOT met!!");
                return false; 
            }
        }

        //Debug.Log("condition is true/met!!");
        return true;
    }


    public override string asText()
    {
        string theText = "[";
        foreach (condition thisCondition in conditionList)
        {
            theText += thisCondition.asText();
            theText += ", ";
        }

        theText += "]";

        return theText;
    }

    public override string asTextSHORT()
    {
        string theText = "[";
        foreach (condition thisCondition in conditionList)
        {
            theText += thisCondition.asTextSHORT();
            theText += ", ";
        }

        theText += "]";

        return theText;
    }

    public override string asTextAllTheWayDown()
    {
        string newString = this + ":  ";
        newString += asText();
        return newString;
    }

    public override string asTextBaseOnly()
    {
        string newString = this + ":  ";
        newString+= asText();
        return newString;
    }
}


//public class useDynamicInputConditionAsStaticCondition



//static/preset variables?????????
public class autoCondition : baseCondition
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

public class enacted : baseCondition
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

}

public class cooldown : baseCondition
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

}

public class planListComplete : baseCondition
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


}

public class adHocHasNoGunCondition : baseCondition
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

public class stickyCondition : nesterCondition
{
    //ondition nestedCondition;

    //int countdown = 0;
    int maxTimer = 2;

    float startTime = 0;


    public stickyCondition(condition nestedConditionIn, int maxTimerInSeconds =2)
    {
        if(maxTimerInSeconds > 11)
        {
            Debug.Log("(maxTimerInSeconds > 11), are you sure you want this to possibly delay effects by " + maxTimerInSeconds + " seconds?");
        }
        theNestedCondition = nestedConditionIn;
        maxTimer = maxTimerInSeconds;
    }


    public override bool met()
    {
        //Debug.Log("Time.fixedTime:  " + Time.fixedTime);
        //Debug.Log("startTime:  " + startTime);
        //Debug.Log("Time.fixedTime - startTime:  " + (Time.fixedTime - startTime));
        //Debug.Log("maxTimer:  " + maxTimer);
        if (Time.fixedTime - startTime < maxTimer)
        {
            //countdown--;
            return true;
        }

        if (theNestedCondition.met())
        {
            //countdown = maxTimer;
            startTime = Time.fixedTime;
            return true;
        }



        return false;

        /*
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
        */


    }

}


public class equippableObjectWIthInterTypeX : objectCriteria
{
    interType interTypeX;

    public equippableObjectWIthInterTypeX(interType interTypeXIn)
    {
        interTypeX = interTypeXIn;
    }

    public override bool evaluateObject(GameObject theObject)
    {

        equippable2 equip = theObject.GetComponent<equippable2>();
        if (equip == null) { return false; }

        if (equip.containsIntertype(interTypeX))
        {
            return true;
        }

        return false;
    }

    GameObject firstObjectOnListWIthInterTypeX(interType interTypeX, List<GameObject> theList)
    {
        //looking at the INTERACTION TYPES of their enactions

        GameObject theItemWeWant = null;

        foreach (GameObject thisObject in theList)
        {

            equippable2 equip = thisObject.GetComponent<equippable2>();
            if (equip == null) { continue; }

            if (equip.containsIntertype(interTypeX))
            {
                theItemWeWant = thisObject;
                break;
            }
        }

        return theItemWeWant;
    }

}



public class adHocHasNoIntertypeXCondition : condition
{

    GameObject theObject;
    interType intertypeX;
    bool returnTrueIfThereIsNoGun = true;


    public adHocHasNoIntertypeXCondition(GameObject theObjectIn, interType intertypeXIn, bool returnTrueIfThereIsNoGunIn = true)
    {
        theObject = theObjectIn;
        intertypeX = intertypeXIn;
        returnTrueIfThereIsNoGun = returnTrueIfThereIsNoGunIn;
    }

    public override bool met()
    {
        //so.  look in:
        //      1)  the playable
        //      2)  the equipper slots [different from "the playable"?]
        //      3)  the inventory
        //so we can break it down!  into each of those....criteria?

        enaction foundEnaction = new find().enactionOnObjectItselfWithIntertypeX(theObject, intertypeX);
        if (foundEnaction != null) { return true; }

        foundEnaction = new find().enactionEquippedByObjectWithIntertypeX(theObject, intertypeX);
        if (foundEnaction != null) { return true; }

        foundEnaction = new find().enactionInObjectsInventoryWithIntertypeX(theObject, intertypeX);
        if (foundEnaction != null) { return true; }


        return false;
    }

    /*
    public bool hasNoGun()
    {
        enaction grabEnact1;
        grabEnact1 = enactionWithInterTypeXOnObjectsPlayable(theObject, interType.peircing);



        if (grabEnact1 == null)
        {
            //ummm sloppy for now
            grabEnact1 = getFireEnactionFromEquipperSlotsToSeeIfNPCHasAGUn(interType.peircing);
        }

        if (grabEnact1 == null) { return true; }

        return false;
    }

    public enaction enactionWithInterTypeXOnObjectsPlayable(GameObject theObject, interType intertypeX)
    {
        foreach (rangedEnaction thisEnaction in listOfIEnactaBoolsOnObject(theObject))
        {

            if (thisEnaction.interInfo.interactionType == intertypeX) { return thisEnactionOrAnyBundleItsIn(theObject, thisEnaction); }
        }



        return null;
    }

    private rangedEnaction getFireEnactionFromEquipperSlotsToSeeIfNPCHasAGUn(interType interTypeX)
    {
        GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, equipperContents());

        //oh no it can ALSO be null
        if (theItemWeWant == null)
        {
            //      conditionalPrint("(theItemWeWant == null)");
            //Debug.DrawLine(Vector3.zero, this.transform.position, Color.magenta, 6f);
            return null;
            //return goGrabPlan1(interType.peircing);
        }


        //Debug.Assert(theItemWeWant != null);

        //grabEnact1 = theItemWeWant.GetComponent<rangedEnaction>();
        return theItemWeWant.GetComponent<rangedEnaction>();
    }

    GameObject firstObjectOnListWIthInterTypeX(interType interTypeX, List<GameObject> theList)
    {
        //looking at the INTERACTION TYPES of their enactions

        GameObject theItemWeWant = null;

        foreach (GameObject thisObject in theList)
        {

            equippable2 equip = thisObject.GetComponent<equippable2>();
            if (equip == null) { continue; }

            if (equip.containsIntertype(interTypeX))
            {
                theItemWeWant = thisObject;
                break;
            }
        }

        return theItemWeWant;
    }
    */

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

    public override string asTextAllTheWayDown()
    {
        throw new NotImplementedException();
    }

    public override string asTextBaseOnly()
    {
        Debug.Log("3333333333333????????????????");
        throw new NotImplementedException();
    }
}




//dynamic/input variables??????????
public class proximity : baseCondition
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
        targetCalc = new movableObjectTargetCalculator(object1, object2);//, desiredDistance);
        this.desiredDistance = desiredDistance;
        this.allowedMargin = allowedMargin;
    }

    public proximity(GameObject object1, Vector3 targetLocation2In, float desiredDistance = 4f)
    {
        this.object1 = object1;
        //this.targetLocation2 = targetLocation2In;

        targetCalc = new staticVectorTargetCalculator(object1, targetLocation2In);//, desiredDistance);
        this.desiredDistance = desiredDistance;
    }

    public override bool met()
    {
        //return false;
        Vector3 position1 = object1.transform.position;
        Vector3 position2 = targetCalc.targetPosition();// object2.transform.position;
        Vector3 vectorBetween = position1 - position2;
        float distance = new proximityCalculator(object1,targetCalc.targetPosition()).calculate();

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

}

public class proximityFromTargetPicker : baseCondition
{
    //for when we want the objects to be CLOSER than the desired distance

    GameObject object1;
    //GameObject object2;
    //Vector3 targetLocation2;
    //targetCalculator targetCalc;
    targetPicker theTargetPicker;
    float desiredDistance = 4f;
    float allowedMargin = 2f;
    public bool debugPrint = false;

    conditionalEffects2 adHocConditionalEffects;

    public proximityFromTargetPicker(GameObject object1, targetPicker theTargetPickerIn, float desiredDistance = 4f, float allowedMargin = 2f)
    {
        this.object1 = object1;
        //this.object2 = object2;
        //targetCalc = new movableObjectTargetCalculator(object1, object2);//, desiredDistance);
        theTargetPicker = theTargetPickerIn;
        this.desiredDistance = desiredDistance;
        this.allowedMargin = allowedMargin;
    }

    public override bool met()
    {

        if (theTargetPicker.pickNext().error()) { return false; }//??????????  depends if i want it or not?  well then, just reverse outcome?  dunno...

        //return false;
        Vector3 position1 = object1.transform.position;
        Vector3 position2 = theTargetPicker.pickNext().targetPosition();// object2.transform.position;
        //  Debug.Log("theTargetPicker.pickNext():  " + theTargetPicker.pickNext());
        //  Debug.Log("theTargetPicker.pickNext().targetPosition():  " + theTargetPicker.pickNext().targetPosition());
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

            Debug.DrawLine(position1, position1 + (Vector3.up * 105), Color.white, 7f);

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
        Vector3 position2 = theTargetPicker.pickNext().targetPosition();// object2.transform.position;
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
        stringToReturn += "proximity between 1)  " + object1 + ", and 2)  " + theTargetPicker.ToString();// object2;
        stringToReturn += ", [desiredDistance = " + desiredDistance + "]";
        stringToReturn += ", [metAsText() = " + metAsText() + "]";


        return stringToReturn;

    }

}


public class proximityRef : baseCondition
{
    //for when we want the objects to be CLOSER than the desired distance
    //why is "Ref" in the name?????  ohhh, references an EXE?

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
        Debug.Assert(theTargetHolder != null);
        Debug.Assert(theTargetHolder.theTargetCalculator != null);
        //this.object2 = object2;
        //targetCalc = new movableObjectTargetCalculator(object1, object2, desiredDistance);
        this.desiredDistance = desiredDistance;
        this.allowedMargin = allowedMarginIn;
    }


    public override bool met()
    {
        /*
        if(theTargetHolder.error() == true) 
        {
            Debug.Log("(theTargetHolder.error() == true)");
            return false;
        }
        */
        //return false;
        Vector3 position1 = object1.transform.position;
        Debug.Assert(theTargetHolder != null);
        Debug.Assert(theTargetHolder.theTargetCalculator != null);
        Vector3 position2 = theTargetHolder.theTargetCalculator.targetPosition();// object2.transform.position;
        Vector3 vectorBetween = position1 - position2;
        float distance = vectorBetween.magnitude;

        //Debug.Log("condition:  " + this);
        //Debug.Log("distance:  " + distance);
        //Debug.Log("desiredDistance:  " + desiredDistance);
        //Debug.Log("theTargetHolder.theTargetCalculator.GetHashCode():  " + theTargetHolder.theTargetCalculator.GetHashCode());
        //Debug.Log("theTargetHolder.theTargetCalculator.targetPosition():  " + theTargetHolder.theTargetCalculator.targetPosition());
        //Debug.Log("theTargetHolder.theTargetCalculator.targetPosition():  " + theTargetHolder.theTargetCalculator.tar);
        //  Debug.DrawLine(position1, position2, Color.magenta, 0.1f);


        if (debugPrint)
        {
            Debug.Log("distance:  " + distance);
            Debug.Log("desiredDistance:  " + desiredDistance);
            Debug.DrawLine((position1 + Vector3.up), (position2 + Vector3.up), Color.blue, 7f);

            Debug.DrawLine(position1, position1 + (Vector3.up * 105), Color.white, 7f);

            Debug.DrawLine(position2, position2 + (Vector3.up * 105), Color.black, 7f);
        }


        if (distance > (desiredDistance+ allowedMargin)) { return false; }


        //Debug.Log("met");
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

}


public class adocThreatLineOfSightCondition : baseCondition
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

public class targetMatchesHitscanOutput : baseCondition
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

public class objectMeetsAllCriteria : objectCriteria
{

    List<objectCriteria> theCriteria = new List<objectCriteria>();

    public objectMeetsAllCriteria(objectCriteria criteria1)
    {
        theCriteria.Add(criteria1);
    }
    public objectMeetsAllCriteria(objectCriteria criteria1, objectCriteria criteria2)
    {
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
    }
    public objectMeetsAllCriteria(objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3)
    {
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
    }
    public objectMeetsAllCriteria(objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3, objectCriteria criteria4)
    {
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
        theCriteria.Add(criteria4);
    }
    public objectMeetsAllCriteria(objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3, objectCriteria criteria4, objectCriteria criteria5)
    {
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
        theCriteria.Add(criteria4);
        theCriteria.Add(criteria5);
    }
    public objectMeetsAllCriteria(objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3, objectCriteria criteria4, objectCriteria criteria5, objectCriteria criteria6)
    {
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
        theCriteria.Add(criteria4);
        theCriteria.Add(criteria5);
        theCriteria.Add(criteria6);
    }
    public objectMeetsAllCriteria(objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3, objectCriteria criteria4, objectCriteria criteria5, objectCriteria criteria6, objectCriteria criteria7)
    {
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
        theCriteria.Add(criteria4);
        theCriteria.Add(criteria5);
        theCriteria.Add(criteria6);
        theCriteria.Add(criteria7);
    }
    public objectMeetsAllCriteria(objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3, objectCriteria criteria4, objectCriteria criteria5, objectCriteria criteria6, objectCriteria criteria7, objectCriteria criteria8)
    {
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
        theCriteria.Add(criteria4);
        theCriteria.Add(criteria5);
        theCriteria.Add(criteria6);
        theCriteria.Add(criteria7);
        theCriteria.Add(criteria8);
    }

    public override bool evaluateObject(GameObject theObject)
    {

        //Debug.Log("###################################################");
        foreach (objectCriteria thisCriteria in theCriteria)
        {

            //Debug.Log("thisCriteria:  "+ thisCriteria);
            if (thisCriteria.evaluateObject(theObject) == false)
            {
                //Debug.Log("NOT MET");
                return false;
            }
            //Debug.Log("met");
        }

        return true;
    }
}

public class isThereAtLeastOneObjectInSet : baseCondition
{
    objectSetGrabber theObjectSetGrabber;


    public isThereAtLeastOneObjectInSet(objectSetGrabber theObjectSetGrabberIn)
    {
        theObjectSetGrabber = theObjectSetGrabberIn;
    }


    public override bool met()
    {
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        if (theObjectSetGrabber.grab() == null){ return false; }
        if (theObjectSetGrabber.grab().Count > 0)
        {
            //Debug.Log("(theObjectSetGrabber.grab().Count > 0) is TRUE:  " + theObjectSetGrabber.grab().Count + "  " + theObjectSetGrabber.grab()[0]);
            return true;
        }

        //Debug.Log("fffffffffffffffffalssssssssssssssssse"   + this.GetHashCode());
        return false;
    }


    public override string asText()
    {
        string stringToReturn = "";

        stringToReturn += "met?  " + met() + ", ";//why is this returning a different result???

        stringToReturn += this.ToString();

        return stringToReturn;
    }
    public override string asTextBaseOnly()
    {
        //Debug.Log("444444444444444444444????????????????");
        string theString = this + " (theObjectSetGrabber = " + theObjectSetGrabber + ")";
        return theString;
    }
}

public class isThereAtLeastOneObjectThatMeetsCriteria : baseCondition
{
    private objectCriteria theCriteria;
    private tag2 initialSearchTag;

    public isThereAtLeastOneObjectThatMeetsCriteria(objectCriteria theCriteriaIn)
    {
        theCriteria = theCriteriaIn;
        initialSearchTag = tag2.zoneable;
    }
    public isThereAtLeastOneObjectThatMeetsCriteria(tag2 initialSearchTagIn, objectCriteria theCriteriaIn)
    {
        theCriteria = theCriteriaIn;
        initialSearchTag = initialSearchTagIn;
    }

    public override bool met()
    {
        List<GameObject> allObjects = tagging2.singleton.allObjectsWithTag(initialSearchTag);
        foreach(GameObject thisObject in allObjects)
        {
            if (theCriteria.evaluateObject(thisObject) == true) { return true; }
        }

        return false;
    }
}


//conditions

public class canSeeStuffStuff : baseCondition
{

    stuffType theStuffType;


    GameObject theObjectThatIsLooking;
    bool returnTrueIfThisObjectCanSeeStuffStuff = true;

    public canSeeStuffStuff(GameObject theObjectThatIsLookingIn, stuffType theStuffTypeIn, bool returnTrueIfThisObjectCanSeeStuffStuffIn = true)
    {
        theObjectThatIsLooking = theObjectThatIsLookingIn;
        returnTrueIfThisObjectCanSeeStuffStuff = returnTrueIfThisObjectCanSeeStuffStuffIn;
        theStuffType = theStuffTypeIn;
    }


    public override bool met()
    {
        OldSpatialDataPoint myData = new OldSpatialDataPoint(new setOfAllNearbyStuffStuff(theObjectThatIsLooking, theStuffType).grab(), theObjectThatIsLooking.transform.position);


        bool threatLineOfSightBool = myData.threatLineOfSightBool();

        if (threatLineOfSightBool == returnTrueIfThisObjectCanSeeStuffStuff)
        {
            return true;
        }

        return false;
    }


    public override string asTextSHORT()
    {
        string stringToReturn = "";
        if (returnTrueIfThisObjectCanSeeStuffStuff)
        {
            stringToReturn += "do IF this object can see stuffStuff";
        }
        else
        {
            stringToReturn += "do if this object CANNOT see stuffStuff";
        }

        return stringToReturn;
    }

}

public class canSeeNumericalVariable : baseCondition
{

    numericalVariable theVariableType;


    GameObject theObjectThatIsLooking;
    bool returnTrueIfThisObjectCanSeeStuffStuff = true;

    public canSeeNumericalVariable(GameObject theObjectThatIsLookingIn, numericalVariable theVariableTypeIn, bool returnTrueIfThisObjectCanSeeStuffStuffIn = true)
    {
        theObjectThatIsLooking = theObjectThatIsLookingIn;
        returnTrueIfThisObjectCanSeeStuffStuff = returnTrueIfThisObjectCanSeeStuffStuffIn;
        theVariableType = theVariableTypeIn;
    }


    public override bool met()
    {
        OldSpatialDataPoint myData = new OldSpatialDataPoint(new setOfAllNearbyNumericalVariable(theObjectThatIsLooking, theVariableType).grab(), theObjectThatIsLooking.transform.position);


        bool threatLineOfSightBool = myData.threatLineOfSightBool();

        if (threatLineOfSightBool == returnTrueIfThisObjectCanSeeStuffStuff)
        {
            return true;
        }

        return false;
    }

    public override string asTextSHORT()
    {
        string stringToReturn = "";
        if (returnTrueIfThisObjectCanSeeStuffStuff)
        {
            stringToReturn += "do IF this object can see stuffStuff";
        }
        else
        {
            stringToReturn += "do if this object CANNOT see stuffStuff";
        }

        return stringToReturn;
    }

}

public class depletableSingleEXEListComplete : baseCondition
{
    List<singleEXE> planList;

    public depletableSingleEXEListComplete(List<singleEXE> planList)
    {
        this.planList = planList;
    }

    public override bool met()
    {
        //Debug.Log("planList.Count:  " + planList.Count);
        foreach (singleEXE planEXE in planList)
        {
            //Debug.Log("planEXE.asText():  " + planEXE.asText());
            if (planEXE == null) { continue; } //messy annoying for now
            if (planEXE.endConditionsMet() == false)
            {
                return false;
            }
        }

        return true;
    }

}

public class individualObjectMeetsAllCriteria : condition //not really "base condition", has nested CRITERIA i may need to print
{
    individualObjectReturner theObjectReturner;
    List<objectCriteria> theCriteria = new List<objectCriteria>();

    public individualObjectMeetsAllCriteria(individualObjectReturner theObjectReturnerIn, objectCriteria criteria1)
    {
        theObjectReturner = theObjectReturnerIn;
        theCriteria.Add(criteria1);
    }
    public individualObjectMeetsAllCriteria(individualObjectReturner theObjectReturnerIn, objectCriteria criteria1, objectCriteria criteria2)
    {
        theObjectReturner = theObjectReturnerIn;
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
    }
    public individualObjectMeetsAllCriteria(individualObjectReturner theObjectReturnerIn, objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3)
    {
        theObjectReturner = theObjectReturnerIn;
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
    }
    public individualObjectMeetsAllCriteria(individualObjectReturner theObjectReturnerIn, objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3, objectCriteria criteria4)
    {
        theObjectReturner = theObjectReturnerIn;
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
        theCriteria.Add(criteria4);
    }
    public individualObjectMeetsAllCriteria(individualObjectReturner theObjectReturnerIn, objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3, objectCriteria criteria4, objectCriteria criteria5)
    {
        theObjectReturner = theObjectReturnerIn;
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
        theCriteria.Add(criteria4);
        theCriteria.Add(criteria5);
    }
    public individualObjectMeetsAllCriteria(individualObjectReturner theObjectReturnerIn, objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3, objectCriteria criteria4, objectCriteria criteria5, objectCriteria criteria6)
    {
        theObjectReturner = theObjectReturnerIn;
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
        theCriteria.Add(criteria4);
        theCriteria.Add(criteria5);
        theCriteria.Add(criteria6);
    }
    public individualObjectMeetsAllCriteria(individualObjectReturner theObjectReturnerIn, objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3, objectCriteria criteria4, objectCriteria criteria5, objectCriteria criteria6, objectCriteria criteria7)
    {
        theObjectReturner = theObjectReturnerIn;
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
        theCriteria.Add(criteria4);
        theCriteria.Add(criteria5);
        theCriteria.Add(criteria6);
        theCriteria.Add(criteria7);
    }
    public individualObjectMeetsAllCriteria(individualObjectReturner theObjectReturnerIn, objectCriteria criteria1, objectCriteria criteria2, objectCriteria criteria3, objectCriteria criteria4, objectCriteria criteria5, objectCriteria criteria6, objectCriteria criteria7, objectCriteria criteria8)
    {
        theObjectReturner = theObjectReturnerIn;
        theCriteria.Add(criteria1);
        theCriteria.Add(criteria2);
        theCriteria.Add(criteria3);
        theCriteria.Add(criteria4);
        theCriteria.Add(criteria5);
        theCriteria.Add(criteria6);
        theCriteria.Add(criteria7);
        theCriteria.Add(criteria8);
    }




    public override bool met()
    {
        foreach (objectCriteria thisCriteria in theCriteria)
        {
            if (thisCriteria.evaluateObject(theObjectReturner.returnObject()) == false)
            {
                return false;
            }
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

    public override string asTextAllTheWayDown()
    {
        string theText = "[";
        foreach (objectCriteria thisCondition in theCriteria)
        {
            theText += thisCondition;
            theText += ", ";
        }

        theText += "]";

        return theText;
    }

    public override string asTextBaseOnly()
    {
        Debug.Log("555555555555555????????????????");
        string theText = this + " (object returner = " + theObjectReturner +")";

        theText += "[";

        theText += asTextAllTheWayDown();

        theText += "]";

        return theText;
    }
}




//either?  unsure?
public class numericalCondition : baseCondition
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


}








public class canAcquireTarget : baseCondition
{
    targetPicker theTargetPicker;

    public canAcquireTarget(targetPicker theTargetPickerIn)
    {
        theTargetPicker = theTargetPickerIn;
    }

    public override bool met()
    {
        return (theTargetPicker.pickNext() != null);
    }

}

public class nonNullObject : condition //not really "base condition", has nested individualObjectReturner i may need to print
{
    individualObjectReturner theIndividualObjectReturner;

    public nonNullObject(individualObjectReturner theIndividualObjectReturnerIn)
    {
        theIndividualObjectReturner = theIndividualObjectReturnerIn;
    }

    public override bool met()
    {
        return (theIndividualObjectReturner.returnObject() != null);
    }











    public override string asText()
    {
        return standardAsText();
    }

    public override string asTextSHORT()
    {
        return standardAsTextSHORT();
    }


    public override string asTextAllTheWayDown()
    {
        return asText();
    }

    public override string asTextBaseOnly()
    {
        Debug.Log("66666666666666666????????????????");
        string newString = "["+this + ": " + theIndividualObjectReturner+"]";
        return newString;
    }
}






//"bool" criteria

public abstract class objectCriteria
{
    // [BOOLEAN] function to evaluate a single object

    public abstract bool evaluateObject(GameObject theObject);
}

public class stickyTrueCriteria : objectCriteria
{
    objectCriteria theNestedCriteria;


    int countdown = 0;
    int maxTimer = 90;


    public stickyTrueCriteria(objectCriteria theNestedCriteriaIn, int maxTimerIn = 90)
    {
        theNestedCriteria = theNestedCriteriaIn;
        maxTimer = maxTimerIn;
    }




    public override bool evaluateObject(GameObject theObject)
    {
        if (countdown > 0)
        {
            countdown--;
            return true;
        }

        if (theNestedCriteria.evaluateObject(theObject))
        {
            countdown = maxTimer;
            return true;
        }

        return false;
    }
}


public class objectVisibleInFOV : objectCriteria, positionCriteria
{
    //FOV = "feild of view" or "field of view".  we assume line of sight is clear
    float horizontalAngleRange = 90f;
    float verticalAngleRange = 60f;
    Transform theSensoryTransform;




    public objectVisibleInFOV(Transform theSensoryTransformIn, float horizontalAngleRangeIn = 90f, float verticalAngleRangeIn = 60f)
    {
        theSensoryTransform = theSensoryTransformIn;
        horizontalAngleRange = horizontalAngleRangeIn;
        verticalAngleRange = verticalAngleRangeIn;
    }





    public override bool evaluateObject(GameObject theObject)
    {
        //get angles [horizontal, vertical] then compare to limit?


        //what about positive VS negative angle or whatever?
        //try:
        //      && horizontalAngleToObject < (360-horizontalAngleRange)







        //Debug.DrawLine(theSensoryTransform.position, (theSensoryTransform.position + (theSensoryTransform.forward * 30)), Color.magenta, 20);

        float horizontalAngleToObject = absoluteHorizontalAngleFinder(theObject.transform.position);
        //Debug.Log("horizontalAngleToObject:  " + horizontalAngleToObject);
        if (horizontalAngleToObject > horizontalAngleRange)
        {
            //Debug.DrawLine(theSensoryTransform.position, theObject.transform.position, Color.red, 20);
            return false;
        }

        float verticalAngleToObject = absoluteVerticalAngleFinder(theObject.transform.position);
        //Debug.Log("verticalAngleToObject:  " + verticalAngleToObject);
        if (verticalAngleToObject > verticalAngleRange)
        {
            //Debug.DrawLine(theSensoryTransform.position, theObject.transform.position, Color.red, 20);
            return false;
        }

        //Debug.DrawLine(theSensoryTransform.position, theObject.transform.position, Color.green, 20);

        return true;







        /*
        Debug.DrawLine(theSensoryTransform.position, (theSensoryTransform.position + (theSensoryTransform.forward * 30)), Color.magenta, 20);

        float horizontalAngleToObject = absoluteHorizontalAngleFinder(theObject);
        Debug.Log("horizontalAngleToObject:  " + horizontalAngleToObject);
        float verticalAngleToObject = absoluteVerticalAngleFinder(theObject);
        Debug.Log("verticalAngleToObject:  " + verticalAngleToObject);

        Debug.DrawLine(theSensoryTransform.position, theObject.transform.position, Color.blue, 20);

        return true;
        */










        /*
        Debug.DrawLine(theSensoryTransform.position, (theSensoryTransform.position+(theSensoryTransform.forward*30)), Color.magenta, 20);

        float horizontalAngleToObject = horizontalAngleFinder(theObject);
        Debug.Log("horizontalAngleToObject:  "+ horizontalAngleToObject);
        if (horizontalAngleToObject > horizontalAngleRange && horizontalAngleToObject < (360 - horizontalAngleRange)) 
        {
            Debug.DrawLine(theSensoryTransform.position, theObject.transform.position, Color.red, 20);
            return false; 
        }

        float verticalAngleToObject = verticalAngleFinder(theObject);
        Debug.Log("verticalAngleToObject:  " + verticalAngleToObject);
        if (verticalAngleToObject > verticalAngleRange && verticalAngleToObject < (360 - verticalAngleRange))
        {
            Debug.DrawLine(theSensoryTransform.position, theObject.transform.position, Color.red,20);
            return false; 
        }

        Debug.DrawLine(theSensoryTransform.position, theObject.transform.position, Color.green,20);

        return true;
        */
    }


    private float absoluteVerticalAngleFinder(Vector3 thePoint)
    {
        //Ray observerLookingRay = new Ray(theSensoryTransform.position, theSensoryTransform.forward);//targetObject.GetComponent<sensorySystem>().lookingRay;
        //Vector3 lineBetweenObserverAndInputObject = theSensoryTransform.position - theObject.transform.position;

        Vector3 lineBetweenObserverAndInput= thePoint - theSensoryTransform.position;


        //float theAngle = Vector3.Angle(observerLookingRay.direction, lineBetweenObserverAndInputObject);
        float theAngle = AngleOffAroundAxis(lineBetweenObserverAndInput,
            theSensoryTransform.forward,
            theSensoryTransform.right);

        if(theAngle < 0) {return - theAngle; }

        return theAngle;
    }

    private float absoluteHorizontalAngleFinder(Vector3 thePoint)
    {
        //Ray observerLookingRay = new Ray(theSensoryTransform.position, theSensoryTransform.forward);//targetObject.GetComponent<sensorySystem>().lookingRay;
        //Vector3 lineBetweenObserverAndInputObject = theSensoryTransform.position - theObject.transform.position;

        Vector3 lineBetweenObserverAndInput = thePoint - theSensoryTransform.position;


        float theAngle = AngleOffAroundAxis(lineBetweenObserverAndInput,
            theSensoryTransform.forward,
            theSensoryTransform.up);


        if (theAngle < 0) { return -theAngle; }

        return theAngle;
    }







    private float verticalAngleFinder(GameObject theObject)
    {
        //Ray observerLookingRay = new Ray(theSensoryTransform.position, theSensoryTransform.forward);//targetObject.GetComponent<sensorySystem>().lookingRay;
        Vector3 lineBetweenObserverAndInputObject = theSensoryTransform.position - theObject.transform.position;



        //float theAngle = Vector3.Angle(observerLookingRay.direction, lineBetweenObserverAndInputObject);
        float theAngle = AngleOffAroundAxis(lineBetweenObserverAndInputObject,
            theSensoryTransform.forward,
            theSensoryTransform.right);


        return theAngle;
    }

    private float horizontalAngleFinder(GameObject theObject)
    {
        Vector3 lineBetweenObserverAndInputObject = theSensoryTransform.position - theObject.transform.position;


        float theAngle = AngleOffAroundAxis(lineBetweenObserverAndInputObject,
            theSensoryTransform.forward,
            theSensoryTransform.up);


        return theAngle;
    }






    /*
    private float getHorizontalAngle(Vector3 lineToTarget)
    {
        //https://forum.unity.com/threads/is-vector3-signedangle-working-as-intended.694105/

        float oneAngle = AngleOffAroundAxis(lineToTarget.normalized, 
            theVectorRotationEnaction.thePartToAimHorizontal.forward, 
            theVectorRotationEnaction.thePartToAimHorizontal.up);

        //float oneAngle = AngleOffAroundAxis(lineToTarget.normalized, this.transform.forward, theVectorRotationEnaction.thePartToAimHorizontal.up);

        return oneAngle;
    }

    private float getVerticalAngle(Vector3 lineToTarget)
    {

        Vector3 start = theVectorRotationEnaction.thePartToAimVertical.position;
        Vector3 offset = new Vector3(0.01f, 0.01f, 0.01f);
        Debug.DrawLine(start + offset, start + lineToTarget.normalized + offset, Color.white, 4f);

        float oneAngle = AngleOffAroundAxis(lineToTarget, 
            theVectorRotationEnaction.thePartToAimVertical.forward, 
            theVectorRotationEnaction.thePartToAimVertical.right);
        //fixed it!  my input vector was just target position!  i needed to be using line from aiming object to the target it is aiming at!  position relative to the person doing the aiming, basically

        //this one has to be negative for some reason??
        return -oneAngle;
    }


    */





    public float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis, bool clockwise = false)
    {
        //from here:
        //https://forum.unity.com/threads/is-vector3-signedangle-working-as-intended.694105/

        //but had to change conversion thing from "MathUtil.RAD_TO_DEG" to the following:
        //Mathf.Rad2Deg


        Vector3 right;
        if (clockwise)
        {
            right = Vector3.Cross(forward, axis);
            forward = Vector3.Cross(axis, right);
        }
        else
        {
            right = Vector3.Cross(axis, forward);
            forward = Vector3.Cross(right, axis);
        }


        return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
    }

    public bool evaluatePosition(Vector3 thePosition)
    {
        float horizontalAngleToObject = absoluteHorizontalAngleFinder(thePosition);
        //Debug.Log("horizontalAngleToObject:  " + horizontalAngleToObject);
        if (horizontalAngleToObject > horizontalAngleRange)
        {
            //Debug.DrawLine(theSensoryTransform.position, theObject.transform.position, Color.red, 20);
            return false;
        }

        float verticalAngleToObject = absoluteVerticalAngleFinder(thePosition);
        //Debug.Log("verticalAngleToObject:  " + verticalAngleToObject);
        if (verticalAngleToObject > verticalAngleRange)
        {
            //Debug.DrawLine(theSensoryTransform.position, theObject.transform.position, Color.red, 20);
            return false;
        }

        //Debug.DrawLine(theSensoryTransform.position, theObject.transform.position, Color.green, 20);

        return true;


    }
}


public class proximityCriteriaBool : objectCriteria
{
    //for when we want the objects to be CLOSER than the desired distance

    targetCalculator weWantSomethingNearToThis;

    float desiredDistance = 4f;
    float allowedMargin = 2f;


    public override bool evaluateObject(GameObject theObject)
    {

        //return false;
        Vector3 position1 = weWantSomethingNearToThis.targetPosition();
        Vector3 position2 = theObject.transform.position;

        Vector3 vectorBetween = position1 - position2;
        float distance = vectorBetween.magnitude;

        //Debug.Log("position1:  " + position1);
        //Debug.Log("position2:  " + position2);
        //Debug.Log("distance:  " + distance);

        //Debug.Log("this.GetHashCode():  " + this.GetHashCode());
        //Debug.Log("desiredDistance:  " + desiredDistance);
        //Debug.Log("allowedMargin:  " + allowedMargin);
        //Debug.Log("(desiredDistance + allowedMargin):  " + (desiredDistance + allowedMargin));
        if (distance > (desiredDistance + allowedMargin)) { return false; }

        return true;
    }





    public proximityCriteriaBool(GameObject objectToBeNearIn, float desiredDistance = 4f, float allowedMargin = 2f)
    {
        weWantSomethingNearToThis = new agnosticTargetCalc(objectToBeNearIn);// gahhhhhhh target calculator assumes access to TWO things!  indexer need one that only needs ONE!!!  [fixed]


        this.desiredDistance = desiredDistance;
        this.allowedMargin = allowedMargin;


        //Debug.Log("this.GetHashCode():  " + this.GetHashCode());
        //Debug.Log("desiredDistance:  " + desiredDistance);
        //Debug.Log("allowedMargin:  " + allowedMargin);


    }

    public proximityCriteriaBool(Vector3 whereWeWantToBeCloseToIn, float desiredDistance = 4f, float allowedMargin = 2f)
    {
        weWantSomethingNearToThis = new agnosticTargetCalc(whereWeWantToBeCloseToIn);

        this.desiredDistance = desiredDistance;
        this.allowedMargin = allowedMargin;
    }
    public proximityCriteriaBool(targetCalculator whereWeWantToBeCloseToIn, float desiredDistance = 4f, float allowedMargin = 2f)
    {
        weWantSomethingNearToThis = whereWeWantToBeCloseToIn;

        this.desiredDistance = desiredDistance;
        this.allowedMargin = allowedMargin;
    }
}


public class objectHasTag : objectCriteria
{
    tagging2.tag2 theTag;

    public objectHasTag(tagging2.tag2 theTagIn)
    {
        this.theTag = theTagIn;
    }

    public override bool evaluateObject(GameObject theObject)
    {
        return tagging2.singleton.allTagsOnObject(theObject).Contains(theTag);
    }
}

public class reverseCriteria : objectCriteria
{
    objectCriteria criteriaToReverse;

    public reverseCriteria(objectCriteria criteriaToReverseIn)
    {

        criteriaToReverse = criteriaToReverseIn;
    }

    public override bool evaluateObject(GameObject theObject)
    {
        if (criteriaToReverse.evaluateObject(theObject)) { return false; }
        return true;
    }
}

public class hasVirtualGamepad : objectCriteria
{
    public override bool evaluateObject(GameObject theObject)
    {
        virtualGamepad theGamepad = theObject.GetComponent<virtualGamepad>();
        return (theGamepad != null);
    }
}

public class hasRtsModule : objectCriteria
{
    public override bool evaluateObject(GameObject theObject)
    {
        rtsModule theComponent = theObject.GetComponent<rtsModule>();
        //Debug.Log("???????????????????????????????????????????????????????");
        //Debug.Log("theComponent:  " + theComponent);
        //Debug.Assert(theComponent != null);
        return (theComponent != null);
    }
}

public class hasAdvancedRtsModule : objectCriteria
{
    public override bool evaluateObject(GameObject theObject)
    {
        advancedRtsModule theComponent = theObject.GetComponent<advancedRtsModule>();
        
        return (theComponent != null);
    }
}


public class hasNoOrders : objectCriteria
{
    public override bool evaluateObject(GameObject theObject)
    {
        rtsModule theComponent = theObject.GetComponent<rtsModule>();
        //if(theComponent.currentReceivedCommandAndWhoGaveThem == null || theComponent.currentReceivedCommandAndWhoGaveThem.Count == 0) { return true; }
        if (theComponent.currentReceivedCommand == null) { return true; }

        return false;
    }
}
public class hasNoAdvancedCommands : objectCriteria
{
    public override bool evaluateObject(GameObject theObject)
    {
        advancedRtsModule theComponent = theObject.GetComponent<advancedRtsModule>();
        //if(theComponent.currentReceivedCommandAndWhoGaveThem == null || theComponent.currentReceivedCommandAndWhoGaveThem.Count == 0) { return true; }
        if (theComponent.currentReceivedCommand == null) { return true; }

        return false;
    }
}

public class lineOfSight : objectCriteria, positionCriteria
{
    GameObject theCentralObserver;
    float theRange = 60f;


    public lineOfSight(GameObject theCentralObserverIn, float theRangeIn = 60f)
    {
        theCentralObserver = theCentralObserverIn;
        theRange = theRangeIn;
    }

    public override bool evaluateObject(GameObject theObject)
    {

        //OldSpatialDataPoint myData = new OldSpatialDataPoint(threatListWithoutSelf(), this.transform.position);



        //return myData.threatLineOfSightBool();


        //bool theBool = false;
        RaycastHit myHit;

        //new Ray(this.transform.position, theBody.theWorldScript.theTagScript.semiRandomUsuallyNearTargetPickerFromList(theBody.theLocalMapZoneScript.theList, this.gameObject).transform.position);
        Vector3 theDirection = theObject.transform.position - theCentralObserver.transform.position;
        Ray myRay = new Ray(theCentralObserver.transform.position, theDirection);


        if (Physics.Raycast(myRay, out myHit, theRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (myHit.collider.gameObject == theObject)
            {
                return true;
            }
        }

        return false;
    }

    public bool evaluatePosition(Vector3 thePosition)
    {
        //"true" = yes, line of sight, visible
        RaycastHit myHit;

        Vector3 theDirection = thePosition - theCentralObserver.transform.position;

        if(theDirection.magnitude > theRange) { return false; }

        Ray myRay = new Ray(theCentralObserver.transform.position, theDirection);

        //we dont have objects for collision, soooo just see if NULL collider, ya
        if (Physics.Raycast(myRay, out myHit, theDirection.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return true;
    }
}



public class intertypeXisOnObject : objectCriteria
{
    interType interTypeX;

    public intertypeXisOnObject(interType interTypeXIn)
    {
        interTypeX = interTypeXIn;
    }

    //that's where "playables" store their enactions too, as (enaction) components all over object
    public override bool evaluateObject(GameObject theObject)
    {
        if(new find().enactionOnObjectItselfWithIntertypeX(theObject,interTypeX) != null) { return true; }

        return false;
    }
}

public class intertypeXisInEquipperSlots : objectCriteria
{

    interType interTypeX;

    public intertypeXisInEquipperSlots(interType interTypeXIn)
    {
        interTypeX = interTypeXIn;
    }

    public override bool evaluateObject(GameObject theObject)
    {
        if (new find().enactionEquippedByObjectWithIntertypeX(theObject, interTypeX) != null) { return true; }

        return false;
    }

    //that's where "playables" store their enactions too, as (enaction) components all over object



    /*
    public override bool evaluateObject(GameObject theObject)
    {
        playable2 thePlayable = theObject.GetComponent<playable2>();

        foreach(var key in thePlayable.equipperSlotsAndContents.Keys)
        {
            GameObject equippedObject = thePlayable.equipperSlotsAndContents[key];

            if (new find().enactionOnObjectItselfWithIntertypeX(equippedObject, interTypeX) != null) { return true; }
        }

        return false;
    }
    */
}

public class intertypeXisInInventory : objectCriteria
{

    interType interTypeX;

    public intertypeXisInInventory(interType interTypeXIn)
    {
        interTypeX = interTypeXIn;
    }

    //that's where "playables" store their enactions too, as (enaction) components all over object
    public override bool evaluateObject(GameObject theObject)
    {
        inventory1 theInventory = theObject.GetComponent<inventory1>();

        foreach (GameObject inventoryItem in theInventory.inventoryItems)
        {
            if (new find().enactionOnObjectItselfWithIntertypeX(inventoryItem, interTypeX) != null) { return true; }
        }

        return false;
    }
}




public interface positionCriteria
{
    bool evaluatePosition(Vector3 thePosition);
}





//"float"/scalar criteria

public abstract class objectEvaluator
{
    // [FLOAT that can later/elsewhere be used to RANK objects, and pick the "most"] function to evaluate a single object

    public abstract float evaluateObject(GameObject theObject);
}

public interface positionEvaluation
{
    float evaluatePosition(Vector3 thePoint);
}



public abstract class evaluateSet
{
    //objectEvaluator theEvaluator;
    //objectSetGrabber theSetGrabber;

    public abstract Dictionary<float, GameObject> evaluateObjectSet();
}

public abstract class sorter
{
    //objectEvaluator theEvaluator;
    //objectSetGrabber theSetGrabber;
    //evaluateSet theSetEvaluator;

    public abstract List<GameObject> sortSet();
}

public class returnSortedIndexList
{
    //gives a list of index integers
    //if you use the first integer on that list to grab from the unsorted other list,
    //you get the first item that would be on the sorted list
    //thus it doesn't matter what TYPE of thing the other list consists of [gameObject, component, class object, etc]
    //ranked with highest float value first

    List<int> sortedIndexList = new List<int>();

    public returnSortedIndexList(List<float> unsortedListIn)
    {
        sortedIndexList = returnFullIndexMap(unsortedListIn);
    }





    /*
    
    public List<int> onePassSort(List<float> unsortedListIn)
    {


        foreach (float thisValue in unsortedListIn)
        {

        }



    }

    public List<int> onePassIndexSorter(List<float> unsortedListIn)
    {
        List<int> sortingTheIndexList = new List<int>();

        

        foreach (float thisValue in unsortedListIn) 
        {

        }

    }



    public List<int> rankThisMess(Dictionary<int, GameObject> dictionaryOfObjects, Dictionary<int, float> dictionaryOfValues)
    {
        List<int> theRanking = new List<int>();
        int numberOfRanks = 0;

        foreach (int thisKey in dictionaryOfValues.Keys)
        {
            numberOfRanks++;  //is this NOT redundant?  don't i just need to know the count of "theRanking"?

            if (theRanking.Count == 0)
            {
                //it's the first rank by default
                theRanking.Add(thisKey);
            }
            else
            {
                //have to actaully rank this....key....
                theRanking = thisMess(theRanking, dictionaryOfObjects, dictionaryOfValues, thisKey);
            }
        }

        return theRanking;
    }

    public List<int> thisMess(List<int> theRanking, Dictionary<int, GameObject> dictionaryOfObjects, Dictionary<int, float> dictionaryOfValues, int thisKey)
    {
        //this function determines where the "current" distance [grabbed from the "dictionaryOfValues", using the "thisKey"]
        //ranks among all of the other values that have been ranked so far.
        //if it's worse than any of the ones ranked so far, it is added to the END of the ranking
        bool lookingForPosition = true;

        //create one index for each entry in "theRanking" [the list of keys ranked by value]
        List<int> listOfRankIndexes = new List<int>();
        int indexCounter = 0;
        while (indexCounter < theRanking.Count)
        {
            listOfRankIndexes.Add(indexCounter);
            indexCounter++;
        }





        //      so.....compare current distance (dictionaryOfDistances[thisKey]) against every single other distance that has been ranked so far,
        //      until i find one it is better than [then STOP the loop!]
        int needToKnowTheIndexForTheTheRankingList = 0;
        foreach (int otherDistanceThatHasBeenRankedSoFarKey in theRanking)
        {
            if (dictionaryOfDistances[thisKey] < dictionaryOfDistances[otherDistanceThatHasBeenRankedSoFarKey])
            {
                theRanking.Insert(needToKnowTheIndexForTheTheRankingList, thisKey);
                lookingForPosition = false;
                break;
            }

            needToKnowTheIndexForTheTheRankingList++;
        }

        //if it wasn't better than any of the ones ranked so far, add it to the end of the list:
        if (lookingForPosition)
        {
            //no position was found, append this key to the end of list
            theRanking.Add(thisKey);
        }

        return theRanking;
    }


    public List<float> insertValueInRanking(List<float> theCurrentRanking, float valueToRank)
    {
        //sort by highest first

        int oldIndex = 0;

        while (oldIndex < theCurrentRanking.Count)
        {
            float thisRankedValue = theCurrentRanking[oldIndex];

            if (valueToRank > thisRankedValue)
            {
                theCurrentRanking.Insert(oldIndex, valueToRank);
                return theCurrentRanking;
            }

            oldIndex++;
        }

        //if it wasn't better than any of the ones ranked so far, add it to the end of the list:
        theCurrentRanking.Add(valueToRank);
        return theCurrentRanking;
    }


    public List<int> insertValueInRankingAndReturnIndexMap(List<int>  currentIndexMap, List<float> theCurrentRanking, float valueToRank, int indexOfValueToRank)
    {
        //sort by highest first

        int oldIndex = 0;

        while (oldIndex < theCurrentRanking.Count)
        {
            float thisRankedValue = theCurrentRanking[oldIndex];

            if (valueToRank > thisRankedValue)
            {
                theCurrentRanking.Insert(oldIndex, valueToRank);
                currentIndexMap.Insert(oldIndex, indexOfValueToRank);
                return currentIndexMap;
            }

            oldIndex++;
        }

        //if it wasn't better than any of the ones ranked so far, add it to the end of the list:
        theCurrentRanking.Add(valueToRank);
        currentIndexMap.Add(indexOfValueToRank);
        return currentIndexMap;
    }

    */

    public List<int> returnFullIndexMap(List<float> unsortedValuesToRank)
    {
        if (unsortedValuesToRank.Count == 0) { return null; }

        //sort by highest first
        List<int> currentIndexMap = new List<int>();
        //List<float> theCurrentRanking = new List<float>();

        //currentIndexMap[0] = 0;
        currentIndexMap.Add(0);
        int currentOldIndex = 1;

        //Debug.Assert((currentOldIndex < unsortedValuesToRank.Count));
        while (currentOldIndex < unsortedValuesToRank.Count)
        {
            float thisUnsortedValue = unsortedValuesToRank[currentOldIndex];

            int temporaryIndexOfCurrentIndexMap = 0;
            while (temporaryIndexOfCurrentIndexMap < currentIndexMap.Count)
            {
                float thisTemporarilySortedValue = unsortedValuesToRank[currentIndexMap[temporaryIndexOfCurrentIndexMap]];

                //Debug.Log("thisTemporarilySortedValue: " + thisTemporarilySortedValue);
                //Debug.Log("thisUnsortedValue:  " + thisUnsortedValue);
                //Debug.Log("(thisTemporarilySortedValue > thisUnsortedValue):  " + (thisTemporarilySortedValue > thisUnsortedValue));
                if (thisTemporarilySortedValue > thisUnsortedValue){temporaryIndexOfCurrentIndexMap++; continue; }


                //Debug.Log("currentIndexMap.Count:  " + currentIndexMap.Count);
                temporaryIndexOfCurrentIndexMap++;
            }

            currentIndexMap.Insert(temporaryIndexOfCurrentIndexMap, currentOldIndex);
            currentOldIndex++;
        }

        return currentIndexMap;
    }






    public List<int> returnIt()
    {
        return sortedIndexList;
    }
}