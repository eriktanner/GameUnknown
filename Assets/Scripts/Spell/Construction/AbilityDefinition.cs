using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Name space for Spell Object
 
 How Spells Are Being Handled over Network:
 1. Player calls Server function which calls Rpc function telling every client to create spellObject
 2. Each spell created on each of the clients machine (may or may not have been shot by them) and is given the same identification number
 3. Player who shot detects for collision and again calls a Server->Rpc and passes in the identification of that spell
 4. Rpc function calls destroy on the object tied to the identification number (deletes all instances locally)
 5. Player who shot spell calls server, notifies that player has been hit by his spell
 IMPORTANT - This is highly optimized however, I am not sure whether having the player who shot detect a hit is good,
 We need to discuss the role of server authority in relation to this and see if it matters
 Also, we need to discuss the lag that may occur during the first server call that may appear to the shooting player*/
public class AbilityDefinition : ScriptableObject {

    public string abilityName = "";
    public string shotBy = "";
    public GameObject prefab = null;
    public GameObject collisionParticle = null;
    public Texture2D icon = null;
    public float cooldown = 0.0f;
    public float damage = 0.0f;
    public float manaCost = 0.0f;
    public float maxRange = 0.0f;
    public float projectileRadius = 0.0f;
    public float projectileSpeed = 0.0f;
    public float castTime = 0.0f;

}

