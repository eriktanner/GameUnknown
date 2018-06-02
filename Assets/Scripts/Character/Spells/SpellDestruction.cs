using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

/*Handles spell destruction, must be attached to each player because we cannot
 use coroutines statically*/
public class SpellDestruction : NetworkBehaviour
{

    SpellManager spellManager;

    void Start()
    {
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
    }

    public void destroySpell(GameObject spellObject)
    {
        string spellName = spellObject.name;

        if (spellName.StartsWith("Fireball"))
            destroyFireball(spellObject);
        if (spellName.StartsWith("Pain"))
            destroyPain(spellObject);
        else
            defaultDestroy(spellObject);
    }

    public void destroySpell(GameObject spellObject, float waitTime)
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
    
    
}
