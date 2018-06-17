using System.Collections;
using UnityEngine;

[RequireComponent((typeof(ManaBar)))]
[RequireComponent((typeof(SpellList)))]
public class CastSpell : Photon.MonoBehaviour
{  
    public ManaBar ManaBar;
    public SpellList SpellList;
    public Transform castSpawn;

    SpellDestruction SpellDestruction;
    Coroutine CastRoutine;
    CastBar CastBar;
    Camera PlayerCam;

    bool SpellLock = false;
    bool DoCancelCast = false;
    


    void Start()
    {
        PlayerCam = Camera.main;
        CastBar = CastBar.Instance;
        SpellDestruction = SpellDestruction.Instance;
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
                FireSpell(SpellList.GetSpellAtIndex(0));
        }
        if (Input.GetButtonDown("Spell2"))
        {
            if (!SpellList.isOnCooldown(1))
                FireSpell(SpellList.GetSpellAtIndex(1));
        }
        if (Input.GetButtonDown("Spell3"))
        {
            if (!SpellList.isOnCooldown(2))
                FireSpell(SpellList.GetSpellAtIndex(2));
        }
        if (Input.GetButtonDown("Spell4"))
        {
            if (!SpellList.isOnCooldown(3))
                FireSpell(SpellList.GetSpellAtIndex(3));
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
        DoCancelCast = Input.GetButtonDown("CancelCast");
    }
    

    void FireSpell(SpellStats spell)
    {
        if (!spell || SpellLock)
            return;
        
        if (spell.prefab == null)
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

    

    private IEnumerator CastAndFire(SpellStats spell)
    {
        SpellLock = true;
        CastBar.CastSpellUI(spell.castTime);
        yield return new WaitForSeconds(spell.castTime);

        RaycastHit camHit;
        bool camFoundHit;
        Vector3 hitPoint;
        hitPoint = GetHitmarkerPointInWorld(spell, out camHit, out camFoundHit);


        if (IsValidCast(spell, camHit, camFoundHit))
            Fire(spell, hitPoint);
        
        SpellLock = false;
    }

    bool IsValidCast(SpellStats spell, RaycastHit camHit, bool camFoundHit)
    {
        bool didHitValidLayer = ValidSpellLayer.SpellHitValidLayerBySpell(spell.name, camHit);
        bool isWithinRangeOfSpell = ValidSpellDistance.SpellIsInRange(spell.name, transform.position, camHit.point, camFoundHit);
        return didHitValidLayer && isWithinRangeOfSpell;
    }


    void Fire(SpellStats spell, Vector3 hitPoint)
    {
        Vector3 aimToFromFirePosition = hitPoint - castSpawn.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(aimToFromFirePosition);

        ManaBar.burnMana(spell.manaCost);
        SpellList.TriggerCooldown(spell);
        NetworkFireSpell(spell.name, rotationToTarget, PhotonNetwork.player.ID);
    }



    void NetworkFireSpell(string spellName, Quaternion rotationToTarget, int shotBy)
    {
        photonView.RPC("RpcFireSpell", PhotonTargets.All, spellName, rotationToTarget, shotBy);
    }

    [PunRPC]
    void RpcFireSpell(string spellName, Quaternion rotationToTarget, int shotBy)
    {
        SpellStats spell = SpellManager.GetSpellStatsFromName(spellName);
        
        GameObject spellObject = SpellCreation.CreateSpellInWorld(spell, castSpawn.position, rotationToTarget, gameObject.name, shotBy);
        SpellDestruction.DestroySpellByTime(spellObject);
    }







    public Vector3 GetHitmarkerPointInWorld(SpellStats spell, out RaycastHit camHit, out bool camFoundHit)
    {
        Ray camRay = PlayerCam.ViewportPointToRay(Vector3.one * 0.5f);

        LayerMask ignorePlayerMask = ~(1 << 8);
        float rangeToUse = spell.maxRange + 3.0f;

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


