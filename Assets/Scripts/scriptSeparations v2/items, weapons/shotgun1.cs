using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;


public class shotgun1 : equippable2
{

    //public GameObject enactionPoint1;


    //can't have this, because it prevents the "awake" function from being called in "equippable2"?!?  -_____-
    //void Awake()
    //{
    //}
    public void Awake()
    {
        //equippable2.Awake();  //why doesn't this work
        //equippable2.test();  //ya, doesn't work
        test();  //well, no error.  which is how i did "callableAwake" before someone on reddit said i could just do "equippable2.Awake()"
        this.GetComponent<equippable2>().Awake();
    }


        // Start is called before the first frame update
        void Start()
    {
        initializeEnactionPoint1();







        //theEquippable2Type = interactionCreator.simpleSlot.hands;
        projectileLauncher.addProjectileLauncher(this.gameObject, enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.shoot1),
            new projectileToGenerate(1, true, 99, 0));
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
        enactionPoint1 = new GameObject("enactionPoint1 in initializeEnactionPoint1() line 52, shotgun1 script");
        enactionPoint1.transform.parent = transform;
        enactionPoint1.transform.position = this.transform.position + this.transform.forward * 0.7f + this.transform.up * 0.3f;
        enactionPoint1.transform.rotation = this.transform.rotation;

    }
}

