using System;
using UnityEngine;


/*Checks if a spell hits a valid layer. We may want certain spells to only be casted on certain layers. For example,
 We may only allow certain spells to be casted on the ground (not a wall)*/
public static class ValidSpellLayer {


    static void DisplayInvalidLayerMessage()
    {
        InGameMessageDisplay.DisplayMessage("Cannot cast there", 3);
    }

    /*Certain spells are going to require certain layers to be hit before particles are instatiated*/
    public static bool SpellHitValidLayerBySpell(string spellName, RaycastHit Hit)
    {
        if (SpellDictionary.GetSpellFromSpellName(spellName).IsValidLayerCheckedGround)
        {
            return validLayerGround(Hit);
        }

        return true;
    }


    static bool validLayerGround(RaycastHit Hit)
    {
        if (Hit.collider == null)
            return false;

        bool isValidLayer = Hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground");
        if (!isValidLayer) DisplayInvalidLayerMessage();
        return isValidLayer;
    }
    
}
