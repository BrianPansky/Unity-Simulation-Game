using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static virtualGamepad;

public class tank2 : MonoBehaviour, Iplayable
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





    public List<IEnactaBool> enactableBoolSet = new List<IEnactaBool>();
    public List<IEnactaVector> enactableVectorSet = new List<IEnactaVector>();
    public GameObject tankHead;
    public GameObject tankBarrel;

    // Start is called before the first frame update
    void Start()
    {
        makeEnactions();

        //Debug.DrawLine(this.transform.position, tankHead.transform.position, Color.white, 777f);
        //Debug.DrawLine(this.transform.position, tankBarrel.transform.position, Color.blue, 777f);
    }

    void makeEnactions()
    {
        enactableBoolSet.Add(
            new intSpherAtor(tankBarrel.transform, interType.tankShot, buttonCategories.primary, 1f)

            );

        enactableVectorSet.Add(new vecTranslation(speed, transform, buttonCategories.vector1));
    }


    public void equip(virtualGamepad gamepad)
    {
        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:


        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            enactaBool.enactionAuthor = gamepad.transform.gameObject;
            gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
        }

        foreach (IEnactaVector enactaV in enactableVectorSet)
        {
            //enactaV.enactionAuthor = gamepad.transform.gameObject;
            gamepad.allCurrentVectorEnactables[enactaV.gamepadButtonType] = enactaV;
        }


    }
}
