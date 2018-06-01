using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpellController : NetworkBehaviour
{

    Spell spell;
    SpellManager spellManager;

    public string shotBy;



    void Start()
    {
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        spell = spellManager.getSpellFromName(gameObject.name);

    }

    /*Plays destroy sequence of a spell OnCollision*/
    void OnCollisionEnter(Collision collision)
    {
        if (!isServer) //Only allow server to handle collision
            return;

        //Debug.Log("Collision With: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);
        GameObject collisionGameObject = collision.gameObject;

        if (collisionGameObject.name != shotBy)
        {
            
            if (collisionGameObject.tag == "Player")
            {
                CmdCollisionDamagePlayer(collisionGameObject.name);
            }
            CmdCollisionDestroySpell(collision.contacts[0].point); //Must be called last b/c of destrction of gameObject
            
        }

    }




    [Command]
    void CmdCollisionDamagePlayer(string playerName)
    {
            /*
            EnemyHealthBar enemyHealthBar = collision.gameObject.GetComponent<EnemyHealthBar>();
            if (enemyHealthBar)
            {
                enemyHealthBar.takeDamage(spellThatHit.damage);
                enemyHealthBar.regenerateHealth();
            }*/

        //Debug.Log("Server Notified We Hit: " + playerName);

    }



    [Command]
    void CmdCollisionDestroySpell(Vector3 position)
    {

        //Debug.Log("CommandDestroySpell");
        NetworkServer.Destroy(gameObject);

        if (spell.collisionParticle)
        {
            GameObject collisionParticles = Instantiate(spell.collisionParticle, position, Quaternion.identity);
            Destroy(collisionParticles, 1.5f); //Depending on what we hit were going to want to adjust destory time
            NetworkServer.Spawn(collisionParticles);

            collisionParticles.transform.parent = GameObject.Find("Managers/SpellManager").transform; //TO DELETE - currently just avoid clutter
        }
    }

}
