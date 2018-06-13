using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWall : Spell {

    public override SpellStats SpellStats { get { return SpellManager.GetSpellStatsFromName("Ice Wall"); } }
    public override bool IsValidDistanceChecked { get { return true; } }
    public override bool IsValidLayerCheckedGround { get { return true; } }
    public override bool IsInstantCollision { get { return true; } }


    public override float TimeFromHitToParticleExplosion { get { return 0; } }


    public override void ParticleDestruction()
    {
        DestroyIceWall explodeIceWall = new DestroyIceWall(gameObject);
        if (explodeIceWall != null)
            explodeIceWall.explodeIceWall();
    }
}
