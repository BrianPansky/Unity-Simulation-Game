using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class interactionScript : MonoBehaviour
{

    //public Dictionary<string, testInteraction> interactionDictionary = new Dictionary<string, testInteraction>();
    //public List<string> listOfInteractions = new List<string>();
    public Dictionary<string, string> dictOfInteractions = new Dictionary<string, string>();


    public worldScript theWorldScript;
    void Start()
    {
        GameObject theWorldObject = GameObject.Find("World");
        theWorldScript = theWorldObject.GetComponent("worldScript") as worldScript;
        theWorldScript.theTagScript.foreignAddTag("interactable", this.gameObject);

    }

    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("YYYYYY     START onTriggerEnter for:  " + this.gameObject.name + "     YYYYYY");

        if (other.tag == "interactionType1")
        {

            authorScript1 theAuthorScript = other.gameObject.GetComponent<authorScript1>();
            //Debug.Log("2222222222222the interaction type is:  " + theAuthorScript.interactionType);
            if (dictOfInteractions.ContainsKey(theAuthorScript.interactionType))
            {
                //      quick way to see effects from far away for testing:
                //this.gameObject.transform.localScale = new Vector3(1f, 44, 1f);



                string theDictEntry = dictOfInteractions[theAuthorScript.interactionType];
                
                //Debug.Log("the corrosponding interaction dictionary entry is:  " + theDictEntry);

                if (theDictEntry == "clickLock")
                {
                    if (theAuthorScript.theAuthor.GetComponent<inventory1>().testInventory1.Contains("testKey1") == true)
                    {
                        this.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 0f, 1f);
                        Vector3 p1 = this.gameObject.transform.position;
                        Vector3 p2 = new Vector3(p1.x, p1.y + 22, p1.z);
                        Debug.DrawLine(p1, p2, new Color(1f, 0f, 1f), 9999f);

                        //Debug.DrawLine(this.gameObject.transform.position, enactionTarget.transform.position, Color.blue, 0.9f);

                        enactionScript theEnactionScript = theAuthorScript.theAuthor.GetComponent<enactionScript>();
                        theEnactionScript.availableEnactions.Add("shoot1");

                        //theAuthorScript.theAuthor.transform.localScale = new Vector3(1f, 22, 1f);

                        if (true == false)
                        {
                            interactionEffects1 authorInteractionScript = theAuthorScript.theAuthor.GetComponent<interactionEffects1>();


                            if (authorInteractionScript.interactionDictionary.ContainsKey("doAShootingEnaction") == false)
                            {
                                testInteraction mainInteraction = authorInteractionScript.generateInteraction("aShootingEnaction");
                                authorInteractionScript.interactionDictionary.Add("doAShootingEnaction", mainInteraction);
                            }
                        }
                    }
                }
                if (theDictEntry == "grabKey")
                {
                    this.gameObject.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f);
                    Vector3 p1 = this.gameObject.transform.position;
                    Vector3 p2 = new Vector3(p1.x, p1.y + 22, p1.z);
                    Debug.DrawLine(p1, p2, new Color(0f, 1f, 0f), 9999f);

                    theAuthorScript.theAuthor.GetComponent<inventory1>().testInventory1.Add("testKey1");
                    
                }

                if (theDictEntry == "die")
                {
                    Debug.Log("this SHOULD destroy the AI...................................................................................................................................................");
                    Vector3 p1 = this.gameObject.transform.position;
                    Vector3 p2 = new Vector3(p1.x, p1.y + 22, p1.z);
                    Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 9999f);

                    //Debug.Log(UnityEngine);
                    //Debug.Log(UnityEngine.Object);
                    UnityEngine.Object.Destroy(this.gameObject.GetComponent<AIHub2>());
                    UnityEngine.Object.Destroy(this.gameObject.GetComponent<interactionScript>());
                }



                if (true == false)
                {
                    interactionMate thisMate = new interactionMate();


                    thisMate.interactionAuthor = theAuthorScript.theAuthor;
                    thisMate.enactThisInteraction = theAuthorScript.enactThisInteraction;
                    thisMate.target1 = this.gameObject;
                    thisMate.enactOn = this.gameObject;
                    //Debug.Log("my interaction type:  " + theAuthorScript.interactionType);

                    //interactionDictionary[theAuthorScript.interactionType].doInteraction(thisMate);
                }
            }
            if(true == false)
            {
                //foreach (string aaaaa in interactionDictionary.Keys)
                {
                    //if (aaaaa == theAuthorScript.interactionType)
                    {

                    }
                }
            }
        }

        //Debug.Log("ZZZZZZZ     END onTriggerEnter for:  " + this.gameObject.name + "     ZZZZZZZ");

    }



    public void clonify(interactionScript blankScriptToFill)
    {
        foreach (string thisKey in dictOfInteractions.Keys)
        {
            blankScriptToFill.dictOfInteractions[thisKey] = dictOfInteractions[thisKey];
        }


    }

}
