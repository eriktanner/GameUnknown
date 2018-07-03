using UnityEngine;

public static class InterfaceToEffects {

	public static void ProcessEffects(GameObject caster, GameObject hitPlayer, AbilityData abilityData)
    {
        if (!GameSettings.SelfFire && caster == hitPlayer)
            return;

        IHaveHealth iHaveHealth = hitPlayer.GetComponent<IHaveHealth>();
        if (iHaveHealth == null)
            return;

        IDamage iDamage = abilityData as IDamage;
        if (iDamage != null)
            EffectDamage.DamageEffect(caster, hitPlayer, abilityData);

        IStun iStun = abilityData as IStun;
        if (iDamage != null)
            EffectStun.StunEffect(hitPlayer, abilityData);
    }

}
