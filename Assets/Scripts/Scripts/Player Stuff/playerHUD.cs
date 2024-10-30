using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;

public class playerHUD : MonoBehaviour
{
    //can just plug these in using Unity's drag and drop, for now:
    public GameObject myCanvas;
    public GameObject myText;
    public GameObject myToDoListText;

    public GameObject playerFakeGameReadout;
    public GameObject NPC0FakeGameReadout;
    public GameObject NPC0printout2;
    public GameObject NPC0GameObject;
    //public AIHub2 theNPC0Hub;

    //public AIHub2 theHub;

    // Start is called before the first frame update
    void Start()
    {
        //theHub = GetComponent<AIHub2>();


        //      some NPC stuff is null error in scene 2, so removing for now:
        //theNPC0Hub = NPC0GameObject.GetComponent<AIHub2>();
    }

    // Update is called once per frame
    void Update()
    {
        //not sure if I should do this once per frame here
        //or only update one time, right after clicking?
        //well, sometimes it might change for reasons besides clicking...

        //      some NPC stuff is null error in scene 2, so removing for now [also in "start" function]:
        //displayAllInventoryItems();
        //displayToDoList();
        //displayPlayerFakeGameReadout();
        //displayNPC0FakeGameReadout();
        //displayNPC0Inventory();
    }

    void displayAllInventoryItems()
    {
        //change text to be items from inventory
        
        /*
        if(theHub.state["inventory"].Count > 0)
        {
            //string inventoryItem;
            //inventoryItem = theHub.state["inventory"][0].name;

            //Debug.Log(inventoryItem);

            //now, how to set the text to say that???
            //myText.GetComponent<Text>().text = inventoryItem;
            string thePrintOut = "";
            foreach(stateItem item in theHub.state["inventory"])
            {
                thePrintOut += "(" + item.name.ToString() + ": " + item.quantity.ToString() + ")";
            }


            //myText.GetComponent<Text>().text = theHub.state["inventory"][0].quantity.ToString();
            myText.GetComponent<Text>().text = thePrintOut;
        }
        else
        {
            myText.GetComponent<Text>().text = "[]";
        }
        */
    }

    void displayToDoList()
    {
        //change text to be items from inventory
        /*
        if (theHub.inputtedToDoList.Count > 0)
        {
            

            //now, how to set the text to say that???
            //myText.GetComponent<Text>().text = inventoryItem;
            string thePrintOut = "";
            foreach (action item in theHub.inputtedToDoList)
            {
                thePrintOut += "(" + item.name.ToString() + ")";
            }


            
            myToDoListText.GetComponent<Text>().text = thePrintOut;
        }
        else
        {
            myToDoListText.GetComponent<Text>().text = "[]";
        }
        */
    }

    void displayPlayerFakeGameReadout()
    {
        //displayXFakeGameReadout(theHub, playerFakeGameReadout);
    }

    void displayNPC0FakeGameReadout()
    {
        //displayXFakeGameReadout(theNPC0Hub, NPC0FakeGameReadout);
    }

    void displayXFakeGameReadout(AIHub3 hub, GameObject textBox)
    {
        string thePrintOut = "";

        thePrintOut += "[";

        //foreach (string categoryName in hub.factionState.Keys)
        {
            //thePrintOut += "[";
            //foreach (stateItem item in hub.factionState[categoryName])
            {
                //thePrintOut += "(" + item.name.ToString() + ": " + item.quantity.ToString() + ")";
            }
            //thePrintOut += "] ";
        }

        thePrintOut += "]";

        textBox.GetComponent<Text>().text = thePrintOut;
    }

    void displayNPC0Inventory()
    {
        
        /*
        if (theNPC0Hub.state["inventory"].Count > 0)
        {
            //string inventoryItem;
            //inventoryItem = theHub.state["inventory"][0].name;

            //Debug.Log(inventoryItem);

            //now, how to set the text to say that???
            //myText.GetComponent<Text>().text = inventoryItem;
            string thePrintOut = "";
            foreach (stateItem item in theNPC0Hub.state["inventory"])
            {
                thePrintOut += "(" + item.name.ToString() + ": " + item.quantity.ToString() + ")";
            }


            //myText.GetComponent<Text>().text = theHub.state["inventory"][0].quantity.ToString();
            NPC0printout2.GetComponent<Text>().text = thePrintOut;
        }
        else
        {
            NPC0printout2.GetComponent<Text>().text = "[]";
        }
        */
    }
}
