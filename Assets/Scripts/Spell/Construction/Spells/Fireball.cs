using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell {

    public override SpellStats SpellStats { get { return SpellManager.GetSpellStatsFromName("Fireball"); } }
    public override bool IsValidDistanceChecked { get { return false; } }
    public override bool IsValidLayerCheckedGround { get { return false; } }
    public override bool IsInstantCollision { get { return false; } }

    
    public override float TimeFromHitToParticleExplosion { get { return 0; } }

}
