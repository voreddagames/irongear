using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FindFurthestEnemy : MonoBehaviour
{
	public List<GameObject> enemies;
	public List<GameObject> engagedEnemies;

	public bool canActivateEncounter;
	public bool canPanCam;
	
	public float distForEncounter = 30.0f;
	public float distForCamPan = 28.0f;

	public float dist;

	public bool didAdd = false;

	private CamPositioning camPositioning;

	// Use this for initialization
	void Start ()
	{
		camPositioning = Camera.main.GetComponent<CamPositioning>();
		//create a list of all enemies in the level
		enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

//		enemies.Sort(delegate(GameObject a1, GameObject a2){
//			return Vector3.Distance(transform.position, a1.transform.position).CompareTo((Vector3.Distance(transform.position, a2.transform.position))); });
//		
//		camPositioning.furthestEnemy = enemies[0].transform.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		CalculateFurthestEnemy();
//		EngageEncounter();
	}

	void CalculateFurthestEnemy()
	{
		//log the distance of the closest enemy to the player
		foreach (GameObject enemy in enemies)
		{
			dist = Vector3.Distance(transform.position, enemy.transform.position);
		}
		//when the enemy is within a specific distance, activate the battle
		if (dist <= distForEncounter)
		{
			canActivateEncounter = true;
		}
		//when the enemy is out of range, deactivate the battle.
		else
		{
			canActivateEncounter = false;
		}
//		if (dist <= distForCamPan)
//		{
//			canPanCam = true;
//			
//			if (!didAdd)
//			{
//				print ("got one");
//				engagedEnemies.Add(camPositioning.furthestEnemy.transform.gameObject);
//				didAdd = true;
//			}
//		}
//		else
//		{
//			canPanCam = false;
//
//			if (camPositioning.furthestEnemy)
//			{
//				print ("lost one");
//				engagedEnemies.Where(go => go != camPositioning.furthestEnemy.transform.gameObject).ToArray();
//			}
//			didAdd = false;
//		}
//
//		if (canPanCam)
//		{
//			engagedEnemies.Sort(delegate(GameObject a1, GameObject a2){
//				return Vector3.Distance(transform.position, a2.transform.position).CompareTo((Vector3.Distance(transform.position, a1.transform.position))); });
//
//			camPositioning.furthestEnemy = engagedEnemies[0].transform.gameObject;
//		}
	}
	#region Trash code
	/*void EngageEncounter()
	{
		float dist = Vector3.Distance(transform.position, camPositioning.furthestEnemy.transform.position);
		
		if (dist <= distForEncounter)
		{
			canActivateEncounter = true;
		}
		else
		{
			canActivateEncounter = false;
		}
		if (dist <= distForCamPan)
		{
			canPanCam = true;
			
			if (!didAdd)
			{
				enemies.Add(transform.gameObject);
				didAdd = true;
			}
			print ("what the double fuck?");
		}
		else
		{
			if (enemies.Count > 0)
			{
				print ("wtf");
				enemies.Where(go => go != camPositioning.furthestEnemy.transform.gameObject).ToArray();
				didAdd = false;
			}
		}
	}*/
	#endregion
}
