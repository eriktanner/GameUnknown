using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This calss will handle things such as putting spells into the caster availible spells to be casted. Will 
 also handle thigns such as cooldown checks and spell to keybind switches
 
Possibly think about giving each spell a class with Spell, cooldown timer, etc*/
public class SpellList : MonoBehaviour {
    
    Spell[] spellList = new Spell[6];
    float[] spellCooldowns = new float[6];


    void Start()
    {
        addToSpellList("Fireball", 0);
        addToSpellList("Fear", 1);
        addToSpellList("Arcane Missile", 2);
        addToSpellList("Soul Void", 3);
        addToSpellList("Ice Wall", 4);
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
            if (spellList[i] == spell)
                spellCooldowns[i] = spellList[i].cooldown;
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
            if (spellList[i] != null && spellName.Equals(spellList[i].spellName))
            {
                spellAlreadyExists = true;
            }
        }

        if (!spellAlreadyExists)
        {
            spellList[index] = SpellManager.getSpellFromName(spellName);
        }
    }

    /*Given a spell, removes from player's spellList*/
    void removeFromSpellList(string spellName)
    {
        for (int i = 0; i < spellList.Length; i++)
        {
            if (spellList[i] != null && spellName.Equals(spellList[i].spellName))
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
