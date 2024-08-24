using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;


public class gun1 : equippable2
{

    public cooldown theCooldown;

    //public GameObject enactionPoint1;
    /*
    void Awake()
    {
        //          callableAwake();
        //needs to be in awake, otherwise noy properly initialized if it is put in inventory [and thus disabled] immediately upon its creation...
        theEquippable2Type = interactionCreator.simpleSlot.hands;
    }
    */

    //can't have this, because it prevents the "awake" function from being called in "equippable2"?!?  -_____-
    public void Awake()
    {
        //equippable2.Awake();  //why doesn't this work
        //equippable2.test();  //ya, doesn't work
        test();  //well, no error.  which is how i did "callableAwake" before someone on reddit said i could just do "equippable2.Awake()"
        this.GetComponent<equippable2>().Awake();


        initializeEnactionPoint1();




        //this.gameObject.AddComponent<projectileLauncher>();
        projectileLauncher.addProjectileLauncher(this.gameObject, enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.shoot1),
            new projectileToGenerate(1, true, 99, 0), 30);

        theCooldown = this.GetComponent<projectileLauncher>().theCooldown;

    }


    // Start is called before the first frame update
    void Start()
    {
        /*
        projectileLauncher x = new projectileLauncher(enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.shoot1),
            new projectileToGenerate(1, true, 99, 0));
        this.gameObject.AddComponent(x);
        */
        /*
        enactableBoolSet.Add(new projectileLauncher(enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.shoot1),
            new projectileToGenerate(1, true, 99, 0)));
        */

        //Debug.Log(this.transform);

        //Vector3 thisBit = (this.gameObject.transform.position);
        //Color whatColor = Color.magenta;
        //Debug.DrawLine(new Vector3(), thisBit, whatColor, 22f);

        //Vector3 thisBit = (enactionPoint1.transform.position);
        //Color whatColor = Color.magenta;
        //Debug.DrawLine(new Vector3(), thisBit, whatColor, 22f);

    }

    void initializeEnactionPoint1()
    {
        enactionPoint1 = new GameObject("enactionPoint1 in initializeEnactionPoint1() line 58, gun1 script");
        enactionPoint1.transform.parent = transform;
        enactionPoint1.transform.position = this.transform.position + this.transform.forward * 0.2f + this.transform.up * 0.3f;
        enactionPoint1.transform.rotation = this.transform.rotation;

    }


    void Update()
    {
        theCooldown.cooling();
    }

}
