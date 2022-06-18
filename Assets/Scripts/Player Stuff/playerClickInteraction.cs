using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class playerClickInteraction : MonoBehaviour
{
    GameObject clickedOn;

    //ad-hoc?
    GameObject selectedNPC;

    //plug-in menu objects:
    public GameObject recruitingMenu;

    public GameObject myPrefabButton1;
    public GameObject myGridCanvas;
    public List<GameObject> currentGridButtons;


    //for debugging:
    public GameObject personOfInterest;


    public bool inMenu;

    public premadeStuffForAI premadeStuff;
    public AI1 theHub;
    public playerHUD myHUD;
    public functionsForAI theFunctions;


    public taggedWith theTagScript;

    // Start is called before the first frame update
    void Start()
    {

        


        premadeStuff = GetComponent<premadeStuffForAI>();
        theHub = GetComponent<AI1>();
        myHUD = GetComponent<playerHUD>();
        inMenu = false;
        recruitingMenu.SetActive(false);

        theFunctions = GetComponent<functionsForAI>();

        theTagScript = GetComponent<taggedWith>();


        //just for testing, give me a gun at start:
        theHub.state = theFunctions.implementALLEffects(premadeStuff.buyGun, theHub.state);


    }

    // Update is called once per frame
    void Update()
    {
        //only interact with world if we are NOT in menu:
        if(inMenu == false)
        {
            //check for mouse click, see what it's "clicking on"
            clickedOn = clickingFunction();

            //now do stuff:
            if (clickedOn != null)
            {

                doStuffAfterWorldClick(clickedOn);

                //blank out mouse click each frame:
                clickedOn = null;
            }
        }

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




    public GameObject clickingFunction()
    {
        GameObject clickedOn;
        clickedOn = null;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit myHit;
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(myRay, out myHit, 3.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
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

    public void doStuffAfterWorldClick(GameObject clickedOn)
    {


        //Debug.Log(clickedOn.name);
        if (clickedOn.name == "workPlace")
        {
            //Debug.Log("MONEY$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");

            theHub.state["inventory"].Add(premadeStuff.money);
        }

        if (clickedOn.tag == "anNPC")
        {
            //switch to menu
            //Switch to recruitment menu
            //enable mouse:
            Cursor.lockState = CursorLockMode.None;
            //activate menu:
            recruitingMenu.SetActive(true);
            //save selected NPC to operate on:
            selectedNPC = GameObject.Find(clickedOn.name);
            //set the "inMenu" state to "true":
            inMenu = true;




            //ad-hoc for now
            //so, NPC can either be a cashier or not
            //need to check (see if their "next action" has the .type = "work"
            //so, need to get their toDoList:
            AI1 targetAI = clickedOn.GetComponent("AI1") as AI1;

            //make the NPC know they are in a "conversation" state
            //(ad-hoc for now)
            targetAI.inConversation = true;

            //check if they are working at the store:
            if (targetAI.toDoList.Count > 0 && (targetAI.toDoList[0].name == "workAsCashier" || targetAI.toDoList[0].name == "hireSomeone"))
            {
                //yes, they are a cashier
                //enable the "buy food" button
                Debug.Log("worker");
                createStoreMenu();

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

    }

    

    ///////////////////////////////////////////////////////////
    //         Dynamic Menu Button Creation/Deletion
    ///////////////////////////////////////////////////////////
    
    public void createRecruitmentButtonGrid()
    {

        //makeButton("recruit to gang", this.recruitButton);

        //makeButton("regular work", this.makeRegularWorkerButton);

        //makeButton("give gun", this.giftGunButton);

        //makeButton("order extortion", this.extortOrderButton);

        //makeButton("test button", this.testButton);

        makeButton("Full Plan test button", this.aFullPlanTest);

        makeButton("shopkeeper Plan test button", this.shopkeeperGivePlanTest);

        //ad hoc, should use same function...
        //see that documentation page about having inputs?
        //need to modify the "makeButton" function, because of strong typing
        //I guess will need a "makeButton1FuncInput" function?
        makeButton("i'm left wing", this.leftPoliticalSideButton);

        makeButton("i'm right wing", this.rightPoliticalSideButton);

    }

    public void createStoreMenu()
    {

        makeButton("buy food", this.tradeButton);

        makeButton("buy gun", this.buyGunButton);

        
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
        planToGive.Add(premadeStuff.doTheWork);
        //planToGive.Add(premadeStuff.buyFood);  //but I generate buy actions now???
        //planToGive.Add(premadeStuff.buyHome);
        planToGive.Add(premadeStuff.eat);

        //now tell them the plan:
        //need to get the social script on them:
        social theSocialScript = selectedNPC.GetComponent("social") as social;
        theSocialScript.toldPlan(planToGive);

        personOfInterest = selectedNPC;
    }

    public void shopkeeperGivePlanTest()
    {
        List<action> planToGive = new List<action>();
        planToGive.Add(premadeStuff.buyShop);
        //planToGive.Add(premadeStuff.buyFood);  //but I generate buy actions now???
        //planToGive.Add(premadeStuff.buyHome);
        planToGive.Add(premadeStuff.hireSomeone);
        planToGive.Add(premadeStuff.beBoss);
        

        //now tell them the plan:
        //need to get the social script on them:
        social theSocialScript = selectedNPC.GetComponent("social") as social;
        theSocialScript.toldPlan(planToGive);

        personOfInterest = selectedNPC;
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
            Debug.Log("recruitment successful");

            //for now ad-hoc
            //just tagging an NPC with "playersGang" when you click it
            theTagScript.foreignAddTag("playersGang", selectedNPC);

            //Debug.Log("should be recruited to gang now, by tagging");

            //but they need to be able to FIND me, their leader, to deliver money to me
            //so, for now, fill their leader variable:
            //need their AI1 script:
            AI1 NPChubScript = selectedNPC.GetComponent("AI1") as AI1;
            NPChubScript.leader = this.gameObject;

            //ALSO NEED TO BLANK-OUT THEIR TARGET!!!
            NPChubScript.target = null;



            //personOfInterest = selectedNPC;
        }
        else
        {
            Debug.Log("I guess they don't trust you enough");
        }

        

    }

    public void closeRecruitmentMenu()
    {
        //set the NPC back to be able to move:
        Debug.Log(selectedNPC.name);
        AI1 targetAI = selectedNPC.GetComponent("AI1") as AI1;
        targetAI.inConversation = false;

        //Debug.Log(targetAI.inConversation);

        //Close recruitment menu
        //lock mouse:
        Cursor.lockState = CursorLockMode.Locked;
        //de-activate menu:
        recruitingMenu.SetActive(false);
        //de-select NPC:
        selectedNPC = null;
        //set the "inMenu" state to "false":
        inMenu = false;
        //Destroy the generated buttons:
        destroyObjectListItems(currentGridButtons);
        //Debug.Log(currentGridButtons.Count);


    }

    public void tradeButton()
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
            //very ad-hoc
            //I should instead be generataing these "buy" buttons at the store based on some INVENTORY that the store HAS.

            stateItem food1;
            food1 = premadeStuff.stateItemCreator("food", "inventory");

            //actionItem fooood;
            //fooood = premadeStuff.convertToActionItem(food1, 1);

            action buyFoooooood;
            buyFoooooood = premadeStuff.actionCreator("buyFood", "buyFromStore", premadeStuff.wantedPrereqsLister(premadeStuff.money), premadeStuff.UNwantedPrereqsLister(), premadeStuff.wantedEffectsLister(food1), premadeStuff.UNwantedEffectsLister(premadeStuff.money), 1, premadeStuff.checkout);

            theHub.state = theFunctions.implementALLEffects(buyFoooooood, theHub.state);
        }
        else
        {
            Debug.Log("no money!");
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

        theFunctions.gift(theHub.state["inventory"], hubOfNPC.state["inventory"], premadeStuff.giftGun);
        //theHub.state = theFunctions.implementALLEffects(premadeStuff.buyGun, theHub.state);
        Debug.Log("gave gun?");
    }


    ///////////////////////////////////////////////////////////
    //          Grabbing and using foreign scripts
    ///////////////////////////////////////////////////////////


    public social grabSocialScript()
    {
        social theSocialScript = selectedNPC.GetComponent("social") as social;
        return theSocialScript;
    }


}
