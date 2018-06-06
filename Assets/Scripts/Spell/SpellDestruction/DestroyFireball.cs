using UnityEngine;


/*Destroys a fireball. Fireballs dissapears but smoke remains TODO*/
public class DestroyFireball : MonoBehaviour {

    GameObject player;
    GameObject spellObject;
    SpellDestruction spellDestruction;

    public DestroyFireball(GameObject playerObject, GameObject spellObjectIn)
    {
        player = playerObject;
        spellObject = spellObjectIn;
        spellDestruction = player.GetComponent<SpellDestruction>();
    }


    /*Allows smoke of fireball to remain, even after fireball hits TODO*/
    public void destroyFireball()
    {
        if (spellObject == null)
            return;

        Spell spell = SpellManager.getSpellFromName(spellObject.name);
        spellDestruction.defaultDestroy(spellObject);
        
    }
}
