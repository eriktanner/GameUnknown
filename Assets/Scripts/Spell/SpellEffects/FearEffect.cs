using UnityEngine;
using System.Collections;
using System;

/*Fear is a shadow spell that must be casted on the ground. It has an area of effect
 a few seconds after it is casted, fearing enemies within a range causing them to lose
 control over their input, replaced with fear input*/
public class FearEffect : SpellEffect {

    public override System.Type SpellType { get { return Type.GetType(typeof(Fear).Name); } }

    const float FEAR_STEP_TIME = 1.0f;
    const float FEAR_PAUSE_TIME = .5f;


    Animator animator;
    vThirdPersonInput playerMovement;
    CastSpell playerCastSpell;

    public FearEffect() {}

    public override void SetupEffect(GameObject playerHit, GameObject particles)
    {
        playerMovement = playerHit.GetComponent<vThirdPersonInput>();
        playerCastSpell = playerHit.GetComponent<CastSpell>();
    }

   

    /*Start of actual fear effect*/
    public override void ProcessEffect(GameObject playerHit, GameObject particles)
    {
        SetupEffect(playerHit, particles);

        playerMovement.PlayerHasControl(false);
        playerCastSpell.SetSpellLock(true);


        TaskManager.CreateTask(Fear());

    }


    IEnumerator Fear()
    {
        FearStep();
        yield return new WaitForSeconds(FEAR_STEP_TIME);
        PauseStep();
        yield return new WaitForSeconds(FEAR_PAUSE_TIME);
        FearStep();
        yield return new WaitForSeconds(FEAR_STEP_TIME);
        PauseStep();
        yield return new WaitForSeconds(FEAR_PAUSE_TIME);
        FearStep();
        yield return new WaitForSeconds(FEAR_STEP_TIME);
        PauseStep();
        yield return new WaitForSeconds(FEAR_PAUSE_TIME);


        playerMovement.PlayerHasControl(true);
        playerMovement.LockMovement(false);
        playerCastSpell.SetSpellLock(false);
    }
    
    void FearStep()
    {
        playerMovement.LockMovement(false);
        playerMovement.SetForwardInput(UnityEngine.Random.Range(-1, 1));
        playerMovement.SetLeftRightInput(UnityEngine.Random.Range(-1, 1));
    }
    
    void PauseStep()
    {
        playerMovement.StopMovement();
        playerMovement.LockMovement(true);
    }
}
