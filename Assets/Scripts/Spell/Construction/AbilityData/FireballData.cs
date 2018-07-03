using UnityEngine;

public class FireballData : AbilityData, IProjectile, IDamage, ICast {
    
    public override string AbilityName { get { return "Fireball"; } }
    public override Ability Ability { get { return new AbilityCastSpell(this); } }
    public override GameObject Prefab { get { return Resources.Load("SpellPrefabs/Magic Fire Ball") as GameObject; } }
    public override GameObject CollisionParticle { get { return Resources.Load("SpellPrefabs/CollisionParticles/PComponent Fire Embers") as GameObject; } }
    public override Texture2D Icon { get; protected set; }
    public override float Cost { get { return 30; } }
    public override float Cooldown { get { return 1; } }
    public override float MaxRange { get { return 50; } }

    public float ProjectileRadius { get { return .15f; } }
    public float ProjectileSpeed { get { return 45; } }
    public float Damage { get { return 150; } }
    public float CastTime { get { return .9f; } }

}
