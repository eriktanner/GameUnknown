using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    Camera skyCam;
    Camera playerCam;


    void Start()
    {
        //skyCam = GameObject.Find("Cameras/SkyCam").GetComponent<Camera>();
        //SetActiveMainCamera("SkyCam");
        //SetCursorToConfinedAndVisible();
    }

    public void findPlayerCam ()
    {
        //playerCam = GameObject.Find("OurPlayer/Character1_Reference/PlayerCam").GetComponent<Camera>();
    }

    public void SetActiveMainCamera(string camToSetActive)
    {
        if (camToSetActive.Equals(skyCam.gameObject.name)) {
            if (skyCam == null)
            {
                Debug.LogWarning("SkyCam is NULL");
                return;
            }
            playerCam.enabled = false;
            skyCam.enabled = true;
        } else if (camToSetActive.Equals(playerCam.gameObject.name)) {
            if (playerCam == null)
            {
                Debug.LogWarning("PlayerCamera IS NULL");
                return;
            }
            playerCam.enabled = true;
            skyCam.enabled = false;
        }
    }


    public void SetCursorToLockAndInvisible()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetCursorToFreeAndVisible()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
