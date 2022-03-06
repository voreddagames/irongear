using UnityEngine;
using System.Collections;

public class AnimateSteamFlaps : MonoBehaviour
{
	public float halfOpenAngle = 40.0f;
	public float fullyOpenAngle = 100.0f;

	public float cycleRate = 2.0f;

	public Transform upperFlaps;
	public Transform lowerFlap;

	public int curFlapIndex;

	public bool canAnimFlap = true;
	public bool hasChangedColor = false;
	public bool isOpeningAllFlapsHalf = false;
	public bool wasOpeningAllFlapsHalf = false;
	public bool isPerformingColorChange = false;

	public bool wasGroundSliding = false;

	public float curCycleRate = 0.0f;

	private PlayerAnimation playerAnimation;
	private Animation upperFlapsAnim;
	private Animation lowerFlapsAnim;
	private PlayerPhysics playerPhysics;
	private PlayerMotor playerMotor;
	private SteamChangeAbility steamChangeAbility;

	public enum FlapStates
	{
		BLHalfOpen,
		BLFullOpen,
		BRHalfOpen,
		BRFullOpen,
		frontHalfOpen,
		frontFullOpen
	}
	public FlapStates flapState { get; set; }

	void Awake()
	{
		upperFlapsAnim = upperFlaps.GetComponent<Animation>();
		lowerFlapsAnim = lowerFlap.GetComponent<Animation>();
		playerPhysics = GetComponent<PlayerPhysics>();
		playerAnimation = GetComponent<PlayerAnimation>();
		playerMotor = GetComponent<PlayerMotor>();
		steamChangeAbility = GetComponent<SteamChangeAbility>();
	}

	// Use this for initialization
	void Start ()
	{
		curCycleRate = cycleRate;
	}
	
	// Update is called once per frame
	public void UpdateAnimateSteamFlaps ()
	{
		CalculateFlapCycle();
	}

	void CalculateFlapCycle()
	{
		//cycle through the three flaps
		if ((playerAnimation.state == PlayerAnimation.animCharacterStates.idle ||
		    playerAnimation.state == PlayerAnimation.animCharacterStates.walking ||
		    playerAnimation.state == PlayerAnimation.animCharacterStates.startCrouch) &&
		    canAnimFlap)
		{
			//a cooldown between each animation
			if (curCycleRate > 0)
			{
				curCycleRate -= Time.deltaTime;
			}
			else
			{
				curFlapIndex++;
				curCycleRate = cycleRate;
				canAnimFlap = false;
			}
		}

		//reset the cycle
		if (curFlapIndex > 3)
		{
			curFlapIndex = 2;
		}

		//call to animate the flaps
		switch(curFlapIndex)
		{
			//steam for back left
		case 1:
			//			BackLeftFlapHalf();
			break;
			//steam for back right
		case 2:
			BackRightFlapHalfOpen();
			break;
		case 3:
			//steam for lower back
			LowerFlapHalfOpen();
			break;
		}

		//when sliding emit from left and right back flaps
		if (playerAnimation.state == PlayerAnimation.animCharacterStates.groundSliding)
		{
			canAnimFlap = false;
//			curCycleRate = cycleRate;
			wasGroundSliding = true;
			UpperFlapFullOpen();
		}
		//close the flaps when no longer sliding
		else
		{
			if (wasGroundSliding)
			{
				UpperFlapFullOpenToClose();
			}
		}

		//open hip flap when double jumping
		if (playerMotor.isDoubleJumping)
		{
			canAnimFlap = false;
//			curCycleRate = cycleRate;
			LowerFlapHalfOpen();
		}
		if (playerAnimation.state == PlayerAnimation.animCharacterStates.falling)
		{
			playerMotor.isDoubleJumping = false;
		}

		//when doing an air dash or when switching to a new color, open all flaps halfway
		if (playerPhysics.IsAirDashing())
		{
			canAnimFlap = false;
			isOpeningAllFlapsHalf = true;
		}

		if (steamChangeAbility.hasPressedColorChange && !hasChangedColor)
		{
			canAnimFlap = false;
			isOpeningAllFlapsHalf = true;
			hasChangedColor = true;
		}

		//call to animate all flaps halfway
		if (isOpeningAllFlapsHalf)
		{
			canAnimFlap = false;
//			curCycleRate = cycleRate;
//			BackLeftFlapHalf();
			wasOpeningAllFlapsHalf = true;
			BackRightFlapHalfOpen();
			LowerFlapHalfOpen();
		}
		else
		{
			if (wasOpeningAllFlapsHalf && !playerPhysics.IsAirDashing())
			{
				BackRightFlapHalfOpenToClose();
				LowerFlapHalfOpenToClose();
			}
		}

		//if switching color
		if (steamChangeAbility.hasPressedColorChange && isPerformingColorChange && wasOpeningAllFlapsHalf)
		{
			canAnimFlap = false;
			isOpeningAllFlapsHalf = true;
			isPerformingColorChange = false;
			//and still performing the flap animations
//			if (upperFlapsAnim.isPlaying && lowerFlapsAnim.isPlaying)
//			{
//				//reset the animations
//				upperFlapsAnim.Stop();
//				lowerFlapsAnim.Stop();
//			}
		}
		//reset when no longer holding color change button
		if (!steamChangeAbility.hasPressedColorChange && !isPerformingColorChange)
		{
			isPerformingColorChange = true;
		}

	}

	void UpperFlapFullOpen()
	{
		if (lowerFlapsAnim.isPlaying)
		{
			lowerFlapsAnim.Stop();
		}
		
		upperFlapsAnim.CrossFade("BL full open");
		upperFlapsAnim["BL full open"].wrapMode = WrapMode.ClampForever;
	}

	void UpperFlapFullOpenToClose()
	{
		float halfOpenCloseTime = upperFlapsAnim["BL full open to close"].time;
		float halfOpenCloseLength = upperFlapsAnim["BL full open to close"].length;
		
		upperFlapsAnim.CrossFade("BL full open to close");
		upperFlapsAnim["BL full open to close"].wrapMode = WrapMode.ClampForever;
		
		if (halfOpenCloseTime >= halfOpenCloseLength - 0.08f)
		{
			canAnimFlap = true;
			wasGroundSliding = false;
		}
	}
	
//	void BackLeftFlapHalf()
//	{
//		float halfOpenTime = upperFlapsAnim["BL half open"].time;
//		float halfOpenLength = upperFlapsAnim["BL half open"].length;
//
//		upperFlapsAnim.CrossFade("BL half open");
//		upperFlapsAnim["BL half open"].wrapMode = WrapMode.ClampForever;
//
//		if (halfOpenTime >= halfOpenLength)
//		{
//			float halfOpenCloseTime = upperFlapsAnim["BL half open to close"].time;
//			float halfOpenCloseLength = upperFlapsAnim["BL half open to close"].length;
//			
//			upperFlapsAnim.CrossFade("BL half open to close");
//			upperFlapsAnim["BL half open to close"].wrapMode = WrapMode.ClampForever;
//
//			if (halfOpenCloseTime >= halfOpenCloseLength - 0.08f)
//			{
//				canAnimFlap = true;
//				hasChangedColor = false;
//				isOpeningAllFlapsHalf = false;
//			}
//		}
//	}

	void BackRightFlapHalfOpen()
	{
		float halfOpenTime = upperFlapsAnim["BR half open"].time;
		float halfOpenLength = upperFlapsAnim["BR half open"].length;

		upperFlapsAnim.CrossFade("BR half open");
		upperFlapsAnim["BR half open"].wrapMode = WrapMode.ClampForever;

		if (halfOpenTime >= halfOpenLength)
		{
			BackRightFlapHalfOpenToClose();
//			float halfOpenCloseTime = upperFlapsAnim["BR half open to close"].time;
//			float halfOpenCloseLength = upperFlapsAnim["BR half open to close"].length;
//			
//			upperFlapsAnim.CrossFade("BR half open to close");
//			upperFlapsAnim["BR half open to close"].wrapMode = WrapMode.ClampForever;
//			
//			if (halfOpenCloseTime >= halfOpenCloseLength - 0.08f)
//			{
//				canAnimFlap = true;
//				hasChangedColor = false;
//				isOpeningAllFlapsHalf = false;
//			}
		}
	}

	void BackRightFlapHalfOpenToClose()
	{
		float halfOpenCloseTime = upperFlapsAnim["BR half open to close"].time;
		float halfOpenCloseLength = upperFlapsAnim["BR half open to close"].length;
		
		upperFlapsAnim.CrossFade("BR half open to close");
		upperFlapsAnim["BR half open to close"].wrapMode = WrapMode.ClampForever;
		
		if (halfOpenCloseTime >= halfOpenCloseLength - 0.08f)
		{
			canAnimFlap = true;
			hasChangedColor = false;
			wasOpeningAllFlapsHalf = false;
			isOpeningAllFlapsHalf = false;
		}
	}

	void LowerFlapHalfOpen()
	{
		float halfOpenTime = lowerFlapsAnim["hip half open"].time;
		float halfOpenLength = lowerFlapsAnim["hip half open"].length;
		
		lowerFlapsAnim.CrossFade("hip half open");
		lowerFlapsAnim["hip half open"].wrapMode = WrapMode.ClampForever;

		if (halfOpenTime >= halfOpenLength)
		{
			LowerFlapHalfOpenToClose();
//			float halfOpenCloseTime = lowerFlapsAnim["hip half open to close"].time;
//			float halfOpenCloseLength = lowerFlapsAnim["hip half open to close"].length;
//			
//			lowerFlapsAnim.CrossFade("hip half open to close");
//			lowerFlapsAnim["hip half open to close"].wrapMode = WrapMode.ClampForever;
//			
//			if (halfOpenCloseTime >= halfOpenCloseLength - 0.08f)
//			{
//				canAnimFlap = true;
//				playerMotor.isDoubleJumping = false;
//				hasChangedColor = false;
//				isOpeningAllFlapsHalf = false;
//			}
		}
	}

	void LowerFlapHalfOpenToClose()
	{
		float halfOpenCloseTime = lowerFlapsAnim["hip half open to close"].time;
		float halfOpenCloseLength = lowerFlapsAnim["hip half open to close"].length;
		
		lowerFlapsAnim.CrossFade("hip half open to close");
		lowerFlapsAnim["hip half open to close"].wrapMode = WrapMode.ClampForever;
		
		if (halfOpenCloseTime >= halfOpenCloseLength - 0.08f)
		{
			canAnimFlap = true;
//			playerMotor.isDoubleJumping = false;
			hasChangedColor = false;
			wasOpeningAllFlapsHalf = false;
			isOpeningAllFlapsHalf = false;
		}
	}
}
