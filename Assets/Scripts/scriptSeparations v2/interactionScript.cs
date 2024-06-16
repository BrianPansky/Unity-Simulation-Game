using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class interactionScript : MonoBehaviour
{


    public Dictionary<enactionCreator.interType, List<effect>> dictOfInteractions = new Dictionary<enactionCreator.interType, List<effect>>();


    int cooldown = 0;


    public enum effect
    {
        damage,
        burn,
        useVehicle,
        activate1
    }





    void Start()
    {

        //              theWorldScript.theTagScript.foreignAddTag("interactable", this.gameObject);

    }

    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("YYYYYY     START onTriggerEnter for:  " + this.gameObject.name + "     YYYYYY");


        foreach (var key in dictOfInteractions.Keys)
        {
            //Debug.Log(key);
        }

        if (other.tag != "interactionType1")
        {

            return;
        }


        authorScript1 theAuthorScript = other.gameObject.GetComponent<authorScript1>();

        if (theAuthorScript.enacting.enactionAuthor == null) { return; }

        if (dictOfInteractions.ContainsKey(theAuthorScript.enacting.interactionType) != true)
        {

            return;
        }



        foreach (effect thisEffect in dictOfInteractions[theAuthorScript.enacting.interactionType])
        {

            if (thisEffect == effect.useVehicle)
            {
                //Debug.Log("thisEffect == effect.useVehicle");
                //Debug.Log("thisEffect == effect.useVehicle");
                //Debug.Log("theAuthorScript.enacting.enactionAuthor.name:  " + theAuthorScript.enacting.enactionAuthor.name);
                //Debug.Log("theAuthorScript.enacting.enactionAuthor.transform.position:  " + theAuthorScript.enacting.enactionAuthor.transform.position);

                CharacterController controller = theAuthorScript.enacting.enactionAuthor.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false;
                }
                
                theAuthorScript.enacting.enactionAuthor.transform.position = this.transform.position;
                theAuthorScript.enacting.enactionAuthor.transform.rotation = this.transform.rotation;

                //body1 theBodyScript = theAuthorScript.theAuthor.GetComponent<body1>();

                //enactionScript theEnactionScript = theAuthorScript.theAuthor.GetComponent<enactionScript>();
                //ad hoc for now
                virtualGamepad gamepad = theAuthorScript.enacting.enactionAuthor.GetComponent<virtualGamepad>();
                tank2 theTank = this.GetComponent<tank2>();
                theTank.equip(gamepad);
                //theEnactionScript.thisNavMeshAgent = theTank.thisNavMeshAgent;
                //      theTank.pilot = theAuthorScript.enacting.enactionAuthor;
                //      theTank.thePilotEnactionScript = theEnactionScript;

                //this is probably great, but it also disables my camera.  will need to re-arrange things...
                //      theBodyScript.theBodyGameObject.active = false;

                //this.gameObject.transform.SetParent(theAuthorScript.theAuthor.transform, true);
                theAuthorScript.enacting.enactionAuthor.transform.SetParent(this.gameObject.transform, true);
                //theEnactionScript.enactionBody = this.gameObject;
                //theEnactionScript.currentlyUsable.Remove("humanBody");
                //theEnactionScript.currentlyUsable.Add("tank");


            }

            /*
            if (thisEffect == effect.damage)
            {

                body1 thisBody = this.gameObject.GetComponent<body1>();
                thisBody.currentHealth -= theAuthorScript.magnitudeOfInteraction;


                this.gameObject.GetComponent<Renderer>().material.color = new Color(((thisBody.currentHealth + 10) / (thisBody.maxHealth + 10)), 0f, 0f);

                if (thisBody.currentHealth < 0 && this.gameObject.name != "Player 1")
                {
                    thisBody.killThisBody();
                }


            }
            else if (thisEffect == effect.burn)
            {
                GameObject makeThis = repository2.singleton.interactionSphere;


                GameObject thisObject = Instantiate(repository2.singleton.interactionSphere, this.transform.position, Quaternion.identity);
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
            else if (thisEffect == effect.useVehicle)
            {
                theAuthorScript.theAuthor.transform.position = this.transform.position;
                theAuthorScript.theAuthor.transform.rotation = this.transform.rotation;

                //body1 theBodyScript = theAuthorScript.theAuthor.GetComponent<body1>();


                enactionScript theEnactionScript = theAuthorScript.theAuthor.GetComponent<enactionScript>();
                //ad hoc for now
                tank1 theTank = this.GetComponent<tank1>();
                theEnactionScript.thisNavMeshAgent = theTank.thisNavMeshAgent;
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
            else if (thisEffect == effect.activate1)
            {
                moveByForce motorScript = this.gameObject.GetComponent<moveByForce>();
                motorScript.turnedOn = true;
            }
            */
        }



        //Debug.Log("ZZZZZZZ     END onTriggerEnter for:  " + this.gameObject.name + "     ZZZZZZZ");

    }


    internal void addInteraction(enactionCreator.interType interactionType, effect effect)
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
            dictOfInteractions.Add(interactionType, makeEffectIntoList(effect));
        }
    }

    public List<effect> makeEffectIntoList(effect e1)
    {
        List<effect> newList = new List<effect>();
        newList.Add(e1);
        return newList;
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
