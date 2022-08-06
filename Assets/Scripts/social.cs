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


    //other scripts
    public functionsForAI theFunctions;
    public AI1 theHub;
    public AI1 thisAI;
    public premadeStuffForAI premadeStuff;
    public taggedWith theTagScript;




    //social hierarchy stuff, ad-hoc list of factions, for now...
    public List<string> factionList = new List<string>();
    public List<string> allyFactionList = new List<string>();
    public List<string> enemyFactionList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //other scripts:
        theFunctions = GetComponent<functionsForAI>();
        theHub = GetComponent<AI1>();
        thisAI = GetComponent<AI1>();
        premadeStuff = GetComponent<premadeStuffForAI>();
        theTagScript = GetComponent<taggedWith>();

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
        factionList.Add(gangTag(leader));
        

    }








    /////////////////    JOB Functions    /////////////////
    public void changeRoles(GameObject agent, action roleToAdd, action roleToRemove)
    {
        //maybe in future the inputs can be some role class object
        //for now, it's just a quick way to add one action and remove another

        addKnownActionToGameObject(agent, roleToAdd);
        removeKnownActionFromGameObject(agent, roleToRemove);

    }

    public bool hiring(GameObject whoToHire, job theJob, string jobLocationTypeTag)
    {
        //for now, ad-hoc enter "jobLocationType" string.  used to find location using tags.  later, pull that info from the boss automatically somehow....
        //Has to return bool to show if it worked or no.  clunky, but oh well?

        //ad-hoc way to hire more than one employee for now:
        //if (listOfCashiers.Contains(customer) == false)
        AI1 targetAI = whoToHire.GetComponent("AI1") as AI1;
        if (targetAI.jobSeeking == true)
        {
            //print("no problem");

            //listOfCashiers.Add(customer);
            //changeRoles(whotoHire, premadeStuff.workAsCashier, premadeStuff.doTheWork);

            //print(customer.name);


            //workerCount += 1;

            //print(workerCount);

            //need the worker to show up at the correct store for their shift:
            //customerAI.roleLocation = thisAI.roleLocation;
            string ownershipTag = "owned by " + this.name;
            //need cashierZone of the owned store:
            GameObject roleLocation = theTagScript.randomTaggedWithMultiple(jobLocationTypeTag, ownershipTag);

            if (roleLocation == null)
            {
                print("cannot find a role location, probably trying to hire someone before you've made a business, or my system is unfinished");

                return false;
            }
            else
            {
                doSuccsessfulHiring(targetAI, theJob, roleLocation);
            }





            return true;
        }
        else
        {
            //print("not job seeking");
            return false;
        }
    }

    public void doSuccsessfulHiring(AI1 targetAI, job theJob, GameObject roleLocation)
    {
        //print(roleLocation);
        targetAI.jobSeeking = false;
        targetAI.leader = this.gameObject;

        //record in factionState:
        //mmm, need it to INCREMENT...
        //NEEDS TO BE DIFFERENT DEPENDING ON WHICH JOB IS BEING HIRED FOR
        //KINDA SEEMS LIKE IT WOULD BE NICE TO HAVE A SIMPLE DICTIONARY WITH TEXT KEYS THAT ARE JOB NAMES, AND NUMBERS FOR QUANTITY, RIGHT???
        theFunctions.incrementItem(thisAI.factionState["unitState"], premadeStuff.employee, 1);
        //thisAI.factionState["unitState"].Add(deepStateItemCopier(premadeStuff.employee));

        //need to add gang tag!
        targetAI.taggedWith.addTag(gangTag(this.gameObject));

        //color:
        changeToFactionColor(targetAI.gameObject, this.gameObject);

        //add tag of job name
        targetAI.taggedWith.addTag(theJob.name);

        //Increase the "clearance level" of the worker:
        //BIT ad-hoc.  Characters might have different clearance levels for different places/factions etc.  Right now I just have one.
        targetAI.clearanceLevel = 1;

        //now...to finish and deliver "theJob" class object...
        targetAI.currentJob = premadeStuff.jobFinisher(theJob, this.gameObject, roleLocation);

        //but still have to add the known actions to their known actions!  sigh.
        foreach (action x in theJob.theKnownActions)
        {
            targetAI.knownActions.Add(premadeStuff.deepActionCopier(x));
            //addKnownActionToGameObject(whoToHire, x);
        }

        //updateFactionState?
    }

    public void commandToDoFetchXAction(action theBringLeaderXAction, GameObject whoToCommand)
    {
        //commandList.Add(premadeStuff.bringLeaderX(premadeStuff.deepStateItemCopier(premadeStuff.food)));

        //need their AI1 script:
        AI1 NPChubScript = whoToCommand.GetComponent("AI1") as AI1;

        //going to blank out their to-do list, and fill it with test "orders":
        //AD HOC, SHOULD NOT DO THIS?!?!?
        NPChubScript.toDoList.Clear();


        NPChubScript.inputtedToDoList.Add(theBringLeaderXAction);
    }

    public void commandToDoXAction(action theXAction, GameObject whoToCommand)
    {
        //commandList.Add(premadeStuff.bringLeaderX(premadeStuff.deepStateItemCopier(premadeStuff.food)));

        //need their AI1 script:
        AI1 NPChubScript = whoToCommand.GetComponent("AI1") as AI1;



        //print(thisAI.gameObject.name);
        //print(whoToCommand.name);
        //printPlan(thisAI.toDoList);
        //printPlan(whoToCommand.toDoList);
        if (thisAI.currentJob != null)
        {
            //print(thisAI.currentJob.name);
        }
        //if (whoToCommand.currentJob != null)
        {
            //print(whoToCommand.currentJob.name);
        }




        //going to blank out their to-do list, and fill it with test "orders":
        //AD HOC, SHOULD NOT DO THIS?!?!?
        NPChubScript.toDoList.Clear();
        //or this one.  hopefully this stops the orders from piling up infinitely...for now.
        //AD-HOC.  in future, shoudl be able to queue up orders!!!
        NPChubScript.inputtedToDoList.Clear();


        //print(thisAI.gameObject.name);
        //print(whoToCommand.name);
        //printPlan(thisAI.toDoList);
        //printPlan(whoToCommand.toDoList);
        if (thisAI.currentJob != null)
        {
            //print(thisAI.currentJob.name);
        }
        //if (whoToCommand.currentJob != null)
        {
            //print(whoToCommand.currentJob.name);
        }


        NPChubScript.inputtedToDoList.Add(theXAction);
    }

    public void commandGROUPtoDoXAction(action theXAction, List<GameObject> whoToCommand)
    {
        foreach (GameObject person in whoToCommand)
        {
            commandToDoXAction(theXAction, person);

            //FOR INVESTIGATING/TESTING:
            //AI1 targetAI = person.GetComponent("AI1") as AI1;
            //targetAI.masterPrintControl = true;
            //targetAI.npcx = targetAI.gameObject.name;
            //Debug.Log("updated ''npcx''");
        }
    }


    public void succeedAtRecruitment(GameObject whoIsRecruited)
    {
        //ok, recruitment suceeds
        Debug.Log("recruitment successful");
        Debug.Log("recruited by:  " + this.gameObject.name);
        Debug.Log("gang tag will be:  " + gangTag(this.gameObject));

        taggedWith foreignTagScript = whoIsRecruited.GetComponent<taggedWith>();
        foreignTagScript.foreignAddTag(gangTag(this.gameObject), whoIsRecruited);
        changeToFactionColor(whoIsRecruited, this.gameObject);

        //Debug.Log("should be recruited to gang now, by tagging");

        //but they need to be able to FIND me, their leader, to deliver money to me
        //so, for now, fill their leader variable:
        //need their AI1 script:
        AI1 NPChubScript = whoIsRecruited.GetComponent("AI1") as AI1;
        NPChubScript.leader = this.gameObject;

        //ALSO NEED TO BLANK-OUT THEIR TARGET!!!
        NPChubScript.target = null;
    }

    public void changeToFactionColor(GameObject toColor, GameObject leader)
    {
        if (leader.name == "Player")
        {
            Debug.Log("Player color");
            toColor.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
        else if (leader.name == "NPC pickpocket")
        {
            Debug.Log("NPC pickpocket color");
            toColor.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }
        else
        {
            Debug.Log("''else'' color");
            toColor.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
        }
    }


    //change knownActions and such:
    public void addKnownActionToGameObject(GameObject agent, action theAction)
    {
        //first, go from "GameObject" to it's script that has knownActions:
        AI1 hubScript = theFunctions.getHubScriptFromGameObject(agent);

        //now add the knownAction:
        hubScript.knownActions.Add(premadeStuff.deepActionCopier(theAction));

    }

    public void removeKnownActionFromGameObject(GameObject agent, action theAction)
    {
        //first, go from "GameObject" to it's script that has knownActions:
        AI1 hubScript = theFunctions.getHubScriptFromGameObject(agent);
        
        //now add the knownAction:
        hubScript.knownActions.RemoveAll(y => y.name == theAction.name);
    }


    //misc tagging stuff
    public string gangTag(GameObject leader)
    {
        string theGangTag = leader.name + "sGang";
        return theGangTag;
    }




}
