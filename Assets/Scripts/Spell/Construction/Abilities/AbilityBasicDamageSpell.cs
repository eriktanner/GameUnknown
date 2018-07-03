using System;
using UnityEngine;

public class AbilityCastSpell : AbilityProjectile {
    

	
    public AbilityCastSpell(AbilityData abilityData)
    {
        AbilityData = abilityData;
    }


    public override void InitAbilityEffectSequence(GameObject caster, GameObject spellObject, RaycastHit hit)
    {
        base.InitAbilityEffectSequence(caster, spellObject, hit);

        InterfaceToEffects.ProcessEffects(Caster, Hit.transform.gameObject, AbilityData);
    }
}
