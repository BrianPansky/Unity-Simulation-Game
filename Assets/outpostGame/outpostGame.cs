using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using static enactionCreator;
using static interactionCreator;

public class outpostGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        genGen.singleton.returnGun1(new Vector3(-2, 0.7f, 6));

        int number = 0;
        while(number < 1)
        {

            new basicSoldierGenerator(tagging2.tag2.team2).doIt(new Vector3(7, 0, 10*number));
            number++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}




public class basicSoldierGenerator : doAtPoint
{
    tagging2.tag2 team;

    public basicSoldierGenerator(tagging2.tag2 theTeamIn)
    {
        this.team = theTeamIn;
    }


    internal override void doIt(Vector3 thisPoint)
    {
        GameObject newObj = repository2.Instantiate(repository2.singleton.placeHolderCylinderPrefab, thisPoint, Quaternion.identity);
        GameObject.Destroy(newObj.GetComponent<Collider>());
        newObj.AddComponent<CapsuleCollider>();

        genGen.singleton.ensureVirtualGamePad(newObj);



        tagging2.singleton.addTag(newObj, team);
        playable2 thePlayable = newObj.AddComponent<playable2>();
        thePlayable.dictOfIvariables[numericalVariable.health] = 2;
        thePlayable.equipperSlotsAndContents[simpleSlot.hands] = null;
        thePlayable.initializeEnactionPoint1();
        genGen.singleton.addArrowForward(thePlayable.enactionPoint1); 
        thePlayable.initializeCameraMount(thePlayable.enactionPoint1.transform);
        genGen.singleton.addArrowForward(newObj, 5f, 0f, 1.2f);


        genGen.singleton.makeBasicEnactions(thePlayable);
        genGen.singleton.makeInteractionsBody4(thePlayable);


        inventory1 theirInventory = newObj.AddComponent<inventory1>();
        //theirInventory.startingItem = genGen.singleton.returnGun1(newObj.transform.position);
        GameObject gun = genGen.singleton.returnGun1(newObj.transform.position);
        theirInventory.inventoryItems.Add(gun);
        interactionCreator.singleton.dockXToY(gun, newObj);


        FSMcomponent theFSMcomponent = newObj.AddComponent<FSMcomponent>();
        theFSMcomponent.theFSMList = new basicSoldierFSM(newObj, team).returnIt();
    }



}

public class FSMcomponent:MonoBehaviour,IupdateCallable
{
    //public FSM theFSM;
    public List<FSM> theFSMList;  //correct way to do parallel!  right at the top level!!!  one for walking/feet, one for hands/equipping/using items etc.

    public List<IupdateCallable> currentUpdateList {  get; set; }
    public void Update()
    {

        //Debug.Log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&     regular Update()        &&&&&&&&&&&&&&&&&&&");
    }

    public void callableUpdate()
    {
        //Debug.Log("============================     callableUpdate()        =================");
        foreach (FSM theFSM in theFSMList)
        {
            //Debug.Log("theFSM:  "+ theFSM);
            //theFSM = theFSM.doAFrame();  //will this be an issue?  or only if i add/remove items from list?
        }

        for (int index = 0; index < theFSMList.Count; index++)
        {
            theFSMList[index] = theFSMList[index].doAFrame();
        }

    }
}

public class basicSoldierFSM : FSM
{

    //FSM theFSM;
    public List<FSM> theFSMList = new List<FSM>();

    public basicSoldierFSM(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {


        theFSMList.Add(feetFSM(theObjectDoingTheEnaction, team));
        theFSMList.Add(handsFSM(theObjectDoingTheEnaction, team));
        
    }

    private FSM handsFSM(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {
        FSM idle = new generateFSM();

        objectCriteria theCriteria = createAttackCriteria(theObjectDoingTheEnaction,team);
        objectSetGrabber theAttackObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);
        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        targetPicker theAttackTargetPicker = generateAttackTargetPicker(theObjectDoingTheEnaction,theAttackObjectSet);

        FSM combat1 = new generateFSM(new aimAtXAndInteractWithY(theObjectDoingTheEnaction, theAttackTargetPicker, interType.peircing, 10f).returnIt());

        

        idle.addSwitchAndReverse(switchToAttack, combat1);



        equipItemFSM equipGun = new equipItemFSM(theObjectDoingTheEnaction, interType.peircing);

        idle.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theFSM);
        //wander.addSwitchAndReverse(switchToAttack, equipGun.theFSM);
        combat1.addSwitchAndReverse(equipGun.theNotEquippedButCanEquipSwitchCondition(theObjectDoingTheEnaction, interType.peircing), equipGun.theFSM);//messy



        idle.name = "hands, idle";
        combat1.name = "hands, combat1";
        equipGun.theFSM.name = "hands, equipGun";
        return idle;;
    }

    private objectCriteria createAttackCriteria(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {
        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new lineOfSight(theObjectDoingTheEnaction),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 25)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        return theCriteria;
    }

    private targetPicker generateAttackTargetPicker(GameObject theObjectDoingTheEnaction, objectSetGrabber theAttackObjectSet)
    {
        
        targetPicker theAttackTargetPicker = new pickNearest(theObjectDoingTheEnaction, theAttackObjectSet);

        return theAttackTargetPicker;
    }

    private FSM feetFSM(GameObject theObjectDoingTheEnaction, tagging2.tag2 team)
    {
        FSM wander = new generateFSM(new randomWanderRepeatable(theObjectDoingTheEnaction).returnIt());




        objectCriteria theCriteria = new objectMeetsAllCriteria(
            new hasVirtualGamepad(),
            new reverseCriteria(new objectHasTag(team)),
            new lineOfSight(theObjectDoingTheEnaction),
            new proximityCriteriaBool(theObjectDoingTheEnaction, 25)
            //new objectVisibleInFOV(theObjectDoingTheEnaction.GetComponent<playable2>().enactionPoint1.transform)
            );

        objectSetGrabber theAttackObjectSet = new allObjectsInSetThatMeetCriteria(new allObjectsInZone(theObjectDoingTheEnaction), theCriteria);

        targetPicker theAttackTargetPicker = new pickNearest(theObjectDoingTheEnaction, theAttackObjectSet);

        FSM combat1 = new generateFSM(new goToX(theObjectDoingTheEnaction, theAttackTargetPicker, 10f).returnIt());

        condition switchToAttack = new stickyCondition(new isThereAtLeastOneObjectInSet(theAttackObjectSet), 10);// theObjectDoingTheEnaction, numericalVariable.health);


        wander.addSwitchAndReverse(switchToAttack, combat1);

        wander.name = "feet, wander";
        combat1.name = "feet, combat1";
        return wander;
    }




    public List<FSM> returnIt()
    {
        return theFSMList;
    }



}

public class equipItemFSM
{

    public FSM theFSM;

    public equipItemFSM(GameObject theObjectDoingTheEnaction, interType interTypeX)
    {

        //equipObjectRepeater
        theFSM = new generateFSM(new equipObjectRepeater(theObjectDoingTheEnaction)); //hmm, this doesn't share cache with the conditions......
        //MAKE SURE TO RETURN IT!!!!!!  
    }

    public condition theNotEquippedButCanEquipSwitchCondition(GameObject theObjectDoingTheEnaction, interType interTypeX)
    {

        //switch condition:
        //      has item we want in inventory
        //      but not in playable or equipped items [just search in virtual gamepad???]
        //  AND we want to CACHE that item if possible, so we can equip it without searching for it again



        objectCriteria hasInterTypeX = new intertypeXisOnObject(interTypeX);

        condition hasInPlayable = new individualObjectMeetsAllCriteria(new presetObject(theObjectDoingTheEnaction), hasInterTypeX);







        objectSetGrabber theEquippedObjectsGrabber = new equippedObjectsGrabber(theObjectDoingTheEnaction);
        //objectCriteria hasInterTypeX = new intertypeXisOnObject(interTypeX);
        pickFirstObjectXFromListY theEquippedObjectPicker = new pickFirstObjectXFromListY(hasInterTypeX, theEquippedObjectsGrabber);


        condition hasEquipped = new nonNullObject(theEquippedObjectPicker);








        //      has item we want in inventory [AND we want to CACHE that item if possible]
        //condition:  object returner returns non-null object
        //object returner [main]:  object on list that meets criteria
        //          the list:   inventory on [set?] object
        //          the criteria: has intertypeX
        //caching etc
        //  plug cache setter into condition checker [so, wrap the above returner]
        //  have cache observer as a variable we can plug into "equipping" enaction

        inventoryGrabber theInvGrabber = new inventoryGrabber(theObjectDoingTheEnaction);
        //objectCriteria hasInterTypeX = new intertypeXisOnObject(interTypeX);
        pickFirstObjectXFromListY theInvObjectPicker = new pickFirstObjectXFromListY(hasInterTypeX, theInvGrabber);


        objectCacheSetter theInvCacheSetter = new objectCacheSetter(theInvObjectPicker);
        objectCacheReceiver theInvCacheReceiver = new objectCacheReceiver(theInvCacheSetter);

        condition hasInInventory = new nonNullObject(theInvCacheSetter);






        condition notEquippedButCanEquip = new multicondition(new reverseCondition(hasInPlayable), new reverseCondition(hasEquipped), hasInInventory);


        //new intertypeXisOnObject();
        //new intertypeXisInEquipperSlots();

        //new targetCacheSetter
        //new targetCacheReceiver



        //      but not in playable or equipped items [just search in virtual gamepad???]
        //similar to above.
        //but the object lists are equipper slot contents, and ...playable is just one object, sooo a set of ONE?
        //does this need cache too???
        //new intertypeXisInInventory();

        //new equipObjectRepeater();


        return notEquippedButCanEquip;
    }


    public FSM returnIt()
    {
        return theFSM;
    }



}



public class equipObjectRepeater : repeater
{
    repeater theRepeater; //hmm, nesting makes problems for printing/debugging contents.....
    individualObjectReturner theIndividualObjectReturner;

    public equipObjectRepeater(GameObject theObjectDoingTheEnaction)
    {

        theRepeater = new simpleExactRepeatOfPerma(new permaPlan2(equipX2(theObjectDoingTheEnaction)));
    }

    //for now
    /*
    public equipObjectRepeater(individualObjectReturner theIndividualObjectReturner, GameObject theObjectDoingTheEnaction, interType interTypeX)
    {

        theRepeater = new simpleExactRepeatOfPerma(new permaPlan2(equipX2(theObjectDoingTheEnaction)));
    }
    */


    public override void doThisThing()
    {
        theRepeater.doThisThing();
    }


    private singleEXE equipX2(GameObject theObjectDoingTheEnaction)//, interType interTypeX)
    {
        GameObject theItemWeWant = null; //for now.....//theIndividualObjectReturner.returnObject();//new find().objectInObjectsInventoryWithIntertypeX(theObjectDoingTheEnaction, interTypeX);//firstObjectOnListWIthInterTypeX(interTypeX, getInventory());

        //IEnactaBool testE1 = new takeFromAndPutBackIntoInventory(this.gameObject);

        IEnactaBool testE1 = theObjectDoingTheEnaction.GetComponent<takeFromAndPutBackIntoInventory>();

        //planEXE2 exe1 = new singleEXE(testE1, theItemWeWant);
        singleEXE exe1 = (singleEXE)testE1.toEXE(theItemWeWant);
        exe1.atLeastOnce();
        //condition thisCondition = new enacted(exe1);
        //exe1.endConditions.Add(thisCondition);
        return exe1;
    }

    public override string baseEnactionsAsText()
    {
        return theRepeater.baseEnactionsAsText();
    }
}





public class goToXAndInteractWithY
{
    repeatWithTargetPicker theRepeater;

    public goToXAndInteractWithY(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, interType theIntertypeY, float proximity)
    {
        theRepeater = thinggggg(theObjectDoingTheEnactions,thingXToGoTo,theIntertypeY,proximity);
    }

    public repeatWithTargetPicker returnIt()
    {
        return theRepeater;
    }

    private repeatWithTargetPicker thinggggg(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, interType theIntertypeY, float proximity)
    {

        //targetPicker getter = new pickNearestExceptSelf(theObjectDoingTheEnactions,
        //    new allNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(
            //genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions,
                //thingXToGoTo.pickNext().realPositionOfTarget(), //messy
                //proximity),
                //new aimTargetPlanGen(theObjectDoingTheEnactions, thingXToGoTo).returnIt(), //messy
                interactWithType(theObjectDoingTheEnactions, theIntertypeY));
        
        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, thingXToGoTo);


        return repeatWithTargetPickerTest;
    }
    public singleEXE interactWithType(GameObject theObjectDoingTheEnactions, interType interTypeX)
    {
        //have to see any available interactions:
        //where?
        //          in current "equipped" enactions in gamepad
        //          in inventory [at which point, need to equip it!  ..............doesn't fit current "singleEXE" paradigm.....
        //just add a step before this in the plan that equips/preps IF necessary???
        //
        //what kind of enaction?
        //      ranged
        //          hitscan
        //          non-hitscan
        //      [no others currently?]
        
        enaction theInteractionEnaction = new find().enactionWithInteractionX(theObjectDoingTheEnactions,interTypeX);
        if (theInteractionEnaction != null)
        {
            singleEXE theSingle = (singleEXE)theInteractionEnaction.standardEXEconversion();
            theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

            return theSingle;
        }


        //if(theInteractionEnaction == null) { return null; }//hmmmmmmmm


        inventory1 theirInventory = theObjectDoingTheEnactions.GetComponent<inventory1>();
        if (theirInventory == null) { return null; }  //or return "go find"?
        
        foreach(GameObject thisObject in theirInventory.inventoryItems)
        {
            theInteractionEnaction = new find().enactionWithInteractionX(thisObject, interTypeX);
            if (theInteractionEnaction != null)
            {
                singleEXE theSingle = (singleEXE)theInteractionEnaction.standardEXEconversion();
                theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

                return theSingle;
            }
            //foreach (collisionEnaction thisEnaction in listOfCollisionEnactionsOnObject(theObject))
            {
                //Debug.Log(thisEnaction.interInfo.interactionType);
                //if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
            }
        }
        



        return null;
    }


    
}


public class goToX
{
    repeatWithTargetPicker theRepeater;

    public goToX(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, float proximity)
    {
        theRepeater = thinggggg(theObjectDoingTheEnactions, thingXToGoTo, proximity);
    }

    public repeatWithTargetPicker returnIt()
    {
        return theRepeater;
    }

    private repeatWithTargetPicker thinggggg(GameObject theObjectDoingTheEnactions, targetPicker thingXToGoTo, float proximity)
    {

        //targetPicker getter = new pickNearestExceptSelf(theObjectDoingTheEnactions,
        //    new allNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(
                genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions,
                thingXToGoTo.pickNext().realPositionOfTarget(), //messy
                proximity)
                //new aimTargetPlanGen(theObjectDoingTheEnactions, thingXToGoTo).returnIt(), //messy
                //interactWithType(theObjectDoingTheEnactions, theIntertypeY)
                );

        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, thingXToGoTo);


        return repeatWithTargetPickerTest;
    }
    


}


public class aimAtXAndInteractWithY
{
    repeatWithTargetPicker theRepeater;

    public aimAtXAndInteractWithY(GameObject theObjectDoingTheEnactions, targetPicker thingXToAimAt, interType theIntertypeY, float proximity)
    {
        theRepeater = thinggggg(theObjectDoingTheEnactions, thingXToAimAt, theIntertypeY, proximity);
    }

    public repeatWithTargetPicker returnIt()
    {
        return theRepeater;
    }

    private repeatWithTargetPicker thinggggg(GameObject theObjectDoingTheEnactions, targetPicker thingXToAimAt, interType theIntertypeY, float proximity)
    {

        //targetPicker getter = new pickNearestExceptSelf(theObjectDoingTheEnactions,
        //    new allNearbyNumericalVariable(theObjectDoingTheEnactions, numVarX));

        //USING FAKE INPUTS FOR TARGETS
        permaPlan2 perma1 = new permaPlan2(
                //genGen.singleton.makeNavAgentPlanEXE(theObjectDoingTheEnactions,
                //thingXToGoTo.pickNext().realPositionOfTarget(), //messy
                //proximity),
                new aimTargetPlanGen(theObjectDoingTheEnactions, thingXToAimAt).returnIt(), //messy
                interactWithType(theObjectDoingTheEnactions, theIntertypeY));

        repeatWithTargetPicker repeatWithTargetPickerTest = new repeatWithTargetPicker(perma1, thingXToAimAt);


        return repeatWithTargetPickerTest;
    }



    public singleEXE interactWithType(GameObject theObjectDoingTheEnactions, interType interTypeX)
    {
        //have to see any available interactions:
        //where?
        //          in current "equipped" enactions in gamepad
        //          in inventory [at which point, need to equip it!  ..............doesn't fit current "singleEXE" paradigm.....
        //just add a step before this in the plan that equips/preps IF necessary???
        //
        //what kind of enaction?
        //      ranged
        //          hitscan
        //          non-hitscan
        //      [no others currently?]

        enaction theInteractionEnaction = new find().enactionWithInteractionX(theObjectDoingTheEnactions, interTypeX);
        if (theInteractionEnaction != null)
        {
            singleEXE theSingle = (singleEXE)theInteractionEnaction.standardEXEconversion();
            theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

            return theSingle;
        }


        //if(theInteractionEnaction == null) { return null; }//hmmmmmmmm


        inventory1 theirInventory = theObjectDoingTheEnactions.GetComponent<inventory1>();
        if (theirInventory == null) { return null; }  //or return "go find"?

        foreach (GameObject thisObject in theirInventory.inventoryItems)
        {
            theInteractionEnaction = new find().enactionWithInteractionX(thisObject, interTypeX);
            if (theInteractionEnaction != null)
            {
                singleEXE theSingle = (singleEXE)theInteractionEnaction.standardEXEconversion();
                theSingle.atLeastOnce();// untilListFinished();  //surely "list" is redundant for a single???

                return theSingle;
            }
            //foreach (collisionEnaction thisEnaction in listOfCollisionEnactionsOnObject(theObject))
            {
                //Debug.Log(thisEnaction.interInfo.interactionType);
                //if (thisEnaction.interInfo.interactionType == interTypeX) { return thisEnaction; }
            }
        }




        return null;
    }

}