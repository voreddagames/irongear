using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    public Transform animatedModel;
    
    public float jumpAnimationSpeed = 30.0f;
	public float runAnimationSpeedModifier = 0.05f;

	public bool isDoneCrouching = false;

    private PlayerPhysics playerPhysics;
	private BoilingPointsSystem boilingPointsSystem;
	private PlayerHealth playerHealth;

	private Animation anim;

    public enum animCharacterStates
    {
        idle,
		walking,
		jumping,
		falling,
		groundSliding,
		dead,
		doubleJumping,
		airDashing,
		startCrouch,
		standFromCrouch,
		slopeSliding,
		shoulderDamage
    }

	public animCharacterStates state { get; set; }
	
	void Awake ()
    {
        playerPhysics = GetComponent<PlayerPhysics>();
		boilingPointsSystem = GetComponent<BoilingPointsSystem>();
		playerHealth = GetComponent<PlayerHealth>();
	}

	void Start()
	{
		anim = animatedModel.GetComponent<Animation>();
	}

    public void FixedUpdatePlayerAnimation()
    {
        ProcessCurrentState();
    }

    void ProcessCurrentState()
    {
		//get the speed of the character and adjust it so the animation's speed feels right
        float characterSpeed = Mathf.Abs(GetComponent<Rigidbody>().velocity.x) * runAnimationSpeedModifier;
        anim["run"].speed = characterSpeed;

//		float curMaxSpeed = playerPhysics.forwardSpeed * playerPhysics.faceDir;

		//on ground
		if (playerPhysics.IsGrounded() && !playerPhysics.IsDead)
		{
			//moving
			if (!playerPhysics.isPressingDown && playerPhysics.moveSpeed != 0)
			{
				state = animCharacterStates.walking;
			}
			//not moving
            if (characterSpeed == 0 && !playerPhysics.canSlideFromCrouch &&
			    state != animCharacterStates.startCrouch &&
			    state != animCharacterStates.standFromCrouch &&
			    state != animCharacterStates.jumping)
            {
                state = animCharacterStates.idle;
            }
            anim.Stop("jump");
        }
		//falling
		else if (GetComponent<Rigidbody>().velocity.y < 0 && !playerPhysics.IsSliding() && !playerPhysics.IsAirDashing())
		{
			state = animCharacterStates.falling;
		}

		if (playerPhysics.IsGrounded() && playerPhysics.IsDead)
		{
			state = animCharacterStates.dead;
		}

		//calculate the animation states
        switch (state)
        {
            case animCharacterStates.idle:
                Idle();
                break;
            case animCharacterStates.walking:
                Walking();
                break;
            case animCharacterStates.jumping:
                Jump();
                break;
            case animCharacterStates.doubleJumping:
                DoubleJump();
                break;
            case animCharacterStates.falling:
                Fall();
                break;
			case animCharacterStates.groundSliding:
                GroundSlide();
                break;
			case animCharacterStates.startCrouch:
				StartCrouch();
				break;
			case animCharacterStates.standFromCrouch:
				StandFromCrouch();
				break;
            case animCharacterStates.airDashing:
                AirDash();
                break;
            case animCharacterStates.dead:
				Death();
                break;
			case animCharacterStates.slopeSliding:
				SlopeSlide();
				break;
			case animCharacterStates.shoulderDamage:
				ShoulderDamage();
				break;
        }
		ResetCrouch();
    }

    void Idle()
    {
        anim.CrossFade("idle0");
    }

    void Walking()
    {
        anim.CrossFade("run");
    }

	#region Obsolete
//    void Jumping()
//    {
//        if ((!animatedModel.animation.isPlaying && playerPhysics.IsGrounded()) ||
//            playerPhysics.IsGrounded())
//        {
//            if (lastState == animCharacterStates.sprinting)
//            {
//                animatedModel.animation.CrossFade("sprint land");
//            }
//            else
//            {
//                animatedModel.animation.CrossFade("jump land");
//            }
//        }
//        else if (!animatedModel.animation.IsPlaying("jump"))
//        {
//            state = animCharacterStates.falling;
//            animatedModel.animation.CrossFade("falling");
//        }
//        else
//        {
//            state = animCharacterStates.jumping;
//        }
//    }
//
//    void DoubleJumping()
//    {
//        if ((!animatedModel.animation.isPlaying && !playerPhysics.IsGrounded()) ||
//            !playerPhysics.IsGrounded())
//        {
//            if (lastState == animCharacterStates.sprinting)
//            {
//                animatedModel.animation.CrossFade("sprint land");
//            }
//            else
//            {
//                animatedModel.animation.CrossFade("jump land");
//            }
//        }
//        else if (!animatedModel.animation.IsPlaying("double jump"))
//        {
//            state = animCharacterStates.falling;
//            animatedModel.animation.CrossFade("falling");
//        }
//        else
//        {
//            state = animCharacterStates.doubleJumping;
//        }
//    }
//
//    void Falling(float moveSpeed)
//    {
//        if (playerPhysics.IsGrounded())
//        {
//            if (state == animCharacterStates.falling && moveSpeed == 0)
//            {
//                state = animCharacterStates.landing;
//                animatedModel.animation.CrossFade("jump land");
//            }
//            //else
//            //{
//            //    animatedModel.animation.CrossFade("jump land");
//            //}
//
//            //state = animCharacterStates.landing;
//        }
//    }
//
//    void Landing()
//    {
//        if (lastState == animCharacterStates.sprinting)
//        {
//            if (animatedModel.animation.IsPlaying("sprint land"))
//            {
//                state = animCharacterStates.sprinting;
//                animatedModel.animation.Play("sprint");
//            }
//        }
//        else
//        {
//            if (!animatedModel.animation.IsPlaying("jump land"))
//            {
//                state = animCharacterStates.idle;
//                animatedModel.animation.Play("idle");
//            }
//        }
//    }

//    void Sliding()
//    {
//        if (!playerPhysics.IsSliding())
//        {
//            state = animCharacterStates.idle;
//            animatedModel.animation.CrossFade("idle");
//        }
//    }

//    void AirDashing()
//    {
//        if ((!animatedModel.animation.isPlaying && !playerPhysics.IsGrounded()) ||
//            !playerPhysics.IsGrounded())
//        {
//            if (lastState == animCharacterStates.sprinting)
//            {
//                animatedModel.animation.CrossFade("sprint land");
//            }
//            else
//            {
//                animatedModel.animation.CrossFade("jump land");
//            }
//        }
//        else if (!animatedModel.animation.IsPlaying("air dash"))
//        {
//            state = animCharacterStates.falling;
//            animatedModel.animation.CrossFade("falling");
//        }
//        else
//        {
//            state = animCharacterStates.airDashing;
//        }
//    }
	#endregion

    void Jump()
    {
		//play high jump animation
        if (state != animCharacterStates.falling &&
            state != animCharacterStates.doubleJumping)
        {
            anim.CrossFade("jump");
			anim["jump"].wrapMode = WrapMode.ClampForever;
        }
    }

    void DoubleJump()
    {
		//stop the high jump animation and play double jump animation
        anim.Stop("jump");
        anim.CrossFade("double jump");
		anim["double jump"].wrapMode = WrapMode.ClampForever;
    }

	public void ShoulderDamage()
	{
		float animSpeed = 1.3f;
		
		anim["right shoulder damage"].speed = animSpeed;
		
		anim.Blend("right shoulder damage", 1, 0.3f);

		float animLength = anim["right shoulder damage"].length;
		float animTime = anim["right shoulder damage"].time;
		
		if (animTime >= (animLength - 0.1f))
		{
			playerHealth.isDamaged = false;
		}
	}

	public void KnockBack()
	{

	}

    void Death()
    {
		//play death animations for the respective tiers
		if (boilingPointsSystem.tierLevel == 1)
		{
        	anim.CrossFade("T1 death fall to knees");
			anim["T1 death fall to knees"].wrapMode = WrapMode.ClampForever;
		}
    }

    void Fall()
    {
		anim.CrossFade("fall");
		anim["fall"].wrapMode = WrapMode.ClampForever;
    }

	void SlopeSlide()
	{
		anim.CrossFade("slide");
		anim["slide"].wrapMode = WrapMode.ClampForever;
	}

    void GroundSlide()
    {
        anim.CrossFade("tactical slide");
		anim["tactical slide"].wrapMode = WrapMode.ClampForever;
    }

	void StartCrouch()
	{
		float animLength = anim["crouch"].length;
		float animTime = anim["crouch"].time;

		if (isDoneCrouching)
		{
			animTime = animLength;
		}
		else
		{
			anim.CrossFade("crouch");
			anim["crouch"].wrapMode = WrapMode.ClampForever;
		}
		if (animTime >= animLength)
		{
			isDoneCrouching = true;
		}
	}

	void ResetCrouch()
	{
		float animTime = anim["crouch"].time;

		if (!playerPhysics.isCrouching)
		{
			isDoneCrouching = false;
			animTime = 0;
			anim["crouch"].time = animTime;
		}
	}

	void StandFromCrouch()
	{
		anim.Play("stand from crouch");
		anim["stand from crouch"].wrapMode = WrapMode.ClampForever;

		float animLength = anim["stand from crouch"].length;
		float animTime = anim["stand from crouch"].time;

		if (animTime >= (animLength))
		{
			state = animCharacterStates.idle;
		}
	}

    void AirDash()
    {
        anim.CrossFade("air dash forward");
		anim["air dash forward"].wrapMode = WrapMode.ClampForever;
    }
}
