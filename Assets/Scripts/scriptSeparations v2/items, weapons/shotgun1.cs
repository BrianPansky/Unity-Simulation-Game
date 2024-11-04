using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;


public class shotgun1 : equippable2
{

    public cooldown theCooldown;

    //can't have this, because it prevents the "awake" function from being called in "equippable2"?!?  -_____-
    //void Awake()
    //{
    //}
    public void Awake()
    {
        test();  //well, no error.  which is how i did "callableAwake" before someone on reddit said i could just do "equippable2.Awake()"
        this.GetComponent<equippable2>().Awake();

        //Physics.DefaultRaycastLayers;
    }


    // Start is called before the first frame update
    void Start()
    {
        initializeEnactionPoint1();


        projectileLauncher.addProjectileLauncher(this.gameObject, enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.shoot1),
            new projectileToGenerate(1, true, 99, 0), 20);


        theCooldown = this.GetComponent<projectileLauncher>().theCooldown;
    }

    void initializeEnactionPoint1()
    {
        enactionPoint1 = new GameObject("enactionPoint1 in initializeEnactionPoint1() line 52, shotgun1 script");
        enactionPoint1.transform.parent = transform;
        enactionPoint1.transform.position = this.transform.position + this.transform.forward * 0.7f + this.transform.up * 0.3f;
        enactionPoint1.transform.rotation = this.transform.rotation;

    }



    void Update()
    {
        theCooldown.cooling();
    }


}

