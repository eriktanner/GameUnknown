using System.Collections;
using UnityEngine;

public static class EffectStun {

    static vThirdPersonInput playerMovement;
    static CastSpell playerCastSpell;
    static float stunDuration;


    public static void StunEffect(GameObject target, AbilityData abilityData)
    {
        SetupEffect(target, abilityData);
        TaskManager.CreateTask(SoulVoid());
    }

    static void SetupEffect(GameObject target, AbilityData abilityData)
    {
        IStun iStun = abilityData as IStun;
        if (iStun == null)
            return;

        playerMovement = target.GetComponent<vThirdPersonInput>();
        playerCastSpell = target.GetComponent<CastSpell>();
        stunDuration = iStun.StunTime;
    }


    static IEnumerator SoulVoid()
    {
        playerMovement.StopMovement();
        playerMovement.LockMovement(true);
        playerCastSpell.SetSpellLock(true);
        yield return new WaitForSeconds(stunDuration);
        playerMovement.LockMovement(false);
        playerMovement.PlayerHasControl(true);
        playerCastSpell.SetSpellLock(false);
    }

}
