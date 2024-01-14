using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class sensorySystem : MonoBehaviour
{
    private worldScript theWorldScript;
    public GameObject target;

    List<GameObject> sensedObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("==================== START OF SENSORY UPDATE ======================");
        target = theWorldScript.theTagScript.findXNearestToYExceptY("interactionEffects1", this.gameObject);
        //Debug.Log(target);
        //Debug.Log(theWorldScript);
        if (theWorldScript.isThereAParentChildRelationshipHere(target, this.gameObject))
        {

            target = theWorldScript.theTagScript.findSECONDNearestXToYExceptY("interactionEffects1", this.gameObject);
        }
       

        //Debug.Log(target);
        //Debug.Log(target.name);
        if(target != null)
        {
            Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, target.GetComponent<Transform>().position, Color.white, 0.1f);
        }
        
        //theWorldScript.theTagScript.findNearestX("interactive1");
        foreach (GameObject obj in sensedObjects)
        {
            //Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, obj.gameObject.GetComponent<Transform>().position, Color.white, 0.1f);
        }


        //Debug.Log("xxxxxxxxxxxxxxxxxxx END of sensory update xxxxxxxxxxxxxxxxxxxxx");
    }




    private void OnTriggerEnter(Collider other)
    {
        //Debug.DrawRay(this.gameObject.GetComponent<Transform>().position, newVector, Color.white, 0.06f);
        //Debug.DrawLine(this.gameObject.GetComponent<Transform>().position, other.gameObject.GetComponent<Transform>().position, Color.white, 6f);
        sensedObjects.Add(other.gameObject);
    }



    public GameObject whichObjectOnListIsNearest(List<GameObject> listOfObjects)
    {
        //closest to what?  to THIS object, i suppose

        GameObject theClosestSoFar = null;

        foreach (GameObject thisObject in listOfObjects)
        {
            if (theClosestSoFar != null)
            {
                float distanceToThisObject = Vector3.Distance(thisObject.transform.position, this.gameObject.transform.position);
                float distanceToTheClosestSoFar = Vector3.Distance(theClosestSoFar.transform.position, this.gameObject.transform.position);

                if (distanceToThisObject < distanceToTheClosestSoFar)
                {
                    theClosestSoFar = thisObject;
                }
            }
            else
            {
                theClosestSoFar = thisObject;
            }
        }

        return theClosestSoFar;
    }





}
