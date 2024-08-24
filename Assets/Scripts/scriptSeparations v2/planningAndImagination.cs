using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;

public class planningAndImagination : MonoBehaviour
{
    //keep in mind that planning is only about causality/logic.  which things CAN be done, in which order, to get an outcome
    //the EVALUATION of which plan is best [or even "good enough"] has to happen elsewhere

    //also, plans and following plans are not the same as plan-"ing"


    //public List<planEXE> plan = new List<planEXE>();
    //public Dictionary<buttonCategories, List<planEXE>> multiPlan = new Dictionary<buttonCategories, List<planEXE>>();
    //public List<Dictionary<buttonCategories, List<planEXE>>> planOfMultiPlans = new List<Dictionary<buttonCategories, List<planEXE>>>();
    public planEXE2 fullPlan;

    public Dictionary<buttonCategories, List<planEXE>> blankMultiPlan()
    {
        return new Dictionary<buttonCategories, List<planEXE>>();
    }

    public void multiPlanAdd(planEXE theExeToAdd, Dictionary<buttonCategories, List<planEXE>> multiPlan)
    {

        //Debug.Log("111  multiPlan.Keys.Count:  " + multiPlan.Keys.Count);
        enaction anEnaction = theExeToAdd.theEnaction;
        Debug.Assert(anEnaction != null);
        if (anEnaction == null)
        {
            Debug.Log("enaction is null");
            Debug.Log("for this theExeToAdd:  " + theExeToAdd);
        }

        buttonCategories theButtonCategory = anEnaction.gamepadButtonType;

        if (multiPlan.ContainsKey(theButtonCategory) == false)
        {
            multiPlan[theButtonCategory] = new List<planEXE>();
        }

        List<planEXE> planToAddItTo = multiPlan[theButtonCategory];

        if (planToAddItTo == null) { planToAddItTo = new List<planEXE>(); }

        planToAddItTo.Add(theExeToAdd);




        //Debug.Log("222  multiPlan.Keys.Count:  " + multiPlan.Keys.Count);
    }




    public bool noPlansInMultiplan(Dictionary<buttonCategories, List<planEXE>> dictionary)
    {
        bool noPlansInMultiplan = true;

        foreach(var key in dictionary.Keys)
        {
            if (dictionary[key].Count > 0) { return false; }
        }

        return noPlansInMultiplan;
    }




}

public abstract class planEXE
{
    public enaction theEnaction;
    public List<condition> startConditions = new List<condition>();
    public List<condition> endConditions = new List<condition>();

    public abstract void executePlan();
    public abstract void target(GameObject theTarget);

    public bool areSTARTconditionsFulfilled()
    {
        //Debug.Log("areSTARTconditionsFulfilled?  for:  " + this.theEnaction);

        foreach (condition thisCondition in startConditions)
        {
            if (thisCondition.met() == false) { return false; }
        }

        //Debug.Log("no conditions remain unfulfilled!");
        return true;
    }

    public bool areENDconditionsFulfilled()
    {
        //Debug.Log("areENDconditionsFulfilled?  for:  "+ this.theEnaction);
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
    //public IEnactaBool theEnaction;
    public List<planEXE> microPlan = new List<planEXE>();

    public boolEXE(IEnactaBool theEnaction, GameObject theTarget)
    {
        this.theEnaction = theEnaction;
    }

    override public void executePlan()
    {
        //Debug.Log("executePlan for:  "+ this.theEnaction);

        foreach (var planEXE in microPlan)
        {
            planEXE.executePlan();
        }

        theEnaction.enact(new inputData().boolean());
    }

    override public void target(GameObject theTarget)
    {

    }

}
public class vectEXE : planEXE
{

    //public IEnactByTargetVector theEnaction;
    public GameObject theTarget;

    public vectEXE(IEnactByTargetVector theEnaction, GameObject theTarget)
    {
        this.theEnaction = theEnaction;
        this.theTarget = theTarget;
    }


    override public void executePlan()
    {

        //Debug.Log("vectEXE, theTarget:  " + theTarget);
        Debug.DrawLine(theTarget.transform.position, new Vector3(), Color.yellow, 6f);
        inputData theInputData = new inputData().vect(theTarget.transform.position);
        theEnaction.enact(theInputData);
    }

    override public void target(GameObject theTarget)
    {
        this.theTarget = theTarget;
    }
}




public abstract class planEXE2
{
    public enaction theEnaction;
    public inputData theInputData;


    //      !!!!!!!!!!!!!!!  MUST input these into constructor!
    private List<planEXE2> parallelSet;
    private List<planEXE2> sequentialSet;

    public List<condition> startConditions = new List<condition>();
    public List<condition> endConditions = new List<condition>();

    public int numberOfTimesExecuted = 0;  //don't do it for things that are called every frame, though?

    public abstract void execute();

    public bool startConditionsMet()
    {
        foreach (condition thisCondition in startConditions)
        {
            //Debug.Log("thisCondition:  " + thisCondition);
            //Debug.Log("thisCondition.met():  " + thisCondition.met());
            if (thisCondition.met() == false) { return false; }
        }

        //Debug.Log("no conditions remain unfulfilled!");
        return true;
    }
    public bool endConditionsMet()
    {
        //Debug.Log("looking at end conditions for:  " + this);

        //if (theEnaction != null) { Debug.Log("looking at end conditions for:  " + theEnaction); }
        foreach (condition thisCondition in endConditions)
        {
            //Debug.Log("thisCondition:  " + thisCondition);
            //if (theEnaction != null) { Debug.Log("thisCondition:  " + thisCondition); }
            if (thisCondition.met() == false) { return false; }
        }
        //Debug.Log("no conditions remain unfulfilled!");

        //if (theEnaction != null) { Debug.Log("so this enaction is finished:  " + theEnaction); }

        return true;
    }

    public void addParallelSet(List<planEXE2> inparallelSet)
    {
        parallelSet = inparallelSet;
    }
    public void addSequentialSet(List<planEXE2> inSequentialSet)
    {
        sequentialSet = inSequentialSet;
    }







    public void executeSequentialSet()
    {
        //this function is here because i want the lists to be private so that parallel and sequential EXEs initialize correctly

        //sequential concerns:
        //      only execute 1st one
        //      remove item from list when its end conditions are met

        if (sequentialSet == null) { Debug.Log("null.....that's an error!"); ; return; }

        if (sequentialSet.Count < 1) { Debug.Log("shouldn't happen?"); return; }

        sequentialSet[0].execute();

        if (sequentialSet[0].endConditionsMet()) { sequentialSet.RemoveAt(0); return; }
    }


    public void executeParallelSet()
    {

        if (parallelSet == null) { Debug.Log("null.....that's an error!"); ; return; }

        //if null.....that's an error!


        List<planEXE2> completedItems = new List<planEXE2>();

        foreach (planEXE2 plan in parallelSet)
        {
            plan.execute();
            if (plan.endConditionsMet()) { completedItems.Add(plan); }
        }

        foreach (planEXE2 plan in completedItems)
        {
            parallelSet.Remove(plan);
        }
    }

}


public class singleEXE : planEXE2
{
    private GameObject target;

    /*
    public singleEXE(enaction theInputEnaction, GameObject target)
    {
        this.theEnaction = theInputEnaction;
        this.target = target;
    }
    */

    public override void execute()
    {

        if (theEnaction != null) {

            if (startConditionsMet() == false) { return; }

            theEnaction.enact(theInputData);
            numberOfTimesExecuted++;
            return; }
        //if null.....that's an error!
        Debug.Log("null.....that's an error!");
    }
}


public class boolEXE2 : singleEXE
{
    //public IEnactaBool theEnaction;
    public List<planEXE> microPlan = new List<planEXE>();

    public boolEXE2(IEnactaBool theInputEnaction, GameObject theTarget)
    {
        this.theEnaction = theInputEnaction;
    }
}

public class vect2EXE2 : singleEXE
{

    //public IEnactByTargetVector theEnaction;
    public GameObject theTarget;

    public vect2EXE2(IEnactByTargetVector theInputEnaction, GameObject theTarget)
    {
        this.theEnaction = theInputEnaction;
        this.theTarget = theTarget;
        theInputData = new inputData().vect2Targ(theTarget);
    }
}

public class vect3EXE2 : singleEXE
{

    //public IEnactByTargetVector theEnaction;
    public GameObject theTarget;

    public vect3EXE2(IEnactByTargetVector theInputEnaction, GameObject theTarget)
    {
        this.theEnaction = theInputEnaction;
        this.theTarget = theTarget;
        theInputData = new inputData().vect3Targ(theTarget);
    }

    public vect3EXE2(IEnactaVector theInputEnaction, GameObject theTarget)
    {
        this.theEnaction = theInputEnaction;
        this.theTarget = theTarget;
        theInputData = new inputData().vect3Targ(theTarget);
    }
}



public class parallelEXE : planEXE2
{

    public parallelEXE(List<planEXE2> list)
    {
        //parallelSet = new List<planEXE2>();

        addParallelSet(list);
        endConditions.Add(new planListComplete(list));
    }


    public override void execute()
    {
        executeParallelSet();
    }


    public void addToParallel(planEXE2 toAdd)
    {

    }


}

public class seriesEXE : planEXE2
{

    public seriesEXE(List<planEXE2> list)
    {
        //sequentialSet = new List<planEXE2>();
        addSequentialSet(list);
        endConditions.Add(new planListComplete(list));
    }

    public override void execute()
    {
        executeSequentialSet();
    }


    public void addToSequential(planEXE2 toAdd)
    {

    }

}
