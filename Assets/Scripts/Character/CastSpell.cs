using System.Collections;
using UnityEngine;

[RequireComponent((typeof(ManaBar)))]
[RequireComponent((typeof(AbilityList)))]
public class CastSpell : Photon.MonoBehaviour
{  
    public ManaBar ManaBar;
    public AbilityList SpellList;
    public Transform castSpawn;
    

    Coroutine CastRoutine;
    CastBar CastBar;
    Camera PlayerCam;

    bool SpellLock = false;
    bool DoCancelCast = false;
    


    void Start()
    {
        PlayerCam = Camera.main;
        CastBar = CastBar.Instance;
    }

    void Update()
    {
        GetInput();
        CancelCast();
    }

    void GetInput()
    {
        if (Input.GetButtonDown("Spell1"))
        {
            if (!SpellList.isOnCooldown(0))
            {
                FireSpell(SpellList.GetAbilityAtIndex(0));
            }
        }
        if (Input.GetButtonDown("Spell2"))
        {
            if (!SpellList.isOnCooldown(1))
                FireSpell(SpellList.GetAbilityAtIndex(1));
        }
        if (Input.GetButtonDown("Spell3"))
        {
            if (!SpellList.isOnCooldown(2))
            {
                FireSpell(SpellList.GetAbilityAtIndex(2));
            }
        }
        if (Input.GetButtonDown("Spell4"))
        {
            if (!SpellList.isOnCooldown(3))
            {
                FireSpell(SpellList.GetAbilityAtIndex(3));
            }
        }
        if (Input.GetButtonDown("Spell5"))
        {
            if (!SpellList.isOnCooldown(4))
                FireSpell(SpellList.GetAbilityAtIndex(4));
        }
        if (Input.GetButtonDown("Spell6"))
        {
            if (!SpellList.isOnCooldown(5))
                FireSpell(SpellList.GetAbilityAtIndex(5));
        }
        DoCancelCast = Input.GetKey(GameKeybindings.CancelCastInput);
    }
    

    void FireSpell(AbilityData abilityData)
    {
        if (SpellLock)
            return;
        
        if (abilityData.Prefab == null)
        {
            Debug.LogWarning("Spell  Prefab is null");
            return;
        }
        
        CastRoutine = StartCoroutine(CastAndFire(abilityData));
    }

    void CancelCast()
    {
        if (DoCancelCast && CastRoutine != null)
        {
            SpellLock = false;
            StopCoroutine(CastRoutine);
        }
    }

    public void SetSpellLock(bool isLocked)
    {
        SpellLock = isLocked;
    }

    

    private IEnumerator CastAndFire(AbilityData abilityData)
    {
        SpellLock = true;

        ICast castAbilityData = (ICast) abilityData;
        if (castAbilityData != null)
            CastBar.CastSpellUI(castAbilityData.CastTime);

        yield return new WaitForSeconds(castAbilityData.CastTime);

        RaycastHit camHit;
        bool camFoundHit;
        Vector3 hitPoint;
        hitPoint = GetHitmarkerPointInWorld(abilityData.MaxRange, out camHit, out camFoundHit);


        if (IsValidCast(abilityData, camHit, camFoundHit))
        {
            ManaBar.burnMana(abilityData.Cost);
            SpellList.TriggerCooldown(abilityData);
            abilityData.Ability.CastAbility(gameObject, castSpawn.position, hitPoint);
        }
        
        SpellLock = false;
    }

    bool IsValidCast(AbilityData abilityData, RaycastHit camHit, bool camFoundHit)
    {
        bool didHitValidLayer = ValidSpellLayer.SpellHitValidLayerBySpell(abilityData.AbilityName, camHit);
        bool isWithinRangeOfSpell = ValidSpellDistance.SpellIsInRange(abilityData.AbilityName, transform.position, camHit.point, camFoundHit);
        return didHitValidLayer && isWithinRangeOfSpell;
    }


    



      

    




    public Vector3 GetHitmarkerPointInWorld(float maxRange, out RaycastHit camHit, out bool camFoundHit)
    {
        Ray camRay = PlayerCam.ViewportPointToRay(Vector3.one * 0.5f);

        LayerMask ignorePlayerMask = ~(1 << 8);
        float rangeToUse = maxRange + 3.0f;

        Vector3 hitPoint;

        if (Physics.Raycast(camRay, out camHit, rangeToUse, ignorePlayerMask))
        {
            camFoundHit = true;
            hitPoint = camHit.point;
        }
        else
        {
            camFoundHit = false;
            hitPoint = PlayerCam.transform.position + PlayerCam.transform.forward * rangeToUse + new Vector3(0, .4f, 0);
        }
        return hitPoint;
    }






}


