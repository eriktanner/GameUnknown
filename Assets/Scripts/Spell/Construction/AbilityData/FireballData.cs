using UnityEngine;

public class FireballData : AbilityData, IBasicSpell, IDamage {
    
    public override string AbilityName { get { return "Fireball"; } }
    public override Ability Ability { get { return new AbilityCastSpell(this); } }
    public override GameObject Prefab { get { return Resources.Load("SpellPrefabs/Magic Fire Ball") as GameObject; } }
    public override Texture2D Icon { get; protected set; }
    public override float Cost { get { return 30; } }
    public override float Cooldown { get { return 1; } }
    public override float MaxRange { get { return 50; } }


    public GameObject CollisionParticles { get { return Resources.Load("SpellPrefabs/CollisionParticles/PComponent Fire Embers") as GameObject; } }
    public float CollisionParticleLifespan { get { return 1.5f; } }
    public float ProjectileRadius { get { return .15f; } }
    public float ProjectileSpeed { get { return 45; } }
    public float Damage { get { return 150; } }
    public float CastTime { get { return .9f; } }

}
