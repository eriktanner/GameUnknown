using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SpellManager : MonoBehaviour
{

    public List<Spell> spellList = new List<Spell>();


    public Spell getSpellFromName(string spellName)
    {
        spellName = Regex.Replace(spellName, @"[0-9]", string.Empty);
        return (Spell) Resources.Load("Spells/" + spellName, typeof(Spell)); //We are going to need to make a dictionary for this for performance
    }

}


