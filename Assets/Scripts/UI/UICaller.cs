using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UICaller : NetworkBehaviour {


    FloatingDamageController floatingDamageController;

    void Start()
    {
        floatingDamageController = GameObject.Find("Managers/GameManager").GetComponent<FloatingDamageController>();
    }
    




}
