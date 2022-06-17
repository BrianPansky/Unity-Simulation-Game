using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class functionsForAI : MonoBehaviour
{
    private GameObject t1;


    public AI1 theAI;// = GetComponent<AI1>();

    public void print(string text)
    {
        Debug.Log(text);
    }

    public bool isThisActionDone(action thisAction, Dictionary<string, List<stateItem>>  state)
    {
        //I might eventually want to change this so that "state" is one of the inputs???

        //assume true, then check and change to false where needed
        bool tf;
        tf = true;

        //print("========================1==========================");
        //print(thisAction.name);

        int howMany;
        howMany = 1;

        foreach (stateItem effectX in thisAction.effects)
        {
            //Debug.Log("how many freaking effects?????????????");
            //Debug.Log(howMany);
            //print("2------checks if no more ______ is in _______, confirm false:");
            //print(effectX.name);
            //print(effectX.stateCategory);
            //print(effectX.inStateOrNot);
            if (isGoalAccomplished(effectX, state) == false)
            {
                //Debug.Log(effectX.name);
                //print("yyyyyyyyyyyyyyyyyyyyyyyyyyyyy");
                return false;
            }
            //print("do we get here?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????");
            howMany += 1;
        }
        //print("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        return tf;
    }

    public bool isGoalAccomplished(stateItem goal, Dictionary<string, List<stateItem>> state)
    {
        //assume false, then check and change to true where needed
        bool tf;
        tf = false;
        //print("3:  should see ''inStateOrNot'' == false");
        //Debug.Log(goal.name);
        //print(goal.inStateOrNot);
        if (goal.inStateOrNot == true)
        {
            foreach (stateItem stateI in state[goal.stateCategory])
            {


                //////////////here's the C# way to say "if this item is in this list"...ya, i twon't let you use the word "in" here:
                //oh no this is text, "name" needs to be just a stateItem!  Or something!
                //print("1111111111111111111111111111111111111111");
                //print(goal.name);
                //print(stateI.name);
                if (stateI.name == goal.name)
                {
                    tf = true;
                }
            }
        }
        if (goal.inStateOrNot == false)
        {
            //actually here I have to reverse it?
            //assume false, then if I find it, change to true?
            //ya I had to do that in my C++ code too
            tf = true;

            //print("4:  should look through each item in list of________:");
            //print(goal.stateCategory);
            foreach (stateItem stateI in state[goal.stateCategory])
            {

                //print("222222222222222222222222222222222222222");
                //print(goal.name);
                //print(stateI.name);
                if (stateI.name == goal.name)
                {
                    //print("whaaaaaat>>>>>>>>>>>>>>>>");
                    //print(stateI.name);
                    //print(goal.name);
                    tf = false;
                }
            }
        }
        return tf;
    }

    public bool prereqChecker(action thisAction, Dictionary<string, List<stateItem>> state)
    {
        //assume true, then check and change to false where needed
        bool tf;
        tf = true;

        foreach (stateItem prereqX in thisAction.prereqs)
        {
            //print("don't count this");
            if (isGoalAccomplished(prereqX, state) == false)
            {
                return false;
            }
        }

        return tf;
    }

    public void constantlyCheckLocationState()
    {

    }

    public void doNextAction(action nextAction, Dictionary<string, List<stateItem>> state)
    {
        if (nextAction.type == "goTo")
        {

            //Debug.Log("hello this is where one thing is printing");
            //Debug.Log(nextAction.effects[0]);
            //Debug.Log("done printing");
            stateItem stateItemX = nextAction.effects[0];
            string name1 = stateItemX.name;
            t1 = GameObject.Find(name1);
            transform.position = Vector3.MoveTowards(transform.position, t1.GetComponent<Transform>().position, theAI.speed * Time.deltaTime);
        }

        //if (nextAction.type == "socialTrade")
        else
        {
            if (prereqChecker(nextAction, state) == true)
            {
                //Debug.Log("cccccccccccccccccccccc");
                //printState(state);
                state = implementALLEffectsForImagination(nextAction, state);
                //printState(state);
            }
        }
    }

    public void printPlan(List<action> plan)
    {
        foreach (action listItem in plan)
        {
            print(listItem.name);
        }
    }

    public void printState(Dictionary<string, List<stateItem>> state)
    {
        string text;
        text = "{";
        foreach (string key in state.Keys)
        {
            text = string.Concat(text, key, ", ");
            foreach (stateItem content in state[key])
            {
                text = string.Concat(text, content.name, ", ");
            }
            text = string.Concat(text, "} ");
        }
        text = string.Concat(text, "}");
        print(text);
    }

    public List<List<action>> problemSolver(stateItem goal, List<action> knownActions, Dictionary<string, List<stateItem>> state)
    {
        List<List<action>> planList = new List<List<action>>();

        if (isGoalAccomplished(goal, state) == false)
        {
            foreach (action thisAction in knownActions)
            {
                foreach (stateItem thisEffect in thisAction.effects)
                {
                    if (goal.name == thisEffect.name & goal.inStateOrNot == thisEffect.inStateOrNot)
                    {
                        if (prereqChecker(thisAction, state))
                        {
                            List<action> shortPlan = new List<action>();
                            shortPlan.Add(thisAction);
                            planList.Add(shortPlan);
                        }
                        else
                        {
                            List<List<action>> plansUnderConstruction = new List<List<action>>();

                            foreach (stateItem eachPrereq in thisAction.prereqs)
                            {
                                if (isGoalAccomplished(eachPrereq, state) == false)
                                {
                                    List<List<action>> plansForThisPrereq = new List<List<action>>();
                                    plansForThisPrereq = problemSolver(eachPrereq, knownActions, state);

                                    if (plansForThisPrereq.Count == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (plansUnderConstruction.Count == 0)
                                        {
                                            plansUnderConstruction = plansForThisPrereq;
                                        }
                                        else
                                        {
                                            List<List<action>> nextStepInConstructingPlans = new List<List<action>>();
                                            foreach (List<action> eachPLanUnderConstruction in plansUnderConstruction)
                                            {
                                                foreach (List<action> eachPLanForThisPrereq in plansForThisPrereq)
                                                {
                                                    eachPLanUnderConstruction.AddRange(eachPLanForThisPrereq);
                                                    nextStepInConstructingPlans.Add(eachPLanUnderConstruction);
                                                }
                                            }
                                            plansUnderConstruction = nextStepInConstructingPlans;
                                        }
                                    }
                                }
                            }
                            //recursionCounter -= 1

                            foreach (List<action> eachWay in plansUnderConstruction)
                            {
                                List<action> eachPlan = new List<action>();
                                eachPlan = eachWay;
                                eachPlan.Add(thisAction);
                                planList.Add(eachPlan);
                            }
                        }
                    }
                }
            }
        }

        return planList;
    }

    public Dictionary<string, List<stateItem>> stateCopyer(Dictionary<string, List<stateItem>> state)
    {
        Dictionary<string, List<stateItem>> newState = new Dictionary<string, List<stateItem>>();

        foreach (string keyString in state.Keys)
        {
            List<stateItem> emptyList = new List<stateItem>();
            //newState[keyString] = state[keyString];
            newState[keyString] = emptyList;
            foreach (stateItem item in state[keyString])
            {
                newState[keyString].Add(item);
            }
        }
        return newState;
    }

    public Dictionary<string, List<stateItem>> removeStateItem(stateItem thisStateItem, Dictionary<string, List<stateItem>> state)
    {
        //just because they are not quite identical enough for .Remove to work properly I don't think

        //find the correct stateItem, then remove it
        foreach (stateItem eachStateItem in state[thisStateItem.stateCategory])
        {
            if (eachStateItem.name == thisStateItem.name)
            {
                state[thisStateItem.stateCategory].Remove(eachStateItem);
                break;
            }
        }

        return state;
    }

    public Dictionary<string, List<stateItem>> implementALLEffectsForImagination(action currentAction, Dictionary<string, List<stateItem>> imaginaryState)
    {
        foreach (stateItem eachEffect in currentAction.effects)
        {
            if (eachEffect.inStateOrNot == true)
            {
                if (eachEffect.stateCategory == "locationState")
                {
                    imaginaryState["locationState"].Clear();
                }
                imaginaryState[eachEffect.stateCategory].Add(eachEffect);
            }
            else
            {
                imaginaryState = removeStateItem(eachEffect, imaginaryState);
                //imaginaryState[eachEffect.stateCategory].Remove(eachEffect);
            }
        }
        return imaginaryState;
    }

    public List<List<action>> simulatingPlansToEnsurePrereqs(List<List<action>> planList, List<action> knownActions, Dictionary<string, List<stateItem>> realState)
    {
        //print("111111111111111111111111111111111111111");
        foreach (List<action> eachPlan in planList)
        {
            Dictionary<string, List<stateItem>> imaginaryState = new Dictionary<string, List<stateItem>>();
            imaginaryState = stateCopyer(realState);
            //print("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            //printState(realState);
            //imaginaryState.Clear();
            //Debug.Log(realState["inventory"]);
            //imaginaryState["inventory"].Remove(money1);
            //printState(imaginaryState);
            //printState(realState);
            //print("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
            int counter;
            int halt;

            counter = 0;
            halt = 0;
            //print("222222222222222222222222222222222222222222222");
            while ((counter + 1) <= eachPlan.Count)
            {
                halt += 1;
                if (halt > 20)
                {
                    break;
                }
                action currentAction;
                currentAction = eachPlan[counter];
                //print("333333333333333333333333333333333333333333333");
                //print(currentAction.name);
                if (prereqChecker(currentAction, imaginaryState) != true)
                {
                    //print("yes this should happen for ''eat'':");
                    //print(currentAction.name);
                    foreach (stateItem eachPrereq in currentAction.prereqs)
                    {
                        if (isGoalAccomplished(eachPrereq, imaginaryState) != true)
                        {
                            //print("and this should happen for ''home''");
                            //print(eachPrereq.name);
                            List<List<action>> prereqFillerList;
                            prereqFillerList = problemSolver(eachPrereq, knownActions, imaginaryState);
                            //print("should have found this plan to fill the prereq:");
                            //printPlan(prereqFillerList[0]);


                            if (prereqFillerList.Count > 0)
                            {
                                //see python code for why this part is unfinished code
                                foreach (action eachAction in prereqFillerList[0])
                                {
                                    eachPlan.Insert(counter, eachAction);
                                }
                                counter += 1;
                            }
                            else
                            {
                                counter += 1;
                                planList.Remove(eachPlan);

                            }
                        }
                    }
                }

                imaginaryState = implementALLEffectsForImagination(currentAction, imaginaryState);
                counter += 1;
            }

        }
        return planList;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        theAI = GetComponent<AI1>();
    }

}
