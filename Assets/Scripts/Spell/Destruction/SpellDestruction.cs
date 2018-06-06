using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

/*Handles spell destruction of spells and their collision particles. Note: destruction does not handle
actual spell effects*/
[RequireComponent((typeof(SpellEffects)))]
public class SpellDestruction : NetworkBehaviour
{
    public SpellEffects spellEffects;
    GameObject localPlayer;

    
    //See OnPlayerStart Class in Utility
    public void setLocalPlayerOnPlayerStart()
    {
        localPlayer = GameObject.Find("Managers/NetworkManager").GetComponent<OurNetworkManager>().client.connection.playerControllers[0].gameObject;
    }




    //*********************************** Lookups ***************************************/

    /*Destroys a casted spell by wait time*/
    public void destroySpell(GameObject spellObject, float waitTime)
    {
        if (spellObject == null)
            return;
        
        string spellName = spellObject.name;

        if (spellName.StartsWith("Fireball"))
            StartCoroutine(waitAndCall(spellObject, waitTime, destroyFireball));
        if (spellName.StartsWith("Pain"))
            StartCoroutine(waitAndCall(spellObject, waitTime, destroyPain));
        else
            StartCoroutine(waitAndCall(spellObject, waitTime, defaultDestroy));
    }


    /*We may want to handle the destruction of the particles differently from the casted spell*/
    public void findParticleWaitTimeAndDestructionMethod(GameObject particles, float waitTime)
    {
        if (particles == null)
            return;

        string spellName = particles.name;

        if (spellName.StartsWith("Magic Fear"))
            StartCoroutine(waitAndCall(particles, 3.75f, explodeFearParticles));
        else if (spellName.StartsWith("Magic Soul Void"))
            StartCoroutine(waitAndCall(particles, 3.75f, explodeSoulVoidParticles));
        else if (spellName.StartsWith("Ice Wall"))
            StartCoroutine(waitAndCall(particles, 0, explodeIceWall));
        else
            StartCoroutine(waitAndCall(particles, waitTime, defaultDestroy));
    }

    /*Depending on the spell, each collision particles may have different destroy times*/
    float GetParticleDestroyTimeBySpell(string spellName)
    {
        Spell spell = SpellManager.getSpellFromName(spellName);

        if (spellName.StartsWith("Fear"))
            return 7.6f;
        else
            return 1.5f;
    }




    //*********************************** Destruction ***************************************/

    public void spawnAndDestroyParticles(string spellName, Vector3 position, float waitTime)
    {
        Spell spell = SpellManager.getSpellFromName(spellName);

        GameObject collisionParticles = Instantiate(spell.collisionParticle, position, Quaternion.identity);
        findParticleWaitTimeAndDestructionMethod(collisionParticles, waitTime);
    }
  
    public void defaultDestroy(GameObject spellObject)
    {
        Destroy(spellObject);
    }

    public void defaultDestroy(GameObject spellObject, float time)
    {
        Destroy(spellObject, time);
    }





    //*********************************** Spells ***************************************/
    //Note: Always called, even if spell does not hit (give "destroy" prefix)

   
    void destroyPain(GameObject spellObject)
    {
        DestroyPain destroyPain = new DestroyPain(gameObject, spellObject);
        destroyPain.destroyPain();
    }
    
    void destroyFireball(GameObject spellObject)
    {
        DestroyFireball destroyFireball = new DestroyFireball(gameObject, spellObject);
        destroyFireball.destroyFireball();
    }




    //******************************** Particles ***************************************/
    //Note: Particle destruction only gets called (automatically) from RPC funcion if spell hits (give "explode" prefix)

    
    void explodeFearParticles(GameObject particles)
    {
        ExplodeFear explodeFear = new ExplodeFear(localPlayer, particles);
        explodeFear.explodeFear();
    }

    void explodeSoulVoidParticles(GameObject particles)
    {
        ExplodeSoulVoid explodeSoulVoid = new ExplodeSoulVoid(localPlayer, particles);
        explodeSoulVoid.explodeSoulVoid();
    }

    void explodeIceWall(GameObject particles)
    {
        if (particles == null)
            Debug.Log("particles are null");
        ExplodeIceWall explodeIceWall = new ExplodeIceWall(localPlayer, particles);
        explodeIceWall.explodeIceWall();
    }








    //********************************* Networking ***************************************/

    [Command]
    public void CmdCallRpcDestroySpellOnCollision(string spellName, Vector3 position)
    {
        RpcDestroySpellOnCollision(spellName, position);
    }

    [ClientRpc]
    void RpcDestroySpellOnCollision(string spellName, Vector3 position)
    {
        Spell spell = SpellManager.getSpellFromName(spellName);
        GameObject spellInWorldToDestroy = SpellManager.getObjectFromSpellName(spellName);

        if (spellInWorldToDestroy == null)
            return;

        Destroy(spellInWorldToDestroy);

        if (spell.collisionParticle)
        {
            GameObject collisionParticles = Instantiate(spell.collisionParticle, position, Quaternion.identity);
            findParticleWaitTimeAndDestructionMethod(collisionParticles, GetParticleDestroyTimeBySpell(spellName)); 
        }
    }




    //*********************************** Utility ***************************************/

    /*Waits a set amount of time, then calls destroy method on the spell object passed in*/
    IEnumerator waitAndCall(GameObject spellObject, float waitTime, Action<GameObject> destroySpellMethod)
    {
        yield return new WaitForSeconds(waitTime);
        destroySpellMethod(spellObject);
    }

}
