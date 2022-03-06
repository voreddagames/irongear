using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//placed on all enemies in order to determine whether or not player is close enough to activate battle.
//######this may need to be placed into a single script.  should experiment#########
public class DetectEnemyEncounter : MonoBehaviour
{
	public static DetectEnemyEncounter instance;

	public bool canActivateEncounter;
	public bool canPanCam;

	public float distForEncounter = 30.0f;
	private float distForCamPan = 25.0f;

	private GameObject player;

	private List<GameObject> enemies;

	public float dist;

	public bool canAddEnemy = true;

	private CamPositioning camPositioning;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		player = GameObject.FindWithTag("Player");
//		enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
		camPositioning = Camera.main.GetComponent<CamPositioning>();
	}

	// Update is called once per frame
	void Update ()
	{
		EngageEncounter();
	}

	void EngageEncounter()
	{
		//find the distance between the player and this object
		dist = Vector3.Distance(transform.position, player.transform.position);

//		foreach (GameObject enemy in enemies)
//		{
//			dist = Vector3.Distance(transform.position, enemy.transform.position);
//		}

//		if (dist <= distForEncounter)
//		{
//			camPositioning.curDetectedEnemy = transform.gameObject;
//			canActivateEncounter = true;
//		}
//		else
//		{
//			canActivateEncounter = false;
//		}

		//if we are within distance of the player we add an enemy to the list of enemies currently within range.
		if (dist <= distForEncounter && canAddEnemy)
		{
			camPositioning.enemies.Add(gameObject);
			canAddEnemy = false;
//			camPositioning.furthestEnemy = transform.gameObject;
		}
		//if we are no longer within range of the player we will remove this enemy from that list
		if (dist > distForEncounter)
		{
			camPositioning.enemies.Remove(gameObject);
			canAddEnemy = true;
		}
//		else if (camPositioning.furthestEnemy == transform.gameObject)
//		{
//			camPositioning.furthestEnemy = null;
//		}
//		if (dist <= distForCamPan)
//		{
//			canPanCam = true;
//
//			if (!didAdd)
//			{
//				findFurthestEnemy.enemies.Add(transform.gameObject);
//				didAdd = true;
//			}
//			print ("what the double fuck?");
//		}
//		else
//		{
//			if (findFurthestEnemy.enemies.Count > 0)
//			{
//				print ("wtf");
//				findFurthestEnemy.enemies.Where(go => go != transform.gameObject).ToArray();
//				didAdd = false;
//			}
//		}
	}
}
