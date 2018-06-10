using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Handles the actuall application of damage over the network*/
public class Damager : Photon.MonoBehaviour {

    FloatingDamageController floatingDamageController;

    // Use this for initialization
    void Start () {
        floatingDamageController = GameObject.Find("Managers/GameManager").GetComponent<FloatingDamageController>();
    }

    public void displayFloatingDamage(Vector3 hitPosition, float damage, bool criticalDamage, int shotBy)
    {
        photonView.RPC("TargetDisplayFloatingDamage", PhotonPlayer.Find(shotBy), hitPosition, damage, criticalDamage, shotBy);
    }

    [PunRPC]
    void TargetDisplayFloatingDamage(Vector3 hitPosition, float damage, bool criticalDamage, int shotBy)
    {

        floatingDamageController.CreateFloatingText(damage, hitPosition, criticalDamage, shotBy);


    }
}
