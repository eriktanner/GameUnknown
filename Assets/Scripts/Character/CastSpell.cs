using System.Collections;
using UnityEngine;

[RequireComponent((typeof(ManaBar)))]
[RequireComponent((typeof(SpellList)))]
public class CastSpell : Photon.MonoBehaviour
{  
    public ManaBar ManaBar;
    public SpellList SpellList;
    public Transform castSpawn;
    

    NetworkAbilities networkAbilities;
    Coroutine CastRoutine;
    CastBar CastBar;
    Camera PlayerCam;

    bool SpellLock = false;
    bool DoCancelCast = false;
    


    void Start()
    {
        PlayerCam = Camera.main;
        CastBar = CastBar.Instance;
        networkAbilities = NetworkAbilities.Instance;
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
                FireSpell(SpellList.GetSpellAtIndex(0));
            }
        }
        if (Input.GetButtonDown("Spell2"))
        {
            if (!SpellList.isOnCooldown(1))
                FireSpell(SpellList.GetSpellAtIndex(1));
        }
        if (Input.GetButtonDown("Spell3"))
        {
            if (!SpellList.isOnCooldown(2))
            {
                FireSpell(SpellList.GetSpellAtIndex(2));
            }
        }
        if (Input.GetButtonDown("Spell4"))
        {
            if (!SpellList.isOnCooldown(3))
            {
                FireSpell(SpellList.GetSpellAtIndex(3));
            }
        }
        if (Input.GetButtonDown("Spell5"))
        {
            if (!SpellList.isOnCooldown(4))
                FireSpell(SpellList.GetSpellAtIndex(4));
        }
        if (Input.GetButtonDown("Spell6"))
        {
            if (!SpellList.isOnCooldown(5))
                FireSpell(SpellList.GetSpellAtIndex(5));
        }
        DoCancelCast = Input.GetKey(GameKeybindings.CancelCastInput);
    }
    

    void FireSpell(Spell spell)
    {
        if (!spell.SpellStats || SpellLock)
            return;
        
        if (spell.SpellStats.prefab == null)
        {
            Debug.LogWarning("Spell  Prefab is null");
            return;
        }

        CastRoutine = StartCoroutine(CastAndFire(spell));
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

    

    private IEnumerator CastAndFire(Spell spell)
    {
        SpellLock = true;
        if (spell.SpellStats.castTime > 0)
            CastBar.CastSpellUI(spell.SpellStats.castTime);
        yield return new WaitForSeconds(spell.SpellStats.castTime);

        RaycastHit camHit;
        bool camFoundHit;
        Vector3 hitPoint;
        hitPoint = GetHitmarkerPointInWorld(spell.SpellStats.maxRange, out camHit, out camFoundHit);


        if (IsValidCast(spell, camHit, camFoundHit))
        {
            ManaBar.burnMana(spell.SpellStats.manaCost);
            SpellList.TriggerCooldown(spell);
            networkAbilities.Fire(spell, castSpawn.position, hitPoint);
        }
        
        SpellLock = false;
    }

    bool IsValidCast(Spell spell, RaycastHit camHit, bool camFoundHit)
    {
        bool didHitValidLayer = ValidSpellLayer.SpellHitValidLayerBySpell(spell.SpellStats.name, camHit);
        bool isWithinRangeOfSpell = ValidSpellDistance.SpellIsInRange(spell.SpellStats.name, transform.position, camHit.point, camFoundHit);
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


