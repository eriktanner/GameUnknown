using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

    GameObject localPlayer;
    HealthBar healthBarHit;

    Spell spell;
    Transform playerHitTransform;
    FloatingDamageController floatingDamageController;

    float damageToDo;
    bool criticalDamage = false;



    public Damage(Spell spell, Transform playerHitTransform)
    {
        this.spell = spell;
        this.playerHitTransform = playerHitTransform;


        localPlayer = GameObject.Find("Managers/NetworkManager").GetComponent<OurNetworkManager>().client.connection.playerControllers[0].gameObject;
        healthBarHit = localPlayer.gameObject.GetComponent<HealthBar>();

        floatingDamageController = GameObject.Find("Managers/UIManager").GetComponent<FloatingDamageController>();

    }
    

    /*Damages player and displays damage text*/
    public void ApplyDamage()
    {
        CalculateDamage();
        displayFloatingDamageText();
        healthBarHit.CmdCollisionDamagePlayer(damageToDo, playerHitTransform.gameObject.name);
    }

    /*Does the randomized calculation of damage, and also randomizes for a critical hit*/
    void CalculateDamage()
    {
        float checkForCrit = Random.value;
        criticalDamage = (checkForCrit < .15f);

        if (criticalDamage)
        {
            damageToDo = CrticalDamageCalculation();
        } else
        {
            damageToDo = NormalDamageCalculation();
        }
    }

    float NormalDamageCalculation()
    {
        float originalDamage = spell.damage;

        float lowerBound = .8f * originalDamage;
        float upperBound = 1.1f * originalDamage;

        return Random.Range(lowerBound, upperBound);
    }

    float CrticalDamageCalculation()
    {
        float originalDamage = spell.damage;

        float lowerBound = 1.3f * originalDamage;
        float upperBound = 1.6f * originalDamage;

        return Random.Range(lowerBound, upperBound);
    }




    void displayFloatingDamageText()
    {
        if (!criticalDamage)
        {
            floatingDamageController.CreateFloatingText(damageToDo, playerHitTransform.gameObject.transform);
        } else
        {
            floatingDamageController.CreateFloatingText(damageToDo, playerHitTransform.gameObject.transform);
            floatingDamageController.CriticalDamageFontSizeIncrease();
        }
    }

}
