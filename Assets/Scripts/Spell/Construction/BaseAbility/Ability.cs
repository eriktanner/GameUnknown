using System.Collections.Generic;
using UnityEngine;


/*Spell Information: Damage, cooldown, spell destruction, collision particle destruction*/
public abstract class Ability : Photon.MonoBehaviour {

    protected virtual AbilityData AbilityData { get; set; }


    public virtual void CastAbility(GameObject caster, Vector3 spawnSpot, Vector3 aimTowards)
    {

    }

    public virtual void InitAbilityEffectSequence(GameObject caster, GameObject spellObject, RaycastHit hit)
    {

    }

}
