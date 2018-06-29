using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaSpell : Spell {





    public virtual void AreaOfEffect()
    {
        if (SpellEffectRadius <= 0)
        {
            Destroy(gameObject, 2.5f);
            return;
        }


        SpellEffect spellEffect = SpellEffectFactory.GetEffectFromFactory(GetType());

        if (spellEffect != null)
        {
            Vector3 origin = gameObject.transform.position;
            List<GameObject> playersInRadiusAndLOS = GameplayUtility.FindPlayersWithinRadiusAndLOS(origin, SpellEffectRadius);

            foreach (GameObject hitPlayer in playersInRadiusAndLOS)
            {
                spellEffect.ProcessEffect(hitPlayer, gameObject);
            }
        }
        else
        {
            Debug.Log("SpellEffect Not Found");
        }


        Destroy(gameObject, 3.0f);
    }



}
