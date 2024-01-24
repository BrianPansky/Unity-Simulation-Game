using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repository2 : MonoBehaviour
{
    //attach this to world object, allow any other game objects to access it through there

    //OBJECTS:
    public GameObject npc2Prefab;
    public GameObject placeHolderCubePrefab;
    public GameObject invisibleCubePrefab;
    public GameObject interactionSphere;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;

    //SCRIPTS:
    public patternScript2 patterns;
    public selfDestructScript1 theSelfDestructScript;
    public interactive1 theInteractive1Script;
    public legibstration theLegibstrata;
    public planningAndImagination thePlanner;
    public interactionEffects1 theInteractionEffects1;

    //public premadeStuffForAI stateGrabber;
    //public AI1 theHub;

    void Awake()
    {


        //MAYBE JUNK:
        //stateGrabber = GetComponent<premadeStuffForAI>();
        //theHub = GetComponent<AI1>();
        // Start is called before the first frame update

    }

    // Update is called once per frame
    void Update()
    {

    }




    public void createPrefabAtPoint(GameObject thePrefab, Vector3 thePoint)
    {
        GameObject toPrint = duplifier(thePrefab, thePoint);


        if (toPrint.GetComponent<interactionEffects1>().interactionsAvailable == null)
        {
            Debug.Log("it's null");
        }
        //Debug.Log(toPrint.GetComponent<interactionEffects1>().interactionsAvailable);
        //Debug.Log(toPrint.GetComponent<interactionEffects1>().interactionsAvailable.Count);

        //List<testInteraction> availableIntertactions = toPrint.GetComponent<interactionEffects1>().interactionsAvailable;
        //foreach (testInteraction thisTestInteraction in availableIntertactions)
        {
            //Debug.Log(thisTestInteraction.name);
        }

    }

    public GameObject createAndReturnPrefabAtPoint(GameObject thePrefab, Vector3 thePoint)
    {
        return duplifier(thePrefab, thePoint);
    }

    public GameObject createPrefabAtPointAndRETURN(GameObject thePrefab, Vector3 thePoint)
    {
        //GameObject newBuilding = new GameObject();
        //newBuilding = Instantiate(thePrefab, thePoint, Quaternion.identity);
        return Instantiate(thePrefab, thePoint, Quaternion.identity);
    }


    public GameObject createAndReturnPrefabAtPointWITHNAME(GameObject thePrefab, Vector3 thePoint, string theName)
    {
        //GameObject theNewObject = Instantiate(thePrefab, thePoint, Quaternion.identity);
        GameObject theNewObject = duplifier(thePrefab,thePoint);
        theNewObject.name = theName;

        //Debug.Log("BEFORE creating object");
        //Debug.Log(theNewObject.name);
        return theNewObject;
    }


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
        GameObject theNewObject = Instantiate(objectToDuplify, thePoint, Quaternion.identity);

        interactionEffects1 thisInteractionEffects1 = theNewObject.GetComponent<interactionEffects1>();
        if(thisInteractionEffects1 != null)
        {
            //now clonify it.
            //call from old one, input new one.
            objectToDuplify.GetComponent<interactionEffects1>().clonify(thisInteractionEffects1);
        }


        return theNewObject;
    }



    ////////////////////    CONDITIONS  ///////////////////////

    //proximity, time....what else.....various states?

    public bool isXCloserThanYToZ(GameObject objectX, GameObject objectY, GameObject objectZ)
    {
        Vector3 theVectorBetweenXandZ = objectX.transform.position - objectZ.transform.position;
        float distanceToX = theVectorBetweenXandZ.sqrMagnitude;

        Vector3 theVectorBetweenYandZ = objectY.transform.position - objectZ.transform.position;
        float distanceToY = theVectorBetweenYandZ.sqrMagnitude;

        Debug.Log("distances");
        Debug.Log(distanceToX);
        Debug.Log(distanceToY);

        if (distanceToX > distanceToY)
        {
            return false;
        }
        else
        {
            return true;
        }

    }




}
