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
    public enum vectorEnactionSubType  // "legibleType"???  "legibleUse"???
    {
        errorYouDidntSetEnumTypeForVECTORENACTIONSUBTYPE,
        navMesh,
        Aim
    }

    //uninformative?
    public enum buttonCategories
    {
        errorYouDidntSetEnumTypeForBUTTONCATEGORIES,
        primary,
        aux1,
        vector1,
        vector2,
        augment1
    }

    //need THIS instead/as well?
    public enum INFORMATIVEbuttonCategories
    {
        errorYouDidntSetEnumTypeForINFORMATIVEbuttonCategories,
        fire1,
        jump,
        move1,
    }


    void Awake()
    {
        //Debug.Log("Awake:  " + this);
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




}

public interface IEnactaBool
{
    //only for bool inputs!
    buttonCategories gamepadButtonType {  get; set; }
    //interactionInfo interInfo { get; set; }
    GameObject enactionAuthor { get; set; }

    void enact();
}

public interface IEnactaVector
{
    //only for vector inputs!
    buttonCategories gamepadButtonType { get; set; }

    void enact(Vector2 inputVector);
}

public interface IEnactByTargetVector
{
    //only for vector inputs!
    //vectorEnactionSubType theTYPEofSubEnactable { get; set; }  //???

    void enact(Vector3 inputVector);
}




public abstract class targetedEnaction : MonoBehaviour, IEnactByTargetVector
{


    public abstract void enact(Vector3 inputVector);

}


public class navAgent : targetedEnaction, IEnactByTargetVector
{

    //only for vector inputs!
    NavMeshAgent theAgent;


    public static void addNavAgentEnaction(GameObject objectToAddNavmeshAgentTo)
    {
        navAgent nA = objectToAddNavmeshAgentTo.AddComponent<navAgent>();



        nA.theAgent = objectToAddNavmeshAgentTo.GetComponent<NavMeshAgent>();
        if (nA.theAgent == null)
        {

            nA.theAgent = objectToAddNavmeshAgentTo.AddComponent<NavMeshAgent>();
        }

        nA.theAgent.baseOffset = 1f; //prevent stutter, being in floor


    }


    public navAgent(GameObject objectToAddNavmeshAgentTo)
    {


        theAgent = objectToAddNavmeshAgentTo.GetComponent<NavMeshAgent>();
        if (theAgent == null)
        {

            theAgent = objectToAddNavmeshAgentTo.AddComponent<NavMeshAgent>();
        }

        theAgent.baseOffset = 1f; //prevent stutter, being in floor
    }

    //only for vector inputs!
    override public void enact(Vector3 inputVector)
    {

        //Debug.Log("destination:  " + theAgent.destination);
        //Debug.Log("inputVector:  " + inputVector);

        if (theAgent.enabled == false)
        {
            Debug.Log("theAgent.enabled == false, theAgent.transform.gameObject:  " + theAgent.transform.gameObject);
            Debug.DrawLine(new Vector3(), theAgent.transform.position, Color.magenta, 200f);
        }

        theAgent.SetDestination(inputVector);
        //theAgent.SetDestination(new Vector3());

        //Debug.Log("2222222222222222destination:  " + theAgent.destination);
        //Debug.DrawLine(new Vector3(), theAgent.transform.position, Color.green, 5f);
        //Debug.DrawLine(new Vector3(), inputVector, Color.red, 5f);
        //Debug.DrawLine(theAgent.transform.position, inputVector, Color.black, 5f);
    }


}

public class aimTarget : targetedEnaction, IEnactByTargetVector
{

    //only for vector inputs!
    public vecRotation theVectorRotationEnaction; //????


    public static void addAaimTargetAndAimTranslation(GameObject objectToAddItTo, float inputSpeed, Transform theHorizontalTransform, Transform theVerticalTransform, buttonCategories gamepadButtonType, float pitchRange = 70f)
    {
        aimTarget aT = objectToAddItTo.AddComponent<aimTarget>();
        //new vecRotation(inputSpeed, theHorizontalTransform, theVerticalTransform, gamepadButtonType);

        aT.theVectorRotationEnaction = vecRotation.addVecRotationAndReturnIt(objectToAddItTo, inputSpeed, theHorizontalTransform, theVerticalTransform, gamepadButtonType);
    }


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
    override public void enact(Vector3 targetPosition)
    {
        //Debug.Log("aimTarget.  enactionAuthor:  ");

        //instantaneous for now
        Vector3 lineFromVertAimerToTarget = targetPosition - theVectorRotationEnaction.thePartToAimVertical.position;
        //Vector2 theXYvectorCoordinates = calculateVector2(inputVector);

        //theVectorRotationEnaction.enact(theXYvectorCoordinates);
        //worldScript.singleton.debugToggle =true;

        //Debug.DrawLine(theVectorRotationEnaction.thePartToAimVertical.position,theVectorRotationEnaction.thePartToAimVertical.position + 10*theVectorRotationEnaction.thePartToAimVertical.forward,Color.white, 44f);
        //      Debug.DrawLine(theVectorRotationEnaction.thePartToAimVertical.position, targetPosition,Color.yellow, 44f);

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
        //https://forum.unity.com/threads/is-vector3-signedangle-working-as-intended.694105/

        float oneAngle = AngleOffAroundAxis(lineToTarget.normalized, theVectorRotationEnaction.thePartToAimHorizontal.forward, theVectorRotationEnaction.thePartToAimHorizontal.up);



        return oneAngle;
    }

    private float getVerticalAngle(Vector3 lineToTarget)
    {
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



public abstract class collisionEnaction: MonoBehaviour, IEnactaBool
{ 
    public interactionInfo interInfo;
    public GameObject enactionAuthor { get; set; }
    public buttonCategories gamepadButtonType { get; set; }
    abstract public void enact();
}


public abstract class rangedEnaction: collisionEnaction
{
    public Transform firePoint;
    //or put these in projectile info?  i guess here makes sense, ALL the info in this class is projectile info, but divide by PARTS, the bullet is a different part than the gun or whatever
    public float range = 7f;
    //need to put these in a gun class, or "launcher"/"firer" class or something...and all the "generator" stuff above?:
    public int firingCooldown = 0;
    public int firingCooldownMax = 0;

}

public class projectileLauncher: rangedEnaction, IEnactaBool
{
    //public buttonCategories gamepadButtonType { get; set; }
    //public interactionInfo interInfo { get; set; }
    public projectileToGenerate theprojectileToGenerate;

    public static void addProjectileLauncher(GameObject objectToAddItTo, Transform firePoint, buttonCategories gamepadButtonType, interactionInfo interInfo, projectileToGenerate theprojectileToGenerate, float range = 99f)
    {
        projectileLauncher pL = objectToAddItTo.AddComponent<projectileLauncher>();
        pL.gamepadButtonType = gamepadButtonType;
        pL.interInfo = interInfo;
        pL.theprojectileToGenerate = theprojectileToGenerate;

        pL.firePoint = firePoint;
        pL.range = range;
    }

    public projectileLauncher(Transform firePoint, buttonCategories gamepadButtonType, interactionInfo interInfo, projectileToGenerate theprojectileToGenerate, float range = 99f)
    {
        this.gamepadButtonType = gamepadButtonType;
        this.interInfo = interInfo;
        this.theprojectileToGenerate = theprojectileToGenerate;

        this.firePoint = firePoint;
        this.range = range;

    }

    override public void enact()
    {
        //Debug.Log("enact projectileLauncher");
        genGen.singleton.projectileGenerator(theprojectileToGenerate, this, firePoint.position+ firePoint.forward, firePoint.forward);
    }

}

public class hitscanEnactor: rangedEnaction, IEnactaBool
{
    //public interactionInfo interInfo { get; set; }

    public static void addHitscanEnactor(GameObject objectToAddItTo, Transform firePoint, buttonCategories gamepadButtonType, interactionInfo interInfo, float range = 7f)
    {
        hitscanEnactor hE = objectToAddItTo.AddComponent<hitscanEnactor>();
        hE.gamepadButtonType = gamepadButtonType;
        hE.interInfo = interInfo;
        //hE.theprojectileToGenerate = theprojectileToGenerate;
        //hE.theprojectileToGenerate.timeUntilSelfDestruct = 0;

        hE.firePoint = firePoint;
        hE.range = range;
    }


    public hitscanEnactor(Transform firePoint, buttonCategories gamepadButtonType, interactionInfo interInfo, float range = 7f)
    {
        this.gamepadButtonType = gamepadButtonType;
        this.interInfo = interInfo;
        //this.theprojectileToGenerate = theprojectileToGenerate;
        //this.theprojectileToGenerate.timeUntilSelfDestruct = 0;

        this.firePoint = firePoint;
        this.range = range;

    }



    override public void enact()
    {
        firingByRaycastHit(range);

        firingCooldown--;
    }


    public void firingByRaycastHit(float theRange)
    {
        //Debug.Log("range:  "+ theRange);

        RaycastHit myHit;
        Ray myRay = new Ray(firePoint.transform.position + firePoint.transform.forward, firePoint.transform.forward);

        if (Physics.Raycast(myRay, out myHit, theRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) == false) { return; }
        if (myHit.transform == null) { return; }

        GameObject newInstantInteractionSphere = comboGen.singleton.instantInteractionSphere(myHit.point);
        colliderInteractor.genColliderInteractor(newInstantInteractionSphere, this);

        //      Debug.DrawLine(this.enactionAuthor.transform.position, myHit.transform.position, Color.yellow, 0.3f);
        firingCooldown--;
    }
    



}







public abstract class vectorMovement : MonoBehaviour, IEnactaVector
{
    public CharacterController controller;
    public Transform theTransform;
    public float speed = 0f;
    public buttonCategories gamepadButtonType { get; set; }


    public abstract void enact(Vector2 inputVector);

}

public class vecTranslation : vectorMovement
{
    //translation motion, like walking forward/back, and STRAFING left/right
    bool screenPlaneInsteadoOfHorizonPlane = false;  //like moving up/down and left/right in starfox.  ad-hoc for now

    public static void addVecTranslation(GameObject objectToAddItTo, float inputSpeed, buttonCategories gamepadButtonType, bool screenPlaneInsteadoOfHorizonPlane = false, bool navmeshToo = true)
    {
        vecTranslation vT = objectToAddItTo.AddComponent<vecTranslation>();

        vT.speed = inputSpeed;
        vT.screenPlaneInsteadoOfHorizonPlane = screenPlaneInsteadoOfHorizonPlane;
        vT.theTransform = objectToAddItTo.transform;
        vT.gamepadButtonType = gamepadButtonType;



        vT.controller = objectToAddItTo.GetComponent<CharacterController>();
        if (vT.controller == null)
        {
            vT.controller = objectToAddItTo.gameObject.AddComponent<CharacterController>();
        }

    }

    public vecTranslation(float inputSpeed, Transform theTransform, buttonCategories gamepadButtonType, bool screenPlaneInsteadoOfHorizonPlane = false, bool navmeshToo = true)
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

    }


    public override void enact(Vector2 inputVector)
    {
        //Debug.Log("inputVector:  " + inputVector);
        Vector3 move = theTransform.right * inputVector.x + theTransform.forward * inputVector.y;
        controller.Move(move * speed * Time.deltaTime);
    }

}

public class vecRotation : vectorMovement
{
    //rotation motion, like turning left/right, looking up/down

    public Transform thePartToAimHorizontal;  //no, just use the "mouse"/"joystick" controls for this one???  ehhhhhh......?
    public Transform thePartToAimVertical;
    
    public float yawSpeed = 1f;
    public float pitchSpeed = 1f;
    public float pitchRange = 70f;

    public float limitedPitchRotation = 0f;


    public static vecRotation addVecRotationAndReturnIt(GameObject objectToAddItTo, float inputSpeed, Transform theHorizontalTransform, Transform theVerticalTransform, buttonCategories gamepadButtonType, float pitchRange = 70f)
    {
        vecRotation theVectorRotationComponent = objectToAddItTo.AddComponent<vecRotation>();

        theVectorRotationComponent.speed = inputSpeed;
        theVectorRotationComponent.yawSpeed = inputSpeed / 444;
        theVectorRotationComponent.pitchSpeed = inputSpeed / 444;
        theVectorRotationComponent.pitchRange = pitchRange;



        theVectorRotationComponent.thePartToAimHorizontal = theHorizontalTransform;
        theVectorRotationComponent.thePartToAimVertical = theVerticalTransform;
        theVectorRotationComponent.gamepadButtonType = gamepadButtonType;

        return theVectorRotationComponent;
    }


    public vecRotation(float inputSpeed, Transform theHorizontalTransform, Transform theVerticalTransform, buttonCategories gamepadButtonType, float pitchRange = 70f)
    {
        speed = inputSpeed;
        yawSpeed = inputSpeed/444;
        pitchSpeed = inputSpeed/444;
        this.pitchRange = pitchRange;



        thePartToAimHorizontal = theHorizontalTransform;
        thePartToAimVertical = theVerticalTransform;
        this.gamepadButtonType = gamepadButtonType;

    }



    public override void enact(Vector2 inputVector)
    {
        //Debug.Log("inputVector:  " + inputVector);

        updatePitch(inputVector.y);
        updateYaw(inputVector.x);
    }

    public void updatePitch(float pitchInput)
    {
        //Debug.Log("11111111111111thePartToAimVertical.transform.gameObject:  " + thePartToAimVertical.transform.gameObject);

        limitedPitchRotation -= pitchInput * pitchSpeed;
        limitedPitchRotation = Mathf.Clamp(limitedPitchRotation, -pitchRange, pitchRange);
        
        thePartToAimVertical.localRotation = Quaternion.Euler(limitedPitchRotation, 0f, 0f);
        
    }

    public void updateYaw(float yawInput)
    {
        //Debug.Log("yawSpeed:  " + yawSpeed);

        thePartToAimHorizontal.Rotate(thePartToAimHorizontal.up * yawInput * yawSpeed);
    }


}

public class turningWithNoStrafe : vectorMovement
{
    //translation motion, like walking forward/back, and STRAFING left/right

    float yawSpeed = 11f;
    bool screenPlaneInsteadoOfHorizonPlane = false;  //like moving up/down and left/right in starfox.  ad-hoc for now


    public turningWithNoStrafe(float inputSpeed, Transform theTransform, buttonCategories gamepadButtonType)
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

    }


    public override void enact(Vector2 inputVector)
    {
        //Debug.Log("inputVector:  " + inputVector);

        Vector3 translate = theTransform.forward * inputVector.y; //needs to be Y!!!!!!!!!!  //and FORWARD vector!!!!!!!!!!

        controller.Move(translate * speed * Time.deltaTime);
        updateYaw(inputVector.x);
    }

    void updateYaw(float yawInput)
    {
        //Debug.Log("thePilotEnactionScript.yawInput:  " + thePilotEnactionScript.yawInput);
        theTransform.Rotate(theTransform.up * yawInput * yawSpeed);

    }




}


