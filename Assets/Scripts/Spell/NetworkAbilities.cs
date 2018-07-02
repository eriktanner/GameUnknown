using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAbilities : Photon.MonoBehaviour {

    public static NetworkAbilities Instance { get; private set; }
    SpellDestruction SpellDestruction;

    void Start()
    {
        EnsureSingleton();
        SpellDestruction = SpellDestruction.Instance;
    }

    void EnsureSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Fire(Spell spell, Vector3 castSpawn, Vector3 hitPoint)
    {
        Vector3 aimToFromFirePosition = hitPoint - castSpawn;
        Quaternion rotationToTarget = Quaternion.LookRotation(aimToFromFirePosition);

        
        NetworkFireSpell(spell, castSpawn, rotationToTarget);
    }


    void NetworkFireSpell(Spell spell, Vector3 castSpawn, Quaternion rotationToTarget)
    {
        if (spell.SpellStats == null || spell.SpellStats.name == null)
        {
            Debug.Log("NetworkAbilities - NetworkFireSpell: null");
            return;
        }

        photonView.RPC("RpcFireSpell", PhotonTargets.All, spell.SpellStats.name, rotationToTarget, castSpawn, PlayerManager.LocalPlayer.name, PhotonNetwork.player.ID);
        photonView.RPC("ServerKeepProjectileCountInSync", PhotonTargets.MasterClient); //Call after fire, network too slow other way around
    }

    [PunRPC]
    void RpcFireSpell(string spellName, Quaternion rotationToTarget, Vector3 castSpawn, string shotByname, int shotBy)
    {
        SpellStats spell = SpellManager.GetSpellStatsFromName(spellName);

        GameObject spellObject = SpellCreation.CreateSpellInWorld(spell, castSpawn, rotationToTarget, shotByname, shotBy);

        SpellDestruction.DestroySpellByTime(spellObject);
    }


    [PunRPC]
    void ServerKeepProjectileCountInSync()
    {
        SpellManager.ProjectileCount++;
        photonView.RPC("SetClientProjectileCountToServerCount", PhotonTargets.All, SpellManager.ProjectileCount);
    }

    [PunRPC]
    void SetClientProjectileCountToServerCount(int serverProjectileCount)
    {
        SpellManager.ProjectileCount = serverProjectileCount;
    }


}
