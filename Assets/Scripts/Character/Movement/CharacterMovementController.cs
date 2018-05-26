using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour {
    
    const float FORWARD_TO_BACKWARD_RATIO = 0.5f;

    public float inputDelay = 0.1f;
    public float forwardVelocity = 6.0f, rotateVelocity = 2.2f;
    public float leftRightVelocity = 4.0f, jumpVelocity = 50.0f;

    Rigidbody rigidBody;
    CapsuleCollider charCollider;
    float currentX = 0.0f;
    float forwardInput, leftRightInput;
    float sensitivity = 1.0f; //Get From Settings
    bool mouseDown, jump;


    void Start ()
    {
        if (GetComponent<Rigidbody>())
            rigidBody = GetComponent<Rigidbody>();
        else Debug.LogError("Character needs a rigid body");

        if (GetComponent<Collider>())
            charCollider = GetComponent<CapsuleCollider>();
        else Debug.LogError("Character needs a collider");

        forwardInput = leftRightInput = 0;
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        leftRightInput = Input.GetAxis("Horizontal");
        mouseDown = Input.GetButton("Fire1");
        jump = Input.GetButtonDown("Jump");
        currentX += Input.GetAxis("Mouse X");
    }


    void Update () {
        GetInput();
    }
    
    void FixedUpdate()
    {
        MoveForward();
        MoveLeftRight();
        Turn();
        Jump();
    }

    void MoveForward()
    {
        float forwardInputToUse = isGrounded() ? forwardInput : forwardInputOnLastJump;
        
        if (Mathf.Abs(forwardInputToUse) > inputDelay)
        {
            Vector3 transformForwardToUse = isGrounded() ? transform.forward : transformForwardOnLastJump;
            float forwardVelocityToUse = forwardInputToUse > 0 ? forwardVelocity : forwardVelocity * FORWARD_TO_BACKWARD_RATIO; 
            rigidBody.MovePosition(rigidBody.position + transformForwardToUse * forwardVelocityToUse * forwardInputToUse * Time.fixedDeltaTime);
            //rigidBody.AddForce(transformToUse.forward * forwardVelocity * forwardInputToUse * .01f, ForceMode.VelocityChange);
        } 
    }

    void MoveLeftRight()
    {
        float leftRightInputToUse = isGrounded() ? leftRightInput : leftRightInputOnLastJump;

        if (Mathf.Abs(leftRightInputToUse) > inputDelay)
        {
            Vector3 transformRightToUse = isGrounded() ? transform.right : transformRightOnLastJump;
            rigidBody.MovePosition(rigidBody.position + transformRightToUse * leftRightVelocity * leftRightInputToUse * Time.fixedDeltaTime);
            //rigidBody.AddForce(transform.right * leftRightVelocity * turnInput * .01f, ForceMode.VelocityChange);
        }
    }

    void Turn()
    {
        Quaternion mouseRotation = Quaternion.Euler(0, currentX * sensitivity * 2.0f, 0.0f);
        rigidBody.MoveRotation(mouseRotation);
    }


    //Jump Player Transform Attributes
    float forwardInputOnLastJump = 0, leftRightInputOnLastJump = 0;
    Vector3 transformForwardOnLastJump = Vector3.zero, transformRightOnLastJump = Vector3.zero;

    void Jump()
    {
        if (jump && isGrounded())
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0 , rigidBody.velocity.z);
            rigidBody.AddForce(transform.up * jumpVelocity, ForceMode.Impulse);

            //Records player transform on jump
            forwardInputOnLastJump = forwardInput;
            transformForwardOnLastJump = transform.forward;
            leftRightInputOnLastJump = leftRightInput;
            transformRightOnLastJump = transform.right;
        }
    }

    //Works okay but is not perfect, okay for now
    bool isGrounded()
    {
        //get the radius of the players capsule collider, and make it a tiny bit smaller than that
        float radius = charCollider.radius * 0.9f;
        print("myradius - " + radius);
        //get the position (assuming its right at the bottom) and move it up by almost the whole radius
        Vector3 pos = transform.position - Vector3.up * .185f;// + Vector3.up * (radius * 0.9f);8
        print("x: " + transform.position.x + " y: " + transform.position.y);
        //returns true if the sphere touches something on that layer
        bool isGrounded = Physics.CheckSphere(pos, radius);

        return isGrounded;
    }

}
