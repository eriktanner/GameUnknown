using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*Handles the servers calls for in-depth spell effects*/
public class SpellEffects : NetworkBehaviour {
    
    [Command]
    public void CmdFearPlayer(GameObject playerHit)
    {
        FearEffect fearSpellEffect = new FearEffect(playerHit);
        fearSpellEffect.initFearSequence();

        Debug.Log("Calling Fear on: " + playerHit.gameObject.name);
    }

    [Command]
    public void CmdSoulVoidPlayer(GameObject playerHit)
    {
        SoulVoidEffect soulVoidEffect = new SoulVoidEffect(playerHit);
        soulVoidEffect.initSoulVoidSequence();

        Debug.Log("Calling Soul Void on: " + playerHit.gameObject.name);
    }

}
