using UnityEngine;

public class SpellIdentifier : MonoBehaviour {

    public string SpellName;
    public int ShotBy;
    public string ShotByName;
    
    

    public static GameObject AddSpellIdentifier(GameObject attachTo, string spellName, string shotByName, int shotBy)
    {
        SpellIdentifier spellIdentifier = attachTo.AddComponent<SpellIdentifier>();
        spellIdentifier.SpellName = SpellManager.getOriginalSpellName(spellName);
        spellIdentifier.ShotByName = shotByName;
        spellIdentifier.ShotBy = shotBy;
        return attachTo;
    }
    
}
