using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class growScript1 : MonoBehaviour
{

    public float growthSpeed = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.gameObject.transform.position = this.gameObject.transform.position + Direction * speed;
        //thisObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        this.gameObject.transform.localScale += new Vector3(growthSpeed, growthSpeed, growthSpeed);
    }
}
