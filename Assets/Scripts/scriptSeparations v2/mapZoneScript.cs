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
    

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collider other):  " + other );
        if(zoneTaggingConditionsMet(other) == false) { return; }

        //add objects to this zone's list, and update their body to reference this zone
        tagging2.singleton.addToZone(other.transform.gameObject, thisZoneNumber);

    }

    private bool zoneTaggingConditionsMet(Collider other)
    {
        if (other.gameObject.GetComponent<interactable2>() == null)
        {
            return false;
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