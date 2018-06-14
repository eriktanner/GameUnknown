using UnityEngine;

/*This class handles the in-world instation of spells*/
public static class SpellCreation {


    /*Creates the spell in world and gives it movement*/
    public static GameObject CreateSpellInWorld(SpellStats spell, Vector3 position, Quaternion rotation, string shotByName, int shotBy)
    {
        GameObject spellObject = GameObject.Instantiate(spell.prefab, position, rotation);

        //Components
        spellObject.AddComponent<SpellMovement>();
        System.Type SpellType = SpellDictionary.GetTypeFromSpellName(spell.name);
        spellObject.AddComponent(SpellType);
        if (PhotonNetwork.isMasterClient)
            SpellCollision.AddSpellCollision(spellObject, spell.projectileRadius);


        //Identify
        SpellIdentifier.AddSpellIdentifier(spellObject, spell.name, shotByName, shotBy);
        AssignUniqueSpellName(spell.name, spellObject);
        spellObject.tag = "Spell";
        spellObject.layer = 10;
        spellObject.transform.parent = SpellManager.SpellManagerTransform;

        return spellObject;
    }


    private static void AssignUniqueSpellName(string originalSpellName, GameObject spellObject)
    {
        spellObject.name = OurGameManager.AddProjectileNumberToSpell(originalSpellName);
        OurGameManager.IncrementProjectileCount();
    }


    public static GameObject CreateCollisionParticlesInWorld(string spellName, Vector3 position, SpellStats spellStats, SpellIdentifier spellIdentifierOfOriginalSpell)
    {
        if (spellIdentifierOfOriginalSpell == null)
        {
            Debug.Log("SpellCreation(CreateCollisionParticles): spellIdentifer is null");
            return null;
        }

        string spellNameToTransfer = spellIdentifierOfOriginalSpell.SpellName;
        int shotByToTransfer = spellIdentifierOfOriginalSpell.ShotByID;
        string shotByNameToTransfer = spellIdentifierOfOriginalSpell.ShotByName;

        GameObject collisionParticles = GameObject.Instantiate(spellStats.collisionParticle, position, Quaternion.identity);
        collisionParticles.AddComponent(SpellDictionary.GetTypeFromSpellName(SpellManager.GetOriginalSpellName(spellName)));
        SpellIdentifier.AddSpellIdentifier(collisionParticles, spellNameToTransfer, shotByNameToTransfer, shotByToTransfer);

        collisionParticles.transform.parent = SpellManager.SpellManagerTransform;

        return collisionParticles;
    }

}
