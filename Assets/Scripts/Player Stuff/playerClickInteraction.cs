using System.Collections;
using System.Collections.Generic;
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



    public bool inMenu;

    public premadeStuffForAI premadeStuff;
    public AI1 theHub;
    public playerHUD myHUD;
    public functionsForAI theFunctions;

    // Start is called before the first frame update
    void Start()
    {
        premadeStuff = GetComponent<premadeStuffForAI>();
        theHub = GetComponent<AI1>();
        myHUD = GetComponent<playerHUD>();
        inMenu = false;
        recruitingMenu.SetActive(false);

        theFunctions = GetComponent<functionsForAI>();

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

            theHub.state["inventory"].Add(premadeStuff.money1);
        }

        if(clickedOn.tag == "anNPC")
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
            if (targetAI.toDoList.Count > 0 && targetAI.toDoList[0].type == "work")
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
        //all buttons should be added to "currentGridButtons"
        //so that they are easy to delete etc. later

        //DYNAMICALLY CREATE RECRUIT BUTTON, AD-HOC FOR NOW
        GameObject myNewGameObject = Instantiate(myPrefabButton1) as GameObject;
        myNewGameObject.GetComponentInChildren<Text>().text = "pickpocket";
        myNewGameObject.transform.SetParent(myGridCanvas.transform);
        myNewGameObject.GetComponent<Button>().onClick.AddListener(() => recruitButton());
        
        //add the button to a list, so I can elswhere delete it:
        currentGridButtons.Add(myNewGameObject);

        //DYNAMICALLY CREATE TEST BUTTON, AD-HOC FOR NOW
        GameObject myNewGameObject2 = Instantiate(myPrefabButton1) as GameObject;
        myNewGameObject2.GetComponentInChildren<Text>().text = "regular work";
        myNewGameObject2.transform.SetParent(myGridCanvas.transform);
        myNewGameObject2.GetComponent<Button>().onClick.AddListener(() => testButton());

        //add the button to a list, so I can elswhere delete it:
        currentGridButtons.Add(myNewGameObject2);
    }

    public void createStoreMenu()
    {
        //all buttons should be added to "currentGridButtons"
        //so that they are easy to delete etc. later

        //DYNAMICALLY CREATE BUYFOOD BUTTON, AD-HOC FOR NOW
        GameObject myNewGameObject = Instantiate(myPrefabButton1) as GameObject;
        myNewGameObject.transform.SetParent(myGridCanvas.transform);

        myNewGameObject.GetComponentInChildren<Text>().text = "buy food";
        myNewGameObject.GetComponent<Button>().onClick.AddListener(() => tradeButton());

        //add the button to a list, so I can elswhere delete it:
        currentGridButtons.Add(myNewGameObject);
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



    ///////////////////////////////////////////////////////////
    //              Specific Button Functions
    ///////////////////////////////////////////////////////////

    public void recruitButton()
    {
        Debug.Log("heloooooooooo");

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

    public void closeRecruitmentMenu()
    {
        //set the NPC back to be able to move:
        Debug.Log(selectedNPC.name);
        AI1 targetAI = selectedNPC.GetComponent("AI1") as AI1;
        targetAI.inConversation = false;

        Debug.Log(targetAI.inConversation);

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

            theHub.state = theFunctions.implementALLEffects(premadeStuff.buyFood, theHub.state);
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

        //need their AI1 script:
        AI1 hubScript = selectedNPC.GetComponent("AI1") as AI1;

        //need the action to add:
        premadeStuffForAI stateGrabber = GetComponent<premadeStuffForAI>();


        //now add work action, remove pickpocket action:
        hubScript.knownActions.Add(stateGrabber.doTheWork);
        hubScript.knownActions.RemoveAll(y => y.name == "pickVictimsPocket");
    }



}
