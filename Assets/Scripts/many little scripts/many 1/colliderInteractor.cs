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
        IInteractable theInteractable = other.GetComponent<IInteractable>();
        if (conditionsMet(other, theInteractable)==false) { return; }

        foreach(Ieffect thisEffect in theInteractable.dictOfInteractions[this.interactionType])
        {
            thisEffect.implementEffect(other.gameObject, this);
        }
    }

    bool conditionsMet(Collider other, IInteractable theInteractable)
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
