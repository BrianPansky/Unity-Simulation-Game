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

    public static void genGrowScript1(GameObject theObject, float inputGrowthSpeed = 0f)//, IEnactaBool enactingThis)
    {
        //use like THIS:
        //growScript1.genGrowScript1(x, y, z);


        //growScript.

        if (inputGrowthSpeed == 0f) { return; }

        //wait what?  what is this line???  how does it not mess up the starting position of every projectile i make???
        theObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        growScript1 growScript = theObject.AddComponent<growScript1>();
        growScript.growthSpeed = inputGrowthSpeed;


    }
    
    public static void OLDgenGrowScript1(GameObject theObject, intSpherAtor interactionSphereCreator)//, IEnactaBool enactingThis)
    {
        //use like THIS:
        //growScript1.genGrowScript1(x, y, z);


        //growScript.

        if (interactionSphereCreator.growthSpeed > 0f)
        {
            //wait what?  what is this line???  how does it not mess up the starting position of every projectile i make???
            theObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            growScript1 growScript = theObject.AddComponent<growScript1>();
            growScript.growthSpeed = interactionSphereCreator.growthSpeed;
        }


    }

    // Update is called once per frame
    void Update()
    {
        //this.gameObject.transform.position = this.gameObject.transform.position + Direction * speed;
        //thisObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        this.gameObject.transform.localScale += new Vector3(growthSpeed, growthSpeed, growthSpeed);
    }
}
