using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*Utility class to help us set references to classes that need references to the local player*/
[RequireComponent((typeof(Camera)))]
public class OnPlayerStart : MonoBehaviour {

    public Camera playerCam;
	
    void Start ()
    {
        SetLocalPlayers();
        GameObject.Find("Managers/CameraManager").GetComponent<CameraManager>().SetPlayerCamOnStart(playerCam);
    }


    void SetLocalPlayers()
    {
        GameObject.Find("Managers/GameManager").GetComponent<FloatingDamageController>().setLocalPlayerOnPlayerStart();
        GameObject.Find("Managers/SpellDestruction").GetComponent<SpellDestruction>().setLocalPlayerOnPlayerStart();
    }
}
