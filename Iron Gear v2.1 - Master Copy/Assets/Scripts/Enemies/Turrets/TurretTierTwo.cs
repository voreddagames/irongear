using UnityEngine;
using System.Collections;

public class TurretTierTwo : TurretPhysics
{
	/* be alerted of player before shooting range.
	 * randomly choose between shooting up or down
	 * then shoot a digger mine
	 * if in shooting range, stop firing digger mines
	 * 
	 * digger mines, while underground, will follow player
	 * after a time the digger mines will stop following and shoot up (or down) to cause damage
	 * 
	 * To Randomize Fire Direction:
	 * multiply the player offset by the random number
	 */ 
	public static TurretTierTwo instance;

	public GameObject diggerMine;

	public bool activateDiggerMine;
	public bool blowUp;

	public float tierTwoAlertRange = 25.0f;
	public float fireDiggerMineRange = 20.0f;

	public float targetDuration = 3.0f;
	public float timedExplosion = 0.5f;

	public float playerOffset = 5.0f;
	public float diggerMineSpeed = 10.0f;
	public float launchUpForce = 20.0f;

	private float curTargetDur;
	private float curTimedExplosion;

	public int rndShotDir = 0;
	private int chooseRnd = 0;

	private Rigidbody diggerMineRB;

//	private GameObject player;
//	private Rigidbody rb;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
//		player = GameObject.FindWithTag("Player");
//		rb = GetComponent<Rigidbody>();

		Initialize();
		curTargetDur = targetDuration;
		curTimedExplosion = timedExplosion;

		alertZone = tierTwoAlertRange;

		diggerMineRB = diggerMine.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		DetectPlayer();
		ShootDiggerMine();
		GetMotion();
	}

	void DetectPlayer()
	{
		//if in alert range
		if (canSee)
		{
			//if in digger mine firing range but not in rocket firing range
			//activate digger mine
			if (dist < fireDiggerMineRange && dist > shootRange && !blowUp)
			{
				activateDiggerMine = true;
			}
		}
	}

	void ShootDiggerMine()
	{
		if (activateDiggerMine)
		{
			if (rndShotDir == 0)
			{
				chooseRnd = Random.Range(0, 5);

				if (chooseRnd < 3)
				{
					rndShotDir = -1;
				}
				else
				{
					rndShotDir = 1;
				}
			}
			//fire digger mine and follow player underground
			if (curTargetDur > 0)
			{
				Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y - playerOffset * rndShotDir, 0);
				diggerMine.transform.position = Vector3.Lerp(diggerMine.transform.position, targetPos, diggerMineSpeed * Time.deltaTime);

				curTargetDur -= Time.deltaTime;
			}
			//after a time prepare to launch out of ground
			else
			{
//				rb.AddForce(Vector3.up * launchUpForce, ForceMode.Impulse);
				diggerMine.transform.LookAt(player.transform);
				activateDiggerMine = false;
				blowUp = true;
			}
		}
		if (blowUp)
		{
			//launch out of ground
			diggerMineRB.velocity = diggerMine.transform.forward * launchUpForce;

			if (curTimedExplosion > 0)
			{
				curTimedExplosion -= Time.deltaTime;
			}
			else
			{
				//reset to object pool
//				Destroy(diggerMine);
			}
		}
	}
}
