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

    //ad-hoc?
    GameObject selectedNPC;

    public string ownershipTag;

    //plug-in menu objects:
    public GameObject recruitingMenu;

    public GameObject myPrefabButton1;
    public GameObject myGridCanvas;
    public List<GameObject> currentGridButtons;


    //for debugging:
    public GameObject personOfInterest;


    public bool inMenu;
    public bool buildMode;

    public premadeStuffForAI premadeStuff;
    public AI1 theHub;
    public playerHUD myHUD;
    public functionsForAI theFunctions;


    public taggedWith theTagScript;


    //bit ad hoc:
    //can i use object class types from other scripts before loading in those scripts??? how to do???
    public List<action> commandList = new List<action>();


    //definitely ad-hoc:
    public bool haveStore;
    public bool weapon;
    public GameObject myPrefab;
    public GameObject currentPrefab;
    public bool swapDirection;

    // Start is called before the first frame update
    void Start()
    {

        


        haveStore = false;
        weapon = false;
        buildMode = false;

        ownershipTag = "owned by " + this.name;

        premadeStuff = GetComponent<premadeStuffForAI>();
        theHub = GetComponent<AI1>();
        myHUD = GetComponent<playerHUD>();
        inMenu = false;
        recruitingMenu.SetActive(false);

        theFunctions = GetComponent<functionsForAI>();

        theTagScript = GetComponent<taggedWith>();


        //just for testing, give me a gun at start:
        //theHub.state = theFunctions.implementALLEffects(premadeStuff.buyGun, theHub.state);


        //ad-hoc list of orders:
        commandList.Add(premadeStuff.bringLeaderX(premadeStuff.deepStateItemCopier(premadeStuff.food)));
        commandList.Add(premadeStuff.bringLeaderX(premadeStuff.deepStateItemCopier(premadeStuff.money)));
        commandList.Add(premadeStuff.bringLeaderX(premadeStuff.deepStateItemCopier(premadeStuff.resource1)));
        commandList.Add(premadeStuff.deepActionCopier(premadeStuff.resource1Dropoff));

        swapDirection = true;
    }

    // Update is called once per frame
    void Update()
    {

        //check for click:
        handleAnyClick();

        //handle other butons:
        toggleBuildMode();

        ///*
        //debugging
        if (personOfInterest != null)
        {

            AI1 NPChubScript2 = personOfInterest.GetComponent("AI1") as AI1;
            if (NPChubScript2.toDoList.Count > 0)
            {
                theFunctions.printPlan(NPChubScript2.toDoList);
            }
            
        }
        //*/
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

    public void toggleBuildMode()
    {
        
        //[SerializeField]
        //private KeyCode newObjectHotkey = KeyCode.A;

    if (Input.GetKeyDown(KeyCode.B))
        {
            //Debug.Log("222222222222222222222");
            if (buildMode == false)
            {
                //Debug.Log("build mode activated");
                buildMode = true;

                //now bring up menu to select what to build
                menuMode();
                createBuildMenu();
            }
            else
            {
                //Debug.Log("build mode de-activated");
                buildMode = false;
            }
        }
}

    //public void buildModeVisualCue()

    private void raycastBuildingPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo) && currentPrefab != null)
        {
            GameObject x = new GameObject();
            x = Instantiate(currentPrefab, hitInfo.point, Quaternion.identity);
            //myPrefab.transform.position = hitInfo.point;
            //myPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }


    public GameObject rayReturnFunction()
    {
        //returns the object that was clicked on

        GameObject clickedOn;
        clickedOn = null;

        

        
        RaycastHit myHit;
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(myRay, out myHit, 7.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (myHit.transform != null)
            {
                //Debug.Log(myHit.transform.gameObject);
                clickedOn = myHit.transform.gameObject;
            }
        }
        

        return clickedOn;
    }

    public GameObject whatDoesBulletHit()
    {
        //returns the object that was clicked on

        GameObject clickedOn;
        clickedOn = null;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit myHit;
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(myRay, out myHit, 50.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if (myHit.transform != null)
                {
                    //Debug.Log(myHit.transform.gameObject);
                    clickedOn = myHit.transform.gameObject;
                }
            }
        }

        return clickedOn;
    }

    public void simpleShoot()
    {
        //beginning shooting mechanic

        clickedOn = whatDoesBulletHit();
        if (clickedOn != null)
        {
            if (clickedOn.tag == "anNPC")
            {
                Debug.Log("you just shot " + clickedOn.name);
                Debug.Log("object to be destroyed ");
                Debug.Log(clickedOn);
                Destroy(clickedOn);


            }

            //blank out mouse click each frame:
            clickedOn = null;
        }

       

    }

    public void doStuffAfterWorldClick(GameObject clickedOn)
    {


        //Debug.Log(clickedOn.name);
        if (clickedOn.name == "workPlace")
        {
            //Debug.Log("MONEY$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");

            //theHub.state["inventory"].Add(premadeStuff.deepStateItemCopier(premadeStuff.money));
            theFunctions.incrementItem(theHub.state["inventory"], premadeStuff.deepStateItemCopier(premadeStuff.money), 1);
        }

        if (clickedOn.tag == "anNPC")
        {
            //switch to menu
            menuMode();
            //save selected NPC to operate on:
            selectedNPC = GameObject.Find(clickedOn.name);




            //ad-hoc for now
            //so, NPC can either be a cashier or not
            //need to check (see if their "next action" has the .type = "work"
            //so, need to get their toDoList:
            AI1 targetAI = clickedOn.GetComponent("AI1") as AI1;

            //make the NPC know they are in a "conversation" state
            //(ad-hoc for now)
            targetAI.inConversation = true;

            //FOR INVESTIGATING/TESTING:
            targetAI.masterPrintControl = true;
            targetAI.npcx = targetAI.gameObject.name;
            Debug.Log("updated ''npcx''");

            //targetAI.state["threatState"].Add(premadeStuff.threat);
            //targetAI.planningState["threatState"].Add(premadeStuff.threat);
            if (targetAI.toDoList.Count > 0)
            {
                //Debug.Log(targetAI.toDoList[0].name);
            }
            else
            {
                Debug.Log("they have no items on their toDoList");
            }

            //targetAI.theFunctions.printState(targetAI.state);
            //targetAI.theFunctions.printState(targetAI.planningState);

            

            //targetAI.masterPrintControl = false;

            //check if they are working at the store:
            if (targetAI.toDoList.Count > 0 && (targetAI.toDoList[0].name == "workAsCashier" || targetAI.toDoList[0].name == "hireSomeone"))
            {
                //yes, they are a cashier
                //enable the "buy food" button
                Debug.Log("worker");
                createStoreMenu();

            }
            //if they are your boss:
            else if (theFunctions.isThisMyLeader(clickedOn))
            {
                //print(clickedOn.name);
                createBossMenu();
            }
            else
            {
                //this is a regular free-roaming NPC
                //disable the "buy food" button
                createRecruitmentButtonGrid();
                Debug.Log("regular free-roaming NPC");
            }

            

            /*
            //for now ad-hoc
            //just making any NPC into a pickpocket when you click on them

            //need their AI1 script:
            AI1 hubScript = clickedOn.GetComponent("AI1") as AI1;

            //need the action to add:
            premadeStuffForAI stateGrabber = GetComponent<premadeStuffForAI>();


            //now add pickpocket action, remove work action:
            hubScript.knownActions.Add(stateGrabber.pickVictimsPocket);
            hubScript.knownActions.RemoveAll(y => y.type == "work");
            */

        }

        if (clickedOn.tag == "aProperty")
        {
            //check if it's for sale:
            //get other script I need:
            taggedWith otherIsTaggedWith = clickedOn.GetComponent<taggedWith>() as taggedWith;
            if (otherIsTaggedWith.tags.Contains("forSale"))
            {
                //ok, it's for sale, now can buy it

                //printTextList(otherIsTaggedWith.tags);
                //remove the "for sale" tag:
                otherIsTaggedWith.foreignRemoveTag("forSale", clickedOn);
                //printTextList(otherIsTaggedWith.tags);
                //add the "owned by _____" tag...:
                string ownershipTag = "owned by " + this.name;
                otherIsTaggedWith.foreignAddTag(ownershipTag, clickedOn);

                //need to remember in the future WHICH store is theirs
                //so they ca go to it, and sned their employees there:
                //thisAI.roleLocation = target;

                //ad-hoc action completion:
                //thisAI.toDoList.RemoveAt(0);


                //ad-hoc update of state:
                //state = implementALLEffects(nextAction, state);

                haveStore = true;
                theFunctions.print("you got a store!");

                //Debug.Log(clickedOn.transform.position);

            }


            else
            {
                //well, this one is NOT for sale, so 
                theFunctions.print("not for sale");

                //Debug.Log(clickedOn.transform.position);
            }

        }

        if (clickedOn.tag == "container")
        {
            //Debug.Log("this is a container");

            //------from workPlace:
            //theFunctions.incrementItem(theHub.state["inventory"], premadeStuff.deepStateItemCopier(premadeStuff.money), 1);

            //------from pickpocketing:
            //AI1 theTargetState = selectedNPC.GetComponent("AI1") as AI1;
            //note the "premadeStuff.pickVictimsPocket" input, this function is clunky like that for now...
            //theFunctions.incrementTwoInventoriesFromActionEffects(theHub.state["inventory"], theTargetState.state["inventory"], premadeStuff.deepActionCopier(premadeStuff.pickVictimsPocket));

            //------from giftgun:
            //AI1 hubOfNPC = selectedNPC.GetComponent("AI1") as AI1;
            //theFunctions.incrementTwoInventoriesFromActionEffects(theHub.state["inventory"], hubOfNPC.state["inventory"], premadeStuff.deepActionCopier(premadeStuff.giftGun));

            AI1 hubOfStorage = clickedOn.GetComponent("AI1") as AI1;

            //ad hoc.  if "swapDirection" = true, swap in one direction.  if false, swap in other direction:
            //[and then change the bool to its opposite]
            if (swapDirection == true)
            {
                swapDirection = false;
                theFunctions.incrementTwoInventoriesFromActionEffects(theHub.state["inventory"], hubOfStorage.state["inventory"], premadeStuff.deepActionCopier(premadeStuff.pickVictimsPocket));
            }
            else
            {
                swapDirection = true;
                theFunctions.incrementTwoInventoriesFromActionEffects(hubOfStorage.state["inventory"], theHub.state["inventory"], premadeStuff.deepActionCopier(premadeStuff.pickVictimsPocket));
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

        //makeButton("regular work", this.makeRegularWorkerButton);

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
        GameObject check = theFunctions.randomTaggedWithMultiple("shop", ownershipTag);
        
        if (check != null)
        {
            theFunctions.print("let's see::::::::::::::::::");
            theFunctions.print(ownershipTag);
            theFunctions.print(check.name);
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

            

            //makeButton("bring me food", this.bringLeaderFoodButton);

            //generate command list:
            foreach (action availableCommand in commandList)
                {
                    //hmm, but commands won't always be "fetch"....whatever?  for now, ad hoc, can fix later if it's a problem...
                    makeButton("bring me " + availableCommand.effects[0].name, () => this.fetchXButton(availableCommand));
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
        Debug.Log("yessssssss!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //makeButton("buy food", this.tradeButton);

        //makeButton("buy gun", this.buyGunButton);

        //generate command completion list:
        //print(clickedOn.name);
        foreach (action order in theHub.inputtedToDoList)
        {
            //I'm making the buttons for the player to complete mission objectives, such as delivering an item to a leader,
            //should first Probably check pre - reqs  using a pre req checker
            makeButton("give " + order.effects[0].name, () => this.giveXbutton(order));
        }

    }


    public void createBuildMenu()
    {

        //make these from a list later, ad hoc for now...
        
        makeButton("build " + myPrefab.name, () => this.selectWhatToBuild(myPrefab));

        makeButton("build " + premadeStuff.storagePrefab.name, () => this.selectWhatToBuild(premadeStuff.storagePrefab));
        


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

    public void aFullPlanTest()
    {
        List<action> planToGive = new List<action>();
        planToGive.Add(premadeStuff.deepActionCopier(premadeStuff.doTheWork));
        //planToGive.Add(premadeStuff.buyFood);  //but I generate buy actions now???
        //planToGive.Add(premadeStuff.buyHome);
        planToGive.Add(premadeStuff.deepActionCopier(premadeStuff.eat));

        //now tell them the plan:
        //need to get the social script on them:
        social theSocialScript = selectedNPC.GetComponent("social") as social;
        theSocialScript.toldPlan(planToGive);

        personOfInterest = selectedNPC;
    }

    public void shopkeeperGivePlanTest()
    {
        List<action> planToGive = new List<action>();
        planToGive.Add(premadeStuff.deepActionCopier(premadeStuff.buyShop));
        //planToGive.Add(premadeStuff.buyFood);  //but I generate buy actions now???
        //planToGive.Add(premadeStuff.buyHome);
        planToGive.Add(premadeStuff.deepActionCopier(premadeStuff.hireSomeone));
        planToGive.Add(premadeStuff.deepActionCopier(premadeStuff.beBoss));
        

        //now tell them the plan:
        //need to get the social script on them:
        social theSocialScript = selectedNPC.GetComponent("social") as social;
        theSocialScript.toldPlan(planToGive);

        personOfInterest = selectedNPC;
    }


    public void pickTheirPocketsButton()
    {
        
        AI1 theTargetState = selectedNPC.GetComponent("AI1") as AI1;
        //note the "premadeStuff.pickVictimsPocket" input, this function is clunky like that for now...
        theFunctions.incrementTwoInventoriesFromActionEffects(theHub.state["inventory"], theTargetState.state["inventory"], premadeStuff.deepActionCopier(premadeStuff.pickVictimsPocket));
        theFunctions.incrementTwoInventoriesFromActionEffects(theHub.state["inventory"], theTargetState.planningState["inventory"], premadeStuff.deepActionCopier(premadeStuff.pickVictimsPocket));


    }


    public void recruitButton()
    {
        //Debug.Log("heloooooooooo");

        //for now, they only join you if thier trust in you is over a certain amount
        //for now, just use default.  Haven't made a "relationship" yet

        //need to get the social script on them:
        social theSocialScript = selectedNPC.GetComponent("social") as social;

        if (theSocialScript.checkTrust(this.name) > 60)
        {
            //ok, recruitment suceeds
            theFunctions.succeedAtRecruitment(selectedNPC);
            /*
            Debug.Log("recruitment successful");


            //for now ad-hoc
            //just tagging an NPC with "playersGang" when you click it
            taggedWith foreignTagScript = selectedNPC.GetComponent<taggedWith>();

            foreignTagScript.foreignAddTag("playersGang", selectedNPC);

            //Debug.Log("should be recruited to gang now, by tagging");

            //but they need to be able to FIND me, their leader, to deliver money to me
            //so, for now, fill their leader variable:
            //need their AI1 script:
            AI1 NPChubScript = selectedNPC.GetComponent("AI1") as AI1;
            NPChubScript.leader = this.gameObject;

            //ALSO NEED TO BLANK-OUT THEIR TARGET!!!
            NPChubScript.target = null;

            */
            refreshRecruitmentMenu();

            //personOfInterest = selectedNPC;
        }
        else
        {
            Debug.Log("I guess they don't trust you enough");
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


    public void closeRecruitmentMenu()
    {
        //if targeting an NPC [or something else?]
        if(selectedNPC != null)
        {
            //set the NPC back to be able to move:
            Debug.Log(selectedNPC.name);
            AI1 targetAI = selectedNPC.GetComponent("AI1") as AI1;
            targetAI.inConversation = false;
            //de-select NPC:
            selectedNPC = null;

            //Debug.Log(targetAI.inConversation);
        }


        //Close recruitment menu
        //lock mouse:
        Cursor.lockState = CursorLockMode.Locked;
        //de-activate menu:
        recruitingMenu.SetActive(false);
        
        //set the "inMenu" state to "false":
        inMenu = false;
        //Destroy the generated buttons:
        destroyObjectListItems(currentGridButtons);
        //Debug.Log(currentGridButtons.Count);


    }

    public void tradeButton()
    {
        //this is just for food?? need to make less ad-hoc


        //GameObject customerLocation = target;
        
        //print("target name is:::::::::::::::::::::::::::::");
        //print(target.name);

        //GameObject cashierMapZone = getCashierMapZone(customerLocation);

        //GameObject cashier = whoIsTrader(cashierMapZone);



        //ad-hoc update of state:
        //state = implementALLEffects(nextAction, state);

        AI1 theHubOfNPC = selectedNPC.GetComponent("AI1") as AI1;
        //print("location:========================================");
        //print(theHubOfNPC.currentJob.roleLocation.name);  //gives name of whole/base/parent part of store
        //SO,theFunctions.getShopInventory(theFunctions.getCashierMapZoneOfStore(theHubOfNPC.currentJob.roleLocation));

        GameObject shopInventory = theFunctions.getShopInventory(theFunctions.getCashierMapZoneOfStore(theHubOfNPC.currentJob.roleLocation));


        //very ad-hoc
        //I should instead be generataing these "buy" buttons at the store based on some INVENTORY that the store HAS.

        //WHY AM I CREATING A STATEITEM FROM SCRATCH??
        //oh well, at elast it's not shallow copied???
        stateItem food1;
        food1 = premadeStuff.stateItemCreator("food", "inventory");

        //actionItem fooood;
        //fooood = premadeStuff.convertToActionItem(food1, 1);

        action buyFoooooood;
        buyFoooooood = premadeStuff.actionCreator("buyFood", "buyFromStore", premadeStuff.wantedPrereqsLister(premadeStuff.deepStateItemCopier(premadeStuff.money)), premadeStuff.UNwantedPrereqsLister(), premadeStuff.wantedEffectsLister(premadeStuff.deepStateItemCopier(food1)), premadeStuff.UNwantedEffectsLister(premadeStuff.deepStateItemCopier(premadeStuff.money)), 1, premadeStuff.checkout);



        //lol
        if (theFunctions.TRYincrementInventoriesOfThisAndTargetFromEffects(shopInventory, buyFoooooood))
        {
            print("purchased food");

        }
        else
        {
            //lol
            Debug.Log("can't buy!");
        }


        /*
        //oh right, don't want to do this for now, don't want to put anything into cashier's inventory...
        //theFunctions.trade(theHub.state["inventory"], inventory2, premadeStuff.buyFood);

        //for now:
        //check if I have money:
        bool haveMoney = false;

        foreach (stateItem thisItem in theHub.state["inventory"])
        {
            if (thisItem.name == "money")
            {
                haveMoney = true;
                break;
            }
        }

        if (haveMoney)
        {
            //very ad-hoc
            //I should instead be generataing these "buy" buttons at the store based on some INVENTORY that the store HAS.

            //WHY AM I CREATING A STATEITEM FROM SCRATCH??
            //oh well, at elast it's not shallow copied???
            stateItem food1;
            food1 = premadeStuff.stateItemCreator("food", "inventory");

            //actionItem fooood;
            //fooood = premadeStuff.convertToActionItem(food1, 1);

            action buyFoooooood;
            buyFoooooood = premadeStuff.actionCreator("buyFood", "buyFromStore", premadeStuff.wantedPrereqsLister(premadeStuff.deepStateItemCopier(premadeStuff.money)), premadeStuff.UNwantedPrereqsLister(), premadeStuff.wantedEffectsLister(premadeStuff.deepStateItemCopier(food1)), premadeStuff.UNwantedEffectsLister(premadeStuff.deepStateItemCopier(premadeStuff.money)), 1, premadeStuff.checkout);

            theHub.state = theFunctions.implementALLEffects(buyFoooooood, theHub.state);
        }
        else
        {
            Debug.Log("no money!");
        }

        */

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

    public void makeRegularWorkerButton()
    {
        Debug.Log("well, THIS button works");
        //for now ad-hoc
        //just making any NPC into a NON-pickpocket when you click on them
        //give them the "workPLace" job instead
        //like hiring them to work at the tall "workPlace"

        //need their AI1 script:
        AI1 hubScript = selectedNPC.GetComponent("AI1") as AI1;

        //need the action to add:
        premadeStuffForAI stateGrabber = GetComponent<premadeStuffForAI>();


        //now add work action, remove pickpocket action:
        hubScript.knownActions.Add(stateGrabber.doTheWork);
        hubScript.knownActions.RemoveAll(y => y.name == "pickVictimsPocket");
    }

    public void recruitPickpocketButton()
    {
        //Debug.Log("heloooooooooo");

        //for now ad-hoc
        //just making any NPC into a pickpocket when you click on them

        //need their AI1 script:
        AI1 hubScript = selectedNPC.GetComponent("AI1") as AI1;

        //need the action to add:
        premadeStuffForAI stateGrabber = GetComponent<premadeStuffForAI>();


        //now add pickpocket action, remove work action:
        hubScript.knownActions.Add(stateGrabber.pickVictimsPocket);
        hubScript.knownActions.RemoveAll(y => y.type == "work");
    }

    public void extortOrderButton()
    {
        //Debug.Log("heloooooooooo");

        //for now ad-hoc
        //just making any NPC go extort the shop...if they have a gun
        

        //need their AI1 script:
        AI1 NPChubScript = selectedNPC.GetComponent("AI1") as AI1;

        //need the action to add:
        premadeStuffForAI stateGrabber = GetComponent<premadeStuffForAI>();


        //going to blank out their to-do list, and fill it with test "orders":
        NPChubScript.toDoList.Clear();

        NPChubScript.toDoList.Add(stateGrabber.extort);
        NPChubScript.toDoList.Add(stateGrabber.giveMoneyToLeader);
        //NPChubScript.toDoList.Add(stateGrabber.doTheWork);
        //NPChubScript.toDoList.Add(stateGrabber.pickVictimsPocket);
        //NPChubScript.toDoList.Add(stateGrabber.handleSecurityMild);
        //NPChubScript.toDoList.Add(stateGrabber.handleSecurityEscalationOne);


        //now add pickpocket action, remove work action:
        //hubScript.knownActions.Add(stateGrabber.extort);
        //hubScript.knownActions.RemoveAll(y => y.type == "work");
    }

    public void bringLeaderFoodButton()
    {
        //for testing

        //for now ad-hoc
        //just making any NPC go extort the shop...if they have a gun


        //need their AI1 script:
        AI1 NPChubScript = selectedNPC.GetComponent("AI1") as AI1;

        //need the action to add:
        premadeStuffForAI stateGrabber = GetComponent<premadeStuffForAI>();


        //going to blank out their to-do list, and fill it with test "orders":
        //AD HOC, SHOULD NOT DO THIS?!?!?
        NPChubScript.toDoList.Clear();

        stateItem food = stateGrabber.food;
        action generatedAction = stateGrabber.bringLeaderX(food);

        NPChubScript.inputtedToDoList.Add(generatedAction);



        //now generate planlist for it:
        //need their functionsForAI script:
        //functionsForAI NPCfunctionsForAIScript = selectedNPC.GetComponent("functionsForAI") as functionsForAI;
        //actually no, i already have AI1 hub, which can call all the functions i need, i think:
        //NPChubScript.planList = NPChubScript.theFunctions.prereqFiller(generatedAction, NPChubScript.knownActions, NPChubScript.state);
        //and AI1 script will do the work of choosing the first one.

        //NPChubScript.toDoList.Add(stateGrabber.extort);
        //but.....they need to buy food first...and will need to PLAN for it...
        //NPChubScript.toDoList.Add(bringFood);

    }

    public void hireCashierButton()
    {
        
        if (selectedNPC != null && selectedNPC.name != "NPC shopkeeper" && selectedNPC.name != "NPC shopkeeper (1)")
        {
            
            AI1 selectedAI = selectedNPC.GetComponent("AI1") as AI1;
            if (selectedAI.jobSeeking == true)
            {
                selectedAI.jobSeeking = false;

                //listOfCashiers.Add(customer);
                theFunctions.changeRoles(selectedNPC, premadeStuff.deepActionCopier(premadeStuff.workAsCashier), premadeStuff.deepActionCopier(premadeStuff.doTheWork));

                print(selectedNPC.name);


                //workerCount += 1;

                //print(workerCount);

                //need the worker to show up at the correct store for their shift:
                //selectedAI.roleLocation = thisAI.roleLocation;
                
                //need cashierZone of the owned store [PROBABLY SHOULDN"T BE RANDOM, BUT USING IT FOR NOW]:
                selectedAI.roleLocation = theFunctions.randomTaggedWithMultiple("shop", ownershipTag);

                //Increase the "clearance level" of the worker:
                //BIT ad-hoc.  Characters might have different clearance levels for different places/factions etc.  Right now I just have one.
                selectedAI.clearanceLevel = 1;

                
            }
        }

    }

    public void hireResource1GathererButton()
    {
        if(theFunctions.hiring(selectedNPC, premadeStuff.resource1GatheringJob, "storage"))
        {
            Debug.Log("hired..........");
        }
        else
        {
            Debug.Log("FAILED TO HIRE");
        }
    }

    public void fetchXButton(action availableCommand)
    {
        theFunctions.commandToDoFetchXAction(availableCommand, selectedNPC);

        /*

        //need their AI1 script:
        AI1 NPChubScript = selectedNPC.GetComponent("AI1") as AI1;
        
        //going to blank out their to-do list, and fill it with test "orders":
        //AD HOC, SHOULD NOT DO THIS?!?!?
        NPChubScript.toDoList.Clear();
        

        NPChubScript.inputtedToDoList.Add(availableCommand);
        */
    }

    public void giveXbutton(action theAction)
    {
        //print(selectedNPC.name);
        if(theFunctions.TRYincrementInventoriesOfThisAndTargetFromEffects(selectedNPC, theAction))
        {
            //success
            //now remove this action from to-do list...
            theHub.inputtedToDoList.Remove(theAction);
        }

        
    }

    public void announcePoliticalSideButton(int yourSide)
    {
        //ad hoc
        //for now modify default trust

        //need to get the social script on them:
        social theSocialScript = selectedNPC.GetComponent("social") as social;

        //check if you match and all that:
        theSocialScript.trustBySide(this.name, yourSide);
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

        foreach (stateItem thisItem in theHub.state["inventory"])
        {
            if (thisItem.name == "money")
            {
                haveMoney = true;
                break;
            }
        }

        if (haveMoney)
        {

            theHub.state = theFunctions.implementALLEffects(premadeStuff.buyGun, theHub.state);
        }
        else
        {
            Debug.Log("no money!");
        }
    }

    public void giftGunButton()
    {
        //need to get the NPC inventory:
        AI1 hubOfNPC = selectedNPC.GetComponent("AI1") as AI1;

        theFunctions.incrementTwoInventoriesFromActionEffects(theHub.state["inventory"], hubOfNPC.state["inventory"], premadeStuff.deepActionCopier(premadeStuff.giftGun));
        //theHub.state = theFunctions.implementALLEffects(premadeStuff.buyGun, theHub.state);
        Debug.Log("gave gun?");
    }

    public void selectWhatToBuild(GameObject thisPrefab)
    {
        currentPrefab = thisPrefab;
    }

    ///////////////////////////////////////////////////////////
    //          Grabbing and using foreign scripts
    ///////////////////////////////////////////////////////////


    public social grabSocialScript()
    {
        social theSocialScript = selectedNPC.GetComponent("social") as social;
        return theSocialScript;
    }



    ////////////////////////////////////////////////
    //         Misc diagnostic functions
    ////////////////////////////////////////////////
    ///
    public void print(string text)
    {
        Debug.Log(text);
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
