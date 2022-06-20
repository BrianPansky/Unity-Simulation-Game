using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class social : MonoBehaviour
{
    //keep track of who is trusted or not
    //probably also personality, psychology
    //maybe also group memberships


    //Matching and Alignment
    //for now just left-right spectrum, as a +- 100 I guess:
    public int politicalSide;

    //how to keep track of relationships?
    //dictionary?  tags?
    //I guess dictionary with person's name, and trust % inside:
    public Dictionary<string, int> trustDict = new Dictionary<string, int>();
    //default trust, out of 100:
    public int defaultTrust;



    public functionsForAI theFunctions;
    public AI1 theHub;




    //social hierarchy stuff, ad-hoc list of factions, for now...
    public List<string> factionList = new List<string>();
    public List<string> allyFactionList = new List<string>();
    public List<string> enemyFactionList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

        theFunctions = GetComponent<functionsForAI>();
        theHub = GetComponent<AI1>();

        //initialize numbers:
        politicalSide = -50;
        defaultTrust = 50;
    }


    /////////////////    TRUST Functions (for FOREIGN calls)    /////////////////

    public void trustBySide(string theirName, int theirSide)
    {
        //input a number representing the political "side" of the other person's memetic interaction
        //compare to this person's "side"
        //update trust accordingly in the relationship dictionary

        

        //check if they match:
        if (theirSide == politicalSide)
        {
            Debug.Log("good!  Same political side!");

            setRelationTrust(theirName, 70);
            //defaultTrust = 70;
        }
        else
        {
            Debug.Log("not the same political side...");

            setRelationTrust(theirName, 20);
            //defaultTrust = 20;
        }
    }

    public int checkTrust(string theirName)
    {
        //tells you how much this person trusts you

        //make sure dicitonary is initialized:
        dictionaryInitialize(theirName);

        return trustDict[theirName];
    }

    public void toldPlan(List<action> thePlan)
    {
        List<List<action>> planListFormat = new List<List<action>>();
        planListFormat.Add(thePlan);

        planListFormat = theFunctions.simulatingPlansToEnsurePrereqs(planListFormat, theHub.knownActions, theHub.state, 20);

        theHub.planList = planListFormat;
    }


    /////////////////    TRUST Functions (for LOCAL calls)    /////////////////

    public void setRelationTrust(string nameOfOther, int trustAmount)
    {
        //input the name of the other person
        //input the amount of trust to set it to (from 0 - 100)

        //first have to check if they are in the dictionary yet
        dictionaryInitialize(nameOfOther);

        //now, update dictionary:
        trustDict[nameOfOther] = trustAmount;

    }

    public void dictionaryInitialize(string nameOfOther)
    {
        //checks the relationship dicitonary for the named person
        //if they are in there already, do nothing
        //otherwise, assign the default trust value

        //so, I'm just adding names, not GameObjects?

        if (!trustDict.ContainsKey(nameOfOther))
        {
            trustDict[nameOfOther] = defaultTrust;
        }
    }






    /////////////////    HIERARCHY Functions    /////////////////

    public void addFactionToList(GameObject leader)
    {
        //takes a leader game object, adds their faction tag to the list of faction tags
        //should check if it's already on the list???  is that easier with dictionaries???
        //so maybe i should use a dictionary???
        factionList.Add(theFunctions.gangTag(leader));
        

    }

}
