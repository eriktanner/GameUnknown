using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*This class will handle more in-depth spell effects on players*/
public class SpellEffects : NetworkBehaviour {
    
    [Command]
    public void CmdFearPlayer(GameObject playerToFear)
    {
        Debug.Log("Calling Fear on: " + playerToFear.gameObject.name);
    }


}
