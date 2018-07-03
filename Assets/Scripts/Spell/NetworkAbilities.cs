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



    #region Projectiles

    public void NetworkRpcDestroySpellOnCollision(string spellName, Vector3 position, int shotBy)
    {
        photonView.RPC("RpcDestroySpellOnCollision", PhotonTargets.All, spellName, position, shotBy);
    }


    [PunRPC] /*Destorys spell, and creates collision particle over the network*/
    void RpcDestroySpellOnCollision(string spellName, Vector3 position, int shotBy)
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

        if (abilityData.CollisionParticle)
        {
            AbilityIdentifier spellIdentifier = spellInWorldToDestroy.GetComponent<AbilityIdentifier>();
            GameObject collisionParticles = SpellCreation.CreateCollisionParticlesInWorld(spellName, position, abilityData, spellIdentifier);
            ((AbilityProjectile) abilityData.Ability).ExplodeParticles(collisionParticles);
        }
        else
        {
            Debug.Log("SpellDestruction - spell.collisionParticle: null");
        }
    }

    [PunRPC]
    void RpcFireSpell(string spellName, Quaternion rotationToTarget, Vector3 castSpawn, string shotByname, int shotBy)
    {
        AbilityData spell = SpellManager.GetSpellStatsFromName(spellName);

        GameObject spellObject = SpellCreation.CreateSpellInWorld(spell, castSpawn, rotationToTarget, shotByname, shotBy);

        ((AbilityProjectile) spell.Ability).DestroySpellByTime(spellObject);
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
