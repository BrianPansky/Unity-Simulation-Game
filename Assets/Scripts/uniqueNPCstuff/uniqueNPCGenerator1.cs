using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uniqueNPCGenerator1 : MonoBehaviour
{
    //for generativity, have peices of NPC generation, as functions or whatever
    //can call those functions to generate NPC differently

    //have these available OUTSIDE of this script???  on the generator?  hmm....
    //public premadeStuffForAI stateGrabber;
    //public AI1 theHub;

    void Awake()
    {
        //stateGrabber = GetComponent<premadeStuffForAI>();
        //theHub = GetComponent<AI1>();
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }


    public void createRegularNPC(AI1 theHub, premadeStuffForAI stateGrabber)
    {
        //takes the AI1 hub, and a premade stuff script [not necessarily the one attached to the NPC?]
        //generates my regular NPC

        //actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.deepStateItemCopier(stateGrabber.hungry), 0);
        //theHub.recurringGoal = goalActionItem;
        //theHub.state = stateGrabber.createNPCstate1();
        //theHub.knownActions = stateGrabber.createKnownActions1();

        generateNPC(theHub,
            stateGrabber.convertToActionItem(stateGrabber.deepStateItemCopier(stateGrabber.hungry), 0),
            stateGrabber.createNPCstate1(),
            stateGrabber.createKnownActions1()
            );

    }

    public void generateNPC(AI1 theHub, actionItem goalActionItem, Dictionary<string, List<stateItem>> initialState, List<action> initialKnownActions)
    {
        //takes the AI1 hub, and custom inputs for goal, and initialization of state and knownActions
        //generates "any" NPC

        //goal:
        theHub.recurringGoal = goalActionItem;

        //state initialization:
        theHub.state = initialState;

        //knownActions initialization:
        theHub.knownActions = initialKnownActions;

    }




    void pickpocketNPC()
    {
        //this is my pickpocket npc [or test NPC]

        //actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.deepStateItemCopier(stateGrabber.placeHolderFactionGoal), 1);

        //theHub.recurringGoal = goalActionItem;
        //theHub.state = stateGrabber.createPickpocketState();
        //theHub.knownActions = stateGrabber.createPickpocketKnownActions();


        //old scrap:
        //actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.deepStateItemCopier(stateGrabber.hungry), 0);
        //actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.quantityOfItemGenerator(stateGrabber.money, 557), 1);
        //actionItem goalActionItem = stateGrabber.convertToActionItem(stateGrabber.quantityOfItemGenerator(stateGrabber.soldier, 1), 1);
    }


}
