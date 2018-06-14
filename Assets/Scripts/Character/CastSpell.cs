using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*This class handles user input related to casting spells*/
[RequireComponent((typeof(ManaBar)))]
public class CastSpell : Photon.MonoBehaviour
{
    SpellList spellList;
    SpellDestruction spellDestruction;
    public Transform castSpawn;
    public ManaBar manaBar;

    CastBar castBar;
    OurGameManager GameManager;
    Camera cam;
    Coroutine castRoutine;

    bool spellLock = false;
    bool cancelCast = false;
    


    void Start()
    {

        //cam = Camera.main; **We will want to cache this upon entering game
        if (GameObject.Find("Canvas/CastBar").GetComponent<CastBar>() != null)
            castBar = GameObject.Find("Canvas/CastBar").GetComponent<CastBar>();
        

        GameManager = GameObject.Find("Managers/GameManager").GetComponent<OurGameManager>();

        spellDestruction = GameObject.Find("Spell").GetComponent<SpellDestruction>();

        spellList = GetComponent<SpellList>();
    }

    void Update()
    {
        GetInput();
        CancelCast();
        cam = Camera.main; //To be cached, needed for networking building
    }

    void GetInput()
    {
        if (Input.GetButtonDown("Spell1"))
        {
            if (!spellList.isOnCooldown(0))
                FireSpell(spellList.GetSpellAtIndex(0));
        }
        if (Input.GetButtonDown("Spell2"))
        {
            if (!spellList.isOnCooldown(1))
                FireSpell(spellList.GetSpellAtIndex(1));
        }
        if (Input.GetButtonDown("Spell3"))
        {
            if (!spellList.isOnCooldown(2))
                FireSpell(spellList.GetSpellAtIndex(2));
        }
        if (Input.GetButtonDown("Spell4"))
        {
            if (!spellList.isOnCooldown(3))
                FireSpell(spellList.GetSpellAtIndex(3));
        }
        if (Input.GetButtonDown("Spell5"))
        {
            if (!spellList.isOnCooldown(4))
                FireSpell(spellList.GetSpellAtIndex(4));
        }
        if (Input.GetButtonDown("Spell6"))
        {
            if (!spellList.isOnCooldown(5))
                FireSpell(spellList.GetSpellAtIndex(5));
        }
        cancelCast = Input.GetButtonDown("CancelCast");
    }
    

    void FireSpell(SpellStats spell)
    {
        if (!spell || spellLock)
            return;
        
        if (spell.prefab == null)
        {
            Debug.LogWarning("Spell  Prefab is null");
            return;
        }

        castRoutine = StartCoroutine(CastAndFire(spell));
    }

    void CancelCast()
    {
        if (cancelCast && castRoutine != null)
        {
            spellLock = false;
            StopCoroutine(castRoutine);
        }
    }

    public void SetSpellLock(bool isLocked)
    {
        spellLock = isLocked;
    }

    

    private IEnumerator CastAndFire(SpellStats spell)
    {
        spellLock = true;
        castBar.CastSpellUI(spell);
        yield return new WaitForSeconds(spell.castTime);
        


        Ray camRay = cam.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit camHit;
        bool camFoundHit;

        LayerMask ignorePlayerMask = ~(1 << 8); 

        Vector3 hitPoint;
        float rangeToUse = spell.maxRange + 4;

        if (Physics.Raycast(camRay, out camHit, rangeToUse, ignorePlayerMask))
        {
            camFoundHit = true;
            hitPoint = camHit.point;
        } else
        {
            camFoundHit = false;
            hitPoint = cam.transform.position + cam.transform.forward * rangeToUse + new Vector3(0, .4f, 0);
        }


        bool didHitValidLayer = ValidSpellLayer.SpellHitValidLayerBySpell(spell.name, camHit);
        bool isWithinRangeOfSpell = ValidSpellDistance.SpellIsInRange(spell.name, transform.position, camHit.point, camFoundHit);
        
        

        if (isWithinRangeOfSpell && didHitValidLayer)
        {
            Vector3 aimToFromFirePosition  = hitPoint - castSpawn.position;
            Quaternion rotationToTarget = Quaternion.LookRotation(aimToFromFirePosition);

            manaBar.burnMana(spell.manaCost);
            spellList.TriggerCooldown(spell);
            NetworkFireSpell(spell.name, rotationToTarget, PhotonNetwork.player.ID);
        }

        spellLock = false;
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
        spellDestruction.DestroySpellByTime(spellObject);
    }

    




 

    

    

    

}


