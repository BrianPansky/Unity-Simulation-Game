using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class playerClickInteraction : MonoBehaviour
{
    //DOESN'T SEEM TO WORK AFTER """""() => this.giveXbutton(order)""""" STUFF
    //SO INSTEAD USE "selectedNPC" WHICH WORKS FINE FOR SOME REASON???
    //CHANGING "clickedOn" TO PUBLIC DIDN'T FIX, AND "selectedNPC" ISN'T PUBLIC EITHER...
    public GameObject clickedOn;


    public sensorySystem theSensorySystem;


    public bool glueGunFirstShotDoneBool = false;
    public GameObject currentGlueStartPoint = null;





    public body1 body;

    //selection
    GameObject selectedNPC;
    public GameObject currentPrefab;
    public GameObject theInteractionSphere;

    public string ownershipTag;

    public GameObject gunLaserSight;

    //plug-in menu objects:
    public GameObject recruitingMenu;
    public GameObject myPrefabButton1;
    public GameObject myGridCanvas;
    public List<GameObject> currentGridButtons;


    //for debugging:
    public GameObject personOfInterest;


    public bool inMenu;
    public bool buildMode;

    //other scripts
    public playerHUD myHUD;
    public taggedWith theTagScript;
    public inventory1 theInventory;
    public worldScript theWorldScript;


    //bit ad hoc:
    //can i use object class types from other scripts before loading in those scripts??? how to do???
    public List<enactionMate> commandList = new List<enactionMate>();


    //definitely ad-hoc:
    public bool haveStore;
    public bool weapon;
    //public GameObject myPrefab;
    
    public bool swapDirection;

    // Start is called before the first frame update
    void Start()
    {


        if (theSensorySystem == null)
        {
            // theSensorySystem = myTest2.GetComponent("sensorySystem") as sensorySystem;
            theSensorySystem = this.GetComponent<sensorySystem>();
            if(theSensorySystem == null)
            {
                theSensorySystem = this.gameObject.AddComponent<sensorySystem>();
            }

            theSensorySystem.body = body;
        }



        if (theInventory == null)
        {
            theInventory = this.GetComponent<inventory1>();
            if (theInventory == null)
            {
                theInventory = this.gameObject.AddComponent<inventory1>();
            }
        }


        if (body == null)
        {

            body = this.GetComponent<body1>();
            if (body == null)
            {
                body = this.gameObject.AddComponent<body1>();
            }

        }

        haveStore = false;
        weapon = false;
        buildMode = false;

        ownershipTag = "owned by " + this.name;

        //other scripts [and components?]:
        myHUD = GetComponent<playerHUD>();
        theTagScript = GetComponent<taggedWith>();
        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;

        inMenu = false;
        recruitingMenu.SetActive(false);



        //just for testing, give me a gun at start:
        //theHub.state = theFunctions.implementALLEffects(premadeStuff.buyGun, theHub.state);


        //ad-hoc list of orders:
        //commandList.Add(premadeStuff.deepActionCopier(premadeStuff.resource1Dropoff));

        swapDirection = true;
    }

    // Update is called once per frame
    void Update()
    {

        findNearZones();

        //update body ray variable:
        theSensorySystem.lookingRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        


        //check for mouse click:
        handleAnyClick();

        //handle other buttons the player has pressed:
        handleOtherButtons();

        ///*
        //debugging
        if (personOfInterest != null)
        {

            //AIHub2 NPChubScript2 = personOfInterest.GetComponent("AIHub2") as AIHub2;
            //if (NPChubScript2.toDoList.Count > 0)
            {
                //theFunctions.printPlan(NPChubScript2.toDoList);
            }
            
        }
        //*/
    }


    public void findNearZones()
    {

    }

    public void handleAnyClick()
    {

        //should just have the logic structure [if statements], and thencall other functions where the details are

        if (Input.GetMouseButtonDown(0))
        {
            if(buildMode == true)
            {
                //ad-hoc:
                //Instantiate(myPrefab, new Vector3(5, 0, -11), Quaternion.identity);
                //Debug.Log("should be made");

                //only build if not in menu:
                if (inMenu == false)
                {
                    raycastBuildingPlacement();
                }
            }
            



            //only interact with world if we are NOT in menu [and not in build mode]:
            else if (inMenu == false)
            {
                if (weapon == false)
                {
                    //check for mouse click, see what it's "clicking on"
                    clickedOn = rayReturnFunction();

                    //now do stuff:
                    if (clickedOn != null)
                    {
                        doStuffAfterWorldClick(clickedOn);

                        //blank out mouse click each frame:
                        clickedOn = null;
                    }



                }
                else
                {
                    //weapon is drawn, so this click is a shooting action
                    simpleShoot();

                }
            }


        }
    }

    public void handleOtherButtons()
    {
        toggleBuildMode();
        toggleWeapon();
        debugMenu();
    }

    public void toggleBuildMode()
    {

        //[SerializeField]
        //private KeyCode newObjectHotkey = KeyCode.A;

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (buildMode == false)
            {
                //Debug.Log("1111111111111111111111111111111111111111111111111111111");
                buildMode = true;

                //now bring up menu to select what to build
                menuMode();
                createBuildMenu();
            }
            else
            {
                //Debug.Log("22222222222222222222222222222222222222222222222222222");
                buildMode = false;
            }
        }
    }

    public void toggleWeapon()
    {

        //[SerializeField]

        //"G" for "gun"....
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (weapon == false)
            {
                weapon = true;
                
            }
            else
            {
                weapon = false;
            }
            //gunLaserSight.GetComponent<Light>().spotAngle = 0.1f;
            gunLaserSight.GetComponent<Light>().enabled = weapon;
        }
    }

    public void debugMenu()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            menuMode();
            createDebugMenu();
        }
            
    }

    private void raycastBuildingPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        //Debug.Log(currentPrefab.name);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo) && currentPrefab != null)
        {
            //theNonAIScript.createBuildingX(theNonAIScript.storagePrefab, hitInfo.point);
        }
    }


    public GameObject rayReturnFunction()
    {
        //returns the object that was clicked on

        GameObject clickedOn;
        clickedOn = null;

        

        
        RaycastHit myHit;
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);  //  COULD use this ray to ALSO update the "body1" lookingRay.  if i want to be more equivalent to NPCs

        //insert "grab" action [from BODY] here




        //using same old code again?  why not?  well, needs ti be modified to have the "interactionType"
        if (Physics.Raycast(myRay, out myHit, 7.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (myHit.transform != null)
            {
                //Debug.Log(myHit.transform.gameObject);
                clickedOn = myHit.transform.gameObject;
                GameObject thisObject = createPrefabAtPointAndRETURN(theInteractionSphere, myHit.point);


                authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
                theAuthorScript.theAuthor = this.gameObject;
                theAuthorScript.interactionType = "standardClick";

            }
        }


        return clickedOn;







        
    }


    void createPrefabAtPoint(GameObject thePrefab, Vector3 thePoint)
    {
        //GameObject newBuilding = new GameObject();
        //newBuilding = Instantiate(thePrefab, thePoint, Quaternion.identity);
        Instantiate(thePrefab, thePoint, Quaternion.identity);
    }

    GameObject createPrefabAtPointAndRETURN(GameObject thePrefab, Vector3 thePoint)
    {
        //GameObject newBuilding = new GameObject();
        //newBuilding = Instantiate(thePrefab, thePoint, Quaternion.identity);
        //return Instantiate(thePrefab, thePoint, Quaternion.identity);
        return Instantiate(thePrefab, thePoint, thePrefab.transform.rotation);
    }


    public Ray rayFromPlayerCamera()
    {
        //can input into "Physics.Raycast" as if it is a Vector3 data type?  oookayyy
        //orrr, is that ONLY if you don't input an origin point!?!?
        //ya, seems like yet ANOTHER hidden version of the "raycast" funciton.  ffs unity.
        //Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        return Camera.main.ScreenPointToRay(Input.mousePosition); ;
    }

    public Vector3 vectorFromPlayerCamera()
    {
        return Camera.main.transform.forward;
    }

    public void simpleShoot()
    {
        //beginning shooting mechanic
        //hmm, Unity graphics are too glitchy, can't handle light placement, will do it later
        //theNonAIScript.raycastFromCameraPrefabPlacement(theNonAIScript.gunFlash1);

        //      [old shooting]
        //      theNonAIScript.basicFiringWithInnacuracy(vectorFromPlayerCamera()*40);

        //do “shoot1” enaction [body aim direction should already be updated]
        //body.theEnactionScript


        //AIhub2 has "makeSimpleEnactionMate" to do this.  should move it to enaction script

        //      enactionMate newEnactionMate = firingFlamethrowerEnactionMate();
        enactionMate newEnactionMate = firingRegularGunEnactionMate();
        //              enactionMate newEnactionMate = firingGlueGunEnactionMate();
        //enactionMate newEnactionMate = firingGlueGunEnactionMate();//firingFlamethrowerEnactionMate();// firingRegularGunEnactionMate();
        //newEnactionMate.enactionTarget = enactionTarget;

        newEnactionMate.enact();

        //blank out mouse click each frame:
        clickedOn = null;

    }

    public enactionMate firingRegularGunEnactionMate()
    {
        enactionMate newEnactionMate = new enactionMate();
        newEnactionMate.enactionAuthor = this.gameObject;
        //newEnactionMate.enactionBody = body;
        //authorSensorySystem
        newEnactionMate.authorSensorySystem = theSensorySystem;
        newEnactionMate.enactThis = "shoot1";

        return newEnactionMate;
    }

    public enactionMate firingFlamethrowerEnactionMate()
    {
        enactionMate newEnactionMate = new enactionMate();
        newEnactionMate.enactionAuthor = this.gameObject;
        //newEnactionMate.enactionBody = body;

        newEnactionMate.authorSensorySystem = theSensorySystem;
        newEnactionMate.enactThis = "shootFlamethrower1";

        return newEnactionMate;
    }

    public enactionMate firingGlueGunEnactionMate()
    {
        enactionMate newEnactionMate = new enactionMate();
        newEnactionMate.enactionAuthor = this.gameObject;
        //newEnactionMate.enactionBody = body;
        newEnactionMate.enactThis = "glueGun";

        newEnactionMate.authorSensorySystem = theSensorySystem;

        newEnactionMate.currentGlueStartPoint = currentGlueStartPoint;
        newEnactionMate.glueGunFirstShotDoneBool = glueGunFirstShotDoneBool;

        return newEnactionMate;
    }


    public void doStuffAfterWorldClick(GameObject clickedOn)
    {


        //Debug.Log(clickedOn.name);
        if (clickedOn.name == "workPlace")
        {
            //theFunctions.incrementItem(theHub.state["inventory"], premadeStuff.deepStateItemCopier(premadeStuff.money), 1);
        }

        if (clickedOn.tag == "anNPC")
        {
            //switch to menu
            menuMode();
            //save selected NPC to operate on:
            selectedNPC = GameObject.Find(clickedOn.name);

            //AD-HOC TEST BUTTON:
            makeButton("[AD-HOC TEST BUTTON]", this.adHocTestButton);


            //ad-hoc for now
            //so, NPC can either be a cashier or not
            //need to check (see if their "next action" has the .type = "work"
            //so, need to get their toDoList:
            AIHub2 targetAI = clickedOn.GetComponent("AIHub2") as AIHub2;

            //make the NPC know they are in a "conversation" state
            //(ad-hoc for now)

            //          ***************targetAI.inConversation = true;

            //FOR INVESTIGATING/TESTING:
            //targetAI.masterPrintControl = true;
            //targetAI.npcx = targetAI.gameObject.name;
            //theHub.npcx = this.gameObject.name;
            //Debug.Log("updated ''npcx''");


            //if (targetAI.toDoList.Count > 0)
            {
                //Debug.Log(targetAI.toDoList[0].name);
            }
            //else
            {
                //Debug.Log("they have no items on their toDoList");
            }




            //check if they are working at the store:
            //if (targetAI.toDoList.Count > 0 && (targetAI.toDoList[0].name == "workAsCashier" || targetAI.toDoList[0].name == "hireSomeone"))
            {
                //yes, they are a cashier
                //enable the "buy food" button

                //createStoreMenu();

            }
            //if they are your boss:
            //else if (theFunctions.isThisMyLeader(clickedOn))
            {

                //createBossMenu();
            }
            //else
            {
                //this is a regular free-roaming NPC
                //disable the "buy food" button

                //createRecruitmentButtonGrid();
            }

        }

        if (clickedOn.tag == "aProperty")
        {
            
            //if (theNonAIScript.TRYbuyingBuilding(clickedOn))
            {

                //haveStore = true;
                //theFunctions.print("you got a store!");
            }
            //else
            {
                //well, this one is NOT for sale, so 
                //theFunctions.print("maybe not for sale");

                //Debug.Log(clickedOn.transform.position);
            }

        }

        if (clickedOn.tag == "container")
        {
            //Debug.Log("this is a container");

            AIHub2 hubOfStorage = clickedOn.GetComponent("AIHub2") as AIHub2;

            //ad hoc.  if "swapDirection" = true, swap in one direction.  if false, swap in other direction:
            //[and then change the bool to its opposite]
            if (swapDirection == true)
            {
                swapDirection = false;
                //theFunctions.incrementTwoInventoriesFromActionEffects(theHub.state["inventory"], hubOfStorage.state["inventory"], premadeStuff.deepActionCopier(premadeStuff.pickVictimsPocket));
            }
            else
            {
                swapDirection = true;
                //theFunctions.incrementTwoInventoriesFromActionEffects(hubOfStorage.state["inventory"], theHub.state["inventory"], premadeStuff.deepActionCopier(premadeStuff.pickVictimsPocket));
            }


        }
    }



    ///////////////////////////////////////////////////////////
    //         Dynamic Menu Button Creation/Deletion
    ///////////////////////////////////////////////////////////

    public void menuMode()
    {
        //switch to menu
        //enable mouse:
        Cursor.lockState = CursorLockMode.None;
        //activate menu:
        recruitingMenu.SetActive(true);
        //set the "inMenu" state to "true":
        inMenu = true;
    }

    public void createRecruitmentButtonGrid()
    {

        makeButton("[pick their pockets]", this.pickTheirPocketsButton);

        makeButton("recruit to gang", this.recruitButton);

        //makeButton("regular ''bank'' work", this.makeBankWorkerButton);

        //makeButton("give gun", this.giftGunButton);

        //makeButton("order extortion", this.extortOrderButton);

        //makeButton("test button", this.testButton);

        //makeButton("Full Plan test button", this.aFullPlanTest);

        //makeButton("shopkeeper Plan test button", this.shopkeeperGivePlanTest);

        //ad hoc, should use same function...
        //see that documentation page about having inputs?
        //need to modify the "makeButton" function, because of strong typing
        //I guess will need a "makeButton1FuncInput" function?
        //makeButton("my side", () => this.announcePoliticalSideButton(-50));

        makeButton("increase trust", this.leftPoliticalSideButton);

        makeButton("decrease trust", this.rightPoliticalSideButton);

        
        


        //only if you own a store, menu will include a hiring option:
        //this is a "GameObject"???!!!?!?
        GameObject check = theTagScript.randomTaggedWithMultiple("shop", ownershipTag);
        
        if (check != null)
        {
            makeButton("hireCashier", this.hireCashierButton);
        }

        //makeButton("tag with X", this.tagWithX);

        //now stuff to do after you recruit someone:
        //---gather a resource
        //---attack enemy
        //---recruit others

        //need to check if person you are talking to is part of your faction.  simple for now, no checking for "rank" or roles...
        taggedWith foreignTagScript = selectedNPC.GetComponent<taggedWith>();

        //foreignTagScript.printAllTags();

        if (foreignTagScript.tags.Contains("PlayersGang"))
        {
            makeButton("hire as gatherer", this.hireResource1GathererButton);
            makeButton("hire as soldier", this.hireSoldierButton);
            

            //makeButton("bring me food", this.bringLeaderFoodButton);

            //generate command list:
            foreach (enactionMate availableCommand in commandList)
                {
                    //hmm, but commands won't always be "fetch"....whatever?  for now, ad hoc, can fix later if it's a problem...
                    //makeButton("bring me " + availableCommand.effects[0].name, () => this.fetchXButton(availableCommand));
                }

            makeButton("give gun", this.giftGunButton);
        }


    }

    public void createStoreMenu()
    {

        makeButton("buy food", this.tradeButton);

        makeButton("buy gun", this.buyGunButton);

        
    }

    public void createBossMenu()
    {
        //generate command completion list:
        //foreach (action order in theHub.inputtedToDoList)
        {
            //I'm making the buttons for the player to complete mission objectives, such as delivering an item to a leader,
            //should first Probably check pre - reqs  using a pre req checker
            //makeButton("give " + order.effects[0].name, () => this.giveXbutton(order));
        }

    }


    public void createBuildMenu()
    {

        //make these from a list later, ad hoc for now...
        
        //makeButton("build " + theNonAIScript.storePrefab.name, () => this.selectWhatToBuild(theNonAIScript.storePrefab));

        //makeButton("build " + theNonAIScript.storagePrefab.name, () => this.selectWhatToBuild(theNonAIScript.storagePrefab));
        


    }

    public void createDebugMenu()
    {
        makeButton("enable print", this.debugButton);
    }

    public void destroyObjectListItems(List<GameObject> theList)
    {
        //destroys each of the game objects that are on a particular list of GameObjects

        foreach(GameObject thisObject in theList)
        {
            Destroy(thisObject);
        }

        //will still have some kind of empty itmes occupying space in the list, have to clear:
        theList.Clear();
        //unclear (no pun intended) whether any other junk data piles up elsewhere when running this, creating and deleting countless GameObjects...?
    }

    public void makeButton(string name, Action function)
    {
        //hmm, tried to use "delegate" or "Action", not really working???

        //DYNAMICALLY CREATE RECRUIT BUTTON, AD-HOC FOR NOW
        GameObject myNewGameObject = Instantiate(myPrefabButton1) as GameObject;
        myNewGameObject.GetComponentInChildren<Text>().text = name;
        myNewGameObject.transform.SetParent(myGridCanvas.transform);
        //I pass in an "Action", C# delegate nonsense, but notice here I have to add brackets after it:
        myNewGameObject.GetComponent<Button>().onClick.AddListener(() => function());

        //add the button to a list, so I can elswhere easily delete it:
        currentGridButtons.Add(myNewGameObject);
    }


    ///////////////////////////////////////////////////////////
    //              Specific Button Functions
    ///////////////////////////////////////////////////////////

    

    public void pickTheirPocketsButton()
    {
        
        AIHub2 theTargetState = selectedNPC.GetComponent("AIHub2") as AIHub2;
        //note the "premadeStuff.pickVictimsPocket" input, this function is clunky like that for now...
        //theFunctions.incrementTwoInventoriesFromActionEffects(theHub.state["inventory"], theTargetState.state["inventory"], premadeStuff.deepActionCopier(premadeStuff.pickVictimsPocket));
        //theFunctions.incrementTwoInventoriesFromActionEffects(theHub.state["inventory"], theTargetState.planningState["inventory"], premadeStuff.deepActionCopier(premadeStuff.pickVictimsPocket));


    }


    public void recruitButton()
    {
        //for now, they only join you if thier trust in you is over a certain amount
        //for now, just use default.  Haven't made a "relationship" yet

        //need to get the social script on them:
        //social theselectedNPCsSocialScript = selectedNPC.GetComponent("social") as social;

        //if (theselectedNPCsSocialScript.checkTrust(this.name) > 60)
        {
            //ok, recruitment suceeds
            //theSocialScript.succeedAtRecruitment(selectedNPC);

            //refreshRecruitmentMenu();
        }
        //else
        {
            //Debug.Log("I guess they don't trust you enough");
        }
    }

    public void adHocTestButton()
    {
        //for testing, ad-hoc

        //testing "hide from shooter"

        //moved to "gunShotSoundSensing()" in sensing


        List<GameObject> everyone = theTagScript.ALLTaggedWithMultiple("person");

        foreach(GameObject thisPerson in everyone)
        {
            Debug.Log(thisPerson.name);
            //need their AIHub2 script:
            //AIHub2 NPChubScript = selectedNPC.GetComponent("AIHub2") as AIHub2;
            AIHub2 NPChubScript = thisPerson.GetComponent("AIHub2") as AIHub2;

            //going to blank out their to-do list, and fill it with test "orders":
            //AD HOC, SHOULD NOT DO THIS?!?!?
            //NPChubScript.toDoList.Clear();

            //NPChubScript.threatObject = this.gameObject;

            //stateItem food = premadeStuff.food;
            //action generatedAction = premadeStuff.bringLeaderX(food);
            //          action actionToInput = premadeStuff.hideFromShooter;

            //NPChubScript.inputtedToDoList.Add(actionToInput);
        }
    }

    public void refreshRecruitmentMenu()
    {
        //see if just closing and opening menu in one frame works and looks fine:
        //closeRecruitmentMenu();

        //no, that does other unwanted stuff.  just pick the stuff i need:
        //Destroy the generated buttons:
        destroyObjectListItems(currentGridButtons);

        //now re-create menu:
        createRecruitmentButtonGrid();

        //seems to work fine :)
    }


    public void tradeButton()
    {
        //this is just for food?? need to make less ad-hoc


        AIHub2 theHubOfNPC = selectedNPC.GetComponent("AIHub2") as AIHub2;

        //GameObject shopInventory = theFunctions.getShopInventory(theFunctions.getCashierMapZoneOfStore(theHubOfNPC.currentJob.roleLocation));


        //very ad-hoc
        //I should instead be generataing these "buy" buttons at the store based on some INVENTORY that the store HAS.

        //WHY AM I CREATING A STATEITEM FROM SCRATCH??
        //oh well, at elast it's not shallow copied???
        //stateItem food1;
        //food1 = premadeStuff.stateItemCreator("food", "inventory");


        //action buyFoooooood;
        //buyFoooooood = premadeStuff.actionCreator("buyFood", "buyFromStore", premadeStuff.wantedPrereqsLister(premadeStuff.deepStateItemCopier(premadeStuff.money)), premadeStuff.UNwantedPrereqsLister(), premadeStuff.wantedEffectsLister(premadeStuff.deepStateItemCopier(food1)), premadeStuff.UNwantedEffectsLister(premadeStuff.deepStateItemCopier(premadeStuff.money)), 1, premadeStuff.checkout);



        //lol
        //if (theFunctions.TRYincrementInventoriesOfThisAndTargetFromEffects(shopInventory, buyFoooooood))
        {
            //print("purchased food");

        }
        //else
        {
            //lol
            //Debug.Log("can't buy!");
        }
    }

    public void testButton()
    {
        Debug.Log("well, THIS button works");
        //for now ad-hoc
        //just making any NPC into a NON-pickpocket when you click on them
        //give them the "workPLace" job instead
        //like hiring them to work at the tall "workPlace"


        //now add work action, remove pickpocket action:
        //hubScript.knownActions.Add(stateGrabber.doTheWork);
        //hubScript.knownActions.RemoveAll(y => y.name == "pickVictimsPocket");
    }

    public void hireCashierButton()
    {
        
        if (selectedNPC != null && selectedNPC.name != "NPC shopkeeper" && selectedNPC.name != "NPC shopkeeper (1)")
        {
            
            AIHub2 selectedAI = selectedNPC.GetComponent("AIHub2") as AIHub2;
            //if (selectedAI.jobSeeking == true)
            {
                //selectedAI.jobSeeking = false;

                //theSocialScript.changeRoles(selectedNPC, premadeStuff.deepActionCopier(premadeStuff.workAsCashier), premadeStuff.deepActionCopier(premadeStuff.doTheWork));




                //need the worker to show up at the correct store for their shift:
                //selectedAI.roleLocation = thisAI.roleLocation;
                
                //need cashierZone of the owned store [PROBABLY SHOULDN"T BE RANDOM, BUT USING IT FOR NOW]:
                //selectedAI.roleLocation = theTagScript.randomTaggedWithMultiple("shop", ownershipTag);

                //Increase the "clearance level" of the worker:
                //BIT ad-hoc.  Characters might have different clearance levels for different places/factions etc.  Right now I just have one.
                //selectedAI.clearanceLevel = 1;

                
            }
        }

    }

    public void hireResource1GathererButton()
    {
        //if(theSocialScript.hiring(selectedNPC, premadeStuff.resource1GatheringJob, "storage"))
        {
            //Debug.Log("hired..........");
        }
        //else
        {
            //Debug.Log("FAILED TO HIRE");
        }
    }

    public void hireSoldierButton()
    {
        //if (theSocialScript.hiring(selectedNPC, premadeStuff.soldierJob, "storage"))
        {
            //Debug.Log("hired..........");
        }
        //else
        {
            //Debug.Log("FAILED TO HIRE");
        }
    }

    public void fetchXButton(enactionMate availableCommand)
    {
        //theSocialScript.commandToDoFetchXAction(availableCommand, selectedNPC);

    }

    public void giveXbutton(enactionMate theAction)
    {
        //print(selectedNPC.name);
        //if(theFunctions.TRYincrementInventoriesOfThisAndTargetFromEffects(selectedNPC, theAction))
        {
            //success
            //now remove this action from to-do list...
            //theHub.inputtedToDoList.Remove(theAction);
        }

        
    }

    public void announcePoliticalSideButton(int yourSide)
    {
        //ad hoc
        //for now modify default trust

        //need to get the social script on them:
        //social theSocialScript = selectedNPC.GetComponent("social") as social;

        //check if you match and all that:
        //theSocialScript.trustBySide(this.name, yourSide);
    }
    

    public void leftPoliticalSideButton()
    {
        announcePoliticalSideButton(-50);
    }

    public void rightPoliticalSideButton()
    {
        //ad hoc
        announcePoliticalSideButton(50);
    }


    public void buyGunButton()
    {
        //oh right, don't want to do this for now, don't want to put anything into cashier's inventory...
        //theFunctions.trade(theHub.state["inventory"], inventory2, premadeStuff.buyFood);

        //for now:
        //check if I have money:
        bool haveMoney = false;

        //foreach (stateItem thisItem in theHub.state["inventory"])
        {
            //if (thisItem.name == "money")
            {
                //haveMoney = true;
                //break;
            }
        }

        //if (haveMoney)
        {

            //theHub.state = theFunctions.implementALLEffects(premadeStuff.buyGun, theHub.state);
        }
        //else
        {
            //Debug.Log("no money!");
        }
    }

    public void giftGunButton()
    {
        //need to get the NPC inventory:
        //AIHub2 hubOfNPC = selectedNPC.GetComponent("AIHub2") as AIHub2;

        //theFunctions.incrementTwoInventoriesFromActionEffects(theHub.state["inventory"], hubOfNPC.state["inventory"], premadeStuff.deepActionCopier(premadeStuff.giftGun));
        
    }

    public void selectWhatToBuild(GameObject thisPrefab)
    {
        currentPrefab = thisPrefab;
    }

    ///////////////////////////////////////////////////////////
    //          Grabbing and using foreign scripts
    ///////////////////////////////////////////////////////////


    

    //////////////////////////////  Misc diagnostic functions  ///////////////////////////////
    public void print(string text)
    {
        Debug.Log(text);
    }

    public void debugButton()
    {
        //enable printing and update npcx for enemy leader
        //AIHub2 enemyLeaderAIScript = foreignGameObject.GetComponent<AIHub2>();
    }


    public class GroundPlacementController : MonoBehaviour
    {
        [SerializeField]
        private GameObject placeableObjectPrefab;
        
        [SerializeField]
        private KeyCode newObjectHotkey = KeyCode.A;

        private GameObject currentPlaceableObject;

        //private float mouseWheelRotation;

        private void Update()
        {
            HandleNewObjectHotkey();

            if (currentPlaceableObject != null)
            {
                MoveCurrentObjectToMouse();
                //RotateFromMouseWheel();
                ReleaseIfClicked();
            }
        }

        private void HandleNewObjectHotkey()
        {  //i think this is just to make it follow mouse around??? yes, partly, bad entangled code...
           //initializes the action i think, toggles it, i dunno...
           //should be called "togglePlacementMode" or something....
            if (Input.GetKeyDown(newObjectHotkey))
            {
                if (currentPlaceableObject != null)
                {
                    Destroy(currentPlaceableObject);
                }
                else
                {
                    currentPlaceableObject = Instantiate(placeableObjectPrefab);
                }
            }
        }

        private void MoveCurrentObjectToMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                currentPlaceableObject.transform.position = hitInfo.point;
                currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            }
        }

        /*
        private void RotateFromMouseWheel()
        {
            Debug.Log(Input.mouseScrollDelta);
            mouseWheelRotation += Input.mouseScrollDelta.y;
            currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
        }
        */

        private void ReleaseIfClicked()
        {
            //....the fuck is this shit?
            if (Input.GetMouseButtonDown(0))
            {
                currentPlaceableObject = null;
            }
        }
    }






    

}
