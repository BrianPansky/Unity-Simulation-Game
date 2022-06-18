using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerClickInteraction : MonoBehaviour
{
    GameObject clickedOn;

    public premadeStuffForAI premadeStuff;
    public AI1 theHub;
    public playerHUD myHUD;

    // Start is called before the first frame update
    void Start()
    {
        premadeStuff = GetComponent<premadeStuffForAI>();
        theHub = GetComponent<AI1>();
        myHUD = GetComponent<playerHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        //check for mouse click, see what it's "clicking on"
        clickedOn = clickingFunciton();

        //now do stuff:
        if (clickedOn != null)
        {
            doStuffAfterClick(clickedOn);

            //blank out mouse click each frame:
            clickedOn = null;
        }
        
    }




    public GameObject clickingFunciton()
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

    public void doStuffAfterClick(GameObject clickedOn)
    {
        //Debug.Log(clickedOn.name);
        if(clickedOn.name == "workPlace")
        {
            //Debug.Log("MONEY$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");

            theHub.state["inventory"].Add(premadeStuff.money1);
        }
    }
}
