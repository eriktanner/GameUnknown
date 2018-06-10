using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This class handles calculation of damage randomization and critical damage of spells*/
public class DamageCalculator : Photon.MonoBehaviour {

    Damager damager;

    GameObject PlayerHit;
    
    

    Spell spell;
    FloatingDamageController floatingDamageController;

    float damageToDo;
    bool criticalDamage = false;


    /*TODO Make a Damage Controller that takes isntances of damage so that we dont keep reinstatiating it*/
    public static DamageCalculator AddDamageComponent(GameObject attachTo, Spell spell, GameObject playerHit)
    {
        DamageCalculator damageComponent = attachTo.AddComponent<DamageCalculator>();
        damageComponent.spell = spell;
        
        damageComponent.PlayerHit = playerHit;
        

        damageComponent.floatingDamageController = GameObject.Find("Managers/GameManager").GetComponent<FloatingDamageController>();
        damageComponent.damager = GameObject.Find("Spell").GetComponent<Damager>();

        return damageComponent;
    }
    

    /*Damages player and displays damage text*/
    public void ApplyDamage()
    {
        CalculateDamage();
        damager.displayFloatingDamage(PlayerHit.transform.position, damageToDo, criticalDamage, gameObject.GetComponent<SpellIdentifier>().ShotBy);
        //healthBarHit.CmdCollisionDamagePlayer(damageToDo, PlayerHit.transform.gameObject.name);
    }

    /*Does the randomized calculation of damage, and also randomizes for a critical hit*/
    void CalculateDamage()
    {
        float checkForCrit = Random.value;
        criticalDamage = (checkForCrit < .15f);

        if (criticalDamage)
        {
            damageToDo = CrticalDamageCalculation();
        } else
        {
            damageToDo = NormalDamageCalculation();
        }
    }

    float NormalDamageCalculation()
    {
        float originalDamage = spell.damage;

        float lowerBound = .8f * originalDamage;
        float upperBound = 1.1f * originalDamage;

        return Random.Range(lowerBound, upperBound);
    }

    float CrticalDamageCalculation()
    {
        float originalDamage = spell.damage;

        float lowerBound = 1.3f * originalDamage;
        float upperBound = 1.6f * originalDamage;

        return Random.Range(lowerBound, upperBound);
    }

    
    

}
