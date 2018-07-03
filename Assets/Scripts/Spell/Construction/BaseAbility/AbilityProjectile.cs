using UnityEngine;
using System;
using System.Collections;

public abstract class AbilityProjectile : Ability {


    public override void InitAbilityEffectSequence(GameObject spellObject, RaycastHit Hit)
    {
        NetworkAbilities.Instance.NetworkCreateCollisionParticles(spellObject.name, Hit.point);
        NetworkAbilities.Instance.NetworkRpcDestroySpellOnCollision(spellObject.name, spellObject.GetComponent<AbilityIdentifier>().ShotByID);
    }


    public void Fire(AbilityData abilityData, Vector3 castSpawn, Vector3 hitPoint)
    {
        Vector3 aimToFromFirePosition = hitPoint - castSpawn;
        Quaternion rotationToTarget = Quaternion.LookRotation(aimToFromFirePosition);


        NetworkFireSpell(abilityData, castSpawn, rotationToTarget);
    }


    void NetworkFireSpell(AbilityData abilityData, Vector3 castSpawn, Quaternion rotationToTarget)
    {
        if (abilityData.AbilityName == null)
        {
            Debug.Log("NetworkAbilities - NetworkFireSpell: null");
            return;
        }

        NetworkAbilities.Instance.photonView.RPC("RpcFireSpell", PhotonTargets.All, abilityData.AbilityName, rotationToTarget, castSpawn, PlayerManager.LocalPlayer.name, PhotonNetwork.player.ID);
        NetworkAbilities.Instance.photonView.RPC("ServerKeepProjectileCountInSync", PhotonTargets.MasterClient); //Call after fire, network too slow other way around
    }



    /*Explodes particles on collision*/
    public void ExplodeParticles(GameObject particleObject)
    {
        if (particleObject == null)
            return;

        AbilityData spell = AbilityDictionary.GetAbilityDataFromSpellObject(particleObject);

        if (spell as IWait != null)
            TaskManager.CreateTask(WaitAndCall(particleObject, ((IWait) spell).WaitTime, spell.Ability.AreaOfEffect));
        else
            spell.Ability.AreaOfEffect(particleObject);
    }

    /*Destroys a casted spell by its lifespan*/
    public void DestroyProjectileByTime(GameObject projectileObject)
    {
        if (projectileObject == null)
            return;

        AbilityData projectile = AbilityDictionary.GetAbilityDataFromSpellObject(projectileObject);

        TaskManager.CreateTask(WaitAndCall(projectileObject, GameplayUtility.GetLifespanOfSpell(projectile), projectile.Ability.TimedDestruction));
    }


    //*********************************** Utility ***************************************/

    IEnumerator WaitAndCall(GameObject spellObject, float waitTime, Action<GameObject> destroyMethod)
    {
        yield return new WaitForSeconds(waitTime);
        if (spellObject != null)
        {
            destroyMethod(spellObject);
        }
    }
}
