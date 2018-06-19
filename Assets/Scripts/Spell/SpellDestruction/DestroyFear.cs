using System.Collections.Generic;
using UnityEngine;


/*Checks for enemies within a radius and in line of sight of fear explosion. Fears enemies
 who qualify (In SpellEffects)*/
public class DestroyFear {

    float RADIUS = 4.0f;
    
    GameObject particles;
    SpellEffects spellEffects;

    public DestroyFear(GameObject particlesIn)
    {
        particles = particlesIn;
        spellEffects = GameObject.Find("Spell").GetComponent<SpellEffects>();
    }



    /*Cast a sphere for all players in range, draw a ray cast to them to see if
    they are in line of sight, if so fear*/
    public void explodeFear()
    {
        if (particles == null)
            return;

        Vector3 origin = particles.gameObject.transform.position;

        List<GameObject> playersInRadiusAndLOS = PlayersWithinRadius.FindPlayersWithinRadiusAndLOS(origin, RADIUS);
        foreach (GameObject hitPlayer in playersInRadiusAndLOS)
        {
            spellEffects.CmdFearPlayer(hitPlayer);
        }

       GameObject.Destroy(particles, 2.0f);
    }

}
