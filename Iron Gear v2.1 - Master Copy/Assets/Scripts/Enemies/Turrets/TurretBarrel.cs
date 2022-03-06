using UnityEngine;
using System.Collections;

public class TurretBarrel : MonoBehaviour
{
    public enum ColorType
	{
		Red,
		Blue,
		Yellow
	}

    public Transform curTurret;
	public Transform rocketSpawnPoint;

	private float bulletSpeed = 30.0f;
	public float searchRate = 2.0f;

	public float curSearchRate = 0.0f;
	public float rndAngle = 0.0f;

	public float curBulletSpeed = 0.0f;

	public Rigidbody bullet;
	
	public float distance = 10.0f,
	rotationSpeed = 10.0f,
	shootRate = 1.0f;
	
	public GameObject player;
	
	public TurretPhysics turretPhysics;
	
	public SteamChangeAbility steamChangeAbility;
	
	public float shootCoolDown = 0.25f;

    void Awake()
    {
        SetUp();

        turretPhysics = curTurret.GetComponent<TurretPhysics>();
    }

	void SetUp()
	{
		player = GameObject.FindWithTag("Player");
		
		steamChangeAbility = player.GetComponent<SteamChangeAbility>();
	}

	// Update is called once per frame
	void Update ()
    {
        Aim();
	}
	
    void Aim()
    {
		Vector3 playerPos = (player.transform.position - transform.position);

		//shoot if turret sees player
        if (TurretPhysics.currentTurretShooting && turretPhysics.canShoot)
        {
//			Quaternion rotAngle = Quaternion.LookRotation(playerPos, Vector3.forward);
//			rotAngle.x = 0;
//			rotAngle.y = 0;
			float angle = (Mathf.Atan2(playerPos.y + 3.0f, playerPos.x) * Mathf.Rad2Deg);
			Quaternion rotAngle = Quaternion.AngleAxis(angle, Vector3.forward);

			//aim towards player
			transform.rotation = Quaternion.Slerp(transform.rotation, rotAngle, rotationSpeed * Time.deltaTime);
//			transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, 0, 0);

			//fire rate calculations
            if (shootCoolDown > 0)
            {
				shootCoolDown -= Time.deltaTime;
            }
			if (shootCoolDown <= 0)
            {
				shootCoolDown = 0;
            }
			curBulletSpeed = Mathf.Abs(turretPhysics.CurMoveSpeed()) + bulletSpeed;

			if (shootCoolDown == 0)
            {
				//fire rocket
                Rigidbody clone = Instantiate(bullet, rocketSpawnPoint.position, rocketSpawnPoint.rotation) as Rigidbody;
				clone.GetComponent<Bullet>().bulletSpeed = curBulletSpeed;
//				clone.AddForce(clone.transform.right * (curBulletSpeed), ForceMode.VelocityChange);
				Physics.IgnoreCollision(clone.transform.GetComponent<Collider>(), curTurret.transform.GetComponent<Collider>());

				shootCoolDown = shootRate;
            }
        }

        //Turret no longer detects the player so start to search
		if (!turretPhysics.CanSee() && !turretPhysics.canShoot)
		{
			shootCoolDown = 0.25f;
			Search();
		}
    }

	void Search()
	{
		//randomly move the turret to different angles when searching for player
		if (curSearchRate > 0)
		{
			curSearchRate -= Time.deltaTime;
		}
		else
		{
			rndAngle = Random.Range(30, 145);
			curSearchRate = searchRate;
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(rndAngle, Vector3.forward), 2 * Time.deltaTime);
	}
}
