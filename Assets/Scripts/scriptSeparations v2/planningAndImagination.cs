using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planningAndImagination : MonoBehaviour
{
    //keep in mind that planning is only about causality/logic.  which things CAN be done, in which order, to get an outcome
    //the EVALUATION of which plan is best [or even "good enough"] has to happen elsewhere

    public List<planEXE> plan = new List<planEXE>();


}

public abstract class planEXE
{
    public List<condition> startConditions = new List<condition>();
    public List<condition> endConditions = new List<condition>();

    public abstract void executePlan();
    public abstract void target(GameObject theTarget);

    public bool areSTARTconditionsFulfilled()
    {
        foreach (condition thisCondition in startConditions)
        {
            if (thisCondition.met() == false) { return false; }
        }
        return true;
    }

    public bool areENDconditionsFulfilled()
    {
        //Debug.Log("areENDconditionsFulfilled?");
        //Debug.Log("endConditions.Count:  " + endConditions.Count);
        foreach (condition thisCondition in endConditions)
        {
            //Debug.Log("thisCondition:  " + thisCondition);
            if (thisCondition.met() == false) { return false; }
        }
        //Debug.Log("no conditions remain unfulfilled!");
        return true;
    }

}


public class boolEXE :  planEXE
{
    IEnactaBool theEnaction;
    List<planEXE> microPlan = new List<planEXE>();

    public boolEXE(IEnactaBool theEnaction, GameObject theTarget)
    {
        this.theEnaction = theEnaction;
    }

    override public void executePlan()
    {
        foreach (var planEXE in microPlan)
        {
            planEXE.executePlan();
        }

        theEnaction.enact();
    }

    override public void target(GameObject theTarget)
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


    override public void executePlan()
    {

        //Debug.Log("vectEXE, theTarget:  " + theTarget);
        //Debug.DrawLine(theTarget.transform.position, new Vector3(), Color.yellow, 6f);
        theEnaction.enact(theTarget.transform.position);
    }

    override public void target(GameObject theTarget)
    {
        this.theTarget = theTarget;
    }
}