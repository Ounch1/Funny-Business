using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public StunController stunController;

    public float runSpeed;
    float horizontalMove = 0f;
    bool jump = false;

    // Particle Systems
    public ParticleSystem runningDust;
    public ParticleSystem jumpingDust;

    // Sounds
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource fallSound;
    
    private void FixedUpdate()
    {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
            jump = false;
    }

    void Update()
    {
        if (stunController.Stun() == false)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            Debug.Log(horizontalMove);
        }
        else
        {
            horizontalMove = 0;
        }
    }

    void Jump()
    {
            jump = true;
            animator.SetBool("IsJumping", true); 
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);  
        fallSound.Play();
        EmitJumpingDust();
    }

   

    //Dust emition
    private void EmitRunningDust()
    {
        runningDust.Play();
    }

    private void EmitJumpingDust()
    {
        jumpingDust.Play();
    }

    //SFX
    private void PlayWalkSFX()
    {
        walkSound.Play();
    }

    private void PlayJumpSFX()
    {
        jumpSound.Play();
    }

}