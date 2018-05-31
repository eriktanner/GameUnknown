using UnityEngine.Networking;
using UnityEngine;

public class NetworkPlayerSetup : NetworkBehaviour {

    
    public Behaviour[] componentsToDisable;
    CameraManager cameraManager;

    Camera sceneCamera;

    void Start()
    {
        cameraManager = GameObject.Find("Managers/CameraManager").GetComponent<CameraManager>();
        

        if (!isLocalPlayer)
        {
            DisableRemoteComponents();
            AssignRemoteLayersAndTags();
        } else
        {
            sceneCamera = GameObject.Find("Cameras/SkyCam").GetComponent<Camera>();
            if (sceneCamera)
            {
                sceneCamera.enabled = false;
                cameraManager.SetCursorToLockAndInvisible();
            }
        }

        RegisterPlayer();
    }

    /*Assigns all player/enemy ids, later register to some kind of dictionary*/
    void RegisterPlayer()
    {
        string id = "Player " + GetComponent<NetworkIdentity>().netId;
        gameObject.name = id;

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
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        gameObject.tag = "Enemy";
    }

    void OnDisable()
    {
        if (sceneCamera)
        {
            sceneCamera.enabled = true;
            cameraManager.SetCursorToFreeAndVisible();
        }
    }
}
