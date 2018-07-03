using UnityEngine;

public class AbilityBasicSpell : AbilityProjectile {
    

	
    public AbilityBasicSpell(AbilityData abilityData)
    {
        AbilityData = abilityData;
    }


    public override void InitAbilityEffectSequence(GameObject caster, GameObject spellObject, RaycastHit hit)
    {
        base.InitAbilityEffectSequence(caster, spellObject, hit);

        InterfaceToEffects.ProcessInitialEffects(caster, hit.transform.gameObject, AbilityData);

        if (AbilityData as ITick != null)
            ((ITick) AbilityData).DOTHitCheck.CheckHit(caster, hit);
    }


}
