using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
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
        //Debug.Log("Awake:  " + this);
        singletonify();

        //doing this so the singleton is ready before other objects we generate need it
        this.gameObject.AddComponent<tagging2>();
        this.gameObject.AddComponent<interactionCreator>();
        this.gameObject.AddComponent<comboGen>();
        this.gameObject.AddComponent<outpostGame>();
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
        //Debug.Log("Start:  " + this);

        GameObject theWorldObject = GameObject.Find("World");

        //axes conventions:
        //      z = "forward"/"rows"/"length"
        //      x = "right"/"columns"/"width"
        //      y = [obviously "up"/"verticalColumns"/"height"]

        //createCubeGridTest();


        Vector3 startPoint = new Vector3(0, 1, 0);
        /*
        int rows = 5;
        int columns = 3;
        int verticalColumns = 1;

        float zScale = 5f;
        float xScale = 3f;
        float yScale = 1f;
        */
        int rows = 30;
        int columns = 3;
        int verticalColumns = 1;

        float zScale = 30f;
        float xScale = 100f;
        float yScale = 20f;
        //List<Vector3> points = new gridOfPoints(startPoint, rows, columns, zScale, xScale, yScale, verticalColumns).returnIt();
        createMapZones(startPoint, rows, columns, zScale, xScale, yScale, verticalColumns);



        Vector3 location = Vector3.zero;

        location = new Vector3(-20, 2.5f, -55);
        //location = new Vector3(-55, 14, -67);

        makePLAYER(new Vector3(-5, 1, -1));
        //theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;

        //generateScene2();
        //generateScene3();

        //          NEWgenerateScene3();
        //oneZoneNoNPCS();
        //generateFlex();

        //Time.timeScale = 0f;
    }

    private static void createCubeGridTest()
    {
        Vector3 startPoint = new Vector3(0, 1, 0);

        int rows = 5;
        int columns = 3;
        int verticalColumns = 1;

        float zScale = 5f;
        float xScale = 3f;
        float yScale = 1f;

        List<Vector3> points = new gridOfPoints(startPoint, rows, columns, zScale, xScale, yScale, verticalColumns).returnIt();

        new doAtEachPoint(new makeMastLineAtPoint(), points);
        new doAtEachPoint(new makeObjectAtPoint(xScale, yScale, zScale), points);
    }

    private static void createMapZones(Vector3 origin, int zRows, int xColumns, float zLength, float xWidth, float yHeight = 10f, int verticalColumns = 1)
    {

        List<Vector3> points = new gridOfPoints(origin, zRows, xColumns, zLength, xWidth, yHeight, verticalColumns).returnIt();

        new doAtEachPoint(new makeMastLineAtPoint(), points);
        new doAtEachPoint(new makeMapZoneAtPoint(xWidth, yHeight, zLength), points);
        //new doAtEachPoint(new makeObjectAtPoint2(xWidth, verticalColumns, zLength), points);
    }

    void makePLAYER(Vector3 location)
    {
        GameObject player = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.player, location);
        genGen.singleton.addBody4ToObject(player);
    }


    void oneZoneNoNPCS()
    {

        //      [reverse the order of the following?][or make "objects" with transform first, duplicate those, then for each one, add the other components?]
        //Which object I want to create 
        //      with all of its properties and its location/ rotation,
        //how many per Zone
        //how many zones
        //how big are zones [they are another kind of object, by definition one per zone] and
        //spacing [patternScript2!  dot singleton]


        int howManyZones = 1;
        int howManySetsPerZone = 2;
        int theZSpacing = 25;
        int theXSpacing = 10;
        float sideOffset = 0;

        //create a line of points, spaced by multiples of "howManysetsPerZone"
        //makeAndFillZones(objectList1(), 11);


        INVERSEmakeAndFillZonesNoNPCs(
            patternScript2.singleton.makeLinePattern2(howManyZones * howManySetsPerZone, theZSpacing, sideOffset),
            theXSpacing
            );


        //do this last to make zone collisions easier
        makeEmptyZones(howManyZones, theZSpacing * howManySetsPerZone);

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

        int regularThinZoneSpacing = 25;
        int oneBIGzoneSpacint = 525;



        int howManyZones = 0;
        int howManySetsPerZone = 2;
        int theZSpacing = regularThinZoneSpacing;
        //int theZSpacing = oneBIGzoneSpacint;
        int theXSpacing = 10;
        float sideOffset = 0;

        //create a line of points, spaced by multiples of "howManysetsPerZone"
        //makeAndFillZones(objectList1(), 11);


        INVERSEmakeAndFillZones(
            patternScript2.singleton.makeLinePattern2(howManyZones * howManySetsPerZone, theZSpacing, sideOffset), 
            theXSpacing
            );


        //do this last to make zone collisions easier
        makeEmptyZones(howManyZones, 5+(theZSpacing * (howManySetsPerZone)));


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

    void INVERSEmakeAndFillZonesNoNPCs(List<Vector3> setsPositions, int theXSpacing = 25)
    {
        float sideOffset = 0f;

        foreach (Vector3 point in setsPositions)
        {
            //List<Vector3> setsPositions = patternScript2.singleton.makeLinePattern2(howManyZones * howManySetsPerZone, theZSpacing, sideOffset);
            //makePrefabsAtListOfPoints(setsPositions, thisPrefab);
            INVERSEobjectListNoNPCs(point, theXSpacing);
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
        //GameObject testCube = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());

        //objectList.Add(genGen.singleton.returnSimpleTank2(this.transform.position));
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
        //GameObject testCube = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());
        //objectList.Add(genGen.singleton.returnGun1(startPosition));
        //objectList.Add(genGen.singleton.returnSimpleTank2(startPosition));
        //objectList.Add(genGen.singleton.returnNPC5(startPosition));
        objectList.Add(genGen.singleton.returnShotgun1(startPosition));
        //objectList.Add(genGen.singleton.returnPineTree1(startPosition));
        //  objectList.Add(genGen.singleton.returnGun1(startPosition));
        //  objectList.Add(genGen.singleton.returnNPC5(startPosition));
        //  objectList.Add(genGen.singleton.returnGun1(startPosition));
        objectList.Add(genGen.singleton.returnPineTree1(startPosition));
        //      objectList.Add(genGen.singleton.returnShotgun1(startPosition));
        //      objectList.Add(genGen.singleton.returnNPC5(startPosition));
        //objectList.Add(genGen.singleton.returnShotgun1(startPosition));
        objectList.Add(genGen.singleton.returnPineTree1(startPosition));
        objectList.Add(genGen.singleton.returnNPC5(startPosition));
        objectList.Add(genGen.singleton.returnGun1(startPosition));
        //  objectList.Add(genGen.singleton.returnNPC5(startPosition));
        //objectList.Add(genGen.singleton.returnNPC4(startPosition));
        //objectList.Add(genGen.singleton.returnSimpleTank2(startPosition));
        //objectList.Add(genGen.singleton.returnShotgun1(startPosition));

        //objectList.Add(genGen.singleton.returnGun1(startPosition));

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

    List<GameObject> INVERSEobjectListNoNPCs(Vector3 startPosition, int theXSpacing = 25)
    {

        List<GameObject> objectList = new List<GameObject>();
        //GameObject testCube = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

        //objectList.Add(returnTestKey1());
        objectList.Add(genGen.singleton.returnGun1(startPosition));
        //objectList.Add(genGen.singleton.returnSimpleTank2(startPosition));
        objectList.Add(genGen.singleton.returnShotgun1(startPosition));
        objectList.Add(genGen.singleton.returnPineTree1(startPosition));
        //objectList.Add(genGen.singleton.returnSimpleTank2(startPosition));


        //objectList.Add(genGen.singleton.returnPineTree1(startPosition + new Vector3(0, 0, theXSpacing + theXSpacing)));


        //objectList.Add(returnTestKey1());


        //returnTestKey1
        //objectList.Add(testCube);
        //objectList.Add(testCube);

        int currentSlotPosition = 0;

        foreach (GameObject obj in objectList)
        {
            obj.transform.position += new Vector3(theXSpacing * currentSlotPosition, 0, 0);
            currentSlotPosition++;
        }

        return objectList;
    }







    GameObject slab1()
    {
        GameObject newObj = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "slab1");
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

        GameObject newObj = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCylinderPrefab, new Vector3(0, 0, 0), "motor");
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

        GameObject newObj = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "anchor");
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

                GameObject newObj = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "slab1");
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
        //GameObject testCube = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

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
        //GameObject testCube = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, startPoint.transform.position, "testCube");

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







    //     [[[[[[[[[[[[    generating specific objects    ]]]]]]]]]]]]]



    GameObject glueGlob(Vector3 location)
    {


        GameObject prefabToUse = repository2.singleton.interactionSphere;


        GameObject newGlue = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(prefabToUse, new Vector3(0, 0, 0), "glue");
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



    /*

    GameObject slab1()
    {
        GameObject newObj = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "slab1");
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

        GameObject newObj = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCylinderPrefab, new Vector3(0, 0, 0), "motor");
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

        GameObject newObj = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "anchor");
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

    */


    GameObject returnTestLocation2()
    {
        GameObject newObj = genGen.singleton.createAndReturnPrefabAtPointWITHNAME(repository2.singleton.placeHolderCubePrefab, new Vector3(0, 0, 0), "returnTestLocation2");

        //newObj.AddComponent<NavMeshAgent>();
        //newObj.AddComponent<AIHub2>();

        return newObj;
    }



    //          GENERATE SPECIFIC OBJECTS
    








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



public class zoneSpacingCalc
{

    int regularThinZoneSpacing = 25;
    int oneBIGzoneSpacint = 525;

    int howManyZones = 1;
    int howManySetsPerZone = 2;
    int theZSpacing = 25;
    int theXSpacing = 10;
    float sideOffset = 0;

    float xScale = 100;
    float yScale = 1;
    float zScale = 100;



    //patternScript2.singleton.makeLinePattern2(howManyZones* howManySetsPerZone, theZSpacing, sideOffset), theXSpacing);



}

public class zoneGen1 //: objectGen
{
    int regularThinZoneSpacing = 25;
    int oneBIGzoneSpacint = 525;

    int howManyZones = 1;
    int howManySetsPerZone = 2;
    int theZSpacing = 25;
    int theXSpacing = 10;
    float sideOffset = 0;

    public zoneGen1(Vector3 position, float xLength, float zWidth, float yHeight = 10f)
    {
        measuredZoneCube(position, xLength, zWidth, yHeight);
    }

    public void generate()
    {

    }

    public GameObject unitZoneCube(Vector3 position)
    {
        GameObject unitZoneCube = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.invisibleCubePrefab, position);
        Rigidbody rigidbody = unitZoneCube.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = false;

        unitZoneCube.AddComponent<mapZoneScript>();

        return unitZoneCube;
    }

    public GameObject measuredZoneCube(Vector3 position, float xLength, float zWidth, float yHeight =10f)
    {
        GameObject newZone = unitZoneCube(position);
        newZone.transform.localScale = new Vector3(xLength, yHeight, zWidth);

        return newZone;
    }




    void NEWgenerateScene3()
    {
        howManyZones = 0;
        howManySetsPerZone = 2;
        theZSpacing = regularThinZoneSpacing;
        //int theZSpacing = oneBIGzoneSpacint;
        theXSpacing = 10;
        sideOffset = 0;


        //INVERSEmakeAndFillZones(
        //  patternScript2.singleton.makeLinePattern2(howManyZones * howManySetsPerZone, theZSpacing, sideOffset),
        //theXSpacing);


        //do this last to make zone collisions easier
        makeEmptyZones(howManyZones, 5 + (theZSpacing * (howManySetsPerZone)));


    }

    void makeEmptyZones(int howManyZones, int theZSpacing)
    {

        List<Vector3> zonePositions = patternScript2.singleton.makeLinePattern1(howManyZones, theZSpacing);


        foreach (Vector3 thisPoint in zonePositions)
        {
            //Instantiate(prefab, thisPoint, default);
            GameObject newObj = repository2.Instantiate(repository2.singleton.mapZone2, thisPoint, Quaternion.identity);
            newObj.transform.localScale = new Vector3(400f, 10f, 1f * theZSpacing);

        }
    }
}


public class zoneGridGen
{
    public void createGridOfZones(Vector3 origin, int xQuantity, int zQuantity, float xLength, float zWidth, float yHeight = 10f, int yQuantity = 1)
    {
        //how to do?
        //      create grid of origins
        //      for each point, place a map zone [all with same length/width/height]

        List<Vector3> positionList = new gridOfPoints(origin,xQuantity,zQuantity,xLength,zWidth,yHeight,yQuantity).returnIt();

        foreach (Vector3 thisPoint in positionList)
        {
            new zoneGen1(thisPoint,xLength,zWidth,yHeight);
        }
    }

    
    public List<Vector3> addPositionLists(List<Vector3> list1, List<Vector3> list2)
    {
        foreach (Vector3 thisPoint in list2)
        {
            list1.Add(thisPoint);
        }

        return list1;
    }

    public List<Vector3> makeLinePattern2(int howMany, float zSpacing, float xOffset, float yOffset = 0f)
    {
        List<Vector3> thisList = new List<Vector3>();

        int whichPositionWeAreOn = 0;

        while (whichPositionWeAreOn < howMany)
        {
            thisList.Add(new Vector3(xOffset, yOffset, whichPositionWeAreOn * zSpacing));
            whichPositionWeAreOn++;
        }

        return thisList;
    }
}

public class doAtEachPoint
{
    public doAtEachPoint(doAtPoint toDo, List<Vector3> thePoints)
    {
        foreach(Vector3 thisPoint in thePoints)
        {
            toDo.doIt(thisPoint);
        }
    }
}

public abstract class doAtPoint
{
    internal abstract void doIt(Vector3 thisPoint);
}

public class makeObjectAtPoint : doAtPoint
{
    private float xScale;
    private float yScale;
    private float zScale;

    public makeObjectAtPoint(float xScale, float yScale, float zScale)
    {
        this.xScale = xScale;
        this.yScale = yScale;
        this.zScale = zScale;
    }

    internal override void doIt(Vector3 thisPoint)
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thisPoint, Quaternion.identity);
        newObj.transform.localScale = new Vector3(xScale, yScale, zScale);
    }
}
public class makeMapZoneAtPoint : doAtPoint
{
    private float xScale;
    private float yScale;
    private float zScale;

    public makeMapZoneAtPoint(float xScale, float yScale, float zScale)
    {
        this.xScale = xScale;
        this.yScale = yScale;
        this.zScale = zScale;
    }

    internal override void doIt(Vector3 thisPoint)
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.invisibleCubePrefab, thisPoint, Quaternion.identity);
        newObj.transform.localScale = new Vector3(xScale, yScale, zScale);

        Collider theCollider = newObj.GetComponent<Collider>();
        theCollider.isTrigger = true;

        Rigidbody rigidbody = newObj.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;


        newObj.AddComponent<mapZoneScript>();

    }






    /*

    internal override void doIt(Vector3 thisPoint)
    {
        GameObject newObj = measuredZoneCube(thisPoint, xScale, zScale, yScale);//repository2.Instantiate(repository2.singleton.placeHolderCubePrefab, thisPoint, Quaternion.identity);
        //newObj.transform.localScale = new Vector3(xScale, yScale, zScale);
    }



    public GameObject unitZoneCube(Vector3 position)
    {
        GameObject unitZoneCube = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.invisibleCubePrefab, position);
        Rigidbody rigidbody = unitZoneCube.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;

        unitZoneCube.AddComponent<mapZoneScript>();

        return unitZoneCube;
    }

    public GameObject measuredZoneCube(Vector3 position, float xLength, float zWidth, float yHeight = 10f)
    {
        GameObject newZone = unitZoneCube(position);
        newZone.transform.localScale = new Vector3(xLength, yHeight, zWidth);

        return newZone;
    }

    */
}

