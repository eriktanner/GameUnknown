using UnityEngine;

/*This class handles the in-world instation of spells*/
public static class SpellCreation {


    /*Creates the spell in world and gives it movement*/
    public static GameObject CreateSpellInWorld(AbilityData spell, Vector3 position, Quaternion rotation, string shotByName, int shotBy)
    {
        GameObject spellObject = GameObject.Instantiate(spell.Prefab, position, rotation);

        //Components
        spellObject.AddComponent<SpellMovement>();
        if (PhotonNetwork.isMasterClient)
            SpellCollision.AddSpellCollision(spellObject, ((IProjectile) spell).ProjectileRadius);


        //Identify
        AbilityIdentifier.AddSpellIdentifier(spellObject, spell.AbilityName, shotByName, shotBy);
        AssignUniqueSpellName(spell.AbilityName, spellObject);
        spellObject.tag = "Spell";
        spellObject.layer = 10;
        spellObject.transform.parent = SpellManager.SpellManagerTransform;

        return spellObject;
    }


    static void AssignUniqueSpellName(string originalSpellName, GameObject spellObject)
    {
        spellObject.name = originalSpellName + SpellManager.ProjectileCount;
    }




    public static GameObject CreateCollisionParticlesInWorld(string abilityName, Vector3 position)
    {
        AbilityData abilityData = AbilityDictionary.GetAbilityDataFromAbilityName(abilityName);

        if (abilityData as IHaveCollisionParticles == null)
        {
            Debug.Log("SpellCreation(CreateCollisionParticles): spellIdentifer is null");
            return null;
        }
        
        GameObject collisionParticles = GameObject.Instantiate(((IHaveCollisionParticles) abilityData).CollisionParticles, position, Quaternion.identity);

        collisionParticles.transform.parent = SpellManager.SpellManagerTransform;
        GameObject.Destroy(collisionParticles, ((IHaveCollisionParticles)abilityData).CollisionParticleLifespan);

        return collisionParticles;
    }

    

}
