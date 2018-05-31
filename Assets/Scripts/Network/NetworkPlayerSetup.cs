using UnityEngine.Networking;
using UnityEngine;

public class NetworkPlayerSetup : NetworkBehaviour {

    
    public Behaviour[] componentsToDisable;
    CameraManager cameraManager;

    Camera sceneCamera;

    void Start()
    {
        //CameraManager cameraManager = GameObject.Find("Managers/CameraManager").GetComponent<CameraManager>();
        //cameraManager.findPlayerCam();

        if (!isLocalPlayer)
        {
            DisableRemoteComponents();
            AssignRemoteLayersAndTags();
        } else
        {
            sceneCamera = GameObject.Find("Cameras/SkyCam").GetComponent<Camera>();
            if (sceneCamera)
                sceneCamera.enabled = false;
        }
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
            sceneCamera.enabled = true;
    }
}
