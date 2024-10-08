﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navmeshAgentDebugging : MonoBehaviour
{

    public NavMeshAgent currentNavMeshAgent;

    public int cooldownTimer2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentNavMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        if (currentNavMeshAgent == null)
        {
            Debug.Log("this object did not have a navmesh agent, adding one now...");
            currentNavMeshAgent = this.gameObject.AddComponent<NavMeshAgent>();
        }


        currentNavMeshAgent.baseOffset = 1; //prevent stutter, being in floor


        currentNavMeshAgent.SetDestination(this.gameObject.transform.position + new Vector3(11, 10.3f, 4));
    }

    // Update is called once per frame
    void Update()
    {

        if (cooldownTimer2 > 55)
        {
            cooldownTimer2 = 0;
            return;
        }

        cooldownTimer2++;

        //Debug.DrawLine(new Vector3(), this.gameObject.transform.position, Color.blue, 0.01f);
        currentNavMeshAgent.SetDestination(this.gameObject.transform.position + new Vector3(21, -0.5f, 14));

        //Debug.DrawLine(new Vector3(), this.gameObject.transform.position, Color.blue, 0.01f);

        //someDebugLogs();

    }

    void someDebugLogs()
    {

        Debug.Log(".......................................................");
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
