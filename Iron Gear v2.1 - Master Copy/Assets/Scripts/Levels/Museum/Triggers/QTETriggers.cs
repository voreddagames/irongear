using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTETriggers : MonoBehaviour
{
	private GameObject player;
	private PlayerMotor playerMotor;

	void Start()
	{
		player = GameObject.FindWithTag("Player");
		playerMotor = player.GetComponent<PlayerMotor>();
	}

	void OnTriggerEnter(Collider col)
	{
		//trigger QTE
		if (col.tag == "Player")
		{
			//activate the bool in m3irv's script
			M3IRVLevelOne.instance.isQTEActive = true;
			//destroy trigger to prevent reactivating QTE
			Destroy(gameObject);

			playerMotor.RemoveControl();
		}
	}
}
