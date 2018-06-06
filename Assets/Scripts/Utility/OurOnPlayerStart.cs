using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*Utility class to help us set references to classes that need references to the local player*/
public class OurOnPlayerStart : MonoBehaviour {

	



    void Start ()
    {
        GameObject.Find("Managers/UIManager").GetComponent<FloatingDamageController>().setLocalPlayer();
    }



}
