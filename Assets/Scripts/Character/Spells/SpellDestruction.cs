using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

/*Handling spell destruction was definitely complicated.
 Reasons why this was so complicated:
 1. Destroy sequences vary by spell
 2. Needed wait times to destroy - therefore cannot simply call NetworkDestroy(does not take wait time)
 3. We need to destroy by two ways - By Collision and By Time, needed to make them cofunctional and easy to perform
 How it works:
 1. DestroyByTime - Call destroySpellOnServer and pass in wait time when spell is created and pass it in, 
 this will wait the time and handle the objects destruction
 2. Collision - Detect collision and call destroySpellOnServer with no wait time, this will be instant
 IMPORTANT: Do not make a destroy sequence without checking if the spellObject == null
 (Wait Destroy may try to destroy object destroyed by collision)*/
public class SpellDestruction : NetworkBehaviour
{

    SpellManager spellManager;

    void Start()
    {
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
    }

    public void destroySpellOnServer(GameObject spellObject)
    {
        string spellName = spellObject.name;

        if (spellName.StartsWith("Fireball"))
            destroyFireball(spellObject);
        if (spellName.StartsWith("Pain"))
            destroyPain(spellObject);
        else
            defaultDestroy(spellObject);
    }

    public void destroySpellOnServer(GameObject spellObject, float waitTime)
    {
        string spellName = spellObject.name;

        if (spellName.StartsWith("Fireball"))
            StartCoroutine(waitAndDestroy(spellObject, waitTime, destroyFireball));
        if (spellName.StartsWith("Pain"))
            StartCoroutine(waitAndDestroy(spellObject, waitTime, destroyPain));
        else
            StartCoroutine(waitAndDestroy(spellObject, waitTime, defaultDestroy));
    }


    /*Waits a set amount of time, then calls destroy method on the spell object passed in*/
    IEnumerator waitAndDestroy(GameObject spellObject, float waitTime, Action<GameObject> destroySpellMethod)
    {
        yield return new WaitForSeconds(waitTime);
        destroySpellMethod(spellObject);
    }

    /*Destroy Immediately*/
    void defaultDestroy(GameObject spellObject)
    {
        Destroy(spellObject);
    }

    /*We want to display collision of pain at end point no matter what, so that its range is known to player*/
    void destroyPain(GameObject spellObject)
    {
        if (spellObject == null)
            return;

        Spell spell = spellManager.getSpellFromName(spellObject.name);
        spawnAndDestroyParticles(spell.name, spellObject.transform.position, 1.5f);

        defaultDestroy(spellObject);
    }

    /*We want the embers and smoke to remain after destruction*/
    void destroyFireball(GameObject spellObject)
    {
        if (spellObject == null)
            return;

        Spell spell = spellManager.getSpellFromName(spellObject.name);
        if (spell != null && spellObject != null)
        {
            //Fireball sequence
            defaultDestroy(spellObject);
        }

    }

    void spawnAndDestroyParticles(string spellName, Vector3 position, float waitTime)
    {
        Spell spell = spellManager.getSpellFromName(spellName);

        GameObject collisionParticles = Instantiate(spell.collisionParticle, position, Quaternion.identity);
        Destroy(collisionParticles, waitTime);

    }
    
    

    [Command] /*Orders server to display collision particles and handle their destruction*/
    void CmdSpawnAndDestroyParticles(string spellName, Vector3 position, float waitTime)
    {
        Spell spell = spellManager.getSpellFromName(spellName);

        GameObject collisionParticles = Instantiate(spell.collisionParticle, position, Quaternion.identity);
        NetworkServer.Spawn(collisionParticles);
        waitAndDestroy(collisionParticles, waitTime, defaultDestroy);
    }

    [Command] /*Tell Server to destroy a game object (use for spells and particles)*/
    void CmdDestroySpellOrParticle(GameObject spellObject)
    {
        NetworkServer.Destroy(spellObject);
    }
}
