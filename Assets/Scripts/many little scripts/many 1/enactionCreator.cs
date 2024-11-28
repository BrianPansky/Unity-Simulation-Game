using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
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
        peircing,
        shootFlamethrower1,
        tankShot,
        melee
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
        thisButtonCategoryIntentionallyLeftBlank,
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


public abstract class enaction : MonoBehaviour
{
    public buttonCategories gamepadButtonType { get; set; }
    public List<condition> prereqs = new List<condition>();

    public bool debugPrint = false;

    abstract public void enact(inputData theInput);

    abstract public planEXE2 toEXE(GameObject target);

    
    public planEXE2 standardEXEconversion()
    {
        //just put this in "toEXE", right?
        planEXE2 exe1 = this.toEXE(null);
        exe1.debugPrint = this.debugPrint; //huh, i think this indicates we're moving in a bottom-up direction, nice
        exe1.atLeastOnce();

        foreach (condition thisCondition in this.prereqs)
        {
            exe1.startConditions.Add(thisCondition);
        }

        return exe1;
    }





    internal void conditionalPrint(string thingToPrint)
    {
        if (debugPrint == false) { return; }


        Debug.Log(thingToPrint);

    }
}

public abstract class IEnactaBool: enaction
{

    public GameObject enactionAuthor { get; set; }



    public void enact()
    {
        enact(new inputData());
    }


    public override planEXE2 toEXE(GameObject target)
    {
        boolEXE2 theEXE = new boolEXE2(this, target);
        return theEXE;
    }


}

public abstract class IEnactaVector : enaction
{
    //???????????????????  is IEnactaVector different from IEnactByTargetVector?????????
    //      well, not all vectors are the result of one single known target, so....maybr different??? not sure....

    public void enact(Vector2 inputV2)
    {
        enact(new inputData(inputV2));
    }

    //why the fuck is enact twice here????????????????????????????????????????????????
    public void enact(Vector3 inputV3)
    {
        enact(new inputData(inputV3));
    }


    public override planEXE2 toEXE(GameObject target)
    {
        vect3EXE2 theEXE = new vect3EXE2(this, target);
        return theEXE;
    }


}

public abstract class IEnactByTargetVector:enaction
{
    //is this redundant?  didn't i already write that here?  what happened to that writing?  or was it somewhere else?
    public void enact(Vector3 inputV3)
    {
        enact(new inputData(inputV3));
    }


    public override planEXE2 toEXE(GameObject target)
    {
        //OFFSET NEEDS TO BE ZERO FOR "aimtarget" to actually aim at the target [otherwise it aims at a sorta navpoint NEAR the target,,,,which can be BEHIND or BELOW the npc trying to target something in front of them!
        //are there any situations where i'll want to use "toEXE" with a NON-zero offset???
        vect3EXE2 theEXE = new vect3EXE2(this, target, 0f);
        return theEXE;
    
    }


}



public class navAgent : IEnactByTargetVector
{
    //only for vector inputs!
    NavMeshAgent theAgent;
    //public bool debugPrint = false;
    public static void addNavAgentEnaction(GameObject objectToAddNavmeshAgentTo)
    {
        navAgent nA = objectToAddNavmeshAgentTo.AddComponent<navAgent>();

        nA.theAgent = objectToAddNavmeshAgentTo.GetComponent<NavMeshAgent>();
        if (nA.theAgent == null)
        {

            nA.theAgent = objectToAddNavmeshAgentTo.AddComponent<NavMeshAgent>();
        }

        nA.theAgent.baseOffset = 1f; //prevent stutter, being in floor
        //nA.theAgent.angularSpeed = 0;
        nA.theAgent.speed = 5.5f;
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
    public override void enact(inputData theInput)
    {
        if (theAgent.enabled == false)
        {
            Debug.Log("theAgent.enabled == false, theAgent.transform.gameObject:  " + theAgent.transform.gameObject);
            Debug.DrawLine(new Vector3(), theAgent.transform.position, Color.magenta, 200f);
        }

        Debug.Assert(theAgent != null);
        Debug.Assert(theInput != null);
        //if(debugPrint == true) { Debug.Log("theAgent.SetDestination(theInput.vect3);, should go?"); }

        //Debug.Log("theInput.vect3:  " + theInput.vect3);

        /*
        if (debugPrint == true)
        {

            Debug.DrawLine(new Vector3(), theAgent.transform.position, Color.green, 2f);
            Debug.DrawLine(theInput.vect3, theAgent.transform.position, Color.blue, 2f);
            Debug.DrawLine(theInput.vect3, new Vector3(), Color.red, 2f);
        }
        */

        conditionalPrint("~~~~~~~~~~   nav agent enacting   ~~~~~~~~");
        Debug.Assert(theAgent.isActiveAndEnabled);
        

        if(theAgent.isOnNavMesh == false)
        {
            theAgent.Warp(theAgent.transform.position);
        }
        Debug.Assert(theAgent.isOnNavMesh);
        theAgent.SetDestination(theInput.vect3);
    }
}

public class aimTarget : IEnactByTargetVector
{
    //only for vector inputs!
    public vecRotation theVectorRotationEnaction; //????

    public static void addAimTargetAndVecRotation(GameObject objectToAddItTo, float inputSpeed, Transform theHorizontalTransform, Transform theVerticalTransform, buttonCategories gamepadButtonType, float pitchRange = 70f)
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
    override public void enact(inputData theInput)
    {
        //instantaneous for now
        //  Vector3 lineFromVertAimerToTarget = theInput.vect3 - theVectorRotationEnaction.thePartToAimVertical.position;
        Vector3 lineFromVertAimerToTarget = theInput.vect3 - this.transform.position;
        //conditionalPrint("theInput.vect3 = " + theInput.vect3);
        //conditionalPrint("theVectorRotationEnaction.thePartToAimVertical.position.vect3 = " + theVectorRotationEnaction.thePartToAimVertical.position);
        //conditionalPrint("theInput.vect3 = " + theInput.vect3);
        theVectorRotationEnaction.updateYaw(translateAngleIntoYawSpeedEtc(getHorizontalAngle(lineFromVertAimerToTarget)));
        lineFromVertAimerToTarget = theInput.vect3 - theVectorRotationEnaction.thePartToAimVertical.position;

        //Debug.Log("aim..................");

        Debug.DrawLine(theVectorRotationEnaction.thePartToAimVertical.position, theInput.vect3, Color.red, 7f);
        theVectorRotationEnaction.updatePitch(getVerticalAngle(lineFromVertAimerToTarget), theVectorRotationEnaction.thePartToAimVertical);
        //theVectorRotationEnaction.updatePitch(0, theVectorRotationEnaction.thePartToAimVertical);


        //conditionalPrint("enacting:  " + this);
        Debug.DrawLine(theInput.vect3, theVectorRotationEnaction.thePartToAimVertical.position, Color.red, 0.02f);

    }




    private float translateAngleIntoYawSpeedEtc(float angle)
    {
        return angle / (theVectorRotationEnaction.yawSpeed);
    }

    private float translateAngleIntoPitchSpeedEtc(float angle)
    {
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

        //float oneAngle = AngleOffAroundAxis(lineToTarget.normalized, this.transform.forward, theVectorRotationEnaction.thePartToAimHorizontal.up);

        return oneAngle;
    }

    private float getVerticalAngle(Vector3 lineToTarget)
    {

        Vector3 start = theVectorRotationEnaction.thePartToAimVertical.position;
        Vector3 offset = new Vector3(0.01f, 0.01f, 0.01f);
        Debug.DrawLine(start + offset, start + lineToTarget.normalized + offset, Color.white, 4f);

        float oneAngle = AngleOffAroundAxis(lineToTarget, theVectorRotationEnaction.thePartToAimVertical.forward, theVectorRotationEnaction.thePartToAimVertical.right);
        //fixed it!  my input vector was just target position!  i needed to be using line from aiming object to the target it is aiming at!  position relative to the person doing the aiming, basically








        conditionalPrint("calculated horizontalAngle = " + oneAngle);
        //Debug.DrawLine(start + offset, start + lineToTarget.normalized + offset, Color.white, 4f);
        Debug.DrawLine(theVectorRotationEnaction.thePartToAimVertical.position - offset, theVectorRotationEnaction.thePartToAimVertical.position + theVectorRotationEnaction.thePartToAimVertical.forward - offset, Color.black, 4f);



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



public abstract class collisionEnaction: IEnactaBool
{ 
    public interactionInfo interInfo;
    public override abstract void enact(inputData theInput);
}


public abstract class rangedEnaction: collisionEnaction
{
    public Transform firePoint;
    //or put these in projectile info?  i guess here makes sense, ALL the info in this class is projectile info, but divide by PARTS, the bullet is a different part than the gun or whatever
    public float range = 113.6f;
    //need to put these in a gun class, or "launcher"/"firer" class or something...and all the "generator" stuff above?:
    public int firingCooldown = 0;
    public int firingCooldownMax = 20;

    //ad-hoc!
    //public cooldown theCooldown;

}

public class projectileLauncher: rangedEnaction
{
    public projectileToGenerate theprojectileToGenerate;

    public static void addProjectileLauncher(GameObject objectToAddItTo, Transform firePoint, buttonCategories gamepadButtonType, interactionInfo interInfo, projectileToGenerate theprojectileToGenerate, int infiringCooldownMax, float range = 99f)
    {
        projectileLauncher pL = objectToAddItTo.AddComponent<projectileLauncher>();
        pL.gamepadButtonType = gamepadButtonType;
        pL.interInfo = interInfo;
        pL.theprojectileToGenerate = theprojectileToGenerate;

        pL.firingCooldownMax = infiringCooldownMax;
        pL.firePoint = firePoint;
        pL.range = range;
        /*
        //pL.theCooldown = new cooldown(infiringCooldownMax);
        if(pL.prereqs == null)
        {
            pL.prereqs = new List<condition>();
        }
        Debug.Assert(pL.prereqs != null);
        Debug.Assert(objectToAddItTo.GetComponent<interactable2>() != null);
        pL.prereqs.Add(new numericalCondition(interactionCreator.numericalVariable.cooldown, objectToAddItTo.GetComponent<interactable2>().dictOfIvariables));
        */
        pL.Add(new numericalCondition(interactionCreator.numericalVariable.cooldown, objectToAddItTo.GetComponent<interactable2>().dictOfIvariables));
    }

    private void Add(condition c1)
    {
        if (prereqs == null)
        {
            prereqs = new List<condition>();
        }

        prereqs.Add(c1);
    }

    public projectileLauncher(Transform firePoint, buttonCategories gamepadButtonType, interactionInfo interInfo, projectileToGenerate theprojectileToGenerate, float range = 99f)
    {
        this.gamepadButtonType = gamepadButtonType;
        this.interInfo = interInfo;
        this.theprojectileToGenerate = theprojectileToGenerate;

        this.firePoint = firePoint;
        this.range = range;

    }

    override public void enact(inputData theInput)
    {
        //theCooldown.fire();
        //Debug.Log("firing projectile??????????????????????????????????????????????????????????????");
        genGen.singleton.projectileGenerator(theprojectileToGenerate, this, firePoint.position+ firePoint.forward, firePoint.forward);
    }

}

public class hitscanEnactor: rangedEnaction
{

    GameObject raycastHitOut;
    public targetCalculator theHitCalculatorOut;
    public adHocBooleanDeliveryClass firingIsDone;

    public static void addHitscanEnactor(GameObject objectToAddItTo, Transform firePoint, buttonCategories gamepadButtonType, interactionInfo interInfo, float range = 114f)
    {

        hitscanEnactor newHitscanEnactor = objectToAddItTo.GetComponent<hitscanEnactor>(); 
        //Debug.Log("OLD newHitscanEnactor:  "+ newHitscanEnactor);
        newHitscanEnactor = objectToAddItTo.AddComponent<hitscanEnactor>();
        //Debug.Log("NEW newHitscanEnactor:  " + newHitscanEnactor);
        newHitscanEnactor.gamepadButtonType = gamepadButtonType;
        newHitscanEnactor.interInfo = interInfo;

        newHitscanEnactor.firePoint = firePoint;
        newHitscanEnactor.range = range;

        //newHitscanEnactor.theCooldown = new cooldown(0);


    }



    /*

    public hitscanEnactor(Transform firePoint, buttonCategories gamepadButtonType, interactionInfo interInfo, float range = 7f)
    {
        this.gamepadButtonType = gamepadButtonType;
        this.interInfo = interInfo;

        this.firePoint = firePoint;
        this.range = range;

    }

    */

    override public void enact(inputData theInput)
    {
        //conditionalPrint("enacting:  " + this);
        //Debug.Log("FIRE.........");
        firingByRaycastHit(range);
    }


    public void firingByRaycastHit(float theRange)
    {

        conditionalPrint("try raycast firing.......");
        RaycastHit myHit;
        Ray myRay = new Ray(firePoint.transform.position, firePoint.transform.forward);

        if (Physics.Raycast(myRay, out myHit, theRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) == false) {

            conditionalPrint("raycast = false, so, didn't hit anything?"); 
            return; }
        if (myHit.transform == null)
        {
            conditionalPrint("(myHit.transform == null), so, didn't hit anything???"); 
            return; }



        conditionalPrint("successfully hit SOMETHING, so firing:  " + myHit.transform);
        theHitCalculatorOut = new movableObjectTargetCalculator(this.gameObject, myHit.transform.gameObject);
        if(firingIsDone != null)  //player doesn't have these fancy debug things!  so causes error if player does it....
        {

            firingIsDone.theBoolSignal = true;
        }
        GameObject newInstantInteractionSphere = comboGen.singleton.instantInteractionSphere(myHit.point);
        colliderInteractor.genColliderInteractor(newInstantInteractionSphere, this);

        Debug.DrawLine(newInstantInteractionSphere.transform.position, firePoint.transform.position, Color.green, 17f);

        firingCooldown--;
    }
}



public class enactEffect : IEnactaBool
{
    Ieffect theEffect;



    public static void addEnactEffect(GameObject objectToAddItTo, Ieffect theEffectIn)
    {

        enactEffect newEnactEffect = objectToAddItTo.AddComponent<enactEffect>();

        newEnactEffect.theEffect = theEffectIn;
    }
    public static enactEffect addEnactEffectAndReturn(GameObject objectToAddItTo, Ieffect theEffectIn)
    {

        enactEffect newEnactEffect = objectToAddItTo.AddComponent<enactEffect>();

        newEnactEffect.theEffect = theEffectIn;

        return newEnactEffect;
    }

    public override void enact(inputData theInput)
    {
        theEffect.implementEffect();
    }
}

public class compoundEnactaBool : IEnactaBool
{

    public List<IEnactaBool> theEnactions = new List<IEnactaBool>();

    public static void addCompoundEnactaBool(GameObject objectToAddItTo, buttonCategories theButtonType, IEnactaBool enaction1)
    {

        compoundEnactaBool newCompoundEnactaBool = objectToAddItTo.AddComponent<compoundEnactaBool>();

        newCompoundEnactaBool.gamepadButtonType = theButtonType;
        newCompoundEnactaBool.theEnactions.Add(enaction1);
    }

    public static void addCompoundEnactaBool(GameObject objectToAddItTo, buttonCategories theButtonType, IEnactaBool enaction1, IEnactaBool enaction2)
    {

        compoundEnactaBool newCompoundEnactaBool = objectToAddItTo.AddComponent<compoundEnactaBool>();
        newCompoundEnactaBool.gamepadButtonType = theButtonType;

        newCompoundEnactaBool.theEnactions.Add(enaction1);
        newCompoundEnactaBool.theEnactions.Add(enaction2);



    }


    public static void addCompoundEnactaBool(GameObject objectToAddItTo, buttonCategories theButtonType, IEnactaBool enaction1, IEnactaBool enaction2, condition condition1)
    {

        compoundEnactaBool newCompoundEnactaBool = objectToAddItTo.AddComponent<compoundEnactaBool>();
        newCompoundEnactaBool.gamepadButtonType = theButtonType;

        newCompoundEnactaBool.theEnactions.Add(enaction1);
        newCompoundEnactaBool.theEnactions.Add(enaction2);

        newCompoundEnactaBool.prereqs.Add(condition1);


    }
    public static void addCompoundEnactaBool(GameObject objectToAddItTo, buttonCategories theButtonType, IEnactaBool enaction1, IEnactaBool enaction2, IEnactaBool enaction3)
    {

        compoundEnactaBool newCompoundEnactaBool = objectToAddItTo.AddComponent<compoundEnactaBool>();
        newCompoundEnactaBool.gamepadButtonType = theButtonType;

        newCompoundEnactaBool.theEnactions.Add(enaction1);
        newCompoundEnactaBool.theEnactions.Add(enaction2);
        newCompoundEnactaBool.theEnactions.Add(enaction3);
    }

    public override void enact(inputData theInput)
    {
        foreach (IEnactaBool thisEnaction in theEnactions)
        {
            thisEnaction.enact();
        }
    }




    public override planEXE2 toEXE(GameObject target)
    {
        //boolEXE2 theEXE = new boolEXE2(this, target);
        return standardEXEconversionWithBundledEffects();
    }

    public planEXE2 standardEXEconversionWithBundledEffects()
    {

        //planEXE2 exe1 = new simultaneousEXE(new boolEXE2(this));
        planEXE2 exe1 = new boolEXE2(this);
        //Debug.Assert(exe1.theEnaction !=null);

        //Debug.Assert(new boolEXE2(this).theEnaction != null);
        //theEnaction.toEXE(null);
        exe1.debugPrint = this.debugPrint; //huh, i think this indicates we're moving in a bottom-up direction, nice
        exe1.atLeastOnce();

        foreach (condition thisCondition in this.prereqs)
        {
            //Debug.Log("thisCondition:  " + thisCondition);
            exe1.startConditions.Add(thisCondition);
        }



        //redundant!  this enaction does them all!
        /*
        foreach (enaction thisEnaction in this.theEnactions)
        {
            //Debug.Log("thisEnaction:  "+ thisEnaction);
            planEXE2 toAdd = thisEnaction.toEXE(null);
            Debug.Assert(toAdd.theEnaction != null);
            toAdd.debugPrint = this.debugPrint;
            exe1.exeList.Add(toAdd);
        }
        */

        return exe1;
    }


    /*
    internal bool containsIntertype(interType intertypeX)
    {

        //uhhhhhhhhhhhh, messyyyyyyyyyyyyy

        foreach (enaction thisEnaction in theEnactions)
        {
            if (thisEnaction.GetType() == collisionEnaction.GetType()) { return true; }
        }





        foreach (collisionEnaction thisEnaction in theEnactions)
        {
            if (thisEnaction.interInfo.interactionType == intertypeX) { return true; }
        }

        foreach (compoundEnactaBool thisEnaction in theEnactions)
        {
            if (thisEnaction.containsIntertype(intertypeX)) { return true; }
        }


        return false;
    }
    */
}


public abstract class vectorMovement : IEnactaVector
{
    public CharacterController controller;
    public Transform theTransform;
    public float speed = 0f;
    public override abstract void enact(inputData theInput);
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


    public override void enact(inputData theInput)
    {
        Vector3 move = theTransform.right * theInput.vect2.x + theTransform.forward * theInput.vect2.y;
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



    public override void enact(inputData theInput)
    {
        updatePitch(theInput.vect2.y, thePartToAimVertical);
        updateYaw(theInput.vect2.x);
    }

    public void updatePitch(float pitchInput, Transform theTransformToRotate)
    {

        //CONFIRMED, THIS CODE MEANS "limitedPitchRotation" = THE ANGLE WE WANT TO SET IT TO!


        //float initial = limitedPitchRotation;
        limitedPitchRotation -= pitchInput;// * pitchSpeed;
        limitedPitchRotation = Mathf.Clamp(limitedPitchRotation, -pitchRange, pitchRange);

        //float relativeAngle = initial - limitedPitchRotation;

        //this.transform.localRotation = Quaternion.Euler(limitedPitchRotation, 0f, 0f);
        theTransformToRotate.localRotation = Quaternion.Euler(limitedPitchRotation, 0f, 0f);
        //thePartToAimVertical.Rotate(thePartToAimVertical.right, relativeAngle);
    }

    public void updateYaw(float yawInput)
    {
        conditionalPrint("yawInput = "+ yawInput);
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


    public override void enact(inputData theInput)
    {
        Vector3 translate = theTransform.forward * theInput.vect2.y; //needs to be Y!!!!!!!!!!  //and FORWARD vector!!!!!!!!!!

        controller.Move(translate * speed * Time.deltaTime);
        updateYaw(theInput.vect2.x);
    }

    void updateYaw(float yawInput)
    {
        theTransform.Rotate(theTransform.up * yawInput * yawSpeed);
    }
}


