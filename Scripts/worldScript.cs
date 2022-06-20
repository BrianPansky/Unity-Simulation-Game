using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldScript : MonoBehaviour
{

    public Dictionary<string, List<GameObject>> taggedStuff = new Dictionary<string, List<GameObject>>();

    public int theTime;

    // Start is called before the first frame update
    void Start()
    {
        theTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeIncrement();
        //Debug.Log("===========================================================");

        if (taggedStuff.ContainsKey("store"))
        {
            foreach(GameObject item in taggedStuff["store"])
            {
                Debug.Log(item.name);
            }
        }

        
    }

    public void timeIncrement()
    {
        theTime += 1;
        //Debug.Log(theTime);
    }
}
