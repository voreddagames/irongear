using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
    public enum EnemyColorType
    {
        Red,
        Blue,
        Yellow
    }

    public static Turret instance;

//    public static Transform currentTurretShooting;

    public EnemyColorType enemyColorType;

	public float moveSpeed = 15.0f;
	public float recoilSpeed = 5.0f;
	public float slowDownSpeed = 10.0f;
	public float turretSpeed;

	public float slowDist = 5.0f;
	public float attackRange = 10.0f;
	public float offSetFromPlayer = 15.0f;
	public float offsetFromEnemy = 20.0f;
//	public float personalBubble = 5.0f;

	public float pushBackForce = 30.0f;
	public float pushBackDistance = 10.0f;

	public float explosionPower = 10.0f;
	public float explosionRadius = 5.0f;
	public float upwardsExplosionMod = 2.0f;

	public Transform explosionTarget;

	public GameObject explosion;
	public GameObject debris;
	public GameObject explodedTurret;
	public Rigidbody[] turretPieces;

//	[HideInInspector]
	public float curSpeed;
	public float forwardSpeed = 0.0f;
	public float newPos;

//	public float curOffsetFromPlayer = 0.0f;
//	public float curOffsetFromEnemy;

    public bool isOnRightEdge = false;
	public bool isOnLeftEdge = false;

	private Vector3 preLoc;
	public Vector3 curDirVel;
	public Vector3 lastPos;
	public Vector3 lastLoc;

    public int faceDirection = 1;
	public int moveDir = 1;
	public int velDir = 1;
	public int dirToMove = 1;

	private float characterWidth;
	private float refVel = 0.0f;
	public float curEnemyOffsetPos;

//	private float distToLeftObject;
//	private float distToRightObject;

    public bool canSee = false;
	public bool isTooClose = false;
	public bool hasDetectedObject = false;
	public bool isKnockBack = false;
	public bool isOnBarrier = false;
	public bool isInRange = false;

    private SteamChangeAbility steamChangeAbility;
	private EnemyHealth enemyHealth;
	private TurretExplode turretExplode;
//	private PlayerPhysics playerPhysics;
	private MeleeSystem meleeSystem;

    private GameObject playerTarget;
	public GameObject closestObject;
//	public GameObject curClosestEnemy;
//	public GameObject lastClosestObject;
//	public GameObject objectOnLeft;

	private int layer = 1 << 11;

	private Rigidbody rb;

	//getters and setters
    public int FaceDirection() { return faceDirection; }
	public int MoveDirection() { return velDir; }
	public bool CanSee() { return canSee; }
    public string ColorType() { return enemyColorType.ToString(); }

    void Awake()
    {
        instance = this;

        playerTarget = GameObject.FindWithTag("Player");

//		playerPhysics = playerTarget.GetComponent<PlayerPhysics>();
		meleeSystem = playerTarget.GetComponent<MeleeSystem>();
        steamChangeAbility = playerTarget.GetComponent<SteamChangeAbility>();
		enemyHealth = GetComponent<EnemyHealth>();
		turretExplode = explodedTurret.GetComponent<TurretExplode>();
    }

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		forwardSpeed = moveSpeed;
		turretSpeed = moveSpeed - 5;
		lastPos = transform.position;
		turretPieces = explodedTurret.GetComponentsInChildren<Rigidbody>();

		//ignore anything that has the same Layer as this object
		layer = ~layer;

		CalculateBounds();
	}

	void CalculateBounds()
	{
		characterWidth = GetComponent<Collider>().bounds.size.z;
	}

	void Update()
	{
		Boom();
	}

    void FixedUpdate()
    {
        GetMotion();
		DetectPlayerPosition();
		CalculateDirectionVelocity();
//		ImpactReaction();
    }

	void LateUpdate()
	{
		DetectGround();
	}
	
	void DetectPlayerPosition()
	{
		RaycastHit hitInfo;

		//check for player behind enemy/on right side
		if (Physics.Raycast(transform.position + Vector3.up, -transform.forward, out hitInfo, attackRange, layer))
		{
//			if (!isPlayerOnLeft)

			//we detected something on the right side, but it is not the player
			if (!hasDetectedObject)
			{
				faceDirection = -1;

//				if (!canDetectPlayer && hitInfo.transform.tag != "Player")
//				{
					closestObject = hitInfo.transform.gameObject;

//				if (closestObject.tag == "Enemy" && dirToMove == 1)
//				{
//					curClosestEnemy = closestObject;
//				}
//				if (hitInfo.transform.tag == "Player")
//				{
//					curClosestEnemy = null;
//				}
//				}
//				if (canDetectPlayer && hitInfo.transform.tag == "Player")
//				{
//					closestObject = playerTarget;
//				}
//					isPlayerOnRight = true;
//				}
				hasDetectedObject = true;
			}
			//we detected the player
			if (hitInfo.transform.tag == "Player")
			{
				closestObject = playerTarget;
			}
			//now we know it is the player and not something else
			else if (canSee)
			{
				hasDetectedObject = false;
			}
		}
//		else
//		{
//			if (isPlayerOnRight)
//			{
//				isPlayerOnRight = false;
//			}
//		}

		//check for player in front of enemy/on left side
		if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hitInfo, attackRange, layer))
		{
//			if (!isPlayerOnRight)

			//we detected something other than the player
			if(!hasDetectedObject)
			{
				faceDirection = 1;
//				if (!canDetectPlayer && hitInfo.transform.tag != "Player")
//				{
					closestObject = hitInfo.transform.gameObject;

//				if (closestObject.tag == "Enemy" && dirToMove == -1)
//				{
//					curClosestEnemy = closestObject;
//				}
//				if (hitInfo.transform.tag == "Player")
//				{
//					curClosestEnemy = null;
//				}
//				}
//				if (canDetectPlayer && hitInfo.transform.tag == "Player")
//				{
//					closestObject = playerTarget;
//				}

//				if (hitInfo.transform.tag == "Player")
//				{
//					isPlayerOnLeft = true;
//				}
				hasDetectedObject = true;
			}

			//we detected the player and not something else
			if (hitInfo.transform.tag == "Player")
			{
				closestObject = playerTarget;
			}
			else if (canSee)
			{
				hasDetectedObject = false;
			}

		}
//		else
//		{
//			if (isPlayerOnLeft)
//			{
//				isPlayerOnLeft = false;
//			}
//		}

		Debug.DrawRay(transform.position + Vector3.up, transform.forward * attackRange, Color.blue);
		Debug.DrawRay(transform.position + Vector3.up, -transform.forward * attackRange, Color.red);

		//player is on right side
		if (faceDirection == -1)
		{
			if (!isOnLeftEdge && !isOnRightEdge)
			{
				moveDir = -1;
				forwardSpeed = moveSpeed;
			}
			//the player is too close so assume player is moving towards
			if (isTooClose)
			{
				//if a gap is between the turret and the player
				//but player is moving towards the turret
				//move back/to the left
				if (isOnRightEdge)
				{
					moveDir = -1;
					forwardSpeed = moveSpeed;
				}
				//backed to a ledge so stop moving
				if (isOnLeftEdge)
				{
					forwardSpeed = 0;
				}
			}
			//player is not too close so assume player is moving away
			else
			{
				//has moved forward to a ledge so stop moving
				if (isOnRightEdge)
				{
					forwardSpeed = 0;
				}
				//if turret was backed into a ledge
				//but player is moving away from the turret
				//move forward and to the right
				//chasing the player
				if (isOnLeftEdge)
				{
					moveDir = 1;
					forwardSpeed = moveSpeed;
				}
			}
		}
		//player is on left side
		else
		{
			if (!isOnLeftEdge && !isOnRightEdge)
			{
				moveDir = 1;
				forwardSpeed = moveSpeed;
			}
			//the player is too close so assume player is moving towards
			if (isTooClose)
			{
				//backed to a ledge so stop moving
				if (isOnRightEdge)
				{
					forwardSpeed = 0;
				}
				//if a gap is between the turret and the player
				//but player is moving towards the turret
				//move back and to the right
				if (isOnLeftEdge)
				{
					moveDir = 1;
					forwardSpeed = moveSpeed;
				}
			}
			//player is not too close so assume player is moving away
			else
			{
				//if turret was backed into a ledge
				//but player is moving away from the turret
				//move forward and to the left
				//chasing the player
				if (isOnRightEdge)
				{
					//set turret to move left
					moveDir = -1;
					forwardSpeed = moveSpeed;
				}
				//has moved forward to a ledge so stop moving
				if (isOnLeftEdge)
				{
					forwardSpeed = 0;
				}
			}
		}
	}

    void GetMotion()
    {
		if (!closestObject)
		{
			return;
		}

		#region Save for now
//		if (objectOnLeft)
//		{
//			distToLeftObject = Vector3.Distance(transform.position, objectOnLeft.transform.position);
//
//			float newPos = Mathf.SmoothDamp(transform.position.x, objectOnLeft.transform.position.x + (offSetFromPlayer * faceDirection), ref refVel, forwardSpeed * Time.deltaTime);
//			
//			if (forwardSpeed != 0)
//			{
//				transform.position = new Vector3(newPos, transform.position.y, 0);
//			}
//		}
//		if (objectOnRight && objectOnRight != playerTarget)
//		{
//			float newPos = Mathf.SmoothDamp(transform.position.x, objectOnRight.transform.position.x + (offSetFromPlayer * faceDirection), ref refVel, forwardSpeed * Time.deltaTime);
//			
//			if (forwardSpeed != 0)
//			{
//				transform.position = new Vector3(newPos, transform.position.y, 0);
//			}
//			distToRightObject = Vector3.Distance(transform.position, objectOnRight.transform.position);
//		}
		#endregion

		float distToPlayer = Vector3.Distance(transform.position, playerTarget.transform.position);
		float distToClosestObject = Vector3.Distance(transform.position, closestObject.transform.position);

		//player is detected
        if (distToPlayer <= attackRange && steamChangeAbility.CurrentColorType() != enemyColorType.ToString())
        {
        	//there is an object that is close to us but is not another enemy
			if (closestObject.tag != "Enemy")
			{
				canSee = true;
			}
			else
			{
				canSee = false;
			}

			if (closestObject == playerTarget)
			{
//				currentTurretShooting = transform;

				//move enemy and keep a distance from player
				newPos = Mathf.SmoothDamp(newPos, turretSpeed * PlayerPhysics.instance.faceDir, ref refVel, forwardSpeed * Time.deltaTime);
//				float newPos = Mathf.SmoothDamp(transform.position.x, playerTarget.transform.position.x + (offSetFromPlayer * faceDirection), ref refVel, forwardSpeed * Time.deltaTime);

				//set that distance to keep from the player
				if (distToPlayer <= offSetFromPlayer)
				{
					rb.velocity = -transform.forward * newPos;
//					rb.velocity = new Vector3(newPos, rb.velocity.y, 0);
				}
//				if (forwardSpeed != 0)
//				{
//					transform.position = new Vector3(newPos, transform.position.y, 0);
//				}
			}

			//set the values to recoil the enemy when player attacks
			if (enemyHealth.isHit)
			{
				forwardSpeed = recoilSpeed;
				enemyHealth.isHit = false;
			}
			lastPos = transform.position;
        }
		//lost sight of player
        else
        {
            canSee = false;

			if (closestObject == playerTarget)
			{
				//slow to a stop
//				float newPos = Mathf.SmoothDamp(transform.position.x, lastPos.x + (slowDist * velDir), ref refVel, slowDownSpeed * Time.deltaTime);
//				transform.position = new Vector3(newPos, transform.position.y, 0);

				if (hasDetectedObject)
				{
					hasDetectedObject = false;
				}
			}
        }

		if ((closestObject != playerTarget && closestObject.tag != "Barrier") && distToPlayer > offSetFromPlayer)
		{
			//move enemy and keep a distance from whatever object is closest
			newPos = Mathf.SmoothDamp(newPos, turretSpeed, ref refVel, forwardSpeed * Time.deltaTime);
//			float newPos = Mathf.SmoothDamp(transform.position.x, closestObject.transform.position.x + (offSetFromPlayer * moveDir), ref refVel, forwardSpeed * Time.deltaTime);
			
//			if (distToClosestObject <= offSetFromPlayer)
//			{
//				rb.velocity = transform.forward * newPos;
//				rb.velocity = new Vector3(newPos, rb.velocity.y, 0);
//			}
			lastPos = transform.position;
		}

		#region save this for now
//		if (distToPlayer <= attackRange)
//		{
////			isPlayerOnLeft = false;
////			isPlayerOnRight = false;
//		}
////		}
//		Vector3 forwardDir = transform.TransformDirection(Vector3.forward);
//		Vector3 dirToPlayer = playerTarget.transform.position - transform.position;
//		float playerPos = Vector3.Dot(forwardDir, dirToPlayer);
//
//		if (playerPos > 0)
//		{
//			dirToMove = 1;
//		}
//		else
//		{
//			dirToMove = -1;
//		}
//		if (closestObject != playerTarget && curClosestEnemy && !canSee)
//		{
//			//player is in front of turret/on left side
//			if (dirToMove == 1)
//			{
//				if (canDetectPlayer)
//				{
////					isPlayerOnLeft = false;
//				}
////				dirToMove = 1;
//
//				float clampVel = Mathf.Clamp(curDirVel.x, -1, 1);
//
//				if (clampVel < 0)
//				{
//					print ("or are you here?  cause i dunno man");
//					curEnemyOffsetPos = lastLoc.x;
//					//					curClosestEnemy = transform.gameObject;
//					curOffsetFromEnemy = 0;
//					//					curEnemyOffsetSpeed = 0;
//				}
//				else
//				{
//					print ("how about here?  are you here?");
//					lastLoc = transform.position;
//					curEnemyOffsetPos = curClosestEnemy.transform.position.x;
//					//					curClosestEnemy = closestObject;
//					curOffsetFromEnemy = offsetFromEnemy;
//				}
//				Vector3 newPos = new Vector3(Mathf.SmoothDamp(transform.position.x, curEnemyOffsetPos + (curOffsetFromEnemy * dirToMove), ref refVel, forwardSpeed * Time.deltaTime),
//				                             transform.position.y, 0);
//				//				if (curEnemyOffsetSpeed != 0)
//				//				{
//				transform.position = newPos;
////				Vector3 newPos = new Vector3(Mathf.SmoothDamp(transform.position.x, closestObject.transform.position.x + (offSetFromPlayer * dirToMove), ref refVel, forwardSpeed * Time.deltaTime),
////				                             transform.position.y, 0);
////				if (forwardSpeed != 0)
////				{
////					transform.position = newPos;
////				}
//			}
//			//player is behind turret/on right side
//			else
//			{
////				dirToMove = -1;
//
////				Vector3 newPos = new Vector3(Mathf.SmoothDamp(transform.position.x, curClosestEnemy.transform.position.x + (curOffsetFromEnemy * dirToMove), ref refVel, forwardSpeed * Time.deltaTime),
////				                             transform.position.y, 0);
//////				if (curEnemyOffsetSpeed != 0)
//////				{
////				transform.position = newPos;
////				}
//
//				float clampVel = Mathf.Clamp(curDirVel.x, -1, 1);
//
//				if (canDetectPlayer)
//				{
////					isPlayerOnRight = false;
//				}
//
//				if (clampVel > 0)
//				{
//					print ("so are you here then?");
//					curEnemyOffsetPos = lastLoc.x;
////					curClosestEnemy = transform.gameObject;
//					curOffsetFromEnemy = 0;
////					curEnemyOffsetSpeed = 0;
//				}
//				else
//				{
//					print("are you here?");
//					lastLoc = transform.position;
//					curEnemyOffsetPos = curClosestEnemy.transform.position.x;
////					curClosestEnemy = closestObject;
//					curOffsetFromEnemy = offsetFromEnemy;
////					curEnemyOffsetSpeed = forwardSpeed;
//				}
//				Vector3 newPos = new Vector3(Mathf.SmoothDamp(transform.position.x, curEnemyOffsetPos + (curOffsetFromEnemy * dirToMove), ref refVel, forwardSpeed * Time.deltaTime),
//				                             transform.position.y, 0);
//				//				if (curEnemyOffsetSpeed != 0)
//				//				{
//				transform.position = newPos;
//				//				}
//			}
//
////			Vector3 newPos = new Vector3(Mathf.SmoothDamp(transform.position.x, closestObject.transform.position.x + (offSetFromPlayer * dirToMove), ref refVel, forwardSpeed * Time.deltaTime),
////			                             transform.position.y, 0);
////
////			if (forwardSpeed != 0)
////			{
////				transform.position = newPos;
////			}
//		}

#endregion

		//object is closer than offset
        if (distToClosestObject <= offSetFromPlayer)
        {
			isTooClose = true;
        }
        else
        {
            isTooClose = false;
        }
//		if (!meleeSystem.hasAttacked)
//		{
//			lastPos = transform.position;
//		}
    }

	void ImpactReaction()
	{
//		if (isHit)
//		{
//			transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, lastPos.x + (pushBackDistance * faceDirection), pushBackForce * Time.deltaTime),
//			                                 transform.position.y, transform.position.z);
//		}
//		if (meleeSystem.canAttack)
//		{
//			isHit = false;
//		}

		//now actually make the turret recoil
		if (meleeSystem.curAttackLimit == 0 && enemyHealth.isHit)
		{
			transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, lastPos.x + (pushBackDistance * moveDir), pushBackForce * Time.deltaTime),
			                                 transform.position.y, transform.position.z);

			enemyHealth.isHit = false;
		}

		if (enemyHealth.isHit)
		{

		}
	}

	void DetectGround()
	{
		float dist = 4.0f;

		float halfWidth = characterWidth * 0.5f;

		//front
		Vector3 origin0 = GetBottomCenter() + transform.forward * halfWidth + transform.up;
		//back
		Vector3 origin1 = GetBottomCenter() + -transform.forward * halfWidth + transform.up;

		Vector3 dir = Vector3.down;

		Debug.DrawRay(origin0, dir * dist, Color.red);
		Debug.DrawRay(origin1, dir * dist, Color.blue);

		RaycastHit hitInfo;

		//near right edge
		if (!Physics.Raycast(origin1, dir, out hitInfo, dist, layer))
		{
			isOnRightEdge = true;
		}
		//not on edge
		else
		{
			if (hitInfo.transform.tag != "Raycast Ignore")
			{
				isOnRightEdge = false;
			}
		}
		//near left edge
		if (!Physics.Raycast(origin0, dir, out hitInfo, dist, layer))
		{
			isOnLeftEdge = true;
		}
		else
		{
			if (hitInfo.transform.tag != "Raycast Ignore")
			{
				isOnLeftEdge = false;
			}
		}

		//fall off edge
		if (isOnLeftEdge && isOnRightEdge)
		{
			enemyHealth.health = 0;
		}
	}

	void Boom()
	{
		//enemy dies
		//activate destroyed turret object
		if (enemyHealth.health <= 0)
		{
			Instantiate(explosion, explosionTarget.position, Quaternion.identity);
			Instantiate(debris, transform.position, Quaternion.identity);
			turretExplode.Explode();
			Destroy(gameObject);
		}
		//enemy still alive
		//ignore this method
		else
		{
			return;
		}
		//set an explosion force on select pieces
		foreach(Rigidbody part in turretPieces)
		{
			part.isKinematic = false;
			part.AddExplosionForce(explosionPower, transform.position, explosionRadius, upwardsExplosionMod, ForceMode.Impulse);
		}
	}

	Vector3 GetBottomCenter()
	{
		return GetComponent<Collider>().bounds.center + GetComponent<Collider>().bounds.extents.y * Vector3.down;
	}

	void CalculateDirectionVelocity()
	{
		curDirVel = (transform.position - preLoc) / Time.deltaTime;

		if (canSee)
		{
			if (curDirVel.x > 1)
			{
				//going right
				curSpeed = forwardSpeed;
				velDir = 1;
			}
			else if (curDirVel.x < -1)
			{
				//going left
				curSpeed = forwardSpeed;
				velDir = -1;
			}
			else
			{
				//not moving
				curSpeed = 0;
				velDir = 0;
			}
		}
		preLoc = transform.position;
	}
}
