using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class M3IRVDodgeMissile : MonoBehaviour
{
	public static M3IRVDodgeMissile instance;

	public enum ColorType
	{
		Red,
		Blue,
		Yellow
	}
	public ColorType colorType;

	public int damage = 2;
	public int maxRespawns = 5;

	public float speed = 30.0f;
	public float extraSpeed = 50.0f;
	public float rotSpeed = 15.0f;

	public float minDist = 2.0f;

	public float explodeDuration = 5.0f;

	public GameObject player;
	public GameObject explosion;
	public GameObject scorchMark;

	public bool hasLostTarget = false;
	public bool hasRespawned = false;

	private float explodeTimer;

	private int respawnCnt = 0;

	private SteamChangeAbility steamChangeAbility;
	private PlayerHealth playerHealth;
	private ABC_StateManager abcStateManager;

	private Quaternion startRot;
	private Vector3 startPos;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		explodeTimer = explodeDuration;
		startRot = transform.rotation;
		startPos = transform.position;

		player = GameObject.FindWithTag("Player");

		steamChangeAbility = player.GetComponent<SteamChangeAbility>();
		playerHealth = player.GetComponent<PlayerHealth>();
		abcStateManager = player.GetComponent<ABC_StateManager>();
	}

	// Update is called once per frame
	void Update ()
	{
		FlyRandomDirection();
		MissilePhysics();
		CollisionExplosion();
	}

	void MissilePhysics()
	{
		if (M3IRVLevelOneOutdated.instance.hasTriggeredDodgeMissiles)
		{
//			GetComponent<Rigidbody>().isKinematic = false;

			//start timer
			TimedExplosion();

			//missile has locked onto player
			if (!hasLostTarget)
			{
				//chase player
				transform.Translate(Vector3.forward * speed * Time.deltaTime);
				transform.rotation = Quaternion.Slerp(transform.rotation,
				                                      Quaternion.LookRotation(player.transform.position - transform.position),
				                                      rotSpeed * Time.deltaTime);
			}
		}
	}

	void FlyRandomDirection()
	{
		Vector3 offset = transform.position - player.transform.position;
		float magDist = offset.sqrMagnitude;
		float sqrDist = minDist * minDist;

		//player has matched the missile's color so the missile has lost player's position
		if (colorType.ToString() == steamChangeAbility.CurrentColorType() && magDist < sqrDist)
		{
			hasLostTarget = true;
		}
		if (hasLostTarget)
		{
			//veer off at a faster speed
			transform.Translate (Vector3.forward * extraSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(50.0f, Vector3.back) * startRot,
			                                      rotSpeed * Time.deltaTime);
		}
	}

	void TimedExplosion()
	{
		//counting down
		explodeTimer -= Time.deltaTime;

		//explode and respawn at M3IRV's hand
		if (explodeTimer <= 0)
		{
			Boom ();
			Respawn();
		}
	}

	void Boom()
	{
		Instantiate(explosion, transform.position, Quaternion.identity);
	}

	void CollisionExplosion()
	{
		RaycastHit hitInfo;

		//explode when hitting stuff
		if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1) && hasLostTarget)
		{
			Vector3 hitPoint = hitInfo.point;
			Quaternion hitRot = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);

			Boom ();
			Instantiate(scorchMark, hitPoint, hitRot);
			Respawn();
		}
	}

	void OnTriggerEnter(Collider col)
	{
		//damage player
		if (col.tag == "Player" && !hasLostTarget)
		{
			abcStateManager.AdjustHealth(-damage);

			Boom ();
			Respawn();
		}
	}

	void Respawn()
	{
		hasRespawned = true;

//		GetComponent<Rigidbody>().isKinematic = true;

		//count up the times the missile has respawned
		respawnCnt++;
		//move back to M3IRV's hand
		transform.position = startPos;

		//randomize the color of the rocket
		int rndColor = Random.Range(0, 2);

		//set the color
		switch(rndColor)
		{
		case 0:
			colorType = ColorType.Blue;
			break;
		case 1:
			colorType = ColorType.Red;
			break;
		case 2:
			colorType = ColorType.Yellow;
			break;
		}

		//reset the timer
		explodeTimer = explodeDuration;

		//respawned the maximum times so complete the QTE and destroy the rocket
		if (respawnCnt == maxRespawns)
		{
			M3IRVLevelOneOutdated.instance.isQTEComplete = true;
			Destroy(gameObject);
		}

		hasLostTarget = false;
	}
}
