using UnityEngine;
using System.Threading;

/*Fear is a shadow spell that must be casted on the ground. It has an area of effect
 a few seconds after it is casted, fearing enemies within a range causing them to lose
 control over their input, replaced with fear input*/
public class FearEffect {

    GameObject player;
    CharacterMovementController playerMovement;
    Animator animator;
    CastSpell playerCastSpell;

    public FearEffect(GameObject playerObject)
    {
        player = playerObject;
        playerMovement = playerObject.GetComponent<CharacterMovementController>();
        animator = playerMovement.animator;
        playerCastSpell  = playerObject.GetComponent<CastSpell>();
    }



    /*Start of actual fear effect*/
    public void initFearSequence()
    {
        playerMovement.playerHasControl(false);
        playerCastSpell.setSpellLock(true);

        animator.Play("Fear");
        new Thread(() =>
        {
            fearStep();
            fearStep();
            fearStep();

            playerMovement.playerHasControl(true);
            playerCastSpell.setSpellLock(false);

        }).Start();

    }


    void fearStep()
    {
        System.Random myRandom = new System.Random();
        float turnTo = (float) myRandom.NextDouble() * 120;
        turnTo = Mathf.Clamp(turnTo, 45, 120);

        float randomDirection = (float) myRandom.NextDouble();
        randomDirection = (randomDirection <= .5) ? 1 : -1;

        for (int i = 0; i < 200; i++)
        {
            playerMovement.currentX += turnTo * randomDirection / 200;
            Thread.Sleep(1);
        }



        float newForward = (float) myRandom.NextDouble();
        playerMovement.forwardInput = Mathf.Clamp(newForward, .55f, .85f);


        Thread.Sleep(1000);
        playerMovement.forwardInput = 0;
        Thread.Sleep(600);
    }

}
