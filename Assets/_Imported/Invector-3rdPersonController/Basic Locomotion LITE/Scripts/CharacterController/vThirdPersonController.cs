using UnityEngine;
using System.Collections;


public class vThirdPersonController : vThirdPersonAnimator
{
    protected virtual void Start()
    {

        if (!photonView.isMine)
        {
            enabled = false;
            return;
        }
#if !UNITY_EDITOR
                Cursor.visible = false;
#endif
    }

    public virtual void Sprint(bool value) {
        isSprinting = value;
    }

    public virtual void Strafe() {
        if (locomotionType == LocomotionType.OnlyFree) return;
        isStrafing = !isStrafing;
    }

    public virtual void Jump() {
        // conditions to do this action
        bool jumpConditions = isGrounded && !isJumping && !isRolling;
        // return if jumpCondigions is false
        if (!jumpConditions) return;
        // trigger jump behaviour
        jumpCounter = jumpTimer;
        isJumping = true;
        // trigger jump animations            
        if (_rigidbody.velocity.magnitude < 1)
            animator.CrossFadeInFixedTime("Jump", 0.1f);
        else
            animator.CrossFadeInFixedTime("JumpMove", 0.2f);
    }

    public virtual void Roll() {
        if (animator.IsInTransition(0)) return;

        bool actionsRoll = !actions || (actions);
        // general conditions to roll
        //bool rollConditions = (input != Vector2.zero || speed > 0.25f) && actionsRoll && isGrounded && !isJumping;
        bool rollConditions = actionsRoll && isGrounded && !isJumping;


        if (!rollConditions || isRolling) return;

        //animator.SetTrigger("ResetState");
        animator.CrossFadeInFixedTime("Roll", 0.1f);
    }


    public virtual void RotateWithAnotherTransform(Transform referenceTransform) {
        var newRotation = new Vector3(transform.eulerAngles.x, referenceTransform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), strafeRotationSpeed * Time.fixedDeltaTime);
        targetRotation = transform.rotation;
    }
}
