using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastDebugger : MonoBehaviour
{
    int delayAtStart = 12;
    int numberOfRaysToFire = 15;
    float angleIncrement = 3.2f;
    float currentAngle = 0;


    string theRedString = "Cube floor (UnityEngine.BoxCollider)";

    //GameObject firePoint;



    // Start is called before the first frame update
    void Start()
    {
        delayAtStart = 22;
        //this.gameObject.layer = LayerMask.GetMask("yesIgnoreThisLayer");

        //firePoint = new GameObject();
        //firePoint.layer = LayerMask.GetMask("yesIgnoreThisLayer");
        //firePoint.transform.position = this.transform.position - this.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (delay()) { return; }


        while (numberOfRaysToFire > 0)
        {
            fireRaycastHit();
            //  tryingRaycastALL();
            currentAngle += angleIncrement;
            rotateFiringPoint();
            numberOfRaysToFire--;
        }


    }


    void tryingRaycastALL()
    {
        Ray myRay = new Ray(this.transform.position + -this.transform.up, -this.transform.up);

        RaycastHit[] myHitsOrWhatever = Physics.RaycastAll(myRay.origin, myRay.direction, 200f, ~LayerMask.GetMask("yesIgnoreThisLayer"), QueryTriggerInteraction.Ignore);


        Debug.Log(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
        foreach (RaycastHit hit in myHitsOrWhatever)
        {

            //Debug.Log("raycastDebugger hit:  " + hit.collider);
            printTextAndLines(hit);
        }


    }



    private void rotateFiringPoint()
    {
        this.transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);
    }

    private bool delay()
    {
        if(delayAtStart < 1) { return false; }

        delayAtStart--;

        return true;
    }

    public void fireRaycastHit()
    {
        RaycastHit myHit;
        Ray myRay = new Ray(this.transform.position, -this.transform.up);

        Debug.DrawRay(myRay.origin, myRay.direction.normalized, Color.black, 9f);

        //if (Physics.Raycast(myRay, out myHit, 200f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) == false)
        if (Physics.Raycast(myRay, out myHit, 200f, ~LayerMask.GetMask("yesIgnoreThisLayer"), QueryTriggerInteraction.Ignore) == false)
        {
            Debug.Log("raycastDebugger hit FALSE");
            return;
        }
        if (myHit.transform == null)
        {
            Debug.Log("raycastDebugger hit null");
            return;
        }

        printTextAndLines(myHit);



    }

    private void printTextAndLines(RaycastHit myHit)
    {
        //Debug.Log("raycastDebugger hit:  " + myHit.collider + ", DISTANCE:  " + myHit.distance);
        Debug.Log("raycastDebugger hit:  " + myHit.collider + ", DISTANCE:  " + (myHit.point - this.transform.position).magnitude);
        //Debug.DrawLine(firePoint.transform.position, myHit.collider.gameObject.transform.position, Color.cyan, 7f);

        if (myHit.collider.ToString() == theRedString)
        {

            Debug.DrawLine(this.transform.position, myHit.point, Color.red, 7f);
        }
        else
        {

            Debug.DrawLine(this.transform.position, myHit.point, Color.green, 7f);
        }
    }
}
