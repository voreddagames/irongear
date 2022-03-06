using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public static PlayerHealth instance;

	public float curMaxHealth;
	public float health = 6;
	public int damage = 1;

    public int tier1Health = 6;
	public int tier2Health = 4;
	public int tier3Health = 2;
	public int tier4Health = 1;

	public int tier1EnemyDmg = 3;
	public int tier2EnemyDmg = 4;
	public int tier3EnemyDmg = 6;

	public bool isHit = false;
	public bool isLevelingTier = false;
	public bool isDamaged = false;
	public bool isHitByTurretMelee = false;

	public ABC_StateManager abcStateManager;

	private PlayerPhysics playerPhysics;
	private BoilingPointsSystem boilingPointsSystem;
	private EnemyHealth enemyHealth;
	private TurretPhysics turretPhysics;
//	private PlayerAnimation playerAnimation;

	public GameObject currentAttackingEnemy;

	public int previousLevel = 0;

//	private bool canTurretMeleeDamage;

	public Rigidbody rb;

	// Use this for initialization
	void Awake ()
    {
		abcStateManager = transform.GetComponent<ABC_StateManager>();
		playerPhysics = GetComponent<PlayerPhysics>();
		boilingPointsSystem = GetComponent<BoilingPointsSystem>();

		instance = this;
//		playerAnimation = GetComponent<PlayerAnimation>();
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	public void UpdatePlayerHealth ()
    {
		transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        //AdjustHealth(0);

		if (health <= 0)
        {
            health = 0;

//			playerMotor.RemoveControl();
           // playerPhysics.IsDead = true;
        }
		DetectDamage();
		ModHitPoints();
	}

	void DetectDamage()
	{
		if (isDamaged && health > 0)
		{
			BroadcastMessage("ShoulderDamage", SendMessageOptions.DontRequireReceiver);
		}
//		if (isKnockedBack && health > 0)
//		{
//			BroadcastMessage("KnockBack", SendMessageOptions.DontRequireReceiver);
//		}
		if (currentAttackingEnemy)
		{
			enemyHealth = currentAttackingEnemy.GetComponent<EnemyHealth>();
			turretPhysics = currentAttackingEnemy.GetComponent<TurretPhysics>();

//			if (turretPhysics.isMeleeAttacking)
//			{
//				canTurretMeleeDamage = true;
//			}
		}
	}

	void ModHitPoints()
	{
		if (boilingPointsSystem.tierLevel != previousLevel)
		{
			//set the player's health for each tier
			switch(boilingPointsSystem.tierLevel)
			{
			case 1:
				curMaxHealth = tier1Health;
				break;
			case 2:
				curMaxHealth = tier2Health;
				break;
			case 3:
				curMaxHealth = tier3Health;
				break;
			case 4:
				curMaxHealth = tier4Health;
				break;
			}
			abcStateManager.AdjustMaxHealth(curMaxHealth);
			previousLevel = boilingPointsSystem.tierLevel;
		}
		

		if (isHit)
		{
		//set the damage in which the player will take according to the attacking enemy's level
			//tier level 1
			if (boilingPointsSystem.tierLevel == 1)
			{
				//attacked by level 1 enemy
				if (enemyHealth.tierLevel == 1)
				{
					damage = tier1EnemyDmg;
				}
				//attacked by level 2 enemy
				if (enemyHealth.tierLevel == 2)
				{
					damage = tier2EnemyDmg;
				}
				//attacked by level 3 enemy
				if (enemyHealth.tierLevel == 3)
				{
					damage = tier3EnemyDmg;
				}
			}
			//tier level 2
			if (boilingPointsSystem.tierLevel == 2)
			{
				if (enemyHealth.tierLevel == 1)
				{
					damage = tier1EnemyDmg;
				}
				if (enemyHealth.tierLevel == 2)
				{
					damage = tier2EnemyDmg;
				}
				if (enemyHealth.tierLevel == 3)
				{
					damage = tier3EnemyDmg;
				}
			}
			//tier level 3
			if (boilingPointsSystem.tierLevel == 3)
			{
				if (enemyHealth.tierLevel == 1)
				{
					damage = tier1EnemyDmg;
				}
				if (enemyHealth.tierLevel == 2)
				{
					damage = tier2EnemyDmg;
				}
				if (enemyHealth.tierLevel == 3)
				{
					damage = tier3EnemyDmg;
				}
			}
			//tier level 4
			if (boilingPointsSystem.tierLevel == 4)
			{
				if (enemyHealth.tierLevel == 1)
				{
					damage = tier1EnemyDmg;
				}
				if (enemyHealth.tierLevel == 2)
				{
					damage = tier2EnemyDmg;
				}
				if (enemyHealth.tierLevel == 3)
				{
					damage = tier3EnemyDmg;
				}
			}
			abcStateManager.AdjustHealth(-damage);
			//playerPhysics.moveSpeed = 5;
			isHit = false;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Bullet" && currentAttackingEnemy)
		{
//			currentAttackingEnemy = TurretPhysics.currentTurretShooting.gameObject;
//			enemyHealth = currentAttackingEnemy.GetComponent<EnemyHealth>();
			isDamaged = true;  //used for damage animation
			isHit = true;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		ContactPoint contact = col.contacts[0];
		
//		float aboveCheck = Vector3.Angle(contact.normal, -transform.up);
		float frontCheck = Vector3.Angle(contact.normal, -transform.forward);
		float backCheck = Vector3.Angle(contact.normal, transform.forward);

		//when the turret does the charge attack
		if (col.transform.tag == "Enemy" &&
		    currentAttackingEnemy && turretPhysics.isMeleeAttacking &&
		    (frontCheck < 60 || backCheck < 60))
		{
			//shake the camera
			Camera.main.GetComponent<PerlinShake>().PlayShake();
			//Camera.main.GetComponent<RandomShake>().PlayShake();
			//Camera.main.GetComponent<PeriodicShake>().PlayShake();

			abcStateManager.AdjustHealth((int)-turretPhysics.meleeDamage);

			isHitByTurretMelee = true;

			//hit the player back
			playerPhysics.moveSpeed = -20 * playerPhysics.faceDir;

			rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
			rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

//			canTurretMeleeDamage = false;
		}
	}

    /*public void AdjustHealth(int adj)
    {
        health += (float)adj;
    }*/
}
