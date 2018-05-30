using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : ScriptableObject {

    public string name = "";
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

