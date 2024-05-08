using System;
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

    // Start is called before the first frame update
    void Start()
    {
        //generateScene2();
        generateScene3();
        //generateFlex();

        //Time.timeScale = 0f;
    }

    
    // Update is called once per frame
    void Update()
    {


        //Debug.Log("--------------------------------------------------------------------------");

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


        returnSimpleTank1();

        generateWIDEZonesAndAgentsEtcMANYAgentsPerZoneWithGuns();
        //generateJustOneNPC();
    }



    void generateFlex()
    {

        int nonZoneOffset = -15;  //so that objects in zones aren't on the edge of the zone, they are inside it
        int thisInitialXValue = -68;
        int thisXValueSpaxing = 7;
        int theZSpacing = 25;
        int howManyZones = 1;
        int loopNumber = 0;




        //anchors(startX, startY, startZ, Xincrement, Yincrement, Zincrement, loopNumberOfZones);
        anchors();
        motors();




        List<GameObject> slabStack = new List<GameObject>();

        slabStack.Add(slab1());

        GameObject theSlab = slab1();
        theSlab.transform.position = new Vector3(-67, 22, 0);




        loopNumber = 0;

        int currentLocation = 0;

        int currentSlab = 0;
        int howManySlabs = 15;

        float vertSpacing = 0f;
        float vertStart = 15f;

        float slightHorizontalOffset = 1.5f;//0.15f

        bool reverse = false;

        while (currentLocation < howManyZones)
        {
            currentLocation++;

            while (currentSlab < howManySlabs)
            {
                currentSlab++;
                //Debug.Log(thisObject);
                //theRespository.VERTICALplaceOnLineAndDuplicate(thisObject, howManySlabs, thisInitialXValue + thisXValueSpaxing * loopNumber, theZSpacing * currentLocation, nonZoneOffset, 18.4f);

                float theX = thisInitialXValue + 4.3f + currentSlab * slightHorizontalOffset;
                float theY = vertStart + (vertSpacing * currentSlab);
                float theZ = nonZoneOffset + (theZSpacing * (currentLocation - 1));

                //theRespository.createPrefabAtPoint(slabStack[0], new Vector3(theX, theY, theZ));


                float halfSize = 1.2f;

                GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, new Vector3(0, 0, 0), "slab1");
                //myTest.transform.localScale = new Vector3(3f, 0.4f, 3f);
                myTest.transform.localScale = new Vector3(0.2f, halfSize * 2, 3f);


                Rigidbody theRigid = myTest.AddComponent<Rigidbody>();
                theRigid.mass = 111;
                theRigid.angularDrag = 2f;

                newton1 theNewton = myTest.AddComponent<newton1>();
                theNewton.thisMass = 111;

                myTest.AddComponent<standardGravity>();






                GameObject aGlue1 = glueGlob(new Vector3(0f, halfSize, halfSize));
                aGlue1.transform.SetParent(myTest.transform);



                GameObject aGlue2 = glueGlob(new Vector3(0f, -halfSize, halfSize));
                aGlue2.transform.SetParent(myTest.transform);



                GameObject aGlue3 = glueGlob(new Vector3(0f, halfSize, -halfSize));
                aGlue3.transform.SetParent(myTest.transform);



                GameObject aGlue4 = glueGlob(new Vector3(0f, -halfSize, -halfSize));
                aGlue4.transform.SetParent(myTest.transform);







                if (reverse == true)
                {
                    myTest.transform.rotation = Quaternion.Euler(0, 180, 0);
                    myTest.transform.rotation = Quaternion.Euler(0, 0, -35);
                    reverse = false;
                }
                else
                {
                    myTest.transform.rotation = Quaternion.Euler(0, 0, 35);
                    reverse = true;
                }

                myTest.transform.position = new Vector3(theX, theY, theZ);


                loopNumber++;
            }

            currentSlab = 0;
        }



    }

    public void anchors()
    {

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());


        objectList.Add(anchor1());



        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int nonZoneOffset = -15;  //so that objects in zones aren't on the edge of the zone, they are inside it
        int thisInitialXValue = -66;
        int thisXValueSpaxing = 8;
        int theZSpacing = 25;
        int howManyZones = 3;
        int loopNumber = 0;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, howManyZones, theZSpacing, thisInitialXValue + thisXValueSpaxing * loopNumber, nonZoneOffset, 18.4f);
            loopNumber++;
        }

    }

    public void motors()
    {

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());


        objectList.Add(motor1());



        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);
        //Debug.Log("=====================    begin generation    =====================");
        int nonZoneOffset = -15;  //so that objects in zones aren't on the edge of the zone, they are inside it
        int thisInitialXValue = -68;
        int thisXValueSpaxing = 28;
        int theZSpacing = 25;
        int howManyZones = 1;

        float vertHeight = 14f;
        foreach (GameObject thisObject in objectList)
        {
            //Debug.Log(thisObject);
            theRespository.placeOnLineAndDuplicate(thisObject, howManyZones, theZSpacing, thisInitialXValue + thisXValueSpaxing, nonZoneOffset, vertHeight);

        }

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
        //objectList.Add(returnSimpleTank1());


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











    //     [[[[[[[[[[[[    generating specific objects    ]]]]]]]]]]]]]



    GameObject glueGlob(Vector3 location)
    {


        GameObject prefabToUse = theRespository.interactionSphere;


        GameObject newGlue = theRespository.createAndReturnPrefabAtPointWITHNAME(prefabToUse, new Vector3(0, 0, 0), "glue");
        selfDestructScript1 killScript = newGlue.GetComponent<selfDestructScript1>();
        UnityEngine.Object.Destroy(killScript);

        glueScript theGlueScript = newGlue.AddComponent<glueScript>();
        theGlueScript.autoGlue = true;

        newGlue.transform.position = location;


        Renderer objectsRenderer = newGlue.GetComponent<Renderer>();
        if (objectsRenderer != null)
        {
            objectsRenderer.material.color = new Color(0f, 0f, 1f);
        }

        return newGlue;
    }



    GameObject slab1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, new Vector3(0, 0, 0), "slab1");
        //myTest.transform.localScale = new Vector3(3f, 0.4f, 3f);
        //          myTest.transform.localScale = new Vector3(0.3f, 1f, 3f);


        //          Rigidbody theRigid = myTest.AddComponent<Rigidbody>();
        //          theRigid.mass = 44;
        //          theRigid.angularDrag = 0.01f;

        //          newton1 theNewton = myTest.AddComponent<newton1>();
        //          theNewton.thisMass = 44;

        //          myTest.AddComponent<standardGravity>();






        //          GameObject aGlue1 = glueGlob(new Vector3(0.25f, 1.5f, 0f));
        //          aGlue1.transform.SetParent(myTest.transform);



        GameObject aGlue2 = glueGlob(new Vector3(-0.25f, -1.5f, 0f));
        aGlue2.transform.SetParent(myTest.transform);



        GameObject aGlue3 = glueGlob(new Vector3(0.25f, 1.5f, 0f));
        aGlue3.transform.SetParent(myTest.transform);



        GameObject aGlue4 = glueGlob(new Vector3(-0.25f, -1.5f, 0f));
        aGlue4.transform.SetParent(myTest.transform);







        return myTest;
    }



    GameObject motor1()
    {

        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCylinderPrefab, new Vector3(0, 0, 0), "motor");
        myTest.transform.localScale = new Vector3(1f, 3f, 1f);
        myTest.transform.rotation = Quaternion.Euler(90, 0, 0);



        Rigidbody theRigid = myTest.AddComponent<Rigidbody>();
        theRigid.mass = 555;


        newton1 theNewton = myTest.AddComponent<newton1>();
        theNewton.thisMass = 555;

        myTest.AddComponent<standardGravity>();

        moveByForce theMove = myTest.AddComponent<moveByForce>();
        theMove.turnedOn = true;
        //theMove.enabled = false;

        GameObject aGlue2 = glueGlob(new Vector3(-0.25f, -0.5f, 0f));
        aGlue2.transform.SetParent(myTest.transform);





        return myTest;
    }


    GameObject anchor1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, new Vector3(0, 0, 0), "anchor");
        myTest.transform.localScale = new Vector3(5f, 11f, 5f);
        //myTest.transform.position += new Vector3(0, 3, 3);



        Rigidbody theRigid = myTest.AddComponent<Rigidbody>();
        theRigid.isKinematic = true;
        theRigid.mass = 888;


        newton1 theNewton = myTest.AddComponent<newton1>();
        theNewton.thisMass = 888f;
        theNewton.allowMovement = false;



        GameObject aGlue2 = glueGlob(new Vector3(2.5f, -3f, 0f));
        aGlue2.transform.SetParent(myTest.transform);





        return myTest;
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
        //                  theEnactionScript.availableEnactions.Add("shoot1");

        body1 theBodyScript = myTest.GetComponent<body1>();
        theBodyScript.theBodyGameObject = myTest;  //ahh, might need to.....fix that in....duplifier....

        return myTest;
    }

    GameObject returnFlamethrowerNPC()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestAgent1");

        myTest.AddComponent<NavMeshAgent>();
        myTest.AddComponent<AIHub2>();

        //AIcontroller

        enactionScript theEnactionScript = myTest.GetComponent<enactionScript>();
        //              theEnactionScript.availableEnactions.Add("shootFlamethrower1");
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

    public GameObject returnSimpleTank1()
    {
        GameObject bottomBit = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.simpleTankBottom, new Vector3(0, 0, 0), "returnSimpleTank1");

        //bottomBit.AddComponent<NavMeshAgent>();
        //bottomBit.AddComponent<AIHub2>();


        bottomBit.AddComponent<interactionScript>();
        interactionScript theInteractionScript = bottomBit.GetComponent<interactionScript>();

        theInteractionScript.addInteraction("standardClick", "useVehicle");



        //bottomBit.AddComponent<tank1>();
        tank1 theTank1Script = bottomBit.GetComponent<tank1>();
        theTank1Script.tankHead = returnSimpleTankTurretAndBarrel();
        theTank1Script.tankHead.transform.SetParent(bottomBit.transform, true);
        //initializeAim(theTank1Script.tankHead.transform.GetChild(0).gameObject, theTank1Script.firingStartPoint, theTank1Script.firingDirectionPoint);


        return bottomBit;
    }


    public void initializeAim(GameObject aimingObject, GameObject startPoint, GameObject endpoint)
    {
        //child of "aimingObject" = "startPoint"
        //child of "startPoint" = "endpoint"

        startPoint = aimingObject.transform.GetChild(0).gameObject;
        endpoint = startPoint.transform.GetChild(0).gameObject;
    }

    GameObject returnSimpleTankTurretAndBarrel()
    {
        GameObject bottomBit = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.simpleTankTurretWITHBarrel, new Vector3(0, 0, 0), "returnSimpleTank1");

        return bottomBit;
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
