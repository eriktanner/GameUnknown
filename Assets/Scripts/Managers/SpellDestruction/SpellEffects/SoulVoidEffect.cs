using UnityEngine;
using System.Threading;

/*Soul void is a shadow spell that is casted on ground and explodes a few
 seconds after it is lands. It damages and stuns players within radius*/
public class SoulVoidEffect : MonoBehaviour {

    int STUN_MILLISECONDS = 2500;

    Spell spell;
    GameObject player;
    CharacterMovementController playerMovement;
    Animator animator;
    CastSpell playerCastSpell;
    GameObject localPlayer;


    public SoulVoidEffect(GameObject playerObject)
    {
        player = playerObject;
        playerMovement = playerObject.GetComponent<CharacterMovementController>();
        animator = playerMovement.animator;
        playerCastSpell = playerObject.GetComponent<CastSpell>();
        spell = SpellManager.getSpellFromName("Soul Void");
        localPlayer = GameObject.Find("Managers/NetworkManager").GetComponent<OurNetworkManager>().client.connection.playerControllers[0].gameObject;
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
        HealthBar healthBar = localPlayer.gameObject.GetComponent<HealthBar>();
        healthBar.CmdCollisionDamagePlayer(spell.damage, player.name);
    }

    void voidStun()
    {
        playerMovement.forwardInput = 0;
        playerMovement.leftRightInput = 0;
        Thread.Sleep(STUN_MILLISECONDS);
    }
    

}
