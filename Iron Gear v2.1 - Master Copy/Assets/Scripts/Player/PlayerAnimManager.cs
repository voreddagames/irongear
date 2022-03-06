using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
	public static float DirectionalInput;

	public GameObject playerAnim;

	private GameObject player;

	private PlayerPhysics playerPhysics;

	private Animator anim;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag("Player");
		playerPhysics = player.GetComponent<PlayerPhysics>();
		anim = playerAnim.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (MeleeSystem.instance.canAttack)
		{
			anim.enabled = true;

			UpdateGrounded();
			UpdateTacticalSlide();
			UpdateRunning();
			UpdateJumpingAnim();
			UpdateCrouching();
			UpdateHitAbove();
		}
		else
		{
			anim.enabled = false;
		}
	}

	#region Bool Parameters
	void UpdateGrounded()
	{
		anim.SetBool("isGrounded", playerPhysics.IsGrounded());
	}

	void UpdateTacticalSlide()
	{
		anim.SetBool("isSliding", playerPhysics.canTacticalSlide);
	}

	void UpdateCrouching()
	{
		anim.SetBool("isCrouching", playerPhysics.isCrouching);
	}

	void UpdateRunning()
	{
		if (DirectionalInput != 0)
		{
			anim.SetBool("isRunning", true);
		}
		else
		{
			anim.SetBool("isRunning", false);
		}
	}

	void UpdateJumpingAnim()
	{
		anim.SetBool("isJumping", playerPhysics.IsInJump);
	}

	void UpdateHitAbove()
	{
		anim.SetBool("didHitAbove", playerPhysics.didCollideAbove);
	}
	#endregion

	#region Trigger Parameters
	public void UpdateDoubleJumpingAnim()
	{
		anim.SetTrigger("isDoubleJumping");
	}

	public void UpdateAirDashingAnim()
	{
		anim.SetTrigger("isAirDashing");
	}
	#endregion
}
