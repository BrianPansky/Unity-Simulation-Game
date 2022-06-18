using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopkeeperNPC : MonoBehaviour
{
    public premadeStuffForAI stateGrabber;
    public AI1 theHub;


    void Awake()
    {
        stateGrabber = GetComponent<premadeStuffForAI>();
        theHub = GetComponent<AI1>();
    }


    // Start is called before the first frame update
    void Start()
    {


        actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.deepStateItemCopier(stateGrabber.profitMotive), 0);
        theHub.recurringGoal = goalActionItem;
        theHub.state = stateGrabber.createShopkeeperState();
        theHub.knownActions = stateGrabber.createShopkeeperKnownActions();
        
        
    }


}
