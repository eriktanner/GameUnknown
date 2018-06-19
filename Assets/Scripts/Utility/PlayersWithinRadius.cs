using System.Collections.Generic;
using UnityEngine;


/*Finds players within a certain radius and in line of sight*/
public class PlayersWithinRadius {


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
        bool isInLineOfSight = true;
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.tag != "Player")
                isInLineOfSight = false;
        }

        return isInLineOfSight;
    }

    private static RaycastHit[] RaycastAllToPlayer(Vector3 origin, GameObject gameObjectHit)
    {
        Vector3 collisionPosition = gameObjectHit.transform.position + new Vector3(0, .3f, 0);
        Vector3 direction = collisionPosition - origin;
        float distance = (collisionPosition - direction).magnitude + .2f;
        return Physics.RaycastAll(origin, direction, distance); ;
    }




}
