using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class initialGenerator2 : MonoBehaviour
{

    public int newEffingInt = 22;

    public Dictionary<string, interactionSubAtom> subAtoms = new Dictionary<string, interactionSubAtom>();
    public List<interactionSubAtom> subAtomInitializerList;

    public Dictionary<string, interactionAtom> atoms = new Dictionary<string, interactionAtom>();
    public List<interactionAtom> atomInitializerList;

    public GameObject startPoint;
    public repository2 theRespository;

    public GameObject testTarget;


    public GameObject fuckThis;

    void Awake()
    {

        //  ==== 1 ====    make SUB atoms here:
        subAtomInitializerList = new List<interactionSubAtom>
        {
            generateInteractionSUBAtomFULL("grabTestKeySubAtom", stringLister(), stringLister()), 
            generateInteractionSUBAtomFULL("proximity0SubAtom", stringLister(), stringLister()),
            generateInteractionSUBAtomFULL("mainInteractionSubAtom", stringLister(), stringLister("standardInteraction1"))
        };
        processSubatomList(subAtomInitializerList);


        //  ==== 2 ====    make [interaction/action] "ATOMS" here:
        atomInitializerList = new List<interactionAtom>
        {
            generateInteractionAtomFULL("grabTestKey1Atom", interactionSUBAtomLister(subAtoms["grabTestKeySubAtom"])),
            generateInteractionAtomFULL("proximity0Atom", interactionSUBAtomLister(subAtoms["proximity0SubAtom"])),
            generateInteractionAtomFULL("standardInteraction1Atom", interactionSUBAtomLister(subAtoms["mainInteractionSubAtom"]))
        };
        processAtomList(atomInitializerList);

    }

    public void processSubatomList(List<interactionSubAtom> theList)
    {
        foreach (interactionSubAtom thisSubAtom in theList)
        {
            //Debug.Log("name:  " + thisSubAtom.name);
            subAtoms.Add(thisSubAtom.name, thisSubAtom);
        }
    }

    public void processAtomList(List<interactionAtom> theList)
    {
        foreach (interactionAtom thisAtom in atomInitializerList)
        {
            atoms.Add(thisAtom.name, thisAtom);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        generateTestAgent1();

        //generateTestTower1();

        generateTestKey1();

        generateTestLOCK1();

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }



    void generateTestAgent1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "generateTestAgent1");
        //myTest.AddComponent<selfDestructScript1>();
        myTest.transform.position += new Vector3(7, 0, 11);

        myTest.AddComponent<NavMeshAgent>();
        myTest.AddComponent<AIHub2>();
        //myTest.GetComponent<AIHub2>().testTarget = testTarget;
        //myTest.AddComponent<interactionEffects1>();
        //myTest.GetComponent<interactive1>().inOutBoolSignal = true;  //didn't work, probably have to wait at least one frame, for it to initialize...?  oof....


        //thisGameObject.transform.parent = yourParentObject.transform;

    }


    void generateTestTower1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "generateTestTower1");
        myTest.AddComponent<interactionEffects1>();
        myTest.transform.localScale = new Vector3(0.5f,5,0.5f);
        myTest.transform.position += new Vector3(0, 5, 0);

        //does this fucking exist?
        //myTest.GetComponent<interactionEffects1>().slotObjects.Add(this.gameObject);
        fuckThis = myTest;

        //List<GameObject> listOfSlots = theRespository.makeListOfObjects(generateTowerSlot(myTest));

        List<GameObject> listOfSlotObjects = new List<GameObject>();
        int quantity = 5;
        for (int i = 0; i < quantity; i++)
        {

            listOfSlotObjects.Add(generateTowerSlot(myTest));
            
        }

        theRespository.placeObjectsOnLinePattern(listOfSlotObjects, theRespository.patterns.spatialRowSimpleOrigin(quantity, 2, 2));

        //theRespository.placeDuplicatesOnLine1(generateTowerSlot(myTest), theRespository.patterns.moveRowToPosition(theRespository.patterns.spatialRowSimpleOrigin(5, 2, 2), startPoint.transform.position));

        //now the hoops:
        //generateTowerHoop(myTest);
        theRespository.placeDuplicatesOnLine1(
                generateTowerHoop(myTest), 
                theRespository.patterns.moveRowToPosition(theRespository.patterns.spatialRowSimpleOrigin(5, 4, 3), 
                startPoint.transform.position += new Vector3(-15, 0, -10))
        );


        //initilaizeSlotAvailability()
        myTest.GetComponent<interactionEffects1>().initilaizeSlotAvailability();

        //generateInteractionSlot1(myTest.transform.position + new Vector3(0, 2, 0));
        //generateRing(myTest.transform.position + new Vector3(-5, -4, 0));


    }



    //          GENERATE SPECIFIC OBJECTS
    GameObject generateTowerSlot(GameObject theTower)
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "generateTowerSlot");
        myTest.transform.localScale = new Vector3(0.7f, 0.3f, 0.7f);


        //null error, tower is not initialized yet even though that happened on previous lines of code?
        //theTower.GetComponent<interactionEffects1>().slotObjects.Add(myTest);
        theTower.GetComponent<interactionEffects1>().slotObjects.Add(myTest);

        return myTest;
    }

    GameObject generateTowerHoop(GameObject theTower)
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "generateTowerHoop");
        //Debug.Log("///////////////////////////////////////////////////////////////after creating object");
        //Debug.Log(myTest.name);
        myTest.transform.localScale = new Vector3(1.7f, 1.3f, 1.7f);

        //myTest.AddComponent<interactive1>();
        myTest.AddComponent<interactionEffects1>();

        //myTest.GetComponent<interactive1>().inOutBoolSignal = true;
        //when interacted with
        //access "tower" script of some kind
        //check next available "slot"
        //move to that location

        //...........this seems like shit.  how is this supposed to work?  extremely ad-hoc.....
        //if i ws gonna seriously systematize it this way:
        //have a "reference object"
        //have a "move to X" interaction effect
        //have a way to get x from reference object, some script should know or specify...
        //...what would be easy...for the reference object itself to know what this means?
        //i hate this

        //or, go "loose".  just make a script or code for "you can pick this object up and put it down....
        //anywhere?  only put it down on tower?
        //and then....somehow have positional check when putting it down......
        //have a placement system
        //some things can be arranged however on shelves etc., but in some cases, the ONLY placement option is a specific sequence?

        //i hate this.

        //well, the only thing the hoop itself should do is be picked up.  it makes no sense to build the other location and access script from the hoop, right?
        //that's teh easy part.  the hard part is putting it DOWN correctly....

        interactionEffects1 interactionScriptOnGeneratedObject = myTest.GetComponent<interactionEffects1>();
        //interactionScriptOnGeneratedObject.interactionsAvailable.Add(interactionScriptOnGeneratedObject.generateInteraction("pickUpItem"));
        interactionScriptOnGeneratedObject.generateInteraction("pickUpItem");
        //kay, that "generates" interaction.  but does it ADD it to OBJECT and the object et to LEGIBSTRATA?


        //well, for now, let's do direct teleport to a notch on tower
        //so:
        //have tower linked to "hoops" from their inception
        //      have positions saved in the tower
        //
        //when clicking a "hoop", it has some modular things to check and do
        //type of interaction outcome:  teleport
        //
        //target selection:  "preselected host, slot sequence" type?
        //      this will look at associated object of some kind ["dock"]
        //      will look at its "slots" in sequence
        //      return the first empty one

        //theTower.GetComponent<interactionEffects1>().;
        //preselectedHost
        myTest.GetComponent<interactionEffects1>().preselectedHost = theTower;

        myTest.GetComponent<interactionEffects1>().typeOfInteractionOutcome = "teleport";
        myTest.GetComponent<interactionEffects1>().targetToSelect = "preselectedHost";
        myTest.GetComponent<interactionEffects1>().targetToSelect = "firstAvailableInSequence";


        return myTest;
    }

    void generateTestKey1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "generateTestKey1");
        myTest.AddComponent<interactionEffects1>();
        myTest.transform.localScale = new Vector3(0.5f, 2, 0.5f);
        myTest.transform.position += new Vector3(0, 3, 3);


        interactionEffects1 interactionScriptOnGeneratedObject = myTest.GetComponent<interactionEffects1>();
        //interactionScriptOnGeneratedObject.generateInteractionFULL("standardInteraction1", atomLister(atoms["grabTestKey1Atom"]));

        interactionMate mainInteractionMate = new interactionMate();

        mainInteractionMate.interactionAuthor = this.gameObject;
        GameObject theWorldObject = GameObject.Find("World");
        worldScript theWorldScript = theWorldObject.GetComponent<worldScript>();
        initialGenerator2 theGeneratorScript = theWorldObject.GetComponent("initialGenerator2") as initialGenerator2;

        //.....why am i using an interactionMate in the initial generator???
        //mainInteractionMate.enactThisInteraction = interactionScriptOnGeneratedObject.generateInteractionFULL("standardInteraction1", theGeneratorScript.atomLister(theGeneratorScript.atoms["standardInteraction1Atom"]));

        testInteraction mainInteraction = interactionScriptOnGeneratedObject.generateInteraction("justGrabIt");

        interactionScriptOnGeneratedObject.interactionDictionary.Add("standardInteraction1", mainInteraction);

    }
    void generateTestLOCK1()
    {
        GameObject myTest = theRespository.createAndReturnPrefabAtPointWITHNAME(theRespository.placeHolderCubePrefab, startPoint.transform.position, "generateTestLOCK1");
        myTest.AddComponent<interactionEffects1>();
        myTest.transform.localScale = new Vector3(1f, 2, 1f);
        myTest.transform.position += new Vector3(0, 0, -8);

        interactionEffects1 interactionScriptOnGeneratedObject = myTest.GetComponent<interactionEffects1>();
        interactionScriptOnGeneratedObject.generateInteractionFULL(
            "grabTestLOCK1",
            atomLister(
                generateInteractionAtomFULL(
                    "grabTestLOCK1Atom",
                    interactionSUBAtomLister(
                        generateInteractionSUBAtomFULL(
                            "grabTestLOCKSubAtom",
                            stringLister("proximity0"
                                ),
                            stringLister(
                                "grabTestLOCKSubAtomEffect1"
                                )
                            )
                        )
                    )
                )
            );


        testInteraction mainInteraction = interactionScriptOnGeneratedObject.generateInteraction("justActivateLock");

        interactionScriptOnGeneratedObject.interactionDictionary.Add("standardInteraction1", mainInteraction);

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







    
    public interactionAtom generateInteractionAtomFULL(string theName, List<interactionSubAtom> theSubAtoms)
    {
        interactionAtom oneToGenerate = new interactionAtom();
        oneToGenerate.name = theName;
        oneToGenerate.subAtoms = theSubAtoms;


        return oneToGenerate;
    }

    public interactionSubAtom generateInteractionSUBAtomFULL(string theName, List<string> prereqs, List<string> effects)
    {
        interactionSubAtom oneToGenerate = new interactionSubAtom();

        oneToGenerate.name = theName;
        oneToGenerate.effects = effects;
        oneToGenerate.prereqs = prereqs;


        if (true == false)
        {
            //just to learn this stuff to make inputting lists easier:

            //public List<stateItem> wantedPrereqsLister(params stateItem[] listofStateItems)
            {
                //List<stateItem> aNewList = new List<stateItem>();

                //foreach (stateItem x in listofStateItems)
                {
                    //aNewList.Add(x);
                }

                //return aNewList;
            }

            //example of use (look slike you can just add as many as you want this way:
            //wantedPrereqsLister(storageOwnership, resource1)


        }





        return oneToGenerate;
    }

    public List<interactionAtom> atomLister(params interactionAtom[] listofAtoms)
    {
        List<interactionAtom> aNewList = new List<interactionAtom>();

        foreach (interactionAtom x in listofAtoms)
        {
            aNewList.Add(x);
        }

        return aNewList;
    }

    public List<interactionSubAtom> interactionSUBAtomLister(params interactionSubAtom[] listofSUBAtoms)
    {
        List<interactionSubAtom> aNewList = new List<interactionSubAtom>();

        foreach (interactionSubAtom x in listofSUBAtoms)
        {
            aNewList.Add(x);
        }

        return aNewList;
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
