using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAbilities : Photon.MonoBehaviour {

    public static NetworkAbilities Instance { get; private set; }
    SpellDestruction SpellDestruction;

    void Start()
    {
        EnsureSingleton();
        SpellDestruction = SpellDestruction.Instance;
    }

    void EnsureSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    #region General

    public void NetworkCreateCollisionParticles(string abilityName, Vector3 position)
    {
        photonView.RPC("RpcCreateCollisionParticles", PhotonTargets.All, abilityName, position);
    }

    [PunRPC]
    public void RpcCreateCollisionParticles(string abilityName, Vector3 position)
    {
        SpellCreation.CreateCollisionParticlesInWorld(abilityName, position);
    }

    #endregion



    #region Projectiles

    public void NetworkRpcDestroySpellOnCollision(string spellName, int shotBy)
    {
        photonView.RPC("RpcDestroySpellOnCollision", PhotonTargets.All, spellName, shotBy);
    }


    [PunRPC] /*Destorys spell, and creates collision particle over the network*/
    void RpcDestroySpellOnCollision(string spellName, int shotBy)
    {

        AbilityData abilityData = SpellManager.GetSpellStatsFromName(spellName);
        GameObject spellInWorldToDestroy = SpellManager.GetObjectFromSpellName(spellName);
        if (spellInWorldToDestroy == null)
        {
            Debug.Log("SpellInWorldToDestroyNotFound: " + spellName);
            return;
        }

        AbilityData spell = AbilityDictionary.GetAbilityDataFromAbilityName(spellName);
        spell.Ability.TimedDestruction(spellInWorldToDestroy);  
    }

    [PunRPC]
    void RpcFireSpell(string spellName, Quaternion rotationToTarget, Vector3 castSpawn, string shotByname, int shotBy)
    {
        AbilityData spell = SpellManager.GetSpellStatsFromName(spellName);

        GameObject spellObject = SpellCreation.CreateSpellInWorld(spell, castSpawn, rotationToTarget, shotByname, shotBy);

        ((AbilityProjectile) spell.Ability).DestroyProjectileByTime(spellObject);
    }


    [PunRPC]
    void ServerKeepProjectileCountInSync()
    {
        SpellManager.ProjectileCount++;
        NetworkAbilities.Instance.photonView.RPC("SetClientProjectileCountToServerCount", PhotonTargets.All, SpellManager.ProjectileCount);
    }

    [PunRPC]
    void SetClientProjectileCountToServerCount(int serverProjectileCount)
    {
        SpellManager.ProjectileCount = serverProjectileCount;
    }
    
    #endregion 

}
