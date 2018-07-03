using System.Collections.Generic;
using UnityEngine;


/*Spell Information: Damage, cooldown, spell destruction, collision particle destruction*/
public abstract class Ability : Photon.MonoBehaviour {

    protected virtual AbilityData AbilityData { get; set; }

    protected virtual GameObject Caster { get; set; }

    protected virtual RaycastHit Hit { get; set; }



    public virtual void CastAbility(GameObject caster, Vector3 spawnSpot, Vector3 aimTowards)
    {
        Caster = caster;
    }

    public virtual void InitAbilityEffectSequence(GameObject caster, GameObject spellObject, RaycastHit hit)
    {
        Caster = caster;
    }



    /*Override for different destruction methods
     Note: Always called, even if spell does not hit*/
    public virtual void TimedDestruction(GameObject spellObject)
    {
        Destroy(spellObject);
    }

    
    
    



    public virtual void DirectHit(GameObject spellObject, GameObject target, int shotBy)
    {
        if (target.tag == "Player")
        {
            if (((IDamage) AbilityData) == null)
                return;

            Damage damage = new Damage(AbilityData, target, shotBy);
            damage.ApplyDamage();
        }

        Destroy(spellObject, 2.0f);
    }



}
