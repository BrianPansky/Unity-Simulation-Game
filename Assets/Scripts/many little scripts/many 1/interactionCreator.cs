using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionCreator : MonoBehaviour
{

    public static interactionCreator singleton;


    //public enum interType

    public enum simpleSlot
    {
        //XYZ Controller buttons,
        errorYouDidntSetEnumTypeForSIMPLESLOT,
        storage,
        seat,
        hands,
        liquidHolder,
        topOfHead,
        face,
        torso

    }


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

    public void dockXToY(GameObject objectX, GameObject objectY, Vector3 offset = new Vector3())
    {
        Debug.Log("should dock??");
        //really, i shouldn't need an oddset.  i should just have "docking" ports positioned correctly on objects....
        objectX.transform.position = objectY.transform.position;



        //theEnactionScript.thisNavMeshAgent = theTank.thisNavMeshAgent;
        //      theTank.pilot = theAuthorScript.enacting.enactionAuthor;
        //      theTank.thePilotEnactionScript = theEnactionScript;

        //this is probably great, but it also disables my camera.  will need to re-arrange things...
        //      theBodyScript.theBodyGameObject.active = false;

        //this.gameObject.transform.SetParent(theAuthorScript.theAuthor.transform, true);
        objectX.gameObject.transform.SetParent(objectY.transform, true);

        objectX.transform.localPosition += offset;

        objectX.transform.rotation = objectY.transform.rotation;
    }

    public void dockToThisObject(GameObject theObjectWeWantToDockToTHISObject, Vector3 offset = new Vector3())
    {
        theObjectWeWantToDockToTHISObject.transform.position = this.transform.position;



        //theEnactionScript.thisNavMeshAgent = theTank.thisNavMeshAgent;
        //      theTank.pilot = theAuthorScript.enacting.enactionAuthor;
        //      theTank.thePilotEnactionScript = theEnactionScript;

        //this is probably great, but it also disables my camera.  will need to re-arrange things...
        //      theBodyScript.theBodyGameObject.active = false;

        //this.gameObject.transform.SetParent(theAuthorScript.theAuthor.transform, true);
        theObjectWeWantToDockToTHISObject.transform.SetParent(this.gameObject.transform, true);

        theObjectWeWantToDockToTHISObject.transform.localPosition += offset;

        theObjectWeWantToDockToTHISObject.transform.rotation = this.transform.rotation;
    }
    public void dockThisToOtherObject(GameObject theObjectWeWantToDockTHISObjectTo, Vector3 offset = new Vector3())
    {
        this.transform.position = theObjectWeWantToDockTHISObjectTo.transform.position;



        //theEnactionScript.thisNavMeshAgent = theTank.thisNavMeshAgent;
        //      theTank.pilot = theAuthorScript.enacting.enactionAuthor;
        //      theTank.thePilotEnactionScript = theEnactionScript;

        //this is probably great, but it also disables my camera.  will need to re-arrange things...
        //      theBodyScript.theBodyGameObject.active = false;

        //this.gameObject.transform.SetParent(theAuthorScript.theAuthor.transform, true);
        this.gameObject.transform.SetParent(theObjectWeWantToDockTHISObjectTo.transform, true);

        this.transform.localPosition += offset;

        this.transform.rotation = theObjectWeWantToDockTHISObjectTo.transform.rotation;
    }
    
    public void makeThisObjectDisappear(GameObject theObjectWeWantToDisappear)
    {
        Renderer theRenderer = theObjectWeWantToDisappear.GetComponent<Renderer>();
        theRenderer.enabled = false;
    }
    public void makeThisObjectAppear(GameObject theObjectWeWantToAppear)
    {
        Renderer theRenderer = theObjectWeWantToAppear.GetComponent<Renderer>();
        theRenderer.enabled = true;
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
