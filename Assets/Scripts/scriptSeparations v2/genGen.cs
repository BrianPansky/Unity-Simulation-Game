using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genGen : MonoBehaviour
{
    //[general generator]
    public static genGen singleton;

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



    public GameObject createPrefabAtPointAndRETURN(GameObject thePrefab, Vector3 thePoint)
    {
        //GameObject newBuilding = new GameObject();
        //newBuilding = Instantiate(thePrefab, thePoint, Quaternion.identity);
        //return Instantiate(thePrefab, thePoint, Quaternion.identity);

        //just so i can keep the rotation of the object i input, for now:
        return Instantiate(thePrefab, thePoint, thePrefab.transform.rotation);
    }





    public GameObject returnSimpleTank1(Vector3 where)
    {

        GameObject bottomBit = Instantiate(repository2.singleton.simpleTankBottom, where, Quaternion.identity);

        //bottomBit.AddComponent<NavMeshAgent>();
        //bottomBit.AddComponent<AIHub2>();


        //  bottomBit.AddComponent<interactionScript>();
        //  interactionScript theInteractionScript = bottomBit.GetComponent<interactionScript>();
        //  theInteractionScript.addInteraction("standardClick", "useVehicle");

        //genGen.singleton.rigid(bottomBit);

        //      DELETED tank1!!!!!!!!!!!!!!!!         tank1 theTank1Script = bottomBit.GetComponent<tank1>();

        //theTank1Script.tankHead = repository2.singleton.simpleTankTurretWITHOUTBarrel; 
        //"Setting the parent of a transform which resides in a Prefab Asset is disabled to prevent data corruption (GameObject: 'simple tank turret without barrel')."
        //hmmm...
        //Debug.Log("genGen:  " + genGen);

        //      DELETED tank1!!!!!!!!!!!!!!!!         theTank1Script.tankHead = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.simpleTankTurretWITHOUTBarrel, where);
        //      DELETED tank1!!!!!!!!!!!!!!!!         theTank1Script.tankHead.transform.SetParent(bottomBit.transform, false);

        //theTank1Script.tankBarrel = repository2.singleton.simpleTankBarrel;
        //      DELETED tank1!!!!!!!!!!!!!!!!         theTank1Script.tankBarrel = genGen.singleton.createPrefabAtPointAndRETURN(repository2.singleton.simpleTankBarrel, where);
        //      DELETED tank1!!!!!!!!!!!!!!!!         theTank1Script.tankBarrel.transform.SetParent(theTank1Script.tankHead.transform, false);


        //      DELETED tank1!!!!!!!!!!!!!!!!         theTank1Script.tankHead.transform.localPosition += new Vector3(0, 1.5f, 0);
        //      DELETED tank1!!!!!!!!!!!!!!!!         theTank1Script.tankBarrel.transform.localPosition += new Vector3(0, 3.4f, 1.5f);

        return bottomBit;
    }

    public GameObject returnPineTree1(Vector3 where)
    {
        GameObject newObj = Instantiate(repository2.singleton.pineTree1, where, Quaternion.identity);


        newObj.AddComponent<interactionScript>();
        interactionScript theInteractionScript = newObj.GetComponent<interactionScript>();

        theInteractionScript.addInteraction(enactionCreator.interType.shootFlamethrower1, interactionScript.effect.burn);

        return newObj;
    }



}
