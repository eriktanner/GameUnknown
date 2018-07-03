using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

/*Handles spell destruction of spells and their collision particles. Note: destruction does not handle
actual spell effects*/
public class SpellDestruction : Photon.MonoBehaviour
{

    public static SpellDestruction Instance { get; private set; }

    void Start()
    {
        EnsureSingleton();
    }

    void EnsureSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }




    
}
