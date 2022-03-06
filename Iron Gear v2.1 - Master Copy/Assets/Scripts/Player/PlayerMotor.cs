using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour
{
	public bool canJump;
	public bool isJumping;
	public bool isDoubleJumping;
	//	public bool isAirDashing;

	private PlayerPhysics playerPhysics;
	private PlayerAnimation playerAnimation;
	private MeleeSystem meleeSystem;
	private SteamChangeAbility steamChangeAbility;
	private PlayerAudio playerAudio;
	private bool hasControl;

	public void GiveControl() { hasControl = true; }
	public void RemoveControl() { hasControl = false; }
	public bool HasControl() { return hasControl; }

	void Awake()
	{
		playerAudio = GetComponent<PlayerAudio>();
		playerAnimation = GetComponent<PlayerAnimation>();
		playerPhysics = GetComponent<PlayerPhysics>();
		meleeSystem = GetComponent<MeleeSystem>();
		steamChangeAbility = GetComponent<SteamChangeAbility>();
	}

	// Use this for initialization
	void Start ()
	{
		hasControl = true;
	}

	public void UpdateMotor ()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);

		if (hasControl)
		{
			//set the buttons that control the steam color ability
			//steamChangeAbility.SteamColorChangePower(Input.GetAxis("RightAnalogHorizontal"), Input.GetAxis("RightAnalogVertical"));

			//we are not currently in a jump so allow jumping
			if (playerPhysics.IsGrounded() && (Input.GetButtonDown("Jump") || Input.GetButtonDown("X")) && !isJumping && !playerPhysics.isCrouching && !playerPhysics.canSlideFromCrouch)
			{
				canJump = true;
				//we are still grounded and trying to do an attack so allow it
				if (meleeSystem.isGroundAttacking)
				{
					meleeSystem.canAttack = true;
				}
			}
			//we havent released the jump button so don't allow jumping
			if (playerPhysics.IsGrounded() && isJumping)
			{
				canJump = false;
			}
			//we are in the air
			if (!playerPhysics.IsGrounded())
			{
				isJumping = true;
				//perform air dash
				if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown ("R1")) && playerPhysics.doubleJumpLimit == 0)
				{
					//					isAirDashing = true;
					playerPhysics.StartAirDash();
				}
			}
			//we are grounded
			else
			{
				//reset jumping
				if ((Input.GetButtonUp("Jump") || !Input.GetButton("Jump")) || (Input.GetButtonUp("X") || !Input.GetButton("X")))
				{
					isJumping = false;
				}
			}
			//we are able to jump
			if ((Input.GetButton("Jump") || Input.GetButton("X")) && canJump && !playerPhysics.isCrouching && meleeSystem.canAttack && !playerPhysics.canTacticalSlide && !playerPhysics.canSlideFromCrouch)
			{
				//perform jump
				playerPhysics.Jump();
			}
			//not grounded so perform double jump
			if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("X")) && !playerPhysics.IsGrounded() && !playerPhysics.isCrouching && meleeSystem.canAttack && !meleeSystem.hasAttacked)
			{
				isDoubleJumping = true;
				//				isHighJumping = false;
				playerPhysics.DoubleJump();
				//playerAnimation.state = PlayerAnimation.animCharacterStates.doubleJumping;
			}
			//released jump button to be out of jump
			if (Input.GetButtonUp("Jump") || Input.GetButtonUp("X"))
			{
				//				isHighJumping = false;
				playerPhysics.IsInJump = false;  
			}
		}
	}

	public void FixedUpdateMotor()
	{
		if (hasControl)
		{
			playerPhysics.SlideFromCrouch(Input.GetButtonDown("Jump"), Input.GetButtonDown("X"));
			//pressing left or right
			playerPhysics.GetMotion(Input.GetAxis("Horizontal"));
			//pressing down for slide
			playerPhysics.TacticalSlide(Input.GetAxisRaw("Vertical"));
		}
	}
}