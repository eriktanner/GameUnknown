using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Controller for the instances of FloatingDamage*/
public class FloatingDamageController : MonoBehaviour {

    public FloatingDamage floatingTextPrefab;

    FloatingDamage instance;
    Vector3 hitPlayerLocation;


    

	public void CreateFloatingText(float damage, Vector3 locationOfHit, bool criticalDamage, int shotBy)
    {
        //Pull from pool of objects (or create for now)
        instance = Instantiate(floatingTextPrefab);
        hitPlayerLocation = locationOfHit;

        instance.initFloatingDamage(locationOfHit, damage, criticalDamage, shotBy);

    }



}
