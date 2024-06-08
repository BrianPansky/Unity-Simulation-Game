using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class senseZoneScript : MonoBehaviour
{
    // for now, this script keeps a list of OTHER nearby zones
    //then the NPC can check those zones


    public List<GameObject> listOfForbiddenZones = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);


        /*

        //first make sure it even HAS my tagging script attached:
        if (other.GetComponent<taggedWith>() != null)
        {
            //cool it does, can get it:
            taggedWith tagScriptToCheck = other.GetComponent<taggedWith>();
            //Debug.Log("hello???");


            if (tagScriptToCheck.tags.Contains("forbidden zone"))
            {
                //Debug.Log("got one!!!");
                listOfForbiddenZones.Add(other.gameObject);
            }
        }

        */
        
    }
    private void OnTriggerExit(Collider other)
    {
        //shouldn't this just check if it's on the list???  Is that doable?  Surely it must be.
        //the "Contains" thing should work?
        if (listOfForbiddenZones.Contains(other.gameObject))
        {
            //print("exit!");
            listOfForbiddenZones.Remove(other.gameObject);
        }
    }
}
