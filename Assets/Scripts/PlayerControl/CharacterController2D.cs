using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] CapsuleCollider2D capsuleCollider2d;                       // Collider from which BoxCast will be coming from


	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private float airTime; // How much time player spends in air
	private Vector3 m_Velocity = Vector3.zero;


	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;


	//Attack box controller
	[SerializeField] private BoxCollider2D damageBox;
	[SerializeField] private SpriteRenderer boxRenderer;
	private float attackDuration;
	public float attackCoolDown;
	private float attackCoolDownTimer;

	//Events
    [Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		damageBox.enabled = false;
		boxRenderer.enabled = false;

		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
    {
		ToggleDamageBox();

		CheckLanding();
		CheckAirTime();

		//Falling check
		if (IsGrounded() == false)
		{ 
			if (m_Rigidbody2D.velocity.y < -1)
			{
				animator.SetBool("IsFalling", true);
			}
		}
		else
        {
			animator.SetBool("IsFalling", false);
		}
	}

	// IsGrounded() Casts rays to check if player is grounded
	private bool IsGrounded()
	{

		float extraHeightText = 0.01f;
		RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider2d.bounds.center, capsuleCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, m_WhatIsGround);
		Color rayColor;
		if (raycastHit.collider != null)
		{
			rayColor = Color.green;
		}
		else
		{
			rayColor = Color.red;
		}

		Debug.DrawRay(capsuleCollider2d.bounds.center + new Vector3(capsuleCollider2d.bounds.extents.x, 0), Vector2.down * (capsuleCollider2d.bounds.extents.y + extraHeightText), rayColor);
		Debug.DrawRay(capsuleCollider2d.bounds.center - new Vector3(capsuleCollider2d.bounds.extents.x, 0), Vector2.down * (capsuleCollider2d.bounds.extents.y + extraHeightText), rayColor);
		Debug.DrawRay(capsuleCollider2d.bounds.center - new Vector3(capsuleCollider2d.bounds.extents.x, capsuleCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (capsuleCollider2d.bounds.extents.y + extraHeightText), rayColor);

		//Debug.Log(raycastHit.collider); //Debug collision tracker (Remove slashes to enable)
		return raycastHit.collider != null;

	}

	// CheckAirTime() Calculates time spent in air and resets airTime timer in case if player hits the ground
	private void CheckAirTime()
    {
		if (IsGrounded())
		{
			airTime = 0f;
		}
		else
		{
			airTime += Time.deltaTime;
		}
	}
	// Activates OnLandEvent in case if IsGrounded is true and airTime is more than 0
	public void CheckLanding()
	{
		if (airTime > 0)
		{
			if (IsGrounded())
			{
				OnLandEvent.Invoke(); 
			}
		}
	}
	// Stores movement of the player
	public void Move(float move, bool jump)
	{

        
			if (IsGrounded() || m_AirControl)
			{

				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}

			}
			// If the player should jump...
			if (IsGrounded() && jump == true)
			{
				// Add a vertical force to the player.
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			}
		
	}
	// Flips the player
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	// Toggles the damage box
	private void ToggleDamageBox()
    {
		if (damageBox.enabled == true)
        {
			attackDuration += Time.deltaTime;
	    }

		if (attackCoolDownTimer >= 0)
        {
			attackCoolDownTimer -= Time.deltaTime;
        }

		if (damageBox.enabled == false)
        {
			if (attackCoolDownTimer <= 0f)
			{
				if (Input.GetKey(KeyCode.Z))
				{				
					damageBox.enabled = true;
					boxRenderer.enabled = true;
					attackCoolDownTimer = attackCoolDown;
				}
			}
			
		}

		if (attackDuration >= 0.1f)
        {
			damageBox.enabled = false;
			boxRenderer.enabled = false;

			attackDuration = 0;
		}

	}
}
