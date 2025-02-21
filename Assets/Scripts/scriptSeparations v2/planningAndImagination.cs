using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using static enactionCreator;
using static tagging2;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

public class planningAndImagination : MonoBehaviour
{
    //keep in mind that planning is only about causality/logic.  which things CAN be done, in which order, to get an outcome
    //the EVALUATION of which plan is best [or even "good enough"] has to happen elsewhere

    //also, plans and following plans are not the same as plan-"ing"


    public planEXE2 fullPlan;


}

public class makeImaginary
{
    //is this a bad idea?  will it take up memory????  or not if i don't assign it to a variable? 
    public makeImaginary(GameObject theObject)
    {
        int Layer = LayerMask.NameToLayer("imagination"); //eleven
        //theObject.layer = Layer;
        SetLayerRecursively(theObject, Layer);
    }

    public static void SetLayerRecursively(GameObject theObject, int layerNumber)
    {
        //from https://discussions.unity.com/t/change-gameobject-layer-at-run-time-wont-apply-to-child/381215/10
        
        if (theObject == null) return;
        foreach (Transform trans in theObject.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}


public class beleifs : MonoBehaviour
{
    //public updateableSetGrabber theSet;
    //public updateableSetGrabber threatMarkerSet;
    //public Dictionary<criteria, thingBeleif> thingBeleifsByCriteria = new Dictionary<Type, thingBeleif>();
    perceptions thePerceptions;
    public List<updateableSetGrabber> beleifObjectSets = new List<updateableSetGrabber>();

    internal static beleifs addThisComponent(GameObject theObject)
    {
        beleifs theComponent = theObject.AddComponent<beleifs>();
        //theComponent.theSet = new threatBeleifSet1(theObject);
        //theComponent.threatMarkerSet = new threatBeleifSet1(theObject);

        return theComponent;
    }

    internal static beleifs ensureComponent(GameObject theObject)
    {
        beleifs theComponent = theObject.GetComponent<beleifs>();
        if (theComponent == null)
        {
            theComponent = beleifs.addThisComponent(theObject);
        }

        return theComponent;
    }



    /*
    void Update()
    {
        //threatMarkerSet.updateSet(thePerceptions.setOfAllCurrentlySensedObjects.grab());  //bit inefficient conversion

        //List<objectIdPair> theIDs = thePerceptions.setOfAllCurrentlySensedObjects.theStoredSet;//grab();

        foreach (var beleifSet in beleifObjectSets)
        {
            beleifSet.updateSet(thePerceptions.setOfAllCurrentlySensedObjects.theStoredSet);
        }
    }
    */

    internal void weDetectedThisObject(GameObject thisObject)
    {

        foreach (var beleifSet in beleifObjectSets)
        {
            beleifSet.updateSetWithOneObject(thisObject);
        }
    }

    internal void sensoryInput(List<GameObject> inputList)
    {
        foreach (var beleifSet in beleifObjectSets)
        {
            beleifSet.updateSet(inputList);
        }
    }

    /*
    internal void sensoryInput(List<GameObject> inputList)
    {
        //update a "set grabber" with this
        //buuut....that's not how set grabbers work......
        //"updatable set grabber"???
        threatMarkerSet.updateSet(inputList);
    }

    internal void weDetectedThisObject(GameObject thisObject)//or do we want to have a class/interface called "beleif", and we just add or update a "beleif" to a list or something?????
    {
        threatMarkerSet.updateSetWithOneObject(thisObject);
    }
    */

}

public class objectsMeetingCriteriaBeleifSet1 : updateableSetGrabber
{
    //Dictionary<objectIdPair, condition> theForgetConditions = new Dictionary<objectIdPair, condition>();
    Dictionary<objectIdPair, GameObject> idPairsLinkedToShadowObjects = new Dictionary<objectIdPair, GameObject>();

    objectCriteria theCriteria;  //criteria for inclusion in this beleif set
    threatPerceptionSet1 objectsSeenThisFrame;

    public objectsMeetingCriteriaBeleifSet1(GameObject theObjectWithBeleifs, objectCriteria theCriteriaIn)
    {
        theCriteria = theCriteriaIn;
        beleifs theBeleifs = theObjectWithBeleifs.GetComponent<beleifs>();
        theBeleifs.beleifObjectSets.Add(this);
    }

    public override List<GameObject> grab()
    {
        return allShadowObjects();
    }

    private List<GameObject> allShadowObjects()
    {
        List<GameObject> newList = new List<GameObject>();
        foreach (objectIdPair thisID in idPairsLinkedToShadowObjects.Keys)
        {
            newList.Add(idPairsLinkedToShadowObjects[thisID]);
        }
        return newList;
    }


    public override void updateSet(List<GameObject> inputList)
    {
        foreach (objectIdPair thisID in convertToIds(inputList))
        {
            updateSetWithOneID(thisID, theCriteria);
        }
    }
    public override void updateSet(List<objectIdPair> inputList)
    {
        foreach (objectIdPair thisID in inputList)
        {
            updateSetWithOneID(thisID, theCriteria);
        }
    }

    private void updateSetWithOneID(objectIdPair thisID, objectCriteria theCriteria)
    {
        //if (theStoredSet.Contains(thisID)){ continue; }

        //theStoredSet.Add(thisID);

        //Debug.Log("updateSetWithOneID:  " + thisID.theObject);
        if (theCriteria.evaluateObject(thisID.theObject) == false)
        {
            //Debug.Log("criteria NOT met"); 
            return;
        }

        //Debug.Log("criteria met:  " + thisID.theObject);
        ensureDictionaryEntry(thisID);

        updateTransform(thisID.theObject, idPairsLinkedToShadowObjects[thisID]);
    }

    private void ensureDictionaryEntry(objectIdPair thisID)
    {
        if (idPairsLinkedToShadowObjects.ContainsKey(thisID)) { return; }

        idPairsLinkedToShadowObjects[thisID] = new GameObject();
    }


    internal override void updateSetWithOneObject(GameObject thisObject)
    {
        updateSetWithOneID(tagging2.singleton.idPairGrabify(thisObject), theCriteria);
    }


    private void updateTransform(GameObject realObject, GameObject shadowObject)
    {
        shadowObject.transform.position = realObject.transform.position;
        shadowObject.transform.rotation = realObject.transform.rotation;
    }

}



public class thingBeleif
{
    objectGen thingRecreator;
    GameObject theShadowObjectOrMarker;

}















public class threatBeleifSet1 : updateableSetGrabber
{
    //Dictionary<objectIdPair, condition> theForgetConditions = new Dictionary<objectIdPair, condition>();
    Dictionary<objectIdPair, GameObject> idPairsLinkedToShadowObjects = new Dictionary<objectIdPair, GameObject>();

    objectCriteria theCriteria;  //criteria for inclusion in this beleif set
    threatPerceptionSet1 objectsSeenThisFrame;

    public threatBeleifSet1(tag2 teamIn)
    {

        objectCriteria theThreatObjectCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(teamIn))
            );

        theCriteria = theThreatObjectCriteria;
    }
    public threatBeleifSet1(GameObject theObject)
    {

        tag2 team = tagging2.singleton.teamOfObject(theObject);

        objectCriteria theThreatObjectCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team))
            );

        theCriteria = theThreatObjectCriteria;
    }

    public override List<GameObject> grab()
    {
        forgetListItemsIfNecessary();
        return allShadowObjects();//convertToObjects(theStoredSet);

    }

    private List<GameObject> allShadowObjects()
    {
        List < GameObject > newList = new List<GameObject>();
        foreach (objectIdPair thisID in idPairsLinkedToShadowObjects.Keys)
        {
            newList.Add(idPairsLinkedToShadowObjects[thisID]);
        }
        return newList;
    }

    private void forgetListItemsIfNecessary()
    {
        //ummm do this later
        //foreach (objectIdPair thisID in theStoredSet)
        {
            //if (theForgetConditions.Keys.Contains(thisID)) { continue; }

        }
    }

    public override void updateSet(List<GameObject> inputList)
    {
        foreach (objectIdPair thisID in convertToIds(inputList))
        {
            updateSetWithOneID(thisID, theCriteria);
        }
    }

    private void updateSetWithOneID(objectIdPair thisID, objectCriteria theCriteria)
    {
        //if (theStoredSet.Contains(thisID)){ continue; }

        //theStoredSet.Add(thisID);

        //Debug.Log("updateSetWithOneID:  " + thisID.theObject);
        if (theCriteria.evaluateObject(thisID.theObject) == false)
        {
            //Debug.Log("criteria NOT met"); 
            return; }

        //Debug.Log("criteria met:  " + thisID.theObject);
        ensureDictionaryEntry(thisID);

        updateTransform(thisID.theObject, idPairsLinkedToShadowObjects[thisID]);
    }

    private void ensureDictionaryEntry(objectIdPair thisID)
    {
        if (idPairsLinkedToShadowObjects.ContainsKey(thisID)) { return; }

        idPairsLinkedToShadowObjects[thisID] = new GameObject();
    }


    internal override void updateSetWithOneObject(GameObject thisObject)
    {
        updateSetWithOneID(tagging2.singleton.idPairGrabify(thisObject), theCriteria);
    }


    private void updateTransform(GameObject realObject, GameObject shadowObject)
    {
        shadowObject.transform.position = realObject.transform.position;
        shadowObject.transform.rotation = realObject.transform.rotation;
    }

    public override void updateSet(List<objectIdPair> inputList)
    {
        throw new NotImplementedException();
    }
}




public class FSMcomponent : MonoBehaviour, IupdateCallable
{
    //PRIVATE BECAUSE THEY REQUIRE "SETUP"
    private List<FSM> theFSMList = new List<FSM>();// = new List<FSM>();  //correct way to do parallel!  right at the top level!!!  one for walking/feet, one for hands/equipping/using items etc.

    public List<IupdateCallable> currentUpdateList { get; set; }

    public void callableUpdate()
    {
        for (int index = 0; index < theFSMList.Count; index++)
        {
            theFSMList[index] = theFSMList[index].doAFrame();
        }

    }

    public void addAndSetupFSM(FSM theFSM)
    {
        theFSM.setup(this.gameObject);
        theFSMList.Add(theFSM);
    }
}

public class FSM
{
    internal Dictionary<condition, FSM> switchBoard = new Dictionary<condition, FSM>();

    internal List<state> stuffToDoInThisState = new List<state>();
    public string name;

    public FSM()
    {

    }

    public FSM(state baseStateIn)
    {
        stuffToDoInThisState.Add(baseStateIn);
    }


    public FSM doAFrame()
    {
        FSM toSwitchTo = firstMetSwitchtCondition();
        if (toSwitchTo != null)
        {
            return toSwitchTo;
        }





        foreach (var thisThingToDo in stuffToDoInThisState)
        {
            thisThingToDo.doThisThing();
        }




        return this;
    }

    
    internal void setup(GameObject gameObject)
    {
        //so, setup all states, and all other linked FSMs?  BUT THAT'S AN INFINITE LOOP!
        //soooo ONLY set up THIS FSM, the others need to be set up.......some other time........somehow....not sure how that will work....
        foreach(var thisState in stuffToDoInThisState)
        {
            thisState.myConstructor(gameObject);
        }
    }
    

    private FSM firstMetSwitchtCondition()
    {
        foreach (condition thisCondition in switchBoard.Keys)
        {
            if (thisCondition.met())
            {
                return switchBoard[thisCondition];
            }
        }

        return null;
    }

    /*
    internal FSM generate(GameObject theNPC)
    {
        FSM deepCopy = new FSM();

        deepCopy.addDeepCopyOfStates(theNPC, stuffToDoInThisState);
        deepCopy.addDeepCopyOfSwitchBoard(theNPC, switchBoard);  //NO WAIT it will AGAIN do an infinite loop!

        return deepCopy;
    }

    private void addDeepCopyOfStates(GameObject theNPC, List<state> stuffToDoInThisStateIn)
    {
        foreach(var thisState in stuffToDoInThisStateIn)
        {
            stuffToDoInThisState.Add(thisState.generate(theNPC));
        }
    }
    */
}

public interface state
{
    //arbitrary code to execute in a state [for FSM]
    void doThisThing();

    //setup should do most of constructor?  anything PARTICUALR to a specific NPC
    //regenerate must have all of the "universal" code/classes so that the particular NPC can be PLUGGED IN, a new NPC every time we need to
    void myConstructor(GameObject theObjectDoingTheEnactionIn);
    state reConstructor(GameObject theObjectDoingTheEnactionIn);


    //example:
    /*
    public state reConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        state newState = new doSimplePatrolState1();
        newState.myConstructor(theObjectDoingTheEnactionIn);
        return newState;
    }

    public void myConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        theObjectDoingTheEnaction = theObjectDoingTheEnactionIn;
        theNavAgent = theObjectDoingTheEnaction.GetComponent<navAgent>();

        GameObject p1 = new GameObject();
        p1.transform.position = theObjectDoingTheEnaction.transform.position + new Vector3(10, 0, 6);
        GameObject p2 = new GameObject();
        p2.transform.position = theObjectDoingTheEnaction.transform.position + new Vector3(-10, 0, 16);
        GameObject p3 = new GameObject();
        p3.transform.position = theObjectDoingTheEnaction.transform.position + new Vector3(10, 0, -6);
        GameObject p4 = new GameObject();
        p4.transform.position = theObjectDoingTheEnaction.transform.position + new Vector3(-10, 0, -16);
        theListOfLocationMarkers.Add(p1);
        theListOfLocationMarkers.Add(p2);
        theListOfLocationMarkers.Add(p3);
        theListOfLocationMarkers.Add(p4);



        makeProxConditionForFirstItemOnList();
    }

    */
}


/*
public class repeaterState : state
{
    //basically the states of "OldFSM"

    //internal List<repeater> repeatingPlans = new List<repeater>();
    repeater theRepeater;

    public void doThisThing()
    {
        /*
        foreach (repeater plan in repeatingPlans)
        {
            //Debug.Log("plan:  " + plan);
            plan.doThisThing();
        }
        
        theRepeater.doThisThing();
    }


    /*
    public void refillAllRepeaters()
    {
        foreach (repeater plan in repeatingPlans)
        {
            plan.refill();
        }
    }
    




    public state reConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        //theRepeater.refill();
        state newState = new repeaterState(theRepeater);
        newState.myConstructor(theObjectDoingTheEnactionIn);
        return newState;
    }

    public void myConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        //wait, we can't do this with repeaters.....that's why i made new fsm system....
        throw new NotImplementedException();
    }


}

*/


/*
public class giveAdvancedRTSCommands : state
{
    generateFSM commandFSMToGive;
    objectSetGrabber unitsToGiveOrdersTo;
    public void doThisThing()
    {
        giveAdvancedCommandToMembersOfObjectSet(commandFSMToGive, unitsToGiveOrdersTo);
    }

    public state reConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        throw new NotImplementedException();
    }

    public void myConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        throw new NotImplementedException();
    }

    private void giveAdvancedCommandToMembersOfObjectSet(generateFSM theCommandFSMToGive, objectSetGrabber grabUnitsToGiveOrdersTo)
    {
        foreach (GameObject unit in grabUnitsToGiveOrdersTo.grab())
        {
            advancedRtsModule theComponent = unit.GetComponent<advancedRtsModule>();

            theComponent.currentReceivedCommand = theCommandFSMToGive;//.generate(unit);
        }
    }
}
*/


public class doSimplePatrolState1 : state
{
    List<GameObject> theListOfLocationMarkers = new List<GameObject>();
    GameObject theObjectDoingTheEnaction;
    navAgent theNavAgent;
    condition proxCondition;


    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    /*
    maybe like this BUT NOT HERE KEEP STATE SIMPLE?: ConstructorForOldFSMgen2ThatUsesPLUGINS(GameObject theObjectDoingTheEnactionIn, baseState state0, plugInOldFSM[] plugInOldFSMs) : this(theObjectDoingTheEnactionIn, state0)
    {
        theBaseOldFSM = state0.makeState(theObjectDoingTheEnactionIn);

        foreach (plugInOldFSM thisPlugin in plugInOldFSMs)
        {
            thisPlugin.addPlugin(theBaseOldFSM, theObjectDoingTheEnactionIn);
        }

        addItToTheirOldFSMComponent(theObjectDoingTheEnactionIn);
    }

    */

    public void doThisThing()
    {
        //how to go to one until proximity to it, then another, then another?
        //well, start with first point on list
        //[when done with it, remove it and add it to end of list]
        //i have a bunch of "goToX" code........


        //old code basically boils down thos this, i think:
        //      gets nav agent enaction
        //      adds proximity condition to enaction/exe
        //      gets target position
        //      uses target position as input data for the vector enaction [nav agent enaction]
        //see:
        //      navAgent theNavAgent = theObjectDoingTheEnaction.GetComponent<navAgent>();
        //      proximityRef condition = new proximityRef(theObjectDoingTheEnaction, theEXE, offsetRoom);
        //      theEXE.endConditions.Add(condition);
        //      Vector3 targetPosition = theTargetCalculator.targetPosition();
        //      theEnaction.enact(new inputData(targetPosition));

        handleProxCondition();
        theNavAgent.enact(new inputData(theListOfLocationMarkers[0]));
    }

    public state reConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        state newState = new doSimplePatrolState1();
        newState.myConstructor(theObjectDoingTheEnactionIn);
        return newState;
    }

    public void myConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        theObjectDoingTheEnaction = theObjectDoingTheEnactionIn;
        theNavAgent = theObjectDoingTheEnaction.GetComponent<navAgent>();

        GameObject p1 = new GameObject();
        p1.transform.position = theObjectDoingTheEnaction.transform.position + new Vector3(10, 0, 6);
        GameObject p2 = new GameObject();
        p2.transform.position = theObjectDoingTheEnaction.transform.position + new Vector3(-10, 0, 16);
        GameObject p3 = new GameObject();
        p3.transform.position = theObjectDoingTheEnaction.transform.position + new Vector3(10, 0, -6);
        GameObject p4 = new GameObject();
        p4.transform.position = theObjectDoingTheEnaction.transform.position + new Vector3(-10, 0, -16);
        theListOfLocationMarkers.Add(p1);
        theListOfLocationMarkers.Add(p2);
        theListOfLocationMarkers.Add(p3);
        theListOfLocationMarkers.Add(p4);



        makeProxConditionForFirstItemOnList();
    }

    private void handleProxCondition()
    {
        //looks to see if they are close to first point
        //if so, moves first point of list to the end
        //then makes new prox condition using that new first point
        if (proxCondition.met())
        {
            moveFirstItemInListToTheEnd();
            makeProxConditionForFirstItemOnList();
        }
    }

    private void makeProxConditionForFirstItemOnList()
    {
        proxCondition = new proximity(theObjectDoingTheEnaction, theListOfLocationMarkers[0], 0,3);
    }

    private void moveFirstItemInListToTheEnd()
    {
        GameObject item1 = theListOfLocationMarkers[0];
        theListOfLocationMarkers.RemoveAt(0);
        theListOfLocationMarkers.Add(item1);

    }
}


public class followAdvancedCommands : state
{
    private advancedRtsModule theComponent;
    private FSM nestedFSM;

    //private string myConstructorTrace;
    private generateFSM mostRecentlyReceivedCommand;

    public followAdvancedCommands()
    {

        //Debug.Log("----------------------------  constructor");
        //myConstructorTrace = new System.Diagnostics.StackTrace(true).ToString();
    }

    public void doThisThing()
    {
        updateThisFSMifThereAreNewOrders();

        if(nestedFSM != null)
        {
            nestedFSM.doAFrame();
        }


        /*
        Debug.Log("----------------------------  doThisThing()");
        Debug.Log("(theComponent != null):  " + (theComponent != null));
        //Debug.Assert(theComponent != null);
        if (theComponent == null)
        {
            Debug.Log("this.GetHashCode()" + this.GetHashCode());
            Debug.Log(myConstructorTrace);
        }
        



        if(theComponent.currentReceivedCommand != null)
        {
            //theComponent.currentReceivedCommand.doAFrame();
        }

        */
    }

    private void updateThisFSMifThereAreNewOrders()
    {
        if(theComponent.currentReceivedCommand != mostRecentlyReceivedCommand)
        {
            mostRecentlyReceivedCommand = theComponent.currentReceivedCommand;
            nestedFSM = mostRecentlyReceivedCommand.generateTheFSM(theComponent.gameObject);
        }
    }

    public void myConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        //Debug.Log("----------------------------  setup");
        //  OH WOW THERE'S MY ERROR!  I CAN'T BELEIVE IT DIDN"T WARN ME I CREATED A LOCAL VARIABLE WITH SAME NAME AS ANOTHER VARIABLE IN THIS CLASS AAAAAAAAA:   advancedRtsModule theComponent = theObjectDoingTheEnactionIn.GetComponent<advancedRtsModule>();
        theComponent = theObjectDoingTheEnactionIn.GetComponent<advancedRtsModule>();

        /*
        Debug.Assert(theComponent != null);
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        Debug.Log("this.GetHashCode()" + this.GetHashCode());
        Debug.Log("theComponent:  " + theComponent);
        Debug.Log("(theComponent != null):  " + (theComponent != null));
        */
    }
    
    public state reConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        //Debug.Log("----------------------------  88888888888888 just in case, reConstructor");
        state newState = new followAdvancedCommands();
        newState.myConstructor(theObjectDoingTheEnactionIn);
        return newState;
    }

    /*
    internal void test()
    {
        Debug.Log("----------------------------  test() ******************");
        Debug.Log("(theComponent != null):  " + (theComponent != null));
        //Debug.Assert(theComponent != null);
        if (theComponent == null)
        {
            Debug.Log("this.GetHashCode()" + this.GetHashCode());
            Debug.Log(myConstructorTrace);
        }
    }
    */
}


public class giveSquadXAdvancedCommandY : state
{
    generateFSM commandFSMToGive;
    objectSetGrabber unitsToGiveOrdersTo;
    private tag2 squad;
    private tag2 team;

    public giveSquadXAdvancedCommandY(tag2 teamIn, tag2 squadIn, state commandedStateIn)
    {
        squad = squadIn;
        team = teamIn;

        commandFSMToGive = new baseStateGen(commandedStateIn);// new FSM(commandedStateIn);
    }
    public giveSquadXAdvancedCommandY(tag2 teamIn, tag2 squadIn, generateFSM commandFSMToGiveIn)
    {
        squad = squadIn;
        team = teamIn;

        commandFSMToGive = commandFSMToGiveIn;
    }


    public state reConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        state newState = new giveSquadXAdvancedCommandY(team,squad, commandFSMToGive);
        newState.myConstructor(theObjectDoingTheEnactionIn);
        return newState;
    }

    public void doThisThing()
    {
        giveAdvancedCommandToMembersOfObjectSet(commandFSMToGive, unitsToGiveOrdersTo);
    }

    public void myConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new objectHasTag(squad),
            new reverseCriteria(new objectHasTag(tag2.teamLeader)),
            new hasAdvancedRtsModule(),
            new hasNoAdvancedCommands()
            );

        unitsToGiveOrdersTo = new setOfAllObjectThatMeetCriteria(new setOfAllObjectsWithTag(team), theCriteria);
    }

    public void initializeCommandFSMToGive()
    {
    }

    private void giveAdvancedCommandToMembersOfObjectSet(generateFSM theCommandFSMToGive, objectSetGrabber grabUnitsToGiveOrdersTo)
    {
        foreach (GameObject unit in grabUnitsToGiveOrdersTo.grab())
        {
            advancedRtsModule theComponent = unit.GetComponent<advancedRtsModule>();
            theComponent.currentReceivedCommand = theCommandFSMToGive;
        }
    }
}


public class goToTargetPickerState : state
{
    GameObject theObjectDoingTheEnaction;
    navAgent theNavAgent;
    targetPicker theTargetPicker;

    public goToTargetPickerState(targetPicker theTargetPickerIn)
    {
        theTargetPicker = theTargetPickerIn;
    }


    public void doThisThing()
    {
        theNavAgent.enact(new inputData(theTargetPicker.pickNext().realPositionOfTarget()));
    }

    public state reConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        state newState = new goToTargetPickerState(theTargetPicker);
        newState.myConstructor(theObjectDoingTheEnactionIn);
        return newState;
    }

    public void myConstructor(GameObject theObjectDoingTheEnactionIn)
    {
        theObjectDoingTheEnaction = theObjectDoingTheEnactionIn;
        theNavAgent = theObjectDoingTheEnaction.GetComponent<navAgent>();
    }
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
        //Debug.Log("looking at end conditions for:  " + this.asText());

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
            //Debug.Log("thisCondition:  " + thisCondition);
            //if (theEnaction != null) { Debug.Log("thisCondition:  " + thisCondition); }
            if (thisCondition.met() == false) 
            {
                //conditionalPrint("this end condition not met:  "+ thisCondition);
                //Debug.Log("this end condition not met:  " + thisCondition);
                return false; 
            }
        }
        //Debug.Log("no conditions remain unfulfilled!");

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


    public boolEXE2(IEnactaBool theInputEnaction, GameObject theTarget) //target is not used at all!!!!!!!!!
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

        theTargetCalculator = new movableObjectTargetCalculator(this.theEnaction.transform.gameObject, possiblyMobileActualTargetIn);//, offsetRoomIn);
    }

    public vect3EXE2(IEnactByTargetVector theInputEnaction, targetPicker targetPickerIn, float offsetRoomIn = 1.8f)
    {
        this.theEnaction = theInputEnaction;
        //this.possiblyMobileActualTarget = possiblyMobileActualTargetIn;
        //this.offsetRoom = offsetRoomIn;

        //theTargetCalculator = new targetCalculatorFromPicker(this.theEnaction.transform.gameObject, targetPickerIn, offsetRoomIn);
        theTargetCalculator = new targetCalculatorFromPicker(targetPickerIn, offsetRoomIn);
    }


    public vect3EXE2(IEnactByTargetVector theInputEnaction, Vector3 stationaryTargetAsVectorIn, float offsetRoomIn = 1.8f)
    {
        this.theEnaction = theInputEnaction;
        //this.stationaryTargetAsVector = stationaryTargetAsVectorIn;
        //this.offsetRoom = offsetRoomIn;
        Debug.Assert(this != null);
        Debug.Assert(this.theEnaction != null);
        Debug.Assert(this.theEnaction.transform != null);
        Debug.Assert(this.theEnaction.transform.gameObject != null);
        theTargetCalculator = new staticVectorTargetCalculator(this.theEnaction.transform.gameObject, stationaryTargetAsVectorIn);//, offsetRoomIn);

    }

    public vect3EXE2(IEnactaVector theInputEnaction, GameObject possiblyMobileActualTargetIn, float offsetRoomIn = 1.8f)
    {
        //!!!!!!!!!!!!! used to fix this error:
        //cannot convert from 'IEnactaVector' to 'IEnactByTargetVector'
        //!!!!!!!!!!!


        this.theEnaction = theInputEnaction;
        //this.possiblyMobileActualTarget = theTarget;
        theTargetCalculator = new movableObjectTargetCalculator(this.theEnaction.transform.gameObject, possiblyMobileActualTargetIn);//, offsetRoomIn);

    }


    public override void execute()
    {

        if (standardExecuteErrors()) { return; }

        //conditionalPrint("should enact this:  " + this.theEnaction);


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
        Vector3 targetPosition = theTargetCalculator.targetPosition();
        //Debug.Log("(theTargetCalculator:  " + theTargetCalculator);
        //Debug.Log("(targetPosition:  " + targetPosition);
        //new mastLine(targetPosition);
        theEnaction.enact(new inputData(targetPosition));
        //executeInputData(new inputData());
        numberOfTimesExecuted++;
        //conditionalPrint("aaaaaa.2222222222222bbbbbbb theRefillPlan.nestedPlanCountToText():  " + nestedPlanCountToText());
    }




    public override bool error()
    {
        return targetError();
    }

    public bool targetError()
    {

        return theTargetCalculator.error();
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

public class simultaneousEXE : singleEXE
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

        theRefillPlan = asSeries(theAIHub.grabAndEquipPlan2(interType.peircing)); //i don't need this, but i had some printouts that needed it, so i'm duct taping it together by giving them this

        this.theConditions = theConditionsIn;
        standardRefill();
    }


    public override void doUpdate()
    {
        standardUpdate();
    }

    public override void refill()
    {
        theCurrentPlan = asSeries(theAIHub.grabAndEquipPlan2(interType.peircing));
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


    public agnosticTargetCalc(GameObject targeterIn, Vector3 targetPositionVectorIn)//, float offsetIn = 1.8f)
    {
        theTargetCalc = new staticVectorTargetCalculator(targeterIn, targetPositionVectorIn);//, offsetIn);
    }

    public agnosticTargetCalc(GameObject targeterIn, GameObject targetIn)//, float offsetIn = 1.8f)
    {
        theTargetCalc = new movableObjectTargetCalculator(targeterIn, targetIn);//, offsetIn);
    }
    public agnosticTargetCalc(Vector3 targetPositionVectorIn)//, float offsetIn = 1.8f)
    {
        theTargetCalc = new staticSingleVectorTargetCalculator(targetPositionVectorIn);//, offsetIn);
    }

    public agnosticTargetCalc(GameObject targetIn)//, float offsetIn = 1.8f)
    {
        theTargetCalc = new singleMovableObjectTargetCalculator(targetIn);//, offsetIn);
    }

    public override Vector3 realPositionOfTarget()
    {
        return theTargetCalc.realPositionOfTarget();
    }

    public override Vector3 targetPosition()
    {
        return theTargetCalc.targetPosition();
    }


    public override bool error()
    {
        return theTargetCalc.error();
    }

    internal override Quaternion realRotationOfTarget()
    {
        return theTargetCalc.realRotationOfTarget();
    }
}

public abstract class targetCalculator
{
    public GameObject targeter;
    public bool isThereAnError = false;

    public abstract Vector3 targetPosition();
    public abstract Vector3 realPositionOfTarget();

    public string asTextString = "";


    /*
    public Vector3 calculateOffsetTargetPosition(GameObject targeter, Vector3 targetPositionVector)
    {

        Vector3 between = targetPositionVector - targeter.transform.position;
        //GameObject placeholderTarget1 = new GameObject();
        Vector3 calculatedOffsetTarget = targetPositionVector - between.normalized * offset;
        //Debug.DrawLine(this.gameObject.transform.position, //placeholderTarget1.transform.position, Color.black, 7f);

        return calculatedOffsetTarget;
    }
    */

    internal string asText()
    {
        return asTextString;
    }


    public abstract bool error();


    internal abstract Quaternion realRotationOfTarget();
}

public class movableObjectTargetCalculator : targetCalculator
{
    GameObject target;

    public movableObjectTargetCalculator(GameObject targeterIn, GameObject targetIn)//, float offsetIn = 1.8f)
    {
        targeter = targeterIn;
        target = targetIn;
        //offset = offsetIn;
        asTextString = targeterIn.ToString();
    }

    public override bool error()
    {
        return (target == null);
    }

    public override Vector3 realPositionOfTarget()
    {
        //ad-hoc duct tape!
        if (target == null)
        {
            //return Vector3.zero;
            return targeter.transform.position;
        }

        return target.transform.position;
    }

    public override Vector3 targetPosition()
    {
        //Debug.Log("***********************************  target.GetHashCode():  "+target.GetHashCode());

        //ad-hoc duct tape!
        if (target == null)
        {
            //return Vector3.zero;
            return targeter.transform.position;
        }
        //return calculateOffsetTargetPosition(targeter, target.transform.position);
        return target.transform.position;
    }

    internal override Quaternion realRotationOfTarget()
    {
        return target.transform.rotation;
    }
}

public class staticVectorTargetCalculator : targetCalculator
{
    Vector3 targetPositionVector;

    public staticVectorTargetCalculator(GameObject targeterIn, Vector3 targetPositionVectorIn)//, float offsetIn = 1.8f)
    {
        targeter = targeterIn;
        targetPositionVector = targetPositionVectorIn;
        //offset = offsetIn;
        asTextString = "static vector:  " + targetPositionVectorIn;
    }

    public override Vector3 realPositionOfTarget()
    {
        return targetPositionVector;
    }

    public override Vector3 targetPosition()
    {
        //return calculateOffsetTargetPosition(targeter, targetPositionVector);
        return targetPositionVector;
    }

    public override bool error()
    {
        return false;
    }

    internal override Quaternion realRotationOfTarget()
    {
        return Quaternion.identity;
    }
}

public class singleMovableObjectTargetCalculator : targetCalculator
{
    //"single" as in "no TARGETER, just the TARGET"
    GameObject target;
    private string myConstructorTrace;

    public singleMovableObjectTargetCalculator(GameObject targetIn)//, float offsetIn = 1.8f)
    {
        myConstructorTrace = new System.Diagnostics.StackTrace(true).ToString();
        target = targetIn;
        //offset = offsetIn;
    }

    public override bool error()
    {
        return (target == null);
    }

    public override Vector3 realPositionOfTarget()
    {
        //ad-hoc duct tape!
        if (target == null)
        {
            isThereAnError = true;
            Debug.Log(myConstructorTrace);
            return Vector3.zero;
            //return targeter.transform.position; //NO THIS VERSION HAS NO TARGETER!!!!!!!!!!
        }

        return target.transform.position;
    }

    public override Vector3 targetPosition()
    {
        //Debug.Log("***********************************  target.GetHashCode():  "+target.GetHashCode());

        //ad-hoc duct tape!
        if (target == null)
        {
            isThereAnError = true;
            Debug.Log(myConstructorTrace);
            return Vector3.zero;
            //return targeter.transform.position; //NO THIS VERSION HAS NO TARGETER!!!!!!!!!!
        }
        return realPositionOfTarget();
    }

    internal override Quaternion realRotationOfTarget()
    {//ad-hoc duct tape!
        if (target == null)
        {
            isThereAnError = true;
            Debug.Log(myConstructorTrace);
            return Quaternion.identity;
            //return targeter.transform.rotation;//NO THIS VERSION HAS NO TARGETER!!!!!!!!!!
        }
        return target.transform.rotation;
    }
}

public class staticSingleVectorTargetCalculator : targetCalculator
{
    Vector3 targetPositionVector;

    public staticSingleVectorTargetCalculator(Vector3 targetPositionVectorIn)//, float offsetIn = 1.8f)
    {
        targetPositionVector = targetPositionVectorIn;
        //offset = offsetIn;
        asTextString = "static vector:  " + targetPositionVectorIn;
    }

    internal override Quaternion realRotationOfTarget()
    {
        return Quaternion.identity;
    }
    public override Vector3 realPositionOfTarget()
    {
        return targetPositionVector;
    }

    public override Vector3 targetPosition()
    {
        return realPositionOfTarget();
    }

    public override bool error()
    {
        return false;
    }
}

public class targetCalculatorFromPicker : targetCalculator
{
    targetPicker theTargetPicker;
    targetCalculator theCalculatorForCurrentTarget;

    private targetPicker targetPickerIn;
    private float offsetRoomIn;

    public targetCalculatorFromPicker(targetPicker targetPickerIn, float offsetRoomIn)
    {
        this.targetPickerIn = targetPickerIn;
        this.offsetRoomIn = offsetRoomIn;
    }

    internal void updateTargetIfNecessary()
    {

        if (theCalculatorForCurrentTarget.error())
        {
            theCalculatorForCurrentTarget = theTargetPicker.pickNext();
            //theCalculatorForCurrentTarget.offset = offsetRoomIn; //messyyyyy
        }
    }

    public override bool error()
    {
        updateTargetIfNecessary();


        if (theCalculatorForCurrentTarget.error())
        {
            return true;
        }

        return false;
    }

    public override Vector3 realPositionOfTarget()
    {
        updateTargetIfNecessary();

        return theCalculatorForCurrentTarget.realPositionOfTarget();
    }

    public override Vector3 targetPosition()
    {
        updateTargetIfNecessary();

        return theCalculatorForCurrentTarget.targetPosition();
    }

    internal override Quaternion realRotationOfTarget()
    {
        updateTargetIfNecessary();

        return theCalculatorForCurrentTarget.realRotationOfTarget();
    }
}












public class permaPlan2
{
    //boil things down so it's EASY to generate [other] abstract classes
    //by simply inputting just [lists of?] enactions and conditions?

    //and make a simple class [this one] that JUST holds one set of enactions and conditions?
    //[then can easily copy from it]


    //what?  just START conditions for the set?  or ALSO end conditions?  [and is there anything else to consider?]

    public List<singleEXE> thePlan = new List<singleEXE>();

    List<condition> startConditions;
    List<condition> endConditions;
    private string myConstructorTrace;

    public permaPlan2(singleEXE step1)
    {

        myConstructorTrace = new System.Diagnostics.StackTrace(true).ToString();
        thePlan.Add(step1);
    }
    public permaPlan2(singleEXE step1, singleEXE step2)
    {
        myConstructorTrace = new System.Diagnostics.StackTrace(true).ToString();
        thePlan.Add(step1);
        thePlan.Add(step2);
    }

    public permaPlan2(singleEXE step1, singleEXE step2, singleEXE step3)
    {

        myConstructorTrace = new System.Diagnostics.StackTrace(true).ToString();
        thePlan.Add(step1);
        thePlan.Add(step2);
        thePlan.Add(step3);
    }
    public permaPlan2(singleEXE step1, singleEXE step2, singleEXE step3, singleEXE step4)
    {

        myConstructorTrace = new System.Diagnostics.StackTrace(true).ToString();
        thePlan.Add(step1);
        thePlan.Add(step2);
        thePlan.Add(step3);
        thePlan.Add(step4);
    }

    public permaPlan2(singleEXE step1, singleEXE step2, singleEXE step3, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(step1);
        thePlan.Add(step2);
        thePlan.Add(step3);
    }
    public permaPlan2(singleEXE step1, singleEXE step2, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(step1);
        thePlan.Add(step2);
    }
    public permaPlan2(singleEXE step1, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(step1);
    }

    public permaPlan2(singleEXE step1, singleEXE step2, condition startCondition1, condition startCondition2)
    {
        startConditions.Add(startCondition1);
        startConditions.Add(startCondition2);
        thePlan.Add(step1);
        thePlan.Add(step2);
    }
    public permaPlan2(singleEXE step1, singleEXE step2, singleEXE step3, condition startCondition1, condition startCondition2)
    {
        startConditions.Add(startCondition1);
        startConditions.Add(startCondition2);
        thePlan.Add(step1);
        thePlan.Add(step2);
        thePlan.Add(step3);
    }



    List<singleEXE> outPutCopyOfThePlan()
    {
        List<singleEXE> newList = new List<singleEXE>();

        foreach (singleEXE thisOne in thePlan)
        {
            newList.Add(thisOne);
        }

        return newList;
    }
    List<condition> outPutCopyOfStartConditions()
    {
        List<condition> newList = new List<condition>();

        foreach (condition thisOne in startConditions)
        {
            newList.Add(thisOne);
        }

        return newList;
    }
    List<condition> outPutCopyOfEndConditions()
    {
        List<condition> newList = new List<condition>();

        foreach (condition thisOne in endConditions)
        {
            newList.Add(thisOne);
        }

        return newList;
    }

    internal depletablePlan convertToDepletable()
    {
        depletablePlan newThing = new depletablePlan();

        foreach (singleEXE thisOne in thePlan)
        {
            if(thisOne == null) 
            {
                Debug.Log(myConstructorTrace);
                continue; 
            }//hmm, bad...

            thisOne.resetEnactionCounter();
            newThing.add(thisOne);
        }

        return newThing;
    }



    public void printBaseEnactions()
    {
        string newString = "";
        //newString += "the base enactions of this perma plan:  ";
        //newString += "[";
        foreach (singleEXE thisEXE in thePlan)
        {

            newString += thisEXE.theEnaction;
        }

        //newString += "]";
        Debug.Log(newString);
    }

    internal string baseEnactionsAsText()
    {
        string newString = "";
        //newString += "the base enactions of this perma plan:  ";
        //newString += "[";
        foreach (singleEXE thisEXE in thePlan)
        {
            newString += thisEXE.theEnaction;
        }

        return newString;
    }
}


public class permaPlan
{
    //boil things down so it's EASY to generate [other] abstract classes
    //by simply inputting just [lists of?] enactions and conditions?

    //and make a simple class [this one] that JUST holds one set of enactions and conditions?
    //[then can easily copy from it]


    //what?  just START conditions for the set?  or ALSO end conditions?  [and is there anything else to consider?]

    List<enaction> thePlan = new List<enaction>();

    List<condition> startConditions;
    List<condition> endConditions;


    public permaPlan(enaction enaction1)
    {
        thePlan.Add(enaction1);
    }
    public permaPlan(enaction enaction1, enaction enaction2)
    {
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
    }
    public permaPlan(enaction enaction1, enaction enaction2, enaction enaction3)
    {
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
        thePlan.Add(enaction3);
    }

    public permaPlan(enaction enaction1, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(enaction1);
    }
    public permaPlan(enaction enaction1, enaction enaction2, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
    }
    public permaPlan(enaction enaction1, enaction enaction2, enaction enaction3, condition startCondition1)
    {
        startConditions.Add(startCondition1);
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
        thePlan.Add(enaction3);
    }

    public permaPlan(enaction enaction1, condition startCondition1, condition startCondition2)
    {
        startConditions.Add(startCondition1);
        startConditions.Add(startCondition2);

        thePlan.Add(enaction1);
    }
    public permaPlan(enaction enaction1, enaction enaction2, condition startCondition1, condition startCondition2)
    {
        startConditions.Add(startCondition1);
        startConditions.Add(startCondition2);
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
    }
    public permaPlan(enaction enaction1, enaction enaction2, enaction enaction3, condition startCondition1, condition startCondition2)
    {
        startConditions.Add(startCondition1);
        startConditions.Add(startCondition2);
        thePlan.Add(enaction1);
        thePlan.Add(enaction2);
        thePlan.Add(enaction3);
    }

    List<enaction> outPutCopyOfThePlan()
    {
        List<enaction> newList = new List<enaction>();

        foreach (enaction thisOne in thePlan)
        {
            newList.Add(thisOne);
        }

        return newList;
    }
    List<condition> outPutCopyOfStartConditions()
    {
        List<condition> newList = new List<condition>();

        foreach (condition thisOne in startConditions)
        {
            newList.Add(thisOne);
        }

        return newList;
    }
    List<condition> outPutCopyOfEndConditions()
    {
        List<condition> newList = new List<condition>();

        foreach (condition thisOne in endConditions)
        {
            newList.Add(thisOne);
        }

        return newList;
    }

    internal depletablePlan convertToDepletable()
    {
        throw new NotImplementedException();
    }
}

public class depletablePlan
{
    //AKA "planEXE", basically?  no!  this REPLACES series exe, and ABOVE this [animalOldFSM] holds PARALLEL.
    //so here:  just use singleEXE instead of enaction?  [for "inputData" for enacting]
    //List<enaction> thePlan = new List<enaction>();
    public List<singleEXE> thePlan = new List<singleEXE>();  //should make a "SUPERsingleEXE" that CANNOT have any mess of holding series in it?
                                                             //no further layers below the single enaction within it?  we'll see.  
                                                             //List<enactable> thePlan = new List<enactable>();  //call it an enactable?

    public List<condition> startConditions = new List<condition>();
    public List<condition> endConditions = new List<condition>();

    public depletablePlan()
    {

        endConditions.Add(new depletableSingleEXEListComplete(thePlan));
    }

    public depletablePlan(singleEXE step1)
    {
        thePlan.Add(step1);

        //always need "empty plan list" as an end condition, for animalOldFSM or something?

        endConditions.Add(new depletableSingleEXEListComplete(thePlan));
    }
    public depletablePlan(singleEXE step1, singleEXE step2)
    {
        thePlan.Add(step1);
        thePlan.Add(step2);

        //always need "empty plan list" as an end condition, for animalOldFSM or something?

        endConditions.Add(new depletableSingleEXEListComplete(thePlan));
    }
    public depletablePlan(singleEXE step1, singleEXE step2, singleEXE step3)
    {
        thePlan.Add(step1);
        thePlan.Add(step2);
        thePlan.Add(step3);

        //always need "empty plan list" as an end condition, for animalOldFSM or something?

        endConditions.Add(new depletableSingleEXEListComplete(thePlan));
    }



    public void add(singleEXE toAdd)
    {
        thePlan.Add(toAdd);
    }

    //public abstract void doTheDepletablePlan();
    public void doTheDepletablePlan()
    {
        executeSequential();
    }

    public void executeSequential()
    {

        //conditionalPrint("executeSequential()");
        //conditionalPrint("x1 nestedPlanCountToText():  " + nestedPlanCountToText());
        //this function is here because i want the lists to be private so that parallel and sequential EXEs initialize correctly

        //sequential concerns:
        //      only execute 1st one
        //      remove item from list when its end conditions are met

        if (thePlan == null)
        {
            Debug.Log("null.....that's an error!");
            return;
        }

        if (thePlan.Count < 1)
        {
            Debug.Log("exeList.Count < 1       shouldn't happen?  or means this plan has reached the end");
            return;
        }


        if (thePlan[0] == null)
        {
            Debug.Log("null.....that's an error!");
            return;
        }

        //exeList[0].grabberDebug = grabberDebug;
        //      conditionalPrint("5555555555555555555555555555grabberDebug.GetInstanceID():  " + grabberDebug.GetInstanceID());
        //      grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);
        //conditionalPrint("x2 nestedPlanCountToText():  " + nestedPlanCountToText());

        if (startConditionsMet() && thePlan[0].startConditionsMet())
        {
            //Debug.Log("start conitions met, should do this:  " + thePlan[0].staticEnactionNamesInPlanStructure());
            //conditionalPrint(" grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);...........");
            //conditionalPrint("??????????????????????????????? exeList[0].theEnaction:  " + exeList[0].theEnaction);
            //grabberDebug.recordCurrentEnaction(exeList[0].theEnaction);
            //conditionalPrint("///////////////////////////////////////////////////////////////////////");
            thePlan[0].execute();
        }

        //conditionalPrint("x3 nestedPlanCountToText():  " + nestedPlanCountToText());

        if (thePlan[0].endConditionsMet())
        {
            //Debug.Log("exeList[0].endConditionsMet()  for:  " + thePlan[0].asText());

            //conditionalPrint("x4 nestedPlanCountToText():  " + nestedPlanCountToText());
            //conditionalPrint("endConditionsMet, so:  exeList.RemoveAt(0)");
            thePlan.RemoveAt(0);

            //conditionalPrint("x5 nestedPlanCountToText():  " + nestedPlanCountToText());

            return;
        }


        //????????
        if (endConditionsMet())
        {
            Debug.Log("exeList[0].endConditionsMet()  for:  " + this);

            //conditionalPrint("x4 nestedPlanCountToText():  " + nestedPlanCountToText());
            //conditionalPrint("endConditionsMet, so:  exeList.RemoveAt(0)");
            thePlan.Clear();

            //conditionalPrint("x5 nestedPlanCountToText():  " + nestedPlanCountToText());

            return;
        }

        //conditionalPrint("x6 nestedPlanCountToText():  " + nestedPlanCountToText());
    }


    public bool startConditionsMet()
    {
        //grabberDebug.debugPrintBool = debugPrint;
        //Debug.Log("tartConditions.Count:  " + startConditions.Count);
        foreach (condition thisCondition in startConditions)
        {
            //Debug.Log("thisCondition:  " + thisCondition);
            //Debug.Log("thisCondition.met():  " + thisCondition.met());
            if (thisCondition.met() == false)
            {

                //if (debugPrint == true) { Debug.Log("this start condition not met:  " + thisCondition); }
                //      grabberDebug.rep
                return false;
            }
        }

        //Debug.Log("no start conditions remain unfulfilled!");
        //Debug.Log("no conditions remain unfulfilled!");
        return true;
    }

    public bool endConditionsMet()
    {
        //Debug.Log("looking at end conditions for:  " + this);

        //if (theEnaction != null) { Debug.Log("looking at end conditions for:  " + theEnaction); }
        foreach (condition thisCondition in endConditions)
        {
            //conditionalPrint("thisCondition:  " + thisCondition);
            //if (theEnaction != null) { Debug.Log("thisCondition:  " + thisCondition); }
            if (thisCondition.met() == false)
            {
                //Debug.Log("this end condition not met:  " + thisCondition);
                //conditionalPrint("this end condition not met:  "+ thisCondition);
                return false;
            }

            //Debug.Log("thisCondition MET:  " + thisCondition);
        }
        //Debug.Log("no conditions remain unfulfilled!");

        //conditionalPrint("no end conditions remain unfulfilled!");
        //if (theEnaction != null) { Debug.Log("so this enaction is finished:  " + theEnaction); }

        return true;
    }

}












public class aimTargetPlanGen
{
    vect3EXE2 theEXE;

    public aimTargetPlanGen(GameObject theObjectDoingTheEnactions, GameObject target)
    {
        theEXE = aimTargetPlan2(theObjectDoingTheEnactions, target);
    }

    private vect3EXE2 aimTargetPlan2(GameObject theObjectDoingTheEnactions, GameObject target)
    {
        aimTarget testE1 = theObjectDoingTheEnactions.GetComponent<aimTarget>();

        vect3EXE2 exe1 = (vect3EXE2)testE1.toEXE(target);
        exe1.atLeastOnce();

        return exe1;
    }




    public aimTargetPlanGen(GameObject theObjectDoingTheEnactions, targetPicker targetPicker)
    {
        theEXE = aimTargetPlanFromTargetPicker(theObjectDoingTheEnactions, targetPicker);
    }

    private vect3EXE2 aimTargetPlanFromTargetPicker(GameObject theObjectDoingTheEnactions, targetPicker targetPicker)
    {
        aimTarget theEnaction = theObjectDoingTheEnactions.GetComponent<aimTarget>();
        targetPicker aimOffsetter = new aimOffsetterTargetPicker(targetPicker, theEnaction);
        vect3EXE2 exe1 = new vect3EXE2(theEnaction,targetPicker);
        exe1.atLeastOnce();

        return exe1;
    }










    public vect3EXE2 returnIt()
    {
        return theEXE;
    }

}

public class equipInteractor : OldFSM
{
    condition theStartCondition;

    public equipInteractor(GameObject agent, interType intertypeXIn)
    {
        //find interactor [on agent?]
        //equip it

        //start condition is "intertypeX is NOT currently equipped but COULD be"
        //AKA all criteria:
        //      intertypeX not in object's current gamepad
        //      intertypeX found in object's inventory
    }


    public void whatToSwitchToWhenDone(OldFSM theThingToSwitchToWhenDone)
    {
        switchBoard[new reverseCondition(theStartCondition)] = theThingToSwitchToWhenDone;
    }
}