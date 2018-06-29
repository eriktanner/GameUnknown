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
