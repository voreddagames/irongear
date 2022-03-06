using UnityEngine;
using System.Collections;

public class FireM3IRVRockets : MonoBehaviour
{
	public static FireM3IRVRockets instance;

	public float fireRate = 0.1f;

	public GameObject rocket;

	public float timeStamp = 0.0f;

	void Start()
	{
		instance = this;

		timeStamp = fireRate;
	}

	public void FireRockets()
	{
		if (timeStamp > 0)
		{
			timeStamp -= Time.deltaTime;
		}
		if (timeStamp <= 0)
		{
			SpawnRockets();
			timeStamp = fireRate;
		}
	}

	void SpawnRockets()
	{
		Instantiate(rocket, transform.position, transform.rotation);

	}
}
