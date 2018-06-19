using UnityEngine;
using System.Collections;

/*Fear is a shadow spell that must be casted on the ground. It has an area of effect
 a few seconds after it is casted, fearing enemies within a range causing them to lose
 control over their input, replaced with fear input*/
public class FearEffect : MonoBehaviour {

    const float FEAR_STEP_TIME = 1.0f;
    const float FEAR_PAUSE_TIME = .5f;


    Animator animator;
    vThirdPersonInput playerMovement;
    CastSpell playerCastSpell;

    public FearEffect(GameObject playerObject)
    { 
        //animator = playerMovement.animator;
        playerMovement = playerObject.GetComponent<vThirdPersonInput>();
        playerCastSpell  = playerObject.GetComponent<CastSpell>();
    }



    /*Start of actual fear effect*/
    public void initFearSequence()
    {
        playerMovement.PlayerHasControl(false);
        playerCastSpell.SetSpellLock(true);

        SpellEffects.Instance.StartCoroutine(Fear()); 

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
        playerMovement.SetForwardInput(Random.Range(-1, 1));
        playerMovement.SetLeftRightInput(Random.Range(-1, 1));
    }
    
    void PauseStep()
    {
        playerMovement.StopMovement();
        playerMovement.LockMovement(true);
    }
}
