using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/*Attach to local player, notify server you've been hit by a spell*/
public class NetworkSpellCollision : NetworkBehaviour {
    
    SpellManager spellManager;


    void Start()
    {
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();

    }

    /*Plays destroy sequence of a spell OnCollision*/
    void OnCollisionEnter(Collision collision)
    {
        collisionDamagePlayer(collision);
    }


    [Client]
    void collisionDamagePlayer(Collision collision)
    {

        if (collision.gameObject.tag == "Spell")
        {

            if (spellManager == null)
                Debug.Log("SpellManager is null");
            if (collision.gameObject.name == null)
                Debug.Log("name is null");

            Spell spellThatHit = spellManager.getSpellFromName(collision.gameObject.name);

            if (gameObject.name.Equals(spellThatHit.shotBy)) //Do not damage self if you casted that spell
                return;

            /*

            EnemyHealthBar enemyHealthBar = collision.gameObject.GetComponent<EnemyHealthBar>();
            if (enemyHealthBar)
            {
                enemyHealthBar.takeDamage(spellThatHit.damage);
                enemyHealthBar.regenerateHealth();
            }*/


            Debug.Log("We Hit: " + collision.gameObject.tag);
            CmdNotifyServerPlayerHitBySpell(gameObject.name);
        }

    }


    [Command]
    void CmdNotifyServerPlayerHitBySpell(string id)
    {
        Debug.Log("Server Notified: " + id + " was hit by a spell");
    }


}
