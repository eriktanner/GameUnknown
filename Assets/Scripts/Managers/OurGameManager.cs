using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurGameManager : MonoBehaviour
{

    static uint projectileCount = 0;

    

    /*This is going to be used to destroy projectiles on all clients
     All clients are local, but we can get references to each local if we name them all the same*/
    public static string AddProjectileNumberToSpell(string spellIn)
    {
        return spellIn + projectileCount;
    }

    public static void IncrementProjectileCount()
    {
        projectileCount++;
    }

    public static GameObject GetPlayerGameObject(string id)
    {
        return GameObject.Find("Managers/GameManager/" + id);
    }


}