using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurGameManager : MonoBehaviour
{

    static uint projectileCount = 0;

    SpawnSpots[] spawnSpots;
    static GameObject localPlayer;
    

    public static GameObject LocalPlayer
    {
        get { return localPlayer; }
    }

    

    void Start()
    {
        spawnSpots = FindObjectsOfType<SpawnSpots>();
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        SpawnSpots spawnSpot = spawnSpots[Random.Range(0, spawnSpots.Length - 1)];
        if (spawnSpot == null)
        {
            Debug.Log("No Spawn spots");
            return;
        }
        localPlayer = PhotonNetwork.Instantiate("OurPlayer", spawnSpot.transform.position, spawnSpot.transform.rotation, 0);
        PhotonNetwork.player.TagObject = localPlayer;

    }

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