using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repository2 : MonoBehaviour
{
    //attach this to world object, allow any other game objects to access it through there
    //or......make it a singletomng:
    public static repository2 singleton;

    //OBJECTS:
    public GameObject player;
    public GameObject npc2Prefab;
    public GameObject placeHolderCubePrefab;
    public GameObject placeHolderCylinderPrefab;
    public GameObject invisibleCubePrefab;
    public GameObject interactionSphere;
    public GameObject invisiblePoint;
    public GameObject mapZone2;
    public GameObject pineTree1;
    public GameObject burntPineTree1;
    public GameObject simpleTankBottom;
    public GameObject simpleTankTurretWITHOUTBarrel;
    public GameObject simpleTankBarrel;
    public GameObject simpleGun1;
    public GameObject shotgun1;
    public GameObject arrowUp;
    public GameObject arrowForward;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;


    public Material explosion1;
    public Material smoke1;

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












    //utility....put here???

    public Vector3 randomNearbyVector(Vector3 positionToBeNear)
    {
        Vector3 vectorToReturn = positionToBeNear;
        float initialDistance = 0f;
        float randomAdditionalDistance = UnityEngine.Random.Range(-20, 20);
        vectorToReturn += new Vector3(initialDistance + randomAdditionalDistance, 0, 0);
        randomAdditionalDistance = UnityEngine.Random.Range(-20, 20);
        vectorToReturn += new Vector3(0, 0, initialDistance + randomAdditionalDistance);

        return vectorToReturn;
    }

    public GameObject pickRandomObjectFromList(List<GameObject> theList)
    {
        //Debug.Log("theList.Count:  "+theList.Count);

        if (theList.Count == 0)
        {
            //Debug.Log("there are zero objects on the list of objects entered into ''pickRandomObjectFromListEXCEPT''");
            return null;
        }


        int numberOfTries = 10; //easy ad hoc way to terminate a potentially infinate loop for now lol
        GameObject thisObject;
        thisObject = null;


        while (numberOfTries > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, theList.Count);
            thisObject = theList[randomIndex];

            if (thisObject != null)
            {
                //Debug.Log(thisObject + " #:  "+ thisObject.GetHashCode());
                //Debug.Log("thisObject.transform.position:  " + thisObject.transform.position);

                return thisObject;
            }

            numberOfTries--;
        }




        return thisObject;

    }

    public GameObject pickRandomObjectFromListEXCEPT(List<GameObject> theList, GameObject notTHISObject)
    {
        if (theList.Count == 0)
        {
            Debug.Log("there are zero objects on the list of objects entered into ''pickRandomObjectFromListEXCEPT''");
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












    //everything else on this script [below] is WRONG and needs to be deleted, or at least moved




    public void placeOnLineAndDuplicate(GameObject theGameObject, int howMany, int theSpacing, int xValue, int startZlocation = 0, float startYlocation = 0f)
    {
        bool needADuplicate = false; //the first input object can simply be PLACED, only subsequent ones need to be duplicates

        foreach (Vector3 thisPosition in patternScript2.singleton.makeLinePattern1(howMany, theSpacing))
        {
            Vector3 fullPosition = thisPosition + new Vector3(xValue, startYlocation, startZlocation);

            if (needADuplicate == false)
            {
                theGameObject.transform.position = fullPosition;
                needADuplicate = true;
            }
            else
            {
                //Debug.Log("fullPosition x:  "+ fullPosition.x);
                //Debug.Log("fullPosition y:  " + fullPosition.z);
                genGen.singleton.createPrefabAtPoint(theGameObject, fullPosition);
            }

        }

    }

}
