using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent((typeof(SpellDestruction)))]
[RequireComponent((typeof(ManaBar)))]
public class CastSpell : NetworkBehaviour
{
    public Spell[] spellList = new Spell[3];
    public Transform castSpawn;
    public SpellDestruction spellDestruction;
    public ManaBar manaBar;

    CastBar castBar;
    SpellManager spellManager;
    Camera cam;
    Coroutine castRoutine;

    bool spellLock = false;
    bool cancelCast = false;
    


    void Start()
    {
        //cam = Camera.main; **We will want to cache this upon entering game
        if (GameObject.Find("Canvas/CastBar").GetComponent<CastBar>() != null)
            castBar = GameObject.Find("Canvas/CastBar").GetComponent<CastBar>();
        if (GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>() != null)
            spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        

        addToSpellList("Fireball", 0);
        addToSpellList("Fear", 1);
        addToSpellList("Arcane Missile", 2);
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
            if (spellList[0])
                FireSpell(spellList[0]);
        }
        if (Input.GetButtonDown("Spell2"))
        {
            if (spellList[1])
                FireSpell(spellList[1]);
        }
        if (Input.GetButtonDown("Spell3"))
        {
            if (spellList[2])
                FireSpell(spellList[2]);
        }
        cancelCast = Input.GetButtonDown("CancelCast");
    }
    

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

    void CancelCast()
    {
        if (cancelCast && castRoutine != null)
        {
            spellLock = false;
            StopCoroutine(castRoutine);
        }

    }

    /*This waits the cast time and then fires the spell, we may want to get rid of the pin point accuracy*/
    private IEnumerator CastAndFire(Spell spell)
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

            CmdCallRpcFireSpell(spell.name, rotationToTarget);
            manaBar.burnMana(spell.manaCost);
            manaBar.regenerateMana();
        }

        spellLock = false;
    }

    [Command] /*Calls Rpc Function. Look into Spell.cs to see how we're handling spells over the network*/
    void CmdCallRpcFireSpell(string spellName, Quaternion rotationToTarget)
    {
        RpcFireSpell(spellName, rotationToTarget);
    }

    [ClientRpc] /*Tells server to create a spell object on each client. */
    void RpcFireSpell(string spellName, Quaternion rotationToTarget)
    {
        Spell spell = SpellManager.getSpellFromName(spellName);
        GameObject spellObject = createSpellInWorld(spell, castSpawn.position, rotationToTarget);
        spellObject.name = OurGameManager.AddProjectileNumberToSpell(spell.name);
        OurGameManager.IncrementProjectileCount();

        if (ValidSpellDistance.hasValidDistanceCheckBeforeCast(spell.name)) //Distance verified BEFORE cast, we can give large destroy time
            spellDestruction.destroySpell(spellObject, 10);
        else
            spellDestruction.destroySpell(spellObject, spell.maxRange / spell.projectileSpeed); //Regular timed destroy, d = r * t (However often innaccurate)
                                                                                                
    }
    






    /*General Functionality*/

    public void setSpellLock(bool isLocked)
    {
        spellLock = isLocked;
    }

    /*Creates the spell in the world and fires it.
     Adds RigidBody, SphereCollider, and SpellCollision to spell*/
    public GameObject createSpellInWorld(Spell spell, Vector3 position, Quaternion rotation)
    {
        GameObject spellObject = Instantiate(spell.prefab, position, rotation);
        SpellCollision.AddSpellCollision(spellObject,spell.projectileRadius, gameObject.name, spellDestruction);
        spellObject.AddComponent<SpellMovement>();

        spellObject.name = spell.name;
        spellObject.tag = "Spell";
        spellObject.layer = 10;
        
        spellObject.transform.parent = spellManager.SpellManagerTransform;
        return spellObject;
    }

    /*Given a spell and index, adds to player's spellList*/
    void addToSpellList(string spellName, int index)
    {
        if (index < 0 || index > 3)
            return;

        bool spellAlreadyExists = false;
        for (int i = 0; i < spellList.Length; i++)
        {
            if (spellList[i] != null && spellName.Equals(spellList[i].spellName))
            {
                spellAlreadyExists = true;
            }
        }

        if (!spellAlreadyExists)
        {
            spellList[index] = SpellManager.getSpellFromName(spellName);
        }
    }

    /*Given a spell, removes from player's spellList*/
    void removeFromSpellList(string spellName)
    {
        for (int i = 0; i < spellList.Length; i++)
        {
            if (spellList[i] != null && spellName.Equals(spellList[i].spellName))
            {
                spellList[i] = null;
            }
        }
    }

    /*Removes spell from spellList's index*/
    void removeFromSpellList(int index)
    {
        if (index < 0 || index > 3 || index > spellList.Length - 1)
            return;
        spellList[index] = null;
    }

}


