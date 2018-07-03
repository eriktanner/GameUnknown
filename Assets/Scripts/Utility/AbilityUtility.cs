using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;


/*Finds players within a certain radius and in line of sight*/
public static class AbilityUtility {


    /*Finds players within a certain radius and in line of sight*/
    public static List<GameObject> FindPlayersWithinRadiusAndLOS(Vector3 origin, float radius)
    {
        List<GameObject> playersInRadiusAndLOS = new List<GameObject>();


        Collider[] hitColliders = Physics.OverlapSphere(origin, radius);


        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject gameObjectHit = hitColliders[i].gameObject;

            if (gameObjectHit.tag == "Player")
            {
                
                RaycastHit[] hits = RaycastAllToPlayer(origin, gameObjectHit);

                bool isInLineOfSight = PlayerIsInLineOfSight(hits);

                if (isInLineOfSight)
                    playersInRadiusAndLOS.Add(gameObjectHit);
            }

        }

        return playersInRadiusAndLOS;
    }


    private static bool PlayerIsInLineOfSight(RaycastHit[] hits)
    {
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.tag != "Player")
                return false;
        }

        return true;
    }

    private static RaycastHit[] RaycastAllToPlayer(Vector3 origin, GameObject gameObjectHit)
    {
        Vector3 collisionPosition = gameObjectHit.transform.position + new Vector3(0, .3f, 0);
        Vector3 direction = collisionPosition - origin;
        float distance = (collisionPosition - direction).magnitude + .2f;
        return Physics.RaycastAll(origin, direction, distance); ;
    }


    public static float GetLifespanOfSpell(AbilityData spell)
    {
        if (spell as IProjectile == null)
            return 0;

        float lifespan = 0;

        if (spell as ICheckForDistance != null) //Arbitrarily large
            lifespan = 10;
        else
        {
            float timeTakesToTravelMaxDistance = spell.MaxRange / ((IProjectile)spell).ProjectileSpeed;
            lifespan = timeTakesToTravelMaxDistance;
        }

        return lifespan;
    }

    public static IEnumerator WaitAndCall(float waitTime, Action postWaitMethod)
    {
        yield return new WaitForSeconds(waitTime);
        postWaitMethod();
    }

}
