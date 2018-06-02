using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OurNetworkManager : NetworkManager {

    CameraManager cameraManager;
    SpellManager spellManager;

    void Start()
    {
        cameraManager = GameObject.Find("Managers/CameraManager").GetComponent<CameraManager>();
        if (cameraManager == null)
            Debug.Log("CamManager is null");
        spellManager = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>();
        if (spellManager == null)
            Debug.Log("SpellManager is null");
        if (spellManager) RegisterSpellPrefabs();
    }

    /*Prefabs that are spawned during runtime over the network need to be registed with the NetworkManager before use*/
    void RegisterSpellPrefabs()
    {
        for (int i = 0; i < spellManager.spellList.Count; i++)
        {
            if (spellManager.spellList[i] != null)
            {
                if (spellManager.spellList[i].prefab != null)
                    ClientScene.RegisterPrefab(spellManager.spellList[i].prefab);
                if (spellManager.spellList[i].collisionParticle != null)
                    ClientScene.RegisterPrefab(spellManager.spellList[i].collisionParticle);
            }
            
        }
    }

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        cameraManager.SetSceneCamActive(false);
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        cameraManager.SetSceneCamActive(false);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        cameraManager.SetSceneCamActive(true);
    }

    public override void OnStopHost()
    {
        base.OnStartHost();
        cameraManager.SetSceneCamActive(true);
    }


}
