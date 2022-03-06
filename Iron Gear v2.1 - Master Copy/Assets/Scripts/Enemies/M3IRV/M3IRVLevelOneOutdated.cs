using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class M3IRVLevelOneOutdated : MonoBehaviour
{
	public static M3IRVLevelOneOutdated instance;

	public float distFromPlayer = 50.0f;

	public List<Transform> QTEPos;

	public int QTEIndex = 0;

	public bool isQTEComplete;
	public bool hasTriggeredCog;
	public bool hasTriggeredFurnace;
	public bool hasTriggeredMissileBarage;
	public bool hasTriggeredDodgeMissiles;

	private ThrowableObject throwableObject;

	void Start()
	{
		instance = this;
	}

	public void UpdateM3IRVLevelOne ()
	{
//		M3IRVPhysics.instance.CalculateMovement();

		TriggerThrowObject();
//		TriggerRocketDodge();
//		TriggerRocketBarage();
		Flee();
	}

	void Flee()
	{
		Vector3 dist = transform.position - M3IRVPhysics.instance.player.transform.position;

		//out of player's view
		if (dist.x > distFromPlayer)
		{
			//move to the waypoint and wait for QTE to trigger
			M3IRVPhysics.instance.moveSpeed = 0;
			if (QTEIndex < QTEPos.Count)
			{
				transform.position = QTEPos[QTEIndex].position;
			}
		}
//		else
//		{

			//after QTE is done flee player
			if (isQTEComplete)
			{
				QTEIndex++;
			M3IRVPhysics.instance.moveSpeed = M3IRVPhysics.instance.flightSpeed;

				isQTEComplete = false;
			}
//		}
	}

	void TriggerThrowObject()
	{
		//set the animation state to throw the object
		if (hasTriggeredCog || hasTriggeredFurnace)
		{
			M3IRVAnimation.instance.state = M3IRVAnimation.animCharacterStates.pickupThrowObjectHoverCenter;
		}

		//get the script on the object M3IRV is holding
		if (M3IRVAnimation.instance.currentObjectHolding)
		{
			throwableObject = M3IRVAnimation.instance.currentObjectHolding.GetComponent<ThrowableObject>();

			//after the object is thrown reset the trigger booleans
			if (throwableObject.hasThrownObject)
			{
				hasTriggeredCog = false;
				hasTriggeredFurnace = false;
			}
		}
	}

	void TriggerRocketDodge()
	{
		//set the animation state to fire the rocket when the rocket has respawned at M3IRV's hand
		if (hasTriggeredDodgeMissiles && M3IRVDodgeMissile.instance.hasRespawned)
		{
			M3IRVAnimation.instance.state = M3IRVAnimation.animCharacterStates.leftArmFiring;
		}
		//after playing the animation reset the respawn boolean
		if (M3IRVAnimation.instance.state != M3IRVAnimation.animCharacterStates.leftArmFiring)
		{
			M3IRVDodgeMissile.instance.hasRespawned = false;
		}
	}

	void TriggerRocketBarage()
	{
		//set animation state to fire with both hands
		if (hasTriggeredMissileBarage)
		{
			M3IRVAnimation.instance.state = M3IRVAnimation.animCharacterStates.fireBothHands;
		}
	}
}
