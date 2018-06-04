using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

/*Fears a player object, takes control of their CharacterMovementController*/
public class FearSpellEffect {

    GameObject player;
    CharacterMovementController playerMovement;
    Animator animator;
    CastSpell playerCastSpell;

    public FearSpellEffect(GameObject playerObject)
    {
        player = playerObject;
        playerMovement = playerObject.GetComponent<CharacterMovementController>();
        animator = playerMovement.animator;
        playerCastSpell  = playerObject.GetComponent<CastSpell>();
    }



    /*Start of actual fear effect*/
    public void initFearSequence()
    {
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
        playerMovement.playerHasControl(false);
        float turnTo = (float)myRandom.NextDouble() * 120;
        turnTo = Mathf.Clamp(turnTo, 45, 120);

        float randomDirection = (float)myRandom.NextDouble();
        randomDirection = (randomDirection <= .5) ? 1 : -1;

        for (int i = 0; i < 200; i++)
        {
            playerMovement.currentX += turnTo * randomDirection / 200;
            Thread.Sleep(1);
        }



        float newForward = (float)myRandom.NextDouble();
        playerMovement.forwardInput = Mathf.Clamp(newForward, .55f, .85f);


        Thread.Sleep(1000);
        playerMovement.forwardInput = 0;
        Thread.Sleep(600);
    }

}
