using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planningAndImagination : MonoBehaviour
{
    //keep in mind that planning is only about causality/logic.  which things CAN be done, in which order, to get an outcome
    //the EVALUATION of which plan is best [or even "good enough"] has to happen elsewhere

    public List<planEXE> plan = new List<planEXE>();


}

public interface planEXE
{
    void executePlan();
    void target(GameObject theTarget);

}


public class boolEXE :  planEXE
{
    IEnactaBool theEnaction;
    List<planEXE> microPlan = new List<planEXE>();

    public boolEXE(IEnactaBool theEnaction, GameObject theTarget)
    {
        this.theEnaction = theEnaction;
    }

    public void executePlan()
    {
        foreach (var planEXE in microPlan)
        {
            planEXE.executePlan();
        }

        theEnaction.enact();
    }

    public void target(GameObject theTarget)
    {

    }

}
public class vectEXE : planEXE
{

    IEnactByTargetVector theEnaction;
    GameObject theTarget;

    public vectEXE(IEnactByTargetVector theEnaction, GameObject theTarget)
    {
        this.theEnaction = theEnaction;
        this.theTarget = theTarget;
    }


    public void executePlan()
    {
        theEnaction.enact(theTarget.transform.position);
    }

    public void target(GameObject theTarget)
    {
        this.theTarget = theTarget;
    }
}