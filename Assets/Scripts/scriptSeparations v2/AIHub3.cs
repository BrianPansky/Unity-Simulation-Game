using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using static tagging2;
using static enactionCreator;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static interactionCreator;


public class AIHub3 : planningAndImagination, IupdateCallable
{

    public NavMeshAgent currentNavMeshAgent;
    virtualGamepad vGpad;

    int adhocCooldown = 0;
    public bool printThisNPC = false;
    bool test = true;


    void Awake()
    {
        vGpad = genGen.singleton.ensureVirtualGamePad(this.gameObject);
    }

    public List<IupdateCallable> currentUpdateList { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        currentNavMeshAgent = genGen.singleton.ensureNavmeshAgent(this.gameObject);

        makeAdHocPlanToDo();
    }

    // Update is called once per frame
    void Update()
    {
        adhocCooldown++;
    }


    public void callableUpdate()
    {
        //Debug.Log("=======================callableUpdate()............");

        doCurrentPlanStep();
        if (adhocCooldown < 44){return;}

        adhocCooldown = 0;
        makeAdHocPlanToDo();
    }






    void makeAdHocPlanToDo()
    {
        plan.Add(equipX(interType.shoot1));
        plan.Add(aimPlan1());
        plan.Add(firePlan2(interType.shoot1));


    }

    private planEXE equipX(interType interTypeX)
    {
        GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, getInventory());

        IEnactaBool testE1 = new takeFromAndPutBackIntoInventory(this.gameObject);

        boolEXE exe1 = new boolEXE(testE1, theItemWeWant);
        return exe1;
    }

    

    planEXE aimPlan1()
    {
        //pick random enemy target
        //aim

        GameObject target = pickRandomObjectFromList(threatListWithoutSelf());

        playable2 thePlayable = this.gameObject.GetComponent<playable2>();
        IEnactByTargetVector testE1 = new aimTarget(new vecRotation(thePlayable.lookSpeed, thePlayable.transform, thePlayable.enactionPoint1.transform, buttonCategories.vector2));

        vectEXE exe1 = new vectEXE(testE1, target);
        return exe1;
    }

    private GameObject pickRandomObjectFromList(List<GameObject> theList)
    {

        if (theList.Count == 0)
        {
            Debug.Log("there are zero objects on the list of objects entered into ''pickRandomObjectFromListEXCEPT''");
            return null;
        }


        int numberOfTries = 10; //easy ad hoc way to terminate a potentially infinate loop for now lol
        GameObject thisObject;
        thisObject = null;


        while (numberOfTries > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, theList.Count);
            thisObject = theList[randomIndex];

            if (thisObject != null)
            {
                return thisObject;
            }

            numberOfTries--;
        }




        return thisObject;

    }

    planEXE firePlan2(interType interTypeX)
    {

        GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, equipperContents());

        //oh no it can be null
        if(theItemWeWant == null) { return null; }//???

        IEnactaBool grabEnact1 = theItemWeWant.GetComponent<IEnactaBool>();

        boolEXE exe1 = new boolEXE(grabEnact1, null);

        return exe1;
    }

    private List<GameObject> equipperContents()
    {
        playable2 thePlayable = this.gameObject.GetComponent<playable2>();

        List<GameObject> theList = new List<GameObject>();
        foreach (var x in thePlayable.equipperSlotsAndContents.Keys)
        {
            if (thePlayable.equipperSlotsAndContents[x] == null) { continue; }
            theList.Add(thePlayable.equipperSlotsAndContents[x]);
        }

        return theList;
    }

    void doCurrentPlanStep()
    {
        if (plan.Count <1) { return; }
        if(plan[0] == null)
        {
            Debug.Log("how to handle this");
            plan.RemoveAt(0); 
            return; }

        plan[0].executePlan();

        plan.RemoveAt(0);
    }


    public List<GameObject> getInventory()
    {
        inventory1 inventory = this.gameObject.GetComponent<inventory1>();
        return inventory.inventoryItems;
    }

    GameObject firstObjectOnListWIthInterTypeX(interType interTypeX, List<GameObject> theList)
    {
        //looking at the INTERACTION TYPES of their enactions

        GameObject theItemWeWant = null;

        foreach (GameObject thisObject in theList)
        {

            equippable2 equip = thisObject.GetComponent<equippable2>();
            if (equip == null) { continue; }

            if (equip.containsIntertype(interTypeX))
            {
                theItemWeWant = thisObject;
                break;
            }
        }

        return theItemWeWant;
    }




    private void justSenseNearbyEquipables(numericalVariable health, bool shouldItBeAddition)
    {
        throw new System.NotImplementedException();
    }


    void justDoRandomByINPUT()
    {
        int bools = vGpad.allCurrentBoolEnactables.Count;
        int vectors = vGpad.allCurrentVectorEnactables.Count;

        int whichToPick = Random.Range(0, bools + vectors);


        int dictionaryEntryCount = 0;

        if (whichToPick <= bools-1)
        {
            foreach (var item in vGpad.allCurrentBoolEnactables.Values)
            {
                if(dictionaryEntryCount == whichToPick)
                {
                    if (item == null) { return; }
                    item.enact();
                }
                dictionaryEntryCount++;
            }
        }
        else
        {
            foreach (var item in vGpad.allCurrentVectorEnactables.Values)
            {
                if (dictionaryEntryCount == whichToPick- bools)
                {

                    if (item == null){return; }
                    int x = Random.Range(-8, 8);
                    int y = Random.Range(-8, 8);

                    Debug.Log("item.enact:  " + item);
                    item.enact(new Vector2(x,y));
                }
                dictionaryEntryCount++;
            }
        }

    }

    void justDoRandomByInputORVector()
    {
        int bools = vGpad.allCurrentBoolEnactables.Count;
        int vectors = vGpad.allCurrentVectorEnactables.Count;
        int byTargets = vGpad.allCurrentTARGETbyVectorEnactables.Count;

        int whichToPick = Random.Range(0, bools + vectors + byTargets);


        int indexCount = 0;

        if (whichToPick <= bools - 1)
        {
            foreach (var item in vGpad.allCurrentBoolEnactables.Values)
            {
                if (indexCount == whichToPick)
                {
                    if (item == null) { return; }
                    item.enact();
                }
                indexCount++;
            }
        }
        else if(whichToPick <= bools + vectors - 1)
        {
            foreach (var item in vGpad.allCurrentVectorEnactables.Values)
            {
                if (indexCount == whichToPick - bools)
                {

                    if (item == null) { return; }
                    int x = Random.Range(-8, 8);
                    int y = Random.Range(-8, 8);

                    item.enact(new Vector2(x, y));
                }
                indexCount++;
            }
        }
        else
        {
            foreach (var item in vGpad.allCurrentTARGETbyVectorEnactables)
            {
                if (indexCount == whichToPick - bools - vectors)
                {

                    if (item == null) { return; }


                    CharacterController controller = this.transform.GetComponent<CharacterController>();
                    if (controller != null)
                    {
                        controller.enabled = false;
                    }

                    //eh, should i store this elsewhere?  but where else would be best?  for all other objects?
                    //or just ALSO store it here on AIHub3, because AI will USE it often enough?
                    //but then when/how to update it?  there's the rub.
                    objectIdPair thisPair = tagging2.singleton.idPairGrabify(this.gameObject);

                    int currentZone = tagging2.singleton.zoneOfObject[thisPair];
                    GameObject target = pickRandomObjectFromListEXCEPT(tagging2.singleton.listInObjectFormat(tagging2.singleton.objectsInZone[currentZone]), this.gameObject);
                    //          Debug.DrawLine(this.transform.position, target.transform.position, Color.blue, 2f);
                    item.enact(target.transform.position);
                }
                indexCount++;
            }
        }

    }

    void justDoRandomByBoolORTarget()
    {
        int bools = vGpad.allCurrentBoolEnactables.Count;
        int byTargets = vGpad.allCurrentTARGETbyVectorEnactables.Count;

        int whichToPick = Random.Range(0, bools + byTargets);



        int indexCount = 0;

        if (whichToPick <= bools - 1)
        {
            IEnactaBool enactaBool = null;

            foreach (buttonCategories key in vGpad.allCurrentBoolEnactables.Keys)
            {
                if (indexCount == whichToPick)
                {
                    if (vGpad.allCurrentBoolEnactables[key] == null)
                    {
                        return;
                    }
                    enactaBool = vGpad.allCurrentBoolEnactables[key];
                    break;
                }
                indexCount++;
            }

            enactaBool.enact();
        }
        else
        {
            doRandomByTarget(whichToPick - bools);
        }

    }

    private void doRandomByTarget(int whichToPick)
    {
        //Debug.Log("_______________________________________doRandomByTarget:  " + whichToPick);

        objectIdPair thisId = tagging2.singleton.idPairGrabify(this.gameObject);
        int currentZone = tagging2.singleton.zoneOfObject[thisId];
        GameObject target = pickRandomObjectFromListEXCEPT(
            tagging2.singleton.listInObjectFormat(tagging2.singleton.objectsInZone[currentZone]), 
            this.gameObject);

        vGpad.allCurrentTARGETbyVectorEnactables[whichToPick].enact(target.transform.position);
    }

    void simpleDodge()
    {
        //Debug.Log("try simpleDodge()");
        //Know positions of nearby threats
        //      For now, just log the position of NPCs[and player], which will include any occupied tanks.
        //          Soooo, tag everything with a virtualGamepad?
        //Calculate simple movement vector
        //Place waypoint in that direction, go with nav agent ?

        List<GameObject> threatList = threatListWithoutSelf();

        //need to combine/balance dodging with other navigation goals.
        //for now, at least allow normal navigation if there are zero threats
        //[and save some calculation that is pointless if there are no threats]: 
        if (threatList.Count < 1) {
            return; }

        //from my older code:
        spatialDataPoint myData = new spatialDataPoint();
        myData.initializeDataPoint(threatListWithoutSelf(), this.transform.position);

        Vector3 adHocThreatAvoidanceVector = myData.applePattern();

        if(currentNavMeshAgent.enabled == true)
        {
            currentNavMeshAgent.SetDestination(this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized * 12f);
        }


        //Debug.Log("-----------------------------------end of dodge............");
    }



    public List<GameObject> threatListWithoutSelf()
    {
        List<GameObject> threatListWithoutSelf = new List<GameObject>();
        List<GameObject> thisThreatList = tagging2.singleton.listInObjectFormat(
            new find().allWithOneTag(
                new find().allInZone(tagging2.singleton.whichZone(this.gameObject)), tagging2.tag2.gamepad));//tagging2.singleton.all

        foreach (GameObject threat in thisThreatList)
        {
            UnityEngine.Vector3 p1 = this.gameObject.transform.position;
            UnityEngine.Vector3 p2 = threat.gameObject.transform.position;
            Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 1f);
            if (threat != null && threat != this.gameObject)
            {
                threatListWithoutSelf.Add(threat);
                Vector3 threatPosition = threat.gameObject.transform.position;
            }
        }

        return threatListWithoutSelf;
    }



    public GameObject pickRandomObjectFromListEXCEPT(List<GameObject> theList, GameObject notTHISObject)
    {
        if (theList.Count == 0)
        {
            Debug.Log("there are zero objects on the list of objects entered into ''pickRandomObjectFromListEXCEPT''");
            return null;
        }


        int numberOfTries = 10; //easy ad hoc way to terminate a potentially infinate loop for now lol
        GameObject thisObject;
        thisObject = null;


        while (numberOfTries > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, theList.Count);
            thisObject = theList[randomIndex];

            if (thisObject != notTHISObject)
            {
                return thisObject;
            }

            numberOfTries--;
        }




        return thisObject;

    }




    void someDebugLogs()
    {

        Debug.Log("CURRENT destination:  " + currentNavMeshAgent.destination);
        Debug.Log("hasPath:  " + currentNavMeshAgent.hasPath);
        Debug.Log("isOnNavMesh:  " + currentNavMeshAgent.isOnNavMesh);
        Debug.Log("isStopped:  " + currentNavMeshAgent.isStopped);
        Debug.Log("pathPending:  " + currentNavMeshAgent.pathPending);
        Debug.Log("remainingDistance:  " + (currentNavMeshAgent.destination - currentNavMeshAgent.nextPosition));

        Debug.Log("destination:  " + currentNavMeshAgent.destination);




        Debug.DrawLine(currentNavMeshAgent.nextPosition, currentNavMeshAgent.destination, Color.green);
        //Debug.DrawLine(this.gameObject.transform.position, theEnactionScript.thisNavMeshAgent.destination, Color.magenta);

    }

}
