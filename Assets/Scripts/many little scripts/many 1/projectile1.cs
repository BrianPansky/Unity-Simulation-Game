using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile1 : MonoBehaviour
{

    public float speed = 0f;
    public Vector3 Direction = new Vector3(0,0,0);
    public bool selfDestructOnCollision = true;

    void Awake()
    {
        //float speed = 0.11f;
        float speed;
        //Vector3 Direction = new Vector3(0, 0, 1);
        Vector3 Direction;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = this.gameObject.transform.position + Direction*speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //technically, this is an interaction.  but whatever?  just put it here.



        if (selfDestructOnCollision && other.gameObject.tag != "aMapZone")
        {
            Destroy(this.gameObject);

            //if (delay == 0)
            {
                //Destroy(this.gameObject);
            }
            //delay -= 1;
        }
    }


}
