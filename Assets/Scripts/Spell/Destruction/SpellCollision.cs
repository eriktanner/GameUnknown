using UnityEngine;

/* Detects collision of spells, for overview of how spells and collisions are being 
 handled look at Spell.cs documentation
 
Uses short sphere cast for non-instant spells, and a long ray cast for instant spells*/
public class SpellCollision : MonoBehaviour
{
    AbilityData spellData;
    SpellManager spellManager;
    SpellDestruction spellDestruction;

    int ShotBy;
    float spellRadius;
    float distanceOfSphereCast;
    bool isInstantCollisionChecked = false;

    void Start()
    {
        spellData = AbilityDictionary.GetAbilityDataFromAbilityName(gameObject.name);
        spellManager = SpellManager.Instance;
        spellDestruction = SpellDestruction.Instance;
        ShotBy = gameObject.GetComponent<AbilityIdentifier>().ShotByID;
       
        if (spellData as IInstantCollision != null)
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
        if (spellData as IProjectile == null)
            return;

        float projectileSpeed = ((IProjectile) spellData).ProjectileSpeed;
        distanceOfSphereCast = projectileSpeed * .1f;

        if (projectileSpeed > 50)
            distanceOfSphereCast *= 2.0f;
        else if (projectileSpeed > 40)
            distanceOfSphereCast *= 1.5f;
        else if (projectileSpeed > 30)
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
            spellData.Ability.DirectHit(gameObject, hitTransform.gameObject, ShotBy);
        }


        spellData.Ability.InitAbilityEffectSequence(PlayerManager.GetPlayerGameObject(gameObject.GetComponent<AbilityIdentifier>().ShotByName), gameObject, Hit);

    }



    /*Spell is instantly spawned at hitmarker*/
    void instantCollisionHitCheck()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        RaycastHit Hit;

        if (Physics.Raycast(origin, direction, out Hit, spellData.MaxRange))
        {
            OnSpellHit(Hit);
        }
    }

}
