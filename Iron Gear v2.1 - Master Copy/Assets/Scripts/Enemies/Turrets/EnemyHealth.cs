using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
	public static EnemyHealth instance;

	public int health = 10;
	public int energyCost = 1;
	public int energyReward = 20;
	public int tierLevel = 1;

	public float toTarget;

	public bool isHit = false;

	public Transform currentEnemy;

	private GameObject playerTarget;

	private CamPositioning camPositioning;
	private BoilingPointsSystem boilingPointsSystem;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		playerTarget = GameObject.FindWithTag("Player");
		boilingPointsSystem = playerTarget.GetComponent<BoilingPointsSystem>();
		camPositioning = Camera.main.GetComponent<CamPositioning>();
	}

	public void AdjustHealth(int dmg)
	{
		isHit = true;

		//damage enemy
		health += dmg;

		AddEnergyToPlayer();
	}

	void AddEnergyToPlayer()
	{
		//add energy to player
		if (health <= 0)
		{
			//enemy has died so remove from the list of enemies that are within range of player
			camPositioning.enemies.Remove(gameObject);
			health = 0;
			
			boilingPointsSystem.AdjustBoilTemp(energyReward);
		}
	}
}
