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
        Vector3 origin = particles.gameObject.transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(origin, RADIUS);


        for (int i = 0; i < hitColliders.Length; i++)
        {

            if (hitColliders[i].gameObject.tag == "Player")
            {
                Vector3 collisionParticles = hitColliders[i].gameObject.transform.position + new Vector3(0, .3f, 0); //Vector offset only beneficial for ground casts (sometimes spell does not work);
                Vector3 direction = collisionParticles - origin;
                float distance = (collisionParticles - direction).magnitude;

                RaycastHit[] hits = Physics.RaycastAll(origin, direction, distance);


                bool isInLineOfSight = true;
                foreach (RaycastHit hit in hits) //Other players will not effect Line of Sight
                {
                    if (hit.transform.gameObject.tag != "Player")
                        isInLineOfSight = false;
                }

                if (isInLineOfSight)
                    spellEffects.CmdFearPlayer(hitColliders[i].gameObject);
            }

        }

        if (particles != null)
            GameObject.Destroy(particles, 2.0f);
    }

}
