using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conditionalEffects2 : MonoBehaviour
{
    public Dictionary<condition, List<Ieffect>> theDictionary;// = new Dictionary<condition, List<Ieffect>>();

    void Awake()
    {
        theDictionary = new Dictionary<condition, List<Ieffect>>(); //why the fuck is it null without this?  i already make a new one at top!
        Debug.Assert(theDictionary != null);
    }

    // Start is called before the first frame update
    void Start()
    {

        Debug.Assert(theDictionary != null);
    }





    void Update()
    {
        //Debug.Log("???????????????????????????????????????????????????????");
        doConditionalEffectsIfMet();
    }






    public conditionalEffects2(condition condition1, Ieffect effect1)
    {
        List<Ieffect> anEffectList = new List<Ieffect>();
        anEffectList.Add(effect1);
        theDictionary.Add(condition1, anEffectList);
    }

    public void doConditionalEffectsIfMet()//call when interacted with?
    {
        //is there a better way to do this?  observer broadcast whatevers?
        //whatever, try this for now?

        //so, this is called when interacted with [may need to distinguish between BEFORE interaction and AFTER?]
        //after interaction is done, check all conditional effects to see if any of them have been triggered.

        //Debug.Log("for this object:  " + this.gameObject + ", conditionalEffects.Keys.Count:  " + theDictionary.Keys.Count);
        foreach (condition thisCondition in theDictionary.Keys)
        {
            //Debug.Log("thisCondition:  "+ thisCondition);
            if (thisCondition.met() == false) { continue; }



            //Debug.Log("theDictionary[thisCondition].Count:  " + theDictionary[thisCondition].Count);

            foreach (Ieffect thisEffect in theDictionary[thisCondition])
            {
                thisEffect.implementEffect();
            }
        }
    }

    internal void add(condition theCondition, Ieffect theEffect)
    {
        List<Ieffect> anEffectList = new List<Ieffect>();
        anEffectList.Add(theEffect);
        Debug.Assert(theDictionary != null);
        Debug.Assert(theCondition != null);
        Debug.Assert(theEffect != null);
        theDictionary.Add(theCondition, anEffectList);
    }
}
