using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Spell Information: Damage, cooldown, spell destruction, collision particle destruction*/
public abstract class Ability : Photon.MonoBehaviour {

    public virtual AbilityData AbilityData { get; protected set; }


    public virtual void CastAbility(GameObject player, Vector3 spawnSpot, Vector3 aimTowards)
    {

    }

    public virtual void InitAbilityEffectSequence(GameObject spellObject, RaycastHit Hit)
    {

    }


    /*Override for different destruction methods
     Note: Always called, even if spell does not hit*/
    public virtual void TimedDestruction(GameObject spellObject)
    {
        Destroy(spellObject);
    }

    
    
    public virtual void AreaOfEffect(GameObject spellObject)
    {
        if (AbilityData as IAOE == null)
        {
            Destroy(spellObject, 2.5f);
            return;
        }


        SpellEffect spellEffect = SpellEffectFactory.GetEffectFromFactory(GetType());

        if (spellEffect != null)
        {
            Vector3 origin = gameObject.transform.position;
            List<GameObject> playersInRadiusAndLOS = GameplayUtility.FindPlayersWithinRadiusAndLOS(origin, ((IAOE)AbilityData).Radius);

            foreach (GameObject hitPlayer in playersInRadiusAndLOS)
            {
                spellEffect.ProcessEffect(hitPlayer, gameObject);
            }
        }
        else
        {
            Debug.Log("SpellEffect Not Found");
        }


        Destroy(spellObject, 3.0f);
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
