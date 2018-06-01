using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent((typeof(SpellDestruction)))]
public class CastSpell : NetworkBehaviour
{
    public List<Spell> spellList = new List<Spell>(3);
    public Transform castSpawn;
    public SpellDestruction spellDestruction;

    CastBar castBar;
    ManaBar manaBar;
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
        if (GameObject.Find("Canvas/ManaBar").GetComponent<ManaBar>() != null)
            manaBar = GameObject.Find("Canvas/ManaBar").GetComponent<ManaBar>();
        if (GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>() != null)
            spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();



        addToSpellList("Fireball", 0);
        addToSpellList("Pain", 1);
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

        firePositionToAim = camFoundHit ? camHit.point - castSpawn.position : camRay.direction + offset;

        Quaternion rotationToTarget = Quaternion.LookRotation(firePositionToAim);

        CmdFireSpell(spell.name, rotationToTarget);
        
        spellLock = false;
    }


    [Command]
    void CmdFireSpell(string spellName, Quaternion rotationToTarget)
    {
        Spell spell = spellManager.getSpellFromName(spellName);
        GameObject spellObject = createSpellInWorld(spell, castSpawn.position, rotationToTarget);
        spellDestruction.destroySpellOnServer(spellObject, spell.maxRange / spell.projectileSpeed);
        NetworkServer.Spawn(spellObject);
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

    /*Creates the spell in the world and fires it.
     Adds RigidBody, SphereCollider, and SpellCollision to spell*/
    public GameObject createSpellInWorld(Spell spell, Vector3 position, Quaternion rotation)
    {
        GameObject spellObject = Instantiate(spell.prefab, position, rotation);
        spellObject.name = spell.name;
        spellObject.tag = "Spell";
        spellObject.layer = 10;
        Rigidbody rigidBody = spellObject.AddComponent<Rigidbody>();
        rigidBody.mass = 0;
        rigidBody.useGravity = false;
        rigidBody.velocity = spellObject.transform.forward * spell.projectileSpeed;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        SphereCollider sphereCollider = spellObject.AddComponent<SphereCollider>();
        sphereCollider.radius = spell.projectileRadius;
        SpellCollision.AddSpellCollision(spellObject, gameObject.name, spellDestruction);

        spellObject.transform.parent = GameObject.Find("Managers/SpellManager").transform; //To remove we dont want a find in a network function
        return spellObject;
    }

    /*Given a spell and index, adds to player's spellList*/
    void addToSpellList(string spellName, int index)
    {
        if (index < 0 || index > 3)
            return;

        bool spellAlreadyExists = false;
        for (int i = 0; i < spellList.Count; i++)
        {
            if (spellList[i] != null && spellName.Equals(spellList[i].spellName))
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
            if (spellList[i] != null && spellName.Equals(spellList[i].spellName))
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


