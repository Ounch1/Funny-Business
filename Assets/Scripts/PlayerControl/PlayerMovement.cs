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

    // Player Variables
    private float originalGravity;

    // Dashing Variables
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashDuration = 1f;
    private float dashTime = 0f;

    // Particle Systems
    public ParticleSystem runningDust;
    public ParticleSystem jumpingDust;

    // RigidBody
    [SerializeField] private Rigidbody2D rigidBody;

    // Sounds
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource fallSound;
    
    private void FixedUpdate()
    {
        // Normal movement when not dashing
        if (isDashing() == false) {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        } 
        else // When dashing, move the player based on the direction they're facing
        {
            if (controller.m_FacingRight) 
            {
                rigidBody.velocity = new Vector3(dashDistance, 0, 0);
            } 
            else 
            {
                rigidBody.velocity = new Vector3(-dashDistance, 0, 0);
            }
        }
        jump = false;
    }

    void Update()
    {
        // Checks if the player is stunned before they can move
        if (stunController.Stun() == false)
        {
            animator.SetBool("IsStunned", false);

            // Dash with left shift

            if (Input.GetKeyDown(KeyCode.LeftShift) && (Time.time - dashTime > dashCooldown || dashTime == 0))
            {
                Dash();
            }

            // If the player is not dashing, they can move normally; otherwise, the dash operates as stated in the FixedUpdated
            if(isDashing() == false) 
            {
                horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

                animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            }
            
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            horizontalMove = 0;

            animator.SetFloat("Speed", 0);

            animator.SetBool("IsStunned", true);
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

    // Sets the originalGravity to what it was before the dash
    void Dash() 
    {
        originalGravity = rigidBody.gravityScale;
        dashTime = Time.time;
    }

    // During the dash, gravity is set to 0. After the dash ends, it's set back to the originalGravity value
    public bool isDashing() {
        if (Time.time - dashTime >= dashDuration && dashTime != 0) {
            rigidBody.gravityScale = originalGravity;
            return false;
        }
        else if (dashTime != 0) 
        {
            rigidBody.gravityScale = 0;
            return true;
        }
        else 
        {
            return false;
        }
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