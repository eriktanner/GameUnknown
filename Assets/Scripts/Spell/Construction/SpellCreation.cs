using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This class handles the in-world instation of spells*/
public class SpellCreation : MonoBehaviour {


    SpellManager spellManager;
    SpellDestruction spellDestruction;

    void Start()
    {
        spellDestruction = GameObject.Find("Spell").GetComponent<SpellDestruction>();
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
    }



    /*Creates the spell in world, gives it movement, and destruction timer*/
    public GameObject CreateSpellInWorld(SpellStats spell, Vector3 position, Quaternion rotation, string shotByName, int shotBy)
    {
        GameObject spellObject = Instantiate(spell.prefab, position, rotation);


        //Components
        spellObject.AddComponent<SpellMovement>();
        System.Type SpellType = SpellDictionary.GetTypeFromSpellName(SpellManager.getOriginalSpellName(spell.name));
        spellObject.AddComponent(SpellType);

        //Identify
        SpellIdentifier.AddSpellIdentifier(spellObject, spell.name, shotByName, shotBy);
        spellObject.name = spell.name;
        spellObject.tag = "Spell";
        spellObject.layer = 10;
        spellObject.transform.parent = spellManager.SpellManagerTransform;


        //Init Lifespan Timer
        spellDestruction.DestroySpellByTime(spellObject);
        return spellObject;
    }





}
