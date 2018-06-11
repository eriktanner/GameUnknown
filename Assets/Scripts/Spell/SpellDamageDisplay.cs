using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Controller for the instances of FloatingDamage*/
public class SpellDamageDisplay : Photon.MonoBehaviour {

    public FloatingDamage floatingTextPrefab;
    SpellDamageDisplay floatingDamageController;

    FloatingDamage instance;
    Vector3 hitPlayerLocation;

    // Use this for initialization
    void Start()
    {
        floatingDamageController = GameObject.Find("Spell").GetComponent<SpellDamageDisplay>();
    }


    public void CreateFloatingText(float damage, Vector3 locationOfHit, bool criticalDamage, int shotBy)
    {
        //Pull from pool of objects (or create for now)
        instance = Instantiate(floatingTextPrefab);
        hitPlayerLocation = locationOfHit;

        instance.initFloatingDamage(locationOfHit, damage, criticalDamage, shotBy);

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
