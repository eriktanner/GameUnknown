using UnityEngine;

public class AbilityCastSpell : AbilityProjectile {
    

	
    public AbilityCastSpell(AbilityData abilityData)
    {
        AbilityData = abilityData;
    }

    public override void CastAbility(GameObject player, Vector3 spawnSpot, Vector3 aimTowards)
    {
        Fire(AbilityData, spawnSpot, aimTowards);
    }
}
