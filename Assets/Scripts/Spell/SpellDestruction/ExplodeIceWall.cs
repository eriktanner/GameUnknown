using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Ice wall creates an ice barrier at the position it is casted, created horizontal from the 
 caster*/
public class ExplodeIceWall : MonoBehaviour {

    float ICE_WALL_LIFETIME = 4.0f;

    GameObject player;
    GameObject particles;
    SpellDestruction spellDestruction;
    SpellManager spellManager;

    public ExplodeIceWall(GameObject playerObject, GameObject particlesIn)
    {
        player = playerObject;
        particles = particlesIn;
        spellDestruction = GameObject.Find("Spell").GetComponent<SpellDestruction>();

        if (GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>() != null)
            spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
    }



    /*Cast a sphere for all players in range, draw a ray cast to them to see if
    they are in line of sight, if so damage and stun*/
    public void explodeIceWall()
    {
        Vector3 wallPosition = particles.gameObject.transform.position;

        GameObject iceWallPrefab = spellManager.getSpawnablePrefab("IceWall");


        Vector3 spaceBetween = wallPosition - player.transform.position;


        //- spaceBetween/4
        GameObject iceWall = Instantiate(iceWallPrefab, wallPosition + new Vector3(0, .5f, 0), player.transform.rotation);


        spellDestruction.defaultDestroy(particles, 2.0f);

        spellDestruction.defaultDestroy(iceWall, ICE_WALL_LIFETIME);
    }

}
