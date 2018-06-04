using UnityEngine;


/*Soul void is a shadow spell that is casted on ground and explodes a few
 seconds after it is lands. It damages and stuns players within radius*/
public class ExplodeSoulVoid : MonoBehaviour {

    float RADIUS = 4.0f;

    GameObject player;
    GameObject particles;
    SpellDestruction spellDestruction;
    SpellEffects spellEffects;

    public ExplodeSoulVoid(GameObject playerObject, GameObject particlesIn)
    {
        player = playerObject;
        particles = particlesIn;
        spellDestruction = player.GetComponent<SpellDestruction>();
        spellEffects = player.GetComponent<SpellEffects>();
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
                Vector3 toPosition = hitColliders[i].gameObject.transform.position;
                Vector3 direction = toPosition - origin;
                float distance = (toPosition - direction).magnitude;

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
