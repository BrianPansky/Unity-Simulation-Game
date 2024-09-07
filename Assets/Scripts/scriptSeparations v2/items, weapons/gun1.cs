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
        test();  //well, no error.  which is how i did "callableAwake" before someone on reddit said i could just do "equippable2.Awake()"
        this.GetComponent<equippable2>().Awake();


        initializeEnactionPoint1();

        dictOfIvariables[numericalVariable.cooldown] = 0f;

        projectileLauncher.addProjectileLauncher(this.gameObject, enactionPoint1.transform, buttonCategories.primary,
            new interactionInfo(interType.shoot1),
            new projectileToGenerate(1, true, 99, 0), 30);

        //theCooldown = this.GetComponent<projectileLauncher>().theCooldown;
        planshell = new parallelEXE();
        planshell.startConditions.Add(new numericalCondition(numericalVariable.cooldown, dictOfIvariables));
        planshell.Add(new singleEXE(this.GetComponent<projectileLauncher>()));
        planshell.Add(new singleEXE(
            new enactEffect(new numericalEffect(numericalVariable.cooldown),
                this.gameObject,
                interactionCreator.singleton.arbitraryInterInfo(30)
                )
            ));


        //now add cooling down increment:
        List<Ieffect> list = new List<Ieffect>();
        Ieffect effect = new numericalEffect(numericalVariable.cooldown);
        list.Add(effect);
        conditionalEffects[new autoCondition()] = new List<Ieffect>(list);

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    void initializeEnactionPoint1()
    {
        enactionPoint1 = new GameObject("enactionPoint1 in initializeEnactionPoint1() line 58, gun1 script");
        enactionPoint1.transform.parent = transform;
        enactionPoint1.transform.position = this.transform.position + this.transform.forward * 0.2f + this.transform.up * 0.3f;
        enactionPoint1.transform.rotation = this.transform.rotation;

    }


}
