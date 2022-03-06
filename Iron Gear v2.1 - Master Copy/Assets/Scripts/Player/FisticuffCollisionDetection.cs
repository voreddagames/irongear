using UnityEngine;
using System.Collections;

public class FisticuffCollisionDetection : MonoBehaviour
{
	public static FisticuffCollisionDetection instance;

	public static bool DidHit = false;

	private GameObject player;
	private MeleeSystem meleeSystem;
	private PlayerPhysics playerPhysics;

	void Awake()
	{
		instance = this;

		player = GameObject.FindWithTag("Player");
		meleeSystem = player.GetComponent<MeleeSystem>();
		playerPhysics = player.GetComponent<PlayerPhysics>();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Enemy" && meleeSystem.isDamagingEnemy)
		{
			//set the current enemy we are attacking to be the enemy the player's feet and hands collided with
			meleeSystem.curEnemy = col.gameObject;

//			meleeSystem.curAdvanceAttackDist = 0;
			meleeSystem.curAdvanceAttackSpeed = 0;

			//stop moving while attacking
			playerPhysics.moveSpeed = 0;

			meleeSystem.DamageEnemy();

			//shake camera on last attack in combo
			if (meleeSystem.curAttackLimit == meleeSystem.attackLimit)
			{
				Camera.main.GetComponent<PerlinShake>().PlayShake();
//				Camera.main.GetComponent<RandomShake>().PlayShake();
				//Camera.main.GetComponent<PeriodicShake>().PlayShake();
			}
			DidHit = true;
		}
	}
}
