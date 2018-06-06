using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*Utility class to help us set references to classes that need references to the local player*/
[RequireComponent((typeof(Camera)))]
public class OurOnPlayerStart : MonoBehaviour {

    public Camera playerCam;
	
    void Start ()
    {
        GameObject.Find("Managers/GameManager").GetComponent<FloatingDamageController>().setLocalPlayerOnStart();
        GameObject.Find("Managers/CameraManager").GetComponent<CameraManager>().SetPlayerCamOnStart(playerCam);
    }


}
