using UnityEngine;
using System;
using System.Collections;

public class AbilityProjectile : Ability {


    public override void InitAbilityEffectSequence(GameObject spellObject, GameObject target, int shotBy)
    {
        NetworkAbilities.Instance.NetworkRpcDestroySpellOnCollision(spellObject.name, target.transform.position, shotBy);
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
            TaskManager.CreateTask(WaitAndCall(particleObject, ((IWait)spell).WaitTime, spell.Ability.AreaOfEffect));
        else
            spell.Ability.AreaOfEffect(particleObject);
    }

    /*Destroys a casted spell by its lifespan*/
    public void DestroySpellByTime(GameObject spellObject)
    {
        if (spellObject == null)
            return;

        AbilityData spell = AbilityDictionary.GetAbilityDataFromSpellObject(spellObject);

        TaskManager.CreateTask(WaitAndCall(spellObject, GameplayUtility.GetLifespanOfSpell(spell), spell.Ability.TimedDestruction));
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
