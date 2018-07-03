using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneMissileData : AbilityData, IBasicSpell, IDamage {

    public override string AbilityName { get { return "Arcane Missile"; } }
    public override Ability Ability { get { return new AbilityBasicSpell(this); } }
    public override GameObject Prefab { get { return Resources.Load("SpellPrefabs/Magic Magic Missile") as GameObject; } }
    public override Texture2D Icon { get; protected set; }
    public override float Cost { get { return 45; } }
    public override float Cooldown { get { return 7; } }
    public override float MaxRange { get { return 30; } }


    public GameObject CollisionParticles { get { return Resources.Load("SpellPrefabs/Empty Spell") as GameObject; } }
    public float CollisionParticleLifespan { get { return 1.5f; } }
    public float ProjectileRadius { get { return .2f; } }
    public float ProjectileSpeed { get { return 20; } }
    public float Damage { get { return 280; } }
    public float CastTime { get { return 1.3f; } }
}
