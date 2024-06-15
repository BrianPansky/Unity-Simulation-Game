using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conditionCreator : MonoBehaviour
{

    public static conditionCreator singleton;


    //public enum interType


    void Awake()
    {
        singletonify();

    }

    void singletonify()
    {
        if (singleton != null && singleton != this)
        {
            Debug.Log("this class is supposed to be a singleton, you should not be making another instance, destroying the new one");
            Destroy(this);
            return;
        }
        singleton = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }



    ////////////////////    CONDITIONS  ///////////////////

    //proximity, time....what else.....various states?

    public bool isXCloserThanYToZ(GameObject objectX, GameObject objectY, GameObject objectZ)
    {
        float distanceToX = distanceBetween(objectX, objectZ);

        float distanceToY = distanceBetween(objectY, objectZ);


        if (distanceToX > distanceToY)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public float distanceBetween(GameObject object1, GameObject object2)
    {
        Vector3 theVectorBetweenXandZ = object1.transform.position - object2.transform.position;
        return theVectorBetweenXandZ.sqrMagnitude;
    }

    public float horizontalDistanceBetween(GameObject object1, GameObject object2)
    {
        Vector3 v1 = object1.transform.position;
        Vector3 v2 = object2.transform.position;
        //Vector3 theVectorBetweenXandZ = object1.transform.position - object2.transform.position;
        Vector3 theHorizontalVectorBetweenXandZ = new Vector3(v1.x - v2.x, 0, v1.z - v2.z);
        return theHorizontalVectorBetweenXandZ.sqrMagnitude;
    }


    public Quaternion rotationFromLookingVector()
    {

        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        return Quaternion.identity;
    }






}
