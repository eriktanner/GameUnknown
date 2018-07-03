using UnityEngine;

public static class EffectDamage {


    public static void DamageEffect(GameObject caster, GameObject target, AbilityData abilityData)
    {
        if (target.tag == "Player")
        {
            if (((IDamage) abilityData) == null)
                return;

            Damage damage = new Damage(abilityData, target, PlayerManager.GetPhotonPlayerFromGameObject(caster).ID);
            damage.ApplyDamage();
        }
        
    }

}
