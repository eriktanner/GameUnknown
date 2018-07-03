using UnityEngine;

public static class EffectDamage {


    public static void DamageEffect(GameObject caster, GameObject target, float baseDamage)
    {
        if (target.tag == "Player")
        {
            Damage damage = new Damage(baseDamage, target, PlayerManager.GetPhotonPlayerFromGameObject(caster).ID);
            damage.ApplyDamage();
        }
        
    }

}
