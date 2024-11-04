using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleInteractable : interactable2
{
    public static void genSimpleInteractable(GameObject theObject, enactionCreator.interType interactionType, IInteraction theInteraction)
    {
        simpleInteractable theSI = theObject.GetComponent<simpleInteractable>();
        if (theSI != null)
        {
            Debug.Log("already has a simpleInteractable component!");
            return;
        }
        theSI = theObject.AddComponent<simpleInteractable>();
        
        //List<IInteraction> needAList = new List<IInteraction>();
        //needAList.Add(theEffect);
        //theSI.dictOfInteractions[interactionType] = needAList;

        theSI.dictOfInteractions = interactionCreator.singleton.addInteraction(theSI.dictOfInteractions, interactionType, theInteraction);
    }
}
