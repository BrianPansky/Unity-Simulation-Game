using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;


public class shotgun1 : equippable2
{

    //public cooldown theCooldown;

    //can't have this, because it prevents the "awake" function from being called in "equippable2"?!?  -_____-
    //void Awake()
    //{
    //}
    public void Awake()
    {
        this.GetComponent<equippable2>().Awake();  //lol does THIS work?

        //Physics.DefaultRaycastLayers;
    }


    // Start is called before the first frame update
    void Start()
    {
        initializeStandardEnactionPoint1(this, 0.7f,0.3f);

        genGen.singleton.standardGun(this);

        //theCooldown = this.GetComponent<projectileLauncher>().theCooldown;
    }




    void Update()
    {
        doCooldown();
        //theCooldown.cooling();
    }
}

