using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static interactable2;

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


    internal Dictionary<enactionCreator.interType, List<Ieffect>> addInteraction(Dictionary<enactionCreator.interType, List<Ieffect>> dictOfInteractionsX, enactionCreator.interType interactionType, Ieffect effect)
    {
        if (dictOfInteractionsX == null)
        {
            dictOfInteractionsX = new Dictionary<enactionCreator.interType, List<Ieffect>>();
        }
        else if (dictOfInteractionsX.ContainsKey(interactionType))
        {
            dictOfInteractionsX[interactionType].Add(effect);
            return dictOfInteractionsX;
        }


        //sigh, need to add the key first, which means the list it unlocks as well...
        List<Ieffect> list = new List<Ieffect>();
        list.Add(effect);
        dictOfInteractionsX.Add(interactionType, list);

        return dictOfInteractionsX;
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


    public Dictionary<enactionCreator.interType, List<Ieffect>> dictOfInteractions = new Dictionary<enactionCreator.interType, List<Ieffect>>();
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
}



public interface Ieffect
{
    //why does it take interactionInfo?  effects should be separate from their causes, so shouldn't need that?  probably no author too?
    //looks like "theInfo" is used to do some ad-hoc CONDITIONS.  but i should just use my/a condition system for that.
    void implementEffect(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo);
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

    public void implementEffect(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo)
    {
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //addToReport
        //report += "[ raycast did NOT hit intended target.  -(intendedTarget:  " + intendedTarget + " - whatRaycastHit: " + whatRaycastHit + ")-";

        theDebugger.recordFailedCondition(theCondition);
    }
}

public class numericalEffect : Ieffect
{
    public interactionCreator.numericalVariable toAlter;
    public bool increaseTheVariable = false;  //if false, we DECREASE the variable
    public int minLevel = 0;  //the minimum interaction level required to implement the effect
    public int maxLevel = 10;  //at or beyond max level, effect is simply 100%?  in between it's normal math


    public numericalEffect(interactionCreator.numericalVariable toAlter, bool increaseTheVariable = false)
    {
        this.toAlter = toAlter;
        this.increaseTheVariable = increaseTheVariable;
    }

    public void implementEffect(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo)
    {
        interactable2 theinteractable2 = objectBeingInteractedWith.GetComponent<interactable2>();
        if (theinteractable2 == null) { return; }

        //.............gonna have to re-do all of this in a way that's legible to NPCs for planning..........make it so i can feed imaginary state in?  ya, probably....[just create imaginary whole game object with imaginary interactable2 etc.........]
        //also, the following is a CONDITION, not an EFFECT!  indeed, it's a NUMERICAL condition, not a numerical effect
        if (theInfo.level < minLevel) { return; }
        if (theInfo.level >= maxLevel)
        {
            if (increaseTheVariable == false) { setInteractableVariable(theinteractable2, toAlter, 0); } //how to implement "max" effect if it's addition???

            return;
        }


        float amount = theInfo.magnitudeOfInteraction;
        if (increaseTheVariable == false) { amount = -amount; }


        adjustInteractableVariable(theinteractable2, toAlter, amount);

        //Debug.Log("theinteractable2.dictOfIvariables[toAlter] += amount;");
        //Debug.Log(toAlter+" = "+ theinteractable2.dictOfIvariables[toAlter]);
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

public class deathEffect : Ieffect
{
    public void implementEffect(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo)
    {
        //Debug.Log("objectBeingInteractedWith:  "+ objectBeingInteractedWith);
        kill(objectBeingInteractedWith);
    }

    public void kill(GameObject objectBeingInteractedWith)
    {
        GameObject.Destroy(objectBeingInteractedWith);
    }
}

public class putInInventory : Ieffect
{
    //why would this be an "effect"?  not an "enaction"?

    public void implementEffect(GameObject objectBeingInteractedWith, GameObject interactionAuthor, interactionInfo theInfo)
    {
        //Debug.Log("objectBeingInteractedWith:  " + objectBeingInteractedWith);


        //     !!!!!!!!!!!!!!!!!!!     quick way to prevent stealing people's guns!
        objectBeingInteractedWith.GetComponent<Collider>().enabled = false;


        //Debug.Log("author:  " + author);
        if(interactionAuthor == null)
        {
            Debug.Log("author is null");
        }
        inventory1 theInventory = interactionAuthor.GetComponent<inventory1>();
        theInventory.putInInventory(objectBeingInteractedWith);
    }
}



public class conditionalEffects
{
    public Dictionary<condition, List<Ieffect>> theDictionary = new Dictionary<condition, List<Ieffect>>();


    public conditionalEffects(condition condition1, Ieffect effect1)
    {
        List<Ieffect> anEffectList = new List<Ieffect>();
        anEffectList.Add(effect1);
        theDictionary.Add(condition1, anEffectList);
    }

    public void doConditionalEffectsIfMet()//call when interacted with?
    {
        //is there a better way to do this?  observer broadcast whatevers?
        //whatever, try this for now?

        //so, this is called when interacted with [may need to distinguish between BEFORE interaction and AFTER?]
        //after interaction is done, check all conditional effects to see if any of them have been triggered.

        //Debug.Log("for this object:  " + this.gameObject + ", conditionalEffects.Keys.Count:  " + conditionalEffects.Keys.Count);
        foreach (condition thisCondition in theDictionary.Keys)
        {
            //Debug.Log("thisCondition:  "+ thisCondition);
            if (thisCondition.met() == false) { continue; }

            foreach (Ieffect thisEffect in theDictionary[thisCondition])
            {
                //Debug.Log("this.gameObject:  "+ this.gameObject);
                //yaaaaaaaaaaaaa nullls, so need to fix somehow.....
                thisEffect.implementEffect(null, null, interactionCreator.singleton.arbitraryInterInfo());  //seems messy...
            }
        }
    }

}