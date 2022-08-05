using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonAIScript : MonoBehaviour
{
    //this script is for code that is not part of the "intelligence".  can be used by both the NPCs AND the player

    //effect prefabs:
    public GameObject gunFlash1;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        selfDestructSomeObjects();
    }


    //========================= WEAPON STUFF =========================
    public void basicFiringFunction(Vector3 theLine)
    {
        //input a vector
        //see what gets hit
        //implement damage or whatever on.....whatever gets hit?
        //[FOR NOW ONLY KILL WHAT IS HIT IF IT IS A "person"!]
        //[and also (for testing) do NOT kill the player!]

        GameObject whatIsHit = whatDoesLineHit(theLine);
        if (whatIsHit != null)
        {
            if (whatIsHit.tag == "anNPC")
            {
                kill(whatIsHit);
            }
            
        }
    }

    public void basicFiringWithInnacuracy(Vector3 theLine)
    {
        basicFiringFunction(makeVectorInaccurate(theLine, 77));
    }

    public void kill(GameObject whoToKill)
    {

        //don't kill player for testing:
        if (whoToKill.name != "Player")
        {
            //NEED TO REMOVE THIS OBJECT FROM ALL LISTS BEFORE DESTROYING IT!
            //OTHERWISE WILL GET NULL/"object does not exist" ERRORS!!!!!

            Debug.Log("a killer just shot " + whoToKill.name);

            this.gameObject.GetComponent<taggedWith>().foreignRemoveALLtags(whoToKill);
            //print("a killer just shot " + whoToKill.name);
            
            //print("object to be destroyed:");
            //print(whoToKill);
            Destroy(whoToKill);
        }
        else
        {
            print("you are shot!");
            //whoToKill = null;
        }
    }


    //========================= misc =========================
    public void selfDestructSomeObjects()
    {
        //no, this doesn't work because instantiated objects have a number appended to their name
        //need to use a different method.  my tags?  tag with god damn "self Destruct"? 
        if(this.gameObject.name == "gunFlash1")
        {
            Destroy(this.gameObject);
        }
    }




    //========================= raycast/vector stuff ========================= [often used for weapon shooting, but may have other uses]
    public GameObject whatDoesLineHit(Vector3 theLine)
    {
        //returns the object that a line [vector3 i guess] points at

        GameObject whatTheLineHits;
        whatTheLineHits = null;


        RaycastHit infoAboutTheThingTheRaycastHit;
        //Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(this.gameObject.transform.position, theLine, out infoAboutTheThingTheRaycastHit, 50.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (infoAboutTheThingTheRaycastHit.transform != null)
            {
                //Debug.Log(myHit.transform.gameObject);
                whatTheLineHits = infoAboutTheThingTheRaycastHit.transform.gameObject;
            }
        }

        

        return whatTheLineHits;
    }


    public Vector3 vectorToTarget(GameObject target)
    {
        //returns a vector between current object/NPC, and the inputted target
        Vector3 theVector = target.transform.position - this.transform.position;

        //Debug.DrawLine(this.transform.position, this.transform.position + this.transform.right, Color.green, 3);
        //Debug.DrawRay(this.gameObject.GetComponent<Transform>().position, theVector * Vector3.Distance(target.transform.position, transform.position), Color.green, 3);

        Debug.DrawRay(this.gameObject.GetComponent<Transform>().position, theVector, Color.green, 3);


        return theVector;
    }

    public Vector3 makeVectorInaccurate(Vector3 originalVector, float accuracy)
    {
        Vector3 newVector = new Vector3();
        //Vector3 newVector = new Vector3(originalVector.x + randomSpread(accuracy), originalVector.y + randomSpread(accuracy), originalVector.z + randomSpread(accuracy));

        //Vector3 axis = Vector3.Cross(originalVector, Vector3.up);

        //newVector = Quaternion.AngleAxis(randomSpread(accuracy), axis) * originalVector;
        newVector = Quaternion.Euler(randomSpread(accuracy), randomSpread(accuracy), randomSpread(accuracy)) * originalVector;

        Debug.DrawRay(this.gameObject.GetComponent<Transform>().position, newVector, Color.blue, 3);

        return newVector;
    }

    public float randomSpread(float accuracy)
    {
        float newNumber = Random.Range(-accuracy, accuracy);

        return newNumber;
    }
    
    public void raycastFromCameraPrefabPlacement(GameObject thePrefabToPlace)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo) && thePrefabToPlace != null)
        {
            GameObject x = new GameObject();
            x = Instantiate(thePrefabToPlace, hitInfo.point, Quaternion.identity);
            //myPrefab.transform.position = hitInfo.point;
            //myPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }
    
    public void showLineFromThisToObject(GameObject target)
    {
        if (target != null)
        {
            Vector3 newVector = vectorToTarget(target);
            //makeVectorInaccurate(newVector, 0.5f);
            makeVectorInaccurate(newVector, 15f);
            makeVectorInaccurate(newVector, 15f);
            makeVectorInaccurate(newVector, 15f);
            makeVectorInaccurate(newVector, 15f);
        }
        else
        {
            Debug.Log("cannot display line, the object is null");
        }
    }



}
