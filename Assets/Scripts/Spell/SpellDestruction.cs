using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

/*Handles spell destruction of spells and their collision particles. Note: destruction does not handle
actual spell effects*/
public class SpellDestruction : Photon.MonoBehaviour
{
    
    /*Explodes particles on collision*/
    public void ExplodeParticles(GameObject particleObject)
    {
        if (particleObject == null)
            return;

        Spell spell = SpellDictionary.GetSpellFromSpellObject(particleObject);

        if (spell != null)
            StartCoroutine(waitAndCall(spell.TimeFromHitToParticleExplosion, spell.ParticleDestruction));
    }

    /*Destroys a casted spell by its lifespan*/
    public void DestroySpellByTime(GameObject spellObject)
    {
        if (spellObject == null)
            return;

        Spell spell = SpellDictionary.GetSpellFromSpellObject(spellObject);
     
        if (spell != null)
            StartCoroutine(waitAndCall(GetLifespanOfSpell(spell), spell.SpellDestruction));   
    }

    


 

    //********************************* Networking ***************************************/

    public void NetworkRpcDestroySpellOnCollision(string spellName, Vector3 position, int shotBy)
    {
        photonView.RPC("RpcDestroySpellOnCollision", PhotonTargets.All, spellName, position, shotBy);
    }

    [PunRPC] /*Destorys spell, and creates collision particle over the network*/
    void RpcDestroySpellOnCollision(string spellName, Vector3 position, int shotBy)
    {

        SpellStats spell = SpellManager.GetSpellStatsFromName(spellName);
        GameObject spellInWorldToDestroy = SpellManager.getObjectFromSpellName(spellName);

        if (spellInWorldToDestroy == null || spell == null)
        {
            Debug.Log("SpellDestruction - RpcDestroySpellOnCollision: null");
            return;
        }
        Destroy(spellInWorldToDestroy);


        if (spell.collisionParticle)
        {

            SpellIdentifier spellIdentifier = spellInWorldToDestroy.GetComponent<SpellIdentifier>();
            if (spellIdentifier == null)
            {
                Debug.Log("SpellDestruction - RpcDestroySpellOnCollision: no spell Identifier");
                return;
            }

            string SpellNameToTransfer = spellIdentifier.SpellName;
            int shotByToTransfer = spellIdentifier.ShotBy;
            string shotByNameToTransfer = spellIdentifier.ShotByName;


            GameObject collisionParticles = Instantiate(spell.collisionParticle, position, Quaternion.identity);
            collisionParticles.AddComponent(SpellDictionary.GetComponentType(SpellManager.getOriginalSpellName(spellName)));
            SpellIdentifier.AddSpellIdentifier(collisionParticles, SpellNameToTransfer, shotByNameToTransfer, shotByToTransfer);

            ExplodeParticles(collisionParticles);
        }
        else {
            Debug.Log("SpellDestruction - spell.collisionParticle: null");
        }

    }


    

    //*********************************** Utility ***************************************/


    /*Waits a set amount of time, then calls destroy method on the spell/particle object passed in*/
    IEnumerator waitAndCall(float waitTime, Action destroyMethod)
    {
        yield return new WaitForSeconds(waitTime);
        if (destroyMethod.Target != null)
            destroyMethod();
    }


    /*Returns the amount of time needed for a spell to travel it's max distance*/
    float GetLifespanOfSpell(Spell spell)
    {
        float lifespan = 0;

        if (spell.IsValidDistanceChecked) //Distance verified BEFORE cast, we can give large destroy time
            lifespan = 10;
        else
            lifespan = spell.SpellStats.maxRange / spell.SpellStats.projectileSpeed; //Regular timed destroy, d = r * t (However often innaccurate)

        return lifespan;
    }
}
