using UnityEngine;

/*Network application of damage*/
public class NetworkDamageApplier : Photon.MonoBehaviour {

    public static NetworkDamageApplier Instance { get; private set; }

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

    /*Apply damage to player hit, then send player's new health amount over the network*/
    public void ApplyDamageFromTo(GameObject hitPlayer, float damage, int shotBy)
    {
        float hitPlayerHealthAfterDamage = hitPlayer.GetComponent<HealthBar>().CalculateTakeDamage(damage);

        PhotonPlayer hitPhotonPlayer = PlayerManager.GetPhotonPlayerFromGameObject(hitPlayer);

        if (hitPlayerHealthAfterDamage <= 0) {
            KillPlayer(hitPhotonPlayer);
        } else {
            photonView.RPC("RpcSetPlayerToNewHealthValue", PhotonTargets.All, hitPlayer.name, hitPlayerHealthAfterDamage);
        }
    }

    void KillPlayer(PhotonPlayer playerToKill)
    {
        if (playerToKill != null)
        {
            photonView.RPC("RpcSetCursorToVisibleOnKickFromMatch", playerToKill);
            PhotonNetwork.CloseConnection(playerToKill); //Die - Note called on master client only
        }
    }

    [PunRPC] 
    void RpcSetCursorToVisibleOnKickFromMatch()
    {
        CameraManager.SetCursorToFreeAndVisible();
    }


    [PunRPC] /*Sets the hit player's health to the new value, automatically regenerates after a while*/
    void RpcSetPlayerToNewHealthValue(string hitPlayerName, float newHealth)
    {
        GameObject hitPlayerGO = PlayerManager.GetPlayerGameObject(hitPlayerName);

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
