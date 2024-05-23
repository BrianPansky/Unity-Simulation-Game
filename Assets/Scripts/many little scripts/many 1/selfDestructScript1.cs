using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestructScript1 : MonoBehaviour
{
    public int currentCounter = 1;

    public int timeUntilSelfDestruct = 100;
    // Start is called before the first frame update
    void Start()
    {
        //delay = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCounter == 0)
        {
            //this.gameObject.transform.position = new Vector3(0, -500, 0);
            //Destroy(this.gameObject);
        }
        if (currentCounter > timeUntilSelfDestruct)
        {
            //Debug.Log("selfDestructScript1, currentCounter > timeUntilSelfDestruct, this.gameObject.name:  " + currentCounter +" > "+ timeUntilSelfDestruct + ", name: " + this.gameObject.name);

            Destroy(this.gameObject);
        }
        currentCounter += 1;
    }

}
