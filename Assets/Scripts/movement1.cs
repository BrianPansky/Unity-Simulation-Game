using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class movement1 : MonoBehaviour
{
    public GameObject aboveKnee;
    public GameObject belowKnee1;
    public GameObject belowKnee2;
    public GameObject knee1;
    public GameObject knee2;
    public GameObject knee3;
    public GameObject foot;

    public GameObject thwackter1;
    public GameObject thwackter2;
    Vector3 thwackterPoint1;
    Vector3 thwackterPoint2;

    List<Vector3> legPartPoints1 = new List<Vector3>();
    List<Vector3> legPartPoints2 = new List<Vector3>();
    List<Vector3> legPartPoints3 = new List<Vector3>();
    List<Vector3> legPartPoints4 = new List<Vector3>();


    float legPartLength1 = 2.5f;
    float legPartLength2 = 2.7f;
    float legPartLength3 = 2f;
    float legPartLength4 = 0.7f;

    float legSpringLengthP2P4;
    float desiredMechHeight;


    Vector3 legpoint1;
    Vector3 legpoint2;
    Vector3 legpoint3;
    Vector3 legpoint4;
    Vector3 legpoint5;

    Vector3 desiredGroundPoint;
    Vector3 currentFootHeightVector;

    float movementSpeed = 0.01f;
    float timeTicker = 0f;
    float timeIncrement = 3;
    float testAngle = 0;

    Vector3 pAK;
    Quaternion rAK;



    float smooth = 5.0f;
    float tiltAngle = 60.0f;


    // Start is called before the first frame update
    void Start()
    {



        pAK = aboveKnee.transform.position;
        rAK = aboveKnee.transform.rotation;

        //numbers in new vector are ("right of camera", up, "away from camera")
        legpoint1 = this.transform.position + new Vector3(0, 0, -1);
        //legpoint1 = new Vector3(legPartLength1, 0, 0);
        legpoint2 = legpoint1 + new Vector3(legPartLength1, 0, 0);
        legpoint3 = legpoint2 + new Vector3(0, -legPartLength2, 0);
        legpoint4 = legpoint3 + new Vector3(0, -legPartLength3, 0);
        legpoint5 = legpoint4 + new Vector3(-legPartLength4, 0, 0);

        //legpoint3 = legpoint2;
        //legpoint4 = legpoint3;
        //legpoint5 = legpoint4;

        desiredMechHeight = (legPartLength2 + legPartLength3) * (0.9f);
        legSpringLengthP2P4 = (legPartLength2 + legPartLength3) * (0.7f);



        thwackterPoint1 = legpoint2 + new Vector3(0, 0, -legPartLength3);
        thwackterPoint2 = legpoint2 + new Vector3(0, 0, legPartLength3);



        //set this to ground under "shoulder":
        desiredGroundPoint = theGroundPointThatIsDown(legpoint1);
        //legpoint5 = desiredGroundPoint;


        //do raycast under foot?
        currentFootHeightVector = theGroundPointThatIsDown(legpoint5);


        legPartPoints1.Add(legpoint1);
        legPartPoints1.Add(legpoint2);
        legPartPoints2.Add(legpoint2);
        legPartPoints2.Add(legpoint3);
        legPartPoints3.Add(legpoint3);
        legPartPoints3.Add(legpoint4);
        legPartPoints4.Add(legpoint4);
        legPartPoints4.Add(legpoint5);


        myObjPutFunc(knee1, legpoint2);
        myObjPutFunc(knee2, legpoint3);
        myObjPutFunc(knee3, legpoint4);
        myObjPutFunc(foot, legpoint5);
    }



    void Update()
    {

        timeTicker += 0.01f;
        //Debug.Log(timeTicker);
        float rotationSpeed = 0.01f;
        testAngle += 1;
        if(timeTicker > 0 && timeTicker < .1)
        {
            rotateMylegPartPointsAboutinputPoint(legPartPoints4, -1.0f, legpoint4);

            rotateMylegPartPointsAboutinputPoint(legPartPoints3, -1.0f, legpoint4);

            rotateMylegPartPointsAboutinputPoint(legPartPoints2, -1.0f, legpoint4);
            rotateMylegPartPointsAboutinputPoint(legPartPoints1, -1.0f, legpoint4);

            foldParts(legPartPoints2, legPartPoints3, 6.0f);
            //rotateMylegPartPointsAboutinputPoint(legPartPoints2, 1.0f, legpoint1);
            //rotateMylegPartPointsAboutinputPoint(legPartPoints2, 1.0f, legpoint1);
            //rotateMylegPartPointsAboutinputPoint(legPartPoints3, 1.0f, legpoint1);
            //rotateMylegPartPointsAboutinputPoint(legPartPoints4, 1.0f, legpoint1);
            //foldParts(legPartPoints2, legPartPoints3, 1.0f);
            //testAngle = 0;
            //timeTicker = 0;
        }
        else if(timeTicker > .2 && timeTicker < .4)
        {

            //OHHHHHHH, THE LEG POINTS ARE NOT THE SAME AS THE POINTS IN THE "PARTS"
            rotateMylegPartPointsAboutinputPoint(legPartPoints4, 1.0f, legpoint2);
            
            rotateMylegPartPointsAboutinputPoint(legPartPoints3, 1.0f, legpoint2);
            
            rotateMylegPartPointsAboutinputPoint(legPartPoints2, 1.0f, legpoint2);
            rotateMylegPartPointsAboutinputPoint(legPartPoints1, 1.0f, legpoint2);

            if (timeTicker < .3)
            {
                //yes, but how to do this motion with it anchored to the other end?
                foldParts(legPartPoints2, legPartPoints3, -6.0f);
            }
                
            //foldParts(legPartPoints3, legPartPoints2, -1.0f);
        }
        else if (timeTicker > .4)
        {
            //timeTicker = -.1f;
            //Debug.Log("================================");
            //Debug.Log(timeTicker);
            timeTicker = 0f;
            //Debug.Log(timeTicker);
        }

        rotateOnePointAboutinputPoint(thwackterPoint1, 9.0f, thwackterPoint2);


        //now for the tricky part:  rotation!  just some basic triogonometry, right?
        //rotateMylegPartPointsAboutPoint(legPartPoints1, timeTicker, 0);
        //rotateMylegPartPointsAboutPoint(legPartPoints2, 0.5f, 0);

        //legPartPoints4[0] = legPartPoints4[0] + (legPartPoints2[1] - legPartPoints4[0]);

        //rotateMylegPartPointsAboutinputPoint(legPartPoints2, 1.0f, legpoint1);
        //rotateMylegPartPointsAboutinputPoint(legPartPoints2, 1.0f, legpoint1);
        //rotateMylegPartPointsAboutinputPoint(legPartPoints3, 1.0f, legpoint1);
        //rotateMylegPartPointsAboutinputPoint(legPartPoints4, 1.0f, legpoint1);

        //foldParts(legPartPoints2, legPartPoints3, 1.0f);

        List<List<Vector3>> setOfSegments = new List<List<Vector3>>();
        setOfSegments.Add(legPartPoints1);
        setOfSegments.Add(legPartPoints2);
        setOfSegments.Add(legPartPoints3);
        setOfSegments.Add(legPartPoints4);

        //rotationChainByKnees(setOfSegments, timeTicker);


        placeUsinglegPartPoints();
        updatePoints();
        myObjPutFunc(thwackter1, thwackterPoint1);
        myObjPutFunc(thwackter2, thwackterPoint2);
        //legPartPointsConsistencyFromTop();


        //lol just folding away all this junk code for now, and using an "if" statement lets me do that in visual studio
        if (timeTicker == 0)
            {
                //aboveKnee.transform.position = transform.position + new Vector3(aboveKnee.transform.position + movementSpeed, aboveKnee.transform.position + movementSpeed, 0);
                //aboveKnee.transform.position.x = aboveKnee.transform.position.x + movementSpeed;

                //aboveKnee.transform.position = aboveKnee.transform.position + new Vector3(aboveKnee.transform.position.x + movementSpeed, aboveKnee.transform.position.y + movementSpeed, 0);


                //causes constant translation movement:
                //aboveKnee.transform.position = aboveKnee.transform.position + new Vector3(movementSpeed, movementSpeed, 0);
                //Debug.Log(aboveKnee.transform.position.x);

                //should set position exactly:
                //aboveKnee.transform.position = startAK + new Vector3(movementSpeed, movementSpeed, 0);

                //makes it jerk in a direction every 30 frames:
                if (timeTicker > 30)
                {
                    //pAK = pAK + new Vector3(5*movementSpeed, 5*movementSpeed, 0);

                    //aboveKnee.transform.position = pAK;


                    //timeTicker = 0;

                    //and for rotation:
                    //rAK = rAK + new Quaternion(5*movementSpeed, 5*movementSpeed, 0, 0);

                    //aboveKnee.transform.rotation = rAK;


                    //timeTicker = 0;

                }


                // Smoothly tilts a transform towards a target rotation.
                float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
                float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;

                // Rotate the cube by converting the angles into a quaternion.
                Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

                // Dampen towards the target rotation
                //aboveKnee.transform.rotation = Quaternion.Slerp(aboveKnee.transform.rotation, target, Time.deltaTime * smooth);
            }

    }


    void myObjPutFunc(GameObject thisObj, Vector3 thisPosition)
    {
        thisObj.transform.position = thisPosition; 
    }

    

    void rotateMylegPartPointsAboutPoint(List<Vector3> thesePartPoints, float theAngle, int thisPosition)
    {
        //note, "theAngle" is how much it turns from one function call
        //thus, if it is called every fram with SAME "theAngle", it is like the angular velocity
        //[constant "theAngle" = constant speed of rotation]
        //"thisPosition" should just refer to one of the object positions, for now.

        //find CURRENT angle [could i just store this in the "legPartPoints"?



        //thesePartPoints[1] = thesePartPoints[1] + new Vector3(Mathf.Cos(theAngle)/70, Mathf.Sin(theAngle)/70, 0);

        Quaternion q = Quaternion.AngleAxis(theAngle, Vector3.fwd + thesePartPoints[0]);
        thesePartPoints[1] = q * thesePartPoints[1]; //Note: q must be first (point * q wouldn't compile)

        //Vector3 vectorToRotate = thesePartPoints[1] - thesePartPoints[0];
        //Vector3 vectorToRotateTowards = vectorToRotate + new Vector3(Mathf.Cos(theAngle) / 70, Mathf.Sin(theAngle) / 70, 0);
        //vectorToRotate = Vector3.RotateTowards(vectorToRotate, vectorToRotateTowards, 3f, 1f);

        //Vector3 vectorToRotate = thesePartPoints[1] + new Vector3(Mathf.Cos(theAngle) / 100, Mathf.Sin(theAngle) / 100, 0);

        //thesePartPoints[1] = vectorToRotate;

    }

    void rotateMylegPartPointsAboutinputPoint(List<Vector3> thesePartPoints, float theAngle, Vector3 thisInputPoint)
    {

        Quaternion q = Quaternion.AngleAxis(theAngle, thisInputPoint);
        thesePartPoints[1] = q * thesePartPoints[1]; //Note: q must be first (point * q wouldn't compile)
        thesePartPoints[0] = q * thesePartPoints[0];
    }

    void rotateOnePointAboutinputPoint(Vector3 thisPartPoint, float theAngle, Vector3 thisInputPoint)
    {

        Quaternion q = Quaternion.AngleAxis(theAngle, thisInputPoint);
        thisPartPoint = q * thisPartPoint; //Note: q must be first (point * q wouldn't compile)
        
    }

    void rotationChainByKnees(List<List<Vector3>> setOfSegments, float theAngle)
    {
        List<int> indexList = new List<int>();
        int index = 0;
        foreach (var segment in setOfSegments)
        {
            indexList.Add(index);
            index++;
        }
        

        

        foreach(var thisIndex in indexList)
        {
            //[subtract initial or something after at end....
            Vector3 translationalMovement = setOfSegments[thisIndex][0];

            //setOfSegments[thisIndex][1] = setOfSegments[thisIndex][1] + new Vector3(Mathf.Cos(30 * theAngle)/70, Mathf.Sin(30 * theAngle)/70, 0);

            rotateMylegPartPointsAboutPoint(setOfSegments[thisIndex], theAngle, 0);

            if (setOfSegments.Count > 1)
            {
                List<List<Vector3>> setOfSegments2 = new List<List<Vector3>>();
                foreach (var thisIndex2 in indexList)
                {
                    if(thisIndex2 > thisIndex)
                    {
                        setOfSegments2.Add(setOfSegments[thisIndex2]);
                    }
                    
                }
                //go recursive:
                rotationChainByKnees(setOfSegments2, theAngle);
            }
            

        }

    }

    void rotationChainSegments()
    {



    }


    void legPartPointsConsistencyFromTop()
    {
        //well, for TRANSLATION, just move all points in a leg part by the vector between
        //its origin, and the end point of the previous leg part
        //what to do for rotation?  well, they accumulate FIRST, probably, ya.  then just do that translation chain

        //legPartPoints2 = legPartPoints2 + legPartPoints1[1] - legPartPoints2[0];

        legPartPoints2[1] = legPartPoints2[1] + legPartPoints1[1] - legPartPoints2[0];
        legPartPoints3[1] = legPartPoints3[1] + legPartPoints2[1] - legPartPoints3[0];
        legPartPoints4[1] = legPartPoints4[1] + legPartPoints3[1] - legPartPoints4[0];

        rotateMylegPartPointsAboutPoint(legPartPoints3, 2, 0);


        //foreach(Vector3 thisPoint in legPartPoints2)
        {
            //thisPoint = thisPoint + legPartPoints1[1] - legPartPoints2[0];
        }

        //legPartPoints2[1] = legPartPoints2[0] + new Vector3(0, -legPartLength2, 0);
        //legPartPoints3[0] = legPartPoints2[1] + new Vector3(0, -legPartLength3, 0);
        //legPartPoints3[1] = legPartPoints3[0] + new Vector3(-legPartLength4, 0, 0);



        //legpoint1 = this.transform.position + new Vector3(0, 0, -1);
        //legpoint1 = new Vector3(legPartLength1, 0, 0);
        legPartPoints2[0] = legPartPoints1[1] + new Vector3(legPartLength1, 0, 0);
        legPartPoints2[1] = legPartPoints2[0] + new Vector3(0, -legPartLength2, 0);
        legPartPoints3[0] = legPartPoints2[1] + new Vector3(0, -legPartLength3, 0);
        legPartPoints3[1] = legPartPoints3[0] + new Vector3(-legPartLength4, 0, 0);

        //legPartPoints1.Add(legpoint1);
        //legPartPoints1.Add(legpoint2);

        //legPartPoints2[0]= legPartPoints1[1];

        //legPartPoints2[0] = legPartPoints1[1];

        //legPartPoints3[0] = legPartPoints2[1];

        //legPartPoints3[0] = legPartPoints1[1];

        //legPartPoints4[0] = legPartPoints3[1];

        //legPartPoints4[0] = legPartPoints1[1];
    }

    void placeUsinglegPartPoints()
    {

        myObjPutFunc(knee1, legPartPoints2[0]);
        myObjPutFunc(knee2, legPartPoints2[1]);
        myObjPutFunc(knee3, legPartPoints3[0]);
        myObjPutFunc(foot, legPartPoints3[1]);
    }

    void updatePoints()
    {
        //legPartPoints1.Add(legpoint1);
        //legPartPoints1.Add(legpoint2);
        //legPartPoints2.Add(legpoint2);
        //legPartPoints2.Add(legpoint3);
        //legPartPoints3.Add(legpoint3);
        //legPartPoints3.Add(legpoint4);
        //legPartPoints4.Add(legpoint4);
        //legPartPoints4.Add(legpoint5);
        legpoint1 = legPartPoints1[0];
        legpoint2 = legPartPoints1[1];
        legpoint3 = legPartPoints2[1];
        legpoint4 = legPartPoints3[1];
        legpoint5 = legPartPoints4[1];

    }


    void foldParts(List<Vector3> segment1, List<Vector3> segment2, float theAngle)
    {
        //rotates first segment:
        rotateMylegPartPointsAboutinputPoint(segment1, -theAngle, segment1[0]);

        //ensures 2nd segment stays "attached" to same endpoint of 1st segment:
        rotateMylegPartPointsAboutinputPoint(segment2, -theAngle, segment1[0]);

        //rotates 2nd segment in opposite direction:
        rotateMylegPartPointsAboutinputPoint(segment2, theAngle, segment2[0]);
    }


    Vector3 theGroundPointThatIsDown(Vector3 downFromWhere)
    {
        RaycastHit myHit;
        //Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Ray myRay = legpoint1 - Vector3.up;
        Ray myRay = new Ray();
        myRay.direction = this.transform.position + Vector3.down;

        if (Physics.Raycast(myRay, out myHit, 33.0f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (myHit.transform != null)
            {
                //Debug.Log(myHit.transform.gameObject);
                //clickedOn = myHit.transform.gameObject;
                //new Vector3(myHit.transform.px, myHit.transform.y, myHit.transform.z);
                return myHit.transform.position;
            }
        }

        //uhhh, if it fails....just return starting point for now....should change to be "max distance" but whatever
        return downFromWhere;
    }



}
