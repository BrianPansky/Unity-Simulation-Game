using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapZoneScript : MonoBehaviour
{
    public bool isItThisZonesTurn = false;
    public int howManyUpdatesPerEntity = 1; //GETS SET BY WORLD SCRIPT

    //public List<GameObject> theMapZoneListOfGameObjects = new List<GameObject>();
    public List<GameObject> theList = new List<GameObject>();
    public List<GameObject> threatList;

    public worldScript theWorldScript;

    // Start is called before the first frame update
    void Start()
    {
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
        if (other.gameObject.tag != "interactionType1")
        {
            theList.Add(other.gameObject);
            //theLocalMapZoneScript
            //AIHub2 theHub = thisObject2.GetComponent<AIHub2>();
            body1 aBody = other.gameObject.GetComponent<body1>();
            if (aBody != null) 
            {
                aBody.theLocalMapZoneScript = this;
            }

            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "interactionType1")
        {
            theList.Remove(other.gameObject);
            Debug.Log("removed this object from map zone list:  " + other.gameObject);
        }
    }



}
