using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static equippable2Setup;

public class equippable2Setup : MonoBehaviour
{
    public static equippable2Setup singleton;


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
