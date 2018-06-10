using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonNetworkManager : Photon.PunBehaviour
{


    
    /*
    const string VERSION = "v1.0.0";
    public string roomName = "TestRoom";
    public string playerPrefabName = "OurPlayer";
    public Transform spawnPoint;

    CameraManager cameraManager;

    void Start () {
        cameraManager = GameObject.Find("Managers/CameraManager").GetComponent<CameraManager>();
        PhotonNetwork.ConnectUsingSettings(VERSION);
	}

    void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    
	void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation, 0);
        cameraManager.SetSceneCamActive(false);
    }*/

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    




}
