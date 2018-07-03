using UnityEngine;

public static class InterfaceToEffects {

	public static void ProcessInitialEffects(GameObject caster, GameObject hitPlayer, AbilityData abilityData)
    {
        if (!GameSettings.SelfFire && caster == hitPlayer)
            return;

        IHaveHealth iHaveHealth = hitPlayer.GetComponent<IHaveHealth>();
        if (iHaveHealth == null)
            return;

        IDamage iDamage = abilityData as IDamage;
        if (iDamage != null)
            EffectDamage.DamageEffect(caster, hitPlayer, iDamage.Damage);

        IStun iStun = abilityData as IStun;
        if (iDamage != null)
            EffectStun.StunEffect(hitPlayer, abilityData);



    }



    public static void ProcessOverTimeEffects(GameObject caster, GameObject hitPlayer, AbilityData abilityData)
    {
        Debug.Log("ProcessOverTime: ");
        if (!GameSettings.SelfFire && caster == hitPlayer)
            return;

        IHaveHealth iHaveHealth = hitPlayer.GetComponent<IHaveHealth>();
        if (iHaveHealth == null)
            return;

        ITick iTick = abilityData as ITick;
        if (iTick == null)
            return;


        IDOT iDOT = abilityData as IDOT;
        if (iDOT != null)
            EffectDamage.DamageEffect(caster, hitPlayer, iDOT.TotalDOT / iTick.NumTicks);

    }

}
