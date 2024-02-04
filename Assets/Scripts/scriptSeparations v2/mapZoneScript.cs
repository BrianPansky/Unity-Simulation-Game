using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapZoneScript : MonoBehaviour
{

    public List<GameObject> theList = new List<GameObject>();

    public worldScript theWorldScript;

    // Start is called before the first frame update
    void Start()
    {
        //add self to a world script list/tag, then NPCs can pick the closest one
        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        theWorldScript.theTagScript.foreignAddTag("mapZone", this.gameObject);
        theWorldScript.listOfMapZoneScripts.Add(this);


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "interactionType1")
        {
            theList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "interactionType1")
        {
            theList.Remove(other.gameObject);
        }
    }



}
