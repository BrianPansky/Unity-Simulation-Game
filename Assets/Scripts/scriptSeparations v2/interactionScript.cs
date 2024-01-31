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
            Debug.Log("2222222222222the interaction type is:  " + theAuthorScript.interactionType);
            if (dictOfInteractions.ContainsKey(theAuthorScript.interactionType))
            {
                string theDictEntry = dictOfInteractions[theAuthorScript.interactionType];
                
                //Debug.Log("the corrosponding interaction dictionary entry is:  " + theDictEntry);

                if (theDictEntry == "clickLock")
                {
                    if (theAuthorScript.theAuthor.GetComponent<inventory1>().testInventory1.Contains("testKey1") == true)
                    {
                        this.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 0f, 1f);

                        enactionScript theEnactionScript = theAuthorScript.theAuthor.GetComponent<enactionScript>();
                        theEnactionScript.availableEnactions.Add("shoot1");

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

                    theAuthorScript.theAuthor.GetComponent<inventory1>().testInventory1.Add("testKey1");
                }

                if (theDictEntry == "die")
                {
                    Debug.Log("this SHOULD destroy the AI...................................................................................................................................................");
                    //Debug.Log(UnityEngine);
                    //Debug.Log(UnityEngine.Object);
                    UnityEngine.Object.Destroy(this.gameObject.GetComponent<AIHub2>());
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



}
