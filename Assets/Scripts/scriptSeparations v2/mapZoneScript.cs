using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static tagging2;

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
    void Start()
    {
        gameObject.tag = "aMapZone";
    }

    private void initializeZoneNumber()
    {
        thisZoneNumber = tagging2.singleton.objectsInZone.Keys.Count;

        tagging2.singleton.objectsInZone[thisZoneNumber] = new List<objectIdPair>();
        tagging2.singleton.zoneOfObject[tagging2.singleton.idPairGrabify(this.gameObject)] = thisZoneNumber;
        tagging2.singleton.addTag(this.gameObject, tag2.mapZone);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(zoneTaggingConditionsMet(other) == false)
        {
            //Debug.Log("(zoneTaggingConditionsMet(other) == false),  Collider other):  " + other);
            return; 
        }

        //add objects to this zone's list, and update their body to reference this zone
        tagging2.singleton.addToZone(other.transform.gameObject, thisZoneNumber);

    }

    private bool zoneTaggingConditionsMet(Collider other)
    {

        if (tagging2.singleton.allTagsOnObject(other.gameObject).Contains(tag2.zoneable))
        {
            //Debug.Log("other.gameObject.tag == \"dontAddToZones\"");
            return true;
        }

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
    //um, so, what's the point of this?
    //doing a word search, NOTHING.  it does NOTHING.
    //i think the idea was that i'd use this mere component INSTEAD of a tag?  ya.  ok.
    //but i haven't done that yet.
    //should do:  if(myGameObject.GetComponent<zoneable2>() == null) or whatever
}



public class miscZoneScript : MonoBehaviour
{
    public List<objectIdPair> theZoneContentsList = new List<objectIdPair>();

    void Start()
    {
        gameObject.tag = "aMapZone";//??????
    }

    private void OnTriggerEnter(Collider other)
    {
        if (zoneInclusionConditionsMet(other) == false)
        {
            //Debug.Log("(zoneTaggingConditionsMet(other) == false),  Collider other):  " + other);
            return;
        }

        //add objects to this zone's list, and update their body to reference this zone
        //tagging2.singleton.addToZone(other.transform.gameObject, thisZoneNumber);


        theZoneContentsList.Add(tagging2.singleton.idPairGrabify(other.gameObject));
    }

    private bool zoneInclusionConditionsMet(Collider other)
    {

        if (other.gameObject.tag == "dontAddToZones")
        {
            //Debug.Log("other.gameObject.tag == \"dontAddToZones\"");
            return false;
        }


        if (other.gameObject.tag == "interactionType1") { return false; }//???

        if (tagging2.singleton.allTagsOnObject(other.gameObject).Contains(tag2.zoneable))
        {
            //Debug.Log("other.gameObject.tag == \"dontAddToZones\"");
            return true;
        }


        //?????
        return true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "interactionType1") { return; }

        objectIdPair theId = tagging2.singleton.idPairGrabify(other.gameObject);

        if (theZoneContentsList.Contains(theId))
        {
            //print("exit!");
            theZoneContentsList.Remove(theId);
        }
    }
}

public class lightZoneScript : MonoBehaviour
{
    void Start()
    {
        gameObject.tag = "aMapZone";//??????
    }

    private void OnTriggerEnter(Collider other)
    {
        if (zoneInclusionConditionsMet(other) == false)
        {
            //Debug.Log("(zoneTaggingConditionsMet(other) == false),  Collider other):  " + other);
            return;
        }

        //add objects to this zone's list, and update their body to reference this zone
        //tagging2.singleton.addToZone(other.transform.gameObject, thisZoneNumber);

        addToStealthModule(other.gameObject);

        //              theZoneContentsList.Add(tagging2.singleton.idPairGrabify(other.gameObject));
    }

    private void addToStealthModule(GameObject gameObject)
    {
        throw new NotImplementedException();
    }

    private bool zoneInclusionConditionsMet(Collider other)
    {

        if (other.gameObject.tag == "dontAddToZones")
        {
            //Debug.Log("other.gameObject.tag == \"dontAddToZones\"");
            return false;
        }


        if (other.gameObject.tag == "interactionType1") { return false; }//???

        if (tagging2.singleton.allTagsOnObject(other.gameObject).Contains(tag2.zoneable))
        {
            //Debug.Log("other.gameObject.tag == \"dontAddToZones\"");
            return true;
        }


        //?????
        return true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "interactionType1") { return; }

        objectIdPair theId = tagging2.singleton.idPairGrabify(other.gameObject);

        /*
        if (theZoneContentsList.Contains(theId))
        {
            //print("exit!");
            theZoneContentsList.Remove(theId);
        }
        */
    }
}
