using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMovement : MonoBehaviour {
    

    float velocity;

    void Start()
    {
        velocity = GameObject.Find("Managers/SpellManager").GetComponent<SpellManager>().getSpellFromName(gameObject.name).projectileSpeed;
    }


    void Update()
    {
        MoveSpell();
    }

    /*Moves the spell in the world*/
    void MoveSpell()
    {
        transform.position += transform.forward * velocity * Time.deltaTime;
    }
}
