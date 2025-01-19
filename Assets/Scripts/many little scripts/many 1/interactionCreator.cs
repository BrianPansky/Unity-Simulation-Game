using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static interactable2;
using static interactionCreator;

public class interactionCreator : MonoBehaviour
{
    public static interactionCreator singleton;

    public enum simpleSlot
    {
        errorYouDidntSetEnumTypeForSIMPLESLOT,
        storage,
        seat,
        hands,
        liquidHolder,
        topOfHead,
        face,
        torso
    }


    public enum numericalVariable
    {
        errorYouDidntSetEnumTypeForNumericalVariable,
        health,
        cooldown
    }


    public enum stuffType
    {
        errorYouDidntSetEnumTypeForStuffType,
        meat1,
        fruit
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






    public interactionInfo arbitraryInterInfo()
    {
        interactionInfo newInfo = new interactionInfo(enactionCreator.interType.self, 1, 11);

        return newInfo;
    }
    public interactionInfo arbitraryInterInfo(float magnitude)
    {
        interactionInfo newInfo = new interactionInfo(enactionCreator.interType.self, magnitude, 11);

        return newInfo;
    }



    public void dockXToY(GameObject objectX, GameObject objectY, Vector3 offset = new Vector3())
    {
        //really, i shouldn't need an offset.  i should just have "docking" ports positioned correctly on objects....
        objectX.transform.position = objectY.transform.position;
        objectX.gameObject.transform.SetParent(objectY.transform, true);
        objectX.transform.localPosition += offset;
        objectX.transform.rotation = objectY.transform.rotation;
    }

    public void makeThisObjectDisappear(GameObject theObjectWeWantToDisappear)
    {
        Renderer theRenderer = theObjectWeWantToDisappear.GetComponent<Renderer>();
        theRenderer.enabled = false;
    }
    public void makeThisObjectAppear(GameObject theObjectWeWantToAppear)
    {
        Renderer theRenderer = theObjectWeWantToAppear.GetComponent<Renderer>();
        theRenderer.enabled = true;
    }


    internal Dictionary<enactionCreator.interType, List<IInteraction>> addInteraction(Dictionary<enactionCreator.interType, List<IInteraction>> dictOfInteractionsX, enactionCreator.interType interactionType, IInteraction interaction)
    {
        if (dictOfInteractionsX == null)
        {
            dictOfInteractionsX = new Dictionary<enactionCreator.interType, List<IInteraction>>();
        }
        else if (dictOfInteractionsX.ContainsKey(interactionType))
        {
            dictOfInteractionsX[interactionType].Add(interaction);
            return dictOfInteractionsX;
        }


        //sigh, need to add the key first, which means the list it unlocks as well...
        List<IInteraction> list = new List<IInteraction>();
        list.Add(interaction);
        dictOfInteractionsX.Add(interactionType, list);

        return dictOfInteractionsX;
    }

    internal void addInteraction(GameObject objectToAddItTo, enactionCreator.interType interactionType, IInteraction interaction)
    {
        interactable2 theInter = objectToAddItTo.GetComponent<interactable2>();
        if (theInter == null)
        {
            theInter = objectToAddItTo.AddComponent<interactable2>();
        }

        theInter.dictOfInteractions = addInteraction(theInter.dictOfInteractions, interactionType, interaction);

    }

    public List<Ieffect> makeEffectIntoList(Ieffect e1)
    {
        List<Ieffect> newList = new List<Ieffect>();
        newList.Add(e1);
        return newList;
    }


}

public class interactionInfo
{
    public enactionCreator.interType interactionType { get; set; }
    public float magnitudeOfInteraction = 1f;
    public int level = 0;

    public interactionInfo(enactionCreator.interType interactionType, float magnitudeOfInteraction = 1f, int level = 0)
    {
        this.interactionType = interactionType;
        this.magnitudeOfInteraction = magnitudeOfInteraction;
        this.level = level;
    }

}


public class interactable2 : zoneable2
{
    public Dictionary<interactionCreator.numericalVariable, float> dictOfIvariables = new Dictionary<interactionCreator.numericalVariable, float>();


    //umm, why isn't this in "interactable2"???
    public Dictionary<enactionCreator.interType, List<IInteraction>> dictOfInteractions = new Dictionary<enactionCreator.interType, List<IInteraction>>();


    public void doCooldown()
    {
        if (dictOfIvariables[numericalVariable.cooldown] > 0)
        {
            dictOfIvariables[numericalVariable.cooldown]--;
        }
    }

    /*
    
    //hmm, i should just build these conditional effects into CONDITIONS THEMSELVES?  [except i want some to require MULTIPLE conditions......sooo....make a "condition set" class?  put it all in there?][i mean mulit-condition IS a condition, so is "if ANY one of these conditions is met" or "if any TWO of these conditions is met", etc][thus, conditions ARE the condition shell, some just need to be multi-condition conditions that accept other conditions too.  but all of them, even SINGLE conditions ones, have built in ability to do effects]
    public Dictionary<condition, List<Ieffect>> conditionalEffects = new Dictionary<condition, List<Ieffect>>();


    //is there a better way to do this?
    public void doConditionalEffectsIfMet()//call when interacted with?
    {
        //is there a better way to do this?  observer broadcast whatevers?
        //whatever, try this for now?

        //so, this is called when interacted with [may need to distinguish between BEFORE interaction and AFTER?]
        //after interaction is done, check all conditional effects to see if any of them have been triggered.

        //Debug.Log("for this object:  " + this.gameObject + ", conditionalEffects.Keys.Count:  " + conditionalEffects.Keys.Count);
        foreach (condition thisCondition in conditionalEffects.Keys)
        {
            //Debug.Log("thisCondition:  "+ thisCondition);
            if (thisCondition.met() == false) { continue; }

            foreach (Ieffect thisEffect in conditionalEffects[thisCondition])
            {
                //Debug.Log("this.gameObject:  "+ this.gameObject);
                thisEffect.implementEffect(this.gameObject, null, interactionCreator.singleton.arbitraryInterInfo());  //seems messy...
            }
        }
    }


    void Update()
    {
        //Debug.Log("?????????????????????????????????????????");
        doConditionalEffectsIfMet();
    }

    */
}




//effects

public interface Ieffect
{
    //why does it take interactionInfo?  effects should be separate from their causes, so shouldn't need that?  probably no author too?
    //looks like "theInfo" is used to do some ad-hoc CONDITIONS.  but i should just use my/a condition system for that.
    void implementEffect();
}

public class adHocDebugEffect : Ieffect
{

    adHocDebuggerForGoGrabPlan theDebugger;
    condition theCondition;

    public adHocDebugEffect(adHocDebuggerForGoGrabPlan theDebuggerIn, condition theCondition)
    {
        theDebugger = theDebuggerIn;
        this.theCondition = theCondition;
    }

    public void implementEffect()
    {
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //addToReport
        //report += "[ raycast did NOT hit intended target.  -(intendedTarget:  " + intendedTarget + " - whatRaycastHit: " + whatRaycastHit + ")-";

        theDebugger.recordFailedCondition(theCondition);
    }
}

public class deathEffect : Ieffect
{
    GameObject theObjectToKill;


    public deathEffect(GameObject theObjectToKillIn)
    {
        theObjectToKill = theObjectToKillIn;
    }

    public void implementEffect()
    {
        //Debug.Log("objectBeingInteractedWith:  "+ objectBeingInteractedWith);
        kill();
    }

    public void kill()
    {
        GameObject.Destroy(theObjectToKill);
    }
}

public class numericalEffect : Ieffect
{
    public interactionCreator.numericalVariable toAlter;
    //public bool increaseTheVariable = false;  //if false, we DECREASE the variable
    //public int minLevel = 0;  //the minimum interaction level required to implement the effect
    //public int maxLevel = 10;  //at or beyond max level, effect is simply 100%?  in between it's normal math
    float amount = 1f;
    interactable2 theInteractable;

    public numericalEffect(interactable2 theInteractableIn, interactionCreator.numericalVariable toAlter, float amountIn = 1f, bool increaseTheVariable = false)
    {
        this.toAlter = toAlter;
        //this.increaseTheVariable = increaseTheVariable;
        this.amount = -amountIn;
        if (increaseTheVariable == false) { amount = -amount; }

        theInteractable = theInteractableIn;
    }

    public void implementEffect()
    {
        adjustInteractableVariable(theInteractable, toAlter, amount);
    }


    private void adjustInteractableVariable(interactable2 theinteractable2, interactionCreator.numericalVariable toAlter, float amount)
    {
        if (theinteractable2.dictOfIvariables.ContainsKey(toAlter) == false) { Debug.Log("ivars.dictOfIvariables.ContainsKey(toAlter) == false.......should never happen?"); return; }

        theinteractable2.dictOfIvariables[toAlter] += amount;
    }


    private void setInteractableVariable(interactable2 theinteractable2, interactionCreator.numericalVariable toAlter, float amount)
    {
        if (theinteractable2.dictOfIvariables.ContainsKey(toAlter) == false) { Debug.Log("ivars.dictOfIvariables.ContainsKey(toAlter) == false.......should never happen?"); return; }

        theinteractable2.dictOfIvariables[toAlter] = amount;
    }

}


public class generateObject : Ieffect
{
    objectGen theGenerator;


    public generateObject(objectGen theGeneratorIn)
    {
        theGenerator = theGeneratorIn;
    }

    public void implementEffect()
    {
        theGenerator.generate();
    }

}

public class generateObjectAtLocation : Ieffect
{
    objectGen theGenerator;
    agnosticTargetCalc theLocation;


    public generateObjectAtLocation(objectGen theGeneratorIn, GameObject enactionPointIn)
    {
        theGenerator = theGeneratorIn;
        theLocation = new agnosticTargetCalc(enactionPointIn);
    }
    public generateObjectAtLocation(objectGen theGeneratorIn, Vector3 spawnPointIn)
    {
        theGenerator = theGeneratorIn;
        theLocation = new agnosticTargetCalc(spawnPointIn);
    }

    public void implementEffect()
    {
        GameObject theObject = theGenerator.generate();
        theObject.transform.position = theLocation.realPositionOfTarget();
        theObject.transform.rotation = theLocation.realRotationOfTarget();

    }

}


//interactions

public abstract class IInteraction
{
    public abstract void doInteraction(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo);
}

public class numericalInteraction : IInteraction
{
    public interactionCreator.numericalVariable toAlter;
    public bool increaseTheVariable = false;  //if false, we DECREASE the variable
    //public int minLevel = 0;  //the minimum interaction level required to implement the effect
    //public int maxLevel = 10;  //at or beyond max level, effect is simply 100%?  in between it's normal math


    public numericalInteraction(interactionCreator.numericalVariable toAlter, bool increaseTheVariable = false)
    {
        this.toAlter = toAlter;
        this.increaseTheVariable = increaseTheVariable;
    }

    public override void doInteraction(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo)
    {
        interactable2 theinteractable2 = objectBeingInteractedWith.GetComponent<interactable2>();
        if (theinteractable2 == null) { return; }


        float amount = theInfo.magnitudeOfInteraction;
        if (increaseTheVariable == false) { amount = -amount; }


        adjustInteractableVariable(theinteractable2, toAlter, amount);

        //Debug.Log("theinteractable2.dictOfIvariables[toAlter] += amount;");
        //Debug.Log(toAlter+" = "+ theinteractable2.dictOfIvariables[toAlter]);
        //Debug.Log("now theinteractable2.dictOfIvariables[toAlter] = " + theinteractable2.dictOfIvariables[toAlter]);
    }

    private void adjustInteractableVariable(interactable2 theinteractable2, interactionCreator.numericalVariable toAlter, float amount)
    {
        if (theinteractable2.dictOfIvariables.ContainsKey(toAlter) == false) { Debug.Log("ivars.dictOfIvariables.ContainsKey(toAlter) == false.......should never happen?"); return; }

        theinteractable2.dictOfIvariables[toAlter] += amount;
    }


    private void setInteractableVariable(interactable2 theinteractable2, interactionCreator.numericalVariable toAlter, float amount)
    {
        if (theinteractable2.dictOfIvariables.ContainsKey(toAlter) == false) { Debug.Log("ivars.dictOfIvariables.ContainsKey(toAlter) == false.......should never happen?"); return; }

        theinteractable2.dictOfIvariables[toAlter] = amount;
    }

}

public class putInInventory : IInteraction
{


    public override void doInteraction(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo)
    {

        //Debug.Log("objectBeingInteractedWith:  " + objectBeingInteractedWith);


        //     !!!!!!!!!!!!!!!!!!!     quick way to prevent stealing people's guns!
        objectBeingInteractedWith.GetComponent<Collider>().enabled = false;


        //Debug.Log("author:  " + author);
        if (interactionAuthor == null)
        {
            Debug.Log("author is null");
        }
        inventory1 theInventory = interactionAuthor.GetComponent<inventory1>();
        theInventory.putInInventory(objectBeingInteractedWith);
    }
}


public class interactionEffect : IInteraction
{
    List<Ieffect> theEffects = new List<Ieffect>();


    public interactionEffect(Ieffect e1)
    {
        theEffects.Add(e1);
    }

    public interactionEffect(Ieffect e1, Ieffect e2)
    {
        theEffects.Add(e1);
        theEffects.Add(e2);
    }
    public interactionEffect(Ieffect e1, Ieffect e2, Ieffect e3)
    {
        theEffects.Add(e1);
        theEffects.Add(e2);
        theEffects.Add(e3);
    }



    public override void doInteraction(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo)
    {
        foreach (Ieffect thisEffect in theEffects)
        {
            thisEffect.implementEffect();
        }
    }
}


//conditional interaction
public class conditionalInteraction: IInteraction
{
    List<interactionCondition> theinteractionConditions = new List<interactionCondition>();
    List<IInteraction> theInteractions = new List<IInteraction>();

    public conditionalInteraction(interactionCondition c1, IInteraction i1)
    {
        theinteractionConditions.Add(c1);
        theInteractions.Add(i1);
    }

    public override void doInteraction(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo)
    {
        //Debug.Log("==================" + Time.fixedTime);

        //only does them if conditions met

        foreach (interactionCondition thisCondition in theinteractionConditions)
        {
            if (thisCondition.met(objectBeingInteractedWith, interactionAuthor, theInfo) == false) { return; }
        }

        foreach (IInteraction thisInteraction in theInteractions)
        {
            thisInteraction.doInteraction(objectBeingInteractedWith, interactionAuthor, theInfo);
        }
    }
}



//interaction Conditions

public abstract class interactionCondition
{

    public abstract bool met(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo);
}

public class interactionLevel: interactionCondition
{

    public interactionCreator.numericalVariable toCompare;

    public int levelCutoff = 1;



    public interactionLevel(int levelCutoffInput)
    {
        levelCutoff = levelCutoffInput;
    }



    public override bool met(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo)
    {
        interactable2 theinteractable2 = objectBeingInteractedWith.GetComponent<interactable2>();
        if (theinteractable2 == null) { return false; }

        if (theInfo.level < levelCutoff) { return false; }

        return true;
    }
}





public class gravityToFall : MonoBehaviour
{
    bool isGrounded = false;
    float gravity = -5f;
    float currentFallRate = 0f;


    void Update()
    {
        if (isThisGrounded() && currentFallRate < 0.7f) 
        {
            currentFallRate = 0f;
            return;
        }

        //Debug.Log("currentFallRate:  "+ currentFallRate);

        fall();

    }

    public void fall()
    {


        currentFallRate += gravity * Time.deltaTime;
        this.gameObject.transform.position += new Vector3(0, currentFallRate,0);

        //controller.Move(velocity * Time.deltaTime);
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

            if (hit.distance < 1.2f)
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