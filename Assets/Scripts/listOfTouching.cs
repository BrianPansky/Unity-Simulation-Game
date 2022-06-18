using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class listOfTouching : MonoBehaviour
{
    //this script keeps a list of all NPC objects currently touching the object this script is attached to

    public List<GameObject> theList = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("it works");
        //Debug.Log(other);
        if (other.gameObject.tag == "anNPC")
        {
            theList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("it works");
        //Debug.Log(other);
        if (other.gameObject.tag == "anNPC")
        {
            theList.Remove(other.gameObject);
        }
    }
}
