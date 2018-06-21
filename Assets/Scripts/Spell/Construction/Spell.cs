using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Spell Information: Damage, cooldown, spell destruction, collision particle destruction*/
public abstract class Spell : MonoBehaviour {

    /*Note: Attach to both Spell Projectile and Collision Particles upon respective instantiation*/

    public virtual SpellStats SpellStats { get; protected set; }
    public virtual bool IsValidDistanceChecked { get; protected set; }
    public virtual bool IsValidLayerCheckedGround { get; protected set; }
    public virtual bool IsInstantCollision { get; protected set; }

    
    public virtual float TimeFromHitToParticleExplosion { get; protected set; }
    public virtual float SpellEffectRadius { get; protected set; }



    /*Override for different destruction methods
     Note: Always called, even if spell does not hit*/
    public virtual void SpellDestruction()
    {
        Destroy(gameObject);
    }

    /*Override for different particle destruction methods
     Note: Call only if spell collides with a surface*/
    public virtual void SpellEffect()
    {
        if (SpellEffectRadius > 0)
        {
            AreaOfEffect();
        } else
        {
            NormalHit();
        }

        Destroy(gameObject, 3.0f);
    }

    protected virtual void AreaOfEffect()
    {
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
    }

    protected virtual void NormalHit()
    {

    }



}
