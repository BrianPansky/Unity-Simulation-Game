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


    public planEXE2 fullPlan;

    public Dictionary<buttonCategories, List<planEXE>> blankMultiPlan()
    {
        return new Dictionary<buttonCategories, List<planEXE>>();
    }

    public void multiPlanAdd(planEXE theExeToAdd, Dictionary<buttonCategories, List<planEXE>> multiPlan)
    {
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


    //      !!!!!!!!!!!!!!!  was supposed to be private so that constructors inputs guarantee it's never null...but then i changed the constructors again...
    private List<planEXE2> exeList;

    public List<condition> startConditions = new List<condition>();
    public List<condition> endConditions = new List<condition>();

    public int numberOfTimesExecuted = 0;  //don't do it for things that are called every frame, though?

    public abstract void execute();

    public void Add(planEXE2 itemToAdd)
    {
        if(exeList == null) { exeList = new List<planEXE2>(); }
        exeList.Add(itemToAdd);
    }

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


    public void atLeastOnce()
    {
        condition thisCondition = new enacted(this);
        endConditions.Add(thisCondition);
    }

    public void untilListFinished()
    {
        if (exeList == null) { exeList = new List<planEXE2>(); }
        condition thisCondition = new planListComplete(exeList);  //should be fine?  lists are references, so will work even if items are added after this??
        endConditions.Add(thisCondition);
    }


    public void executeSequential()
    {
        //this function is here because i want the lists to be private so that parallel and sequential EXEs initialize correctly

        //sequential concerns:
        //      only execute 1st one
        //      remove item from list when its end conditions are met

        if (exeList == null) { Debug.Log("null.....that's an error!"); return; }

        if (exeList.Count < 1) { Debug.Log("exeList.Count < 1       shouldn't happen?"); return; }

        exeList[0].execute();

        if (exeList[0].endConditionsMet())
        {
            //Debug.Log("exeList[0].endConditionsMet()  for:  " + exeList[0]);
            if (exeList[0].theEnaction != null)
            {
                //Debug.Log("exeList[0].endConditionsMet()  for theEnaction:  " + exeList[0].theEnaction);
            }
            
            exeList.RemoveAt(0); return;
        }
    }


    public void executeParallel()
    {

        if (exeList == null) { Debug.Log("null.....that's an error!"); ; return; }

        //if null.....that's an error!


        List<planEXE2> completedItems = new List<planEXE2>();

        foreach (planEXE2 plan in exeList)
        {
            plan.execute();
            if (plan.endConditionsMet()) { completedItems.Add(plan); }
        }

        foreach (planEXE2 plan in completedItems)
        {
            exeList.Remove(plan);
        }
    }

}


public class singleEXE : planEXE2
{
    private GameObject target;



    public override void execute()
    {

        if (theEnaction == null) { Debug.Log("null.....that's an error!"); return;}

        if (startConditionsMet() == false) { return; }

        theEnaction.enact(theInputData);
        numberOfTimesExecuted++;
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

    public parallelEXE()
    {

        //      !!!!!!!!!!!!!!!!!   endConditions.Add(new planListComplete(list));
    }
    public parallelEXE(planEXE2 item)
    {
        Add(item);
        //      !!!!!!!!!!!!!!!!!   endConditions.Add(new planListComplete(list));
    }
    public parallelEXE(planEXE2 item1, planEXE2 item2)
    {
        Add(item1);
        Add(item2);
        //      !!!!!!!!!!!!!!!!!   endConditions.Add(new planListComplete(list));
    }
    public parallelEXE(planEXE2 item1, planEXE2 item2, planEXE2 item3)
    {
        Add(item1);
        Add(item2);
        Add(item3);
        //      !!!!!!!!!!!!!!!!!   endConditions.Add(new planListComplete(list));
    }
    public parallelEXE(planEXE2 item1, planEXE2 item2, planEXE2 item3, planEXE2 item4)
    {
        Add(item1);
        Add(item2);
        Add(item3);
        Add(item4);
        //      !!!!!!!!!!!!!!!!!   endConditions.Add(new planListComplete(list));
    }



    public override void execute()
    {
        executeParallel();
    }
}

public class seriesEXE : planEXE2
{
    public seriesEXE()
    {
        //      !!!!!!!!!!!!!!!!!   endConditions.Add(new planListComplete(list));
    }
    public seriesEXE(planEXE2 item)
    {
        Add(item);
        //      !!!!!!!!!!!!!!!!!   endConditions.Add(new planListComplete(list));
    }
    public seriesEXE(planEXE2 item1, planEXE2 item2)
    {
        Add(item1);
        Add(item2);
        //      !!!!!!!!!!!!!!!!!   endConditions.Add(new planListComplete(list));
    }
    public seriesEXE(planEXE2 item1, planEXE2 item2, planEXE2 item3)
    {
        Add(item1);
        Add(item2);
        Add(item3);
        //      !!!!!!!!!!!!!!!!!   endConditions.Add(new planListComplete(list));
    }
    public seriesEXE(planEXE2 item1, planEXE2 item2, planEXE2 item3, planEXE2 item4)
    {
        Add(item1);
        Add(item2);
        Add(item3);
        Add(item4);
        //      !!!!!!!!!!!!!!!!!   endConditions.Add(new planListComplete(list));
    }

    public override void execute()
    {
        executeSequential();
    }
}
