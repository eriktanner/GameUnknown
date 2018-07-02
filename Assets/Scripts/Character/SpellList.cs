using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This calss will handle things such as putting spells into the caster availible spells to be casted. Will 
 also handle thigns such as cooldown checks and spell to keybind switches
 
Possibly think about giving each spell a class with Spell, cooldown timer, etc*/
public class SpellList : MonoBehaviour {
    

    /*Seperate into SpellListComponent and SpellCooldown Component*/


    Spell[] spellList = new Spell[6];
    float[] spellCooldowns = new float[6];


    void Start()
    {
        addToSpellList("Fireball", 0);
        addToSpellList("Pain", 1);
        addToSpellList("Arcane Missile", 2);
        addToSpellList("Soul Void", 3);
        addToSpellList("Ice Wall", 4);
        addToSpellList("Fear", 5);
    }

    void Update()
    {
        UpdateCooldownTimers();
    }

    /*Can we do this more effeciently?*/
    void UpdateCooldownTimers()
    {
        for(int i = 0; i < 6; i++)
        {
            spellCooldowns[i] -= Time.deltaTime;
            if (spellCooldowns[i] < 0)
                spellCooldowns[i] = 0;
        }
    }
 

    public Spell GetSpellAtIndex(int index)
    {
        if (index < 0 || index > 5)
        {
            return null;
        }
        return spellList[index];
    }

    /*Triggers cooldown at spellCooldown index that corresponds to the spell index*/
    public void TriggerCooldown(Spell spell)
    {
        for (int i = 0; i < 6; i++)
        {
            if (spellList[i].SpellStats == spell.SpellStats)
                spellCooldowns[i] = spellList[i].SpellStats.cooldown;
        } 
    }

    public bool isOnCooldown(int index)
    {
        return spellCooldowns[index] > 0;
    }





    /*Given a spell and index, adds to player's spellList*/
    void addToSpellList(string spellName, int index)
    {
        if (index < 0 || index > 6)
            return;

        bool spellAlreadyExists = false;
        for (int i = 0; i < spellList.Length; i++)
        {
            if (spellList[i] != null && spellName.Equals(spellList[i].SpellStats.spellName))
            {
                spellAlreadyExists = true;
            }
        }

        if (!spellAlreadyExists)
        {
            spellList[index] = SpellDictionary.GetSpellFromSpellName(spellName);
        }
    }


    /*Given a spell, removes from player's spellList*/
    void removeFromSpellList(string spellName)
    {
        for (int i = 0; i < spellList.Length; i++)
        {
            if (spellList[i] != null && spellName.Equals(spellList[i].SpellStats.spellName))
            {
                spellList[i] = null;
            }
        }
    }

    /*Removes spell from spellList's index*/
    void removeFromSpellList(int index)
    {
        if (index < 0 || index > 6 || index > spellList.Length - 1)
            return;
        spellList[index] = null;
    }
}
