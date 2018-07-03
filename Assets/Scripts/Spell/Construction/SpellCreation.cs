using UnityEngine;

/*This class handles the in-world instation of spells*/
public static class SpellCreation {


    /*Creates the spell in world and gives it movement*/
    public static GameObject CreateSpellInWorld(AbilityData spell, Vector3 position, Quaternion rotation, string shotByName, int shotBy)
    {
        GameObject spellObject = GameObject.Instantiate(spell.Prefab, position, rotation);

        //Components
        spellObject.AddComponent<SpellMovement>();
        //System.Type SpellType = AbilityDictionary.GetTypeFromAbilityName(spell.AbilityName);
        //spellObject.AddComponent(SpellType);
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




    public static GameObject CreateCollisionParticlesInWorld(string spellName, Vector3 position, AbilityData spellStats, AbilityIdentifier spellIdentifierOfOriginalSpell)
    {
        if (spellIdentifierOfOriginalSpell == null)
        {
            Debug.Log("SpellCreation(CreateCollisionParticles): spellIdentifer is null");
            return null;
        }

        string spellNameToTransfer = spellIdentifierOfOriginalSpell.AbilityName;
        int shotByToTransfer = spellIdentifierOfOriginalSpell.ShotByID;
        string shotByNameToTransfer = spellIdentifierOfOriginalSpell.ShotByName;

        GameObject collisionParticles = GameObject.Instantiate(spellStats.CollisionParticle, position, Quaternion.identity);
        //collisionParticles.AddComponent(AbilityDictionary.GetTypeFromAbilityName(SpellManager.GetOriginalSpellName(spellName)));
        AbilityIdentifier.AddSpellIdentifier(collisionParticles, spellNameToTransfer, shotByNameToTransfer, shotByToTransfer);

        collisionParticles.transform.parent = SpellManager.SpellManagerTransform;

        return collisionParticles;
    }

}
