using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Handles all things damage related. Damage calculation, displays, actual damage*/
public class Damage {


    SpellStats Spell;
    GameObject PlayerHit;
    int ShotBy;


    SpellDamageDisplay damageDisplay;
    SpellDamageApplier damageApplier;


    public Damage(SpellStats spell, GameObject playerHit, int shotBy)
    {
        Spell = spell;
        PlayerHit = playerHit;
        ShotBy = shotBy;
        damageDisplay = GameObject.Find("Spell").GetComponent<SpellDamageDisplay>();
        damageApplier = GameObject.Find("Spell").GetComponent<SpellDamageApplier>();
    }





    /*Damages player and displays damage text*/
    public void ApplyDamage()
    {
        DamageCalculator damageCalculation = new DamageCalculator(Spell);

        damageDisplay.displayFloatingDamage(PlayerHit.transform.position, damageCalculation.DamageToDo, damageCalculation. IsCriticalDamage, ShotBy);
        damageApplier.ApplyDamageFromTo(PlayerHit, damageCalculation.DamageToDo, ShotBy);
    }

}
