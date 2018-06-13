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

    int ShotBy;
    float spellRadius;
    float distanceOfSphereCast;
    bool isInstantCollisionChecked = false;

    void Start()
    {
        spell = SpellDictionary.GetSpellFromSpellObject(gameObject);
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        spellDestruction = GameObject.Find("Spell").GetComponent<SpellDestruction>();
        ShotBy = gameObject.GetComponent<SpellIdentifier>().ShotBy;


        isInstantCollisionChecked = spell.IsInstantCollision;
        if (isInstantCollisionChecked)
        {
            instantCollisionHitCheck();
        }
        SetDistanceOfSpehereCast();
    }


    /*Creates new SpellCollision Component, then attaches it to the passed in GameObject*/
    public static void AddSpellCollision(GameObject attachTo, float projRadius)
    {
        SpellCollision spellCollisionComponent = attachTo.AddComponent<SpellCollision>();
        spellCollisionComponent.spellRadius = projRadius;
    }

    /*Faster spells require longer sphere casts*/
    void SetDistanceOfSpehereCast()
    {
        distanceOfSphereCast = spell.SpellStats.projectileSpeed * .1f;

        if (spell.SpellStats.projectileSpeed > 50)
            distanceOfSphereCast *= 2.0f;
        else if (spell.SpellStats.projectileSpeed > 40)
            distanceOfSphereCast *= 1.5f;
        else if (spell.SpellStats.projectileSpeed > 30)
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

        Transform hitTransform = Hit.transform;
        PhotonView HitPhotonView = hitTransform.gameObject.GetPhotonView();
        
        if (HitPhotonView != null && HitPhotonView.viewID != ShotBy)
        {
            hasAlreadyHit = true;
            

            if (hitTransform.tag == "Player")
            {
                Damage damage = new Damage(spell.SpellStats, hitTransform.gameObject, ShotBy);
                damage.ApplyDamage();
            }
            
        }

        spellDestruction.NetworkRpcDestroySpellOnCollision(gameObject.name, Hit.point, ShotBy);
    }



    /*Spell is instantly spawned at hitmarker*/
    void instantCollisionHitCheck()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        RaycastHit Hit;

        if (Physics.Raycast(origin, direction, out Hit, spell.SpellStats.maxRange))
        {
            OnSpellHit(Hit);
        }
    }

}
