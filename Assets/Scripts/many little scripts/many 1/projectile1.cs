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
        speed = 0.11f;
        Direction = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = this.gameObject.transform.position + Direction*speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //technically, this is an interaction.  but whatever?  just put it here.

        if (selfDestructOnCollision)
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
