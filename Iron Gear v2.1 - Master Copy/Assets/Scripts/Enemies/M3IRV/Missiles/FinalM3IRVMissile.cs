using UnityEngine;
using System.Collections;

public class FinalM3IRVMissile : MonoBehaviour
{
	public Transform target;
	public Transform explosion;

	public float speed = 10.0f;
	
	// Update is called once per frame
	void Update ()
	{
		MissilePhysics();
	}

	void MissilePhysics()
	{
		//trigger missile to fire down at scaffolding
		if (FinalMissileTrigger.instance.hasTriggeredFinalMissile)
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
			transform.LookAt(target.position);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		//trigger explosion and break scaffolding
		if (col == target.GetComponent<Collider>())
		{
			BreakScafoldingChains.instance.isBroken = true;
			Instantiate(explosion, target.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
