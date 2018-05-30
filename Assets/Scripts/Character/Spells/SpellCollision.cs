using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellCollision : MonoBehaviour {

    Spell spell;
    SpellManager spellManager;

    void Start()
    {
        spell = (Spell) Resources.Load("Spells/" + gameObject.name, typeof(Spell));
        spellManager = GameObject.Find("SpellManager").GetComponent<SpellManager>();
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
        }
    }

    void collisionDamagePlayer(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            print("Collided With Enemy");
        }
        print("Collision Occured");
    }

}
