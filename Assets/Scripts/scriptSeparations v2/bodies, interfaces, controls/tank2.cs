using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static virtualGamepad;

public class tank2 : playable
{

    //      mouse look stuff
    public float lookSpeed = 290f;
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

    // Start is called before the first frame update
    void Start()
    {
        makeEnactions();

        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.interactable);

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

    void makeEnactions()
    {
        //printEnactaBoolSet();

        enactableBoolSet.Add(
            new intSpherAtor(tankBarrel.transform, interType.tankShot, buttonCategories.primary, 1f)

            );

        //printEnactaBoolSet();

        enactableVectorSet.Add(new vecTranslation(speed, transform, buttonCategories.vector1));


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
