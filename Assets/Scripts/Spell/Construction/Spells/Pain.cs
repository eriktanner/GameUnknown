using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pain : Spell {

    public override SpellStats SpellStats { get { return SpellManager.GetSpellStatsFromName("Pain"); } }
    public override bool IsValidDistanceChecked { get { return false; } }
    public override bool IsValidLayerCheckedGround { get { return false; } }
    public override bool IsInstantCollision { get { return true; } }


    public override float TimeFromHitToParticleExplosion { get { return 0; } }

    public override void SpellDestruction()
    {
        DestroyPain destroyPain = new DestroyPain(gameObject);
        destroyPain.explodePain();
    }

    public override void ParticleDestruction()
    {
        DestroyPain destroyPain = new DestroyPain(gameObject);
        destroyPain.explodePain();
    }
}
