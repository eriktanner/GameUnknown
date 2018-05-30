using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimator : MonoBehaviour {

    Animator m_Animator;
    float horizontalMovement, verticalMovement;

    void Start()
    {
        //Get the animator, which you attach to the GameObject you are intending to animate.
        m_Animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        //Translate the left and right button presses or the horizontal joystick movements to a float
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        //Sends the value from the horizontal axis input to the animator. Change the settings in the
        //Animator to define when the character is walking or running
        
    }

    void LateUpdate()
    {
        //m_Animator.SetFloat("InputX", horizontalMovement);
        //m_Animator.SetFloat("InputZ", verticalMovement);
    }
}
