using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopkeeperNPC : MonoBehaviour
{
    public premadeStuffForAI stateGrabber;
    public AI1 theHub;
    // Start is called before the first frame update
    void Start()
    {
        stateGrabber = GetComponent<premadeStuffForAI>();
        theHub = GetComponent<AI1>();

        actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.profitMotive, 0);
        theHub.recurringGoal = goalActionItem;
        //print(stateGrabber.profitMotive0.name);
        theHub.state = stateGrabber.createShopkeeperState();
        theHub.knownActions = stateGrabber.createShopkeeperKnownActions();
        

        //ad-hoc make the shopkeeper "know" which store is "theirs":
        //theHub.roleLocation = 
    }


}
