using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static stateHolder;

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

    public List<string> makeStringsIntoList(string s1, string s2 = null, string s3 = null, string s4 = null)
    {
        //input 4 strings
        //get backa  list of all of them that are NOT null

        List<string> allStrings = new List<string>();
        allStrings.Add(s1);
        allStrings.Add(s2);
        allStrings.Add(s3);
        allStrings.Add(s4);

        List<string> nonNullStrings = new List<string>();

        foreach (string thisString in allStrings)
        {
            if (thisString != null)
            {
                nonNullStrings.Add(thisString);
            }
        }

        return nonNullStrings;
    }




}

public class interactionInfo
{
    public enactionCreator.interType interactionType { get; set; }
    public float magnitudeOfInteraction = 1f;
    public int level = 0;

    public interactionInfo(enactionCreator.interType interactionType, float magnitudeOfInteraction = 1f)
    {
        this.interactionType = interactionType;
        this.magnitudeOfInteraction = magnitudeOfInteraction;
    }

}


public class stateHolder : zoneable2
{
    public Dictionary<interactionCreator.numericalVariable, float> dictOfIvariables = new Dictionary<interactionCreator.numericalVariable, float>();

    //why is this here, but other effects are somewhere else?  which place should they be???
    public Dictionary<condition, List<Ieffect>> conditionalEffects = new Dictionary<condition, List<Ieffect>>();

    public void callThisWhenInteractedWIth()
    {
        //is there a better way to do this?  observer broadcast whatevers?
        //whatever, try this for now?

        //so, this is called when interacted with [may need to distinguish between BEFORE interaction and AFTER?]
        //after interaction is done, check all conditional effects to see if any of them have been triggered.

        foreach (condition thisCondition in conditionalEffects.Keys)
        {
            if (thisCondition.met() == false) { continue; }

            foreach (Ieffect thisEffect in conditionalEffects[thisCondition])
            {
                //ummmm, takes a collider interaction script?!?!?!?  seems now like that's a bad idea...
                thisEffect.implementEffect(this.gameObject, null);
            }
        }
    }
}

public interface IInteractable
{
    Dictionary<enactionCreator.interType, List<Ieffect>> dictOfInteractions { get; set; }
}




public interface Ieffect
{
    void implementEffect(GameObject objectBeingInteractedWith, colliderInteractor theCollisionInteractionScript);
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

    public void implementEffect(GameObject objectBeingInteractedWith, colliderInteractor theCollisionInteractionScript)
    {
        stateHolder theStateHolder = objectBeingInteractedWith.GetComponent<stateHolder>();
        if (theStateHolder == null) { return; }

        //.............gonna have to re-do all of this in a way that's legible to NPCs for planning..........make it so i can feed imaginary state in?  ya, probably....[just create imaginary whole game object with imaginary stateHolder etc.........]
        if (theCollisionInteractionScript.level < minLevel) { return; }
        if (theCollisionInteractionScript.level >= maxLevel)
        {
            if (increaseTheVariable == false) { setInteractableVariable(theStateHolder, toAlter, 0); } //how to implement "max" effect if it's addition???

            return;
        }


        float amount = theCollisionInteractionScript.magnitudeOfInteraction;
        if (increaseTheVariable == false) { amount = -amount; }


        adjustInteractableVariable(theStateHolder, toAlter, amount);

        //Debug.Log("theStateHolder.dictOfIvariables[toAlter] += amount;");
    }

    private void adjustInteractableVariable(stateHolder theStateHolder, interactionCreator.numericalVariable toAlter, float amount)
    {
        if (theStateHolder.dictOfIvariables.ContainsKey(toAlter) == false) { Debug.Log("ivars.dictOfIvariables.ContainsKey(toAlter) == false.......should never happen?"); return; }

        theStateHolder.dictOfIvariables[toAlter] += amount;
    }


    private void setInteractableVariable(stateHolder theStateHolder, interactionCreator.numericalVariable toAlter, float amount)
    {
        if (theStateHolder.dictOfIvariables.ContainsKey(toAlter) == false) { Debug.Log("ivars.dictOfIvariables.ContainsKey(toAlter) == false.......should never happen?"); return; }

        theStateHolder.dictOfIvariables[toAlter] = amount;
    }

}

public class playAsPlayable2 : Ieffect
{
    public void implementEffect(GameObject objectBeingInteractedWith, colliderInteractor theCollisionInteractionScript)
    {
        //ad hoc for now
        playable2 thePlayable2 = objectBeingInteractedWith.GetComponent<playable2>();
        if (thePlayable2.occupied == true) { Debug.Log("thePlayable2.occupied == true, this playable2 object:  " + objectBeingInteractedWith); return; }

        GameObject author = theCollisionInteractionScript.enactionAuthor;
        virtualGamepad gamepad = author.GetComponent<virtualGamepad>();
        thePlayable2.playAsPlayable2(gamepad);
        disableCharacterControllerAndNavmeshAgent(author);
        interactionCreator.singleton.dockXToY(author, objectBeingInteractedWith);
    }

    private void disableCharacterControllerAndNavmeshAgent(GameObject objectToDisable)
    {
        CharacterController controller = objectToDisable.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        NavMeshAgent nva = objectToDisable.GetComponent<NavMeshAgent>();
        if (nva != null)
        {
            nva.enabled = false;
        }
    }
}

public class equipequippable2 : Ieffect
{
    public void implementEffect(GameObject objectBeingInteractedWith, colliderInteractor theCollisionInteractionScript)
    {
        //do i use this anymore?  moved to the equippable2s and/or equippers.....
        //well, this was for equipping something INSTANTLY when you pick it up.....


        GameObject author = theCollisionInteractionScript.enactionAuthor;
        playable2 thePlayable2 = author.GetComponent<playable2>();
        gun1 theGun = objectBeingInteractedWith.GetComponent<gun1>();

        theGun.equipThisequippable2(thePlayable2);
        interactionCreator.singleton.dockXToY(objectBeingInteractedWith, thePlayable2.enactionPoint1, new Vector3(0.6f, 0.1f, 0));
    }
}

public class putInInventory : Ieffect
{
    public void implementEffect(GameObject objectBeingInteractedWith, colliderInteractor theCollisionInteractionScript)
    {
        //Debug.Log("objectBeingInteractedWith:  " + objectBeingInteractedWith);


        //     !!!!!!!!!!!!!!!!!!!     quick way to prevent stealing people's guns!
        objectBeingInteractedWith.GetComponent<Collider>().enabled = false;


        GameObject author = theCollisionInteractionScript.enactionAuthor;
        //Debug.Log("author:  " + author);
        if(author == null)
        {
            Debug.Log("author is null");
        }
        inventory1 theInventory = author.GetComponent<inventory1>();
        theInventory.putInInventory(objectBeingInteractedWith);
    }
}

