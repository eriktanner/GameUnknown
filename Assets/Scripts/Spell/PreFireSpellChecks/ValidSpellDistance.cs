using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*We may not want ceratin spells to cast if not a valid range for them*/
public static class ValidSpellDistance {



    static void DisplayOutOfRangeMessage()
    {
        InGameMessageDisplay.DisplayMessage("Out of range", 3);
    }

   
    /*Certain spells are going to require certain layers to be hit before particles are instatiated*/
    public static bool SpellIsInRange(string spellName, Vector3 origin, Vector3 hitPosition, bool rayDidMakeContact)
    {

        float distance = (hitPosition - origin).magnitude;
        Spell spell = SpellDictionary.GetSpellFromSpellName(spellName);


        if (spell != null && spell.IsValidDistanceChecked)
        {
            return validRange(spellName, distance, rayDidMakeContact);
        }

        if (spell == null)
        {
            Debug.Log("SpellDictionary.GetSpellFromSpellName(spellName: is null");
        }
        return true;
    }

    static bool validRange(string spellName, float distance, bool rayDidMakeContact)
    {
        SpellStats spell = SpellManager.GetSpellStatsFromName(spellName);
        if (spell == null)
        {
            Debug.Log("SpellDictionary.GetSpellFromSpellName(spellName(validRange): is null");
        }
        bool isInRange = distance < spell.maxRange;
        if (!isInRange || !rayDidMakeContact)
        {
            DisplayOutOfRangeMessage();
            return false;
        }
        return true;
    }

    



    

}
