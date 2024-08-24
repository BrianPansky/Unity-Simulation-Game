using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static interactionCreator;

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



public interface condition
{

    bool met();
}

public class proximity : condition
{
    //for when we want the objects to be CLOSER than the desired distance

    GameObject object1;
    GameObject object2;
    float desiredDistance = 4f;

    public proximity(GameObject object1, GameObject object2, float desiredDistance = 4f)
    {
        this.object1 = object1;
        this.object2 = object2;
        this.desiredDistance = desiredDistance;
    }

    public bool met()
    {
        Vector3 position1 = object1.transform.position;
        Vector3 position2 = object2.transform.position;
        Vector3 vectorBetween = position1 - position2;
        float distance = vectorBetween.magnitude;

        //Debug.Log("condition:  " + this);
        //Debug.Log("distance:  " + distance);
        //Debug.Log("desiredDistance:  " + desiredDistance);
        //Debug.DrawLine(position1, position2, Color.blue, 0.1f);

        if (distance > desiredDistance) { return false; }

        return true;
    }
}

public class enacted : condition
{
    //for when we want the objects to be CLOSER than the desired distance

    int howManyTimesToEnact = 1;
    planEXE2 theEnactionEXE;

    public enacted(planEXE2 theEnactionEXE, int inputHowManyTimesToEnact = 1)
    {
        this.howManyTimesToEnact = inputHowManyTimesToEnact;
        this.theEnactionEXE = theEnactionEXE;
    }

    public bool met()
    {

        //Debug.Log("times:  " + times);
        //Debug.Log("theEnactionEXE.areSTARTconditionsFulfilled()???:  ");
        //if (theEnactionEXE.startConditionsMet())
        {
            //Debug.Log("yes");
            //timesLeft--;
        }

        //Debug.Log("times:  " + times);
        if (theEnactionEXE.numberOfTimesExecuted >= howManyTimesToEnact) { return true; }

        return false;
    }
}

public class cooldown : condition
{
    //for when we want the objects to be CLOSER than the desired distance

    public int cooldownMax = 130;
    public int cooldownTimer = 0;

    public cooldown(int cooldownMax = 130)
    {
        this.cooldownMax = cooldownMax;
    }

    public bool met()
    {

        //Debug.Log("cooldownMax:  " + cooldownMax + "  cooldownTimer:  "+ cooldownTimer);
        if (cooldownTimer < 1) {
            //Debug.Log("cooldownTimer < 1   TRUEEEE");
            return true; }

        //cooldownTimer--;

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

public class planListComplete : condition
{
    List<planEXE2> planList;

    public planListComplete(List<planEXE2> planList)
    {
        this.planList = planList;
    }

    public bool met()
    {

        //Debug.Log("planList.Count:  " + planList.Count);
        foreach (planEXE2 planEXE in planList)
        {
            if (planEXE.endConditionsMet() == false) { return false; }
        }

        //Debug.Log("planList complete!");
        return true;
    }
}