using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickpocketNPC : MonoBehaviour
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
        //this is my pickpocket npc [or test NPC]

        //actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.deepStateItemCopier(stateGrabber.hungry), 0);
        actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.deepStateItemCopier(stateGrabber.placeHolderFactionGoal), 1);
        //actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.quantityOfItemGenerator(stateGrabber.money, 557), 1);
        //actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.quantityOfItemGenerator(stateGrabber.soldier, 1), 1);
        theHub.recurringGoal = goalActionItem;
        theHub.state = stateGrabber.createPickpocketState();
        theHub.knownActions = stateGrabber.createPickpocketKnownActions();

    }
}
