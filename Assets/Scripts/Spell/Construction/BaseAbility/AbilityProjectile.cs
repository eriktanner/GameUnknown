using UnityEngine;
using System;
using System.Collections;

public abstract class AbilityProjectile : Ability {


    public override void CastAbility(GameObject player, Vector3 spawnSpot, Vector3 aimTowards)
    {
        base.CastAbility(player, spawnSpot, aimTowards);
        Fire(AbilityData, spawnSpot, aimTowards);
    }

    public override void InitAbilityEffectSequence(GameObject caster, GameObject spellObject, RaycastHit hit)
    {
        NetworkAbilities.Instance.NetworkCreateCollisionParticles(spellObject.name, hit.point);
        NetworkAbilities.Instance.NetworkRpcDestroySpellOnCollision(spellObject.name);
    }


    public void Fire(AbilityData abilityData, Vector3 castSpawn, Vector3 hitPoint)
    {
        Vector3 aimToFromFirePosition = hitPoint - castSpawn;
        Quaternion rotationToTarget = Quaternion.LookRotation(aimToFromFirePosition);


        NetworkFire(abilityData, castSpawn, rotationToTarget);
    }


    void NetworkFire(AbilityData abilityData, Vector3 castSpawn, Quaternion rotationToTarget)
    {
        if (abilityData.AbilityName == null)
        {
            Debug.Log("NetworkAbilities - NetworkFireSpell: null");
            return;
        }

        NetworkAbilities.Instance.photonView.RPC("RpcFireSpell", PhotonTargets.All, abilityData.AbilityName, rotationToTarget, castSpawn, PlayerManager.LocalPlayer.name, PhotonNetwork.player.ID);
        NetworkAbilities.Instance.photonView.RPC("ServerKeepProjectileCountInSync", PhotonTargets.MasterClient); //Call after fire, network too slow other way around
    }



    

    /*Destroys a casted spell by its lifespan*/
    public void DestroyProjectileByTime(GameObject projectileObject)
    {
        if (projectileObject == null)
            return;

        AbilityData projectile = AbilityDictionary.GetAbilityDataFromSpellObject(projectileObject);

        TaskManager.CreateTask(TimedWaitDestroy(projectileObject, AbilityUtility.GetLifespanOfSpell(projectile), TimedDestruction));
    }

    public virtual void TimedDestruction(GameObject spellObject)
    {
        Destroy(spellObject);
    }

    //*********************************** Utility ***************************************/

    IEnumerator TimedWaitDestroy(GameObject spellObject, float waitTime, Action<GameObject> destroyMethod)
    {
        yield return new WaitForSeconds(waitTime);
        if (spellObject != null)
        {
            destroyMethod(spellObject);
        }
    }
}
