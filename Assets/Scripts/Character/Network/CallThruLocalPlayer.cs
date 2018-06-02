using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/*This class will be used to call Network functions such as [Command] thru this player. Use as a Utility*/
public class CallThruLocalPlayer : NetworkBehaviour {

    SpellManager spellManager;


    void Start()
    {
        if (GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>() != null)
            spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
    }

    [Command]
    public void CmdCallRpcDestroySpellOnCollision(string spellName, Vector3 position)
    {
        RpcDestroySpellOnCollision(spellName, position);
    }

    [ClientRpc]
    void RpcDestroySpellOnCollision(string spellName, Vector3 position)
    {
        Spell spell = spellManager.getSpellFromName(spellName);
        Destroy(GameObject.Find("Managers/SpellManager/" + spellName));
        if (spell.collisionParticle)
        {
            GameObject collisionParticles = Instantiate(spell.collisionParticle, position, Quaternion.identity);
            Destroy(collisionParticles, 1.5f); //Depending on what we hit were going to want to adjust destory time
        }
    }

    [Command]
    public void CmdCollisionDamagePlayer(string playerName)
    {
        /*
        EnemyHealthBar enemyHealthBar = collision.gameObject.GetComponent<EnemyHealthBar>();
        if (enemyHealthBar)
        {
            enemyHealthBar.takeDamage(spellThatHit.damage);
            enemyHealthBar.regenerateHealth();
        }*/

        Debug.Log("Server Notified We Hit: " + playerName);

    }

}
