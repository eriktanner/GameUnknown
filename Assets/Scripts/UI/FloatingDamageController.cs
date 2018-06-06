﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageController : MonoBehaviour {

    public FloatingDamage floatingTextPrefab;
    GameObject localPlayer;

    FloatingDamage instance;
    Transform hitPlayerLocation;

    
    /* Gets set in OnPlayerStart */
    public void setLocalPlayer()
    {
        localPlayer = GameObject.Find("Managers/NetworkManager").GetComponent<OurNetworkManager>().client.connection.playerControllers[0].gameObject;
    }

	public void CreateFloatingText(float damage, Transform location)
    {
        //Pull from pool of objects (or create for now)
        instance = Instantiate(floatingTextPrefab);
        hitPlayerLocation = location;

        
        
        instance.transform.position = hitPlayerLocation.position + instance.PositionOfText;
        
        instance.SetText(damage);

    }

    void Update()
    {
        if (instance != null && hitPlayerLocation != null)
            instance.transform.LookAt(2 * hitPlayerLocation.position - localPlayer.transform.position + new Vector3(0, 3, 0)); //Orients it the right way
    }

   

    public void CriticalDamageFontSizeIncrease()
    {
        instance.FontSizeIncrease();
    }


}
