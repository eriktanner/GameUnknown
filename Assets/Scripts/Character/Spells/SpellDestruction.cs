using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

/*Handles spell destruction, must be attached to each player because we cannot
 use coroutines statically*/
[RequireComponent((typeof(SpellEffects)))]
public class SpellDestruction : NetworkBehaviour
{
    public SpellEffects spellEffects;
    SpellManager spellManager;

    void Start()
    {
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        
    }
 
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


    /*We may want to handle the destruction of the particles differently from the casted spell */
    public void destroyParticlesByCollision(GameObject particles, float waitTime)
    {
        if (particles == null)
            return;
        
        string spellName = particles.name;

        if (spellName.StartsWith("Magic Fear"))
            StartCoroutine(waitAndCall(particles, 3.75f, fearExplode));
        else
            StartCoroutine(waitAndCall(particles, waitTime, defaultDestroy));
    }

    /*Waits a set amount of time, then calls destroy method on the spell object passed in*/
    IEnumerator waitAndCall(GameObject spellObject, float waitTime, Action<GameObject> destroySpellMethod)
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

        Spell spell = SpellManager.getSpellFromName(spellObject.name);
        spawnAndDestroyParticles(spell.name, spellObject.transform.position, 1.5f);

        defaultDestroy(spellObject);
    }

    

    /*We want the embers and smoke to remain after destruction*/
    void destroyFireball(GameObject spellObject)
    {
        if (spellObject == null)
            return;

        Spell spell = SpellManager.getSpellFromName(spellObject.name);
        if (spell != null && spellObject != null)
        {
            //Fireball sequence
            defaultDestroy(spellObject);
        }

    }




    

    /*Cast a sphere for all players in range, draw a ray cast to them to see if
     they are in line of sight, if so fear*/
    void fearExplode(GameObject particles)
    {
        Vector3 origin = particles.gameObject.transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(origin, 4.0f);

        
        for (int i = 0; i < hitColliders.Length; i++)
        {
            
            if (hitColliders[i].gameObject.tag == "Player")
            {
                Vector3 toPosition = hitColliders[i].gameObject.transform.position;
                Vector3 direction = toPosition - origin;
                float distance = (toPosition - direction).magnitude;

                RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, distance);

                
                bool isInLineOfSight = true;
                foreach(RaycastHit hit in hits) //Other players will not effect Line of Sight
                {
                    
                    if (hit.transform.gameObject.tag != "Player")
                        isInLineOfSight = false;
                }

                spellEffects.CmdFearPlayer(hitColliders[i].gameObject);
            }
            
        }

        Destroy(particles, 2.0f);
    }




    




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
            destroyParticlesByCollision(collisionParticles, GetParticleDestroyTimeBySpell(spellName)); 
        }
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

    

    void spawnAndDestroyParticles(string spellName, Vector3 position, float waitTime)
    {
        Spell spell = SpellManager.getSpellFromName(spellName);

        GameObject collisionParticles = Instantiate(spell.collisionParticle, position, Quaternion.identity);
        destroyParticlesByCollision(collisionParticles, waitTime);
    }



    

}
