using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/* Detects collision of spells, for overview of how spells and collisions are being 
 handled look at Spell.cs documentation
 
Uses short sphere cast for non-instant spells, and a long ray cast for instant spells*/
public class SpellCollision : MonoBehaviour
{
    Spell spell;
    SpellManager spellManager;
    SpellDestruction spellDestruction;
    GameObject localPlayer;

    string shotBy;
    float spellRadius;
    float distanceOfSphereCast;
    bool isInstantCollisionChecked = false;

    void Start()
    {
        spell = SpellManager.getSpellFromName(gameObject.name);
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        spellDestruction = GameObject.Find("Spell").GetComponent<SpellDestruction>();
        localPlayer = GameObject.Find("Managers/NetworkManager").GetComponent<OurNetworkManager>().client.connection.playerControllers[0].gameObject;


        isInstantCollisionChecked = isInstantCollisionCheckBySpell(spell.name);
        if (isInstantCollisionChecked)
        {
            instantCollisionHitCheck();
        }
        SetDistanceOfSpehereCast();
    }


    /*Creates new SpellCollision Component, then attaches it to the passed in GameObject*/
    public static void AddSpellCollision(GameObject attachTo, float projRadius, string shotByIn)
    {
        SpellCollision spellCollisionComponent = attachTo.AddComponent<SpellCollision>();
        spellCollisionComponent.spellRadius = projRadius;
        spellCollisionComponent.shotBy = shotByIn;
    }

    /*Faster spells require longer sphere casts*/
    void SetDistanceOfSpehereCast()
    {
        distanceOfSphereCast = spell.projectileSpeed * .1f;

        if (spell.projectileSpeed > 50)
            distanceOfSphereCast *= 2.0f;
        else if (spell.projectileSpeed > 40)
            distanceOfSphereCast *= 1.5f;
        else if (spell.projectileSpeed > 30)
            distanceOfSphereCast *= 1.2f;
    }



    void Update()
    {
        if (!isInstantCollisionChecked)
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
                Damage damage = new Damage(spell, hitTransform);
                damage.ApplyDamage();
            }

            Debug.Log("PreCMD: " );
            spellDestruction.CmdCallRpcDestroySpellOnCollision(gameObject.name, Hit.point);
        }

    }



    /*Spell is instantly spawned at hitmarker*/
    void instantCollisionHitCheck()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        RaycastHit Hit;

        if (Physics.Raycast(origin, direction, out Hit, spell.maxRange))
        {
            OnSpellHit(Hit);
        }
    }


    /*Similar results to is ValidDistanceCheck, except also includes spells such as Pain*/
    static bool isInstantCollisionCheckBySpell(string spellName)
    {
        if (spellName.StartsWith("Pain"))
            return true;
        if (spellName.StartsWith("Fear"))
            return true;
        else if (spellName.StartsWith("Soul Void"))
            return true;
        else if (spellName.StartsWith("Ice Wall"))
            return true;
        else
            return false;
    }
    

}
