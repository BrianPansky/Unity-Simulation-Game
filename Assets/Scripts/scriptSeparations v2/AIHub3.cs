using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIHub3 : MonoBehaviour
{
    //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

    public NavMeshAgent currentNavMeshAgent;


    int adhocCooldown = 0;


    virtualGamepad vGpad;

    bool test = true;


    // Start is called before the first frame update
    void Start()
    {
        if (currentNavMeshAgent == null)
        {
            currentNavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
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

        if(adhocCooldown < 44)
        {
            adhocCooldown++;
            return;
        }

        adhocCooldown = 0;
        //Debug.Log("0000000000000000000destination:  " + currentNavMeshAgent.destination);
        //currentNavMeshAgent.SetDestination(this.gameObject.transform.position + new Vector3(1, -0.5f, -14));


        justDoRandomByInputORVector();

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

                    GameObject target = tagging2.singleton.pickRandomObjectFromListEXCEPT(tagging2.singleton.listInObjectFormat(tagging2.singleton.objectsInZone[currentZone]), this.gameObject);
                    item.enact(target.transform.position);
                    //item.enact(this.transform.position + new Vector3(-3, -0.5f, 0));

                    //Debug.Log("3333333333333333333333333 target: " + target);

                }
                indexCount++;
            }
        }

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
