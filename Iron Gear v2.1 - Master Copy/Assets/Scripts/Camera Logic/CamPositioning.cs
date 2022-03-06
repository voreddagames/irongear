using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamPositioning : MonoBehaviour
{
	public bool triggerCog = false;

	public float tiltAngle = -40.0f;
	public float camEncounterOffsetZ = 10.0f;
	public float maxEncounterOffsetX = 30.0f;
	public float camOffsetZ = 10.0f;
	public float camOffsetY = 5.0f;
	public float camOffsetX = 10.0f;

	public float camOffsetXSpeed = 10.0f;
	public float camOffsetZSpeedIn = 10.0f;
	public float camOffsetZSpeedOut = 10.0f;
	public float offsetEncounterSpeedOut = 5.0f;

	public bool isPlayerJumping;
	public bool hasEncounteredEnemy;

	public GameObject furthestEnemy;
	public GameObject curDetectedEnemy;
	public GameObject camPanTarget;

	public float curPosZ;
	public float curPosX;
	public float origPosX;
	public float origPosZ;

	public float delta;

	private float curTime;
	public float furthestEnemyDist;

	public int camPanDir;

	public List<GameObject> enemies;

	private bool canActivateEncounter;
	private bool isRotationReset;

	private GameObject playerTarget;

	private PlayerMotor playerMotor;
	private FindFurthestEnemy findFurthestEnemy;
	private PlayerPhysics playerPhysics;

	private Quaternion startRot;

    void Awake()
    {
        playerTarget = GameObject.FindWithTag("Player");
		playerMotor = playerTarget.GetComponent<PlayerMotor>();
		playerPhysics = playerTarget.GetComponent<PlayerPhysics>();
		findFurthestEnemy = playerTarget.GetComponent<FindFurthestEnemy>();
		transform.position = new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y, transform.position.z);
    }

	// Use this for initialization
	void Start ()
    {
		startRot = transform.rotation;
		origPosZ = transform.position.z;
		origPosX = transform.position.x;
		curPosX = origPosX;
		curPosZ = origPosZ;
	}

	void Update ()
    {
		ModDistance();
	}

	void ModDistance()
	{
		transform.position = new Vector3(Mathf.Lerp(transform.position.x, playerTarget.transform.position.x, camOffsetXSpeed * Time.deltaTime),
		                                 Mathf.Lerp (transform.position.y, playerTarget.transform.position.y + camOffsetY, camOffsetXSpeed * Time.deltaTime),
		                                 curPosZ);

		//if player is currently in battle pull camera back to give a wider angle of view
		if (enemies.Count > 0)
		{
			if (!M3IRVLevelOne.instance.isQTEActive)
			{
				curPosZ = Mathf.MoveTowards(curPosZ, origPosZ - camEncounterOffsetZ, offsetEncounterSpeedOut * Time.deltaTime);
			}
		}

		//When player is no longer in battle resume normal camera view angles
		else
		{
			furthestEnemy = null;
			//create camera dynamic when player is jumping
			if (isPlayerJumping)
			{
				curPosZ = Mathf.MoveTowards(curPosZ, origPosZ - camOffsetZ, camOffsetZSpeedOut * Time.deltaTime);
			}
			//move camera back to original position
			else
			{
				curPosZ = Mathf.MoveTowards(curPosZ, origPosZ, camOffsetZSpeedIn * Time.deltaTime);
			}
		}
	}

	#region Trash Code
	//		detectEnemyEncounter = playerTarget.GetComponent<DetectEnemyEncounter>();

	/*
	 * 	private DetectEnemyEncounter detectEnemyEncounter;

		Vector3 pos = transform.position;

//		transform.position = new Vector3(curPosX, Mathf.Lerp (transform.position.y, playerTarget.transform.position.y + camOffsetY, camOffsetXSpeed * Time.deltaTime),
//		                                 curPosZ);
//		if (playerPhysics.moveSpeed == (playerPhysics.forwardSpeed * playerPhysics.faceDir))
		if (playerPhysics.moveSpeed > (playerPhysics.forwardSpeed * 0.5f) ||
		    playerPhysics.moveSpeed < (-playerPhysics.forwardSpeed * 0.5f))
//		if (playerPhysics.moveSpeed != 0)
		{
			int offsetDir = (int)playerPhysics.moveSpeed;
			offsetDir = Mathf.Clamp(offsetDir, -1, 1);

			curPosX = Mathf.SmoothStep(transform.position.x, playerTarget.transform.position.x + (camOffsetX * offsetDir), camOffsetXSpeed * Time.deltaTime);
//			pos = new Vector3(Mathf.SmoothStep(transform.position.x, playerTarget.transform.position.x + (camOffsetX * offsetDir), camOffsetXSpeed * Time.deltaTime),
//			                  Mathf.SmoothStep (transform.position.y, playerTarget.transform.position.y + camOffsetY, camOffsetXSpeed * Time.deltaTime),
//			                  curPosZ);
		}

		else
//		if (playerPhysics.moveSpeed < (playerPhysics.forwardSpeed * 0.5f) ||
//		    playerPhysics.moveSpeed > (-playerPhysics.forwardSpeed * 0.5f))
		{
			curPosX = Mathf.SmoothStep(transform.position.x, playerTarget.transform.position.x, camOffsetXSpeed * Time.deltaTime);
		}
		transform.position = new Vector3(curPosX, Mathf.SmoothStep (transform.position.y, playerTarget.transform.position.y + camOffsetY, camOffsetXSpeed * Time.deltaTime),
		                                 curPosZ);
		                                 */
	/*		if (curDetectedEnemy)
//		{
//			if (curDetectedEnemy.GetComponent<DetectEnemyEncounter>().canActivateEncounter)
//			{*/
	/*					for (int i = 0; i < enemies.Count; i++)
//					{
//						float dist = (playerTarget.transform.position - enemies[i].transform.position).magnitude;
//
//						if (dist > furthestEnemyDist)
//						{
//							furthestEnemy = enemies[i].transform.gameObject;
//						}
//						furthestEnemyDist = dist;
//					}
				
//			foreach(GameObject enemy in enemies)
//			{
//				float dist = (playerTarget.transform.position - enemy.transform.position).magnitude;
//
//				if (dist > furthestEnemyDist)
//				{
//					furthestEnemy = enemy;
//				}
//				furthestEnemyDist = dist;
//			}
//			if (furthestEnemy)
//			{
//				Vector3 curEnemyLoc = playerTarget.transform.InverseTransformPoint(furthestEnemy.transform.position);
//
//				//on left side
//				if (curEnemyLoc.x < 0)
//				{
//					camPanDir = 1;
//				}
//				//on right side
//				else
//				{
//					camPanDir = -1;
//				}
//				curPosX = Mathf.MoveTowards(curPosX, furthestEnemy.transform.position.x - camOffsetX * camPanDir, camOffsetXSpeed * Time.deltaTime);
//			}
//			else
//			{
//				curPosX = Mathf.MoveTowards(transform.position.x, playerTarget.transform.position.x, camOffsetXSpeed * Time.deltaTime);
//			}*/
	//			curPosX = Mathf.MoveTowards(transform.position.x, playerTarget.transform.position.x, camOffsetXSpeed * Time.deltaTime);

	//		if (!triggerCog)
	//		{
	//			ModDistance();
	//
	//			if (!isRotationReset)
	//			{
	//				if (transform.rotation.x != 0)
	//				{
	//					transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), 20 * Time.deltaTime);
	//				}
	//				else
	//				{
	//					isRotationReset = true;
	//				}
	//			}
	//		}
	//		else
	//		{
	//			if (playerTarget)
	//			{
	//				QTECogZoom();
	//				isRotationReset = false;
	//			}
	//		}

	//	void QTECogZoom()
	//	{
	////		curTime = Mathf.MoveTowards(curTime, 0.5f, 2 * Time.deltaTime);
	////		Time.timeScale = curTime;
	//
	//		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(tiltAngle, 0, 0)), 20 * Time.deltaTime);
	//		curPosZ = Mathf.MoveTowards(curPosZ, origPosZ - camEncounterOffsetZ, offsetEncounterSpeedOut * Time.deltaTime);
	//
	//		transform.position = new Vector3(Mathf.Lerp(transform.position.x, playerTarget.transform.position.x, camOffsetXSpeed * Time.deltaTime),
	//		                                 Mathf.Lerp (transform.position.y, playerTarget.transform.position.y + camOffsetY, camOffsetXSpeed * Time.deltaTime),
	//		                                 curPosZ);
	//	}
	/*//			if (findFurthestEnemy.canPanCam)
//			{
//				hasEncounteredEnemy = true;
//
//				delta = 1 - Mathf.Pow(Vector3.Distance(playerTarget.transform.position, furthestEnemy.transform.position), 5.0f / 9.0f);
//
////				if (!Mathf.Approximately(delta, 1))
////				{
//				curPosX = Mathf.MoveTowards (curPosX, playerTarget.transform.position.x, delta);
//				}
//			}
//			else
//			{
//				hasEncounteredEnemy = false;
//			}
		
//		if (!findFurthestEnemy.canActivateEncounter)
//			if (!furthestEnemy.GetComponent<DetectEnemyEncounter>().canActivateEncounter)*/
	//		}
	//		else
	//		{
	//			if (isPlayerJumping)
	//			{
	//				curPosZ = Mathf.MoveTowards(curPosZ, origPosZ - camOffsetZ, camOffsetZSpeedOut * Time.deltaTime);
	//			}
	//			else
	//			{
	//				curPosZ = Mathf.MoveTowards(curPosZ, origPosZ, camOffsetZSpeedIn * Time.deltaTime);
	//			}
	//			curPosX = Mathf.MoveTowards(transform.position.x, playerTarget.transform.position.x, camOffsetXSpeed * Time.deltaTime);
	//		}
	//		if (!hasEncounteredEnemy)
	//		{
	//			curPosX = Mathf.MoveTowards(curPosX, playerTarget.transform.position.x, camOffsetXSpeed * Time.deltaTime);
	//			origPosX = transform.position.x;
	//		}
	#endregion
}
