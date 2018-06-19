using UnityEngine;
using System.Collections;

/*Soul void is a shadow spell that is casted on ground and explodes a few
 seconds after it is lands. It damages and stuns players within radius*/
public class SoulVoidEffect {

    float STUN_SECONDS = 2.5f;

    int ShotBy;
    float spellDamage;
    Animator animator;
    GameObject PlayerHit;
    vThirdPersonInput playerMovement;
    CastSpell playerCastSpell;
    SpellDamageApplier damageApplier;


    public SoulVoidEffect(GameObject playerObject, int shotBy)
    {
        //animator = playerMovement.animator;
        PlayerHit = playerObject;
        ShotBy = shotBy;
        playerMovement = playerObject.GetComponent<vThirdPersonInput>();
        spellDamage = SpellManager.GetSpellStatsFromName("Soul Void").damage;
        playerCastSpell = playerObject.GetComponent<CastSpell>();
        damageApplier = SpellDamageApplier.Instance;
    }



    /*Damages and stuns players*/
    public void initSoulVoidSequence()
    {
        //animator.Play("Fear");

        damagePlayer();

        SpellEffects.Instance.StartCoroutine(SoulVoid());

    }


    IEnumerator SoulVoid()
    {
        playerMovement.StopMovement();
        playerMovement.LockMovement(true);
        playerCastSpell.SetSpellLock(true);
        yield return new WaitForSeconds(STUN_SECONDS);
        playerMovement.LockMovement(false);
        playerMovement.PlayerHasControl(true);
        playerCastSpell.SetSpellLock(false);
    }

    void damagePlayer()
    {
        damageApplier.ApplyDamageFromTo(PlayerHit, spellDamage, ShotBy);
    }

    

}
