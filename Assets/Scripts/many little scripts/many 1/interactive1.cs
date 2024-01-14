using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactive1 : MonoBehaviour
{
    //messing around with object interaction
    public GameObject mainGameObject;
    public GameObject secondaryGameObject;

    public GameObject input1;
    public List<GameObject> connectedObjects = new List<GameObject>();
    public bool inOutBoolSignal;
    public bool docked;

    // Start is called before the first frame update
    void Start()
    {
        //ALL interaction scripts should list themselves in a world [or nearby] abstract object, so that npcs can find nearby interactions?
        //ok, so how?  how is best way [that is least annoying] to call "world script" [or whatever] from here?
        //this is what my taggins system uses, and that has been going fine [and what i'm doing right now is actually quite similar to tagging, maybe i should do that either also or instead......hmm, ya i should do THAT, sorta....well, i can make tags from HERE?  don't need a tagging script?]
        //indeed the world script itself should have a convenient link to a tagging script.  to ALL scripts i might want.  duh.
        GameObject theWorldObject = GameObject.Find("World");
        worldScript theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;

        theWorldScript.theTagScript.foreignAddTag("interactive1", this.gameObject);




        //BOOL NEVER STARTS AS "NULL"??????  OKAYYYYYY
        //if (inOutBoolSignal == null)
        {
            //inOutBoolSignal = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }



    
}
