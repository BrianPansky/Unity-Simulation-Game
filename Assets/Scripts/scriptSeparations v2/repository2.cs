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
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;


    public Material explosion1;
    public Material smoke1;

    void Awake()
    {
        singletonify();

        //MAYBE JUNK:
        //stateGrabber = GetComponent<premadeStuffForAI>();
        //theHub = GetComponent<AIHub2>();
        // Start is called before the first frame update

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










    //everything else on this script [below] is WRONG and needs to be deleted, or at least moved

    /*
    public void placeObjectsOnLinePattern(List<GameObject> theObjects, List<Vector3> theLinePattern)
    {
        int indexPosition = 0;

        foreach (GameObject thisObject in theObjects)
        {
            thisObject.transform.position = theLinePattern[indexPosition];
            indexPosition += 1;
        }
    }

    public void placeDuplicatesOnLine1(GameObject theGameObject, List<Vector3> vector3s)
    {
        //just to minimize all that:
        if (true == false)
        {


            //Debug.Log("rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr      start       rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");




            //Debug.Log("test integer = " + theGameObject.GetComponent<interactionEffects1>().testThisFuckingInt);

            //Debug.Log("test integer list " + theGameObject.GetComponent<interactionEffects1>().testThisFuckingIntList);

            //foreach (int x in theGameObject.GetComponent<interactionEffects1>().testThisFuckingIntList)
            {
                //Debug.Log(x);
            }

            //Debug.Log("test class object " + theGameObject.GetComponent<interactionEffects1>().fuckingTestClassObject);
            //Debug.Log("test class object integer = " + theGameObject.GetComponent<interactionEffects1>().fuckingTestClassObject.fuckingInt);


            //Debug.Log("test class object list " + theGameObject.GetComponent<interactionEffects1>().fuckingTestClassObjectList);
            //Debug.Log("COUNT of test class object list = " + theGameObject.GetComponent<interactionEffects1>().fuckingTestClassObjectList.Count);

            //foreach (fuckingTestClass x in theGameObject.GetComponent<interactionEffects1>().fuckingTestClassObjectList)
            {
                //Debug.Log(x);
                //Debug.Log(x.fuckingInt);
            }

            //Debug.Log("test class object of ''testInteraction'' type: " + theGameObject.GetComponent<interactionEffects1>().fuckThis);


            //foreach (testInteraction x in theGameObject.GetComponent<interactionEffects1>().fuckThis)
            {
                //Debug.Log(x);
                //Debug.Log(x.name);
            }

            //Debug.Log(x.name);
            //Debug.Log("test class object of ''testInteraction'' type in interactionsAvailable: " + theGameObject.GetComponent<interactionEffects1>().interactionsAvailable);

            //foreach (testInteraction x in theGameObject.GetComponent<interactionEffects1>().interactionsAvailable)
            {
                //Debug.Log(x);
                //Debug.Log(x.name);
            }




























            //List<string> myKeyList = new List<string>(this.theWorldScript.interactionLegibstration.Keys);
            //foreach(string key in myKeyList)
            {
                //Debug.Log("HELLLOOOOOOOOOOOOOOOOOOOOO?????????????????????????????????????????????????????????");
                //Debug.Log(key);
            }

            //Debug.Log("test integer = " + theGameObject.GetComponent<interactionEffects1>().testThisFuckingInt);
            //Debug.Log("test integer list " + theGameObject.GetComponent<interactionEffects1>().testThisFuckingIntList);

            //foreach (int x in theGameObject.GetComponent<interactionEffects1>().testThisFuckingIntList)
            {
                //Debug.Log(x);
            }

            //Debug.Log("test class object " + theGameObject.GetComponent<interactionEffects1>().fuckingTestClassObject);
            //Debug.Log("test class object integer = " + theGameObject.GetComponent<interactionEffects1>().fuckingTestClassObject.fuckingInt);

            //Debug.Log("test class object " + theGameObject.GetComponent<interactionEffects1>().fuckingTestClassObjectList);


            //foreach(fuckingTestClass x in theGameObject.GetComponent<interactionEffects1>().fuckingTestClassObjectList)
            {
                //Debug.Log(x);
                //Debug.Log(x.fuckingInt);
            }

            //testInteraction FUCK = new testInteraction();
            //FUCK.name = "fuck you, that's why";
            //interactionScriptOnGeneratedObject.fuckThis.Add(FUCK);

            //Debug.Log(theGameObject.GetComponent<interactionEffects1>().fuckThis.Count);
            //Debug.Log("test class object of ''testInteraction'' type: " + theGameObject.GetComponent<interactionEffects1>().fuckThis);


            //foreach (testInteraction x in theGameObject.GetComponent<interactionEffects1>().fuckThis)
            {
                //Debug.Log(x);
                //Debug.Log(x.name);
            }


            //Debug.Log("test class object of ''testInteraction'' type in interactionsAvailable: " + theGameObject.GetComponent<interactionEffects1>().interactionsAvailable);


            //foreach (testInteraction x in theGameObject.GetComponent<interactionEffects1>().interactionsAvailable)
            {
                //Debug.Log(x);
                //Debug.Log(x.name);
            }

            //List<testInteraction> availableIntertactions = theGameObject.GetComponent<interactionEffects1>().interactionsAvailable;
            //Debug.Log(availableIntertactions.Count);
            //foreach (testInteraction thisTestInteraction in availableIntertactions)
            {
                //Debug.Log(thisTestInteraction.name);
            }
        }

        //Debug.Log("rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr    /  midpoint  /     rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");

        foreach (Vector3 thisPosition in vector3s)
        {
            createPrefabAtPoint(theGameObject, thisPosition);
        }
        //Debug.Log("rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr      END       rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
    }

    public void placeDuplicatesOnLine2(GameObject theGameObject, int howMany, int theSpacing, int xValue)
    {


        foreach (Vector3 thisPosition in patternScript2.singleton.makeLinePattern1(howMany, theSpacing))
        {
            Vector3 fullPosition = thisPosition + new Vector3(xValue, 0, 0);
            //Debug.Log("fullPosition x:  "+ fullPosition.x);
            //Debug.Log("fullPosition y:  " + fullPosition.z);
            createPrefabAtPoint(theGameObject, fullPosition);
        }

    }


    public void placeOnLineAndDuplicate(GameObject theGameObject, int howMany, int theSpacing, int xValue, int startZlocation = 0, float startYlocation = 0f)
    {
        bool needADuplicate = false; //the first input object can simply be PLACED, only subsequent ones need to be duplicates

        foreach (Vector3 thisPosition in makeLinePattern1(howMany, theSpacing))
        {
            Vector3 fullPosition = thisPosition + new Vector3(xValue, startYlocation, startZlocation);

            if(needADuplicate == false)
            {
                theGameObject.transform.position = fullPosition;
                needADuplicate = true;
            }
            else
            {
                //Debug.Log("fullPosition x:  "+ fullPosition.x);
                //Debug.Log("fullPosition y:  " + fullPosition.z);
                createPrefabAtPoint(theGameObject, fullPosition);
            }
            
        }

    }

    public void VERTICALplaceOnLineAndDuplicate(GameObject theGameObject, int howMany, int theSpacing, int xValue, int startZlocation = 0, float startYlocation = 0f)
    {
        bool needADuplicate = false; //the first input object can simply be PLACED, only subsequent ones need to be duplicates

        int currentOne = 0;

        foreach (Vector3 thisPosition in makeLinePattern1(howMany, theSpacing))
        {
            Vector3 fullPosition = new Vector3(xValue, startYlocation + currentOne, startZlocation);

            currentOne++;

            if (needADuplicate == false)
            {
                theGameObject.transform.position = fullPosition;
                needADuplicate = true;
            }
            else
            {
                //Debug.Log("fullPosition x:  "+ fullPosition.x);
                //Debug.Log("fullPosition y:  " + fullPosition.z);
                createPrefabAtPoint(theGameObject, fullPosition);
            }

        }

    }



    

    internal List<GameObject> makeListOfObjects(GameObject gameObject, int quantity)
    {
        List<GameObject> listToReturn = new List<GameObject>();

        while (quantity > 0)
        {
            GameObject thisObject = duplifier(gameObject, this.transform.position);
            quantity -= 1;
        }

        return listToReturn;
    }


    public GameObject duplifier(GameObject objectToDuplify, Vector3 thePoint)
    {
        //so, unity instantiate doesn't copy all variables [such as class objects and their variables]
        //so, gotta do that manually.
        //this function will:
        //      create a new instance
        //      update all of the problem variables in a deep way
        //to copy them in a deep way
        //      each script will have a "clonify" function/method to take the input script from new copy, and deep copy old one 

        //assume "Quaternion.identity" for now.
        //GameObject theNewObject = Instantiate(objectToDuplify, thePoint, Quaternion.identity);
        //NO!  now i'm using the input object rotation
        GameObject theNewObject = Instantiate(objectToDuplify, thePoint, objectToDuplify.transform.rotation);

        interactionScript thisinteractionScript = theNewObject.GetComponent<interactionScript>();
        if (thisinteractionScript != null)
        {
            //now clonify it.
            //call from old one, input new one.
            objectToDuplify.GetComponent<interactionScript>().clonify(thisinteractionScript);
        }

        mapZoneScript thisMapZoneScript = theNewObject.GetComponent<mapZoneScript>();
        if (thisMapZoneScript != null)
        {
            //now clonify it.
            //call from old one, input new one.

            //objectToDuplify.GetComponent<mapZoneScript>().clonify(thisMapZoneScript);
        }


        return theNewObject;
    }




    */
}
