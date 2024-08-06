using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapZoneScript : MonoBehaviour
{
    public int thisZoneNumber = 0;
    public bool isItThisZonesTurn = false;
    public int howManyUpdatesPerEntity = 1; //GETS SET BY WORLD SCRIPT

    public List<GameObject> theList = new List<GameObject>();
    public List<GameObject> threatList = new List<GameObject>();

    public worldScript theWorldScript;

    void Awake()
    {
        initializeZoneNumber();
    }


    private void initializeZoneNumber()
    {
        thisZoneNumber = tagging2.singleton.objectsInZone.Keys.Count;

        tagging2.singleton.objectsInZone[thisZoneNumber] = new List<objectIdPair>();
        tagging2.singleton.zoneOfObject[tagging2.singleton.idPairGrabify(this.gameObject)] = thisZoneNumber;
    }
    

    public void oldZoneTurnOffUNUSED()
    {
        if (isItThisZonesTurn)
        {
            howManyUpdatesPerEntity--;
            if (howManyUpdatesPerEntity == 0)
            {
                isItThisZonesTurn = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collider other):  " + other );
        if(zoneTaggingConditionsMet(other) == false) { return; }

        //add objects to this zone's list, and update their body to reference this zone
        tagging2.singleton.addToZone(other.transform.gameObject, thisZoneNumber);


        if (tagging2.singleton.allTagsOnObject(other.transform.gameObject).Contains(tagging2.tag2.zoneable))
        {
            
        }
        else
        {
            //Debug.Log("does NOT Contains(tagging2.tag2.zoneable), DON'T addToZone");
        }
    }

    private bool zoneTaggingConditionsMet(Collider other)
    {
        if (other.gameObject.GetComponent<IInteractable>() == null)
        {
            return false;
        }
        else
        {
        }

        if (other.gameObject.tag == "dontAddToZones")
        {
            //Debug.Log("other.gameObject.tag == \"dontAddToZones\"");
            return false;
        }

        return true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "interactionType1")
        {
        }
    }
}

public class zoneable2 : MonoBehaviour
{
}