using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SpellManager : MonoBehaviour
{

    public static SpellManager Instance { get; private set; }
    public static List<GameObject> spawnablePrefabs = new List<GameObject>();
    public static Transform SpellManagerTransform { get; private set; }

    public List<SpellStats> spellList = new List<SpellStats>();


    void Start()
    {
        EnsureSingleton();
        SpellManagerTransform = GameObject.Find("Managers/SpellManager").transform;
        SpellDictionary.InitSpellDictionary();
    }

    void EnsureSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }



    public static SpellStats GetSpellStatsFromName(string spellName)
    {
        spellName = GetOriginalSpellName(spellName);
        return (SpellStats) Resources.Load("SpellStats/" + spellName, typeof(SpellStats)); //We are going to need to make a dictionary for this for performance
    }

    public static string GetOriginalSpellName(string spellName)
    {
        string removeFromString = Regex.Replace(spellName, @"[0-9]", string.Empty);
        removeFromString = Regex.Replace(removeFromString, "Clone", string.Empty);
        removeFromString = Regex.Replace(removeFromString, "[()]", string.Empty);
        return removeFromString;
    }

    
    public static GameObject GetObjectFromSpellName(string spellName)
    {
        return GameObject.Find("Managers/SpellManager/" + spellName);
    }

    public static GameObject GetSpawnableSpellPrefab(string prefabName)
    {
        foreach (GameObject prefab in spawnablePrefabs)
        {
            if (prefab.name.Equals(prefabName))
                return prefab;
        }
        return null;
    }

    

}


