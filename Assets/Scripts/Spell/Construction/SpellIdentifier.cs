using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellIdentifier : MonoBehaviour {
    public int ShotBy;
    public GameObject PlayerGameObject;

    public PhotonPlayer PlayerPhotonPlayer;

    public static GameObject AddSpellIdentifier(GameObject attachTo, GameObject playerGameObject, int shotBy)
    {
        SpellIdentifier spellIdentifier = attachTo.AddComponent<SpellIdentifier>();
        spellIdentifier.PlayerGameObject = playerGameObject;
        spellIdentifier.PlayerPhotonPlayer = PhotonPlayer.Find(shotBy);
        spellIdentifier.ShotBy = shotBy;
        return attachTo;
    }
    
}
