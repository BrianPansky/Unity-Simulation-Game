using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planningAndImagination : MonoBehaviour
{

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
    private IEnactaBool testE1;

    public boolEXE(IEnactaBool theEnaction)
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

    public void executePlan()
    {
        theEnaction.enact(theTarget.transform.position);
    }

    public void target(GameObject theTarget)
    {
        this.theTarget = theTarget;
    }
}