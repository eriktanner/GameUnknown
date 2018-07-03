using UnityEngine;


/*Displays pain collision particle even if it does not make a collision*/
public class DestroyPain {
    
    GameObject particleObject;
    SpellDestruction spellDestruction;

    public DestroyPain(GameObject spellObjectIn)
    {
        particleObject = spellObjectIn;
    }


    public void explodePain()
    {
        if (particleObject == null)
            return;

        //Ability spell = AbilityDictionary.GetAbilityFromSpellObject(particleObject);

        /*

        if (spell == null)
        {
            Debug.Log("DestroyPain - Spell Lookup is null: ");
            GameObject.Destroy(particleObject);
            return;
        }
        
    */

        //GameObject collisionParticles = GameObject.Instantiate(spell.AbilityData.CollisionParticle, particleObject.transform.position, Quaternion.identity);
        //GameObject.Destroy(collisionParticles, 1.5f);
    }
}
