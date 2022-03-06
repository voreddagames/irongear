using UnityEngine;
using System.Collections;
using System.Linq;

public class TurretPhysics : MonoBehaviour
{
	public enum EnemyColorType
	{
		Red,
		Blue,
		Yellow
	}
	public EnemyColorType enemyColorType;

//	public static Turret instance;
	
	public static Transform currentTurretShooting;

	public float meleeAttackRateSameColor = 3.0f;
	public float meleeAttackRateDiffColor = 1.5f;
	public float meleeChargeUpTime = 1.0f;
	public float meleeAttackTime = 1.5f;
	public float minPatrolTime = 0.5f;
	public float maxPatrolTime = 2.0f;

	public float alertZone = 20.0f;
	public float shootRange = 15.0f;
	public float personalBubble = 5.0f;
	public float vantageZone = 10.0f;
	public float comfortZone = 11.0f;
	public float meleeAttackRange = 2.0f;

	public float moveSpeed = 15.0f;
	public float recoilSpeed = 5.0f;
	public float recoilAcceleration = 20.0f;
	public float meleeChargeUpSpeed = 5.0f;
	public float meleeSpeed = 20.0f;
	public float meleeAcceleration = 15.0f;
	public float patrolSpeed = 5.0f;
	public float acceleration = 10.0f;
	public float deceleration = 15.0f;
	public float gravityForce = 10.0f;

	public float explosionPower = 10.0f;
	public float explosionRadius = 5.0f;
	public float upwardsExplosionMod = 2.0f;

	public float flipUprightSpeed = 50.0f;
	public float upsidedownRotationThreshold = 20.0f;
	public float groundCheckDist = 2.0f;

	public float meleeDamage = 5.0f;

	public Transform explosionTarget;
	public Transform alertLight;
	public Transform playerTarget;

	public GameObject turretModel;
	public GameObject explosion;
	public GameObject debris;
	public GameObject explodedTurret;
	public GameObject detectionBubble;
	public Transform[] turretPieces;
	public Transform[] waypoints;

	public int curWaypointIndex;

//	public bool canDetectPlayer;

	//[HideInInspector]
	public bool canShoot;
//	[HideInInspector]
	public bool canMelee = false;
//	[HideInInspector]
	public bool isMeleeAttacking;
//	[HideInInspector]
	public bool isBarrierInFront;
//	[HideInInspector]
	public bool isBarrierBehind;
	[HideInInspector]
	public bool canMoveFromEnemy;
	[HideInInspector]
	public bool canMoveFromPlayer;
//	[HideInInspector]
	public bool canSee;
//	[HideInInspector]
	public bool isTooClose;
	[HideInInspector]
	public bool isPatrolling = false;
	public bool canRecoil;

	public bool inRangeForMelee;
	public bool isGrounded;

	public int faceDir = 1;
	public int moveDir = 1;
	public int rotDir = 1;

	private int curIndexForRecoilStandAttack;
	private int curIndexForRecoilCrouchAttack;
	private int curIndexForRecoilAirAttack;

	private float characterWidth;

	public float curMoveSpeed;

	public float curMeleeAttackRate = 0.5f;
	private float curMeleeChargeUpTime;
	public float curMeleeAttackTime;
	public float curPatrolTime;

	public float rndPatrolDur;

	public float maxDistForWaypointSpawn = 10.0f;
	public float offsetDisForWaypointSpawn = 2.0f;

//	[HideInInspector]
	public GameObject player;
	private Transform detectedObject;

	private Vector3 preLoc;
	[HideInInspector]
	public Vector3 curDirVel;

	private Vector3 lastPosOnPatrol;

	private Rigidbody rb;

	public LayerMask layers;
	private int ignoreCollisionLayer = 1 << 11; //layer for ignoring collisions

	public float rayDist = 4.0f;

	private SteamChangeAbility steamChangeAbility;
	public EnemyHealth enemyHealth;
	private TurretExplode turretExplode;
	private DetectEnemyEncounter detectEnemyEncounter;
	public DetectPlayerLevel detectPlayerLevel;
	private MeleeSystem meleeSystem;
	private FisticuffCollisionDetection fisticuffCollisionDetection;

	public bool CanSee() { return canSee; }
	public string ColorType() { return enemyColorType.ToString(); }
	public float CurMoveSpeed() { return curMoveSpeed; }
	public bool CanRecoil() { return canRecoil; }

	public float dist;

	// Use this for initialization
	void Start ()
	{
//		player = GameObject.FindWithTag("Player");
//
//		steamChangeAbility = player.GetComponent<SteamChangeAbility>();
//		enemyHealth = GetComponent<EnemyHealth>();
//		turretExplode = explodedTurret.GetComponent<TurretExplode>();
//		detectEnemyEncounter = GetComponent<DetectEnemyEncounter>();
//		detectPlayerLevel = detectionBubble.GetComponent<DetectPlayerLevel>();
//
//		turretPieces = explodedTurret.GetComponentsInChildren<Transform>();
//		rb = GetComponent<Rigidbody>();
//
//		ignoreCollisionLayer = ~ignoreCollisionLayer;
//		layers = ~layers;
//
//		alertLight.gameObject.SetActive(false);
//
//		curMeleeChargeUpTime = meleeChargeUpTime;
//		curPatrolTime = patrolTime;

		Initialize();
		CalculateBounds();
	}

	public void Initialize()
	{
		player = GameObject.FindWithTag("Player");

		meleeSystem = player.GetComponent<MeleeSystem>();
		steamChangeAbility = player.GetComponent<SteamChangeAbility>();
		enemyHealth = GetComponent<EnemyHealth>();
		turretExplode = explodedTurret.GetComponent<TurretExplode>();
		detectEnemyEncounter = GetComponent<DetectEnemyEncounter>();
		detectPlayerLevel = detectionBubble.GetComponent<DetectPlayerLevel>();
		turretPieces = explodedTurret.GetComponentsInChildren<Transform>();
		rb = GetComponent<Rigidbody>();
		
		ignoreCollisionLayer = ~ignoreCollisionLayer;
		layers = ~layers;
		
		alertLight.gameObject.SetActive(false);
		
		curMeleeChargeUpTime = meleeChargeUpTime;
		//curPatrolTime = patrolTime;
	}

	void CalculateBounds()
	{
		characterWidth = GetComponent<Collider>().bounds.size.z;
	}

	// Update is called once per frame
	void Update()
	{
		GetMotion();
		//ImpactReaction();
		Patrol();
	}

	void LateUpdate()
	{
		Boom();
	}

	void FixedUpdate ()
	{
		//DetectWall();
		ApplyGravity();
		//GetMotion();
		CalculateDirectionVelocity();
		MeleeAttack();
		CheckGround();
		//FlipUpright();
	}

	public void MeleeAttack()
	{
		float dist = Vector3.Distance(player.transform.position, transform.position);

		//in melee attack range
		if (dist <= meleeAttackRange)
		{
			inRangeForMelee = true;
			//perform melee actions for when turret's color is different from player's
			/*if (canSee)
			{
				if (curMeleeAttackRate <= 0)
				{
					canMelee = true;
					curMeleeAttackRate = meleeAttackRateDiffColor;
				}
				if (!canMelee)
				{
					//play charge animation here
					//stop moving
//					curMoveSpeed = 0;
					curMeleeAttackRate -= Time.deltaTime;
				}
			}
			else*/

			//perform melee count down for when turret's color is the same as player's
			if (!canSee)
			{
				//activate count down
				if (!canMelee)
				{
					curMeleeAttackRate -= Time.deltaTime;
				}
				
				//count down complete.  ready to attack
				if (curMeleeAttackRate <= 0)
				{
					canMelee = true;
					curMeleeAttackRate = meleeAttackRateSameColor;
				}
			}
		}
		else
		{
			//we are not in range for a melee attack
			inRangeForMelee = false;

			if (!canMelee)
			{
				curMeleeAttackRate = meleeAttackRateSameColor;
			}
		}

		//we are able to melee/charge attack
		if (canMelee)
		{
			PlayerHealth.instance.currentAttackingEnemy = transform.gameObject;

			//indicate melee attack
			if (curMeleeChargeUpTime > 0)
			{
				//play charge up animation here

//				curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, -meleeChargeUpSpeed, meleeAcceleration * Time.deltaTime);
				curMoveSpeed -= meleeChargeUpSpeed * Time.deltaTime;
				curMeleeChargeUpTime -= Time.deltaTime;
			}
			//execute charge/melee attack
			if (curMeleeAttackTime > 0 && curMeleeChargeUpTime <= 0)
			{
				isMeleeAttacking = true;
//				curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, meleeSpeed, meleeAcceleration * Time.deltaTime);
				curMoveSpeed += meleeSpeed * Time.deltaTime;
				curMeleeAttackTime -= Time.deltaTime;
			}
			//charge attack complete
			if (curMeleeAttackTime <= 0 || (isBarrierBehind || isBarrierInFront))
			{
				curMoveSpeed = 0;
				isMeleeAttacking = false;
				canMelee = false;
			}
//			if (isBarrierBehind || isBarrierInFront)
//			{
//				curMoveSpeed = 0;
//				isMeleeAttacking = false;
//				canMelee = false;
//			}
		}
		//we are not able to perform the charge attack
		else
		{
//			curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, 0, meleeAcceleration * Time.deltaTime);
			curMeleeChargeUpTime = meleeChargeUpTime;
			curMeleeAttackTime = meleeAttackTime;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "Player" && isMeleeAttacking)
		{
			canMelee = false;
			curMoveSpeed = 0;
			isMeleeAttacking = false;
		}
	}

	void ApplyGravity()
	{
		rb.AddForce(Vector3.down + Physics.gravity * gravityForce, ForceMode.Acceleration);
	}

	public void GetMotion()
	{
		Vector3 diff = player.transform.position - transform.position;
		Vector3 dir = transform.TransformDirection(Vector3.forward);
		float loc = Vector3.Dot(dir, diff);

		dist = Vector3.Distance(transform.position, player.transform.position);

		RaycastHit lineHitInfo;

		//player behind enemy turn to face player
		if (loc < 0 && !canMelee)
		{
			transform.rotation = Quaternion.LookRotation(transform.forward * -1);
		}

		if (dist < alertZone)
		{
			canSee = true;
		}
		else
		{
			canSee = false;
		}

		//player is a different color so do attack stuff
		if (steamChangeAbility.CurrentColorType() != enemyColorType.ToString())
		{
			//alerted of player
			//if (dist < alertZone)
			if (canSee)
			{
				//canSee = true;

				//check if in range to shoot
				if (dist < shootRange)
				{
					//check to see if player is in view to shoot
					if (Physics.Linecast(transform.position, playerTarget.position, out lineHitInfo, layers) && !canMelee)
					{
						//we are in range so start shooting and deal damage
						if (lineHitInfo.transform.tag == "Player")
						{
							alertLight.gameObject.SetActive(true);
							canShoot = true;

							PlayerHealth.instance.currentAttackingEnemy = transform.gameObject;
							currentTurretShooting = transform;

							Debug.DrawLine(transform.position, player.transform.position, Color.yellow);
						}
						//no longer in range so we can't shoot
						else
						{
							canShoot = false;
							Debug.DrawLine(transform.position, player.transform.position, Color.blue);
						}
					}
				}

				if (detectPlayerLevel.isOnSameLevel)
				{
					//move forward to the vantage point
					if (dist > vantageZone)
					{
						isTooClose = false;
						//keep moving forward if no barrier is detected
						if (!isBarrierInFront && !canMelee)
						{
							curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, moveSpeed, acceleration * Time.deltaTime);
						}
						//stop moving as a barrier is detected
						if (isBarrierInFront && !canRecoil)
						{
							curMoveSpeed = 0;
						}
					}
					//player is too close
					if (dist < personalBubble)
					{
						canMoveFromPlayer = true;
					}
					//the enemy is far enough away to stop but too close to move forward
					//so stop moving
					if (dist < vantageZone && dist > comfortZone)
					{
						canMoveFromPlayer = false;
					}
	//				if (canMoveFromPlayer)
					if (dist < personalBubble)
					{
						//move away from player
						if (!detectedObject)
						{
							isTooClose = true;
							//keep moving away if no barrier is behind
							if (!isBarrierBehind && !canMelee)
							{
								moveDir = -1;
								curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, moveSpeed * moveDir, acceleration * Time.deltaTime);
							}
							//stop as there is a barrier behind
							if (isBarrierBehind && !canRecoil)
							{
								curMoveSpeed = 0;
							}
						}
					}
	//				else
					if (dist < vantageZone && dist > comfortZone)
					{
						//stop moving
	//					if (dist < vantageZone)
						if (!isBarrierBehind && !canMelee && !canRecoil)
						{
							curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, 0, deceleration * Time.deltaTime);
						}
					}
				}
			}
			//out of range so turret cannot see the player nor fire any rockets
			else
			{
				curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, 0, deceleration * Time.deltaTime);
				canShoot = false;
				//canSee = false;
			}
//			if (enemyHealth.isHit)
//			{
//				ImpactReaction();
//				//curMoveSpeed = recoilSpeed;
//				enemyHealth.isHit = false;
//			}
			lastPosOnPatrol = transform.position;
		}
		//is the same color so cannot see the player nor fire any rockets
		else
		{
			if (!canMelee && !canRecoil)
			{
				curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, 0, deceleration * Time.deltaTime);
			}
			canSee = false;
			canShoot = false;
		}

		RaycastHit hitInfo;

		//check for another turret/enemy of player in front
		if (Physics.Raycast(transform.position, transform.forward, out hitInfo, vantageZone, ignoreCollisionLayer))
		{
			if (hitInfo.transform.tag == "Enemy")
			{
				detectedObject = hitInfo.transform;
			}
			else
			{
				detectedObject = null;
			}
		}
		else
		{
			detectedObject = null;
		}

		//has detected another enemy
		if (detectedObject)
		{
			//Vector3 enemyDiff = detectedObject.transform.position - transform.position;
			float enemyDist = Vector3.Distance(transform.position, detectedObject.position);

			//enemy is too close so move away
			if (enemyDist < personalBubble)
			{
				canMoveFromEnemy = true;
			}
			//the enemy is far enough away to stop but too close to move forward
			//so stop moving
			if (enemyDist > comfortZone && enemyDist < vantageZone)
			{
				canMoveFromEnemy = false;
			}
			//player is in range and enemy is far enough away from another enemy so move forward towards the player
			if (canSee && enemyDist > vantageZone && !canMelee)
			{
				curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, moveSpeed, acceleration * Time.deltaTime);
			}
			if (canMoveFromEnemy)
			{
				isTooClose = true;
				//move away from enemy as long as no barrier is detected
				if (!isBarrierBehind && !canMelee)
				{
					moveDir = -1;
					curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, moveSpeed * moveDir, acceleration);
				}
				//stop moving as a barrier is detected
				if (isBarrierBehind && !canRecoil)
				{
					curMoveSpeed = 0;
				}
			}
			//slow to a stop
			else
			{
				if (!isBarrierBehind && !canMelee && !canRecoil)
				{
					curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, 0, deceleration * Time.deltaTime);
				}
			}
		}

		if (isGrounded)
		{
			//move turret
			Vector3 vel = rb.velocity;
			vel = transform.forward * curMoveSpeed;
			vel.y = rb.velocity.y;
			rb.velocity = vel;

			//if turret stops lock rigidbody
			if (curMoveSpeed == 0)
			{
				rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX |
					RigidbodyConstraints.FreezeRotation;
			}
			//able to move so unlock rigidbody
			else
			{
				rb.constraints &= ~RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
				rb.constraints = RigidbodyConstraints.FreezeRotation;
			}
		}
	}

	void ImpactReaction()
	{
		//enemy is hit by player so recoil each attack
		if (canRecoil)
		{
			curMoveSpeed = Mathf.Lerp(curMoveSpeed, -recoilSpeed, recoilAcceleration * Time.deltaTime);

			if (curMoveSpeed <= -recoilSpeed)
			{
				canRecoil = false;
			}
		}
		else
		{
			if (meleeSystem.curAttackLimit == meleeSystem.attackLimit && enemyHealth.isHit)
			{
				canRecoil = true;
			}
		}
		//curMoveSpeed = Mathf.MoveTowards(curMoveSpeed, 0, deceleration * Time.deltaTime);
	}

	void Patrol()
	{
		if (canSee || canMoveFromEnemy || inRangeForMelee || canMelee || canRecoil)
		{
			isPatrolling = false;
			return;
		}

		RaycastHit hitInfo;

		int layerMask = 1 << 12;
		layerMask = ~layerMask;

		if (!isPatrolling)
		{
			//check right side
			if (Physics.Raycast(transform.position, Vector3.right, out hitInfo, maxDistForWaypointSpawn))
			{
				//if we detect an obstruction, spawn the right way point slightly closer to the turret from that obstruction
				waypoints[0].position = new Vector3(transform.position.x + (hitInfo.distance - offsetDisForWaypointSpawn), transform.position.y, transform.position.z);
			}
			else
			{
				//if we didn't detect an obstruction, spawn at a set distance
				waypoints[0].position = new Vector3(transform.position.x + (maxDistForWaypointSpawn - offsetDisForWaypointSpawn), transform.position.y, transform.position.z);
			}
			//check left side
			if (Physics.Raycast(transform.position, Vector3.left, out hitInfo, maxDistForWaypointSpawn))
			{
				waypoints[1].position = new Vector3(transform.position.x - (hitInfo.distance - offsetDisForWaypointSpawn), transform.position.y, transform.position.z);
			}
			else
			{
				waypoints[1].position = new Vector3(transform.position.x - (maxDistForWaypointSpawn - offsetDisForWaypointSpawn), transform.position.y, transform.position.z);
			}
			isPatrolling = true;
		}
		else
		{
			//wait time before moving to next waypoint
			if (curPatrolTime > 0)
			{
				curPatrolTime -= Time.deltaTime;
			}
			//wait time is over
			else
			{
				//once we reached the position of the current waypoint
				if (transform.position.x == waypoints[curWaypointIndex].position.x)
				{
					//choose a random time to stay at this spot
					curPatrolTime = Random.Range(minPatrolTime, maxPatrolTime);

					//count for the next waypoint
					if (curWaypointIndex < waypoints.Length - 1)
					{
						curWaypointIndex++;
					}
					else
					{
						curWaypointIndex--;
					}
				}
				//move to the current waypoint
				transform.position = Vector3.MoveTowards(transform.position,
					new Vector3(waypoints[curWaypointIndex].position.x, transform.position.y, transform.position.z), patrolSpeed * Time.deltaTime);	
			}
		}
		Debug.DrawRay(transform.position, Vector3.right * maxDistForWaypointSpawn, Color.green);
		Debug.DrawRay(transform.position, Vector3.left * maxDistForWaypointSpawn, Color.red);

		#region Old Code
		//if things are hindering us from our patrol functions kick out of this method
//		if (canSee || (isBarrierBehind || isBarrierInFront) ||
//			canMoveFromEnemy || inRangeForMelee || canMelee)
//		{
//			return;
//		}
//
//		//we have reached the end of moving one direction
//		if (!isPatrolling)
//		{
//			//start counting down time to move the other direction
//			if (curPatrolTime > 0)
//			{
//				curPatrolTime -= Time.deltaTime;
//			}
//			//time to move again
//			else
//			{
//				curPatrolTime = 0;
//				//make sure to actually move the opposite direction
//				if (moveDir == 1)
//				{
//					moveDir = -1;
//				}
//				else
//				{
//					moveDir = 1;
//				}
//				//randomize the range of patrol
//				rndPatrolDur = Random.Range(2, 4);
//				lastPosOnPatrol = transform.position;
//				isPatrolling = true;
//			}
//		}
//		//we are going to actually move now
//		else
//		{
//			transform.position += Vector3.right * patrolSpeed * moveDir * Time.deltaTime;
//			transform.rotation = Quaternion.LookRotation(Vector3.right * moveDir);
//
//			if (rndPatrolDur > 0)
//			{
//				rndPatrolDur -= Time.deltaTime;
//			}
//			else
//			{
//				curPatrolTime = patrolTime;
//				isPatrolling = false;
//			}
//		}
		#endregion
	}

	public void Boom()
	{
		//enemy dies
		//activate destroyed turret object
		if (enemyHealth.health <= 0)
		{
			Instantiate(explosion, explosionTarget.position, Quaternion.identity);
			Instantiate(debris, transform.position, Quaternion.identity);

			//Camera.main.GetComponent<PerlinShake>().PlayShake();
			Camera.main.GetComponent<RandomShake>().PlayShake();
//			Camera.main.GetComponent<PeriodicShake>().PlayShake();

			turretExplode.Explode();
			detectEnemyEncounter.canActivateEncounter = false;
			Destroy(turretModel);
			Destroy(gameObject);
		}
		//enemy still alive
		//ignore this method
		else
		{
			return;
		}

		//set an explosion force on select pieces
		foreach(Transform part in turretPieces.Skip(2))
		{
			part.GetComponent<MeshCollider>().isTrigger = false;
			part.GetComponent<Rigidbody>().isKinematic = false;
			part.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, transform.position, explosionRadius, upwardsExplosionMod, ForceMode.Impulse);
		}
	}

	void DetectWall()
	{
		RaycastHit hitInfo;
		Ray ray = new Ray(transform.position, transform.forward);

		int layerMask = 1 << 12;
		layerMask = ~layerMask;

		if (!isTooClose)
		{
			//check for barrier in front
			//if detected, turn around
			if (Physics.Raycast(transform.position, transform.forward, out hitInfo, rayDist))
			{
				if (hitInfo.collider.transform.tag == "Barrier")
				{
					Debug.Log("hit wall");

					if (moveDir == 1)
					{
						moveDir = -1;
					}
					else
					{
						moveDir = 1;
					}
					if (isGrounded)
					{
						transform.rotation = Quaternion.LookRotation(Vector3.right * moveDir);
					}
					isBarrierInFront = true;
				}
			}
			//no barrier is in front
			else if (isBarrierInFront)
			{
				isBarrierInFront = false;
			}
			//isBarrierBehind = false;
		}
		else
		{
			//check for barrier behind
			if (Physics.Raycast(transform.position, -transform.forward, out hitInfo, rayDist))
			{
				//also check to see if there's an enemy as we are assuming that enemy is already backed into a wall.
				if (hitInfo.collider.transform.tag == "Barrier" || hitInfo.collider.transform.tag == "Enemy")
				{
					isBarrierBehind = true;
				}

			}
			//no barrier is behind
			else if (isBarrierBehind)
			{
				isBarrierBehind = false;
			}

		//isBarrierInFront = false;
		}

	}

	void FlipUpright()
	{
		if (!isGrounded)
		{
			Quaternion flipRotation = Quaternion.Lerp(transform.rotation,
				Quaternion.LookRotation(new Vector3(0, 90 , 0)), flipUprightSpeed * Time.deltaTime);
			
			rb.constraints &= ~RigidbodyConstraints.FreezePositionX | ~RigidbodyConstraints.FreezePositionZ;
			rb.constraints &= ~RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

			if (transform.rotation.x < upsidedownRotationThreshold)
			{
				//play flip animation
				transform.rotation = flipRotation;
			}
			if (transform.rotation.x > upsidedownRotationThreshold && transform.rotation.x < 0)
			{
				transform.rotation = flipRotation;
			}
		}
		else
		{
			if (transform.rotation.x == 0)
			{
				//transform.rotation = Quaternion.LookRotation(Vector3.right * moveDir);
			}
		}
	}

	void CheckGround()
	{
		if (Physics.Raycast(transform.position, Vector3.down, groundCheckDist))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
	}

	Vector3 GetBottomCenter()
	{
		return GetComponent<Collider>().bounds.center + GetComponent<Collider>().bounds.extents.y * Vector3.down;
	}

	void CalculateDirectionVelocity()
	{
		curDirVel = (transform.position - preLoc) / Time.deltaTime;
		preLoc = transform.position;
	}
}
