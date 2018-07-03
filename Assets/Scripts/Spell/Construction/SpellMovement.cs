using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Updates the in-world position of a spell after it has been instatiated, giving it movement*/
public class SpellMovement : MonoBehaviour {
    
    float velocity;

    void Start()
    {
        velocity = ((IProjectile) SpellManager.GetSpellStatsFromName(gameObject.name)).ProjectileSpeed;
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

    public void FreezeSpellMovement()
    {
        velocity = 0;
    }
}
