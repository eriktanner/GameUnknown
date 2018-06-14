﻿using System.Collections;
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
            StartCoroutine(waitAndCall(particleObject, spell.TimeFromHitToParticleExplosion, spell.ParticleDestruction));
    }

    /*Destroys a casted spell by its lifespan*/
    public void DestroySpellByTime(GameObject spellObject)
    {
        if (spellObject == null)
            return;

        Spell spell = SpellDictionary.GetSpellFromSpellObject(spellObject);
     
        if (spell != null)
            StartCoroutine(waitAndCall(spellObject, GetLifespanOfSpell(spell), spell.SpellDestruction));   
    }

    


 

    //********************************* Networking ***************************************/

    public void NetworkRpcDestroySpellOnCollision(string spellName, Vector3 position, int shotBy)
    {
        photonView.RPC("RpcDestroySpellOnCollision", PhotonTargets.All, spellName, position, shotBy);
    }

    [PunRPC] /*Destorys spell, and creates collision particle over the network*/
    void RpcDestroySpellOnCollision(string spellName, Vector3 position, int shotBy)
    {

        SpellStats spellStats = SpellManager.GetSpellStatsFromName(spellName);
        GameObject spellInWorldToDestroy = SpellManager.getObjectFromSpellName(spellName);
        Spell spell = (Spell)spellInWorldToDestroy.GetComponent(SpellDictionary.GetTypeFromSpellName(SpellManager.getOriginalSpellName(spellName)));

        if (spellInWorldToDestroy == null || spellStats == null || spell == null)
        {
            Debug.Log("SpellDestruction - RpcDestroySpellOnCollision: null");
            return;
        }


        spell.SpellDestruction();


        if (spellStats.collisionParticle)
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


            GameObject collisionParticles = Instantiate(spellStats.collisionParticle, position, Quaternion.identity);
            collisionParticles.AddComponent(SpellDictionary.GetTypeFromSpellName(SpellManager.getOriginalSpellName(spellName)));
            SpellIdentifier.AddSpellIdentifier(collisionParticles, SpellNameToTransfer, shotByNameToTransfer, shotByToTransfer);

            ExplodeParticles(collisionParticles);
        }
        else {
            Debug.Log("SpellDestruction - spell.collisionParticle: null");
        }
    }


    

    //*********************************** Utility ***************************************/
    
    IEnumerator waitAndCall(GameObject spellObject, float waitTime, Action destroyMethod)
    {
        yield return new WaitForSeconds(waitTime);
        if (spellObject != null)
        {
            destroyMethod();
        }
    }

    float GetLifespanOfSpell(Spell spell)
    {
        float lifespan = 0;

        if (spell.IsValidDistanceChecked) //Arbitrarily large
            lifespan = 10;
        else{
            float timeTakesToTravelMaxDistance = spell.SpellStats.maxRange / spell.SpellStats.projectileSpeed;
            lifespan = timeTakesToTravelMaxDistance;
        }
           
        return lifespan;
    }
}
