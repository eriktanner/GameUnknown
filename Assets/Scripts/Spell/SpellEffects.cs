using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*Handles the servers calls for in-depth spell effects*/
public class SpellEffects : Photon.MonoBehaviour {

    public static SpellEffects Instance { get; private set; }

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


    public void CmdFearPlayer(GameObject playerHit)
    {
        FearEffect fearSpellEffect = new FearEffect(playerHit);
        fearSpellEffect.initFearSequence();

        Debug.Log("Calling Fear on: " + playerHit.gameObject.name);
    }
    
    public void CmdSoulVoidPlayer(GameObject playerHit, int shotBy)
    {
        SoulVoidEffect soulVoidEffect = new SoulVoidEffect(playerHit, shotBy);
        soulVoidEffect.initSoulVoidSequence();

        Debug.Log("Calling Soul Void on: " + playerHit.gameObject.name);
    }

}
