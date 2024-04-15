using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestructScript1 : MonoBehaviour
{
    public int delay = 1;
    // Start is called before the first frame update
    void Start()
    {
        delay = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (delay == 0)
        {
            //this.gameObject.transform.position = new Vector3(0, -500, 0);
            //Destroy(this.gameObject);
        }
        if (delay == -33)
        {
            Destroy(this.gameObject);
        }
        delay -= 1;
    }

}
