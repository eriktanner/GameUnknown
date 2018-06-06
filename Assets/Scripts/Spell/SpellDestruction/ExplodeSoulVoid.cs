using UnityEngine;


/*Checks for enemies within a radius and in line of sight of soul void explosion. Stuns
 * damages enemies who qualify (In SpellEffects)*/
public class ExplodeSoulVoid : MonoBehaviour {

    float RADIUS = 3.5f;

    GameObject player;
    GameObject particles;
    SpellDestruction spellDestruction;
    SpellEffects spellEffects;

    public ExplodeSoulVoid(GameObject playerObject, GameObject particlesIn)
    {
        player = playerObject;
        particles = particlesIn;
        spellDestruction = GameObject.Find("Managers/SpellDestruction").GetComponent<SpellDestruction>();
        spellEffects = GameObject.Find("Managers/SpellDestruction").GetComponent<SpellEffects>();
    }



    /*Cast a sphere for all players in range, draw a ray cast to them to see if
    they are in line of sight, if so damage and stun*/
    public void explodeSoulVoid()
    {
        Vector3 origin = particles.gameObject.transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(origin, RADIUS);


        for (int i = 0; i < hitColliders.Length; i++)
        {

            if (hitColliders[i].gameObject.tag == "Player")
            {
                Vector3 collisionPosition = hitColliders[i].gameObject.transform.position + new Vector3(0, .2f, 0); //Vector offset only beneficial for ground casts (sometimes spell does not work)
                Vector3 direction = collisionPosition - origin;
                float distance = (collisionPosition - direction).magnitude;

                RaycastHit[] hits = Physics.RaycastAll(player.transform.position, player.transform.forward, distance);


                bool isInLineOfSight = true;
                foreach (RaycastHit hit in hits) //Other players will not effect Line of Sight
                {
                    if (hit.transform.gameObject.tag != "Player")
                        isInLineOfSight = false;
                }

                if (isInLineOfSight)
                    spellEffects.CmdSoulVoidPlayer(hitColliders[i].gameObject);
            }

        }

        spellDestruction.defaultDestroy(particles, 2.0f);
    }

}
