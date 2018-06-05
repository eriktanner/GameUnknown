﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SpellManager : MonoBehaviour
{

    public List<Spell> spellList = new List<Spell>();
    public List<GameObject> spawnablePrefabs = new List<GameObject>();
    Transform spellManagerTransform;


    void Start()
    {
        spellManagerTransform = GameObject.Find("Managers/SpellManager").transform;
    }

    public Transform SpellManagerTransform
    {
        get { return spellManagerTransform; }
    }

    public static Spell getSpellFromName(string spellName)
    {
        spellName = Regex.Replace(spellName, @"[0-9]", string.Empty);
        return (Spell) Resources.Load("Spells/" + spellName, typeof(Spell)); //We are going to need to make a dictionary for this for performance
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


