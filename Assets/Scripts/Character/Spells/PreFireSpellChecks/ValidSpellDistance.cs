using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*We may not want ceratin spells to cast if not a valid range for them*/
public static class ValidSpellDistance {



    static void DisplayOutOfRangeMessage()
    {
        InGameMessageDisplay.DisplayMessage("Out of range");
        //Debug.Log("Out of Range");
    }

    /*We want to reduce the magnitude of the valid range because otherwise it might cast and be destroyed by
     the timed destroy. We'd rather it not be casted at all*/
    static float Range(float rangeIn)
    {
        return rangeIn - 3;
    }

    /*Certain spells are going to require certain layers to be hit before particles are instatiated*/
    public static bool SpellIsInRange(string spellName, Vector3 origin, Vector3 hitPosition, bool rayDidMakeContact)
    {

        if (spellName.StartsWith("Fear"))
            return validRangeFear(origin, hitPosition, rayDidMakeContact);
        else
            return true;

    }

    static bool validRangeFear(Vector3 origin, Vector3 hitPosition, bool rayDidMakeContact)
    {
        Spell spell = SpellManager.getSpellFromName("Fear");
        bool isInRange = (hitPosition - origin).magnitude < Range(spell.maxRange);
        if (!isInRange) DisplayOutOfRangeMessage();
        return isInRange;
    }
}
