using System.Collections.Generic;
using UnityEngine;


/*Checks for enemies within a radius and in line of sight of soul void explosion. Stuns
 * damages enemies who qualify (In SpellEffects)*/
public class DestroySoulVoid {

    float RADIUS = 3.5f;

    int ShotBy;
    GameObject particles;
    SpellEffects spellEffects;

    public DestroySoulVoid(GameObject particlesIn)
    {
        SpellIdentifier spellIdentifier = particlesIn.GetComponent<SpellIdentifier>();
        if (spellIdentifier == null)
            return;
        
        particles = particlesIn;
        ShotBy = spellIdentifier.ShotByID;
        spellEffects = GameObject.Find("Spell").GetComponent<SpellEffects>();
    }



    /*Cast a sphere for all players in range, draw a ray cast to them to see if
    they are in line of sight, if so damage and stun*/
    public void explodeSoulVoid()
    {
        if (particles == null)
            return;

        Vector3 origin = particles.gameObject.transform.position;

        List<GameObject> playersInRadiusAndLOS = GameplayUtility.FindPlayersWithinRadiusAndLOS(origin, RADIUS);
        foreach(GameObject hitPlayer in playersInRadiusAndLOS)
        {
            spellEffects.CmdSoulVoidPlayer(hitPlayer, ShotBy);
        }
            
        GameObject.Destroy(particles, 2.0f);
    }

}
