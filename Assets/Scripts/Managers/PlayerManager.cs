using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance { get; private set; }

    public static GameObject LocalPlayer { get; private set; }

    public static Dictionary<GameObject, PhotonPlayer> playerToPhotonPlayer = new Dictionary<GameObject, PhotonPlayer>();


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


        LocalPlayer = PhotonNetwork.Instantiate("NewPlayer 4", spawnSpot.transform.position, spawnSpot.transform.rotation, 0);
        PhotonNetwork.player.TagObject = LocalPlayer;
    }


    //TODO make this more efficient - only track newly added or left players
    /*Keeps a u to date list of whoever is currently in the game*/
    public static void TrackListOfPlayers()
    {
        var photonViews = FindObjectsOfType<PhotonView>();
        foreach (var view in photonViews)
        {
            var player = view.owner;
            //Objects in the scene don't have an owner, its means view.owner will be null
            if (player != null)
            {
                if (!playerToPhotonPlayer.ContainsKey(view.gameObject))
                    playerToPhotonPlayer.Add(view.gameObject, player);
            }
        }
    }


    public static GameObject GetPlayerGameObject(string id)
    {
        return GameObject.Find("Managers/PlayerManager/" + id);
    }


    public static PhotonPlayer GetPhotonPlayerFromGameObject(GameObject hitPlayer)
    {
        if (playerToPhotonPlayer.ContainsKey(hitPlayer))
            return playerToPhotonPlayer[hitPlayer];
        return null;
    }
}
