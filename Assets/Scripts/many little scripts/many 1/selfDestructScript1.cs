﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestructScript1 : MonoBehaviour
{
    int delay = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (delay == 0)
        {
            Destroy(this.gameObject);
        }
        delay -= 1;
    }

}