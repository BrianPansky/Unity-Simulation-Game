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


    int numberOfNearZones = 9;

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
    }

    // Update is called once per frame
    void Update()
    {
        //tagging2.singleton.addTag(this.gameObject, tagging2.tag2.interactable);


        //Debug.Log("======================================================");

        updateZoneLists();
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
            List<IupdateCallable> alltheCallablesInThisZone = getAllCallablesInThisZone(whichZoneWeAreLookingAt);

            if(whichZoneWeAreLookingAt < numberOfNearZones)
            {
                nearZones.Add(alltheCallablesInThisZone);
            }
            else
            {
                farZones.Add(alltheCallablesInThisZone);
            }

            whichZoneWeAreLookingAt++;
        }
    }

    private List<IupdateCallable> getAllCallablesInThisZone(int whichZoneWeAreLookingAt)
    {

        //Debug.Log("1111111111111111111111111");
        List < IupdateCallable > newList = new List<IupdateCallable> ();

        foreach(objectIdPair thisPair in tagging2.singleton.objectsInZone[whichZoneWeAreLookingAt])
        {
            IupdateCallable thisCallable = thisPair.theObject.GetComponent<IupdateCallable>();
            //Debug.Log(thisCallable);
            if(thisCallable == null) { continue; }
            newList.Add(thisCallable);
        }

        return newList;
    }

    private void updateNearZones(List<List<IupdateCallable>> setOfZones)
    {
        if (setOfZones.Count == 0) { return; }
        foreach (List<IupdateCallable> zone in setOfZones)
        {
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
        foreach (IupdateCallable callable in zoneList)
        {
            //Debug.Log(callable);
            callable.callableUpdate();
        }
    }

}

public interface IupdateCallable
{


    void callableUpdate();
}