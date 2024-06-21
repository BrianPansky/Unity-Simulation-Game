using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionCreator : MonoBehaviour
{

    public static interactionCreator singleton;


    //public enum interType


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



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class interactionInfo
{
    public GameObject enactionAuthor { get; set; }
    public enactionCreator.interType interactionType { get; set; }
    public float magnitudeOfInteraction = 1f;

    public interactionInfo(enactionCreator.interType interactionType, float magnitudeOfInteraction = 1f)
    {
        this.interactionType = interactionType;
        this.magnitudeOfInteraction = magnitudeOfInteraction;
    }
}
