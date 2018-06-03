using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValidSpellLayer {



    static void DisplayInvalidLayerMessage()
    {
        InGameMessageDisplay.DisplayMessage("Cannot cast there");
    }


    /*Certain spells are going to require certain layers to be hit before particles are instatiated*/
    public static bool SpellHitValidLayerBySpell(string spellName, RaycastHit Hit)
    {
        

        if (spellName.StartsWith("Fear"))
            return validLayerFear(Hit);
        else
            return true;

    }

    static bool validLayerFear(RaycastHit Hit)
    {
        if (Hit.collider == null)
            return false;

        bool isValidLayer = Hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground");
        if (!isValidLayer) DisplayInvalidLayerMessage();
        return isValidLayer;
    }
}
