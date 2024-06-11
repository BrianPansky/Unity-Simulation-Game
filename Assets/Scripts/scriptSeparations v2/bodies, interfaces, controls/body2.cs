using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static enactionCreator;
using static UnityEditor.LightmapEditorSettings;
using static virtualGamepad;

public class body2 : MonoBehaviour, Iplayable
{

    public GameObject enactionPoint1;


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
    


    public interactionScript theInteractionScript;
    //public mapZoneScript theLocalMapZoneScript;

    public NavMeshAgent thisNavMeshAgent;


    void Awake()
    {
        currentHealth = maxHealth;
        connectingComponents();




    }

    void connectingComponents()
    {

        if (thisNavMeshAgent == null)
        {
            thisNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            if (thisNavMeshAgent == null)
            {
                thisNavMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            }
        }


        if (theInteractionScript == null)
        {
            theInteractionScript = this.gameObject.AddComponent<interactionScript>();
            theInteractionScript.dictOfInteractions = new Dictionary<interType, List<interactionScript.effect>>();//new Dictionary<string, List<string>>(); //for some reason it was saying it already had that key in it, but it should be blank.  so MAKING it blank.
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        customizeTheComponents();
        makeEnactions();

        plugIntoGamepadIfThereIsOne();
    }

    void customizeTheComponents()
    {

        //theInteractionScript.dictOfInteractions.Add("bullet1", "die");  shoot1
        theInteractionScript.addInteraction(interType.shoot1, interactionScript.effect.damage);
        theInteractionScript.addInteraction(interType.shootFlamethrower1, interactionScript.effect.damage);
    }
    void makeEnactions()
    {
        enactableBoolSet.Add(
            new intSpherAtor(this.transform, interType.standardClick, buttonCategories.primary, 1f, true)
            
            );

        enactableVectorSet.Add(new vecTranslation(speed, transform, buttonCategories.vector1));
    }

    public void plugIntoGamepadIfThereIsOne()
    {
        virtualGamepad gamepad = gameObject.GetComponent<virtualGamepad>();
        if(gamepad == null ){return; }

        equip(gamepad);

    }


    public void equip(virtualGamepad gamepad)
    { 
        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:


        foreach(IEnactaBool enactaBool in enactableBoolSet)
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



}
