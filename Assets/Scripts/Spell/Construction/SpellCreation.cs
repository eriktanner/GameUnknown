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
    public GameObject CreateSpellInWorld(Spell spell, Vector3 position, Quaternion rotation)
    {
        GameObject spellObject = Instantiate(spell.prefab, position, rotation);
        spellObject.AddComponent<SpellMovement>();

        spellObject.name = spell.name;
        spellObject.tag = "Spell";
        spellObject.layer = 10;

        
        if (ValidSpellDistance.hasValidDistanceCheckBeforeCast(spell.name)) //Distance verified BEFORE cast, we can give large destroy time
            spellDestruction.destroySpell(spellObject, 10);
        else
            spellDestruction.destroySpell(spellObject, spell.maxRange / spell.projectileSpeed); //Regular timed destroy, d = r * t (However often innaccurate)

        spellObject.transform.parent = spellManager.SpellManagerTransform;
        return spellObject;
    }



}
