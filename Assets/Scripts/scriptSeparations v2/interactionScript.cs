using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static interactionScript;

public class interactionScript : MonoBehaviour
{
    //public Dictionary<enactionCreator.interType, List<effect>> dictOfInteractions = new Dictionary<enactionCreator.interType, List<effect>>();
    //          public Dictionary<enactionCreator.interType, List<Ieffect>> dictOfInteractions = new Dictionary<enactionCreator.interType, List<Ieffect>>();


    int cooldown = 0;
    public enum effect
    {
        errorYouDidntSetEnumTypeForEFFECT,
        damage,
        burn,
        useVehicle,
        activate1,
        putInInventory,
        equip
    }


    void Awake()
    {
        //Debug.Log("Awake:  " + this);
        //Debug.Log("awake this.gameObject:  " + this.gameObject.GetInstanceID());
        if (tagging2.singleton == null)
        {
            Debug.Log("tagging2.singleton is null, cannot use it to add tags.  this happens for objects that have been added to the scene using the editor, because they exist before any scripts are called.  solution:  do not add prefabs to scene using editor.  generate them after singletons have initialized.");
        }

        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.interactable);
        tagging2.singleton.addTag(this.gameObject, tagging2.tag2.zoneable);

        //Debug.Log("now show what tags are on the object:  ");
        foreach (tagging2.tag2 thisTag in tagging2.singleton.allTagsOnObject(this.gameObject))
        {
            //Debug.Log(thisTag);
        }
    }
    void OnEnable()
    {

        if (this.gameObject.GetComponent<safeDestroy>() == null)
        {
            this.gameObject.AddComponent<safeDestroy>();
        }

        //Debug.Log("tagging2.singleton:  " + tagging2.singleton);
        
    }
    void Start()
    {
        //Debug.Log("start this.gameObject:  " + this.gameObject.GetInstanceID());

    }

    

    /*
    


    */

}






