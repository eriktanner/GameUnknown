using UnityEngine;
using System.Threading;

/*Soul void is a shadow spell that is casted on ground and explodes a few
 seconds after it is lands. It damages and stuns players within radius*/
public class SoulVoidEffect : MonoBehaviour {

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
        localPlayer = GameObject.Find("Managers/NetworkManager").GetComponent<OurNetworkManager>().client.connection.playerControllers[0].gameObject;
    }



    /*Damages and stuns players*/
    public void initSoulVoidSequence()
    {
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
        healthBar.CmdCollisionDamagePlayer("Soul Void", player.name);
    }

    void voidStun()
    {
        playerMovement.playerHasControl(false);
        playerMovement.forwardInput = 0;
        playerMovement.leftRightInput = 0;
        Thread.Sleep(3000);
    }
    

}
