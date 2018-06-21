using System;
using System.Collections.Generic;
using UnityEngine;
using System;

/*Anything Type related*/
public static class SpellDictionary
{

    public static Dictionary<string, System.Type> nameToType = new Dictionary<string, System.Type>();



    public static void InitSpellDictionary()
    {
        nameToType.Add("Arcane Missile", Type.GetType(typeof(ArcaneMissile).Name));
        nameToType.Add("Fear", Type.GetType(typeof(Fear).Name));
        nameToType.Add("Fireball", Type.GetType(typeof(Fireball).Name));
        nameToType.Add("Ice Wall", Type.GetType(typeof(IceWall).Name));
        nameToType.Add("Pain", Type.GetType(typeof(Pain).Name));
        nameToType.Add("Soul Void", Type.GetType(typeof(SoulVoid).Name));
    }


    public static System.Type GetTypeFromSpellName(string spellName)
    {
        spellName = SpellManager.GetOriginalSpellName(spellName);
        return nameToType[spellName];
    }

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


}