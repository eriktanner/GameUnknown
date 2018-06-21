using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pain : Spell {

    public override SpellStats SpellStats { get { return SpellManager.GetSpellStatsFromName("Pain"); } }
    public override bool IsValidDistanceChecked { get { return false; } }
    public override bool IsValidLayerCheckedGround { get { return false; } }
    public override bool IsInstantCollision { get { return false; } }


    public override float TimeFromHitToParticleExplosion { get { return 0; } }

    
    public override void SpellDestruction()
    {
        //DestroyPain destroyPain = new DestroyPain(gameObject);
        //destroyPain.explodePain();

        gameObject.GetComponent<SpellMovement>().FreezeSpellMovement();
        Destroy(gameObject, 3.0f);
    }
    

    public override void SpellEffect()
    {
        //DestroyPain destroyPain = new DestroyPain(gameObject);
        //destroyPain.explodePain();
        Destroy(gameObject);
    }
}
