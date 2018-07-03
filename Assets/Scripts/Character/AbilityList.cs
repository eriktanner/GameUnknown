using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This calss will handle things such as putting spells into the caster availible spells to be casted. Will 
 also handle thigns such as cooldown checks and spell to keybind switches
 
Possibly think about giving each spell a class with Spell, cooldown timer, etc*/
public class AbilityList : MonoBehaviour {
    

    /*Seperate into SpellListComponent and SpellCooldown Component*/


    AbilityData[] abilityList = new AbilityData[6];
    float[] abilityCooldowns = new float[6];


    void Start()
    {
        addToSpellList("Fireball", 0);
        //addToSpellList("Pain", 1);
        //addToSpellList("Arcane Missile", 2);
        //addToSpellList("Soul Void", 3);
        //addToSpellList("Ice Wall", 4);
        //addToSpellList("Fear", 5);
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
            abilityCooldowns[i] -= Time.deltaTime;
            if (abilityCooldowns[i] < 0)
                abilityCooldowns[i] = 0;
        }
    }
 

    public AbilityData GetAbilityAtIndex(int index)
    {
        if (index < 0 || index > 5)
        {
            return null;
        }
        return abilityList[index];
    }

    /*Important: Works under the assumption ability passed in is from the user's abilityList (Ensures Spell Does in Fact have an AbilityData)*/
    public void TriggerCooldown(AbilityData abilityData)
    {
        for (int i = 0; i < 6; i++)
        {
            try{
                if (abilityList[i].AbilityName != null && abilityList[i].AbilityName.Equals(abilityData.AbilityName))
                    abilityCooldowns[i] = abilityList[i].Cooldown;
            } catch { }
        } 
    }

    public bool isOnCooldown(int index)
    {
        return abilityCooldowns[index] > 0;
    }





    /*Given a spell and index, adds to player's spellList*/
    void addToSpellList(string abilityName, int index)
    {
        if (index < 0 || index > 6)
            return;

        bool spellAlreadyExists = false;
        for (int i = 0; i < abilityList.Length; i++)
        {
            if (abilityList[i] != null && abilityName.Equals(abilityList[i].AbilityName))
            {
                spellAlreadyExists = true;
            }
        }

        if (!spellAlreadyExists)
        {
            try {
                abilityList[index] = AbilityDictionary.GetAbilityDataFromAbilityName(abilityName);
            } catch {
                Debug.Log("AbilityList - addToSpellList: abilityData Does Not Exist");
            }
        }
    }


    /*Given a spell, removes from player's spellList*/
    void removeFromAbilityList(string abilityName)
    {
        for (int i = 0; i < abilityList.Length; i++)
        {
            if (abilityList[i] != null && abilityName.Equals(abilityList[i].AbilityName))
            {
                abilityList[i] = null;
            }
        }
    }

    /*Removes spell from spellList's index*/
    void removeFromAbilityList(int index)
    {
        if (index < 0 || index > 6 || index > abilityList.Length - 1)
            return;
        abilityList[index] = null;
    }
}
