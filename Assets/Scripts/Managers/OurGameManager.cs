using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurGameManager : MonoBehaviour
{
    public static OurGameManager Instance { get; private set; }

    public static GameObject LocalPlayer { get; private set; }

    static uint projectileCount = 0;
    static SpawnSpots[] spawnSpots;

    



    void Start()
    {
        EnsureSingleton();
        spawnSpots = FindObjectsOfType<SpawnSpots>(); 
        SpawnPlayer();
    }

    void EnsureSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void SpawnPlayer()
    {
        SpawnSpots spawnSpot = spawnSpots[Random.Range(0, spawnSpots.Length - 1)];
        if (spawnSpot == null)
        {
            Debug.Log("No Spawn spots");
            return;
        }

        LocalPlayer = PhotonNetwork.Instantiate("OurPlayer", spawnSpot.transform.position, spawnSpot.transform.rotation, 0);
        PhotonNetwork.player.TagObject = LocalPlayer;
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