using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageController : MonoBehaviour {

    public FloatingDamage floatingTextPrefab;
    GameObject localPlayer;

    FloatingDamage instance;
    Transform hitPlayerLocation;

    
    /* Gets set in OnPlayerStart */
    public void setLocalPlayerOnStart()
    {
        localPlayer = GameObject.Find("Managers/NetworkManager").GetComponent<OurNetworkManager>().client.connection.playerControllers[0].gameObject;
    }

	public void CreateFloatingText(Transform playerWhoShot, float damage, Transform locationOfHit)
    {
        //Pull from pool of objects (or create for now)
        instance = Instantiate(floatingTextPrefab);
        hitPlayerLocation = locationOfHit;

        instance.initFloatingDamage(playerWhoShot.position, locationOfHit.position, damage);
    }

    void Update()
    {
        if (instance != null && hitPlayerLocation != null && localPlayer != null)
            instance.transform.LookAt(2 * hitPlayerLocation.position - localPlayer.transform.position + new Vector3(0, 3, 0)); //Orients it the right way
    }

   

    public void CriticalDamageFontSizeIncrease()
    {
        instance.FontSizeIncrease();
    }


}
