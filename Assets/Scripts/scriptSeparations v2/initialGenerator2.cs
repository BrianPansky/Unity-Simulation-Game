using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class initialGenerator2 : MonoBehaviour
{

    private static initialGenerator2 singleton;

    public int newEffingInt = 22;


    public GameObject startPoint;

    public GameObject testTarget;


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

        GameObject theWorldObject = GameObject.Find("World");
        //theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;

        //generateScene2();
        //generateScene3();

        NEWgenerateScene3();
        
        //Time.timeScale = 0f;
    }





    void NEWgenerateScene3()
    {
        //      [reverse the order of the following?][or make "objects" with transform first, duplicate those, then for each one, add the other components?]
        //Which object I want to create 
        //      with all of its properties and its location/ rotation,
        //how many per Zone
        //how many zones
        //how big are zones [they are another kind of object, by definition one per zone] and
        //spacing [patternScript2!  dot singleton]


        int howManyZones = 31;
        int howManySetsPerZone = 2;
        int theZSpacing = 25;
        int theXSpacing = 5;
        float sideOffset = 0;

        //create a line of points, spaced by multiples of "howManysetsPerZone"
        makeEmptyZones(howManyZones, theZSpacing * howManySetsPerZone);

        //makeAndFillZones(objectList1(), 11);


        INVERSEmakeAndFillZones(
            patternScript2.singleton.makeLinePattern2(howManyZones * howManySetsPerZone, theZSpacing, sideOffset), 
            theXSpacing
            );
    }


    void INVERSEmakeAndFillZones(List<Vector3> setsPositions, int theXSpacing = 25)
    {
        float sideOffset = 0f;

        foreach (Vector3 point in  setsPositions)
        {
            //List<Vector3> setsPositions = patternScript2.singleton.makeLinePattern2(howManyZones * howManySetsPerZone, theZSpacing, sideOffset);
            //makePrefabsAtListOfPoints(setsPositions, thisPrefab);
            INVERSEobjectList1(point, theXSpacing);
            //sideOffset += theZSpacing / 2;
        }



    }




    void makeAndFillZones(List<GameObject> objectList, int howManyZones, int howManySetsPerZone = 2, int theZSpacing = 25)
    {
        // no, this is bad, fails to "deep copy".  invert it
        //create a line of points, spaced by multiples of "howManysetsPerZone"
        makeEmptyZones(howManyZones, theZSpacing * howManySetsPerZone);

        float sideOffset = 0f;

        foreach (GameObject thisPrefab in objectList)
        {
            List<Vector3> setsPositions = patternScript2.singleton.makeLinePattern2(howManyZones * howManySetsPerZone, theZSpacing, sideOffset);
            makePrefabsAtListOfPoints(setsPositions, thisPrefab);

            sideOffset += theZSpacing/2;
        }


        destroyThemAll(objectList);


    }

    void makeEmptyZones(int howManyZones, int theZSpacing)
    {

        List<Vector3> zonePositions = patternScript2.singleton.makeLinePattern1(howManyZones, theZSpacing);

        foreach (Vector3 thisPoint in zonePositions)
        {
            //Instantiate(prefab, thisPoint, default);
            GameObject newObj = Instantiate(repository2.singleton.mapZone2, thisPoint, Quaternion.identity);
            newObj.transform.localScale = new Vector3(400f, 10f, 1f * theZSpacing);

        }
    }

    void makePrefabsAtListOfPoints(List<Vector3> positions, GameObject prefab)
    {
        foreach (Vector3 thisPoint in positions)
        {
            //Instantiate(prefab, thisPoint, default);
            Instantiate(prefab, thisPoint, Quaternion.identity);
        }
    }

    void destroyThemAll(List<GameObject> objectList)
    {

        int countUp = 0;
        int amount = objectList.Count;
        while (countUp < amount)
        {
            Destroy(objectList[countUp]);

            countUp++;
        }
    }





    List<GameObject> objectList1()
    {

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());

        objectList.Add(genGen.singleton.returnSimpleTank2(this.transform.position));
        objectList.Add(genGen.singleton.returnPineTree1(this.transform.position));
        objectList.Add(genGen.singleton.returnPineTree1(this.transform.position));


        //objectList.Add(returnTestKey1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);

        return objectList;
    }

    List<GameObject> INVERSEobjectList1(Vector3 startPosition, int theXSpacing = 25)
    {

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());

        objectList.Add(genGen.singleton.returnSimpleTank2(startPosition+ new Vector3(-theXSpacing, 0, 0)));
        objectList.Add(genGen.singleton.returnNPC4(startPosition + new Vector3(theXSpacing, 0, 0)));
        objectList.Add(genGen.singleton.returnPineTree1(startPosition + new Vector3(theXSpacing+ theXSpacing, 0, 0)));
        //objectList.Add(genGen.singleton.returnPineTree1(startPosition + new Vector3(0, 0, theXSpacing + theXSpacing)));


        //objectList.Add(returnTestKey1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);

        int currentSlotPosition = 0;

        foreach(GameObject obj in objectList)
        {
            obj.transform.position += new Vector3(theXSpacing * currentSlotPosition, 0, 0);
            currentSlotPosition++;
        }

        return objectList;
    }





    /*public GameObject MODULARIZE returnSimpleTank1()
    {
        GameObject bottomBit = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.simpleTankBottom, new Vector3(0, 0, 0), "returnSimpleTank1");

        //bottomBit.AddComponent<NavMeshAgent>();
        //bottomBit.AddComponent<AIHub2>();


        //  bottomBit.AddComponent<interactionScript>();
        //  interactionScript theInteractionScript = bottomBit.GetComponent<interactionScript>();

        //  theInteractionScript.addInteraction("standardClick", "useVehicle");

        Rigidbody rigidbody = bottomBit.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        //  interactionScript theInteractions = bottomBit.AddComponent<interactionScript>();
        //  theInteractions.addInteraction("tankShot", "burn");

        //bottomBit.AddComponent<tank1>();
        tank1 theTank1Script = bottomBit.GetComponent<tank1>();
        theTank1Script.tankHead = returnSimpleTankTurretWithoutBarrel();
        theTank1Script.tankHead.transform.rotation = Quaternion.identity;
        theTank1Script.tankHead.transform.SetParent(bottomBit.transform, true);
        //initializeAim(theTank1Script.tankHead.transform.GetChild(0).gameObject, theTank1Script.firingStartPoint, theTank1Script.firingDirectionPoint);

        theTank1Script.tankBarrel = returnSimpleTankBarrel();
        theTank1Script.tankBarrel.transform.position += new Vector3(0, 2.1f, 1.2f);
        theTank1Script.tankBarrel.transform.rotation = Quaternion.identity;
        theTank1Script.tankBarrel.transform.SetParent(theTank1Script.tankHead.transform, false);

        return bottomBit;
    }
    GameObject MODULARIZE returnSimpleTankTurretWithoutBarrel()
    {
        GameObject turret = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.simpleTankTurretWITHOUTBarrel, new Vector3(0, 0, 0), "returnSimpleTank1");

        return turret;
    }

    
    GameObject returnSimpleTankTurretWithoutBarrel()
    {
        GameObject turret = Instantiate(, new Vector3(0, 0, 0), "returnSimpleTank1");

        return turret;
    }

    GameObject returnSimpleTankBarrel()
    {
        GameObject barrel = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(, new Vector3(0, 0, 0), "returnSimpleTank1");

        return barrel;
    }


    GameObject MODULARIZE returnSimpleTankBarrel()
    {
        GameObject barrel = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.simpleTankBarrel, new Vector3(0, 0, 0), "returnSimpleTank1");

        return barrel;
    }
    GameObject MODULARIZE returnFlamethrowerNPC()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestAgent1");

        newObj.AddComponent<NavMeshAgent>();
        newObj.AddComponent<AIHub2>();

        //AIcontroller

        enactionScript theEnactionScript = newObj.GetComponent<enactionScript>();
        //              theEnactionScript.availableEnactions.Add("shootFlamethrower1");
        //shootFlamethrower1
        return newObj;
    }
    GameObject MODULARIZE returnPineTree1()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.pineTree1, new Vector3(0, 0, 0), "returnPineTree1");


        newObj.AddComponent<interactionScript>();
        interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        theInteractionScript.addInteraction("shootFlamethrower1", "burn");

        return newObj;
    }


    GameObject MODULARIZE slab1()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "slab1");
        //newObj.transform.localScale = new Vector3(3f, 0.4f, 3f);
        //          newObj.transform.localScale = new Vector3(0.3f, 1f, 3f);


        //          Rigidbody theRigid = newObj.AddComponent<Rigidbody>();
        //          theRigid.mass = 44;
        //          theRigid.angularDrag = 0.01f;

        //          newton1 theNewton = newObj.AddComponent<newton1>();
        //          theNewton.thisMass = 44;

        //          newObj.AddComponent<standardGravity>();






        //          GameObject aGlue1 = glueGlob(new Vector3(0.25f, 1.5f, 0f));
        //          aGlue1.transform.SetParent(newObj.transform);



        GameObject aGlue2 = glueGlob(new Vector3(-0.25f, -1.5f, 0f));
        aGlue2.transform.SetParent(newObj.transform);



        GameObject aGlue3 = glueGlob(new Vector3(0.25f, 1.5f, 0f));
        aGlue3.transform.SetParent(newObj.transform);



        GameObject aGlue4 = glueGlob(new Vector3(-0.25f, -1.5f, 0f));
        aGlue4.transform.SetParent(newObj.transform);







        return newObj;
    }
    GameObject MODULARIZE motor1()
    {

        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCylinderPrefab, new Vector3(0, 0, 0), "motor");
        newObj.transform.localScale = new Vector3(1f, 3f, 1f);
        newObj.transform.rotation = Quaternion.Euler(90, 0, 0);



        Rigidbody theRigid = newObj.AddComponent<Rigidbody>();
        theRigid.mass = 555;


        newton1 theNewton = newObj.AddComponent<newton1>();
        theNewton.thisMass = 555;

        newObj.AddComponent<standardGravity>();

        moveByForce theMove = newObj.AddComponent<moveByForce>();
        theMove.turnedOn = true;
        //theMove.enabled = false;

        GameObject aGlue2 = glueGlob(new Vector3(-0.25f, -0.5f, 0f));
        aGlue2.transform.SetParent(newObj.transform);





        return newObj;
    }
    GameObject MODULARIZE anchor1()
    {

        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "anchor");
        newObj.transform.localScale = new Vector3(5f, 11f, 5f);
        //newObj.transform.position += new Vector3(0, 3, 3);



        Rigidbody theRigid = newObj.AddComponent<Rigidbody>();
        theRigid.isKinematic = true;
        theRigid.mass = 888;


        newton1 theNewton = newObj.AddComponent<newton1>();
        theNewton.thisMass = 888f;
        theNewton.allowMovement = false;



        GameObject aGlue2 = glueGlob(new Vector3(2.5f, -3f, 0f));
        aGlue2.transform.SetParent(newObj.transform);





        return newObj;
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

        int tanks = 11;

        while(tanks > 0)
        {
            tanks--;
            GameObject t1 = returnSimpleTank1();
            t1.transform.position += t1.transform.forward * 8*tanks;
            t1.transform.position += t1.transform.right * -40;
        }
        


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
                //repository2.singleton.VERTICALplaceOnLineAndDuplicate(thisObject, howManySlabs, thisInitialXValue + thisXValueSpaxing * loopNumber, theZSpacing * currentLocation, nonZoneOffset, 18.4f);

                float theX = thisInitialXValue + 4.3f + currentSlab * slightHorizontalOffset;
                float theY = vertStart + (vertSpacing * currentSlab);
                float theZ = nonZoneOffset + (theZSpacing * (currentLocation - 1));

                //repository2.singleton.createPrefabAtPoint(slabStack[0], new Vector3(theX, theY, theZ));


                float halfSize = 1.2f;

                GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "slab1");
                //newObj.transform.localScale = new Vector3(3f, 0.4f, 3f);
                newObj.transform.localScale = new Vector3(0.2f, halfSize * 2, 3f);


                Rigidbody theRigid = newObj.AddComponent<Rigidbody>();
                theRigid.mass = 111;
                theRigid.angularDrag = 0f;
                theRigid.isKinematic = true;

                newton1 theNewton = newObj.AddComponent<newton1>();
                theNewton.thisMass = 111;

                newObj.AddComponent<standardGravity>();






                GameObject aGlue1 = glueGlob(new Vector3(0f, halfSize, halfSize));
                aGlue1.transform.SetParent(newObj.transform);


                GameObject aGlue2 = glueGlob(new Vector3(0f, -halfSize, halfSize));
                aGlue2.transform.SetParent(newObj.transform);



                GameObject aGlue3 = glueGlob(new Vector3(0f, halfSize, -halfSize));
                aGlue3.transform.SetParent(newObj.transform);



                GameObject aGlue4 = glueGlob(new Vector3(0f, -halfSize, -halfSize));
                aGlue4.transform.SetParent(newObj.transform);


                if (true == false)
                {

                }







                if (reverse == true)
                {
                    newObj.transform.rotation = Quaternion.Euler(0, 180, 0);
                    newObj.transform.rotation = Quaternion.Euler(0, 0, -35);
                    reverse = false;
                }
                else
                {
                    newObj.transform.rotation = Quaternion.Euler(0, 0, 35);
                    reverse = true;
                }

                newObj.transform.position = new Vector3(theX, theY, theZ);


                loopNumber++;
            }

            currentSlab = 0;
        }



    }

    public void anchors()
    {

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, howManyZones, theZSpacing, thisInitialXValue + thisXValueSpaxing * loopNumber, nonZoneOffset, 18.4f);
            loopNumber++;
        }

    }

    public void motors()
    {

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, howManyZones, theZSpacing, thisInitialXValue + thisXValueSpaxing, nonZoneOffset, vertHeight);

        }

    }









    void generateJustOneNPC()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, 1, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }
        
        GameObject theZoneObject = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2");
        theZoneObject.transform.localScale = new Vector3(400f, 100f, 145f);
    }


    void generateWIDEZonesAndAgentsEtc()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, howMany * 2, theZSpacing, thisInitialXValue + thisXValueSpaxing * loopNumber, nonZoneOffset);
            loopNumber++;
        }


        List<GameObject> zoneList = new List<GameObject>();
        GameObject theZoneObject = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2");
        theZoneObject.transform.localScale = new Vector3(400f, 10f, 45f);
        zoneList.Add(theZoneObject);

        loopNumber = 0;
        foreach (GameObject thisObject in zoneList)
        {
            //Debug.Log(thisObject);
            repository2.singleton.placeOnLineAndDuplicate(thisObject, howMany, theZSpacing * 2, thisInitialXValue + thisXValueSpaxing * 2 * loopNumber);
            loopNumber++;
        }



    }

    void generateWIDEZonesAndAgentsEtcMANYAgentsPerZoneWithGuns()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());



        objectList.Add(returnFlamethrowerNPC());

        //objectList.Add(returnPineTree1());

        //objectList.Add(returnFlamethrowerNPC());
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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, howManyZones * 2, theZSpacing, thisInitialXValue + thisXValueSpaxing * loopNumber, nonZoneOffset);
            loopNumber++;
        }


        List<GameObject> zoneList = new List<GameObject>();
        GameObject theZoneObject = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2");
        theZoneObject.transform.localScale = new Vector3(800f, 10f, 50f);
        zoneList.Add(theZoneObject);

        loopNumber = 0;
        foreach (GameObject thisObject in zoneList)
        {
            //Debug.Log(thisObject);
            repository2.singleton.placeOnLineAndDuplicate(thisObject, howManyZones, theZSpacing * 2, thisXValueSpaxing * 2 * loopNumber);
            loopNumber++;
        }



    }


    void generateWIDEZonesAndAgentsEtcMANYAgentsPerZone()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, howManyZones * 2, theZSpacing, thisInitialXValue + thisXValueSpaxing * loopNumber, nonZoneOffset);
            loopNumber++;
        }


        List<GameObject> zoneList = new List<GameObject>();
        GameObject theZoneObject = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2");
        theZoneObject.transform.localScale = new Vector3(400f, 10f, 45f);
        zoneList.Add(theZoneObject);

        loopNumber = 0;
        foreach (GameObject thisObject in zoneList)
        {
            //Debug.Log(thisObject);
            repository2.singleton.placeOnLineAndDuplicate(thisObject, howManyZones, theZSpacing * 2,  thisXValueSpaxing * 2 * loopNumber);
            loopNumber++;
        }



    }




    void wellThisHasEachZoneRightBEsideEAchOtherWithZeroSpaceInBetweenNeat()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, howMany*2, theZSpacing, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }


        List<GameObject> zoneList = new List<GameObject>();
        GameObject theZoneObject = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2");
        theZoneObject.transform.localScale = new Vector3(200f, 1f, 50f);
        zoneList.Add(theZoneObject);

        loopNumber = 0;
        foreach (GameObject thisObject in zoneList)
        {
            //Debug.Log(thisObject);
            repository2.singleton.placeOnLineAndDuplicate(thisObject, howMany, theZSpacing*2, thisInitialXValue + thisXValueSpaxing*2 * loopNumber);
            loopNumber++;
        }



    }







    void generateOneNPCPerZone()
    {
        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());

        objectList.Add(returnTestKey1());

        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestKey1());
        objectList.Add(repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, 111, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
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
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

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
        objectList.Add(repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, 111, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
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
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

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
        objectList.Add(repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, 201, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
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
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestAgent1());

        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, 202, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
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
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");


        objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, 33, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
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
        //GameObject testCube = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");


        //objectList.Add(returnTestAgent1());
        objectList.Add(returnTestKey1());
        objectList.Add(returnTestLOCK1());
        objectList.Add(repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.mapZone2, new Vector3(0, 0, 0), "mapZone2")); //objectList.Add(returnTestLocation2());
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
            repository2.singleton.placeOnLineAndDuplicate(thisObject, 3, 11, thisInitialXValue + thisXValueSpaxing * loopNumber);
            loopNumber++;
        }
    }











    //     [[[[[[[[[[[[    generating specific objects    ]]]]]]]]]]]]]



    GameObject glueGlob(Vector3 location)
    {


        GameObject prefabToUse = repository2.singleton.interactionSphere;


        GameObject newGlue = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(prefabToUse, new Vector3(0, 0, 0), "glue");
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
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "slab1");
        //newObj.transform.localScale = new Vector3(3f, 0.4f, 3f);
        //          newObj.transform.localScale = new Vector3(0.3f, 1f, 3f);


        //          Rigidbody theRigid = newObj.AddComponent<Rigidbody>();
        //          theRigid.mass = 44;
        //          theRigid.angularDrag = 0.01f;

        //          newton1 theNewton = newObj.AddComponent<newton1>();
        //          theNewton.thisMass = 44;

        //          newObj.AddComponent<standardGravity>();






        //          GameObject aGlue1 = glueGlob(new Vector3(0.25f, 1.5f, 0f));
        //          aGlue1.transform.SetParent(newObj.transform);



        GameObject aGlue2 = glueGlob(new Vector3(-0.25f, -1.5f, 0f));
        aGlue2.transform.SetParent(newObj.transform);



        GameObject aGlue3 = glueGlob(new Vector3(0.25f, 1.5f, 0f));
        aGlue3.transform.SetParent(newObj.transform);



        GameObject aGlue4 = glueGlob(new Vector3(-0.25f, -1.5f, 0f));
        aGlue4.transform.SetParent(newObj.transform);







        return newObj;
    }



    GameObject motor1()
    {

        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCylinderPrefab, new Vector3(0, 0, 0), "motor");
        newObj.transform.localScale = new Vector3(1f, 3f, 1f);
        newObj.transform.rotation = Quaternion.Euler(90, 0, 0);



        Rigidbody theRigid = newObj.AddComponent<Rigidbody>();
        theRigid.mass = 555;


        newton1 theNewton = newObj.AddComponent<newton1>();
        theNewton.thisMass = 555;

        newObj.AddComponent<standardGravity>();

        moveByForce theMove = newObj.AddComponent<moveByForce>();
        theMove.turnedOn = true;
        //theMove.enabled = false;

        GameObject aGlue2 = glueGlob(new Vector3(-0.25f, -0.5f, 0f));
        aGlue2.transform.SetParent(newObj.transform);





        return newObj;
    }


    GameObject anchor1()
    {

        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "anchor");
        newObj.transform.localScale = new Vector3(5f, 11f, 5f);
        //newObj.transform.position += new Vector3(0, 3, 3);



        Rigidbody theRigid = newObj.AddComponent<Rigidbody>();
        theRigid.isKinematic = true;
        theRigid.mass = 888;


        newton1 theNewton = newObj.AddComponent<newton1>();
        theNewton.thisMass = 888f;
        theNewton.allowMovement = false;



        GameObject aGlue2 = glueGlob(new Vector3(2.5f, -3f, 0f));
        aGlue2.transform.SetParent(newObj.transform);





        return newObj;
    }


    void generateTestAgent1()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "generateTestAgent1");

        //GameObject newObj2 = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.invisiblePoint, startPoint.transform.position + new Vector3(0, 0, 0.8f), "generateTestAgent1 pointer");

        //newObj2.transform.SetParent(newObj.transform, true);

        //newObj.AddComponent<selfDestructScript1>();
        newObj.transform.position += new Vector3(7, 0, 11);

        newObj.AddComponent<NavMeshAgent>();
        newObj.AddComponent<AIHub2>();
        //newObj.GetComponent<AIHub2>().pointerPointToPutOnBody = newObj2;


        //newObj.GetComponent<AIHub2>().testTarget = testTarget;
        //newObj.AddComponent<interactionEffects1>();
        //newObj.GetComponent<interactive1>().inOutBoolSignal = true;  //didn't work, probably have to wait at least one frame, for it to initialize...?  oof....


        //thisGameObject.transform.parent = yourParentObject.transform;

    }


    GameObject returnTestAgent2()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestAgent1");

        newObj.AddComponent<NavMeshAgent>();
        newObj.AddComponent<AIHub2>();

        //AIcontroller

        enactionScript theEnactionScript = newObj.GetComponent<enactionScript>();
        //                  theEnactionScript.availableEnactions.Add("shoot1");

        body1 theBodyScript = newObj.GetComponent<body1>();
        theBodyScript.theBodyGameObject = newObj;  //ahh, might need to.....fix that in....duplifier....

        return newObj;
    }

    GameObject returnFlamethrowerNPC()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestAgent1");

        newObj.AddComponent<NavMeshAgent>();
        newObj.AddComponent<AIHub2>();

        //AIcontroller

        enactionScript theEnactionScript = newObj.GetComponent<enactionScript>();
        //              theEnactionScript.availableEnactions.Add("shootFlamethrower1");
        //shootFlamethrower1
        return newObj;
    }

    GameObject returnPineTree1()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.pineTree1, new Vector3(0, 0, 0), "returnPineTree1");


        newObj.AddComponent<interactionScript>();
        interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        theInteractionScript.addInteraction("shootFlamethrower1", "burn");

        return newObj;
    }

    GameObject returnTestAgent1()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0,0,0), "returnTestAgent1");
        
        newObj.AddComponent<NavMeshAgent>();
        newObj.AddComponent<AIHub2>();


        //enactionScript theEnactionScript = newObj.GetComponent<enactionScript>();
        //theEnactionScript.availableEnactions.Add("shoot1");

        return newObj;
    }

    public GameObject returnSimpleTank1()
    {
        GameObject bottomBit = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.simpleTankBottom, new Vector3(0, 0, 0), "returnSimpleTank1");

        //bottomBit.AddComponent<NavMeshAgent>();
        //bottomBit.AddComponent<AIHub2>();


        //  bottomBit.AddComponent<interactionScript>();
        //  interactionScript theInteractionScript = bottomBit.GetComponent<interactionScript>();

        //  theInteractionScript.addInteraction("standardClick", "useVehicle");

        Rigidbody rigidbody = bottomBit.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        //  interactionScript theInteractions = bottomBit.AddComponent<interactionScript>();
        //  theInteractions.addInteraction("tankShot", "burn");

        //bottomBit.AddComponent<tank1>();
        tank1 theTank1Script = bottomBit.GetComponent<tank1>();
        theTank1Script.tankHead = returnSimpleTankTurretWithoutBarrel();
        theTank1Script.tankHead.transform.rotation = Quaternion.identity;
        theTank1Script.tankHead.transform.SetParent(bottomBit.transform, true);
        //initializeAim(theTank1Script.tankHead.transform.GetChild(0).gameObject, theTank1Script.firingStartPoint, theTank1Script.firingDirectionPoint);

        theTank1Script.tankBarrel = returnSimpleTankBarrel();
        theTank1Script.tankBarrel.transform.position += new Vector3(0, 2.1f, 1.2f);
        theTank1Script.tankBarrel.transform.rotation = Quaternion.identity;
        theTank1Script.tankBarrel.transform.SetParent(theTank1Script.tankHead.transform, false);

        return bottomBit;
    }


    public void initializeAim(GameObject aimingObject, GameObject startPoint, GameObject endpoint)
    {
        //child of "aimingObject" = "startPoint"
        //child of "startPoint" = "endpoint"

        startPoint = aimingObject.transform.GetChild(0).gameObject;
        endpoint = startPoint.transform.GetChild(0).gameObject;
    }



    GameObject returnTestLocation2()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestLocation2");

        //newObj.AddComponent<NavMeshAgent>();
        //newObj.AddComponent<AIHub2>();

        return newObj;
    }



    //          GENERATE SPECIFIC OBJECTS
    

    GameObject generateTowerHoop(GameObject theTower)
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "generateTowerHoop");
        //Debug.Log("///////////////////////////////////////////////////////////////after creating object");
        //Debug.Log(newObj.name);
        newObj.transform.localScale = new Vector3(1.7f, 1.3f, 1.7f);

        //newObj.AddComponent<interactive1>();
        

























        return newObj;
    }

    void generateTestKey1()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "generateTestKey1");
        interactionScript theInteractionScript = newObj.AddComponent<interactionScript>();
        newObj.transform.localScale = new Vector3(0.5f, 2, 0.5f);
        newObj.transform.position += new Vector3(0, 3, 3);

        //theInteractionScript.listOfInteractions.Add("grabKey");
        theInteractionScript.addInteraction("standardClick", "grabKey");


    }
    void generateTestLOCK1()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "generateTestLOCK1");
        interactionScript theInteractionScript = newObj.AddComponent<interactionScript>();
        newObj.transform.localScale = new Vector3(1f, 2, 1f);
        newObj.transform.position += new Vector3(0, 0, -8);

        //theInteractionScript.listOfInteractions.Add("clickLock");
        theInteractionScript.addInteraction("standardClick", "clickLock");




    }

    GameObject returnTestKey1()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestKey1");
        interactionScript theInteractionScript = newObj.AddComponent<interactionScript>();
        newObj.transform.localScale = new Vector3(0.5f, 2, 0.5f);
        //newObj.transform.position += new Vector3(0, 3, 3);

        //theInteractionScript.listOfInteractions.Add("grabKey");
        theInteractionScript.addInteraction("standardClick", "grabKey");

        return newObj;

    }
    GameObject returnTestLOCK1()
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestLOCK1");
        interactionScript theInteractionScript = newObj.AddComponent<interactionScript>();
        newObj.transform.localScale = new Vector3(1f, 3, 1f);
        //newObj.transform.localScale = new Vector3(1f, 2, 1f);
        //newObj.transform.position += new Vector3(0, 0, -8);

        //theInteractionScript.listOfInteractions.Add("clickLock");
        theInteractionScript.addInteraction("standardClick", "clickLock");



        return newObj;

    }


    void generateInteractionSlot1(Vector3 theLocation)
    {
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, theLocation, "generateInteractionSlot1");
        newObj.transform.localScale = new Vector3(0.7f, 0.3f, 0.7f);




    }

    void generateRing(Vector3 theLocation)
    {
        //"ring", for now cube, whatever
        GameObject newObj = repository2.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, theLocation, "generateRing");
        newObj.transform.localScale = new Vector3(1.7f, 0.5f, 1.7f);



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


    */
}

public class generatorJob
{
    Vector3 originPoint;


    
}


