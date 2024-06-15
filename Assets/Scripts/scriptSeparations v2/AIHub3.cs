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


        somethingInTHisCodeIsBreakingNavMeshAgent();

        if(vGpad.allCurrentBoolEnactables.Count == 0)
        {
            return;
        }

        vGpad.allCurrentBoolEnactables[0].enact();
    }


    void somethingInTHisCodeIsBreakingNavMeshAgent()
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
