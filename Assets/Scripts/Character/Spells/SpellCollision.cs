using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellCollision : MonoBehaviour
{

    Spell spell;
    SpellManager spellManager;
    

    void Start()
    {
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        spell = spellManager.getSpellFromName(gameObject.name);
        Physics.IgnoreLayerCollision(8, 10); //Ignore's collision between player and spell

    }

    /*Plays destroy sequence of a spell OnCollision*/
    void OnCollisionEnter(Collision collision)
    {
        collisionDamagePlayer(collision);
        collisionDestroySpell(collision);
    }

    void collisionDestroySpell(Collision collision)
    {
        Destroy(gameObject);

        if (spell.collisionParticle)
        {
            GameObject collisionParticles = Instantiate(spell.collisionParticle, collision.contacts[0].point, Quaternion.identity);
            Destroy(collisionParticles, 1.5f); //Depending on what we hit were going to want to adjust destory time
            collisionParticles.transform.parent = GameObject.Find("Managers/SpellManager").transform;
        }
    }

    void collisionDamagePlayer(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            EnemyHealthBar enemyHealthBar = collision.gameObject.GetComponent<EnemyHealthBar>();
            if (enemyHealthBar)
            {
                enemyHealthBar.takeDamage(spell.damage);
                enemyHealthBar.regenerateHealth();
            }
        }
        
        Debug.Log("We Hit: " + collision.gameObject.tag);
    }
}
