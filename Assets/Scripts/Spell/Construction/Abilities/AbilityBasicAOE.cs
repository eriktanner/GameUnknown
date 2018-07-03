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
            TaskManager.CreateTask(AbilityUtility.WaitAndCall(((IWait) AbilityData).WaitTime, AreaOfEffect, caster, hit));
        else
            AreaOfEffect(caster, hit);
    }
    
    public virtual void AreaOfEffect(GameObject caster, RaycastHit hit)
    {
        if (AbilityData as IAOE == null)
        {
            return;
        }

        InitialAreaOfEffect(caster, hit);

        if (AbilityData as ITick != null)
            ((ITick) AbilityData).DOTHitCheck.CheckHit(caster, hit);
    }

    void InitialAreaOfEffect(GameObject caster, RaycastHit hit)
    {
        List<GameObject> playersInRadiusAndLOS = AbilityUtility.FindPlayersWithinRadiusAndLOS(hit.point, ((IAOE) AbilityData).Radius);

        foreach (GameObject hitPlayer in playersInRadiusAndLOS)
        {
            InterfaceToEffects.ProcessInitialEffects(caster, hitPlayer, AbilityData);
        }
    }

    

}
