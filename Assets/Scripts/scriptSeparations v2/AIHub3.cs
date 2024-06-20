using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using static tagging2;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class AIHub3 : MonoBehaviour, IupdateCallable
{
    //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

    public NavMeshAgent currentNavMeshAgent;
    public bool printThisNPC = false;

    int adhocCooldown = 0;


    virtualGamepad vGpad;

    bool test = true;

    public List<IupdateCallable> currentUpdateList { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (currentNavMeshAgent == null)
        {
            currentNavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
            if (currentNavMeshAgent == null)
            {
                currentNavMeshAgent = this.gameObject.AddComponent<NavMeshAgent>();
            }
        }
        vGpad = this.transform.GetComponent<virtualGamepad>();
        if (vGpad == null)
        {

            vGpad = this.transform.gameObject.AddComponent<virtualGamepad>();
        }

        //test
        
    }

    // Update is called once per frame
    void Update()
    {

        adhocCooldown++;
    }


    public void callableUpdate()
    {

        if (adhocCooldown < 44)
        {
            return;
        }

        adhocCooldown = 0;
        //Debug.Log("0000000000000000000destination:  " + currentNavMeshAgent.destination);
        //currentNavMeshAgent.SetDestination(this.gameObject.transform.position + new Vector3(1, -0.5f, -14));

        //justDoRandomByINPUT();
        //justDoRandomByInputORVector();
        justDoRandomByBoolORTarget();
        simpleDodge();

        /*
        if(vGpad.allCurrentBoolEnactables.Count == 0)
        {
            return;
        }

        vGpad.allCurrentBoolEnactables[0].enact();
        */
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
                        //Debug.Log("?????????????????????????????????????");
                        controller.enabled = false;
                    }

                    //int x = Random.Range(-8, 8);
                    //int y = Random.Range(-8, 8);

                    //item.enact(new Vector2(x, y));

                    //eh, should i store this elsewhere?  but where else would be best?  for all other objects?
                    //or just ALSO store it here on AIHub3, because AI will USE it often enough?
                    //but then when/how to update it?  there's the rub.
                    objectIdPair thisPair = tagging2.singleton.idPairGrabify(this.gameObject);

                    int currentZone = tagging2.singleton.zoneOfObject[thisPair];
                    /*
                    Debug.Log("mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm");
                    foreach (List<objectIdPair> obidpList in tagging2.singleton.objectsInZone.Values)
                    {
                        Debug.Log("obidpList.Count: " + obidpList.Count);
                        
                    }
                    */

                    GameObject target = tagging2.singleton.pickRandomObjectFromListEXCEPT(tagging2.singleton.listInObjectFormat(tagging2.singleton.objectsInZone[currentZone]), this.gameObject);
                    //          Debug.DrawLine(this.transform.position, target.transform.position, Color.blue, 2f);
                    item.enact(target.transform.position);
                    //item.enact(this.transform.position + new Vector3(-3, -0.5f, 0));

                    //Debug.Log("3333333333333333333333333 target: " + target);

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


        Debug.Log("whichToPick" + whichToPick);

        int indexCount = 0;

        if (whichToPick <= bools - 1)
        {

            Debug.Log("whichToPick <= bools - 1");
            Debug.Log("bools - 1:  " + (bools - 1));

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
        else
        {
            Debug.Log("else");
            doRandomByTarget(whichToPick - bools);
        }

    }

    private void doRandomByTarget(int whichToPick)
    {
        Debug.Log("doRandomByTarget:  " + whichToPick);

        objectIdPair thisId = tagging2.singleton.idPairGrabify(this.gameObject);
        Debug.Log("thisId:  " + thisId);
        int currentZone = tagging2.singleton.zoneOfObject[thisId];
        Debug.Log("currentZone:  " + currentZone);
        GameObject target = tagging2.singleton.pickRandomObjectFromListEXCEPT(
            tagging2.singleton.listInObjectFormat(tagging2.singleton.objectsInZone[currentZone]), 
            this.gameObject);

        Debug.Log("target:  " + target);
        Debug.Log("this.transform.position.ToString()):  " + this.transform.position.ToString());
        Debug.Log("target.transform.position:  " + target.transform.position.ToString());
        Debug.DrawLine(this.transform.position, target.transform.position, Color.blue, 2f);
        Debug.Log("there should be a line, ok" + thisId);
        //item.enact(target.transform.position);

        vGpad.allCurrentTARGETbyVectorEnactables[whichToPick].enact(target.transform.position);


        /*

        foreach (IEnactByTargetVector item in vGpad.allCurrentTARGETbyVectorEnactables)
        {
            if (printThisNPC)
            {
                Debug.Log("item: " + item);
            }

            if (indexCount == whichToPick - bools)
            {

                if (item == null) { continue; }


                CharacterController controller = this.transform.GetComponent<CharacterController>();
                if (controller != null)
                {
                    //Debug.Log("?????????????????????????????????????");
                    controller.enabled = false;
                }

                //int x = Random.Range(-8, 8);
                //int y = Random.Range(-8, 8);

                //item.enact(new Vector2(x, y));

                //eh, should i store this elsewhere?  but where else would be best?  for all other objects?
                //or just ALSO store it here on AIHub3, because AI will USE it often enough?
                //but then when/how to update it?  there's the rub.
                objectIdPair thisPair = tagging2.singleton.idPairGrabify(this.gameObject);

                int currentZone = tagging2.singleton.zoneOfObject[thisPair];
                /*
                Debug.Log("mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm");
                foreach (List<objectIdPair> obidpList in tagging2.singleton.objectsInZone.Values)
                {
                    Debug.Log("obidpList.Count: " + obidpList.Count);

                }

        //item.enact(this.transform.position + new Vector3(-3, -0.5f, 0));
        if (printThisNPC)
                {
                    Debug.DrawLine(this.transform.position, target.transform.position, Color.magenta, 2f);
                    Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> target: " + target);
                    Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> currentNavMeshAgent.destination: " + currentNavMeshAgent.destination);
                    Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> currentNavMeshAgent.enabled: " + currentNavMeshAgent.enabled);
                    Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> currentNavMeshAgent.hasPath: " + currentNavMeshAgent.hasPath);
                    Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> currentNavMeshAgent.isActiveAndEnabled: " + currentNavMeshAgent.isActiveAndEnabled);
                    Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> currentNavMeshAgent.isOnNavMesh: " + currentNavMeshAgent.isOnNavMesh);
                    Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> currentNavMeshAgent.isPathStale: " + currentNavMeshAgent.isPathStale);
                    Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> currentNavMeshAgent.isStopped: " + currentNavMeshAgent.isStopped);
                    //Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> currentNavMeshAgent.isActiveAndEnabled: " + currentNavMeshAgent);
                }
                //

            }
            
        }

        */


    }

    void simpleDodge()
    {
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
            //Debug.Log("threatList.Count < 1:  return");
            return; }

        //from my older code:
        //
        spatialDataPoint myData = new spatialDataPoint();
        myData.initializeDataPoint(threatListWithoutSelf(), this.transform.position);

        Vector3 adHocThreatAvoidanceVector = myData.applePattern();

        if(currentNavMeshAgent.enabled == true)
        {
            //currentNavMeshAgent.SetDestination(this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized * 8f);
            //UnityEngine.Vector3 p1 = this.gameObject.transform.position;
            currentNavMeshAgent.SetDestination(this.gameObject.transform.position + adHocThreatAvoidanceVector.normalized * 12f);

        }

    }



    public List<GameObject> threatListWithoutSelf()
    {
        List<GameObject> threatListWithoutSelf = new List<GameObject>();
        List<GameObject> thisThreatList = tagging2.singleton.listInObjectFormat(new find().allWithOneTag(new find().allInZone(tagging2.singleton.whichZone(this.gameObject)), tagging2.tag2.gamepad));//tagging2.singleton.all

        //Debug.Log("body.theLocalMapZoneScript.threatList.Count:  " + body.theLocalMapZoneScript.threatList.Count);
        //printAllIdNumbers(body.theLocalMapZoneScript.threatList);

        foreach (GameObject threat in thisThreatList)
        {
            UnityEngine.Vector3 p1 = this.gameObject.transform.position;
            UnityEngine.Vector3 p2 = threat.gameObject.transform.position;
            Debug.DrawLine(p1, p2, new Color(1f, 0f, 0f), 1f);
            if (threat != null && threat != this.gameObject)
            {
                threatListWithoutSelf.Add(threat);
                Vector3 threatPosition = threat.gameObject.transform.position;
                //Debug.Log("threat:  " + threat.name + "    threatPosition x+y+z:  X:  " + threatPosition.x + "  Y:  " + threatPosition.y + "  Z:  " + threatPosition.z);

            }
        }
        //Debug.Log("threatListWithoutSelf.Count:  " + threatListWithoutSelf.Count);
        //printAllIdNumbers(threatListWithoutSelf);
        return threatListWithoutSelf;
    }



    void somethingInTHisCodeIsBreakingNavMeshAgentitwasjusttheinputvectorwastoohighabovefloorithinkmaybe()
    {


        if (test == false)
        {
            return;
            
        }


        CharacterController controller = this.transform.GetComponent<CharacterController>();
        if (controller != null)
        {
            //Debug.Log("?????????????????????????????????????");
            controller.enabled = false;
        }


        if (vGpad.allCurrentTARGETbyVectorEnactables.Count == 0)
        {
            //vGpad.allCurrentTARGETbyVectorEnactables[virtualGamepad.buttonCategories.vector1].enact(new Vector3(22, 11, 17));
            //Debug.Log("..................................");
            return;
        }




        GameObject target = tagging2.singleton.findXNearestToY(this.gameObject, tagging2.tag2.interactable);
        Debug.DrawLine(this.transform.position, target.transform.position, Color.blue, 1f);


        //vGpad.allCurrentTARGETbyVectorEnactables[0].enact(new Vector3(10, 0, 20));
        vGpad.allCurrentTARGETbyVectorEnactables[0].enact(target.transform.position);

        test = false;



        //Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzdestination:  " + currentNavMeshAgent.destination);
        //someDebugLogs();
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
