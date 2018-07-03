
using UnityEngine;
using System;

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

        if (AbilityDictionary.GetAbilityDataFromAbilityName(spellName) as ICheckForDistance != null)
        {
            return validRange(spellName, distance, rayDidMakeContact);
        }

        return true;
    }

    static bool validRange(string spellName, float distance, bool rayDidMakeContact)
    {
        AbilityData spell = SpellManager.GetSpellStatsFromName(spellName);

        bool isInRange = distance < spell.MaxRange;
        if (!isInRange || !rayDidMakeContact)
        {
            DisplayOutOfRangeMessage();
            return false;
        }
        return true;
    }



    

}
