using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repositoryOfScriptsAndPrefabs : MonoBehaviour
{
    //attach this to world object, allow any other game objects to access it through there

    //OBJECTS:
    public GameObject npc2Prefab;
    public GameObject placeHolderCubePrefab;
    public GameObject invisibleCubePrefab;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;

    //SCRIPTS:
    public patternScript2 patterns;
    public selfDestructScript1 theSelfDestructScript;
    public interactive1 theInteractive1Script;

    //public premadeStuffForAI stateGrabber;
    //public AI1 theHub;

    void Awake()
    {


        //MAYBE JUNK:
        //stateGrabber = GetComponent<premadeStuffForAI>();
        //theHub = GetComponent<AI1>();
        // Start is called before the first frame update
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void createPrefabAtPoint(GameObject thePrefab, Vector3 thePoint)
    {
        Instantiate(thePrefab, thePoint, Quaternion.identity);
    }

    public GameObject createAndReturnPrefabAtPoint(GameObject thePrefab, Vector3 thePoint)
    {
        return Instantiate(thePrefab, thePoint, Quaternion.identity);
    }


    public void placeObjectsOnLinePattern(List<GameObject> theObjects, List<Vector3> theLinePattern)
    {
        int indexPosition = 0;

        foreach (GameObject thisObject in theObjects)
        {
            thisObject.transform.position = theLinePattern[indexPosition];
            indexPosition += 1;
        }
    }



}
