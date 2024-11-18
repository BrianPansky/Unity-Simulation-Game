using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderInteractor : MonoBehaviour
{
    public enactionCreator.interType interactionType;
    public float magnitudeOfInteraction = 1f;
    public int level = 0;
    public GameObject enactionAuthor;


    //to prevent multiple happening per frame for some reason,
    //even if bullet supposedly has already self destructed!  [doesn't actually destroy until end of frame].
    //won't help if i WANT multiple things to get hit on same frame.....???  ughhhh
    //like...splash damage?
    //in that case, (in collision trigger) add to a list ONLY if they aren't already on the list, then do stuff to them in UPDATE?
    //bool doneCollisionThisFrame = false; 
    //List<Collider> thingsWeHitThisFrame = new List<Collider>();
    List<GameObject> thingsWeHitThisFrame = new List<GameObject>();


    void Update()
    {
        //doneCollisionThisFrame = false;

        //thingsWeHitThisFrame = new List<Collider>();
        thingsWeHitThisFrame = new List<GameObject>();
    }

    public static void genColliderInteractor(GameObject newObjectForProjectile, collisionEnaction theEnactable)
    {
        colliderInteractor ci = newObjectForProjectile.AddComponent<colliderInteractor>();
        interactionInfo ii = theEnactable.interInfo;
        ci.interactionType = ii.interactionType;
        ci.magnitudeOfInteraction = ii.magnitudeOfInteraction;
        ci.level = ii.level;
        ci.enactionAuthor = theEnactable.enactionAuthor;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("this.interactionType:  " + this.interactionType);
        //Debug.Log("thingsWeHitThisFrame.Count:  " + thingsWeHitThisFrame.Count);
        if (thingsWeHitThisFrame.Contains(other.gameObject)) { return; }

        thingsWeHitThisFrame.Add(other.gameObject);
        //Debug.Log("thingsWeHitThisFrame.Count:  " + thingsWeHitThisFrame.Count);
        //Debug.Log("other:  " + other);
        //Debug.Log("other.GetInstanceID():  " + other.GetInstanceID());
        //Debug.Log("other.gameObject:  " + other.gameObject);

        theInteractionCode(other);
    }

    private void theInteractionCode(Collider other)
    {
        //if (doneCollisionThisFrame) { return; }
        //Debug.Log("................ONE COLLISION TRIGGER ENTER..............." + Time.fixedTime);
        interactable2 theInteractable = other.GetComponent<interactable2>();
        if (noErrors(other, theInteractable) == false) { return; }

        //Debug.Log("theInteractable.dictOfInteractions[this.interactionType].Count" + theInteractable.dictOfInteractions[this.interactionType].Count);

        foreach (IInteraction thisInteraction in theInteractable.dictOfInteractions[this.interactionType])
        {
            thisInteraction.doInteraction(other.gameObject, enactionAuthor, thisToInterInfo());
        }

        //doneCollisionThisFrame = true;
    }

    private interactionInfo thisToInterInfo()
    {
        return new interactionInfo(interactionType, magnitudeOfInteraction, level);
    }

    bool noErrors(Collider other, interactable2 theInteractable)
    {
        if (theInteractable == null) { return false; }
        if (theInteractable.dictOfInteractions == null)
        {
            //Debug.Log("i don't think this should ever happen:   theInteractable.dictOfInteractions == null");
            return false;
        }
        if (theInteractable.dictOfInteractions.ContainsKey(this.interactionType) != true)
        {
            //Debug.Log("dictOfInteractions.ContainsKey(theAuthorScript.interactionType) != true, object being interacted with doesn't have this key:  " + this.interactionType);
            //Debug.Log("instead, it only has the following:  ");

            //foreach (var key in theInteractable.dictOfInteractions.Keys)
            {
                //Debug.Log(key);
            }
            //Debug.Log("..........................................................");
            return false;
        }

        return true;
    }

}
