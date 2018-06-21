using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulVoid : Spell {

    public override SpellStats SpellStats { get { return SpellManager.GetSpellStatsFromName("Soul Void"); } }
    public override bool IsValidDistanceChecked { get { return true; } }
    public override bool IsValidLayerCheckedGround { get { return true; } }
    public override bool IsInstantCollision { get { return true; } }


    public override float TimeFromHitToParticleExplosion { get { return 1f; } }
    public override float SpellEffectRadius { get { return 3.5f; } }

    /*
    public override void ParticleDestruction()
    {
        DestroySoulVoid explodeSoulVoid = new DestroySoulVoid(gameObject);
        explodeSoulVoid.explodeSoulVoid();
    }*/
}
