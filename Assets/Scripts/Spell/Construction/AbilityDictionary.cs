using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

/*Anything Type related*/
public static class AbilityDictionary
{

    public static Dictionary<string, AbilityData> nameToAbilityData;


    public static AbilityData GetAbilityDataFromSpellObject(GameObject spellObject)
    {
        AbilityIdentifier abilityIdentifier = spellObject.GetComponent<AbilityIdentifier>();

        if (abilityIdentifier == null) { 
            Debug.Log("SpellDictionary(GetSpellFromSpellObject): " + spellObject.name + " SpellIdentifier is null.");
            return null;
        }

        AbilityData lookup = GetAbilityDataFromAbilityName(abilityIdentifier.AbilityName);
        return lookup;
    }

    public static AbilityData GetAbilityDataFromAbilityName(string abilityName)
    {
        abilityName = SpellManager.GetOriginalSpellName(abilityName);
        if (nameToAbilityData == null)
            RegisterNamesToTypes();

        if (nameToAbilityData.ContainsKey(abilityName))
        {
            AbilityData foundData = nameToAbilityData[abilityName];
            return foundData;
        }
        return null;
    }

    private static void RegisterNamesToTypes()
    {
        var spellTypes = Assembly.GetAssembly(typeof(AbilityData)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AbilityData)));


        nameToAbilityData = new Dictionary<string, AbilityData>();


        foreach (var type in spellTypes)
        {
            var tempAbilityData = Activator.CreateInstance(type) as AbilityData;
            

            if (!nameToAbilityData.ContainsKey(tempAbilityData.AbilityName))
            {
                nameToAbilityData.Add(tempAbilityData.AbilityName, tempAbilityData);
            }

        }
    }


}