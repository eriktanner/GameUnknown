using UnityEngine;


/*Destroys a fireball. Fireballs dissapears but smoke remains TODO*/
public class DestroyFireball {

    GameObject player;
    GameObject spellObject;
    SpellDestruction spellDestruction;

    public DestroyFireball(GameObject playerObject, GameObject spellObjectIn)
    {
        player = playerObject;
        spellObject = spellObjectIn;
    }


    /*Allows smoke of fireball to remain, even after fireball hits TODO*/
    public void destroyFireball()
    {
        if (spellObject == null)
            return;
        
        GameObject.Destroy(spellObject);
    }

    /*Destoys collision particles after set timer*/
    public void explodeFireball()
    {
        if (spellObject == null)
            return;

        GameObject.Destroy(spellObject, 1.5f);
    }
}
