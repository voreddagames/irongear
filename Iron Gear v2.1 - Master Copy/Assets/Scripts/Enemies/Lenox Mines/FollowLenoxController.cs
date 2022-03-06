using UnityEngine;
using System.Collections;

public class FollowLenoxController : MonoBehaviour
{
	public float spinSpeed = 10.0f;

	public Transform controller;

	private LenoxMines lenoxMines;

	void Awake()
	{
		lenoxMines = controller.GetComponent<LenoxMines>();
	}

	// Update is called once per frame
	void Update ()
	{
		Follow();
	}

	void Follow()
	{
		//while the mine controller exists we will perform functions
		if (controller)
		{
			//make sure the model is following the mine controller
			transform.position = controller.position;
//			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.right * lenoxMines.curMoveDir), lenoxMines.turnSpeed);

			//if the mine is not attacking the player we will spin while patrolling
			if (!lenoxMines.isAttacking)
			{
				transform.Rotate(Vector3.up * spinSpeed);
			}
			//when player is detected we will look at the player
			else
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, controller.rotation, lenoxMines.turnSpeed);
			}
		}
		//when the mine controller no longer exists destroy the model
		//###WILL CHANGE FOR RESPAWNING PURPOSES###
		else
		{
			Destroy (gameObject);
		}
	}
}
