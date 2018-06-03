using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Detects collision of spells, for overview of how spells and collisions are being 
 handled look at Spell.cs documentation
 
We are going to want to change our system to using raycast because a spell may not hit on the local
But appears to hit on other clients. This causes the spell to bounce off to other clients, we dont want that, we at least 
want the spell to pass thru, hence no collider. This is more efficient anyways, we wont need the 
collider or rigid body. We are going to need to use a SphereCast*/
public class SpellCollision : MonoBehaviour
{

    Spell spell;
    SpellManager spellManager;
    SpellDestruction spellDestruction;
    GameObject localPlayer;

    string shotBy;
    float spellRadius;
    float distanceOfSphereCast;

void Start()
    {
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        spell = SpellManager.getSpellFromName(gameObject.name);
        localPlayer = GameObject.Find("Managers/NetworkManager").GetComponent<OurNetworkManager>().client.connection.playerControllers[0].gameObject;

        distanceOfSphereCast = spell.projectileSpeed * .05f;
    }


    /*Creates new SpellCollision Component, then attaches it to the passed in GameObject*/
    public static void AddSpellCollision(GameObject attachTo, float projRadius, string shotByIn, SpellDestruction spellDestructionIn)
    {
        SpellCollision spellCollisionComponent = attachTo.AddComponent<SpellCollision>();
        spellCollisionComponent.spellRadius = projRadius;
        spellCollisionComponent.shotBy = shotByIn;
        spellCollisionComponent.spellDestruction = spellDestructionIn;
    }



    void Update()
    {
        CheckForHit();
    }

    /*Creates a SphereCast a short distance out from the spell use to detect if we hit another object.
     Faster spells will have longer SphereCasts*/
    void CheckForHit()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        RaycastHit Hit;


        if (Physics.SphereCast(origin, spellRadius, direction, out Hit, distanceOfSphereCast))
        {
            OnSpellHit(Hit);
        }

    }


    bool hasAlreadyHit = false;
    /*Plays destroy sequence of a spell collision*/
    void OnSpellHit(RaycastHit Hit)
    {
        if (hasAlreadyHit)
            return;

        if (localPlayer == null || !localPlayer.name.Equals(shotBy)) //Only allow person who shot to check collision
            return;



        Transform hitTransform = Hit.transform;

        if (!hitTransform.gameObject.name.Equals(shotBy))
        {
            hasAlreadyHit = true;

            if (hitTransform.tag == "Player")
            {
                HealthBar healthBar = localPlayer.gameObject.GetComponent<HealthBar>();
                healthBar.CmdCollisionDamagePlayer(spell.name, hitTransform.gameObject.name);
            }
            SpellDestruction spellDestruction = localPlayer.gameObject.GetComponent<SpellDestruction>();
            spellDestruction.CmdCallRpcDestroySpellOnCollision(gameObject.name, Hit.point);

            
        }

    }



    
        



    


}
