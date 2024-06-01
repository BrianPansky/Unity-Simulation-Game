using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tank1 : MonoBehaviour
{
    public GameObject firePoint;

    public float turnSpeed = 1f;
    float speed = 0.2f;
    float turretTurnSpeed = 0.2f;
    float barrelPitchSpeed = 1f;

    float reloadTimeMax = 52f;

    float currentReloadTime = 0f;
    float barrelPitch = 0f;
    float pitchRange = 30f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    bool isGrounded;



    public float maxHealth = 100f;
    public float currentHealth = 0f;  //set to max health in "awake"




    public List<enactionMate> enactionSet = new List<enactionMate>();





    public GameObject tankHead;
    public GameObject tankBarrel;

    public GameObject firingStartPoint;
    public GameObject firingDirectionPoint;




    public NavMeshAgent thisNavMeshAgent;






    //bit ad hoc for now:
    public GameObject pilot;
    public sensorySystem thePilotSensorySystem;
    public enactionScript thePilotEnactionScript;
    public interactionScript theInteractionScript;

    public enactionScript theTANKEnactionScript;





    //what are these?
    public LayerMask groundMask;




    //do i need these?
    public Vector3 rotationVector = Vector3.zero;
    public Vector3 verticalAim = Vector3.zero;
    public Vector3 tankTreadsDirection = Vector3.zero;
    Vector3 velocity;
    public CharacterController controller;









    void Awake()
    {
        currentHealth = maxHealth;
    }


    // Start is called before the first frame update
    void Start()
    {

        initializeOtherScriptReferences();



        theInteractionScript.addInteraction("tankShot", "burn");
        theInteractionScript.addInteraction("standardClick", "useVehicle");

        Debug.Log("note to self:  this is what you were working on:");
        //                          theTANKEnactionScript.

        //theTANKEnactionScript.firePoint = firePoint;
        theTANKEnactionScript.firePoint = tankBarrel;
        Debug.DrawLine(tankBarrel.transform.position, new Vector3(), Color.white, 555f);
        theTANKEnactionScript.addEnactionSphere(this.gameObject, "tankShot", default, 20f, 47, default);


    }


    void initializeOtherScriptReferences()
    {

        
        if (theTANKEnactionScript == null)
        {
            theTANKEnactionScript = this.GetComponent<enactionScript>();
            if (theTANKEnactionScript == null)
            {
                theTANKEnactionScript = this.gameObject.AddComponent<enactionScript>();
            }
        }

        if (theInteractionScript == null)
        {
            theInteractionScript = this.GetComponent<interactionScript>();
            if (theInteractionScript == null)
            {
                theInteractionScript = this.gameObject.AddComponent<interactionScript>();
            }
        }


        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(conditionsMet() == false)
        {
            return;
        }


        //Debug.Log("theTANKEnactionScript:  " + theTANKEnactionScript);
        theTANKEnactionScript.makeAllSpheresLookAtButtons(thePilotEnactionScript.theGamePad, thePilotEnactionScript);


        fillPilotScriptReferences();

        updateTurretYaw();
        updateBottomYaw();
        updateForwardBackwardMotion();
        updateBarrelPitch();

        //updateFiring();

    }

    void updateFiring()
    {
        //use jump button for now:
        if (thePilotEnactionScript.theGamePad.jump && currentReloadTime > reloadTimeMax)
        {
            currentReloadTime = 0f;
            projectileGenerator(pilot, "tankShot", (tankBarrel.transform.position + tankBarrel.transform.forward*5 + tankBarrel.transform.up * 2), tankBarrel.transform.forward);
            
        }

        if(currentReloadTime < reloadTimeMax+1)
        {
            currentReloadTime++;
        }

        thePilotEnactionScript.theGamePad.jump = false;
    }

    void updateForwardBackwardMotion()
    {

        this.gameObject.transform.localPosition += this.gameObject.transform.forward*thePilotEnactionScript.theGamePad.z * speed;
        //this.gameObject.transform.forward += new Vector3(0f, 0f, thePilotEnactionScript.z * speed);
        //Vector3 move = transform.right * theEnactionScript.x + transform.forward * theEnactionScript.z;
        //this.gameObject.transform.localPosition += new Vector3(0f, 0f, thePilotEnactionScript.z * speed);
    }

    public bool conditionsMet()
    {
        if (pilot == null)
        {
            return false;
        }

        return true;
    }

    void updateTurretYaw()
    {

        //                  xRotation -= theEnactionScript.mouseY;
        //                  xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //                  transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //Debug.Log("thePilotEnactionScript.yawInput:  " + thePilotEnactionScript.yawInput);
        tankHead.transform.Rotate(Vector3.up * thePilotEnactionScript.theGamePad.yawInput * turretTurnSpeed);
    }

    void updateBottomYaw()
    {

        //                  xRotation -= theEnactionScript.mouseY;
        //                  xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //                  transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //Debug.Log("thePilotEnactionScript.yawInput:  " + thePilotEnactionScript.yawInput);
        this.gameObject.transform.Rotate(Vector3.up * thePilotEnactionScript.theGamePad.x * turnSpeed);
    }

    void updateBarrelPitch()
    {
        barrelPitch -= thePilotEnactionScript.theGamePad.pitchInput * barrelPitchSpeed;
        barrelPitch = Mathf.Clamp(barrelPitch, -pitchRange, pitchRange);
        //tankBarrel
        //tankHead.transform.Rotate(Vector3.up * thePilotEnactionScript.yawInput);

        //tankBarrel.transform.localRotation = Quaternion.Euler(thePilotEnactionScript.pitchInput, 0f, 0f);

        //tankBarrel.transform.Rotate(Vector3.right * barrelPitch);
        tankBarrel.transform.localRotation = Quaternion.Euler(barrelPitch, 0f, 0f);

    }



    public void fillPilotScriptReferences()
    {
        //thePilotSensorySystem;
        //public enactionScript thePilotEnactionScript;
        if(thePilotSensorySystem == null)
        {
            thePilotSensorySystem = pilot.GetComponent<sensorySystem>();
        }
        if (thePilotEnactionScript == null)
        {
            thePilotEnactionScript = pilot.GetComponent<enactionScript>();
        }
    }




    public void projectileGenerator(GameObject author, string interactionType, Vector3 startPoint, Vector3 direction, bool sdOnCollision = true, int timeUntilSelfDestruct = 99, float growthSpeed = 0f, float magnitudeOfInteraction = 1f)
    {
        GameObject prefabToUse = thePilotSensorySystem.theWorldScript.theRepository.interactionSphere;


        GameObject newProjectile = thePilotSensorySystem.theWorldScript.theRepository.createPrefabAtPointAndRETURN(prefabToUse, startPoint);
        //UnityEngine.Object.Destroy(thisObject.GetComponent<selfDestructScript1>());

        //newProjectile.transform.position += enactionBody.lookingRay.direction;
        //theInteractionMate.interactionAuthor.transform.position + new Vector3(0, 0, 0)
        projectile1 projectileScript = newProjectile.AddComponent<projectile1>();
        projectileScript.Direction = direction;
        projectileScript.selfDestructOnCollision = sdOnCollision;
        selfDestructScript1 killScript = newProjectile.GetComponent<selfDestructScript1>();
        killScript.timeUntilSelfDestruct = timeUntilSelfDestruct;
        //killScript.

        if (growthSpeed > 0f)
        {
            newProjectile.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            growScript1 growScript = newProjectile.AddComponent<growScript1>();
            growScript.growthSpeed = growthSpeed;
        }

        //      should this use "interactionMate" isntead?

        authorScript1 theAuthorScript = newProjectile.GetComponent<authorScript1>();
        theAuthorScript.theAuthor = author;
        //theAuthorScript.enactThisInteraction = theInteractionMate.enactThisInteraction;
        //theAuthorScript.interactionType = "bullet1";
        theAuthorScript.interactionType = interactionType;
        theAuthorScript.magnitudeOfInteraction = magnitudeOfInteraction;


        //threatAlert(theAuthorScript.theAuthor);
    }



}
