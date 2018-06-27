using UnityEngine;

public class NetworkPlayerSetup : Photon.MonoBehaviour {

    
    public Behaviour[] componentsToDisable;

    OurGameManager GameManager;
    


    void Start()
    {
        GameManager = OurGameManager.Instance;


        if (!photonView.isMine)
        {
            DisableRemoteComponents();
        } else
        {
            //SceneCamera.SetSceneCamActive(false);
        }

        AssignRemoteLayersAndTags();
        RegisterPlayer();
    }

    /*Assigns all player/enemy ids, later register to some kind of dictionary*/
    void RegisterPlayer()
    {
        string id = "Player " + GetComponent<PhotonView>().ownerId;
        gameObject.name = id;
        gameObject.transform.parent = GameObject.Find("Managers/PlayerManager").transform;
        PlayerManager.TrackListOfPlayers();
    }

    
    


    /*Disables all remote player components*/
    void DisableRemoteComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            if (componentsToDisable[i] != null)
            componentsToDisable[i].enabled = false;
        }
    }

    void AssignRemoteLayersAndTags()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.tag = "Player";
    }
    
}
