using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class initialGenerator2 : MonoBehaviour
{

    public int newEffingInt = 22;


    public GameObject startPoint;
    public repository2 theRespository;

    public GameObject testTarget;


    public GameObject fuckThis;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //generateScene2();
        generateScene3();

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }


    void generateScene1()
    {
        generateTestAgent1();

        //generateTestTower1();

        generateTestKey1();

        generateTestLOCK1();
    }

    void generateScene2()
    {
        //generateVeryFewZonesAndAgents();
        //generateOneZoneOneNPC();
        //generateOneNPCPerZone();
        //generateTestAgent1();
        //generateFewZonesButMoreAgentsPerZone();
        generateManyZonesANDAgentsPerZone();
        //generateManyZonesAndAgentsPerZone();


    }

    void generateScene3()
    {

        //generateWIDEZonesAndAgentsEtc();
        //generateWIDEZonesAndAgentsEtcMANYAgentsPerZone();


        generateWIDEZonesAndAgentsEtcMANYAgentsPerZoneWithGuns();
        //generateJustOneNPC();
    }


    void generateJustOneNPC()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());

        objectList.Add(returnTestAgent2());
        
        //objectList.Add(theZoneObject); 
        //objectList.Add(returnTestKey1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int thisInitialXValue = -44;
        int thisXValueSpaxing = 10;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, 1, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }
        
        GameObject theZoneObject = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2");
        theZoneObject.transform.localScale = new Vector3(400f, 100f, 145f);
    }


    void generateWIDEZonesAndAgentsEtc()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());

        objectList.Add(returnTestKey1());

        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestKey1());
        objectList.Add(returnTestKey1());



        //objectList.Add(returnTestKey1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int nonZoneOffset = -15;  //so that objects in zones aren't on the edge of the zone, they are inside it
        int thisInitialXValue = -44;
        int thisXValueSpaxing = 10;
        int theZSpacing = 25;
        int howMany = 10;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, howMany * 2, theZSpacing, thisInitialXValue + thisXValueSpaxing * loopNumber, nonZoneOffset);
            loopNumber++;
        }


        List<GameObject> zoneList = new List<GameObject>();
        GameObject theZoneObject = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2");
        theZoneObject.transform.localScale = new Vector3(400f, 10f, 45f);
        zoneList.Add(theZoneObject);

        loopNumber = 0;
        foreach (GameObject thisObject in zoneList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, howMany, theZSpacing * 2, thisInitialXValue + thisXValueSpaxing * 2 * loopNumber);
            loopNumber++;
        }



    }

    void generateWIDEZonesAndAgentsEtcMANYAgentsPerZoneWithGuns()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());



        objectList.Add(returnFlamethrowerNPC());

        objectList.Add(returnPineTree1());
        objectList.Add(returnFlamethrowerNPC());


        //objectList.Add(returnTestKey1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int nonZoneOffset = -15;  //so that objects in zones aren't on the edge of the zone, they are inside it
        int thisInitialXValue = -64;
        int thisXValueSpaxing = 10;
        int theZSpacing = 25;
        int howManyZones = 11;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, howManyZones * 2, theZSpacing, thisInitialXValue + thisXValueSpaxing * loopNumber, nonZoneOffset);
            loopNumber++;
        }


        List<GameObject> zoneList = new List<GameObject>();
        GameObject theZoneObject = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2");
        theZoneObject.transform.localScale = new Vector3(800f, 10f, 50f);
        zoneList.Add(theZoneObject);

        loopNumber = 0;
        foreach (GameObject thisObject in zoneList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, howManyZones, theZSpacing * 2, thisXValueSpaxing * 2 * loopNumber);
            loopNumber++;
        }



    }


    void generateWIDEZonesAndAgentsEtcMANYAgentsPerZone()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());



        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestKey1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());

        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestKey1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());




        //objectList.Add(returnTestKey1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int nonZoneOffset = -15;  //so that objects in zones aren't on the edge of the zone, they are inside it
        int thisInitialXValue = -64;
        int thisXValueSpaxing = 10;
        int theZSpacing = 25;
        int howManyZones = 22;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, howManyZones * 2, theZSpacing, thisInitialXValue + thisXValueSpaxing * loopNumber, nonZoneOffset);
            loopNumber++;
        }


        List<GameObject> zoneList = new List<GameObject>();
        GameObject theZoneObject = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2");
        theZoneObject.transform.localScale = new Vector3(400f, 10f, 45f);
        zoneList.Add(theZoneObject);

        loopNumber = 0;
        foreach (GameObject thisObject in zoneList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, howManyZones, theZSpacing * 2,  thisXValueSpaxing * 2 * loopNumber);
            loopNumber++;
        }



    }




    void wellThisHasEachZoneRightBEsideEAchOtherWithZeroSpaceInBetweenNeat()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());

        objectList.Add(returnTestKey1());

        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestKey1());
        objectList.Add(returnTestKey1());

        //objectList.Add(returnTestKey1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int thisInitialXValue = -44;
        int thisXValueSpaxing = 10;
        int theZSpacing = 25;
        int howMany = 5;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, howMany*2, theZSpacing, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }


        List<GameObject> zoneList = new List<GameObject>();
        GameObject theZoneObject = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2");
        theZoneObject.transform.localScale = new Vector3(200f, 1f, 50f);
        zoneList.Add(theZoneObject);

        loopNumber = 0;
        foreach (GameObject thisObject in zoneList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, howMany, theZSpacing*2, thisInitialXValue + thisXValueSpaxing*2 * loopNumber);
            loopNumber++;
        }



    }







    void generateOneNPCPerZone()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());

        objectList.Add(returnTestKey1());

        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestKey1());
        objectList.Add(theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
        objectList.Add(returnTestKey1());

        //objectList.Add(returnTestKey1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int thisInitialXValue = -44;
        int thisXValueSpaxing = 10;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, 111, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }
    }

    void generateFewZonesButMoreAgentsPerZone()
    {
        //generateTestAgent1();
        //generateTestAgent1();
        //generateTestAgent1();



        //generateTestTower1();

        //generateTestKey1();

        //generateTestLOCK1();

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());

        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int thisInitialXValue = -114;
        int thisXValueSpaxing = 10;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, 111, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }
    }

    void generateManyZonesANDAgentsPerZone()
    {
        //generateTestAgent1();
        //generateTestAgent1();
        //generateTestAgent1();



        //generateTestTower1();

        //generateTestKey1();

        //generateTestLOCK1();

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestKey1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestKey1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());

        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int thisInitialXValue = -114;
        int thisXValueSpaxing = 10;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, 201, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }
    }


    void generateManyZonesAndAgentsPerZone()
    {
        //generateTestAgent1();
        //generateTestAgent1();
        //generateTestAgent1();



        //generateTestTower1();

        //generateTestKey1();

        //generateTestLOCK1();

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());

        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int thisInitialXValue = -44;
        int thisXValueSpaxing = 10;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, 202, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }
    }

    void generateVeryFewZonesAndAgents()
    {
        //generateTestAgent1();
        //generateTestAgent1();
        //generateTestAgent1();



        //generateTestTower1();

        //generateTestKey1();

        //generateTestLOCK1();

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");


        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(returnTestAgent1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int thisInitialXValue = -14;
        int thisXValueSpaxing = 10;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, 33, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }
    }

    void generateOneZoneOneNPC()
    {
        //generateTestAgent1();
        //generateTestAgent1();
        //generateTestAgent1();



        //generateTestTower1();

        //generateTestKey1();

        //generateTestLOCK1();

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");


        //objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
        objectList.Add(returnTestKey1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int thisInitialXValue = -14;
        int thisXValueSpaxing = 10;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, 3, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }
    }


    void generateTestAgent1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "generateTestAgent1");

        //GameObject myTest2 = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.invisiblePoint, startPoint.transform.position + new Vector3(0, 0, 0.8f), "generateTestAgent1 pointer");

        //myTest2.transform.SetParent(myTest.transform, true);

        //myTest.AddComponent<selfDestructScript1>();
        myTest.transform.position += new Vector3(7, 0, 11);

        myTest.AddComponent<NavMeshAgent>();
        myTest.AddComponent<AIHub2>();
        //myTest.GetComponent<AIHub2>().pointerPointToPutOnBody = myTest2;


        //myTest.GetComponent<AIHub2>().testTarget = testTarget;
        //myTest.AddComponent<interactionEffects1>();
        //myTest.GetComponent<interactive1>().inOutBoolSignal = true;  //didn't work, probably have to wait at least one frame, for it to initialize...?  oof....


        //thisGameObject.transform.parent = yourParentObject.transform;

    }


    GameObject returnTestAgent2()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestAgent1");

        myTest.AddComponent<NavMeshAgent>();
        myTest.AddComponent<AIHub2>();

        //AIcontroller

        enactionScript theEnactionScript = myTest.GetComponent<enactionScript>();
        theEnactionScript.availableEnactions.Add("shoot1");
        
        return myTest;
    }

    GameObject returnFlamethrowerNPC()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestAgent1");

        myTest.AddComponent<NavMeshAgent>();
        myTest.AddComponent<AIHub2>();

        //AIcontroller

        enactionScript theEnactionScript = myTest.GetComponent<enactionScript>();
        theEnactionScript.availableEnactions.Add("shootFlamethrower1");
        //shootFlamethrower1
        return myTest;
    }

    GameObject returnPineTree1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.pineTree1, new Vector3(0, 0, 0), "returnPineTree1");


        myTest.AddComponent<interactionScript>();
        interactionScript theInteractionScript = myTest.GetComponent<interactionScript>();

        theInteractionScript.addInteraction("shootFlamethrower1", "burn");

        return myTest;
    }

    GameObject returnTestAgent1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, new Vector3(0,0,0), "returnTestAgent1");
        
        myTest.AddComponent<NavMeshAgent>();
        myTest.AddComponent<AIHub2>();


        //enactionScript theEnactionScript = myTest.GetComponent<enactionScript>();
        //theEnactionScript.availableEnactions.Add("shoot1");

        return myTest;
    }

    GameObject returnTestLocation2()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestLocation2");

        //myTest.AddComponent<NavMeshAgent>();
        //myTest.AddComponent<AIHub2>();

        return myTest;
    }



    //          GENERATE SPECIFIC OBJECTS
    

    GameObject generateTowerHoop(GameObject theTower)
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "generateTowerHoop");
        //Debug.Log("///////////////////////////////////////////////////////////////after creating object");
        //Debug.Log(myTest.name);
        myTest.transform.localScale = new Vector3(1.7f, 1.3f, 1.7f);

        //myTest.AddComponent<interactive1>();
        

























        return myTest;
    }

    void generateTestKey1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "generateTestKey1");
        interactionScript theInteractionScript = myTest.AddComponent<interactionScript>();
        myTest.transform.localScale = new Vector3(0.5f, 2, 0.5f);
        myTest.transform.position += new Vector3(0, 3, 3);

        //theInteractionScript.listOfInteractions.Add("grabKey");
        theInteractionScript.addInteraction("standardClick", "grabKey");


    }
    void generateTestLOCK1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "generateTestLOCK1");
        interactionScript theInteractionScript = myTest.AddComponent<interactionScript>();
        myTest.transform.localScale = new Vector3(1f, 2, 1f);
        myTest.transform.position += new Vector3(0, 0, -8);

        //theInteractionScript.listOfInteractions.Add("clickLock");
        theInteractionScript.addInteraction("standardClick", "clickLock");




    }

    GameObject returnTestKey1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestKey1");
        interactionScript theInteractionScript = myTest.AddComponent<interactionScript>();
        myTest.transform.localScale = new Vector3(0.5f, 2, 0.5f);
        //myTest.transform.position += new Vector3(0, 3, 3);

        //theInteractionScript.listOfInteractions.Add("grabKey");
        theInteractionScript.addInteraction("standardClick", "grabKey");

        return myTest;

    }
    GameObject returnTestLOCK1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestLOCK1");
        interactionScript theInteractionScript = myTest.AddComponent<interactionScript>();
        myTest.transform.localScale = new Vector3(1f, 3, 1f);
        //myTest.transform.localScale = new Vector3(1f, 2, 1f);
        //myTest.transform.position += new Vector3(0, 0, -8);

        //theInteractionScript.listOfInteractions.Add("clickLock");
        theInteractionScript.addInteraction("standardClick", "clickLock");



        return myTest;

    }


    void generateInteractionSlot1(Vector3 theLocation)
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, theLocation, "generateInteractionSlot1");
        myTest.transform.localScale = new Vector3(0.7f, 0.3f, 0.7f);




    }

    void generateRing(Vector3 theLocation)
    {
        //"ring", for now cube, whatever
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, theLocation, "generateRing");
        myTest.transform.localScale = new Vector3(1.7f, 0.5f, 1.7f);



    }




    public List<string> stringLister(params string[] listofStrings)
    {
        List<string> aNewList = new List<string>();

        foreach (string x in listofStrings)
        {
            aNewList.Add(x);
        }

        return aNewList;
    }



}

public class generatorJob
{
    Vector3 originPoint;


    
}
