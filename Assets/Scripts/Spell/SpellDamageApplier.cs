using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Network application of damage*/
public class SpellDamageApplier : Photon.MonoBehaviour {

   

    /*Apply damage to player hit, then send player's new health amount over the network*/ //EFFECIENCY
    public void ApplyDamageFromTo(GameObject hitPlayer, float damage, int shotBy)
    {
        float hitPlayerHealthAfterDamage = hitPlayer.GetComponent<HealthBar>().CalculateTakeDamage(damage);
        PhotonPlayer hitPhotonPlayer = null;
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            if ((GameObject)photonPlayer.TagObject == hitPlayer)
            {
                hitPhotonPlayer = photonPlayer;
                break;
            }
        }

        //if (hitPhotonPlayer != null)
            photonView.RPC("RpcSetPlayerToNewHealthValue", PhotonTargets.All, hitPlayer.name, hitPlayerHealthAfterDamage);
       // else
        {

           // Debug.Log("Could not find PhotonPlayer of hitPlayer");
        }
    }

    [PunRPC] /*Sets the hit player's health to the new value, automatically regenerates after a while*/
    void RpcSetPlayerToNewHealthValue(string hitPlayerName, float newHealth)
    {
        GameObject hitPlayerGO = OurGameManager.GetPlayerGameObject(hitPlayerName);

        
        

        if (hitPlayerGO != null)
        {

            hitPlayerGO.GetComponent<HealthBar>().SetHealth(newHealth);

            if (hitPlayerGO.GetComponent<HealthBar>() == null)
                Debug.Log("HealthBar hitPlayer is null");

        }
        else
            Debug.Log("RPCSetPlayerToNewHealthValue: Could not find hitPlayerId: " + hitPlayerName);

    }
}
