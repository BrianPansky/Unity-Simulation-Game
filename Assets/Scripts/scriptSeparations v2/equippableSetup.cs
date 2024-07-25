using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static equippable2Setup;

public class equippable2Setup : MonoBehaviour
{


    public static equippable2Setup singleton;

    /*

    //uninformative?
    public enum buttonCategories
    {
        errorYouDidntSetEnumTypeForBUTTONCATEGORIES,
        primary,
        aux1,
        vector1,
        vector2,
        augment1
    }

    //need THIS instead/as well?
    public enum INFORMATIVEbuttonCategories
    {
        errorYouDidntSetEnumTypeForINFORMATIVEbuttonCategories,
        fire1,
        jump,
        move1,
    }

    */

    void Awake()
    {
        //Debug.Log("Awake:  " + this);
        singletonify();

    }
    void singletonify()
    {
        if (singleton != null && singleton != this)
        {
            Debug.Log("this class is supposed to be a singleton, you should not be making another instance, destroying the new one");
            Destroy(this);
            return;
        }
        singleton = this;
    }





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



/*
public abstract class gamePadButtonable
{
    //      public equippable2Setup.buttonCategories gamepadButtonType { get; set; }
    //public IEnactaBool theEnaction;

    //      public abstract void plugIntoController(virtualGamepad gamepad);
}

*/

