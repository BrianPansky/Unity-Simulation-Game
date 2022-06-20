using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSpeed = 290f;

    public Transform playerBody;

    float xRotation = 0f;

    //need the "playerClickInteraction" script 
    //(in order to check if we are in a menu or not)
    public playerClickInteraction thePlayerClickInteractionScript;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //need the "playerClickInteraction" script 
        //(in order to check if we are in a menu or not)
        //but that script is on the PARENT object, so first need to get the parent object:
        GameObject thisParent = this.transform.parent.gameObject;
        //now can get the script:
        thePlayerClickInteractionScript = thisParent.GetComponent<playerClickInteraction>(); //this.gameObject.GetComponent("playerClickInteraction") as playerClickInteraction;

        if (thePlayerClickInteractionScript == null)
        {
            Debug.Log("wtffffffffffffffffffffffffffffffffffffffffffffff");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ONLY can look around if we aren't in a menu!!!
        if (thePlayerClickInteractionScript.inMenu == false)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
        
    }
}
