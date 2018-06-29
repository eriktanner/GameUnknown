using UnityEngine;
using System.Collections;
using System;

/*Soul void is a shadow spell that is casted on ground and explodes a few
 seconds after it is lands. It damages and stuns players within radius*/
public class SoulVoidEffect : SpellEffect {

    public override System.Type SpellType { get { return Type.GetType(typeof(SoulVoid).Name); } }


    float STUN_SECONDS = 2.5f;

    int ShotBy;
    float spellDamage;
    Animator animator;
    GameObject PlayerHit;
    vThirdPersonInput playerMovement;
    CastSpell playerCastSpell;
    NetworkDamageApplier damageApplier;

    public SoulVoidEffect() {}

    public override void SetupEffect(GameObject playerHit, GameObject particles)
    {
        //animator = playerMovement.animator;
        PlayerHit = playerHit;
        SpellIdentifier spellIdentifier = particles.GetComponent<SpellIdentifier>();

        if (spellIdentifier)
        {
            ShotBy = spellIdentifier.ShotByID;
        }

        playerMovement = playerHit.GetComponent<vThirdPersonInput>();
        spellDamage = SpellManager.GetSpellStatsFromName("Soul Void").damage;
        playerCastSpell = playerHit.GetComponent<CastSpell>();
        damageApplier = NetworkDamageApplier.Instance;
    }

    /*Damages and stuns players*/
    public override void ProcessEffect(GameObject playerHit, GameObject particles)
    {
        SetupEffect(playerHit, particles);
        //animator.Play("Fear");

        damagePlayer();
        TaskManager.CreateTask(SoulVoid());
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
