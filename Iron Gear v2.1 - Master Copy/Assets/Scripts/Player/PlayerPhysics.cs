using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour
{
	public static PlayerPhysics instance;

	public float forwardSpeed = 20.0f;
	public float slideSpeed = 30.0f;

	public float forwardGroundAcceleration = 30.0f;
	public float airControlAcceleration = 10.0f;

	public float slowDownSpeed = 0.5f;
	public float slowSlideSpeed = 0.5f;
	public float slowToCrouch = 5.0f;
	public float knockBackDecceleration = 15.0f;

	public float jumpVelocity = 20.0f;
	public float doubleJumpForce = 30.0f;
	public float airDashForce = 70.0f;
	public float slideFromCrouchForce = 10.0f;

	public float gravity = 60.0f;
	
	public float airDashDuration = 0.2f;
	public float jumpDuration = 0.2f;
	public float slideFromCrouchDuration = 0.3f;

	public float slideFromCrouchDelay = 2.0f;
	public float tacticalSlideDelay = 2.0f;

	public float maxAngleToWalk = 60.0f;
	public float colliderCrouchScale = 0.5f;
	public float colliderSlideScale = 0.75f;

//	[HideInInspector]
	public float moveSpeed = 0.0f;

	public int doubleJumpLimit = 1;
	public int airDashLimit = 1;
	public int faceDir = 1;

	[HideInInspector]
	public bool canTacticalSlide = false;
//	[HideInInspector]
	public bool isCrouching = false;
//	[HideInInspector]
	public bool canSlideFromCrouch = false;
	[HideInInspector]
	public bool isPressingDown = false;

	private bool isGrounded = false;
	private bool isOnWall = false;
	public bool didCollideAbove = false;
	private bool isAirDashing = false;
	private bool isSliding = false;
	private bool canApplyGravity = true;
	
	private int curAirDashLimit;

	private float origColliderSizeY;
	private float origColliderCenterY;
	private float characterHeight; //bounding box height
	private float characterWidth; //bounding box width

	private float jumpDur = 0.0f;
	private float airDashDur = 0.0f;
	private float slideFromCrouchDur = 0.0f;

	private float curSlideFromCrouchDelay = 0.0f;
	private float curTacticalSlideDelay = 0.0f;

	public Vector3 startPos = Vector3.zero;
	private Vector3 moveVector = Vector3.zero;
	private Vector3 slideDir = Vector3.zero;
	private Vector3 velDirection = Vector3.zero;

	private GameObject playerModel;
	public GameObject loadingScreenGUI;
	public GameObject master;

	private Rigidbody rigidbodyComponent;

    private PlayerMotor playerMotor;
   // private PlayerAnimation playerAnimation;
	private AimMotor aimMotor;
	private MeleeSystem meleeSystem;
	private CamPositioning camPositioning;
	private LoadingScreen loadingScreen;
	private GameMaster gameMaster;
	private PlayerHealth playerHealth;
	private PlayerAnimManager playerAnimManager;
//	private PlayerAudio playerAudio;

    //getters and setters
    public bool IsGrounded() { return isGrounded; }
    public bool IsSliding() { return isSliding; }
	public bool IsOnWall() { return isOnWall; }
    public Vector3 MoveDirection() { return moveVector; }
    public float MoveSpeed() { return moveSpeed; }
	public void ApplyingGravity() { canApplyGravity = true; }
	public void RemovingGravity() { canApplyGravity = false; }
	public bool IsAirDashing() { return isAirDashing; }

    public bool IsInJump { get; set; }
    public bool IsJumpPressed { get; set; }
    public bool IsDead { get; set; }
	public Vector3 MoveVector { get; set; }

    void Awake()
    {
		instance = this;

		playerModel = GameObject.FindWithTag("CL8-Ton");
		//aimMotor = playerModel.GetComponent<AimMotor>();

		//playerAnimManager = playerModel.GetComponent<PlayerAnimManager>();
//		playerAudio = GetComponent<PlayerAudio>();
        playerMotor = GetComponent<PlayerMotor>();
       //playerAnimation = GetComponent<PlayerAnimation>();
		meleeSystem = GetComponent<MeleeSystem>();
		playerHealth = GetComponent<PlayerHealth>();
		camPositioning = Camera.main.GetComponent<CamPositioning>();
    }

	// Use this for initialization
	void Start ()
    {
		master = GameObject.FindWithTag("GameController");
		gameMaster = master.GetComponent<GameMaster>();

		loadingScreenGUI = GameObject.FindWithTag("Loading Screen");
		loadingScreen = loadingScreenGUI.GetComponent<LoadingScreen>();

		rigidbodyComponent = GetComponent<Rigidbody>();
        //initial start position
//        transform.position = startPos;

		curAirDashLimit = airDashLimit;
        airDashDur = airDashDuration;

		curTacticalSlideDelay = tacticalSlideDelay;
		curSlideFromCrouchDelay = slideFromCrouchDelay;

		slideFromCrouchDur = slideFromCrouchDuration;

        CalculateBounds();
	}

    void CalculateBounds()
    {
        characterHeight = ((BoxCollider)GetComponent<Collider>()).size.y;
        characterWidth = ((BoxCollider)GetComponent<Collider>()).size.z;

		origColliderSizeY = ((BoxCollider)GetComponent<Collider>()).size.y;
		origColliderCenterY = ((BoxCollider)GetComponent<Collider>()).center.y;
    }

    public void UpdatePlayerPhysics()
    {
		UpdateDeathInfo();
		UpdateJumping();
    }

	public void FixedUpdatePlayerPhysics()
    {
		AttackMotionMod();
    	ApplyGravity();
        
		UpdateAirDash();
        AirDash();
		KnockBack();
    }

	public void LateUpdatePlayerPhysics()
	{
		DetectWall();
        UpdateGroundInfo();
	}

    public void GetMotion(float dir)
    {
		PlayerAnimManager.DirectionalInput = dir;

		if (!isSliding)
		{
			//ground controls when not hitting a wall
			if (!isOnWall && isGrounded)
			{
				//apply directional input to vector
				moveVector = Vector3.right * moveSpeed;
			}
			//we hit a wall
			if (isOnWall && !playerHealth.isHitByTurretMelee)
			{
				moveSpeed = 0;
				//playerAnimation.state = PlayerAnimation.animCharacterStates.idle;
			}
			//air controls
			if (!isGrounded && faceDir != 0 && dir != 0)
			{
				moveSpeed = Mathf.MoveTowards(moveSpeed, forwardSpeed * faceDir, airControlAcceleration * Time.deltaTime);
				moveVector = Vector3.right * moveSpeed;
			}
			//apply Y velocity
			moveVector.y = GetComponent<Rigidbody>().velocity.y;

			//no directional key is pressed
			if (dir == 0 && isGrounded && !canTacticalSlide && !isCrouching && !canSlideFromCrouch && !playerHealth.isHitByTurretMelee)
			{
				if (!IsInJump && moveSpeed == 0 && meleeSystem.canAttack)
				{
					//constrain rigidbody
					rigidbodyComponent.constraints = RigidbodyConstraints.FreezePositionX |
						RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
				}
				//slow down
				moveSpeed = Mathf.MoveTowards(moveSpeed, 0, slowDownSpeed * Time.deltaTime);
			}

//			if (!meleeSystem.canAttack)
//			{
//				//unfreeze
//				rigidbodyComponent.constraints &= ~RigidbodyConstraints.FreezePositionX;
//				rigidbodyComponent.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
//			}
			//directional key is pressed
//			if (dir != 0 && meleeSystem.canAttack)

			if (dir != 0)
			{
				//unfreeze
				rigidbodyComponent.constraints &= ~RigidbodyConstraints.FreezePositionX;
				rigidbodyComponent.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

				//walking on the ground
				if (isGrounded && !canTacticalSlide && !isCrouching && !isOnWall)
				{
					//walk speed
					moveSpeed = Mathf.MoveTowards(moveSpeed, forwardSpeed * faceDir, forwardGroundAcceleration * Time.deltaTime);
				}
				if (!aimMotor.hasControl && !canTacticalSlide && !canSlideFromCrouch)
				{
					//set face direction according to the directional key press
					faceDir = (int)Mathf.Sign(dir);
				}
			}
			//we are attacking so stop moving
			if (meleeSystem.isAirAttacking)
			{
				moveSpeed = 0;
				moveVector = Vector3.right * moveSpeed;
			}
			//rotate
			transform.rotation = Quaternion.LookRotation(Vector3.right * faceDir);
		}
		else
		{
			SlideDownSlope();
		}
//        apply to velocity
		rigidbodyComponent.velocity = moveVector;
		velDirection = transform.InverseTransformDirection(rigidbodyComponent.velocity);
    }

	void KnockBack()
	{
		//turret did a charge attack
		if (playerHealth.isHitByTurretMelee)
		{
			//set the speed in which to knock the player back
			moveSpeed = Mathf.MoveTowards(moveSpeed, 0, knockBackDecceleration * Time.deltaTime);

			if (moveSpeed > -0.1f && moveSpeed < 0.1f)
			{
				playerHealth.isHitByTurretMelee = false;
			}
		}
	}

	void AttackMotionMod()
	{
		if (Input.GetAxis("Horizontal") != 0 && moveSpeed != 0 && !meleeSystem.canAttack && meleeSystem.hasAttacked)
		{
			moveSpeed = Mathf.MoveTowards(moveSpeed, meleeSystem.curAdvanceAttackSpeed * faceDir, meleeSystem.attackAdvanceSpeedModifier * Time.deltaTime);
		}
		if (!meleeSystem.hasAttacked && !meleeSystem.canAttack)
		{
			moveSpeed = Mathf.MoveTowards(moveSpeed, 0, meleeSystem.attackAdvanceSpeedModifier * 2 * Time.deltaTime);
		}
	}

//	public void ClosePunchesMotion()
//	{
//		if (Input.GetAxis("Horizontal") != 0)
//		{
//			moveSpeed = Mathf.MoveTowards(moveSpeed, meleeSystem.curAdvanceAttackSpeed * faceDir, meleeSystem.attackAdvanceSpeedModifier);
//		}
//	}

	#region Crouch and Sliding Methods
	void SlideDownSlope()
	{
		//reset constraints
		rigidbodyComponent.constraints &= ~RigidbodyConstraints.FreezePositionX;
		rigidbodyComponent.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

		//apply slide values
		moveVector = slideDir;
		
		//turn the collider instantly towards the move direction
		transform.rotation = Quaternion.LookRotation(Vector3.right * faceDir);

	//	playerAnimation.state = PlayerAnimation.animCharacterStates.slopeSliding;
	}

	public void SlideFromCrouch(bool jumpInput, bool ps4JumpInput)
	{
		BoxCollider boxCol = (BoxCollider)GetComponent<Collider>();
		
		Vector3 boxCenter = boxCol.center;
		Vector3 boxSize = boxCol.size;

		//pressed slide
		if (isCrouching && !canSlideFromCrouch && (jumpInput || ps4JumpInput))
		{
			canSlideFromCrouch = true;
		}
		//perform slide logic
		if (slideFromCrouchDur > 0 && canSlideFromCrouch)
		{
			isCrouching = false;

			rigidbodyComponent.constraints &= ~RigidbodyConstraints.FreezePositionX;
			rigidbodyComponent.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			
			//resize collider
			boxSize.y = origColliderSizeY * colliderSlideScale;
			boxCenter.y = origColliderCenterY - (origColliderSizeY * (1 - colliderSlideScale)) * 0.5f;
			
			//set animation state
//			playerAnimation.state = PlayerAnimation.animCharacterStates.groundSliding;
			
			//slide forward for a set time
			slideFromCrouchDur -= Time.deltaTime;
			moveSpeed = slideFromCrouchForce * faceDir;
		}
		//slide duration done
		if (slideFromCrouchDur <= 0)
		{
			//stop sliding
			canSlideFromCrouch = false;
			isCrouching = true;
			moveSpeed = 0;

			//start the delay countdown
			curSlideFromCrouchDelay -= Time.deltaTime;
		}
		//delay countdown is up allow sliding
		if (curSlideFromCrouchDelay <= 0)
		{
			slideFromCrouchDur = slideFromCrouchDuration;
			curSlideFromCrouchDelay = slideFromCrouchDelay;
		}
		//apply collider settings
		boxCol.size = boxSize;
		boxCol.center = boxCenter;
	}

	public void TacticalSlide(float vertInput)
	{
		BoxCollider boxCol = (BoxCollider)GetComponent<Collider>();

		Vector3 boxCenter = boxCol.center;
		Vector3 boxSize = boxCol.size;

		//once at top speed start counting down the slide delay
		if (Mathf.Abs(moveSpeed) >= (forwardSpeed - 1))
		{
			if (curTacticalSlideDelay > 0)
			{
				curTacticalSlideDelay -= Time.deltaTime;
			}
		}
		//if no longer top speed and hasn't yet slid
		//reset tactical slide delay
		else if (!canTacticalSlide)
		{
			curTacticalSlideDelay = tacticalSlideDelay;
		}

		//find if player pressed down
		if (!isPressingDown && vertInput < -0.5f)
		{
			//do tactical slide
			if (curTacticalSlideDelay <= 0)
			{
				curTacticalSlideDelay = 0;
				canTacticalSlide = true;
			}
			//do normal crouch
			else
			{
				isCrouching = true;
			}
			isPressingDown = true;
		}

		if (isPressingDown && isGrounded && !canSlideFromCrouch)
		{
			//slide while at top speed moving forward
			if (canTacticalSlide)
			{
				//set animation state
			//	playerAnimation.state = PlayerAnimation.animCharacterStates.groundSliding;

				//resize collider
				boxSize.y = origColliderSizeY * colliderSlideScale;
				boxCenter.y = origColliderCenterY - (origColliderSizeY * (1 - colliderSlideScale)) * 0.5f;

				//apply collider settings
				boxCol.size = boxSize;
				boxCol.center = boxCenter;

				//slow down so we don't slide forever
				moveSpeed = Mathf.MoveTowards(moveSpeed, 0, slowSlideSpeed * Time.deltaTime);
			}
			//normal crouch
			if (isCrouching)
			{
				curTacticalSlideDelay = tacticalSlideDelay;
				Crouch(boxSize, boxCenter, boxCol);
			}

		}
		if (moveSpeed == 0 && canTacticalSlide)
		{
			isCrouching = true;
			curTacticalSlideDelay = tacticalSlideDelay;
			canTacticalSlide = false;
		}

		if (vertInput > -0.5f && isGrounded && isPressingDown)
		{
			//reset collider
			ResetCollider();
		}
	}

	void Crouch(Vector3 boxSize, Vector3 boxCenter, BoxCollider boxCol)
	{
		//set animation state
	//	playerAnimation.state = PlayerAnimation.animCharacterStates.startCrouch;

		//slow down to a stationary crouch
		moveSpeed = Mathf.MoveTowards(moveSpeed, 0, slowToCrouch * Time.deltaTime);

		//resize collider
		boxSize.y = origColliderSizeY * colliderCrouchScale;
		boxCenter.y = origColliderCenterY - (origColliderSizeY * (1 - colliderCrouchScale)) * 0.5f;

		//apply collider settings
		boxCol.size = boxSize;
		boxCol.center = boxCenter;
	}

	void ResetCollider()
	{
		BoxCollider boxCol = (BoxCollider)GetComponent<Collider>();
		
		Vector3 boxCenter = boxCol.center;
		Vector3 boxSize = boxCol.size;

//		if (playerAnimation.state != PlayerAnimation.animCharacterStates.idle &&
//		    playerAnimation.state == PlayerAnimation.animCharacterStates.startCrouch)
//		{
//			//set animation state
//			playerAnimation.state = PlayerAnimation.animCharacterStates.standFromCrouch;
//
//		}

		//if we are currently pressing the down button, then make sure we no longer allow those functions
		if (isPressingDown)
		{
			canTacticalSlide = false;
			isPressingDown = false;
		}
		//reset collider
		boxSize.y = origColliderSizeY;
		boxCenter.y = origColliderCenterY;

		//apply collider settings
		boxCol.size = boxSize;
		boxCol.center = boxCenter;

		canSlideFromCrouch = false;
		isCrouching = false;

		slideFromCrouchDur = slideFromCrouchDuration;
		curSlideFromCrouchDelay = slideFromCrouchDelay;
	}
	#endregion

	void UpdateDeathInfo()
	{
		if (IsDead)
		{
			playerMotor.RemoveControl();
			
			if (isGrounded)
			{
				rigidbodyComponent.constraints = RigidbodyConstraints.FreezeAll;

//				loadingScreen.lastLevelLoaded = loadingScreen.curLevel;
//				loadingScreen.isLoadingNextLevel = true;
			}
		}
		if (!playerMotor.HasControl())
		{
			rigidbodyComponent.constraints = RigidbodyConstraints.FreezePositionX |
				RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			moveSpeed = 0;
		}
		else
		{
			rigidbodyComponent.constraints &= ~RigidbodyConstraints.FreezePositionX;
			rigidbodyComponent.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		}
	}

    #region Jumping Methods
    public void Jump()
    {
		
        //perform high jump
		if (jumpDur > 0)
        {
			

            IsInJump = true;
            
            //start the jump duration countdown
            jumpDur -= Time.deltaTime;

			Vector3 vel = rigidbodyComponent.velocity;

            //apply the high jump force
            vel.y = jumpVelocity;
			rigidbodyComponent.velocity = vel;

			camPositioning.isPlayerJumping = true;
        }
    }

    //perform double jump
    public void DoubleJump()
    {
        if (doubleJumpLimit > 0)
        {
			//subtract jump limit
            doubleJumpLimit--;

			Vector3 vel = rigidbodyComponent.velocity;

            //apply the double jump force
//			rigidbody.AddForce(Vector3.up * doubleJumpForce, ForceMode.VelocityChange);
            vel.y = doubleJumpForce;
			rigidbodyComponent.velocity = vel;

//			playerAudio.canPlayDoubleJump = true;
			camPositioning.isPlayerJumping = true;
			playerAnimManager.UpdateDoubleJumpingAnim();
        }
		else
		{
			camPositioning.isPlayerJumping = false;
		}
    }

    void UpdateJumping()
    {        
        //stop the high jump
        if (!IsInJump || jumpDur <= 0 || didCollideAbove)
        {
			camPositioning.isPlayerJumping = false;
            jumpDur = 0;
        }
        if (IsInJump)
        {
			//playerAnimation.state = PlayerAnimation.animCharacterStates.jumping;
        }

        //reset some values to their defaults
        if (isGrounded)
        {
            IsInJump = false;

            jumpDur = jumpDuration;
            doubleJumpLimit = 1;
        }
    }
    #endregion

    #region Air Dash Methods
    void AirDash()
    {
        //perform air dash
        if (curAirDashLimit > 0 && isAirDashing && airDashDur > 0)
        {
			Vector3 vel = rigidbodyComponent.velocity;

            //start air dash duration countdown
            airDashDur -= Time.deltaTime;

			vel.y = 0;
			rigidbodyComponent.velocity = vel;

            //apply the air dash force to the moveSpeed
            moveSpeed = airDashForce * faceDir;
        }
    }

    public void StartAirDash()
    {
        isAirDashing = true;

		playerAnimManager.UpdateAirDashingAnim();
//        playerAnimation.AirDash();
//		playerAnimation.state = PlayerAnimation.animCharacterStates.airDashing;
    }

    void UpdateAirDash()
    {
        //the duration of the air dash is over
        if (airDashDur < 0)
        {
            //count down the airDashLimit and set some values
            curAirDashLimit--;
            airDashDur = 0;

            //if we are currently dashing, reset moveSpeed to normal then stop air dashing
			if (isAirDashing)
			{
				moveSpeed = forwardSpeed * faceDir;
//				playerMotor.isAirDashing = false;
				isAirDashing = false;
			}
        }

        //reset values
        if (isGrounded)
        {
            airDashDur = airDashDuration;
            curAirDashLimit = airDashLimit;
        }
    }
    #endregion
		
    void ApplyGravity()
    {
        //always apply gravity
		if (!isSliding && canApplyGravity)
        {
			rigidbodyComponent.AddForce(Vector3.down + Physics.gravity * gravity, ForceMode.Acceleration);
		}
    }

    void OnCollisionEnter(Collision col)
    {
        ContactPoint contact = col.contacts[0];

        float aboveCheck = Vector3.Angle(contact.normal, -transform.up);
		float frontCheck = Vector3.Angle(contact.normal, -transform.forward);
		float backCheck = Vector3.Angle(contact.normal, transform.forward);

        //check for a collision above
        if (aboveCheck < 60)
        {
            didCollideAbove = true;
        }
		//check for collision below
		if (aboveCheck < -60)
		{
			//shake the camera a bit when landing

			//Camera.main.GetComponent<PerlinShake>().PlayShake();
			//Camera.main.GetComponent<RandomShake>().PlayShake();
			Camera.main.GetComponent<PeriodicShake>().PlayShake();
		}
		//check for collision in front
		if (frontCheck < 60)
		{
			if (col.collider.tag == "Wall")
			{
				isOnWall = true;
			}
			isAirDashing = false;

			//if we're sliding and hit a wall then stop sliding
			if (canTacticalSlide || canSlideFromCrouch)
			{
				moveSpeed = 0;
				slideFromCrouchDur = 0;
			}
		}
		//check for collision behind
		//make sure we aren't sliding since our back will be facing a wall
		if (backCheck < 60 && !isSliding && ((velDirection.x * -faceDir) < 0))
		{
			moveSpeed = 0;
		}
    }

    void UpdateGroundInfo()
    {
        float distBelowFeet = 0.05f;
        //used to place the raycast at the correct position
        float extraHeight = characterHeight * 0.75f;
        float halfPlayerWidth = characterWidth * 0.5f;

        //the ray's origins
        Vector3 origin0 = GetBottomCenter() + transform.forward * halfPlayerWidth + transform.up * extraHeight;
        Vector3 origin1 = GetBottomCenter() + -transform.forward * halfPlayerWidth + transform.up * extraHeight;

        //ray direction
        Vector3 dir = Vector3.down;

        RaycastHit hitInfo;

        //we're hitting ground
        if (Physics.Raycast(origin0, dir, out hitInfo, (extraHeight + distBelowFeet)))
        {
			if (hitInfo.transform.tag != "Raycast Ignore")
			{
            	HitGround(hitInfo);
			}
        }
        else if (Physics.Raycast(origin1, dir, out hitInfo, (extraHeight + distBelowFeet)))
        {
			if (hitInfo.transform.tag != "Raycast Ignore")
			{
            	HitGround(hitInfo);
			}
        }
        //we're not hitting ground
        else
        {
            isGrounded = false;
			isSliding = false;
        }
		Debug.DrawRay(origin0, dir * (extraHeight + distBelowFeet), Color.blue);
		Debug.DrawRay(origin1, dir * (extraHeight + distBelowFeet), Color.green);
    }

    void HitGround(RaycastHit hitInfo)
    {
        //apply the info from the surface normal of the ground that was hit by the ray
        //swap the Z and Y to give proper values
        slideDir = new Vector3(hitInfo.normal.y, -hitInfo.normal.x, 0);
        //then calculate the angle of the ground that was hit
        //calculating the surface normal's Y (not the axis)
        float groundAngle = Vector3.Angle(slideDir, new Vector3(slideDir.x, 0, 0));
        
		//too steep so slide
        if (groundAngle > maxAngleToWalk)
        {
			if (hitInfo.transform.tag == "Steep Surface")
			{
	            //apply some speed
				//sliding to the left
	            if (slideDir.y > 0)
	            {
	                slideDir *= -slideSpeed;
	            }
				//sliding to the right
	            else
	            {
	                slideDir *= slideSpeed;
				}
				if (isSliding)
				{
					//turn around when sliding if facing the wall
					if (slideDir.x > 0)
					{
						faceDir = 1;
					}
					else
					{
						faceDir = -1;
					}
				}
	            //we're sliding
	            isSliding = true;

	            //stop air dashing
	            isAirDashing = false;
			}
			//go up/down steep surface
			else
			{
				isGrounded = true;
			}
//            isGrounded = false;
        }
		//not too steep
        else
        {
            //we are grounded
            isGrounded = true;

            //reset these values
            didCollideAbove = false;
            isAirDashing = false;
            isSliding = false;
        }
    }

    void DetectWall()
    {
        RaycastHit hitInfo;

        float dist = 1.0f;
        float radiusCheck = 0.5f;

        //check for no wall in front
        if (!Physics.SphereCast(transform.position, radiusCheck, transform.forward, out hitInfo, dist))
        {
			//we're not on a wall
			isOnWall = false;
        }

		#region To be trashed
        //check for the wall behind
//        if (Physics.SphereCast(transform.position, radiusCheck, -transform.forward, out hitInfo, dist))
//        {
//            //set the direction vector to zero to allow an instant move away from the wall
//            if ((moveVector.x < 0 && faceDir == 1) || (moveVector.x > -0 && faceDir == -1))
//            {
//                moveVector.x = 0;
//            }
//        }
		#endregion
    }

//    public void SetRespawnPoint(Vector3 spawnPoint)
//    {
//        startPos = spawnPoint;
//    }

    public Vector3 GetBottomCenter()
    {
        return GetComponent<Collider>().bounds.center + GetComponent<Collider>().bounds.extents.y * Vector3.down;
    }
}