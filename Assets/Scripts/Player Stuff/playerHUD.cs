﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;

public class playerHUD : MonoBehaviour
{
    //can just plug these in using Unity's drag and drop, for now:
    public GameObject myCanvas;
    public GameObject myText;

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
    }

    void displayAllInventoryItems()
    {
        //change text to be items from inventory
        
        if(theHub.state["inventory"].Count > 0)
        {
            string inventoryItem;
            inventoryItem = theHub.state["inventory"][0].name;

            //Debug.Log(inventoryItem);

            //now, how to set the text to say that???
            myText.GetComponent<Text>().text = inventoryItem;
        }
        else
        {
            myText.GetComponent<Text>().text = "[]";
        }
        
    }
}