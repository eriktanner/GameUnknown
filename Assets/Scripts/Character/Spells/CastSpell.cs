using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpell : MonoBehaviour
{
    public List<Spell> spellList = new List<Spell>(3);

    Vector3 firePosition;
    CastBar castBar;
    ManaBar manaBar;
    SpellManager spellManager;
    Camera cam;
    Coroutine castRoutine;

    bool spellLock = false;
    bool cancelCast = false;


    void Start()
    {
        cam = Camera.main;
        castBar = GameObject.Find("Canvas/CastBar").GetComponent<CastBar>();
        manaBar = GameObject.Find("Canvas/ManaBar").GetComponent<ManaBar>();
        spellManager = GameObject.Find("SpellManager").GetComponent<SpellManager>();

        addToSpellList("Fireball", 0);
        addToSpellList("Pain", 1);
        addToSpellList("Arcane Missile", 2);
    }

    void Update()
    {
        firePosition = transform.Find("Character1_Reference/FirePosition").position; //may want to reference from editor for effeciency
        GetInput();
        CancelCast();
    }

    void GetInput()
    {
        if (Input.GetButtonDown("Spell1"))
        {
            FireSpell(spellList[0]);
        }
        if (Input.GetButtonDown("Spell2"))
        {
            FireSpell(spellList[1]);
        }
        if (Input.GetButtonDown("Spell3"))
        {
            FireSpell(spellList[2]);
        }
        cancelCast = Input.GetButtonDown("CancelCast");
    }

    bool shootSpell = false;
    void FireSpell(Spell spell)
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

    /*This waits the cast time and then fires the spell, we may want to get rid of the pin point accuracy*/
    private IEnumerator CastAndFire(Spell spell)
    {
        spellLock = true;
        castBar.CastSpellUI(spell);
        yield return new WaitForSeconds(spell.castTime);
        manaBar.burnMana(spell.manaCost);
        manaBar.regenerateMana();


        Ray camRay = cam.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit camHit;
        bool camFoundHit = false;
        Vector3 firePositionToAim;

        LayerMask ignorePlayerMask = ~(1 << 8);

        if (Physics.Raycast(camRay, out camHit, ignorePlayerMask))
        {
            camFoundHit = true;
        }

        Vector3 offset = Vector3.zero;
        if (spell.maxRange <= 30)
            offset = new Vector3(-.04f, .06f, 0);
        else offset = new Vector3(-.03f, .05f, 0);

        firePositionToAim = camFoundHit ? camHit.point - firePosition : camRay.direction + offset;

        Quaternion rotationToTarget = Quaternion.LookRotation(firePositionToAim);
        GameObject spellObject = spellManager.createSpellInWorld(spell, firePosition, rotationToTarget);
        spellManager.DestroySpell(spellObject, spell.maxRange / spell.projectileSpeed);
        spellLock = false;
    }

    void CancelCast()
    {
        if (cancelCast && castRoutine != null)
        {
            spellLock = false;
            StopCoroutine(castRoutine);
        }

    }





    /*SpellList Functionality*/

    /*Given a spell and index, adds to player's spellList*/
    void addToSpellList(string spellName, int index)
    {
        if (index < 0 || index > 3)
            return;

        bool spellAlreadyExists = false;
        for (int i = 0; i < spellList.Count; i++)
        {
            if (spellList[i] != null && spellName.Equals(spellList[i].name))
            {
                spellAlreadyExists = true;
            }
        }

        if (!spellAlreadyExists)
        {
            spellList.Insert(index, spellManager.getSpellFromName(spellName));
        }
    }

    /*Given a spell, removes from player's spellList*/
    void removeFromSpellList(string spellName)
    {
        for (int i = 0; i < spellList.Count; i++)
        {
            if (spellList[i] != null && spellName.Equals(spellList[i].name))
            {
                spellList.RemoveAt(i);
            }
        }
    }

    /*Removes spell from spellList's index*/
    void removeFromSpellList(int index)
    {
        if (index < 0 || index > 3 || index > spellList.Count - 1)
            return;
        spellList.RemoveAt(index);
    }

}


