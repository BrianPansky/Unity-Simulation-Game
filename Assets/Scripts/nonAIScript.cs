using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonAIScript : MonoBehaviour
{
    //this script is for code that is not part of the "intelligence".  can be used by both the NPCs AND the player

    //effect prefabs:
    public GameObject gunFlash1;

    //building prefabs:
    public GameObject storePrefab;
    public GameObject storagePrefab;

    //other prefabs:
    public GameObject invisibleTarget;

    //other scripts:
    public AI1 thisAI;
    public taggedWith taggedWith;
    public social theSocialScript;
    public functionsForAI theFunctions;
    public premadeStuffForAI premadeStuff;


    void Awake()
    {
        //initialize other scripts:
        thisAI = GetComponent<AI1>();
        taggedWith = GetComponent<taggedWith>();
        theSocialScript = GetComponent<social>();
        theFunctions = GetComponent<functionsForAI>();
        premadeStuff = GetComponent<premadeStuffForAI>();

        
    }

    // Start is called before the first frame update
    void Start()
    {
        //ONLY NEEDED FOR SOME OBJECTS:
        //selfDestructSomeObjects();
        //storageInitialization();
    }

    // Update is called once per frame
    void Update()
    {
    }


    //========================= WEAPON STUFF =========================
    public void basicFiringFunction(Vector3 theLine)
    {
        //input a vector
        //see what gets hit
        //implement damage or whatever on.....whatever gets hit?
        //[FOR NOW ONLY KILL WHAT IS HIT IF IT IS A "person"!]
        //[and also (for testing) do NOT kill the player!]

        //first, make non-allies [and not self] afraid and hide [should move this to SENSING?  probably eventually, so use "theFunctions" for now to indicate that]:
        theFunctions.gunShotSoundSensing();

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
        basicFiringFunction(makeVectorInaccurate(theLine, 11));
    }

    public void kill(GameObject whoToKill)
    {

        //don't kill player for testing:
        if (whoToKill.name != "Player")
        {
            //NEED TO REMOVE THIS OBJECT FROM ALL LISTS BEFORE DESTROYING IT!
            //OTHERWISE WILL GET NULL/"object does not exist" ERRORS!!!!!
            
            Debug.Log("a killer just shot " + whoToKill.name);
            if (this.gameObject.name == thisAI.npcx)
            {

                taggedWith.printAllTags();
            }
            this.gameObject.GetComponent<taggedWith>().foreignRemoveALLtags(whoToKill);
            //print("a killer just shot " + whoToKill.name);
            if (this.gameObject.name == thisAI.npcx)
            {

                taggedWith.printAllTags();
            }

            //print("object to be destroyed:");
            //print(whoToKill);
            Destroy(whoToKill);
            

        }
        else
        {
            //print("you are shot!");
            //whoToKill = null;
        }

        
    }



    //========================= BUILDING STUFF =========================
    public void createBuildingX(GameObject buildingX, Vector3 locationVector)
    {
        //input a prefab
        //will instantiate it RIGHT WHERE npc IS STANDING
        //and will update ownership tags
        //some ad-hoc junk too, alas [does "HIRING" of the builder!!!]

        
        //create building/prefab:
        GameObject newBuilding = new GameObject();
        //newShop = Instantiate(storePrefab, new Vector3(5, 0, -11), Quaternion.identity);

        //---get XYZ values from gameObject.transform somehow
        //---adjust Y value
        //---combine XYZ values into...a 3Vector or whatever...somehow
        //---plug into instantiate function
        //Vector3 whereToPlace = new Vector3(gameObject.transform.x, (gameObject.transform.y - 113), gameObject.transform.z);

        //newBuilding = Instantiate(buildingX, new Vector3(gameObject.transform.position.x, (gameObject.transform.position.y - 1), gameObject.transform.position.z), Quaternion.identity);
        newBuilding = Instantiate(buildingX, locationVector, Quaternion.identity);


        //now "buy" it:

        //check if it's for sale:
        //get other script I need:
        taggedWith otherIsTaggedWith = newBuilding.GetComponent<taggedWith>() as taggedWith;

        //Debug.Log("1111111111111111111111111111111111111111111111");
        //string ownershipTag = "owned by " + this.name;
        //Debug.Log(ownershipTag);
        string ownershipTag = "owned by " + thisAI.leader.name;
        //Debug.Log(ownershipTag);
        otherIsTaggedWith.foreignAddTag(ownershipTag, newBuilding);

        //need to remember in the future WHICH store is theirs
        //so they ca go to it, and sned their employees there:
        //thisAI.roleLocation = target;

        //ad-hoc action completion:
        //thisAI.toDoList.RemoveAt(0);


        //make this NPC self employed, so i will have role-location for buying from them...bit ad-hoc...
        //SHOULD MOVE THIS TO "CREATE STORE" ENACTION, BUT
        //REQUIRES THE "NEWBUILDING" GAME OBJECT, NOT JUST THE PREFAB!
        //so, should RETURN this generated building object, to be used further
        //[or have OTHER way to "find" it for use]
        if (buildingX.name == "storeToCreate")
        {
            //won't this add a DUPLICATE "leader" tag?
            theSocialScript.doSuccsessfulHiring(thisAI, premadeStuff.cashierJob, newBuilding);
        }


    }




    //buy/sell stuff
    public bool TRYbuyingBuilding(GameObject buildingToBuy)
    {

        //check if it's for sale:
        //get other script I need:
        taggedWith otherIsTaggedWith = buildingToBuy.GetComponent<taggedWith>() as taggedWith;
        if (otherIsTaggedWith.tags.Contains("forSale"))
        {
            //ok, it's for sale, now can buy it

            //printTextList(otherIsTaggedWith.tags);
            //remove the "for sale" tag:
            otherIsTaggedWith.foreignRemoveBOTHTags("forSale", buildingToBuy);
            //printTextList(otherIsTaggedWith.tags);
            //add the "owned by _____" tag...:
            string ownershipTag = "owned by " + this.name;
            otherIsTaggedWith.foreignAddTag(ownershipTag, buildingToBuy);


            return true;
        }
        else
        {
            return false;
        }


    }





    //========================= for specific objects =========================
    public void selfDestructSomeObjects()
    {
        //no, this doesn't work because instantiated objects have a number appended to their name
        //need to use a different method.  my tags?  tag with god damn "self Destruct"? 
        //or no wait, they only have "(Clone)" appended to their names?  so try that some day...
        if(this.gameObject.name == "gunFlash1(Clone)")
        {
            Destroy(this.gameObject);
        }
    }

    public void testByName(string theName)
    {
        if(this.gameObject.name == theName)
        {
            Debug.Log("heloooooooooooooooooooooooooo");
        }
    }

    public void storageInitialization()
    {
        if (this.gameObject.name == "storageToCreate(Clone)")
        {
            thisAI.secondaryObject = taggedWith.findNearestX("resource1");
            //Debug.Log(thisAI.secondaryObject.name);
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

        if (Physics.Raycast(this.gameObject.transform.position, theLine, out infoAboutTheThingTheRaycastHit, 500.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
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

    public Vector3 vectorFromXToY(GameObject objectX, GameObject objectY)
    {
        //returns a vector between current object/NPC, and the inputted target
        Vector3 theVector = objectY.transform.position - objectX.transform.position;

        //Debug.DrawLine(this.transform.position, this.transform.position + this.transform.right, Color.green, 3);
        //Debug.DrawRay(this.gameObject.GetComponent<Transform>().position, theVector * Vector3.Distance(target.transform.position, transform.position), Color.green, 3);

        //Debug.DrawRay(objectX.GetComponent<Transform>().position, theVector, Color.white, 3);


        return theVector;
    }

    public bool doesLineOfSightSeeTarget(GameObject target)
    {
        GameObject whatIsHit = whatDoesLineHit(vectorToTarget(target) * 6);
        if(whatIsHit == null)
        {
            return false;
        }
        if (target.name == whatIsHit.name)
        {
            //Debug.Log("yes");
            return true;
        }
        else
        {
            //Debug.Log(target.name);
            if(whatIsHit != null)
            {
                //Debug.Log(whatIsHit.name);
            }
            else
            {
                Debug.Log("null");
            }
            return false;
        }
    }

    public Vector3 makeVectorInaccurate(Vector3 originalVector, float accuracy)
    {
        Vector3 newVector = new Vector3();
        //Vector3 newVector = new Vector3(originalVector.x + randomSpread(accuracy), originalVector.y + randomSpread(accuracy), originalVector.z + randomSpread(accuracy));

        //Vector3 axis = Vector3.Cross(originalVector, Vector3.up);

        //newVector = Quaternion.AngleAxis(randomSpread(accuracy), axis) * originalVector;
        newVector = Quaternion.Euler(randomSpread(accuracy), randomSpread(accuracy), randomSpread(accuracy)) * originalVector;

        Debug.DrawRay(this.gameObject.GetComponent<Transform>().position, newVector, Color.red, 3);

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
