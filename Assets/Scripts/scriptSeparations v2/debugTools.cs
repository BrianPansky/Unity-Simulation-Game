using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugTools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


}

public class mastLine
{
    public mastLine(GameObject theObject)
    {
        Vector3 thisPoint = theObject.transform.position;
        Debug.DrawLine(thisPoint, (thisPoint + (Vector3.up * 105)), Color.green, 17f);
    }
    public mastLine(Vector3 thisPoint)
    {
        Debug.DrawLine(thisPoint, (thisPoint + (Vector3.up * 105)), Color.green, 17f);
    }
}


//"do at point" plug-in component:

public class makeMastLineAtPoint : doAtPoint
{
    internal override void doIt(Vector3 thisPoint)
    {

        Debug.DrawLine(thisPoint, (thisPoint + (Vector3.up * 105)), Color.white, 17f);
    }
}
public class makeMastLine
{
    public makeMastLine(Vector3 thisPoint)
    {
        Debug.DrawLine(thisPoint, (thisPoint + (Vector3.up * 104)), Color.white, 14f);
    }

    public makeMastLine(Vector3 thisPoint, Color theColor)
    {
        Debug.DrawLine(thisPoint, (thisPoint + (Vector3.up * 104)), theColor, 14f);
    }
}

public class makeShortMastLineAtPoint
{
    public makeShortMastLineAtPoint(Vector3 thisPoint)
    {
        Debug.DrawLine(thisPoint, (thisPoint + (Vector3.up * 4)), Color.white, 14f);
    }

    public makeShortMastLineAtPoint(Vector3 thisPoint, Color theColor)
    {
        Debug.DrawLine(thisPoint, (thisPoint + (Vector3.up * 4)), theColor, 14f);
    }
}


public class debugSwitchboard : MonoBehaviour
{
    public bool ALL = false;

    //just:  what something IS, whether or not it FIRES/succeeds
    public bool enactionShallow = false;
    public bool conditionShallow = false;
    public bool planEXEShallow = false;
    public bool repeaterShallow = false;
    public bool OldFSMShallow = false;


    //inner processes:
    public bool enactionDeep = false;
    public bool conditionDeep = false;
    public bool planEXEDeep = false;
    public bool repeaterDeep = false;
    public bool OldFSMDeep = false;
}



public class nestedLayerDebug
{
    //put one in each layer to manage which level you want to print from, etc
    //it does not need to be input into initialization functions
    //just do this is higher level (after making lower level, or wherever needed):
    //      higherLevelDebugThing.
    //only issue is it then can't debug inside of their initialization functions.......

    int layerNumber = 0;
    int layerToPrintCutoff = 0;
    bool debugPrintBool = false;

    nestedLayerDebug(int desiredLayerDepthToPrint)
    {
        layerToPrintCutoff = desiredLayerDepthToPrint;
    }

    nestedLayerDebug(nestedLayerDebug upperLevelDebugInfo)
    {
        layerNumber = upperLevelDebugInfo.layerNumber + 1;
    }




    void debugDownload(nestedLayerDebug lowerLayerDebugInfo)
    {
        //place this BEFORE wherever debug printouts are needed

        if(lowerLayerDebugInfo == null) { lowerLayerDebugInfo = new nestedLayerDebug(0); }

        lowerLayerDebugInfo.layerNumber = layerNumber + 1;
        lowerLayerDebugInfo.debugPrintBool = debugPrintBool;

    }

    void debugStartPoint(nestedLayerDebug upperLevelDebugInfo)
    {
        //place this BEFORE wherever debug printouts are needed

        layerNumber = upperLevelDebugInfo.layerNumber + 1;
        debugPrintBool = upperLevelDebugInfo.debugPrintBool;

    }

    void conditionalPrint(string toPrint)
    {
        if (debugPrintBool == false) { return; }
        if (layerNumber > layerToPrintCutoff) { return; }

        Debug.Log(toPrint);
    }
}


public class adHocDebuggerForGoGrabPlan: MonoBehaviour
{
    public bool debugPrintBool = false;
    string report = "";
    //string recordOfCurrentEnaction = "";
    string recordOfEnactionThisFrame = "";
    string recordOfEnactionPreviousFrame = "";

    //"true" means going, "false" means stopped
    bool stopStartBoolThisFrame = false;
    bool stopStartBoolPreviousFrame = false;

    public void Update()
    {

        //conditionalPrint("this.GetInstanceID():  " + this.GetInstanceID());
        quickStopStartNote();
        if (reportConditionsMet() == false)
        {

            //conditionalPrint("xxxxxxxxxxxxxxxxxxxxxxxx     (reportConditionsMet() == false)        xxxxxxxxxxxxxxxxxxxxxxxxxxx");
            return;
        }


        conditionalPrint("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@     report        @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        addToReport(recordOfEnactionThisFrame);
        conditionalPrint(report);

        report = "";
    }

    private void quickStopStartNote()
    {
        //"true" means going, "false" means stopped
        if (stopStartBoolThisFrame == false && stopStartBoolPreviousFrame == true)
        {
            conditionalPrint("enaction STOPPED");
        }
        else
        if (stopStartBoolThisFrame == true && stopStartBoolPreviousFrame == false)
        {
            conditionalPrint("enaction STARTED");
        }
    }

    private bool reportConditionsMet()
    {
        //hmm, no, lots if this should be moved to the other funtions?  i think?  like i'm doing in "didRaycastHitCorrectTarget"
        //well, no, i want to prevent ALL reports unless some criteria are met?  maybe?  eh, dunno
        if (debugPrintBool == false) { return false; }


        //conditionalPrint("recordOfEnactionThisFrame:  " + recordOfEnactionThisFrame);
        //conditionalPrint("recordOfEnactionPreviousFrame:  " + recordOfEnactionPreviousFrame);
        //conditionalPrint("recordOfEnactionThisFrame==recordOfEnactionPreviousFrame???" + (recordOfEnactionThisFrame == recordOfEnactionPreviousFrame));
        if (recordOfEnactionThisFrame==recordOfEnactionPreviousFrame) { return false; }

        return true;
    }

    public void recordCurrentEnaction(enaction theEnaction)
    {
        //conditionalPrint("recordCurrentEnaction");
        recordOfEnactionPreviousFrame = recordOfEnactionThisFrame;
        if (theEnaction == null)
        {
            //conditionalPrint("(theEnaction == null)");
            recordOfEnactionThisFrame = "(theEnaction == null)";
            return ;
        }


        //conditionalPrint("1recordOfEnactionPreviousFrame:  " + recordOfEnactionPreviousFrame);
        recordOfEnactionPreviousFrame = recordOfEnactionThisFrame;
        //conditionalPrint("2recordOfEnactionPreviousFrame:  " + recordOfEnactionPreviousFrame);

        //conditionalPrint("**********************************       recordOfEnactionThisFrame = theEnaction.ToString();     ****************************************");
        recordOfEnactionThisFrame = "[''theEnaction.ToString()'' = "+theEnaction.ToString() + "]";
        //conditionalPrint("recordOfEnactionThisFrame:  " + recordOfEnactionThisFrame);
        //conditionalPrint("3recordOfEnactionPreviousFrame:  " + recordOfEnactionPreviousFrame);


        //conditionalPrint("////////////////////////////////////////////////////////");
    }


    public void recordFailedCondition(condition theCondition)
    {
        addToReport("[condition FAILED:  ");
        //  report += theCondition.whyDidItFail();
        //report += theCondition.asTextSHORT();
        addToReport(theCondition.asText());
        addToReport("]");
    }



    /*

    public void didRaycastHitCorrectTarget(GameObject intendedTarget, GameObject whatRaycastHit)
    {
        if(whatRaycastHit == intendedTarget) { return ; }

        report += "[ raycast did NOT hit intended target.  -(intendedTarget:  " + intendedTarget+ " - whatRaycastHit: " + whatRaycastHit +")-";
    }

    public void didRaycastHitCorrectTarget(Vector3 intendedTarget, Vector3 whatRaycastHit, float marginOfError = 3f)
    {

    }

    */


    public void addToReport(string addThisToTheReport)
    {
        //need to change it so it only adds NEW info!  and resets to an empty string?  not creating longer and longer and longer string forever!
        //      report += addThisToTheReport;
    }


    void conditionalPrint(string toPrint)
    {
        if (debugPrintBool == false) { return; }
        //if (layerNumber > layerToPrintCutoff) { return; }

        Debug.Log(toPrint);
    }


}


public class adHocBooleanDeliveryClass
{
    //reeeeeeeallly need a better way to do this.

    public bool theBoolSignal = false;
}



/*
public interface context
{
    //place this interface on classes that nest other classes
    //plug it into those nested classes
    //nested classes etc call this so i can see what the context is when debugging?

    string contextAsString();
}
*/



public class miscDebug:MonoBehaviour
{
    public bool debugPrintBool = false;

    void Update()
    {

        if (debugPrintBool == true)
        {
            tagging2.singleton.printAllTags(this.gameObject);
            //Debug.Log("the name of this OldFSM:  " + );
        }
    }
}

public class contradictionInvestigator2
{
    internal condition handsSwitchToAttack;
    internal objectSetGrabber handsAttackObjectSetForAiming;
    internal objectSetGrabber theRealObjectSet;
    internal objectSetGrabber theVisibleNearObjectSet;
    internal condition objectPermanenceKludgeUsingStickiness;
    internal condition realObjectConditionWithNoStickiness;

    internal void printout()
    {
        if (handsSwitchToAttack == null) { return; }
        Debug.Log("= =     = =     = =     = =     = =     = =     = =     = =     = =     = =     = =     = =     = =     ");
        Debug.Log(">>>>>>>>>>>>>handsSwitchToAttack.met():  " + handsSwitchToAttack.met());
        Debug.Log("handsSwitchToAttack.asText():  " + handsSwitchToAttack.asText());
        Debug.Log("handsSwitchToAttack.asTextAllTheWayDown():  " + handsSwitchToAttack.asTextAllTheWayDown());
        Debug.Log("handsAttackObjectSet.grab().Count:  " + handsAttackObjectSetForAiming.grab().Count);
        Debug.Log("theRealObjectSet.grab().Count:  " + theRealObjectSet.grab().Count);
        Debug.Log("theVisibleNearObjectSet.grab().Count:  " + theVisibleNearObjectSet.grab().Count);
        Debug.Log("objectPermanenceKludgeUsingStickiness.met():  " + objectPermanenceKludgeUsingStickiness.met());
        Debug.Log("realObjectConditionWithNoStickiness.met():  " + realObjectConditionWithNoStickiness.met());
        Debug.Log("_/     _/     _/     _/     _/     _/     _/     _/     _/     _/     _/     _/     _/     _/     _/     ");
    }
}

public class contradictionInvestigator
{
    internal condition handsSwitchToAttack;
    internal objectSetGrabber handsAttackObjectSet;
    internal condition feetSwitchToAttack;
    internal objectSetGrabber feetAttackObjectSet;

    internal void printout()
    {
        if (handsSwitchToAttack == null) { return; }
        Debug.Log(">>>>>>>>>>>>>handsSwitchToAttack.met():  "+handsSwitchToAttack.met());
        Debug.Log("feetSwitchToAttack.met():  " + feetSwitchToAttack.met());
        Debug.Log("handsAttackObjectSet.grab().Count:  " + handsAttackObjectSet.grab().Count);
        Debug.Log("feetAttackObjectSet.grab().Count:  " + feetAttackObjectSet.grab().Count);
    }
}