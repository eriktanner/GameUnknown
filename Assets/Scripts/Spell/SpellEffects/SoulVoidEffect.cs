using UnityEngine;
using System.Threading;

/*Soul void is a shadow spell that is casted on ground and explodes a few
 seconds after it is lands. It damages and stuns players within radius*/
public class SoulVoidEffect : MonoBehaviour {

    int STUN_MILLISECONDS = 2500;

    int ShotBy;
    SpellStats spell;
    GameObject PlayerHit;
    CharacterMovementController playerMovement;
    Animator animator;
    CastSpell playerCastSpell;
    SpellDamageApplier damageApplier;


    public SoulVoidEffect(GameObject playerObject, int shotBy)
    {
        PlayerHit = playerObject;
        ShotBy = shotBy;
        playerMovement = playerObject.GetComponent<CharacterMovementController>();
        animator = playerMovement.animator;
        playerCastSpell = playerObject.GetComponent<CastSpell>();
        spell = SpellManager.GetSpellStatsFromName("Soul Void");
        damageApplier = GameObject.Find("Spell").GetComponent<SpellDamageApplier>();
    }



    /*Damages and stuns players*/
    public void initSoulVoidSequence()
    {
        playerMovement.playerHasControl(false);
        playerCastSpell.setSpellLock(true);
        animator.Play("Fear");

        damagePlayer();

        new Thread(() =>
        {
            voidStun();

            playerMovement.playerHasControl(true);
            playerCastSpell.setSpellLock(false);

        }).Start();

    }


    void damagePlayer()
    {
        damageApplier.ApplyDamageFromTo(PlayerHit, spell.damage, ShotBy);
    }

    void voidStun()
    {
        playerMovement.forwardInput = 0;
        playerMovement.leftRightInput = 0;
        Thread.Sleep(STUN_MILLISECONDS);
    }
    

}
