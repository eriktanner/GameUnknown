using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SpellManager : MonoBehaviour
{

    public List<SpellStats> spellList = new List<SpellStats>();
    public List<GameObject> spawnablePrefabs = new List<GameObject>();
    Transform spellManagerTransform;


    void Start()
    {
        spellManagerTransform = GameObject.Find("Managers/SpellManager").transform;
        SpellDictionary.InitSpellDictionary();
    }

    public Transform SpellManagerTransform
    {
        get { return spellManagerTransform; }
    }

    public static SpellStats GetSpellStatsFromName(string spellName)
    {
        spellName = getOriginalSpellName(spellName);
        return (SpellStats) Resources.Load("SpellStats/" + spellName, typeof(SpellStats)); //We are going to need to make a dictionary for this for performance
    }

    public static string getOriginalSpellName(string spellName)
    {
        string removeFromString = Regex.Replace(spellName, @"[0-9]", string.Empty);
        removeFromString = Regex.Replace(removeFromString, "Clone", string.Empty);
        removeFromString = Regex.Replace(removeFromString, "[()]", string.Empty);
        return removeFromString;
    }
    

    public static GameObject getObjectFromSpellName(string spellName)
    {
        return GameObject.Find("Managers/SpellManager/" + spellName);
    }

    public GameObject getSpawnablePrefab(string prefabName)
    {
        foreach (GameObject prefab in spawnablePrefabs)
        {
            if (prefab.name.Equals(prefabName))
                return prefab;
        }
        return null;
    }

    

}


