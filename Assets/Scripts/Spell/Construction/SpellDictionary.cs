using System;
using System.Collections.Generic;
using UnityEngine;

/*Anything Type related*/
public static class SpellDictionary
{

    public static Dictionary<string, System.Type> nameToComponent = new Dictionary<string, System.Type>();



    public static void InitSpellDictionary()
    {
        nameToComponent.Add("Arcane Missile", new ArcaneMissile().GetType());
        nameToComponent.Add("Fear", new Fear().GetType());
        nameToComponent.Add("Fireball", new Fireball().GetType());
        nameToComponent.Add("Ice Wall", new IceWall().GetType());
        nameToComponent.Add("Pain", new Pain().GetType());
        nameToComponent.Add("Soul Void", new SoulVoid().GetType());
    }


    public static System.Type GetComponentType(string spellName)
    {
        return nameToComponent[spellName];
    }



    /*Retrieves the spell name from the spellObject's spell identifier and lookups/returns the corresponding SpellType*/
    public static Spell GetSpellFromSpellObject(GameObject spellObject)
    {
        SpellIdentifier spellIdentifier = spellObject.GetComponent<SpellIdentifier>();

        if (spellIdentifier == null) { 
            Debug.Log("SpellDictionary(GetSpellFromSpellObject): " + spellObject.name + " SpellIdentifier is null.");
            return null;
        }


        string spellName = SpellManager.getOriginalSpellName(spellIdentifier.SpellName);
        if (spellName == null)
        {
            Debug.Log("SpellDictionary(GetSpellFromSpellObject): " + spellObject.name + " spellName is null.");
            return null;
        }

        System.Type lookup = GetComponentType(spellName);
        return (Spell)spellObject.GetComponent(lookup);
    }


    /*Retrieves the spell name from the spellObject's spell identifier and lookups/returns the corresponding SpellType*/
    public static Spell GetSpellFromSpellName(string spellNameIn)
    {
        string spellName = SpellManager.getOriginalSpellName(spellNameIn);
        System.Type spellType = GetComponentType(spellName);
        Spell mySpell = (Spell) Activator.CreateInstance(spellType);

        GameObject temp = new GameObject();
        System.Type SpellType = GetComponentType(SpellManager.getOriginalSpellName(spellNameIn));
        temp.AddComponent(SpellType);



        return (Spell) temp.GetComponent(spellType);
    }


}