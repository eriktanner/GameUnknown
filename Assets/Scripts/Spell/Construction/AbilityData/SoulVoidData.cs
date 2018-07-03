using UnityEngine;

public class SoulVoidData : AbilityData, IBasicAOE, IDamage, IWait, IInstantCollision, IStun
{
    public override string AbilityName { get { return "Soul Void"; } }
    public override Ability Ability { get { return new AbilityBasicAOE(this); } }
    public override GameObject Prefab { get { return Resources.Load("SpellPrefabs/Empty Spell") as GameObject; } }
    public override Texture2D Icon { get; protected set; }
    public override float Cost { get { return 50; } }
    public override float Cooldown { get { return 12; } }
    public override float MaxRange { get { return 20; } }


    public GameObject CollisionParticles { get { return Resources.Load("SpellPrefabs/CollisionParticles/Magic Soul Void") as GameObject; } }
    public float CollisionParticleLifespan { get { return 4.2f; } }
    public float ProjectileRadius { get { return .15f; } }
    public float ProjectileSpeed { get { return 99; } }
    public float Radius { get { return 4; } }
    public float Damage { get { return 50; } }
    public float CastTime { get { return .4f; } }
    public float WaitTime { get { return 1.5f; } }
    public float StunTime { get { return 2.0f; } }


}
