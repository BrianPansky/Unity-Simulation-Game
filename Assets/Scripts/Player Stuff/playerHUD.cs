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

    public AI1 theHub;

    // Start is called before the first frame update
    void Start()
    {
        theHub = GetComponent<AI1>();
    }

    // Update is called once per frame
    void Update()
    {
        //not sure if I should do this once per frame here
        //or only update one time, right after clicking?
        //well, sometimes it might change for reasons besides clicking...
        displayAllInventoryItems();
        displayToDoList();
    }

    void displayAllInventoryItems()
    {
        //change text to be items from inventory
        
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
        
    }

    void displayToDoList()
    {
        //change text to be items from inventory

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

    }
}
