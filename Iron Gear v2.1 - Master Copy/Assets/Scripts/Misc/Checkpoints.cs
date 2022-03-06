using UnityEngine;
using System.Collections;

public class Checkpoints : MonoBehaviour
{
	public static Checkpoints instance;
	public static bool isRespawning;
	public bool isResettingLevel;

	public float respawnTimer;

	public float curRespawnTime;

	private Vector3 origPos;

    private Transform currentCheckpoint;

	private GameObject master;

	private GameMaster gameMaster;
	private BoilingPointsSystem boilingPointsSystem;
	private PlayerHealth playerHealth;
	private PlayerMotor playerMotor;
	private CamPositioning camPositioning;

	private Rigidbody rb;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
    {
		origPos = transform.position;

		curRespawnTime = respawnTimer;

		master = GameObject.FindWithTag("GameController");
		gameMaster = master.GetComponent<GameMaster>();

		rb = GetComponent<Rigidbody>();

		boilingPointsSystem = GetComponent<BoilingPointsSystem>();
		playerHealth = GetComponent<PlayerHealth>();
		playerMotor = GetComponent<PlayerMotor>();
		camPositioning = GetComponent<CamPositioning>();
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
//        if (PlayerPhysics.instance.IsDead)
//        {
//            PlayerPhysics.instance.IsDead = false;
//        }
            //if (currentCheckpoint)
            //{
            //    Player.instance.colorMeter = Player.instance.maxPowerMeter;
            //    Player.instance.currentColor = 0;

            //    transform.position = currentCheckpoint.position;
            //}
            //Player.isDead = false;
		Respawn();
	}

	void Respawn()
	{
		//after death respawn the player at the most recent checkpoint
		if (PlayerPhysics.instance.IsDead && Input.GetKeyDown(KeyCode.Space))
		{
			isResettingLevel = true;
			isRespawning = true;

			if (currentCheckpoint)
			{
				transform.position = currentCheckpoint.position;
			}
			else
			{
				transform.position = origPos;
			}
		}

		if (isRespawning)
		{
			ResetStats();
		}
		else
		{
			curRespawnTime = respawnTimer;
		}
	}

	void ResetStats()
	{
		//reset player's stats
		playerHealth.health = playerHealth.tier1Health;
		boilingPointsSystem.boilTemp = boilingPointsSystem.startBoilTemperature;

		rb.constraints &= ~RigidbodyConstraints.FreezePositionX | ~RigidbodyConstraints.FreezePositionY;
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

		//camPositioning.enemies.Clear();

		PlayerPhysics.instance.IsDead = false;

		//a cool down time before allowing movement
		if (curRespawnTime > 0)
		{
			curRespawnTime -= Time.deltaTime;
		}
		else
		{

			playerMotor.GiveControl();
			isRespawning = false;
		}
	}

    void OnTriggerEnter(Collider col)
    {
    	//set the currect checkpoint to be the most recent one the player moved through.
        if (col.tag == "Checkpoint")
        {
            currentCheckpoint = col.transform;
//			SetRespawnPoint(col.transform.position);
//			gameMaster.startPos = col.transform.position;
        }
    }
}
