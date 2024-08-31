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
using System.Threading;
using static UnityEditor.PlayerSettings;
using UnityEngine.XR;
using UnityEngine.Assertions;


public class AIHub3 : planningAndImagination, IupdateCallable
{

    public NavMeshAgent currentNavMeshAgent;
    virtualGamepad vGpad;

    GameObject placeholderTarget1;

    int adhocCooldown = 0;
    public bool printThisNPC = false;
    bool test = true;


    void Awake()
    {
        placeholderTarget1 = new GameObject();
        vGpad = genGen.singleton.ensureVirtualGamePad(this.gameObject);
    }

    public List<IupdateCallable> currentUpdateList { get; set; }
    

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("=================================      START      ===============================");

        fullPlan = goGrabThenEquip(interType.shoot1);
    }

    public planEXE2 goGrabThenEquip(interType interTypeX)
    {
        planEXE2 zerothShell = new seriesEXE(goGrabPlan1(interTypeX));
        zerothShell.Add(equipX2(interTypeX));
        zerothShell.untilListFinished();

        return zerothShell;
    }

    public planEXE2 goGrabPlan1(interType interTypeX)
    {
        //ad-hoc hand-written plan
        GameObject target = pickRandomObjectFromList(allNearbyEquippablesWithInterTypeX(interTypeX));


        planEXE2 firstShell = new seriesEXE();
        firstShell.Add(walkToTarget2(target, 1.9f));
        firstShell.Add(aimTargetPlan2(target));
        firstShell.Add(firePlan2nd2(interType.standardClick, target));
        firstShell.untilListFinished();

        return firstShell;
    }

    public planEXE2 combatBehaviorPlan1()
    {
        //ad-hoc hand-written plan
        GameObject target2 = pickRandomObjectFromList(threatListWithoutSelf());

        if(target2 == null) { return new singleEXE(); }

        planEXE2 secondShell = new seriesEXE();
        secondShell.Add(aimTargetPlan2(target2));
        secondShell.Add(firePlan2nd2(interType.shoot1, target2));
        secondShell.untilListFinished();

        planEXE2 parallel = new parallelEXE(secondShell, combatDodgeEXE2());
        parallel.untilListFinished();
        return parallel;
    }

    private planEXE2 combatDodgeEXE2()
    {
        //ad-hoc hand-coded plan

        //calculate a position to "dodge" towards
        //place an empty "nav point" object there
        //then just use walkToTarget(target)

        //look at simpleDodge
        List<GameObject> threatList = threatListWithoutSelf();

        Vector3 adHocThreatAvoidanceVector = new spatialDataPoint(threatListWithoutSelf(), this.transform.position).applePattern();

        placeholderTarget1.transform.position = this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized * 4.7f;
        

        return walkToTarget2(placeholderTarget1, 1.9f);
    }

    private planEXE2 equipX2(interType interTypeX)
    {
        GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, getInventory());

        //IEnactaBool testE1 = new takeFromAndPutBackIntoInventory(this.gameObject);

        IEnactaBool testE1 = this.gameObject.GetComponent<takeFromAndPutBackIntoInventory>();

        //planEXE2 exe1 = new singleEXE(testE1, theItemWeWant);
        planEXE2 exe1 = testE1.toEXE(theItemWeWant);
        exe1.atLeastOnce();
        //condition thisCondition = new enacted(exe1);
        //exe1.endConditions.Add(thisCondition);
        return exe1;
    }
    

    private planEXE2 firePlan2nd2(interType interTypeX, GameObject target)
    {

        //either playable will already have the type, or it might be in equipper slots
        rangedEnaction grabEnact1;
        grabEnact1 = enactionWithInterTypeXOnObjectsPlayable(this.gameObject, interTypeX);

        if (grabEnact1 == null)
        {
            GameObject theItemWeWant = firstObjectOnListWIthInterTypeX(interTypeX, equipperContents());

            //oh no it can ALSO be null
            if(theItemWeWant == null)
            {
                //Debug.DrawLine(Vector3.zero, this.transform.position, Color.magenta, 6f);
                return goGrabPlan1(interType.shoot1);
            }
            

            //Debug.Assert(theItemWeWant != null);

            grabEnact1 = theItemWeWant.GetComponent<rangedEnaction>();
        }

        planEXE2 exe1 = grabEnact1.toEXE(null);

        //Debug.Log("grabEnact1.theCooldown:  " + grabEnact1.theCooldown);
        //Debug.Log("grabEnact1.theCooldown.cooldownMax:  " + grabEnact1.theCooldown.cooldownMax);
        //Debug.Log("grabEnact1.theCooldown.cooldownTimer:  " + grabEnact1.theCooldown.cooldownTimer);
        exe1.startConditions.Add(grabEnact1.theCooldown);
        exe1.atLeastOnce();
        //condition thisCondition = new enacted(exe1);
        //exe1.endConditions.Add(thisCondition);

        return exe1;
    }

    private planEXE2 aimTargetPlan2(GameObject target)
    {
        aimTarget testE1 = this.gameObject.GetComponent<aimTarget>();

        planEXE2 exe1 = testE1.toEXE(target);
        exe1.atLeastOnce();

        return exe1;
    }

    private planEXE2 walkToTarget2(GameObject target, float offsetRoom = 0f)
    {
        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();
        Debug.Assert(theNavAgent != null);

        Vector3 targetPosition = target.transform.position;
        Vector3 between = targetPosition - this.transform.position;
        placeholderTarget1.transform.position = targetPosition - between.normalized * offsetRoom;


        planEXE2 theEXE = new vect3EXE2(theNavAgent, placeholderTarget1);
        proximity condition = new proximity(this.gameObject, placeholderTarget1, offsetRoom*1.4f);
        theEXE.endConditions.Add(condition);

        return theEXE;
    }












    // Update is called once per frame
    void Update()
    {
        adhocCooldown++;
    }



    public void callableUpdate()
    {
        //Debug.Log("=======================callableUpdate()............");

        //Debug.Log("333      multiPlan.Keys.Count:  " + multiPlan.Keys.Count);


        if (fullPlan == null) { Debug.Log("fullPlan == null");
                                fullPlan = combatBehaviorPlan1(); return; }
        if(fullPlan.endConditionsMet() ) { //Debug.Log("fullPlan.endConditionsMet()");
                                                                fullPlan = combatBehaviorPlan1(); return; }

        //Debug.Log("we have a plan, fullPlan.execute()");
        fullPlan.execute();
        
    }



    void randomWanderPlan()
    {
        //ad-hoc hand-coded plan

        //k, they go to random navpoints, great
        //[though...........those navpoints are never DELETED............]
        //[they should have just ONE "nextNav" object, and just MOVE it around ???]

        GameObject target = createNavpointInRandomDirection();
        //              enaction anEnaction = walkToTarget(target).theEnaction;
        //              buttonCategories theButtonCategory = anEnaction.gamepadButtonType;
        //              multiPlanAdd(walkToTarget(target), blankMultiPlan());
    }


    public GameObject createNavpointInRandomDirection()
    {
        GameObject theNavpoint = new GameObject();

        float initialDistance = 2f;
        float randomAdditionalDistance = UnityEngine.Random.Range(0, 33);
        theNavpoint.transform.position += new Vector3(initialDistance+ randomAdditionalDistance, 0,0);
        randomAdditionalDistance = UnityEngine.Random.Range(0, 33);
        theNavpoint.transform.position += new Vector3(0,0, initialDistance + randomAdditionalDistance);

        return theNavpoint;
    }


    public bool threatLineOfSight()
    {
        //super ad-hoc for now

        spatialDataPoint myData = new spatialDataPoint(threatListWithoutSelf(), this.transform.position);



        return myData.threatLineOfSightBool();
    }





    private GameObject pickRandomObjectFromList(List<GameObject> theList)
    {

        if (theList.Count == 0)
        {
            //Debug.Log("there are zero objects on the list of objects entered into ''pickRandomObjectFromListEXCEPT''");
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




    public rangedEnaction enactionWithInterTypeXOnObjectsPlayable(GameObject theObject, interType intertypeX)
    {
        foreach (rangedEnaction thisEnaction in listOfIEnactaBoolsOnObject(theObject))
        {

            if (thisEnaction.interInfo.interactionType == intertypeX) { return thisEnaction; }
        }



        return null;
    }

    private List<IEnactaBool> listOfIEnactaBoolsOnObject(GameObject theObject)
    {
        //hmm:
        //List<IEnactaBool> theList = [.. theObject.GetComponents<collisionEnaction>()];


        List<IEnactaBool> theList = new List<IEnactaBool>();

        foreach (collisionEnaction thisEnaction in theObject.GetComponents<collisionEnaction>())
        {
            theList.Add(thisEnaction);
        }


        return theList;
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




    public List<GameObject> allNearbyEquippablesWithInterTypeX(interType theInterType)
    {

        List<GameObject> theListOfALL = new find().allObjectsInObjectsZone(this.gameObject);
        List<GameObject> theListOfEquippables = new List<GameObject> ();

        foreach (GameObject thisObject in theListOfALL)
        {

            equippable2 equip = thisObject.GetComponent<equippable2>();
            if (equip == null) { continue; }

            if (equip.containsIntertype(theInterType))
            {
                theListOfEquippables.Add(thisObject);
            }
        }

        return theListOfEquippables;
    }
    //justSenseNearbyEquipables


    public GameObject firstNearbyEquipableWithInteractionType(interType theInterType)
    {

        GameObject theItemWeWant = null;

        List<GameObject> theList = new find().allObjectsInObjectsZone(this.gameObject);

        theItemWeWant = firstObjectOnListWIthInterTypeX(theInterType,theList);

        return theItemWeWant;
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



    public List<GameObject> threatListWithoutSelf()
    {
        List<GameObject> threatListWithoutSelf = new List<GameObject>();
        //Debug.Log("tagging2.singleton.whichZone(this.gameObject):  " + tagging2.singleton.whichZone(this.gameObject));
        List<objectIdPair> theList = new find().allInZone(tagging2.singleton.whichZone(this.gameObject));
        //Debug.Log("theList.Count:  " + theList.Count);
        List<GameObject> thisThreatList = tagging2.singleton.listInObjectFormat(
            new find().allWithOneTag(
                new find().allInZone(tagging2.singleton.whichZone(this.gameObject)), tagging2.tag2.gamepad));//tagging2.singleton.all

        //Debug.Log("thisThreatList.Count:  " + thisThreatList.Count);

        foreach (GameObject threat in thisThreatList)
        {
            //UnityEngine.Vector3 p1 = this.gameObject.transform.position;
            //UnityEngine.Vector3 p2 = threat.gameObject.transform.position;
            //Debug.DrawLine(p1, p2, new Color(0f, 0f, 1f), 1f);
            if (threat != null && threat != this.gameObject)
            {
                //UnityEngine.Vector3 p1 = this.gameObject.transform.position;
                //UnityEngine.Vector3 p2 = threat.gameObject.transform.position;
                //Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 1f);
                threatListWithoutSelf.Add(threat);
            }
        }

        //UnityEngine.Vector3 p3 = this.gameObject.transform.position;
        //UnityEngine.Vector3 p4 = Vector3.zero;
        //Debug.DrawLine(p3, p4, new Color(0f, 0f, 0f), 1f);
        //Debug.Assert(threatListWithoutSelf.Count > 0);
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
