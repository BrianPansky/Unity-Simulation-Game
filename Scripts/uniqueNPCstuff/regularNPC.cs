using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regularNPC : MonoBehaviour
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
        //this is my regular NPC
        
        actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.deepStateItemCopier(stateGrabber.hungry), 0);
        theHub.recurringGoal = goalActionItem;
        theHub.state = stateGrabber.createNPCstate1();
        theHub.knownActions = stateGrabber.createKnownActions1();

    }


}
