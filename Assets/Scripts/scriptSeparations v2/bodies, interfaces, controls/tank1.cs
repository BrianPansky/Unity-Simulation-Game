using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tank1 : MonoBehaviour
{

    public CharacterController controller;

    public float turnSpeed = 1f;
    float speed = 0.2f;

    public Vector3 rotationVector = Vector3.zero;
    public Vector3 verticalAim = Vector3.zero;
    public Vector3 tankTreadsDirection = Vector3.zero;

    public GameObject tankHead;
    public GameObject tankBarrel;

    public GameObject firingStartPoint;
    public GameObject firingDirectionPoint;




    public NavMeshAgent thisNavMeshAgent;



    public float gravity = -9.81f;


    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;



    public float maxHealth = 100f;
    public float currentHealth = 0f;  //set to max health in "awake"


    public List<enactionMate> enactionSet = new List<enactionMate>();






    //bit ad hoc for now:
    public GameObject pilot;

    public sensorySystem thePilotSensorySystem;
    public enactionScript thePilotEnactionScript;


    void Awake()
    {
        currentHealth = maxHealth;
    }


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //tankHead.transform.rotation += new Quaternion();

        if(conditionsMet() == false)
        {
            return;
        }


        fillPilotScriptReferences();

        //updateAim();
        updateTurretYaw();
        updateBottomYaw();
        updateForwardBackwardMotion();

    }

    void updateForwardBackwardMotion()
    {

        this.gameObject.transform.localPosition += this.gameObject.transform.forward*thePilotEnactionScript.z * speed;
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
        tankHead.transform.Rotate(Vector3.up * thePilotEnactionScript.yawInput);
    }

    void updateBottomYaw()
    {

        //                  xRotation -= theEnactionScript.mouseY;
        //                  xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //                  transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //Debug.Log("thePilotEnactionScript.yawInput:  " + thePilotEnactionScript.yawInput);
        this.gameObject.transform.Rotate(Vector3.up * thePilotEnactionScript.x * turnSpeed);
    }


    void updateForwardBackMotion()
    {



        Vector3 move = transform.right * thePilotEnactionScript.x + transform.forward * thePilotEnactionScript.z;

        controller.Move(move * speed * Time.deltaTime);

    }


    public void updateAim()
    {
        //tankHead;
        //tankBarrel;

        //tankHead.transform.rotation = ;
        tankHead.transform.Rotate(thePilotSensorySystem.lookingRay.direction);

    }


    public Vector3 theShotAim()
    {
        return firingDirectionPoint.transform.position - firingStartPoint.transform.position;
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


}
