using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Ice wall creates an ice barrier at the position it is casted, created horizontal from the 
 caster*/
public class DestroyIceWall {

    float ICE_WALL_LIFETIME = 4.0f;

    GameObject player;
    GameObject particles;
    SpellManager spellManager;

    public DestroyIceWall(GameObject particlesIn)
    {
        player = OurGameManager.GetPlayerGameObject(particlesIn.GetComponent<SpellIdentifier>().ShotByName);
        particles = particlesIn;

        if (GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>() != null)
            spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
    }



    /*Cast a sphere for all players in range, draw a ray cast to them to see if
    they are in line of sight, if so damage and stun*/
    public void explodeIceWall()
    {
        GameObject iceWallPrefab = spellManager.getSpawnablePrefab("Ice Wall");

        Vector3 wallPosition = particles.gameObject.transform.position;
        Vector3 spaceBetween = wallPosition - player.transform.position;

        GameObject iceWall = null;
        if (iceWallPrefab != null)
            iceWall = GameObject.Instantiate(iceWallPrefab, wallPosition + new Vector3(0, .5f, 0), player.transform.rotation);

        if (iceWall != null)
            GameObject.Destroy(iceWall, ICE_WALL_LIFETIME);
    }

}
