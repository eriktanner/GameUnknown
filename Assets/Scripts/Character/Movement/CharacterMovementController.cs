using UnityEngine;
using System;


[RequireComponent((typeof(Rigidbody)))]
[RequireComponent((typeof(CapsuleCollider)))]
public class CharacterMovementController : MonoBehaviour {

    const float FORWARD_TO_BACKWARD_RATIO = 0.5f;

    public float inputDelay = 0.1f;
    public float forwardVelocity = 5.0f, rotateVelocity = 2.2f;
    public float leftRightVelocity = 4.0f, jumpVelocity = 4.0f;
    public float dashVelocity = 8.0f, dashCooldownTime = 12.0f;


    float lockedForwardInput = 0, lockedLeftRightInput = 0;
    Vector3 transformForwardOnLastLock = Vector3.zero, transformRightOnLastLock = Vector3.zero;

    Rigidbody rigidBody;
    CapsuleCollider charCollider;
    Animator animator;
    float currentX = 0.0f;
    float forwardInput, leftRightInput;
    float sensitivity = 1.0f; //Get From Settings
    bool mouseHold, mouseDown, rightMouseDown, jump, dash;

    bool playerIsInControl = true;

    //Func<float, float> controlInput;



    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        charCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        forwardInput = leftRightInput = 0;
    }

    void GetInput()
    {
        if (playerIsInControl)
        {
            forwardInput = Input.GetAxis("Vertical");
            leftRightInput = Input.GetAxis("Horizontal");
            mouseDown = Input.GetButtonDown("Fire1");
            mouseHold = Input.GetButton("Fire1");
            rightMouseDown = Input.GetButtonDown("Fire2");
            jump = Input.GetButtonDown("Jump");
            dash = Input.GetButtonDown("Dash");
            currentX += Input.GetAxis("Mouse X");
        }
        else
        {

        }

    }


    void Update() {
        GetInput();
    }

    void FixedUpdate()
    {
        if (!playerIsInControl)
            return;

        MoveForward();
        MoveLeftRight();
        Turn();
        Jump();
        Dash();
    }



    void MoveForward()
    {
        float forwardInputToUse = isGrounded() ? forwardInput : lockedForwardInput;
        if (animator) animator.SetFloat("InputZ", forwardInputToUse);

        if (Mathf.Abs(forwardInput) > inputDelay)
        {
            Vector3 transformForwardToUse = isGrounded() ? transform.forward : transformForwardOnLastLock;
            float forwardVelocityToUse = forwardInputToUse > 0 ? forwardVelocity : forwardVelocity * FORWARD_TO_BACKWARD_RATIO;
            rigidBody.MovePosition(rigidBody.position + transform.forward * forwardVelocityToUse * forwardInputToUse * Time.fixedDeltaTime);
        }
    }

    void MoveLeftRight()
    {
        float leftRightInputToUse = isGrounded() ? leftRightInput : lockedLeftRightInput;
        if (animator) animator.SetFloat("InputX", leftRightInputToUse);

        if (Mathf.Abs(leftRightInput) > inputDelay)
        {
            Vector3 transformRightToUse = isGrounded() ? transform.right : transformRightOnLastLock;
            rigidBody.MovePosition(rigidBody.position + transform.right * leftRightVelocity * leftRightInputToUse * Time.fixedDeltaTime);
        }
    }

    void Turn()
    {
        Quaternion mouseRotation = Quaternion.Euler(0, currentX * sensitivity * 2.0f, 0.0f);
        rigidBody.MoveRotation(mouseRotation);
    }



    void Jump()
    {
        if (jump && isGrounded())
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
            rigidBody.AddForce(transform.up * jumpVelocity, ForceMode.Impulse);
            if (animator) animator.Play("Jump");

            lockedForwardInput = forwardInput;
            lockedLeftRightInput = leftRightInput;
            transformForwardOnLastLock = transform.forward;
            transformRightOnLastLock = transform.right;

        }
    }



    /*Tells us if player is currently on a hard surface - THIS NEEDS TO BE FIXED
     This messes up escpecially on the client I don't know why*/
    bool isGrounded()
    {
        float radius = charCollider.radius * .4f;
        Vector3 pos = transform.position + Vector3.up * (radius * 0.2f);
        LayerMask ignorePlayerMask = ~(1 << 8);
        return Physics.CheckSphere(pos, radius, ignorePlayerMask);
    }

    float dashNextAllowedTimeStamp = -100;
    void Dash()
    {
        if (dash && dashNextAllowedTimeStamp <= Time.time)
        {
            dashNextAllowedTimeStamp = Time.time + dashCooldownTime;
            rigidBody.velocity = new Vector3(rigidBody.velocity.x / 2, rigidBody.velocity.y, rigidBody.velocity.z / 2);
            rigidBody.AddForce(transform.forward * dashVelocity, ForceMode.Impulse);
        }
    }

    /*Takes away or grants control over movement to local player*/
    public void playerHasControl(bool hasControl)
    {

    }

}
