using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static enactionCreator;
using static UnityEditor.LightmapEditorSettings;
using static virtualGamepad;

public class body2 : playable
{

    public GameObject enactionPoint1;


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


    void Awake()
    {
        currentHealth = maxHealth;
        connectingComponents();



        initializeEnactionPoint1();
        initializeCameraMount();

    }

    void initializeEnactionPoint1()
    {
        enactionPoint1 = new GameObject("aaaaaaaaaaaaaaaaaaaaaaaaaaaaenactionPoint1 in initializeEnactionPoint1() line 54, body2 script");
        enactionPoint1.transform.parent = transform;
        enactionPoint1.transform.position = this.transform.position + this.transform.forward*0.5f;
        enactionPoint1.transform.rotation = this.transform.rotation;

    }


    void initializeCameraMount()
    {
        cameraMount = new GameObject("cameraMount in initializeCamera() line 64, body2 script").transform;
        //cameraMount.transform.SetParent(transform, false);
        cameraMount.transform.SetParent(enactionPoint1.transform, false); //has to be child of ENACTION point for this body!  because THAT is the point which the gamepad rotates!!!

        //cameraMount.transform.parent = transform;
        //cameraMount.transform.position = this.transform.position + this.transform.forward * 0.1f;
        //cameraMount.transform.rotation = this.transform.rotation;

    }



    void connectingComponents()
    {
        /*
        if (thisNavMeshAgent == null)
        {
            thisNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            if (thisNavMeshAgent == null)
            {
                thisNavMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            }


            currentNavMeshAgent.baseOffset = 1; //prevent stutter, being in floor
        }
        */

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

        //      plugIntoGamepadIfThereIsOne();
    }

    public void plugIntoGamepadIfThereIsOne()
    {
        virtualGamepad gamepad = gameObject.GetComponent<virtualGamepad>();
        if (gamepad == null)
        {
            Debug.Log("gamepad == null for:  " + this.gameObject.name);
            return;
        }

        equip(gamepad);

    }

    public void equip(virtualGamepad gamepad)
    {
        //          if (occupied == true) { return; }
        //          occupied = true;


        //Debug.Log("is cameraMount  null:  " + cameraMount + "  for this object:  " + this.gameObject.name);
        //Debug.Log("is gamepad.theCamera null:  " + gamepad.theCamera + "  for this object:  " + this.gameObject.name);
        if (cameraMount != null && gamepad.theCamera != null)
        {

            gamepad.theCamera.transform.SetParent(cameraMount, false);
        }



        //controller plugs in its button categories, and bodies/weapons/items, and vehicles FILL them:

        foreach (IEnactaBool enactaBool in enactableBoolSet)
        {
            //this "object is null" error is usually the only kind of error where it isn't clear which variable went wrong
            //and EVERY TIME it's a situation like this, where there are a TON of variables in a single line.
            //so i need to print sooooooo many....
            Debug.Log("enactaBool:  " + enactaBool);
            //Debug.Log("enactaBool.interInfo:  " + enactaBool.interInfo);
            //Debug.Log("enactaBool.interInfo.enactionAuthor:  " + enactaBool.interInfo.enactionAuthor);
            //Debug.Log("gamepad:  " + gamepad);
            //Debug.Log("gamepad.transform:  " + gamepad.transform);
            //ebug.Log("gamepad.transform.gameObject:  " + gamepad.transform.gameObject);
            enactaBool.interInfo.enactionAuthor = gamepad.transform.gameObject;
            gamepad.allCurrentBoolEnactables[enactaBool.gamepadButtonType] = enactaBool;
        }



        foreach (IEnactaVector enactaV in enactableVectorSet)
        {
            //enactaV.enactionAuthor = gamepad.transform.gameObject;
            gamepad.allCurrentVectorEnactables[enactaV.gamepadButtonType] = enactaV;
        }

        gamepad.allCurrentTARGETbyVectorEnactables.Clear();
        gamepad.allCurrentTARGETbyVectorEnactables = enactableTARGETVectorSet;


        if (gamepad.theCamera == null) { return; }

        /*
        Debug.Log(cameraMount);
        if (cameraMount == null)
        {
            defaultCameraMountGenerator();
        }
        Debug.Log(cameraMount);
        gamepad.theCamera.transform.SetParent(cameraMount, false);
        */
    }

    void customizeTheComponents()
    {

        //theInteractionScript.dictOfInteractions.Add("bullet1", "die");  shoot1
        theInteractionScript.addInteraction(interType.shoot1, interactionScript.effect.damage);
        theInteractionScript.addInteraction(interType.shootFlamethrower1, interactionScript.effect.damage);
    }
    void makeEnactions()
    {

        //enactableBoolSet.Add(new intSpherAtor(this.transform, interType.standardClick, buttonCategories.primary, 1f, true));
        enactableBoolSet.Add(new projectileLauncher(enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.standardClick), 
            new projectileInfo(1)));


        enactableVectorSet.Add(new vecTranslation(speed, this.transform, buttonCategories.vector1));
        enactableVectorSet.Add(new vecRotation(lookSpeed, this.transform, enactionPoint1.transform, buttonCategories.vector2));


        enactableTARGETVectorSet.Add(new navAgent(this.gameObject));
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

        theInteractionScript.addInteraction(enactionCreator.interType.tankShot, interactionScript.effect.damage);
        theInteractionScript.addInteraction(enactionCreator.interType.shoot1, interactionScript.effect.damage);
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
