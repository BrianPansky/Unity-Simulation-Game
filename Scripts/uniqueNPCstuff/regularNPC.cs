﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regularNPC : MonoBehaviour
{
    public premadeStuffForAI stateGrabber;
    public AI1 theHub;
    // Start is called before the first frame update
    void Start()
    {
        //this is my regular NPC
        stateGrabber = GetComponent<premadeStuffForAI>();
        theHub = GetComponent<AI1>();

        theHub.recurringGoal = stateGrabber.hungry0;
        theHub.state = stateGrabber.createNPCstate1();
        theHub.knownActions = stateGrabber.createKnownActions1();
        theHub.map = stateGrabber.createMap1();

    }


}