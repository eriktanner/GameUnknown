using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

/*Anything Type related*/
public static class SpellDictionary
{

    public static Dictionary<string, System.Type> nameToType;


    public static Spell GetSpellFromSpellObject(GameObject spellObject)
    {
        SpellIdentifier spellIdentifier = spellObject.GetComponent<SpellIdentifier>();

        if (spellIdentifier == null) { 
            Debug.Log("SpellDictionary(GetSpellFromSpellObject): " + spellObject.name + " SpellIdentifier is null.");
            return null;
        }

        System.Type lookup = GetTypeFromSpellName(spellIdentifier.SpellName);
        if (lookup == null)
            return null;

        return (Spell) spellObject.GetComponent(lookup);
    }


    public static System.Type GetTypeFromSpellName(string spellName)
    {
        spellName = SpellManager.GetOriginalSpellName(spellName);
        if (nameToType == null)
            RegisterNamesToTypes();


        if (nameToType.ContainsKey(spellName))
        {
            System.Type foundEffect = nameToType[spellName];
            return foundEffect;
        }
        return null;
    }

    private static void RegisterNamesToTypes()
    {
        var spellTypes = Assembly.GetAssembly(typeof(Spell)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Spell)));

        nameToType = new Dictionary<string, System.Type>();

        foreach (var type in spellTypes)
        {
            var tempSpell = Activator.CreateInstance(type) as Spell;

            if (tempSpell.SpellStats != null && !nameToType.ContainsKey(tempSpell.SpellStats.name))
            {
                nameToType.Add(tempSpell.SpellStats.name, tempSpell.GetType());
            }
        }
    }


}