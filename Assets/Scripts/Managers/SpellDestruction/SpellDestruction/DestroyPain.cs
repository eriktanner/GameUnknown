using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Pain is a short range, fast casted shadow spell. We want it to show its collision
 particles even if it doesnt hit a collision. This is because its spell prefab is
 the empty prefab and we want to show that it was in fact casted*/
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
