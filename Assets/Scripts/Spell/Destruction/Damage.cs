using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Handles all things damage related. Damage calculation, displays, actual damage*/
public class Damage {


    float BaseDamage;
    GameObject PlayerHit;
    int ShotBy;


    SpellDamageDisplay damageDisplay;
    NetworkDamageApplier damageApplier;


    public Damage(float baseDamage, GameObject playerHit, int shotBy)
    {
        BaseDamage = baseDamage;
        PlayerHit = playerHit;
        ShotBy = shotBy;
        damageDisplay = GameObject.Find("Spell").GetComponent<SpellDamageDisplay>();
        damageApplier = GameObject.Find("Spell").GetComponent<NetworkDamageApplier>();
    }





    /*Damages player and displays damage text*/
    public void ApplyDamage()
    {
        DamageCalculator damageCalculation = new DamageCalculator(BaseDamage);

        Debug.Log("hitPlayer position: " + PlayerHit.transform.position);

        damageDisplay.displayFloatingDamage(PlayerHit.transform.position, damageCalculation.DamageToDo, damageCalculation. IsCriticalDamage, ShotBy);
        damageApplier.ApplyDamageFromTo(PlayerHit, damageCalculation.DamageToDo, ShotBy);
    }

}
