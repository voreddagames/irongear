using UnityEngine;
using System.Collections;

public class DestroyRocketRain : MonoBehaviour
{
	public GameObject explosion;

	public float rocketSpeed = 20.0f;
	public float maxRandRot = 10.0f;
	public float minRandRot = -10.0f;

	private float randRocketRot;

	private bool canRandomizeRot = true;

	void Update()
	{
		MoveRocket();
	}

	void MoveRocket()
	{
		if (canRandomizeRot)
		{
			randRocketRot = Random.Range(minRandRot, maxRandRot);
			canRandomizeRot = false;
		}
		else
		{
			//rockets are falling from above so rotate them downwards
			transform.rotation = Quaternion.AngleAxis(randRocketRot, Vector3.forward);
			transform.Translate(Vector3.down * rocketSpeed * Time.deltaTime);
		}
	}

	void OnTriggerEnter()
	{
		//explode on impact

		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}
