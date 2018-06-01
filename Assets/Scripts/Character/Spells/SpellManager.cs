using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{

    public List<Spell> spellList = new List<Spell>();


    public Spell getSpellFromName(string spellName)
    {
        return (Spell)Resources.Load("Spells/" + spellName, typeof(Spell)); //We are going to need to make a dictionary for this for performance
    }

}


