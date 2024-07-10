using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static enactionCreator;
using static equippableSetup;
using static UnityEditor.LightmapEditorSettings;

public class body2 : playable
{

    //public GameObject enactionPoint1;


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






    public interactionScript theInteractionScript;
    //public mapZoneScript theLocalMapZoneScript;

    public NavMeshAgent thisNavMeshAgent;
    public inventory1 theInventory;

    void Awake()
    {
        //Debug.Log("Awake:  " + this);
        currentHealth = maxHealth;
        initializingComponents();

        equipperSlotsAndContents[interactionCreator.simpleSlot.hands] = null;

        initializeEnactionPoint1();
        initializeCameraMount(enactionPoint1.transform);

    }

    



    void initializingComponents()
    {
        //theInteractionScript = genGen.singleton.ensureInteractionScript(this.gameObject);
        theInventory = genGen.singleton.ensureInventory1Script(this.gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        makeInteractions();
        makeEnactions();

        plugIntoGamepadIfThereIsOne();
    }


    void makeEnactions()
    {
        enactableBoolSet.Add(new hitscanEnactor(enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.standardClick)));
        

        enactableVectorSet.Add(new vecTranslation(speed, this.transform, buttonCategories.vector1));

        enactableTARGETVectorSet.Add(new navAgent(this.gameObject));
        new aimTarget(
            new vecRotation(lookSpeed, this.transform, enactionPoint1.transform, buttonCategories.vector2)
            ).addToBothLists(enactableVectorSet, enactableTARGETVectorSet);

    }

    private void makeInteractions()
    {
        dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, interType.shoot1, new damage());
        dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, interType.shootFlamethrower1, new damage());
        dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.tankShot, new damage());
        dictOfInteractions = interactionCreator.singleton.addInteraction(dictOfInteractions, enactionCreator.interType.tankShot, new damage());  //uhhh, double damage
    }

    public bool isThisGrounded()
    {
        //old version is this one line of code:
        //return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask, QueryTriggerInteraction.Ignore);

        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);

        // Cast a ray straight downwards.
        if (Physics.Raycast(downRay, out hit))
        {
            //Debug.Log(hit.distance);
            //when i start on ground, it returns:  1.080001

            if (hit.distance < 1.09f)
            {

                return true;
            }


            // The "error" in height is the difference between the desired height
            // and the height measured by the raycast distance.
            //float hoverError = hoverHeight - hit.distance;

            // Only apply a lifting force if the object is too low (ie, let
            // gravity pull it downward if it is too high).
            //if (hoverError > 0)
            {
                // Subtract the damping from the lifting force and apply it to
                // the rigidbody.
                //float upwardSpeed = rb.velocity.y;
                //float lift = hoverError * hoverForce - upwardSpeed * hoverDamp;
                //rb.AddForce(lift * Vector3.up);
            }
        }

        return false;
    }


    void Update()
    {
        //Debug.DrawLine(enactionPoint1.transform.position, enactionPoint1.transform.position+enactionPoint1.transform.forward, Color.magenta, 0.3f);
    }

}
