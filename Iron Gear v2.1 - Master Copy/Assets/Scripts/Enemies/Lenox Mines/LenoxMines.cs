using UnityEngine;
using System.Collections;

public class LenoxMines : MonoBehaviour
{
    public enum ColorType
    {
        Red,
        Blue,
        Yellow
    }

    public ColorType colorType;

//	public GameObject rendererGO;
//	public Renderer rendererMat;

	public Light explosionWarningLight;

	public float distToMove = 10.0f;
	public float attackRange = 30.0f;
	public float damageRadius = 10.0f;
	public float explosionRadius = 5.0f;

	public float turnSpeed = 20.0f;
	public float speed = 10.0f;
	public float attackSpeed = 25.0f;

	public float detonationTime = 3.0f;
	public float timerAcceleration = 0.2f;
	public float minTimer = 0.5f;

	public float explosionForce = 10.0f;
	public float upwardsExplosionMod = 2.0f;

	public int detonationDamage = 6;

    public GameObject explosion;
	public GameObject detectionCone;

	public Vector3 desiredPos;

	public GameObject explodedMine;
	public Rigidbody[] minePieces;

    private GameObject player;

	public bool isAttacking = false;

    private SteamChangeAbility steamChangeAbility;
//    private PlayerHealth playerHealth;
	private DetectionLight detectionLight;
	private ExplodedLenoxMine explodedLenoxMine;
	private DetectEnemyEncounter detectEnemyEncounter;
	private CamPositioning camPositioning;

    public Vector3 initPos = Vector3.zero;
	public Vector3 startPoint;
	public Vector3 endPoint;
	private Vector3 preLoc;

	private Rigidbody rb;

	public float lastTime = 0.0f;
	private float startTime;
	public float attkSpeed = 0.0f;

	public float i;
	public int curMoveDir = 1;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");

        steamChangeAbility = player.GetComponent<SteamChangeAbility>();
//        playerHealth = player.GetComponent<PlayerHealth>();
		detectionLight = detectionCone.GetComponent<DetectionLight>();
		explodedLenoxMine = explodedMine.GetComponent<ExplodedLenoxMine>();
		camPositioning = Camera.main.GetComponent<CamPositioning>();
    }

	// Use this for initialization
	void Start ()
    {
		startTime = Time.time;

		rb = GetComponent<Rigidbody>();

		explosionWarningLight.enabled = false;

		//values for patrol movement
        initPos = transform.position;
		desiredPos = new Vector3(initPos.x + distToMove, initPos.y, initPos.z);
		startPoint = initPos;
		endPoint = desiredPos;

		//get the rigidbodies of the broken mine pieces for the explosion force
		minePieces = explodedMine.GetComponentsInChildren<Rigidbody>();
		detectEnemyEncounter = GetComponent<DetectEnemyEncounter>();
//		center = initPos + ((desiredPos - initPos) * 0.5f);
//		dir = (desiredPos - initPos).normalized;
	}

//	void OnDrawGizmosSelected()
//	{
//		Gizmos.color = Color.red;
//		Color alphaSphere = Gizmos.color;
//		alphaSphere.a = 0.5f;
//		Gizmos.color = alphaSphere;
//
//		Gizmos.DrawSphere(transform.position, damageRadius);
//	}

	void FixedUpdate ()
    {
        GetMotion();
		CurrentMoveDirection();
	}

    void GetMotion()
    {
        //find the distance on the Z plane
        float distToPlayer = player.transform.position.x - transform.position.x;

        //the player was detected by the search light so we can start attacking
        if (detectionLight.hasDetected)
        {
            isAttacking = true;
        }

        if (!isAttacking)
        {
            //patrol back and forth when not attacking
			i = (Time.time - startTime) / speed;

			transform.position = new Vector3(Mathf.SmoothStep(startPoint.x, endPoint.x, i),
			                                 transform.position.y, transform.position.z);

			if (i >= 1)
			{
				startTime = Time.time;

				startPoint = endPoint;

				if (endPoint == initPos)
				{
					endPoint = desiredPos;
				}
				else
				{
					endPoint = initPos;
				}
			}
        }
		//attacking
		else
		{
			//make sure the color is not the same
			//###TAKE OUT THE hasDetected BOOL CHECK###
			if (colorType.ToString() != steamChangeAbility.CurrentColorType() && detectionLight.hasDetected)
			{
				//look at player
				transform.LookAt(player.transform.position);
//				transform.position = Vector3.Lerp(transform.position, player.transform.position, attackSpeed * Time.deltaTime);
			}
			//###MAKE SURE THIS ELSE STATEMENT IS NECESSARY###
			else
			{
				detectionLight.hasDetected = false;
			}
			//chase the player
			attkSpeed = Mathf.MoveTowards(attkSpeed, attackSpeed, attackSpeed * Time.deltaTime);
			rb.velocity = transform.forward * attkSpeed;
//			else
//			{
//				transform.position += Vector3.right * (speed * 3) * curMoveDir * Time.deltaTime;
//			}

			//###MAKE SURE THIS PARAMETER IS NECESSARY###
			Timer (distToPlayer);
		}
//		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.right * curMoveDir), turnSpeed);
    }

    void Timer(float dist)
    {
		if (Time.time - lastTime > detonationTime)
		{
			lastTime = Time.time;
			
			//blink the mine
			ToggleFlash();
			
			//speed up the blinking
			detonationTime -= timerAcceleration;
		}
		//time is up.  explode now and check for damage
		if (detonationTime <= minTimer)
		{
			DamagePlayer();
			Explode ();
		}
    }

	void DamagePlayer()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);

		//harm the player if player is in range of explosion
		int i = 0;
		while (i < hitColliders.Length)
		{
			if (hitColliders[i].tag == "Player")
			{
				hitColliders[i].GetComponent<ABC_StateManager>().AdjustHealth(-detonationDamage);
			}
			i++;
		}
	}

	void Explode()
	{
		//enemy dies so activate destroyed turret object
		Instantiate(explosion, transform.position, Quaternion.identity);

		explodedLenoxMine.Explode();
		camPositioning.enemies.Remove(gameObject);
		//kill object
		Destroy(gameObject);

		//set an explosion force on select pieces
		foreach(Rigidbody part in minePieces)
		{
			part.isKinematic = false;
			part.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsExplosionMod, ForceMode.Impulse);
		}

	}

	void ToggleFlash()
	{
		if (explosionWarningLight.enabled)
		{
			explosionWarningLight.enabled = false;
		}
		else
		{
			explosionWarningLight.enabled = true;
		}
//		if (rendererMat.material.color == Color.blue)
//		{
//			rendererMat.material.color = Color.red;
//		}
//		else
//		{
//			rendererMat.material.color = Color.blue;
//		}
	}

	void OnTriggerEnter(Collider col)
	{
		//explode if player touches mine regardless of color or detection
		if (col.tag == "Player")
		{
			DamagePlayer();
			Explode();
		}
	}

	void CurrentMoveDirection()
	{
		Vector3 curDirVel = (transform.position - preLoc) / Time.deltaTime;
		
		if (curDirVel.x > 1)
		{
			//going right
			curMoveDir = 1;
		}
		else if (curDirVel.x < -1)
		{
			//going left
			curMoveDir = -1;
		}
		preLoc = transform.position;
	}
}
