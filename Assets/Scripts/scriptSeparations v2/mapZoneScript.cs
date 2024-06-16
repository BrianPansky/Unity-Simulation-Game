using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapZoneScript : MonoBehaviour
{

    public int thisZoneNumber = 0;






    public bool isItThisZonesTurn = false;
    public int howManyUpdatesPerEntity = 1; //GETS SET BY WORLD SCRIPT

    //public List<GameObject> theMapZoneListOfGameObjects = new List<GameObject>();
    //              public List<GameObject> theList = new List<GameObject>();
    //              public List<GameObject> threatList;

    public List<GameObject> theList = new List<GameObject>();
    public List<GameObject> threatList = new List<GameObject>();

    public worldScript theWorldScript;

    void Awake()
    {

        initializeZoneNumber();
    }

    // Start is called before the first frame update
    void Start()
    {
        //tagging2.singleton.zoneOfObject[tagging2.singleton.]

        /*

        //add self to a world script list/tag, then NPCs can pick the closest one
        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        theWorldScript.theTagScript.foreignAddTag("mapZone", this.gameObject);

        //Debug.Log("adding map zone:  ");
        //Debug.Log("BEFORE theWorldScript.listOfMapZoneScripts.Count:  " + theWorldScript.listOfMapZoneScripts.Count);
        //Debug.Log("BEFORE theWorldScript.listOfMapZoneObjects.Count:  " + theWorldScript.listOfMapZoneObjects.Count);
        theWorldScript.listOfMapZoneScripts.Add(this);
        theWorldScript.listOfMapZoneObjects.Add(this.gameObject);
        //Debug.Log("AFTER theWorldScript.listOfMapZoneScripts.Count:  " + theWorldScript.listOfMapZoneScripts.Count);
        //Debug.Log("AFTER theWorldScript.listOfMapZoneObjects.Count:  " + theWorldScript.listOfMapZoneObjects.Count);

        */

    }

    private void initializeZoneNumber()
    {
        thisZoneNumber = tagging2.singleton.objectsInZone.Keys.Count;

        tagging2.singleton.objectsInZone[thisZoneNumber] = new List<objectIdPair>();
        tagging2.singleton.zoneOfObject[tagging2.singleton.idPairGrabify(this.gameObject)] = thisZoneNumber;
    }
    

    // Update is called once per frame
    void Update()
    {
        
        
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
        //add objects to this zone's list, and update their body to reference this zone
        if (other.gameObject.tag == "interactionType1") { return; }
        if (other.gameObject.tag == "dontAddToZones") { return; }

        {
            tagging2.singleton.addToZone(other.transform.gameObject, thisZoneNumber);
            


            //theList.Add(other.gameObject);

            /*

            //theLocalMapZoneScript
            //AIHub2 theHub = thisObject2.GetComponent<AIHub2>();
            body1 aBody = other.gameObject.GetComponent<body1>();
            if (aBody != null) 
            {
                if (aBody.theLocalMapZoneScript != null)
                {
                    aBody.theLocalMapZoneScript.theList.Remove(other.gameObject);
                    aBody.theLocalMapZoneScript.threatList.Remove(other.gameObject);
                }
                
                aBody.theLocalMapZoneScript = this;
            }

            */

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "interactionType1")
        {
            

        }
    }



    


}

