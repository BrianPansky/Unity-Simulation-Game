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
    bool done = false;

    float limitedPitchRotation = 0f;


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
        if (done || delay()) { return; }

        //testRaycast();
        testPitchRotation();

        done = true;
    }

    private void testPitchRotation()
    {
        List<float> pitchesToIncrement = new List<float>();
        pitchesToIncrement.Add(0f);
        //pitchesToIncrement.Add(1f);
        pitchesToIncrement.Add(18f);
        pitchesToIncrement.Add(48f);
        pitchesToIncrement.Add(125f);
        //pitchesToIncrement.Add(90f);
        //pitchesToIncrement.Add(180f);
        //pitchesToIncrement.Add(30f);
        //pitchesToIncrement.Add(15f);
        //pitchesToIncrement.Add( 15f);
        pitchesToIncrement.Add(0f);
        pitchSet(pitchesToIncrement);
    }


    private void pitchSet(List<float> pitchesToIncrement)
    {

        float offset = 0.03f;
        Vector3 startPoint = this.transform.position;

        foreach (float pitchIncrement in pitchesToIncrement)
        {

            Debug.DrawLine(startPoint, startPoint + this.transform.forward, Color.magenta, 7f);

            updatePitch(pitchIncrement, this.transform);
            startPoint.x += offset;
        }
    }


    private void pitchSpray()
    {

        //int whichIncrrement = 0;
        float pitchInput = 6f;
        float offset = 0.03f;
        Vector3 startPoint = this.transform.position;

        while (numberOfRaysToFire > 0)
        {
            Debug.DrawLine(startPoint, startPoint + this.transform.forward, Color.magenta, 7f);

            updatePitch(pitchInput, this.transform);
            startPoint.x += offset;
            numberOfRaysToFire--;
        }
    }


    public void updatePitch(float pitchInput, Transform theTransformToRotate)
    {
        //CONFIRMED, THIS CODE MEANS "limitedPitchRotation" = THE ANGLE WE WANT TO SET IT TO!


        //float initial = limitedPitchRotation;
        limitedPitchRotation -= pitchInput;// * pitchSpeed;
        //limitedPitchRotation = Mathf.Clamp(limitedPitchRotation, -pitchRange, pitchRange);

        //float relativeAngle = initial - limitedPitchRotation;

        theTransformToRotate.localRotation = Quaternion.Euler(limitedPitchRotation, 0f, 0f);
        //thePartToAimVertical.Rotate(thePartToAimVertical.right, relativeAngle);
    }







    private void testRaycast()
    {
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
