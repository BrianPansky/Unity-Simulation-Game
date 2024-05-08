using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
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

    //public GameObject pointerPoint;
    //public Vector3 pointerOrigin;

    //public Vector3 lookingVector;



    //      mouse look stuff
    public float mouseSpeed = 290f;
    public Transform playerBody;
    float xRotation = 0f;

    public Ray lookingRay;





    public GameObject theBodyGameObject;
    public float standardClickDistance = 7.0f;

    public worldScript theWorldScript;
    public enactionScript theEnactionScript;
    public interactionScript theInteractionScript;
    public mapZoneScript theLocalMapZoneScript;


    public NavMeshAgent thisNavMeshAgent;

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;



    public float maxHealth = 100f;
    public float currentHealth = 0f;  //set to max health in "awake"


    public List<enactionMate> enactionSet = new List<enactionMate>();

    public bool gamePadActive = true;


    void Awake()
    {
        currentHealth = maxHealth;
        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;


        if (controller == null)
        {
            controller = gameObject.GetComponent<CharacterController>();
            if (controller == null)
            {
                controller = gameObject.AddComponent<CharacterController>();
            }
        }



        //initializeNavmeshAgent();


        if (theEnactionScript == null)
        {
            theEnactionScript = this.GetComponent<enactionScript>();
            if (theEnactionScript == null)
            {
                theEnactionScript = this.gameObject.AddComponent<enactionScript>();
            }
        }

        theEnactionScript.currentlyUsable.Add("humanBody");  //partial implemention for now.  later need limbs and whatever else

        //enactionScript.availableEnactions.Add("walk");

        //      theEnactionScript.availableEnactions.Add("navMeshWalk");
        //      theEnactionScript.availableEnactions.Add("aim");
        //      theEnactionScript.availableEnactions.Add("standardClick");




        enactionSet.Add(navMeshWalk());
        enactionSet.Add(aim());
        enactionSet.Add(standardClick());

    }

    void initializeNavmeshAgent()
    {
        if (thisNavMeshAgent == null)
        {
            thisNavMeshAgent = GetComponent<NavMeshAgent>();
            if (thisNavMeshAgent == null)
            {

                thisNavMeshAgent = this.gameObject.AddComponent<NavMeshAgent>();
            }

            thisNavMeshAgent.speed = 13f;
        }
    }

    enactionMate navMeshWalk()
    {
        enactionMate newEnactionMate = new enactionMate();

        newEnactionMate.navmeshResume = true;
        //      newEnactionMate.navmeshTarget








        //                  enactionAuthor.GetComponent<enactionScript>().thisNavMeshAgent.Resume();

        //                  Vector3 lineBetweenNormalized = enactionTarget.transform.position.normalized - enactionAuthor.transform.position.normalized;
        //                  Vector3 positionNEARtheTarget = enactionTarget.transform.position - lineBetweenNormalized * 1.5f;

        //                  enactionAuthor.GetComponent<AIHub2>().thisNavMeshAgent.SetDestination(positionNEARtheTarget);
        //                  inTransit = true;













        //Debug.Log("navMeshWalk");
        //body1 theBody = this.gameObject.GetComponent<body1>();
        //theBody.lookingRay = new Ray(this.transform.position, (theBody.theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theBody.theWorldScript.theTagScript.ALLTaggedWithMultiple("interactable"), this.gameObject).transform.position - this.transform.position));

        //Vector3 targetVector = theBody.theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theBody.theWorldScript.theTagScript.findXNearestToY("mapZone", this.gameObject).GetComponent<mapZoneScript>().theList, this.gameObject).transform.position;
        //Vector3 targetVector = theBody.theWorldScript.theTagScript.semiRandomUsuallyNearTargetPickerFromList(theBody.theLocalMapZoneScript.theList, this.gameObject).transform.position;



        //set an endpoint not QUITE at the target object, so there's room
        //Vector3 currentNPCPosition = enactionAuthor.transform.position;
        //Vector3 targetObjectPosition = enactionTarget.transform.position;
        //Vector3 lineBetween = targetObjectPosition - currentNPCPosition;
        //Vector3 lineBetweenNormalized = lineBetween.normalized;
        //Vector3 lineBetweenNormalized = targetObjectPosition.normalized - currentNPCPosition.normalized;
        //Vector3 positionNEARtheTarget = targetObjectPosition - lineBetweenNormalized*2;

        //  just in case fewer lines and variables is faster:




        //this.gameObject.GetComponent<AIHub2>().thisNavMeshAgent.isStopped = true;




        return newEnactionMate;
    }

    enactionMate aim()
    {
        enactionMate newEnactionMate = new enactionMate();
        newEnactionMate.enactThis = "aim";









        //                  Vector3 startPoint = enactionBody.pointerOrigin();
        //                  Vector3 endPoint = enactionTarget.gameObject.transform.position;
        //                  enactionBody.lookingRay = new Ray(startPoint, (endPoint - startPoint));
        //                  deleteThisEnaction = true;

















        //Debug.Log("aim");
        //set lookingRay to a random target
        //body1 theBody = enactionAuthor.GetComponent<body1>();

        //theBody.lookingRay = new Ray(this.transform.position, (theBody.theWorldScript.theTagScript.pickRandomObjectFromListEXCEPT(theBody.theWorldScript.theTagScript.findXNearestToY("mapZone", this.gameObject).GetComponent<mapZoneScript>().theList, this.gameObject).transform.position - this.transform.position));
        //semiRandomUsuallyNearTargetPicker
        //enactionBody.lookingRay = new Ray(enactionBody.pointerPoint.transform.position, (enactionTarget.transform.position - enactionBody.pointerPoint.transform.position));

        //Vector3 startPoint = enactionBody.pointerOrigin();
        //Vector3 endPoint = enactionTarget.transform.position;

        //Vector3 endPoint = startPoint + new Vector3(6,7,8);

        //mastLine(startPoint, Color.white, 1f);
        //mastLine(endPoint, Color.green, 1f);

        //          WHICH OF THESE IS THE CORRECT WAY TO DO IT????
        //enactionBody.lookingRay = new Ray(startPoint, (startPoint - endPoint));

        //Debug.DrawRay(enactionBody.lookingRay.origin, enactionBody.lookingRay.direction, Color.magenta, 77f);

        //new Ray(this.transform.position, (adhocPrereqFillerTest[0].target1.transform.position - this.transform.position));











        return newEnactionMate;
    }
    enactionMate standardClick()
    {
        enactionMate newEnactionMate = new enactionMate();




        newEnactionMate.enactThis = "firingByRaycastHit";
        newEnactionMate.range = standardClickDistance;



        return newEnactionMate;
    }





    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log("pointerPoint:  " + pointerPoint);

        //GameObject myTest2 = theWorldScript.theRespository.createAndReturnPrefabAtPointWITHNAME(theWorldScript.theRespository.invisiblePoint, this.gameObject.transform.position + new Vector3(0, 0, 0.8f), this.gameObject.name + "pointer");

        //myTest2.transform.SetParent(this.gameObject.transform, true);
        //pointerPoint = myTest2;
        //pointerOrigin = this.gameObject.transform.position + new Vector3(0, 0, 0.8f);

        //Debug.Log("pointerPoint:  " + pointerPoint);

        //"bullet1"
        if(theInteractionScript == null)
        {
            this.gameObject.AddComponent<interactionScript>();
            theInteractionScript = this.gameObject.GetComponent<interactionScript>();
            theInteractionScript.dictOfInteractions = new Dictionary<string, List<string>>(); //for some reason it was saying it already had that key in it, but it should be blank.  so MAKING it blank.
        }

        //theInteractionScript.dictOfInteractions.Add("bullet1", "die");  shoot1
        theInteractionScript.addInteraction("shoot1", "damage");
        theInteractionScript.addInteraction("shootFlamethrower1", "damage");
        //.Add("walk");
    }

    // Update is called once per frame
    void Update()
    {
        if (conditionsMet() == false)
        {
            return;
        }
        //updatePitch();
        updateYaw();


        //      Vector3 startV = lookingRay.origin;

        //Vector3 diffV = (endV - startV);
        //Vector3 drawV = endV + diffV * lengthMultiplier * lengthMultiplier * diffV.sqrMagnitude * diffV.sqrMagnitude / 10;





        //Vector3 startV = this.gameObject.transform.position;
        //Vector3 endV = startV + (4*lookingRay.direction);//.operator(2);// * someBoolsAdded[thePointIndex];
        //Debug.DrawLine(startV, endV, new Color(0f, 1f, 0f), 1f);





        isGrounded = isThisGrounded();

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        Vector3 move = transform.right * theEnactionScript.x + transform.forward * theEnactionScript.z;

        controller.Move(move * speed * Time.deltaTime);


        //Debug.Log("isGrounded:  " + isGrounded);

        //Debug.Log("theEnactionScript.jump:  " + theEnactionScript.jump);
        if (theEnactionScript.jump && isGrounded)
        {

            //Debug.Log("ya, body is jumping");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            theEnactionScript.jump = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);





    }

    public bool conditionsMet()
    {
        //      this can't work, because we'd never know when to add them back on after they are removed:  [unless....exiting a vehicle inherently KNOWS to add that back?  hmm, maybe]
        if (theEnactionScript.currentlyUsable.Contains("humanBody") != true)
        {
            return false;
        }


        //ad hoc way to do this sorta:
        //if (theEnactionScript.bodyCanBeUsed == false)
        {
            //return false;
        }

        return true;
    }

    void updateYaw()
    {

        //                  xRotation -= theEnactionScript.mouseY;
        //                  xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //                  transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        this.gameObject.transform.Rotate(Vector3.up * theEnactionScript.yawInput);
    }


    public void killThisBody()
    {
        //this.gameObject.SetActive(false);

        theLocalMapZoneScript.theList.Remove(this.gameObject);
        theLocalMapZoneScript.threatList.Remove(this.gameObject);



        theWorldScript.theTagScript.foreignRemoveALLtags(this.gameObject);

        Debug.Log("destroy this object:  " + this.gameObject.GetInstanceID() + this.gameObject);
        UnityEngine.Object.Destroy(this.gameObject);

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
