using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static UnityEngine.GraphicsBuffer;

public class planningAndImagination : MonoBehaviour
{
    //keep in mind that planning is only about causality/logic.  which things CAN be done, in which order, to get an outcome
    //the EVALUATION of which plan is best [or even "good enough"] has to happen elsewhere

    //also, plans and following plans are not the same as plan-"ing"


    public planEXE2 fullPlan;


}



public abstract class planEXE2
{
    public enaction theEnaction;
    //public inputData theInputData;

    public bool debugPrint = false;
    //nestedLayerDebug debug;
    //public adHocDebuggerForGoGrabPlan grabberDebug;// = new adHocDebuggerForGoGrabPlan();

    //      !!!!!!!!!!!!!!!  was supposed to be private so that constructors inputs guarantee it's never null...but then i changed the constructors again...
    public List<planEXE2> exeList;

    public List<condition> startConditions = new List<condition>();
    public List<condition> endConditions = new List<condition>();

    public int numberOfTimesExecuted = 0;  //don't do it for things that are called every frame, though?

    public abstract void execute();

    public abstract bool error();

    abstract public void setTarget(targetCalculator targetCalc);

    public void doConditionalEffectsAdHocDebugThing(targetCalculator theTargetCalculatorIn, hitscanEnactor theHitscanEnactorIn, adHocDebuggerForGoGrabPlan grabberDebugIn, Dictionary<condition, List<Ieffect>> conditionalEffectsIn, adHocBooleanDeliveryClass signalThatFiringIsDone)
    {
        //okSuperAdhocPlaceToDoThisDebugNonsense

        targetMatchesHitscanOutput theCondition = new targetMatchesHitscanOutput(theTargetCalculatorIn);//, theHitCalculatorIn);

        theCondition.firingIsDone = signalThatFiringIsDone;
        theCondition.theHitScanner = theHitscanEnactorIn;

        Ieffect theEffect = new adHocDebugEffect(grabberDebugIn, theCondition);
        List<Ieffect> theEffects = new List<Ieffect>();
        theEffects.Add(theEffect);
        conditionalEffectsIn[theCondition] = theEffects;
    }



    public void resetEnactionCounter()
    {
        numberOfTimesExecuted = 0;
    }


    public string nestedPlanCountToText()
    {
        string stringToReturn = "";

        if (exeList == null) { return "[(exeList == null), no nested plans to count]"; }
        if (exeList.Count == 0) { return "[(exeList.Count == 0), no nested plans to count]"; }

        stringToReturn += "[exeList.Count = " + exeList.Count;

        foreach(planEXE2 thisPlan in exeList)
        {
            if (thisPlan !=null)
            {

                stringToReturn += thisPlan.nestedPlanCountToText();
            }
        }


        stringToReturn += "]";

        return stringToReturn;
    }


    public bool standardExecuteErrors()
    {
        if (theEnaction == null) { Debug.Log("null.....that's an error!"); return true; } //is it, though?

        //conditionalPrint("startConditionsMet():  "+ startConditionsMet());
        if (startConditionsMet() == false) { return true; }

        return false;
    }

    public void executeSequential()
    {

        //conditionalPrint("executeSequential()");
        //conditionalPrint("x1 nestedPlanCountToText():  " + nestedPlanCountToText());
        //this function is here because i want the lists to be private so that parallel and sequential EXEs initialize correctly

        //sequential concerns:
        //      only execute 1st one
        //      remove item from list when its end conditions are met

        if (exeList == null) { //Debug.Log("null.....that's an error!"); 
            return; }

        if (exeList.Count < 1) { 
            //Debug.Log("exeList.Count < 1       shouldn't happen?"); 
            return; }


        if (exeList[0] == null)
        { //Debug.Log("null.....that's an error!"); 
            return;
        }

        exeList[0].debugPrint = debugPrint;

        //exeList[0].grabberDebug = grabberDebug;
        //      conditionalPrint("5555555555555555555555555555grabberDebug.GetInstanceID():  " + grabberDebug.GetInstanceID());
        //      grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);
        //conditionalPrint("x2 nestedPlanCountToText():  " + nestedPlanCountToText());

        if (startConditionsMet())
        {
            //Debug.Log("start conitions met, should do this:  " + exeList[0].staticEnactionNamesInPlanStructure());
            //conditionalPrint(" grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);...........");
            //conditionalPrint("??????????????????????????????? exeList[0].theEnaction:  " + exeList[0].theEnaction);
            //grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);
            //conditionalPrint("///////////////////////////////////////////////////////////////////////");
            exeList[0].execute();
        }

        //conditionalPrint("x3 nestedPlanCountToText():  " + nestedPlanCountToText());

        if (exeList[0].endConditionsMet())
        {
            //Debug.Log("exeList[0].endConditionsMet()  for:  " + exeList[0]);
            if (exeList[0].theEnaction != null)
            {
                //Debug.Log("exeList[0].endConditionsMet()  for theEnaction:  " + exeList[0].theEnaction);
            }


            //conditionalPrint("x4 nestedPlanCountToText():  " + nestedPlanCountToText());
            //conditionalPrint("endConditionsMet, so:  exeList.RemoveAt(0)");
            exeList.RemoveAt(0);

            //conditionalPrint("x5 nestedPlanCountToText():  " + nestedPlanCountToText());

            return;
        }

        //conditionalPrint("x6 nestedPlanCountToText():  " + nestedPlanCountToText());
    }

    public void executeParallel()
    {
        if (exeList == null) { Debug.Log("null.....that's an error!"); ; return; }

        //if null.....that's an error!


        List<planEXE2> completedItems = new List<planEXE2>();

        foreach (planEXE2 plan in exeList)
        {

            //plan.grabberDebug = grabberDebug;
            plan.execute();
            if (plan.endConditionsMet()) { completedItems.Add(plan); }
        }

        foreach (planEXE2 plan in completedItems)
        {
            exeList.Remove(plan);
        }
    }





    public bool startConditionsMet()
    {
        //grabberDebug.debugPrintBool = debugPrint;
        //Debug.Log("tartConditions.Count:  " + startConditions.Count);
        foreach (condition thisCondition in startConditions)
        {
            //Debug.Log("thisCondition:  " + thisCondition);
            //Debug.Log("thisCondition.met():  " + thisCondition.met());
            if (thisCondition.met() == false) {

                //if (debugPrint == true) { Debug.Log("this start condition not met:  " + thisCondition); }
                //      grabberDebug.rep
                return false; }
        }

        //Debug.Log("no start conditions remain unfulfilled!");
        //Debug.Log("no conditions remain unfulfilled!");
        return true;
    }
    
    public bool endConditionsMet()
    {
        Debug.Log("looking at end conditions for:  " + this);

        if (debugPrint == true) 
        {
            if(theEnaction != null)
            {
                //  conditionalPrint("-----------------------------looking at end conditions for a single enaction:  " + theEnaction.ToString());
            }
            else if(exeList != null)
            {

                //conditionalPrint("...............................looking at end conditions for an exeList???  the count of the list:  " + exeList.Count);
            }
            else
            {
                conditionalPrint("uhhhhhhhhh.....???????????? both the enaction AND the exeList are null...........");
            }
        }
        //if (theEnaction != null) { Debug.Log("looking at end conditions for:  " + theEnaction); }
        foreach (condition thisCondition in endConditions)
        {
            //conditionalPrint("thisCondition:  " + thisCondition);
            Debug.Log("thisCondition:  " + thisCondition);
            //if (theEnaction != null) { Debug.Log("thisCondition:  " + thisCondition); }
            if (thisCondition.met() == false) 
            {
                //conditionalPrint("this end condition not met:  "+ thisCondition);
                Debug.Log("this end condition not met:  " + thisCondition);
                return false; 
            }
        }
        Debug.Log("no conditions remain unfulfilled!");

        //conditionalPrint("no end conditions remain unfulfilled!");
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


    public string asText()
    {

        string theString = "";

        theString += staticEnactionNamesInPlanStructure();

        return theString;
    }


    public string infoString3()
    {
        string theString = "";

        theString += this.ToString();
        theString += ":  ";
        theString += theEnaction;

        theString += conditionsAsText();

        if (exeList == null) { return theString; }


        theString += "[ ";
        foreach (planEXE2 plan in exeList)
        {
            if (plan == null) { theString += "(plan == null)"; continue; }
            theString += plan.asText();
            theString += ", ";
        }

        theString += "]";
        return theString;
    }



    public string staticEnactionNamesInPlanStructure()
    {
        string theString = "";

        theString += this.ToString();
        theString += ":  ";
        theString += theEnaction;

        if (exeList == null) { return theString; }


        theString += "[ ";
        foreach (planEXE2 plan in exeList)
        {
            if (plan == null) { theString += "(plan == null)"; continue; }
            theString += plan.staticEnactionNamesInPlanStructure();
            theString += ", ";
        }

        theString += "]";
        return theString;
    }



    public string conditionsAsText()
    {
        string stringToReturn = "";

        stringToReturn += "number of START conditions:  " + startConditions.Count;

        foreach (condition condition in startConditions)
        {
            stringToReturn += ", ";
            stringToReturn += condition.asText();
        }
        stringToReturn += ", number of END conditions:  " + endConditions.Count;

        foreach (condition condition in endConditions)
        {
            stringToReturn += ", ";
            stringToReturn += condition.asText();
        }


        return stringToReturn;
    }


    internal void conditionalPrint(string thingToPrint)
    {
        if (debugPrint == false) { return; }


        Debug.Log(thingToPrint);

    }





    public void Add(planEXE2 itemToAdd)
    {
        if (exeList == null) { exeList = new List<planEXE2>(); }
        exeList.Add(itemToAdd);
    }

    internal void Add(List<planEXE2> addFromList)
    {

        if (exeList == null) { exeList = new List<planEXE2>(); }
        foreach (planEXE2 item in addFromList)
        {
            exeList.Add(item); 
        }
    }
}


public abstract class singleEXE : planEXE2
{
    //private GameObject target;

    

    public abstract override void execute();
}



public class boolEXE2 : singleEXE
{
    //public IEnactaBool theEnaction;
    public List<planEXE2> microPlan = new List<planEXE2>();  //am i still gonna use this?  i don't think so....

    public boolEXE2(enaction theEnactionIn)
    {
        this.theEnaction = theEnactionIn;
    }

    public override void execute()
    {
        //conditionalPrint("7777777777777777777777777777grabberDebug.GetInstanceID():  " + grabberDebug.GetInstanceID());
        //      grabberDebug.recordCurrentEnaction(this.theEnaction);
        //conditionalPrint("aaaaaa.1111111111111aaaaaaa theRefillPlan.nestedPlanCountToText():  " + nestedPlanCountToText());
        if (standardExecuteErrors()) { return; }

        //conditionalPrint("should enact this:  " + this.theEnaction);
        theEnaction.debugPrint = debugPrint;
        theEnaction.enact(new inputData());
        //executeInputData(new inputData());
        numberOfTimesExecuted++;

        //conditionalPrint("aaaaaa.1111111111111bbbbbbb theRefillPlan.nestedPlanCountToText():  " + nestedPlanCountToText());
    }


    public override bool error()
    {
        return false;
    }

    override public void setTarget(targetCalculator targetCalc)
    {
        //Debug.Log("ahh..........?????????????");
    }


    public boolEXE2(IEnactaBool theInputEnaction, GameObject theTarget)
    {
        this.theEnaction = theInputEnaction;
    }



    internal void conditionalPrint(string thingToPrint)
    {
        if (debugPrint == false) { return; }


        Debug.Log(thingToPrint);

    }
}

public class vect3EXE2 : singleEXE
{
    //public GameObject possiblyMobileActualTarget;
    //public Vector3 stationaryTargetAsVector;
    //public float offsetRoom = 0f;

    public targetCalculator theTargetCalculator;


    public vect3EXE2(IEnactByTargetVector theInputEnaction, GameObject possiblyMobileActualTargetIn, float offsetRoomIn = 1.8f)
    {
        this.theEnaction = theInputEnaction;
        //this.possiblyMobileActualTarget = possiblyMobileActualTargetIn;
        //this.offsetRoom = offsetRoomIn;

        theTargetCalculator = new movableObjectTargetCalculator(this.theEnaction.transform.gameObject, possiblyMobileActualTargetIn, offsetRoomIn);
    }


    public vect3EXE2(IEnactByTargetVector theInputEnaction, Vector3 stationaryTargetAsVectorIn, float offsetRoomIn = 1.8f)
    {
        this.theEnaction = theInputEnaction;
        //this.stationaryTargetAsVector = stationaryTargetAsVectorIn;
        //this.offsetRoom = offsetRoomIn;

        theTargetCalculator = new staticVectorTargetCalculator(this.theEnaction.transform.gameObject, stationaryTargetAsVectorIn, offsetRoomIn);

    }

    public vect3EXE2(IEnactaVector theInputEnaction, GameObject possiblyMobileActualTargetIn, float offsetRoomIn = 1.8f)
    {
        //!!!!!!!!!!!!! used to fix this error:
        //cannot convert from 'IEnactaVector' to 'IEnactByTargetVector'
        //!!!!!!!!!!!


        this.theEnaction = theInputEnaction;
        //this.possiblyMobileActualTarget = theTarget;
        theTargetCalculator = new movableObjectTargetCalculator(this.theEnaction.transform.gameObject, possiblyMobileActualTargetIn, offsetRoomIn);

    }


    public override void execute()
    {

        if (standardExecuteErrors()) { return; }

        //conditionalPrint("should enact this:  " + this.theEnaction);
        //Debug.Log("theTarget:  " + target);


        //theEnaction.enact(new inputData(offsetDestination(target.transform.position)));
        //conditionalPrint("(target:  " + target);
        //conditionalPrint("(target.transform.position:  " + target.transform.position);

        inputData theInput = new inputData();

        if (debugPrint == true)
        {
            Debug.DrawLine(new Vector3(), theEnaction.transform.position, Color.green, 2f);
            Debug.DrawLine(theInput.vect3, theEnaction.transform.position, Color.blue, 2f);
        }

        if (debugPrint == true)
        {
            Debug.DrawLine(theInput.vect3, theEnaction.transform.position, Color.red, 3f);
        }


        //conditionalPrint("8888888888888888888888888888grabberDebug.GetInstanceID():  " + grabberDebug.GetInstanceID());
        //grabberDebug.recordCurrentEnaction(theEnaction);
        theEnaction.enact(new inputData(theTargetCalculator.targetPosition()));
        //executeInputData(new inputData());
        numberOfTimesExecuted++;
        //conditionalPrint("aaaaaa.2222222222222bbbbbbb theRefillPlan.nestedPlanCountToText():  " + nestedPlanCountToText());
    }




    public override bool error()
    {
        return false;
    }



    override public void setTarget(targetCalculator targetCalc)
    {
        theTargetCalculator = targetCalc;


        //Debug.Log("theTargetCalculator:  " + theTargetCalculator);
        //Debug.Log("newTtheTargetCalculatorarget.targetPosition():  " + theTargetCalculator.targetPosition());
        //Debug.Log("theTargetCalculator.GetHashCode():  " + theTargetCalculator.GetHashCode());
    }

}



public class parallelEXE : planEXE2
{

    public parallelEXE()
    {


    }
    public parallelEXE(planEXE2 item)
    {
        Add(item);

    }
    public parallelEXE(planEXE2 item1, planEXE2 item2)
    {
        Add(item1);
        Add(item2);

    }
    public parallelEXE(planEXE2 item1, planEXE2 item2, planEXE2 item3)
    {
        Add(item1);
        Add(item2);
        Add(item3);

    }
    public parallelEXE(planEXE2 item1, planEXE2 item2, planEXE2 item3, planEXE2 item4)
    {
        Add(item1);
        Add(item2);
        Add(item3);
        Add(item4);

    }



    public override void execute()
    {
        executeParallel();
    }


    public override bool error()
    {
        foreach (planEXE2 exe in exeList)
        {
            if (exe == null)
            {
                conditionalPrint("error (in a parallelEXE):  (exe == null)");
                return true;
            }
            if (exe.error())
            {
                conditionalPrint("error (in a parallelEXE):  exe.error(), for THIS exe:  " + exe.asText());
                return true;
            }
        }

        return false;
    }


    override public void setTarget(targetCalculator targetCalc)
    {
        foreach(planEXE2 exe in exeList)
        {
            exe.setTarget(targetCalc);
        }
    }
}

public class seriesEXE : planEXE2
{
    public seriesEXE()
    {
    }
    public seriesEXE(planEXE2 item)
    {
        Add(item);
    }
    public seriesEXE(planEXE2 item, List<condition> listOfCOnditionsIn)
    {
        Add(item);

        foreach (var x in listOfCOnditionsIn)
        {
            startConditions.Add(x);
            //Debug.Log("2startConditions.Count:  " + startConditions.Count);
        }
        
    }
    public seriesEXE(planEXE2 item1, planEXE2 item2)
    {
        Add(item1);
        Add(item2);
    }
    public seriesEXE(planEXE2 item1, planEXE2 item2, planEXE2 item3)
    {
        Add(item1);
        Add(item2);
        Add(item3);
    }
    public seriesEXE(planEXE2 item1, planEXE2 item2, planEXE2 item3, planEXE2 item4)
    {
        Add(item1);
        Add(item2);
        Add(item3);
        Add(item4);
    }

    public override void execute()
    {
        //grabberDebug.debugPrintBool = debugPrint;
        executeSequential();
    }


    public override bool error()
    {

        if (exeList == null)
        {
            conditionalPrint("error:  (exeList == null)");
            return true;
        }

        if (exeList.Count < 1)
        {
            conditionalPrint("error:  (exeList.Count < 1) [might often just mean end of plan, i think]");
            return true;
        }

        //|| exeList.Count < 1) { return true; }


        //hmm, those EXEs should have their OWN ability to check for errors?  but.....well, not if they're NULL they won't.......
        foreach (planEXE2 exe in exeList)
        {
            if (exe == null)
            {

                //                  conditionalPrint("error:  (exe == null)");
                return true;
            }

            if (exe.error())
            {

                conditionalPrint("error:  exe.error(), for this exe:  " +exe.asText());
                return true;
            }

            // || ) { return true; }
        }

        return false;
    }


    override public void setTarget(targetCalculator targetCalc)
    {
        foreach (planEXE2 exe in exeList)
        {
            exe.setTarget(targetCalc);
        }
    }
}

public class simultaneousEXE : planEXE2
{

    public simultaneousEXE()
    {


    }
    public simultaneousEXE(planEXE2 item)
    {
        Add(item);

    }
    public simultaneousEXE(planEXE2 item1, planEXE2 item2)
    {
        Add(item1);
        Add(item2);

    }
    public simultaneousEXE(planEXE2 item1, planEXE2 item2, planEXE2 item3)
    {
        Add(item1);
        Add(item2);
        Add(item3);

    }
    public simultaneousEXE(planEXE2 item1, planEXE2 item2, planEXE2 item3, planEXE2 item4)
    {
        Add(item1);
        Add(item2);
        Add(item3);
        Add(item4);

    }



    public override void execute()
    {
        executeAll();
    }

    private void executeAll()
    {
        foreach (planEXE2 exe in exeList) { exe.execute(); }
    }

    public override bool error()
    {
        foreach (planEXE2 exe in exeList)
        {
            if (exe == null)
            {
                conditionalPrint("error (in a parallelEXE):  (exe == null)");
                return true;
            }
            if (exe.error())
            {
                conditionalPrint("error (in a parallelEXE):  exe.error(), for THIS exe:  " + exe.asText());
                return true;
            }
        }

        return false;
    }


    override public void setTarget(targetCalculator targetCalc)
    {
        foreach (planEXE2 exe in exeList)
        {
            exe.setTarget(targetCalc);
        }
    }
}





public abstract class adHocPlanRefillThing
{
    public List<condition> theConditions = new List<condition>();

    public planEXE2 theRefillPlan;

    public planEXE2 theCurrentPlan = new seriesEXE();
    public bool debugPrint = false;
    nestedLayerDebug debug;


    public planEXE2 asSeries(planEXE2 theRefillPlan)
    {
        //shallow copy!  don't use for current plan!

        if(theRefillPlan == null) {return null; }
        if(theRefillPlan.exeList == null)
        {
            return new seriesEXE(theRefillPlan);
        }

        return theRefillPlan;
    }

    public abstract void doUpdate();


    public void standardUpdate()
    {

        //conditionalPrint(">>>>> theRefillPlan.nestedPlanCountToText():  " + theRefillPlan.nestedPlanCountToText());
        //Debug.Log(">>>>>>>trying to do an update for an ''adHocPlanRefillThing''");

        //implement if conditions met, remove completed planEXEs
        //refill current plan if it's empty
        //if (theCurrentPlan == null || theCurrentPlan.exeList == null || theCurrentPlan.exeList.Count == 0) { refill(); }


        //conditionalPrint("standard update (for adhoc refill thing).  first, does currentPlan have any errors?");


        //conditionalPrint("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxrefill thing:  " + theRefillPlan.asText());

        transferDebugBool();

        refillIfNeeded();


        //Debug.Log("----------------------------doCurrentPlan():  " + theCurrentPlan.asText());
        doCurrentPlan();
    }

    private void transferDebugBool()
    {
        if (theCurrentPlan != null)
        {
            theCurrentPlan.debugPrint = debugPrint;
        }
    }

    private void refillIfNeeded()
    {
        if (theCurrentPlan == null)
        {
            refill();
        }
        else if (theCurrentPlan.error())
        {
            conditionalPrint("theCurrentPlan.error(), refilling");
            refill();
        }
        else if (theCurrentPlan.endConditionsMet())
        {
            conditionalPrint("theCurrentPlan.endConditionsMet()");
            refill();
        }
    }

    public abstract void refill();

    public void standardRefill()
    {
        if (theRefillPlan == null)
        {
            //conditionalPrint("(theRefillPlan == null)");
            return; }  //seems very messy



        List<planEXE2> refillWithThis = new List<planEXE2>();


        foreach (planEXE2 plan in theRefillPlan.exeList)
        {
            //conditionalPrint("******** add this:  " + plan.asText());
            refillWithThis.Add(plan);
        }


        theCurrentPlan.Add(refillWithThis);

    }

    private void doCurrentPlan()
    {
        //well this is a messy way to do this, only an EXE should do this....ya...i'm reinventing my EXEs.....:
        if (startConditionsMet())
        {

            //  conditionalPrint("startConditionsMet(), so execute this:  " + theCurrentPlan.asText());
            //Debug.Log("theCurrentPlan.execute()");
            //conditionalPrint("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa theRefillPlan.nestedPlanCountToText():  " + theRefillPlan.nestedPlanCountToText());
            theCurrentPlan.execute();
            //conditionalPrint("bbbbbbbbbbbbbbbbbbbbbbbbbbbbb theRefillPlan.nestedPlanCountToText():  " + theRefillPlan.nestedPlanCountToText());
        }
        else
        {
            //  conditionalPrint("start Conditions NOT Met(), so DON'T execute");
        }


    }


    public bool startConditionsMet()
    {
        foreach (condition thisCondition in theConditions)
        {
            //Debug.Log("thisCondition, met?:  " + thisCondition + ",  " + thisCondition.met());
            //Debug.Log("thisCondition.met():  " + thisCondition.met());
            if (thisCondition.met() == false)
            {
                //Debug.Log("this start condition not met:  " + thisCondition);

                return false;
            }
        }

        //Debug.Log("no start conditions remain unfulfilled!");
        //Debug.Log("no conditions remain unfulfilled!");
        return true;
    }

    public abstract string asText();

    public abstract string conditionsAsText();

    public abstract string conditionsAsTextDETAIL();





    public void conditionalPrint(string toPrint)
    {
        if (debugPrint == false) { return; }
        Debug.Log(toPrint);
    }

}



public class adHocRefillThingGeneral : adHocPlanRefillThing
{
    public adHocRefillThingGeneral(List<condition> theConditionsIn, planEXE2 theRefillPlanIn)// : base(theConditions, theRefillPlan)
    {
        //why is it requiring me to include this?  especially for the weird reason of what INPUT it has.  who cares what input it has???

        //List<condition> theConditionsIn, List<planEXE2> theRefillPlanIn
        this.theConditions = theConditionsIn;
        this.theRefillPlan = asSeries(theRefillPlanIn);
        //conditionalPrint("this.theRefillPlan" + this.theRefillPlan);
        //conditionalPrint("this.theRefillPlan.exeList" + this.theRefillPlan.exeList);
        //conditionalPrint("this.theRefillPlan.exeList.Count" + this.theRefillPlan.exeList.Count);
        //  this.theCurrentPlan = asSeries(theRefillPlanIn);
        standardRefill();
        //conditionalPrint("this.theCurrentPlan" + this.theCurrentPlan);
        //conditionalPrint("this.theCurrentPlan.exeList" + this.theCurrentPlan.exeList);
        //conditionalPrint("this.theCurrentPlan.exeList.Count" + this.theCurrentPlan.exeList.Count);
        //if(this.theRefillPlan == null) { this.error()}
        if (theCurrentPlan == null)
        {
            //can happen.......
            return;
        }
        theCurrentPlan.atLeastOnce();  //hmmmm, seems silly to always need  this

    }

    public override void doUpdate()
    {
        standardUpdate();
    }

    public override void refill()
    {
        standardRefill();
    }



    public override string asText()
    {
        string stringToReturn = "";

        stringToReturn += conditionsAsText();

        return stringToReturn;
    }

    public override string conditionsAsText()
    {
        string stringToReturn = "";

        stringToReturn += "number of conditions:  " + theConditions.Count;

        foreach (condition condition in theConditions)
        {
            stringToReturn += ", ";
            //stringToReturn += condition.ToString();
            stringToReturn += condition.asTextSHORT();
        }

        return stringToReturn;
    }
    public override string conditionsAsTextDETAIL()
    {
        string stringToReturn = "";

        stringToReturn += "number of conditions:  " + theConditions.Count;

        foreach (condition condition in theConditions)
        {
            stringToReturn += ", ";
            stringToReturn += condition.asText();
        }

        return stringToReturn;
    }


}

public class adHocRandomWanderRefill : adHocPlanRefillThing
{
    GameObject enactorObject;

    public adHocRandomWanderRefill(List<condition> theConditionsIn, planEXE2 theRefillPlanIn, GameObject enactorObjectIn)// : base(theConditions, theRefillPlan) //wtf is this???
    {
        //why is it requiring me to include this?  especially for the weird reason of what INPUT it has.  who cares what input it has???
        this.theConditions = theConditionsIn;
        this.theRefillPlan = theRefillPlanIn;
        this.enactorObject = enactorObjectIn;
    }

    public override void doUpdate()
    {
        if(theCurrentPlan != null && theCurrentPlan.exeList !=null && theCurrentPlan.exeList.Count > 0)
        {
            //Debug.Log("*********************theCurrentPlan.exeList[0].endConditionsMet():  " + theCurrentPlan.exeList[0].endConditionsMet());
            foreach (condition thisCondition in theCurrentPlan.exeList[0].endConditions)
            {
                //Debug.Log(thisCondition.asText());
            }
        }
        standardUpdate();
    }

    public override void refill()
    {

        conditionalPrint("[adHocRandomWanderRefill] RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRefilllll!!!!!!!!!!!!!!");
        theCurrentPlan = asSeries(randomWanderPlan());
        theCurrentPlan.atLeastOnce();  //hmmmm, seems silly to always need  this
    }


    private planEXE2 walkToObject(GameObject target, float offsetRoom = 1.10f)
    {

        //gaggggg what a mess



        if (target == null)
        {
            Debug.Log("target is null, so plan to walk to target is null");
            Debug.Log(target.GetInstanceID());
            return null;
        }
        //give it some room so they don't step on object they want to arrive at!
        //just do their navmesh agent enaction.
        //      navAgent theNavAgent = this.gameObject.GetComponent<navAgent>();
        vect3EXE2 theEXE = (vect3EXE2)theRefillPlan.theEnaction.toEXE(target);  //gaaaaaaahhhhhhhhhh messy
        //theNavAgent.target
        //Debug.Assert(theNavAgent != null);


        //planEXE2 theEXE = new vect3EXE2(theNavAgent, target);
        //      theEXE.endConditions = theRefillPlan.endConditions;
        proximity condition = new proximity(enactorObject, target, offsetRoom * 1.4f);
        //condition.debugPrint = theNavAgent.debugPrint;
        theEXE.endConditions.Add(condition);

        //theEXE.debugPrint = theNavAgent.debugPrint;

        return theEXE;
    }

    public planEXE2 randomWanderPlan()
    {
        //ad-hoc hand-coded plan

        //k, they go to random navpoints, great
        //[though...........those navpoints are never DELETED............]
        //[they should have just ONE "nextNav" object, and just MOVE it around ???]

        //  GameObject target = createNavpointInRandomDirection();
        GameObject placeholderTarget1 = new GameObject();
        moveToRandomNearbyLocation(placeholderTarget1);
        //              enaction anEnaction = walkToTarget(target).theEnaction;
        //              buttonCategories theButtonCategory = anEnaction.gamepadButtonType;
        //              multiPlanAdd(walkToTarget(target), blankMultiPlan());

        return walkToObject(placeholderTarget1);
    }

    private static void moveToRandomNearbyLocation(GameObject theObject)
    {
        float initialDistance = 2f;
        float randomAdditionalDistance = UnityEngine.Random.Range(0, 33);
        theObject.transform.position += new Vector3(initialDistance + randomAdditionalDistance, 0, 0);
        randomAdditionalDistance = UnityEngine.Random.Range(0, 33);
        theObject.transform.position += new Vector3(0, 0, initialDistance + randomAdditionalDistance);
    }


    public override string asText()
    {
        string stringToReturn = "";



        return stringToReturn;
    }






    public override string conditionsAsText()
    {
        string stringToReturn = "";

        stringToReturn += "number of conditions:  " + theConditions.Count;

        foreach (condition condition in theConditions)
        {
            stringToReturn += ", ";
            //stringToReturn += condition.ToString();
            stringToReturn += condition.asTextSHORT();
        }

        return stringToReturn;
    }

    public override string conditionsAsTextDETAIL()
    {
        string stringToReturn = "";

        stringToReturn += "number of conditions:  " + theConditions.Count;

        foreach (condition condition in theConditions)
        {
            stringToReturn += ", ";
            stringToReturn += condition.asText();
        }

        return stringToReturn;
    }



}

public class adHocGoGrabAndEquipRefill : adHocPlanRefillThing
{

    AIHub3 theAIHub;  //wowwwww very adhoc...really just indicates i need to put the code there instead?  or some kind of refactor



    public adHocGoGrabAndEquipRefill(List<condition> theConditionsIn, AIHub3 theAIHubIn)
    {
        theAIHub = theAIHubIn;

        theRefillPlan = asSeries(theAIHub.grabAndEquipPlan2(interType.shoot1)); //i don't need this, but i had some printouts that needed it, so i'm duct taping it together by giving them this

        this.theConditions = theConditionsIn;
        standardRefill();
    }


    public override void doUpdate()
    {
        standardUpdate();
    }

    public override void refill()
    {
        theCurrentPlan = asSeries(theAIHub.grabAndEquipPlan2(interType.shoot1));
        theCurrentPlan.atLeastOnce();  //hmmmm, seems silly to always need  this
    }









    public override string asText()
    {
        return "asText not implemented";
    }

    public override string conditionsAsText()
    {
        return "conditionsAsText not implemented";
    }

    public override string conditionsAsTextDETAIL()
    {
        return "conditionsAsTextDETAIL not implemented";
    }

}




public class agnosticTargetCalc:targetCalculator
{
    targetCalculator theTargetCalc;


    public agnosticTargetCalc(GameObject targeterIn, Vector3 targetPositionVectorIn, float offsetIn = 1.8f)
    {
        theTargetCalc = new staticVectorTargetCalculator(targeterIn, targetPositionVectorIn, offsetIn);
    }

    public agnosticTargetCalc(GameObject targeterIn, GameObject targetIn, float offsetIn = 1.8f)
    {
        theTargetCalc = new movableObjectTargetCalculator(targeterIn, targetIn, offsetIn);
    }

    public override Vector3 realPositionOfTarget()
    {
        return theTargetCalc.realPositionOfTarget();
    }

    public override Vector3 targetPosition()
    {
        return theTargetCalc.targetPosition();
    }
}

public abstract class targetCalculator
{
    public GameObject targeter;
    public float offset = 1.8f;

    public abstract Vector3 targetPosition();
    public abstract Vector3 realPositionOfTarget();

    public string asTextString = "";

    public Vector3 calculateOffsetTargetPosition(GameObject targeter, Vector3 targetPositionVector)
    {

        Vector3 between = targetPositionVector - targeter.transform.position;
        //GameObject placeholderTarget1 = new GameObject();
        Vector3 calculatedOffsetTarget = targetPositionVector - between.normalized * offset;
        //Debug.DrawLine(this.gameObject.transform.position, //placeholderTarget1.transform.position, Color.black, 7f);

        return calculatedOffsetTarget;
    }


    internal string asText()
    {
        return asTextString;
    }
}

public class movableObjectTargetCalculator : targetCalculator
{
    GameObject target;

    public movableObjectTargetCalculator(GameObject targeterIn, GameObject targetIn, float offsetIn = 1.8f)
    {
        targeter = targeterIn;
        target = targetIn;
        offset = offsetIn;
        asTextString= targeterIn.ToString();
    }

    public override Vector3 realPositionOfTarget()
    {
        return target.transform.position;
    }

    public override Vector3 targetPosition()
    {
        //Debug.Log("***********************************  target.GetHashCode():  "+target.GetHashCode());
        return calculateOffsetTargetPosition(targeter, target.transform.position);
    }


}

public class staticVectorTargetCalculator : targetCalculator
{
    Vector3 targetPositionVector;

    public staticVectorTargetCalculator(GameObject targeterIn, Vector3 targetPositionVectorIn, float offsetIn = 1.8f)
    {
        targeter = targeterIn;
        targetPositionVector = targetPositionVectorIn;
        offset = offsetIn;
        asTextString = "static vector:  " + targetPositionVectorIn;
    }

    public override Vector3 realPositionOfTarget()
    {
        return targetPositionVector;
    }

    public override Vector3 targetPosition()
    {
        return calculateOffsetTargetPosition(targeter, targetPositionVector);
    }
}