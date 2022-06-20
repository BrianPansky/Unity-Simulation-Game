using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalSimulationInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        initializeFactions();


    }
    


    public void initializeFactions()
    {

        //first get faction list
        List<string> factionList = new List<string>();
        List<GameObject> leaderObjList = new List<GameObject>();

        //ad-hoc:
        leaderObjList.Add(GameObject.Find("Player"));
        leaderObjList.Add(GameObject.Find("NPC pickpocket"));

        foreach (GameObject leader in leaderObjList)
        {
            //should use "auto generate gang tag" thing here:
            factionList.Add(leader.name + "sGang");

            //add stuff to their tags
            taggedWith taggedWith = leader.GetComponent<taggedWith>();
            taggedWith.addTag("leader");
            taggedWith.addTag(leader.name + "sGang");
        }

        //now give it to leaders?  so, need to get their social scripts
        foreach (GameObject leader in leaderObjList)
        {
            social theSocialScript = leader.GetComponent<social>();

            //now...add list of ENEMIES?  and also full faction list too?????
            foreach (string gangTag in factionList)
            {
                //add this gang to their list of all gangs, seems redundant but whatev:
                theSocialScript.factionList.Add(gangTag);

                //for now, add ALL gangs to list of enemies
                //except their OWN faction, so check if it's their own factions' tag:
                //should use "auto generate gang tag" thing here:
                if (gangTag != (leader.name + "sGang"))
                {
                    theSocialScript.enemyFactionList.Add(gangTag);
                }
                
            }
        }

    }
}
