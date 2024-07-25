using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class interactionCreator : MonoBehaviour
{

    public static interactionCreator singleton;


    //public enum interType

    public enum simpleSlot
    {
        //XYZ Controller buttons,
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
        health
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



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void dockXToY(GameObject objectX, GameObject objectY, Vector3 offset = new Vector3())
    {
        //Debug.Log("should dock??");
        //really, i shouldn't need an oddset.  i should just have "docking" ports positioned correctly on objects....
        objectX.transform.position = objectY.transform.position;



        //theEnactionScript.thisNavMeshAgent = theTank.thisNavMeshAgent;
        //      theTank.pilot = theAuthorScript.enacting.enactionAuthor;
        //      theTank.thePilotEnactionScript = theEnactionScript;

        //this is probably great, but it also disables my camera.  will need to re-arrange things...
        //      theBodyScript.theBodyGameObject.active = false;

        //this.gameObject.transform.SetParent(theAuthorScript.theAuthor.transform, true);
        objectX.gameObject.transform.SetParent(objectY.transform, true);

        objectX.transform.localPosition += offset;

        objectX.transform.rotation = objectY.transform.rotation;
    }

    public void dockToThisObject(GameObject theObjectWeWantToDockToTHISObject, Vector3 offset = new Vector3())
    {
        theObjectWeWantToDockToTHISObject.transform.position = this.transform.position;



        //theEnactionScript.thisNavMeshAgent = theTank.thisNavMeshAgent;
        //      theTank.pilot = theAuthorScript.enacting.enactionAuthor;
        //      theTank.thePilotEnactionScript = theEnactionScript;

        //this is probably great, but it also disables my camera.  will need to re-arrange things...
        //      theBodyScript.theBodyGameObject.active = false;

        //this.gameObject.transform.SetParent(theAuthorScript.theAuthor.transform, true);
        theObjectWeWantToDockToTHISObject.transform.SetParent(this.gameObject.transform, true);

        theObjectWeWantToDockToTHISObject.transform.localPosition += offset;

        theObjectWeWantToDockToTHISObject.transform.rotation = this.transform.rotation;
    }
    public void dockThisToOtherObject(GameObject theObjectWeWantToDockTHISObjectTo, Vector3 offset = new Vector3())
    {
        this.transform.position = theObjectWeWantToDockTHISObjectTo.transform.position;



        //theEnactionScript.thisNavMeshAgent = theTank.thisNavMeshAgent;
        //      theTank.pilot = theAuthorScript.enacting.enactionAuthor;
        //      theTank.thePilotEnactionScript = theEnactionScript;

        //this is probably great, but it also disables my camera.  will need to re-arrange things...
        //      theBodyScript.theBodyGameObject.active = false;

        //this.gameObject.transform.SetParent(theAuthorScript.theAuthor.transform, true);
        this.gameObject.transform.SetParent(theObjectWeWantToDockTHISObjectTo.transform, true);

        this.transform.localPosition += offset;

        this.transform.rotation = theObjectWeWantToDockTHISObjectTo.transform.rotation;
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
        //dictOfInteractions

        /*
        if (dictOfInteractionsX == null)
        {

            //Debug.Log("dictOfInteractions == null, fixing");
            dictOfInteractionsX = new Dictionary<enactionCreator.interType, List<Ieffect>>();
        }
        */
        if (dictOfInteractionsX == null)
        {

            //Debug.Log("dictOfInteractions == null, fixing");
            //  Debug.Log("dictOfInteractions == null, trying to add interactionType:  " + interactionType);
            dictOfInteractionsX = new Dictionary<enactionCreator.interType, List<Ieffect>>();
            return dictOfInteractionsX;
        }
        //else if
        if (dictOfInteractionsX.ContainsKey(interactionType))
        {
            //Debug.Log("dictOfInteractions.ContainsKey(interactionType), filling");
            //add the game object to the list of objects tagged with that tag:
            dictOfInteractionsX[interactionType].Add(effect);
            return dictOfInteractionsX;
        }


        //Debug.Log("create key, add value");
        //sigh, need to add the key first, which means the list it unlocks as well...
        List<Ieffect> list = new List<Ieffect>();
        list.Add(effect);
        dictOfInteractionsX.Add(interactionType, list);

        /*
        if (dictOfInteractionsX == null)
        {

            Debug.Log("dictOfInteractions == null");
        }
        else
        {
            foreach (var typex in dictOfInteractionsX.Keys)
            {
                Debug.Log(typex);
            }
        }
        */


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
    //public GameObject enactionAuthor { get; set; }
    public enactionCreator.interType interactionType { get; set; }
    public float magnitudeOfInteraction = 1f;
    public int level = 0;

    public interactionInfo(enactionCreator.interType interactionType, float magnitudeOfInteraction = 1f)
    {
        this.interactionType = interactionType;
        this.magnitudeOfInteraction = magnitudeOfInteraction;
    }

}


public class stateHolder: MonoBehaviour
{
    public Dictionary<interactionCreator.numericalVariable, float> dictOfIvariables = new Dictionary<interactionCreator.numericalVariable, float>();
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
        //Ivariables ivars = objectBeingInteractedWith.GetComponent<Ivariables>();
        stateHolder theStateHolder = objectBeingInteractedWith.GetComponent<stateHolder>();
        if (theStateHolder == null) {return; }

        //interactionInfo interInfo = theAuthorScript.interactionInfo;
        //if (interInfo==null) { return; } //??????? why this happen?
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

        Debug.Log("theStateHolder.dictOfIvariables[toAlter] += amount;");
    }

    private void adjustInteractableVariable(stateHolder theStateHolder, interactionCreator.numericalVariable toAlter, float amount)
    {
        if (theStateHolder.dictOfIvariables.ContainsKey(toAlter) == false) { Debug.Log("ivars.dictOfIvariables.ContainsKey(toAlter) == false.......should never happen?"); return; }

        Debug.Log("ivars.dictOfIvariables[toAlter]:  "+ theStateHolder.dictOfIvariables[toAlter]);
        theStateHolder.dictOfIvariables[toAlter] += amount;

        Debug.Log("ivars.dictOfIvariables[toAlter] += amount;" + theStateHolder.dictOfIvariables[toAlter]);
    }


    private void setInteractableVariable(stateHolder theStateHolder, interactionCreator.numericalVariable toAlter, float amount)
    {
        if (theStateHolder.dictOfIvariables.ContainsKey(toAlter) == false) { Debug.Log("ivars.dictOfIvariables.ContainsKey(toAlter) == false.......should never happen?"); return; }

        Debug.Log("ivars.dictOfIvariables[toAlter]:  " + theStateHolder.dictOfIvariables[toAlter]);
        theStateHolder.dictOfIvariables[toAlter] = amount;

        Debug.Log("ivars.dictOfIvariables[toAlter] = amount;" + theStateHolder.dictOfIvariables[toAlter]);
    }

}

public class playAsPlayable2 : Ieffect
{
    public void implementEffect(GameObject objectBeingInteractedWith, colliderInteractor theCollisionInteractionScript)
    {

        //Debug.Log("thisEffect == effect.useVehicle");
        //Debug.Log("theAuthorScript:  " + theAuthorScript);

        //ad hoc for now
        playable2 thePlayable2 = objectBeingInteractedWith.GetComponent<playable2>();
        if (thePlayable2.occupied == true){Debug.Log("thePlayable2.occupied == true, this playable2 object:  " + objectBeingInteractedWith); return; }

        //IEnactaBool toEnact = theAuthorScript.enacting;
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
        //  Debug.Log("thisEffect == effect.equip");


        //IEnactaBool toEnact = theAuthorScript.enacting;
        GameObject author = theCollisionInteractionScript.enactionAuthor;
        playable2 thePlayable2 = author.GetComponent<playable2>();
        gun1 theGun = objectBeingInteractedWith.GetComponent<gun1>();
        //if (theGun.occupied == true) { return; }



        theGun.equipThisequippable2(thePlayable2);
        interactionCreator.singleton.dockXToY(objectBeingInteractedWith, thePlayable2.enactionPoint1, new Vector3(0.6f, 0.1f, 0));
    }
}

public class putInInventory : Ieffect
{
    public void implementEffect(GameObject objectBeingInteractedWith, colliderInteractor theCollisionInteractionScript)
    {
        //Debug.Log("thisEffect == effect.putInInventory");

        //if (theGun.occupied == true) { return; }

        //IEnactaBool toEnact = theAuthorScript.enacting;
        GameObject author = theCollisionInteractionScript.enactionAuthor;
        inventory1 theInventory = author.GetComponent<inventory1>();
        theInventory.putInInventory(objectBeingInteractedWith);
    }
}

/*
public class damage : Ieffect
{


    int health = 2;  //hmm, does this seem like the right way to do this?  i mean, "damage" and "equip" and other things are supposed to be the interfaces i put on other objects/scripts.....

    public damage(int health = 2)
    {
        this.health = health;
    }

    public void implementEffect(GameObject objectBeingInteractedWith, authorScript1 theAuthorScript)
    {
        //Debug.Log("thisEffect == effect.damage");

        health--;
        if (health > 0) { return; }

        killThisBody(objectBeingInteractedWith);

    }

    public void killThisBody(GameObject objectToDestroy)
    {
        //move to separate script?
        //did that, soooooo now it should be safe to simply just do THIS here:
        UnityEngine.Object.Destroy(objectToDestroy);

    }

}
public class burn : Ieffect
{


    public void implementEffect(GameObject objectBeingInteractedWith, authorScript1 theAuthorScript)
    {
        Debug.Log("burn effect is not implemented yet");
    }

*/




