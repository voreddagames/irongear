using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MeleeSystem : MonoBehaviour
{
	public static MeleeSystem instance;

	public List<int> allStandingAttacks;
	public List<int> allCrouchedAttacks;
	public List<int> allAirAttacks;

	public int numOfStandingAttacks = 6;
	public int numOfCrouchedAttacks = 3;
	public int numOfAirAttacks = 3;

	public int attackLimit = 3;

	//	[HideInInspector]
	public bool hasAttacked = false;
	public bool canAttack = true;
	public bool isPressingAttack = false;
	public bool isAirAttacking = false;
	public bool isGroundAttacking = false;
	public bool isCrouchAttacking = false;

	public float rangeToDmg = 1.0f;
	public float rangeToCurEnemy = 10.0f;

	public float attackAdvanceSpeedDown = 15.0f;
	public float attackAdvanceSpeedUp = 10.0f;
	public float attackAdvanceSpeedModifier = 10.0f;
	public float contactAttackAdvanceDist = 2.0f;
	public float contactAdvanceSpeed = 5.0f;

	public float airAttackHangTime = 0.5f;
	public float comboCoolDown = 0.2f;

	public int damage = 2;
	public int tier1Dmg = 2;
	public int tier2Dmg = 4;
	public int tier3Dmg = 8;
	public int tier4Dmg = 12;

	public GameObject impactFX;

	public Transform leftFistCol;
	public Transform rightFistCol;
	public Transform leftFootCol;
	public Transform rightFootCol;

	public Transform curFisticuffCol;

	public Vector3 lastPos;

	public GameObject curEnemy;

	public bool isDamagingEnemy;
	//	public bool isAttackingEnemy;

	private PlayerPhysics playerPhysics;
	private BoilingPointsSystem boilingPointsSystem;
	private EnemyHealth enemyHealth;
	private TurretPhysics turretPhysics;
	private PlayerMotor playerMotor;

	private float rangeFromTarget;

	private float curComboCoolDown = 0.0f;
	private float curAirAttackHangTime = 0.0f;

	//	[HideInInspector]
	public float curAdvanceAttackSpeed = 0.0f;
	[HideInInspector]
	public float curAdvanceAttackDist = 0.0f;

	private int previousLevel = 0;
	//	[HideInInspector]
	public int curAttackLimit = 0;
	private int curNumberOfAttacks;
	public int curStandingAttackIndex;
	public int curCrouchingAttackIndex;
	public int curAirAttackIndex;

	private Vector3 toTarget;

	void Awake ()
	{
		instance = this;

		playerPhysics = GetComponent<PlayerPhysics>();
		boilingPointsSystem = GetComponent<BoilingPointsSystem>();
		playerMotor = GetComponent<PlayerMotor>();
	}

	void Start()
	{
		//		curAdvanceAttackDist = attackAdvanceDist;
		//		curAdvanceAttackSpeed = attackAdvanceSpeed;

		//curAttackLimit = attackLimit;
	}

	public void UpdateMeleeSystem()
	{
		if (playerMotor.HasControl())
		{
			//call the attack buttons
			AttackInput(Input.GetButtonDown("Square"), Input.GetMouseButtonDown(0));
		}
		//		AttackInput(Input.GetMouseButtonDown(0));
	}

	public void FixedUpdateMeleeSystem ()
	{
		ProcessAttackDirection();
		ModAttackPoints();
		CalculateNumberOfAttacks();
		CalculateAttack();
	}

	void ModAttackPoints()
	{
		if (boilingPointsSystem.tierLevel != previousLevel)
		{
			//set the player's health for each tier
			switch(boilingPointsSystem.tierLevel)
			{
			case 1:
				damage = tier1Dmg;
				break;
			case 2:
				damage = tier2Dmg;
				break;
			case 3:
				damage = tier3Dmg;
				break;
			case 4:
				damage = tier4Dmg;
				break;
			}
			previousLevel = boilingPointsSystem.tierLevel;
		}
	}

	void CalculateNumberOfAttacks()
	{
		switch(boilingPointsSystem.tierLevel)
		{
		case 1:
			numOfStandingAttacks = 3;
			numOfCrouchedAttacks = 3;
			numOfAirAttacks = 3;
			break;
		}
	}

	void ProcessAttackDirection()
	{
		#region Temporary Trash
		//		RaycastHit hitInfo;

		//		if (Physics.Raycast(transform.position, transform.forward, out hitInfo, rangeToDmg))
		//		{
		//			if (hitInfo.transform.tag == "Enemy")
		//			{
		//				curEnemy = hitInfo.transform.gameObject;
		//			}
		//if ray is not intersecting an enemy
		//			else
		//			{
		//				curEnemy = null;
		//			}
		//		}
		//intersecting nothing
		//		else
		//		{
		//			curEnemy = null;
		//		}


		//check for enemy on right side
		//		if (Physics.Raycast(transform.position, Vector3.right, out hitInfo, rangeToDmg))
		//		{
		//			if (hitInfo.transform.tag == "Enemy")
		//			{
		//				enemyRight = hitInfo.transform.gameObject;
		//			}
		//			//if ray is not intersecting an enemy
		//			else
		//			{
		//				enemyRight = null;
		//			}
		//		}
		//		//inter secting nothing
		//		else
		//		{
		//			enemyRight = null;
		//		}
		//check for enemy on left side
		//		if (Physics.Raycast(transform.position, Vector3.left, out hitInfo, rangeToDmg))
		//		{
		//			if (hitInfo.transform.tag == "Enemy")
		//			{
		//				enemyLeft = hitInfo.transform.gameObject;
		//			}
		//			else
		//			{
		//				enemyLeft = null;
		//			}
		//		}
		//		else
		//		{
		//			enemyLeft = null;
		//		}
		#endregion

		//attacking
		if (!canAttack)
		{
			//on ground attack
			if (playerPhysics.IsGrounded())
			{
				//if we were in the middle of an air attack cancel the air attack
				if (isAirAttacking)
				{
					canAttack = true;
				}
				//if we were in the middle of a standing attack and then crouch or tactical slide,
				//cancel ground attack
				if ((playerPhysics.isCrouching || playerPhysics.canTacticalSlide) && isGroundAttacking)
				{
					canAttack = true;
				}
				if (isCrouchAttacking && !playerPhysics.isCrouching)
				{
					canAttack = true;
				}
			}
			//attacking in air
			//stop from falling
			else
			{
				//if we are in the middle of a ground or crouch attack cancel those
				if (isGroundAttacking || isCrouchAttacking)
				{
					canAttack = true;
				}
				//reset hang time
				curAirAttackHangTime = airAttackHangTime;
				//stop falling
				playerPhysics.RemovingGravity();
			}
			#region Temporary Trash
			//			if (curEnemy)
			//			{
			//				Vector3 offset = curEnemy.transform.position - transform.position;
			//
			//				float sqrLen = offset.sqrMagnitude;
			//				float magDist = rangeToDmg * rangeToDmg;
			//
			//				if (sqrLen < magDist)
			//				{
			//					if (isDamagingEnemy)
			//					{
			//						curAdvanceAttackDist = 0;
			//						curAdvanceAttackSpeed = 0;
			//
			//						playerPhysics.moveSpeed = 0;
			//
			////						DamageEnemy();
			//						
			//					}
			//				}
			//				if (sqrLen > magDist + 50)
			//				{
			//					curAdvanceAttackDist = attackAdvanceDist;
			//					curAdvanceAttackSpeed = attackAdvanceSpeed;
			//				}
			//			}
			//			else
			//			{
			//				curAdvanceAttackDist = attackAdvanceDist;
			//				curAdvanceAttackSpeed = attackAdvanceSpeed;
			//			}

			//advance forward when attacking
			//			transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, lastXPos + curAdvanceAttackDist * playerPhysics.faceDir, curAdvanceAttackSpeed * Time.deltaTime),
			//			                                 transform.position.y, transform.position.z);

			//			if (enemyLeft)
			//			{
			//				if (isDamagingLeftEnemy)
			//				{
			//					DamageLeftEnemy();
			//				}
			//				if (isAttackingLeftEnemy)
			//				{
			//					turretScript = enemyLeft.GetComponent<Turret>();
			//					turretScript.PushBack();
			//				}
			//			}
			//			if (enemyRight)
			//			{
			//				if (isDamagingRightEnemy)
			//				{
			//					DamageRightEnemy();
			//				}
			//				if (isAttackingRightEnemy)
			//				{
			//					turretScript = enemyRight.GetComponent<Turret>();
			//					turretScript.PushBack();
			//				}
			//			}
			#endregion
		}

		//done attacking
		else
		{
			//			isAttackingEnemy = false;
			isDamagingEnemy = false;
			isGroundAttacking = false;
			isCrouchAttacking = false;

			hasAttacked = false;

			if (isAirAttacking)
			{
				//hover in air for a time before falling
				curAirAttackHangTime -= Time.deltaTime;
			}
			else
			{
				curAirAttackHangTime = 0;
			}

			if (curAirAttackHangTime <= 0)
			{
				//allow falling again
				playerPhysics.ApplyingGravity();
				isAirAttacking = false;
			}
			//			curAdvanceAttackDist = attackAdvanceDist;
			//			curAdvanceAttackSpeed = attackAdvanceSpeed;

			curFisticuffCol = null;
		}

		if (curAttackLimit > 0 && curComboCoolDown > 0 && canAttack)
		{			
			//rest time before being able to attack
			if (curComboCoolDown > 0)
			{
				curComboCoolDown -= Time.deltaTime;
			}
		}
		//reset stuff when rest time is up
		if (curComboCoolDown <= 0)
		{
			curComboCoolDown = 0;

			curStandingAttackIndex = 0;
			curCrouchingAttackIndex = 0;
			curAirAttackIndex = 0;
			//reset the limit and the cool down time
			curAttackLimit = 0;
			curComboCoolDown = comboCoolDown;
		}
	}

	void AttackInput(bool attackButton, bool mouseAttack)
	{
		if ((attackButton || mouseAttack) && hasAttacked && canAttack)
		{
			isPressingAttack = true;
		}
		//attacking
		if ((attackButton || mouseAttack) && !hasAttacked && curAttackLimit < attackLimit &&
			!playerPhysics.canTacticalSlide && !playerPhysics.canSlideFromCrouch)
		{
			//count down the number of attacks
			curAttackLimit++;

			//attacking on the ground
			if (playerPhysics.IsGrounded())
			{
				//we are not crouching therefore we are performing standing attacks
				if (!playerPhysics.isCrouching)
				{
					curStandingAttackIndex++;
					isGroundAttacking = true;
				}
				//we are crouching so therefore we are performing crouching attacks
				else
				{
					curCrouchingAttackIndex++;
					isCrouchAttacking = true;
				}
				//if we were attacking in the air, cancel that action
				if (isAirAttacking)
				{
					isAirAttacking = false;
				}

			}
			//attacking in the air
			else
			{
				//stop any actions related to attacking while standing on the ground
				if (isGroundAttacking)
				{
					isGroundAttacking = false;
				}
				//stop any actions related to attacking while crouching
				if (isCrouchAttacking)
				{
					isCrouchAttacking = false;
				}
				//count up the air attacks performed
				curAirAttackIndex++;
				isAirAttacking = true;
			}
			isDamagingEnemy = true;

			lastPos = transform.position;

			//we are attacking so get out of this if statement
			//to prevent continuous attacking
			hasAttacked = true;

			canAttack = false;
		}

		#region Temporary Trash
		//attacking to the left
		//		if (attackButton < -0.25f && !hasAttacked && curAttackLimit > 0)
		//		{
		//			//turn face left
		//			playerPhysics.faceDir = -1;
		//
		//			curAttackLimit--;
		//
		//			if (playerPhysics.IsGrounded())
		//			{
		//				if (!playerPhysics.isCrouching)
		//				{
		//					curStandingAttackIndex++;
		//				}
		//				else
		//				{
		//					curCrouchingAttackIndex++;
		//				}
		//
		//				if (isAirAttacking)
		//				{
		//					isAirAttacking = false;
		//				}
		//			}
		//			else
		//			{
		//				curAirAttackIndex++;
		//				isAirAttacking = true;
		//			}
		//			playerPhysics.moveSpeed = 0;
		//
		//			isAttackingLeftEnemy = true;
		//			isDamagingLeftEnemy = true;
		//
		//			lastXPos = transform.position.x;
		//
		//			hasAttacked = true;
		//		}
		#endregion

		//no longer attacking
		if ((!attackButton || !mouseAttack) && canAttack)
		{
			isPressingAttack = false;
			hasAttacked = false;
		}
	}

	void CalculateAttack()
	{
		//call for random attack animations
		//tier one attacks
		if (!isPressingAttack && !canAttack && boilingPointsSystem.tierLevel == 1)
		{
			//standing attacks
			if (!playerPhysics.isCrouching && allStandingAttacks.Count == numOfStandingAttacks &&
				playerPhysics.IsGrounded() && curStandingAttackIndex != 0)
			{
				switch (curStandingAttackIndex)
				{
				case 1:
					curFisticuffCol = leftFistCol;
					BroadcastMessage("T1LeftCross", SendMessageOptions.DontRequireReceiver);
					curAdvanceAttackSpeed = attackAdvanceSpeedUp;
					break;
				case 2:
					curFisticuffCol = rightFistCol;
					BroadcastMessage("T1RightUppercut", SendMessageOptions.DontRequireReceiver);
					curAdvanceAttackSpeed = attackAdvanceSpeedUp;
					break;
				case 3:
					curFisticuffCol = rightFootCol;
					BroadcastMessage("T1RightRoundKick", SendMessageOptions.DontRequireReceiver);
					curAdvanceAttackSpeed = attackAdvanceSpeedUp;
					break;
					#region Temporary Trash
					//				case 3:
					//					BroadcastMessage("T1RightCross", SendMessageOptions.DontRequireReceiver);
					//					break;
					//				case 4:
					//					BroadcastMessage("T1RightStomp", SendMessageOptions.DontRequireReceiver);
					//					break;
					//				case 5:
					//					BroadcastMessage("T1LeftSpinKick", SendMessageOptions.DontRequireReceiver);
					//					break;
					#endregion
				}
			}
			//crouched attacks
			if (playerPhysics.isCrouching && allCrouchedAttacks.Count == numOfCrouchedAttacks &&
				playerPhysics.IsGrounded() && curCrouchingAttackIndex != 0)
			{
				switch(curCrouchingAttackIndex)
				{
				case 1:
					curFisticuffCol = leftFistCol;
					BroadcastMessage("T1CrouchLeftPunch", SendMessageOptions.DontRequireReceiver);
					break;
				case 2:
					curFisticuffCol = rightFistCol;
					BroadcastMessage("T1CrouchRightPunch", SendMessageOptions.DontRequireReceiver);
					break;
				case 3:
					curFisticuffCol = rightFootCol;
					BroadcastMessage("T1CrouchRightKick", SendMessageOptions.DontRequireReceiver);
					break;
				}
			}
			//in air attacks
			if (!playerPhysics.IsGrounded() && allAirAttacks.Count == numOfAirAttacks && curAirAttackIndex != 0 &&
				isAirAttacking)
			{
				switch (curAirAttackIndex)
				{
				case 1:
					BroadcastMessage("T1AirLeftJab", SendMessageOptions.DontRequireReceiver);
					break;
				case 2:
					BroadcastMessage("T1AirRightHook", SendMessageOptions.DontRequireReceiver);
					break;
				case 3:
					BroadcastMessage("T1AirRightKick", SendMessageOptions.DontRequireReceiver);
					break;
				}
			}
		}
	}

	public void DamageEnemy()
	{
		Instantiate(impactFX, curFisticuffCol.position, transform.rotation);

		//spend some boiling points
		enemyHealth = curEnemy.GetComponent<EnemyHealth>();
		boilingPointsSystem.AdjustBoilTemp(-enemyHealth.energyCost);
		//apply damage to the current enemy being hit
		enemyHealth.AdjustHealth(-damage);
		isDamagingEnemy = false;
	}

	#region Temporary Trash
	//	void DamageRightEnemy()
	//	{
	//		if (isDamagingRightEnemy)
	//		{
	//			//spend some boiling points
	//			enemyHealth = enemyRight.GetComponent<EnemyHealth>();
	//			boilingPointsSystem.AdjustBoilTemp(-enemyHealth.energyCost);
	//			//apply damage
	//			enemyHealth.AdjustHealth(-damage);
	//			isDamagingRightEnemy = false;
	//		}
	//	}

	//	void DamageLeftEnemy()
	//	{
	//		if (isDamagingLeftEnemy)
	//		{
	//			enemyHealth = enemyLeft.GetComponent<EnemyHealth>();
	//			boilingPointsSystem.AdjustBoilTemp(-enemyHealth.energyCost);
	//
	//			//apply damage
	//			enemyHealth.AdjustHealth(-damage);
	//			isDamagingLeftEnemy = false;
	//		}
	//	}
	#endregion
}