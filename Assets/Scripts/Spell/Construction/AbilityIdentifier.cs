using UnityEngine;

public class AbilityIdentifier : MonoBehaviour {

    public string AbilityName;
    public int ShotByID;
    public string ShotByName;
    
    

    public static GameObject AddSpellIdentifier(GameObject attachTo, string spellName, string shotByName, int shotBy)
    {
        AbilityIdentifier spellIdentifier = attachTo.AddComponent<AbilityIdentifier>();
        spellIdentifier.AbilityName = SpellManager.GetOriginalSpellName(spellName);
        spellIdentifier.ShotByName = shotByName;
        spellIdentifier.ShotByID = shotBy;
        return attachTo;
    }
    
}
