using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class worldScript : MonoBehaviour
{
    public static worldScript singleton;

    public bool debugToggle = false;

    int numberOfNearZones = 9;


    //List<List<IupdateCallable>> zoneListOfCallables = new List<List<IupdateCallable>>();

    List<List<IupdateCallable>> nearZones = new List<List<IupdateCallable>>();
    List<List<IupdateCallable>> farZones = new List<List<IupdateCallable>>();
    int currentFarZone = 0;

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
        new helpINeedRotation().doTesting();
    }

    // Update is called once per frame
    void Update()
    {
        //tagging2.singleton.addTag(this.gameObject, tagging2.tag2.interactable);


        //Debug.Log("======================================================");

        updateZoneLists();

        //Debug.Log("222222222222222222222222 zoneList.Count:  " + nearZones.Count);
        //Debug.Log("333333333333333333333333 zoneList.Count:  " + farZones.Count);
        updateNearZones(nearZones);
        updateFarZones(farZones);

        updateWhichFarZoneWillBeCurrent();
    }






    private void updateWhichFarZoneWillBeCurrent()
    {
        currentFarZone++;
        if (currentFarZone < farZones.Count) { return; }
        currentFarZone = 0;
    }

    private void updateZoneLists()
    {
        int whichZoneWeAreLookingAt = 0;
        nearZones.Clear();
        farZones.Clear();

        //Debug.Log("000000000000000000");

        while (whichZoneWeAreLookingAt < tagging2.singleton.objectsInZone.Keys.Count)
        {

            //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            List<IupdateCallable> alltheCallablesInThisZone = getAllCallablesInThisZone(whichZoneWeAreLookingAt);
            //Debug.Log("alltheCallablesInThisZone.Count:  " + alltheCallablesInThisZone.Count);

            if (whichZoneWeAreLookingAt < numberOfNearZones)
            {
                //Debug.Log("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");
                nearZones.Add(alltheCallablesInThisZone);
            }
            else
            {
                //Debug.Log("cccccccccccccccccccccccccccccccccccccc");
                farZones.Add(alltheCallablesInThisZone);
            }

            whichZoneWeAreLookingAt++;
        }


        //Debug.Log("yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy zoneList.Count:  " + nearZones.Count);
        //Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz zoneList.Count:  " + farZones.Count);
    }

    private List<IupdateCallable> getAllCallablesInThisZone(int whichZoneWeAreLookingAt)
    {

        //Debug.Log("1111111111111111111111111");
        List < IupdateCallable > newList = new List<IupdateCallable> ();

        foreach(objectIdPair thisPair in tagging2.singleton.objectsInZone[whichZoneWeAreLookingAt])
        {
            if(thisPair == null)
            {
                Debug.Log("idPair is null");
            }
            if (thisPair.theObject == null)
            {
                Debug.Log("the object is null");
                Debug.Log("id number:  " + thisPair.theObjectIdNumber);


                Debug.Log("zzzzzzzzzso....zone _______with id________ contains pair with id ________ for object with id________:  "
                    + whichZoneWeAreLookingAt+ "zone id:  "+ whichZoneWeAreLookingAt.GetHashCode()
                    + "contains pair (boolean):  " + tagging2.singleton.objectsInZone[whichZoneWeAreLookingAt].Contains(thisPair)
                    + "pair id:  " + thisPair.GetHashCode() + "object id:  " + thisPair.theObjectIdNumber);
            }
            IupdateCallable thisCallable = thisPair.theObject.GetComponent<IupdateCallable>();
            //Debug.Log(thisCallable);
            if(thisCallable == null) { continue; }
            newList.Add(thisCallable);
            thisCallable.currentUpdateList = newList;
        }

        return newList;
    }

    private void updateNearZones(List<List<IupdateCallable>> setOfZones)
    {
        if (setOfZones.Count == 0)
        {
            //Debug.Log("setOfZones.Count == 0");
            return; }
        foreach (List<IupdateCallable> zone in setOfZones)
        {
            //Debug.Log("callAllonOneZoneList(zone);:");
            callAllonOneZoneList(zone);
        }
    }

    private void updateFarZones(List<List<IupdateCallable>> setOfZones)
    {
        if(setOfZones.Count == 0) { return; }
        callAllonOneZoneList(setOfZones[currentFarZone]);
    }

    private void callAllonOneZoneList(List<IupdateCallable> zoneList)
    {

        //Debug.Log("calllllllllllllllllllllllllllllllllllllll");

            //Debug.Log("zoneList.Count:  " + zoneList.Count);
            foreach (IupdateCallable callable in zoneList)
        {
            //Debug.Log(callable);
            callable.callableUpdate();
        }
    }


    public void removeIupdateCallableFromItsList(IupdateCallable theCallable)
    {
        //when object is destroyed
        //public List<IupdateCallable> currentUpdateList { get; set; }

        theCallable.currentUpdateList.Remove(theCallable);

    }

}

public interface IupdateCallable
{
    //easy way to remove from world script when object is destroyed:
    List<IupdateCallable> currentUpdateList { get; set; }

    void callableUpdate();
}

public class helpINeedRotation
{
    public void doTesting()
    {
        Vector3 inputVector = new Vector3(1,1,0);
        Vector3 forward = new Vector3(1, 0, 0);
        Vector3 upAxis = new Vector3(0, 1, 0);
        Vector3 v4 = new Vector3(1, 0.8f, 0);
        Vector3 v5 = new Vector3(1, 0.2f, 0);
        Vector3 v6 = new Vector3(0, 0, 1);
        Vector3 v7 = new Vector3(1, 0, 1);

        float angle = AngleOffAroundAxis(inputVector.normalized,forward,upAxis);

        //Debug.Log("angle:  " + angle);
        //um, that's returning 0 degrees, just like it's precisely supposed to be designed NOT to do.....
        //oh, no, wait,  that IS precisely what it is designed to do.  ok, ya, good.  next


        angle = AngleOffAroundAxis(v4.normalized, forward, upAxis);

        //Debug.Log("angle:  " + angle);

        angle = AngleOffAroundAxis(v6.normalized, forward, upAxis);

        //Debug.Log("angle:  " + angle);
        
        angle = AngleOffAroundAxis(v7.normalized, forward, upAxis);

        //Debug.Log("angleFromForwardToInputAroundUpAxis:  " + angle);


        //ok, all works as it should.  soooo...

    }

    public float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis, bool clockwise = false)
    {
        //from here:
        //https://forum.unity.com/threads/is-vector3-signedangle-working-as-intended.694105/

        //but had to change conversion thing from "MathUtil.RAD_TO_DEG" to the following:
        //Mathf.Rad2Deg


        Vector3 right;
        if (clockwise)
        {
            right = Vector3.Cross(forward, axis);
            forward = Vector3.Cross(axis, right);
        }
        else
        {
            right = Vector3.Cross(axis, forward);
            forward = Vector3.Cross(right, axis);
        }


        return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
    }



}