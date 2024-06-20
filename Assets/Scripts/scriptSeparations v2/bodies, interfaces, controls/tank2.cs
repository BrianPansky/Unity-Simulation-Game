using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static virtualGamepad;

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

        makeInteractions();
    }


    // Start is called before the first frame update
    void Start()
    {
        makeEnactions();

        //tagging2.singleton.addTag(this.gameObject, tagging2.tag2.interactable);

        initializeCamera();

        //Debug.Log("sssssssssssssssssssssssss");

        //Debug.Log("this.gameObject.transform.position:  " + this.gameObject.transform.position);
        //Debug.Log("this.gameObject.transform.up:  " + this.gameObject.transform.up);

        //Vector3 thisBit = 22 * (this.gameObject.transform.up - this.gameObject.transform.position);
        //Vector3 thisBit = (this.gameObject.transform.up - this.gameObject.transform.position);
        //          Vector3 thisBit = (this.gameObject.transform.position + 11*this.gameObject.transform.up);

        //          Debug.DrawLine(this.gameObject.transform.position, thisBit, Color.white, 777f);
        //Debug.DrawLine(this.transform.position, tankHead.transform.position, Color.white, 777f);
        //Debug.DrawLine(this.transform.position, tankBarrel.transform.position, Color.blue, 777f);
    }

    private void makeInteractions()
    {
        if (theInteractionScript == null)
        {
            theInteractionScript = this.gameObject.GetComponent<interactionScript>(); 

            if (theInteractionScript == null)
            {
                theInteractionScript = this.gameObject.AddComponent<interactionScript>();
                
            }
            
            //do i still need this?
            //theInteractionScript.dictOfInteractions = new Dictionary<interType, List<interactionScript.effect>>();//new Dictionary<string, List<string>>(); //for some reason it was saying it already had that key in it, but it should be blank.  so MAKING it blank.
        }


        theInteractionScript.addInteraction(enactionCreator.interType.standardClick, interactionScript.effect.useVehicle);
        theInteractionScript.addInteraction(enactionCreator.interType.tankShot, interactionScript.effect.damage);


        //Debug.Log("add the tags to tank2:  " + randomIndex);
    }

    void initializeCamera()
    {
        cameraMount = new GameObject("cameraMount in initializeCamera() line 54, tank2 script").transform;
        //cameraMount.transform.SetParent(transform, false);

        //Debug.Log("tankBarrel:  " + tankBarrel);
        cameraMount.transform.SetParent(tankBarrel.transform, false); //has to be child of ENACTION point for this body!  because THAT is the point which the gamepad rotates!!!
        //      cameraMount.transform.position += 0.2f*cameraMount.transform.up;
        cameraMount.transform.position += 0.1f * cameraMount.transform.up;
        //cameraMount.transform.position += 0.8f * cameraMount.transform.up - cameraMount.transform.forward;

        //cameraMount.transform.parent = transform;
        //cameraMount.transform.position = this.transform.position + this.transform.forward * 0.1f;
        //cameraMount.transform.rotation = this.transform.rotation;

    }

    void makeEnactions()
    {
        //printEnactaBoolSet();

        enactableBoolSet.Add(
            new intSpherAtor(tankBarrel.transform, interType.tankShot, buttonCategories.primary, 1f)

            );

        //printEnactaBoolSet();

        //enactableVectorSet.Add(new vecTranslation(speed, transform, buttonCategories.vector1));
        //  enactableVectorSet.Add(new vecRotation(lookSpeed, tankHead.transform, tankBarrel.transform, buttonCategories.vector2));
        new aimTarget(
            new vecRotation(lookSpeed, tankHead.transform, tankBarrel.transform, buttonCategories.vector2)
            ).addToBothLists(enactableVectorSet, enactableTARGETVectorSet);
        enactableVectorSet.Add(new turningWithNoStrafe(speed, transform, buttonCategories.vector1));

        enactableTARGETVectorSet.Add(new navAgent(this.gameObject));

    }

    private void printEnactaBoolSet()
    {
        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            Debug.Log("11111111111111111enactaBool:  " + enactaBool);
            Debug.Log("enactaBool.interactionType:  " + enactaBool.interactionType);
            Debug.Log("enactaBool.gamepadButtonType:  " + enactaBool.gamepadButtonType);
            //if (allCurrentBoolEnactables[thisKey] == null) { continue; }

            //if (allCurrentBoolEnactables[thisKey].interactionType == null) { continue; }
        }
    }



}
