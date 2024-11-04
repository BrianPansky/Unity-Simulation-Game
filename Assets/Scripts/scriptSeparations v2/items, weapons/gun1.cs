using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static enactionCreator;
using static interactionCreator;


public class gun1 : equippable2
{

    public cooldown theCooldown;


    //can't have this, because it prevents the "awake" function from being called in "equippable2"?!?  -_____-
    public void Awake()
    {
       this.GetComponent<equippable2>().Awake();








        /*

        dictOfIvariables[numericalVariable.cooldown] = 0f;

        projectileLauncher.addProjectileLauncher(this.gameObject, enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.shoot1),
            new projectileToGenerate(1, true, 99, 0), 30);

        //theCooldown = this.GetComponent<projectileLauncher>().theCooldown;
        planshell = new parallelEXE();
        planshell.startConditions.Add(new numericalCondition(numericalVariable.cooldown, dictOfIvariables));
        planshell.Add(new boolEXE2(this.GetComponent<projectileLauncher>()));
        planshell.Add(new boolEXE2(
            new enactEffect(new numericalEffect(numericalVariable.cooldown),
                this.gameObject,
                interactionCreator.singleton.arbitraryInterInfo(30)
                )
            ));


        //now add cooling down increment:
        //List<Ieffect> list = new List<Ieffect>();
        Ieffect effect = new numericalEffect(numericalVariable.cooldown);
        //list.Add(effect);
        //conditionalEffects[new autoCondition()] = new List<Ieffect>(list);

        genGen.singleton.addConditionalEffect(this.gameObject, new autoCondition(), effect);

        */

        




        initializeStandardEnactionPoint1(this, 0.2f, 0.3f);

        genGen.singleton.standardGun(this);

        theCooldown = this.GetComponent<projectileLauncher>().theCooldown;

    }


    // Start is called before the first frame update
    void Start()
    {

    }


    void Update()
    {
        theCooldown.cooling();
    }

}
