using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class interactionScript : MonoBehaviour
{

    //public Dictionary<string, testInteraction> interactionDictionary = new Dictionary<string, testInteraction>();
    //public List<string> listOfInteractions = new List<string>();
    public Dictionary<string, List<string>> dictOfInteractions = new Dictionary<string, List<string>>();


    int cooldown = 0;



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

            if (theAuthorScript.theAuthor == null) { return; }

            if (dictOfInteractions.ContainsKey(theAuthorScript.interactionType))
            {


                foreach (string thisEffect in dictOfInteractions[theAuthorScript.interactionType])
                {
                    if (thisEffect == "clickLock")
                    {
                        if (theAuthorScript.theAuthor.GetComponent<inventory1>().testInventory1.Contains("testKey1") == true)
                        {
                            this.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 0f, 1f);
                            Vector3 p1 = this.gameObject.transform.position;
                            Vector3 p2 = new Vector3(p1.x, p1.y + 22, p1.z);
                            //Debug.DrawLine(p1, p2, new Color(1f, 0f, 1f), 9999f);

                            //Debug.DrawLine(this.gameObject.transform.position, enactionTarget.transform.position, Color.blue, 0.9f);


                            enactionScript theEnactionScript = theAuthorScript.theAuthor.GetComponent<enactionScript>();
                            //              theEnactionScript.availableEnactions.Add("shoot1");

                            //theAuthorScript.theAuthor.transform.localScale = new Vector3(1f, 22, 1f);

                        }
                    }
                    else if (thisEffect == "grabKey")
                    {
                        this.gameObject.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f);
                        Vector3 p1 = this.gameObject.transform.position;
                        Vector3 p2 = new Vector3(p1.x, p1.y + 22, p1.z);
                        //Debug.DrawLine(p1, p2, new Color(0f, 1f, 0f), 9999f);

                        theAuthorScript.theAuthor.GetComponent<inventory1>().testInventory1.Add("testKey1");

                    }
                    else if (thisEffect == "damage")
                    {

                        body1 thisBody = this.gameObject.GetComponent<body1>();
                        thisBody.currentHealth -= theAuthorScript.magnitudeOfInteraction;


                        this.gameObject.GetComponent<Renderer>().material.color = new Color(((thisBody.currentHealth + 10) / (thisBody.maxHealth + 10)), 0f, 0f);

                        if (thisBody.currentHealth < 0 && this.gameObject.name != "Player 1")
                        {
                            thisBody.killThisBody();
                        }


                    }
                    else if (thisEffect == "burn")
                    {
                        GameObject makeThis = theWorldScript.theRepository.interactionSphere;


                        GameObject thisObject = theWorldScript.theRepository.createPrefabAtPointAndRETURN(makeThis, this.transform.position);
                        //UnityEngine.Object.Destroy(thisObject.GetComponent<selfDestructScript1>());
                        thisObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                        //thisObject.transform.position += lookingRay.direction;
                        //theInteractionMate.interactionAuthor.transform.position + new Vector3(0, 0, 0)
                        projectile1 projectileScript = thisObject.AddComponent<projectile1>();
                        projectileScript.Direction = Vector3.up;
                        projectileScript.selfDestructOnCollision = false;
                        selfDestructScript1 killScript = thisObject.GetComponent<selfDestructScript1>();
                        killScript.timeUntilSelfDestruct = 30;

                        growScript1 growScript = thisObject.AddComponent<growScript1>();
                        growScript.growthSpeed = 0.3f;


                    }
                    else if (thisEffect == "useVehicle")
                    {
                        theAuthorScript.theAuthor.transform.position = this.transform.position;
                        theAuthorScript.theAuthor.transform.rotation = this.transform.rotation;

                        body1 theBodyScript = theAuthorScript.theAuthor.GetComponent<body1>();


                        enactionScript theEnactionScript = theAuthorScript.theAuthor.GetComponent<enactionScript>();
                        //ad hoc for now
                        tank1 theTank = this.GetComponent<tank1>();
                        theTank.pilot = theAuthorScript.theAuthor;
                        theTank.thePilotEnactionScript = theEnactionScript;

                        //this is probably great, but it also disables my camera.  will need to re-arrange things...
                        //      theBodyScript.theBodyGameObject.active = false;

                        //this.gameObject.transform.SetParent(theAuthorScript.theAuthor.transform, true);
                        theAuthorScript.theAuthor.transform.SetParent(this.gameObject.transform, true);

                        theEnactionScript.enactionBody = this.gameObject;
                        theEnactionScript.currentlyUsable.Remove("humanBody");
                        theEnactionScript.currentlyUsable.Add("tank");



                    }
                    else if(thisEffect == "activateMotor")
                    {
                        moveByForce motorScript = this.gameObject.GetComponent<moveByForce>();
                        motorScript.turnedOn = true;
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

    internal void addInteraction(string interactionType, string effect)
    {
        //dictOfInteractions

        if (dictOfInteractions.ContainsKey(interactionType))
        {
            //add the game object to the list of objects tagged with that tag:
            dictOfInteractions[interactionType].Add(effect);
        }
        else
        {
            //sigh, need to add the key first, which means the list it unlocks as well...
            dictOfInteractions.Add(interactionType, makeStringsIntoList(effect));
        }
    }


    public List<string> makeStringsIntoList(string s1, string s2 = null, string s3 = null, string s4 = null)
    {
        //input 4 strings
        //get backa  list of all of them that are NOT null

        List<string> allStrings = new List<string>();
        allStrings.Add(s1);
        allStrings.Add(s2);
        allStrings.Add(s3);
        allStrings.Add(s4);

        List<string> nonNullStrings = new List<string>();

        foreach (string thisString in allStrings)
        {
            if (thisString != null)
            {
                nonNullStrings.Add(thisString);
            }
        }

        return nonNullStrings;
    }
}
