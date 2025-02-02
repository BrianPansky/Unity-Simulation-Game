using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerInputs : MonoBehaviour
{
    virtualGamepad theVirtualGamePad;

    //      mouse look stuff:
    public float mouseSpeed = 290f;
    public Transform playerBody;
    float xRotation = 0f;

    float verticalCameraRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        theVirtualGamePad = genGen.singleton.ensureVirtualGamePad(this.gameObject);


        NavMeshAgent anyNavAgent = this.gameObject.GetComponent<NavMeshAgent>();
        if (anyNavAgent != null)
        {
            anyNavAgent.enabled = false;
        }
        
        this.gameObject.AddComponent<gravityToFall>();
    }
}
