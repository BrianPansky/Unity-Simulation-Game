using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleInteractable : MonoBehaviour, IInteractable
{
    public Dictionary<enactionCreator.interType, List<Ieffect>> dictOfInteractions { get; set; }

    public static void genSimpleInteractable(GameObject theObject, enactionCreator.interType interactionType, Ieffect theEffect)
    {
        simpleInteractable theSI = theObject.GetComponent<simpleInteractable>();
        if (theSI != null)
        {
            Debug.Log("already has a simpleInteractable component!");
            return;
        }
        theSI = theObject.AddComponent<simpleInteractable>();
        
        List<Ieffect> needAList = new List<Ieffect>();
        //needAList.Add(theEffect);
        //theSI.dictOfInteractions[interactionType] = needAList;

        theSI.dictOfInteractions = interactionCreator.singleton.addInteraction(theSI.dictOfInteractions, interactionType, theEffect);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
