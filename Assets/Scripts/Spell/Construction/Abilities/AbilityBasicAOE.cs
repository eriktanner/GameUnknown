using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBasicAOE : AbilityProjectile {


    public AbilityBasicAOE(AbilityData abilityData)
    {
        AbilityData = abilityData;
    }


    
    public override void InitAbilityEffectSequence(GameObject caster, GameObject spellObject, RaycastHit hit)
    {
        base.InitAbilityEffectSequence(caster, spellObject, hit);

        if (AbilityData as IWait != null)
            TaskManager.CreateTask(AbilityUtility.WaitAndCall(((IWait) AbilityData).WaitTime, AreaOfEffect));
        else
            AreaOfEffect();
    }
    
    public virtual void AreaOfEffect()
    {
        if (AbilityData as IAOE == null)
        {
            return;
        }
        
        List<GameObject> playersInRadiusAndLOS = AbilityUtility.FindPlayersWithinRadiusAndLOS(Hit.point, ((IAOE)AbilityData).Radius);

        foreach (GameObject hitPlayer in playersInRadiusAndLOS)
        {
            InterfaceToEffects.ProcessEffects(Caster, hitPlayer, AbilityData);
        }
    }

}
