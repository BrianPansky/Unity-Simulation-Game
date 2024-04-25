using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class authorScript1 : MonoBehaviour
{
    //to track who does an interaction
    //      should this use "interactionMate" isntead?

    public GameObject theAuthor;
    public string interactionType;

    public float magnitudeOfInteraction = 1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("the author is:  "  + theAuthor.name);
        //Debug.Log("the author is:  " + theAuthor);
        //Debug.Log("authorScript is ON this object:  " + this.gameObject.name);
        //Debug.Log("33333333333333333333333333333333333333333 the interactionType is:  " + interactionType);
    }
}
