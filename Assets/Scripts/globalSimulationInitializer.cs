using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalSimulationInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        testObjectGeneration();


        initializeFactions();


    }
    

    public void testObjectGeneration()
    {
        
        //Debug.DrawRay(thisAI.threatObject.GetComponent<Transform>().position, threatLine, Color.white, 3);

        GameObject X = Resources.Load("resource1") as GameObject;

        Vector3 Y = new Vector3();
        Y.Set(0, 20, -55);

        generateXatY(X, Y);
    }


    public void initializeFactions()
    {

        //first get faction list
        List<string> factionList = new List<string>();
        List<GameObject> leaderObjList = new List<GameObject>();

        //ad-hoc:
        leaderObjList.Add(GameObject.Find("Player"));
        leaderObjList.Add(GameObject.Find("NPC pickpocket"));
        leaderObjList.Add(GameObject.Find("NPC shopkeeper"));
        leaderObjList.Add(GameObject.Find("NPC shopkeeper (1)"));
        
        //  ad-hoc check when i don't have all those NPCs
        foreach(GameObject obj in leaderObjList) 
        { 
            if(obj == null)
            {
                return;
            }
        }

        //make them their OWN leaders:
        foreach (GameObject thisLeader in leaderObjList)
        {
            AI1 thisAI = thisLeader.GetComponent<AI1>();

            thisAI.leader = thisLeader;
        }

        //tag them:
        foreach (GameObject thisLeader in leaderObjList)
        {
            //should use "auto generate gang tag" thing here:
            factionList.Add(thisLeader.name + "sGang");

            //add stuff to their tags
            taggedWith taggedWith = thisLeader.GetComponent<taggedWith>();
            taggedWith.addTag("leader");
            taggedWith.addTag(thisLeader.name + "sGang");
        }

        //now give it to leaders?  so, need to get their social scripts
        foreach (GameObject thisLeader in leaderObjList)
        {
            social theSocialScript = thisLeader.GetComponent<social>();

            //now...add list of ENEMIES?  and also full faction list too?????
            foreach (string gangTag in factionList)
            {
                //add this gang to their list of all gangs, seems redundant but whatev:
                theSocialScript.factionList.Add(gangTag);

                //for now, add ALL gangs to list of enemies
                //except their OWN faction, so check if it's their own factions' tag:
                //should use "auto generate gang tag" thing here:
                if (gangTag != (thisLeader.name + "sGang"))
                {
                    theSocialScript.enemyFactionList.Add(gangTag);
                }
                
            }
        }

    }


    public void generateXatY(GameObject X, Vector3 Y)
    {
        if (X != null)
        {
            Instantiate(X, Y, Quaternion.identity);
        }
    }
}
