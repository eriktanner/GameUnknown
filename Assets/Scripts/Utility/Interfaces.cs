using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IHaveHealth
{
    float CurrentHealth { get; set; }
    float MaxHealth { get; set; }
}

interface IHaveMana
{
    float CurrentMana { get; }
    float MaxMana { get; }
}

interface IHaveCollisionParticles
{
    GameObject CollisionParticles { get; }
    float CollisionParticleLifespan { get; }
}

interface IBasicSpell : IProjectile, ICast, IHaveCollisionParticles { }
interface IBasicAOE : IBasicSpell, IAOE, IInstantCollision { }

interface IWait
{
    float WaitTime { get; }
}

interface ICanTick
{

}



interface ICast
{
    float CastTime { get; }
}

interface IProjectile
{
    float ProjectileRadius { get; }
    float ProjectileSpeed { get; }
}

interface IAOE
{
    float Radius { get; }
}

interface IDamage
{
    float Damage { get; }
}

interface IHeal
{
    float Heal { get; }
}

interface ITick
{
    float NumTicks { get; }
    float TimeBetweenTicks { get; }
    DOTHitCheck DOTHitCheck { get; }
}

interface IDOT : ITick
{
    float TotalDOT { get; }
}

interface IHOT : ITick
{
    float TotalHOT { get; }
}

interface IStunOT
{
    float StunTime { get; }
}

interface IStun
{
    float StunTime { get; }
}

interface IInstantCollision { }
interface ICheckForGround { }
interface ICheckForDistance { }