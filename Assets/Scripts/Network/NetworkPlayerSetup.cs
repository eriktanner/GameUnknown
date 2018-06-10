using UnityEngine.Networking;
using UnityEngine;

public class NetworkPlayerSetup : Photon.MonoBehaviour {

    
    public Behaviour[] componentsToDisable;
    CameraManager cameraManager;
    

    void Start()
    {
        cameraManager = GameObject.Find("Managers/CameraManager").GetComponent<CameraManager>();
        

        if (!photonView.isMine)
        {
            DisableRemoteComponents();
        } else
        {
            cameraManager.SetSceneCamActive(false);
        }


        AssignRemoteLayersAndTags();
        RegisterPlayer();
    }

    /*Assigns all player/enemy ids, later register to some kind of dictionary*/
    void RegisterPlayer()
    {
        string id = "Player " + GetComponent<PhotonView>().ownerId;
        gameObject.name = id;
        gameObject.transform.parent = GameObject.Find("Managers/GameManager").transform;

    }

    /*Disables all remote player components*/
    void DisableRemoteComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    void AssignRemoteLayersAndTags()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.tag = "Player";
    }
    
}
