using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comboGen : MonoBehaviour
{
    //for making partially pre-defined combinatios of things more easily

    public static comboGen singleton;

    void Awake()
    {
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








}
