using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This class handles calculation of damage randomization and critical damage of spells*/
public class DamageCalculator : Photon.MonoBehaviour {

    AbilityData spell;
    public float DamageToDo { get; set; }
    public bool IsCriticalDamage { get; set; }

    float baseDamage;


    /*TODO Make a Damage Controller that takes isntances of damage so that we dont keep reinstatiating it*/
    public DamageCalculator(AbilityData spell)
    {
        if (((IDamage)spell) == null)
            return;
        baseDamage = ((IDamage)spell).Damage;
        this.spell = spell;

        CalculateDamage();
    }


    /*Does the randomized calculation of damage, and also randomizes for a critical hit*/
    void CalculateDamage()
    {
        float checkForCrit = Random.value;
        IsCriticalDamage = (checkForCrit < .15f);

        if (IsCriticalDamage)
        {
            DamageToDo = CrticalDamageCalculation();
        } else
        {
            DamageToDo = NormalDamageCalculation();
        }
    }

    float NormalDamageCalculation()
    {
        float lowerBound = .8f * baseDamage;
        float upperBound = 1.1f * baseDamage;

        return Random.Range(lowerBound, upperBound);
    }

    float CrticalDamageCalculation()
    {
        float lowerBound = 1.3f * baseDamage;
        float upperBound = 1.6f * baseDamage;

        return Random.Range(lowerBound, upperBound);
    }

    
    

}
