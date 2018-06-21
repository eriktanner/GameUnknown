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

        string spellName = SpellManager.GetOriginalSpellName(spellIdentifier.SpellName);
        if (spellName == null)
        {
            Debug.Log("SpellDictionary(GetSpellFromSpellObject): " + spellObject.name + " spellName is null.");
            return null;
        }

        System.Type lookup = GetTypeFromSpellName(spellName);
        return (Spell)spellObject.GetComponent(lookup);
    }


    /*This needs to be fixed - Both creates a Gamebject and Destroys it - really bad effeciency - gets called every spell*/
    public static Spell GetSpellFromSpellName(string spellNameIn)
    {
        GameObject temp = new GameObject();
        System.Type spellType = GetTypeFromSpellName(SpellManager.GetOriginalSpellName(spellNameIn));

        if (spellType == null)
        {
            Debug.Log("SpellDictionary(GetSpellFromSpellName): spellType is null");
            return null;
        }

        GameObject.Destroy(temp, 3.0f);

        temp.AddComponent(spellType);
        return (Spell) temp.GetComponent(spellType);
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
        var EffectTypes = Assembly.GetAssembly(typeof(Spell)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Spell)));

        nameToType = new Dictionary<string, System.Type>();

        foreach (var type in EffectTypes)
        {
            var tempSpell = Activator.CreateInstance(type) as Spell;

            if (!nameToType.ContainsKey(tempSpell.SpellStats.name))
                nameToType.Add(tempSpell.SpellStats.name, tempSpell.GetType());
        }

    }


}