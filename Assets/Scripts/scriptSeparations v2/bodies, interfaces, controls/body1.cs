using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class body1 : MonoBehaviour
{
    //different things can be controlled in the game [human, vehicle, menu]
    //this is for a human body
    //it contains the actions that could be done if you press buttons on a controller
    //      for now, only actions which are IDENTICAL for both NPC and player will be put here
    //note this doesn't need to LIMIT the NPC.  they can ALSO have OTHER ways of doing similar things [like walking with navmesh]

    //so, which actions does it have?
    //forward, back, strafing, aiming up down left/right
    //jump
    //grab?  i guess.  without a body, even an NPC can't grab anything?
    //use items?


    public Vector3 lookingVector;
    public Ray lookingRay;



    public worldScript theWorldScript;
    public interactionEffects1 interactionScript;


    void Awake()
    {
        this.gameObject.AddComponent<interactionEffects1>();
        interactionScript = this.gameObject.GetComponent<interactionEffects1>();

        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        //interactionScript


        interactionMate mainInteractionMate = new interactionMate();

        mainInteractionMate.interactionAuthor = this.gameObject;
        initialGenerator2 theGeneratorScript = theWorldObject.GetComponent("initialGenerator2") as initialGenerator2;


        //mainInteractionMate.enactThisInteraction = interactionScript.generateInteractionFULL("standardInteraction1", theGeneratorScript.atomLister(theGeneratorScript.atoms["standardInteraction1Atom"]));

        testInteraction mainInteraction = interactionScript.generateInteraction("standardInteraction1");

        interactionScript.interactionDictionary.Add("doARegularClick", mainInteraction);

        testInteraction walkInteraction = interactionScript.generateInteraction("walkSomewhere");

        interactionScript.interactionDictionary.Add("walkSomewhere", walkInteraction);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        




    }





}
