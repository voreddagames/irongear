using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTest : MonoBehaviour
{
	private Animator anim;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		TempGroundCheck();

		Jump();
		DoubleJump();
		Running();
		Crouching();
		AirDash();
		TacticalSlide();

		//Falling();
		//Landing();
	}

	void Running()
	{
		float dirInput = Input.GetAxis("Horizontal");

		if (dirInput != 0.0f)
		{
			if (anim.GetBool("isGrounded") &&
				!anim.GetBool("isCrouching"))
			{
				anim.SetBool("isRunning", true);
			}
		}
		else
		{
			anim.SetBool("isRunning", false);
		}
	}

	void TacticalSlide()
	{
		if (Input.GetKey(KeyCode.S) &&
			anim.GetBool("isRunning"))
		{
			anim.SetBool("isSliding", true);
		}
		if (Input.GetKeyUp(KeyCode.S) ||
			!anim.GetBool("isRunning"))
		{
			anim.SetBool("isSliding", false);
		}
	}

	void Crouching()
	{
		if (!anim.GetBool("isRunning"))
		{
			if (Input.GetKey(KeyCode.S))
			{
				anim.SetBool("isCrouching", true);
			}
			if (Input.GetKeyUp(KeyCode.S))
			{
				anim.SetBool("isCrouching", false);
			}
		}
	}

	void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space) &&
			!anim.GetBool("isCrouching") &&
			!anim.GetBool("isSliding") &&
			anim.GetBool("isGrounded"))
		{
			anim.SetTrigger("isJumping");
		}
	}

	void DoubleJump()
	{
		if (Input.GetKeyDown(KeyCode.Space) &&
			!anim.GetBool("isGrounded"))
		{
			anim.SetTrigger("isDoubleJumping");
		}
	}

	void AirDash()
	{
		if (!anim.GetBool("isGrounded") &&
			Input.GetKeyDown(KeyCode.LeftShift))
		{
			anim.SetTrigger("isAirDashing");
		}
	}

	void TempGroundCheck()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			if (!anim.GetBool("isGrounded"))
			{
				anim.SetBool("isGrounded", true);
			}
			else
			{
				anim.SetBool("isGrounded", false);
			}
		}
	}
}
