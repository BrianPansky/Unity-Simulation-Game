using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.GraphicsBuffer;

public class worldScript : MonoBehaviour
{
    Recorder behaviourUpdateRecorder;
    int oldElapsedNanoseconds = 0;


    public GameObject thePlayer;
    public Dictionary<string, List<GameObject>> taggedStuff = new Dictionary<string, List<GameObject>>();
    public Dictionary<string, List<testInteraction>> interactionLegibstration = new Dictionary<string, List<testInteraction>>();
    //public List<GameObject> interactionLegibstration = new List<GameObject>();
    public int theTime;

    public List<GameObject> listOfMapZoneObjects = new List<GameObject>();
    public List<mapZoneScript> listOfMapZoneScripts = new List<mapZoneScript>();
    public List<mapZoneScript> listOfNEARMapZoneScripts = new List<mapZoneScript>();
    public List<mapZoneScript> listOfFARMapZoneScripts = new List<mapZoneScript>();
    public List<List<AIHub2>> listOfNEARAIHub2ScriptsPerZone = new List<List<AIHub2>>();
    public List<List<AIHub2>> listOfFARAIHub2ScriptsPerZone = new List<List<AIHub2>>();
    public int numberOfMapZones;
    int numberOfNEARMapZones = 9;  //INPUT this one [could do it in editor, but i'll do it here for now]
    public int numberOfFARMapZones;
    public bool nearORfar = true;
    public int currentMapZone = 1;
    public int currentNEARMapZone = 1;
    int framesPerZone = 1; //   for some reason this variable keeps getting overwritten and breaking things.  so i removed the "public" status of it.
    public int currentZoneFrame = 0;
    int callableUpdatesPerZoneFrame = 11; // aaand this one too.  this variable keeps getting overwritten and breaking things.  so i removed the "public" status of it.
    public int callableUpdateCounter = 0;



    //anothe rsplit idea:  have number of possible buckets to divide all far ones into
    //  at start, go through each zone, add to a bucket in turn, perhaps
    //then for actually updating, have this list OF LISTS of zones.  and every frame, update one zone FROM EACH list
    public List<List<mapZoneScript>> listOfListsOfFARMapZoneScripts = new List<List<mapZoneScript>>();
    int numberOfOfFarZoneBuckets = 1;   //so, currently, it should be set to only ONE. that should behave the same as my current setup





    public int effingTimer = 0;

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
        //numberOfNEARMapZones = 9;
        theTime = 0;
        //I'M JUST PLUGGING THIS IN [in the editor, right hand side] INSTEAD   theTagScript = this.gameObject.GetComponent<taggedWith>();
        //Debug.Log("00000000000000000000000000numberOfNEARMapZones:  " + numberOfNEARMapZones);
    }



    // Start is called before the first frame update
    void Start()
    {
        behaviourUpdateRecorder = Recorder.Get("BehaviourUpdate");
        behaviourUpdateRecorder.enabled = true;




        //Debug.Log("111111111111111111111111111111111numberOfNEARMapZones:  " + numberOfNEARMapZones);
        theTagScript.addTag("worldObject");

        numberOfMapZones = listOfMapZoneScripts.Count;
        refreshNearAndFarLists();

    }

    // Update is called once per frame
    void Update()
    {
        //if (behaviourUpdateRecorder.isValid)
        {
            //printDeltaTime();
        }
        //else
        {
            //Debug.Log("behaviourUpdateRecorder.isValid is FALSE!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + behaviourUpdateRecorder.isValid);
        }
            

        //wtf is the difference between "currentZoneFrame" and "currentMapZone"?????????????
        //ohhhh ya.  for the current map zone, it might get to be updated for multiple frames.
        //Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzlistOfNEARAIHub2ScriptsPerZone.Count:  " + listOfNEARAIHub2ScriptsPerZone.Count);
        //Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzlistOfFARAIHub2ScriptsPerZone.Count:  " + listOfFARAIHub2ScriptsPerZone.Count);

        
        timeIncrement();

        //printDeltaTime();
        //Debug.Log("333333333333333333333333333numberOfNEARMapZones:  " + numberOfNEARMapZones);
        if (effingTimer == 0)
        {
            
            effingTimer = 70;
            
        }
        effingTimer--;

        //Debug.Log("currentMapZone:  " + currentMapZone);
        //Debug.Log("numberOfFARMapZones:  " + numberOfFARMapZones);
        //printDeltaTime();


        


        //      we should be looking at [current?] FAR map zones.
        //Debug.Log("mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm");
        //Debug.Log("currentMapZone" + currentMapZone);
        //Debug.Log("numberOfFARMapZones" + numberOfFARMapZones);
        if (currentMapZone > numberOfFARMapZones - 1)
        {
            currentMapZone = 1;
            //numberOfMapZones = listOfMapZoneScripts.Count;
            refreshNearAndFarLists();
        }
        else
        {
            if (currentZoneFrame == framesPerZone)
            {
                currentMapZone++;
                currentZoneFrame = 1;

            }
            else
            {
                currentZoneFrame++;
            }
        }
        //printDeltaTime();

        //Debug.Log("framesPerZone:  " + framesPerZone);
        //Debug.Log("currentZoneFrame:  " + currentZoneFrame);

        updateNearAndFarEveryFrame();
        //printDeltaTime();
        //updateNearOneframeTheFarNextFrameEtc();

        //updateNearAndFarEveryFrameByZoneTurn();


        
    }

    public void updateNearAndFarEveryFrameByZoneTurn()
    {

        if (currentZoneFrame == framesPerZone)
        {
            currentMapZone++;
            currentZoneFrame = 1;

        }
        else
        {
            currentZoneFrame++;
        }

        //printDeltaTime();
        //Debug.Log("listOfNEARMapZoneScripts.Count:  " + listOfNEARMapZoneScripts.Count);
        //Debug.Log("listOfFARMapZoneScripts.Count:  " + listOfFARMapZoneScripts.Count);
        //Debug.Log("currentMapZone:  " + currentMapZone);
        //Debug.Log("(currentMapZone - 1):  " + (currentMapZone - 1));
        //updateOneZone(listOfFARMapZoneScripts[currentMapZone - 1], callableUpdatesPerZoneFrame);
        //Debug.Log("listOfFARAIHub2ScriptsPerZone.Count):  " + listOfFARAIHub2ScriptsPerZone.Count);

        updateOneZoneTurn(listOfFARMapZoneScripts[currentMapZone - 1], callableUpdatesPerZoneFrame);

        //printDeltaTime();

        //Debug.Log("DO NEAR!!!!!!!!!!!!!" + listOfNEARMapZoneScripts.Count);

        if (currentNEARMapZone > listOfNEARMapZoneScripts.Count - 1)
        {
            currentNEARMapZone = 1;
        }

        //updateOneZone(listOfNEARMapZoneScripts[currentNEARMapZone - 1], callableUpdatesPerZoneFrame);
        //updateOneZoneFromListOfAIHub2s(listOfNEARAIHub2ScriptsPerZone[currentMapZone - 1], callableUpdatesPerZoneFrame);
        updateOneZoneTurn(listOfNEARMapZoneScripts[currentNEARMapZone - 1], callableUpdatesPerZoneFrame);
        currentNEARMapZone++;

        //printDeltaTime();

    }

    public void updateOneZoneTurn(mapZoneScript theMapZone, int howManyUpdatesPerEntity)
    {

    }

    public void printDeltaTime()
    {
        //Debug.Log("oldElapsedNanoseconds: " + oldElapsedNanoseconds);
        //Debug.Log("behaviourUpdateRecorder.elapsedNanoseconds: " + behaviourUpdateRecorder.elapsedNanoseconds);

        //Debug.Log("BehaviourUpdate time since previous printout (behaviourUpdateRecorder.elapsedNanoseconds - oldElapsedNanoseconds): " + (behaviourUpdateRecorder.elapsedNanoseconds - oldElapsedNanoseconds));
        //Debug.Log("((int)behaviourUpdateRecorder.elapsedNanoseconds): " + ((int)behaviourUpdateRecorder.elapsedNanoseconds));

        //Debug.Log("BehaviourUpdate time since previous printout (((int)behaviourUpdateRecorder.elapsedNanoseconds) - oldElapsedNanoseconds)): " + (((int)behaviourUpdateRecorder.elapsedNanoseconds) - oldElapsedNanoseconds));
        //oldElapsedNanoseconds = ((int)behaviourUpdateRecorder.elapsedNanoseconds);


        //              Instruct FrameTimingManager to collect and cache information

        //FrameTimingManager.CaptureFrameTimings();





        // Read cached information about N last frames (10 in this example)

        // The returned value tells how many samples is actually returned

        //uint ret = FrameTimingManager.GetLatestTimings((uint)m_FrameTimings.Length, m_FrameTimings);

        //if (ret > 0)

        {

            // Your code logic here

        }

    }

    void printTheDamnNPCScripts(List<AIHub2> listOfNPCs)
    {
        Debug.Log("listOfNPCs.Count:  " + listOfNPCs.Count);
        int numberThatAreNotNull = 0;
        //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        //printDeltaTime();
        foreach (AIHub2 thisNPCHub in listOfNPCs)
        {
            //Debug.Log("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");
            //Debug.Log("is this null?:  " + thisNPCHub);
            if (thisNPCHub != null)
            {
                //Debug.Log("CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC");
                numberThatAreNotNull++;
            }
        }
        Debug.Log("numberThatAreNotNull:  " + numberThatAreNotNull);


    }

    public void updateNearAndFarEveryFrame()
    {
        //this also cycles through all of them.  only updates ONE near per frame, one far per frame



        //printDeltaTime();
        //Debug.Log("listOfNEARMapZoneScripts.Count:  " + listOfNEARMapZoneScripts.Count);
        //Debug.Log("listOfFARMapZoneScripts.Count:  " + listOfFARMapZoneScripts.Count);
        //Debug.Log("currentMapZone:  " + currentMapZone);
        //Debug.Log("(currentMapZone - 1):  " + (currentMapZone - 1));
        //updateOneZone(listOfFARMapZoneScripts[currentMapZone - 1], callableUpdatesPerZoneFrame);

        //Debug.Log("/////////////////////////////     START printouts for updating near and far every frame    ///////////////////////////////");
        //Debug.Log("listOfFARAIHub2ScriptsPerZone.Count:  " + listOfFARAIHub2ScriptsPerZone.Count);
        //Debug.Log("listOfFARAIHub2ScriptsPerZone[currentMapZone - 1].Count:  " + listOfFARAIHub2ScriptsPerZone[currentMapZone - 1].Count);
        //printTheDamnNPCScripts(listOfFARAIHub2ScriptsPerZone[currentMapZone - 1]);
        updateOneZoneFromListOfAIHub2s(listOfFARAIHub2ScriptsPerZone[currentMapZone - 1], callableUpdatesPerZoneFrame);

        //printDeltaTime();

        //Debug.Log("DO NEAR!!!!!!!!!!!!!" + listOfNEARMapZoneScripts.Count);

        
        //updateOneZone(listOfNEARMapZoneScripts[currentNEARMapZone - 1], callableUpdatesPerZoneFrame);
        //updateOneZoneFromListOfAIHub2s(listOfNEARAIHub2ScriptsPerZone[currentMapZone - 1], callableUpdatesPerZoneFrame);
        updateOneZoneFromListOfAIHub2s(listOfNEARAIHub2ScriptsPerZone[currentNEARMapZone - 1], 1);
        

        //printDeltaTime();
        //Debug.Log("-----------------------------     END printouts for updating near and far every frame    -------------------------------");
        //Debug.Log("-----------------------------     END printouts for updating near and far every frame    -------------------------------");
        //Debug.Log("currentNEARMapZone" + currentNEARMapZone);
        //Debug.Log("listOfNEARMapZoneScripts.Count" + listOfNEARMapZoneScripts.Count);
        if (currentNEARMapZone > listOfNEARMapZoneScripts.Count - 1)
        {
            currentNEARMapZone = 1;
        }
        else
        {
            currentNEARMapZone++;
        }

    }


    public void refreshNearAndFarLists()
    {
        numberOfMapZones = listOfMapZoneScripts.Count;
        numberOfFARMapZones = listOfFARMapZoneScripts.Count;

        List<List<GameObject>> splitZones = theTagScript.nearestXNumberOfYToZExceptYAndTheRemainder(numberOfNEARMapZones, listOfMapZoneObjects, thePlayer);

        listOfNEARMapZoneScripts = new List<mapZoneScript>();
        listOfFARMapZoneScripts = new List<mapZoneScript>();
        listOfNEARAIHub2ScriptsPerZone = new List<List<AIHub2>>();
        listOfFARAIHub2ScriptsPerZone = new List<List<AIHub2>>();
        //public List<AIHub2> listOfNEARAIHub2Scripts = new List<AIHub2>();
        //public List<AIHub2> listOfFARAIHub2Scripts = new List<AIHub2>();
        //AIHub2 theHub = thisObject.GetComponent<AIHub2>();
        //int nearIndex = 0;
        //int farIndex = 0;

        //public List<List<AIHub2>> listOfNEARAIHub2ScriptsPerZone = new List<List<AIHub2>>();
        //Debug.Log("refreshing lists, splitZones[0].Count:  " + splitZones[0].Count);
        foreach (GameObject thisObject in splitZones[0])
        {
            //listOfNEARMapZoneScripts.Add(thisObject.GetComponent<mapZoneScript>());
            //listOfNEARMapZoneScripts.Add(thisObject.GetComponent<mapZoneScript>());
            mapZoneScript theMapZoneScript = thisObject.GetComponent<mapZoneScript>();
            theMapZoneScript.howManyUpdatesPerEntity = callableUpdatesPerZoneFrame;
            listOfNEARMapZoneScripts.Add(theMapZoneScript);

            //Debug.Log("refreshing lists, theMapZoneScript.theList.Count:  " + theMapZoneScript.theList.Count);
            int numberOfAgents = 0;
            int numberOfNonAgents = 0;
            List < AIHub2 > newBatchOfNPCsPerZone = new List<AIHub2>();
            foreach (GameObject thisObject2 in theMapZoneScript.theList)
            {

                AIHub2 thisNPCHub = thisObject2.GetComponent<AIHub2>();
                if(thisObject2.name == "returnTestAgent1(Clone)" || thisObject2.name == "returnTestAgent1")
                {
                    numberOfAgents++;
                    //Debug.Log("is this null?:  " + thisNPCHub + "for this object:  " + thisObject2);
                    if (thisNPCHub != null)
                    {
                        //Debug.Log("no, it is not null");
                        newBatchOfNPCsPerZone.Add(thisNPCHub);
                    }
                    else
                    {
                        //Debug.Log("yes, it is null");
                    }
                }
                else
                {
                    numberOfNonAgents++;
                }
            }
            //Debug.Log("numberOfAgents:  " + numberOfAgents);
            //Debug.Log("numberOfNonAgents:  " + numberOfNonAgents);
            //printTheDamnNPCScripts(newBatchOfNPCsPerZone);
            listOfNEARAIHub2ScriptsPerZone.Add(newBatchOfNPCsPerZone);
            //nearIndex++;
        }
        foreach (GameObject thisObject in splitZones[1])
        {
            mapZoneScript theMapZoneScript = thisObject.GetComponent<mapZoneScript>();
            theMapZoneScript.howManyUpdatesPerEntity = callableUpdatesPerZoneFrame;
            listOfFARMapZoneScripts.Add(theMapZoneScript);

            List<AIHub2> newBatchOfNPCsPerZone = new List<AIHub2>();
            foreach (GameObject thisObject2 in theMapZoneScript.theList)
            {
                //Debug.Log("thisObject2:  " + thisObject2);

                AIHub2 thisNPCHub = thisObject2.GetComponent<AIHub2>();
                //listOfFARAIHub2ScriptsPerZone[farIndex].Add(theHub);
                //Debug.Log("is this null?:  " + thisNPCHub + "for this object:  " + thisObject2);
                if (thisNPCHub != null)
                {
                    newBatchOfNPCsPerZone.Add(thisNPCHub);
                }
                    
            }
            //printTheDamnNPCScripts(newBatchOfNPCsPerZone);
            listOfFARAIHub2ScriptsPerZone.Add(newBatchOfNPCsPerZone);
            //farIndex++;
        }
        //Debug.Log("22222222222222222222222numberOfNEARMapZones:  " + numberOfNEARMapZones);
    }

    public void updateNearOneframeTheFarNextFrameEtc()
    {

        //Debug.Log("which to do???????????????????????????? (true = near)" + nearORfar);
        if (nearORfar == true)
        {

            //Debug.Log("DO NEAR!!!!!!!!!!!!!" + listOfNEARMapZoneScripts.Count);

            if (currentNEARMapZone > listOfNEARMapZoneScripts.Count - 1)
            {
                currentNEARMapZone = 1;
            }

            //updateOneZone(listOfNEARMapZoneScripts[currentNEARMapZone - 1], callableUpdatesPerZoneFrame);
            updateOneZoneFromListOfAIHub2s(listOfNEARAIHub2ScriptsPerZone[currentNEARMapZone - 1], callableUpdatesPerZoneFrame);
            currentNEARMapZone++;
            nearORfar = false;
        }
        else
        {
            //Debug.Log("DO FAR!!!!!!!!!!!!!" + listOfFARMapZoneScripts.Count);
            if (currentZoneFrame == framesPerZone)
            {
                currentMapZone++;
                currentZoneFrame = 1;

            }
            else
            {
                currentZoneFrame++;
            }


            //Debug.Log(listOfNEARMapZoneScripts.Count);
            //Debug.Log(listOfFARMapZoneScripts.Count);
            Debug.Log(currentMapZone - 1);
            //updateOneZone(listOfFARMapZoneScripts[currentMapZone - 1], callableUpdatesPerZoneFrame);
            updateOneZoneFromListOfAIHub2s(listOfFARAIHub2ScriptsPerZone[currentMapZone - 1], callableUpdatesPerZoneFrame);
            nearORfar = true;
        }



    }


    public void updateOneZoneFromListOfAIHub2s(List<AIHub2> listOfNPCs, int howManyUpdatesPerEntity)
    {
        //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        //printDeltaTime();
        foreach (AIHub2 thisNPCHub in listOfNPCs)
        {
            //Debug.Log("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");
            if (thisNPCHub != null)
            {

                //Debug.Log("ccccccccccccccccccccccccccccccccccccccccccccccc");
                while (callableUpdateCounter < howManyUpdatesPerEntity)
                {
                    //Debug.Log("dddddddddddddddddddddddddddddddddddddddddddddddd");
                    //printDeltaTime();
                    callableUpdateCounter++;
                    thisNPCHub.callableUpdate();
                }
                callableUpdateCounter = 0;
            }
        }
    }



    public void oldUpdateV1()
    {

        numberOfMapZones = listOfMapZoneScripts.Count;
        timeIncrement();

        Debug.Log(currentMapZone);
        Debug.Log(numberOfMapZones);

        if (currentMapZone == numberOfMapZones)
        {
            currentMapZone = 1;
            //numberOfMapZones = listOfMapZoneScripts.Count;
        }
        else
        {
            foreach (GameObject thisObject in listOfMapZoneScripts[currentMapZone - 1].theList)
            {
                AIHub2 theHub = thisObject.GetComponent<AIHub2>();
                if (theHub != null)
                {
                    while (callableUpdateCounter < callableUpdatesPerZoneFrame)
                    {
                        callableUpdateCounter++;
                        theHub.callableUpdate();
                    }
                    callableUpdateCounter = 0;

                }
            }
        }


        if (currentZoneFrame == framesPerZone)
        {
            currentMapZone++;
            currentZoneFrame = 0;
            //nearestXNumberOfYToZExceptYAndTheRemainder
        }
        else
        {
            currentZoneFrame++;
        }

    }

    public void farUpdateUNISED(mapZoneScript currentZone)
    {
        //updateOneZone(currentZone, callableUpdatesPerZoneFrame);
    }



    public void updateOneZoneByGettingEachAIHub2(mapZoneScript currentZone, int howManyUpdatesPerEntity)
    {
        foreach (GameObject thisObject in currentZone.theList)
        {
            AIHub2 theHub = thisObject.GetComponent<AIHub2>();
            if (theHub != null)
            {
                while (callableUpdateCounter < howManyUpdatesPerEntity)
                {
                    callableUpdateCounter++;
                    theHub.callableUpdate();
                }
                callableUpdateCounter = 0;
            }
        }
    }
    public void nearUpdateUNUSED(mapZoneScript currentZone)
    {
        //updateOneZone(currentZone, callableUpdatesPerZoneFrame);
    }

    public void OLDnearUpdateUNUSED(List<mapZoneScript> listOfZones)
    {
        foreach(mapZoneScript thisZone in listOfZones)
        {
            //updateOneZone(thisZone, 1);
        }
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
