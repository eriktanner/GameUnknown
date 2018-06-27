using UnityEngine;
using System.Collections;


public abstract class vThirdPersonAnimator : vThirdPersonMotor
{
    #region Variables        
    // match cursorObject to help animation to reach their cursorObject
    [HideInInspector]
    public Transform matchTarget;
    // head track control, if you want to turn off at some point, make it 0
    [HideInInspector]
    public float lookAtWeight;

    private float randomIdleCount, randomIdle;
    private Vector3 lookPosition;
    private float _speed = 0;
    private float _direction = 0;

    int baseLayer { get { return animator.GetLayerIndex("Base Layer"); } }
    int underBodyLayer { get { return animator.GetLayerIndex("UnderBody"); } }
    int rightArmLayer { get { return animator.GetLayerIndex("RightArm"); } }
    int leftArmLayer { get { return animator.GetLayerIndex("LeftArm"); } }
    int upperBodyLayer { get { return animator.GetLayerIndex("UpperBody"); } }
    int fullbodyLayer { get { return animator.GetLayerIndex("FullBody"); } }

    #endregion

    public virtual void UpdateAnimator()
    {
        if (animator == null || !animator.enabled) return;

        LayerControl();
        ActionsControl();

        // trigger by input
        RollAnimation();

        LocomotionAnimation();
    }

    public void LayerControl() {
        baseLayerInfo = animator.GetCurrentAnimatorStateInfo(baseLayer);
        underBodyInfo = animator.GetCurrentAnimatorStateInfo(underBodyLayer);
        rightArmInfo = animator.GetCurrentAnimatorStateInfo(rightArmLayer);
        leftArmInfo = animator.GetCurrentAnimatorStateInfo(leftArmLayer);
        upperBodyInfo = animator.GetCurrentAnimatorStateInfo(upperBodyLayer);
        fullBodyInfo = animator.GetCurrentAnimatorStateInfo(fullbodyLayer);
    }

    public void ActionsControl() {
        // to have better control of your actions, you can filter the animations state using bools 
        // this way you can know exactly what animation state the character is playing

        isRolling = baseLayerInfo.IsName("Roll");
        animator.SetBool("IsRolling", isRolling);
        Debug.Log("IsRolling: " + isRolling);
        inTurn = baseLayerInfo.IsName("TurnOnSpot");
    }

    #region Locomotion Animations

    public void LocomotionAnimation() {
        animator.SetBool("IsStrafing", isStrafing);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("GroundDistance", groundDistance);

        if (!isGrounded)
            animator.SetFloat("VerticalVelocity", verticalVelocity);

        if (isStrafing) {
            // strafe movement get the input 1 or -1
            animator.SetFloat("InputHorizontal", !lockMovement ? direction : 0f, 0.25f, Time.deltaTime);
        }

        animator.SetFloat("InputVertical", !lockMovement ? speed : 0f, 0.25f, Time.deltaTime);

        if (turnOnSpotAnim) {
            GetTurnOnSpotDirection(transform, Camera.main.transform, ref _speed, ref _direction, input);
            FreeTurnOnSpot(_direction * 180);
        }
    }

    public void FreeTurnOnSpot(float direction) {
        bool inTransition = animator.IsInTransition(0);
        float directionDampTime = inTurn || inTransition ? 1000000 : 0;
        animator.SetFloat("TurnOnSpotDirection", direction, directionDampTime, Time.deltaTime);
    }

    public void GetTurnOnSpotDirection(Transform root, Transform camera, ref float _speed, ref float _direction, Vector2 input) {
        Vector3 rootDirection = root.forward;
        Vector3 stickDirection = new Vector3(input.x, 0, input.y);

        // Get camera rotation.    
        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f; // kill Y
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);
        // Convert joystick input in Worldspace coordinates            
        Vector3 moveDirection = referentialShift * stickDirection;

        Vector2 speedVec = new Vector2(input.x, input.y);
        _speed = Mathf.Clamp(speedVec.magnitude, 0, 1);

        if (_speed > 0.01f) // dead zone
        {
            Vector3 axis = Vector3.Cross(rootDirection, moveDirection);
            _direction = Vector3.Angle(rootDirection, moveDirection) / 180.0f * (axis.y < 0 ? -1 : 1);
        } else {
            _direction = 0.0f;
        }
    }

    public void OnAnimatorMove() {
        if (!this.enabled) return;

        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (isGrounded) {
            transform.rotation = animator.rootRotation;

            var speedDir = new Vector2(direction, speed);
            var strafeSpeed = (isSprinting ? 1.5f : 1f) * Mathf.Clamp(speedDir.magnitude, 0f, 1f);
            // strafe extra speed
            if (isStrafing) {
                if (strafeSpeed <= 0.5f)
                    ControlSpeed(strafeWalkSpeed);
                else if (strafeSpeed > 0.5f && strafeSpeed <= 1f)
                    ControlSpeed(strafeRunningSpeed);
                else
                    ControlSpeed(strafeSprintSpeed);

     
            } else if (!isStrafing) {
                // free extra speed                
                if (speed <= 0.5f)
                    ControlSpeed(freeWalkSpeed);
                else if (speed > 0.5 && speed <= 1f)
                    ControlSpeed(freeRunningSpeed);
                else
                    ControlSpeed(freeSprintSpeed);

                
            }
        }
    }

    #endregion

    #region Action Animations  

    void RollAnimation() {
       
        if (isRolling) {

            if (isStrafing && (input != Vector2.zero || speed > 0.25f)) {
                // check the right direction for rolling if you are strafing
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, 25f * Time.fixedDeltaTime, 0.0f);
                var rot = Quaternion.LookRotation(newDir);
                var eulerAngles = new Vector3(transform.eulerAngles.x, rot.eulerAngles.y, transform.eulerAngles.z);
                transform.eulerAngles = eulerAngles;
            }

            if (baseLayerInfo.normalizedTime > 0.1f && baseLayerInfo.normalizedTime < 0.3f)
                _rigidbody.useGravity = false;

            // prevent the character to rolling up 
            if (verticalVelocity >= 1)
                _rigidbody.velocity = Vector3.ProjectOnPlane(_rigidbody.velocity, groundHit.normal);

            // reset the rigidbody a little ealier to the character fall while on air
            if (baseLayerInfo.normalizedTime > 0.3f)
                _rigidbody.useGravity = true;
        }
    }

    #endregion

    #region Trigger Animations       

    public void TriggerAnimationState(string animationClip, float transition) {
        animator.CrossFadeInFixedTime(animationClip, transition);
    }

    public bool IsAnimatorTag(string tag) {
        if (animator == null) return false;
        if (baseLayerInfo.IsTag(tag)) return true;
        if (underBodyInfo.IsTag(tag)) return true;
        if (rightArmInfo.IsTag(tag)) return true;
        if (leftArmInfo.IsTag(tag)) return true;
        if (upperBodyInfo.IsTag(tag)) return true;
        if (fullBodyInfo.IsTag(tag)) return true;
        return false;
    }

    #endregion

}
