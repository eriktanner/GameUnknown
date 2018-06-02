using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/*Detects collision of spells, call initSpellCollision to init needed info*/
public class SpellCollision : NetworkBehaviour
{

    Spell spell;
    SpellManager spellManager;

    SpellDestruction spellDestruction;
    string shotBy;

    GameObject localPlayer;


    void Start()
    {
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        spell = spellManager.getSpellFromName(gameObject.name);
        localPlayer = GameObject.Find("Managers/NetworkManager").GetComponent<OurNetworkManager>().client.connection.playerControllers[0].gameObject;
    }


    /*Creates new SpellCollision Component, then attaches it to the passed in GameObject*/
    public static void AddSpellCollision(GameObject attachTo, string shotByIn, SpellDestruction spellDestructionIn)
    {
        SpellCollision spellCollisionComponent = attachTo.AddComponent<SpellCollision>();
        spellCollisionComponent.shotBy = shotByIn;
        spellCollisionComponent.spellDestruction = spellDestructionIn;
    }

    /*Plays destroy sequence of a spell OnCollision*/
    void OnCollisionEnter(Collision collision)
    {
        if (localPlayer == null || !localPlayer.name.Equals(shotBy)) //Only allow person who shot to check collision
            return;
        
        GameObject collisionGameObject = collision.gameObject;

        if (!collisionGameObject.name.Equals(shotBy))
        {

            CallThruLocalPlayer localPlayerCalls = localPlayer.gameObject.GetComponent<CallThruLocalPlayer>();
            if (collisionGameObject.tag == "Player")
            {
                localPlayerCalls.CmdCollisionDamagePlayer(collisionGameObject.name);
            }
            localPlayerCalls.CmdCallRpcDestroySpellOnCollision(gameObject.name, collision.contacts[0].point);
        }

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
