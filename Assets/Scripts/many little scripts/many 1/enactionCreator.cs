using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using static enactionCreator;
using static UnityEngine.GraphicsBuffer;

public class enactionCreator : MonoBehaviour
{
    public static enactionCreator singleton;


    public enum interType
    {
        errorYouDidntSetEnumTypeForINTERTYPE,
        self,
        standardClick,
        shoot1,
        shootFlamethrower1,
        tankShot
    }
    
    
    public enum vectorEnactionSubType  // "legibleType"???
    {
        errorYouDidntSetEnumTypeForVECTORENACTIONSUBTYPE,
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
    /*
    GameObject enactionAuthor
    {
        get; set;
    }
    */

    virtualGamepad.buttonCategories gamepadButtonType {  get; set; }
    interactionInfo interInfo { get; set; }

    //      enactionCreator.interType interactionType {  get; set; }

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
            Debug.Log("theAgent.enabled == false, theAgent.transform.gameObject:  " + theAgent.transform.gameObject);
            Debug.DrawLine(new Vector3(), theAgent.transform.position, Color.magenta, 200f);
        }
        
        theAgent.SetDestination(inputVector);
        //theAgent.SetDestination(new Vector3());

        //Debug.Log("2222222222222222destination:  " + theAgent.destination);
    }




}

public class aimTarget : IEnactByTargetVector
{
    public vecRotation theVectorRotationEnaction;

    public vectorEnactionSubType theTYPEofSubEnactable { get; set; }
    //virtualGamepad.buttonCategories gamepadButtonType { get; set; }

    //IEnactByTargetVectorSubType theSubType;

    public aimTarget(vecRotation theInputVectorRotationEnaction)
    {
        theVectorRotationEnaction = theInputVectorRotationEnaction;
    }

    public void addToBothLists(List<IEnactaVector> enactableVectorSet, List<IEnactByTargetVector> enactableTARGETVectorSet)
    {
        enactableVectorSet.Add(theVectorRotationEnaction);

        enactableTARGETVectorSet.Add(this);
    }

    //only for vector inputs!
    public void enact(Vector3 targetPosition)
    {
        //Debug.Log("aimTarget.  enactionAuthor:  ");

        //instantaneous for now
        Vector3 lineFromVertAimerToTarget = targetPosition - theVectorRotationEnaction.thePartToAimVertical.position;
        //Vector2 theXYvectorCoordinates = calculateVector2(inputVector);

        //theVectorRotationEnaction.enact(theXYvectorCoordinates);
        //worldScript.singleton.debugToggle =true;

        //Debug.DrawLine(theVectorRotationEnaction.thePartToAimVertical.position,theVectorRotationEnaction.thePartToAimVertical.position + 10*theVectorRotationEnaction.thePartToAimVertical.forward,Color.white, 44f);
        Debug.DrawLine(theVectorRotationEnaction.thePartToAimVertical.position, targetPosition,Color.yellow, 44f);

        theVectorRotationEnaction.updateYaw(translateAngleIntoYawSpeedEtc(getHorizontalAngle(lineFromVertAimerToTarget)));
        theVectorRotationEnaction.updatePitch(translateAngleIntoPitchSpeedEtc(getVerticalAngle(lineFromVertAimerToTarget)));
        //theVectorRotationEnaction.updateYaw(translateAngleIntoYawSpeedEtc(-90));
        //theVectorRotationEnaction.updatePitch(translateAngleIntoPitchSpeedEtc(getVerticalAngle(inputVector)));

        //Debug.DrawLine(theVectorRotationEnaction.thePartToAimVertical.position,theVectorRotationEnaction.thePartToAimVertical.position + 10*theVectorRotationEnaction.thePartToAimVertical.forward,Color.blue, 44f);
        //Debug.DrawLine(theVectorRotationEnaction.thePartToAimVertical.position, targetPosition,Color.red, 44f);

        //worldScript.singleton.debugToggle = false;
    }

    private float translateAngleIntoYawSpeedEtc(float angle)
    {
        //Debug.Log(":::::::::::::::::::translateAngleIntoYawSpeedEtc::::::::::::::::::::");
        //Debug.Log("angle:  " + angle);
        //Debug.Log("theVectorRotationEnaction.yawSpeed:  " + theVectorRotationEnaction.yawSpeed);
        //Debug.Log("angle / (theVectorRotationEnaction.yawSpeed):  " + angle / (theVectorRotationEnaction.yawSpeed));
        return angle / (theVectorRotationEnaction.yawSpeed);
    }

    private float translateAngleIntoPitchSpeedEtc(float angle)
    {
        //Debug.Log("----------------------translateAngleIntoPitchSpeedEtc:----------------------");
        //Debug.Log("angle:  " + angle);
        //Debug.Log("theVectorRotationEnaction.pitchSpeed:  " + theVectorRotationEnaction.pitchSpeed);
        //Debug.Log("angle / (theVectorRotationEnaction.pitchSpeed):  " + angle / (theVectorRotationEnaction.pitchSpeed));

        return angle / theVectorRotationEnaction.pitchSpeed;
    }

    Vector2 calculateVector2(Vector3 inputVector)
    {
        float horizontal = getHorizontalAngle(inputVector);
        float vertical = getVerticalAngle(inputVector);


        return new Vector2(horizontal, vertical);
    }

    private float getHorizontalAngle(Vector3 lineToTarget)
    {
        //float inputHorizontalAngle = Vector3.Angle(inputVector, theVectorRotationEnaction.thePartToAimHorizontal.forward);
        //float currentAimingHorizontalAngle = Vector3.Angle(inputVector, theVectorRotationEnaction.thePartToAimHorizontal.forward);


        //float oneAngle = Vector3.Angle(inputVector, theVectorRotationEnaction.thePartToAimHorizontal.forward);
        //      float oneAngle = Vector3.SignedAngle(inputVector, theVectorRotationEnaction.thePartToAimHorizontal.forward, theVectorRotationEnaction.thePartToAimHorizontal.up);

        /*
        Vector3 currentPerpendicularToRotationAxis = theVectorRotationEnaction.thePartToAimHorizontal.forward;
        //Vector3 desiredPerpendicularToRotationAxis = findOnePerpendicularVector(inputVector.normalized, theVectorRotationEnaction.thePartToAimHorizontal.up);
        Vector3 desiredPerpendicularToRotationAxis = findOnePerpendicularVector(theVectorRotationEnaction.thePartToAimHorizontal.up, inputVector.normalized);

        float oneAngle = Vector3.Angle(currentPerpendicularToRotationAxis, desiredPerpendicularToRotationAxis);

        */

        //fucking hell:
        //https://forum.unity.com/threads/is-vector3-signedangle-working-as-intended.694105/

        float oneAngle = AngleOffAroundAxis(lineToTarget.normalized, theVectorRotationEnaction.thePartToAimHorizontal.forward, theVectorRotationEnaction.thePartToAimHorizontal.up);



        return oneAngle;
    }

    private float getVerticalAngle(Vector3 lineToTarget)
    {
        //float oneAngle = Vector3.Angle(inputVector, theVectorRotationEnaction.thePartToAimVertical.forward);
        //      float oneAngle = Vector3.SignedAngle(inputVector, theVectorRotationEnaction.thePartToAimVertical.forward, theVectorRotationEnaction.thePartToAimHorizontal.right);


        //RotateAround(Vector3 point, Vector3 axis, float angle); 
        //and need perpendicular components [perpendicular to axis of rotation]


        /*
        Vector3 currentPerpendicularToRotationAxis = theVectorRotationEnaction.thePartToAimVertical.forward;
        //Vector3 desiredPerpendicularToRotationAxis = findOnePerpendicularVector(inputVector.normalized, theVectorRotationEnaction.thePartToAimHorizontal.right);
        Vector3 desiredPerpendicularToRotationAxis = findOnePerpendicularVector(theVectorRotationEnaction.thePartToAimHorizontal.right, inputVector.normalized);

        float oneAngle = Vector3.Angle(currentPerpendicularToRotationAxis, desiredPerpendicularToRotationAxis);

        */

        //float oneAngle = AngleOffAroundAxis(inputVector, theVectorRotationEnaction.thePartToAimVertical.forward, theVectorRotationEnaction.thePartToAimHorizontal.right);
        //......aaaand, still not producing the rotation i want....gahh...
        //try normalized???
        //float oneAngle = AngleOffAroundAxis(inputVector.normalized, theVectorRotationEnaction.thePartToAimVertical.forward, theVectorRotationEnaction.thePartToAimHorizontal.right);
        //uhhhh, no, that's somehow catastrophically worse.
        float oneAngle = AngleOffAroundAxis(lineToTarget.normalized, theVectorRotationEnaction.thePartToAimVertical.forward, theVectorRotationEnaction.thePartToAimHorizontal.right);
        //fixed it!  my input vector was just target position!  i needed to be using line from aiming object to the target it is aiming at!  position relative to the person doing the aiming, basically

        //this one has to be negative for some reason??
        return -oneAngle;
    }


    public float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis, bool clockwise = false)
    {
        //from here:
        //https://forum.unity.com/threads/is-vector3-signedangle-working-as-intended.694105/

        //but had to change conversion thing from "MathUtil.RAD_TO_DEG" to the following:
        //Mathf.Rad2Deg


        Vector3 right;
        if (clockwise)
        {
            right = Vector3.Cross(forward, axis);
            forward = Vector3.Cross(axis, right);
        }
        else
        {
            right = Vector3.Cross(axis, forward);
            forward = Vector3.Cross(right, axis);
        }


        return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
    }


    public Vector3 findOnePerpendicularVector(Vector3 vector1, Vector3 vector2)
    {
        //https://docs.unity3d.com/2019.3/Documentation/Manual/ComputingNormalPerpendicularVector.html
        Vector3 perpendicular = new Vector3();

        perpendicular = Vector3.Cross(vector1, vector2);

        return perpendicular;
    }


}


public class projectileLauncher: rangedEnaction, IEnactaBool
{
    public virtualGamepad.buttonCategories gamepadButtonType { get; set; }
    public interactionInfo interInfo { get; set; }
    /*
    public Transform firePoint;
    //or put these in projectile info?  i guess here makes sense, ALL the info in this class is projectile info, but divide by PARTS, the bullet is a different part than the gun or whatever
    //public bool hitscan = false;  //just make a separate class for this?
    public float range = 99f;  //this can also be the time until projectile "self destruct"?
    //need to put these in a gun class, or "launcher"/"firer" class or something...and all the "generator" stuff above?:
    public int firingCooldown = 0;
    public int firingCooldownMax = 0;
    //      can i do without the following?
    //public float enactionCost = 1f;  //maybe like ammo? //wait until i'm ready to implement ammo, then clearly name it AMMO

    */


    public projectileLauncher(Transform firePoint, virtualGamepad.buttonCategories gamepadButtonType, interactionInfo interInfo, projectileInfo theProjectileInfo, float range = 99f)
    {
        this.gamepadButtonType = gamepadButtonType;
        this.interInfo = interInfo;
        this.theProjectileInfo = theProjectileInfo;

        this.firePoint = firePoint;
        this.range = range;
        //int firingCooldown = 0;
        //int firingCooldownMax = 0;
    }

    public void enact()
    {
        //Debug.Log("enact projectileLauncher");
        genGen.singleton.projectileGenerator(theProjectileInfo, this, firePoint.position+ firePoint.forward, firePoint.forward);
    }

}

public class hitscanEnactor: rangedEnaction, IEnactaBool
{
    public virtualGamepad.buttonCategories gamepadButtonType { get; set; }
    public interactionInfo interInfo { get; set; }
    /*
    public Transform firePoint;
    //or put these in projectile info?  i guess here makes sense, ALL the info in this class is projectile info, but divide by PARTS, the bullet is a different part than the gun or whatever
    public float range = 7f;
    //need to put these in a gun class, or "launcher"/"firer" class or something...and all the "generator" stuff above?:
    public int firingCooldown = 0;
    public int firingCooldownMax = 0;

    */


    public hitscanEnactor(Transform firePoint, virtualGamepad.buttonCategories gamepadButtonType, interactionInfo interInfo, projectileInfo theProjectileInfo, float range = 7f)
    {
        this.gamepadButtonType = gamepadButtonType;
        this.interInfo = interInfo;
        this.theProjectileInfo = theProjectileInfo;

        this.firePoint = firePoint;
        this.range = range;
        //int firingCooldown = 0;
        //int firingCooldownMax = 0;

        this.theProjectileInfo.timeUntilSelfDestruct = 0;
    }



    public void enact()
    {
        firingByRaycastHit(range);



        /*

        //Vector3 startPoint = authorSensorySystem.pointerOrigin();
        //Vector3 endPoint = enactionTarget.gameObject.transform.position;
        //authorSensorySystem.lookingRay = new Ray(startPoint, (endPoint - startPoint));
        //          Debug.Log("firingByRaycastHit");

        RaycastHit myHit;
        //      Ray myRay = authorSensorySystem.lookingRay;
        //Ray myRay = theEnactionScript.primaryRay;
        Ray myRay = new Ray(firePoint.transform.position + firePoint.transform.forward, firePoint.transform.forward);

        if (Physics.Raycast(myRay, out myHit, range, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) == false) { return; }

        //&& myHit.transform.gameObject == enactionTarget
        if (myHit.transform == null) { return; }

        GameObject anInteractionSphere = repository2.singleton.interactionSphere;
        GameObject thisObject = genGen.singleton.createPrefabAtPointAndRETURN(anInteractionSphere, myHit.point);
        //genGen.singleton.projectileGenerator();
        //      genGen.singleton.projectileGenerator(new projectileInfo(0), this, firePoint.position+firePoint.forward, firePoint.forward);


        //      should this use "interactionMate" isntead?
        //authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
        //theAuthorScript.enacting = this;
        //          GENAuthorScript1(thisObject);

        //theAuthorScript.theAuthor = enactionAuthor;
        //theAuthorScript.interactionType = interactionType;



        //see how far interactionSphere is from it's supposed target:
        //Debug.DrawLine(thisObject.transform.position, enactionTarget.transform.position, Color.red, 0.9f);
        //                  deleteThisEnaction = true;

        */

        firingCooldown--;
    }


    public void firingByRaycastHit(float theRange)
    {
        //Debug.Log("range:  "+ theRange);
        //Vector3 startPoint = authorSensorySystem.pointerOrigin();
        //Vector3 endPoint = enactionTarget.gameObject.transform.position;
        //authorSensorySystem.lookingRay = new Ray(startPoint, (endPoint - startPoint));
        //      Debug.Log("firingByRaycastHit");

        RaycastHit myHit;
        //      Ray myRay = authorSensorySystem.lookingRay;
        //Ray myRay = theEnactionScript.primaryRay;
        Ray myRay = new Ray(firePoint.transform.position + firePoint.transform.forward, firePoint.transform.forward);

        if (Physics.Raycast(myRay, out myHit, theRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) == false) { return; }

        //&& myHit.transform.gameObject == enactionTarget
        if (myHit.transform == null) { return; }

        GameObject anInteractionSphere = repository2.singleton.interactionSphere;
        GameObject thisObject = genGen.singleton.createPrefabAtPointAndRETURN(anInteractionSphere, myHit.point);
        selfDestructScript1 sds = thisObject.GetComponent<selfDestructScript1>();
        sds.timeUntilSelfDestruct = 0;


        //      should this use "interactionMate" isntead?
        //authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
        //theAuthorScript.enacting = this;
        //              genAuthorScript1(thisObject);

        authorScript1.FILLAuthorScript1(thisObject, this);


        Debug.DrawLine(this.interInfo.enactionAuthor.transform.position, myHit.transform.position, Color.yellow, 0.3f);


        //theAuthorScript.theAuthor = enactionAuthor;
        //theAuthorScript.interactionType = interactionType;



        //see how far interactionSphere is from it's supposed target:
        //Debug.DrawLine(thisObject.transform.position, enactionTarget.transform.position, Color.red, 0.9f);
        //                  deleteThisEnaction = true;


        firingCooldown--;
    }


    private void genAuthorScript1(GameObject thisObject)
    {
        authorScript1 theAuthorScript = thisObject.GetComponent<authorScript1>();
        theAuthorScript.enacting = this;
        theAuthorScript.enacting.interInfo = this.interInfo;
    }



}

public abstract class rangedEnaction
{
    public Transform firePoint;
    //or put these in projectile info?  i guess here makes sense, ALL the info in this class is projectile info, but divide by PARTS, the bullet is a different part than the gun or whatever
    public float range = 7f;
    //need to put these in a gun class, or "launcher"/"firer" class or something...and all the "generator" stuff above?:
    public int firingCooldown = 0;
    public int firingCooldownMax = 0;

    public projectileInfo theProjectileInfo;
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



    // orrrrr divie by part....
    //all INTERACTION info
    //all PROJECTILE info
    //all GAMEPAD info
    //all GENERATOR info, mere initialization stuff
    //etc



    //>>>>>>>>>> info needed when it collides with something
    //[should probably be the only info on here, right?  err...it's a generator...so...
    //....so this generator shouldn't store ANY info?  hmmm....i dunno.....just have ....
    //...for GENERATOR, just have info needed to put a new object in the game world
    //all other info should be packaged and ready to drop into the sub-components?  not a huge mess here?]:
    
    


    //>>>>>>>>>> all INTERACTION info
    public GameObject enactionAuthor { get; set; }
    public interactionInfo interInfo { get; set; }
    public enactionCreator.interType interactionType { get; set; }
    public float magnitudeOfInteraction = 1f;



    //>>>>>>>>>> all PROJECTILE info
    public bool sdOnCollision = true;
    public int timeUntilSelfDestruct = 99;
    public float growthSpeed = 0f;
    public bool affectedByGravity = false;



    //>>>>>>>>>> all GAMEPAD info
    public virtualGamepad.buttonCategories gamepadButtonType { get; set; }
    //interactionInfo IEnactaBool.theInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }



    //>>>>>>>>>> all GENERATOR info, mere initialization stuff
    //>>>>>>>>>> info needed to put a new object in the game world [THIS should probably be the only info on here, right?]:
    public Transform firePoint;
    //or put these in projectile info?  i guess here makes sense, ALL the info in this class is projectile info, but divide by PARTS, the bullet is a different part than the gun or whatever
    public bool hitscan = false;  //just make a separate class for this?
    public float range = 7f;
    //need to put these in a gun class, or "launcher"/"firer" class or something...and all the "generator" stuff above?:
    public int firingCooldown = 0;
    public int firingCooldownMax = 0;
    //      can i do without the following?
    public float enactionCost = 1f;  //maybe like ammo?







    /*

    //mmmmm, probably NOT gonna arrange it like this.  it's too vague and mixing things together, i dunno

    //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    //>>>>>>>>>> info needed when it collides with something
    //[should probably be the only info on here, right?  err...it's a generator...so...
    //....so this generator shouldn't store ANY info?  hmmm....i dunno.....just have ....
    //...for GENERATOR, just have info needed to put a new object in the game world
    //all other info should be packaged and ready to drop into the sub-components?  not a huge mess here?]:
    public GameObject enactionAuthor { get; set; }
    public enactionCreator.interType interactionType { get; set; }
    public float magnitudeOfInteraction = 1f;
    public bool sdOnCollision = true;

    //>>>>>>>>>> well, other info needed AFTER being generated:
    public int timeUntilSelfDestruct = 99;
    public float growthSpeed = 0f;
    public bool affectedByGravity = false;


    //>>>>>>>>>> info needed to put a new object in the game world [THIS should probably be the only info on here, right?]:
    public Transform firePoint;


    //>>>>>>>>>> needed ....BEFORE generating/firing it:
    public virtualGamepad.buttonCategories gamepadButtonType { get; set; }
    public bool hitscan = false;
    public float range = 7f;
    //need to put cooldowns somewhere else?  no, this class object basically IS the gun [or at least this relevant PART of the gun]?
    public int firingCooldown = 0;
    public int firingCooldownMax = 0;
    //      can i do without the following?
    public float enactionCost = 1f;  //maybe like ammo?


    //>>>>>>>>>> unsure i want/need these:
    //      public GameObject enactionTarget;
    //      public GameObject returnClickedOn;


    */


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
        //Debug.Log("enact this intSpherAtor.  enactionAuthor, interactionType:  " + enactionAuthor + ", "  + interactionType);
        //, sensorySystem inputAuthorSensorySystem
        //authorSensorySystem = inputAuthorSensorySystem;

        if (hitscan == true)
        {
            firingByRaycastHit(range);
            //return;  //turn it off to make NPCs fire "standard click", for faster random testing
        }

        //          genGen.singleton.projectileGenerator(this);
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





        //                      GENAuthorScript1(thisObject);








        //theAuthorScript.theAuthor = enactionAuthor;
        //theAuthorScript.interactionType = interactionType;



        //see how far interactionSphere is from it's supposed target:
        //Debug.DrawLine(thisObject.transform.position, enactionTarget.transform.position, Color.red, 0.9f);
        //                  deleteThisEnaction = true;


        firingCooldown--;
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

    public Transform thePartToAimHorizontal;  //no, just use the "mouse"/"joystick" controls for this one???  ehhhhhh......?
    public Transform thePartToAimVertical;
    //CharacterController thePartToAimHorizontal;  //no, just use the "mouse"/"joystick" controls for this one???  ehhhhhh......?
    //CharacterController thePartToAimVertical;

    public float yawSpeed = 1f;
    public float pitchSpeed = 1f;
    public float pitchRange = 70f;

    public float limitedPitchRotation = 0f;


    public vecRotation(float inputSpeed, Transform theHorizontalTransform, Transform theVerticalTransform, virtualGamepad.buttonCategories gamepadButtonType, float pitchRange = 70f)
    {
        speed = inputSpeed;
        yawSpeed = inputSpeed/444;
        pitchSpeed = inputSpeed/444;
        this.pitchRange = pitchRange;



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

    public void updatePitch(float pitchInput)
    {
        if (worldScript.singleton.debugToggle)
        {

            Debug.Log(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,updatePitch");
        }
        //Debug.Log("11111111111111thePartToAimVertical.transform.gameObject:  " + thePartToAimVertical.transform.gameObject);

        limitedPitchRotation -= pitchInput * pitchSpeed;
        //limitedPitchRotation -= 30;

        //Debug.Log("limitedPitchRotation:  " + limitedPitchRotation);
        //              limitedPitchRotation = Mathf.Clamp(limitedPitchRotation, -90f, 90f);
        limitedPitchRotation = Mathf.Clamp(limitedPitchRotation, -pitchRange, pitchRange);
        //Debug.Log("limitedPitchRotation:  " + limitedPitchRotation);
        //   thePartToAimVertical.Rotate(thePartToAimVertical.right * limitedPitchRotation);
        //Debug.Log("111111111111111111thePartToAimVertical.GetInstanceID():  " + thePartToAimVertical.transform.GetInstanceID());
        //Debug.Log("thePartToAimHorizontal.transform x+y+z:  X:  " + thePartToAimHorizontal.transform.rotation.x + "  Y:  " + thePartToAimHorizontal.transform.rotation.y + "  Z:  " + thePartToAimHorizontal.transform.rotation.z + "  W:  " + thePartToAimHorizontal.transform.rotation.w);
        //Debug.Log("thePartToAimVertical.transform x+y+z:  X:  " + thePartToAimVertical.transform.rotation.x + "  Y:  " + thePartToAimVertical.transform.rotation.y + "  Z:  " + thePartToAimVertical.transform.rotation.z + "  W:  " + thePartToAimVertical.transform.rotation.w);
        if (worldScript.singleton.debugToggle)
        {
            Debug.Log("thePartToAimVertical.transform.forward x+y+z:  X:  " + thePartToAimVertical.transform.forward.x + "  Y:  " + thePartToAimVertical.transform.forward.y + "  Z:  " + thePartToAimVertical.transform.forward.z);

        }
        thePartToAimVertical.localRotation = Quaternion.Euler(limitedPitchRotation, 0f, 0f);
        if (worldScript.singleton.debugToggle)
        {
            Debug.Log("thePartToAimVertical.transform.forward x+y+z:  X:  " + thePartToAimVertical.transform.forward.x + "  Y:  " + thePartToAimVertical.transform.forward.y + "  Z:  " + thePartToAimVertical.transform.forward.z);

        }
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

    public void updateYaw(float yawInput)
    {
        

        if (worldScript.singleton.debugToggle)
        {
            //Debug.Log("___________________________________updateYaw");
            //Debug.Log("yawSpeed:  " + yawSpeed);
            //Debug.Log("yawInput:  " + yawInput);
            //Debug.Log("yawInput * yawSpeed:  " + yawInput * yawSpeed);
            Vector3 thisThing = thePartToAimHorizontal.up * yawInput * yawSpeed;
            //Debug.Log("thePartToAimHorizontal.transform.up x+y+z:  X:  " + thePartToAimHorizontal.transform.up.x + "  Y:  " + thePartToAimHorizontal.transform.up.y + "  Z:  " + thePartToAimHorizontal.transform.up.z);
            //Debug.Log("thePartToAimHorizontal.up * yawInput * yawSpeed x+y+z:  X:  " + thisThing.x + "  Y:  " + thisThing.y + "  Z:  " + thisThing.z);
            //Debug.Log("thePartToAimHorizontal.transform.forward x+y+z:  X:  " + thePartToAimHorizontal.transform.forward.x + "  Y:  " + thePartToAimHorizontal.transform.forward.y + "  Z:  " + thePartToAimHorizontal.transform.forward.z);

        }
        //                  xRotation -= theEnactionScript.mouseY;
        //                  xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //                  transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //Debug.Log("yawSpeed:  " + yawSpeed);

        //Debug.Log("222222222222222222222222thePartToAimHorizontal.transform.gameObject:  " + thePartToAimHorizontal.transform.gameObject);

        //Debug.Log("222222222222222222222222thePartToAimHorizontal.GetInstanceID():  " + thePartToAimHorizontal.transform.GetInstanceID());

        thePartToAimHorizontal.Rotate(thePartToAimHorizontal.up * yawInput * yawSpeed);
        if (worldScript.singleton.debugToggle)
        {
            Debug.Log("thePartToAimHorizontal.transform.forward x+y+z:  X:  " + thePartToAimHorizontal.transform.forward.x + "  Y:  " + thePartToAimHorizontal.transform.forward.y + "  Z:  " + thePartToAimHorizontal.transform.forward.z);

        }
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


