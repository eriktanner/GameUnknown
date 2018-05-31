using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    Camera skyCam;
    Camera playerCam;


    void Start()
    {
        skyCam = GameObject.Find("Cameras/SkyCam").GetComponent<Camera>();
        playerCam = GameObject.Find("Cameras/PlayerCam").GetComponent<Camera>();
        SetActiveMainCamera("PlayerCam");
        //SetActiveMainCamera("SkyCam");
    }

    void SetActiveMainCamera(string camToSetActive)
    {
        if (camToSetActive.Equals(skyCam.gameObject.name)) {
            if (skyCam == null)
                return;
            playerCam.enabled = false;
            skyCam.enabled = true;
        } else if (camToSetActive.Equals(playerCam.gameObject.name)) {
            if (playerCam == null)
                return;
            playerCam.enabled = true;
            skyCam.enabled = false;
        }
    }

}
