using UnityEngine;

public class SpellIdentifier : MonoBehaviour {

    public string SpellName;
    public int ShotByID;
    public string ShotByName;
    
    

    public static GameObject AddSpellIdentifier(GameObject attachTo, string spellName, string shotByName, int shotBy)
    {
        SpellIdentifier spellIdentifier = attachTo.AddComponent<SpellIdentifier>();
        spellIdentifier.SpellName = SpellManager.GetOriginalSpellName(spellName);
        spellIdentifier.ShotByName = shotByName;
        spellIdentifier.ShotByID = shotBy;
        return attachTo;
    }
    
}
