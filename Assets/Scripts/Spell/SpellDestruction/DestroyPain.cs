using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Displays pain collision particle even if it does not make a collision*/
public class DestroyPain  {

    GameObject player;
    GameObject spellObject;
    SpellDestruction spellDestruction;

    public DestroyPain(GameObject playerObject, GameObject spellObjectIn)
    {
        player = playerObject;
        spellObject = spellObjectIn;
        spellDestruction = player.GetComponent<SpellDestruction>();
    }


    public void destroyPain()
    {
        if (spellObject == null)
            return;

        Spell spell = SpellManager.getSpellFromName(spellObject.name);
        spellDestruction.spawnAndDestroyParticles(spell.name, spellObject.transform.position, 1.5f); //Creates particles

        spellDestruction.defaultDestroy(spellObject); //Destroys spell prefab
    }
}
