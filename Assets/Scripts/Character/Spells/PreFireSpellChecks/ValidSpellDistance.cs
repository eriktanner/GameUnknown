using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*We may not want ceratin spells to cast if not a valid range for them*/
public static class ValidSpellDistance {



    static void DisplayOutOfRangeMessage()
    {
        InGameMessageDisplay.DisplayMessage("Out of range", 3);
    }

    /*Looks up to see if a spell requires a valid distance in order for it to be casted. We need this so
     that we can assign that spell cast an arbirtatily large destroy time (so that it not a valid cast and
     destroyed by time before it gets to its hit point) (Destroy by time often does not produce same length 
     of spell travel)*/
    public static bool hasValidDistanceCheckBeforeCast(string spellName)
    {
        if (spellName.StartsWith("Fear"))
            return true;
        else if (spellName.StartsWith("Soul Void"))
            return true;
        else
            return false;
    }

    /*Certain spells are going to require certain layers to be hit before particles are instatiated*/
    public static bool SpellIsInRange(string spellName, Vector3 origin, Vector3 hitPosition, bool rayDidMakeContact)
    {
        float distance = (hitPosition - origin).magnitude;

        if (spellName.StartsWith("Fear"))
            return validRangeFear(distance, rayDidMakeContact);
        else if (spellName.StartsWith("Soul Void"))
            return validRangeSoulVoid(distance, rayDidMakeContact);
        else
            return true;

    }

    static bool validRangeFear(float distance, bool rayDidMakeContact)
    {
        Spell spell = SpellManager.getSpellFromName("Fear");
        bool isInRange = distance < spell.maxRange;
        if (!isInRange || !rayDidMakeContact)
        {
            DisplayOutOfRangeMessage();
            return false;
        }
        return true;
    }

    static bool validRangeSoulVoid(float distance, bool rayDidMakeContact)
    {
        Spell spell = SpellManager.getSpellFromName("Soul Void");
        bool isInRange = distance < spell.maxRange;
        if (!isInRange || !rayDidMakeContact)
        {
            DisplayOutOfRangeMessage();
            return false;
        }
        return true;
    }



    

}
