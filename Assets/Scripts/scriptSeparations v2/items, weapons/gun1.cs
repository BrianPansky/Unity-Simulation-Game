using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static virtualGamepad;

public class gun1 : equippable
{



    public GameObject enactionPoint1;





    public interactionScript theInteractionScript;



    // Start is called before the first frame update
    void Start()
    {
        initializeEnactionPoint1();


        if (theInteractionScript == null)
        {
            theInteractionScript = this.gameObject.AddComponent<interactionScript>();
            theInteractionScript.dictOfInteractions = new Dictionary<interType, List<interactionScript.effect>>();//new Dictionary<string, List<string>>(); //for some reason it was saying it already had that key in it, but it should be blank.  so MAKING it blank.
        }


        List < interactionScript.effect >  thing = new List<interactionScript.effect>();
        thing.Add(interactionScript.effect.equip);

        theInteractionScript.dictOfInteractions[interType.standardClick] = thing;


        theEquippableType = interactionCreator.simpleSlot.hands;

        enactableBoolSet.Add(new projectileLauncher(enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.shoot1),
            new projectileInfo(1, true, 99, 0)));

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
        enactionPoint1.transform.position = this.transform.position + this.transform.forward * 0.2f + this.transform.up * 0.4f;
        enactionPoint1.transform.rotation = this.transform.rotation;

    }

}
