using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static equippableSetup;


public class tank2 : playable
{
    interactionScript theInteractionScript;
    //      mouse look stuff
    //public float lookSpeed = 0.002f;
    float lookSpeed = 290f;
    public float standardClickDistance = 7.0f;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    public bool isGrounded;



    public float maxHealth = 100f;
    public float currentHealth = 0f;  //set to max health in "awake"





    public GameObject tankHead;
    public GameObject tankBarrel;

    void Awake()
    {

        //Debug.Log("Awake:  " + this);
        makeInteractions();
    }


    // Start is called before the first frame update
    void Start()
    {
        //initializeCustomEnactionPoint1(GameObject parent, Vector3 offset)
        initializeCustomEnactionPoint1(tankBarrel, 2*this.transform.forward);
        //enactionPoint1.transform.position += enactionPoint1.transform.forward;
        initializeCameraMount(tankBarrel.transform, 0.1f * tankBarrel.transform.up); //has to be child of ENACTION point for this body!  because THAT is the point which the gamepad rotates!!!


        makeEnactions();
        //Debug.DrawLine(this.transform.position, tankBarrel.transform.position, Color.blue, 777f);
    }

    

    private void makeInteractions()
    {
        //theInteractionScript = genGen.singleton.ensureInteractionScript(this.gameObject);

        //Debug.Log("///////////////////////////////////////////////////////////////////////");
        if(dictOfInteractions == null)
        {

            //Debug.Log("dictOfInteractions == null");
        }
        else
        {
            foreach (var typex in dictOfInteractions.Keys)
            {
                Debug.Log(typex);
            }
        }




        //Debug.Log("add the tags to tank2:  " + randomIndex);
        dictOfInteractions = dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.standardClick, new playAsPlayable());

        /*
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        if (dictOfInteractions == null)
        {

            Debug.Log("dictOfInteractions == null");
        }
        else
        {
            foreach (var typex in dictOfInteractions.Keys)
            {
                Debug.Log(typex);
            }
        }

        */

        dictOfInteractions = dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.tankShot, new damage(4));


        /*
        Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        if (dictOfInteractions == null)
        {

            Debug.Log("dictOfInteractions == null");
        }
        else
        {
            foreach (var typex in dictOfInteractions.Keys)
            {
                Debug.Log(typex);
            }
        }
        */

        //Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
    }

    void makeEnactions()
    {
        //printEnactaBoolSet();

        enactableBoolSet.Add(new projectileLauncher(enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.tankShot),
            new projectileToGenerate(1, true, 99, 0, false, true)));


        //printEnactaBoolSet();

        new aimTarget(
            new vecRotation(lookSpeed, tankHead.transform, tankBarrel.transform, buttonCategories.vector2, 25f)
            ).addToBothLists(enactableVectorSet, enactableTARGETVectorSet);
        
        enactableVectorSet.Add(new turningWithNoStrafe(speed, transform, buttonCategories.vector1));

        enactableTARGETVectorSet.Add(new navAgent(this.gameObject));

    }

    private void printEnactaBoolSet()
    {
        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            Debug.Log("11111111111111111enactaBool:  " + enactaBool);
            //Debug.Log("enactaBool.interactionType:  " + enactaBool.interInfo.interactionType);
            Debug.Log("enactaBool.gamepadButtonType:  " + enactaBool.gamepadButtonType);
            //if (allCurrentBoolEnactables[thisKey] == null) { continue; }

            //if (allCurrentBoolEnactables[thisKey].interactionType == null) { continue; }
        }
    }

    void OnDestroy()
    {
        comboGen.singleton.tankDeathExplosion(this.transform.position);
    }

}
