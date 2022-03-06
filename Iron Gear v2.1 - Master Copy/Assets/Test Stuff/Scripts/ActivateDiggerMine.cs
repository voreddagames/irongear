using UnityEngine;
using System.Collections;

public class ActivateDiggerMine : MonoBehaviour
{
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			TurretTierTwo.instance.activateDiggerMine = true;
			Destroy(gameObject);
		}
	}
}
