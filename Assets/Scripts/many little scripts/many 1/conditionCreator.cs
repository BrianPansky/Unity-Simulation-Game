using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conditionCreator : MonoBehaviour
{

    public static conditionCreator singleton;


    //public enum interType


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



    // Start is called before the first frame update
    void Start()
    {
        
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
        //closest to what?  to THIS object, i suppose

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
        //closest to what?  to THIS object, i suppose

        GameObject theClosestSoFar = null;

        foreach (objectIdPair thisIdPair in listOfObjects)
        {
            if (theClosestSoFar != null)
            {
                float distanceToThisObject = Vector3.Distance(thisIdPair.theObject.transform.position, objectWeWantItClosestTo.transform.position);
                float distanceToTheClosestSoFar = Vector3.Distance(theClosestSoFar.transform.position, objectWeWantItClosestTo.transform.position);

                if (distanceToThisObject < distanceToTheClosestSoFar)
                {
                    theClosestSoFar = thisIdPair.theObject;
                }
            }
            else
            {
                theClosestSoFar = thisIdPair.theObject;
            }
        }

        return theClosestSoFar;
    }


    /*




    public GameObject findXNearestToY(GameObject objectWeWantItClosestTo, List<GameObject> theList)
    {
        //      EXCEPT for the input object itself!!!


        //one tag input for now
        //return nearest object with that tag
        //other funciton can be called "nearest XYZ" or something lol


        //stackoverflow.com/questions/63106256/find-and-return-nearest-gameobject-with-tag-unity
        //var sorted = NearGameobjects.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        //  List<GameObject> allPotentialTargets = listInObjectFormat(objectsWithTag[tagToLookFor]);
        List<objectIdPair> allPotentialTargets = objectsWithTag[tagToLookFor];
        //List<GameObject> sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        //var sortedListByDistance = allPotentialTargets.OrderBy(obj => (col.transform.position - transform.position).sqrMagnitude);
        return whichOBJECTOnObjectIdPairListIsNearestToInputtedObject(objectWeWantItClosestTo, allPotentialTargets);
    }




    public GameObject whichOBJECTOnObjectIdPairListIsNearestToInputtedObject(GameObject objectWeWantItClosestTo, List<objectIdPair> allPotentialTargets)
    {
        //      EXCEPT for the input object itself!!!

        //how to make it not return the inputted object?

        GameObject theClosestSoFar = null;
        //Debug.Log("===================================================");
        //Debug.Log("objectWeWantItClosestTo.GetInstanceID():  " + objectWeWantItClosestTo.GetInstanceID());

        foreach (objectIdPair thisObjectIdPair in allPotentialTargets)
        {
            //Debug.DrawLine(objectWeWantItClosestTo.transform.position, thisObjectIdPair.theObject.transform.position, Color.green, 12f);

            //Debug.Log(":::::::::::::::::::::::::::::::::::::::::::");

            //Debug.Log("thisObjectIdPair.theObjectIdNumber:  " + thisObjectIdPair.theObjectIdNumber);

            if (thisObjectIdPair.theObjectIdNumber == objectWeWantItClosestTo.GetInstanceID())
            {
                //Debug.Log("1111111111111111111111111111111111111111");
                continue;
            }

            if (theClosestSoFar == null)
            {
                //Debug.Log("22222222222222222222222222222222222222222222222");
                theClosestSoFar = thisObjectIdPair.theObject;
                continue;
            }

            float distanceToThisObject = Vector3.Distance(thisObjectIdPair.theObject.transform.position, objectWeWantItClosestTo.transform.position);
            float distanceToTheClosestSoFar = Vector3.Distance(theClosestSoFar.transform.position, objectWeWantItClosestTo.transform.position);

            //Debug.Log("distanceToThisObject:  " + distanceToThisObject);
            //Debug.Log("distanceToTheClosestSoFar:  " + distanceToTheClosestSoFar);
            if (distanceToThisObject > distanceToTheClosestSoFar)
            {

                //Debug.Log("distanceToThisObject > distanceToTheClosestSoFar!!!!!!!!!!");
                continue;
            }

            //Debug.Log("444444444444444444444444444444444444");
            theClosestSoFar = thisObjectIdPair.theObject;

        }


        //Debug.DrawLine(objectWeWantItClosestTo.transform.position, theClosestSoFar.transform.position, Color.red, 2f);

        return theClosestSoFar;
    }




    */






}
