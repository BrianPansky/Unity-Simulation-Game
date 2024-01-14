using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class basicInputOutput1 : MonoBehaviour
{

    public bool inOutBoolSignal;
    public bool outBoolSignal;
    public bool inBoolSignal;
    public bool dockedBool;

    public GameObject dockedObject;



    // Start is called before the first frame update
    void Start()
    {
        inOutBoolSignal = false;
        outBoolSignal = false;
        inBoolSignal = false;
        dockedBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (outBoolSignal == true)
        {
            outBoolSignal = false;
            //transform.parent; this.transform.Rotate(0.0f, 30.0f, 0.0f, Space.World);
            //this.transform.parent.transform.Rotate(90.0f, 30.0f, 0.0f, Space.World);
            basicInputOutput1 inOutScript = dockedObject.GetComponent<basicInputOutput1>();
            inOutScript.inBoolSignal = true;
            //interactionScript.fire();
            //interactionScript.connectedObjects.Add(this.transform.parent);
        }
        if (inBoolSignal == true)
        {
            inBoolSignal = false;
            //transform.parent; this.transform.Rotate(0.0f, 30.0f, 0.0f, Space.World);
            //this.transform.parent.transform.Rotate(90.0f, 30.0f, 0.0f, Space.World);
            interactive1 interactionScript = this.transform.parent.GetComponent<interactive1>();
            interactionScript.inOutBoolSignal = true;
            //interactionScript.fire();
            //interactionScript.connectedObjects.Add(this.transform.parent);
        }
        if (inOutBoolSignal == true)
        {
            inOutBoolSignal = false;
            //transform.parent; this.transform.Rotate(0.0f, 30.0f, 0.0f, Space.World);
            //this.transform.parent.transform.Rotate(90.0f, 30.0f, 0.0f, Space.World);
            interactive1 interactionScript = this.transform.parent.GetComponent<interactive1>();
            interactionScript.inOutBoolSignal = true;
            //interactionScript.fire();
            //interactionScript.connectedObjects.Add(this.transform.parent);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log(other.gameObject.name);
        if (other.tag == "inputOutput1" && dockedBool == false)
        {
            dockedBool = true;
            dockedObject = other.gameObject;

            //interactive1 interactionScript = this.transform.parent.GetComponent<interactive1>();
            //interactionScript.docked = true;






            //basicInputOutput1 inOutScript = other.GetComponent<basicInputOutput1>();
            //inOutScript.inOutBoolSignal = true;
            //Destroy(this.gameObject);
            //interactive1 interactionScript = other.GetComponent<interactive1>();

            //taggedWith = GetComponent<taggedWith>();
        }
        else if(other.tag == "interactionType1")
        {
            //this.transform.Rotate(30.0f, 00.0f, 0.0f, Space.World);
        }

    }
}
