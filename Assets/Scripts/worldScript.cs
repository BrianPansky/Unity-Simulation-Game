using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class worldScript : MonoBehaviour
{

    public Dictionary<string, List<GameObject>> taggedStuff = new Dictionary<string, List<GameObject>>();
    public Dictionary<string, List<testInteraction>> interactionLegibstration = new Dictionary<string, List<testInteraction>>();
    //public List<GameObject> interactionLegibstration = new List<GameObject>();
    public int theTime;



    //other scripts:
    //public AI1 thisAI;
    public premadeStuffForAI premadeStuff;
    public taggedWith theTagScript;
    public nonAIScript theNonAIScript;
    //public social theSocialScript;
    //public repositoryOfScriptsAndPrefabs theRespository;
    public repository2 theRespository;
    void Awake()
    {
        theTime = 0;

        //I'M JUST PLUGGING THIS IN [in the editor, right hand side] INSTEAD   theTagScript = this.gameObject.GetComponent<taggedWith>();
        
    }



    // Start is called before the first frame update
    void Start()
    {
        theTagScript.addTag("worldObject");


    }

    // Update is called once per frame
    void Update()
    {
        timeIncrement();
        //foreach(GameObject x in taggedStuff["interactive1"])
        {
            //Debug.Log(x.name);
        }
        //Debug.Log("====zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz=======");

        /*
        if (taggedStuff.ContainsKey("store"))
        {
            foreach(GameObject item in taggedStuff["store"])
            {
                Debug.Log(item.name);
            }
        }
        */
        
    }

    public void timeIncrement()
    {
        theTime += 1;
        //Debug.Log(theTime);
    }


    public bool isThereAParentChildRelationshipHere(GameObject object1, GameObject object2)
    {
        //if EITHER is a parent of the other, return "true"
        //Debug.Log("if EITHER is a parent of the other, return true");
        //Debug.Log(object1);
        //Debug.Log(object2);

        //greatgrandchild1.transform.parent.gameObject

        //note the bit about "root".  basically that checks if it HAS a parent
        //if there is not parent, simply checking "myObject.transform.parent.gameObject" would have null reference error
        if (object1.transform.root != object1.transform && object1.transform.parent.gameObject == object2.gameObject)
        {
            return true;
        }
        if (object2.transform.root != object2.transform && object2.transform.parent.gameObject == object1.gameObject)
        {
            return true;
        }

        return false;
    }


    

}
