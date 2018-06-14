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


    /*Override for different destruction methods
     Note: Always called, even if spell does not hit*/
    public virtual void SpellDestruction()
    {
        Destroy(gameObject);
    }

    /*Override for different particle destruction methods
     Note: Call only if spell collides with a surface*/
    public virtual void ParticleDestruction()
    {
        Destroy(gameObject, 3.0f);
    }



}
