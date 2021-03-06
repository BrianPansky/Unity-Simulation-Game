using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializing : MonoBehaviour
{
    public premadeStuffForAI stateGrabber;
    public AI1 theHub;
    // Start is called before the first frame update
    void Start()
    {

        stateGrabber = GetComponent<premadeStuffForAI>();
        theHub = GetComponent<AI1>();

        actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.hungry, 0);
        theHub.recurringGoal = goalActionItem;
        theHub.state = stateGrabber.createPLAYERstate();

        //ad-hoc, [see 395478]:
        theHub.theFunctions.incrementItem(theHub.factionState["unitState"], stateGrabber.soldier, 555);

    }
}
