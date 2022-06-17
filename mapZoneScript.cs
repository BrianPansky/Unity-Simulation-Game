using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapZoneScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("it works");
        //Debug.Log(other);
        if(other.gameObject.tag == "anNPC")
        {
            //Debug.Log("it works");

            //now we can update the "locationState" of the NPC
            AI1 scriptX = other.GetComponent<AI1>();
            //Debug.Log(this.transform.parent.name);
            List<stateItem> newLocationList = new List<stateItem>();
            //Debug.Log(f);
            newLocationList.Add(scriptX.map[this.transform.parent.name]);
            scriptX.state["locationState"] = newLocationList;

            //Component theScript = other.GetComponent("Script");
            //Debug.Log(theScript);  //returns Null, because there is no component called "script", the script is called "AI1"
            //.locationState = this.name;

        }
    }

}