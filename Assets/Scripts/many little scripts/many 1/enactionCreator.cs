using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static enactionCreator;
using static UnityEngine.GraphicsBuffer;

public class enactionCreator : MonoBehaviour
{
    public static enactionCreator singleton;


    public enum interType
    {
        standardClick,
        shoot1,
        shootFlamethrower1,
        tankShot
    }
    
    
    public enum vectorEnactionSubType  // "legibleType"???
    {
        navMesh,
        Aim
    }

    /*
    public enum legibleUse
    {
        navMesh,
        Aim
    }
    */

    void Awake()
    {
        singletonify();

    }

    void singletonify()
    {
        if (singleton != null && singleton != this)
        {
            Debug.Log("this class is supposed to be a singleton, you should not be making another instance, destroying the new one");
            Destroy(this);
            return;
        }
        singleton = this;
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }






    /*
    public void addEnactionSphere(GameObject author, enactionCreator.interType interactionType, bool sdOnCollision = true, float magnitudeOfInteraction = 1f, int timeUntilSelfDestruct = 99, float growthSpeed = 0f)
    {
        //adds interaction sphere generator to list of sphere enactions

        //      interesting idea:  note the FUNCTION generating input "projectileStartPoint1()":
        //projectileGenerator(enactionAuthor, "shootFlamethrower1", projectileStartPoint1(), authorSensorySystem.lookingRay.direction, false, 10, 0.3f, 10);

        intSpherAtor newIntSpherAtor = new intSpherAtor();
        newIntSpherAtor.enactionAuthor = author;
        newIntSpherAtor.firePoint = this.firePoint;
        newIntSpherAtor.interactionType = interactionType;
        newIntSpherAtor.sdOnCollision = sdOnCollision;
        newIntSpherAtor.magnitudeOfInteraction = magnitudeOfInteraction;

        //Debug.Log("timeUntilSelfDestructzzzzzzzzzzzzzzzzzzz:  " + timeUntilSelfDestruct);
        newIntSpherAtor.timeUntilSelfDestruct = timeUntilSelfDestruct;
        newIntSpherAtor.growthSpeed = growthSpeed;


        interactionSphereList.Add(newIntSpherAtor);
    }



    internal void addHitscanSphere(GameObject author, enactionCreator.interType interactionType, float inputRange = 5f, float magnitudeOfInteraction = 1f)
    {
        //adds interaction sphere generator to list of sphere enactions

        //      interesting idea:  note the FUNCTION generating input "projectileStartPoint1()":
        //projectileGenerator(enactionAuthor, "shootFlamethrower1", projectileStartPoint1(), authorSensorySystem.lookingRay.direction, false, 10, 0.3f, 10);

        intSpherAtor newIntSpherAtor = new intSpherAtor();
        newIntSpherAtor.hitscan = true;
        newIntSpherAtor.range = inputRange;
        newIntSpherAtor.timeUntilSelfDestruct = 0;

        newIntSpherAtor.enactionAuthor = author;
        newIntSpherAtor.firePoint = this.firePoint;
        newIntSpherAtor.interactionType = interactionType;
        newIntSpherAtor.sdOnCollision = false;
        newIntSpherAtor.magnitudeOfInteraction = magnitudeOfInteraction;

        //Debug.Log("timeUntilSelfDestructzzzzzzzzzzzzzzzzzzz:  " + timeUntilSelfDestruct);

        newIntSpherAtor.growthSpeed = 0;


        interactionSphereList.Add(newIntSpherAtor);
    }



    */





}

public interface IEnactaBool
{

    GameObject enactionAuthor
    {
        get; set;
    }

    virtualGamepad.buttonCategories gamepadButtonType {  get; set; }
    enactionCreator.interType interactionType {  get; set; }

    //only for bool inputs!
    void enact();
}

public interface IEnactaVector
{

    virtualGamepad.buttonCategories gamepadButtonType { get; set; }


    //only for vector inputs!
    void enact(Vector2 inputVector);
}

public interface IEnactByTargetVector
{
    vectorEnactionSubType theTYPEofSubEnactable { get; set; }

    //virtualGamepad.buttonCategories gamepadButtonType { get; set; }


    //only for vector inputs!
    void enact(Vector3 inputVector);
}

public class byTargetVector : IEnactByTargetVector
{
    IEnactByTargetVector theSubEnactable;

    public vectorEnactionSubType theTYPEofSubEnactable { get; set; }  // "legibleType"??


    byTargetVector(IEnactByTargetVector theInputSubType)
    {
        //or use "IEnactByTargetVectorSubType", and give them the enact function but named "enactSUBTYPE"???
        //or is  this redundant?  just use THOSE on bodies/virtualGamepad???
        //but, ai needs to know which is which.  so.  need "type" info accessible.
        //and don't want to have to do "if" branches every time they enact.  hence this solution.  ok.
        //errrrr, but i just put that info in the sub-types anyways
        //so ya, this class is redundant, don't use it
        theSubEnactable = theInputSubType;

        theTYPEofSubEnactable = theSubEnactable.theTYPEofSubEnactable;
    }

    public void enact(Vector3 inputVector)
    {
        theSubEnactable.enact(inputVector);
    }


    //interface IEnactByTargetVectorSubType
}

public class navAgent: IEnactByTargetVector
{
    NavMeshAgent theAgent;

    public vectorEnactionSubType theTYPEofSubEnactable { get; set; }
    //virtualGamepad.buttonCategories gamepadButtonType { get; set; }

    //IEnactByTargetVectorSubType theSubType;

    public navAgent(GameObject objectToAddNavmeshAgentTo)
    {
        theTYPEofSubEnactable = vectorEnactionSubType.navMesh;


        theAgent = objectToAddNavmeshAgentTo.GetComponent<NavMeshAgent>();
        if (theAgent == null)
        {

            theAgent = objectToAddNavmeshAgentTo.AddComponent<NavMeshAgent>();
        }

        theAgent.baseOffset = 1f; //prevent stutter, being in floor
    }

    //only for vector inputs!
    public void enact(Vector3 inputVector)
    {

        //Debug.Log("destination:  " + theAgent.destination);
        //Debug.Log("inputVector:  " + inputVector);
        
        if(theAgent.enabled == false)
        {
            Debug.Log("theAgent.transform.gameObject:  " + theAgent.transform.gameObject);
            Debug.DrawLine(new Vector3(), theAgent.transform.position, Color.magenta, 200f);
        }
        
        theAgent.SetDestination(inputVector);
        //theAgent.SetDestination(new Vector3());

        //Debug.Log("2222222222222222destination:  " + theAgent.destination);
    }




}
public class aimTarget : IEnactByTargetVector
{
    GameObject thePartToAimHorizontal;  //no, just use the "mouse"/"joystick" controls for this one???  ehhhhhh......?

    GameObject thePartToAimVertical;

    public vectorEnactionSubType theTYPEofSubEnactable { get; set; }
    //virtualGamepad.buttonCategories gamepadButtonType { get; set; }

    //IEnactByTargetVectorSubType theSubType;

    public aimTarget(GameObject inputPartToAimHorizontal, GameObject inputPartToAimVertical)
    {
        theTYPEofSubEnactable = vectorEnactionSubType.Aim;
        this.thePartToAimHorizontal = inputPartToAimHorizontal;
        this.thePartToAimVertical = inputPartToAimVertical;

        /*
        theAgent = objectToAddNavmeshAgentTo.GetComponent<NavMeshAgent>();
        if (theAgent == null)
        {

            theAgent = objectToAddNavmeshAgentTo.AddComponent<NavMeshAgent>();
        }
        */
    }

    //only for vector inputs!
    public void enact(Vector3 inputVector)
    {
        throw new NotImplementedException();
    }




}

public class intSpherAtor : IEnactaBool
{
    //short for "interaction sphere generator"

    //types:
    //      regular [bullet]
    //      area of effect/splash/growth [explosion or standardclick]
    //          which has subtype that is ranged and instant, like standard click


    //have variables to save values that an NPC [or gamepad] will need to "read" to use it
    //other values should be plugged in by the body/vehicle that has this enaction


    public GameObject enactionAuthor { get; set; }

    //      public GameObject firePoint;
    //public GameObject firePoint;
    public Transform firePoint;
    //      public enactionScript theEnactionScript;  //looks at this script to see if "fire" button has been pressed, etc

    //      public string gamepadButtonType;
    public virtualGamepad.buttonCategories gamepadButtonType { get; set; }

    //      public enactionCreator.interType interactionType;
    public enactionCreator.interType interactionType { get; set; }
    public float magnitudeOfInteraction = 1f;

    public bool hitscan = false;

    public float range = 7f;
    //public Ray firingRay;  //has to be filled RIGHT at moment of firing, so i will have it as an INPUT variable, not permanent
    public bool sdOnCollision = true; //"projectileSelfDestructOnCollision"
    public int timeUntilSelfDestruct = 99; //"timeUntilProjectileSelfDestruct"
    public bool affectedByGravity = false;

    public float growthSpeed = 0f;


    //need to put these somewhere else?  no, this class object basically IS the gun [or at least this relevant PART of the gun]?
    public int firingCooldown = 0;
    public int firingCooldownMax = 0;
    //      can i do without the following?
    public float enactionCost = 1f;  //maybe like ammo?


    //unsure i want/need these:
    public GameObject enactionTarget;
    //public sensorySystem authorSensorySystem;
    public GameObject returnClickedOn;


    public intSpherAtor(Transform inputFirePoint, enactionCreator.interType inputInteractionType, virtualGamepad.buttonCategories inputGamepadButtonType, float inputMagnitudeOfInteraction = 1f, bool inputHitscan = false)
    {
        //enactionCreator.interType interactionType,
        //bool sdOnCollision = true,
        //int timeUntilSelfDestruct = 99,
        //float growthSpeed = 0f,
        //float magnitudeOfInteraction = 1f),
        //float theRange,
        //bool hitscan = false


        interactionType = inputInteractionType;

        firePoint = inputFirePoint;
        gamepadButtonType = inputGamepadButtonType;
        magnitudeOfInteraction = inputMagnitudeOfInteraction;
        hitscan = inputHitscan;
        range = 7f;
        sdOnCollision = true;
        timeUntilSelfDestruct = 99;
        affectedByGravity = false;
        growthSpeed = 0f;




    }


    public void enact()
    {
        //Debug.Log("enact this.  enactionAuthor, interactionType:  " + enactionAuthor + ", "  + interactionType);
        //, sensorySystem inputAuthorSensorySystem
        //authorSensorySystem = inputAuthorSensorySystem;

        if (hitscan == true)
        {
            firingByRaycastHit(range);
            //return;  //turn it off to make NPCs fire "standard click", for faster random testing
        }


        projectileGenerator(firePoint, interactionType, sdOnCollision, timeUntilSelfDestruct, growthSpeed, magnitudeOfInteraction);
    }

    public void firingByRaycastHit(float theRange)
    {
        //Vector3 startPoint = authorSensorySystem.pointerOrigin();
        //Vector3 endPoint = enactionTarget.gameObject.transform.position;
        //authorSensorySystem.lookingRay = new Ray(startPoint, (endPoint - startPoint));
        //          Debug.Log("firingByRaycastHit");

        RaycastHit myHit;
        //      Ray myRay = authorSensorySystem.lookingRay;
        //Ray myRay = theEnactionScript.primaryRay;
        Ray myRay = new Ray(firePoint.transform.position + firePoint.transform.forward, firePoint.transform.forward);

        if (Physics.Raycast(myRay, out myHit, theRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) == false) { return; }

        //&& myHit.transform.gameObject == enactionTarget
        if (myHit.transform == null){return; }

        GameObject anInteractionSphere = repository2.singleton.interactionSphere;
        GameObject thisObject = genGen.singleton.createPrefabAtPointAndRETURN(anInteractionSphere, myHit.point);




        //      should this use "interactionMate" isntead?
        //authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
        //theAuthorScript.enacting = this;
        genAuthorScript1(thisObject);

        //theAuthorScript.theAuthor = enactionAuthor;
        //theAuthorScript.interactionType = interactionType;



        //see how far interactionSphere is from it's supposed target:
        //Debug.DrawLine(thisObject.transform.position, enactionTarget.transform.position, Color.red, 0.9f);
        //                  deleteThisEnaction = true;


        firingCooldown--;
    }

    public void projectileGenerator(Transform firePoint, enactionCreator.interType interactionType, bool sdOnCollision = true, int timeUntilSelfDestruct = 99, float growthSpeed = 0f, float magnitudeOfInteraction = 1f)
    {

        //      GameObject prefabToUse = repository2.singleton.interactionSphere;

        //Debug.Log("startPoint:  " + startPoint);

        //      GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, startPoint);


        GameObject prefabToUse = repository2.singleton.interactionSphere;

        //Debug.Log("startPoint:  " + startPoint);

        //Vector3 startPoint = theEnactionScript.primaryRay.origin;
        //      Vector3 startPoint = theEnactionScript.primaryFiringPoint.transform.position;
        //  Vector3 startPoint = author.transform.position + author.transform.forward;
        //Vector3 startPoint = this.firePoint.transform.position + this.firePoint.transform.forward;
        Vector3 startPoint = firePoint.position + firePoint.forward;

        GameObject newProjectile = genGen.singleton.createPrefabAtPointAndRETURN(prefabToUse, startPoint);







        //UnityEngine.Object.Destroy(thisObject.GetComponent<selfDestructScript1>());

        //Debug.Log("newProjectile.transform.position:  " + newProjectile.transform.position);

        //newProjectile.transform.position += enactionBody.lookingRay.direction;
        //theInteractionMate.interactionAuthor.transform.position + new Vector3(0, 0, 0)
        projectile1 projectileScript = newProjectile.AddComponent<projectile1>();
        projectileScript.speed = 1f;
        //              projectileScript.Direction = theEnactionScript.primaryRay.direction;
        projectileScript.Direction = this.firePoint.transform.forward;
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


        genAuthorScript1(newProjectile);

        /*
        //      should this use "interactionMate" isntead?

        authorScript1 theAuthorScript = newProjectile.GetComponent<authorScript1>();
        theAuthorScript.theAuthor = author;
        //theAuthorScript.enactThisInteraction = theInteractionMate.enactThisInteraction;
        //theAuthorScript.interactionType = "bullet1";
        theAuthorScript.interactionType = interactionType;
        theAuthorScript.magnitudeOfInteraction = magnitudeOfInteraction;
        */

        //Debug.Log("projectile made supposedly");
        //mastLine(startPoint, Color.red);
        //mastLine(newProjectile.transform.position, Color.blue);


        //Debug.DrawLine(newProjectile.transform.position, new Vector3(), Color.red);

        //threatAlert(this);
    }

    private void genAuthorScript1(GameObject thisObject)
    {
        authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
        theAuthorScript.enacting = this;
    }

    void threatAlert(GameObject theThreat)
    {
        Debug.Log("need to fix threatAlert for TANK");

        //take the threat object, add them to the threat list in this/their "map zone"
        //try not to add them as duplicate if they are already added

        //      #1, access their map zone:
        //AIHub2 thisHub = theThreat.GetComponent<AIHub2>();
        //              body1 thisBody = theThreat.GetComponent<body1>();
        //hmm, lists like this always go bad though if the object is destryed......but....ad-hoc.....[and i have such a list on map zones ALREADY]
        //              List<GameObject> thisThreatList = thisBody.theLocalMapZoneScript.threatList;

        //      #2, add them to a "list of threats" if they aren't already
        //[hmmmm, would be easier to use tags?  easier to code it, but that system KILLS game performance.....]
        //[what about adding a child object, with a UNITY tag that is relevant?  is that faster?  one way to find out.......?]
        //              if (isFuckingThingOnListAlready(theThreat, thisThreatList))
        {
            //do nothing, they are already on the list
        }
        //              else
        {
            //              thisThreatList.Add(theThreat);
        }


    }



    void mastLine(Vector3 startPoint, Color theColor, float theHeight = 10f)
    {
        Vector3 p1 = startPoint;
        Vector3 p2 = new Vector3(p1.x, p1.y + theHeight, p1.z);
        Debug.DrawLine(p1, p2, theColor, 22f);
    }

}


public abstract class vectorMovement : IEnactaVector
{
    public CharacterController controller;
    public Transform theTransform;
    public float speed = 0f;

    public virtualGamepad.buttonCategories gamepadButtonType { get; set; }

    public abstract void enact(Vector2 inputVector);

}

public class vecTranslation : vectorMovement
{
    //translation motion, like walking forward/back, and STRAFING left/right
    


    bool screenPlaneInsteadoOfHorizonPlane = false;  //like moving up/down and left/right in starfox.  ad-hoc for now


    public vecTranslation(float inputSpeed, Transform theTransform, virtualGamepad.buttonCategories gamepadButtonType, bool screenPlaneInsteadoOfHorizonPlane = false, bool navmeshToo = true)
    {
        speed = inputSpeed;
        this.screenPlaneInsteadoOfHorizonPlane = screenPlaneInsteadoOfHorizonPlane;
        this.theTransform = theTransform;
        this.gamepadButtonType = gamepadButtonType;



        controller = theTransform.GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = theTransform.gameObject.AddComponent<CharacterController>();
        }

        /*
        //maybe automatically add navmesh here too???  by default
        if (navmeshToo && theTransform.GetComponent<NavMeshAgent>() == null)
        {
            theTransform.gameObject.AddComponent<NavMeshAgent>();
        
            theAgent.baseOffset = 1; //prevent stutter, being in floor
        }
        */
    }


    public override void enact(Vector2 inputVector)
    {
        //Debug.Log("inputVector:  " + inputVector);

        Vector3 move = theTransform.right * inputVector.x + theTransform.forward * inputVector.y;

        //move.y = 0f;
        controller.Move(move * speed * Time.deltaTime);
        //controller.Move(move * speed);

    }
}

public class vecRotation : vectorMovement
{
    //rotation motion, like turning left/right, looking up/down

    Transform thePartToAimHorizontal;  //no, just use the "mouse"/"joystick" controls for this one???  ehhhhhh......?
    Transform thePartToAimVertical;
    //CharacterController thePartToAimHorizontal;  //no, just use the "mouse"/"joystick" controls for this one???  ehhhhhh......?
    //CharacterController thePartToAimVertical;

    float yawSpeed = 1f;
    float pitchSpeed = 1f;

    float limitedPitchRotation = 0f;


    public vecRotation(float inputSpeed, Transform theHorizontalTransform, Transform theVerticalTransform, virtualGamepad.buttonCategories gamepadButtonType)
    {
        speed = inputSpeed;
        yawSpeed = inputSpeed/444;
        pitchSpeed = inputSpeed/444;



        //this.theTransform = theTransform;
        thePartToAimHorizontal = theHorizontalTransform;
        thePartToAimVertical = theVerticalTransform;
        this.gamepadButtonType = gamepadButtonType;

        /*
        thePartToAimHorizontal = theHorizontalTransform.GetComponent<CharacterController>();
        if (thePartToAimHorizontal == null)
        {
            thePartToAimHorizontal = theHorizontalTransform.gameObject.AddComponent<CharacterController>();
        }

        thePartToAimVertical = theVerticalTransform.GetComponent<CharacterController>();
        if (thePartToAimVertical == null)
        {
            thePartToAimVertical = theVerticalTransform.gameObject.AddComponent<CharacterController>();
        }

        */

        //maybe automatically add navmesh here too???  by default
    }

    public override void enact(Vector2 inputVector)
    {

        //Debug.Log("inputVector:  " + inputVector);

        //Vector3 move = theTransform.right * inputVector.x + theTransform.forward * inputVector.y;

        //controller.Move(move * speed * Time.deltaTime);

        //      thePartToAimHorizontal.RotateAround(thePartToAimHorizontal.position, thePartToAimHorizontal.up, inputVector.x);
        //      thePartToAimVertical.RotateAround(thePartToAimVertical.position, thePartToAimVertical.right, inputVector.y);
        //controller.Move(move * speed * Time.deltaTime);
        //thePartToAimVertical
        updatePitch(inputVector.y);
        updateYaw(inputVector.x);
    }

    void updatePitch(float pitchInput)
    {
        //Debug.Log("11111111111111thePartToAimVertical.transform.gameObject:  " + thePartToAimVertical.transform.gameObject);

        limitedPitchRotation -= pitchInput*pitchSpeed;

        //Debug.Log("limitedPitchRotation:  " + limitedPitchRotation);
        limitedPitchRotation = Mathf.Clamp(limitedPitchRotation, -90f, 90f);
        //Debug.Log("limitedPitchRotation:  " + limitedPitchRotation);
        //   thePartToAimVertical.Rotate(thePartToAimVertical.right * limitedPitchRotation);
        //Debug.Log("111111111111111111thePartToAimVertical.GetInstanceID():  " + thePartToAimVertical.transform.GetInstanceID());
        //Debug.Log("thePartToAimHorizontal.transform x+y+z:  X:  " + thePartToAimHorizontal.transform.rotation.x + "  Y:  " + thePartToAimHorizontal.transform.rotation.y + "  Z:  " + thePartToAimHorizontal.transform.rotation.z + "  W:  " + thePartToAimHorizontal.transform.rotation.w);
        //Debug.Log("thePartToAimVertical.transform x+y+z:  X:  " + thePartToAimVertical.transform.rotation.x + "  Y:  " + thePartToAimVertical.transform.rotation.y + "  Z:  " + thePartToAimVertical.transform.rotation.z + "  W:  " + thePartToAimVertical.transform.rotation.w);
        thePartToAimVertical.localRotation = Quaternion.Euler(limitedPitchRotation, 0f, 0f);
        //thePartToAimVertical.localRotation = Quaternion.AngleAxis(limitedPitchRotation, thePartToAimVertical.right);
        //Debug.Log("thePartToAimHorizontal.transform x+y+z:  X:  " + thePartToAimHorizontal.transform.rotation.x + "  Y:  " + thePartToAimHorizontal.transform.rotation.y + "  Z:  " + thePartToAimHorizontal.transform.rotation.z + "  W:  " + thePartToAimHorizontal.transform.rotation.w);
        //Debug.Log("thePartToAimVertical.transform x+y+z:  X:  " + thePartToAimVertical.transform.rotation.x + "  Y:  " + thePartToAimVertical.transform.rotation.y + "  Z:  " + thePartToAimVertical.transform.rotation.z + "  W:  " + thePartToAimVertical.transform.rotation.w);
        //thePartToAimVertical.Rotate(thePartToAimHorizontal.right * pitchInput);

        //thePartToAimVertical.localRotation = Quaternion.(limitedPitchRotation, 0f, 0f);
        //                  playerBody.Rotate(Vector3.up * theVirtualGamePad.mouseX);
    }

    /*
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
    */

    void updateYaw(float yawInput)
    {

        //                  xRotation -= theEnactionScript.mouseY;
        //                  xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //                  transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //Debug.Log("thePilotEnactionScript.yawInput:  " + thePilotEnactionScript.yawInput);

        //Debug.Log("222222222222222222222222thePartToAimHorizontal.transform.gameObject:  " + thePartToAimHorizontal.transform.gameObject);

        //Debug.Log("222222222222222222222222thePartToAimHorizontal.GetInstanceID():  " + thePartToAimHorizontal.transform.GetInstanceID());
        //Debug.Log("thePartToAimHorizontal.transform x+y+z:  X:  " + thePartToAimHorizontal.transform.rotation.x + "  Y:  " + thePartToAimHorizontal.transform.rotation.y + "  Z:  " + thePartToAimHorizontal.transform.rotation.z + "  W:  " + thePartToAimHorizontal.transform.rotation.w);
        thePartToAimHorizontal.Rotate(thePartToAimHorizontal.up * yawInput * yawSpeed);
        //Debug.Log("thePartToAimHorizontal.transform x+y+z:  X:  " + thePartToAimHorizontal.transform.rotation.x + "  Y:  " + thePartToAimHorizontal.transform.rotation.y + "  Z:  " + thePartToAimHorizontal.transform.rotation.z + "  W:  " + thePartToAimHorizontal.transform.rotation.w);
        //float xRotation = yawInput * yawSpeed;
        //thePartToAimHorizontal.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }


}

public class turningWithNoStrafe : vectorMovement
{
    //translation motion, like walking forward/back, and STRAFING left/right

    float yawSpeed = 11f;


    bool screenPlaneInsteadoOfHorizonPlane = false;  //like moving up/down and left/right in starfox.  ad-hoc for now


    public turningWithNoStrafe(float inputSpeed, Transform theTransform, virtualGamepad.buttonCategories gamepadButtonType)
    {
        speed = inputSpeed;
        this.theTransform = theTransform;
        this.gamepadButtonType = gamepadButtonType;
        yawSpeed = inputSpeed / 10;


        controller = theTransform.GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = theTransform.gameObject.AddComponent<CharacterController>();
        }

        /*
        //maybe automatically add navmesh here too???  by default
        if (navmeshToo && theTransform.GetComponent<NavMeshAgent>() == null)
        {
            theTransform.gameObject.AddComponent<NavMeshAgent>();
        
            theAgent.baseOffset = 1; //prevent stutter, being in floor
        }
        */
    }


    public override void enact(Vector2 inputVector)
    {
        //Debug.Log("inputVector:  " + inputVector);

        Vector3 translate = theTransform.forward * inputVector.y; //needs to be Y!!!!!!!!!!  //and FORWARD vector!!!!!!!!!!

        //move.y = 0f;
        controller.Move(translate * speed * Time.deltaTime);

        //+theTransform.forward * inputVector.y;  //needs to be X!!!!!!!!!!
        updateYaw(inputVector.x);
        //controller.Move(move * speed);

    }


    void updateYaw(float yawInput)
    {

        //                  xRotation -= theEnactionScript.mouseY;
        //                  xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //                  transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //Debug.Log("thePilotEnactionScript.yawInput:  " + thePilotEnactionScript.yawInput);

        //Debug.Log("222222222222222222222222thePartToAimHorizontal.transform.gameObject:  " + thePartToAimHorizontal.transform.gameObject);

        //Debug.Log("222222222222222222222222thePartToAimHorizontal.GetInstanceID():  " + thePartToAimHorizontal.transform.GetInstanceID());
        //Debug.Log("thePartToAimHorizontal.transform x+y+z:  X:  " + thePartToAimHorizontal.transform.rotation.x + "  Y:  " + thePartToAimHorizontal.transform.rotation.y + "  Z:  " + thePartToAimHorizontal.transform.rotation.z + "  W:  " + thePartToAimHorizontal.transform.rotation.w);
        theTransform.Rotate(theTransform.up * yawInput * yawSpeed);
        //Debug.Log("thePartToAimHorizontal.transform x+y+z:  X:  " + thePartToAimHorizontal.transform.rotation.x + "  Y:  " + thePartToAimHorizontal.transform.rotation.y + "  Z:  " + thePartToAimHorizontal.transform.rotation.z + "  W:  " + thePartToAimHorizontal.transform.rotation.w);
        //float xRotation = yawInput * yawSpeed;
        //thePartToAimHorizontal.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }




}


