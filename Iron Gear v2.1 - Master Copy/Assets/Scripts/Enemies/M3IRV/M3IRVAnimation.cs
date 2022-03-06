using UnityEngine;
using System.Collections;

public class M3IRVAnimation : MonoBehaviour
{
	public static M3IRVAnimation instance;

	public Transform currentObjectHolding;

	public Transform animatedModel;

	private M3IRVPhysics m3irvPhysics;
	private ThrowableObject throwableObject;
	private Animation anim;

	public enum animCharacterStates
	{
		idle,
		fightingStance,
		pointing,
		running,
		rightArmFiring,
		leftArmFiring,
		rightUppercut,
		shakesAngrily,
		jumping,
		startHover,
		handsOpenHoverStrafeL,
		handsOpenHoverStrafeR,
		handsClosedHoverCenter,
		handsClosedHoverStrafeL,
		handsClosedHoverStrafeR,
		falling,
		landing,
		rightHook,
		leftPunch,
		leftArmSweep,
		pickupObject,
		throwObject,
		pickupThrowObjectHoverCenter,
		pickupObjectHoverStrafeL,
		throwObjectHoverStrafeL,
		pickupObjectHoverStrafeR,
		throwObjectHoverStrafeR,
		fireBothHands
	}
	public animCharacterStates state { get; set; }

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		m3irvPhysics = GetComponent<M3IRVPhysics>();
		anim = animatedModel.GetComponent<Animation>();
	}

	public void UpdateM3IRVAnimation ()
	{
		ProcessCurrentState();
	}

	void ProcessCurrentState()
	{
		//hover to the left
		if (m3irvPhysics.moveVector.x > 0.01f)
		{
			state = animCharacterStates.handsOpenHoverStrafeL;
		}
		//strafe right
		else if (m3irvPhysics.moveVector.x < -0.01f)
		{
			state = animCharacterStates.handsClosedHoverStrafeR;
		}
		//hover in place
		else
		{
			if (state != animCharacterStates.pickupThrowObjectHoverCenter &&
		    	state != animCharacterStates.fireBothHands)
			{
				state = animCharacterStates.handsClosedHoverCenter;
			}
		}

		//set the state to play animations
		switch(state)
		{
		case animCharacterStates.handsOpenHoverStrafeL:
			HandsOpenHoverLeft();
			break;
		case animCharacterStates.handsClosedHoverCenter:
			HandsOpenHoverCenter();
			break;
		case animCharacterStates.pickupThrowObjectHoverCenter:
			PickupThrowObjectHoverCenter();
			break;
		case animCharacterStates.leftArmFiring:
			LeftArmFiring();
			break;
		case animCharacterStates.fireBothHands:
			FireBothHands();
			break;
		}
	}
	
	void HandsOpenHoverLeft()
	{
		anim.CrossFade("hand open hover L");
		anim["hand open hover L"].wrapMode = WrapMode.Loop;
	}

	void HandsOpenHoverCenter()
	{
		anim.CrossFade("fist hover center");
		anim["fist hover center"].wrapMode = WrapMode.Loop;

	}

	void PickupThrowObjectHoverCenter()
	{
		//get the script on the object M3IRV is currently holding
		if (currentObjectHolding)
		{
			throwableObject = currentObjectHolding.GetComponent<ThrowableObject>();
		}
		//throw the object while hovering
		if (state != animCharacterStates.handsClosedHoverCenter)
		{
			anim.CrossFade("pickup throw hover center");
		}
		anim["pickup throw hover center"].wrapMode = WrapMode.ClampForever;

		float animTime = anim["pickup throw hover center"].time;
		float animLength = anim["pickup throw hover center"].length;

		//after animation is done playing once
		if (animTime >= (animLength - 0.5f))
		{
			//stop the animation
//			anim.Stop("pickup throw hover center");

			//tell the object it has been thrown and allow it to perform it's functions
			throwableObject.hasThrownObject = true;

			//no longer holding an object
//			currentObjectHolding = null;

			//go back to hover state
			state = animCharacterStates.handsClosedHoverCenter;
		}
	}

	void FireBothHands()
	{
		anim.CrossFade("fire both hands");
		anim["fire both hands"].wrapMode = WrapMode.Loop;
	}

	void LeftArmFiring()
	{
		anim.CrossFade("fire L arm");
		anim["fire L arm"].wrapMode = WrapMode.ClampForever;

		//play animation once
		if (anim["fire L arm"].time >= anim["fire L arm"].length)
		{
			anim.Stop("fire L arm");
			state = animCharacterStates.handsClosedHoverCenter;
		}
	}
}




